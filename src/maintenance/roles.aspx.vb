
Partial Class maintenance_roles
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            If Not Page.IsPostBack Then
                _login = Master.FindControl("logMaintenance")
                _content = _login.FindControl("ContentPlaceHolder1")
                gvRoles.DataSource = Roles.GetAllRoles
                gvRoles.DataBind()
                If gvRoles.Rows.Count > 0 Then gvRoles.HeaderRow.Cells(0).Text = "Current Roles:"
                BindRoles()
                For Each user As Object In Membership.GetAllUsers
                    drpUser.Items.Add(New ListItem(user.ToString, user.ToString))
                Next
                bindUserRoles()
            End If
        End If
    End Sub
    Protected Sub btnRoleSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        If Not Roles.RoleExists(txtRole.Text) Then Roles.CreateRole(txtRole.Text)
        txtRole.Text = ""
        gvRoles.DataSource = Roles.GetAllRoles
        gvRoles.DataBind()
        bindRoles()
    End Sub
    Protected Sub btnAddRoleSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        If Not Roles.IsUserInRole(drpRoles.SelectedValue, drpRoles.SelectedValue) Then
            Roles.AddUserToRole(drpUser.SelectedValue, drpRoles.SelectedValue)
            bindUserRoles()
        End If
    End Sub
    Protected Sub drpUser_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        bindUserRoles()
    End Sub
    Protected Sub bindUserRoles()
        gvUserRoles.DataSource = Roles.GetRolesForUser(drpUser.SelectedValue)
        gvUserRoles.DataBind()
        If gvUserRoles.Rows.Count > 0 Then gvUserRoles.HeaderRow.Cells(0).Text = drpUser.SelectedValue & "'s roles"
    End Sub
    Protected Sub BindRoles()
        drpRoles.Items.Clear()
        For Each role As String In Roles.GetAllRoles
            drpRoles.Items.Add(New ListItem(role, role))
        Next
    End Sub
End Class
