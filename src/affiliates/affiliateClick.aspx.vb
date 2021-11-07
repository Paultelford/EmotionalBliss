Imports System.Data
Imports System.data.SqlClient

Partial Class affiliates_affiliateClick
    Inherits BasePage
    Private Const _formatDP As Integer = 2

    Protected Sub gvAffs_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'User has selected an affilaite for the 1st time, show the product panels.
        panBProducts.Visible = True
        panComponents.Visible = True
        'Reset the linkbuttons/labels visibility
        Dim pan As Panel = panComponents
        restoreVisibility(pan, "Component")
        pan = panBProducts
        restoreVisibility(pan, "Product")
    End Sub
    Protected Sub restoreVisibility(ByRef p As Panel, ByVal type As String)
        Dim lnk As LinkButton = p.FindControl("lnkSave" & type & "List")
        Dim lbl As Label = p.FindControl("lblSaveComplete" & type)
        Dim lblError As Label = p.FindControl("lblError" & type)
        lnk.Visible = True
        lbl.Visible = False
        lblError.Text = ""
    End Sub
    Protected Sub lnkSaveComponentList_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("comp")
        If Page.IsValid Then
            Dim txt As TextBox
            For Each row As GridViewRow In gvComponent.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    txt = row.FindControl("txtPercentage")
                    saveItem("Component", CInt(gvComponent.DataKeys(row.RowIndex).Value), CDec(txt.Text))
                End If
            Next
        End If
    End Sub
    Protected Sub lnkSaveProductList_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("prod")
        If Page.IsValid Then
            Dim txt As TextBox
            For Each row As GridViewRow In gvProduct.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    txt = row.FindControl("txtPercentage")
                    saveItem("Product", CInt(gvProduct.DataKeys(row.RowIndex).Value), CDec(txt.Text))
                End If
            Next
        End If
    End Sub
    Protected Sub saveItem(ByVal type As String, ByVal affProductBuyingID As Integer, ByVal percentage As Decimal)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateClickThroughPercentageInsertUpdate", oConn)
        Dim bError As Boolean = False
        Dim pan As Panel = Nothing
        Select Case LCase(type)
            Case "component"
                pan = panComponents
            Case "product"
                pan = panBProducts
        End Select
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@affProductBuyingID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@percentage", SqlDbType.Decimal))
            .Parameters("@affID").Value = gvAffs.DataKeys(gvAffs.SelectedIndex).Value
            .Parameters("@affProductBuyingID").Value = affProductBuyingID
            .Parameters("@percentage").Value = percentage
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            Dim lbl As Label = pan.FindControl("lblError" & type)
            lbl.Text = "<font color='red'>An error occured while saving<br></font>"
            Dim si As New siteInclude
            si.addError("affiliates/affiliateClick.aspx.vb", "saveItem(type=" & type & ",affProductBuyingID=" & affProductBuyingID & ",percentage=" & percentage & "); " & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            Dim lnk As LinkButton = pan.FindControl("lnkSave" & type & "List")
            Dim lbl As Label = pan.FindControl("lblSaveComplete" & type)
            Dim gv As GridView = pan.FindControl("gv" & type)
            lnk.Visible = False
            lbl.Visible = True
        End If
    End Sub
    Protected Function getAffFullName(ByVal fn As Object, ByVal sn As Object)
        Dim result As String = ""
        If Not IsDBNull(fn) Then result = result & fn.ToString & " "
        If Not IsDBNull(sn) Then result = result & sn.ToString
        Return result
    End Function
    Protected Function showPercentage(ByVal o As Object) As String
        Dim result As Decimal = 0
        If Not IsDBNull(o) Then result = CDec(o.ToString)
        Return CStr(FormatNumber(result, _formatDP))
    End Function
End Class
