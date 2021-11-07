Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_productLinks
    Inherits System.Web.UI.Page
    Private Const _gvProducts_namePos As Integer = 1
    Private Const _gvProducts_typePos As Integer = 3
    Private Const _gvProducts_countryPos As Integer = 5

    Protected Sub gvProducts_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblLinkID As Label
        For Each row As GridViewRow In gvProducts.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblLinkID = row.FindControl("lblLinkID")
                If lblLinkID.Text = "" Then
                    'No associations exist for this product, make the background gray
                    'row.BackColor = System.Drawing.ColorTranslator.FromHtml("#eeeeee")
                    row.ForeColor = Drawing.Color.Red
                End If
                'Show product type
                Select Case row.Cells(_gvProducts_typePos).Text
                    Case "product"
                        row.Cells(_gvProducts_typePos).Text = "Product"
                    Case "bproduct"
                        row.Cells(_gvProducts_typePos).Text = "Boxed Product"
                    Case "component"
                        row.Cells(_gvProducts_typePos).Text = "Component"
                End Select
            End If
        Next
    End Sub
    Protected Sub gvProducts_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        panLinkTypes.Visible = True
        'Clear up Link Types
        txtNewLinkType.Text = ""
        lblNewError.Text = ""
    End Sub
    Protected Sub btnAddLinkType_click(ByVal sender As Object, ByVal e As EventArgs)
        'Check that the new link type does not already exist
        Dim bMatchFound As Boolean = False
        Dim lnkTypeSelect As LinkButton
        For Each row As GridViewRow In gvLinkTypes.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lnkTypeSelect = row.FindControl("lnkTypeSelect")
                If LCase(txtNewLinkType.Text) = LCase(lnkTypeSelect.Text) Then bMatchFound = True
            End If
        Next
        If Not bMatchFound Then
            'Add new type
            addNewType(txtNewLinkType.Text)
            gvLinkTypes.DataBind() 'Refresh
            'Clear up Link Types
            txtNewLinkType.Text = ""
            lblNewError.Text = ""
        Else
            lblNewError.Text = "<font color='red'>Link Type already exists.</font>"
        End If
    End Sub
    Protected Sub addNewType(ByVal txt As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleLinkTypeInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@lnkName", SqlDbType.VarChar, 50))
            .Parameters("@lnkName").Value = txt
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub gvLinkTypes_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Must check that the selected link type is not already associated with a different product from the same country
        Dim lnk As LinkButton
        Dim lbl As Label
        lnk = gvLinkTypes.SelectedRow.FindControl("lnkTypeSelect")
        lbl = gvProducts.SelectedRow.FindControl("lblCountryCode")
        If Not linkAlreadyAssociatedWithCountry(lnk.Text, lbl.Text) Then
            'All ok
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductOnSaleLinkInsert", oConn)
            Dim bError As Boolean = False
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@posID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@lnkTypeID", SqlDbType.Int))
                .Parameters("@posID").Value = gvProducts.DataKeys(gvProducts.SelectedIndex).Value
                .Parameters("@lnkTypeID").Value = gvLinkTypes.DataKeys(gvLinkTypes.SelectedIndex).Value
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                bError = True
                lblError.Text = ex.ToString
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If Not bError Then
                'Clean up
                txtNewLinkType.Text = ""
                lblNewError.Text = ""
                panLinkTypes.Visible = False
                gvProducts.SelectedIndex = -1
                gvProducts.DataBind()
                lblError.Text = ""
            End If
        Else
            lblError.Text = lblError.Text & "The Link Type '" & lnk.Text & "' is already associated with a product from " & gvProducts.SelectedRow.Cells(_gvProducts_countryPos).Text
        End If
    End Sub
    Protected Sub btnTypeDelete_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleLinkByIDDelete", oConn)
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters("@id").Value = CInt(btn.CommandArgument)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblError.Text = ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then gvProducts.DataBind()
    End Sub
    Protected Function linkAlreadyAssociatedWithCountry(ByVal linkType As String, ByVal countryCode As String) As Boolean
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleLinkByTypeCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim bResult = False
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@linkType", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@linkType").Value = linkType
            .Parameters("@countryCode").Value = countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then bResult = True
        Catch ex As Exception
            bError = True
            lblError.Text = ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return bResult
    End Function
End Class
