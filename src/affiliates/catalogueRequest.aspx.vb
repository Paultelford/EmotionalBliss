
Partial Class affiliates_catalogueRequest
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Not (Session("EBAffEBDistributor") Or Session("EBAffEBUser")) Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub dvRequest_inserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        dvRequest.Visible = False
        lblComplete.Visible = True
        lnkAgain.Visible = True
        dvRequest.Controls.Clear()
    End Sub
    Protected Sub lnkAgain_click(ByVal sender As Object, ByVal e As EventArgs)
        dvRequest.Visible = True
        lblComplete.Visible = False
        lnkAgain.Visible = False
    End Sub
End Class
