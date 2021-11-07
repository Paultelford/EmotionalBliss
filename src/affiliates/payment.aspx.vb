Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude
Imports System.Net
Imports System.IO
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class affiliates_payment
    Inherits BasePage
    Private drpMainCountry As DropDownList
    Private emailBody As String = ""
    Private Const _maxDeliveryLength As Integer = 200

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.load
        drpMainCountry = Master.FindControl("drpMainCountry")
        If Not Page.IsPostBack Then
            'Make Accout option visible for Distributors and Users
            If Session("EBAffEBDistributor") Or Session("EBAffEBUser") Then chkAccount.Visible = True
            loadAffAddress()
            'Account/CC Option is now different for each Affilaite. The code to show/hide is in txtBillCountry_load()
        End If
        If Session("EBAffHideCountry") Then
            trCountry.Visible = False
            trCountry2.Visible = False
        End If

    End Sub

    'Page Events
    Protected Sub loadAffAddress()
        'Grab Affiliate Address from DB
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters("@affID").Value = Session("EBAffID")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim rs As DataRow = ds.Tables(0).Rows(0)
                txtBillName.Text = rs("affCompany")
                If Not IsDBNull(rs("affAdd1")) Then txtBillAdd1.Text = rs("affAdd1")
                If Not IsDBNull(rs("affAdd2")) Then txtBillAdd2.Text = rs("affAdd2")
                If Not IsDBNull(rs("affAdd3")) Then txtBillAdd3.Text = rs("affAdd3")
                If Not IsDBNull(rs("affAdd4")) Then txtBillAdd4.Text = rs("affAdd4")
                If Not IsDBNull(rs("affAdd5")) Then txtBillAdd5.Text = rs("affAdd5")
                If Not IsDBNull(rs("affPostcode")) Then txtBillPostcode.Text = rs("affPostcode")
                If Not IsDBNull(rs("countryName")) Then txtBillCountry.Text = rs("countryName")
                If Not IsDBNull(rs("affEmail")) Then txtEmail.Text = rs("affEmail")
                If Not IsDBNull(rs("affToAdd1")) Then txtShipAdd1.Text = rs("affToAdd1")
                If Not IsDBNull(rs("affToAdd2")) Then txtShipAdd2.Text = rs("affToAdd2")
                If Not IsDBNull(rs("affToAdd3")) Then txtShipAdd3.Text = rs("affToAdd3")
                If Not IsDBNull(rs("affToPostcode")) Then txtShipPostcode.Text = rs("affToPostcode")
                If Not IsDBNull(rs("toCountryName")) Then txtShipCountry.Text = rs("toCountryName")
                txtShipCountry.Text = txtShipCountry.Text
                'Show Account only for certain affiliates
                If Not (Session("EBAffEBDistributor") Or Session("EBAffEBUser")) Then
                    chkCC.Visible = CBool(rs("canOrderWithCC"))
                    chkAccount.Visible = CBool(rs("canOrderWithAccount"))
                End If
            End If
        Catch ex As Exception
            'Response.Write(ex)
            'Response.End()
            lblError.Text = ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
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

    'User Events
    Protected Sub chkCC_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkCC.Checked Then
            lblBillName.Text = "Name on card:"
            tdCardDetails.Visible = True
            tdBillAddress.Visible = True
            tdShipAddress.Visible = False
            trAccount.Visible = False
        End If
        resetButtons()
    End Sub
    Protected Sub chkAccount_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        If chkAccount.Checked Then
            lblBillName.Text = "Company Name:"
            tdCardDetails.Visible = False
            tdBillAddress.Visible = True
            tdShipAddress.Visible = False
            'trAccount.Visible = True
        End If
        resetButtons()
    End Sub
    Sub btnGetOut_click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("receipt.aspx", False)
    End Sub
    Protected Sub resetButtons()
        tdBreak.Visible = tdCardDetails.Visible
        btnSubmitBill.Visible = False
        btnSubmitShipBill.Visible = False
        radNo.Checked = False
        radYes.Checked = False
    End Sub
    Protected Sub radYes_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        tdShipAddress.Visible = False
        btnSubmitBill.Visible = True
        btnSubmitShipBill.Visible = False
    End Sub
    Protected Sub radNo_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        tdShipAddress.Visible = True
        btnSubmitBill.Visible = False
        btnSubmitShipBill.Visible = True
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
    Protected Sub btnSubmitShipBill_click(ByVal sender As Object, ByVal e As EventArgs)
        Validate()
        If Page.IsValid Then
            'All ok, store details in db
            commitDetails()
            'Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('s_redirect()',200)", True)
        End If
    End Sub
    Protected Sub btnSubmitBill_click(ByVal sender As Object, ByVal e As EventArgs)
        Validate()
        If Page.IsValid Then
            'All ok, store details in db
            commitDetails()
            'Server.Transfer("receipt.aspx", True)
            'Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('s_redirect()',200)", True)
        End If
    End Sub

    'Subs
    Protected Sub commitDetails()
        'Check for maxlength on the special details
        If Len(txtDelivery.Text) < _maxDeliveryLength Then
            Dim Type As String = ""
            Dim affID = CType(Session("EBAffID"), Integer)
            'Dim prefix As String = getPrefixForAffiliateType(Session("EBAffID"))
            Dim prefix As String
            Dim orderCountry As String
            Dim userOrderID As String
            Dim customerID As Integer = 0
            Dim si As New siteInclude
            If chkAccount.Checked Then
                Type = "affaccount"
            End If
            If chkCC.Checked Then
                Type = "affcc"
            End If
            If Session("EBAffEBDistributor") Then
                prefix = "50"
                Type = Replace(Type, "aff", "dist")
                orderCountry = "ZZ"
            Else
                prefix = "40"
                orderCountry = Session("EBAffCountryCode")
            End If
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
                .Parameters("@phone").Value = "0"
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
                .Parameters("@fiscal").Value = ""
                .Parameters("@dob").Value = ""
                .Parameters("@gender").Value = ""
                .Parameters("@delivery").Value = txtDelivery.Text
                .Parameters("@outID").Direction = ParameterDirection.Output
            End With
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
                customerID = CType(oCmd.Parameters("@outID").Value, Integer)
            Catch ex As Exception

                si.addError("affiliates/payment.aspx", "commitDetails(); " & ex.ToString)
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            Dim orderID As Integer = 0 'The shopOrder PK field
            userOrderID = commitOrder(customerID, prefix, Type, orderID, orderCountry)
            Session("EBTmpOrderID") = userOrderID
            Session("EBTmpOrderPrefix") = prefix
            Session("EBTmpUniqueID") = orderID
            'Set Purchase Order No
            Profile.EBAffCart.PurchaseOrderNo = txtPurchaseOrderNo.Text
            'Attempt to deferr card details, cancel order if it fails
            'PT asked for change on 25 3 09 - No charge or deferment should take place, as order totals must be amended in orderView page, then th eorder can be charged
            If Type = "affcc" Then


                'If LCase(take3DPayment(orderID, "PAYMENT")) = "ok" Then
                'Successfully deferred and stored in database
                'Add 'Order complete, thankyou' msg to order log
                si = New siteInclude
                si.AddToOrderLog(orderID, "Order Placed, Thankyou.", "System", True, "Emailed")
                si = Nothing
                'Send email confirmation
                'sendConfirmationEmail(userOrderID, orderID, "confirmation")
                'Profile.EBAffCart.emptyBasket() 'Empty the basket
                'Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "window", "alert('doh!!!');", True)
                updateOrderStatus(orderID, "placed")
                Response.Redirect("receipt.aspx", False)
                'Else
                '   deleteOrder(orderID)
                'End If
            Else
                'Must have been account order, set order status to 'Placed' and redirect to receipt
                updateOrderStatus(orderID, "placed")
                Response.Redirect("receipt.aspx", False)
            End If
        Else
            'txtDelivery is greater than 200 characters, show error message
            lblDeliveryError.Text = "<font color='red'>Max length " & _maxDeliveryLength & " charcaters, you have used " & Len(txtDelivery.Text)
        End If
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
        emailBody = emailBody & "INVOICE NUMBER       : " & newOrderID & uCase(Session("EBAffCountryCode")) & Chr(13)
        emailBody = emailBody & "ORDER DATE AND TIME  : " & FormatDateTime(Now(), DateFormat.ShortDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime) & Chr(13) & Chr(13)
        emailBody = emailBody & "BILLING ADDRESS" & chr(13)
        emailBody = emailBody & txtBillName.Text & chr(13)
        If txtBillAdd1.Text <> "" Then emailBody = emailBody & txtBilLAdd1.Text & chr(13)
        If txtBillAdd2.Text <> "" Then emailBody = emailBody & txtBilLAdd2.Text & chr(13)
        If txtBillAdd3.Text <> "" Then emailBody = emailBody & txtBilLAdd3.Text & chr(13)
        If txtBillAdd4.Text <> "" Then emailBody = emailBody & txtBilLAdd4.Text & chr(13)
        If txtBillAdd5.Text <> "" Then emailBody = emailBody & txtBilLAdd5.Text & chr(13)
        If txtBillPostcode.Text <> "" Then emailBody = emailBody & txtBillPostcode.Text & chr(13)
        If txtBillCountry.Text <> "" And Not Session("EBAffHideCountry") Then emailBody = emailBody & txtBillCountry.Text & Chr(13)
        emailBody = emailBody & Chr(13)
        emailBody = emailBody & "SHIPPING ADDRESS" & chr(13)
        If Not CType(radYes.Checked, Boolean) Then
            emailBody = emailBody & txtShipName.Text & chr(13)
            If txtShipAdd1.Text <> "" Then emailBody = emailBody & txtShipAdd1.Text & chr(13)
            If txtShipAdd2.Text <> "" Then emailBody = emailBody & txtShipAdd2.Text & chr(13)
            If txtShipAdd3.Text <> "" Then emailBody = emailBody & txtShipAdd3.Text & chr(13)
            If txtShipAdd4.Text <> "" Then emailBody = emailBody & txtShipAdd4.Text & chr(13)
            If txtShipAdd5.Text <> "" Then emailBody = emailBody & txtShipAdd5.Text & chr(13)
            If txtShipPostcode.Text <> "" Then emailBody = emailBody & txtShipPostcode.Text & chr(13)
            If txtShipCountry.Text <> "" And Not Session("EBAffHideCountry") Then emailBody = emailBody & txtShipCountry.Text & Chr(13)
            emailBody = emailBody & Chr(13)
        Else
            emailBody = emailBody & txtBillName.Text & Chr(13)
            If txtBillAdd1.Text <> "" Then emailBody = emailBody & txtBillAdd1.Text & Chr(13)
            If txtBillAdd2.Text <> "" Then emailBody = emailBody & txtBillAdd2.Text & Chr(13)
            If txtBillAdd3.Text <> "" Then emailBody = emailBody & txtBillAdd3.Text & Chr(13)
            If txtBillAdd4.Text <> "" Then emailBody = emailBody & txtBillAdd4.Text & Chr(13)
            If txtBillAdd5.Text <> "" Then emailBody = emailBody & txtBillAdd5.Text & Chr(13)
            If txtBillPostcode.Text <> "" Then emailBody = emailBody & txtBillPostcode.Text & Chr(13)
            If txtBillCountry.Text <> "" And Not Session("EBAffHideCountry") Then emailBody = emailBody & txtBillCountry.Text & Chr(13)
            emailBody = emailBody & Chr(13)
        End If
        emailBody = emailBody & "QUANTITY   "
        emailBody = emailBody & "PRODUCT(DESCRIPTION)" & Chr(13)
        emailBody = emailBody & "--------------------------------------------------------" & Chr(13)
        Dim item
        For Each item In Profile.EBAffCart.Items
            emailBody = emailBody & item.Qty & "          "
            emailBody = emailBody & item.Name & Chr(13)
        Next
        emailBody = emailBody & "--------------------------------------------------------" & Chr(13) & Chr(13)
        emailBody = emailBody & "Order Total: £" & FormatNumber(CDec(Profile.EBAffCart.TotalInc), 2) & Chr(13) & Chr(13)
        emailBody = emailBody & "If you require any further information please call our help desk on 0870 041 00 22" & Chr(13) & Chr(13)
        emailBody = emailBody & "<a href='http://www.emotionalbliss.co.uk'>http://www.emotionalbliss.co.uk</a>"
        subject = "Order Confirmation"
    End Sub
    Protected Sub commitCartItem(ByVal orderID As String, ByRef cartItem As Object)
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
            'Response.Write(ex)
            'Response.End()
            lblError.Text = ex.ToString
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
    Protected Sub updateOrderStatus(ByVal id As Integer, ByVal newStatus As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDStatusUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@distStatus", SqlDbType.VarChar, 20))
            .Parameters("@orderID").Value = id
            .Parameters("@status").Value = newStatus
            .Parameters("@distStatus").Value = ""
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteinclude
            si.addError("affilaites/payment.aspx", "updateOrderStatus(id=" & id & ", status=" & newStatus & "); " & ex.ToString())
            si = Nothing
        End Try
    End Sub

    'Functions
    Protected Function commitOrder(ByVal customerID As Integer, ByVal orderPrefix As String, ByVal orderType As String, ByRef uid As Integer, ByVal countryCode As String) As Integer
        Dim orderID As String
        Dim ID As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopAffOrderInsert", oConn)
        Dim goodsVatRate As Decimal = 0
        Dim newOrderID As Integer = 0
        Dim shipping As Decimal = Profile.EBAffCart.Shipping
        Dim shippingVatRate As Decimal = Profile.EBAffCart.ShippingVatRate
        Dim shippingTotal As Decimal = Profile.EBAffCart.ShippingTotal
        Dim distributorID As Integer
        Dim clickID As Integer = 0
        Dim orderSource As String = ""
        Dim paymentMethod As String = ""
        Dim currency As String = getOrderCurrency(countryCode)
        Select Case LCase(orderType)
            Case "affaccount"
                orderSource = "affiliate"
                paymentMethod = "account"
            Case "affcc"
                orderSource = "affiliate"
                paymentMethod = "cc"
            Case "distaccount"
                orderSource = "distributor"
                paymentMethod = "account"
                currency = getDistributorBuyingCurrency(Session("EBAffID"))
            Case "distcc"
                orderSource = "distributor"
                paymentMethod = "cc"
                currency = getDistributorBuyingCurrency(Session("EBAffID"))
        End Select
        If Session("EBAffClickThroughID") <> "" Then clickID = Session("EBAffClickThroughID")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderType", SqlDbType.VarChar, 15))
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
            .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.VarChar, 15))
            .Parameters.Add(New SqlParameter("@affiliateID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@purchaseOrder", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@orderSource", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@paymentMethod", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@orderType").Value = orderType
            .Parameters("@goods").Value = Profile.EBAffCart.TotalEx
            .Parameters("@goodsVat").Value = Profile.EBAffCart.GoodsVat
            .Parameters("@shipping").Value = shipping
            .Parameters("@shippingVatRate").Value = shippingVatRate
            .Parameters("@shippingTotal").Value = shippingTotal
            .Parameters("@orderTotal").Value = Profile.EBAffCart.TotalInc + shippingTotal
            .Parameters("@orderCountryCode").Value = countryCode
            .Parameters("@customerID").Value = customerID
            .Parameters("@orderPrefix").Value = orderPrefix
            .Parameters("@clickThroughID").Value = clickID
            '.Parameters("@currency").Value = getOrderCurrency(Session("EBShopCountry"))
            .Parameters("@currency").Value = currency
            .Parameters("@newOrderID").Direction = ParameterDirection.Output
            .Parameters("@affiliateID").Value = Session("EBAffID")
            .Parameters("@purchaseOrder").Value = txtPurchaseOrderNo.Text
            .Parameters("@orderSource").Value = orderSource
            .Parameters("@paymentMethod").Value = paymentMethod
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
                si.addError("shop/payment.aspx", "Error adding order in commitOrder():" & ex.ToString)
                si = Nothing
            Catch e As Exception
            End Try
            lblError.Text = "There has been an error processing your details.  Your order cannot be completed at this time."
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Try
            'Add to affiliates/distributors statement
            Dim si As New siteInclude
            Select Case orderPrefix
                Case 40
                    'Adding to affiliates statement has been moved to scan page, so once order has been placed, shipping can be altered before the whole lot is added to statement
                    'si.affAddToStatement(Session("EBAffID"), 0, calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), ID, 0, 2)
                    'Now add some credit to distributors statement
                    'si.affAddToStatement(GetDistributorID(Session("EBAffEBDistributorCountryCode")), calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), 0, ID, 0, 2)
                Case 50
                    'Add order to distributors statement
                    'si.affAddToStatement(Session("EBAffID"), 0, calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), ID, 0, 1)
            End Select
            si = Nothing
        Catch ex As Exception
            'Response.Write(ex)
            'Response.End()
            lblError.Text = ex.ToString
        End Try

        'Commit cart items to DB
        Dim item
        For Each item In Profile.EBAffCart.Items
            commitCartItem(ID, item)
        Next
        Return newOrderID
    End Function
    Protected Function commitOrderOld(ByVal customerID As Integer, ByVal orderPrefix As String, ByVal orderType As String, ByVal orderCountry As String) As String
        Dim orderID As String
        Dim ID As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderAffInsert", oConn)
        Dim shippingVatRate As Decimal = 0
        Dim shipping As Decimal = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderType", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@goodsTotal", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@goodsTotalInc", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@goodsVatRate", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingTotal", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingVatRate", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderCountryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@customerID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@orderPrefix", SqlDbType.VarChar, 3))
            .Parameters.Add(New SqlParameter("@clickThroughID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@affiliateID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.VarChar, 15))
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@orderType").Value = orderType
            .Parameters("@goodsTotal").Value = Profile.EBAffCart.TotalEx
            .Parameters("@goodsTotalInc").Value = Profile.EBAffCart.TotalInc
            .Parameters("@goodsVatRate").Value = 0
            .Parameters("@shippingTotal").Value = shipping
            .Parameters("@shippingVatRate").Value = shippingVatRate
            .Parameters("@orderCountryCode").Value = orderCountry
            .Parameters("@customerID").Value = customerID
            .Parameters("@orderPrefix").Value = orderPrefix
            .Parameters("@clickThroughID").Value = 0
            .Parameters("@affiliateID").Value = Session("EBAffID")
            .Parameters("@currency").Value = getOrderCurrency(Session("EBAffCountryCode"))
            .Parameters("@newOrderID").Direction = ParameterDirection.Output
            .Parameters("@ID").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            orderID = oCmd.Parameters("@newOrderID").Value
            ID = oCmd.Parameters("@ID").Value
        Catch ex As Exception
            'Response.Write(ex)
            'Response.End()
            lblError.Text = ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try

        Try
            'Add to affiliates/distributors statement
            Dim si As New siteInclude
            Select Case orderPrefix
                Case 40
                    'Add to affiliates statement
                    si.affAddToStatement(Session("EBAffID"), 0, calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), ID, 0, 2)
                    'Now add some credit to distributors statement
                    si.affAddToStatement(GetDistributorID(orderCountry), calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), 0, ID, 0, 2)
                Case 50
                    'Add order to distributors statement
                    si.affAddToStatement(Session("EBAffID"), 0, calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), ID, 0, 1)
            End Select

            si = Nothing
        Catch ex As Exception
            'Response.Write(ex)
            'Response.End()
            lblError.Text = ex.ToString
        End Try

        'Commit cart items to DB
        Dim item
        For Each item In Profile.EBAffCart.Items
            commitCartItem(ID, item)
        Next
        Return orderID
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
    Protected Function getPrefixForAffiliateType(ByVal affID As Integer) As String
        'NO LONGER CALLED, DETAILS ALREADY STORED IN SESSION
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByIDDistSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters("@affID").Value = affID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If CType(ds.Tables(0).Rows(0)("affEBDistributor"), Boolean) Then
                    result = "50"
                Else
                    result = "40"
                End If
            Else
                'error, affID not found
                'Response.Redirect("default.aspx")
                lblError.text = "AffID Not Found. Please login"
            End If
        Catch ex As Exception
            'Response.Write(ex)
            'Response.End()
            lblError.Text = ex.ToString
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
    Protected Function take3DPayment(ByVal id As Integer, ByVal chargeType As String) As String
        Dim countryCode As String = Session("EBAffCountryCode")
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
            strCardNumber = txtCard.Text
            If drpStartMonth.SelectedValue <> "" And drpStartYear.SelectedValue <> "" Then strStartDate = make2Digit(drpStartMonth.SelectedValue) & Right(drpStartYear.SelectedValue, 2)
            strExpiryDate = make2Digit(drpEndMonth.SelectedValue) & Right(drpEndYear.SelectedValue, 2)
            strIssueNumber = txtIssue.Text
            strCV2 = txtCV2.Text
            decTotal = Profile.EBAffCart.TotalInc
            strCurrency = getOrderCurrency(Session("EBAffCountryCode"))
            strGiftAid = "0"
            strDescription = "EmotionalBliss Order"
            strBillingAddress = txtBillAdd1.Text
            strBillingPostCode = txtBillPostcode.Text
            strDeliveryAddress = txtShipAdd1.Text
            strDeliveryPostCode = txtShipPostcode.Text

            'Create post string
            strPost = "VPSProtocol=2.22"
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
            strPost = strPost & "&BillingAddress=" & URLEncode(strBillingAddress)
            strPost = strPost & "&BillingPostCode=" & URLEncode(strBillingPostCode)
            If bDeliverySame Then
                strPost = strPost & "&DeliveryAddress=" & URLEncode(strBillingAddress)
                strPost = strPost & "&DeliveryPostCode=" & URLEncode(strBillingPostCode)
            Else
                strPost = strPost & "&DeliveryAddress=" & URLEncode(strDeliveryAddress)
                strPost = strPost & "&DeliveryPostCode=" & URLEncode(strDeliveryPostCode)
            End If
            strPost = strPost & "&GiftAidPayment=" & strGiftAid
            If strTransactionType <> "AUTHENTICATE" Then strPost = strPost & "&ApplyAVSCV2=0"
            strPost = strPost & "&ClientIPAddress=" & Request.ServerVariables("REMOTE_HOST")
            strPost = strPost & "&Apply3DSecure=2"
            strPost = strPost & "&AccountType=E"

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
                    strPageError = "Your server was unable to register this transaction with Protx." & _
                    "  Check that you do not have a firewall restricting the POST and " & _
                    "that your server can correctly resolve the address " & siteInclude.getSagePayURL(strConnectTo, "puchase")
                Else
                    strPageError = "An Error has occurred whilst trying to register this transaction.<BR>" & _
                    "The Error Number is: " & Err.Number & "<BR>" & _
                    "The Description given is: " & Err.Description
                End If
                Dim si As New siteinclude
                si.addError("callcentre/payment.asp.vb", "take3DPayment(id=" & id & "); " & strPageError)
                si = Nothing
                strPageError = "An error occured while trying to contant your bank.<br>We appologies for any inconvenience. Please try again later.<br>Your card has not been charged and the order is incomplete."
            Else
                '** No transport level errors, so the message got the Protx **
                '** Analyse the response from VSP Direct to check that everything is okay **
                '** Registration results come back in the Status and StatusDetail fields **
                strStatus = findField("Status", strResponse)
                strStatusDetail = findField("StatusDetail", strResponse)
                'lblStatus.Text = strStatus
                'lblStatusDetail.Text = strStatusDetail

                'Add details to database
                Session("EBTmpUniqueID") = id
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
                    session("OrderTotal") = decTotal
                    session("EBTmpOrderID") = id
                    session("EBShopCountry") = countryCode
                    Session("Currency") = strCurrency
                    'panCard.Visible = False
                    'panBill.Visible = False
                    'panShip.Visible = False
                    'pan3DSecure.Visible = True
                    result = "3dauth"
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
                        Dim si As New siteInclude
                        si.AddToOrderLog(id, "Protx Error: " & strDBStatus & "; txID=" & strVendorTxCode, "Payment System", True)
                        si = Nothing
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
                        If lcase(strStatus) = "ok" Then oCmd.Parameters("@status").Value = "deferred"
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
                        oCmd.Parameters("@currency").Value = ucase(strCurrency)
                        oCmd.Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                        oCmd.Parameters("@country").Value = countryCode
                        oCmd.Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
                        oCmd.Parameters("@bank").Value = Convert.ToDecimal("1.0")
                        oCmd.Parameters.Add(New SqlParameter("@newOrderStatus", SqlDbType.VarChar, 10))
                        oCmd.Parameters("@newOrderStatus").Value = "deferred"
                        oCmd.Parameters.Add(New SqlParameter("@extraPayment", SqlDbType.Bit))
                        oCmd.Parameters("@extraPayment").Value = False
                        If lcase(strStatus) = "ok" Then
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
                        lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details."
                        Dim si As New siteInclude
                        si.addError("callcentre/payment.aspx", "takePayment_liveAuthAddParam(id=" & id & "); " & ex.ToString)
                        si = Nothing
                    End Try
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details."
                        Dim si As New siteInclude
                        si.addError("callcentre/payment.aspx", "takePayment_liveAuthExSql(id=" & id & "); " & ex.ToString)
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
            If len(strPageError) > 0 Then
                lblError.Text = strPageError
                'radYes.Checked = False
                'radNo.Checked = False
                If lCase(txtEmail.Text) = "noreply@emotionalbliss.com" Then txtEmail.Text = ""
                ScriptManager.RegisterStartupScript(lblError, Me.GetType, "onloader", "self.setTimeout(""scroll(0,0);"",200);", True)
            End If
        Catch ex As Exception
            Dim si As New siteinclude
            si.addError("callcentre/payment.asp.vb", "take3DPayment_b(id=" & id & "); " & ex.tostring())
            si = Nothing
        End Try
        Return lcase(result)
    End Function
    Protected Function GetDistributorID(ByVal countryCode As String) As Integer
        'Returns a specified countrys DistributorID
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliatesDistByCountryCodeSelect", oConn)
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
            lblError.Text = ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
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
            Dim si As New siteInclude
            si.addError("affiliates/payment.aspx", "getOrderCurrency(countryCode=" & countryCode & "); " & ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
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
    Function formatDateElement(ByVal d As String)
        Dim sResult As String = d
        If Len(d) = 1 Then
            sResult = "0" & d
        End If
        Return sResult
    End Function
    Protected Function getDistributorBuyingCurrency(ByVal distID As Integer) As String
        Dim result As String = "gbp"
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByIDBuyingCurrencySelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters("@affID").Value = distID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then If Not IsDBNull(ds.Tables(0).Rows(0)("affCurrencyCodeBuys")) Then result = ds.Tables(0).Rows(0)("affCurrencyCodeBuys")
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("affiliates/payment.aspx", "getDistributorBuyingCurrency(distID=" & distID & "); " & ex.ToString)
            si = Nothing
        End Try
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
            siteInclude.addError("payment.aspx.vb", "getUserOrderID(id=" & id.ToString() & "); " & ex.ToString())
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
    Public Shared Function SystemURLs(ByVal strConnectTo As String, ByVal strType As String) As String
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
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/vspdirect-register.vsp"
                Case "refund"
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/refund.vsp"
                Case "release"
                    strSystemURL = "https://ukvps.protx.com/vspgateway/service/release.vsp"
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
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/vspdirect-register.vsp"
                Case "refund"
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/refund.vsp"
                Case "release"
                    strSystemURL = "https://ukvpstest.protx.com/vspgateway/service/release.vsp"
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
