Imports System.Data
Imports System.Data.SqlClient

Partial Class applicationDistributor
    Inherits BasePage
    Private _drpLanguage As DropDownList
    Private bLanguageChanged As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        '_drpLanguage = Master.FindControl("drpLanguage")
        'regPassword.ErrorMessage = "Password must be at least " & Membership.MinRequiredPasswordLength & " charcaters."
        'chkSameDelivery.Attributes.Add("onclick", "copyData('" & txtToAdd1.ClientID & "','" & txtToAdd2.ClientID & "','" & txtToAdd3.ClientID & "','" & txtToPostcode.ClientID & "','" & drpToCountry.ClientID & "','" & txtAdd1.ClientID & "','" & txtAdd2.ClientID & "','" & txtAdd3.ClientID & "','" & txtPostcode.ClientID & "','" & drpCountry.ClientID & "');")
        If Not Page.IsPostBack Then
            Dim tmp As String = ""
            'PageTitle.Text = getDBResourceString("PageTitle")
            lblIntro.Text = getDBResourceString("lblIntro")
            lblContactDetails.Text = getDBResourceString("lblContactDetails")
            lblTitle.Text = getDBResourceString("lbLTitle")
            lblFirstName.Text = getDBResourceString("lblFirstName")
            lblLastname.Text = getDBResourceString("lblLastName")
            lblPhoneNo.Text = getDBResourceString("lblPhoneNo")
            lblEmail.Text = getDBResourceString("lblEmail")
            'lblInvoiceAddress.Text = getDBResourceString("lblInvoiceAddress")
            'lblPayToName.Text = getDBResourceString("lblPayToName")
            lblAdd1.Text = getDBResourceString("lblAdd1")
            lblAdd2.Text = getDBResourceString("lblAdd2")
            lblAdd3.Text = getDBResourceString("lblAdd3")
            lblPostcode.Text = getDBResourceString("lblPostcode")
            lblCountry.Text = getDBResourceString("lblCountry")
            lblSiteURL.Text = getDBResourceString("lblSiteURL")
            lblCompany.Text = getDBResourceString("lblCompany")
            'lblDeliveryAddress.Text = getDBResourceString("lblDeliveryAddress")
            'lblDeliveryInstructions.Text = getDBResourceString("lblDeliveryInstructions")
            'lblToAdd1.Text = getDBResourceString("lblToAdd1")
            'lblToAdd2.Text = getDBResourceString("lblToAdd2")
            'lblToAdd3.Text = getDBResourceString("lblToAdd3")
            'lblToPostcode.Text = getDBResourceString("lblToPostcode")
            'lblToCountry.Text = getDBResourceString("lblToCountry")
            'lblVat.Text = getDBResourceString("lblVat")
            'lblCompanyAdd1.Text = getDBResourceString("lblCompanyAdd1")
            'lblCompanyAdd2.Text = getDBResourceString("lblCompanyAdd2")
            'lblCompanyAdd3.Text = getDBResourceString("lblCompanyAdd3")
            'lblCompanyPostcode.Text = getDBResourceString("lblCompanyPostcode")
            'lblCompanyCountry.Text = getDBResourceString("lblCompanyCountry")
            lblCredentials.Text = getDBResourceString("lblCredentials")
            lblVatNumber.Text = getDBResourceString("lblVatNumber")
            lblUsername.Text = getDBResourceString("lblUsername")
            lblPassword.Text = getDBResourceString("lblPassword")
            lblConfirmPassword.Text = getDBResourceString("lblConfirmPassword")
            lblAgreement.Text = getDBResourceString("lblAgreement")
            lblErrorUsername.Text = getDBResourceString("lblErrorUsername")
            lblErrorPassword.Text = getDBResourceString("lblErrorPassword")
            lblAgree.Text = getDBResourceString("lblAgree")
            lblComplete.Text = getDBResourceString("lblComplete")
            'btnAgree.ImageUrl = getDBResourceString("btnAgree")
            tmp = getDBResourceString("cssSubmit")
            If tmp <> "" Then lnkAgree.CssClass = tmp
            tmp = getDBResourceString("ttSubmit")
            If tmp <> "" Then lnkAgree.ToolTip = tmp
            reqTitle.ErrorMessage = getDBResourceString("text_Required")
            reqFirstName.ErrorMessage = getDBResourceString("text_Required")
            reqLastName.ErrorMessage = getDBResourceString("text_Required")
            reqPhoneNo.ErrorMessage = getDBResourceString("text_Required")
            reqEmail.ErrorMessage = getDBResourceString("text_Required")
            reqCustomerBase.ErrorMessage = getDBResourceString("text_Required")
            'reqPayToName.ErrorMessage = getDBResourceString("text_Required")
            reqAdd1.ErrorMessage = getDBResourceString("text_Required")
            'reqAdd2.ErrorMessage = getDBResourceString("text_Required")
            'reqAdd3.ErrorMessage = getDBResourceString("text_Required")
            'reqPostcode.ErrorMessage = getDBResourceString("text_Required")
            reqCompany.ErrorMessage = getDBResourceString("text_Required")
            'reqSiteName.ErrorMessage = getDBResourceString("text_Required")
            'reqURL.ErrorMessage = getDBResourceString("text_Required")
            reqUsername.ErrorMessage = getDBResourceString("text_Required")
            reqPassword.ErrorMessage = getDBResourceString("text_Required")
            regPassword.ErrorMessage = getDBResourceString("text_PasswordMin")
            reqConfirmPassword.ErrorMessage = getDBResourceString("text_Required")
            'reqCompanyPosition.ErrorMessage = getDBResourceString("text_Required")
            'lblCompanyPosition.Text = getDBResourceString("lblCompanyPosition")
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Or bLanguageChanged Then
            drpCountry.SelectedValue = Session("EBLanguage")
            'drpToCountry.SelectedValue = Session("EBLanguage")
        End If
    End Sub

    'User
    
    Protected Sub btnAgree_click(ByVal sender As Object, ByVal e As EventArgs)
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
                Dim oCmd As New SqlCommand("procAffiliateDistributorInsert2", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@title", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@firstname", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@surname", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@phone", SqlDbType.VarChar, 30))
                    .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
                    '.Parameters.Add(New SqlParameter("@payToName", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@postcode", SqlDbType.VarChar, 10))
                    .Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                    '.Parameters.Add(New SqlParameter("@toAdd1", SqlDbType.VarChar, 50))
                    '.Parameters.Add(New SqlParameter("@toAdd2", SqlDbType.VarChar, 50))
                    '.Parameters.Add(New SqlParameter("@toAdd3", SqlDbType.VarChar, 50))
                    '.Parameters.Add(New SqlParameter("@toPostcode", SqlDbType.VarChar, 10))
                    '.Parameters.Add(New SqlParameter("@toCountry", SqlDbType.VarChar, 5))
                    '.Parameters.Add(New SqlParameter("@sameDelivery", SqlDbType.Bit))
                    .Parameters.Add(New SqlParameter("@company", SqlDbType.VarChar, 50))
                    '.Parameters.Add(New SqlParameter("@companyPosition", SqlDbType.VarChar, 50))
                    '.Parameters.Add(New SqlParameter("@siteName", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@siteURL", SqlDbType.VarChar, 50))
                    '.Parameters.Add(New SqlParameter("@category", SqlDbType.VarChar, 30))
                    '.Parameters.Add(New SqlParameter("@charity", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@vat", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@password", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@affLinkParam", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@customerBase", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@customerBaseArea", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@wholesaleSell", SqlDbType.VarChar, 200))
                    .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                    .Parameters("@title").Value = txtTitle.Text
                    .Parameters("@firstname").Value = txtFirstName.Text
                    .Parameters("@surname").Value = txtLastName.Text
                    .Parameters("@phone").Value = txtPhoneNo.Text
                    .Parameters("@email").Value = txtEmail.Text
                    '.Parameters("@payToName").Value = txtPayToName.Text
                    .Parameters("@add1").Value = txtAdd1.Text
                    .Parameters("@add2").Value = txtAdd2.Text
                    .Parameters("@add3").Value = txtAdd3.Text
                    .Parameters("@postcode").Value = txtPostcode.Text
                    .Parameters("@country").Value = drpCountry.Text
                    '.Parameters("@toAdd1").Value = txtToAdd1.Text
                    '.Parameters("@toAdd2").Value = txtToAdd2.Text
                    '.Parameters("@toAdd3").Value = txtToAdd3.Text
                    '.Parameters("@toPostcode").Value = txtToPostcode.Text
                    '.Parameters("@toCountry").Value = drpToCountry.Text
                    '.Parameters("@sameDelivery").Value = chkSameDelivery.Checked
                    .Parameters("@company").Value = txtCompany.Text
                    '.Parameters("@companyPosition").Value = txtCompanyPosition.Text
                    '.Parameters("@siteName").Value = ""
                    .Parameters("@siteURL").Value = txtSiteURL.Text
                    '.Parameters("@category").Value = ""
                    .Parameters("@vat").Value = txtVatNumber.Text
                    '.Parameters("@charity").Value = ""
                    .Parameters("@username").Value = txtUsername.Text
                    .Parameters("@password").Value = txtPassword.Text
                    .Parameters("@affLinkParam").Value = "?language=" & drpCountry.Text
                    '.Parameters("@wholesaleStores").Value = radStores.SelectedItem.Text
                    .Parameters("@customerBase").Value = radCustomerBase.SelectedItem.Text
                    .Parameters("@customerBaseArea").Value = txtArea.Text
                    .Parameters("@wholesaleSell").Value = txtSellOther.Text
                    .Parameters("@affID").Direction = ParameterDirection.Output
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                    affID = CType(oCmd.Parameters("@affID").Value, Integer)
                    If affID > 0 Then result = True
                    siteInclude.debug("Distributor: All ok, affID=" & affID)
                    If result Then
                        storeCheckboxItems(affID)
                        'Add to affLog
                        siteInclude.addToAffiliateLog(affID, "Distributor application form submitted.", True, "", "System")
                        'Send email to PT
                        Try
                            siteInclude.sendEmail("pt@peartreeuk.co.uk", "scott@emotionalbliss.com", "", "New Affiliate Signup", "noreply@emotionalbliss.com", "EB Affiliates", "A new affiliate has signed up." & Chr(10) & "Country: " & drpCountry.SelectedItem.Text & Chr(10) & "Type: Affiliate")
                        Catch ex As Exception
                            siteInclude.addError("application.aspx.vb", "btnAgree_click(affID=" & affID & "); " & ex.ToString)
                        End Try
                    End If

                Catch ex As Exception
                    siteInclude.addError("applicationDistributor.aspx.vb", "btnAgree_click(); " & ex.ToString)
                    Response.Write(ex.ToString)
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
        Else
            lblComplete.Text = "You must fill out all required fields"
        End If
    End Sub
    Protected Sub storeCheckboxItems(ByVal affID As Integer)
        For Each item As ListItem In chkSell.Items
            If item.Selected Then addItem(item.Text, affID)
        Next
    End Sub
    Protected Sub addItem(ByVal txt As String, ByVal affID As Integer)
        Try
            Dim param() As String = {"@affID", "@item"}
            Dim paramValue() As String = {affID.ToString, txt}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar}
            Dim paramSize() As Integer = {0, 50}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procAffiliateSellsInsert")
        Catch ex As Exception
            siteInclude.addError("applicationDistributor.aspx.vb", "addItem(affID=" & affID & ", txt=" & txt & "); " & ex.ToString)
        Finally
        End Try
    End Sub
End Class
