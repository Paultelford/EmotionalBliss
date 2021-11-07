Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class maintenance_orderView
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private _fv As FormView
    Private Const _gvHistory_statusPos As Integer = 6
    Private Const _gvHistory_amountPos As Integer = 2
    Private Const _gvHistory_vendorTxPos As Integer = 8
    Private Const _gvOrderItems_qtyParamPos As Integer = 0
    Private Const _gvOrderItems_vatParamPos As Integer = 1
    Private Const _gvOrderItems_costParamPos As Integer = 2
    Private Const _gvOrderItems_qtyPos As Integer = 2
    Private Const _gvOrderItems_vatPos As Integer = 4
    Private Const _gvOrderItems_costPos As Integer = 6
    Private Const _drpStatus_cancelledPos As Integer = 0
    Private Const _drpStatus_placedPos As Integer = 1
    Private Const _drpStatus_paidPos As Integer = 2
    Private Const _drpStatus_failedPos As Integer = 3
    Private Const _drpStatus_pendingPos As Integer = 4
    Private Const _drpStatus_completePos As Integer = 5
    Private Const _gvBasket_qtyPos As Integer = 4
    Private Const _gvBasket_stockPos As Integer = 8
    Private Const _gvBasket_editPos As Integer = 13
    Private Const _dvCosts_shippingPos As Integer = 2
    Private _newQty As Integer = 0
    Private _newCost As Decimal = 0
    Private _newVat As Decimal = 0
    Private Const _countryCode As String = "zz"

    Protected Sub Page_Load(ByVal sener As Object, ByVal e As EventArgs) Handles Me.Load
        _login = Master.FindControl("logMaintenance")
        _content = _login.FindControl("ContentPlaceholder1")
        _fv = _content.FindControl("fvOrder")
    End Sub
    Protected Sub Page_LoadComplete(ByVal sener As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            Dim fv As FormView = _content.FindControl("fvOrder")
            Dim lblType As Label = fv.FindControl("lblType")
            Dim panPayment As Panel
            Dim panCC As Panel = fv.FindControl("panCardDetails")
            Dim lblStatus As Label = fv.FindControl("lblStatus")
            Dim txtCheque As TextBox = fv.FindControl("txtCheque")
            Dim btnChSubmit As Button = fv.FindControl("btnChequeSubmit")
            Dim btnChequeCleared As Button = fvOrder.FindControl("btnChequeCleared")
            Dim txtAccount As TextBox = fv.FindControl("txtAccount")
            Dim lblReadyToScan As Label = fv.FindControl("lblReadyToScan")
            Dim btnAccountSubmit As Button = fv.FindControl("btnAccountSubmit")
            Dim lblAffReadyToScan As Label = fv.FindControl("lblAffReadyToScan")
            Dim btnAffAccountSubmit As Button = fv.FindControl("btnAffAccountSubmit")
            Dim txtIDeal As TextBox = fv.FindControl("txtIDeal")
            Dim btnIDealSubmit As Button = fv.FindControl("btnIDealSubmit")
            Dim btnIDealPaymentCleared As Button = fvOrder.FindControl("btnIDealPaymentCleared")
            Dim tdProtx As HtmlTableCell = fv.FindControl("tdProtx")
            Dim tdProtxExtra As HtmlTableCell = fv.FindControl("tdProtxExtra")
            Dim panLog As Panel = fv.FindControl("panLog")
            'Set Order Type text
            If True Then
                Select Case LCase(lblType.Text)
                    Case "cc"
                        lblType.Text = "Credit Card"
                        panPayment = fv.FindControl("panPaymentCC")
                        panCC.Visible = True
                        If LCase(lblStatus.Text) = "paid" Or LCase(lblStatus.Text) = "complete" Then
                            tdProtx.Visible = False
                        End If
                        tdProtxExtra.Visible = Not tdProtx.Visible
                    Case "affcc"
                        lblType.Text = "Aff Credit Card"
                        panPayment = fv.FindControl("panPaymentCC")
                        panCC.Visible = True
                        If lblStatus.Text = "Paid" Then
                            tdProtx.Visible = False
                        End If
                        tdProtxExtra.Visible = Not tdProtx.Visible
                    Case "cheque"
                        lblType.Text = "Cheque"
                        panPayment = fv.FindControl("panPaymentCheque")
                        If LCase(lblStatus.Text) = "placed" Then
                            'Show the cheque number textbox and submit button
                            txtCheque.Visible = True
                            btnChSubmit.Visible = True
                        End If
                        If LCase(lblStatus.Text) = "paymentpending" Then
                            btnChequeCleared.Visible = True
                        End If
                    Case "account"
                        lblType.Text = "Account"
                        panPayment = fv.FindControl("panPaymentAccount")
                        If LCase(lblStatus.Text) = "placed" Then
                            'txtAccount.Visible = True
                            btnAccountSubmit.Visible = True
                        End If
                    Case "ideal"
                        lblType.Text = "iDeal"
                        panPayment = fv.FindControl("panPaymentIDeal")
                        If LCase(lblStatus.Text) = "placed" Then
                            'Show the cheque number textbox and submit button
                            txtIDeal.Visible = True
                            btnIDealSubmit.Visible = True
                        End If
                        If LCase(lblStatus.Text) = "inprogress" Then
                            'Show 'payment complete' button, clicking it will change status from inrpgress to paid.
                            btnIDealPaymentCleared.Visible = True
                        End If
                    Case "affaccount"
                        lblType.Text = "AffAccount"
                        panPayment = fv.FindControl("panAffAccountPayment")
                        If LCase(lblStatus.Text) = "placed" Then
                            'txtAccount.Visible = True
                            btnAffAccountSubmit.Visible = True
                        Else
                            lblAffReadyToScan.Visible = True
                        End If
                    Case "distaccount"
                        lblType.Text = "Dist Account"
                        panPayment = fv.FindControl("panAffAccountPayment")
                        If LCase(lblStatus.Text) = "placed" Then
                            'txtAccount.Visible = True
                            btnAffAccountSubmit.Visible = True
                        Else
                            lblAffReadyToScan.Visible = True
                        End If
                    Case "distcc"
                        lblType.Text = "Dist Credit Card"
                        panPayment = fv.FindControl("panPaymentCC")
                        panCC.Visible = True
                        If lblStatus.Text = "Paid" Then
                            tdProtx.Visible = False
                        End If
                        tdProtxExtra.Visible = Not tdProtx.Visible
                End Select
                panPayment.Visible = True
                'Set Status dropdown's available options (depending on order status)
                setStatusDropdown()
                'Hide payment buttons if order is cancelled
                Dim drpStatus As DropDownList = fvOrder.FindControl("drpStatus")
                If drpStatus.SelectedValue = "cancelled" Then
                    panPayment.Visible = False
                End If
                lblPaymentPanelName.Text = panPayment.ID
            Else
                'No datat returned, order does not exist or belongs to another country
                lblDenied.Text = "The order you are trying to view does not exist, or belongs to another country."
            End If
        End If
    End Sub
    Protected Sub btnProtx_click(ByVal sender As Object, ByVal e As EventArgs)
        'For now just update the order status to Paid
        'Attempt protx transaction
        Dim status As String = protxTestCode(False)
        If LCase(status) <> "failed" Then Response.Redirect("orderView.aspx?id=" & Request.QueryString("ID"))
    End Sub
    Protected Sub btnChequeSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add the cheque number to the shopCustomer table and set the order status to Paid
        '(In future change status to InProgress while cheque clears, and add button to change status to Paid once cheque clears)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderCustomerChequeStatusUpdate", oConn)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim txtCheque As TextBox = fvOrder.FindControl("txtCheque")
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        Dim lblChequeNo As Label = fvOrder.FindControl("lblChequeNo")
        Dim btnChequeSubmit As Button = fvOrder.FindControl("btnChequeSubmit")
        Dim btnChequeCleared As Button = fvOrder.FindControl("btnChequeCleared")
        Dim panLog As Panel = fvOrder.FindControl("panLog")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@chequeNo", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@chequeNo").Value = txtCheque.Text
            .Parameters("@orderID").Value = hidOrderID.Value
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Add details to order log
        Dim si As New siteInclude
        Dim userName As String = "Unknown"
        Try
            userName = Membership.GetUser.UserName
        Catch ex As Exception
        End Try
        si.AddToOrderLog(hidOrderID.Value, "Cheque " & txtCheque.Text & " recieved.", userName, True)
        si = Nothing
        txtCheque.Visible = False
        lblStatus.Text = "PaymentPending"
        lblChequeNo.Text = txtCheque.Text
        btnChequeSubmit.Visible = False
        btnChequeCleared.Visible = True
        panLog.DataBind()
    End Sub
    Protected Sub btnChequeCleared_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderCustomerChequePaidUpdate", oConn)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        Dim lblStatusUpdate As Label = fvOrder.FindControl("lblStatusUpdate")
        Dim drpStatus As DropDownList = fvOrder.FindControl("drpStatus")
        Dim btnChequeCleared As Button = fvOrder.FindControl("btnChequeCleared")
        Dim panLog As Panel = fvOrder.FindControl("panLog")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@status").Value = "Paid"
            .Parameters("@orderID").Value = hidOrderID.Value
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Add details to order log
        Dim si As New siteInclude
        Dim userName As String = "Unknown"
        Try
            userName = Membership.GetUser.UserName
        Catch ex As Exception
        End Try
        si.AddToOrderLog(hidOrderID.Value, "Payment cleared.", userName, True)
        si = Nothing
        btnChequeCleared.Visible = False
        drpStatus.Items(_drpStatus_paidPos).Enabled = True
        lblStatus.Text = "Paid"
        lblStatusUpdate.Text = "<font color='red'>Status updated to 'Paid'</font>"
        drpStatus.SelectedValue = "Paid"
        panLog.DataBind()
    End Sub
    Protected Sub btnAccountSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add the account number to the shopCustomer table and set the order status to Paid
        '(In future change status to InProgress while cheque clears, and add button to change status to Paid once cheque clears)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderCustomerAccountStatusUpdate", oConn)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim lblReadyToScan As Label = fvOrder.FindControl("lblReadyToScan")
        Dim btnAccountSubmit As Button = fvOrder.FindControl("btnAccountSubmit")
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        Dim panLog As Panel = fvOrder.FindControl("panLog")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@accountNo", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters("@accountNo").Value = ""
            .Parameters("@orderID").Value = hidOrderID.Value
            .Parameters("@status").Value = "Paid"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Add details to order log
        Dim si As New siteInclude
        Dim userName As String = "Unknown"
        Try
            userName = Membership.GetUser.UserName
        Catch ex As Exception
        End Try
        si.AddToOrderLog(hidOrderID.Value, "Payment complete.", userName, True)
        si = Nothing
        btnAccountSubmit.Visible = False
        lblStatus.Text = "Paid"
        panLog.DataBind()
    End Sub
    Protected Sub btnIDealSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add the account number to the shopCustomer table and set the order status to Paid
        '(In future change status to InProgress while cheque clears, and add button to change status to Paid once cheque clears)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderCustomerIDealStatusUpdate", oConn)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim txtIDeal As TextBox = fvOrder.FindControl("txtIDeal")
        Dim lblIDealNo As Label = fvOrder.FindControl("lblIDealNo")
        Dim btnIDealSubmit As Button = fvOrder.FindControl("btnIDealSubmit")
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        Dim btnIDealPaymentCleared As Button = fvOrder.FindControl("btnIDealPaymentCleared")
        Dim panLog As Panel = fvOrder.FindControl("panLog")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@iDealNo", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters("@iDealNo").Value = txtIDeal.Text
            .Parameters("@orderID").Value = hidOrderID.Value
            .Parameters("@status").Value = "PaymentPending"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        lblIDealNo.Text = txtIDeal.Text
        txtIDeal.Visible = False
        btnIDealSubmit.Visible = False
        btnIDealPaymentCleared.Visible = True
        lblStatus.Text = "InProgress"
        panLog.DataBind()
    End Sub
    Protected Sub btnIDealPaymentCleared_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderCustomerIDealPaidUpdate", oConn)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim txtIDeal As TextBox = fvOrder.FindControl("txtIDeal")
        Dim lblIDealNo As Label = fvOrder.FindControl("lblIDealNo")
        Dim btnIDealSubmit As Button = fvOrder.FindControl("btnIDealSubmit")
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        Dim btnIDealPaymentCleared As Button = fvOrder.FindControl("btnIDealPaymentCleared")
        Dim panLog As Panel = fvOrder.FindControl("panLog")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters("@orderID").Value = hidOrderID.Value
            .Parameters("@status").Value = "paid"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Add details to order log
        Dim si As New siteInclude
        Dim userName As String = "Unknown"
        Try
            userName = Membership.GetUser.UserName
        Catch ex As Exception
        End Try
        si.AddToOrderLog(hidOrderID.Value, "iDeal Treansaction complete.", userName, True)
        si = Nothing
        txtIDeal.Visible = False
        btnIDealSubmit.Visible = False
        lblStatus.Text = "Paid"
        btnIDealPaymentCleared.Visible = False
        panLog.DataBind()
    End Sub
    Protected Sub btnAffAccountSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add the account number to the shopCustomer table and set the order status to Paid
        '(In future change status to InProgress while cheque clears, and add button to change status to Paid once cheque clears)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderCustomerAccountStatusUpdate", oConn)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim lblAffReadyToScan As Label = fvOrder.FindControl("lblAffReadyToScan")
        Dim btnAffAccountSubmit As Button = fvOrder.FindControl("btnAffAccountSubmit")
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        Dim panLog As Panel = fvOrder.FindControl("panLog")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@accountNo", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters("@accountNo").Value = ""
            .Parameters("@orderID").Value = hidOrderID.Value
            .Parameters("@status").Value = "Paid"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Add details to order log
        Dim si As New siteInclude
        Dim userName As String = "Unknown"
        Try
            userName = Membership.GetUser.UserName
        Catch ex As Exception
        End Try
        si.AddToOrderLog(hidOrderID.Value, "Payment complete.", userName, True)
        si = Nothing
        lblAffReadyToScan.Visible = True
        btnAffAccountSubmit.Visible = False
        lblStatus.Text = "Paid"
        Dim drpStatus As DropDownList = fvOrder.FindControl("drpStatus")
        'Once order is Paid, the only option is to change it to Cancelled. (So said Craig on 5-9-08)
        For Each item As ListItem In drpStatus.Items
            item.Enabled = False
        Next
        drpStatus.Items(_drpStatus_paidPos).Enabled = True
        drpStatus.Items(_drpStatus_cancelledPos).Enabled = True
        drpStatus.SelectedValue = "Paid"
        panLog.DataBind()
    End Sub
    Protected Function showDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d.ToString, DateFormat.LongDate) & " " & FormatDateTime(d.ToString, DateFormat.ShortTime)
        Return result
    End Function
    Protected Sub fvOrder_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Add orderID to the Edit Link
        Dim fv As FormView = CType(sender, FormView)
        If Not fv.Row Is Nothing Then
            Dim lnkEdit As HyperLink = fv.FindControl("lnkEdit")
            Try
                lnkEdit.NavigateUrl = "~/affiliates/orderEdit.aspx?id=" & Request.QueryString("id")
            Catch ex As Exception
            End Try
            'Remove blank address lines from Shipping and Billing addresses
            fixAddress("Name", fv)
            fixAddress("Add1", fv)
            fixAddress("Add2", fv)
            fixAddress("Add3", fv)
            fixAddress("Add4", fv)
            fixAddress("Add5", fv)
            fixAddress("Postcode", fv)
            fixAddress("Country", fv)
            'Show the correct Protx button (The protx 'extra' info should be shown if order is 'complete' or 'paid')
            Dim lblStatus As Label = fv.FindControl("lblStatus")
            Dim tdProtx As HtmlTableCell = fv.FindControl("tdProtx")
            Dim tdProtxExtra As HtmlTableCell = fv.FindControl("tdProtxExtra")
            If LCase(lblStatus.Text) = "complete" Or LCase(lblStatus.Text) = "complete" Then tdProtxExtra.Visible = True
            tdProtx.Visible = Not tdProtxExtra.Visible
        End If
    End Sub
    Protected Sub fixAddress(ByVal id As String, ByRef fv As FormView)
        Dim lbl As Label = fv.FindControl("lblBill" & id)
        If Not lbl Is Nothing Then
            If lbl.Text = "<br>" Then lbl.Visible = False
            lbl = fv.FindControl("lblShip" & id)
            If lbl.Text = "<br>" Then lbl.Visible = False
        End If
    End Sub
    Protected Sub btnProtxExtra_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("extrapayment")
        If Page.IsValid Then
            Dim panLog As Panel = fvOrder.FindControl("panLog")
            Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
            Dim txtPartAmount As TextBox = fvOrder.FindControl("txtPartAmount")
            Dim drpExtraVat As DropDownList = fvOrder.FindControl("drpExtraVat")
            'Take money
            Dim x As String = protxTestCode(True)
            panLog.DataBind() 'Update transaction gridview
            'Add to sales ledger
            Dim dGoods As Decimal = 0
            Dim dVat As Decimal = 0
            If CBool(drpExtraVat.SelectedValue) Then
                dGoods = FormatNumber(CDec(txtPartAmount.Text) / (1 + (getVatRate() / 100)), 2)
                dVat = CDec(txtPartAmount.Text) - dGoods
            Else
                dGoods = CDec(txtPartAmount.Text)
            End If
            'Add to sales ledger if transaction was successful
            If LCase(x) = "paid" Then
                Dim si As New siteInclude
                si.addToSalesLedger(hidOrderID.Value, 0, 1, _countryCode, 0, dGoods, 0, dVat, 0)
                si = Nothing
            End If
        End If
    End Sub
    Function formatDateElement(ByVal d As String)
        Dim sResult As String = d
        If Len(d) = 1 Then
            sResult = "0" & d
        End If
        Return sResult
    End Function
    Protected Function protxTestCode(ByVal extraPayment As Boolean) As String
        Dim fv As FormView = fvOrder
        Dim txtPartAmount As TextBox = fv.FindControl("txtPartAmount")
        Dim hidOrderID As HiddenField = fv.FindControl("hidOrderID")
        Dim vendorTxCode As String = "EBPAYMENT" & formatDateElement(Day(Date.Today)) & formatDateElement(Month(Date.Today)) & Year(Date.Today) & Hour(Now()) & Minute(Now()) & hidOrderID.Value
        Dim tx As New Protx.Vsp.DirectTransaction(vendorTxCode, Protx.Vsp.VspTransactionType.Payment)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopCustomerPaymentByOrderIDSelect", oConn)
        Dim sStatus As String
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@orderid", SqlDbType.Int))
        oCmd.Parameters("@orderid").Value = hidOrderID.Value
        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
        Dim da As New SqlDataAdapter(oCmd)
        Dim ds As New DataSet
        da.Fill(ds)
        Dim rs As DataRow
        Dim countryCode As String
        Dim bOK As Boolean = False
        Dim vspResponse As Protx.Vsp.DirectResponse
        Dim SI As New siteInclude
        Dim sHTML As String = ""
        Dim lblCCResults As Label = fv.FindControl("lblCCResults")
        Dim bDataError As Boolean = False
        Try
            If ds.Tables(0).Rows.Count > 0 Then
                rs = ds.Tables(0).Rows(0)
                countryCode = rs("orderCountryCode")
                'tx.Amount = Decimal.Parse(rs("ordertotal"))
                tx.Amount = CType(FormatNumber(rs("ordertotal"), 2), Decimal)
                If extraPayment Then tx.Amount = txtPartAmount.Text
                tx.Currency = UCase(rs("orderCurrency")) 'Valid amounts are GBP,EUR,USD
                tx.Description = "EmotionalBliss Order"
                tx.CardHolder = rs("billName")
                'tx.CardNumber = fes.DecryptData(ConfigurationManager.AppSettings("ccKey").ToString, rs("ccEnc"))
                'tx.CardNumber = "4747474747474747"
                tx.CardNumber = decryptCardNo(rs("ccEnc"))
                'tx.ExpiryDate = formatDateElement((rs("ccExpMonth"))) & Right(rs("ccExpYear"), 2)
                tx.ExpiryDate = rs("cardExp")

                'If Not IsDBNull(rs("ccStartMonth")) Then
                'startDate = rs("ccStartMonth") & rs("ccStartYear")
                'If startDate.Length > 0 Then
                ' tx.StartDate = formatDateElement((rs("ccStartMonth"))) & Right(rs("ccStartYear"), 2)
                'End If
                'End If
                If Not IsDBNull(rs("cardStart")) Then If rs("cardStart") <> "" Then tx.StartDate = rs("cardStart")

                'If Not IsDBNull(rs("ccIssue")) Then
                'tx.IssueNumber = rs("ccIssue")
                'End If
                If Not IsDBNull(rs("cardIssue")) Then If rs("cardIssue") <> "" Then tx.IssueNumber = rs("cardIssue")

                If Not IsDBNull(rs("cardCV2")) Then If rs("cardCV2") <> "" Then tx.CV2 = rs("cardCV2")

                tx.CardType = CType(System.Enum.Parse(GetType(Protx.Vsp.VspCardType), rs("cardType"), False), Protx.Vsp.VspCardType)

                tx.BillingAddress = rs("billAdd1")
                tx.BillingPostCode = rs("billPostcode")
                'tx.DeliveryAddress = ""
                'tx.DeliveryPostCode = ""

                tx.CustomerName = rs("billName")
                'tx.ContactNumber = ""
                'tx.ContactFax = ""
                tx.CustomerEMail = Trim(rs("email"))
                'tx.Basket = ""
                tx.GiftAidPayment = False
                tx.ApplyAvsCv2 = CType(System.Int32.Parse(0), Protx.Vsp.ApplyChecksFlag)
                tx.ClientIPAddress = Request.UserHostAddress
                'tx.CAVV = ""
                'tx.XID = ""
                'tx.ECI = ""
                'tx.ThreeDSecureStatus = ""
            End If
        Catch ex As Exception
            'Response.Write(ex)
            lblCCResults.Text = "Invalid data passed to protx component: " & ex.Message & "<br>Check card details and try again."
            lblCCResults.ForeColor = Drawing.Color.Red
            bDataError = True
            sStatus = "Failed"
        Finally
            da.Dispose()
            ds.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try


        'ALL PROTX DETAILS READY FOR TRANSACTION
        If Not bDataError Then
            Try
                vspResponse = tx.Send()
                If (Protx.Vsp.VspStatusType.OK = vspResponse.Status) Then bOK = True
            Catch ex As Exception
                'ROUTING PROBLEM/TIMEOUT FROM PROTX
                'log error with siteInclude, and show error to userwww
                SI.addError("affiliates/orderView.aspx.vb", "protxTextCode(ID=" & hidOrderID.Value & ") Protx transaction did not return anything. Possible timeout/routing probs at protx end; " & ex.ToString)
                sHTML = "<font color='red'>Protx transaction did not return anything. Possible timeout/routing probs at protx end. Check protx server status and try again later.</font>"
            End Try
            Dim userName As String = "Unknown"
            Try
                userName = Membership.GetUser.UserName
            Catch ex As Exception
            End Try
            'Response.Write(vspResponse.VPSProtocol)
            If bOK Then
                Try
                    If bOK Then
                        sHTML = sHTML & "Transaction Complete<br>"
                        'Add transaction result to orderLog
                        SI.AddToOrderLog(hidOrderID.Value, "Card Successfully Charged, txID=" & vendorTxCode, userName, True)
                    Else
                        sHTML = sHTML & "<font color='red'>Error occured</font><br>"
                        SI.AddToOrderLog(hidOrderID.Value, "Transaction attempted, txID=" & vendorTxCode & ".<br>Error Message:" & vspResponse.StatusDetail, userName, True)
                    End If

                    If Not extraPayment Then
                        If (bOK) Then
                            sStatus = "Paid"
                        Else
                            sStatus = "Failed"
                        End If
                    Else
                        Dim drpStatus As DropDownList = fv.FindControl("drpStatus")
                        sStatus = drpStatus.SelectedValue
                        If (bOK) Then sStatus = "Paid"
                    End If
                Catch ex As Exception
                    'Protx.Vsp.VspException
                    'Response.Write(ex)
                    'Response.End()
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
                    oCmd.Parameters("@orderID").Value = CInt(hidOrderID.Value)
                    oCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                    oCmd.Parameters("@currency").Value = tx.Currency
                    oCmd.Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                    oCmd.Parameters("@country").Value = countryCode
                    oCmd.Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
                    oCmd.Parameters("@bank").Value = Convert.ToDecimal("1.0")
                    oCmd.Parameters.Add(New SqlParameter("@newOrderStatus", SqlDbType.VarChar, 10))
                    oCmd.Parameters("@newOrderStatus").Value = sStatus
                    oCmd.Parameters.Add(New SqlParameter("@extraPayment", SqlDbType.Bit))
                    oCmd.Parameters("@extraPayment").Value = extraPayment
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
            Else '!bOK
                'Add the failure to protx table
                sStatus = "Failed"
                Try
                    oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                    oCmd = New SqlCommand("procProtxInsert", oConn)
                    oCmd.CommandType = CommandType.StoredProcedure
                    oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 30))
                    oCmd.Parameters("@vendorTxCode").Value = vendorTxCode
                    oCmd.Parameters.Add(New SqlParameter("@VPSProtocol", SqlDbType.VarChar, 10))
                    oCmd.Parameters("@VPSProtocol").Value = ""
                    oCmd.Parameters.Add(New SqlParameter("@VPSTxID", SqlDbType.VarChar, 40))
                    oCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                    oCmd.Parameters("@status").Value = "Failed"
                    oCmd.Parameters.Add(New SqlParameter("@statusDetail", SqlDbType.VarChar, 255))
                    Try
                        oCmd.Parameters("@statusDetail").Value = vspResponse.StatusDetail
                    Catch ex As Exception
                        SI.addError("affiliatesOrderView.aspx.vb", "testProtxCode(id=" & hidOrderID.Value & "); " & ex.ToString)
                        oCmd.Parameters("@statusDetail").Value = "Possible routing/protx problem. See error log for more info"
                    End Try
                    oCmd.Parameters.Add(New SqlParameter("@txAuthNo", SqlDbType.VarChar, 20))
                    oCmd.Parameters.Add(New SqlParameter("@securityKey", SqlDbType.VarChar, 20))
                    oCmd.Parameters.Add(New SqlParameter("@AVSCV2", SqlDbType.VarChar, 50))
                    oCmd.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money))
                    oCmd.Parameters("@amount").Value = tx.Amount
                    oCmd.Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                    oCmd.Parameters("@orderID").Value = CInt(hidOrderID.Value)
                    oCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                    oCmd.Parameters("@currency").Value = tx.Currency
                    oCmd.Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                    oCmd.Parameters("@country").Value = countryCode
                    oCmd.Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
                    oCmd.Parameters("@bank").Value = Convert.ToDecimal("1.0")
                    oCmd.Parameters.Add(New SqlParameter("@newOrderStatus", SqlDbType.VarChar, 10))
                    oCmd.Parameters.Add(New SqlParameter("@extraPayment", SqlDbType.Bit))
                    oCmd.Parameters("@extraPayment").Value = extraPayment
                    oCmd.Parameters("@newOrderStatus").Value = "Failed"
                    oCmd.Parameters("@VPSTxID").Value = ""
                    oCmd.Parameters("@txAuthNo").Value = ""
                    oCmd.Parameters("@securityKey").Value = ""
                    oCmd.Parameters("@AVSCV2").Value = ""
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
            End If 'bOK
            If bOK Then
                'Show results in lblTransactionResults Label, Page_Load has enabled it
                sHTML = sHTML & "Protocol: " & vspResponse.VPSProtocol & "<br>"
                sHTML = sHTML & "Status: " & vspResponse.Status.ToString() & "<br>"
                sHTML = sHTML & "Response: " & vspResponse.StatusDetail & "<br>"
                If Protx.Vsp.VspStatusType.OK = vspResponse.Status Then
                    sHTML = sHTML & "Auth No: " & vspResponse.TxAuthNo & "<br><br>"
                Else
                    sHTML = sHTML & "Auth No: <br><br>"
                End If
            Else
                Try
                    sHTML = sHTML & vspResponse.StatusDetail.ToString()
                Catch ex As Exception

                End Try
            End If 'bOK
            lblCCResults.Text = sHTML
            lblCCResults.ForeColor = Drawing.Color.Red
            Dim panPaymentResults As Panel = fv.FindControl("panPaymentResults")
            Dim lblStatus As Label = fv.FindControl("lblStatus")
            Dim tdProtx As HtmlTableCell = fv.FindControl("tdProtx")
            Dim tdProtxExtra As HtmlTableCell = fv.FindControl("tdProtxExtra")
            panPaymentResults.Visible = True
            Dim panTransHistory As Panel = fvOrder.FindControl("panTransHistory")
            Dim gvHistory As GridView = fvOrder.FindControl("gvHistory")
            panTransHistory.Visible = True
            gvHistory.DataBind()
            lblStatus.Text = sStatus
            If sStatus = "Paid" Then
                tdProtx.Visible = False
            Else
                tdProtx.Visible = True
            End If
            tdProtxExtra.Visible = Not tdProtx.Visible

        Else '!bDataError()

        End If
        Return sStatus
    End Function
    Protected Sub btnMsgSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim s As Button = CType(sender, Button)
        Dim si As New siteInclude
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim txtNewMessage As TextBox = fvOrder.FindControl("txtNewMessage")
        Dim chkCustomerVisible As CheckBox = fvOrder.FindControl("chkCustomerVisible")
        Dim gvTrace As GridView = fvOrder.FindControl("gvTrace")
        Dim drpContact As DropDownList = fvOrder.FindControl("drpContact")
        Dim userName As String = "Unknown"
        Try
            userName = Membership.GetUser.UserName
        Catch ex As Exception
        End Try
        si.AddToOrderLog(hidOrderID.Value, txtNewMessage.Text, userName, chkCustomerVisible.Checked, drpContact.SelectedValue)
        si = Nothing
        chkCustomerVisible.Checked = False
        txtNewMessage.Text = ""
        gvTrace.DataBind()
        Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('document.getElementById(""tdOrderLog"").style.display="""";document.getElementById(""spanOrderLog"").innerHTML=""Hide Order Log"";',200);", True)
    End Sub
    Protected Sub setStatusDropdown()
        Dim drpStatus As DropDownList = _fv.FindControl("drpStatus")
        Dim lblStatus As Label = _fv.FindControl("lblStatus")
        Dim status As String = lblStatus.Text
        For Each item As ListItem In drpStatus.Items
            If LCase(item.Value) = LCase(status) Then
                item.Selected = True
            Else
                item.Selected = False
            End If
        Next
        'Disable all, then enable depending on new status
        For Each item As ListItem In drpStatus.Items
            item.Enabled = False
        Next
        Select Case LCase(status)
            Case "cancelled"
                drpStatus.Items(_drpStatus_placedPos).Enabled = True
                drpStatus.Items(_drpStatus_cancelledPos).Enabled = True
            Case "placed"
                drpStatus.Items(_drpStatus_cancelledPos).Enabled = True
                drpStatus.Items(_drpStatus_placedPos).Enabled = True
            Case "paid"
                drpStatus.Items(_drpStatus_cancelledPos).Enabled = True
                drpStatus.Items(_drpStatus_paidPos).Enabled = True
            Case "complete"
                drpStatus.Items(_drpStatus_cancelledPos).Enabled = True
                drpStatus.Items(_drpStatus_completePos).Enabled = True
            Case "pending"
                drpStatus.Items(_drpStatus_cancelledPos).Enabled = True
                drpStatus.Items(_drpStatus_placedPos).Enabled = True
                drpStatus.Items(_drpStatus_pendingPos).Enabled = True
            Case "failed"
                drpStatus.Items(_drpStatus_paidPos).Enabled = True
                drpStatus.Items(_drpStatus_failedPos).Enabled = True
                drpStatus.Items(_drpStatus_placedPos).Enabled = True
                drpStatus.Items(_drpStatus_cancelledPos).Enabled = True
        End Select
    End Sub
    Protected Sub drpStatus_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Update orders status to whatever the user just selected
        Dim drpStatus As DropDownList = CType(sender, DropDownList)
        Dim gvTrace As GridView = fvOrder.FindControl("gvTrace")
        Dim lblStatusUpdate As Label = fvOrder.FindControl("lblStatusUpdate")
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim bError As Boolean = False
        Dim lblStatus As Label = _fv.FindControl("lblStatus")
        lblStatus.Text = drpStatus.SelectedItem.Text
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDStatusUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@distStatus", SqlDbType.VarChar, 20))
            .Parameters("@orderID").Value = hidOrderID.Value
            .Parameters("@status").Value = drpStatus.SelectedValue
            .Parameters("@distStatus").Value = ""
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblStatusUpdate.Text = "<font color='red'>An error occured while updating order status.</font>"
            Try
                Dim si As New siteInclude
                si.addError("affiliates/orderView.aspx.vb", "drpStatus_selectedIndexChanged();" & ex.ToString)
            Catch ex2 As Exception
            End Try
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            'fvOrder.DataBind()
            lblStatusUpdate.Text = "<font color='red'>Status changed to '" & formatWord(drpStatus.SelectedValue) & "'.</font>"
            AddToOrderLog(hidOrderID.Value, "Status changed to <b>" & formatWord(drpStatus.SelectedValue), Membership.GetUser.UserName, True, "N/A")
            gvTrace.DataBind()
        End If
        Dim btnChequeCleared As Button = fvOrder.FindControl("btnChequeCleared")
        Dim pan As Panel = fvOrder.FindControl("panPaymentCheque")
        Dim txtCheque As TextBox = fvOrder.FindControl("txtCheque")
        Dim btnChSubmit As Button = fvOrder.FindControl("btnChequeSubmit")
        Dim hidOrderType As HiddenField = _fv.FindControl("hidOrderType")
        Select Case LCase(drpStatus.SelectedValue)
            Case "placed"
                If LCase(hidOrderType.Value) = "cheque" Then
                    'If a chrque order has been changed from cancelled to placed, then show the payment panel and cheque number input box
                    txtCheque.Visible = True
                    btnChSubmit.Visible = True
                End If
                If LCase(hidOrderType.Value) = "distaccount" Then
                    'Distributor account order
                    Dim btnAffAccountSubmit As Button = _fv.FindControl("btnAffAccountSubmit")
                    pan = _fv.FindControl("panAffAccountPayment")
                    Dim lblAffReadyToScan As Label = _fv.FindControl("lblAffReadyToScan")
                    If LCase(lblStatus.Text) = "placed" Then
                        'txtAccount.Visible = True
                        btnAffAccountSubmit.Visible = True
                        Response.Write("A")
                    Else
                        lblAffReadyToScan.Visible = True
                        Response.Write("B")
                    End If
                End If
                If LCase(hidOrderType.Value) = "distcc" Then
                    'Distributor cc order
                    pan = _fv.FindControl("panPaymentCC")
                    Dim tdProtx As HtmlTableCell = _fv.FindControl("tdProtx")
                    Dim tdProtxExtra As HtmlTableCell = _fv.FindControl("tdProtxExtra")
                    If LCase(hidOrderType.Value) = "paid" Then
                        tdProtx.Visible = False
                    End If
                    tdProtxExtra.Visible = Not tdProtx.Visible
                End If
                pan.Visible = True
            Case "paymentpending"
                btnChequeCleared.Visible = True
            Case Else

        End Select
        Dim panPayment As Panel = fvOrder.FindControl(lblPaymentPanelName.Text)
        If LCase(drpStatus.SelectedValue) = "cancelled" Then
            panPayment.Visible = False
        Else
            panPayment.Visible = True
        End If
        'Update the lblStatus with new status, as the setStatusDropDown sub relies on it to set the enabled state of all items in the dropdown
        Try
            setStatusDropdown()
        Catch ex As Exception
            Response.Write(ex.ToString)
        End Try

    End Sub
    Protected Function formatWord(ByVal w As String) As String
        Dim result As String = w
        If Len(w) > 1 Then
            result = UCase(Left(w, 1))
            result = result & LCase(Right(w, Len(w) - 1))
        End If
        Return result
    End Function
    Protected Function decryptCardNo(ByVal encCC As Object) As String
        Dim result As String = ""
        If Not IsDBNull(encCC) Then
            Dim fes As New FE_SymmetricNamespace.FE_Symmetric
            result = fes.DecryptData(ConfigurationManager.AppSettings("aesKey").ToString, encCC.ToString)
        End If
        Return result
    End Function
    Protected Function encryptCard(ByVal n As String) As String
        Dim enc As String = ""
        If Not Left(n, 1) = "*" Then
            Dim fes As New FE_SymmetricNamespace.FE_Symmetric
            enc = fes.EncryptData(ConfigurationManager.AppSettings("aesKey").ToString, n)
        Else
            Dim btnUpdateCCEnc As Button = fvOrder.FindControl("btnUpdateCCEnc")
            btnUpdateCCEnc.Visible = False
        End If
        Return enc
    End Function
    Protected Sub btnUpdateCCEnc_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim lblCardNo As Label = fvOrder.FindControl("lblCardNo")
        Dim orderID As Integer = hidOrderID.Value
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopCustomerByIDEncryptCCUpdate", oConn)
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@customerID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@enc", SqlDbType.VarChar, 50))
            .Parameters("@customerID").Value = getCustomerID(orderID)
            .Parameters("@enc").Value = encryptCard(lblCardNo.Text)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then fvOrder.DataBind()
    End Sub
    Protected Function getCustomerID(ByVal id As Integer) As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopCustomerPaymentByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim cID As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then cID = ds.Tables(0).Rows(0)("customerID")
        Catch ex As Exception
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return cID
    End Function
    Protected Sub gvHistory_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gvHistory As GridView = fvOrder.FindControl("gvHistory")
        For Each row As GridViewRow In gvHistory.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If Len(LCase(row.Cells(_gvHistory_vendorTxPos).Text)) > 5 Then 'VendorTxCode exists
                    If Not (LCase(row.Cells(_gvHistory_statusPos).Text) = "ok" And Left(LCase(row.Cells(_gvHistory_vendorTxPos).Text), 5) = "ebpay") Then
                        'Row does not contain a payemnt reference
                        Dim btn As Button = row.FindControl("btnRefund")
                        btn.Visible = False
                    End If
                    If (LCase(row.Cells(_gvHistory_statusPos).Text) = "ok" And Left(LCase(row.Cells(_gvHistory_vendorTxPos).Text), 5) = "ebref") Then row.ForeColor = Drawing.Color.Red
                End If
                If UCase(row.Cells(_gvHistory_statusPos).Text) <> "OK" Then
                    'If status<>'OK' then make font non bold, and italic
                    row.Font.Italic = True
                    row.Font.Bold = False
                End If
            End If
        Next
    End Sub
    Protected Sub gvHistory_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim gvHistory As GridView = fvOrder.FindControl("gvHistory")
        Dim tblRefund As Table = fvOrder.FindControl("tblRefund")
        Dim txtRefund As TextBox = fvOrder.FindControl("txtRefund")
        txtRefund.Text = gvHistory.Rows(gvHistory.SelectedIndex).Cells(_gvHistory_amountPos).Text
        tblRefund.Visible = True
    End Sub
    Protected Sub btnConfirmRefund_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("ref")
        If Page.IsValid Then
            Dim gvHistory As GridView = fvOrder.FindControl("gvHistory")
            Dim tblRefund As Table = fvOrder.FindControl("tblRefund")
            Dim txtRefund As TextBox = fvOrder.FindControl("txtRefund")
            Dim lblRefundError As Label = fvOrder.FindControl("lblRefundError")

            If CDec(txtRefund.Text) > CDec(gvHistory.Rows(gvHistory.SelectedIndex).Cells(_gvHistory_amountPos).Text) Then
                lblRefundError.Text = "<font color='red'>The refund ammount cannot exceed the original charge.</font>"
            Else
                lblRefundError.Text = ""
                processRefund(CDec(txtRefund.Text), gvHistory.Rows(gvHistory.SelectedIndex).Cells(_gvHistory_vendorTxPos).Text, gvHistory.DataKeys(gvHistory.SelectedIndex).Value)
            End If
        End If
    End Sub
    Protected Sub processRefund(ByVal amount As Decimal, ByVal rvTX As String, ByVal protxID As Integer)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim _vendorTxCode As String = "EBREFUND" & formatDateElement(Day(Date.Today)) & formatDateElement(Month(Date.Today)) & Year(Date.Today) & Hour(Now()) & Minute(Now()) & hidOrderID.Value
        Dim tx As New Protx.Vsp.RefundTransaction(_vendorTxCode)
        Dim bError As Boolean = False
        Dim lblCurrency As Label = fvOrder.FindControl("lblCurrency")
        Dim VPSTxID As String = ""
        Dim SecurityKey As String = ""
        Dim RelatedTxAuthNo As String = ""
        Dim _VPSProtocol As String = ""
        Dim _status As String = ""
        Dim _statusDetail As String = ""
        Dim _txAuthNo As String = ""
        Dim _VPSTxId As String = ""
        Dim _securityKey As String = ""
        Dim _orderID As Integer = 0
        Dim _returnID As Integer = 0
        Dim _currency As String = UCase(lblCurrency.Text)
        Dim _country As String = _countryCode
        Dim _relatedVendorTxCode As String = rvTX
        Dim panPaymentResults As Panel = fvOrder.FindControl("panPaymentResults")
        Dim lblCCResults As Label = fvOrder.FindControl("lblCCResults")
        Dim tblRefund As Table = fvOrder.FindControl("tblRefund")
        Dim gvHistory As GridView = fvOrder.FindControl("gvHistory")
        Dim vspResponse As Protx.Vsp.RefundResponse
        Try
            _orderID = hidOrderID.Value
        Catch ex As Exception
        End Try
        getRefundDetails(protxID, VPSTxID, SecurityKey, RelatedTxAuthNo)
        Dim lblRefundError As Label = fvOrder.FindControl("lblRefundError")
        If VPSTxID <> "" Then
            tx.Amount = amount
            tx.RelatedVendorTxCode = rvTX
            tx.VPSTxID = VPSTxID
            tx.SecurityKey = SecurityKey
            tx.TxAuthNo = RelatedTxAuthNo
            tx.Currency = UCase(lblCurrency.Text)
            tx.Description = "EmotionalBliss Refund"
            If Not Application("isDevBox") Then
                vspResponse = tx.Send()
                If vspResponse.Status = Protx.Vsp.VspStatusType.OK Then
                    'Yay, refund successful. 
                    lblRefundError.Text = "ALL OK"
                    _VPSProtocol = vspResponse.VPSProtocol
                    '_status = vspResponse.Status #Protx is returng a status of '0' when successfully refunding (for some unknown reason), so hard code to 'OK'
                    _status = "OK"
                    _statusDetail = vspResponse.StatusDetail
                    _txAuthNo = vspResponse.TxAuthNo
                    _VPSTxId = vspResponse.VPSTxID
                    _
                Else
                    'Failure!
                    lblRefundError.Text = "<font color='red'>" & vspResponse.StatusDetail & "</font>"
                    _VPSProtocol = vspResponse.VPSProtocol
                    _status = vspResponse.Status
                    _statusDetail = vspResponse.StatusDetail
                End If
            Else
                'Must be running on dev box, refund dont seem to work. Set values to be added to database
                _VPSProtocol = tx.VPSProtocol
                _status = "OK"
                _statusDetail = "Not submitted to protx, but added to DB as OK"
            End If
            'Store result in database
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProtxRefundInsert", oConn)
            oCmd.CommandType = CommandType.StoredProcedure
            With oCmd
                .Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 25))
                .Parameters.Add(New SqlParameter("@relatedVendorTxCode", SqlDbType.VarChar, 30))
                .Parameters.Add(New SqlParameter("@VPSProtocol", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@VPSTxID", SqlDbType.VarChar, 40))
                .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@statusDetail", SqlDbType.VarChar, 255))
                .Parameters.Add(New SqlParameter("@txAuthNo", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@securityKey", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@amount", SqlDbType.Money))
                .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
                .Parameters("@vendorTxCode").Value = _vendorTxCode
                .Parameters("@relatedVendorTxCode").Value = rvTX
                .Parameters("@VPSProtocol").Value = _VPSProtocol
                .Parameters("@VPSTxID").Value = _VPSTxId
                .Parameters("@status").Value = _status
                .Parameters("@statusDetail").Value = _statusDetail
                .Parameters("@txAuthNo").Value = _txAuthNo
                .Parameters("@securityKey").Value = _securityKey
                .Parameters("@amount").Value = amount
                .Parameters("@orderID").Value = _orderID
                .Parameters("@currency").Value = _currency
                .Parameters("@country").Value = _country
                .Parameters("@bank").Value = 0
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                bError = True
                lblRefundError.Text = "<font color='red'>An error occured adding refund details to database.</font>"
                Response.Write(ex.ToString)
            Finally
                'clear upski
                oCmd.Dispose()
                oConn.Dispose()
                tx = Nothing
                vspResponse = Nothing
            End Try
            If Not bError Then
                'Add to SalesLedger
                addRefundToSalesLedger(_orderID, _country, amount)
                'Add to order log
                Dim si As siteInclude
                si.AddToOrderLog(_orderID, "Card Successfully Refunded " & Session("EBAffCurrencySign") & FormatNumber(amount, 2), Membership.GetUser.UserName, True, "N/A")
                si = Nothing
                'Change order status to cancelled
                updateOrderStatus("Cancelled")
                'Clean up screen
                Dim gvTrace As GridView = fvOrder.FindControl("gvTrace")
                gvTrace.DataBind()
                Dim drpStatus As DropDownList = fvOrder.FindControl("drpStatus")
                drpStatus.Visible = False
                tblRefund.Visible = False
                gvHistory.SelectedIndex = -1
                gvHistory.DataBind()
            End If
        Else
            lblRefundError.Text = "<font color='red'>Error occured; Could not find VPSTxID.</font>"
            Try
                Dim si As New siteInclude
                si.addError("affilaites/orderView.aspx", "processRefund(protxID=" & protxID & ");")
                si = Nothing
            Catch ex As Exception
            End Try
        End If
    End Sub
    Protected Sub getRefundDetails(ByVal protxID As Integer, ByRef vpstxid As String, ByRef securityKey As String, ByRef relatedTx As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProtxByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@protxID", SqlDbType.Int))
            .Parameters("@protxID").Value = protxID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                vpstxid = ds.Tables(0).Rows(0)("VPSTxID")
                securityKey = ds.Tables(0).Rows(0)("SecurityKey")
                relatedTx = ds.Tables(0).Rows(0)("txAuthNo")
            End If
        Catch ex As Exception
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try

    End Sub
    Protected Sub lnkEditShip_click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub lnkEditBill_click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub gvOrderItems_updating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        'Validate user entry
        Dim lblErrorItems As Label = fvOrder.FindControl("lblErrorItems")
        Dim gv As GridView = CType(sender, GridView)
        If e.NewValues(_gvOrderItems_qtyParamPos).ToString <> "" And e.NewValues(_gvOrderItems_vatParamPos).ToString <> "" And e.NewValues(_gvOrderItems_costParamPos).ToString <> "" Then
            If IsNumeric(e.NewValues(_gvOrderItems_qtyParamPos).ToString) And IsNumeric(e.NewValues(_gvOrderItems_vatParamPos).ToString) And IsNumeric(e.NewValues(_gvOrderItems_costParamPos).ToString) Then
                'All ok, do nothing, let gridview datasource do the updating
                _newQty = CDec(e.NewValues(_gvOrderItems_qtyParamPos).ToString)
                _newVat = CDec(e.NewValues(_gvOrderItems_vatParamPos).ToString)
                _newCost = CDec(e.NewValues(_gvOrderItems_costParamPos).ToString)
            Else
                'Invalid entry - at least one box has non-numerical data in it
                lblErrorItems.Text = "<font color='red'>Invalid data was entered. Item costs could not be updated. Please try again.</font>"
                e.Cancel = True
            End If
        Else
            'Invalid entry - at least one box has been left empty
            lblErrorItems.Text = "<font color='red'>You have left one of the input boxes empty. Please try again.</font>"
            e.Cancel = True
        End If
        If Not e.Cancel Then gv.DataBind()
    End Sub
    Protected Sub gvOrderItems_updated(ByVal sender As Object, ByVal e As GridViewUpdatedEventArgs)
        Dim lblErrorItems As Label = fvOrder.FindControl("lblErrorItems")
        'lblErrorItems.Text = "affected rows=" & e.AffectedRows
        If e.AffectedRows Then
            'Update was ok, now update the ordertotals
            updateOrderTotals(False)
            Try

            Catch ex As Exception
                lblErrorItems.Text = "<font color='red'>An error occured while calculating new totals. Please try later.</font>"
                Dim si As New siteInclude
                si.addError("affiliates/orderView.aspx.vb", "gvOrderItems_updated(orderID=" & Request.QueryString("orderID") & "); " & ex.ToString)
                si = Nothing
            End Try
        End If
    End Sub
    Protected Sub updateOrderTotals(ByVal calledFromShippingUpdate As Boolean)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDCostsUpdate", oConn)
        Dim lblErrorItems As Label = fvOrder.FindControl("lblErrorItems")
        Dim txtShipping As TextBox = fvOrder.FindControl("txtShipping")
        Dim lblDiscount As Label = fvOrder.FindControl("lblDiscount")
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@goods", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@goodsVat", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shipping", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingTotal", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderTotal", SqlDbType.Decimal))
            .Parameters("@id").Value = Request.QueryString("id")
            .Parameters("@goods").Value = getGoods()
            .Parameters("@goodsVat").Value = getGoodsVat()
            .Parameters("@shipping").Value = getShipping()
        End With
        If calledFromShippingUpdate Then oCmd.Parameters("@shipping").Value = CDec(txtShipping.Text)
        oCmd.Parameters("@shippingTotal").Value = getShippingTotal(oCmd.Parameters("@shipping").Value, oConn)
        oCmd.Parameters("@orderTotal").Value = calcTotal(oCmd.Parameters("@goods").Value, oCmd.Parameters("@goodsVat").Value, oCmd.Parameters("@shippingTotal").Value) + lblDiscount.Text
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblErrorItems.Text = "<font color='red'>An error occured whlie calculating order totals.<br>Please check the order adds up correctly.<br>If not please conatct Tech Support. Sorry for any inconvenience.</font>"
            Dim si As New siteInclude
            si.addError("affiliates/orderView.aspx.vb", "updateOrderTotals(orderID=" & Request.QueryString("orderID") & "); " & ex.ToString)
            si = Nothing
        End Try
        'Response.Redirect("orderView.aspx?id=" & Request.QueryString("id"))
        fvOrder.DataBind()
        setStatusDropdown()
    End Sub
    Protected Function getGoods() As Decimal
        Dim gv As GridView = fvOrder.FindControl("gvOrderItems")
        Dim total As Decimal = 0
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If row.Cells(2).Controls.Count > 0 Then
                    'Row is still in edit state, grab textbox controls
                    total = total + CDec(_newCost * _newQty)
                Else
                    'Row is normal, grab cell.text
                    total = total + CDec(row.Cells(_gvOrderItems_costPos).Text * row.Cells(_gvOrderItems_qtyPos).Text)
                End If
            End If
        Next
        Return total
    End Function
    Function getGoodsVat() As Decimal
        Dim gv As GridView = fvOrder.FindControl("gvOrderItems")
        Dim total As Decimal = 0
        Dim itemVat As Decimal
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If row.Cells(2).Controls.Count > 0 Then
                    'Row is still in edit state, grab textbox controls
                    itemVat = FormatNumber(((_newCost / 100) * _newVat) * _newQty, 2)
                Else
                    'Row is normal, grab cell.text
                    itemVat = FormatNumber((CDec((row.Cells(_gvOrderItems_costPos).Text) / 100) * row.Cells(_gvOrderItems_vatPos).Text) * row.Cells(_gvOrderItems_qtyPos).Text, 2)
                End If
                total = total + itemVat
            End If
        Next
        Return total
    End Function
    Function getShipping() As Decimal
        Dim lblShipping As Label = fvOrder.FindControl("lblShipping")
        Return CDec(lblShipping.Text)
    End Function
    Function getShippingTotal(ByVal shipping As Decimal, ByRef oConn As SqlConnection) As Decimal
        'Grab shipping vatrate from database and return shipping inc vat
        Dim oCmd As New SqlCommand("procShippingVatByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim shippingVatRate As Decimal = 0
        Dim lblErrorItems As Label = fvOrder.FindControl("lblErrorItems")
        Dim total As Decimal = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = _countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then shippingVatRate = ds.Tables(0).Rows(0)("vatRate")
        Catch ex As Exception
            lblErrorItems.Text = "<font color='red'>An error occured whlie calculating order totals.<br>Please check the order adds up correctly.<br>If not please conatct Tech Support. Sorry for any inconvenience.</font>"
            Dim si As New siteInclude
            si.addError("affilaites/orderView.aspx.vb", "getShippingTotal(orderid=" & Request.QueryString("id") & ", shipping=" & shipping & "); " & ex.ToString)
            si = Nothing
        End Try
        If shippingVatRate > 0 Then total = shipping + FormatNumber((shipping / 100) * shippingVatRate, 2)
        Return total
    End Function
    Protected Function calcTotal(ByVal goods As Decimal, ByVal goodsVat As Decimal, ByVal shippingTotal As Decimal) As Decimal
        Return goods + goodsVat + shippingTotal
    End Function
    Protected Sub lnkShippingEdit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim tr As HtmlTableRow = fvOrder.FindControl("trShipping")
        tr.Visible = True
    End Sub
    Protected Sub addRefundToSalesLedger(ByVal orderID As Integer, ByVal countryCode As String, ByVal orderTotal As Decimal)
        Dim vatRate As Decimal = getVatRate()
        Dim goods As Decimal = 0
        Dim vat As Decimal = 0
        Dim hidOrderPrefix As HiddenField = fvOrder.FindControl("hidOrderPrefix")
        Try
            goods = FormatNumber(orderTotal / ((vatRate / 100) + 1), 2)
            vat = orderTotal - goods
            Dim si As New siteInclude
            si.addToSalesLedger(orderID, 0, 2, countryCode, hidOrderPrefix.Value, 0, goods, 0, vat)
            si = Nothing
        Catch ex As Exception
            Dim lblRefundError As Label = fvOrder.FindControl("lblRefundError")
            lblRefundError.Text = "<font color='red'>An error occured while adding refund details to the sales ledger. (The refund may not appear on the ledger)<br>Please contact tech support.</font>"
            Dim si As New siteInclude
            si.addError("affiliates/orderView.aspx.vb", "addRefundToSalesLedger2(orderid=" & orderID & ", countrycode=" & countryCode & ",total=" & orderTotal & "); " & ex.ToString)
        End Try
    End Sub
    Protected Function getVatRate() As Decimal
        Dim result As Decimal = 0
        Dim countryCode As String = _countryCode
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShippingVatByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = ds.Tables(0).Rows(0)("vatRate")
        Catch ex As Exception
            Dim lblRefundError As Label = fvOrder.FindControl("lblRefundError")
            lblRefundError.Text = "<font color='red'>An error occured while calculating the refund's vat amount.<br>Please contact tech support.</font>"
            Dim si As New siteInclude
            si.addError("affiliates/orderView.aspx.vb", "getVatRate(countrycode=" & countryCode & "); " & ex.ToString)
            si = Nothing
        End Try
        Return result
    End Function
    Protected Sub updateOrderStatus(ByVal status As String)
        Dim lblStatusUpdate As Label = fvOrder.FindControl("lblStatusUpdate")
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim bError As Boolean = False
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDStatusUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@distStatus", SqlDbType.VarChar, 20))
            .Parameters("@orderID").Value = hidOrderID.Value
            .Parameters("@status").Value = status
            .Parameters("@distStatus").Value = ""
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblStatusUpdate.Text = "<font color='red'>An error occured while updating order status.</font>"
            Try
                Dim si As New siteInclude
                si.addError("affiliates/orderView.aspx.vb", "updateOrderStatus(status=" & status & ");" & ex.ToString)
            Catch ex2 As Exception
            End Try
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            'fvOrder.DataBind()
            lblStatusUpdate.Text = "<font color='red'>Status changed to '" & formatWord(status) & "'.</font>"
        End If

        Dim panPayment As Panel = fvOrder.FindControl(lblPaymentPanelName.Text)
        If LCase(status) = "cancelled" Then
            panPayment.Visible = False
        Else
            panPayment.Visible = True
        End If
    End Sub
    Protected Function showOutstanding(ByVal quantity As Object, ByVal despatched As Object) As String
        Dim qty As Integer = 0
        Dim qtyDespatched As Integer = 0
        If Not IsDBNull(quantity) Then qty = CInt(quantity.ToString)
        If Not IsDBNull(despatched) Then qtyDespatched = CInt(despatched.ToString)
        Return CStr(qty - qtyDespatched)
    End Function
    Protected Function showDespatchQty(ByVal outstanding As Integer) As String
        Dim gv As GridView = fvOrder.FindControl("gvBasket")
        Dim stock As Integer = CInt(gv.Rows(gv.Rows.Count).Cells(_gvBasket_stockPos).Text)
        Dim result As Integer = outstanding
        If stock < outstanding Then result = stock
        Return CStr(result)
    End Function
    Protected Sub gvBasket_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim result As Integer = 0
            Dim txtDespatch As TextBox = e.Row.FindControl("txtDespatch")
            Try
                Dim lblOutstanding As Label = e.Row.FindControl("lblOutstanding")
                Dim iOutstanding As Integer = CInt(lblOutstanding.Text)
                Dim iStock As Integer = CInt(e.Row.Cells(_gvBasket_stockPos).Text)
                result = iOutstanding
                If iStock < iOutstanding Then result = iStock
            Catch ex As Exception
            End Try
            txtDespatch.Text = CStr(result)
        End If
    End Sub
    Protected Sub gvBasket_rowUpdated(ByVal sender As Object, ByVal e As GridViewUpdatedEventArgs)
        'Item row has been updated in shopOrderItem table.
        'Calculate new goodsTotal/goodsVatTotal and update shopOrder table
        Dim dv As DetailsView = fvOrder.FindControl("dvCosts")
        calcTotal()
        dv.DataBind()
    End Sub
    Protected Sub calcTotal()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderItemByOrderIDTotalsSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim gv As GridView = fvOrder.FindControl("gvBasket")
        Dim dv As DetailsView = fvOrder.FindControl("dvCosts")
        Dim itemPriceIncVat = 0
        Dim goods As Decimal = 0
        Dim goodsVat As Decimal = 0
        Dim lblShipping As Label = dv.FindControl("lblShipping")
        Dim shipping As Decimal = CDec(lblShipping.Text)
        Dim shippingVatRate As Decimal = hidVatRate.Value
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    itemPriceIncVat = FormatNumber(row("price") * (1 + (row("vatRate") / 100)), 2)
                    goods = goods + row("price") * row("qty")
                    goodsVat = goodsVat + FormatNumber((itemPriceIncVat - row("price")) * row("qty"), 2)
                Next
                'Update shopOrder
                oCmd = New SqlCommand("procShopOrderByIDCostsUpdate2", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@goods", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@goodsVat", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@shipping", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@shippingVatRate", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@shippingTotal", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@orderTotal", SqlDbType.Decimal))
                    .Parameters("@ID").Value = Request.QueryString("id")
                    .Parameters("@goods").Value = goods
                    .Parameters("@goodsVat").Value = goodsVat
                    .Parameters("@shipping").Value = shipping
                    .Parameters("@shippingVatRate").Value = hidVatRate.Value
                    .Parameters("@shippingTotal").Value = FormatNumber(shipping + ((shippingVatRate / 100) * shipping))
                    .Parameters("@orderTotal").Value = goods + goodsVat + oCmd.Parameters("@shippingTotal").Value
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    Throw ex
                End Try
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("maintenance/orderView.aspx.vb", "calcTotal(orderID=" & Request.QueryString("id") & "); " & ex.ToString)
            si = Nothing
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub gvBasket_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Update the VatRate hidden field with the vate rate from any row
        Dim gv As GridView = CType(sender, GridView)
        Dim lbl As Label
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lbl = row.FindControl("lblItemVatRate")
                hidVatRate.Value = lbl.Text
                Exit For
            End If
        Next
    End Sub
    Protected Sub btnEditShipping_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim dv As DetailsView = fvOrder.FindControl("dvCosts")
        Dim btnEditShipping As LinkButton = CType(sender, LinkButton)
        Dim btnUpdateShipping As LinkButton = fvOrder.FindControl("btnUpdateShipping")
        btnEditShipping.Visible = False
        btnUpdateShipping.Visible = True
        dv.ChangeMode(DetailsViewMode.Edit)
    End Sub
    Protected Sub txtShipping_textChanged(ByVal sender As Object, ByVal e As EventArgs)
       
    End Sub
    Protected Sub btnUpdateShipping_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        btn.Visible = False
        'Update shopOrder with the new shipping and shipingRate values. (By default Distributor orders have 0% for the shipping rate, so running this update will grab the current vatrate and update the order with it)
        Dim dv As DetailsView = fvOrder.FindControl("dvCosts")
        Dim txt As TextBox = dv.FindControl("txtShipping")
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDShippingUpdate", oConn)
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@shipping", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingVatRate", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingTotal", SqlDbType.Decimal))
            .Parameters("@id").Value = Request.QueryString("id")
            .Parameters("@shipping").Value = CDec(txt.Text)
            .Parameters("@shippingVatRate").Value = CDec(hidVatRate.Value)
            .Parameters("@shippingTotal").Value = FormatNumber((CDec(txt.Text) / 100) * CDec(hidVatRate.Value), 2)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            Dim lblError As Label = fvOrder.FindControl("lblError")
            lblError.Text = "An error occured while updating the shipping."
            Dim si As New siteInclude
            si.addError("maintenance/orderView.aspx", "txtShipping_textChanged(id=" & Request.QueryString("id") & "); " & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            Dim btnEditShipping As LinkButton = fvOrder.FindControl("btnEditShipping")
            btnEditShipping.Visible = True
            dv.ChangeMode(DetailsViewMode.ReadOnly)
            dv.DataBind()
            calcTotal()
            dv.DataBind()
        End If
    End Sub
End Class

