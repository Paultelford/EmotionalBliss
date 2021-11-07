
Partial Class affiliates_royaltyOrder
    Inherits BasePage

    Protected Sub fvAffiliate_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Check that affiliate is looking at their own order (stop em fiddling with the orderid querystring)
        If CBool(Session("EBAffEBDistributor")) Then
            'User is Distributor, just check on country code
            Dim lblCountryCode As Label = fvAffiliate.FindControl("lblCountryCode")
            'If LCase(lblCountryCode.Text) <> LCase(Session("EBAffEbDistributorCountryCode")) Then showAccessDenied()
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
    Protected Sub gvItems_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Calculate total royalty for order
        Dim total As Decimal = 0
        Dim lbl As Label
        For Each row As GridViewRow In gvItems.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lbl = row.FindControl("lblRoyalty")
                total = total + Replace(lbl.Text, Session("EBAffCurrencySign"), "")
            End If
        Next
        lbl = gvItems.FooterRow.FindControl("lbltotal")
        lbl.Text = Session("EBAffCurrencySign") & FormatNumber(total, 2)
    End Sub
    Protected Sub showAccessDenied()
        fvAffiliate.Visible = False
        gvItems.Visible = False
        'fvTotals.Visible = False
        lblError.Text = "<font color='red'>Unable to retrieve details for this order.</font>"
    End Sub
End Class
