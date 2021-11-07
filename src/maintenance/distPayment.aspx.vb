Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class maintenance_distPayment
    Inherits System.Web.UI.Page
    'Private drpMainCountry As DropDownList
    Private emailBody As String = ""
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        _login = Master.FindControl("logMaintenance")
        _content = _login.FindControl("ContentPlaceholder1")
    End Sub
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
    Protected Sub btnSubmitShipBill_click(ByVal sender As Object, ByVal e As EventArgs)
        Validate()
        If Page.IsValid Then
            'All ok, store details in db
            commitDetails()
            Response.Redirect("distReceipt.aspx", False)
            'Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('s_redirect()',200)", True)
        End If
    End Sub
    Protected Sub btnSubmitBill_click(ByVal sender As Object, ByVal e As EventArgs)
        Validate()
        If Page.IsValid Then
            'All ok, store details in db
            commitDetails()
            Response.Redirect("distreceipt.aspx", False)
            'Server.Transfer("receipt.aspx", True)
            'Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('s_redirect()',200)", True)
        End If
    End Sub
    Protected Sub txtBillCountry_load(ByVal sender As Object, ByVal e As EventArgs)
        'Grab Affiliate Address from DB
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters("@affID").Value = Session("EBTmpMaintenanceDistBasket_DistID")
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
                txtShipCountry.Text = txtBillCountry.Text
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
    Protected Sub commitDetails()
        Dim Type As String = ""
        Dim affID = CType(Session("EBTmpMaintenanceDistBasket_DistID"), Integer)
        'Dim prefix As String = getPrefixForAffiliateType(Session("EBTmpMaintenanceDistBasket_DistID"))
        Dim prefix As String
        Dim orderCountry As String
        Dim userOrderID As String
        Dim customerID As Integer = 0
        Dim si As New siteInclude
        If chkAccount.Checked Then
            Type = "distaccount"
        End If
        If chkCC.Checked Then
            Type = "distcc"
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
            .Parameters.Add(New SqlParameter("@cardCv2", SqlDbType.VarChar, 3))
            .Parameters.Add(New SqlParameter("@useBillAdd", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@orderType", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@accountNo", SqlDbType.VarChar, 12))
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@dob", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@gender", SqlDbType.VarChar, 10))
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
            .Parameters("@dob").Value = ""
            .Parameters("@gender").Value = ""
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
        'Add 'Order complete, thankyou' msg to order log
        si = New siteInclude
        si.AddToOrderLog(orderID, "Order Placed, Thankyou.", "System", True, "Emailed")
        si = Nothing
        'Set Purchase Order No
        Profile.EBAffCart.PurchaseOrderNo = txtPurchaseOrderNo.Text
        'Send email confirmation
        sendConfirmationEmail(userOrderID, orderID, "confirmation")
        'Profile.EBAffCart.emptyBasket() 'Empty the basket
        'Page.ClientScript.RegisterClientScriptBlock(Me.GetType, "window", "alert('doh!!!');", True)
    End Sub
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
            Case "distcc"
                orderSource = "distributor"
                paymentMethod = "cc"
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
            .Parameters("@orderCountryCode").Value = "zz"
            .Parameters("@customerID").Value = customerID
            .Parameters("@orderPrefix").Value = "50"
            .Parameters("@clickThroughID").Value = clickID
            .Parameters("@currency").Value = "gbp"
            .Parameters("@newOrderID").Direction = ParameterDirection.Output
            .Parameters("@affiliateID").Value = Session("EBTmpMaintenanceDistBasket_DistID")
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
                    'si.affAddToStatement(Session("EBTmpMaintenanceDistBasket_DistID"), 0, calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), ID, 0, 2)
                    'Now add some credit to distributors statement
                    'si.affAddToStatement(GetDistributorID(Session("EBAffEBDistributorCountryCode")), calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), 0, ID, 0, 2)
                Case 50
                    'Add order to distributors statement
                    'si.affAddToStatement(Session("EBTmpMaintenanceDistBasket_DistID"), 0, calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), ID, 0, 1)
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
            .Parameters("@affiliateID").Value = Session("EBTmpMaintenanceDistBasket_DistID")
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
                    si.affAddToStatement(Session("EBTmpMaintenanceDistBasket_DistID"), 0, calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), ID, 0, 2)
                    'Now add some credit to distributors statement
                    si.affAddToStatement(GetDistributorID(orderCountry), calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), 0, ID, 0, 2)
                Case 50
                    'Add order to distributors statement
                    si.affAddToStatement(Session("EBTmpMaintenanceDistBasket_DistID"), 0, calcOrderTotal(Profile.EBAffCart.TotalInc, shipping, shippingVatRate), ID, 0, 1)
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
    Protected Function getCard(ByVal t As String) As String
        'Dim cp As ContentPlaceHolder = Master.FindControl("ContentPlaceHolder1")
        Dim m As DropDownList = _content.FindControl("drp" & t & "Month")
        Dim y As DropDownList = _content.FindControl("drp" & t & "Year")
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
    Sub btnGetOut_click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("distReceipt.aspx", False)
    End Sub
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
        emailBody = emailBody & "has been despatched showing the relevant delivery details." & Chr(13) & Chr(13)
        'emailBody = emailBody & "You can also monitor the progress of your order by clicking the 'Track your" & Chr(13)
        'emailBody = emailBody & "order'(button located top right of the website <a href='http://www.emotionalbliss.co.uk'>http://www.emotionalbliss.co.uk</a>)" & Chr(13) & Chr(13)
        'emailBody = emailBody & "Simply enter your surname and invoice number as listed below." & Chr(13) & Chr(13)
        'emailBody = emailBody & "All credit card orders will only be debited on despatch." & Chr(13) & Chr(13)
        'emailBody = emailBody & "All cheque orders will be despatched on cleared funds." & Chr(13) & Chr(13)
        'emailBody = emailBody & "(UK)0870 041 00 22" & Chr(13) & "(Europe) 0044 870 041 0022" & Chr(13) & Chr(13)
        emailBody = emailBody & "INVOICE NUMBER       : " & newOrderID & "GB" & Chr(13)
        emailBody = emailBody & "ORDER DATE AND TIME  : " & FormatDateTime(Now(), DateFormat.ShortDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime) & Chr(13) & Chr(13)
        emailBody = emailBody & "QUANTITY   "
        emailBody = emailBody & "PRODUCT(DESCRIPTION)" & Chr(13)
        emailBody = emailBody & "--------------------------------------------------------" & Chr(13)
        Dim item
        For Each item In Profile.EBAffCart.Items
            emailBody = emailBody & item.Qty & "          "
            emailBody = emailBody & item.Name & Chr(13)
        Next
        emailBody = emailBody & "--------------------------------------------------------" & Chr(13) & Chr(13)
        emailBody = emailBody & "Order Total: £" & FormatNumber(CDec(Profile.EBAffCart.TotalEx), 2) & " (exc VAT and Delivery charges)" & Chr(13) & Chr(13)
        emailBody = emailBody & "If you require any further information please call Paul Telford on +44 7753 818118" & Chr(13) & Chr(13)
        'emailBody = emailBody & "<a href='http://www.emotionalbliss.co.uk'>http://www.emotionalbliss.co.uk</a>"
        subject = "Order Confirmation"
    End Sub
End Class
