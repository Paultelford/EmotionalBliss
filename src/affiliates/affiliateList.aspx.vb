Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class affiliates_affiliateList
    Inherits BasePage
    Private Const _gvAffiliates_namePos As Integer = 1
    Private Const _gvAffiliates_passwordPos As Integer = 6
    Private Const _gvAffiliates_passwordSpacerPos As Integer = 7
    Private Const _gvAffiliates_chkboxPos As Integer = 12
    Private Const _dvAffiliate_productsSoldPos As Integer = 21
    Private Const _dvAffiliate_usernamePos As Integer = 25
    Private Const _dvAffiliate_passwordPos As Integer = 26

    'System
    Protected Sub Page_Load(ByVal Sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        If Session("EBAffEBDistributor") Then
            gvAffiliates.Columns(_gvAffiliates_passwordPos).Visible = True
            gvAffiliates.Columns(_gvAffiliates_passwordSpacerPos).Visible = True
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        gvAffiliates.DataBind()
    End Sub

    'Page
    Protected Sub dvAffiliateEdit_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox
        Dim aExceptions() As String = {"Phone"} 'Array of headertext. All will be shown regardless
        For Each row As DetailsViewRow In dvAffiliateEdit.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Try
                    txt = row.Cells(1).Controls(0)
                    'Set up exceptions
                    If Not headerIsException(aExceptions, row.Cells(0).Text.ToLower()) Then If txt.Text = "" Then row.Visible = False
                Catch ex As Exception
                    'Not to worry, must be an non-editable control
                End Try

            End If
        Next
        If dvAffiliateEdit.Rows.Count > 0 Then
            If drpType.SelectedValue = "1" Or drpType.SelectedValue = "4" Then
                'Affiliate or country rep was chose
                dvAffiliateEdit.Rows(_dvAffiliate_productsSoldPos).Visible = False
            Else
                dvAffiliateEdit.Rows(_dvAffiliate_productsSoldPos).Visible = True
            End If
        End If
    End Sub
    'User
    Protected Sub btnMsgSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim si As New siteInclude
        Dim userName As String = "Unknown"
        Try
            userName = Membership.GetUser.UserName
        Catch ex As Exception
        End Try
        siteInclude.addToAffiliateLog(dvAffiliateEdit.SelectedValue, txtNewMessage.Text, chkCustomerVisible.Checked, drpContact.SelectedValue, userName)
        chkCustomerVisible.Checked = False
        txtNewMessage.Text = ""
        gvTrace.DataBind()
        'Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('document.getElementById(""tdOrderLog"").style.display="""";document.getElementById(""spanOrderLog"").innerHTML=""Hide Order Log"";',200);", True)
    End Sub
    Protected Sub drpStatus_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Try and add filters depending on what was selected in the 'Status:' dropdown
        Select Case LCase(drpStatus.SelectedItem.Text)
            Case "active"
                sqlAffiliates.FilterExpression = "affActive=true"
            Case "inactive"
                sqlAffiliates.FilterExpression = "affActive=false"
            Case "rejected"
                sqlAffiliates.FilterExpression = "affRejected=true"
        End Select
    End Sub
    Protected Sub btnChangePassword_click(ByVal Sender As Object, ByVal e As EventArgs)
        'Set username and old password
        Dim btn As LinkButton = CType(Sender, LinkButton)
        Dim user As MembershipUser = Membership.GetUser(btn.CommandArgument)
        hidUsername.Value = btn.CommandArgument
        hidPassword.Value = user.GetPassword
        panPassword.Visible = True
    End Sub
    Protected Sub btnSubmitPassword_click(ByVal Sender As Object, ByVal e As EventArgs)
        'Text password confirmation matches password
        Page.Validate("pass")
        If Page.IsValid Then
            If txtPassword.Text <> txtPasswordConfirm.Text Then
                lblErrorPassword.Text = "<font color='red'>Passwords do not match.</font>"
            Else
                'All ok, change password
                lblErrorPassword.Text = ""
                Dim user As MembershipUser = Membership.GetUser(hidUsername.Value)
                user.ChangePassword(hidPassword.Value, txtPassword.Text)
                'All ok, update in database
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procAffiliateByUsernamePasswordUpdate", oConn)
                Dim bError As Boolean = False
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@password", SqlDbType.VarChar, 20))
                    .Parameters("@username").Value = hidUsername.Value
                    .Parameters("@password").Value = txtPassword.Text
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    bError = True
                    lblErrorPassword.Text = "<font color='red'>An error occured while updating password. Please contact support.</font>"
                    Dim si As New siteInclude
                    si.addError("affiliates/affiliateList.aspx", "btnSubmitPassword_click(username=" & hidUsername.Value & ", password=" & txtPassword.Text & "); " & ex.ToString)
                    si = Nothing
                Finally
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
                'Databind to show new password in gridview
                panPassword.Visible = bError
                gvAffiliates.DataBind()
                If Not bError Then
                    txtPassword.Text = ""
                    txtPasswordConfirm.Text = ""
                End If
            End If
        End If
    End Sub
    Protected Sub chkActive_checkedChange(ByVal Sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox = CType(Sender, CheckBox)
        chk.Checked = False
    End Sub
    Protected Sub gvAffiliates_rowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        sqlAffiliateEdit.SelectParameters(0).DefaultValue = gvAffiliates.DataKeys(e.NewEditIndex).Value
        dvAffiliateEdit.DataBind()
        dvAffiliateEdit.Visible = True
        panLog.Visible = True
        'Disable the Password checkbox if the affiliate has never been activated (as membership object will not exist for affiliate till its activated)
        Dim chkFirstTime As CheckBox = gvAffiliates.Rows(e.NewEditIndex).FindControl("chkFirstTime")
        Dim txtPW As TextBox = dvAffiliateEdit.Rows(siteInclude.getDVRowByHeader(dvAffiliateEdit, "Password")).Cells(1).Controls(0)
        If chkFirstTime.Checked Then txtPW.Enabled = False
        e.Cancel = True
    End Sub
    Protected Sub gvAffiliates_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox = gvAffiliates.Rows(gvAffiliates.SelectedIndex).Cells(siteInclude.getGVRowByHeader(gvAffiliates, "Active")).Controls(0)
        Dim chkFirstTime As CheckBox = gvAffiliates.Rows(gvAffiliates.SelectedIndex).FindControl("chkFirstTime")
        Dim affID As Integer = gvAffiliates.DataKeys(gvAffiliates.SelectedIndex).Value
        Dim lblUsername As Label = gvAffiliates.SelectedRow.FindControl("lblUsername")
        Dim mu As MembershipUser = Membership.GetUser(lblUsername.Text)
        If Not chk.Checked Then
            'Distributor is trying to activate an affilaite.
            'Find out if its a first time activation
            If chkFirstTime.Checked Then
                'First time activation, show confirmation messages
                pan1.Visible = True
                lblSelectedAffiliate.Text = gvAffiliates.Rows(gvAffiliates.SelectedIndex).Cells(_gvAffiliates_namePos).Text
                dvAffiliateEdit.Visible = False
            Else
                'No confirmation needed, re-activate affiliate
                changeAffiliateStatus(affID, True)
                mu.UnlockUser()
                mu.IsApproved = True
                Membership.UpdateUser(mu)
                gvAffiliates.SelectedIndex = -1
            End If
        Else
            'An affilaite has been made inactive.
            changeAffiliateStatus(affID, False)
            mu.IsApproved = False
            Membership.UpdateUser(mu)
            gvAffiliates.SelectedIndex = -1
        End If
        mu = Nothing
    End Sub
    Protected Sub btnAccept_click(ByVal Sender As Object, ByVal e As EventArgs)
        Dim bError As Boolean = False
        Try
            changeAffiliateStatus(gvAffiliates.DataKeys(gvAffiliates.SelectedIndex).Value, True)
            'Do the profile stuff
            Dim lblUser As Label = gvAffiliates.SelectedRow.FindControl("lblUsername")
            Dim lblPass As Label = gvAffiliates.SelectedRow.FindControl("lblPassword")
            Membership.CreateUser(lblUser.Text, lblPass.Text)
        Catch ex As Exception
            lblComplete.Text = "An error occured. Affiliate was not activated."
            bError = True
            siteInclude.addError("affiliates/affiliateList.aspx.vb", "btnAccept_click(affID=" & gvAffiliates.DataKeys(gvAffiliates.SelectedIndex).Value & "); " & ex.ToString)
        End Try

        If Not bError Then
            lblComplete.Text = "User creted and emailed."
            siteInclude.addToAffiliateLog(gvAffiliates.DataKeys(gvAffiliates.SelectedIndex).Value, "Affiliate activated for the 1st time. Email sent.", True, "Emailed", Membership.GetUser.UserName)
            'Add code here to email affiliate with their acceptence info
            sendConfirmationEmail()
        End If
        'Unselected the selected row (reset gridview)
        gvAffiliates.SelectedIndex = -1
    End Sub
    Protected Sub dvAffiliateEdit_itemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
        'Update user profile with new password
        Dim u As String = dvAffiliateEdit.Rows(_dvAffiliate_usernamePos).Cells(1).Text
        Dim p As TextBox = dvAffiliateEdit.Rows(siteInclude.getDVRowByHeader(dvAffiliateEdit, "Password")).Cells(1).Controls(0)
        Try
            Dim mu As MembershipUser = Membership.GetUser(u)
            mu.ChangePassword(mu.GetPassword, p.Text)
        Catch ex As Exception
            'If error occurs its because the affliate has not been activated for the 1st tmie, so they do not exist at all in membership profile.
            'No need to show error as password cannot be edited till affiliate is activated
            'lblError.Text = ex.ToString
        End Try
    End Sub
    Protected Sub dvAffiliateEdit_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        'Hide gridview and rebind main gridview to reflect changes amde
        dvAffiliateEdit.Visible = False
        gvAffiliates.DataBind()
    End Sub
    Protected Sub btnCancel_click(ByVal Sender As Object, ByVal e As EventArgs)
        pan1.Visible = False
    End Sub

    Protected Sub gv_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvAffiliates.PageIndexChanging
        Dim gv As GridView = CType(sender, GridView)
        gv.PageIndex = e.NewPageIndex
        gv.DataBind()
    End Sub
    Protected Sub gv_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvAffiliates.DataBound
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerData(gv)
    End Sub
    Protected Sub gv_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvAffiliates.DataBinding
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerControl(gv)
    End Sub

    'Subs
    Protected Sub changeAffiliateStatus(ByVal affID As Integer, ByVal state As Boolean)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateActiveByIDUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@active", SqlDbType.Bit))
            .Parameters("@affID").Value = affID
            .Parameters("@active").Value = state
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = lblError.Text & ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        pan1.Visible = False
        gvAffiliates.DataBind()
    End Sub
    Protected Sub sendConfirmationEmail()
        Dim toAdd As String = ""
        Dim ccAdd As String = ""
        Dim subject As String = "Emotional Bliss Affiliate Approval"
        Dim msg As MailMessage
        Dim chk As CheckBox
        Dim lblEmail As Label = gvAffiliates.SelectedRow.FindControl("lblEmail")
        Dim plainView As AlternateView
        Dim htmlView As AlternateView
        Dim emailBody As String = ""
        emailBody = "Thank you for joining Emotional Bliss!" & Chr(13) & Chr(13)
        emailBody = emailBody & "To access your account on <http://www.emotionalbliss.com>www.emotionalbliss.com</a> select your country first and then please click on B2B --> Account Login and use your username and password. " & Chr(13) & Chr(13)
        emailBody = emailBody & "If you would like any further information or need any help, please do not hesitate to contact us on <a href='mailto:orders@emotionalbliss.com'>orders@emotionalbliss.com</a> or by telephone on +44 8700 410 022" & Chr(13) & Chr(13)
        emailBody = emailBody & "Thank you and kind regards" & Chr(13) & "The Emotional Bliss Team"
        msg = New MailMessage
        msg.To.Add(lblEmail.Text)
        If ccAdd <> "" Then msg.CC.Add(ccAdd)
        msg.From = New MailAddress("noreply@emotionalbliss.com")
        msg.Subject = subject
        msg.IsBodyHtml = True
        plainView = AlternateView.CreateAlternateViewFromString(emailBody, Nothing, "text/plain")
        htmlView = AlternateView.CreateAlternateViewFromString(Replace(emailBody, Chr(13), "<br>"), Nothing, "text/html")
        msg.AlternateViews.Add(plainView)
        msg.AlternateViews.Add(htmlView)
        Try
            Dim client As New SmtpClient
            client.Send(msg)
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            msg.Dispose()
        End Try
    End Sub
    Protected Function showSpecial(ByVal con As Boolean, ByVal roy As Boolean) As String
        Dim result As String = ""
        If con Then result = result & "Consultancy, "
        If roy Then result = result & "Royalty, "
        If result <> "" Then result = Left(result, Len(result) - 2)
        Return result
    End Function

    'Functions
    Protected Function showAffName(ByVal fn As Object, ByVal sn As Object)
        Dim result As String = ""
        If Not IsDBNull(sn) Then result = sn.ToString & ", "
        If Not IsDBNull(fn) Then result = result & fn.ToString
        Return result
    End Function
    Protected Function headerIsException(ByVal arr As String(), ByVal header As String) As Boolean
        Dim result As String = False
        For iLoop As Integer = 0 To UBound(arr)
            If LCase(arr(iLoop)) = LCase(header) Then result = True
        Next
        Return result
    End Function
End Class
