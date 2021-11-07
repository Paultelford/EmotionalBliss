
Partial Class affiliates_warranty
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Membership.Provider.ApplicationName = "ebAffProvider"
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then
            Response.Redirect("default.aspx")
        End If
    End Sub

End Class
