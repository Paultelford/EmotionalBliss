
Partial Class maintenance_productAssemblyCompletePrint
    Inherits System.Web.UI.Page
    Private Const _qtyPos As Integer = 2
    Private fv As FormView

    Protected Function showDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        Return result
    End Function
    Protected Sub gvComponents_dataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim lblFailed As Label
        Dim lblQty As Label
        fv = fvComplete
        If e.Row.RowType = DataControlRowType.DataRow Then
            lblQty = fv.FindControl("lblQty")
            lblFailed = fv.FindControl("lblQtyFailed")
            e.Row.Cells(_qtyPos).Text = (Convert.ToInt32(e.Row.Cells(_qtyPos).Text) / Convert.ToInt32(lblQty.Text)) * Convert.ToInt32(lblFailed.Text)
        End If
    End Sub
End Class
