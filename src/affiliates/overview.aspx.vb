
Partial Class affiliates_overview
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Membership.Provider.ApplicationName = "ebAffProvider"
        lblAffID.Text = "<font color='white'>" & Session("EBAffID") & "</font>"
        lblCountryCode.Text = "<font color='white'>" & Session("EBAffEBDistributorCountryCode") & "</font>"
        If Membership.GetUser Is Nothing Then
            lblText.Text = "<font color='white'>It is nothing</font>"
        Else
            lblText.Text = "<font color='white'>Obj found</font>"
        End If

    End Sub
End Class
