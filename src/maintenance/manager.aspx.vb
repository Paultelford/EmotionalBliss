Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_manager
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _gvReactive_usernamePos As Integer = 0

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Membership.Provider.ApplicationName = "EBProvider"
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        _content = Master.FindControl("ContentPlaceHolder1")

        'to delete---
        'lblMessage.Text = ""
        'Dim gvMain As GridView = _login.FindControl("gvMaintenanceUsers")
        'Membership.Provider.ApplicationName = "EBProvider"
        'gvMain.DataSource = Membership.GetAllUsers
        'gvMain.DataBind()
        'Membership.Provider.ApplicationName = "EBAffProvider"
        'gvDistributors.DataSource = Membership.GetAllUsers
        'gvDistributors.DataBind()
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Roles.IsUserInRole("administrator") Then
            'lblAccessDenied = _content.FindControl("lblAccessDenied")
            lblAccessDenied.Visible = True
            panUsers.Visible = False
            lnkNewUser.Visible = False
        End If
    End Sub

    'Page Events
    Protected Sub gvMaintenance_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Membership.Provider.ApplicationName = "EBProvider"
        gv.DataSource = Membership.GetAllUsers
        gv.DataBind()
        'lblPassword.Text = ""
        'lnkShowPassword.Visible = True
    End Sub
    Protected Sub gvDistributors_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Membership.Provider.ApplicationName = "EBAffProvider"
        gv.DataSource = Membership.GetAllUsers
        gv.DataBind()
        'lblPassword.Text = ""
        'lnkShowPassword.Visible = True
    End Sub
    Protected Sub lblMessage_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label = CType(sender, Label)
        lbl.Text = ""
    End Sub
    Protected Sub gvMaintenance_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        e.Row.BackColor = Drawing.Color.White 'Reset background color to white for all unselected crows
    End Sub
    Protected Sub gvDistributors_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        'Dim drpType As DropDownList = _content.FindControl("drpType")
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.BackColor = Drawing.Color.White 'Reset background color to white for all unselected crows
            'Only show Affilaites or Distributors (Depending on dropdown value)
            If LCase(drpType.SelectedValue) = "distributors" Then
                If Not Roles.IsUserInRole(gvDistributors.DataKeys(e.Row.RowIndex).Value, "Distributor") Then e.Row.Visible = False
            Else
                'Remove Distributors and Users
                If Roles.IsUserInRole(gvDistributors.DataKeys(e.Row.RowIndex).Value, "Distributor") Then e.Row.Visible = False
                If Roles.IsUserInRole(gvDistributors.DataKeys(e.Row.RowIndex).Value, "User") Then e.Row.Visible = False
            End If
        End If
    End Sub
    Protected Sub drpCountry_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        Dim hid As Label = dvAff.FindControl("lblHidCountry")
        Try
            drp.SelectedValue = hid.Text
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub drpCurrency_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        Dim hid As Label = dvAff.FindControl("lblHidCurrency")
        Try
            drp.SelectedValue = hid.Text
            'Make the DataText show in caps
            For Each li As ListItem In drp.items
                li.Text = uCase(li.Text)
            Next
        Catch ex As Exception

        End Try
    End Sub
    Protected Sub drpCurrency2_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        Try
            'Make the DataText show in caps
            For Each li As ListItem In drp.items
                li.Text = uCase(li.Text)
            Next
        Catch ex As Exception

        End Try
    End Sub

    'User Events
    Protected Sub gvMaintenence_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvMaintenance.Rows(gvMaintenance.SelectedIndex).BackColor = Drawing.Color.Aqua 'Hilight selected user
        hidProvider.Value = "EBProvider"
        hidUser.Value = gvMaintenance.DataKeys(gvMaintenance.SelectedIndex).Value
        'Hide others
        panReActivate.Visible = False
        panReActivateDetails.Visible = False
        gvReactive.SelectedIndex = -1
        showUserDetails()
    End Sub
    Protected Sub gvDistributors_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvDistributors.Rows(gvDistributors.SelectedIndex).BackColor = Drawing.Color.Cyan 'Hilight selected user
        hidProvider.Value = "EBAffProvider"
        hidUser.Value = gvDistributors.DataKeys(gvDistributors.SelectedIndex).Value
        showUserDetails()
        'Hide others
        panReActivate.Visible = False
        panReActivateDetails.Visible = False
        gvReactive.SelectedIndex = -1
    End Sub
    Protected Sub gvReactivate_selectedIndexChanged(ByVal sender As Object, ByVal E As EventArgs)
        panReActivateDetails.Visible = True
        btnReActivateSubmit.Text = btnReActivateSubmit.Text & " " & gvReactive.SelectedRow.Cells(_gvReactive_usernamePos).Text
    End Sub
    Protected Sub chkLockedOut_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Membership.Provider.ApplicationName = hidProvider.Value
        Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
        If chkLockedOut.Checked Then
            chkLockedOut.Checked = False
            showMessage("You cannot lock a user out. Try deleting them Inactive instead.")
        Else
            user.UnlockUser()
            showMessage("User <b>" & user.UserName & "</b> has been unlocked.")
        End If

    End Sub
    Protected Sub chkRoleAdmin_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Membership.Provider.ApplicationName = hidProvider.Value
        Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
        If chkRoleAdmin.Checked Then
            Roles.AddUserToRole(user.UserName, "Administrator")
            showMessage("User <b>" & user.UserName & "</b> added to Administrator role.")
        Else
            Roles.RemoveUserFromRole(user.UserName, "Administrator")
            showMessage("User <b>" & user.UserName & "</b> removed from Administrator role.")
        End If
    End Sub
    Protected Sub chkRoleDistributor_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Membership.Provider.ApplicationName = hidProvider.Value
        Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
        If chkRoleDistributor.Checked Then
            Roles.AddUserToRole(user.UserName, "Distributor")
            showMessage("User <b>" & user.UserName & "</b> added to Distributor role.")
        Else
            Roles.RemoveUserFromRole(user.UserName, "Distributor")
            showMessage("User <b>" & user.UserName & "</b> removed from Distributor role.")
        End If
    End Sub
    Protected Sub chkRoleConsultancy_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Membership.Provider.ApplicationName = hidProvider.Value
        Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
        If chkRoleConsultancy.Checked Then
            Roles.AddUserToRole(user.UserName, "Consultancy")
            showMessage("User <b>" & user.UserName & "</b> added to Consultancy role.")
        Else
            Roles.RemoveUserFromRole(user.UserName, "Consultancy")
            showMessage("User <b>" & user.UserName & "</b> removed from Consultancy role.")
        End If
    End Sub
    Protected Sub chkRoleRoyalty_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Membership.Provider.ApplicationName = hidProvider.Value
        Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
        If chkRoleRoyalty.Checked Then
            Roles.AddUserToRole(user.UserName, "Royalty")
            showMessage("User <b>" & user.UserName & "</b> added to Royalty role.")
        Else
            Roles.RemoveUserFromRole(user.UserName, "Royalty")
            showMessage("User <b>" & user.UserName & "</b> removed from Royalty role.")
        End If
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub btnDelete_click(ByVal sender As Object, ByVal e As EventArgs)
        Membership.Provider.ApplicationName = hidProvider.Value
        Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
        Dim username As String = user.UserName
        If Membership.DeleteUser(username, True) Then
            showMessage("User <b>" & username & "</b> has been deleted.")
            panUserEdit.Visible = False
            If Membership.Provider.ApplicationName = "EBAffProvider" Then
                'If user was affilaite/distributor then set as inactive in the affilaites table
                setAffInactive(username)
            End If
            Membership.Provider.ApplicationName = "EBProvider"
            gvMaintenance.DataSource = Membership.GetAllUsers
            gvMaintenance.DataBind()
            Membership.Provider.ApplicationName = "EBAffProvider"
            gvDistributors.DataSource = Membership.GetAllUsers
            gvDistributors.DataBind()
        Else
            showMessage("User <b>" & username & "</b> could not be deleted.")
        End If
    End Sub
    Protected Sub lnkNewUser_click(ByVal sender As Object, ByVal e As EventArgs)
        panUserEdit.Visible = False
        panUserAdd.Visible = True
        Membership.Provider.ApplicationName = "EBAffProvider"
    End Sub
    Protected Sub btnCreate_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim bError As Boolean = False
        Dim bDistributor As Boolean = False
        'Lookup username to see if it exists
        If LCase(drpAddType.SelectedValue) = "peartree" Then
            Membership.Provider.ApplicationName = "EBProvider"
        Else
            Membership.Provider.ApplicationName = "EBAffProvider"
            bDistributor = True
        End If
        If Not Membership.GetUser(txtUsername.Text) Is Nothing Then
            lblMessage.Text = "Username already exists, please choose another."
            bError = True
        End If
        If Not bError Then
            'Text for password minimum length
            If Len(txtPassword.Text) < Membership.MinRequiredPasswordLength Then
                lblMessage.Text = "Password too short, must be at least " & Membership.MinRequiredPasswordLength & " characters."
                bError = True
            End If
            If Not bError Then
                'Username ok, password length ok
                If bDistributor Then
                    'Create Distributor/User and add to database
                    Membership.CreateUser(txtUsername.Text, txtPassword.Text)
                    Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                    Dim oCmd As SqlCommand
                    If LCase(drpAddType.SelectedValue) = "distributor" Then
                        oCmd = New SqlCommand("procAffiliateDistributorInsert", oConn)
                    Else
                        oCmd = New SqlCommand("procAffiliateUserInsert", oConn)
                    End If
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@affFirstName", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@affSurname", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@affUsername", SqlDbType.VarChar, 20))
                        .Parameters.Add(New SqlParameter("@affPassword", SqlDbType.VarChar, 20))
                        .Parameters.Add(New SqlParameter("@affUserCountryCode", SqlDbType.VarChar, 5))
                        .Parameters.Add(New SqlParameter("@affActive", SqlDbType.Bit))
                        .Parameters("@affFirstName").Value = ""
                        .Parameters("@affSurname").Value = ""
                        .Parameters("@affUsername").Value = txtUsername.Text
                        .Parameters("@affPassword").Value = txtPassword.Text
                        .Parameters("@affUserCountryCode").Value = drpCountry.SelectedValue
                        .Parameters("@affActive").Value = True
                    End With
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        lblMessage.Text = "Error creating new user; " & ex.ToString
                        bError = True
                    Finally
                        oCmd.Dispose()
                        oConn.Dispose()
                    End Try
                    If Not bError Then
                        lblMessage.Text = "The user has been created"
                        gvDistributors.DataSource = Membership.GetAllUsers
                        gvDistributors.DataBind()
                        panNewCountry.Visible = False
                        'add shopIntro to the siteMenus table for this country.  This is needed by the Distributor Departments page. 
                        'If shopIntro doesnt exist then the departments do not get added correctly
                        addShopIntro(drpCountry.SelectedValue)
                    End If
                Else
                    'Create Peartree user
                    Membership.CreateUser(txtUsername.Text, txtPassword.Text)
                    lblMessage.Text = "The user has been created"
                    Membership.Provider.ApplicationName = hidProvider.Value
                    gvMaintenance.DataSource = Membership.GetAllUsers
                    gvMaintenance.DataBind()
                End If
                If Not bError Then
                    'Add user to selected roles, suppress error if they are already in the selected roles
                    Try
                        If chkAddAdministrator.Checked Then Roles.AddUserToRole(txtUsername.Text, "Administrator")
                        If chkAddDistributor.Checked Then Roles.AddUserToRole(txtUsername.Text, "Distributor")
                        If chkAddConsultancy.Checked Then Roles.AddUserToRole(txtUsername.Text, "Consultancy")
                    Catch ex As Exception
                    End Try
                    'clear user data so its empty if user tries to add another user
                    txtUsername.Text = ""
                    txtPassword.Text = ""
                    chkAddAdministrator.Checked = False
                    chkAddDistributor.Checked = False
                    chkAddConsultancy.Checked = False
                    drpAddType.SelectedIndex = 0
                    panUserAdd.Visible = False
                End If

            End If
        End If
    End Sub
    Protected Sub drpAddType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Select Case LCase(drpAddType.SelectedValue)
            Case "distributor"
                drpCountry.Visible = True
                hidProvider.Value = "EBAffProvider"
            Case "user"
                drpCountry.Visible = True
                hidProvider.Value = "EBAffProvider"
            Case "peartree"
                drpCountry.Visible = False
                hidProvider.Value = "EBProvider"
        End Select
    End Sub
    Protected Sub btnShowPassword_click(ByVal sender As Object, ByVal e As EventArgs)
        lnkShowPassword.Visible = False
        lblPassword.Visible = True
        Membership.Provider.ApplicationName = hidProvider.Value
        Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
        Try
            lblPassword.Text = user.GetPassword
            tblPassword.Visible = True
            lblPassword.Text = FormatNumber("guts", 2)
        Catch ex As Exception
            'Password must be hashed and cannot be retrieved.
            'Prompt user to reset the password
            tblPassword.Visible = False
            tblResetPassword.Visible = True
        End Try
    End Sub
    Protected Sub lnkChangePassword_click(ByVal sender As Object, ByVal e As EventArgs)
        tblNewPasssword.Visible = True
    End Sub
    Protected Sub btnConfirmNewPass_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("newPass")
        If Page.IsValid Then
            Membership.Provider.ApplicationName = hidProvider.Value
            Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
            user.ChangePassword(lblPassword.Text, txtNewPassword.Text)
            showMessage("Users password has been changed to '" & txtNewPassword.Text & "'.")
            'Tidy up
            lnkShowPassword.Visible = True
            tblPassword.Visible = False
            txtNewPassword.Text = ""
        End If
    End Sub
    Protected Sub btnResetPasswordSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("resetPass")
        If Page.IsValid Then
            Dim user As MembershipUser = Membership.GetUser(lblUsername.Text)
            Dim newPassword As String = Membership.Provider.ResetPassword(lblUsername.Text, Nothing)
            user.ChangePassword(newPassword, txtResetPassword.Text)
            'swap table visibility back
            tblPassword.Visible = True
            tblResetPassword.Visible = False
            showMessage("Users password has been changed to '" & txtResetPassword.Text & "'.")
            'Tidy up
            lnkShowPassword.Visible = True
            tblPassword.Visible = False
            txtNewPassword.Text = ""
        End If
    End Sub
    Protected Sub lnkReActivate_click(ByVal sender As Object, ByVal e As EventArgs)
        panReActivate.Visible = True
        'Hide others
        panUserEdit.Visible = False
        gvMaintenance.SelectedIndex = -1
        gvDistributors.SelectedIndex = -1
        panNewCountry.Visible = False
        txtNewCurrencyCode.Text = ""
        txtNewCurrencySign.Text = ""
        txtNewCountryName.Text = ""
        txtNewCountryCode.Text = ""
    End Sub
    Protected Sub btnReActivateSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("np")
        If Page.IsValid() Then
            Dim username As String = gvReactive.SelectedRow.Cells(_gvReactive_usernamePos).Text
            'Update users membership profile
            Dim bOK As Boolean = True
            Try
                If False Then
                    Dim user As MembershipUser = Membership.GetUser(username)
                    Dim newPassword As String = Membership.Provider.ResetPassword(username, Nothing)
                    user.ChangePassword(user.ResetPassword(), txtNewPass.Text)
                End If
                Membership.CreateUser(username, txtNewPass.Text)
            Catch ex As Exception
                bOK = False
                lblMessage.Text = "<font color='red'>An error occured while Re Activating the user. Please contact tech support.</font>"
                Dim si As New siteInclude
                si.AddError("maintenance/manager.aspx.vb", "btnReActivateSubmit_click(name=" & gvReactive.SelectedRow.Cells(_gvReactive_usernamePos).Text & ", password=" & txtNewPass.Text & "); " & ex.ToString())
                si = Nothing
            End Try
            If bOK Then
                'Update new Password in db and set to active
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procAffiliateByUsernamePasswordActiveUpdate", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@password", SqlDbType.VarChar, 20))
                    .Parameters("@username").Value = username
                    .Parameters("@password").Value = txtNewPass.Text
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    Dim si As New siteInclude
                    si.AddError("maintenance/manager.aspx.vb", "btnReActivateSubmit_click(name=" & username & ", password=" & txtNewPass.Text & "); " & ex.ToString())
                    si = Nothing
                    bOK = False
                    lblMessage.Text = "<font color='red'>An error occured while Re Activating the user. Please contact tech support.</font>"
                Finally
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
                If bOK Then
                    'Tidy up
                    txtNewPass.Text = ""
                    panReActivate.Visible = False
                    panReActivateDetails.Visible = False
                    gvReactive.SelectedIndex = -1
                    Membership.Provider.ApplicationName = "EBAffProvider"
                    gvDistributors.DataSource = Membership.GetAllUsers
                    gvDistributors.DataBind()
                End If
            End If
        End If
    End Sub
    Protected Sub dvAff_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        lblMessage.Text = "Affiliate details updated"
        panUserEdit.Visible = False
    End Sub
    Protected Sub lnkNewCountry_click(ByVal sender As Object, ByVal e As EventArgs)
        panNewCountry.Visible = True
    End Sub
    Protected Sub txtNewCountryName_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Make each unique word start with a capital
        Dim txt As TextBox = CType(sender, TextBox)
        Dim words() As String = split(Trim(txt.Text), " ")
        txt.Text = ""
        For Each word As String In words
            txt.Text = txt.Text & formatWord(word) & " "
        Next
        txt.Text = Left(txt.Text, Len(txt.Text) - 1)
    End Sub
    Protected Sub txtNewCountryCode_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = CType(sender, TextBox)
        txt.Text = uCase(txt.Text)
    End Sub
    Protected Sub txtNewCurrencyCode_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = CType(sender, TextBox)
        txt.Text = uCase(txt.Text)
    End Sub
    Protected Sub btnNewCountry_click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub btnNewCurrency_click(ByVal sender As Object, ByVal e As EventArgs)
        'Insert currency coe to db.
        'SP will return 1 if successfully added, or 0 if the code already exists
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCurrencyInsert", oConn)
        Dim result As Boolean
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@currencyCode", SqlDbType.VarChar, 3))
            .Parameters.Add(New SqlParameter("@currencySign", SqlDbType.VarChar, 1))
            .Parameters.Add(New SqlParameter("@result", SqlDbType.Bit))
            .Parameters("@currencyCode").Value = lCase(txtNewCurrencyCode.Text)
            .Parameters("@currencySign").Value = txtNewCurrencySign.Text
            .Parameters("@result").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            result = CBool(oCmd.Parameters("@result").Value)
            If Not result Then
                'Currency code already exists and was not added
                lblMessage.Text = "<font color='red'>The Currency Code '" & txtNewCurrencyCode.Text & "' already exists in the Currency table.</font>"
            Else
                'Currency code was added ok.
                'Add the code to the drpNewCountryCurrency dropdown so it can immediately be chosen if the user wants to add a new country
                drpNewCountryCurrency.DataBind()
                drpCurrency.DataBind()
                'Clean up
                txtNewCurrencyCode.Text = ""
                txtNewCurrencySign.Text = "" 'Show complete msg
                lblMessage.Text = "<font color='red'>New currency Added.</font>"
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.AddError("maintenance/manager.aspx.vb", "btnNewCurrency_click(code=" & txtNewCurrencyCode.Text & ", sign=" & txtNewCurrencySign.Text & "); " & ex.ToString())
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnAddCountry_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("ncc")
        If Page.IsValid() Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procCountryInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryName", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@currencyCode", SqlDbType.VarChar, 3))
                .Parameters.Add(New SqlParameter("@defaultLanguage", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@result", SqlDbType.Bit))
                .Parameters("@countryName").Value = txtNewCountryName.Text
                .Parameters("@countryCode").Value = txtNewCountryCode.Text
                .Parameters("@currencyCode").Value = lCase(drpNewCountryCurrency.SelectedValue)
                .Parameters("@defaultLanguage").Value = drpNewCountryLanguage.SelectedValue
                .Parameters("@result").Direction = ParameterDirection.Output
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
                If CBool(oCmd.Parameters("@result").Value) Then
                    'Country was added ok.
                    'Add the code to the drpNewCountryCurrency dropdown so it can immediately be chosen if the user wants to add a new country
                    drpCountry.DataBind()
                    'Clean up
                    txtNewCountryName.Text = ""
                    txtNewCountryCode.Text = ""
                    'Show complete msg
                    lblMessage.Text = "<font color='red'>New Country Added!</font>"
                Else
                    lblMessage.Text = "<font color='red'>The Country Code is already in use by another country. Please choose another.</font>"
                End If
            Catch ex As Exception
                Dim si As New siteInclude
                si.AddError("maintenance/manager.aspx.vb", "btnAddCountry_click(name=" & txtNewCountryName.Text & ",code=" & drpNewCountryCurrency.SelectedValue & "); " & ex.ToString())
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub

    'Subs
    Protected Sub showMessage(ByVal s As String)
        lblMessage.Text = s
    End Sub
    Protected Sub showUserDetails()
        'Show user edit panel
        panUserEdit.Visible = True
        Membership.Provider.ApplicationName = hidProvider.Value
        Dim user As MembershipUser = Membership.GetUser(hidUser.Value)
        lblUsername.Text = hidUser.Value
        chkLockedOut.Checked = user.IsLockedOut
        chkRoleAdmin.Checked = Roles.IsUserInRole(hidUser.Value, "Administrator")
        chkRoleDistributor.Checked = Roles.IsUserInRole(hidUser.Value, "Distributor")
        chkRoleConsultancy.Checked = Roles.IsUserInRole(hidUser.Value, "Consultancy")
        'Tidy up
        lnkShowPassword.Visible = True
        tblPassword.Visible = False
        txtNewPassword.Text = ""
    End Sub
    Protected Sub setAffInactive(ByVal name As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByUsernameActiveUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@active", SqlDbType.Bit))
            .Parameters("@username").Value = name
            .Parameters("@active").Value = False
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.AddError("maintenance/manager.aspx.vb", "setAffInactive(name=" & name & "); " & ex.ToString())
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub addShopIntro(ByVal countryCode)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procImageMapsInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@action", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@parentIndex", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 150))
            .Parameters.Add(New SqlParameter("@section", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@navText", SqlDbType.VarChar, 150))
            .Parameters.Add(New SqlParameter("@menuName", SqlDbType.VarChar, 150))
            .Parameters("@type").Value = "parent"
            .Parameters("@country").Value = lcase(countryCode)
            .Parameters("@action").Value = "postback"
            .Parameters("@parentIndex").Value = "0"
            .Parameters("@url").Value = "~/shopIntro.aspx"
            .Parameters("@section").Value = "menu"
            .Parameters("@navText").Value = "Shop Intro"
            .Parameters("@menuName").Value = "Shop"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblMessage.Text = "Error creating shopIntro entry for " & countryCode & "; " & ex.ToString
            Dim si As New siteInclude
            si.addError("maintenance/manager.aspx.vb", "addShopIntro(country=" & countryCode & "); " & ex.ToString())
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub

    'Functions 
    Protected Function showDate(ByVal o As Object) As String
        Dim result As String = "Unknown"
        If Not IsDBNull(o) Then result = "<font color='black'>" & FormatDateTime(o.ToString, DateFormat.ShortDate) & "</font>"
        Return result
    End Function
    Protected Function showUserType(ByVal dist As Boolean, ByVal user As Boolean) As String
        Dim result As String = "Affiliate"
        If dist Then result = "Distributor"
        If user Then result = "EB User"
        Return result
    End Function
    Protected Function formatWord(ByVal word) As String
        Dim result As String = ""
        result = uCase(Left(word, 1))
        result = result & lCase(Right(word, Len(word) - 1))
        Return result
    End Function
End Class
