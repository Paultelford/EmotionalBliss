Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_userManagement
    Inherits System.Web.UI.Page
    Protected Const _gvCurrentUsers_loginNamePos As Integer = 2
    Protected Const _gvCurrentUsers_activePos As Integer = 8

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        If Not CBool(Session("EBAffEBDistributor")) Then Response.Redirect("default.aspx")
        lblCountryCode.Text = UCase(Session("EBAffEBDistributorCountryCode"))

    End Sub

    'Page Events
    Protected Sub gvCurrentUsers_rowDatabound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.RowState = DataControlRowState.Normal Then If Not CType(e.Row.Cells(_gvCurrentUsers_activePos).Text, Boolean) Then e.Row.BackColor = Drawing.ColorTranslator.FromHtml("#EFEFEF")
        End If
    End Sub
    Protected Sub gvCurrentUsers_rowDeleted(ByVal sender As Object, ByVal e As GridViewDeletedEventArgs)
        'User has been deleted from db, now remove from meberhip profile
        Membership.DeleteUser(e.Values(_gvCurrentUsers_loginNamePos))
    End Sub

    'User Events
    Protected Sub btnSelectAll_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox
        For Each row As GridViewRow In gvPermissions.Rows
            If row.RowType = DataControlRowType.DataRow Then
                chk = row.FindControl("chkAccess")
                chk.Checked = True
            End If
        Next
    End Sub
    Protected Sub btnSelectNone_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox
        For Each row As GridViewRow In gvPermissions.Rows
            If row.RowType = DataControlRowType.DataRow Then
                chk = row.FindControl("chkAccess")
                chk.Checked = False
            End If
        Next
    End Sub
    Protected Sub btnCancel_click(ByVal sender As Object, ByVal e As EventArgs)
        panAddUser.Visible = False
        panEditPermissions.Visible = False
        panCurrentUsers.Visible = True
    End Sub
    Protected Sub gvCurrentUsers_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Show the selected users current permissions
        Dim affID As Integer = gvCurrentUsers.SelectedDataKey.Value
        panAddUser.Visible = False
        panCurrentUsers.Visible = False
        panEditPermissions.Visible = True
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateMenuPermissionsByAffIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters("@affID").Value = affID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            gvPermissions.DataSource = ds
            gvPermissions.DataBind()
            If ds.Tables(0).Rows.Count > 0 Then lblEditUsersFullname.Text = "<font color='black'>" & ds.Tables(0).Rows(0)("affFirstname") & " " & ds.Tables(0).Rows(0)("affSurname")
        Catch ex As Exception
            lblEditError.Text = ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnEditUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        'Step 1 - Remove all data for this user from the affiliateMenuPermissions table
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffilaiteMenuPermissionByUserIDDelete", oConn)
        Dim bError As Boolean = False
        Dim userID As Integer = gvCurrentUsers.SelectedDataKey.Value
        Dim chkAccess As CheckBox
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affUserID", SqlDbType.Int))
            .Parameters("@affUserID").Value = userID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblEditError.Text = lblEditError.Text & "An error occurend while clearing users permissions." & ex.ToString
            bError = True
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            'Add new permissions 1 by 1
            For Each row As GridViewRow In gvPermissions.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    chkAccess = row.FindControl("chkAccess")
                    If chkAccess.Checked Then
                        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                        oCmd = New SqlCommand("procAffiliateMenuPermissionsInsert", oConn)
                        With oCmd
                            .CommandType = CommandType.StoredProcedure
                            .Parameters.Add(New SqlParameter("@affUserID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@affMenuID", SqlDbType.Int))
                            .Parameters("@affUserID").Value = userID
                            .Parameters("@affMenuID").Value = gvPermissions.DataKeys(row.RowIndex).Value
                        End With
                        Try
                            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                            oCmd.ExecuteNonQuery()
                        Catch ex As Exception
                            lblEditError.Text = lblEditError.Text & "Error occured while updating users permissions. " & ex.ToString
                            bError = True
                        Finally
                            oCmd.Dispose()
                            oConn.Dispose()
                        End Try
                    End If
                End If
            Next
            If Not bError Then
                'Success, go back to original view
                panCurrentUsers.Visible = True
                panEditPermissions.Visible = False
                panAddUser.Visible = False
                lblEditError.Text = ""
            End If
        End If
    End Sub
    Protected Sub btnAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        panAddUser.Visible = True
        panCurrentUsers.Visible = False
        panEditPermissions.Visible = False
    End Sub
    Protected Sub btnAddUserCance_click(ByVal sender As Object, ByVal e As EventArgs)
        'User has clicked Cancel whilest in the process of adding a new user. Change view back to the Current Users list asnd clear add fields
        txtFirstname.Text = ""
        txtSurname.Text = ""
        txtUsername.Text = ""
        chkActive.Checked = False
        panAddUser.Visible = False
        panEditPermissions.Visible = False
        panCurrentUsers.Visible = True
    End Sub
    Protected Sub gvCurrenctUsers_editing()

    End Sub
    Protected Sub btnAddUserSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("add")
        If Page.IsValid Then
            'Check that username hasnt already been taken in the db or membership profile
            Dim bError As Boolean = False
            bError = testUsername1
            If Not bError Then bError = testUsername2
            If Not bError Then
                'Username not found. Its ok to Create new user
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procAffiliateUserInsert", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@affFirstname", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@affSurname", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@affUsername", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@affUserCountryCode", SqlDbType.VarChar, 5))
                    .Parameters.Add(New SqlParameter("@affActive", SqlDbType.Bit))
                    .Parameters.Add(New SqlParameter("@affPassword", SqlDbType.VarChar, 20))
                    .Parameters("@affFirstname").Value = txtFirstname.Text
                    .Parameters("@affSurname").Value = txtSurname.Text
                    .Parameters("@affUsername").Value = txtUsername.Text
                    .Parameters("@affUserCountryCode").Value = Session("EBAffEBDistributorCountryCode")
                    .Parameters("@affActive").Value = chkActive.Checked
                    .Parameters("@affPassword").Value = txtPassword.Text
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    lblAddUserError.Text = lblAddUserError.Text & "<br>An error occured while adding new details to database. Please contact tech support.<br>" & ex.ToString
                    bError = True
                Finally
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
                If Not bError Then
                    'Create new users Membership profile
                    Try
                        Membership.CreateUser(txtUsername.Text, txtPassword.Text)
                    Catch ex As Exception
                        lblAddUserError.Text = lblAddUserError.Text & "<br>An error occured while adding the new username and password to the membership provider. " & ex.ToString
                        bError = True
                    End Try
                End If
                'Refresh panels and current users gridview
                If Not bError Then
                    panCurrentUsers.Visible = True
                    panAddUser.Visible = False
                    panEditPermissions.Visible = False
                    gvCurrentUsers.DataBind()
                    txtFirstname.Text = ""
                    txtSurname.Text = ""
                    txtUsername.Text = ""
                    txtPassword.Text = ""
                    chkActive.Checked = False
                End If
            End If
        End If
    End Sub
    Protected Sub gvCurrenctUsers_rowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        Dim lbl As label = gvCurrentUsers.Rows(e.NewEditIndex).FindControl("lblPassword")
        Dim lnk As linkbutton = gvCurrentUsers.Rows(e.NewEditIndex).FindControl("lnkShowPassword")
        lbl.Visible = True
        lnk.visible = False
        gvCurrentUsers.EditIndex = -1
        e.cancel = True
    End Sub

    'Functions    
    Protected Function getUserAccess(ByVal pid As Object) As Boolean
        Dim result As Boolean = True
        If IsDBNull(pid) Then result = False
        Return result
    End Function
    Protected Function testUsername1() As Boolean
        'Returns True if username already exists in membership profiles
        Membership.Provider.ApplicationName = "ebAffProvider"
        Dim result As Boolean = CType(Membership.FindUsersByName(txtUsername.Text).Count, Boolean)
        If result Then lblAddUserError.Text = "The login name you supplied already exists. Please try another."
        Return result
    End Function
    Protected Function testUsername2() As Boolean
        'Returns True if username already exists in Affilaites table
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByUsernameSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affUsername", SqlDbType.VarChar, 20))
            .Parameters("@affUsername").Value = txtUsername.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            result = CType(ds.Tables(0).Rows.Count, Boolean)
        Catch ex As Exception
            lblAddUserError.Text = "An error occured whilest checking database for username. Please contact tech support<br>" & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If result Then lblAddUserError.Text = lblAddUserError.Text & "The login name you supplied already exists. Please try another."
        Return result
    End Function
    
    
End Class
