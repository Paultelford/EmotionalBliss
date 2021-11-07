Imports System.Data

Partial Class affiliates_salesLedgerPopup
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim sp As String = "procSalesLedgerByDateSelect22"
        If CBool(Request.QueryString("bypayment")) Then
            sp = "procShopOrderByPaymentDateSelect2"
        End If
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@countryCode", "@date", "@type", "@currency", "@actionID"}
            Dim paramValue() As String = {Request.QueryString("countryCode"), Request.QueryString("day"), Request.QueryString("type"), Request.QueryString("currency"), Request.QueryString("actionID")}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int}
            Dim paramSize() As Integer = {5, 0, 10, 5, 0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, sp)
            gv.DataSource = dt
            gv.DataBind()
        Catch ex As Exception
            siteInclude.addError("", "(); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
    End Sub

   
End Class
