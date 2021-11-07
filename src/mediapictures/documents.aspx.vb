
Partial Class mediapictures_documents
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If LCase(Session("ebMediaAccess")) <> "true" Then Response.Redirect("~/mediapictures/default.aspx")
    End Sub
End Class
