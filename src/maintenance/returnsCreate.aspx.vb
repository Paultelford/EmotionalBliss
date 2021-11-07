Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_returnsCreate
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("login.aspx")
    End Sub

    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Create a new return
        If drpDistributors.SelectedValue <> 0 Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procReturnDistInsert", oConn)
            Dim iReturnsNo As Integer = 0
            panComplete.Visible = True
            panCreate.Visible = False
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@add4", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@postcode", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@phone", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@shop", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@distributorID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@returnsID", SqlDbType.Int))
                .Parameters("@name").Value = ""
                .Parameters("@add1").Value = ""
                .Parameters("@add2").Value = ""
                .Parameters("@add3").Value = ""
                .Parameters("@add4").Value = ""
                .Parameters("@postcode").Value = ""
                .Parameters("@countryCode").Value = ""
                .Parameters("@email").Value = ""
                .Parameters("@phone").Value = ""
                .Parameters("@orderID").Value = 0
                .Parameters("@affID").Value = 0
                .Parameters("@shop").Value = ""
                .Parameters("@distributorID").Value = drpDistributors.SelectedValue
                .Parameters("@returnsID").Direction = ParameterDirection.Output
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
                iReturnsNo = oCmd.Parameters("@returnsID").Value
                lblReturnsID.Text = iReturnsNo
            Catch ex As Exception
                For Each c As SqlParameter In oCmd.Parameters
                    'If Not IsDBNull(c.Value) Then Response.Write(c.XmlSchemaCollectionName & ": " & c.Value & c.Size & "-" & Len(CStr(c.Value)) & "<br>")
                Next
                Response.Write(ex)
                Response.End()
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
End Class
