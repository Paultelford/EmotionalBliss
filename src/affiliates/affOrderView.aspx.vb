
Partial Class affiliates_affOrderView
    Inherits BasePage

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        Dim drp As DropDownList = Master.FindControl("drpAffmenu")
        drp.SelectedValue = "orders"
    End Sub
    Protected Sub drpStatus_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Deselect the selected index, as the selcted order may no longer be in the gridview
        gvOrders.SelectedIndex = -1
    End Sub
End Class
