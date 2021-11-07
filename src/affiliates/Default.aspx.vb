Imports System.Data
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class affiliates_Default
    Inherits BasePage

    'System
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Session("EBAffID") = ""
        If Not Request.Cookies("affSetting") Is Nothing Then
            Membership.Provider.ApplicationName = "ebAffProvider"
            Dim bSuccess As Boolean = Membership.ValidateUser(decryptText(Request.Cookies("affSetting")("username")), decryptText(Request.Cookies("affSetting")("password")))
            If bSuccess Then
                setSessionVariables(decryptText(Request.Cookies("affSetting")("username")), decryptText(Request.Cookies("affSetting")("password")))
                'Empty basket each time affiliate logs in, this ensures they have latest prices
                Profile.EBAffCart.emptyBasket()
                'User has been automatically logged in, and session variabvles are setup. So the remainder of the PageLoad sub will redirect the logged in user to their destnation
            Else
                'Password must have changed since cookie was dropped (or user deleted/locked) remove cookie
                removeCookie("affSetting")
            End If
        End If
        If Session("EBAffID") <> "" Then
            Session("EBAffMenuIndex") = 0
            Session("EBTextEdit") = CBool(Session("EBAffEBDistributor")) Or CBool(Session("EBAffEBUser"))
            Session("EBDistMenuValue") = ""
            If CType(Session("EBAffEBDistributor"), Boolean) Or CType(Session("EBAffEBUser"), Boolean) Then
                'Send distributor to the Overview page by default 
                Server.Transfer("~/affiliates/overview.aspx")

            Else
                'Show addiliate overview on this page
                'Server.Transfer("~/affiliates/basket.aspx")
                panLogin.Visible = False
                panOverview.Visible = True
            End If
        Else
            'Focus login element
            Dim txt As TextBox = loginAff.FindControl("UserName")
            Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('focusElement(""" & txt.ClientID & """)',200)", True)
        End If
        lblError.Text = ""
        'Try hard coding resolution to 1024
        Session("EBSiteRes") = "medium"
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        Dim drp As DropDownList = Master.FindControl("drpAffmenu")
        drp.SelectedValue = "overview"
        If Session("EBAffHideCountry") Then
            If dvAffiliate.Rows.Count > 0 Then dvAffiliate.Rows(siteInclude.getDVRowByHeader(dvAffiliate, "Country")).Visible = False
        End If
    End Sub
    'Page

    'User
    Protected Sub dvAffiliate_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvAffiliate.Rows
            If row.Cells(1).Text = "*" Then row.Visible = False
        Next
    End Sub
    Protected Sub loginAff_authenticate(ByVal sender As Object, ByVal e As AuthenticateEventArgs)
        'Manually auth the user credentials, then set Session variables
        Membership.Provider.ApplicationName = "ebAffProvider"
        If Membership.ValidateUser(loginAff.UserName, loginAff.Password) Then
            affSetup()
            e.Authenticated = True
        Else
            e.Authenticated = False
            Dim user As MembershipUser = Membership.GetUser(loginAff.UserName)
            If Not user Is Nothing Then
                If user.IsLockedOut Then
                    loginAff.FailureText = "This account has been locked due to multiple incorrect username/password attempts."
                End If
            End If
        End If
    End Sub
    Protected Sub affSetup()
        setSessionVariables(loginAff.UserName, loginAff.Password)
        'Empty basket each time affiliate logs in, this ensures they have latest prices
        Profile.EBAffCart.emptyBasket()
        'Make cookie if Remember Me checkbox was ticked
        Dim myCookie As HttpCookie
        myCookie = New HttpCookie("affSetting")
        If CBool(loginAff.RememberMeSet.ToString) Then
            'Create cookie
            myCookie("username") = encryptText(loginAff.UserName)
            myCookie("password") = encryptText(loginAff.Password)
            myCookie.Expires = DateTime.Now.AddDays(3)
            Response.Cookies.Add(myCookie)
        Else
            'If Remember Me is not checked, then remove any existing cookie
            removeCookie("affSetting")
        End If
    End Sub
    Protected Sub setSessionVariables(ByVal user As String, ByVal pass As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliatesByUsernamePWSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affUsername", SqlDbType.VarChar, 20))
            .Parameters("@affUsername").Value = user
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim rs As DataRow = ds.Tables(0).Rows(0)
                Session("EBAffID") = CType(rs("affID"), String)
                Session("EBAffUsername") = rs("affiliate")
                Session("EBAffCountryCode") = rs("affCountryCode")
                Session("EBAffCurrencyCode") = rs("affCurrencyCode")
                Session("EBAffCurrencySign") = rs("currencySign")
                Session("EBAffEBDistributor") = rs("affEBDistributor")
                Session("EBAffEBRoyalty") = rs("affRoyalty")
                'Session("EBAffEBDistributorCountryCode") = rs("affEBDistributorCountryCode")
                Session("EBAffEBDistributorCountryCode") = rs("affCountryCode")
                Session("EBAffEBUser") = rs("affEBUser")
                Session("EBAffEBUserCountryCode") = rs("affEBUserCountryCode")
                Session("EBDistID") = rs("distID")
                Session("EBAffTypeID") = rs("affTypeID")
                Session("EBAffHideCountry") = rs("hideCountry")
                If rs("affEBDistributor") Or rs("affEBUser") Then Session("EBTextEdit") = True
            Else
                Response.Redirect("loginError.aspx")
            End If
        Catch ex As Exception
            lblError.Text = "Could not log you in at this time, an error occured."
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function encryptText(ByVal n As String) As String
        Dim enc As String = ""
        Try
            Dim fes As New FE_SymmetricNamespace.FE_Symmetric
            enc = fes.EncryptData(ConfigurationManager.AppSettings("aesKey").ToString, n)
        Catch ex As Exception

        End Try
        Return enc
    End Function
    Protected Function decryptText(ByVal n As String) As String
        Dim dec As String = ""
        Try

        Catch ex As Exception
            Dim fes As New FE_SymmetricNamespace.FE_Symmetric
            dec = fes.DecryptData(ConfigurationManager.AppSettings("aesKey").ToString, n)
        End Try
        Return dec
    End Function
    Protected Sub removeCookie(ByVal c As String)
        Dim myCookie As New HttpCookie(c)
        myCookie.Expires = DateTime.Now.AddDays(-1D)
        Response.Cookies.Add(myCookie)
    End Sub
    Protected Sub lnkForgot_click(ByVal sender As Object, ByVal e As EventArgs)
        panForgot.Visible = True
        loginAff.Visible = False
        lnkForgot.Visible = False
    End Sub
    Protected Sub btnForgotSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Lookup username and email address in the database
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliatesByUsernameEmailSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
            .Parameters("@username").Value = txtUsername.Text
            .Parameters("@email").Value = txtEmail.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'User found
                lblError.Text = "Your details have been found and emailed to <b>" & txtEmail.Text & "</b>"
                sendConfirmation(ds.Tables(0).Rows(0)("affEmail"), ds.Tables(0).Rows(0)("affPassword"))
            Else
                'Not found, inform user.
                lblError.Text = "<font color='red'>The username '" & txtUsername.Text & "' and email '" & txtEmail.Text & "' combination was not found. Please try again.</font>"
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("affiliates/default.aspx", "btnForgotSubmit_click(); " & ex.ToString)
            si = Nothing
            lblError.Text = "<font color='red'>An error occured while trying to check your details. We are sorry for any inconvenience.<br>Please try again later</font>"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub sendConfirmation(ByVal email As String, ByVal pw As String)
        Dim toAdd As String = email
        Dim subject As String = "Emotional Bliss affiliate forgotten password"
        Dim msg As MailMessage
        Dim chk As CheckBox
        Dim plainView As AlternateView
        Dim htmlView As AlternateView
        msg = New MailMessage
        msg.To.Add(toAdd)
        msg.From = New MailAddress("noreply@emotionalbliss.com")
        msg.Subject = subject
        msg.IsBodyHtml = True
        plainView = AlternateView.CreateAlternateViewFromString(emailBody(pw), Nothing, "text/plain")
        htmlView = AlternateView.CreateAlternateViewFromString(Replace(emailBody(pw), Chr(13), "<br>"), Nothing, "text/html")
        msg.AlternateViews.Add(plainView)
        msg.AlternateViews.Add(htmlView)
        Dim client As New SmtpClient
        client.Send(msg)
        msg.Dispose()
    End Sub
    Protected Function emailBody(ByVal pw As String) As String
        Dim body As String = ""
        body = "This is an automated email from Emotional Bliss." & Chr(13) & Chr(13)
        body = body & "A password request was submitted using this email address." & Chr(13)
        body = body & "If you did not make this request, then please contact our <a href='mailto:support@emotionalbliss.com'>support</a> staff." & Chr(13) & Chr(13)
        body = body & "Your affiliate account password is: " & pw
        Return body
    End Function
End Class
