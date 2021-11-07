Imports System.Drawing
Imports System.Drawing.Imaging

Partial Class DefaultMedia
    Inherits BasePage
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        If Session("ebMediaAccess") <> "true" Then ebMenu1.menuName = "Press"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Session("ebMediaAccess") = "true" Then
            panLoggedin.Visible = True
            panLogin.Visible = False
        Else
            panLoggedin.Visible = False
            panLogin.Visible = True
            txtUsername.Focus()
        End If
        lblIntro.Text = getDBResourceString("lblIntro")
    End Sub

    'Page

    'User
    Protected Sub btnLogin_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("login")
        If Page.IsValid Then
            If LCase(txtUsername.Text) = "media" And LCase(txtPassword.Text) = "ebmedia" Then
                Session("ebMediaAccess") = "true"
                Response.Redirect("default.aspx")
            Else
                lblError.Text = "Incorrect Username or Password"
            End If
        End If
    End Sub

End Class
