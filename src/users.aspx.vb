
Partial Class users
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        Response.Write(Membership.Provider.ApplicationName)
        'If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")

    End Sub
    Protected Sub drpProviders_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        setProvider()
        bindUsers()
        bindRoles()
    End Sub
    Protected Sub btnAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        setProvider()
        Membership.CreateUser(txtUsername.Text, "placebo")
        Membership.GetUser(txtUsername.Text).IsApproved = True
        bindUsers()
    End Sub
    Protected Sub btnRoleAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        Roles.CreateRole(txtNewRole.Text)
        bindRoles()
    End Sub
    Protected Sub btnCreate_click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Membership.CreateUser(txtCreate.Text, "placebo")
        Catch ex As Exception
            lblErrorCreate.Text = ex.ToString
        End Try

    End Sub
    Protected Sub btnRoleAssign_click(ByVal sender As Object, ByVal e As EventArgs)
        lblError.Text = ""
        lblError2.Text = ""
        lblError3.Text = ""
        Try
            Roles.AddUserToRole(txtUser.Text, txtRole.Text)
        Catch ex As Exception
            lblError.Text = ex.ToString
        End Try
    End Sub
    Protected Sub btnChange_click(ByVal sender As Object, ByVal e As EventArgs)
        lblError.Text = ""
        lblError2.Text = ""
        lblError3.Text = ""
        setProvider()
        Try
            Membership.GetUser(txtUser_Pass.Text).ResetPassword()
            Membership.GetUser(txtUser_Pass.Text).ChangePassword(Membership.GetUser(txtUser_Pass.Text).GetPassword, txtNewPass.Text)
            lblError2.Text = "Password updated"
        Catch ex As Exception
            lblError2.Text = ex.ToString
        End Try
    End Sub
    Protected Sub btnDelete_clicked(ByVal sender As Object, ByVal e As EventArgs)
        lblError.Text = ""
        lblError2.Text = ""
        lblError3.Text = ""
        setProvider()
        Try
            Membership.DeleteUser(txtDelete.Text)
        Catch ex As Exception
            lblError3.Text = ex.ToString
        End Try
        bindUsers()
    End Sub
    Protected Sub btnGetPassword_click(ByVal sender As Object, ByVal e As EventArgs)
        lblError.Text = ""
        lblError2.Text = ""
        lblError3.Text = ""
        setProvider()
        Try
            lblPW.Text = Membership.GetUser(txtGetPassword.Text).GetPassword
        Catch ex As Exception
            lblPW.Text = ex.ToString
        End Try
        bindUsers()
    End Sub
    Protected Sub txtApprove_click(ByVal sender As Object, ByVal e As EventArgs)
        lblError.Text = ""
        lblError2.Text = ""
        lblError3.Text = ""
        setProvider()
        Try
            Dim usr As MembershipUser = Membership.GetUser(txtApprove.Text)
            usr.IsApproved = True
            Membership.UpdateUser(usr)
            bindUsers()
        Catch ex As Exception
            lblApproveError.Text = ex.ToString()
        End Try
    End Sub
    Protected Sub setProvider()
        Membership.Provider.ApplicationName = drpProviders.SelectedValue
    End Sub
    Protected Sub bindUsers()
        gvUsers.DataSource = Membership.GetAllUsers
        gvUsers.DataBind()
    End Sub
    Protected Sub bindRoles()
        gvRoles.DataSource = Roles.GetAllRoles
        gvRoles.DataBind()
    End Sub
End Class
