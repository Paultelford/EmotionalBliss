Imports System.Data
Imports System.Data.SqlClient

Partial Class MasterPageCallcentre
    Inherits System.Web.UI.MasterPage

    Protected Sub Pgae_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If LCase(Request.ServerVariables("SCRIPT_NAME")) <> "/callcentre/default.aspx" Then
            If Session("EBShopCountry") = "" Then Response.Redirect("sessionTimeout.aspx")
        End If
        If Not Page.IsPostBack Then
            'Get country name and display it
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters("@countryCode").Value = Session("EBShopCountry")
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then lblCountryName.Text = ds.Tables(0).Rows(0)("countryName")
            Catch ex As Exception
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
End Class

