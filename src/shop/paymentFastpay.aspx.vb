
Partial Class shop_paymentFastpay
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            form1.Method = "POST"
            form1.Action = "http://www.fasterpay.co.uk/payfaster.php"
            'form1.Action = "testFP.aspx"
            'FPIDENTITY.Text = Session("EBTmpuqID")
            'FPNAME.Text = Session("EBTmpOrderID") & UCase(Session("EBShopCountry"))
            'FPIDENTIFY.Text = "2069"
            'FPNAME.Text = "Order No.10763GB"
            'FPPRICE.Text = FormatNumber(Profile.EBCart.TotalInc, 2)
            'FPPRICE.Text = "1.00"

            'FPMERCHANT.Text = "55697"
            Dim cs As ClientScriptManager = Page.ClientScript
            cs.RegisterStartupScript(Me.GetType(), "loadsubmit", "self.setTimeout(""document." & form1.UniqueID & ".submit();"",200);", True)
        End If
    End Sub

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        'Session("EBTmp_FPIDENTITY") = ""
        'Session("EBTmp_FPPRICE") = ""
        'Session("EBTmp_FPNAME") = ""
    End Sub
End Class
