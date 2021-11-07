
Partial Class affiliates_MasterPageAff
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblUsername.Text = Session("EBAffUsername")
    End Sub
    Protected Sub lnkLogout_click(ByVal sender As Object, ByVal e As EventArgs)
        Session("EBAffUsername") = Nothing
        Session("EBAffID") = Nothing
        Response.Redirect("~/affiliates/default.aspx")
    End Sub
End Class

