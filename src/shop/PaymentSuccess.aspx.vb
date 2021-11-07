Imports System.Data
Imports System.Data.SqlClient
Imports EmailManagements
Partial Class shop_PaymentSuccess
    Inherits System.Web.UI.Page
    Protected Sub btnbacktohome_Click(sender As Object, e As EventArgs) Handles btnbacktohome.Click
        Response.Redirect("~\homeIntro.aspx")
    End Sub

    Private Sub btnbacktohome_Load(sender As Object, e As EventArgs) Handles btnbacktohome.Load
        Dim OrderID As Integer = 0
        If Not Page.IsPostBack Then
            If (Request.QueryString("guid") = Nothing) Then

                'GetSMTPDetails().Send(GetEmailContent("Customer Quote", GetInqueryandQuoteContent(null, objQuoteModel), WebConfigurationManager.AppSettings["FromEmail"]));
                'EmailManagement.GetSMTPDetails().Send(EmailManagement.GetEmailContent("",))
                lblPaymentMsg.Text = "Your Payment Sucessfully"
                lblPaymentID.Text = "Payment ID :- " + Request.QueryString("PaymentID")
                OrderID = Convert.ToInt32(Request.QueryString("OrderID"))
                updateOrderStatus(OrderID, "paid")
                EmailManagements.EmailManagement.SendCreditCardEmail(OrderID, Request.QueryString("PaymentID"))
                'Profile.EBCart.emptyBasket()
            Else
                'lblPaymentID.Text = "Payment ID :- " + Request.QueryString("PaymentID")
                'OrderID = Convert.ToInt32(Request.QueryString("OrderID"))
                'updateOrderStatus(OrderID, "paid")
                If ((Request.QueryString("paymentId") = Nothing) And (Request.QueryString("PayerID") = Nothing)) Then
                    lblPaymentMsg.Text = "Your Payment UnSucessfully"
                    lblTokenID.Text = "Token :- " + Request.QueryString("token")
                    updateOrderStatus(OrderID, "fail")
                Else
                    lblPaymentMsg.Text = "Your Payment Sucessfully"
                    OrderID = Convert.ToInt32(Request.QueryString("guid"))
                    lblPaymentID.Text = "Payment ID :- " + Request.QueryString("paymentId")
                    lblTokenID.Text = "Token :- " + Request.QueryString("token")
                    lblPayerID.Text = "PayerID :- " + Request.QueryString("PayerID")
                    updateOrderStatus(OrderID, "paid")
                    EmailManagements.EmailManagement.SendPayPalEmail(OrderID, Request.QueryString("paymentId"), Request.QueryString("token"), Request.QueryString("PayerID"))
                End If
            End If

            Response.Redirect("receipt.aspx")
        End If


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
            siteInclude.addError("shop/payment.aspx", "updateOrderStatus(id=" & uqID & ",status=" & status & "); " & ex.ToString)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
End Class
