
Partial Class affiliates_logout
    Inherits System.Web.UI.Page
    Protected Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session("EBAffID") = ""
        Session("EBAffUsername") = ""
        Session("EBAffCountryCode") = ""
        Session("EBAffCurrencyCode") = ""
        Session("EBAffCurrencySign") = ""
        Session("EBAffEBDistributor") = "false"
        Session("EBAffEBDistributorCountryCode") = ""
        Session("EBAffEBUser") = "false"
        Session("EBAffEBUserCountryCode") = ""
        Session("EBDistMenuValue") = ""
        Session("EBDistID") = ""
        Session("EBAffTypeID") = ""
        Session("EBHideCountry") = ""
        Cache.Remove("EBImageMap" & Session("EBLanguage"))
        FormsAuthentication.SignOut()
        'Response.Redirect("default.aspx")
        Dim myCookie As New HttpCookie("affSetting")
        myCookie.Expires = DateTime.Now.AddDays(-1D)
        Response.Cookies.Add(myCookie)
    End Sub
End Class
