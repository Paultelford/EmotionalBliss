
Partial Class shop_ideal_accept
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Session("EBTmpIDealStatus") = Request.QueryString("idealstatus")
        Session("EBTmpIDealTx") = Request.QueryString("trxid")
        Server.Transfer("receipt.aspx?auth=ideal")
    End Sub
End Class
