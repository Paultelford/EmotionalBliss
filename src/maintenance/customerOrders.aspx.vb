
Partial Class maintenance_customerOrders
    Inherits System.Web.UI.Page

    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        drpCountry.Items.Clear()
        drpCountry.Items.Add(New ListItem("All", "%"))
    End Sub
End Class
