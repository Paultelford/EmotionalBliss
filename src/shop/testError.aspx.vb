
Partial Class shop_testError
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Response.Write(Context.Items("source") & "<br>")
        Response.Write(Context.Items("errorcode") & "<br>")
        Response.Write(Context.Items("paymenttype") & "<br>")
    End Sub
End Class
