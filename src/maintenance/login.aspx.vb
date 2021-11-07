
Partial Class maintenance_login
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Membership.Provider.ApplicationName = "ebProvider"
        If Not Page.IsPostBack Then
            Dim txt As TextBox = CType(login1.FindControl("UserName"), TextBox)
            Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('focusLogin(""" & txt.ClientID & """)',200)", True)
        End If
    End Sub

    Protected Sub loginAff_authenticate(ByVal sender As Object, ByVal e As AuthenticateEventArgs)
        'Manually auth the user credentials, then set Session variables
        Membership.Provider.ApplicationName = "ebProvider"
        If Membership.ValidateUser(login1.UserName, login1.Password) Then
            'Empty basket each time affiliate logs in, this ensures they have latest prices
            Profile.EBAffCart.emptyBasket()
            e.Authenticated = True
            Session("EBEditAllCountryText") = True
        Else
            e.Authenticated = False
        End If
    End Sub
End Class
