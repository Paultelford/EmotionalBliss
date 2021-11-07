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
    Private aData(4) As String
    Private bDebug As Boolean = False

    'System events
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        gvEmails.Visible = bDebug
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblCompany.Text = ConfigurationManager.AppSettings("companyName")
        lnkUnsubscribe.NavigateUrl = "http://" & Request.ServerVariables("SERVER_NAME")
        If Request.ServerVariables("SERVER_PORT") <> 80 Then lnkUnsubscribe.NavigateUrl = lnkUnsubscribe.NavigateUrl & ":" & Request.ServerVariables("SERVER_PORT")
        lnkUnsubscribe.NavigateUrl = lnkUnsubscribe.NavigateUrl & "/unsubscribe.aspx"
        lblAddresses.Text = ""
        'lblServer.Text = "isDev=" & Application("isDev") & ", idDevBox=" & Application("isDevBox")
    End Sub

    'Page events
    Protected Sub drpGroups_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        drpGroups.Items.Clear()
        drpGroups.Items.Add(New ListItem("Choose Group...", ""))
        'drpGroups.Items.Add(New ListItem("Users", "0"))
        drpGroups.Items.Add(New ListItem("Newsletter", "1"))
    End Sub

    'User
    Protected Sub drpSource_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblEmailCount.Text = "Clicking 'Send Email' will sent to " & getEmailCountDT(drpSource.SelectedValue) & " email addresses."
        If bDebug Then
            Dim dt As DataTable = getEmailDT(drpSource.SelectedValue)
            gvEmails.DataSource = dt
            gvEmails.DataBind()
        End If
        scrollDown()
    End Sub
    Protected Sub btnSend_click(ByVal sender As Object, ByVal e As EventArgs)
        'Validate dropdown/email textbox
        If Not (drpSource.SelectedIndex = 0) Then
            'All ok, contiune
            If FCKeditor1.Value = "" Then
                lblError.Text = "<font color='red'>Please eneter some body text</font>"
            Else
                Dim bUseGroupEmail As Boolean = True
                Dim bValidEmail = True
                Dim regExpEmail As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                'test for valid email addresses
                'Dim imgHtml As String = "<center><a href='http://www.emotionalbliss.com'><img border='0' src='http://www.emotionalbliss.co.uk/design/images/logo-emotional-bliss2.gif'></a><br><br>"

                Dim imgHtml As String = "<center>"

                Dim dt As DataTable = getEmailDT(drpSource.SelectedValue)
               
                Try

                    For Each row As DataRow In dt.Rows
                        If Not IsDBNull(row("email")) Then
                            'siteInclude.debug(row("email"))
                            Try
                                siteInclude.sendSQLEmail(Trim(row("email")), "", "", txtSubject.Text, "noreply@emotionalbliss.co.uk", "Emotional Bliss", imgHtml & "<br><br>" & Replace(FCKeditor1.Value, "@EMAIL", row("email")) & "<br><br><font color='#777777' size='-2'>Copyright ©" & Now.Year & " Emotional Bliss. All rights reserved.</font><br><font color='#777777'>If you like our newsletter and feel it is of value to you, perhaps it is of value to a colleague, - please pass it on.<br />However if you no longer wish to receive this newsletter please <a href='http://" & Request.ServerVariables("HTTP_HOST") & "/unsubscribe.aspx?email=" & Trim(row("email")) & "'>unsubscribe</a> and you will be removed from our mailing list.</font></center>", siteInclude._emailType.emailHtml)
                            Catch ex As Exception
                            End Try
                        End If
                    Next
                Catch ex As Exception
                    siteInclude.addError("emailSend.aspx.vb", "btnSend_click(); " & ex.ToString)
                    Response.Write(ex)
                    Response.End()
                Finally
                    dt.Dispose()
                End Try

                lblError.Text = ""
                lblSent.Text = "<font color='red'>The email has been sent to the selected group</font>"
                'btnSend.Visible = False
                scrollDown()
            End If
        Else
            'Email textbox is empty and dropdown hasnt been set
            lblError.Text = "Please choose a group from the dropdown, or enter an email address into the textbox."
        End If
    End Sub

    'Subs
    Protected Sub scrollDown()
        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType, "scdown", "document.location='emailSend.aspx#a';", True)
    End Sub

    'Functions
    Protected Function getEmailDT(ByVal iSource As Integer) As DataTable
        Dim sql As String = ""
        Select Case iSource
            Case 1
                'Newsletter
                sql = "select distinct email from newsletter where active=1 and email not in (select email from emailUnsubscribe)"
            Case 2
                'New Orders
                sql = "select distinct c.email from shopOrder so inner join shopCustomer c on so.customerID=c.customerID where (so.orderSource = 'shopper' or so.orderSource='callcenter') and so.orderStatus='complete' and c.email not in (select email from emailUnsubscribe) and (so.orderCountryCode<>'nl' and so.orderCountryCode<>'be')"
            Case 3
                'Old orders
                sql = "select distinct email from oldemails1 where email not in (select email from emailUnsubscribe)"
            Case 4
                'New orders NL/BE
                sql = "select distinct c.email from shopOrder so inner join shopCustomer c on so.customerID=c.customerID where (so.orderSource = 'shopper' or so.orderSource='callcenter') and so.orderStatus='complete' and c.email not in (select email from emailUnsubscribe) and (so.orderCountryCode='nl' or so.orderCountryCode='be')"
            Case 5
                'Old orders NL/BE
                sql = "select distinct oldebemail as email from oldebemailsNLBE where oldebemail not in (select email from emailUnsubscribe)"
            Case 6
                'Test
                sql = "select distinct email from emailTest where email not in (select email from emailUnsubscribe)"
            Case 7
                'PT MD
                sql = "select 'tickets_ptmd@emotionalbliss.com' as email"
            Case 8
                'Tell A Friend (Sender)
                sql = "select distinct address as email from email e inner join emailGroups g on e.[group]=g.groupID where address not in (select email from emailUnsubscribe) and g.groupID=4"
            Case 9
                'Tell A Friend (Recippient)
                sql = "select distinct address as email from email e inner join emailGroups g on e.[group]=g.groupID where address not in (select email from emailUnsubscribe) and g.groupID=5"
        End Select
        Return siteInclude.getSQLStatement(sql)
    End Function
    Protected Function getEmailCountDT(ByVal iSource As Integer) As Integer
        Dim sql As String = ""
        Select Case iSource
            Case 1
                'Newsletter
                sql = "select count(distinct email) as items from newsletter where active=1 and email not in (select email from emailUnsubscribe)"
            Case 2
                'New Orders
                sql = "select count(distinct c.email) as items from shopOrder so inner join shopCustomer c on so.customerID=c.customerID where (so.orderSource = 'shopper' or so.orderSource='callcenter') and so.orderStatus='complete' and c.email not in (select email from emailUnsubscribe) and (so.orderCountryCode<>'nl' and so.orderCountryCode<>'be')"
            Case 3
                'Old orders
                sql = "select count(distinct email) as items from oldemails1 where email not in (select email from emailUnsubscribe)"
            Case 4
                'New orders NL/BE
                sql = "select count(distinct c.email) as items from shopOrder so inner join shopCustomer c on so.customerID=c.customerID where (so.orderSource = 'shopper' or so.orderSource='callcenter') and so.orderStatus='complete' and c.email not in (select email from emailUnsubscribe) and (so.orderCountryCode='nl' or so.orderCountryCode='be')"
            Case 5
                'Old orders NL/BE
                sql = "select count(distinct oldebemail) as items from oldebemailsNLBE where oldebemail not in (select email from emailUnsubscribe)"
            Case 6
                'Test
                sql = "select count(distinct email) as items from emailTest where email not in (select email from emailUnsubscribe)"
            Case 7
                'PT MD
                sql = "select 1 as items"
            Case 8
                'Tell A Friend (Sender)
                sql = "select count(distinct address) as items from email e inner join emailGroups g on e.[group]=g.groupID where address not in (select email from emailUnsubscribe) and g.groupID=4"
            Case 9
                'Tell A Friend (Recippient)
                sql = "select count(distinct address) as items from email e inner join emailGroups g on e.[group]=g.groupID where address not in (select email from emailUnsubscribe) and g.groupID=5"
        End Select
        Dim dt As DataTable = siteInclude.getSQLStatement(sql)
        Return dt.Rows(0)("items")
    End Function
End Class
