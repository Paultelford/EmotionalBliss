Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Text.RegularExpressions


Partial Class maintenance_emailSend
    Inherits System.Web.UI.Page
    Private Const _bIncludeLogoImage As Boolean = True
    Private Const openingTag As String = "<span style='font-size: 12pt; font-family: &quot;Times New Roman&quot;,&quot;serif&quot;; color: rgb(40, 40, 40);'>"
    Private Const closingTag As String = "</span>"

    'System events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblCompany.Text = ConfigurationManager.AppSettings("companyName")
        lnkUnsubscribe.NavigateUrl = "http://" & Request.ServerVariables("SERVER_NAME")
        If Request.ServerVariables("SERVER_PORT") <> 80 Then lnkUnsubscribe.NavigateUrl = lnkUnsubscribe.NavigateUrl & ":" & Request.ServerVariables("SERVER_PORT")
        lnkUnsubscribe.NavigateUrl = lnkUnsubscribe.NavigateUrl & "/unsubscribe.aspx"
        lblAddresses.Text = ""
    End Sub

    'Page events
    Protected Sub drpGroups_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        drpGroups.Items.Clear()
        drpGroups.Items.Add(New ListItem("Choose Group...", ""))
        'drpGroups.Items.Add(New ListItem("Users", "0"))
        drpGroups.Items.Add(New ListItem("Newsletter", "1"))
    End Sub
    Protected Sub btnSend_click(ByVal sender As Object, ByVal e As EventArgs)
        'Validate dropdown/email textbox
        If Not (txtEmail.Text = "" And drpGroups.SelectedIndex = 0) Then
            'All ok, contiune
            If FCKeditor1.Value = "" Then
                lblError.Text = "<font color='red'>Please eneter some body text</font>"
            Else
                Dim bUseGroupEmail As Boolean = True
                Dim bValidEmail = True
                Dim regExpEmail As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                'test for valid email addresses
                If txtEmail.Text <> "" Then
                    bUseGroupEmail = False
                    Dim arr As Array = Split(txtEmail.Text, ",")
                    'Test each address with a regular expression(regExpEmail)
                    For Each str As String In arr
                        If Not Regex.IsMatch(Trim(str), regExpEmail) Then bValidEmail = False
                    Next
                    If Not bValidEmail Then
                        lblError.Text = "<font color='red'>An invalid email address has been entered. Mail not sent.</font>"
                    Else
                        'All addresses are ok, send email
                        Dim msg As MailMessage
                        Dim plainView As AlternateView
                        Dim htmlView As AlternateView
                        Dim logo As LinkedResource
                        Dim client As SmtpClient
                        For Each str As String In arr
                            siteInclude.sendSQLEmail(Trim(str), "", "", txtSubject.Text, "noreply@" & ConfigurationManager.AppSettings("companyDomain"), ConfigurationManager.AppSettings("companyName"), "<center><a href='" & Request.ServerVariables("HTTP_HOST") & "'><img src='http://" & Request.ServerVariables("HTTP_HOST") & "/media/logoforemail.jpg' border='0'></a><br><br>" & FCKeditor1.Value & "<br><br><font color='#777777' size='-2'>Copyright ©" & Now.Year & " " & ConfigurationManager.AppSettings("companyName") & ". All rights reserved.</font><br><font color='#777777'>If you like our newsletter and feel it is of value to you, perhaps it is of value to a colleague, - please pass it on.<br />However if you no longer wish to receive this newsletter please <a href='http://" & Request.ServerVariables("HTTP_HOST") & "/unsubscribe.aspx?email=" & Trim(str) & "'>unsubscribe</a> and you will be removed from our mailing list.</font></center>", siteInclude._emailType.emailHtml)
                            If False Then
                                msg = New MailMessage
                                msg.To.Add(Trim(str))
                                msg.From = New MailAddress("noreply@" & ConfigurationManager.AppSettings("companyDomain"))
                                msg.Subject = txtSubject.Text
                                msg.IsBodyHtml = True
                                plainView = AlternateView.CreateAlternateViewFromString(Replace(FCKeditor1.Value, "@EMAIL", str) & "<br><br>Copyright ©" & Now.Year & " " & lblCompany.Text & ". All rights reserved.", Nothing, "text/plain")
                                If _bIncludeLogoImage Then
                                    htmlView = AlternateView.CreateAlternateViewFromString("<center><a href='" & Request.ServerVariables("HTTP_HOST") & "'><img src=cid:companylogo border='0'></a><br><br>" & openingTag & Replace(FCKeditor1.Value, "@EMAIL", str) & closingTag & "<br><br><font color='#777777' size='-2'>Copyright ©" & Now.Year & " " & lblCompany.Text & ". All rights reserved.</font><br><font color='#777777'>If you like our newsletter and feel it is of value to you, perhaps it is of value to a colleague, - please pass it on.<br />However if you no longer wish to receive this newsletter please <a href='http://" & Request.ServerVariables("HTTP_HOST") & "/unsubscribe.aspx?email=" & Trim(str) & "'>unsubscribe</a> and you will be removed from our mailing list.</font></center>", Nothing, "text/html")
                                    logo = New LinkedResource(Request.ServerVariables("APPL_PHYSICAL_PATH") & "media\logoforemail.jpg")
                                    logo.ContentId = "companylogo"
                                    htmlView.LinkedResources.Add(logo)
                                Else
                                    htmlView = AlternateView.CreateAlternateViewFromString("<center><a href='" & Request.ServerVariables("HTTP_HOST") & "'></a><br><br>" & openingTag & Replace(FCKeditor1.Value, "@EMAIL", str) & closingTag & "<br><br><font color='#777777' size='-2'>Copyright ©" & Now.Year & " " & lblCompany.Text & ". All rights reserved.</font><br><font color='#777777'>If you like our newsletter and feel it is of value to you, perhaps it is of value to a colleague, - please pass it on.<br />However if you no longer wish to receive this newsletter please <a href='http://" & Request.ServerVariables("HTTP_HOST") & "/unsubscribe.aspx?email=" & Trim(str) & "'>unsubscribe</a> and you will be removed from our mailing list.</font></center>", Nothing, "text/html")
                                End If
                                msg.AlternateViews.Add(plainView)
                                msg.AlternateViews.Add(htmlView)
                                client = New SmtpClient
                                Try
                                    client.Send(msg)
                                    lblAddresses.Text = lblAddresses.Text & "Email sent to '<i>" & msg.To.ToString & "</i>' successfully.<br>"
                                Catch ex As Exception
                                Finally
                                    msg.Dispose()
                                End Try
                            End If
                        Next
                        lblError.Text = ""
                        lblSent.Text = "<font color='red'>The email has been sent to the specified addresses</font>"
                        'btnSend.Visible = False
                    End If
                Else

                    'Send email
                    Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                    Dim oCmd As SqlCommand
                    If CInt(drpGroups.SelectedValue) > 1 Then
                        'User created Group selected
                        oCmd = New SqlCommand("procEmailByGroupIDSelect", oConn)
                        oCmd.Parameters.Add(New SqlParameter("@groupID", SqlDbType.Int))
                        oCmd.Parameters("@groupID").Value = Convert.ToInt32(drpGroups.SelectedValue)
                    Else
                        'Newsletters was selected
                        oCmd = New SqlCommand("procNewsletterByActiveEmailSelect", oConn)
                        oCmd.Parameters.Add(New SqlParameter("@active", SqlDbType.Int))
                        oCmd.Parameters("@active").Value = drpGroups.SelectedValue
                    End If
                    Dim da As New SqlDataAdapter
                    Dim ds As New DataSet
                    Dim msg As MailMessage
                    Dim plainView As AlternateView
                    Dim htmlView As AlternateView
                    Dim logo As LinkedResource
                    Dim client As SmtpClient
                    oCmd.CommandType = CommandType.StoredProcedure
                    Try
                        Dim d As siteInclude
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        da = New SqlDataAdapter(oCmd)
                        da.Fill(ds)
                        For Each row As DataRow In ds.Tables(0).Rows
                            If Not IsDBNull(row("email")) Then
                                siteInclude.sendSQLEmail(Trim(row("email")), "", "", txtSubject.Text, "noreply@" & ConfigurationManager.AppSettings("companyDomain"), ConfigurationManager.AppSettings("companyName"), "<center><a href='" & Request.ServerVariables("HTTP_HOST") & "'><img src='http://" & Request.ServerVariables("HTTP_HOST") & "/media/logoforemail.jpg' border='0'></a><br><br>" & FCKeditor1.Value & "<br><br><font color='#777777' size='-2'>Copyright ©" & Now.Year & " " & ConfigurationManager.AppSettings("companyName") & ". All rights reserved.</font><br><font color='#777777'>If you like our newsletter and feel it is of value to you, perhaps it is of value to a colleague, - please pass it on.<br />However if you no longer wish to receive this newsletter please <a href='http://" & Request.ServerVariables("HTTP_HOST") & "/unsubscribe.aspx?email=" & Trim(row("email")) & "'>unsubscribe</a> and you will be removed from our mailing list.</font></center>", siteInclude._emailType.emailHtml)
                                If False Then
                                    msg = New MailMessage
                                    msg.To.Add(row("email"))
                                    msg.From = New MailAddress("noreply@" & ConfigurationManager.AppSettings("companyDomain"))
                                    msg.Subject = txtSubject.Text
                                    msg.IsBodyHtml = True
                                    plainView = AlternateView.CreateAlternateViewFromString(Replace(FCKeditor1.Value, "@EMAIL", row("email")) & "<br><br>Copyright ©" & Now.Year & lblCompany.Text & ". All rights reserved.", Nothing, "text/plain")
                                    If _bIncludeLogoImage Then
                                        htmlView = AlternateView.CreateAlternateViewFromString("<center><a href='" & Request.ServerVariables("HTTP_HOST") & "'><img src=cid:companylogo border='0'></a><br><br>" & openingTag & FCKeditor1.Value & closingTag & "<br><br><font color='#777777' size='-2'>Copyright ©" & Now.Year & " " & lblCompany.Text & ". All rights reserved.</font><br><font color='#777777'>If you like our newsletter and feel it is of value to you, perhaps it is of value to a colleague, - please pass it on.<br />However if you no longer wish to receive this newsletter please <a href='http://" & Request.ServerVariables("HTTP_HOST") & "/unsubscribe.aspx?email=" & row("email") & "'>unsubscribe</a> and you will be removed from our mailing list.</font></center>", Nothing, "text/html")
                                        logo = New LinkedResource(Request.ServerVariables("APPL_PHYSICAL_PATH") & "media\logoforemail.jpg")
                                        logo.ContentId = "companylogo"
                                        htmlView.LinkedResources.Add(logo)
                                    Else
                                        htmlView = AlternateView.CreateAlternateViewFromString("<center><a href='" & Request.ServerVariables("HTTP_HOST") & "'></a><br><br>" & openingTag & Replace(FCKeditor1.Value, "@EMAIL", row("email")) & closingTag & "<br><br><font color='#777777' size='-2'>Copyright ©" & Now.Year & " " & lblCompany.Text & ". All rights reserved.</font><br><font color='#777777'>If you like our newsletter and feel it is of value to you, perhaps it is of value to a colleague, - please pass it on.<br />However if you no longer wish to receive this newsletter please <a href='http://" & Request.ServerVariables("HTTP_HOST") & "/unsubscribe.aspx?email=" & row("email") & "'>unsubscribe</a> and you will be removed from our mailing list.</font></center>", Nothing, "text/html")
                                    End If
                                    msg.AlternateViews.Add(plainView)
                                    msg.AlternateViews.Add(htmlView)
                                    client = New SmtpClient
                                    Try
                                        client.Send(msg)
                                        lblAddresses.Text = lblAddresses.Text & "Email sent to " & msg.To.ToString & " successfully.<br>"
                                    Catch ex As Exception
                                    Finally
                                        msg.Dispose()
                                    End Try
                                End If
                            End If
                        Next
                    Catch ex As Exception
                        siteInclude.addError("emailSend.aspx.vb", "btnSend_click(); " & ex.ToString)
                        Response.Write(ex)
                        Response.End()
                    Finally
                        ds.Dispose()
                        da.Dispose()
                        oCmd.Dispose()
                        oConn.Dispose()
                    End Try

                    lblError.Text = ""
                    lblSent.Text = "<font color='red'>The email has been sent to the selected group</font>"
                    'btnSend.Visible = False
                End If
            End If
        Else
            'Email textbox is empty and dropdown hasnt been set
            lblError.Text = "Please choose a group from the dropdown, or enter an email address into the textbox."
        End If
    End Sub
End Class
