Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Data.SqlClient
Imports siteInclude

Partial Class affiliates_orderView
    Inherits BasePage
    Private Const _gvHistory_statusPos As Integer = 6
    Private Const _gvHistory_amountPos As Integer = 2
    Private Const _gvHistory_vendorTxPos As Integer = 8
    Private Const _gvOrderItems_qtyParamPos As Integer = 0
    Private Const _gvOrderItems_vatParamPos As Integer = 1
    Private Const _gvOrderItems_costParamPos As Integer = 2
    Private Const _gvOrderItems_qtyPos As Integer = 2
    Private Const _gvOrderItems_vatPos As Integer = 4
    Private Const _gvOrderItems_costPos As Integer = 6
    Private Const _drpStatus_paidPos As Integer = 2
    Public Const _CancelledPos As Integer = 0
    Public Const _PlacedPos As Integer = 1
    Public Const _PaidPos As Integer = 2
    Public Const _DeferredPos As Integer = 3
    Public Const _FailedPos As Integer = 4
    Public Const _CompletePos As Integer = 5
    Private _newQty As Integer = 0
    Private _newCost As Decimal = 0
    Private _newVat As Decimal = 0

    'System Events
    Protected Sub Page_Load(ByVal sener As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            Dim fv As FormView = fvOrder
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
            Dim tdProtxRelease As HtmlTableCell = fv.FindControl("tdProtxRelease")
            Dim tdProtxExtra As HtmlTableCell = fv.FindControl("tdProtxExtra")
            Dim panLog As Panel = fv.FindControl("panLog")
            Dim hid3dTransaction As hiddenfield = fvOrder.FindControl("hid3dTransaction")
            'Set Order Type text
            If Not fv.Row Is Nothing Then
                Select Case LCase(lblType.Text)
                    Case "cc"
                        lblType.Text = "Credit Card"
                        panPayment = fv.FindControl("panPaymentCC")
                        panCC.Visible = True
                        If LCase(lblStatus.Text) = "paid" Or LCase(lblStatus.Text) = "complete" Then
                            tdProtx.Visible = False
                            tdProtxRelease.Visible = False
                        End If
                        tdProtxExtra.Visible = Not tdProtx.Visible
                        If CBool(hid3dTransaction.Value) Then tdProtxExtra.visible = False
                    Case "phone"
                        lblType.Text = "Call Centre/CC"
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
                        If LCase(lblStatus.Text) = "paid" Or LCase(lblStatus.Text) = "complete" Then
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
                    Case "ddebit"
                        lblType.Text = "Direct Debit"
                        panPayment = fv.FindControl("panPaymentCheque")
                        Dim lblChequeNumber As Label = fv.FindControl("lblChequeNumber")
                        If LCase(lblStatus.Text) = "placed" Then
                            'Show the cheque number textbox and submit button
                            txtCheque.Visible = True
                            btnChSubmit.Visible = True
                        End If
                        If LCase(lblStatus.Text) = "paymentpending" Then
                            btnChequeCleared.Visible = True
                            lblChequeNumber.Visible = False
                        End If
                    Case "paypal"
                        lblType.Text = "Paypal"
                        panPayment = fv.FindControl("panPaymentPaypal")
                    Case "fastpay"
                        lblType.Text = "FastPay"
                        panPayment = fv.FindControl("panPaymentPaypal")
                    Case "affaccount"
                        lblType.Text = "AffAccount"
                        panPayment = fv.FindControl("panAffAccountPayment")
                        If LCase(lblStatus.Text) = "placed" Then
                            'txtAccount.Visible = True
                            btnAffAccountSubmit.Visible = True
                        Else
                            lblAffReadyToScan.Visible = True
                        End If
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
                'If order is 3dSecure, then hide the older payment button and also hide the take extra funds payment button

                If drpStatus.SelectedValue = "deferred" Then
                    If Not (LCase(lblStatus.Text) = "paid" Or LCase(lblStatus.Text) = "complete") Then tdProtxRelease.visible = True
                    tdProtx.visible = False
                    tdProtxExtra.visible = False
                End If
            Else
                'No datat returned, order does not exist or belongs to another country
                lblDenied.Text = "The order you are trying to view does not exist, or belongs to another country."
            End If
        End If
    End Sub

    'Page Events
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
                siteInclude.addError("affiliates/orderView.aspx.vb", "gvOrderItems_updated(orderID=" & Request.QueryString("orderID") & "); " & ex.ToString)
            End Try
        End If
    End Sub
    Protected Sub lnkVoucher_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As HyperLink = CType(sender, HyperLink)
        If lnk.Text <> "" Then
            lnk.NavigateUrl = "orderViewVoucher.aspx?v=" & lnk.Text
            lnk.Text = "(" & lnk.Text & ")"
        End If
    End Sub

    'User Events
    Protected Sub btnProtx_click(ByVal sender As Object, ByVal e As EventArgs)
        'Attempt protx transaction
        Dim status As String = protxTestCode(False)
        If LCase(status) <> "failed" Then
            'Update Payment Date
            siteInclude.updateReceiptField(Request.QueryString("id"), "paymentDate", FormatDateTime(Now(), DateFormat.LongDate) & " " & FormatDateTime(Now(), DateFormat.LongTime), True)
            Response.Redirect("orderView.aspx?id=" & Request.QueryString("ID"))
        End If

    End Sub
    Protected Sub btnProtxRelease_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim strVendorName As String = "peartree1"
        Dim strVPSTxId As String = ""
        Dim strVendorTxCode As String = ""
        Dim strSecurityKey As String = ""
        Dim strTxAuthNo As String = ""
        Dim decAmount As Decimal = 0
        Dim strConnectTo As String = "TEST"
        Dim strPost As String = ""
        Dim strResponse As String = ""
        Dim strPageError As String = ""
        Dim strStatus As String = ""
        Dim strResult As String = ""
        Dim strCurrency As String = ""
	Dim strDescription As String = "EB Goods"
        Dim tdProtxRelease As HtmlTableCell = fvOrder.FindControl("tdProtxRelease")
        Dim gvHistory As gridview = fvOrder.FindControl("gvHistory")
        Dim hidOrderID As hiddenfield = fvOrder.FindControl("hidOrderID")
        If Not Application("isDevBox") Then strConnectTo = "LIVE"
        'Retrieve protx values from protx table
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProtxByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim row As DataRow
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = hidOrderID.Value
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'Details found, continue with Release
                row = ds.Tables(0).Rows(0)
                strVPSTxId = row("VPSTxID")
		strCurrency= row("Currency")
                strVendorTxCode = row("vendorTxCode")
                strSecurityKey = row("securityKey")
                strTxAuthNo = row("txAuthNo")
                decAmount = row("amount")
                '** Build the Release message **
                strPost = "VPSProtocol=2.23"
                strPost = strPost & "&TxType=RELEASE"
                strPost = strPost & "&Vendor=" & strVendorName
                strPost = strPost & "&Amount=" & FormatNumber(decAmount, 2, -1, 0, 0)
		'strPost = strPost & "&Currency=" & strCurrency
		'strPost = strPost & "&Description=" & strDescription
                strPost = strPost & "&VPSTxId=" & strVPSTxId
                strPost = strPost & "&VendorTxCode=" & strVendorTxCode
                strPost = strPost & "&SecurityKey=" & strSecurityKey
                strPost = strPost & "&TxAuthNo=" & strTxAuthNo

                '** The full transaction registration POST has now been built **
                Dim objUTFEncode As New UTF8Encoding
                Dim arrRequest As Byte()
                Dim objStreamReq As Stream
                Dim objStreamRes As StreamReader
                Dim objHttpRequest As HttpWebRequest
                Dim objHttpResponse As HttpWebResponse
                Dim objUri As New Uri(siteInclude.getSagePayURL(strConnectTo, "release"))

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
                    '** An non zero Err.number indicates an error of some kind **
                    '** Check for the most common error... unable to reach the purchase URL **  
                    If Err.Number = -2147012889 Then
                        strPageError = "Your server was unable to register this transaction with Protx." & _
                        "  Check that you do not have a firewall restricting the POST and " & _
                        "that your server can correctly resolve the address " & siteInclude.getSagePayURL(strConnectTo, "puchase")
                    Else
                        strPageError = "An Error has occurred whilst trying to register this transaction.<BR>" & _
                        "The Error Number is: " & Err.Number & "<BR>" & _
                        "The Description given is: " & Err.Description
                    End If
                Else
                    '** No transport level errors, so the message got the Protx **
                    '** Analyse the response from VSP Direct to check that everything is okay **
                    strStatus = findField("Status", strResponse)
                    If Left(strStatus, 2) = "OK" Then
                        '** An OK status mean that the transaction has been successfully Releasesd **
                        strResult = "SUCCESS : The transaction was Released successfully and a new Release transaction was created."
                    Else
                        '** All other Statuses are errors of one form or another.  Display them on the screen with no database updates **
                        strResult = strStatus & " : " & findField("StatusDetail", strResponse)
                    End If
                End If
                Dim lblError As label = fvOrder.FindControl("lblError")
                Dim lblProtxError As label = fvOrder.FindControl("lblProtxError")
                Dim drpStatus As dropDownList = fvOrder.FindControl("drpStatus")
                If Left(strStatus, 2) <> "OK" Then
                    lblProtxError.Text = strPageError & "<br>"
                    'Set status to failed
                    drpStatus.Items(_FailedPos).Enabled = True
                    drpStatus.SelectedIndex = _FailedPos
                    updateOrderStatus("failed")
                    'Update order log
                    siteInclude.AddToOrderLog(hidOrderID.Value, "Payment failed.<br>Error code " & strResult, Membership.GetUser.UserName, True)
                Else
                    'Set status to paid
                    drpStatus.Items(_PaidPos).Enabled = True
                    drpStatus.SelectedIndex = _PaidPos
                    updateOrderStatus("paid")
                    'Update order log
                    siteInclude.AddToOrderLog(hidOrderID.Value, "Payment successfully Released, txID=" & strVendorTxCode, Membership.GetUser.UserName, True)
                End If
                lblError.Text = strResult
                updateProtxStatus(strStatus, strResult, strVendorTxCode)
                Dim gv As GridView = fvOrder.FindControl("gvTrace")
                gv.DataBind()
                gvHistory.DataBind()
                tdProtxRelease.visible = False
                'Update Payment Date
                siteInclude.updateReceiptField(Request.QueryString("id"), "paymentDate", FormatDateTime(Now(), DateFormat.LongDate) & " " & FormatDateTime(Now(), DateFormat.LongTime), True)
            End If
        Catch ex As Exception
            siteInclude.debug("URL=" & siteInclude.getSagePayURL(strConnectTo, "release"))
            siteInclude.addError("affiliates/orderView.aspx.vb", "btnProtxRelease_click(Byval id=" & hidOrderID.Value & "); " & ex.ToString())
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnChequeSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add the cheque number to the shopCustomer table and set the order status to Paid
        '(In future change status to InProgress while cheque clears, and add button to change status to Paid once cheque clears)
        Dim oConn As New sqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New sqlCommand("procShopOrderCustomerChequeStatusUpdate", oConn)
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
        Dim userName As String = "Unknown"
        Try
            userName = Membership.GetUser.UserName
        Catch ex As Exception
        End Try
        'Update Payment Date
        siteInclude.updateReceiptField(Request.QueryString("id"), "paymentDate", FormatDateTime(Now(), DateFormat.LongDate) & " " & FormatDateTime(Now(), DateFormat.LongTime), True)
        siteInclude.AddToOrderLog(hidOrderID.Value, "Payment cleared.", userName, True)
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
        panLog.DataBind()
    End Sub
    Protected Sub btnEditShipping_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim fv As FormView = fvOrder
        Dim pan As Panel = fv.FindControl("panEditShipping")
        pan.Visible = Not pan.Visible
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
                si.addToSalesLedger(hidOrderID.Value, 0, 1, Session("EBAffEBDistributorCountryCode"), 0, dGoods, 0, dVat, 0)
                si = Nothing
            End If
        End If
    End Sub
    Protected Sub gvHistory_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim gvHistory As GridView = fvOrder.FindControl("gvHistory")
        Dim tblRefund As Table = fvOrder.FindControl("tblRefund")
        Dim txtRefund As TextBox = fvOrder.FindControl("txtRefund")
        'txtRefund.Text = gvHistory.Rows(gvHistory.SelectedIndex).Cells(_gvHistory_AmountPos).Text
        tblRefund.Visible = True
    End Sub
    Protected Sub btnConfirmRefund_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("ref")
        If Page.IsValid Then
            Dim gvHistory As GridView = fvOrder.FindControl("gvHistory")
            Dim tblRefund As Table = fvOrder.FindControl("tblRefund")
            Dim txtRefund As TextBox = fvOrder.FindControl("txtRefund")
            Dim lblRefundError As Label = fvOrder.FindControl("lblRefundError")

            If CDec(txtRefund.Text) > CDec(gvHistory.Rows(gvHistory.SelectedIndex).Cells(_gvHistory_AmountPos).Text) Then
                lblRefundError.Text = "<font color='red'>The refund ammount cannot exceed the original charge.</font>"
            Else
                lblRefundError.Text = ""
                processRefund(CDec(txtRefund.Text), gvHistory.Rows(gvHistory.SelectedIndex).Cells(_gvHistory_vendorTxPos).Text, gvHistory.DataKeys(gvHistory.SelectedIndex).Value)
            End If
        End If
    End Sub
    Protected Sub drpStatus_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Update orders status to whatever the user just selected
        Dim drpStatus As DropDownList = CType(sender, DropDownList)
        Dim gvTrace As GridView = fvOrder.FindControl("gvTrace")
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
        Select Case LCase(drpStatus.SelectedValue)
            Case "placed"
                'If a chrque order has been changed from cancelled to placed, then show the payment panel and cheque number input box
                pan.Visible = True
                txtCheque.Visible = True
                btnChSubmit.Visible = True
            Case "paymentpending"
                btnChequeCleared.Visible = True
            Case "deferred"
                Dim tdProtxRelease As HtmlTableCell = fvOrder.FindControl("tdProtxRelease")
                tdProtxRelease.visible = True
            Case Else

        End Select
        Dim panPayment As Panel = fvOrder.FindControl(lblPaymentPanelName.text)
        If LCase(drpStatus.SelectedValue) = "cancelled" Then
            panPayment.Visible = False
        Else
            panPayment.Visible = True
        End If

    End Sub
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
    Protected Sub btnMsgSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
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
    Protected Sub lnkEditShip_click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub lnkEditBill_click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub lnkShippingEdit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim tr As HtmlTableRow = fvOrder.FindControl("trShipping")
        tr.Visible = True
    End Sub
    Protected Sub lnkTaxEdit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim tr As HtmlTableRow = fvOrder.FindControl("trTax")
        tr.Visible = True
        tr = fvOrder.FindControl("trTax2")
        tr.Visible = True
        Dim txtTax As TextBox = fvOrder.FindControl("txtTax")
        Dim txtTaxShip As TextBox = fvOrder.FindControl("txtTaxShip")
        setTaxValues(txtTax, txtTaxShip)
    End Sub
    Protected Sub btnShippingUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("shippingUpdate")
        Dim drpStatus As DropDownList
        If Page.IsValid Then
            updateOrderTotals("ship")
            drpStatus = fvOrder.FindControl("drpStatus")
            Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
            drpStatus.SelectedValue = lblStatus.Text
        End If
        Dim hidPrefix As HiddenField = fvOrder.FindControl("hidOrderPrefix")
        If hidPrefix.Value = "40" And LCase(drpStatus.SelectedValue) <> "paid" And LCase(drpStatus.SelectedValue) <> "complete" Then
            Dim pan As Panel = fvOrder.FindControl("panAffAccountPayment")
            Dim btn As Button = fvOrder.FindControl("btnAffAccountSubmit")
            'pan.Visible = True
            'btn.Visible = True
        End If
    End Sub
    Protected Sub btnTaxUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("TaxUpdate")
        Dim drpStatus As DropDownList
        If Page.IsValid Then
            updateOrderTotals("tax")
            drpStatus = fvOrder.FindControl("drpStatus")
            Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
            drpStatus.SelectedValue = lblStatus.Text
        End If
        Dim hidPrefix As HiddenField = fvOrder.FindControl("hidOrderPrefix")
        If hidPrefix.Value = "40" And LCase(drpStatus.SelectedValue) <> "paid" And LCase(drpStatus.SelectedValue) <> "complete" Then
            Dim pan As Panel = fvOrder.FindControl("panAffAccountPayment")
            Dim btn As Button = fvOrder.FindControl("btnAffAccountSubmit")
            pan.Visible = True
            btn.Visible = True
        End If
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
    Protected Sub setStatusDropdown()
        Dim drpStatus As DropDownList = fvOrder.FindControl("drpStatus")
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        Dim status As String = lblStatus.Text
        For Each item As ListItem In drpStatus.Items
            If LCase(item.Value) = LCase(status) Then item.Selected = True
        Next
        Select Case LCase(status)
            Case "cancelled"
                drpStatus.Items(_CompletePos).Enabled = False
                drpStatus.Items(_DeferredPos).Enabled = False
                drpStatus.Items(_PaidPos).Enabled = True
            Case "placed"
                drpStatus.Items(_PaidPos).Enabled = True
                drpStatus.Items(_CompletePos).Enabled = False
                drpStatus.Items(_DeferredPos).Enabled = False
            Case "paid"
                'drpStatus.Items(_CancelledPos).Enabled = False
                drpStatus.Items(_PlacedPos).Enabled = False
                drpStatus.Items(_CompletePos).Enabled = False
                drpStatus.Items(_DeferredPos).Enabled = False
            Case "deferred"
                drpStatus.Items(_PaidPos).Enabled = False
                drpStatus.Items(_CompletePos).Enabled = False
            Case "complete"
                drpStatus.Items(_CancelledPos).Enabled = False
                drpStatus.Items(_PlacedPos).Enabled = False
                drpStatus.Items(_PaidPos).Enabled = False
                drpStatus.Items(_DeferredPos).Enabled = False
            Case "failed"
                drpStatus.Items(_CompletePos).Enabled = False
                drpStatus.Items(_PaidPos).Enabled = False
                drpStatus.Items(_DeferredPos).Enabled = True
        End Select
    End Sub
    Protected Sub processRefund(ByVal amount As Decimal, ByVal rvTX As String, ByVal protxID As Integer)
        Dim hidOrderID As HiddenField = fvOrder.FindControl("hidOrderID")
        Dim vendorTxCode As String = "EBREFUND" & formatDateElement(Day(Date.Today)) & formatDateElement(Month(Date.Today)) & Year(Date.Today) & Hour(Now()) & Minute(Now()) & hidOrderID.Value

        Dim orderID As Integer = CInt(hidOrderID.Value)
        Dim lblRefundError As Label = fvOrder.FindControl("lblRefundError")
        Dim lblCurrency As Label = fvOrder.FindControl("lblCurrency")
        'Dim lnk As LinkButton = CType(sender, LinkButton)
        'Dim dcfc As DataControlFieldCell = lnk.Parent
        'Dim row As GridViewRow = dcfc.Parent
        'Dim sagepayID As Integer = gvTransactions.SelectedValue
        'Dim hidVPSTxId As HiddenField = row.FindControl("hidVPSTxId")
        'Dim hidVendorTxCode As HiddenField = row.FindControl("hidVendorTxCode")
        'Dim hidSecurityKey As HiddenField = row.FindControl("hidSecurityKey")
        'Dim hidTxAuthNo As HiddenField = row.FindControl("hidTxAuthNo")
        'Dim hidRelatedVPSTxId As HiddenField = row.FindControl("hidRelatedVPSTxId")
        'Dim hidRelatedVendorTxCode As HiddenField = row.FindControl("hidRelatedVendorTxCode")
        'Dim hidRelatedSecurityKey As HiddenField = row.FindControl("hidRelatedSecurityKey")
        'Dim hidRelatedTxAuthNo As HiddenField = row.FindControl("hidRelatedTxAuthNo")
        'Dim txtAmount As TextBox = row.FindControl("txtAmount")
        Dim oSage As New SagepayRefund(orderID)
        Dim _country As String = Session("EBAffEBDistributorCountryCode")
        If Application("isDevBox") Then
            oSage.pay.Server = "TEST"
        Else
            oSage.pay.Server = "LIVE"
        End If
        oSage.pay.VPSProtocol = "2.23"
        oSage.pay.TxType = "REFUND"
        oSage.pay.Vendor = "peartree1"
        oSage.pay.VendorTxCode = vendorTxCode
        oSage.pay.Amount = amount
        oSage.pay.Currency = UCase(lblCurrency.Text)
        oSage.pay.Description = "Refund of " & FormatNumber(amount, 2) & " for eDecks order " & orderID
        'If hidRelatedTxAuthNo.Value <> "" Then
        If False Then
            'Must be a Release, use the Deferred related details
            'oSage.pay.RelatedVPSTxId = hidRelatedVPSTxId.Value
            'oSage.pay.RelatedVendorTxCode = hidRelatedVendorTxCode.Value
            'oSage.pay.RelatedSecuritykey = hidRelatedSecurityKey.Value
            'oSage.pay.RelatedTxAuthNo = hidRelatedTxAuthNo.Value
        Else
            'Must be a immediate Payment. Use its own details.
            'oSage.pay.RelatedVPSTxId = hidVPSTxId.Value
            oSage.pay.RelatedVendorTxCode = rvTX
            'oSage.pay.RelatedSecuritykey = hidSecurityKey.Value
            'oSage.pay.RelatedTxAuthNo = hidTxAuthNo.Value
            getRefundDetails(protxID, oSage.pay.RelatedVPSTxId, oSage.pay.RelatedSecuritykey, oSage.pay.RelatedTxAuthNo)
        End If

        'oSage.dumpSnapshot()
        oSage.postData("refund")
        Try
            If oSage.findField("Status") = "OK" Then
                siteInclude.AddToOrderLog(orderID, "Card refunded " & FormatNumber(amount, 2) & ".", Membership.GetUser.UserName, False, "N/A")
                'Add to SalesLedger
                addRefundToSalesLedger(orderID, _country, amount)
            Else
                siteInclude.AddToOrderLog(orderID, "Refunded for " & FormatNumber(amount, 2) & " failed: " & oSage.findField("Status"), Membership.GetUser.UserName, False, "N/A")
                lblRefundError.Text = oSage.findField("Status") & ", " & oSage.findField("StatusDetail")
            End If
            oSage.storeRefundResponse()
        Catch ex As Exception
            siteInclude.addError("maintenance/orderView.aspx.vb", "lnkRefundConfirm_click(OrderID=" & orderID & ", VendorTxCode=" & vendorTxCode & ", hidVPSTxId=" & oSage.pay.RelatedVPSTxId & ", hidVendorTxCode=" & oSage.pay.RelatedVendorTxCode & ", hidSecurityKey=" & oSage.pay.RelatedSecuritykey & ", amount=" & amount & "); " & ex.ToString())
        End Try
        'Clean up screen
        oSage = Nothing
        Dim gvHistory As GridView = fvOrder.FindControl("gvHistory")
        Dim gvTrace As GridView = fvOrder.FindControl("gvTrace")
        Dim drpStatus As DropDownList = fvOrder.FindControl("drpStatus")
        Dim tblRefund As Table = fvOrder.FindControl("tblRefund")
        gvTrace.DataBind()
        drpStatus.Visible = False
        tblRefund.Visible = False
        gvHistory.SelectedIndex = -1
        gvHistory.DataBind()


        If False Then
            Dim tx As New Protx.Vsp.RefundTransaction(vendorTxCode)
            Dim bError As Boolean = False

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
            'Dim _country As String = Session("EBAffEBDistributorCountryCode")
            Dim _relatedVendorTxCode As String = rvTX
            Dim panPaymentResults As Panel = fvOrder.FindControl("panPaymentResults")
            Dim lblCCResults As Label = fvOrder.FindControl("lblCCResults")
            Dim vspResponse As Protx.Vsp.RefundResponse
            Try
                _orderID = hidOrderID.Value
            Catch ex As Exception
            End Try
            siteInclude.debug("A) '" & protxID & "','" & VPSTxID & "','" & SecurityKey & "','" & RelatedTxAuthNo & "'")
            getRefundDetails(protxID, VPSTxID, SecurityKey, RelatedTxAuthNo)
            siteInclude.debug("B) '" & protxID & "','" & VPSTxID & "','" & SecurityKey & "','" & RelatedTxAuthNo & "'")
            If VPSTxID <> "" Then
                Try
                    tx.Amount = amount
                    tx.RelatedVendorTxCode = rvTX
                    tx.VPSTxID = VPSTxID
                    tx.SecurityKey = SecurityKey
                    tx.TxAuthNo = RelatedTxAuthNo
                    tx.Currency = UCase(lblCurrency.Text)
                    tx.Description = "EmotionalBliss Refund"
                    'If Not Application("isDevBox") Then
                    If True Then
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
                        .Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@relatedVendorTxCode", SqlDbType.VarChar, 50))
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
                        .Parameters("@vendorTxCode").Value = vendorTxCode
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

                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    bError = True
                    lblRefundError.Text = "<font color='red'>An error occured adding refund details to database.</font>"
                    siteInclude.addError("orderView.aspx", ex.ToString)
                Finally
                    'clear upski
                    'oCmd.Dispose()
                    'oConn.Dispose()
                    tx = Nothing
                    vspResponse = Nothing
                End Try
            End If
            If Not bError Then
                'Add to SalesLedger
                addRefundToSalesLedger(_orderID, _country, amount)
                'Add to order log
                Dim si As siteInclude
                siteInclude.AddToOrderLog(_orderID, "Card Successfully Refunded " & Session("EBAffCurrencySign") & FormatNumber(amount, 2), Membership.GetUser.UserName, True, "N/A")
                si = Nothing
                'Change order status to cancelled
                updateOrderStatus("Cancelled")
                'Clean up screen
                gvTrace.DataBind()
                drpStatus.Visible = False
                tblRefund.Visible = False
                gvHistory.SelectedIndex = -1
                gvHistory.DataBind()
            Else
                lblRefundError.Text = "<font color='red'>Error occured; Could not find VPSTxID.</font>"
                Try
                    Dim si As New siteInclude
                    siteInclude.addError("affilaites/orderView.aspx", "processRefund(protxID=" & protxID & "); ")
                    si = Nothing
                Catch ex As Exception
                End Try
            End If
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
    Protected Sub updateOrderTotals(ByVal updateTaxOrShipping As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDCostsUpdate", oConn)
        Dim lblErrorItems As Label = fvOrder.FindControl("lblErrorItems")
        Dim txtShipping As TextBox = fvOrder.FindControl("txtShipping")
        Dim txtTax As TextBox = fvOrder.FindControl("txtTax")
        Dim txtTaxShip As TextBox = fvOrder.FindControl("txtTaxShip")
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
            .Parameters("@shippingTotal").Value = getShippingTotal(oCmd.Parameters("@shipping").Value, oConn)
        End With
        If updateTaxOrShipping = "ship" Then
            oCmd.Parameters("@shipping").Value = CDec(txtShipping.Text)
            oCmd.Parameters("@shippingTotal").Value = getShippingTotal(oCmd.Parameters("@shipping").Value, oConn)
        End If
        If updateTaxOrShipping = "tax" Then
            oCmd.Parameters("@goodsVat").Value = txtTax.Text
            oCmd.Parameters("@shippingTotal").Value = oCmd.Parameters("@shipping").Value + txtTaxShip.Text
        End If
        oCmd.Parameters("@orderTotal").Value = calcTotal(oCmd.Parameters("@goods").Value, oCmd.Parameters("@goodsVat").Value, oCmd.Parameters("@shippingTotal").Value) + lblDiscount.Text
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblErrorItems.Text = "<font color='red'>An error occured whlie calculating order totals.<br>Please check the order adds up correctly.<br>If not please conatct Tech Support. Sorry for any inconvenience.</font>"
            siteInclude.addError("affiliates/orderView.aspx.vb", "updateOrderTotals(orderID=" & Request.QueryString("id") & "); " & ex.ToString)
        End Try
        'Response.Redirect("orderView.aspx?id=" & Request.QueryString("id"))
        fvOrder.DataBind()
        setStatusDropdown()
        rebindPaymentPanels()
    End Sub
    Protected Sub addRefundToSalesLedger(ByVal orderID As Integer, ByVal countryCode As String, ByVal orderTotal As Decimal)
        Dim vatRate As Decimal = getVatRate()
        Dim goods As Decimal = 0
        Dim vat As Decimal = 0
        Dim hidOrderPrefix As HiddenField = fvOrder.FindControl("hidOrderPrefix")
        Try
            goods = FormatNumber(orderTotal / ((vatRate / 100) + 1), 2)
            vat = orderTotal - goods
            siteInclude.addToSalesLedger(orderID, 0, 2, countryCode, hidOrderPrefix.Value, 0, goods, 0, vat)
        Catch ex As Exception
            Try
                Dim lblRefundError As Label = fvOrder.FindControl("lblRefundError")
                lblRefundError.Text = "<font color='red'>An error occured while adding refund details to the sales ledger. (The refund may not appear on the ledger)<br>Please contact tech support.</font>"
                siteInclude.addError("affiliates/orderView.aspx.vb", "addRefundToSalesLedger2(orderid=" & orderID & ", countrycode=" & countryCode & ",total=" & orderTotal & "); " & ex.ToString)
            Catch ex2 As Exception
                siteInclude.addError("affiliates/orderView.aspx.vb", "addRefundToSalesLedger(); " & ex2.ToString)
            End Try
        End Try
    End Sub
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
    Protected Sub updateProtxStatus(ByVal status As String, ByVal statusDetail As String, ByVal vendorTxCode As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProtxByVendorTxCodeStatusUpdate", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 50))
        oCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
        oCmd.Parameters.Add(New SqlParameter("@statusDetail", SqlDbType.VarChar, 255))
        oCmd.Parameters("@vendorTxCode").Value = vendorTxCode
        oCmd.Parameters("@status").Value = status
        oCmd.Parameters("@statusDetail").Value = statusDetail
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("affiliates/orderView.aspx.vb", "updateProtxStatus(status=" & status & ", vendorTxCode=" & vendorTxCode & "); " & ex.ToString())
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub rebindPaymentPanels()
        Dim fv As FormView = fvOrder
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
        Dim tdProtxRelease As HtmlTableCell = fv.FindControl("tdProtxRelease")
        Dim tdProtxExtra As HtmlTableCell = fv.FindControl("tdProtxExtra")
        Dim panLog As Panel = fv.FindControl("panLog")
        Dim hid3dTransaction As HiddenField = fvOrder.FindControl("hid3dTransaction")
        If Not fv.Row Is Nothing Then
            Select Case LCase(lblType.Text)
                Case "cc"
                    lblType.Text = "Credit Card"
                    panPayment = fv.FindControl("panPaymentCC")
                    panCC.Visible = True
                    If LCase(lblStatus.Text) = "paid" Or LCase(lblStatus.Text) = "complete" Then
                        tdProtx.Visible = False
                        tdProtxRelease.Visible = False
                    End If
                    tdProtxExtra.Visible = Not tdProtx.Visible
                    If CBool(hid3dTransaction.Value) Then tdProtxExtra.Visible = False
                Case "phone"
                    lblType.Text = "Call Centre/CC"
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
                    If LCase(lblStatus.Text) = "paid" Or LCase(lblStatus.Text) = "complete" Then
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
            'If order is 3dSecure, then hide the older payment button and also hide the take extra funds payment button

            If drpStatus.SelectedValue = "deferred" Then
                If Not (LCase(lblStatus.Text) = "paid" Or LCase(lblStatus.Text) = "complete") Then tdProtxRelease.Visible = True
                tdProtx.Visible = False
                tdProtxExtra.Visible = False
            End If
        End If
    End Sub

    'Functions
    Protected Function showDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d.ToString, DateFormat.LongDate) & " " & FormatDateTime(d.ToString, DateFormat.ShortTime)
        Return result
    End Function
    Protected Function formatDateElement(ByVal d As String)
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
        Dim strVendorTxCode As String = "EBPAYMENT" & formatDateElement(Day(Date.Today)) & formatDateElement(Month(Date.Today)) & Year(Date.Today) & Hour(Now()) & Minute(Now()) & Second(Now()) & "_" & getUserOrderID(hidOrderID.Value) & UCase(Session("EBAffEBDistributorCountryCode"))
        'Dim tx As New Protx.Vsp.DirectTransaction(vendorTxCode, Protx.Vsp.VspTransactionType.Payment)
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
        Dim bOK As Boolean = True
        Dim vspResponse As Protx.Vsp.DirectResponse
        Dim SI As New siteInclude
        'SI = New siteInclude
        'SI.debug("Test123")
        'SI = Nothing
        Dim sHTML As String = ""
        Dim lblCCResults As Label = fv.FindControl("lblCCResults")
        Dim bDataError As Boolean = False
        'New protx code
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
            If ds.Tables(0).Rows.Count > 0 Then
                rs = ds.Tables(0).Rows(0)
                If extraPayment Then
                    strDescription = "EmotionalBliss Extra"
                    decTotal = FormatNumber(txtPartAmount.Text, 2, -1, 0, 0) '** Formatted to 2 decimal places with leading digit but no commas or currency symbols **
                Else
                    strDescription = "EmotionalBliss Payment"
                    Dim lblOrderTotal As Label = fvOrder.FindControl("lblOrderTotal")
                    decTotal = lblOrderTotal.Text
                End If
                countryCode = rs("orderCountryCode")
                strCurrency = UCase(rs("orderCurrency"))
                'Set values
                strConnectTo = "TEST"
                If Not Application("isDevBox") Then
                    strConnectTo = "LIVE"
                End If
                strVendorName = "peartree1"
                strGiftAid = "0"


                'Create post string
                strPost = "VPSProtocol=2.22"
                strPost = strPost & "&TxType=PAYMENT"
                strPost = strPost & "&Vendor=" & strVendorName
                strPost = strPost & "&VendorTxCode=" & strVendorTxCode
                strPost = strPost & "&Amount=" & decTotal
                strPost = strPost & "&Currency=" & strCurrency
                strPost = strPost & "&Description=" & URLEncode(strDescription)
                strPost = strPost & "&CardHolder=" & URLEncode(rs("billName"))
                strPost = strPost & "&CardNumber=" & decryptCardNo(rs("ccEnc"))
                If Not IsDBNull(rs("cardStart")) Then
                    If Len(rs("cardstart")) > 0 Then strPost = strPost & "&StartDate=" & rs("cardStart")
                End If
                strPost = strPost & "&ExpiryDate=" & rs("cardExp")
                If Not IsDBNull(rs("cardIssue")) Then
                    If Len(rs("cardIssue")) > 0 Then strPost = strPost & "&IssueNumber=" & rs("cardIssue")
                End If
                strPost = strPost & "&CV2=" & rs("cardCV2")
                strPost = strPost & "&CardType=" & rs("cardType")
                strPost = strPost & "&BillingAddress=" & URLEncode(rs("billAdd1"))
                strPost = strPost & "&BillingPostCode=" & URLEncode(rs("billPostcode"))
                'If bDeliverySame Then
                'strPost = strPost & "&DeliveryAddress=" & URLEncode(strBillingAddress)
                'strPost = strPost & "&DeliveryPostCode=" & URLEncode(strBillingPostCode)
                'Else
                'strPost = strPost & "&DeliveryAddress=" & URLEncode(strDeliveryAddress)
                'strPost = strPost & "&DeliveryPostCode=" & URLEncode(strDeliveryPostCode)
                'End If
                strPost = strPost & "&GiftAidPayment=0"
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
                    bOK = False
                    If Err.Number = -2147012889 Then
                        strPageError = "Your server was unable to register this transaction with Protx." & _
                        "  Check that you do not have a firewall restricting the POST and " & _
                        "that your server can correctly resolve the address " & siteInclude.getSagePayURL(strConnectTo, "puchase")
                    Else
                        strPageError = "An Error has occurred whilst trying to register this transaction.<BR>" & _
                        "The Error Number is: " & Err.Number & "<BR>" & _
                        "The Description given is: " & Err.Description
                    End If
                    strPageError = "An error occured while trying to contant your bank.<br>We appologies for any inconvenience. Please try again later.<br>Your card has not been charged and the order is incomplete."
                Else
                    '** No transport level errors, so the message got the Protx **
                    '** Analyse the response from VSP Direct to check that everything is okay **
                    '** Registration results come back in the Status and StatusDetail fields **
                    strStatus = findField("Status", strResponse)
                    strStatusDetail = findField("StatusDetail", strResponse)
                    If LCase(strStatus) <> "ok" Then bOK = False


                End If
            End If
        Catch ex As Exception
            SI = New siteInclude
            SI.addError("affiliates/orderView.asp.vb", "take3DPayment2(id=" & hidOrderID.Value & "); " & strPageError & "; " & ex.ToString)
            SI = Nothing
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
                    SI.AddToOrderLog(hidOrderID.Value, "Card Successfully Charged, txID=" & strVendorTxCode, userName, True)
                Else
                    sHTML = sHTML & "<font color='red'>Error occured</font><br>"
                    SI.AddToOrderLog(hidOrderID.Value, "Transaction attempted, txID=" & strVendorTxCode & ".<br>Error Message:" & strStatusDetail, userName, True)
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
                oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 50))
                oCmd.Parameters("@vendorTxCode").Value = strVendorTxCode
                oCmd.Parameters.Add(New SqlParameter("@VPSProtocol", SqlDbType.VarChar, 10))
                oCmd.Parameters("@VPSProtocol").Value = findField("VPSProtocol", strResponse)
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
                oCmd.Parameters("@orderID").Value = CInt(hidOrderID.Value)
                oCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                oCmd.Parameters("@currency").Value = strCurrency
                oCmd.Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                oCmd.Parameters("@country").Value = countryCode
                oCmd.Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
                oCmd.Parameters("@bank").Value = Convert.ToDecimal("1.0")
                oCmd.Parameters.Add(New SqlParameter("@newOrderStatus", SqlDbType.VarChar, 10))
                oCmd.Parameters("@newOrderStatus").Value = sStatus
                oCmd.Parameters.Add(New SqlParameter("@extraPayment", SqlDbType.Bit))
                oCmd.Parameters("@extraPayment").Value = extraPayment
                If UCase(strStatus) = "OK" Then
                    oCmd.Parameters("@VPSTxID").Value = findField("VPSTxId", strResponse)
                    oCmd.Parameters("@txAuthNo").Value = findField("TxAuthNo", strResponse)
                    oCmd.Parameters("@securityKey").Value = findField("SecurityKey", strResponse)
                    oCmd.Parameters("@AVSCV2").Value = findField("AVSCV2", strResponse)
                Else
                    oCmd.Parameters("@VPSTxID").Value = ""
                    oCmd.Parameters("@txAuthNo").Value = ""
                    oCmd.Parameters("@securityKey").Value = ""
                    oCmd.Parameters("@AVSCV2").Value = ""
                End If
            Catch ex As Exception
                SI = New siteInclude
                SI.addError("affilaites/orderView.aspx.vb", "protxTestCode(err2); " & ex.ToString)
                SI = Nothing
            End Try
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                SI = New siteInclude
                SI.addError("affilaites/orderView.aspx.vb", "protxTestCode(err3; " & ex.ToString)
                SI = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Else '!bOK
            'Add the failure to protx table
            sStatus = "Failed"
            Try
                'payment failed, add to order log
                Dim inc As siteInclude
                inc.AddToOrderLog(hidOrderID.Value, "Payment failed.<br>Error code " & strStatusDetail.ToString(), Membership.GetUser.UserName, True)
                inc = Nothing
                'Rebind order log to show failre details
                Dim gv As GridView = fvOrder.FindControl("gvTrace")
                gv.DataBind()
                'Add to protx table
                oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                oCmd = New SqlCommand("procProtxInsert", oConn)
                oCmd.CommandType = CommandType.StoredProcedure
                oCmd.Parameters.Add(New SqlParameter("@vendorTxCode", SqlDbType.VarChar, 50))
                oCmd.Parameters("@vendorTxCode").Value = strVendorTxCode
                oCmd.Parameters.Add(New SqlParameter("@VPSProtocol", SqlDbType.VarChar, 10))
                oCmd.Parameters("@VPSProtocol").Value = findField("VPSProtocol", strResponse)
                oCmd.Parameters.Add(New SqlParameter("@VPSTxID", SqlDbType.VarChar, 40))
                oCmd.Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                oCmd.Parameters("@status").Value = "Failed"
                oCmd.Parameters.Add(New SqlParameter("@statusDetail", SqlDbType.VarChar, 255))
                Try
                    oCmd.Parameters("@statusDetail").Value = strStatusDetail
                Catch ex As Exception
                    SI.addError("affiliatesOrderView.aspx.vb", "testProtxCode(id=" & hidOrderID.Value & "); " & ex.ToString)
                    oCmd.Parameters("@statusDetail").Value = "Possible routing/protx problem. See error log for more info"
                End Try
                oCmd.Parameters.Add(New SqlParameter("@txAuthNo", SqlDbType.VarChar, 20))
                oCmd.Parameters.Add(New SqlParameter("@securityKey", SqlDbType.VarChar, 20))
                oCmd.Parameters.Add(New SqlParameter("@AVSCV2", SqlDbType.VarChar, 50))
                oCmd.Parameters.Add(New SqlParameter("@amount", SqlDbType.Money))
                oCmd.Parameters("@amount").Value = decTotal
                oCmd.Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                oCmd.Parameters("@orderID").Value = CInt(hidOrderID.Value)
                oCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                oCmd.Parameters("@currency").Value = strCurrency
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
                SI = New siteInclude
                SI.addError("affilaites/orderView.aspx.vb", "protxTestCode(adding to db after failure); " & ex.ToString)
                SI = Nothing
            End Try
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                SI = New siteInclude
                SI.addError("affilaites/orderView.aspx.vb", "protxTestCode(adding to db after failure2); " & ex.ToString)
                SI = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If 'bOK
        If bOK Then
            'Show results in lblTransactionResults Label, Page_Load has enabled it
            sHTML = sHTML & "Protocol: " & findField("VPSProtocol", strResponse) & "<br>"
            sHTML = sHTML & "Status: " & strStatus & "<br>"
            sHTML = sHTML & "Response: " & strStatusDetail & "<br>"
            If "OK" = UCase(strStatus) Then
                sHTML = sHTML & "Auth No: " & findField("TxAuthNo", strResponse) & "<br><br>"
            Else
                sHTML = sHTML & "Auth No: <br><br>"
            End If
        Else
            Try
                sHTML = sHTML & strStatusDetail
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
        Return sStatus
    End Function
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
        siteInclude.debug("shipping=" & lblShipping.Text)
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
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
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
    Protected Function getVatRate() As Decimal
        Dim result As Decimal = 0
        Dim countryCode As String = Session("EBAffEBDistributorCountryCode")
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
    Protected Function formatSource(ByVal o As Object) As String
        Dim result As String = ""
        If Not IsDBNull(o) Then
            Select Case o.ToString
                Case "shopper"
                    result = "Web Shopper"
                Case "affiliate"
                    result = "Affiliate"
                Case "distributor"
                    result = "Distributor"
                Case "callcentre"
                    result = "Call Centre"
            End Select
        End If
        Return result
    End Function
    Protected Function formatPayment(ByVal o As Object) As String
        Dim result As String = ""
        If Not IsDBNull(o) Then
            Select Case o.ToString
                Case "account"
                    result = "EB Account"
                Case "bankaccount"
                    result = "Bank Account"
                Case "cc"
                    result = "Credit Card"
                Case "cheque"
                    result = "Cheque"
                Case "ideal"
                    result = "iDeal"
            End Select
        End If
        Return result
    End Function
    Protected Sub setTaxValues(ByRef vat As TextBox, ByRef vatShip As TextBox)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIdTaxesSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters("@id").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                vat.Text = ds.Tables(0).Rows(0)("goodsVat")
                vatShip.Text = ds.Tables(0).Rows(0)("shippingVat")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("affiliates/orderViwe.aspx.vb", "setTaxValues(id=" & Request.QueryString("id") & "); " & ex.ToString())
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function showIs3DSecure(ByVal o As Object) As String
        Dim result As String = "No"
        If Not IsDBNull(o) Then
            If LCase(o.ToString()) = "true" Then result = "Yes"
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

