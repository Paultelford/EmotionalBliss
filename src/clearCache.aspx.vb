
Partial Class clearCache
    Inherits System.Web.UI.Page

    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Cache.Remove(drpCacheName.SelectedValue & uCase(txtCountryCode.text))
        lblComplete.text = "Cache '" & drpCacheName.SelectedValue & uCase(txtCountryCode.text) & "' has been cleared."
    End Sub
End Class
