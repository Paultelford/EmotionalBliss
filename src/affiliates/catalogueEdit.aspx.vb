
Partial Class affiliates_catalogueEdit
    Inherits BasePage

    Protected Sub dvAddress_itemCommand(ByVal sender As Object, ByVal e As DetailsViewCommandEventArgs)
        If LCase(e.CommandName) = "cancel" Then Server.Transfer("cataloguePrintPop.aspx")
    End Sub
    Protected Sub dvAddress_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        Server.Transfer("cataloguePrintPop.aspx")
    End Sub
End Class
