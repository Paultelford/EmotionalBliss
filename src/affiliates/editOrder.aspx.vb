Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_editOrder
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub drpDepartment_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub gvPages_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If drpDepartment.SelectedIndex > 0 Then
            btnUpdate.Visible = True
        Else
            btnUpdate.Visible = False
        End If
    End Sub
    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByIDPriorityUpdate", oConn)
        Dim txtPriority As TextBox
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
        oCmd.Parameters.Add(New SqlParameter("@priority", SqlDbType.Int))
        For Each row As GridViewRow In gvPages.Rows
            If row.RowType = DataControlRowType.DataRow Then
                txtPriority = row.FindControl("txtPriority")
                oCmd.Parameters("@id").Value = gvPages.DataKeys(row.RowIndex).Value
                oCmd.Parameters("@priority").Value = txtPriority.Text
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    lblError.Text = lblError.Text & "An error occured. " & ex.ToString
                End Try
            End If
        Next
        gvPages.DataBind()
    End Sub
End Class
