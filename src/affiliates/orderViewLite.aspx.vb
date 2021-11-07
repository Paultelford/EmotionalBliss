
Partial Class affiliates_orderViewLite
    Inherits BasePage
    Private _dvCustomer_orderTypePos As Integer = 9

    Protected Sub dvCustomer_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim si As New siteInclude
        dvCustomer.Rows(_dvCustomer_orderTypePos).Cells(1).Text = si.getOrderType(dvCustomer.Rows(_dvCustomer_orderTypePos).Cells(1).Text)
        si = Nothing
    End Sub
    Protected Sub gvItems_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim dPriceTot As Decimal = 0
        Dim dRoyaltyTot As Decimal = 0
        Dim lbl As Label
        Dim currencySign As String = ""
        Dim bCurrencyFound As Boolean = False
        For Each row As GridViewRow In gvOrderItems.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lbl = row.FindControl("lblPrice")
                dPriceTot = dPriceTot + CDec(lbl.Text)
                lbl = row.FindControl("lblRoyalty")
                dRoyaltyTot = dRoyaltyTot + CDec(lbl.Text)
                If Not bCurrencyFound Then
                    lbl = row.FindControl("lblCurrencySign")
                    currencySign = lbl.Text
                    bCurrencyFound = True
                End If
            End If
        Next
        If dPriceTot > 0 Then
            lbl = gvOrderItems.FooterRow.FindControl("lblPriceTotal")
            lbl.Text = FormatNumber(dPriceTot, 2)
            lbl = gvOrderItems.FooterRow.FindControl("lblCurrencySign1")
            lbl.Text = currencySign
        End If
        If dRoyaltyTot > 0 Then
            lbl = gvOrderItems.FooterRow.FindControl("lblRoyaltyTotal")
            lbl.Text = FormatNumber(dRoyaltyTot, 2)
            lbl = gvOrderItems.FooterRow.FindControl("lblCurrencySign3")
            lbl.Text = currencySign
        End If
    End Sub
End Class
