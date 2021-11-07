Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_returnsPending
    Inherits System.Web.UI.Page
    Private Const _gvParts_nameIDPos As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate()
        If Page.IsValid Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procReturnsDistUpdate", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@returnsID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@distAction", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@comments", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 20))
                .Parameters("@returnsID").Value = Request.QueryString("id")
                .Parameters("@status").Value = "Complete"
                .Parameters("@distAction").Value = DBNull.Value
                .Parameters("@comments").Value = txtComments.Text
                .Parameters("@username").Value = txtUsername.Text
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Dim si As New siteInclude
                si.addError("maintenance/returnsPending.aspx.vb", ex.ToString)
                si = Nothing
                lblError.Text = "An error occured while processing the return.  Please contact tech support or try again later."
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            panDetails.Visible = False
            panComplete.Visible = True
        End If
        'Add items recieved to db
        Dim txtQty As TextBox
        Dim lblAffProductBuyingID As Label
        Dim lblType As Label
        For Each row As GridViewRow In gvParts.Rows
            If row.RowType = DataControlRowType.DataRow Then
                txtQty = row.FindControl("txtQty")
                If txtQty.Text <> "" Then
                    lblAffProductBuyingID = row.FindControl("lblAffProductBuyingID")
                    lblType = row.FindControl("lblType")
                    Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                    Dim oCmd As New SqlCommand("procReturnsItemInsert", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@returnsID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@affProductBuyingID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 10))
                        .Parameters("@returnsID").Value = Request.QueryString("id")
                        .Parameters("@name").Value = Server.HtmlDecode(row.Cells(_gvParts_nameIDPos).Text)
                        .Parameters("@affProductBuyingID").Value = lblAffProductBuyingID.Text
                        .Parameters("@qty").Value = txtQty.Text
                        .Parameters("@type").Value = lblType.Text
                    End With
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        Dim si As New siteInclude
                        si.addError("maintenance/returnsPending.aspx.vb", ex.ToString)
                        si = Nothing
                        lblError.Text = "An error occured while processing the returned items.  Please contact tech support or try again later."
                    Finally
                        oCmd.Dispose()
                        oConn.Dispose()
                    End Try
                End If
            End If
        Next

    End Sub
End Class
