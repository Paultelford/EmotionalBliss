Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class shop_countryChange
    Inherits System.Web.UI.Page

    Protected Sub Page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        btnChange.Text = btnChange.Text & CType(Context.Items("newCountryName"), String) & " shop"
        If Not Page.IsPostBack Then newCountryCode.Value = CType(Context.Items("newCountryCode"), String)
    End Sub
    Protected Sub btnChange_click(ByVal sender As Object, ByVal e As EventArgs)
        Session("EBShopCountry") = newCountryCode.Value
        Session("EBShopCurrency") = getCurrency(Session("EBShopCountry"))
        Profile.EBCart.emptyBasket()
        Server.Transfer("default.aspx")
    End Sub
    Private Function getCurrency(ByVal countryCode As String) As String
        Dim result As String = "gbp"
        Dim oConn As New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New System.Data.SqlClient.SqlCommand("procCountryByCountryCodeCurrencySelect", oConn)
        Dim da As New System.Data.SqlClient.SqlDataAdapter
        Dim ds As New System.Data.DataSet
        With oCmd
            .CommandType = Data.CommandType.StoredProcedure
            .Parameters.Add(New System.Data.SqlClient.SqlParameter("@countryCode", Data.SqlDbType.VarChar))
            .Parameters("@countryCode").Value = countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New System.Data.SqlClient.SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0)("currencyCode")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("MasterPageShop.aspx.vb", "getCurrency(" & countryCode & ")::" & ex.ToString)
            si = Nothing
        End Try
        Return result
    End Function
End Class