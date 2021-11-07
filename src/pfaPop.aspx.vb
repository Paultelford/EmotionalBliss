
Partial Class pfaPop
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        img.ImageUrl = "~/images/pfaAwards/" & Request.QueryString("img")
    End Sub
End Class
