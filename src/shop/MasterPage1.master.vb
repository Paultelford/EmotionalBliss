Partial Class shop_masterPage1
    Inherits System.Web.UI.MasterPage
    'Private drpMainCountry As DropDownList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("EBShopCountry") = "" Then
            Response.Write("Initialising country to UK<br>")
            Session("EBShopCountry") = "uk"
        End If

        bindBasket()
    End Sub

    Protected Sub drpMainCountry_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If Session("EBShopCountry") = "" Then
            For Each li As ListItem In drpMainCountry.items
                If LCase(li.Value) = "uk" Then li.Selected = True
            Next
        Else
            drpMainCountry.selectedValue = Session("EBShopCountry")
        End If
    End Sub
    Protected Sub drpMainCountry_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If basketHasItems Then
            'If user has items in the basket, then they must be redirected to a page informing them that the basket will be emptied, and asked if the wish to continue
            Context.Items("newCountryCode") = drpMainCountry.SelectedValue
            Context.Items("newCountryName") = drpMainCountry.SelectedItem.Text
            Server.Transfer("countryChange.aspx", True)
        Else
            If getPageName() <> "default.aspx" Then
                'Return user to default.aspx when country change occurs
                Session("EBShopCountry") = drpMainCountry.SelectedValue
                Server.Transfer("default.aspx")
            Else
                'User is on default.aspx, and basket is empty. Allow country change
                Session("EBShopCountry") = drpMainCountry.SelectedValue
            End If
        End If
    End Sub
    Protected Function getPageName() As String
        Dim sResult As String
        Dim arr As Array = Split(Request.ServerVariables("HTTP_REFERER"), "/")
        sResult = LCase(arr(UBound(arr)))
        Return sResult
    End Function
    Protected Function basketHasItems() As Boolean
        Return CType(Profile.EBCart.Items.Count > 0, Boolean)
    End Function
    Public Sub bindBasket()
        'Dim gvBasket As GridView = Master.FindControl("gvBasket")
        If Not Profile.EBCart Is Nothing Then
            gvBasket.DataSource = Profile.EBCart.Items
            gvBasket.DataBind()
            lblBasketTotal.Text = Profile.EBCart.TotalEx.ToString
            'lblTotal.Font.Size = 8
        End If
    End Sub
End Class
