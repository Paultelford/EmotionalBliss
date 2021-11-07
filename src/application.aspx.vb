Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Globalization

Partial Class application
    Inherits BasePage
    Private _drpLanguage As DropDownList
    Private bLanguageChanged As Boolean = False

   
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _drpLanguage = Master.FindControl("drpLanguage")
        regPassword.ErrorMessage = "Password must be at least " & Membership.MinRequiredPasswordLength & " charcaters."
        chkSameDelivery.Attributes.Add("onclick", "copyData('" & txtToAdd1.ClientID & "','" & txtToAdd2.ClientID & "','" & txtToAdd3.ClientID & "','" & txtToPostcode.ClientID & "','" & drpToCountry.ClientID & "','" & txtAdd1.ClientID & "','" & txtAdd2.ClientID & "','" & txtAdd3.ClientID & "','" & txtPostcode.ClientID & "','" & drpCountry.ClientID & "');")
        If Page.IsPostBack Then

        Else
            '1st time page runs grab language translations from db and popuplate page
            Dim a() As String = {"lblErrorUsername", "lblErrorPassword", "lblAgree", "lblComplete"}
            'setResources(a)
            PageTitle.Text = getDBResourceString("PageTitle")
            lblIntro.Text = getDBResourceString("lblIntro")
            lblContactDetails.Text = getDBResourceString("lblContactDetails")
            lblTitle.Text = getDBResourceString("lbLTitle")
            lblFirstName.Text = getDBResourceString("lblFirstName")
            lblLastname.Text = getDBResourceString("lblLastName")
            lblPhoneNo.Text = getDBResourceString("lblPhoneNo")
            lblEmail.Text = getDBResourceString("lblEmail")
            lblInvoiceAddress.Text = getDBResourceString("lblInvoiceAddress")
            lblPayToName.Text = getDBResourceString("lblPayToName")
            lblAdd1.Text = getDBResourceString("lblAdd1")
            lblAdd2.Text = getDBResourceString("lblAdd2")
            lblAdd3.Text = getDBResourceString("lblAdd3")
            lblPostcode.Text = getDBResourceString("lblPostcode")
            lblCountry.Text = getDBResourceString("lblCountry")
            lblDeliveryAddress.Text = getDBResourceString("lblDeliveryAddress")
            lblDeliveryInstructions.Text = getDBResourceString("lblDeliveryInstructions")
            lblToAdd1.Text = getDBResourceString("lblToAdd1")
            lblToAdd2.Text = getDBResourceString("lblToAdd2")
            lblToAdd3.Text = getDBResourceString("lblToAdd3")
            lblToPostcode.Text = getDBResourceString("lblToPostcode")
            lblToCountry.Text = getDBResourceString("lblToCountry")
            lblSiteInfo.Text = getDBResourceString("lblSiteInfo")
            lblCompany.Text = getDBResourceString("lblCompany")
            lblSiteName.Text = getDBResourceString("lblSiteName")
            lblURL.Text = getDBResourceString("lblURL")
            lblCategory.Text = getDBResourceString("lblCategory")
            lblRelated.Text = getDBResourceString("lblRelated")
            lblNonRelated.Text = getDBResourceString("lblNonRelated")
            lblNonProfit.Text = getDBResourceString("lblNonProfit")
            lblCharity.Text = getDBResourceString("lblCharity")
            lblVat.Text = getDBResourceString("lblVat")
            lblCredentials.Text = getDBResourceString("lblCredentials")
            lblUsername.Text = getDBResourceString("lblUsername")
            lblPassword.Text = getDBResourceString("lblPassword")
            lblConfirmPassword.Text = getDBResourceString("lblConfirmPassword")
            lblAgreement.Text = getDBResourceString("lblAgreement")
            lblErrorUsername.Text = getDBResourceString("lblErrorUsername")
            lblErrorPassword.Text = getDBResourceString("lblErrorPassword")
            lblAgree.Text = getDBResourceString("lblAgree")
            lblComplete.Text = getDBResourceString("lblComplete")
            btnAgree.ImageUrl = getDBResourceString("btnAgree")
            reqTitle.ErrorMessage = getDBResourceString("text_Required")
            reqFirstName.ErrorMessage = getDBResourceString("text_Required")
            reqLastName.ErrorMessage = getDBResourceString("text_Required")
            reqPhoneNo.ErrorMessage = getDBResourceString("text_Required")
            reqEmail.ErrorMessage = getDBResourceString("text_Required")
            reqPayToName.ErrorMessage = getDBResourceString("text_Required")
            reqAdd1.ErrorMessage = getDBResourceString("text_Required")
            'reqAdd2.ErrorMessage = getDBResourceString("text_Required")
            'reqAdd3.ErrorMessage = getDBResourceString("text_Required")
            reqPostcode.ErrorMessage = getDBResourceString("text_Required")
            reqCompany.ErrorMessage = getDBResourceString("text_Required")
            reqSiteName.ErrorMessage = getDBResourceString("text_Required")
            reqURL.ErrorMessage = getDBResourceString("text_Required")
            reqUsername.ErrorMessage = getDBResourceString("text_Required")
            reqPassword.ErrorMessage = getDBResourceString("text_Required")
            'regPassword.ErrorMessage = getDBResourceString("text_PasswordMin")
            reqConfirmPassword.ErrorMessage = getDBResourceString("text_Required")
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Or bLanguageChanged Then
            drpCountry.SelectedValue = Session("EBLanguage")
            drpToCountry.SelectedValue = Session("EBLanguage")
        End If
    End Sub
    Protected Sub chkSameDelivery_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox = CType(sender, CheckBox)
        If chk.Checked Then
            'Copy delivery details from the Billing/Contact details section
            txtToAdd1.Text = txtAdd1.Text
            txtToAdd2.Text = txtAdd2.Text
            txtToAdd3.Text = txtAdd3.Text
            txtToPostcode.Text = txtPostcode.Text
            drpToCountry.SelectedValue = drpCountry.SelectedValue
        End If
    End Sub
    Protected Sub btnAgree_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Page.Validate()
        If Page.IsValid Then
            Dim result As Boolean = False
            Dim affID As Integer = 0
            'hide error messages
            lblErrorPassword.Visible = False
            lblErrorUsername.Visible = False
            'Check for matching passwords
            If Trim(LCase(txtPassword.Text)) <> Trim(LCase(txtConfirmPassword.Text)) Then
                lblErrorPassword.Visible = True
                lblErrorPassword.Text = "<font color='red'>* Your passwords do not match.</font>"
            Else
                'Pass all user input to stored procedure, which will check for valid username before committing the details
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procAffiliateInsert", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@title", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@firstname", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@surname", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@phone", SqlDbType.VarChar, 30))
                    .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@payToName", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@postcode", SqlDbType.VarChar, 10))
                    .Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                    .Parameters.Add(New SqlParameter("@toAdd1", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@toAdd2", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@toAdd3", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@toPostcode", SqlDbType.VarChar, 10))
                    .Parameters.Add(New SqlParameter("@toCountry", SqlDbType.VarChar, 5))
                    .Parameters.Add(New SqlParameter("@sameDelivery", SqlDbType.Bit))
                    .Parameters.Add(New SqlParameter("@company", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@siteName", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@siteURL", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@category", SqlDbType.VarChar, 30))
                    .Parameters.Add(New SqlParameter("@charity", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@vat", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@password", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@affLinkParam", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                    .Parameters("@title").Value = txtTitle.Text
                    .Parameters("@firstname").Value = txtFirstName.Text
                    .Parameters("@surname").Value = txtLastName.Text
                    .Parameters("@phone").Value = txtPhoneNo.Text
                    .Parameters("@email").Value = txtEmail.Text
                    .Parameters("@payToName").Value = txtPayToName.Text
                    .Parameters("@add1").Value = txtAdd1.Text
                    .Parameters("@add2").Value = txtAdd2.Text
                    .Parameters("@add3").Value = txtAdd3.Text
                    .Parameters("@postcode").Value = txtPostcode.Text
                    .Parameters("@country").Value = drpCountry.Text
                    .Parameters("@toAdd1").Value = txtToAdd1.Text
                    .Parameters("@toAdd2").Value = txtToAdd2.Text
                    .Parameters("@toAdd3").Value = txtToAdd3.Text
                    .Parameters("@toPostcode").Value = txtToPostcode.Text
                    .Parameters("@toCountry").Value = drpToCountry.Text
                    .Parameters("@sameDelivery").Value = chkSameDelivery.Checked
                    .Parameters("@company").Value = txtCompany.Text
                    .Parameters("@siteName").Value = txtSiteName.Text
                    .Parameters("@siteURL").Value = txtURL.Text
                    .Parameters("@category").Value = getCategory()
                    .Parameters("@vat").Value = txtVat.Text
                    .Parameters("@charity").Value = txtCharity.Text
                    .Parameters("@username").Value = txtUsername.Text
                    .Parameters("@password").Value = txtPassword.Text
                    .Parameters("@affLinkParam").Value = "?language=" & drpCountry.Text
                    .Parameters("@affID").Direction = ParameterDirection.Output
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                    affID = CType(oCmd.Parameters("@affID").Value, Integer)
                    If affID > 0 Then result = True
                    If result Then
                        'Add to affLog
                        siteInclude.addToAffiliateLog(affID, "Affiliate application form submitted.", True, "", "System")
                        'Send email to PT
                        Try
                            siteInclude.sendEmail("pt@peartreeuk.co.uk", "scott@emotionalbliss.com", "", "New Affiliate Signup", "noreply@emotionalbliss.com", "EB Affiliates", "A new affiliate has signed up." & Chr(10) & "Country: " & drpCountry.SelectedItem.Text & Chr(10) & "Type: Affiliate")
                        Catch ex As Exception
                            siteInclude.addError("application.aspx.vb", "btnAgree_click(affID=" & affID & "); " & ex.ToString)
                        End Try
                    End If
                Catch ex As Exception
                    siteInclude.addError("application.aspx.vb", "btnAgree_clickB(affID=" & affID & "); " & ex.ToString)
                Finally
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
                If result Then
                    lblComplete.Visible = True
                    tblApplicationForm.Visible = False
                    lblIntro.Visible = False
                Else
                    lblErrorUsername.Visible = True
                    lblErrorUsername.ForeColor = Drawing.Color.Red
                End If
            End If 'Passwords macth
        End If
    End Sub
    Protected Function getCategory()
        Dim result As String = ""
        If radNonProfit.Checked Then result = "nonProfit"
        If radNonRelated.Checked Then result = "nonRelated"
        If radRelated.Checked Then result = "related"
        Return result
    End Function
    Protected Sub setResources(ByRef a As Array)
        Dim c As Label
        For Each s As String In a
            c = FindControl(s)
            c.Text = getDBResourceString(s)
        Next
    End Sub
End Class
