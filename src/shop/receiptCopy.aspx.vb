Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude
Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient

Partial Class shop_receiptCopy
    Inherits BasePage
    Private Const _gvBasket_productPos As Integer = 0
    Private Const _gvBasket_qtyPos As Integer = 1
    Private Const _gvBasket_unitPricePos As Integer = 2
    Private Const _gvBasket_rowPricePos As Integer = 4
    Private emailBody As String = ""
    Protected currencySign As String = ""
    Private Const _debug As Boolean = False

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Only hide lblThanks if questionaire is to be shown, for the moment only on GB
            If LCase(Session("EBShopCountry")) = "gb" Then
                lblThanks.Visible = False
            Else
                panSurvey.Visible = False
            End If
            'lblPrefix.Text = Context.Items("orderPrefix")
            'lblOrderID.Text = Context.Items("orderID")
            'lblPrefix.Text = Session("EBTmpOrderPrefix")
            lblOrderID.Text = Session("EBTmpOrderID") & UCase(Session("EBShopCountry"))
            hidNewOrderID.Value = Session("EBTmpOrderID")
            gvBasket.DataSource = Profile.EBCart.Items
            gvBasket.DataBind()
            Dim pkID As Integer = getOrderIDByNewOrderID(Session("EBTmpOrderID"), UCase(Session("EBShopCountry")))
            Dim ice As ICEPAY.ICEPAY
            ice = New ICEPAY.ICEPAY(Application("icepayMerchantID"), Application("icepayMerchantCode"))
            If Session("EBTmpOrderPrefix") = "25" Then
                If ice.GetData.orderID <> Nothing Then
                    pkID = Request.QueryString("reference")
                    'Icepay order (DDEBIT or IDEAL)
                    'Add Icepays orderID and TransID to shopCustomer table
                    Try
                        Dim sDate As String = FormatDateTime(Now(), DateFormat.LongDate)
                        Dim status As String = "paid"
                        If LCase(Request.QueryString("PaymentMethod")) = "ddebit" Then
                            status = "PaymentPending"
                            sDate = String.Empty
                        End If
                        Dim param() As String = {"@id", "@icepayOrderID", "@icepayTransID", "@icepayPaymentID", "@orderStatus", "@paymentDate"}
                        Dim paramValue() As String = {Request.QueryString("reference"), Request.QueryString("orderID"), Request.QueryString("transactionID"), Request.QueryString("paymentID"), status, sDate}
                        Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.BigInt, SqlDbType.VarChar, SqlDbType.DateTime}
                        Dim paramSize() As Integer = {0, 10, 50, 0, 20, 0}
                        siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procShopCustomerByOrderIDIcepayUpdate")
                    Catch ex As Exception
                        siteInclude.addError("shop/receipt.aspx.vb", "Page_Load(id=" & Request.QueryString("reference") & "); " & ex.ToString)
                    Finally
                    End Try
                    'Show complete text
                    lblComplete.Text = CType(GetLocalResourceObject("lblPaymentComplete4"), String)
                    Select Case LCase(Request.QueryString("status"))
                        Case "ok"
                            lblPaymentResults.Text = CType(getDBResourceString("lblComplete4"), String)
                        Case "err"
                            lblPaymentResults.Text = CType(getDBResourceString("lblPaymentResults3"), String)
                        Case Else
                            lblPaymentResults.Text = CType(getDBResourceString("lblPaymentResults4"), String)
                    End Select
                    'lblComplete.Text = CType(GetLocalResourceObject("lblPaymentComplete4"), String)
                    lblComplete2.Text = getDBResourceString("lblComplete2")
                ElseIf Request.QueryString("auth") = "paypal" Then
                    'Paypal order, funds have been taken and transaction details stored
                    'Show complete text
                    lblComplete.Text = CType(GetLocalResourceObject("lblPaymentComplete4"), String)
                    lblPaymentResults.Text = CType(getDBResourceString("lblComplete4"), String)
                Else
                    'Postal order
                    'lblComplete.Text = CType(GetLocalResourceObject("lblPaymentComplete3"), String)
                    lblComplete3.Text = getDBResourceString("lblComplete3")
                End If
            Else
                'Creditcard order
                'Live auth removed, dont show result
                If False Then
                    If CType(Session("EBTmpOrderPaid"), String) <> "" Then
                        'Payment has already been taken, show results
                        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                        Dim oCmd As New SqlCommand("procProtxByNewOrderIDSelect", oConn)
                        Dim da As New SqlDataAdapter
                        Dim ds As New DataSet
                        With oCmd
                            .CommandType = CommandType.StoredProcedure
                            .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                            .Parameters("@newOrderID").Value = Session("EBTmpOrderID")
                            .Parameters("@countryCode").Value = Session("EBShopCountry")
                        End With
                        Try
                            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                            da = New SqlDataAdapter(oCmd)
                            da.Fill(ds)
                            If ds.Tables(0).Rows.Count > 0 Then
                                If LCase(ds.Tables(0).Rows(0)("status")) = "ok" Then
                                    lblPaymentResults.Text = CType(getDBResourceString("lblPaymentResults1"), String) & " " & ds.Tables(0).Rows(0)("vendorTxCode") & "<br>"
                                    'lblPaymentResults.Text = "Payment complete.<br>Transaction number " & ds.Tables(0).Rows(0)("vendorTxCode") & "<br>"
                                    lblPaymentResults.Text = lblPaymentResults.Text & CType(getDBResourceString("lblPaymentResults2"), String)
                                    'lblPaymentResults.Text = lblPaymentResults.Text & "The voucher has been emailed to the recipient, and a copy has been sent to your email address. <font color='red'><b> STILL TO BE DONE!</b></font>"
                                    lblComplete.Text = CType(getDBResourceString("lblPaymentComplete2"), String)
                                Else
                                    'Payment failed
                                    'If CBool(Session("EBTmpOrderPaid")) Then
                                    'lblPaymentResults.Text = CType(GetLocalResourceObject("lblPaymentResults1"), String)
                                    'lblComplete.Text = CType(GetLocalResourceObject("lblPaymentComplete1"), String)
                                    'End If
                                    lblPaymentResults.Text = CType(getDBResourceString("lblPaymentResults3"), String)
                                    lblComplete.Text = CType(getDBResourceString("lblPaymentComplete1"), String)
                                End If
                            End If
                        Catch ex As Exception
                            Response.Write(ex)
                            Response.End()
                        End Try
                    End If
                End If
                lblComplete4.Text = getDBResourceString("lblComplete4")
            End If
            If Session("EBTmpVoucherNumber") <> "" Then
                'Show voucher numbers if orderwas voucher only
                lblCompleteVoucher.Text = getDBResourceString("lblCompleteVoucher") & " <b>" & Session("EBTmpVoucherNumber") & "</b>"
            End If
            'sendConfirmationEmail(lblOrderID.Text, pkID, "confirmation")
            loadDBResources()
            getCustomerAddress()
        End If
    End Sub
    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Unload
        If Not _debug Then
            'Empty basket
            Profile.EBCart.emptyBasket()
            'Clear tmp session data
            Session("EBTmpOrderPrefix") = ""
            Session("EBTmpOrderID") = ""
            Session("EBTmpOrderType") = ""
            Session("EBTmpOrderPaid") = ""
            Session("EBTmpUniqueID") = ""
            Session("EBTmpIDealStatus") = ""
            Session("EBTmpIDealTx") = ""
            Session("EBAffClickThroughID") = ""
            Session("EBTmpVoucherNumber") = ""
            'Remove basket from database
            removeCart(Session("EBsID"))
            Session("EBsID") = ""
        End If
    End Sub

    'Page Events
    Protected Sub gvBasket_Load(ByVal sender As Object, ByVal e As EventArgs)
        Try
            currencySign = Profile.EBCart.CurrencySign
            'Dim lblTotal As Label = gvBasket.FooterRow.FindControl("lblTotal")
            'lblTotal.ForeColor = Drawing.Color.Black
            'gvBasket.FooterRow.Cells(_gvBasket_productPos).Text = CType(GetLocalResourceObject("_cbLblShipping"), String) & "<br>"
            'gvBasket.FooterRow.Cells(_gvBasket_productPos).Text = gvBasket.FooterRow.Cells(_gvBasket_productPos).Text & CType(GetLocalResourceObject("_cbLblVAT"), String) & "<br>"
            'gvBasket.FooterRow.Cells(_gvBasket_productPos).Text = gvBasket.FooterRow.Cells(_gvBasket_productPos).Text & "<b>" & CType(GetLocalResourceObject("_cbLblTotal"), String) & "</b>"
            'gvBasket.FooterRow.Cells(_gvBasket_rowPricePos).Text = "0.00<br>" & Profile.EBCart.TotalInc - Profile.EBCart.TotalEx & "<br><b>" & Profile.EBCart.TotalInc & "</b>"
            Dim lblShippingCostEx As Label = gvBasket.FooterRow.FindControl("lblShippingCostEx")
            Dim lblShippingCostInc As Label = gvBasket.FooterRow.FindControl("lblShippingCostInc")
            Dim lblVatCost As Label = gvBasket.FooterRow.FindControl("lblVatCost")
            Dim lblTotalCost As Label = gvBasket.FooterRow.FindControl("lblTotalCost")
            Dim lblVoucherDiscountText As Label = gvBasket.FooterRow.FindControl("lblVoucherDiscountText")
            Dim lblCouponDiscountText As Label = gvBasket.FooterRow.FindControl("lblCouponDiscountText")
            Dim lblVoucherDiscount As Label = gvBasket.FooterRow.FindControl("lblVoucherDiscount")
            Dim lblCouponDiscount As Label = gvBasket.FooterRow.FindControl("lblCouponDiscount")
            Dim trCoupon As HtmlTableRow = gvBasket.FooterRow.FindControl("trCoupon")
            Dim trCouponAmount As HtmlTableRow = gvBasket.FooterRow.FindControl("trCouponAmount")
            Dim trVoucher As HtmlTableRow = gvBasket.FooterRow.FindControl("trVoucher")
            Dim trVoucherAmount As HtmlTableRow = gvBasket.FooterRow.FindControl("trVoucherAmount")
            Dim lblTmp As New Label
            Dim VoucherDiscount As Decimal = 0
            If Profile.EBCart.VoucherNumber <> "" Then VoucherDiscount = Profile.EBCart.VoucherCredit
            'setShipping(lblShippingCostEx, lblShippingCostInc, lblTmp)
            lblShippingCostEx.Text = currencySign & Profile.EBCart.Shipping
            lblShippingCostInc.Text = currencySign & Profile.EBCart.ShippingTotal
            lblVatCost.Text = currencySign & FormatNumber(CStr((Profile.EBCart.GoodsVat) + (CDec(Profile.EBCart.ShippingTotal) - CDec(Profile.EBCart.Shipping))), 2)
            lblTotalCost.Text = currencySign & Profile.EBCart.TotalInc
            If VoucherDiscount <> 0 Then
                'Show discount rows
                If Profile.EBCart.VoucherIsCoupon Then
                    trCoupon.Visible = True
                    trCouponAmount.Visible = True
                    lblCouponDiscountText.Visible = True
                    lblCouponDiscount.Text = currencySign & VoucherDiscount & "<br>"
                Else
                    trVoucher.Visible = True
                    trVoucherAmount.Visible = True
                    lblVoucherDiscountText.Visible = True
                    lblVoucherDiscount.Text = currencySign & VoucherDiscount & "<br>"
                End If
            End If
        Catch ex As Exception
            'Session must have timed out
            lblPaymentResults.Text = CType(GetLocalResourceObject("errSessionTimeoutMsg"), String)
            lblReceipt.Text = CType(GetLocalResourceObject("errSessionTimeout"), String)
            lblOrderNo.Visible = False
        End Try
    End Sub
    Protected Sub gvBasket_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Check for a consultancy voucher in the basket (Its product code will start with 'VC'
        For Each row As GridViewRow In gvBasket.Rows
            Dim lblProdCode As Label
            If row.RowType = DataControlRowType.DataRow Then
                lblProdCode = row.FindControl("lblProductCode")
                If Len(lblProdCode.Text) > 2 Then
                    If LCase(Left(lblProdCode.Text, 2)) = "vc" Then
                        'Consulatncy voucher found, create it in database and show voucher number to the user onscreen
                        Dim replys As String = Mid(lblProdCode.Text, 3, 1)
                        createConsultancyVoucher(replys)
                    End If
                End If
            End If
        Next
    End Sub


    'User Events
    Protected Sub btnBackHome_click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("/homeIntro.aspx")
    End Sub
    Protected Sub drpWhere_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        siteInclude.addItemToDropdown(CType(sender, DropDownList), "Please Choose....", "")
    End Sub
    Protected Sub drpWhere_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Update db
        'siteInclude.debug("drpWhere changed")
        'siteInclude.debug("hidNewOrderID=" & hidNewOrderID.Value)
        'siteInclude.debug("hidNewOrderID=" & Session("EBShopCountry"))
        'siteInclude.debug("surveyQ1=" & drpWhere.SelectedValue)

        Try
            Dim param() As String = {"@newOrderID", "@countryCode", "@surveyQ1", "@sourceQ1"}
            Dim paramValue() As String = {hidNewOrderID.Value, Session("EBShopCountry"), drpWhere.SelectedValue, "web"}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar}
            Dim paramSize() As Integer = {0, 5, 50, 20}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procShopOrderByNewOrderIDSurveyQ1Update")
        Catch ex As Exception
            siteInclude.addError("callcentre/receipt.aspx.vb", "drpWhere_selectedIndexChanged(newOrderID=" & Session("EBTmpOrderID") & ", country=" & Session("EBShopCountry") & "); " & ex.ToString())
        End Try
        'Clean up
        panSurvey.Visible = False
        lblThanks.Visible = True
    End Sub

    'Subs
    Protected Sub fixAddress(ByVal id As String, ByRef fv As FormView)
        Dim lbl As Label = fv.FindControl("lblBill" & id)
        If Not lbl Is Nothing Then
            If lbl.Text = "<br>" Then lbl.Visible = False
            lbl = fv.FindControl("lblShip" & id)
            If lbl.Text = "<br>" Then lbl.Visible = False
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
            .Parameters("@countryCode").Value = Session("EBLanguage")
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
            End If
        Catch ex As Exception
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        lblShippingCost.Text = cost
        lblShippingVatRate.Text = vatRate
        lblShippingTotalCost.Text = totalCost
    End Sub
    Protected Sub createConsultancyVoucher(ByVal replys As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procConsultancyCreateInsert", oConn)
        Dim cardName As String = ""
        Dim email As String = ""
        Dim id As Integer = 0
        Dim bError As Boolean = False
        Dim code As String = ""
        getNameAndEmail(cardName, email, id, Session("EBTmpOrderID"))
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@cardName", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@replys", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100))
            .Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 10))
            .Parameters("@countryCode").Value = Session("EBLanguage")
            .Parameters("@cardName").Value = cardName
            .Parameters("@orderID").Value = id
            .Parameters("@replys").Value = replys
            .Parameters("@email").Value = email
            .Parameters("@code").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            code = oCmd.Parameters("@code").Value
            lblConsultancy.Text = lblConsultancy.Text & "Your unique Consultancy Voucher number is " & code & ".<br>Details will be emailed to <font color='blue'>" & email & "</font>"
            lblConsultancy.Text = lblConsultancy.Text & "<br><br>Or you get get started right now by clicking <a href='../consultancy.aspx?code=" & code & "'>here</a>."
        Catch ex As Exception
            bError = True
            Try
                Dim si As New siteInclude
                si.addError("shop/receipt.aspx.vb", "createConsultancyVoucher(" & replys & "); " & ex.ToString & "<br>")
                si = Nothing
                lblConsultancy.Text = lblConsultancy.Text & "<font color='red'>An error occured whilest creating your consultancy voucher. Please contact <a href='mailto:support@emotionalbliss.com'>support@emotionalbliss.com</a> including your order number.<br>Please accept our appologies. We will resolve the problem as soon as possible."
            Catch ex1 As Exception
            End Try
        End Try
        If Not bError Then
            sendConfirmationEmailVoucher(cardName, email, code)
        End If
    End Sub
    Protected Sub getNameAndEmail(ByRef cn As String, ByRef e As String, ByRef orderID As Integer, ByVal newOrderID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim ocmd As New SqlCommand("procShopOrderByNewIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet

        With ocmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@newOrderID").Value = newOrderID
            .Parameters("@countryCode").Value = Session("EBLanguage")
        End With
        Try
            If ocmd.Connection.State = 0 Then ocmd.Connection.Open()
            da = New SqlDataAdapter(ocmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                cn = ds.Tables(0).Rows(0)("billName")
                e = ds.Tables(0).Rows(0)("email")
                orderID = ds.Tables(0).Rows(0)("id")
            End If
        Catch ex As Exception
            Try
                Dim si As New siteInclude
                si.addError("shop/receipt.aspx.vb", "getNameAndEmail(" & orderID & "); " & ex.ToString)
                si = Nothing
                lblConsultancy.Text = lblConsultancy.Text & "<font color='red'>An error occured whilest creating your consultancy voucher. Please contact <a href='mailto:support@emotionalbliss.com'>support@emotionalbliss.com</a> including your order number.<br>Please accept our appologies. We will resolve the problem as soon as possible.<br>"
            Catch ex1 As Exception
            End Try
        Finally
            ds.Dispose()
            da.Dispose()
            ocmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub sendConfirmationEmailVoucher(ByVal cardname As String, ByVal email As String, ByVal voucherNo As String)
        If voucherNo <> "" Then
            Dim toAdd As String = email
            Dim ccAdd As String = ""
            Dim subject As String = "EB Consultacy"
            Dim msg As MailMessage
            Dim chk As CheckBox
            Dim plainView As AlternateView
            Dim htmlView As AlternateView
            Dim emailBody As String = "This is an automated email from EmotionalBliss." & Chr(13) & Chr(13)
            emailBody = emailBody & "Your unique Consultancy ID is " & voucherNo & Chr(13) & Chr(13)
            emailBody = emailBody & "Please visit <a href='http://www.emotionalbliss.co.uk/consultancy.aspx'>http://www.emotionalbliss.co.uk/consultancy.aspx</a> to begin your sessions." & Chr(13) & Chr(13) & Chr(13)
            emailBody = emailBody & "Emotional Bliss"
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
            siteInclude.sendSQLEmail(toAdd, ccAdd, "", subject, "noreply@emotionalbliss.com", "Emotional Bliss", Replace(emailBody, Chr(10), "<br>"), siteInclude._emailType.emailHtml)
        End If
    End Sub
    Protected Sub removeCart(ByVal sid As String)
        If sid <> "" Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procEBCartByIDDelete", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
                .Parameters("@id").Value = sid
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Dim si As New siteInclude
                si.addError("shop/receipt.aspx.vb", "removeCart(" & sid & "); Could not be removed; " & ex.ToString)
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Sub loadDBResources()
        'lblReceipt.Text = getDBResourceString("lblReceipt")
        lblOrderNo.Text = getDBResourceString("lblOrderNo")
        lblShoppingBasketText.Text = getDBResourceString("lblShoppingBasketText")
        If gvBasket.Rows.Count > 0 Then
            Dim lblVAT As Label = gvBasket.FooterRow.FindControl("lblVAT")
            Dim lblVoucherDiscountText As Label = gvBasket.FooterRow.FindControl("lblVoucherDiscountText")
            Dim lblTotal As Label = gvBasket.FooterRow.FindControl("lblTotal")
            Dim lblCouponDiscountText As Label = gvBasket.FooterRow.FindControl("lblCouponDiscountText")
            Dim lblShipping As Label = gvBasket.FooterRow.FindControl("lblShipping")
            Dim tmp As String = ""
            lblYourReceiptText.Text = getDBResourceString("lblYourReceiptText")
            lblVAT.Text = getDBResourceString("lblVAT")
            lblVoucherDiscountText.Text = getDBResourceString("lblVoucherDiscountText")
            lblTotal.Text = getDBResourceString("lblTotal")
            lblCouponDiscountText.Text = getDBResourceString("lblCouponDiscountText")
            lblShipping.Text = getDBResourceString("lblShipping")
            lblDate.Text = FormatDateTime(Now(), DateFormat.LongDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime)
            lblDateText.Text = getDBResourceString("lblDateText")
            lblBillingText.Text = getDBResourceString("lblBillingText")
            lblShippingText.Text = getDBResourceString("lblShippingText")
            lnkBackHome.Text = getDBResourceString("lnkBackHome")
            lnkPrint.Text = getDBResourceString("lnkPrint")
            tmp = getDBResourceString("imgBackHome")
            If tmp <> "" Then imgBackHome.ImageUrl = trimCrap(tmp)
            tmp = getDBResourceString("imgPrint")
            If tmp <> "" Then imgPrint.ImageUrl = trimCrap(tmp)
            'imgBackHome.Attributes.Add("onclick", "document.location='/homeIntro.aspx';")
            'imgPrint.Attributes.Add("onclick", "openPrintPop(1);")
            gvBasket.HeaderRow.Cells(_gvBasket_productPos).Text = getDBResouceString("lblProduct")
            gvBasket.HeaderRow.Cells(_gvBasket_qtyPos).Text = getDBResouceString("lblQty")
            gvBasket.HeaderRow.Cells(_gvBasket_unitPricePos).Text = getDBResouceString("lblUnitPrice")
            gvBasket.HeaderRow.Cells(_gvBasket_rowPricePos).Text = getDBResouceString("lblRowPrice")
        End If

    End Sub
    Protected Sub addShippingToTotal()
        Dim lblTotalIncVat As Label = gvBasket.FooterRow.FindControl("lblTotalIncVat")
        Dim lblShippingTotalCost As Label = gvBasket.FooterRow.FindControl("lblShippingTotalCost")
        Try
            lblTotalIncVat.Text = CStr(CDec(lblTotalIncVat.Text) + CDec(lblShippingTotalCost.Text))
        Catch ex As Exception
            lblTotalIncVat.Text = "Err#"
        End Try
    End Sub
    Protected Sub getCustomerAddress()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderDetailsByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@orderID").Value = Session("EBTmpUniqueID")
            .Parameters("@countryCode").Value = Session("EBShopCountry")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'Show address
                Dim row As DataRow = ds.Tables(0).Rows(0)
                addData("name", row)
                addData("add1", row)
                addData("add2", row)
                addData("add3", row)
                addData("add4", row)
                addData("add5", row)
                addData("postcode", row)
                addData("country", row)
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/receipt.aspx", "getCustomerAddress(orderID=" & Session("EBTmpUniqueID") & "); " & ex.ToString)
            si = Nothing
        End Try
    End Sub
    Protected Sub addData(ByVal s, ByVal r)
        If Not IsDBNull(r("bill" + s)) Then If r("bill" + s) <> "" Then lblBillAddress.Text = lblBillAddress.Text & r("bill" + s) & "<br>"
        If Not IsDBNull(r("ship" + s)) Then If r("ship" + s) <> "" Then lblShipAddress.Text = lblShipAddress.Text & r("ship" + s) & "<br>"
    End Sub
    Protected Sub sendConfirmationEmail(ByVal newOrderID As String, ByVal orderID As Integer, ByVal type As String)
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
        Try
            'client.Send(msg)
        Catch ex As Exception

        Finally
            msg.Dispose()
        End Try
        siteInclude.sendSQLEmail(toAdd, ccAdd, "", subject, "noreply@emotionalbliss.com", "Emotional Bliss", Replace(emailBody, Chr(10), "<br>"), siteInclude._emailType.emailHtml)
    End Sub
    Protected Sub makeEmail(ByVal emailType As String, ByVal newOrderID As String, ByVal orderID As Integer, ByRef toAdd As String, ByRef ccAdd As String, ByRef subject As String, ByVal country As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailMasterByCountryCodeTypeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim row As DataRow
        Dim email As String = getEmailByOrderID(orderID)
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
                toAdd = email
                subject = row("emailSubject") 'Set Subject from db
                For Each r As DataRow In ds.Tables(0).Rows
                    'Set email body
                    emailBody = emailBody & r("text")
                Next
                If LCase(emailType) = "order confirmation" Then
                    'Normal order                    
                    'Replace special commands in email body
                    'emailBody = Replace(emailBody, "@orderID", newOrderID & UCase(LCase(Session("EBShopCountry"))))
                    emailBody = Replace(emailBody, "@orderID", newOrderID)
                    emailBody = Replace(emailBody, "@date", FormatDateTime(Now(), DateFormat.ShortDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime))
                    emailBody = Replace(emailBody, "@billingAdd", getAddress(orderID, "bill"))
                    emailBody = Replace(emailBody, "@shippingAdd", getAddress(orderID, "ship"))
                    emailBody = Replace(emailBody, "@items", getItemText())
                    emailBody = Replace(emailBody, "@totalPrice", Profile.EBCart.CurrencySign & FormatNumber(CDec(Profile.EBCart.TotalInc), 2))
                Else
                    'Voucher order
                    'If txtEmail="" AND the order is for a voucher, then use the purchaser's email address from the voucher
                    'If LCase(email) = "noreply@emotionalbliss.com" And purchaserEmail <> "" Then toAdd = purchaserEmail
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
                    '_voucherNumber = row("number")
                    'purchaserEmail = row("purchaserEmail")
                End If
            Else
                'No results were found, this means the email has not been created for the current country yet.
                'Use GB as default
                makeEmail(emailType, newOrderID, orderID, toAdd, ccAdd, subject, "gb")
            End If
        Catch ex As Exception
            siteInclude.addError("shop/receipt.aspx.vb", "makeEmail(orderID=" & orderID & ",emailType=" & emailType & ",countryCode=" & Session("EBShopCountry") & "); " & ex.ToString())
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
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
            siteInclude.addError("shop/receipt.aspx.vb", "getAddress(id=" & id & ", type=" & addType & "); " & ex.ToString)
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
    Protected Function trimCrap(ByVal fck As String) As String
        Dim result As String = Replace(fck, "<p>", "")
        result = Replace(result, "</p>", "")
        Return result
    End Function
    Protected Function getEmailByOrderID(ByVal id As Integer) As String
        Dim result As String = "noreply@emotionalbliss.com"
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@orderid"}
            Dim paramValue() As String = {id.ToString}
            Dim paramType() As SqlDbType = {SqlDbType.Int}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procShopCustomerByOrderIDSelect")
            If dt.Rows.Count > 0 Then result = dt.Rows(0)("email")
        Catch ex As Exception
            siteInclude.addError("shop/receipt.aspx.vb", "getEmailByOrderID(); " & ex.ToString())
        Finally
            dt.Dispose()
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
            siteInclude.addError("shop/receipt.aspx.vb", "getVoucherDetails(orderID=" & orderID & "); " & ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return row
    End Function
    Protected Function getOrderIDByNewOrderID(ByVal newOrderID As String, ByVal countryCode As String) As Integer
        Dim dt As New DataTable
        Dim result As Integer = 0
        Try
            Dim param() As String = {"@newOrderID", "@countryCode"}
            Dim paramValue() As String = {newOrderID, countryCode}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar}
            Dim paramSize() As Integer = {0, 5}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procShopOrderByNewIDSelect")
            If dt.Rows.Count > 0 Then result = dt.Rows(0)("id")
        Catch ex As Exception
            siteInclude.addError("shop/receipt.aspx", "getOrderIDByNewOrderID(); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
        Return result
    End Function
End Class

