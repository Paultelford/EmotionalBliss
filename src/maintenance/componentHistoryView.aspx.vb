

Partial Class maintenance_componentHistoryView

    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Select Case Request.QueryString("type")
            Case "component"
                Response.Redirect("componentHistoryViewComponent.aspx?id=" & Request.QueryString("cid"))
            Case "batch"
                Response.Redirect("componentHistoryViewBatch.aspx?id=" & Request.QueryString("cid"))
            Case "order"
                Response.Redirect("componentHistoryViewOrder.aspx?id=" & Request.QueryString("cid"))
        End Select
    End Sub


End Class