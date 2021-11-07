Imports System.Data
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Data.SqlClient
Imports siteInclude

Partial Class shop_basket
    Inherits BasePage
    Private _currencySign As String
    Private Const _gvBasket_vatPos As Integer = 4
    Private Const _gvBasket_totalIncPos As Integer = 5
    Private Const _gvBasket_itemPos As Integer = 0
    Private Const _gvBasket_pricePos As Integer = 1
    Private Const _gvBasket_subTotalPos As Integer = 3
    Private Const _gvBasket_qtyPos As Integer = 2

    'System Events
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        'Get currency code from database and place it in the basket for future use
        If Not Page.IsPostBack Then
            Profile.EBCart.CurrencySign = getCurrencySign()
        Else
            _currencySign = Profile.EBCart.CurrencySign
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Clear error
        lblError.Text = ""
        If Not Page.IsPostBack Then
            bindBasket()
            If Not basketHasItems() Then btnContinue.Visible = False
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then loadDBResources()
    End Sub

    'Page Events
    Protected Sub gvBasket_load(ByVal sender As Object, ByVal e As EventArgs)
        gvBasket.AlternatingRowStyle.BackColor = Drawing.Color.White
    End Sub
    Protected Sub gvBasket_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If basketHasItems() Then
            bindVoucher()
            Dim lblRowSubTotal As Label
            Dim lblProductCode As Label
            Dim txtQty As TextBox
            Dim btnDeleteItem As ImageButton
            Dim subTotal As Decimal = 0
            Dim voucherDiscount As Decimal = 0
            Dim couponDiscount As Decimal = 0
            For Each row As GridViewRow In gvBasket.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    lblRowSubTotal = row.FindControl("lblSubtotal")
                    subTotal = subTotal + CType(lblRowSubTotal.Text, Decimal)
                End If
            Next
            Dim fr As GridViewRow = gvBasket.FooterRow
            Dim lblShippingCost As Label = fr.FindControl("lblShippingCost")
            Dim lblVatCost As Label = fr.FindControl("lblVatCost")
            Dim lblTotalCost As Label = fr.FindControl("lblTotalCost")
            Dim lblVatRate As Label = fr.FindControl("lblVatRate")
            Dim lblVoucherDiscount As Label = fr.FindControl("lblVoucherDiscountCost")
            Dim lblCouponDiscountCost As Label = fr.FindControl("lblCouponDiscountCost")
            'voucherDiscount = CType(lblVoucherDiscount.Text, Decimal)
            'couponDiscount = CType(lblCouponDiscountCost.Text, Decimal)
            'If the basket product is 'Customs Charge', then hide the Remove Item button (and remove the Qty input box)
            For Each row As GridViewRow In gvBasket.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    lblProductCode = row.FindControl("lblPearTreeProductCode")
                    If LCase(lblProductCode.Text) = "customs" Then
                        txtQty = row.FindControl("txtQty")
                        btnDeleteItem = row.FindControl("btnDeleteItem")
                        txtQty.Enabled = False
                        btnDeleteItem.Visible = False
                    End If
                End If
            Next
            If False Then
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procShopVatByCountryCodeSelect", oConn)
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
                        Dim row As DataRow = ds.Tables(0).Rows(0)
                        lblShippingCost.Text = _currencySign & "|" & FormatNumber(row("shippingRate"), 2)
                        lblVatRate.Text = "(" & FormatNumber(row("vatRate"), 1) & "%)"
                        'lblVatCost.Text = FormatNumber((((subTotal + lblShippingCost.Text - voucherDiscount) / 100) * row("vatrate")), 2)
                        'lblTotalCost.Text = FormatNumber((subTotal + lblShippingCost.Text + lblVatCost.Text) - couponDiscount - voucherDiscount, 2)
                        'Response.Write(subTotal & " + " & lblShippingCost.Text & " + " & lblVatCost.Text & " - " & couponDiscount)
                    End If
                Catch ex As Exception
                    lblError.Text = "An error occured calculating the basket totals; " & ex.ToString
                Finally
                    ds.Dispose()
                    da.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            End If
            updateShipping()
            'Basket cannot have Gift Voucher and normal products in basket. (Also test for consultancy voucher)
            testForVoucher()
        End If
    End Sub
    Protected Sub btnDeleteItem_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As ImageButton = CType(sender, ImageButton)
        Dim tmp As String = ""
        tmp = getDBResourceString("imgRemove")
        If tmp <> "" Then btn.ImageUrl = trimCrap(tmp)
    End Sub

    'User Events
    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim txt As TextBox
        Dim lbl As Label
        For Each row As GridViewRow In gvBasket.Rows
            If row.RowType = DataControlRowType.DataRow Then
                txt = row.FindControl("txtQty")
                lbl = row.FindControl("lblOldQty")
                If (CType(txt.Text, Integer) <> CType(lbl.Text, Integer)) And CType(txt.Text, Integer) <> 0 Then
                    'update qty for current row
                    Profile.EBCart.UpdateItem(txt.Text, CType(gvBasket.DataKeys(row.RowIndex).Value, String))
                End If
            End If
        Next
        'rebind basket
        bindBasket()
        updateShipping()
    End Sub
    Protected Sub gvBasket_rowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Profile.EBCart.RemoveItem(CType(gvBasket.DataKeys(e.RowIndex).Value, String))
        'If basketHasItems Then
        'Server.Transfer("basket.aspx")
        'Else
        'Server.Transfer("basket.aspx")
        'Response.Redirect("~/shopIntro.aspx")
        'End If
        bindBasket()
        checkVoucherStillValid()
        bindBasket()
        If Not basketHasItems() Then btnContinue.Visible = False
    End Sub
    Protected Sub btnContinueShopping_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("~/shopIntro.aspx")
    End Sub
    Protected Sub btnAddVoucher_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim imgBtn As ImageButton = gvBasket.FooterRow.FindControl("btnAddVoucher")
        Dim lblPrefix As Label = gvBasket.FooterRow.FindControl("lblVoucherPrefix")
        Dim txtNumber As TextBox = gvBasket.FooterRow.FindControl("txtVoucherNumber")
        Dim btnSubmit As ImageButton = gvBasket.FooterRow.FindControl("btnVoucherSubmit")
        lblPrefix.Text = "<font size='2'>" & CType(GetLocalResourceObject("lblEnterVoucher"), String) & "&nbsp;&nbsp;&nbsp;&nbsp;</font><font size='2' color='black'>" & UCase(Session("EBLanguage")) & "</font>"
        txtNumber.Visible = True
        imgBtn.Visible = False
        btnSubmit.Visible = True
    End Sub
    Protected Sub btnVoucherSubmit_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procVoucherByNumberSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        'Dim txtNumber As TextBox = gvBasket.FooterRow.FindControl("txtVoucherNumber")
        'Dim lblVoucherError As Label = gvBasket.FooterRow.FindControl("lblVoucherError")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@number", SqlDbType.VarChar, 10))
            .Parameters("@number").Value = txtVoucherNumber.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            Dim dt As DataTable = ds.Tables(0)
            If dt.Rows.Count > 0 Then
                'Voucher found
                'Test to see if its valid, or out of date
                Dim bCoupon As Boolean = CType(dt.Rows(0)("coupon"), Boolean)
                Dim bAddToBasket As Boolean = True
                Dim endDate As Date = Now().Date 'Use todays date for all single use vouchers, as they have no expiry date
                If Not IsDBNull(dt.Rows(0)("endDate")) Then endDate = CType(dt.Rows(0)("endDate"), Date)
                Dim credit As Decimal = dt.Rows(0)("credit")
                Dim active As Boolean = CType(dt.Rows(0)("active"), Boolean)
                If DateDiff(DateInterval.Day, Now().Date, endDate) < 0 Then
                    lblVoucherError.Text = "<font color='red'>" & CType(GetLocalResourceObject("errExpired"), String) & "</font>"
                Else
                    If Not active Then
                        lblVoucherError.Text = "<font color='red'>" & CType(GetLocalResourceObject("errInactive"), String) & "</font>"
                    Else
                        'A coupon can only be added to the basket if an associated product is in the shopping basket
                        If bCoupon Then
                            'Test for associated items
                            Dim bAssociationExists As Boolean = False
                            For Each row As GridViewRow In gvBasket.Rows
                                If Not bAssociationExists Then
                                    bAssociationExists = isProductAssociated(gvBasket.DataKeys(row.RowIndex).Value, txtVoucherNumber.Text)
                                End If
                                bAddToBasket = bAssociationExists
                            Next
                            If Not bAssociationExists Then
                                lblVoucherError.Text = "<font color='red'>" & CType(GetLocalResourceObject("errVoucherNotAssociated"), String) & " " & getAssociatedProductList(txtVoucherNumber.Text) & "</font>"
                            End If
                        End If
                        If bAddToBasket Then
                            'Voucher is valid - 
                            'Profile.EBCart.AddItem("vvv", "Voucher", credit * -1, 0, credit * -1, 0, 0)
                            Profile.EBCart.AddVoucher(txtVoucherNumber.Text, CType(dt.Rows(0)("coupon"), Boolean), credit)
                            'Hide voucher boxes
                            'Dim imgBtn As ImageButton = gvBasket.FooterRow.FindControl("btnAddVoucher")
                            'Dim lblPrefix As Label = gvBasket.FooterRow.FindControl("lblVoucherPrefix")
                            'Dim btnVoucherSubmit As ImageButton = gvBasket.FooterRow.FindControl("btnVoucherSubmit")
                            'Dim btnAddVoucher As ImageButton = gvBasket.FooterRow.FindControl("btnAddVoucher")
                            Dim lblVoucherDiscountCost As Label = gvBasket.FooterRow.FindControl("lblVoucherDiscountCost")
                            Dim lblVoucherLineBreak1 As Label = gvBasket.FooterRow.FindControl("lblVoucherLineBreak1")
                            'lblPrefix.Visible = False
                            'txtVoucherNumber.Visible = False
                            'imgBtn.Visible = False
                            'btnVoucherSubmit.Visible = False
                            'btnAddVoucher.Visible = False
                            lblVoucherDiscountCost.Visible = True
                            lblVoucherLineBreak1.Visible = True
                            tblVoucher.Visible = False
                            bindBasket()
                            loadDBResources()
                            lblVoucherError.Text = ""
                        End If
                    End If
                End If
            Else
                'Doesnt exist
                lblVoucherError.Text = "<font color='red'>" & CType(GetLocalResourceObject("errNotExist"), String) & "</font>"
            End If
        Catch ex As Exception
            lblVoucherError.Text = lblVoucherError.Text & "<br>Error occured; " & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnContinue_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        'Store cart object as binary data in EBCart table, as well as other nessessary Session variables
        Dim sID As Integer = storeData()
        If sID > 0 Then
            Profile.EBCart.emptyBasket()
            If Application("isDevBox") Then
                Response.Redirect("http://www.eb-dev.com/shop/payment.aspx?sid=" & sID)
            Else
                Response.Redirect("https://secure.emotionalbliss.com/shop/payment.aspx?sid=" & sID)
            End If

        Else
            lblError.Text = "Session/Cart data could not be stored."
        End If
    End Sub

    'Subs
    Protected Sub bindBasket()
        If Not Profile.EBCart Is Nothing Then
            gvBasket.DataSource = Profile.EBCart.Items
            gvBasket.DataBind()
        End If
        addShippingToTotal()
    End Sub
    Protected Sub bindVoucher()
        If Profile.EBCart.VoucherNumber <> "" Then
            Dim lblDiscount As Label
            Dim lblDiscountCost As Label
            Dim lblDiscountLineBreak As Label
            Dim lblCouponVatTotal As Label
            Dim lblTotalIncVatCoupon As Label
            Dim lblVat As Label
            Dim lblCurrency1 As Label
            Dim lblCurrency2 As Label
            Dim lblCurrency3 As Label
            Dim lblCurrency4 As Label
            If Profile.EBCart.VoucherIsCoupon Then
                'Show the coupon labels
                lblDiscount = gvBasket.FooterRow.FindControl("lblCouponDiscount")
                lblDiscountCost = gvBasket.FooterRow.FindControl("lblCouponDiscountCost")
                lblDiscountLineBreak = gvBasket.FooterRow.FindControl("lblVoucherLineBreak2")
                lblCouponVatTotal = gvBasket.FooterRow.FindControl("lblCouponVatTotal")
                lblVat = gvBasket.FooterRow.FindControl("lblCouponOrderVat")
                lblVat.Visible = True
                lblTotalIncVatCoupon = gvBasket.FooterRow.FindControl("lblTotalIncVatCoupon")
                lblCouponVatTotal.Visible = True
                lblTotalIncVatCoupon.Visible = True
                lblTotalIncVatCoupon.Text = FormatNumber(Profile.EBCart.TotalInc, 2)
                lblCouponVatTotal.Text = FormatNumber(Profile.EBCart.CouponVat, 2)
                'Hide Vat And Other total column
                gvBasket.Columns(_gvBasket_vatPos).Visible = False
                gvBasket.Columns(_gvBasket_totalIncPos).Visible = False
            Else
                'Show the voucher labels
                lblDiscount = gvBasket.FooterRow.FindControl("lblVoucherDiscount")
                lblDiscountCost = gvBasket.FooterRow.FindControl("lblVoucherDiscountCost")
                lblDiscountLineBreak = gvBasket.FooterRow.FindControl("lblVoucherLineBreak1")
                'Show Vat And Other total column
                gvBasket.Columns(_gvBasket_vatPos).Visible = True
                gvBasket.Columns(_gvBasket_totalIncPos).Visible = True
            End If
            lblDiscount.Visible = True
            lblDiscountCost.Visible = True
            lblDiscountLineBreak.Visible = True
            lblDiscountCost.Text = "<nobr>" & FormatNumber(Profile.EBCart.VoucherCredit, 2) & "</nobr>"
        Else
            gvBasket.Columns(_gvBasket_vatPos).Visible = True
            gvBasket.Columns(_gvBasket_totalIncPos).Visible = True
        End If
    End Sub
    Protected Sub updateShipping()
        Dim lblShippingCost As Label = gvBasket.FooterRow.FindControl("lblShippingCost")
        Dim lblShippingTotalCost As Label = gvBasket.FooterRow.FindControl("lblShippingTotalCost")
        Dim lblShippingVatRate As Label = gvBasket.FooterRow.FindControl("lblShippingVatRate")
        setShipping(lblShippingCost, lblShippingTotalCost, lblShippingVatRate)
    End Sub
    Protected Sub addShippingToTotal()
        If basketHasItems() Then
            Dim lblTotalIncVat As Label = gvBasket.FooterRow.FindControl("lblTotalIncVat")
            lblTotalWeight.Text = Profile.EBCart.TotalWeight & "g"
            Try
                lblTotalIncVat.Text = Profile.EBCart.CurrencySign & Profile.EBCart.TotalInc
            Catch ex As Exception
                lblTotalIncVat.Text = "Err#"
            End Try
        End If
    End Sub
    Protected Sub setShipping(ByRef lblShippingCost As Label, ByRef lblShippingTotalCost As Label, ByRef lblShippingVatRate As Label)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShippingWeightByCountryWeightSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim cost As String = "0.00"
        Dim vatRate As String = "0.0"
        Dim totalCost As String = "0.00"
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@weight", SqlDbType.Int))
            .Parameters("@countryCode").Value = Session("EBShopCountry")
            .Parameters("@weight").Value = Profile.EBCart.TotalWeight
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                cost = FormatNumber(ds.Tables(0).Rows(0)("price"), 2)
                vatRate = FormatNumber(ds.Tables(0).Rows(0)("VatRate"), 1)
                totalCost = FormatNumber(ds.Tables(0).Rows(0)("price") + ds.Tables(0).Rows(0)("shippingVatCost"), 2)
                addShippingToCouponTotal(cost, vatRate, totalCost)
            End If
        Catch ex As Exception
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        lblShippingCost.Text = Profile.EBCart.CurrencySign & cost
        lblShippingVatRate.Text = vatRate
        lblShippingTotalCost.Text = Profile.EBCart.CurrencySign & totalCost
        'Update shipping costs within EBCart
        Profile.EBCart.setShipping(cost, vatRate)
    End Sub
    Protected Sub addShippingToCouponTotal(ByVal cost As Decimal, ByVal vatRate As Decimal, ByVal totalCost As Decimal)
        Dim lblCouponVatTotal As Label = gvBasket.FooterRow.FindControl("lblCouponVatTotal")
        Dim lblTotalIncVatCoupon As Label = gvBasket.FooterRow.FindControl("lblTotalIncVatCoupon")
        Dim lblCouponDiscountCost As Label = gvBasket.FooterRow.FindControl("lblCouponDiscountCost")
        Try
            lblCouponVatTotal.Text = CDec(lblCouponVatTotal.Text) + totalCost - cost
            'Show currency signs
            lblCouponVatTotal.Text = Profile.EBCart.CurrencySign & lblCouponVatTotal.Text
            lblTotalIncVatCoupon.Text = Profile.EBCart.CurrencySign & lblTotalIncVatCoupon.Text
            lblCouponDiscountCost.Text = Profile.EBCart.CurrencySign & lblCouponDiscountCost.Text
        Catch ex As Exception
            lblCouponVatTotal.Text = "#Err"
        End Try
        Try
            'lblTotalIncVatCoupon.Text = CDec(lblTotalIncVatCoupon.Text) + totalCost
        Catch ex As Exception
            lblTotalIncVatCoupon.Text = "#Err"
        End Try
    End Sub
    Protected Sub checkVoucherStillValid()
        'If there is a coupon in the basket, then make sure there are still objects in the basket that the voucher can be used on
        Dim isAssociated As Boolean = False
        If Profile.EBCart.VoucherNumber <> "" Then
            If Profile.EBCart.VoucherIsCoupon Then
                For Each row As GridViewRow In gvBasket.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        If isProductAssociated(gvBasket.DataKeys(row.RowIndex).Value, Profile.EBCart.VoucherNumber) Then isAssociated = True
                    End If
                Next
                If Not isAssociated Then Profile.EBCart.removeVoucher()
                bindVoucher()
            End If
        End If
    End Sub
    Protected Sub testForVoucher()
        'No longer needed, as Deferred payment is used on all orders to ensure funds are available
        If False Then
            Dim bVoucherInBasket As Boolean = False
            Dim bNonVoucherInBasket As Boolean = False
            Dim lblPearTreeProductCode As Label
            For Each row As GridViewRow In gvBasket.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    lblPearTreeProductCode = row.FindControl("lblPearTreeProductCode")
                    If Len(lblPearTreeProductCode.Text) > 2 Then
                        If Left(LCase(lblPearTreeProductCode.Text), 2) = "v_" Or Left(LCase(lblPearTreeProductCode.Text), 2) = "vc" Then
                            'V_' signifies a voucher, 'VC' is for a consultancy voucher
                            bVoucherInBasket = True
                        Else
                            bNonVoucherInBasket = True
                        End If
                    End If
                Else
                    bNonVoucherInBasket = True
                End If
            Next
            If bVoucherInBasket And bNonVoucherInBasket Then
                'Vouchers and products found in basket
                'This is not allowed, remove the continue button and show an error message
                lblError.Text = "<font color='red'>You have a voucher and other product in the basket. Vouchers must be purchased seperately.<br>Please remove the voucher or other products in order to proceed.</font>"
            End If
            btnContinue.Visible = Not (bVoucherInBasket And bNonVoucherInBasket)
        End If
        btnContinue.Visible = True
    End Sub
    Protected Sub loadDBResources()
        lblShoppingBasketText.Text = getDBResourceString("lblShoppingBasketText")
        lblHeader.Text = getDBResourceString("lblShoppingBasket")
        If gvBasket.Rows.Count > 0 Then
            Dim lblShipping As Label = gvBasket.FooterRow.FindControl("lblShipping")
            Dim lblVoucherDiscount As Label = gvBasket.FooterRow.FindControl("lblVoucherDiscount")
            Dim lblVAT As Label = gvBasket.FooterRow.FindControl("lblVAT")
            Dim lblCouponDiscount As Label = gvBasket.FooterRow.FindControl("lblCouponDiscount")
            Dim lblCouponOrderVat As Label = gvBasket.FooterRow.FindControl("lblCouponOrderVat")
            Dim lblTotal As Label = gvBasket.FooterRow.FindControl("lblTotal")
            Dim tmp As String = ""
            'Dim reqTxtVoucherNumber As RequiredFieldValidator = gvBasket.FooterRow.FindControl("reqTxtVoucherNumber")
            'Dim regTxtVoucherNumber As RegularExpressionValidator = gvBasket.FooterRow.FindControl("regTxtVoucherNumber")
            'lblShipping.Text = getDBResourceString("lblShipping")
            lblShipping.Text = getDBResourceString("lblShipping")
            lblVoucherDiscount.Text = getDBResourceString("lblVoucherDiscount")
            lblVAT.Text = getDBResourceString("lblVAT")
            lblCouponDiscount.Text = getDBResourceString("lblCouponDiscount")
            lblCouponOrderVat.Text = getDBResourceString("lblCouponOrderVat")
            lblTotal.Text = getDBResourceString("lblTotal")
            lblBasketNote.Text = getDBResourceString("lblBasketNote")
            'lblTotal.Text = "<b>Total</b>"
            'reqTxtVoucherNumber.ErrorMessage = getDBResourceString("errRequired")
            'regTxtVoucherNumber.ErrorMessage = getDBResourceString("errVocuherWrongLength")
            gvBasket.HeaderRow.Cells(_gvBasket_itemPos).Text = getDBResourceString("headerItem")
            gvBasket.HeaderRow.Cells(_gvBasket_pricePos).Text = getDBResourceString("headerPrice")
            gvBasket.HeaderRow.Cells(_gvBasket_qtyPos).Text = getDBResourceString("headerQty")
            gvBasket.HeaderRow.Cells(_gvBasket_subTotalPos).Text = getDBResourceString("headerSubTotal")
            gvBasket.HeaderRow.Cells(_gvBasket_vatPos).Text = getDBResourceString("headerVat")
            gvBasket.HeaderRow.Cells(_gvBasket_totalIncPos).Text = getDBResourceString("headerTotal")
            reqTxtVoucherNumber.ErrorMessage = "<br>* Voucher number required"
            regTxtVoucherNumber.ErrorMessage = "<br>* Voucher number must be 8 digits"
            tmp = getDBResourceString("imgProceed")
            If tmp <> "" Then btnContinue.ImageUrl = trimCrap(tmp)
            tmp = getDBResourceString("imgUpdateBasket")
            If tmp <> "" Then btnUpdate.ImageUrl = trimCrap(tmp)
            tmp = getDBResourceString("imgContinueShopping")
            If tmp <> "" Then btnContinueShopping.ImageUrl = trimCrap(tmp)
            tmp = getDBResourceString("imgSubmit")
            If tmp <> "" Then btnVoucherSubmit.ImageUrl = trimCrap(tmp)
            'gvBasket.HeaderRow.Cells(_gvBasket_itemPos).Text = "Item"
            'gvBasket.HeaderRow.Cells(_gvBasket_pricePos).Text = "Price"
            'gvBasket.HeaderRow.Cells(_gvBasket_qtyPos).Text = "Qty"
            'gvBasket.HeaderRow.Cells(_gvBasket_subTotalPos).Text = "Subtotal"
            'gvBasket.HeaderRow.Cells(_gvBasket_vatPos).Text = "Vat(%)"
            'gvBasket.HeaderRow.Cells(_gvBasket_totalIncPos).Text = "Total"
        End If
    End Sub

    'Functions
    Protected Function getSitePrefix() As String
        Dim result As String = ""
        Dim arr As Array = Split(Request.ServerVariables("SERVER_NAME"), ".")
        Return result
    End Function
    Function basketHasItems() As Boolean
        Return CType(Profile.EBCart.Items.Count > 0, Boolean)
    End Function
    Protected Function isProductAssociated(ByVal posID As Integer, ByVal voucherNumber As String) As Boolean
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procVoucherPosLinkByVoucherSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim bResult As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@posID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@voucherNumber", SqlDbType.VarChar, 10))
            .Parameters("@posID").Value = posID
            .Parameters("@voucherNumber").Value = voucherNumber
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then bResult = True
        Catch ex As Exception
            Dim si As siteInclude
            si.addError("shop/basket.aspx.vb", "isProductAssociated::" & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return bResult
    End Function
    Protected Function getAssociatedProductList(ByVal voucherNumber As String) As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procVoucherPosLinkByVoucherProducsSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = ""
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@voucherNumber", SqlDbType.VarChar, 10))
            .Parameters("@voucherNumber").Value = voucherNumber
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    result = result & row("saleName") & ","
                Next
                result = Left(result, Len(result) - 1)
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/basket.aspx", "getAssociatedProductList('" & voucherNumber & ")::" & ex.ToString)
            result = "Error retreiving associated product list for voucher '" & voucherNumber & "'"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function getCurrencySign() As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCurrencyByCounrtyCodeSelect2", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = ""
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = Session("EBShopCountry")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = ds.Tables(0).Rows(0)("currencySign")
        Catch ex As Exception
            Try
                Dim si As New siteInclude
                si.addError("shop/basket.aspx", "getCurrencySign(Session('EBlanguage')=" & Session("EBShopCountry") & ");" & ex.ToString)
                si = Nothing
            Catch ex2 As Exception
            End Try
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function storeData() As Integer
        Dim aData() As Byte
        Dim stream As New MemoryStream()
        Dim bFormatter As New BinaryFormatter()
        bFormatter.Serialize(stream, Profile.EBCart)
        aData = stream.ToArray
        Dim sessionID As Integer = 0
        Dim clickID As Integer = 0
        If Session("EBAffClickThroughID") <> "" Then clickID = Session("EBAffClickThroughID")
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEBCartInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@data", SqlDbType.VarBinary, 4000))
            .Parameters.Add(New SqlParameter("@EBLanguage", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@EBShopCountry", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@EBAffClickThroughID", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters("@data").Value = aData
            .Parameters("@EBLanguage").Value = Session("EBLanguage")
            .Parameters("@EBShopCountry").Value = Session("EBShopCountry")
            .Parameters("@EBAffClickThroughID").Value = clickID
            .Parameters("@id").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            sessionID = oCmd.Parameters("@id").Value
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/basket.aspx.vb", "storeData();" & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
            Try
                bFormatter = Nothing
                stream.Close()
                stream.Dispose()
            Catch ex2 As Exception
            End Try
        End Try
        Return sessionID
    End Function
    Protected Function trimCrap(ByVal fck As String) As String
        Dim result As String = Replace(fck, "<p>", "")
        result = Replace(result, "</p>", "")
        Return result
    End Function
End Class
