Imports System.Data

Partial Class fp
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim POST_BACK_PASS As String = "test"
        Dim VALID_IP As String = "78.129.201.24"
        Dim orderID As String = ""
        Dim TransactionID As String = ""
        Dim PaymentStatus As String = ""
        Dim PaymentAmount As String = ""
        Dim NewID As String = ""
       
        If (Request("PBPW") = POST_BACK_PASS) Then
            If (Request("NEWID") <> "") Then
                ' NEW TRANSACTION ID
                ' Get Original ID and Transaction ID
                orderID = Request("FPIDENTIFY")
                TransactionID = Request("TRXID")
                NewID = Request("NEWID")
                'Store the transaction details and update the order status to 'Deferred'
                storeResponse(orderID, TransactionID, NewID)
                ' Give "ok" to confirm ID is received
                Response.Redirect("/ok.asp")
            Else
                ' NEW COMPLETED TRANSACTION
                ' Get all Variables
                orderID = Request("FPIDENTIFY")
                TransactionID = Request("TRXID")
                PaymentStatus = Request("FPSTATUS")
                PaymentAmount = Request("FPPRICE")

                ' Check Status (Just in case)

                If (PaymentStatus = "P") Then
                    ' Fully paid transaction
                    ' Update the payment entry in the fastpay table
                    updateResponse(orderID, TransactionID, PaymentStatus, PaymentAmount)
                    'Send email to PT to notify of payment
                    siteInclude.sendEmail("scott@emotionalbliss.com", "", "", "Fastpay payment complete", "noreply@emotionalbliss.co.uk", "Fastpay payments", "Payment of " & PaymentAmount & " made for order " & orderID)
                    ' Give "ok"
                    Response.Redirect("/ok.asp")
                Else
                    ' Capture any other status (not used), give "ok" once done
                    siteInclude.addError("fp.aspx.vb", "Unknown status returned (" & PaymentStatus & "); " & Request.ServerVariables("QUERY_STRING"))
                    Response.Redirect("/ok.asp")
                End If
            End If
        Else
            siteInclude.addError("fp.aspx.vb", "Incorrect Password being used")
            Response.Write("ERROR: Incorrect Post Back Password")
        End If
    End Sub
    Protected Sub storeResponse(ByVal orderID As String, ByVal transID As String, ByVal NewID As String)
        Try
            Dim param() As String = {"@orderid", "@trxID", "@newID"}
            Dim paramValue() As String = {orderID, transID, NewID}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Int}
            Dim paramSize() As Integer = {0, 255, 0}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procFastPayInsert")
        Catch ex As Exception
            siteInclude.addError("fp.aspx.vb", "storeResponse(orderID=" & orderID & ", transID=" & transID & ", newID=" & NewID & "); " & ex.ToString)
        Finally
        End Try
    End Sub
    Protected Sub updateResponse(ByVal orderID As String, ByVal transID As String, ByVal status As String, ByVal amount As String)
        Try
            Dim param() As String = {"@orderid", "@trxID", "@status", "@amount"}
            Dim paramValue() As String = {orderID, transID, status, amount}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Decimal}
            Dim paramSize() As Integer = {0, 255, 1, 0}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procFastPayByTrxIDUpdate")
        Catch ex As Exception
            siteInclude.addError("fp.aspx.vb", "storeResponse(orderID=" & orderID & ", transID=" & transID & ", status=" & status & ", amount=" & amount & "); " & ex.ToString)
        Finally
        End Try
    End Sub
End Class
