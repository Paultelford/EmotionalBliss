
Partial Class mediapictures_logout
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session("ebMediaAccess") = ""
    End Sub
End Class
