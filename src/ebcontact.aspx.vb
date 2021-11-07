Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class ebcontact
    Inherits BasePage
    Protected Const _dvContact_firstNamePos As Integer = 0
    Protected Const _dvContact_LastNamePos As Integer = 1
    Protected Const _dvContact_phonePos As Integer = 3
    Protected Const _dvContact_subjectPos As Integer = 5
    Protected Const _dvContact_orderNoPos As Integer = 6

    'System
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim drpDepartment As DropDownList = dvContact.FindControl("drpDepartment")
        If Not Page.IsPostBack Then
            If Request.QueryString("dept") <> "" Then
                Try
                    drpDepartment.SelectedValue = Request.QueryString("dept")
                Catch ex As Exception

                End Try
            End If
            
        End If
        If Not Page.IsPostBack Then
            loadDBResources()
            'siteInclude.debug("EBLanguage=" & Session("EBLanguage"))
            'siteInclude.debug("EBShopCountry=" & Session("EBShopCountry"))
            If LCase(Session("EBLanguage")) <> "gb" Or LCase(Session("EBShopCountry")) <> "gb" Then
                'lblCountryName.Text = "United Kingdom<br>"
                'lblEBPhone.Text = "+44 8700 410022"
            End If
        End If
        If LCase(Session("EBLanguage")) = "nl" Or LCase(Session("EBLanguage")) = "be" Then
            'panAddress.Visible = False
            panNLAddress.Visible = True
        End If
    End Sub

    'Page
    Protected Sub dvContact_inserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
        'Send email
        Dim EBMailAddress As String
        Dim msg As MailMessage
        Dim bError As Boolean = False
        Dim drpDepartment As DropDownList = dvContact.FindControl("drpDepartment")
        Dim txtEmail As TextBox = dvContact.FindControl("txtEmail")
        Dim txtMessage As TextBox = dvContact.FindControl("txtMessage")
        Dim email As String = ""
        Dim subject As String = getText(_dvContact_subjectPos)
        'Select Case CInt(drpDepartment.SelectedValue)
        '    Case 1
        '        EBMailAddress = "tickets_cs@emotionalbliss.com"
        '    Case 2
        '        EBMailAddress = "tickets_ae@emotionalbliss.com"
        '    Case 3
        '        EBMailAddress = "tickets_m@emotionalbliss.com"
        '    Case 4
        '        EBMailAddress = "tickets_a@emotionalbliss.com"
        '    Case 5
        '        EBMailAddress = "tickets_r@emotionalbliss.com"
        '    Case 6
        '        EBMailAddress = "tickets_w@emotionalbliss.com"
        '    Case 7
        '        EBMailAddress = "tickets_pi@emotionalbliss.com"
        '    Case 8
        '        EBMailAddress = "tickets_we@emotionalbliss.com"
        '    Case 9
        '        EBMailAddress = "tickets_de@emotionalbliss.com"
        '    Case 10
        '        EBMailAddress = "tickets_cre@emotionalbliss.com"
        '    Case 11
        '        EBMailAddress = "tickets_b2b@emotionalbliss.com"
        '    Case 12
        '        EBMailAddress = "tickets_ptmd@emotionalbliss.com"
        '    Case 13
        '        EBMailAddress = "tickets_jasmine@emotionalbliss.com"
        'End Select
        'If LCase(Session("EBLanguage")) = "nl" Then
        EBMailAddress = "enquiries@emotionalbliss.com"
        subject = drpDepartment.SelectedItem.Text
            'End If
            Try
            'EBMailAddress = "scott@emotionalbliss.com"
            email = "Name: " & getText(_dvContact_firstNamePos) & " " & getText(_dvContact_LastNamePos) & "<br>"
            email = email & "Email: " & txtEmail.Text & "<br>"
            email = email & "Phone: " & getText(_dvContact_phonePos) & "<br>"
            email = email & "Subject: " & getText(_dvContact_subjectPos) & "<br>"
            email = email & "Order No: " & getText(_dvContact_orderNoPos) & "<br>"
            email = email & "Message: " & txtMessage.Text & "<br>"
            email = Replace(email, Chr(13), "<BR>")

            siteInclude.sendContactEmail(EBMailAddress, "", "", subject, txtEmail.Text, getText(_dvContact_firstNamePos) & " " & getText(_dvContact_LastNamePos), email)

            'siteInclude.sendSQLEmail(EBMailAddress, "", "", subject, txtEmail.Text, getText(_dvContact_firstNamePos) & " " & getText(_dvContact_LastNamePos), email, siteInclude._emailType.emailHtml)

            'msg = New MailMessage
            'msg.To.Add(EBMailAddress)
            'msg.From = New MailAddress(txtEmail.Text)
            'msg.Subject = subject
            'msg.IsBodyHtml = True
            'msg.Body = email
            'Dim client As New SmtpClient
            'client.Send(msg)
            'msg.Dispose()
        Catch ex As Exception
            siteInclude.addError("ebcontacts.aspx.vb", "dvContact_inserting(); " & ex.ToString())
            lblError.Text = "<font color='red'>An error occured while submitting your details.<br>We are sorry for any inconvenience.<br>Please try again later."
            bError = True
        End Try
        If Not bError Then
            dvContact.Visible = False
            'lblError.Text = "Thank you for your email to EB.<BR><BR>You will recieve an email with your unique contact reference number.<BR>We will contact you as soon as possible."
            lblError.Text = "Thank you for contacting emotional bliss.<BR><BR>We will contact you as soon as possible."
        End If
        e.Cancel = True
    End Sub

    'Subs
    Protected Sub loadDBResources()
        dvContact.Rows(siteInclude.getDVRowByHeader(dvContact, "First Name:")).Cells(0).Text = getDBResourceString("FirstName")
        dvContact.Rows(siteInclude.getDVRowByHeader(dvContact, "Last Name:")).Cells(0).Text = getDBResourceString("LastName")
        dvContact.Rows(siteInclude.getDVRowByHeader(dvContact, "Email Address:")).Cells(0).Text = getDBResourceString("EmailAddress")
        dvContact.Rows(siteInclude.getDVRowByHeader(dvContact, "Telephone:")).Cells(0).Text = getDBResourceString("Telephone")
        dvContact.Rows(siteInclude.getDVRowByHeader(dvContact, "Department:")).Cells(0).Text = getDBResourceString("Department")
        dvContact.Rows(siteInclude.getDVRowByHeader(dvContact, "Subject:")).Cells(0).Text = getDBResourceString("Subject")
        dvContact.Rows(siteInclude.getDVRowByHeader(dvContact, "Order Number(Optional):")).Cells(0).Text = getDBResourceString("OrderNumber")
        dvContact.Rows(siteInclude.getDVRowByHeader(dvContact, "Message:")).Cells(0).Text = getDBResourceString("Message")
        Dim drp As DropDownList = dvContact.FindControl("drpDepartment")
        siteInclude.setDropdownTextByID(getDBResourceString("SelectDepartment"), 0, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("CustomerService"), 1, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("AffiliateEnquiry"), 2, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("PressEnquiry"), 3, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("Accounts"), 4, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("Returns"), 5, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("WebsiteFeedback"), 6, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("ProductInformation"), 7, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("WholesaleEnquiries"), 8, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("DistributionEnquiries"), 9, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("CountryRepresentative"), 10, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("B2BEnquiry"), 11, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("PTMD"), 12, drp)
        siteInclude.setDropdownTextByID(getDBResourceString("JasmineFeedback"), 13, drp)
        Dim tmp As String
        Dim btnInsert As ImageButton = dvContact.FindControl("btnInsert")
        'tmp = siteInclude.trimCrap(getDBResourceString("imgSendEmail"))
        'CType(dvContact.FindControl("lnkSendEmail"), LinkButton).Text = getDBResourceString("lnkSendEmail")
        'If tmp <> "" Then btnInsert.ImageUrl = tmp
    End Sub

    'Funcs
    Protected Function getText(ByVal row As Integer) As String
        Dim txt As TextBox = dvContact.Rows(row).Cells(1).Controls(0)
        Return txt.Text
    End Function
End Class
