
Partial Class affiliates_marketing_pc
    Inherits System.Web.UI.Page

    Protected Sub lnkPostcode_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Dim cs As ClientScriptManager = Page.ClientScript
        cs.RegisterStartupScript(Me.GetType(), "loade", "self.setTimeout(""passPostcode('" & getCharData(lnk.CommandArgument) & "')"",200);", True)
    End Sub

    Protected Function getCharData(ByVal s As String) As String
        'Remove numeric characters
        Dim result As String = s
        If Len(s) > 1 Then If IsNumeric(Mid(s, 2, 1)) Then result = Left(s, 1)
        Return result
    End Function
End Class
