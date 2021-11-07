Imports System.Data

Partial Class shop_postback
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ice As New ICEPAY.ICEPAY(Application("icepayMerchantID"), Application("icepayMerchantCode"))
        If ice.OnPostBack Then
            Dim data As ICEPAY.ICEPAY.Postback = ice.GetPostback()
            Select Case data.status.ToUpper()
                Case "OK" ' Successful payment
                    Session("EBTmpIcepaySource") = ""
                    Session("EBTmpIcepayErrorcode") = ""
                    Session("EBTmpIcepayPaymentType") = ""
                    Session("EBTmpUniqueID") = Request.QueryString("Reference")
                Case "OPEN" ' Payment is not yet completed
                Case "ERR" ' Error happened
                    'Session("EBTmpIcepaySource") = "icepay"
                    'Session("EBTmpIcepayErrorcode") = data.statusCode
                    'Session("EBTmpIcepayPaymentType") = LCase(Request.QueryString("PaymentMethod"))
                    'Session("EBTmpUniqueID") = Request.QueryString("Reference")
                    'siteInclude.debug("postback:" & LCase(Request.QueryString("PaymentMethod")))
                Case "REFUND" ' Merchant did a refund
                Case "CBACK" ' Charge back by end-user
            End Select
        End If
    End Sub
End Class
