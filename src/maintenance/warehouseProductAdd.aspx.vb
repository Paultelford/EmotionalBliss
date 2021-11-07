
Partial Class maintenance_warehouseProductAdd
    Inherits System.Web.UI.Page

    Protected Sub drpBProduct_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpBProducts.SelectedValue = "0" Then
            dvBProduct.ChangeMode(DetailsViewMode.Insert)
        Else
            dvBProduct.ChangeMode(DetailsViewMode.Edit)
        End If
    End Sub
    Protected Sub dvProduct_itemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        drpBProducts.Items.Clear()
        drpBProducts.Items.Add(New ListItem("Select...", "0"))
        drpBProducts.DataBind()
    End Sub
    Protected Sub dvProduct_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        drpBProducts.Items.Clear()
        drpBProducts.Items.Add(New ListItem("Select...", "0"))
        drpBProducts.DataBind()
    End Sub
End Class
