
Partial Class affiliates_callcentreNews
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Not (Session("EBAffEBDistributor") Or Session("EBAffEBUser")) Then Response.Redirect("default.aspx")
    End Sub

    Protected Sub gvNews_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        dvArticle.ChangeMode(DetailsViewMode.Edit)
    End Sub
    Protected Sub dvArticle_itemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        gvNews.DataBind()
    End Sub
    Protected Sub dvArticle_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        gvNews.DataBind()
        dvArticle.ChangeMode(DetailsViewMode.Insert)
    End Sub
End Class
