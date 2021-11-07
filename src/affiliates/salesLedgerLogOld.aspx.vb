
Partial Class affiliates_salesLedgerLogOld
    Inherits BasePage
    Private Const _gvSalesLedger_chargedPos As Integer = 2
    Private Const _gvSalesLedger_creditPos As Integer = 4
    Private Const _gvSalesLedger_chequePos As Integer = 6
    Private Const _gvSalesLedger_chequeSpacerPos As Integer = 7
    Private Const _gvSalesLedger_affiliatePos As Integer = 8
    Private Const _gvSalesLedger_affiliateSpacerPos As Integer = 9
    Private Const _gvSalesLedger_vatPos As Integer = 10
    Private Const _gvSalesLedger_totalPos As Integer = 12
    Private Const _gvSalesLedgerDay_goodsPos As Integer = 2
    Private Const _gvSalesLedgerDay_vatPos As Integer = 4


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        lnkShowComplete.NavigateUrl = "salesLedgerpop.aspx?startdate=" & date1.getStartDate & "&enddate=" & date1.getEndDate & "&type=" & drpType.SelectedValue
    End Sub
    Protected Sub gvSalesLedger_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        panLog.Visible = False
        panDetails.Visible = True
        lblDay.Text = "Sales ledger for " & FormatDateTime(gvSalesLedger.DataKeys(gvSalesLedger.SelectedIndex).Value, DateFormat.LongDate)
    End Sub
    Protected Function showDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d.ToString, DateFormat.LongDate)
        Return result
    End Function
    Protected Sub lnkBack_click(ByVal sender As Object, ByVal e As EventArgs)
        panDetails.Visible = False
        panLog.Visible = True
    End Sub
    Protected Sub gvSalesLedger_databound(ByVal sender As Object, ByVal e As EventArgs)
        Dim iCharged As Integer = 0
        Dim dCredit As Decimal = 0
        Dim dCheque As Decimal = 0
        Dim dAffiliate As Decimal = 0
        Dim dVat As Decimal = 0
        Dim dTotal As Decimal = 0
        Dim lblTotal As Label
        Dim bShowFooter As Boolean = False
        Dim lblActionID As Label
        For Each row As GridViewRow In gvSalesLedger.Rows
            If row.RowType = DataControlRowType.DataRow Then
                bShowFooter = True
                lblActionID = row.FindControl("lblActionID")
                lblTotal = row.FindControl("lblTotalIncVat")
                'Make font red if row is a refund row
                If lblActionID.Text = "2" Or lblActionID.Text = "3" Then
                    For Each cell As Object In row.Cells
                        cell.forecolor = Drawing.Color.Red
                        lblTotal.ForeColor = Drawing.Color.Red
                    Next
                End If
                Try
                    iCharged = iCharged + row.Cells(_gvSalesLedger_chargedPos).Text
                    dCredit = dCredit + row.Cells(_gvSalesLedger_creditPos).Text
                    If drpType.SelectedValue = "%" Then dCheque = dCheque + row.Cells(_gvSalesLedger_chequePos).Text
                    If drpType.SelectedValue = "%" Then dAffiliate = dAffiliate + row.Cells(_gvSalesLedger_affiliatePos).Text
                    'lblError.Text = lblError.Text & row.Cells(_gvSalesLedger_vatPos).Text & ","
                    dVat = dVat + row.Cells(_gvSalesLedger_vatPos).Text
                    dTotal = dTotal + lblTotal.Text
                Catch ex As Exception
                    lblError.Text = lblError.Text & ex.ToString
                End Try
            End If
        Next

        If bShowFooter Then
            gvSalesLedger.FooterRow.Cells(_gvSalesLedger_chargedPos).Text = iCharged
            gvSalesLedger.FooterRow.Cells(_gvSalesLedger_creditPos).Text = FormatNumber(dCredit, 2)
            If drpType.SelectedValue = "%" Then gvSalesLedger.FooterRow.Cells(_gvSalesLedger_chequePos).Text = FormatNumber(dCheque, 2)
            If drpType.SelectedValue = "%" Then gvSalesLedger.FooterRow.Cells(_gvSalesLedger_affiliatePos).Text = FormatNumber(dAffiliate, 2)
            gvSalesLedger.FooterRow.Cells(_gvSalesLedger_vatPos).Text = FormatNumber(dVat, 2)
            gvSalesLedger.FooterRow.Cells(_gvSalesLedger_totalPos).Text = FormatNumber(dTotal, 2)
        End If
    End Sub
    Protected Sub gvSalesLedgerDay_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim dGoods As Decimal = 0
        Dim dVat As Decimal = 0
        Dim bShowFooter As Boolean = False
        For Each row As GridViewRow In gvSalesLedgerDay.Rows
            If row.RowType = DataControlRowType.DataRow Then
                bShowFooter = True
                dGoods = dGoods + row.Cells(_gvSalesLedgerDay_goodsPos).Text
                dVat = dVat + row.Cells(_gvSalesLedgerDay_vatPos).Text
            End If
        Next
        If bShowFooter Then
            gvSalesLedgerDay.FooterRow.Cells(_gvSalesLedgerDay_goodsPos).Text = FormatNumber(dGoods, 2)
            gvSalesLedgerDay.FooterRow.Cells(_gvSalesLedgerDay_vatPos).Text = FormatNumber(dVat, 2)
        End If
    End Sub
    Protected Sub sqlSalesLedgerDay_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        'This will load the 'actionID' parameter with a 1 (Sale) or 2 (Refund), so that only the correct items are returned for the selcted day

        Dim lblActionID As Label = gvSalesLedger.SelectedRow.FindControl("lblActionID")
        'lblError.Text = lblActionID.Text & " " & Now().TimeOfDay.ToString
        e.Command.Parameters("@actionID").Value = lblActionID.Text
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'If '%'(All) is selected, then show the extra columns in the SalesLedger Gridview
        If drpType.SelectedValue = "%" Then
            gvSalesLedger.Columns(_gvSalesLedger_chequePos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_chequeSpacerPos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_affiliatePos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_affiliateSpacerPos).Visible = True
        Else
            gvSalesLedger.Columns(_gvSalesLedger_chequePos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_chequeSpacerPos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_affiliatePos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_affiliateSpacerPos).Visible = False
        End If
    End Sub

End Class

