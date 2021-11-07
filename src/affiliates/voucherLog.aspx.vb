Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_voucherLog
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
    End Sub
    Protected Function formatCapString(ByVal o As Object) As String
        Dim result As String = ""
        If Len(o.ToString) > 2 Then
            result = UCase(Left(o.ToString, 1)) & LCase(Right(o.ToString, Len(o.ToString) - 1))
        End If
        Return result
    End Function
    Protected Function isOrderUsed(ByVal o As Object, ByVal id As Object) As String
        Dim result As String = "Unused"
        If Not IsDBNull(o) Then
            result = "<a href='~/affiliates/orderView.aspx?id=" & id.ToString & "'>" & o.ToString & "</a>"
        End If
        Return result
    End Function
    Protected Sub gvLog_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim row As GridViewRow = e.Row
        If row.RowType = DataControlRowType.DataRow Then
            Dim lblProductOnSale As Label = row.FindControl("lblPS")
            If lblProductOnSale.Text = "0" Then
                'This means the voucher was created by a distributor, hilight the current row to show this
                row.BackColor = Drawing.ColorTranslator.FromHtml("#cccccc")
            End If
        End If
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpType.SelectedValue = "0" Then
            panMultiUse.Visible = False
        Else
            panMultiUse.Visible = True
        End If
        panSingleUse.Visible = Not panMultiUse.Visible
    End Sub
    Protected Function showActive(ByVal o As Object) As String
        Dim result As String = "No"
        If CType(o.ToString, Boolean) Then result = "Yes"
        Return result
    End Function
    Protected Sub btnActive_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procVoucherByIDActiveToggle", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@voucherID", SqlDbType.Int))
            .Parameters("@voucherID").Value = CInt(btn.CommandArgument)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "An error occured, " & ex.Message
            Dim si As New siteInclude
            si.addError("affiliates/voucherLog.aspx.vb", "btnActive_click(voucherID=" & btn.CommandArgument & "); " & ex.ToString)
            si = Nothing
        End Try
        gvMultiLog.DataBind()
    End Sub
End Class
