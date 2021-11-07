
Partial Class maintenance_managerDelete
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Membership.DeleteUser(Request.QueryString("name"), False)
        Response.Redirect("manager.aspx")
    End Sub

End Class
