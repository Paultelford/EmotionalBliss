Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_statementInvoice
    Inherits BasePage
    Private _currency As String = "£"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        
    End Sub
    Protected Function showOrderCode(ByVal nID As Object, ByVal cc As Object) As String
        Dim result As String = ""
        If Not IsDBNull(nID) Then result = nID.ToString
        If Not IsDBNull(cc) Then result = result & UCase(cc.ToString)
        Return result
    End Function
    Protected Function showName(ByVal f As Object, ByVal s As Object) As String
        Dim first As String = ""
        Dim second As String = ""
        If Not IsDBNull(f) Then first = formatText(f.ToString)
        If Not IsDBNull(s) Then second = formatText(s.ToString)
        Return first & " " & second
    End Function
    Protected Function calcVat(ByVal goodsVat As Decimal, ByVal shipping As Decimal, ByVal shippingTotal As Decimal) As String
        Return FormatNumber(goodsVat + (shippingTotal - shipping), 2)
    End Function
    Protected Sub fvAffiliate_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Check that affiliate is looking at their own order (stop em fiddling with the orderid querystring)
        Dim hidAffiliateID As HiddenField = fvAffiliate.FindControl("hidAffiliateID")
        If CBool(Session("EBAffEBDistributor")) Then
            'User is Distributor, just check on country code
            Dim lblCountryCode As Label = fvAffiliate.FindControl("lblCountryCode")
            If LCase(lblCountryCode.Text) <> LCase(Session("EBAffEbDistributorCountryCode")) And hidAffiliateID.Value <> Session("EBAffID") Then showAccessDenied()
        Else
            'User is affiliate, check affID matches
            Try
                Dim lblAffID As Label = fvAffiliate.FindControl("lblAffID")
                If lblAffID.Text <> Session("EBAffID") Then showAccessDenied()
            Catch ex As Exception
                'Order must not be an affiliate order, and has thrown an exception, show access deinied error
                showAccessDenied()
            End Try
        End If
        'Add <br>'s to address lines in the formview
        Dim lbl As Label
        For iLoop As Integer = 1 To 5
            lbl = fvAffiliate.FindControl("lblAdd" & iLoop)
            If lbl.Text <> "" Then lbl.Text = lbl.Text & "<br>"
        Next
    End Sub
    Protected Sub showAccessDenied()
        fvAffiliate.Visible = False
        gvOrderItems.Visible = False
        fvTotals.Visible = False
        lblError.Text = "<font color='red'>Unable to retrieve details for this order.</font>"
    End Sub
    Protected Function formatText(ByVal text As String) As String
        'Formats the word to start with a capital letter and the rest lowercase
        Dim result As String = ""
        If Len(text) > 1 Then
            result = UCase(Left(text, 1))
            result = result & LCase(Right(text, Len(text) - 1))
        Else
            result = UCase(text)
        End If
        Return result
    End Function
    Protected Function showCurrency() As String
        Return _currency
    End Function
End Class
