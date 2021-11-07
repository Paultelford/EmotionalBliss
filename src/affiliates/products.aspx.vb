
Partial Class affiliates_products
    Inherits BasePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub drpProd_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.SelectedValue = "0" Then
            dvProd.ChangeMode(DetailsViewMode.Insert)
        Else
            dvProd.ChangeMode(DetailsViewMode.Edit)
        End If
    End Sub
    Protected Sub dvProd_itemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        'Clear the dropdown, and rebind it.
        drpProd.Items.Clear()
        drpProd.Items.Add(New ListItem("Please choose....", "0"))
        drpProd.DataBind()
        dvProd.ChangeMode(DetailsViewMode.Insert)
    End Sub
    Protected Sub dvProd_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        'Clear the dropdown, and rebind it.
        drpProd.Items.Clear()
        drpProd.Items.Add(New ListItem("Please choose....", "0"))
        drpProd.DataBind()
        dvProd.ChangeMode(DetailsViewMode.Insert)
    End Sub
    Protected Sub dvProd_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim dv As DetailsView = CType(sender, DetailsView)
        Dim lbl As Label = dv.FindControl("lblCurrency")
        lbl.Text = UCase(Session("EBAffCurrencyCode"))
    End Sub
End Class
