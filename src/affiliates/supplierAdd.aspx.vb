
Partial Class maintenance_supplierAdd
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub drpMan_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpMan.SelectedValue = "0" Then
            'The following line is causing the DV to disappear for some reason, so use response.redirect until a fix can be found
            dvSupplier.ChangeMode(DetailsViewMode.Insert)
            'Response.Redirect("suppliersAdd.aspx")
        Else
            dvSupplier.ChangeMode(DetailsViewMode.Edit)
        End If
    End Sub
    Protected Sub dvSupplier_itemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        'Clear drpMan before rebinding, as it will get appended.
        drpMan.Items.Clear()
        drpMan.Items.Add(New ListItem("Please choose....", "0"))
        drpMan.DataBind()
        dvSupplier.ChangeMode(DetailsViewMode.Insert)
    End Sub
    Protected Sub dvSupplier_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        'Clear drpMan before rebinding, as it will get appended.
        drpMan.Items.Clear()
        drpMan.Items.Add(New ListItem("Please choose....", "0"))
        drpMan.DataBind()
        dvSupplier.ChangeMode(DetailsViewMode.Insert)
    End Sub
    Protected Sub drpCurrency_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        For Each li As ListItem In drp.Items
            li.Text = UCase(li.Text)
        Next
    End Sub
End Class
