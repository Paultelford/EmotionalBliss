Imports System.Data

Partial Class affiliates_execSummary
    Inherits BasePage
    Private Const _gvSummary_orderTypePos As Integer = 0
    Private Const _gvSummary_ECPos As Integer = 2
    Private Const _gvSummary_ECAmountPos As Integer = 3
    Private Const _gvSummary_spacer1Pos As Integer = 4
    Private Const _gvSummary_NECPos As Integer = 5
    Private Const _gvSummary_NECAmountPos As Integer = 6
    Private Const _gvSummary_spacer2Pos As Integer = 7
    Private Const _gvSummary_otherPos As Integer = 8
    Private Const _gvSummary_otherAmountPos As Integer = 9
    Private Const _gvSummary_spacer3Pos As Integer = 10
    Private Const _gvSummary_regionalVatPos As Integer = 11
    Private Const _gvSummary_countryPos As Integer = 12
    Private Const _gvSummary_countryAmountPos As Integer = 13
    Private Const _gvSummary_countryVatPos As Integer = 14
    Private Const _gvSummary_totalSalesPos As Integer = 16
    Private Const _gvSummary_totalVatPos As Integer = 17
    Private Const _gvSummary_totalPos As Integer = 18

    Private _orderTypes As New Hashtable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        If LCase(Session("EBAffEBDistributorCountryCode")) = "gb" Then drpCountryOverview.Visible = True
        'Setup hashtable
        _orderTypes.Add("201", "Web")
        _orderTypes.Add("202", "Web Refund")
        _orderTypes.Add("251", "Post")
        _orderTypes.Add("252", "Post Refund")
        _orderTypes.Add("301", "Phone")
        _orderTypes.Add("302", "Phone Refund")
        _orderTypes.Add("401", "Affiliate Bulk")
        _orderTypes.Add("402", "Affiliate Refunds")
    End Sub
    Protected Sub drpCountryOverview_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpCountryOverview.SelectedValue = "%" Then
            gvSummary.Columns(_gvSummary_ECPos).Visible = True
            gvSummary.Columns(_gvSummary_ECAmountPos).Visible = True
            gvSummary.Columns(_gvSummary_NECPos).Visible = True
            gvSummary.Columns(_gvSummary_NECAmountPos).Visible = True
            gvSummary.Columns(_gvSummary_ECPos).Visible = True
            gvSummary.Columns(_gvSummary_ECAmountPos).Visible = True
            gvSummary.Columns(_gvSummary_otherPos).Visible = True
            gvSummary.Columns(_gvSummary_otherAmountPos).Visible = True
            gvSummary.Columns(_gvSummary_spacer1Pos).Visible = True
            gvSummary.Columns(_gvSummary_spacer2Pos).Visible = True
            gvSummary.Columns(_gvSummary_spacer3Pos).Visible = True
        Else
            gvSummary.Columns(_gvSummary_ECPos).Visible = False
            gvSummary.Columns(_gvSummary_ECAmountPos).Visible = False
            gvSummary.Columns(_gvSummary_NECPos).Visible = False
            gvSummary.Columns(_gvSummary_NECAmountPos).Visible = False
            gvSummary.Columns(_gvSummary_ECPos).Visible = False
            gvSummary.Columns(_gvSummary_ECAmountPos).Visible = False
            gvSummary.Columns(_gvSummary_otherPos).Visible = False
            gvSummary.Columns(_gvSummary_otherAmountPos).Visible = False
            gvSummary.Columns(_gvSummary_spacer1Pos).Visible = False
            gvSummary.Columns(_gvSummary_spacer2Pos).Visible = False
            gvSummary.Columns(_gvSummary_spacer3Pos).Visible = False
        End If
    End Sub
    Protected Sub gvSummary_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblType As Label
        Dim bSingleCountryOnly As Boolean = True
        Dim lblRegionalVat As Label
        Dim lblLocalVat As Label
        Dim lblActionID As Label
        'Total sales
        Dim ecSales As Integer = 0
        Dim necSales As Integer = 0
        Dim otherSales As Integer = 0
        Dim localSales As Integer = 0
        'Total vat
        Dim ecVat As Decimal = 0
        Dim necVat As Decimal = 0
        Dim otherVat As Decimal = 0
        Dim localVat As Decimal = 0
        'Total amount
        Dim ec As Decimal = 0
        Dim nec As Decimal = 0
        Dim other As Decimal = 0
        Dim vat As Decimal = 0
        Dim local As Decimal = 0
        'Footer totals
        Dim ecSalesTot As Integer = 0
        Dim ecAmountTot As Decimal = 0
        Dim necSalesTot As Integer = 0
        Dim necAmountTot As Decimal = 0
        Dim otherSalesTot As Integer = 0
        Dim otherAmountTot As Decimal = 0
        Dim localSalesTot As Integer = 0
        Dim localAmountTot As Decimal = 0
        Dim totalSaleTot As Integer = 0
        Dim totalVatTot As Decimal = 0
        Dim totalTotalTot As Decimal = 0
        Dim oddRow As Boolean = True
        If drpCountryOverview.SelectedValue = "%" Then bSingleCountryOnly = False
        For Each row As GridViewRow In gvSummary.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblRegionalVat = row.FindControl("lblRegionalVat")
                lblLocalVat = row.FindControl("lblLocalVat")
                'Show order type
                lblType = row.FindControl("lblType")
                lblType.Text = _orderTypes(lblType.Text)
                'Show total sales
                If gvSummary.Columns(_gvSummary_ECPos).Visible Then ecSales = CInt(row.Cells(_gvSummary_ECPos).Text)
                If gvSummary.Columns(_gvSummary_NECPos).Visible Then ecSales = CInt(row.Cells(_gvSummary_NECPos).Text)
                If gvSummary.Columns(_gvSummary_otherPos).Visible Then ecSales = CInt(row.Cells(_gvSummary_otherPos).Text)
                localSales = CInt(row.Cells(_gvSummary_countryPos).Text)
                row.Cells(_gvSummary_totalSalesPos).Text = CStr(ecSales + necSales + otherSales + localSales)
                'Show total vat
                localVat = CDec(lblLocalVat.Text)
                vat = CDec(lblRegionalVat.Text)
                If drpCountryOverview.SelectedValue = "%" Then
                    row.Cells(_gvSummary_totalVatPos).Text = FormatNumber(localVat + vat, 2)
                Else
                    row.Cells(_gvSummary_totalVatPos).Text = FormatNumber(localVat, 2)
                    vat = 0
                End If
                'Show total amount
                If gvSummary.Columns(_gvSummary_ECAmountPos).Visible Then ec = CDec(row.Cells(_gvSummary_ECAmountPos).Text)
                If gvSummary.Columns(_gvSummary_NECAmountPos).Visible Then nec = CDec(row.Cells(_gvSummary_NECAmountPos).Text)
                If gvSummary.Columns(_gvSummary_otherAmountPos).Visible Then other = CDec(row.Cells(_gvSummary_otherAmountPos).Text)
                If gvSummary.Columns(_gvSummary_regionalVatPos).Visible Then vat = CDec(lblRegionalVat.Text)
                local = CDec(row.Cells(_gvSummary_countryAmountPos).Text)
                localVat = CDec(lblLocalVat.Text)
                row.Cells(_gvSummary_totalPos).Text = FormatNumber(CStr(ec + nec + other + local + vat + localVat), 2)
                'Highlight refunds in red
                lblActionID = row.FindControl("lblActionID")
                If lblActionID.Text = "2" Then row.ForeColor = Drawing.Color.Red
                'Apply alternative bg color to total columns
                If Not oddRow Then row.Cells(_gvSummary_totalSalesPos).BackColor = System.Drawing.ColorTranslator.FromHtml("#dddddd")
                If Not oddRow Then row.Cells(_gvSummary_totalVatPos).BackColor = System.Drawing.ColorTranslator.FromHtml("#dddddd")
                If Not oddRow Then row.Cells(_gvSummary_totalPos).BackColor = System.Drawing.ColorTranslator.FromHtml("#dddddd")
                oddRow = Not oddRow
                'Add to column totals
                If gvSummary.Columns(_gvSummary_ECPos).Visible Then ecSalesTot = ecSalesTot + CInt(row.Cells(_gvSummary_ECPos).Text)
                If gvSummary.Columns(_gvSummary_ECAmountPos).Visible Then ecAmountTot = ecAmountTot + CDec(row.Cells(_gvSummary_ECAmountPos).Text)
                If gvSummary.Columns(_gvSummary_NECPos).Visible Then necSalesTot = necSalesTot + CInt(row.Cells(_gvSummary_NECPos).Text)
                If gvSummary.Columns(_gvSummary_NECAmountPos).Visible Then necAmountTot = necAmountTot + CDec(row.Cells(_gvSummary_NECAmountPos).Text)
                If gvSummary.Columns(_gvSummary_otherPos).Visible Then otherSalesTot = otherSalesTot + CInt(row.Cells(_gvSummary_otherPos).Text)
                If gvSummary.Columns(_gvSummary_otherAmountPos).Visible Then otherAmountTot = otherAmountTot + CDec(row.Cells(_gvSummary_otherAmountPos).Text)
                localSalesTot = localSalesTot + CInt(row.Cells(_gvSummary_countryPos).Text)
                localAmountTot = localAmountTot + CDec(row.Cells(_gvSummary_countryAmountPos).Text)
                totalSaleTot = totalSaleTot + CInt(row.Cells(_gvSummary_totalSalesPos).Text)
                totalVatTot = totalVatTot + CDec(row.Cells(_gvSummary_totalVatPos).Text)
                totalTotalTot = totalTotalTot + CDec(row.Cells(_gvSummary_totalPos).Text)
            End If
        Next
        If gvSummary.Rows.Count > 0 Then
            'Show country code in gridview header
            gvSummary.HeaderRow.Cells(_gvSummary_countryPos).Text = UCase(Session("EBAffEBDistributorCountryCode")) & " Sales"
            'Show currency headers
            gvSummary.HeaderRow.Cells(_gvSummary_ECAmountPos).Text = Session("EBAffCurrencySign")
            gvSummary.HeaderRow.Cells(_gvSummary_NECAmountPos).Text = Session("EBAffCurrencySign")
            gvSummary.HeaderRow.Cells(_gvSummary_otherAmountPos).Text = Session("EBAffCurrencySign")
            gvSummary.HeaderRow.Cells(_gvSummary_countryAmountPos).Text = Session("EBAffCurrencySign")
            'Show Toatls
            gvSummary.FooterRow.Cells(_gvSummary_ECPos).Text = CStr(ecSalesTot)
            gvSummary.FooterRow.Cells(_gvSummary_ECAmountPos).Text = CStr(ecAmountTot)
            gvSummary.FooterRow.Cells(_gvSummary_NECPos).Text = CStr(necSalesTot)
            gvSummary.FooterRow.Cells(_gvSummary_NECAmountPos).Text = CStr(necAmountTot)
            gvSummary.FooterRow.Cells(_gvSummary_otherPos).Text = CStr(otherSalesTot)
            gvSummary.FooterRow.Cells(_gvSummary_otherAmountPos).Text = CStr(otherAmountTot)
            gvSummary.FooterRow.Cells(_gvSummary_countryPos).Text = CStr(localSalesTot)
            gvSummary.FooterRow.Cells(_gvSummary_countryAmountPos).Text = CStr(localAmountTot)
            gvSummary.FooterRow.Cells(_gvSummary_totalSalesPos).Text = CStr(totalSaleTot)
            gvSummary.FooterRow.Cells(_gvSummary_totalVatPos).Text = CStr(totalVatTot)
            gvSummary.FooterRow.Cells(_gvSummary_totalPos).Text = CStr(totalTotalTot)
        End If
    End Sub
End Class
