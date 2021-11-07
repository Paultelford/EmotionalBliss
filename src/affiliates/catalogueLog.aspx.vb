
Partial Class affiliates_catalogueLog
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Not (Session("EBAffEBDistributor") Or Session("EBAffEBUser")) Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub gvCatalogue_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvDay.Visible = True
    End Sub
End Class
