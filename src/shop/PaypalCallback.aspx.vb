Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Text

Partial Class shop_PaypalCallback
    Inherits System.Web.UI.Page
    Private strResponse As String
    Private bTestMode As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        strResponse = URLDecode(Request.Form.ToString)
        siteInclude.debug("reply2=" & strResponse)
        If LCase(findField("Status", strResponse)) = "paypalok" Then
            'Customer has entered card details, they have been accepted and the transaction is ready to be made. Complete post must be sent by us to finalise the transaction
            'Add details to database first
            siteInclude.debug("Adding reply2 to database")
            Try
                Dim param() As String = {"@orderID", "@VPSProtocol", "@Status", "@StatusDetail", "@VPSTxID", "@addressStatus", "@payerStatus", "@DeliverySurname", "@DeliveryFirstname", "@DeliveryAddress1", "@DeliveryCity", "@DeliveryPostcode", "@DeliveryCountry", "@CustomerEmail", "@PayerID"}
                Dim paramValue() As String = {Session("EBTmpUniqueID"), findField("VPSProtocol", strResponse), findField("Status", strResponse), findField("StatusDetail", strResponse), findField("VPSTxId", strResponse), findField("AddressStatus", strResponse), findField("PayerStatus", strResponse), findField("DeliverySurname", strResponse), findField("DeliveryFirstnames", strResponse), findField("DeliveryAddress1", strResponse), findField("DeliveryCity", strResponse), findField("DeliveryPostCode", strResponse), findField("DeliveryCountry", strResponse), findField("CustomerEMail", strResponse), findField("PayerID", strResponse)}
                Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar}
                Dim paramSize() As Integer = {0, 4, 15, 255, 40, 20, 20, 20, 20, 100, 40, 10, 2, 255, 15}
                siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procPaypalInsert2")
            Catch ex As Exception
                siteInclude.addError("paypalCallback.sapx", "Page_Load(paypalok:VPSTxID=" & findField("VPSTxId", strResponse) & "); " & ex.ToString)
            Finally
            End Try
            siteInclude.debug("sending to CompleteTransaction")
            completeTransaction()
        Else
            'OrderID should still be stored in Session("EBTmpUniqueID")
            Session("EBTmpStatus") = findField("Status", strResponse)
            Session("EBTmpStatusDetail") = findField("StatusDetail", strResponse)
            Session("EBTmpError") = "paypal"
            If Not bTestMode Then Server.Transfer("payment.aspx")
        End If


        lblOrderID.Text = Session("EBTmpUniqueID")
        lblStatus.Text = findField("Status", strResponse)
        lblStatusDetail.Text = findField("StatusDetail", strResponse)
        lblProtocol.Text = findField("VPSProtocol", strResponse)
    End Sub

    Public Shared Function findField(ByVal strFieldName As String, ByVal strThisResponse As String) As String
        Dim iItem As Integer
        Dim arrItems(1) As String
        Dim strFindField As String = ""
        arrItems = Split(strThisResponse, "&")
        For iItem = LBound(arrItems) To UBound(arrItems)
            If InStr(arrItems(iItem), strFieldName & "=") = 1 Then
                strFindField = Mid(arrItems(iItem), Len(strFieldName) + 2)
                Exit For
            End If
        Next
        Return Replace(strFindField, "+", " ")
    End Function
    Public Shared Function findFieldCRLF(ByVal strFieldName As String, ByVal strThisResponse As String) As String
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
        Return Replace(strFindField, "+", " ")
    End Function

    Protected Sub lnkPaypalOK_click(ByVal sender As Object, ByVal e As EventArgs)
        completeTransaction()
    End Sub
    Protected Sub completeTransaction()
        'Send the 'COMPLETE' tx post to sagepay
        Dim strPost As String = ""
        Dim strVendorName As String = "peartree1"
        Dim strConnectTo As String = "TEST"
        Dim strDescription As String = "ESL Order"
        If Not Application("isDevBox") Then
            strConnectTo = "LIVE"
        End If
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@orderID", "@replyNo"}
            Dim paramValue() As String = {Session("EBTmpUniqueID"), "1"}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.Int}
            Dim paramSize() As Integer = {0, 0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procPaypalByOrderIDReplySelect")
            If dt.Rows.Count > 0 Then
                strPost = "VPSProtocol=2.23"
                'strPost = strPost & "&Vendor=" & strVendorName
                'strPost = strPost & "&Currency=GBP"
                'strPost = strPost & "&VendorTxCode=" & dt.Rows(0)("vendorTxCode")
                'strPost = strPost & "&Description=" & URLEncode(strDescription)
                strPost = strPost & "&TxType=" & "COMPLETE"
                strPost = strPost & "&Amount=" & FormatNumber(dt.Rows(0)("amount"), 2, -1, 0, 0) '** Formatted to 2 decimal places with leading digit but no commas or currency symbols **
                strPost = strPost & "&Accept=YES"
                strPost = strPost & "&VPSTxId=" & dt.Rows(0)("VPSTxId")
                If bTestMode Then siteInclude.debug(strPost)
                '** The full transaction registration POST has now been built **
                Dim objUTFEncode As New UTF8Encoding
                Dim arrRequest As Byte()
                Dim objStreamReq As Stream
                Dim objStreamRes As StreamReader
                Dim objHttpRequest As HttpWebRequest
                Dim objHttpResponse As HttpWebResponse
                Dim objUri As New Uri(siteInclude.getSagePayURL(strConnectTo, "complete"))
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
                siteInclude.debug("replay3=" & strResponse)
                objStreamRes.Close()
                If Err.Number <> 0 Then
                    Session("EBTmpStatusDetail") = "An Error has occurred whilst trying to register this transaction.<BR>The Error Number is: " & Err.Number & "<BR>The Description given is: " & Err.Description
                    Session("EBTmpStatus") = findFieldCRLF("Status", strResponse)
                    Session("EBTmpError") = "paypal"
                    Response.Redirect("payment.aspx")
                Else
                    If LCase(findFieldCRLF("Status", strResponse)) = "ok" Then
                        'Final confirmation has been returned by sagepay, add to database then redirect to the receipt page
                        finalRedirect(strResponse)
                    Else
                        Session("EBTmpStatus") = findFieldCRLF("Status", strResponse)
                        'Session("EBTmpStatus") = "Test error"
                        Session("EBTmpStatusDetail") = findFieldCRLF("StatusDetail", strResponse)
                        Session("EBTmpError") = "paypal"
                        Response.Redirect("payment.aspx")
                    End If
                End If
            End If
        Catch ex As Exception
            siteInclude.addError("paypalCallback.sapx", "lnkPaypalOK_click(orderid=" & Session("EBTmpUniqueID") & "); " & ex.ToString)
        Finally
            dt.Dispose()
        End Try
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
    Protected Sub finalRedirect(ByVal strResponse As String)
        Dim orderID As String = Session("EBTmpUniqueID")
        Try
            Dim param() As String = {"@orderID", "@VPSProtocol", "@status", "@statusDetail", "@VPSTxId", "@SecurityKey"}
            Dim paramValue() As String = {orderID, findFieldCRLF("VPSProtocol", strResponse), findFieldCRLF("Status", strResponse), findFieldCRLF("StatusDetail", strResponse), findFieldCRLF("VPSTxId", strResponse), findFieldCRLF("SecurityKey", strResponse)}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar}
            Dim paramSize() As Integer = {0, 4, 15, 255, 40, 10}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procPaypalInsert3")
        Catch ex As Exception
            siteInclude.addError("paypalCallback.sapx", "Page_Load(ok:VPSTxID=" & findFieldCRLF("VPSTxId", strResponse) & "); " & ex.ToString)
        Finally
        End Try
        'Payment has been taken, update order status
        updateOrderStatus(orderID, "Paid")
        'Set the paymentDate in ShopOrder to current datetime
        updatePaymentDate(orderID)
        If Not bTestMode Then Response.Redirect("receipt.aspx?auth=paypal")
    End Sub
    Protected Sub lnkOK_click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("receipt.aspx?auth=paypal")
    End Sub
    Protected Sub lnkFail_click(ByVal sender As Object, ByVal e As EventArgs)
        Context.Items("source") = "paypal"
        Server.Transfer("payment.aspx")
    End Sub

    Protected Sub updatePaymentDate(ByVal orderID As Integer)
        Try
            Dim param() As String = {"@id"}
            Dim paramValue() As String = {orderID.ToString}
            Dim paramType() As SqlDbType = {SqlDbType.Int}
            Dim paramSize() As Integer = {0}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procShopOrderByIDPaymentDateUpdate")
        Catch ex As Exception
            siteInclude.addError("shop/paypalcallback.aspx.vb", "updatePaymentDate(orderid=" & orderID & ");" & ex.ToString())
        End Try
    End Sub
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
    Public Shared Function URLDecode(ByVal strString As String) As String
        Return HttpUtility.UrlDecode(strString)
    End Function
    Public Shared Function URLEncode(ByVal strString As String) As String
        Return HttpUtility.UrlEncode(strString)
    End Function
End Class
