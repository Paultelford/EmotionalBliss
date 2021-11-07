Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports s2sVB
Imports siteInclude
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports PayPal.Api
Imports PaypalConfigurationClass.Models
Imports Stripe

Partial Class shop_payment
    Inherits BasePage
    Private drpMainCountry As DropDownList
    Private voucherOnlyOrder As Boolean = False
    Private emailBody As String = ""
    Private voucherNumber As String = ""
    Private _voucherNumber As String = ""
    Private Const _maxDeliveryLength As Integer = 200
    Private lblPleaseWait As Label
    Private purchaserEmail As String = ""
    Private _buttonClicked As String = ""
    Private CustomerOrderID As Integer = 0
    Public clientSecret As String = ""

    'System Events
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        'User should have come through to secure server, which will have lost all trace of session vars and shopping cart, grab em back from DB
        If Request.QueryString("sid") <> "" Then Profile.EBCart = getCart(Request.QueryString("sid"))
        If Profile.EBCart Is Nothing Then
            lblError.Text = "Your Shopping Cart is nothing"
            Profile.EBCart = New EBCart
            Response.Redirect("../shopIntro.aspx")
        Else
            If Profile.EBCart.Items.Count = 0 Then
                lblError.Text = "Your Shopping Cart is empty"
                Response.Redirect("../shopIntro.aspx")
            End If
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            drpMainCountry = Master.FindControl("drpMainCountry")
            Dim panLeftTopMenu As Panel = Master.FindControl("panLeftTopMenu")
            panLeftTopMenu.Visible = False
            If Not Page.IsPostBack Then
                'Shop Specific code
                Select Case Session("EBShopCountry")
                    Case "gb"
                        'tblLookup1.Visible = True
                        'tblLookup2.Visible = True
                        'this line commited by shailesh parmar date on 30122020
                        'chkPost.Visible = True
                        chkPost.Visible = False
                        chkPaypal.Visible = True
                        tdPaypalLogo.Visible = True
                        lblPostBreak.Text = ""
                        lblIDealBreak.Text = ""
                        lblDDebitBreak.Text = ""
                        If Application("isDevBox") Then lblFastpayBreak.Text = "<br>"
                        If Application("isDevBox") Then chkFastpay.Visible = True
                    Case "ie"
                        reqBillPostcode.Enabled = False
                        reqShipPostcode.Enabled = False
                        pBill.Visible = False
                        pShip.Visible = False
                        txtBillPostcode.Text = "IE"
                        txtShipPostcode.Text = "IE"
                    Case "es"
                        ES_Fiscal.Visible = True
                        lblPostBreak.Text = ""
                        lblIDealBreak.Text = ""
                        lblDDebitBreak.Text = ""
                    Case "nl"
                        chkIDeal.Visible = True
                        lblIDealBreak.Text = "<br>"
                        chkDDebit.Visible = True
                        'lblDDebitBreak.Text = "<br>"
                        'chkPaypal.Visible = True
                    Case "be"
                        'chkPost.Visible = True
                        'chkIDeal.Visible = True
                        chkEDebit.Visible = True
                        chkDDebit.Visible = True
                        ' lblPostBreak.Text = ""
                        lblIDealBreak.Text = ""
                        lblDDebitBreak.Text = "<br>"
                    Case "de"
                        'chkGiro.Visible = True
                        chkPaypal.Visible = True
                        lblPaypalBreak.Text = "<br>"
                        lblPostBreak.Text = ""
                        lblIDealBreak.Text = ""
                        lblDDebitBreak.Text = ""
                    Case "us"
                        'chkPost.Visible = True
                        lblStateProvince.Visible = True
                        lblStateProvince2.Visible = True
                        drpState.Visible = True
                        drpState2.Visible = True
                        txtBillAdd4.Visible = False
                        txtShipAdd4.Visible = False
                    Case "eu"
                        'tblLookup1.Visible = True
                        'tblLookup2.Visible = True
                        'chkPost.Visible = True
                        'chkPaypal.Visible = True
                        'tdPaypalLogo.Visible = True
                        'lblPostBreak.Text = "<br>"
                        'lblIDealBreak.Text = ""
                        'lblDDebitBreak.Text = ""
                        drpEUState.Visible = True
                        txtBillCountry.Visible = False
                        txtShipCountry.Visible = False
                        'lblStateProvince.Visible = True
                        'lblStateProvince2.Visible = True
                        'If Application("isDevBox") Then lblFastpayBreak.Text = "<br>"
                        'If Application("isDevBox") Then chkFastpay.Visible = True
                    Case "br"
                        chkPost.Visible = True
                End Select
                chkPaypal.Visible = True
                lblPaypalBreak.Text = "<br>"
                'make DOB dropdown items
                drpDay.Items.Add(New ListItem("-", ""))
                drpMonth.Items.Add(New ListItem("-", ""))
                drpYear.Items.Add(New ListItem("-", ""))
                For iLoop As Integer = 1 To 31
                    drpDay.Items.Add(New ListItem(CStr(iLoop), iLoop))
                Next
                Dim months As String = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec"
                Dim aMonth As Array = Split(months, ",")
                For Each m As String In aMonth
                    drpMonth.Items.Add(New ListItem(m, m))
                Next
                For iLoop As Integer = 1900 To Now().Year - 10
                    drpYear.Items.Add(New ListItem(CStr(iLoop), CStr(iLoop)))
                Next
            End If
            'Check for voucher in basket, if found then live auth must be used, so 'Pay By CC' option is the only choice
            'siteInclude.debug("Checking...")
            If voucherOnlyPurchase() Or consultancyInBasket() Then
                'Hide all other types of payment
                hidVoucherPurchase.Value = "True"
                chkPost.Visible = False
                lblPostBreak.Visible = False
                'siteInclude.debug("Set chkPost.Visible = False")
                chkAccount.Visible = False
                chkIDeal.Visible = False
                'chkCC.Checked = True
            End If
            'If the user's country code is 'EEC' then make them choose a state from the dropdown
            'siteInclude.debug("CountryCode=" & Session("EBShopCountry"))
            If UCase(Session("EBShopCountry") = "EU") Then drpEUState.Visible = True
            lblError.Text = ""
            'PayPalPayment("")
        Catch ex As Exception
            'siteInclude.addError("payment.aspx.vb", "Page_Load(); " & ex.ToString)
        End Try
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        loadDBResources()
        Dim ice As ICEPAY.ICEPAY
        ice = New ICEPAY.ICEPAY(Application("icepayMerchantID"), Application("icepayMerchantCode"))
        'Check for error from the 3DComplete.aspx.vb page.
        If Context.Items("source") = "3dcomplete" Or ice.GetData.orderID <> Nothing Or Session("EBTmpError") = "paypal" Then
            'User has been redirected from 3dcomplete, refill all fields on page and show error
            lblError.Text = Context.Items("errorcode")
            chkCC.Checked = True
            txtBillName.Text = Session("ebsBillName")
            If LCase(Session("ebsEmail")) <> "noreply@emotionalbliss.com" Then txtEmail.Text = Session("ebsEmail")
            txtFiscal.Text = Session("ebsBillFiscal")
            txtBillAdd1.Text = Session("ebsBillAdd1")
            txtBillAdd2.Text = Session("ebsBillAdd2")
            txtBillAdd3.Text = Session("ebsBillAdd3")
            txtBillAdd4.Text = Session("ebsBillAdd4")
            txtBillPostcode.Text = Session("ebsBillPostcode")
            txtPhone.Text = Session("ebsPhone")
            txtAccount.Text = Session("ebsAccount")
            txtDelivery.Text = Session("ebsDelivery")
            drpDay.SelectedValue = Session("ebsDay")
            drpMonth.SelectedValue = Session("ebsMonth")
            drpYear.SelectedValue = Session("ebsYear")
            drpGender.SelectedValue = Session("ebsGender")
            'radYes.Checked = Session("ebsYes")
            'radNo.Checked = Session("ebsNo")
            txtShipName.Text = Session("ebsShipName")
            txtShipAdd1.Text = Session("ebsShipAdd1")
            txtShipAdd2.Text = Session("ebsShipAdd2")
            txtShipAdd3.Text = Session("ebsShipAdd3")
            txtShipAdd4.Text = Session("ebsShipAdd4")
            txtShipPostcode.Text = Session("ebsShipPostcode")
            'Update page control state
            panCard.Visible = True
            panBill.Visible = True
            panShip.Visible = False
            trAccount.Visible = False
            'tdPersonalInfo.Visible = True
            'tdPersonalSpacer.Visible = True
            lblBillName.Text = "Name on card:"
            updateProgress()
        End If
        If Session("EBTmpError") = "paypal" Then
            chkPaypal.Visible = True
            chkPaypal.Checked = True
            chkCC.Checked = False
            panCard.Visible = False
            lblError.Text = "Paypal error<br>Status: " & Session("EBTmpStatus") & "<br>Detail: " & Session("EBTmpStatusDetail")
            lblBillName.Text = "Name:"
            'siteInclude.debug("**DELETED Line169")
            deleteOrder(Session("EBTmpUniqueID"))
            Session("EBTmpUniqueID") = ""
            Session("EBTmpError") = ""
            Session("EBTmpStatus") = ""
            Session("EBTmpStatusDetail") = ""
        End If
        If ice.GetData.orderID <> Nothing Then
            'Payment failed
            'Delete the order
            deleteOrder(Request.QueryString("Reference"))
            lblError.Text = ice.GetData.statusCode
            'Set controls state
            chkCC.Checked = False
            panCard.Visible = False
            If LCase(Request.QueryString("PaymentMethod")) = "ideal" Then
                chkIDeal.Checked = True
                panIDealBank.Visible = True
            Else
                chkDDebit.Checked = True
            End If
        End If
    End Sub

    'User Control Events
    Protected Sub chkCC_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkCC.Checked Then
            panCard.Visible = True
            panBill.Visible = True
            panShip.Visible = False
            trAccount.Visible = False
            panIDealBank.Visible = False
            'tdPersonalInfo.Visible = True
            'tdPersonalSpacer.Visible = True
            lblBillName.Text = "Name on card:"
            updateProgress()
        End If
        resetButtons()
    End Sub
    Protected Sub chkPost_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkPost.Checked Then
            panCard.Visible = False
            panBill.Visible = True
            panShip.Visible = False
            trAccount.Visible = False
            panIDealBank.Visible = False
            'tdPersonalInfo.Visible = True
            'tdPersonalSpacer.Visible = False
            lblBillName.Text = "Name:"
            updateProgress()
        End If
        resetButtons()
    End Sub
    Protected Sub chkAccount_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkAccount.Checked Then
            'tdCardDetails.Visible = False
            'tdBillAddress.Visible = True
            'tdShipAddress.Visible = False
            'trAccount.Visible = True
            'tdPersonalInfo.Visible = True
            panIDealBank.Visible = False
            updateProgress()
        End If
        resetButtons()
    End Sub
    Protected Sub chkIDeal_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkIDeal.Checked Then
            panBill.Visible = True
            panShip.Visible = False
            panIDealBank.Visible = True
            panCard.Visible = False
            updateProgress()
        End If
        resetButtons()
    End Sub
    Protected Sub chkDDebit_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Try
            If chkDDebit.Checked Then
                panBill.Visible = True
                panShip.Visible = False
                panIDealBank.Visible = False
                panCard.Visible = False
                updateProgress()
            End If
            resetButtons()
        Catch ex As Exception
            siteInclude.debug(ex.ToString)
        End Try
    End Sub
    Protected Sub chkEDebit_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkEDebit.Checked Then
            panBill.Visible = True
            panShip.Visible = False
            panCard.Visible = False
            updateProgress()
        End If
        resetButtons()
    End Sub
    Protected Sub chkGiro_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkGiro.Checked Then
            panBill.Visible = True
            panShip.Visible = False
            panCard.Visible = False
            updateProgress()
        End If
        resetButtons()
    End Sub
    Protected Sub chkPaypal_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkPaypal.Checked Then
            panBill.Visible = True
            panShip.Visible = False
            panCard.Visible = False
            updateProgress()
        End If
        resetButtons()
    End Sub
    Protected Sub chkFastpay_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkFastpay.Checked Then
            panBill.Visible = True
            panShip.Visible = False
            panCard.Visible = False
            updateProgress()
        End If
        resetButtons()
    End Sub
    Protected Sub radYes_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        panShip.Visible = False
        panBill.Visible = True
        If chkDDebit.Checked Or chkIDeal.Checked Or chkEDebit.Checked Then
            btnIcePay.Visible = True
        Else
            btnSubmitBill.Visible = True
            btnSubmitShipBill.Visible = False
        End If
        tblButtons.Visible = True
    End Sub
    Protected Sub radNo_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        panShip.Visible = True
        If chkCC.Checked Then panBill.Visible = False
        If chkIDeal.Checked Or chkDDebit.Checked Or chkEDebit.Checked Then
            btnIcePay.Visible = True
            'If chkCC.Checked Then btnViewBilling.Visible = True
        Else
            btnSubmitBill.Visible = False
            btnSubmitShipBill.Visible = True
        End If
        tblButtons.Visible = True
    End Sub
    Protected Sub btnSubmitShipBill_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("add")
        If Page.IsValid Then
            Dim btn As LinkButton = CType(sender, LinkButton)
            btn.Visible = False
            _buttonClicked = "shipbill"
            'All ok, store details in db
            commitDetails()
        End If
    End Sub
    Protected Sub btnSubmitBill_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("add")
        If Page.IsValid Then
            Dim btn As LinkButton = CType(sender, LinkButton)
            btn.Visible = False
            _buttonClicked = "bill"
            'All ok, store details in db
            commitDetails()
            If chkPaypal.Checked Then
                PayPalPayment()
            ElseIf chkCC.Checked Then
                'CreditCardPayment()
                CreditCardPaymentNew()
            End If

        End If
    End Sub


    Protected Sub CreditCardPaymentNew()
        StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings("stripeSecretKey"))

        Dim tokenOptions = New StripeTokenCreateOptions()
        tokenOptions.Card = New StripeCreditCardOptions()
        tokenOptions.Card.Number = txtCard.Text
        tokenOptions.Card.ExpirationMonth = drpEndMonth.SelectedValue
        tokenOptions.Card.ExpirationYear = drpEndYear.SelectedValue
        tokenOptions.Card.Cvc = txtCV2.Text

        Try
            Dim tokenService = New StripeTokenService()
            Dim token = tokenService.Create(tokenOptions)
            Dim charge = New StripeChargeCreateOptions()
            charge.Amount = Convert.ToInt32(Profile.EBCart.TotalInc * 100)
            charge.Description = "Emotional Bliss Shopping"
            charge.Currency = getOrderCurrency(Profile.EBShop)
            charge.SourceTokenOrExistingSourceId = token.Id

            Dim chargeService = New StripeChargeService
            Dim StripeCharge = chargeService.Create(charge)

            If StripeCharge.Status = "succeeded" Then
                Response.Redirect("~/shop/PaymentSuccess.aspx?PaymentID=" + StripeCharge.Id + "&OrderID=" + Session("EBTmpUniqueID").ToString())
            End If

        Catch ex As Exception
            lblError.Text = "INVALID - The transaction failed due to there being an issue with the card details you entered."
        End Try

    End Sub



    'This Method created by shailesh parmar date on 25122020
    Protected Sub CreditCardPayment()
        Dim objAPIContext As New APIContext()
        objAPIContext = PaypalConfiguration.GetAPIContext()
        Dim createdPayment As New Payment()
        createdPayment = PaypalConfiguration.PaymentWithCreditCard(objAPIContext, Profile.EBCart, CustomerOrderID)
        If createdPayment.state.ToLower() = "approved" Then
            Response.Redirect("~/shop/PaymentSuccess.aspx?PaymentID=" + createdPayment.id + "&OrderID=" + Session("EBTmpUniqueID").ToString())
        End If
    End Sub


    'This method created by shailesh parmar Date on 28112020
    Protected Sub PayPalPayment()
        Dim objAPIContext As New APIContext()
        objAPIContext = PaypalConfiguration.GetAPIContext()
        Dim payerId As String = Request.QueryString("sid")
        'If String.IsNullOrEmpty("") Then
        Dim baseURI As String = Request.Url.Scheme + "://" + Request.Url.Authority + "/shop/PaymentSuccess.aspx?"
        Dim Guid As String = Session("EBTmpUniqueID").ToString()
        Dim createdPayment As New Payment()
        createdPayment = PaypalConfiguration.CreatePayment(objAPIContext, baseURI + "guid=" + Guid, Profile.EBCart, CustomerOrderID)
        'Dim linkss As VariantType
        Dim paypalRedirectUrl As String = Nothing
        'linkss = createdPayment.links.GetEnumerator()
        For Each item As Links In createdPayment.links '.GetEnumerator() '.MoveNext()
            'debug.Write(item & " ")
            If item.rel.ToLower().Trim().Equals("approval_url") Then
                paypalRedirectUrl = item.href
            End If
        Next
        'While createdPayment.links.GetEnumerator().MoveNext()
        '    Dim lnk As Links = createdPayment.links.GetEnumerator().Current()
        '    If lnk.rel.ToLower().Trim().Equals("approval_url") Then
        '        paypalRedirectUrl = lnk.href
        '    End If
        'End While
        Session.Add(Guid, createdPayment.id)
        Response.Redirect(paypalRedirectUrl)
        'Response.Redirect("https://www.sandbox.paypal.com/checkoutweb/signup?token=" + createdPayment.token + "&country.x=GB&locale.x=en_GB&locale.x=en_GB&country.x=GB")
        ' Else

        'End If

    End Sub

    Protected Sub lnkLookup_click(ByVal sender As Object, ByVal e As CommandEventArgs)
        Dim PCSoap As New PremiseOnWeb.PostCoderWebSOAP
        'clear previous results
        clearAddress(e.CommandName)
        lblBillPostcodeError.Text = ""
        lblShipPostcodeError.Text = ""
        'For Testing Purposes the username and password are loaded from a global variables.
        Dim TestIdent As String = "VBNetTestThoroughfare" 'This is a custom field that we track for you
        Dim postcode As String = txtLookupShipPostcode.Text
        If e.CommandName = "bill" Then postcode = txtLookupBillPostcode.Text
        If postcode <> "" Then
            'This line makes the soap call. Any error here will be a .Net error, service errors are tracked differently.
            Dim TFResult As PremiseOnWeb.ThrfareAddressResult = PCSoap.getThrfareAddresses(postcode, TestIdent, ConfigurationManager.AppSettings("TFUsername").ToString, ConfigurationManager.AppSettings("TFPassword").ToString)

            If Not (TFResult Is Nothing) Then
                If TFResult.error_code <> "0" Then
                    'Errors With Lookup
                    Response.Write("Error found:" & TFResult.error_message)
                Else
                    showResults(TFResult, e.CommandName)
                End If
            End If
            TFResult = Nothing
        End If
        PCSoap.Dispose()
    End Sub
    Protected Sub btnViewBilling_click(ByVal sender As Object, ByVal e As EventArgs)
        'tdShipAddress.Visible = False
        'tdBillAddress.Visible = True
        'btnViewShipping.visible = True
    End Sub
    Protected Sub btnViewShipping_click(ByVal sender As Object, ByVal e As EventArgs)
        'tdShipAddress.Visible = True
        'If chkCC.checked Then tdBillAddress.Visible = False
    End Sub
    Protected Sub btnIcePay_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("add")
        If Page.IsValid Then
            Dim btn As LinkButton = CType(sender, LinkButton)
            btn.Visible = False
            _buttonClicked = "icepay"
            'All ok, store details in db
            commitDetails()
        Else
            siteInclude.debug("error")
        End If
    End Sub

    'Databinding Events
    Protected Sub dropdown_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        drp.Items.Add(New ListItem("Select...", "0"))
    End Sub
    Protected Sub drpStartYear_load(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsPostBack Then
            Dim list As New ArrayList
            For iLoop As Integer = 1990 To Now().Year
                list.Add(New ListItem(iLoop, iLoop))
            Next
            drpStartYear.DataSource = list
            drpStartYear.DataBind()
        End If
    End Sub
    Protected Sub drpEndYear_load(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsPostBack Then
            Dim list As New ArrayList
            For iLoop As Integer = Now().Year To Now().Year + 12
                list.Add(New ListItem(iLoop, iLoop))
            Next
            drpEndYear.DataSource = list
            drpEndYear.DataBind()
        End If
    End Sub
    Protected Sub drpEndMonth_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        drpEndMonth.SelectedValue = Now().Month
    End Sub
    Protected Sub txtBillCountry_load(ByVal sender As Object, ByVal e As EventArgs)
        'Grab countryName from DB
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = Session("EBShopCountry")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                txtBillCountry.Text = ds.Tables(0).Rows(0)("countryName")
                txtShipCountry.Text = txtBillCountry.Text
            End If
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub lblPleaseWait_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label = CType(sender, Label)
        lbl.Text = getDBResourceString("lblPleaseWait", "global")
    End Sub
    Protected Sub litPleaseWait_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Literal = CType(sender, Literal)
        lbl.Text = getDBResourceString("lblPleaseWait", "global")
    End Sub

    'Subs
    Protected Sub updateProgress()
        'Update the leftmenu progress bar
        litPaymentText.Text = "Payment Method"
        litDeliveryText.Text = "<span class='title3'>Delivery Address</span>"
        imgMiddleSection.ImageUrl = "/design/shop/images/basket-menu.gif"
        imgMiddleSection.Height = "50"
        imgBottomSection.ImageUrl = "/design/shop/images/basket-menu-activ.gif"
        imgBottomSection.Height = "55"
        updateTree.Update()
    End Sub
    Protected Sub resetButtons()
        'tdBreak.Visible = tdCardDetails.Visible
        btnSubmitBill.Visible = False
        btnSubmitShipBill.Visible = False
        radNo.Checked = False
        radYes.Checked = False
        tblButtons.Visible = False
    End Sub
    Protected Sub bindMonths(ByVal sender As Object, ByVal e As EventArgs)
        If Not Page.IsPostBack Then
            Dim drp As DropDownList = CType(sender, DropDownList)
            Dim list As New ArrayList
            For iLoop As Integer = 1 To 12
                list.Add(New ListItem(iLoop, iLoop))
            Next
            drp.DataSource = list
            drp.DataBind()
        End If
    End Sub
    Protected Sub commitDetails()
        If Len(txtDelivery.Text) < _maxDeliveryLength Then
            Dim Type As String = ""
            Dim affID As Integer = 0
            Dim prefix As String = ""
            Dim ID As Integer = 0
            Dim orderID As Integer = 0
            Dim bError As Boolean = False
            Dim bLiveAuth As Boolean = False
            Dim bVoucherOrder As Boolean = False
            Dim voucherNumber As String = ""
            If chkAccount.Checked Then
                Type = "account"
                prefix = "20"
            End If
            If chkCC.Checked Then
                Type = "cc"
                prefix = "20"
            End If
            If chkPost.Checked Then
                Type = "cheque"
                prefix = "25"
            End If
            If chkIDeal.Checked Then
                Type = "ideal"
                prefix = "25"
            End If
            If chkDDebit.Checked Then
                Type = "ddebit"
                prefix = "25"
            End If
            If chkEDebit.Checked Then
                Type = "ddebit"
                prefix = "25"
            End If
            If chkGiro.Checked Then
                Type = "giro"
                prefix = "25"
            End If
            If chkPaypal.Checked Then
                Type = "paypal"
                prefix = "25"
            End If
            If chkFastpay.Checked Then
                Type = "fastpay"
                prefix = "25"
            End If
            'Add American State to txtAdd4 fields
            If LCase(Session("EBShopCountry")) = "us" Then
                txtBillAdd4.Text = drpState.SelectedValue
                If Not CType(radYes.Checked, Boolean) Then txtShipAdd4.Text = drpState2.SelectedValue
            End If
            'Do EUState
            If drpEUState.Visible = True Then
                Session("EUShopCountry") = drpEUState.SelectedValue
            End If
            'If user doesnt enter an email, then default to noreply@emotionalbliss.com
            If txtEmail.Text = "" Then txtEmail.Text = "noreply@emotionalbliss.com"
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procShopCustomerInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@billName", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@billAdd1", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@billAdd2", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@billAdd3", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@billAdd4", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@billAdd5", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@billPostcode", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@phone", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@billCountry", SqlDbType.VarChar, 30))
                .Parameters.Add(New SqlParameter("@shipName", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@shipAdd1", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@shipAdd2", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@shipAdd3", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@shipAdd4", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@shipAdd5", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@shipPostcode", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@shipCountry", SqlDbType.VarChar, 30))
                .Parameters.Add(New SqlParameter("@cardNo", SqlDbType.VarChar, 30))
                .Parameters.Add(New SqlParameter("@ccEnc", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@cardExp", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@cardStart", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@cardIssue", SqlDbType.VarChar, 3))
                .Parameters.Add(New SqlParameter("@cardType", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@cardCv2", SqlDbType.VarChar, 4))
                .Parameters.Add(New SqlParameter("@useBillAdd", SqlDbType.Bit))
                .Parameters.Add(New SqlParameter("@orderType", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@accountNo", SqlDbType.VarChar, 12))
                .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@fiscal", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@dob", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@gender", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@delivery", SqlDbType.VarChar, 200))
                .Parameters.Add(New SqlParameter("@outID", SqlDbType.Int))
                .Parameters("@billName").Value = txtBillName.Text
                .Parameters("@billAdd1").Value = txtBillAdd1.Text
                .Parameters("@billAdd2").Value = txtBillAdd2.Text
                .Parameters("@billAdd3").Value = txtBillAdd3.Text
                .Parameters("@billAdd4").Value = txtBillAdd4.Text
                .Parameters("@billAdd5").Value = txtBillAdd5.Text
                .Parameters("@billPostcode").Value = txtBillPostcode.Text
                .Parameters("@phone").Value = txtPhone.Text
                .Parameters("@billCountry").Value = txtBillCountry.Text
                .Parameters("@shipName").Value = txtShipName.Text
                .Parameters("@shipAdd1").Value = txtShipAdd1.Text
                .Parameters("@shipAdd2").Value = txtShipAdd2.Text
                .Parameters("@shipAdd3").Value = txtShipAdd3.Text
                .Parameters("@shipAdd4").Value = txtShipAdd4.Text
                .Parameters("@shipAdd5").Value = txtShipAdd5.Text
                .Parameters("@shipPostcode").Value = txtShipPostcode.Text
                .Parameters("@shipCountry").Value = txtShipCountry.Text
                .Parameters("@cardNo").Value = txtCard.Text 'getHidCardNo(txtCard.Text)
                .Parameters("@ccEnc").Value = encryptCard(txtCard.Text)
                .Parameters("@cardExp").Value = getCard("End")
                .Parameters("@cardStart").Value = getCard("Start")
                .Parameters("@cardIssue").Value = txtIssue.Text
                .Parameters("@cardType").Value = drpCardType.Text
                .Parameters("@cardCv2").Value = txtCV2.Text
                .Parameters("@useBillAdd").Value = CType(radYes.Checked, Boolean)
                .Parameters("@orderType").Value = Type
                .Parameters("@accountNo").Value = txtAccount.Text
                .Parameters("@affID").Value = affID
                .Parameters("@email").Value = txtEmail.Text
                .Parameters("@fiscal").Value = txtFiscal.Text
                .Parameters("@dob").Value = ""
                .Parameters("@gender").Value = drpGender.SelectedValue
                .Parameters("@delivery").Value = txtDelivery.Text
                .Parameters("@outID").Direction = ParameterDirection.Output
            End With
            'If lCase(Session("EBShopCountry")) <> "gb" Then oCmd.Parameters("@fiscal").Value = ""
            If IsDate(drpDay.SelectedValue & " " & drpMonth.SelectedValue & " " & drpYear.SelectedValue) Then oCmd.Parameters("@dob").Value = FormatDateTime(drpDay.SelectedValue & " " & drpMonth.SelectedValue & " " & drpYear.SelectedValue, DateFormat.GeneralDate)
            If CType(radYes.Checked, Boolean) Then
                oCmd.Parameters("@shipName").Value = txtBillName.Text
                oCmd.Parameters("@shipAdd1").Value = txtBillAdd1.Text
                oCmd.Parameters("@shipAdd2").Value = txtBillAdd2.Text
                oCmd.Parameters("@shipAdd3").Value = txtBillAdd3.Text
                oCmd.Parameters("@shipAdd4").Value = txtBillAdd4.Text
                oCmd.Parameters("@shipAdd5").Value = txtBillAdd5.Text
                oCmd.Parameters("@shipPostcode").Value = txtBillPostcode.Text
                oCmd.Parameters("@shipCountry").Value = txtBillCountry.Text
            End If
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
                ID = CType(oCmd.Parameters("@outID").Value, Integer)
                CustomerOrderID = ID
            Catch ex As Exception
                Try
                    Dim si As New siteInclude
                    si.addError("shop/payment.aspx", "Error adding customer details in commitDetails():" & ex.ToString)
                    si = Nothing
                    bError = True
                    lblError.Text = "<b><font color='red'>There has been an error processing your details.  Your order cannot be completed at this time.</font></b>"
                    btnSubmitBill.Visible = False
                    btnSubmitShipBill.Visible = False
                    tblButtons.Visible = False
                Catch e As Exception
                End Try
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            Dim uqID As Integer = 0 'The shopOrder PK field
            If Not bError Then
                Try
                    orderID = commitOrder(ID, prefix, Type, uqID) 'Bring back new orderID and orderID PK
                    If prefix = 25 Then updateOrderStatus(uqID, "Cancelled") 'Set cheque orders status to cancelled
                    Session("EBTmpUniqueID") = uqID
                    'siteInclude.debug("Order created, Session(EBTmpUniqueID)=" & Session("EBTmpUniqueID"))
                Catch ex As Exception
                    Dim si As New siteInclude
                    si.addError("shop/payment.aspx", "Error in commitOrderID(" & ID & "," & prefix & "," & Type & "," & uqID & ");" & ex.ToString)
                    si = Nothing
                End Try
            Else
                'An error occured withing commitOrder(). Set orderID=0 to stop the continuaiton of the order process
                orderID = 0
            End If
            If orderID <> 0 Then 'If orderID contains a 0 then there has been an error 
                'Add Order Placed message to orderlog
                Try
                    siteInclude.AddToOrderLog(uqID, "Order Placed", "System", True, "Emailed")
                Catch ex As Exception
                End Try
                Dim d As New siteInclude
                'Context.Items("orderID") = orderID
                'Context.Items("orderPrefix") = prefix
                'Context.Items("orderPaid") = ""
                Session("EBTmpOrderType") = Type
                If CBool(hidVoucherPurchase.Value) Then voucherOnlyOrder = True
                Session("EBTmpOrderPaid") = False
                Session("EBTmpOrderID") = CStr(orderID)
                Session("EBTmpuqID") = CStr(uqID)
                Session("EBTmpUniqueID") = uqID
                Session("EBTmpOrderPrefix") = CStr(prefix)
                storeCustomerDetails()
                'siteInclude.debug("Payment.vb::CommitDetails(); storeCustomerDetails(); Session(EBTmpOrderID)=" & Session("EBTmpOrderID"))
                If chkDDebit.Checked Or chkIDeal.Checked Or chkEDebit.Checked Or chkGiro.Checked Or chkFastpay.Checked Then
                    'Payment must be made via Icepay
                    Dim ice
                    storeCustomerDetails() 'Incase transactions fails and user gets sent back here
                    If chkDDebit.Checked Then
                        ice = New ICEPAY.ICEPAY_DDebit(Application("icepayMerchantID"), Application("icepayMerchantCode"))
                        ice.SetReference(uqID.ToString)
                        Response.Redirect(ice.Pay("NL", "NL", "EUR", Profile.EBCart.TotalInc * 100, orderID & UCase(Session("EBShopCountry"))))
                    End If
                    If chkEDebit.Checked Then
                        ice = New ICEPAY.ICEPAY_DirectEBank(Application("icepayMerchantID"), Application("icepayMerchantCode"))
                        Response.Redirect(ice.Pay("BE", "NL", Profile.EBCart.TotalInc * 100, orderID & UCase(Session("EBShopCountry"))))
                    End If
                    If chkIDeal.Checked Then
                        ice = New ICEPAY.ICEPAY_iDEAL(Application("icepayMerchantID"), Application("icepayMerchantCode"))
                        ice.SetReference(uqID.ToString)
                        Response.Redirect(ice.Pay(drpIDealBank.SelectedValue, Profile.EBCart.TotalInc * 100, orderID & UCase(Session("EBShopCountry"))))
                    End If
                    If chkGiro.Checked Then
                        ice = New ICEPAY.ICEPAY_GiroPAY(Application("icepayMerchantID"), Application("icepayMerchantCode"))
                        ice.SetReference(uqID.ToString)
                        Response.Redirect(ice.Pay(Profile.EBCart.TotalInc * 100, orderID & UCase(Session("EBShopCountry"))))
                    End If
                    If chkFastpay.Checked Then
                        'Set the order status to 'Cancelled' as page will stop running so wont be set later on.
                        siteInclude.updateReceiptField(uqID, "orderStatus", "cancelled", True)
                        submitFormToFastpay(uqID)
                    End If
                Else
                    If Type <> "cheque" Then
                        siteInclude.addError("test", "voucherOnlyOrder=" & voucherOnlyOrder)
                        siteInclude.addError("test", "consultancyInBasket=" & consultancyInBasket())
                        'If the shopper is buying a voucher or consultancy voucher, then use live authorization (voucher/consultancy will be emailed to the shopper immediately if auth is successful)
                        If voucherOnlyOrder Or consultancyInBasket() Then
                            'bLiveAuth = True
                            'Session("EBTmpOrderPaid") = take3DPayment(uqID, "PAYMENT") 'Live payment no longer needed for voucher orders
                            bVoucherOrder = True
                            If chkPaypal.Checked Then
                                Session("EBTmpOrderDeferred") = take3DPayment(uqID, "PAYMENT")
                            Else
                                'Changed to PAYMENT rather then DEFERRED on 12/10/2011 sp
                                'Session("EBTmpOrderDeferred") = take3DPayment(uqID, "DEFERRED")
                                Session("EBTmpOrderDeferred") = take3DPayment(uqID, "PAYMENT")
                            End If
                        Else
                            If chkPaypal.Checked Then
                                Session("EBTmpOrderDeferred") = take3DPayment(uqID, "PAYMENT")
                            Else
                                'Changed to PAYMENT rather then DEFERRED on 12/10/2011 sp
                                'Session("EBTmpOrderDeferred") = take3DPayment(uqID, "DEFERRED")
                                Session("EBTmpOrderDeferred") = take3DPayment(uqID, "PAYMENT")
                            End If
                        End If
                        If Profile.EBCart.VoucherNumber <> "" Then
                            'A voucher/coupon was used in this order
                            'Add the orderID to the voucher
                            addOrderToVoucher(uqID, Profile.EBCart.VoucherNumber)
                        End If
                        Session("EBTmpUniqueID") = uqID
                        siteInclude.addError("payment.aspx.vb1", "EBTmpOrderDeferred=" & Session("EBTmpOrderDeferred"))
                        If bVoucherOrder Then
                            'Only goto receipt page if transaction was successful
                            If Session("EBTmpOrderDeferred") = "ok" Then
                                'Voucher orders will always have vat of 0, and goods total will always be same as orderTotal.
                                'Dont add to sales ledger yet, as voucher orders are deferred now instead of live payments
                                'si.addToSalesLedger(uqID, 0, 1, Session("EBShopCountry"), prefix, Profile.EBCart.GoodsIncVat, 0, 0, 0)
                                'si = Nothing
                                If voucherOnlyOrder Then
                                    setVoucherActive()
                                    'If Not consultancyInBasket() Then sendConfirmationEmail(orderID, uqID, "voucher")
                                End If
                                sendConfirmationEmail(orderID, uqID, "confirmation")
                                'Add voucher despatched via email to order log
                                siteInclude.AddToOrderLog(uqID, "Voucher " & _voucherNumber & " despatched via email", "System", True, "Email")
                                Response.Redirect("receipt.aspx")
                            Else
                                'Payment failed, delete the order
                                siteInclude.addError("payment.aspx.vb2", "**DELETED voucher line 735" & uqID)
                                'deleteOrder(uqID)
                            End If
                        Else
                            'DEFERRED payment - Changed by pt on 12/10/2011, immediate payemnt now taken. 
                            'updateOrderStatus(uqID, "paid")

                            siteInclude.addError("CRAIG", Session("EBTmpOrderDeferred"))

                            If Session("EBTmpOrderDeferred") = "ok" Then
                                'Card not 3dsecure, but it has been authorised/deferred - continue to receipt page
                                'sendConfirmationEmail(orderID, uqID, "confirmation")
                                siteInclude.addError("payment.aspx.vb3", "**A" & uqID)
                                'Response.Redirect("receipt.aspx")
                            ElseIf Session("EBTmpOrderDeferred") = "3dauth" Then
                                'Order is 3D Auth, store all form values incase 3dauth fails and user gets redirected back here
                                'storeCustomerDetails()
                                siteInclude.addError("payment.aspx.vb4", "**B" & uqID)
                                setOrderAs3DAuth(uqID)
                            Else
                                'Payment failed, delete the order
                                siteInclude.addError("payment.aspx.vb5", "**DELETED " & uqID)
                                'This line commited by shailesh parmar date on 29122020
                                'deleteOrder(uqID)
                            End If

                        End If
                    Else
                        'Cheque order
                        Response.Redirect("receipt.aspx")
                    End If
                End If
            End If
            'Server.Transfer("receipt.aspx")
        Else
            'txtDelivery is greater than 200 characters, show error message
            lblDeliveryError.Text = "<font color='red'>Max length " & _maxDeliveryLength & " charcaters, you have used " & Len(txtDelivery.Text)
        End If
    End Sub
    Protected Sub storeCustomerDetails()
        'store all form values incase 3dauth fails and user gets redirected back here
        Session("ebsBillName") = txtBillName.Text
        Session("ebsEmail") = txtEmail.Text
        Session("ebsBillFiscal") = txtFiscal.Text
        Session("ebsBillAdd1") = txtBillAdd1.Text
        Session("ebsBillAdd2") = txtBillAdd2.Text
        Session("ebsBillAdd3") = txtBillAdd3.Text
        Session("ebsBillAdd4") = txtBillAdd4.Text
        Session("ebsBillPostcode") = txtBillPostcode.Text
        Session("ebsPhone") = txtPhone.Text
        Session("ebsAccount") = txtAccount.Text
        Session("ebsDelivery") = txtDelivery.Text
        Session("ebsDay") = drpDay.SelectedValue
        Session("ebsMonth") = drpMonth.SelectedValue
        Session("ebsYear") = drpYear.SelectedValue
        Session("ebsGender") = drpGender.SelectedValue
        Session("ebsYes") = radYes.Checked
        Session("ebsNo") = radNo.Checked
        Session("ebsShipName") = txtShipName.Text
        Session("ebsShipAdd1") = txtShipAdd1.Text
        Session("ebsShipAdd2") = txtShipAdd2.Text
        Session("ebsShipAdd3") = txtShipAdd3.Text
        Session("ebsShipAdd4") = txtShipAdd4.Text
        Session("ebsShipPostcode") = txtShipPostcode.Text
    End Sub
    Protected Sub commitCartItem(ByVal orderID As Integer, ByRef cartItem As Object)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderItemInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@saleID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@vatRate", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@distBuyingID", SqlDbType.Int))
            .Parameters("@orderID").Value = orderID
            .Parameters("@saleID").Value = cartItem.ID
            .Parameters("@qty").Value = cartItem.qty
            .Parameters("@price").Value = cartItem.Price
            .Parameters("@vatRate").Value = cartItem.Vat
            .Parameters("@distBuyingID").Value = cartItem.DistBuyingID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Try
                Dim si As New siteInclude
                si.addError("shop/payment.aspx", "Error adding order item in commitCartItem(" & orderID & "," & cartItem.ID & "):" & ex.ToString)
                si = Nothing
            Catch e As Exception
            End Try
            lblError.Text = "There has been an error processing your details.  Your order cannot be completed at this time."
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Private Sub showResults(ByRef TFResult As PremiseOnWeb.ThrfareAddressResult, ByVal cName As String)
        Try
            Dim p = TFResult.addresses(0)
            Select Case cName
                Case "bill"
                    If p.street <> "" Then txtBillAdd1.Text = txtLookupBillHouse.Text & " " & p.street.ToString
                    If p.dependent_locality <> "" Then txtBillAdd2.Text = p.dependent_locality.ToString
                    If p.post_town <> "" Then txtBillAdd3.Text = p.post_town.ToString
                    If p.county <> "" Then txtBillAdd4.Text = p.county.ToString
                    If p.postcode <> "" Then txtBillPostcode.Text = p.postcode.ToString
                Case "ship"
                    If p.street <> "" Then txtShipAdd1.Text = txtLookupShipHouse.Text & " " & p.street.ToString
                    If p.dependent_locality <> "" Then txtShipAdd2.Text = p.dependent_locality.ToString
                    If p.post_town <> "" Then txtShipAdd3.Text = p.post_town.ToString
                    If p.county <> "" Then txtShipAdd4.Text = p.county.ToString
                    If p.postcode <> "" Then txtShipPostcode.Text = p.postcode.ToString
            End Select

        Catch ex As Exception
            'If error was returned, then postcode was not valid and found no address
            Select Case cName
                Case "bill"
                    lblBillPostcodeError.Text = "Invalid Postcode"
                Case "ship"
                    lblShipPostcodeError.Text = "Invalid Postcode"
            End Select

        End Try
    End Sub
    Protected Sub clearAddress(ByVal cName As String)
        Select Case cName
            Case "bill"
                txtBillAdd1.Text = ""
                txtBillAdd2.Text = ""
                txtBillAdd3.Text = ""
                txtBillAdd4.Text = ""
                txtBillAdd5.Text = ""
                txtBillPostcode.Text = ""
            Case "ship"
                txtShipAdd1.Text = ""
                txtShipAdd2.Text = ""
                txtShipAdd3.Text = ""
                txtShipAdd4.Text = ""
                txtShipAdd5.Text = ""
                txtShipPostcode.Text = ""
        End Select
    End Sub
    Protected Sub updateVoucher(ByVal id As Integer, ByVal num As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procVoucherByNumUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@voucherNumber", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@active", SqlDbType.Bit))
            .Parameters("@id").Value = id
            .Parameters("@voucherNumber").Value = num
            .Parameters("@active").Value = False
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx.vb", "updateVoucher(" & id & "," & num & ")::" & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub setVoucherToUsed(ByVal orderID As Integer, ByVal num As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procVoucherByNumUsedUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@voucherNumber", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@active", SqlDbType.Bit))
            .Parameters("@id").Value = orderID
            .Parameters("@voucherNumber").Value = num
            .Parameters("@active").Value = False
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx.vb", "setVoucherToUsed(" & orderID & "," & num & ")::" & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub addOrderToVoucher(ByVal orderID As Integer, ByVal voucherNumber As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDVoucherUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@voucherNumber", SqlDbType.VarChar, 10))
            .Parameters("@orderID").Value = orderID
            .Parameters("@voucherNumber").Value = voucherNumber
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("payment.aspx.vb7", "addOrderToVoucher(" & orderID & "," & voucherNumber & ")::" & ex.ToString)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub sendConfirmationEmail(ByVal newOrderID As String, ByVal orderID As Integer, ByVal type As String)
        'siteInclude.debug("sending email from payment.vb")
        Dim toAdd As String = ""
        Dim ccAdd As String = ""
        Dim subject As String = ""
        emailBody = ""
        Dim msg As MailMessage
        Dim chk As CheckBox
        Dim plainView As AlternateView
        Dim htmlView As AlternateView
        Dim d As New siteInclude
        'If LCase(type) = "confirmation" Then setEmailBody(newOrderID, orderID, toAdd, ccAdd, subject)
        'If LCase(type) = "voucher" Then setEmailBodyVoucher(newOrderID, orderID, toAdd, ccAdd, subject)
        If LCase(type) = "confirmation" Then makeEmail("Order Confirmation", newOrderID, orderID, toAdd, ccAdd, subject, Session("EBShopCountry"))
        If LCase(type) = "voucher" Then makeEmail("Voucher Confirmation", newOrderID, orderID, toAdd, ccAdd, subject, Session("EBShopCountry"))
        msg = New MailMessage
        msg.To.Add(toAdd)
        If ccAdd <> "" Then msg.CC.Add(ccAdd)
        msg.From = New MailAddress("noreply@emotionalbliss.com")
        msg.Subject = subject
        msg.IsBodyHtml = True
        plainView = AlternateView.CreateAlternateViewFromString(emailBody, Nothing, "text/plain")
        htmlView = AlternateView.CreateAlternateViewFromString(Replace(emailBody, Chr(10), "<br>"), Nothing, "text/html")
        msg.AlternateViews.Add(plainView)
        msg.AlternateViews.Add(htmlView)
        'Dim client As New SmtpClient
        'client.Send(msg)
        msg.Dispose()
        siteInclude.sendSQLEmail(toAdd, ccAdd, "", subject, "noreply@emotionalbliss.com", "Emotional Bliss", emailBody, siteInclude._emailType.emailHtml)
    End Sub
    Protected Sub makeEmail(ByVal emailType As String, ByVal newOrderID As String, ByVal orderID As Integer, ByRef toAdd As String, ByRef ccAdd As String, ByRef subject As String, ByVal country As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailMasterByCountryCodeTypeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim row As DataRow
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@emailType", SqlDbType.VarChar, 30))
            .Parameters("@countryCode").Value = country
            .Parameters("@emailType").Value = emailType
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)

            If ds.Tables(0).Rows.Count > 0 Then
                row = ds.Tables(0).Rows(0)
                toAdd = txtEmail.Text
                subject = row("emailSubject") 'Set Subject from db
                For Each r As DataRow In ds.Tables(0).Rows
                    'Set email body
                    emailBody = emailBody & r("text")
                Next
                If LCase(emailType) = "order confirmation" Then
                    'Normal order                    
                    'Replace special commands in email body
                    emailBody = Replace(emailBody, "@orderID", newOrderID & UCase(LCase(Session("EBShopCountry"))))
                    emailBody = Replace(emailBody, "@date", FormatDateTime(Now(), DateFormat.ShortDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime))
                    emailBody = Replace(emailBody, "@billingAdd", getAddress(orderID, "bill"))
                    emailBody = Replace(emailBody, "@shippingAdd", getAddress(orderID, "ship"))
                    emailBody = Replace(emailBody, "@items", getItemText())
                    emailBody = Replace(emailBody, "@totalPrice", Profile.EBCart.CurrencySign & FormatNumber(CDec(Profile.EBCart.TotalInc), 2))
                Else
                    'Voucher order
                    'If txtEmail="" AND the order is for a voucher, then use the purchaser's email address from the voucher
                    If LCase(txtEmail.Text) = "noreply@emotionalbliss.com" And purchaserEmail <> "" Then toAdd = purchaserEmail
                    'Replace special commands in email body
                    row = getVoucherDetails(orderID)
                    ccAdd = row("purchaserEmail")
                    toAdd = row("recipientEmail")
                    emailBody = Replace(emailBody, "@recipient", row("recipient"))
                    emailBody = Replace(emailBody, "@purchaser", row("purchaser"))
                    emailBody = Replace(emailBody, "@recipientEmail", row("recipientEmail"))
                    emailBody = Replace(emailBody, "@purchaserEmail", row("purchaserEmail"))
                    emailBody = Replace(emailBody, "@voucherAmount", Profile.EBCart.CurrencySign & row("credit"))
                    emailBody = Replace(emailBody, "@voucherNumber", row("number"))
                    emailBody = Replace(emailBody, "@comment", row("comment"))
                    'Not sure if still needed
                    _voucherNumber = row("number")
                    purchaserEmail = row("purchaserEmail")
                End If
            Else
                'No results were found, this means the email has not been created for the current country yet.
                'Use GB as default
                makeEmail(emailType, newOrderID, orderID, toAdd, ccAdd, subject, "gb")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "makeEmail(orderID=" & orderID & ",emailType=" & emailType & ",countryCode=" & Session("EBShopCountry") & "); " & ex.ToString())
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub setVoucherActive()
        Dim item
        Dim oConn As SqlConnection
        Dim ocmd As SqlCommand
        For Each item In Profile.EBCart.Items
            oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            ocmd = New SqlCommand("procVoucherByCodeActiveUpdate", oConn)
            With ocmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@number", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@active", SqlDbType.Bit))
                .Parameters("@number").Value = item.voucherNumber
                .Parameters("@active").Value = True
            End With
            Try
                If ocmd.Connection.State = 0 Then ocmd.Connection.Open()
                ocmd.ExecuteNonQuery()
            Catch ex As Exception
                Dim si = New siteInclude
                si.addError("shop/payment.aspx", "setVoucherActive(number=" & item.voucherNumber & "); " & ex.ToString)
                si = Nothing
            Finally
                ocmd.Dispose()
                oConn.Dispose()
            End Try
        Next
    End Sub
    Protected Sub updateOrderStatus(ByVal uqID As Integer, ByVal status As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDStatusUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderid", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@distStatus", SqlDbType.VarChar, 20))
            .Parameters("@orderid").Value = uqID
            .Parameters("@status").Value = status
            .Parameters("@distStatus").Value = status
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            'If LCase(status) = "paid" Then
            'Update PaymentDate
            'oCmd = New SqlCommand("procShopOrderByIDPaymentDateUdate", oConn)
            'oCmd.CommandType = CommandType.StoredProcedure
            'oCmd.Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            'oCmd.Parameters("@id").Value = uqID
            'If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            'oCmd.ExecuteNonQuery()
            'End If
        Catch ex As Exception
            siteInclude.addError("shop/payment.aspx", "updateOrderStatus(id=" & uqID & ",status=" & status & "); " & ex.ToString)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub deleteOrder(ByVal uqID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDDelete", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters("@id").Value = uqID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            'Removed by PTs request 3 June 2010. SP
            'Added back by PT 14 June 2010. SP
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            siteInclude.addError("shop/payment.aspx.vb", "deleteOrder(uqID=" & uqID & "); " & ex.ToString)
        End Try
    End Sub
    Protected Sub submitFormToFastpay(ByVal orderID As Integer)
        Dim strResponse As String = ""
        Dim strPost As String = "FPIDENTITY=2069"
        strPost = strPost & "&FPPRICE=" & FormatNumber(Profile.EBCart.TotalInc, 2)
        strPost = strPost & "&FPNAME=" & txtBillName.Text
        strPost = strPost & "&FPREUSE=0"
        strPost = strPost & "&FPMERCHANT=55697"
        strPost = strPost & "&FPCOMPANYNAME=Pear Tree UK"
        Session("EBTmp_FPIDENTITY") = CStr(orderID)
        Session("EBTmp_FPPRICE") = CStr(FormatNumber(Profile.EBCart.TotalInc, 2))
        Session("EBTmp_FPNAME") = txtBillName.Text
        Response.Redirect("paymentFastpay.aspx")
        '** The full transaction registration POST has now been built **
        Dim objUTFEncode As New UTF8Encoding
        Dim arrRequest As Byte()
        Dim objStreamReq As Stream
        Dim objStreamRes As StreamReader
        Dim objHttpRequest As HttpWebRequest
        Dim objHttpResponse As HttpWebResponse
        Dim objUri As New Uri("http://www.fasterpay.co.uk/payfaster.php")
        objHttpRequest = HttpWebRequest.Create(objUri)
        objHttpRequest.KeepAlive = False
        objHttpRequest.Method = "POST"
        objHttpRequest.ContentType = "application/x-www-form-urlencoded"
        arrRequest = objUTFEncode.GetBytes(strPost)
        objHttpRequest.ContentLength = arrRequest.Length
        objStreamReq = objHttpRequest.GetRequestStream()
        objStreamReq.Write(arrRequest, 0, arrRequest.Length)
        objStreamReq.Close()
        'Get response
        objHttpResponse = objHttpRequest.GetResponse()
        objStreamRes = New StreamReader(objHttpResponse.GetResponseStream(), Encoding.ASCII)
        strResponse = objStreamRes.ReadToEnd()
        objStreamRes.Close()
        siteInclude.debug(strResponse)
    End Sub
    Protected Sub loadDBResources()
        'Dim lblPleaseWait As Label = up1.FindControl("lblPleaseWait")
        'lblPleaseWait.Text = 
        Dim errRequired As String = getDBResourceString("errRequired", "global")
        Dim tmp As String = ""
        lblShoppingBasketText.Text = getDBResourceString("lblShoppingBasketText")
        lblHowPay.Text = getDBResourceString("lblHowPay")
        chkCC.Text = getDBResourceString("lblChkCC")
        chkPost.Text = getDBResourceString("lblPost")
        chkIDeal.Text = getDBResourceString("lblIDeal")
        chkAccount.Text = getDBResourceString("lblAccount")
        chkDDebit.Text = trimCrap(getDBResourceString("lblDDebit"))
        chkEDebit.Text = trimCrap(getDBResourceString("lblEDebit"))
        chkGiro.Text = trimCrap(getDBResourceString("lblGiro"))
        chkPaypal.Text = trimCrap(getDBResourceString("lblPaypal"))
        chkFastpay.Text = trimCrap(getDBResourceString("lblFastpay"))
        lblCardDetails.Text = getDBResourceString("lblCardDetails")
        lblCardType.Text = getDBResourceString("lblCardType")
        lblCardNo.Text = getDBResourceString("lblCardNo")
        lblIDealBank.Text = getDBResourceString("lblIDealBank")
        regex1.ErrorMessage = getDBResourceString("errInvalid", "global")
        RequiredFieldValidator4.ErrorMessage = getDBResourceString("errRequired", "global")
        lblStartDate.Text = getDBResourceString("lblStartDate")
        lblSwitchOnly.Text = getDBResourceString("lblSwitchOnly")
        lblEndDate.Text = getDBResourceString("lblEndDate")
        lblIssueNo.Text = getDBResourceString("lblIssueNo")
        lblSwitchOnly2.Text = lblSwitchOnly.Text
        lblCV2.Text = getDBResourceString("lblCV2")
        lblCV2Exp.Text = getDBResourceString("lblCV2Exp")
        reqTxtCV2.ErrorMessage = errRequired
        regTxtCV2.ErrorMessage = getDBResourceString("regTxtCV2")
        lblBillingDetails.Text = getDBResourceString("lblBillingDetails")
        'btnViewShipping.Text = getDBResourceString("btnViewShipping")
        req1.ErrorMessage = errRequired
        req3.ErrorMessage = errRequired
        lblEmail.Text = getDBResourceString("lblEmail")
        regTxtEmail.ErrorMessage = getDBResourceString("regTxtEmail")
        lblHouseNo.Text = getDBResourceString("lblHouseNo")
        reqBillLookupHouse.ErrorMessage = errRequired
        lblPostcode.Text = getDBResourceString("lblPostcode")
        reqBillLookupPostcode.Text = errRequired
        lblAddress.Text = getDBResourceString("lblAddress")
        req2.ErrorMessage = errRequired
        lblCity.Text = getDBResourceString("lblCity")
        lblCity2.Text = getDBResourceString("lblCity")
        reqTxtShipAdd3.ErrorMessage = getDBResourceString("errRequired", "global")
        reqTxtBillAdd3.ErrorMessage = getDBResourceString("errRequired", "global")
        lblPostcode2.Text = lblPostcode.Text
        lblPostcode3.Text = lblPostcode.Text
        lblPostcode4.Text = lblPostcode.Text
        reqBillPostcode.ErrorMessage = errRequired
        reqTxtPhone.ErrorMessage = errRequired
        lblCountry.Text = getDBResourceString("lblCountry")
        lblAccount.Text = getDBResourceString("lblAccountNo")
        lblShippingAddress.Text = getDBResourceString("lblShippingAddress")
        btnViewBilling.Text = getDBResourceString("btnViewBilling")
        lblBillName.Text = getDBResourceString("lblNameOnCard")
        lblPhone.Text = getDBResourceString("lblPhone")
        lblDeliveryInstructions.Text = getDBResourceString("lblDeliveryInstructions")
        lblName.Text = getDBResourceString("lblName")
        RequiredFieldValidator1.ErrorMessage = errRequired
        lblHouseNo2.Text = lblHouseNo.Text
        reqLookupShipHouse.ErrorMessage = errRequired
        reqLookupShipPostcode.ErrorMessage = errRequired
        lblAddress2.Text = lblAddress.Text
        RequiredFieldValidator2.ErrorMessage = errRequired
        reqShipPostcode.ErrorMessage = errRequired
        lblCountry2.Text = lblCountry.Text
        lblDOB.Text = getDBResourceString("lblDOB")
        lblGender.Text = getDBResourceString("lblGender")
        drpGender.Items(0).Text = getDBResourceString("drpSelect", "global")
        drpGender.Items(1).Text = getDBResourceString("drpMale")
        drpGender.Items(2).Text = getDBResourceString("drpFemale")
        drpIDealBank.Items(0).Text = trimCrap(getDBResourceString("drpSelect", "global"))
        reqDrpIDealBank.ErrorMessage = getDBResourceString("errRequired", "global")
        lblDeliverTo.Text = getDBResourceString("lblDeliverTo")
        radYes.Text = trimCrap(getDBResourceString("radYes"))
        radNo.Text = trimCrap(getDBResourceString("radNo"))
        litCardDetails.Text = getDBResourceString("lblCardDetails")
        litDelivery.Text = getDBResouceString("lblBillingDetails")
        litShippingAddress.Text = getDBResouceString("lblShippingAddress")
        tmp = trimCrap(getDBResourceString("lnkSubmitShipBill"))
        If tmp <> "" Then btnSubmitShipBill.Text = tmp
        tmp = trimCrap(getDBResourceString("lnkSubmitBill"))
        If tmp <> "" Then btnSubmitBill.Text = tmp
        'End If
        tmp = getDBResourceString("lnkIcepay")
        'If tmp <> "" Then btnIcePay.Text = tmp
        btnIcePay.Text = trimCrap(tmp)

    End Sub
    Protected Sub setOrderAs3DAuth(ByVal orderID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByID3dAuthUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@3dTransaction", SqlDbType.Bit))
            .Parameters("@id").Value = orderID
            .Parameters("@3dTransaction").Value = True
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx.vb", "setOrderAs3DAuth(orderID= " & orderID & "); " & ex.ToString())
            si = Nothing
        End Try
    End Sub

    'Functions
    Protected Function getAddress(ByVal id As Integer, ByVal addType As String) As String
        Dim add As String = ""
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopCustomerBillAddByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        If addType = "ship" Then oCmd.CommandText = "procShopCustomerShipAddByOrderIDSelect"
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables(0).Rows(0)
                add = row(addType & "Name") & Chr(10)
                If Not IsDBNull(row(addType & "Add1")) Then If row(addType & "Add1") <> "" Then add = add & row(addType & "Add1") & Chr(10)
                If Not IsDBNull(row(addType & "Add2")) Then If row(addType & "Add2") <> "" Then add = add & row(addType & "Add2") & Chr(10)
                If Not IsDBNull(row(addType & "Add3")) Then If row(addType & "Add3") <> "" Then add = add & row(addType & "Add3") & Chr(10)
                If Not IsDBNull(row(addType & "Add4")) Then If row(addType & "Add4") <> "" Then add = add & row(addType & "Add4") & Chr(10)
                If Not IsDBNull(row(addType & "Add5")) Then If row(addType & "Add5") <> "" Then add = add & row(addType & "Add5") & Chr(10)
                If Not IsDBNull(row(addType & "Postcode")) Then add = add & row(addType & "Postcode") & Chr(10)
                If Not IsDBNull(row(addType & "Country")) Then add = add & row(addType & "Country") & Chr(10)
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "getAddress(id=" & id & ", type=" & addType & "); " & ex.ToString)
            si = Nothing
        End Try
        Return add
    End Function
    Protected Function getItemText() As String
        Dim txt As String = "QUANTITY   "
        txt = txt & "PRODUCT(DESCRIPTION)" & Chr(10)
        txt = txt & "--------------------------------------------------------" & Chr(10)
        Dim item
        For Each item In Profile.EBCart.Items
            txt = txt & item.Qty & "          "
            txt = txt & item.Name & Chr(10)
        Next
        txt = txt & "--------------------------------------------------------" & Chr(10)
        Return txt
    End Function
    Protected Function getOrderCurrency(ByVal countryCode As String) As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCountryByCountryCodeCurrencySelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = "gbp"
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = ds.Tables(0).Rows(0)("currencyCode")
        Catch ex As Exception
            result = "gbp"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function take3DPayment(ByVal id As Integer, ByVal chargeType As String) As String
        Dim countryCode As String = Session("EBShopCountry")
        Dim strVendorTxCode As String = "EBPAYMENT" & formatDateElement(Day(Date.Today)) & formatDateElement(Month(Date.Today)) & Year(Date.Today) & Hour(Now()) & Minute(Now()) & Second(Now()) & "_" & getUserOrderID(id) & UCase(countryCode)
        Dim result As String = ""
        'transaction variables
        Dim strBillingAddress As String = ""
        Dim strBillingPostCode As String = ""
        Dim strDeliveryAddress As String = ""
        Dim strDeliveryPostCode As String = ""
        Dim strPageError As String = ""
        Dim strVendorName As String = ""
        Dim strCardHolder As String = ""
        Dim strCardType As String = ""
        Dim strCardNumber As String = ""
        Dim strStartDate As String = ""
        Dim strExpiryDate As String = ""
        Dim strIssueNumber As String = ""
        Dim strCV2 As String = ""
        Dim decTotal As Decimal = 0.0
        Dim strCurrency As String = ""
        Dim strDescription As String = ""
        'Returned variables
        Dim strResponse As String = ""
        Dim strStatusDetail As String = ""
        Dim strVPSTxId As String = ""
        Dim strSecurityKey As String = ""
        Dim strTxAuthNo As String = ""
        Dim strAVSCV2 As String = ""
        Dim strAddressResult As String = ""
        Dim strPostCodeResult As String = ""
        Dim strCV2Result As String = ""
        Dim str3DSecureStatus As String = ""
        Dim strCAVV As String = ""
        Dim strDBStatus As String = ""
        Dim strGiftAid As String = ""
        Dim strCompletionURL As String = ""
        Dim str3DRedirectHTML As String = ""
        'other variables
        Dim strStatus As String = ""
        Dim strPost As String = ""
        Dim strTransactionType As String = ""
        Dim strConnectTo As String = ""
        Dim bDeliverySame As Boolean = False
        '3d specific variables
        Dim strACSURL As String = ""
        Dim strPAReq As String = ""
        Dim strMD As String = ""
        Try
            'Set values
            strConnectTo = "TEST"
            If Not Application("isDevBox") Then
                strConnectTo = "LIVE"
            End If
            strVendorName = "peartree1"
            strTransactionType = chargeType
            strCardHolder = txtBillName.Text
            strCardType = drpCardType.SelectedValue
            If chkPaypal.Checked Then strCardType = "PAYPAL"
            strCardNumber = txtCard.Text
            If drpStartMonth.SelectedValue <> "" And drpStartYear.SelectedValue <> "" Then strStartDate = make2Digit(drpStartMonth.SelectedValue) & Right(drpStartYear.SelectedValue, 2)
            strExpiryDate = make2Digit(drpEndMonth.SelectedValue) & Right(drpEndYear.SelectedValue, 2)
            strIssueNumber = txtIssue.Text
            strCV2 = txtCV2.Text
            decTotal = Profile.EBCart.TotalInc
            strCurrency = getCurrencyCode()
            strGiftAid = "0"
            strDescription = "Emotional Bliss Order"
            strBillingAddress = txtBillAdd1.Text
            strBillingPostCode = txtBillPostcode.Text
            strDeliveryAddress = txtShipAdd1.Text
            strDeliveryPostCode = txtShipPostcode.Text

            'Create post string
            'If LCase(strTransactionType) = "paypal" Then
            strPost = "VPSProtocol=2.23"
            'Else
            'strPost = "VPSProtocol=2.22"
            'End If
            strPost = strPost & "&TxType=" & strTransactionType
            strPost = strPost & "&Vendor=" & strVendorName
            strPost = strPost & "&VendorTxCode=" & strVendorTxCode
            strPost = strPost & "&Amount=" & FormatNumber(decTotal, 2, -1, 0, 0) '** Formatted to 2 decimal places with leading digit but no commas or currency symbols **
            strPost = strPost & "&Currency=" & strCurrency
            strPost = strPost & "&Description=" & URLEncode(strDescription)
            strPost = strPost & "&CardHolder=" & strCardHolder
            strPost = strPost & "&CardNumber=" & strCardNumber
            If Len(strStartDate) > 0 Then
                strPost = strPost & "&StartDate=" & strStartDate
            End If
            strPost = strPost & "&ExpiryDate=" & strExpiryDate
            If Len(strIssueNumber) > 0 Then
                strPost = strPost & "&IssueNumber=" & strIssueNumber
            End If
            strPost = strPost & "&CV2=" & strCV2
            strPost = strPost & "&CardType=" & strCardType
            Dim sCountryCode As String = Session("EBShopCountry")
            strPost = strPost & "&BillingAddress1=" & txtBillAdd1.Text
            strPost = strPost & "&BillingPostCode=" & txtBillPostcode.Text
            strPost = strPost & "&BillingFirstnames=" & getFirstname(txtBillName.Text)
            strPost = strPost & "&BillingSurname=" & getSurname(txtBillName.Text)
            strPost = strPost & "&BillingCity=" & txtBillAdd3.Text
            If LCase(sCountryCode) = "eu" Then
                strPost = strPost & "&BillingCountry=" & drpEUState.SelectedValue
            Else
                strPost = strPost & "&BillingCountry=" & sCountryCode
            End If
            strPost = strPost & "&DeliveryFirstnames=" & getFirstname(txtBillName.Text)
            strPost = strPost & "&DeliverySurname=" & getSurname(txtBillName.Text)
            If LCase(Session("EBShopCountry")) = "us" Then
                strPost = strPost & "&BillingState=" & drpState.SelectedItem.Text
            End If
            If radYes.Checked Then bDeliverySame = True
            If bDeliverySame Then
                strPost = strPost & "&DeliveryAddress1=" & txtBillAdd1.Text
                strPost = strPost & "&DeliveryPostCode=" & txtBillPostcode.Text
                strPost = strPost & "&DeliveryCity=" & txtBillAdd3.Text
                If LCase(sCountryCode) = "eu" Then
                    strPost = strPost & "&DeliveryCountry=" & drpEUState.SelectedValue
                Else
                    strPost = strPost & "&DeliveryCountry=" & sCountryCode
                End If
                If LCase(Session("EBShopCountry")) = "us" Then
                    strPost = strPost & "&DeliveryState=" & drpState.SelectedItem.Text
                    'txtBillAdd4.Text = drpState.SelectedItem.Text
                End If
            Else
                strPost = strPost & "&DeliveryAddress1=" & txtShipAdd1.Text
                strPost = strPost & "&DeliveryPostCode=" & txtShipPostcode.Text
                strPost = strPost & "&DeliveryCity=" & txtShipAdd3.Text
                If LCase(sCountryCode) = "eu" Then
                    strPost = strPost & "&DeliveryCountry=" & drpEUState2.SelectedValue
                Else
                    strPost = strPost & "&DeliveryCountry=" & sCountryCode
                End If
                If LCase(Session("EBShopCountry")) = "us" Then
                    strPost = strPost & "&DeliveryState=" & drpState2.SelectedItem.Text
                    'txtShipAdd4.Text = drpState2.SelectedItem.Text
                End If
            End If

            strPost = strPost & "&GiftAidPayment=" & strGiftAid
            If strTransactionType <> "AUTHENTICATE" Then strPost = strPost & "&ApplyAVSCV2=0"
            strPost = strPost & "&ClientIPAddress=" & Request.ServerVariables("REMOTE_HOST")
            strPost = strPost & "&Apply3DSecure=0"
            strPost = strPost & "&AccountType=E"
            If chkPaypal.Checked Then strPost = strPost & "&PayPalCallBackURL=" & URLEncode("https://secure.emotionalbliss.com/shop/PayPalCallBack.aspx")
            siteInclude.debug(strPost)
            '** The full transaction registration POST has now been built **
            Dim objUTFEncode As New UTF8Encoding
            Dim arrRequest As Byte()
            Dim objStreamReq As Stream
            Dim objStreamRes As StreamReader
            Dim objHttpRequest As HttpWebRequest
            Dim objHttpResponse As HttpWebResponse
            Dim objUri As New Uri(siteInclude.getSagePayURL(strConnectTo, "purchase"))
            objHttpRequest = HttpWebRequest.Create(objUri)
            objHttpRequest.KeepAlive = False
            objHttpRequest.Method = "POST"
            objHttpRequest.ContentType = "application/x-www-form-urlencoded"
            arrRequest = objUTFEncode.GetBytes(strPost)
            objHttpRequest.ContentLength = arrRequest.Length
            objStreamReq = objHttpRequest.GetRequestStream()
            objStreamReq.Write(arrRequest, 0, arrRequest.Length)
            objStreamReq.Close()
            'Get response
            objHttpResponse = objHttpRequest.GetResponse()
            objStreamRes = New StreamReader(objHttpResponse.GetResponseStream(), Encoding.ASCII)
            strResponse = objStreamRes.ReadToEnd()
            objStreamRes.Close()
            If Err.Number <> 0 Then
                'Check for the most common error... unable to reach the purchase URL
                result = "error"
                If Err.Number = -2147012889 Then
                    strPageError = "Your server was unable to register this transaction with Protx." &
                    "  Check that you do not have a firewall restricting the POST and " &
                    "that your server can correctly resolve the address " & siteInclude.getSagePayURL(strConnectTo, "puchase")
                Else
                    strPageError = "An Error has occurred whilst trying to register this transaction.<BR>" &
                    "The Error Number is: " & Err.Number & "<BR>" &
                    "The Description given is: " & Err.Description
                End If
                Dim si As New siteInclude
                si.addError("shop/payment.asp.vb", "take3DPayment(id=" & id & "); " & strPageError)
                si = Nothing
                strPageError = "An error occured while trying to contant your bank.<br>We appologies for any inconvenience. Please try again later.<br>Your card has not been charged and the order is incomplete."
            Else
                '** No transport level errors, so the message got the Protx **
                '** Analyse the response from VSP Direct to check that everything is okay **
                '** Registration results come back in the Status and StatusDetail fields **
                strStatus = "OK" 'findField("Status", strResponse)
                strStatusDetail = findField("StatusDetail", strResponse)
                'lblStatus.Text = strStatus
                'lblStatusDetail.Text = strStatusDetail
                siteInclude.debug("STATUS=" & strStatus)
                'Add details to database

                'response.write("'" & strStatus & "'")
                If strStatus = "3DAUTH" Then
                    '** This is a 3D-Secure transaction, so we need to redirect the customer to their bank **
                    '** for authentication.  First get the pertinent information from the response **
                    strMD = findField("MD", strResponse)
                    strACSURL = findField("ACSURL", strResponse)
                    strPAReq = findField("PAReq", strResponse)
                    'strPageState = "3DRedirect"
                    'lblMD.Text = strMD
                    'lblACSUrl.Text = strACSURL
                    'lblPAReq.Text = strPAReq
                    Session("MD") = strMD
                    Session("PAReq") = strPAReq
                    Session("ACSURL") = strACSURL
                    Session("VendorTxCode") = strVendorTxCode
                    Session("GiftAid") = strGiftAid
                    Session("OrderTotal") = decTotal
                    'Session("EBTmpOrderID") = id
                    Session("EBShopCountry") = countryCode
                    Session("Currency") = strCurrency
                    panCard.Visible = False
                    panBill.Visible = False
                    panShip.Visible = False
                    pan3DSecure.Visible = True
                    result = "3dauth"
                ElseIf LCase(strStatus) = "ppredirect" Then
                    'Add returned values to the paypal table then redirect to the supplied URL
                    Try
                        Dim param() As String = {"@orderID", "@vpstxid", "@statusDetail", "@status", "@vpsprotocol", "@paypalRedirectURL", "@amount", "@vendorTxCode"}
                        Dim paramValue() As String = {id.ToString, findField("VPSTxId", strResponse), strStatusDetail, strStatus, findField("VPSProtocol", strResponse), findField("PayPalRedirectURL", strResponse), decTotal, strVendorTxCode}
                        Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal, SqlDbType.VarChar}
                        Dim paramSize() As Integer = {0, 40, 255, 15, 4, 255, 0, 40}
                        siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procPaypalInsert")
                    Catch ex As Exception
                        siteInclude.addError("payment.asp.vb", "take3DPayment(paypal)" & ex.ToString)
                    Finally
                    End Try
                    Response.Redirect(findField("PayPalRedirectURL", strResponse))
                Else
                    '** If this isn't 3D-Auth, then this is an authorisation result (either successful or otherwise) **
                    '** Get the results form the POST if they are there **
                    strVPSTxId = findField("VPSTxId", strResponse)
                    strSecurityKey = findField("SecurityKey", strResponse)
                    strTxAuthNo = findField("TxAuthNo", strResponse)
                    strAVSCV2 = findField("AVSCV2", strResponse)
                    strAddressResult = findField("AddressResult", strResponse)
                    strPostCodeResult = findField("PostCodeResult", strResponse)
                    strCV2Result = findField("CV2Result", strResponse)
                    str3DSecureStatus = findField("3DSecureStatus", strResponse)
                    strCAVV = findField("CAVV", strResponse)

                    '** Great, the signatures DO match, so we can update the database and redirect the user appropriately **
                    Dim bErrorLogged As Boolean = False
                    If strStatus = "OK" Then
                        strDBStatus = "AUTHORISED - The transaction was successfully authorised with the bank."
                        siteInclude.AddToOrderLog(id, strDBStatus, "Payment System", True)
                        bErrorLogged = True
                    ElseIf strStatus = "MALFORMED" Then
                        strDBStatus = "MALFORMED - " & SQLSafe(Left(strStatusDetail, 255))
                    ElseIf strStatus = "INVALID" Then
                        strDBStatus = "INVALID - " & SQLSafe(Left(strStatusDetail, 255))
                    ElseIf strStatus = "NOTAUTHED" Then
                        strDBStatus = "DECLINED - The transaction was not authorised by the bank."
                    ElseIf strStatus = "REJECTED" Then
                        strDBStatus = "REJECTED - The transaction failed the 3D-Secure or AVS/CV2 rule-bases."
                    ElseIf strStatus = "AUTHENTICATED" Then
                        strDBStatus = "AUTHENTICATED - The transaction was successfully 3D-Secure Authenticated and can now be Authorised."
                    ElseIf strStatus = "REGISTERED" Then
                        strDBStatus = "REGISTERED - The transaction could not be 3D-Secure Authenticated, but has been registered to be Authorised."
                    ElseIf strStatus = "ERROR" Then
                        strDBStatus = "ERROR - There was an error during the payment process.  The error details are: " & SQLSafe(strStatusDetail)
                    Else
                        strDBStatus = "UNKNOWN - An unknown status was returned from Protx.  The Status was: " & SQLSafe(strStatus) & ", with StatusDetail:" & SQLSafe(strStatusDetail)
                        Dim si As New siteInclude
                        si.AddToOrderLog(id, "Protx Error: " & strDBStatus & "; txID=" & strVendorTxCode, "Payment System", True)
                        si = Nothing
                        bErrorLogged = True
                        strDBStatus = "An Unknown error occured.<br>We appologies for any inconvenience. Please try again later.<br>Your card has not been charged and the order is incomplete."
                    End If
                    result = strStatus
                    If Not bErrorLogged Then
                        'Add error to order log - Unless it was already added (Only happens if its an Unknown Protx error)
                        'Dim si As New siteInclude
                        siteInclude.AddToOrderLog(id, "Protx Error: " & strDBStatus & "; txID=" & strVendorTxCode, "Payment System", True)
                        'si = Nothing
                    End If
                    '** Update our database with the results from the Notification POST **
                    Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                    Dim oCmd As New SqlCommand("procProtxInsert", oConn)
                    Try
                        oCmd.CommandType = CommandType.StoredProcedure
                        oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 50))
                        oCmd.Parameters("@vendorTxCode").Value = strVendorTxCode
                        oCmd.Parameters.Add(New SqlParameter("@VPSProtocol", SqlDbType.VarChar, 10))
                        oCmd.Parameters("@VPSProtocol").Value = Convert.ToString(findField("VPSProtocol", strResponse))
                        oCmd.Parameters.Add(New SqlParameter("@VPSTxID", SqlDbType.VarChar, 40))
                        oCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                        oCmd.Parameters("@status").Value = strStatus
                        oCmd.Parameters.Add(New SqlParameter("@statusDetail", SqlDbType.VarChar, 255))
                        oCmd.Parameters("@statusDetail").Value = strStatusDetail
                        oCmd.Parameters.Add(New SqlParameter("@txAuthNo", SqlDbType.VarChar, 20))
                        oCmd.Parameters.Add(New SqlParameter("@securityKey", SqlDbType.VarChar, 20))
                        oCmd.Parameters.Add(New SqlParameter("@AVSCV2", SqlDbType.VarChar, 50))

                        oCmd.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money))
                        oCmd.Parameters("@amount").Value = decTotal
                        oCmd.Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                        oCmd.Parameters("@orderID").Value = id
                        oCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                        oCmd.Parameters("@currency").Value = UCase(strCurrency)
                        oCmd.Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                        oCmd.Parameters("@country").Value = countryCode
                        oCmd.Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
                        oCmd.Parameters("@bank").Value = Convert.ToDecimal("1.0")
                        oCmd.Parameters.Add(New SqlParameter("@newOrderStatus", SqlDbType.VarChar, 10))
                        'Changed 12/10/2011 sp
                        'oCmd.Parameters("@newOrderStatus").Value = "deferred"
                        'This line commited by shailesh parmar date on 31122020
                        'oCmd.Parameters("@newOrderStatus").Value = "paid"
                        oCmd.Parameters("@newOrderStatus").Value = "Failed"
                        oCmd.Parameters.Add(New SqlParameter("@extraPayment", SqlDbType.Bit))
                        oCmd.Parameters("@extraPayment").Value = False
                        If LCase(strStatus) = "ok" Then
                            oCmd.Parameters("@VPSTxID").Value = strVPSTxId
                            oCmd.Parameters("@txAuthNo").Value = strTxAuthNo
                            oCmd.Parameters("@securityKey").Value = strSecurityKey
                            oCmd.Parameters("@AVSCV2").Value = strCV2Result
                        Else
                            oCmd.Parameters("@VPSTxID").Value = ""
                            oCmd.Parameters("@txAuthNo").Value = ""
                            oCmd.Parameters("@securityKey").Value = ""
                            oCmd.Parameters("@AVSCV2").Value = ""
                        End If
                    Catch ex As Exception
                        lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details.</font>"
                        Dim si As New siteInclude
                        si.addError("shop/payment.aspx", "takePayment_liveAuthAddParam(id=" & id & "); " & ex.ToString)
                        si = Nothing
                    End Try
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details.</font>"
                        Dim si As New siteInclude
                        si.addError("shop/payment.aspx", "takePayment_liveAuthExSql(id=" & id & "); " & ex.ToString)
                        si = Nothing
                    Finally
                        oCmd.Dispose()
                        oConn.Dispose()
                    End Try

                    '** Work out where to send the customer **
                    Session("VendorTxCode") = strVendorTxCode
                    If strStatus = "OK" Or strStatus = "AUTHENTICATED" Or strStatus = "REGISTERED" Then
                        'strCompletionURL = "orderSuccessful.aspx"
                        lblError.Text = "ALL OK"
                        'response.redirect("receipt.aspx")
                    Else
                        'strCompletionURL = "orderFailed.aspx"

                    End If

                    strPageError = strDBStatus

                    '** Finally, if we're in LIVE then go stright to the success page **
                    '** In other modes, we allow this page to display and ask for Proceed to be clicked **
                    If strConnectTo = "LIVE" Then
                        'Response.Redirect(strCompletionURL)
                        'Response.End()
                    Else
                        'lblCompletionURL.Text = strCompletionURL

                    End If
                End If
            End If
            If Len(strPageError) > 0 Then
                lblError.Text = strPageError
                radYes.Checked = False
                radNo.Checked = False
                If LCase(txtEmail.Text) = "noreply@emotionalbliss.com" Then txtEmail.Text = ""
                'Dim cs As ClientScriptManager = Page.ClientScript
                'cs.RegisterStartupScript(Me.GetType(), "loade", "self.setTimeout(""alert('bugger');"",200);", True)
                ScriptManager.RegisterStartupScript(lblError, Me.GetType, "onloader", "self.setTimeout(""scroll(0,400);"",200);", True)
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.asp.vb", "take3DPayment_b(id=" & id & "); " & ex.ToString())
            si = Nothing
        End Try
        Return LCase(result)
    End Function
    Protected Function takePayment(ByVal id As Integer) As Boolean
        Dim vendorTxCode As String = "EBPAYMENT" & formatDateElement(Day(Date.Today)) & formatDateElement(Month(Date.Today)) & Year(Date.Today) & Hour(Now()) & Minute(Now()) & Second(Now()) & CStr(id)
        Dim tx As New Protx.Vsp.DirectTransaction(vendorTxCode, Protx.Vsp.VspTransactionType.Payment)
        Dim sStatus As String
        Dim countryCode As String
        Try
            countryCode = Session("EBShopCountry")
            'tx.Amount = Decimal.Parse(rs("ordertotal"))
            tx.Amount = Profile.EBCart.TotalInc
            tx.Currency = getCurrencyCode()
            tx.Description = "EmotionalBliss Order"
            tx.CardHolder = txtBillName.Text
            'tx.CardNumber = fes.DecryptData(ConfigurationManager.AppSettings("ccKey").ToString, rs("ccEnc"))
            'tx.CardNumber = "4747474747474747"
            tx.CardNumber = txtCard.Text
            'tx.ExpiryDate = formatDateElement((rs("ccExpMonth"))) & Right(rs("ccExpYear"), 2)
            tx.ExpiryDate = make2Digit(drpEndMonth.SelectedValue) & Right(drpEndYear.SelectedValue, 2)

            If drpStartMonth.SelectedValue <> "" And drpStartYear.SelectedValue <> "" Then tx.StartDate = make2Digit(drpStartMonth.SelectedValue) & Right(make2Digit(drpStartYear.SelectedValue), 2)

            If txtIssue.Text <> "" Then tx.IssueNumber = txtIssue.Text

            If txtCV2.Text <> "" Then tx.CV2 = txtCV2.Text

            tx.CardType = CType(System.Enum.Parse(GetType(Protx.Vsp.VspCardType), drpCardType.SelectedValue, False), Protx.Vsp.VspCardType)

            tx.BillingAddress = txtBillAdd1.Text
            tx.BillingPostCode = txtBillPostcode.Text

            tx.CustomerName = txtBillName.Text
            tx.CustomerEMail = txtEmail.Text
            tx.GiftAidPayment = False
            tx.ApplyAvsCv2 = CType(System.Int32.Parse(0), Protx.Vsp.ApplyChecksFlag)
            tx.ClientIPAddress = Request.UserHostAddress
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details.</font>"
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "takePayment(id=" & id & "); " & ex.ToString)
            si = Nothing
        End Try

        Dim vspResponse As Protx.Vsp.DirectResponse = tx.Send()
        Dim sHTML As String = ""

        'Dim lblCCResults As Label = FV.FindControl("lblCCResults")
        Dim bSuccess As Boolean = False
        Dim userName As String = "Live Auth"
        Try
            If (Protx.Vsp.VspStatusType.OK = vspResponse.Status) Then
                sStatus = "Paid"
                bSuccess = True
                'Add transaction result to orderLog
                Dim si As New siteInclude
                si.AddToOrderLog(id, "Card Successfully Charged, txID=" & vendorTxCode, userName, True)
                si = Nothing
            Else
                sStatus = "Failed"
                bSuccess = False
                lblError.Text = "<br><font color='red'>We were unable to charge your card, your bank replied with:<br>" & vspResponse.StatusDetail & "<br>Please check your details, make any nessessary amendments, then click on 'Complete Order' to try again.</font>"
                Dim si As New siteInclude
                si.AddToOrderLog(id, "Transaction attempted, txID=" & vendorTxCode & ".<br>Error Message:" & vspResponse.StatusDetail, userName, True)
                si = Nothing
            End If
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details.</font>"
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "takePayment_liveAuth(id=" & id & "); " & ex.ToString)
            si = Nothing
        End Try
        'Add to db
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProtxInsert", oConn)
        Try
            oCmd.CommandType = CommandType.StoredProcedure
            oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 50))
            oCmd.Parameters("@vendorTxCode").Value = vendorTxCode
            oCmd.Parameters.Add(New SqlParameter("@VPSProtocol", SqlDbType.VarChar, 10))
            oCmd.Parameters("@VPSProtocol").Value = Convert.ToString(vspResponse.VPSProtocol)
            oCmd.Parameters.Add(New SqlParameter("@VPSTxID", SqlDbType.VarChar, 40))
            oCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            oCmd.Parameters("@status").Value = vspResponse.Status
            oCmd.Parameters.Add(New SqlParameter("@statusDetail", SqlDbType.VarChar, 255))
            oCmd.Parameters("@statusDetail").Value = vspResponse.StatusDetail
            oCmd.Parameters.Add(New SqlParameter("@txAuthNo", SqlDbType.VarChar, 20))
            oCmd.Parameters.Add(New SqlParameter("@securityKey", SqlDbType.VarChar, 20))
            oCmd.Parameters.Add(New SqlParameter("@AVSCV2", SqlDbType.VarChar, 50))

            oCmd.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money))
            oCmd.Parameters("@amount").Value = tx.Amount
            oCmd.Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            oCmd.Parameters("@orderID").Value = id
            oCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
            oCmd.Parameters("@currency").Value = tx.Currency
            oCmd.Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
            oCmd.Parameters("@country").Value = countryCode
            oCmd.Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
            oCmd.Parameters("@bank").Value = Convert.ToDecimal("1.0")
            oCmd.Parameters.Add(New SqlParameter("@newOrderStatus", SqlDbType.VarChar, 10))
            oCmd.Parameters("@newOrderStatus").Value = sStatus
            oCmd.Parameters.Add(New SqlParameter("@extraPayment", SqlDbType.Bit))
            oCmd.Parameters("@extraPayment").Value = False
            '27/7/10 Seems to be entering empty values for these 4 parameters. Changed the IF command, and set params to "" in advance to avoid any errors.
            oCmd.Parameters("@VPSTxID").Value = ""
            oCmd.Parameters("@txAuthNo").Value = ""
            oCmd.Parameters("@securityKey").Value = ""
            oCmd.Parameters("@AVSCV2").Value = ""
            'If Protx.Vsp.VspStatusType.OK = vspResponse.Status Then
            If bSuccess Then
                oCmd.Parameters("@VPSTxID").Value = vspResponse.VPSTxID
                oCmd.Parameters("@txAuthNo").Value = vspResponse.TxAuthNo
                oCmd.Parameters("@securityKey").Value = vspResponse.SecurityKey
                oCmd.Parameters("@AVSCV2").Value = vspResponse.Cv2ResultText
            End If
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details.</font>"
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "takePayment_liveAuthAddParam(id=" & id & "); " & ex.ToString)
            si = Nothing
        End Try
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details.</font>"
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "takePayment_liveAuthExSql(id=" & id & "); " & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
            'Clear tx
            tx = Nothing
        End Try
        If Not bSuccess Then
            'Show the Complete Order button again.
            If _buttonClicked = "bill" Then
                tblButtons.Visible = True
                btnSubmitBill.Visible = True
            Else
                tblButtons.Visible = True
                btnSubmitShipBill.Visible = True
            End If
            'Hide Please Wait message
            ScriptManager.RegisterStartupScript(panShip, Me.GetType, "onloader", "self.setTimeout(""showPleaseWaitMsg('none')"",200);", True)
        End If
        Return bSuccess
    End Function
    Protected Function takePaymentOld(ByVal id As Integer) As Boolean
        'Live auth is not going to be used
        Dim result As Boolean = False
        If False Then

            'Takes payment and returns success or fail (true or false)
            Dim sStatus As String = ""
            Dim vendorTxCode As String = "EBPAYMENT" & formatDateElement(Day(Date.Today)) & formatDateElement(Month(Date.Today)) & Year(Date.Today) & Hour(Now()) & Minute(Now()) & id
            Dim tx As New Protx.Vsp.DirectTransaction(vendorTxCode, Protx.Vsp.VspTransactionType.Payment)
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procShopCustomerPaymentByOrderIDSelect", oConn)
            oCmd.CommandType = CommandType.StoredProcedure
            oCmd.Parameters.Add(New SqlParameter("@orderid", SqlDbType.Int))
            oCmd.Parameters("@orderid").Value = id
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            Dim da As New SqlDataAdapter(oCmd)
            Dim ds As New DataSet
            da.Fill(ds)
            Dim rs As DataRow
            Dim countryCode As String
            'Dim fes As New FE_SymmetricNamespace.FE_Symmetric
            Try
                If ds.Tables(0).Rows.Count > 0 Then
                    rs = ds.Tables(0).Rows(0)
                    countryCode = rs("orderCountryCode")
                    tx.Amount = CType(FormatNumber(rs("ordertotal"), 2), Decimal)
                    tx.Currency = UCase(rs("orderCurrency")) 'Valid amounts are GBP,EUR,USD
                    tx.Description = "EmotionalBliss Order"
                    tx.CardHolder = rs("billName")
                    tx.CardNumber = rs("cardNo")
                    tx.ExpiryDate = rs("cardExp")
                    If Not IsDBNull(rs("cardStart")) Then If rs("cardStart") <> "" Then tx.StartDate = rs("cardStart")
                    If Not IsDBNull(rs("cardIssue")) Then If rs("cardIssue") <> "" Then tx.IssueNumber = rs("cardIssue")
                    If Not IsDBNull(rs("cardCV2")) Then If rs("cardCV2") <> "" Then tx.CV2 = rs("cardCV2")
                    tx.CardType = CType(System.Enum.Parse(GetType(Protx.Vsp.VspCardType), rs("cardType"), False), Protx.Vsp.VspCardType)
                    tx.BillingAddress = rs("billAdd1")
                    tx.BillingPostCode = rs("billPostcode")
                    tx.CustomerName = rs("billName")
                    tx.CustomerEMail = rs("email")
                    tx.GiftAidPayment = False
                    tx.ApplyAvsCv2 = CType(System.Int32.Parse(0), Protx.Vsp.ApplyChecksFlag)
                    tx.ClientIPAddress = Request.UserHostAddress
                End If
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            Finally
                da.Dispose()
                ds.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try

            Dim vspResponse As Protx.Vsp.DirectResponse = tx.Send()
            Dim sHTML As String = ""
            Dim SI As New siteInclude
            Try

                If (Protx.Vsp.VspStatusType.OK = vspResponse.Status) Then
                    result = True
                    sStatus = "Paid"
                    'Add transaction result to orderLog
                    SI.AddToOrderLog(id, "Card Successfully Charged, txID=" & vendorTxCode, "System", True)
                Else
                    sStatus = "Failed"
                    result = False
                    SI.AddToOrderLog(id, "Transaction attempted, txID=" & vendorTxCode & ".<br>Error Message:" & vspResponse.StatusDetail, "System", True)
                End If
            Catch ex As Exception
                'Protx.Vsp.VspException
                Response.Write(ex)
                Response.End()
            End Try
            'Add to db
            Try
                oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                oCmd = New SqlCommand("procProtxInsert", oConn)
                oCmd.CommandType = CommandType.StoredProcedure
                oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 50))
                oCmd.Parameters("@vendorTxCode").Value = vendorTxCode
                oCmd.Parameters.Add(New SqlParameter("@VPSProtocol", SqlDbType.VarChar, 10))
                oCmd.Parameters("@VPSProtocol").Value = Convert.ToString(vspResponse.VPSProtocol)
                oCmd.Parameters.Add(New SqlParameter("@VPSTxID", SqlDbType.VarChar, 40))
                oCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                oCmd.Parameters("@status").Value = vspResponse.Status
                oCmd.Parameters.Add(New SqlParameter("@statusDetail", SqlDbType.VarChar, 255))
                oCmd.Parameters("@statusDetail").Value = vspResponse.StatusDetail
                oCmd.Parameters.Add(New SqlParameter("@txAuthNo", SqlDbType.VarChar, 20))
                oCmd.Parameters.Add(New SqlParameter("@securityKey", SqlDbType.VarChar, 20))
                oCmd.Parameters.Add(New SqlParameter("@AVSCV2", SqlDbType.VarChar, 50))

                oCmd.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money))
                oCmd.Parameters("@amount").Value = tx.Amount
                oCmd.Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                oCmd.Parameters("@orderID").Value = id
                oCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                oCmd.Parameters("@currency").Value = tx.Currency
                oCmd.Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                oCmd.Parameters("@country").Value = Session("EBShopCountry")
                oCmd.Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
                oCmd.Parameters("@bank").Value = Convert.ToDecimal("1.0")
                oCmd.Parameters.Add(New SqlParameter("@newOrderStatus", SqlDbType.VarChar, 10))
                oCmd.Parameters("@newOrderStatus").Value = sStatus
                If Protx.Vsp.VspStatusType.OK = vspResponse.Status Then
                    oCmd.Parameters("@VPSTxID").Value = vspResponse.VPSTxID
                    oCmd.Parameters("@txAuthNo").Value = vspResponse.TxAuthNo
                    oCmd.Parameters("@securityKey").Value = vspResponse.SecurityKey
                    oCmd.Parameters("@AVSCV2").Value = vspResponse.Cv2ResultText
                Else
                    oCmd.Parameters("@VPSTxID").Value = ""
                    oCmd.Parameters("@txAuthNo").Value = ""
                    oCmd.Parameters("@securityKey").Value = ""
                    oCmd.Parameters("@AVSCV2").Value = ""
                End If
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            End Try
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            Finally
                oCmd.Dispose()
                oConn.Dispose()
                'Clear tx
                tx = Nothing
            End Try
        End If
        Return result
    End Function
    Function formatDateElement(ByVal d As String)
        Dim sResult As String = d
        If Len(d) = 1 Then
            sResult = "0" & d
        End If
        Return sResult
    End Function
    Protected Function encryptCard(ByVal n As String) As String
        Dim fes As New FE_SymmetricNamespace.FE_Symmetric
        Dim enc As String = fes.EncryptData(ConfigurationManager.AppSettings("aesKey").ToString, n)
        Return enc
    End Function
    Protected Function getHidCardNo(ByVal ccNum As String) As String
        Dim result As String = ""
        If Len(ccNum) > 4 Then
            For iLoop As Integer = 1 To Len(ccNum) - 4
                result = result & "*"
            Next
            result = result & Right(ccNum, 4)
        End If
        Return result
    End Function
    Protected Function getCart(ByVal sid As Integer) As Object
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEBCartByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim oCart As EBCart = Nothing
        Dim stream As New MemoryStream
        Dim bFormatter As New BinaryFormatter
        Dim bDataReturnedOK As Boolean = False
        Dim sCountry As String = ""
        Dim sLanguage As String = ""
        Dim sAffClickThrough As Integer
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters("@id").Value = sid
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                stream = New MemoryStream(CType(ds.Tables(0).Rows(0)("data"), Byte()))
                oCart = CType(bFormatter.Deserialize(stream), EBCart)
                sLanguage = ds.Tables(0).Rows(0)("EBLanguage")
                sCountry = ds.Tables(0).Rows(0)("EBShopCountry")
                sAffClickThrough = ds.Tables(0).Rows(0)("EBAffClickThroughID")
                bDataReturnedOK = True
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx.vb", "getCart(" & sid & ");" & ex.ToString)
            si = Nothing
        Finally
            Try
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
                bFormatter = Nothing
                stream.Close()
                stream.Dispose()
            Catch ex2 As Exception
            End Try
        End Try
        If bDataReturnedOK Then
            'Cart grabbed from DB, set session variables
            Session("EBsID") = Request.QueryString("sid")
            Session("EBLanguage") = sLanguage
            Session("EBShopCountry") = sCountry
            If sAffClickThrough <> 0 Then Session("EBAffClickThroughID") = CStr(sAffClickThrough)
        End If
        Return oCart
    End Function
    Protected Function voucherOnlyPurchase() As Boolean
        Dim item
        Dim bVoucherPurchase As Boolean = False
        For Each item In Profile.EBCart.Items
            If InStr(LCase(item.ProductCode), "v_") Then bVoucherPurchase = True
        Next
        Return bVoucherPurchase
    End Function
    Protected Function getCurrencyCode() As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCurrencyByCounrtyCodeSelect2", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = "GBP"
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = Session("EBShopCountry")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = ds.Tables(0).Rows(0)("currencyCode")
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx.vb", "getCurrencyCode(langauge=" & Session("EBShopCountry") & "); " & ex.ToString)
            si = Nothing
        End Try
        Return result
    End Function
    Function getVoucherDetails(ByVal orderID As Integer) As DataRow
        Dim row As DataRow = Nothing
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procVoucherByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = orderID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then row = ds.Tables(0).Rows(0)
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "getVoucherDetails(orderID=" & orderID & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return row
    End Function
    Protected Function consultancyInBasket() As Boolean
        Dim result As Boolean = False
        Dim item
        For Each item In Profile.EBCart.Items
            If LCase(Left(item.ProductCode, 2)) = "vc" Then result = True
        Next
        Return result
    End Function
    Protected Function commitOrder(ByVal customerID As Integer, ByVal orderPrefix As String, ByVal orderType As String, ByRef uid As Integer) As Integer
        Dim orderID As String
        Dim ID As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderInsert", oConn)
        If drpEUState.Visible Then
            oCmd = New SqlCommand("procShopOrderInsertWEU", oConn)
            'siteInclude.debug("drpEU was visible, SP changed to procShopOrderInsertWEU")
        End If

        Dim goodsVatRate As Decimal = 0
        Dim newOrderID As Integer = 0
        Dim shipping As Decimal = Profile.EBCart.Shipping
        Dim shippingVatRate As Decimal = Profile.EBCart.ShippingVatRate
        Dim shippingTotal As Decimal = Profile.EBCart.ShippingTotal
        Dim discount As Decimal = Profile.EBCart.VoucherCredit
        Dim discountVat As Decimal = 0
        Dim distributorID As Integer
        Dim clickID As Integer = 0
        Dim bError As Boolean = False
        Dim orderSource As String = "shopper"
        Dim paymentMethod As String = ""
        Select Case LCase(orderType)
            Case "account"
                paymentMethod = "bankaccount"
            Case "cc"
                paymentMethod = "cc"
            Case "cheque"
                paymentMethod = "cheque"
            Case "ideal"
                paymentMethod = "ideal"
            Case "ddebit"
                paymentMethod = "ddebit"
            Case "paypal"
                paymentMethod = "paypal"
            Case "fastpay"
                paymentMethod = "fastpay"
        End Select
        Try
            If CStr(Session("EBAffClickThroughID")) <> "" Then clickID = CInt(Session("EBAffClickThroughID"))
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "Error setting AffClickTroughtID to '" & Session("EBAffClickThroughID") & "'; " & ex.ToString)
        End Try
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderType", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@goods", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@goodsVat", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shipping", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingVatRate", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingTotal", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderTotal", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderCountryCode", SqlDbType.VarChar, 5))
            If drpEUState.Visible Then .Parameters.Add(New SqlParameter("@EUCountryCode", SqlDbType.VarChar, 5))
            'If drpEUState.Visible Then .Parameters.Add(New SqlParameter("@EUCountryCodeID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@customerID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@orderPrefix", SqlDbType.VarChar, 3))
            .Parameters.Add(New SqlParameter("@clickThroughID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@discount", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@discountVat", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderSource", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@paymentMethod", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.VarChar, 15))
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@orderType").Value = orderType
            .Parameters("@goods").Value = Profile.EBCart.TotalEx
            .Parameters("@goodsVat").Value = If(Profile.EBCart.CouponVat > 0, Profile.EBCart.CouponVat, Profile.EBCart.GoodsVat)
            .Parameters("@shipping").Value = shipping
            .Parameters("@shippingVatRate").Value = shippingVatRate
            .Parameters("@shippingTotal").Value = shippingTotal
            .Parameters("@orderTotal").Value = Profile.EBCart.TotalInc
            .Parameters("@orderCountryCode").Value = Session("EBShopCountry")
            If drpEUState.Visible Then .Parameters("@EUCountryCode").Value = drpEUState.SelectedValue
            'If drpEUState.Visible Then .Parameters("@EUCountryCodeID").Value = drpEUState.SelectedValue
            .Parameters("@customerID").Value = customerID
            .Parameters("@orderPrefix").Value = orderPrefix
            .Parameters("@clickThroughID").Value = clickID
            .Parameters("@currency").Value = getOrderCurrency(Session("EBShopCountry"))
            .Parameters("@discount").Value = discount
            .Parameters("@discountVat").Value = discountVat
            .Parameters("@orderSource").Value = orderSource
            .Parameters("@paymentMethod").Value = paymentMethod
            .Parameters("@newOrderID").Direction = ParameterDirection.Output
            .Parameters("@ID").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            orderID = oCmd.Parameters("@newOrderID").Value
            ID = oCmd.Parameters("@ID").Value
            uid = ID
            newOrderID = Left(oCmd.Parameters("@newOrderID").Value, Len(oCmd.Parameters("@newOrderID").Value) - 2)
        Catch ex As Exception
            Try
                Dim si As New siteInclude
                bError = True
                si.addError("shop/payment.aspx", "Error adding order in commitOrder(s):" & ex.ToString)
                si = Nothing
                tblButtons.Visible = False
                btnSubmitBill.Visible = False
                btnSubmitShipBill.Visible = False
                lblError.Text = "<b><font color='red'>There has been an error processing your details.  Your order cannot be completed at this time.</font></b>"
            Catch e As Exception
            End Try
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            Dim si As New siteInclude
            'Add to distributors statement
            distributorID = getDistID(Session("EBShopCountry"))
            If distributorID > 0 Then

                'si.affAddToStatement(distributorID, calcOrderTotal(Profile.EBCart.TotalInc, shipping, shippingVatRate), 0, ID, 0, 4)
                si.affAddToStatement(distributorID, Profile.EBCart.TotalInc, 0, ID, 0, 4)
                si = Nothing
            End If
            'Commit cart items to DB - and handle vouchers if user bought one
            Dim item
            For Each item In Profile.EBCart.Items
                commitCartItem(ID, item)
                If item.voucherNumber <> "" Then
                    updateVoucher(ID, item.voucherNumber)
                    voucherNumber = voucherNumber & item.voucherNumber & " & "
                Else
                    voucherOnlyOrder = False
                End If
            Next
            If voucherNumber.Length > 3 Then voucherNumber = Left(voucherNumber, Len(voucherNumber) - 3)
            Session("EBTmpVoucherNumber") = voucherNumber
            'If customer used a voucher as par payment, then set it's status to Used (unless its a coupon)
            'Dim si As New siteInclude
            If Profile.EBCart.VoucherNumber <> "" Then
                If Not Profile.EBCart.VoucherIsCoupon Then
                    setVoucherToUsed(uid, Profile.EBCart.VoucherNumber)
                End If
            End If
        End If
        If bError Then newOrderID = 0
        'siteInclude.debug("Returning newOrderID=" & newOrderID)
        Return newOrderID
    End Function
    Protected Function getCard(ByVal t As String) As String
        Dim cp As ContentPlaceHolder = Master.FindControl("ContentPlaceHolder1")
        Dim m As DropDownList = cp.FindControl("drp" & t & "Month")
        Dim y As DropDownList = cp.FindControl("drp" & t & "Year")
        Dim result As String
        result = make2Digit(m.SelectedValue) & make2Digit(Right(y.SelectedValue, 2))
        Return result
    End Function
    Protected Function make2Digit(ByVal n As String) As String
        Dim result As String
        If Len(n) = 1 Then
            result = "0" & n
        Else
            result = n
        End If
        Return result
    End Function
    Protected Function getDistID(ByVal countryCode As String) As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByCountryCodeDistSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0)("affID")
            End If
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function calcOrderTotal(ByVal goodsInc As Decimal, ByVal ship As Decimal, ByVal shipVat As Decimal) As Decimal
        Dim result As Decimal = 0.0
        result = result + goodsInc
        result = result + (ship * ((shipVat / 100) + 1))
        Return result
    End Function
    Protected Function trimCrap(ByVal fck As String) As String
        Dim result As String = Replace(fck, "<p>", "")
        result = Replace(result, "</p>", "")
        Return result
    End Function
    Protected Function getFirstname(ByVal s As String) As String
        Dim fullname As String = Trim(s)
        Dim result As String = ""
        If InStr(fullname, " ") Then
            Dim a() As String = Split(fullname, " ")
            result = a(0)
        Else
            result = fullname
        End If
        Return result
    End Function
    Protected Function getSurname(ByVal s As String) As String
        Dim fullname As String = Trim(s)
        Dim result As String = ""
        If InStr(fullname, " ") Then
            Dim a() As String = Split(fullname, " ")
            result = a(UBound(a))
        Else
            result = fullname
        End If
        Return result
    End Function
    Protected Function getUserOrderID(ByVal id As Integer) As String
        Dim dt As New DataTable
        Dim result As String = ""
        Try
            Dim param() As String = {"@id"}
            Dim paramValue() As String = {id.ToString()}
            Dim paramType() As SqlDbType = {SqlDbType.Int}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procShopOrderByOrderIDSelect")
            If dt.Rows.Count > 0 Then result = dt.Rows(0)("newOrderID").ToString()
        Catch ex As Exception
            siteInclude.addError("payment.aspx.vb8", "getUserOrderID(id=" & id.ToString() & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
        Return result
    End Function
    'Proxt Functions
    Public Shared Function URLEncode(ByVal strString As String) As String
        Return HttpUtility.UrlEncode(strString)
    End Function
    Public Shared Function findField(ByVal strFieldName As String, ByVal strThisResponse As String) As String
        Dim iItem As Integer
        Dim arrItems(1) As String
        Dim strFindField As String = ""
        arrItems = Split(strThisResponse, vbCrLf)
        For iItem = LBound(arrItems) To UBound(arrItems)
            If InStr(arrItems(iItem), strFieldName & "=") = 1 Then
                strFindField = Mid(arrItems(iItem), Len(strFieldName) + 2)
                Exit For
            End If
        Next
        Return strFindField
    End Function
    Public Shared Function SystemURL(ByVal strConnectTo As String, ByVal strType As String) As String
        Dim strSystemURL As String = ""
        If strConnectTo = "LIVE" Then
            Select Case strType
                Case "abort"
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/abort.vsp"
                Case "authorise"
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/authorise.vsp"
                Case "cancel"
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/cancel.vsp"
                Case "purchase"
                    strSystemURL = "https://live.sagepay.com/gateway/service/vspdirect-register.vsp"
                Case "refund"
                    strSystemURL = "https://live.sagepay.com/gateway/service/refund.vsp"
                Case "release"
                    strSystemURL = "https://live.sagepay.com/gateway/service/release.vsp"
                Case "repeat"
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/repeat.vsp"
                Case "void"
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/void.vsp"
                Case "3dcallback"
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/direct3dcallback.vsp"
                Case "showpost"
                    strSystemURL = "https://ukvpstest.protx.com/showpost/showpost.asp"
            End Select
        ElseIf strConnectTo = "TEST" Then
            Select Case strType
                Case "abort"
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/abort.vsp"
                Case "authorise"
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/authorise.vsp"
                Case "cancel"
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/cancel.vsp"
                Case "purchase"
                    strSystemURL = "https://test.sagepay.com/gateway/service/vspdirect-register.vsp"
                Case "refund"
                    strSystemURL = "https://test.sagepay.com/gateway/service/refund.vsp"
                Case "release"
                    strSystemURL = "https://test.sagepay.com/gateway/service/release.vsp"
                Case "repeat"
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/repeat.vsp"
                Case "void"
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/void.vsp"
                Case "3dcallback"
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/direct3dcallback.vsp"
                Case "showpost"
                    strSystemURL = "https://ukvpstest.protx.com/showpost/showpost.asp"
            End Select
        Else
            Select Case strType
                Case "abort"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectGateway.asp?Service=VendorAbortTx"
                Case "authorise"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectGateway.asp?Service=VendorAuthoriseTx"
                Case "cancel"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectGateway.asp?Service=VendorCancelTx"
                Case "purchase"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectGateway.asp"
                Case "refund"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectGateway.asp?Service=VendorRefundTx"
                Case "release"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectGateway.asp?Service=VendorReleaseTx"
                Case "repeat"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectGateway.asp?Service=VendorRepeatTx"
                Case "void"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectGateway.asp?Service=VendorVoidTx"
                Case "3dcallback"
                    strSystemURL = "https://ukvpstest.protx.com/VSPSimulator/VSPDirectCallback.asp"
                Case "showpost"
                    strSystemURL = "https://ukvpstest.protx.com/showpost/showpost.asp"
            End Select
        End If

        Return strSystemURL
    End Function
    Public Shared Function SQLSafe(ByVal strRawText As String) As String
        Dim strCleanedText As String = ""
        Dim iCharPos As Integer = 1
        Do While iCharPos <= Len(strRawText)
            '** Double up single quotes, but only if they aren't already doubled **
            If Mid(strRawText, iCharPos, 1) = "'" Then
                strCleanedText = strCleanedText & "''"
                If iCharPos <> Len(strRawText) Then
                    If Mid(strRawText, iCharPos + 1, 1) = "'" Then iCharPos = iCharPos + 1
                End If
            Else
                strCleanedText = strCleanedText & Mid(strRawText, iCharPos, 1)
            End If
            iCharPos = iCharPos + 1
        Loop
        Return Trim(strCleanedText)
    End Function
End Class

