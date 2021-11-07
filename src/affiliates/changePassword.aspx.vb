Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class affiliates_changePassword
    Inherits BasePage

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        Dim drp As DropDownList = Master.FindControl("drpAffmenu")
        drp.SelectedValue = "password"
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate()
        lblComplete.Text = ""
        If Page.IsValid Then
            If Trim(LCase(txtNewPassword.Text)) = Trim(LCase(txtConfirm.Text)) Then
                'make sure new password has length of 5 or more characters
                If Len(txtNewPassword.Text) < Membership.MinRequiredPasswordLength Then
                    lblComplete.Text = "<font color='red'>New password must be at least 5 characters</font>"
                Else
                    'New passwords match, make change
                    Dim changeOK As Boolean = Membership.GetUser.ChangePassword(txtCurrentPassword.Text, txtNewPassword.Text)
                    If changeOK Then
                        'save change in database
                        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                        Dim oCmd As New SqlCommand("procAffilaitesByIDPasswordupdate", oConn)
                        With oCmd
                            .CommandType = CommandType.StoredProcedure
                            .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 20))
                            .Parameters.Add(New SqlParameter("@password", SqlDbType.VarChar, 20))
                            .Parameters("@username").Value = Membership.GetUser.UserName
                            .Parameters("@password").Value = txtNewPassword.Text
                        End With
                        Try
                            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                            oCmd.ExecuteNonQuery()
                            lblComplete.Text = "You password has been changed."
                        Catch ex As Exception
                            lblComplete.Text = "<font color='red'>An error occured while changing you password. Please contact <a href='mailto:support@emotionalbliss.com'>support@emotionalbliss.com</a>.<br>Sorry for any inconvenience"
                            Dim si As New siteInclude
                            si.addError("affiliates/changePassword", "btnSubmit_click(); " & ex.ToString)
                        Finally
                            oCmd.Dispose()
                            oConn.Dispose()
                        End Try
                        tblChangePassword.Visible = False
                    Else
                        lblComplete.Text = "<font color='red'>Current password is not correct.</font>"
                    End If 'changeOK
                End If
            Else
                lblComplete.Text = "<font color='red'>New password and Confirm password fields do not match.</font>"
            End If 'Trim(pw)=Trim(confirmPW)
        End If
    End Sub
End Class
