
Partial Class maintenance_productAdd
    Inherits System.Web.UI.Page
    Private Const _countryRow As Integer = 5
    Private Const _currencyRow As Integer = 4

    Protected Sub drpProduct_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpProduct.SelectedValue = "0" Then
            dvProduct.ChangeMode(DetailsViewMode.Insert)
        Else
            dvProduct.ChangeMode(DetailsViewMode.Edit)
        End If
    End Sub
    Protected Sub dvProduct_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        Server.Transfer("productAdd.aspx")
    End Sub
    Protected Sub dvProduct_itemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        Server.Transfer("productAdd.aspx")
    End Sub
    Protected Sub drpCountry_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = dvProduct.Rows(_countryRow).Cells(1).FindControl("drpCountry")
        For Each li As ListItem In drp.Items
            If LCase(li.Value) = "uk" Then li.Attributes.Add("selected", "true")
        Next
    End Sub
    Protected Sub drpCurrency_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = dvProduct.Rows(_currencyRow).Cells(1).FindControl("drpCurrency")
        For Each li As ListItem In drp.Items
            If li.Text = "GBP" Then li.Attributes.Add("selected", "true")
        Next
    End Sub
End Class
