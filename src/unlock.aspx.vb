
Partial Class unlock
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        For Each user As MembershipUser In Membership.GetAllUsers
            Response.Write(user.UserName)
            If user.IsLockedOut Then Response.Write(" LOCKEDOUT")
            Response.Write("<br>")
        Next
        If Request.QueryString("u") <> "" Then
            Dim u As MembershipUser = Membership.GetUser(Request.QueryString("u"))
            u.UnlockUser()
            Response.Write("<br><br>" & u.UserName & " has been unlocked")
        End If
    End Sub
End Class
