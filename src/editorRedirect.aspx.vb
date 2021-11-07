
Partial Class editorRedirect
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session("EBLanguage") = Request.QueryString("lang")
        Server.Transfer(Request.QueryString("target"))
    End Sub
End Class
