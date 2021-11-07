
Partial Class maintenance_orderList
    Inherits System.Web.UI.Page
    Private drpMainCountry As DropDownList
    Private _login As LoginView
    Private Const _gvOrders_typePos As Integer = 4

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            drpMainCountry = _login.FindControl("drpMainCountry")
            drpMainCountry.Visible = True
            If Page.IsPostBack Then
                gvOrders.DataBind()
            Else
                If Session("EBMaintOrderList_drpOrderStatus") <> "" Then drpOrderStatus.SelectedValue = Session("EBMaintOrderList_drpOrderStatus")
                If Session("EBDistOrderType_drpOrderPrefix") <> "" Then drpOrderType.SelectedValue = Session("EBDistOrderType_drpOrderPrefix")
            End If
        End If
    End Sub
    Protected Sub drpOrderStatus_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Session("EBMaintOrderList_drpOrderStatus") = drpOrderStatus.SelectedValue
    End Sub
    Protected Sub btnShowAll_click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub SqlOrders_onSelecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        e.Command.Parameters(0).Value = drpMainCountry.SelectedValue
    End Sub
    Protected Sub drpOrderPrefix_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Session("EBDistOrderType_drpOrderPrefix") = drpOrderType.SelectedValue
    End Sub
    Protected Sub gvOrders_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As GridViewRow In gvOrders.Rows
            If row.RowType = DataControlRowType.DataRow Then
                Select Case LCase(row.Cells(_gvOrders_typePos).Text)
                    Case "distaccount"
                        row.Cells(_gvOrders_typePos).Text = "Account"
                    Case "distcc"
                        row.Cells(_gvOrders_typePos).Text = "cc"
                End Select
            End If
        Next
        'Show page position
        lblIndexLo.Text = gvOrders.PageIndex + 1
        lblIndexHi.Text = gvOrders.PageCount
    End Sub
End Class
