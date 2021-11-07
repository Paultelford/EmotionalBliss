
Partial Class maintenance_manufacturerAdd
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub drpMan_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If dvManufacturer.SelectedValue = "0" Then
            dvManufacturer.ChangeMode(DetailsViewMode.Insert)
        Else
            dvManufacturer.ChangeMode(DetailsViewMode.Edit)
        End If
    End Sub
    Protected Sub dvManufacturer_itemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        'Clear textboxes and rebind dropdown
        drpMan.Items.Clear()
        drpMan.Items.Add(New ListItem("Please choose....", "0"))
        drpMan.DataBind()
        drpMan.SelectedIndex = 0
    End Sub
    Protected Sub dvManufacurer_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        'Clear textboxes and rebind dropdown
        drpMan.Items.Clear()
        drpMan.Items.Add(New ListItem("Please choose....", "0"))
        drpMan.DataBind()
        drpMan.SelectedIndex = 0
    End Sub
End Class
