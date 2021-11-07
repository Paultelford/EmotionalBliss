Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net

Partial Class _3DComplete
    Inherits System.Web.UI.Page
    Public strStatus As String = ""
    Private emailBody As String = ""
    Private voucherOnlyOrder As Boolean = False
    Private purchaserEmail As String = ""
    Private voucherNumber As String = ""
    Private _voucherNumber As String = ""
    Private Const _maxDeliveryLength As Integer = 200

    'to return the appropriate css class depending on the status returned
    Function getStyle() As String
        If strStatus = "OK" Or strStatus = "AUTHENTICATED" Or strStatus = "REGISTERED" Then
            getStyle = "infoheader"
        Else
            getStyle = "errorheader"
        End If
        Return getStyle
    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'siteInclude.debug("3DComplete.vb::Page_Load(); Session(EBTmpOrderID)=" & Session("EBTmpOrderID"))
        Dim strMD As String = ""
        Dim strPaRes As String = ""
        Dim strVendorTxCode As String = ""
        Dim strPost As String = ""
        Dim strResponse As String = ""
        Dim strPageError As String = ""
        Dim strStatusDetail As String = ""
        Dim strACSURL As String = ""
        Dim strPAReq As String = ""
        Dim strPageState As String = ""
        Dim strVPSTxId As String = ""
        Dim strCurrency As String
        Dim strSecurityKey As String = ""
        Dim strTxAuthNo As String = ""
        Dim strAVSCV2 As String = ""
        Dim strAddressResult As String = ""
        Dim strPostCodeResult As String = ""
        Dim strCV2Result As String = ""
        Dim str3DSecureStatus As String = ""
        Dim strCAVV As String = ""
        Dim strDBStatus As String = ""
        Dim strSQL As String = ""
        Dim strGiftAid As String = ""
        Dim strCompletionURL As String = ""
        Dim strThisEntry As String = ""
        Dim intQuantity As Integer
        Dim decTotal As Decimal
        Dim orderID As Integer = 0
        Dim strCountryCode As String = ""
        Dim strConnectTo As String = "TEST"
        Dim result As String = ""
        If Not Application("isDevBox") Then
            strConnectTo = "LIVE"
        End If
        orderID = Session("EBTmpUniqueID")
        ' ** Otherwise, create the POST for Protx ensuring to URLEncode the PaRes before sending it **
        If Not Request.Form("MD") Is Nothing Then
            strMD = Request.Form("MD")
            strPaRes = Request.Form("PaRes")

            '** POST for Protx VSP Direct 3D completion page **
            strPost = "MD=" & strMD & "&PARes=" & strPaRes

            '** The full transaction registration POST has now been built **
            Dim objUTFEncode As New UTF8Encoding
            Dim arrRequest As Byte()
            Dim objStreamReq As Stream
            Dim objStreamRes As StreamReader
            Dim objHttpRequest As HttpWebRequest
            Dim objHttpResponse As HttpWebResponse
            Dim objUri As New Uri(siteInclude.getSagePayURL(strConnectTo, "3dcallback"))
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
                    "that your server can correctly resolve the address " & SystemURL(strConnectTo, "puchase")
                Else
                    strPageError = "An Error has occurred whilst trying to register this transaction.<BR>" & _
                    "The Error Number is: " & Err.Number & "<BR>" & _
                    "The Description given is: " & Err.Description
                End If

            Else
                '** No transport level errors, so the message got the Protx **
                '** Analyse the response from VSP Direct to check that everything is okay **
                '** Registration results come back in the Status and StatusDetail fields **
                strStatus = findField("Status", strResponse)
                strStatusDetail = findField("StatusDetail", strResponse)

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
                strCurrency = findField("Currency", strResponse)
                strVendorTxCode = Session("VendorTxCode")
                decTotal = Session("orderTotal")
                strCountryCode = Session("EBShopCountry")
                strCurrency = UCase(Session("Currency"))

                '** Great, the signatures DO match, so we can update the database and redirect the user appropriately **
                Dim bErrorLogged As Boolean = False
                If strStatus = "OK" Then
                    strDBStatus = "AUTHORISED - The transaction was successfully authorised with the bank."
                    siteInclude.AddToOrderLog(orderID, strDBStatus & "; txID=" & strVendorTxCode, "Payment System", True)
                    bErrorLogged = True
                ElseIf strStatus = "MALFORMED" Then
                    strDBStatus = "MALFORMED - The StatusDetail was:" & SQLSafe(Left(strStatusDetail, 255))
                ElseIf strStatus = "INVALID" Then
                    strDBStatus = "INVALID - The StatusDetail was:" & SQLSafe(Left(strStatusDetail, 255))
                ElseIf strStatus = "NOTAUTHED" Then
                    strDBStatus = "DECLINED - The transaction was not authorised by the bank."
                ElseIf strStatus = "REJECTED" Then
                    strDBStatus = "REJECTED - The transaction was failed by your 3D-Secure or AVS/CV2 rule-bases."
                ElseIf strStatus = "AUTHENTICATED" Then
                    strDBStatus = "AUTHENTICATED - The transaction was successfully 3D-Secure Authenticated and can now be Authorised."
                ElseIf strStatus = "REGISTERED" Then
                    strDBStatus = "REGISTERED - The transaction was could not be 3D-Secure Authenticated, but has been registered to be Authorised."
                ElseIf strStatus = "ERROR" Then
                    strDBStatus = "ERROR - There was an error during the payment process.  The error details are: " & SQLSafe(strStatusDetail)
                Else
                    
                    strDBStatus = "UNKNOWN - An unknown status was returned from Protx.  The Status was: " & SQLSafe(strStatus) & ", with StatusDetail:" & SQLSafe(strStatusDetail)
                    Dim si As New siteInclude
                    si.AddToOrderLog(orderID, "Protx Error: " & strDBStatus & "; txID=" & strVendorTxCode, "Payment System", True)
                    si = Nothing
                    bErrorLogged = True
                    strDBStatus = "An Unknown error occured.<br>We appologies for any inconvenience. Please try again later.<br>Your card has not been charged and the order is incomplete."
                End If
                result = strStatus
                If Not bErrorLogged Then
                    'Add error to order log - Unless it was already added (Only happens if its an Unknown Protx error)
                    'Dim si As New siteInclude
                    siteInclude.AddToOrderLog(orderID, "Protx Error: " & strDBStatus & "; txID=" & strVendorTxCode, "Payment System", True)
                    'si = Nothing
                End If



                'set the values for display on the page. 
                'lblStatus.Text = strStatus
                'lblPost.Text = strPost
                'lblReply.Text = strResponse
                'lblVendorTxCode.Text = strVendorTxCode
                'lblVPSTxId.Text = strVPSTxId
                'lblSecurityKey.Text = strSecurityKey
                'lblTxAuthNo.Text = strTxAuthNo
                'lbl3DSecure.Text = str3DSecureStatus
                'lblCAVV.Text = strCAVV

                If LCase(strStatus) = "ok" Then
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
                        oCmd.Parameters("@orderID").Value = orderID
                        oCmd.Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                        oCmd.Parameters("@currency").Value = strCurrency
                        oCmd.Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                        oCmd.Parameters("@country").Value = strCountryCode
                        oCmd.Parameters.Add(New SqlParameter("@bank", SqlDbType.Decimal))
                        oCmd.Parameters("@bank").Value = Convert.ToDecimal("1.0")
                        oCmd.Parameters.Add(New SqlParameter("@newOrderStatus", SqlDbType.VarChar, 10))
                        'Changed 12-10-2011 sp
                        oCmd.Parameters("@newOrderStatus").Value = "paid"
                        'oCmd.Parameters("@newOrderStatus").Value = "deferred"
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
                        lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details."
                        Dim si As New siteInclude
                        si.addError("shop/payment.aspx", "takePayment_liveAuthAddParam(id=" & ID & "); " & ex.ToString)
                        si = Nothing
                        Response.End()
                    End Try
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        lblError.Text = "<font color='red'>An error occured while charging you card. Please check you entered the correct details."
                        Dim si As New siteInclude
                        si.addError("shop/payment.aspx", "takePayment_liveAuthExSql(id=" & ID & "); " & ex.ToString)
                        si = Nothing
                        Response.End()
                    Finally
                        oCmd.Dispose()
                        oConn.Dispose()
                    End Try
                    'siteInclude.debug("Sending email conf from 3DComplete.aspx.vb, Session(EBTmpOrderID)=" & Session("EBTmpOrderID") & ", Session(EBTmpuqID)=" & Session("EBTmpuqID"))
                    'sendConfirmationEmail(Session("EBTmpUniqueID"), Session("EBTmpuqID"), "confirmation")
                    'siteInclude.debug("Sent")
                Else
                    'Payment failed, delete the order
                    siteInclude.addError("3dcomplete.aspx.vb", "**3DComplete Deleted, strStatus=" & strStatus & ",orderid=" & orderID)
                    deleteOrder(orderID)
                End If
                '** Work out where to send the customer **
                Session("VendorTxCode") = strVendorTxCode
                If strStatus = "OK" Or strStatus = "AUTHENTICATED" Or strStatus = "REGISTERED" Then
                    Response.Redirect("receipt.aspx")
                Else

                    strPageError = strDBStatus
                    Context.Items.Add("errorcode", strPageError)
                    Context.Items.Add("source", "3dcomplete")
                    Server.Transfer("payment.aspx", True)
                End If

                '** Finally, if we're in LIVE then go stright to the success page **
                '** In other modes, we allow this page to display and ask for Proceed to be clicked **

                If strPageError.Length > 0 Then
                    'pnlError.Visible = True
                    'lblError.Text = strPageError
                Else
                    'pnlNoError.Visible = True
                End If
            End If
        End If
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
            siteInclude.addError("shop/3DComplete.aspx.vb", "deleteOrder(uqID=" & uqID & "); " & ex.ToString)
        End Try
    End Sub
    Protected Sub proceed_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Clear()
        Response.Redirect(Request.Form("CompletionURL"))
        Response.End()
    End Sub
    Protected Sub back_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Response.Clear()
        Server.Transfer("customerDetails.aspx")
        Response.End()
    End Sub
    'Functions
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
    Protected Sub sendConfirmationEmail(ByVal newOrderID As String, ByVal orderID As Integer, ByVal type As String)

        Dim toAdd As String = ""
        Dim ccAdd As String = ""
        Dim subject As String = ""
        emailBody = ""
        Dim msg As MailMessage
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
        Dim client As New SmtpClient
        client.Send(msg)
        msg.Dispose()
    End Sub
    Protected Sub makeEmail(ByVal emailType As String, ByVal newOrderID As String, ByVal orderID As Integer, ByRef toAdd As String, ByRef ccAdd As String, ByRef subject As String, ByVal country As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailMasterByCountryCodeTypeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim row As DataRow
        Dim billAdd As String = ""
        Dim shipAdd As String = ""
        getAddressAndEmail(orderID, billAdd, shipAdd, toAdd)
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
                toAdd = toAdd
                subject = row("emailSubject") 'Set Subject from db
                For Each r As DataRow In ds.Tables(0).Rows
                    'Set email body
                    emailBody = emailBody & r("text")
                Next
                If lcase(emailType) = "order confirmation" Then
                    'Normal order                    
                    'Replace special commands in email body
                    emailBody = replace(emailBody, "@orderID", newOrderID & ucase(lcase(session("EBShopCountry"))))
                    emailBody = Replace(emailBody, "@date", FormatDateTime(Now(), DateFormat.ShortDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime))
                    emailBody = Replace(emailBody, "@billingAdd", billAdd)
                    emailBody = Replace(emailBody, "@shippingAdd", shipAdd)
                    emailBody = replace(emailBody, "@items", getItemText())
                    emailBody = replace(emailBody, "@totalPrice", Profile.EBCart.CurrencySign & FormatNumber(CDec(Profile.EBCart.TotalInc), 2))
                Else
                    'Voucher order
                    'If txtEmail="" AND the order is for a voucher, then use the purchaser's email address from the voucher
                    If LCase(toAdd) = "noreply@emotionalbliss.com" And purchaserEmail <> "" Then toAdd = purchaserEmail
                    'Replace special commands in email body
                    row = getVoucherDetails(orderID)
                    ccAdd = row("purchaserEmail")
                    toAdd = row("recipientEmail")
                    emailBody = replace(emailBody, "@recipient", row("recipient"))
                    emailBody = replace(emailBody, "@purchaser", row("purchaser"))
                    emailBody = replace(emailBody, "@recipientEmail", row("recipientEmail"))
                    emailBody = replace(emailBody, "@purchaserEmail", row("purchaserEmail"))
                    emailBody = replace(emailBody, "@voucherAmount", Profile.EBCart.CurrencySign & row("credit"))
                    emailBody = replace(emailBody, "@voucherNumber", row("number"))
                    emailBody = replace(emailBody, "@comment", row("comment"))
                    'Not sure if still needed
                    _voucherNumber = row("number")
                    purchaserEmail = row("purchaserEmail")
                End If
            Else
                'No results were found, this means the email has not been created for the current country yet.
                'Use GB as default
                'siteInclude.debug("Using GB as default")
                makeEmail(emailType, newOrderID, orderID, toAdd, ccAdd, subject, "gb")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/3dcomplete.aspx.vb", "makeEmail(orderID=" & orderID & ",emailType=" & emailType & ",countryCode=" & Session("EBShopCountry") & "); " & ex.ToString())
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub getAddressAndEmail(ByVal id As Integer, ByRef bill As String, ByRef ship As String, ByRef email As String)
        Dim add As String = ""
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopCustomerByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
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
                bill = row("billName") & Chr(10)
                If Not IsDBNull(row("billAdd1")) Then If row("billAdd1") <> "" Then bill = bill & row("billAdd1") & Chr(10)
                If Not IsDBNull(row("billAdd2")) Then If row("billAdd2") <> "" Then bill = bill & row("billAdd2") & Chr(10)
                If Not IsDBNull(row("billAdd3")) Then If row("billAdd3") <> "" Then bill = bill & row("billAdd3") & Chr(10)
                If Not IsDBNull(row("billAdd4")) Then If row("billAdd4") <> "" Then bill = bill & row("billAdd4") & Chr(10)
                If Not IsDBNull(row("billAdd5")) Then If row("billAdd5") <> "" Then bill = bill & row("billAdd5") & Chr(10)
                If Not IsDBNull(row("billPostcode")) Then If row("billPostcode") <> "" Then bill = bill & row("billPostcode") & Chr(10)
                If Not IsDBNull(row("billCountry")) Then If row("billCountry") <> "" Then bill = bill & row("billCountry") & Chr(10)
                ship = row("shipName") & Chr(10)
                If Not IsDBNull(row("shipAdd1")) Then If row("shipAdd1") <> "" Then ship = ship & row("shipAdd1") & Chr(10)
                If Not IsDBNull(row("shipAdd2")) Then If row("shipAdd2") <> "" Then ship = ship & row("shipAdd2") & Chr(10)
                If Not IsDBNull(row("shipAdd3")) Then If row("shipAdd3") <> "" Then ship = ship & row("shipAdd3") & Chr(10)
                If Not IsDBNull(row("shipAdd4")) Then If row("shipAdd4") <> "" Then ship = ship & row("shipAdd4") & Chr(10)
                If Not IsDBNull(row("shipAdd5")) Then If row("shipAdd5") <> "" Then ship = ship & row("shipAdd5") & Chr(10)
                If Not IsDBNull(row("shipPostcode")) Then ship = ship & row("shipPostcode") & Chr(10)
                If Not IsDBNull(row("shipCountry")) Then ship = ship & row("shipCountry") & Chr(10)
                email = row("email")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/3dcomplete.aspx.vb", "getAddressAndEmail(id=" & id & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
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
            si.addError("shop/3dcomplete.aspx.vb", "getVoucherDetails(orderID=" & orderID & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return row
    End Function
End Class
