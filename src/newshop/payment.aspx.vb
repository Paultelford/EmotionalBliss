Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text
Imports s2sVB
Imports siteInclude
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class shop_payment
    Inherits BasePage
    Private drpMainCountry As DropDownList
    Private voucherOnlyOrder As Boolean = False
    Private emailBody As String = ""
    Private voucherNumber As String = ""
    Private Const _maxDeliveryLength As Integer = 200
    Private lblPleaseWait As Label

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
        drpMainCountry = Master.FindControl("drpMainCountry")
        If Not Page.IsPostBack Then
            If LCase(Session("EBLanguage")) = "nl" Or LCase(Session("EBLanguage")) = "be" Then
                chkAccount.Visible = True
                'chkIDeal.Visible = True
            End If
            If LCase(Session("EBLanguage")) = "gb" Then
                trAddSearch1.Visible = True
                trAddSearch2.Visible = True
                trAddSearch3.Visible = True
            End If
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
        If voucherOnlyPurchase() Or consultancyInBasket() Then
            'Hide all other types of payment
            hidVoucherPurchase.Value = "True"
            chkPost.Visible = False
            chkAccount.Visible = False
            chkIDeal.Visible = False
            'chkCC.Checked = True
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        loadDBResources()
    End Sub
    Protected Sub chkCC_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkCC.Checked Then
            tdCardDetails.Visible = True
            tdBillAddress.Visible = True
            tdShipAddress.Visible = False
            trAccount.Visible = False
            tdPersonalInfo.Visible = True
            tdPersonalSpacer.Visible = True
            lblBillName.Text = "Name on card:"
        End If
        resetButtons()
    End Sub
    Protected Sub chkPost_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkPost.Checked Then
            tdCardDetails.Visible = False
            tdBillAddress.Visible = True
            tdShipAddress.Visible = False
            trAccount.Visible = False
            tdPersonalInfo.Visible = True
            tdPersonalSpacer.Visible = False
            lblBillName.Text = "Name:"
        End If
        resetButtons()
    End Sub
    Protected Sub chkAccount_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkAccount.Checked Then
            tdCardDetails.Visible = False
            tdBillAddress.Visible = True
            tdShipAddress.Visible = False
            trAccount.Visible = True
            tdPersonalInfo.Visible = True
        End If
        resetButtons()
    End Sub
    Protected Sub chkIDeal_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkIDeal.Checked Then
            tdCardDetails.Visible = False
            tdBillAddress.Visible = True
            tdShipAddress.Visible = False
            trAccount.Visible = False
            tdPersonalInfo.Visible = True
            tdPersonalSpacer.Visible = False
        End If
        resetButtons()
    End Sub
    Protected Sub resetButtons()
        'tdBreak.Visible = tdCardDetails.Visible
        btnSubmitBill.Visible = False
        btnSubmitShipBill.Visible = False
        radNo.Checked = False
        radYes.Checked = False
    End Sub
    Protected Sub radYes_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        tdShipAddress.Visible = False
        tdBillAddress.Visible = True
        btnSubmitBill.Visible = True
        btnSubmitShipBill.Visible = False
    End Sub
    Protected Sub radNo_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        tdShipAddress.Visible = True
        If chkCC.checked Then tdBillAddress.Visible = False
        btnSubmitBill.Visible = False
        btnSubmitShipBill.Visible = True
        If chkCC.Checked Then btnViewBilling.Visible = True
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
    Protected Sub btnSubmitShipBill_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Page.Validate("add")
        If Page.IsValid Then
            Dim btn As ImageButton = CType(sender, ImageButton)
            btn.Visible = False
            'All ok, store details in db
            commitDetails()
        End If
    End Sub
    Protected Sub btnSubmitBill_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Page.Validate("add")
        If Page.IsValid Then
            Dim btn As ImageButton = CType(sender, ImageButton)
            btn.Visible = False
            'All ok, store details in db
            commitDetails()
        End If
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
            .Parameters("@countryCode").Value = Session("EBLanguage")
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
    Protected Sub commitDetails()
        If Len(txtDelivery.Text) < _maxDeliveryLength Then
            Dim Type As String = ""
            Dim affID As Integer = 0
            Dim prefix As String = ""
            Dim ID As Integer = 0
            Dim orderID As Integer = 0
            Dim bError As Boolean = False
            Dim bLiveAuth As Boolean = False
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
                prefix = "20"
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
                .Parameters.Add(New SqlParameter("@cardCv2", SqlDbType.VarChar, 3))
                .Parameters.Add(New SqlParameter("@useBillAdd", SqlDbType.Bit))
                .Parameters.Add(New SqlParameter("@orderType", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@accountNo", SqlDbType.VarChar, 12))
                .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
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
                .Parameters("@cardNo").Value = getHidCardNo(txtCard.Text)
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
                .Parameters("@dob").Value = ""
                .Parameters("@gender").Value = drpGender.SelectedValue
                .Parameters("@delivery").Value = txtDelivery.Text
                .Parameters("@outID").Direction = ParameterDirection.Output
            End With
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
            Catch ex As Exception
                Try
                    Dim si As New siteInclude
                    si.addError("shop/payment.aspx", "Error adding customer details in commitDetails():" & ex.ToString)
                    si = Nothing
                    bError = True
                    lblError.Text = "<b><font color='red'>There has been an error processing your details.  Your order cannot be completed at this time.</font></b>"
                    btnSubmitBill.Visible = False
                    btnSubmitShipBill.Visible = False
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
                    Dim si As New siteInclude
                    si.AddToOrderLog(uqID, "Order Placed", "System", True, "Emailed")
                Catch ex As Exception
                End Try
                'Context.Items("orderID") = orderID
                'Context.Items("orderPrefix") = prefix
                'Context.Items("orderPaid") = ""
                Session("EBTmpOrderType") = Type
                If CBool(hidVoucherPurchase.Value) Then voucherOnlyOrder = True
                Session("EBTmpOrderPaid") = False
                Session("EBTmpOrderID") = CStr(orderID)
                Session("EBTmpOrderPrefix") = CStr(prefix)
                'If the shopper is buying a voucher or consultancy voucher, then use live authorization (voucher/consultancy will be emailed to the shopper immediately if auth is successful)
                If voucherOnlyOrder Or consultancyInBasket() Then
                    bLiveAuth = True
                    Session("EBTmpOrderPaid") = takePayment(uqID)
                End If
                If Profile.EBCart.VoucherNumber <> "" Then
                    'A voucher/coupon was used in this order
                    'Add the orderID to the voucher
                    addOrderToVoucher(uqID, Profile.EBCart.VoucherNumber)
                End If

                If Type = "ideal" Then
                    Session("EBTmpUniqueID") = uqID
                    Response.Redirect("iDeal.aspx")
                Else
                    If bLiveAuth Then
                        'Only goto receipt page if transaction was successful
                        If CBool(Session("EBTmpOrderPaid")) Then
                            Dim si As New siteInclude
                            'Voucher orders will always have vat of 0, and goods total will always be same as orderTotal.
                            si.addToSalesLedger(uqID, 0, 1, Session("EBLanguage"), prefix, Profile.EBCart.GoodsIncVat, 0, 0, 0)
                            si = Nothing
                            If voucherOnlyOrder Then
                                setVoucherActive()
                                sendConfirmationEmail(orderID, uqID, "voucher")
                            End If
                            sendConfirmationEmail(orderID, uqID, "confirmation")
                            Response.Redirect("receipt.aspx")
                        Else
                            'Payment failed, delete the order
                            deleteOrder(uqID)
                        End If
                    Else
                        'Live auth not used, goto receipt page
                        sendConfirmationEmail(orderID, uqID, "confirmation")
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
    Protected Function commitOrder(ByVal customerID As Integer, ByVal orderPrefix As String, ByVal orderType As String, ByRef uid As Integer) As Integer
        Dim orderID As String
        Dim ID As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderInsert", oConn)
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
            .Parameters("@goodsVat").Value = If(Profile.EBCart.CouponVat > 0, Profile.EBCart.CouponVat, Profile.EBCart.GoodsVat) + Profile.EBCart.ShippingVat)
            .Parameters("@shipping").Value = shipping
            .Parameters("@shippingVatRate").Value = shippingVatRate
            .Parameters("@shippingTotal").Value = shippingTotal
            .Parameters("@orderTotal").Value = Profile.EBCart.TotalInc
            .Parameters("@orderCountryCode").Value = Session("EBLanguage")
            .Parameters("@customerID").Value = customerID
            .Parameters("@orderPrefix").Value = orderPrefix
            .Parameters("@clickThroughID").Value = clickID
            .Parameters("@currency").Value = getOrderCurrency(Session("EBLanguage"))
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
            distributorID = getDistID(Session("EBLanguage"))
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
        Return newOrderID
    End Function
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
    Protected Function takePayment(ByVal id As Integer) As Boolean
        Dim vendorTxCode As String = "EBPAYMENT" & formatDateElement(Day(Date.Today)) & formatDateElement(Month(Date.Today)) & Year(Date.Today) & Hour(Now()) & Minute(Now()) & CStr(id)
        Dim tx As New Protx.Vsp.DirectTransaction(vendorTxCode, Protx.Vsp.VspTransactionType.Payment)
        Dim sStatus As String
        Dim countryCode As String
        Try
            countryCode = Session("EBLanguage")
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
            lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details."
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "takePayment(id=" & id & "); " & ex.ToString)
            si = Nothing
        End Try

        Dim vspResponse As Protx.Vsp.DirectResponse = tx.Send()
        Dim sHTML As String = ""

        'Dim lblCCResults As Label = FV.FindControl("lblCCResults")
        Dim bSuccess As Boolean
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
            lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details."
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "takePayment_liveAuth(id=" & id & "); " & ex.ToString)
            si = Nothing
        End Try
        'Add to db
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProtxInsert", oConn)
        Try
            oCmd.CommandType = CommandType.StoredProcedure
            oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 30))
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
            lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details."
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "takePayment_liveAuthAddParam(id=" & id & "); " & ex.ToString)
            si = Nothing
        End Try
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details."
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "takePayment_liveAuthExSql(id=" & id & "); " & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
            'Clear tx
            tx = Nothing
        End Try
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
                oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 30))
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
                oCmd.Parameters("@country").Value = Session("EBLanguage")
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
    Protected Sub btnViewBilling_click(ByVal sender As Object, ByVal e As EventArgs)
        tdShipAddress.Visible = False
        tdBillAddress.Visible = True
        btnViewShipping.visible = True
    End Sub
    Protected Sub btnViewShipping_click(ByVal sender As Object, ByVal e As EventArgs)
        tdShipAddress.Visible = True
        If chkCC.checked Then tdBillAddress.Visible = False
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
            si.addError("payment.aspx.vb", "addOrderToVocuher(" & orderID & "," & voucherNumber & ")::" & ex.ToString)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub sendConfirmationEmail(ByVal newOrderID As String, ByVal orderID As Integer, ByVal type As String)
        Dim toAdd As String = ""
        Dim ccAdd As String = ""
        Dim subject As String = ""
        Dim msg As MailMessage
        Dim chk As CheckBox
        Dim plainView As AlternateView
        Dim htmlView As AlternateView
        If LCase(type) = "confirmation" Then setEmailBody(newOrderID, orderID, toAdd, ccAdd, subject)
        If LCase(type) = "voucher" Then setEmailBodyVoucher(newOrderID, orderID, toAdd, ccAdd, subject)
        msg = New MailMessage
        msg.To.Add(toAdd)
        If ccAdd <> "" Then msg.CC.Add(ccAdd)
        msg.From = New MailAddress("noreply@emotionalbliss.com")
        msg.Subject = subject
        msg.IsBodyHtml = True
        plainView = AlternateView.CreateAlternateViewFromString(emailBody, Nothing, "text/plain")
        htmlView = AlternateView.CreateAlternateViewFromString(Replace(emailBody, Chr(13), "<br>"), Nothing, "text/html")
        msg.AlternateViews.Add(plainView)
        msg.AlternateViews.Add(htmlView)
        Dim client As New SmtpClient
        client.Send(msg)
        msg.Dispose()
    End Sub
    Protected Sub setEmailBody(ByVal newOrderID As String, ByVal orderID As Integer, ByRef toAdd As String, ByRef ccAdd As String, ByRef subject As String)
        toAdd = txtEmail.Text
        emailBody = "*** Emotional Bliss ORDER CONFIRMATION ***" & Chr(13) & Chr(13)
        emailBody = emailBody & "Thank you for shopping at Emotional Bliss." & Chr(13) & Chr(13)
        emailBody = emailBody & "Your order has been received and we will advise you by email when your order" & Chr(13)
        emailBody = emailBody & "has been despatched showing your personal tracker reference." & Chr(13) & Chr(13)
        emailBody = emailBody & "You can also monitor the progress of your order by clicking the 'Track your" & Chr(13)
        emailBody = emailBody & "order'(button located top right of the website <a href='http://www.emotionalbliss.co.uk'>http://www.emotionalbliss.co.uk</a>)" & Chr(13) & Chr(13)
        emailBody = emailBody & "Simply enter your surname and invoice number as listed below." & Chr(13) & Chr(13)
        emailBody = emailBody & "All credit card orders will only be debited on despatch." & Chr(13) & Chr(13)
        emailBody = emailBody & "All cheque orders will be despatched on cleared funds." & Chr(13) & Chr(13)
        'emailBody = emailBody & "(UK)0870 041 00 22" & Chr(13) & "(Europe) 0044 870 041 0022" & Chr(13) & Chr(13)
        emailBody = emailBody & "INVOICE NUMBER       : " & newOrderID & "GB" & Chr(13)
        emailBody = emailBody & "ORDER DATE AND TIME  : " & FormatDateTime(Now(), DateFormat.ShortDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime) & Chr(13) & Chr(13)
        emailBody = emailBody & "QUANTITY   "
        emailBody = emailBody & "PRODUCT(DESCRIPTION)" & Chr(13)
        emailBody = emailBody & "--------------------------------------------------------" & Chr(13)
        Dim item
        For Each item In Profile.EBCart.Items
            emailBody = emailBody & item.Qty & "          "
            emailBody = emailBody & item.Name & Chr(13)
        Next
        emailBody = emailBody & "--------------------------------------------------------" & Chr(13) & Chr(13)
        emailBody = emailBody & "Order Total: £" & FormatNumber(CDec(Profile.EBCart.TotalInc), 2) & Chr(13) & Chr(13)
        emailBody = emailBody & "If you require any further information please call our help desk on 0870 041 00 22" & Chr(13) & Chr(13)
        emailBody = emailBody & "<a href='http://www.emotionalbliss.co.uk'>http://www.emotionalbliss.co.uk</a>"
        subject = "Order Confirmation"
    End Sub
    Protected Sub setEmailBodyVoucher(ByVal newOrderID As String, ByVal orderID As Integer, ByRef toAdd As String, ByRef ccAdd As String, ByRef subject As String)
        Dim row As DataRow = getVoucherDetails(orderID)
        emailBody = "This is a voucher order:" & Chr(13) & Chr(13)
        emailBody = emailBody & "To: " & row("recipient") & Chr(13)
        emailBody = emailBody & "From: " & row("purchaser") & Chr(13) & Chr(13)
        emailBody = emailBody & "Voucher Amount: " & getCurrencyCode() & row("credit") & Chr(13)
        emailBody = emailBody & "Voucher Number: " & row("number") & Chr(13) & Chr(13)
        emailBody = emailBody & "Comment: " & row("comment") & Chr(13)
        toAdd = row("recipientEmail")
        ccAdd = row("purchaserEmail")
        subject = "Vocuher Delivery"
    End Sub
    Protected Function encryptCard(ByVal n As String) As String
        Dim fes As New FE_SymmetricNamespace.FE_Symmetric
        Dim enc As String = fes.EncryptData(ConfigurationManager.AppSettings("aesKey").ToString, n)
        Return enc
    End Function
    Protected Function getHidCardNo(ByVal ccNum As String) As String
        Dim result As String = ""
        If len(ccNum) > 4 Then
            For iLoop As Integer = 1 To len(ccNum) - 4
                result = result & "*"
            Next
            result = result & right(ccNum, 4)
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
                sCountry = ds.Tables(0).Rows(0)("EBLanguage")
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
            Session("EBLanguage") = sCountry
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
            .Parameters("@countryCode").Value = Session("EBLanguage")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = ds.Tables(0).Rows(0)("currencyCode")
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx.vb", "getCurrencyCode(langauge=" & Session("EBLanguage") & "); " & ex.ToString)
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
    Protected Function consultancyInBasket()
        Dim result As Boolean = False
        Dim item
        For Each item In Profile.EBCart.Items
            If LCase(Left(item.ProductCode, 2)) = "vc" Then result = True
        Next
        Return result
    End Function
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
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx", "updateOrderStatus(id=" & uqID & ",status=" & status & "); " & ex.ToString)
            si = Nothing
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
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/payment.aspx.vb", "deleteOrder(uqID=" & uqID & "); " & ex.ToString)
            si = Nothing
        End Try
    End Sub
    Protected Sub loadDBResources()
        'Dim lblPleaseWait As Label = up1.FindControl("lblPleaseWait")
        'lblPleaseWait.Text = 
        Dim errRequired As String = getDBResourceString("errRequired", "global")
        lblHowPay.Text = getDBResourceString("lblHowToPay")
        chkCC.Text = getDBResourceString("lblChkCC")
        chkPost.Text = getDBResourceString("lblPost")
        chkIDeal.Text = getDBResourceString("lblIDeal")
        chkAccount.Text = getDBResourceString("lblAccount")
        lblCardDetails.Text = getDBResourceString("lblCardDetails")
        lblCardType.Text = getDBResourceString("lblCardType")
        lblCardNo.Text = getDBResourceString("lblCardNo")
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
        btnViewShipping.Text = getDBResourceString("btnViewShipping")
        req1.ErrorMessage = errRequired
        lblEmail.Text = getDBResourceString("lblEmail")
        regTxtEmail.ErrorMessage = getDBResourceString("regTxtEmail")
        lblHouseNo.Text = getDBResourceString("lblHouseNo")
        reqBillLookupHouse.ErrorMessage = errRequired
        lblPostcode.Text = getDBResourceString("lblPostcode")
        reqBillLookupPostcode.Text = errRequired
        lblAddress.Text = getDBResourceString("lblAddress")
        req2.ErrorMessage = errRequired
        lblPostcode2.Text = lblPostcode.Text
        lblPostcode3.Text = lblPostcode.Text
        lblPostcode4.Text = lblPostcode.Text
        req3.ErrorMessage = errRequired
        reqTxtPhone.ErrorMessage = errRequired
        lblCountry.Text = getDBResourceString("lblCountry")
        lblAccount.Text = getDBResourceString("lblAccountNo")
        lblShippingAddress.Text = getDBResourceString("lblShippingAddress")
        btnViewBilling.Text = getDBResourceString("btnViewBilling")
        lblName.Text = getDBResourceString("lblName")
        RequiredFieldValidator1.ErrorMessage = errRequired
        lblHouseNo2.Text = lblHouseNo.Text
        reqLookupShipHouse.ErrorMessage = errRequired
        reqLookupShipPostcode.ErrorMessage = errRequired
        lblAddress2.Text = lblAddress.Text
        RequiredFieldValidator2.ErrorMessage = errRequired
        RequiredFieldValidator3.ErrorMessage = errRequired
        lblCountry2.Text = lblCountry.Text
        lblDOB.Text = getDBResourceString("lblDOB")
        lblGender.Text = getDBResourceString("lblGender")
        drpGender.Items(0).Text = getDBResourceString("drpSelect", "global")
        drpGender.Items(1).Text = getDBResourceString("drpMale")
        drpGender.Items(2).Text = getDBResourceString("drpFemale")
        lblDeliverTo.Text = getDBResourceString("lblDeliverTo")
        radYes.Text = getDBResourceString("radYes")
        radNo.Text = getDBResourceString("radNo")


    End Sub
    Protected Sub lblPleaseWait_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label = CType(sender, Label)
        lbl.Text = getDBResourceString("lblPleaseWait", "global")
    End Sub
End Class