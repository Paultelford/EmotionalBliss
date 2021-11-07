Imports System.Data

Partial Class affiliates_salesLedgerAccounts
    Inherits BasePage
    Private Const _gvSalesLedger_chargedPos As Integer = 2
    Private Const _gvSalesLedger_creditPos As Integer = 4
    Private Const _gvSalesLedger_chequePos As Integer = 6
    Private Const _gvSalesLedger_chequeSpacerPos As Integer = 7
    Private Const _gvSalesLedger_affiliatePos As Integer = 8
    Private Const _gvSalesLedger_affiliateSpacerPos As Integer = 9
    Private Const _gvSalesLedger_affiliateAccPos As Integer = 10
    Private Const _gvSalesLedger_affiliateAccSpacerPos As Integer = 11
    Private Const _gvSalesLedger_vatPos As Integer = 12
    Private Const _gvSalesLedger_totalPos As Integer = 14
    Private Const _gvSalesLedgerDay_goodsPos As Integer = 2
    Private Const _gvSalesLedgerDay_vatPos As Integer = 4
    Private Const _gvSalesLedgerDay_affiliateSpacerPos As Integer = 7
    Private _transType As String()
    Private _currencySign As String()
    Private _groupType As String()
    Private gvType As String = ""
    Private Enum _CurrencyType
        Pound = 0
        Euro = 1
        USDollar = 2
        [size] = 2
    End Enum
    Private Enum _ChargeType
        Paypal = 0
        Amex = 1
        CC = 2
        Icepay = 3
        Accounts = 4
        [size] = 4
    End Enum

    'System
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            lblCountryCode.Text = UCase(Session("EBAffEBDistributorCountryCode"))
            lblCurrency.Text = UCase(Session("EBAffCurrencyCode"))
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        lnkShowComplete.NavigateUrl = "salesLedgerpop.aspx?startdate=" & date1.getStartDate & "&enddate=" & date1.getEndDate & "&type=" & drpType.SelectedValue
    End Sub

    'Page
    Protected Sub drpType_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        drpType.Items.Clear()
        drpType.Items.Add(New ListItem("Select...", ""))
        drpType.Items.Add(New ListItem("All", "%"))
        drpType.Items.Add(New ListItem("AMEX", "amex"))
    End Sub
    Protected Sub drpCountry_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Rename the 'all' items to 'Please Choose....'
        siteInclude.addItemToDropdown(drpCountry, "All", "%")
        siteInclude.addItemToDropdown(drpCountry, "Please Choose....", "")
    End Sub

    'User
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblDateError.Text = ""
        bindData2()
    End Sub
    Protected Sub gvSalesLedger_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        gvType = gv.ID
        siteInclude.debug("Setting type to " & gvType)
        panLog.Visible = False
        panDetails.Visible = True
        lblDay.Text = "Sales ledger for " & FormatDateTime(gv.DataKeys(gv.SelectedIndex).Value, DateFormat.LongDate)
        Dim lblActionID As Label = gv.SelectedRow.FindControl("lblActionID")
        gvSalesLedgerDay.DataBind()
    End Sub
    Protected Sub drpCurrency_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        bindData2()
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
        Dim di As Integer = 0
        Dim type As String
        If InStr(LCase(Request.ServerVariables("PATH_INFO")), "accounts") > 0 Then
            'New accounts page is being viewed, startdate must be >= 1st Oct 2009
            di = DateDiff(DateInterval.Day, CDate(FormatDateTime("1 october 2009", DateFormat.LongDate)), CDate(FormatDateTime(date1.getStartDate, DateFormat.LongDate)))
            If di < 0 Then
                'Invalid
                lblDateError.Text = "<font color='red'>The Start Date cannot be older than 1st October 2009. The search results will be incorrect.<br>If you would like to view transactions before 1st October 2009 then please use <a href='salesLedgerMonthly.aspx'>this page</a>.</font>"
            End If
        Else
            'Old accounts page is being viewed, end date must be before 1st Oct 2009
            di = DateDiff(DateInterval.Day, CDate(FormatDateTime("1 october 2009", DateFormat.LongDate)), CDate(FormatDateTime(date1.getEndDate, DateFormat.LongDate)))
            If di >= 0 Then
                'Invalid
                lblDateError.Text = "<font color='red'>The End Date cannot be after the 1st October 2009. The search results will be incorrect.<br>If you would like to view transactions after 1st October 2009 then please use <a href='salesLedgerAccounts.aspx'>this page</a>.</font>"
            End If
        End If
        Dim gv As GridView = CType(sender, GridView)
        type = Replace(gv.ID, "gv", "") 'get the TransType
        If LCase(type) = "salesledger" Then type = drpType.SelectedValue 'This is the TransType that gets passed to the getGoodsTotal function.
        Dim iCharged As Integer = 0
        Dim dCredit As Decimal = 0
        Dim dCheque As Decimal = 0
        Dim dAffiliate As Decimal = 0
        Dim dAffiliateAcc As Decimal = 0
        Dim dVat As Decimal = 0
        Dim dGoods As Decimal = 0
        Dim dTotal As Decimal = 0
        Dim lblTotal As Label
        Dim bShowFooter As Boolean = False
        Dim lblActionID As Label
        Dim lblGoods As Label
        Dim lnkDay As LinkButton
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                bShowFooter = True
                lblActionID = row.FindControl("lblActionID")
                lblTotal = row.FindControl("lblTotalIncVat")
                lblGoods = row.FindControl("lblGoodsTotal")
                lnkDay = row.FindControl("lnkDay")
                'Get the goods total (for sales only)
                If lblActionID.Text = "1" Then lblGoods.Text = getGoodsTotal(lnkDay.Text, type)
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
                    dGoods = dGoods + Convert.ToDecimal(lblGoods.Text)
                    If drpType.SelectedValue = "%" Then dCheque = dCheque + row.Cells(_gvSalesLedger_chequePos).Text
                    If drpType.SelectedValue = "%" Then dAffiliate = dAffiliate + row.Cells(_gvSalesLedger_affiliatePos).Text
                    If drpType.SelectedValue = "%" Then dAffiliateAcc = dAffiliateAcc + row.Cells(_gvSalesLedger_affiliateAccPos).Text
                    'lblError.Text = lblError.Text & row.Cells(_gv_vatPos).Text & ","
                    'dVat = dVat + row.Cells(_gv_vatPos).Text
                    dTotal = dTotal + lblTotal.Text
                Catch ex As Exception
                    lblError.Text = lblError.Text & ex.ToString
                End Try
            End If
        Next

        If bShowFooter Then
            gv.FooterRow.Cells(_gvSalesLedger_chargedPos).Text = iCharged
            gv.FooterRow.Cells(_gvSalesLedger_creditPos).Text = FormatNumber(dCredit, 2)
            If drpType.SelectedValue = "%" Then gv.FooterRow.Cells(_gvSalesLedger_chequePos).Text = FormatNumber(dCheque, 2)
            If drpType.SelectedValue = "%" Then gv.FooterRow.Cells(_gvSalesLedger_affiliatePos).Text = FormatNumber(dAffiliate, 2)
            If drpType.SelectedValue = "%" Then gv.FooterRow.Cells(_gvSalesLedger_affiliateAccPos).Text = FormatNumber(dAffiliateAcc, 2)
            'gv.FooterRow.Cells(_gv_vatPos).Text = FormatNumber(dVat, 2)
            gv.FooterRow.Cells(siteInclude.getGVRowByHeader(gv, "Goods Total")).Text = FormatNumber(dGoods, 2)
            gv.FooterRow.Cells(siteInclude.getGVRowByHeader(gv, "Total (Inc Vat)")).Text = FormatNumber(dTotal, 2)
        End If
    End Sub
    Protected Sub gvSalesLedgerDay_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim dGoods As Decimal = 0
        Dim dVat As Decimal = 0
        Dim bShowFooter As Boolean = False
        Dim lbl As Label
        For Each row As GridViewRow In gvSalesLedgerDay.Rows
            If row.RowType = DataControlRowType.DataRow Then
                bShowFooter = True
                dGoods = dGoods + row.Cells(_gvSalesLedgerDay_goodsPos).Text
                dVat = dVat + row.Cells(_gvSalesLedgerDay_vatPos).Text
                'Highlight affAccount orders in red
                lbl = row.FindControl("lblOrderType")
                If LCase(lbl.Text) = "affaccount" Then
                    'Aff Account order found, highlight row and show text
                    row.BackColor = Drawing.Color.Pink
                    row.Cells(_gvSalesLedgerDay_affiliateSpacerPos).Text = "Aff Account"
                End If

            End If
        Next
        If bShowFooter Then
            gvSalesLedgerDay.FooterRow.Cells(_gvSalesLedgerDay_goodsPos).Text = FormatNumber(dGoods, 2)
            gvSalesLedgerDay.FooterRow.Cells(_gvSalesLedgerDay_vatPos).Text = FormatNumber(dVat, 2)
        End If
    End Sub
    Protected Sub sqlSalesLedgerDay_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        'This will load the 'actionID' parameter with a 1 (Sale) or 2 (Refund), so that only the correct items are returned for the selcted day
        Dim gv As GridView = panLog.FindControl(gvType)
        siteInclude.debug(gvType)
        siteInclude.debug(Replace(gvType, "gv", ""))
        Dim lblActionID As Label = gv.SelectedRow.FindControl("lblActionID")
        'lblError.Text = lblActionID.Text & " " & Now().TimeOfDay.ToString
        e.Command.Parameters("@actionID").Value = lblActionID.Text
        e.Command.Parameters("@type").Value = Replace(gvType, "gv", "")
        e.Command.Parameters("@ledgerDay").Value = gv.SelectedValue
        'If gvSalesLedger is selected then use the value in the dropdown
        If LCase(gvType) = "gvsalesledger" Then e.Command.Parameters("@type").Value = drpType.SelectedValue
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        bindData2()
    End Sub
    Protected Sub drpCountry_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'If drpType.SelectedValue = "%" Or drpCountry.SelectedValue = "%" Then bindData2()
        bindData2()
    End Sub
    Protected Sub bindData()
        'If '%'(All) is selected, then show the extra columns in the SalesLedger Gridview
        If drpType.SelectedValue = "%" Or drpCountry.SelectedValue = "%" Then
            gvSalesLedger.Columns(_gvSalesLedger_chequePos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_chequeSpacerPos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_affiliatePos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_affiliateSpacerPos).Visible = True
            gvSalesLedger.Visible = False
            'Hide the main gridview, and show/bind all the seperate ones
            Dim gv As GridView
            Dim lbl As Label
            Dim dt As New DataTable
            Dim bFirstPass As Boolean = True
            Dim dtCountry As DataTable
            Dim lblNewCountry As Label
            Dim panNew As Panel
            Dim panCopy As Panel

            'Loop through each payment type, if ALL countries are selected the do an outer loop of countrys
            dtCountry = loadCountryDataTable(drpCountry.SelectedValue)
            siteInclude.debug("Ready to databind with Type=" & drpType.SelectedValue & " and Country=" & drpCountry.SelectedValue & ". Rows.Count=" & dtCountry.Rows.Count)
            For Each rowCountry As DataRow In dtCountry.Rows

                For Each t As String In _transType
                    If bFirstPass Then
                        gv = panLog.FindControl("gv" & t)
                        lbl = panLog.FindControl("lbl" & t)
                    Else
                        'panCopy = panDynamicContent.FindControl("panGridvew" & rowCountry("countryName"))
                        panCopy = panNew
                        gv = panNew.FindControl("gv" & t)
                        lbl = panNew.FindControl("lbl" & t)
                        lblCountry = panNew.FindControl("countryName")
                    End If
                    'lblCountry.Text = rowCountry("countryName")
                    Try
                        lbl.Visible = False
                    Catch ex As Exception
                        siteInclude.debug("Error setting " & t & " to false")
                    End Try

                    Try
                        Dim param() As String = {"@countryCode", "@startDate", "@endDate", "@type"}
                        'Dim paramValue() As String = {Session("EBAffEBDistributorCountryCode"), date1.getStartDate, date1.getEndDate, t}
                        Dim paramValue() As String = {rowCountry("countryCode"), date1.getStartDate, date1.getEndDate, t}
                        Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar}
                        Dim paramSize() As Integer = {5, 0, 0, 50}
                        dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procSalesLedgerByPaymentDateSelect")
                        siteInclude.debug("dt.rows=" & dt.Rows.Count)
                        If dt.Rows.Count > 0 Then
                            lbl.Visible = True
                            gv.Visible = True
                        End If
                        gv.DataSource = dt
                        gv.DataBind()
                    Catch ex As Exception
                        siteInclude.addError("affiliates/saledLedgerAccounts.aspx.vb", "drpType_selectedIndexChanged(type=" & t & "); " & ex.ToString())
                    Finally
                        dt.Dispose()
                    End Try
                Next
                'Create new controls in panDynamicContent for use on the next pass
                Dim btest = True
                If btest Then
                    bFirstPass = False
                    panNew = New Panel
                    panNew.ID = "panGridview" & rowCountry("countryName")
                    'lblNewCountry = lblCountry
                    lblNewCountry = New Label
                    lblNewCountry.ID = "lblCountry"
                    panNew.Controls.Add(lblNewCountry)
                    For Each t As String In _transType
                        gv = New GridView
                        lbl = New Label
                        'gv = panLog.FindControl("gv" & t)
                        'lbl = panLog.FindControl("lbl" & t)
                        panNew.Controls.Add(lbl)
                        panNew.Controls.Add(gv)
                    Next
                End If
            Next
        Else
            gvSalesLedger.Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_chequePos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_chequeSpacerPos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_affiliatePos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_affiliateSpacerPos).Visible = False
            'Hide all other gridviews
            Dim gv As GridView
            Dim lbl As Label
            Dim dt As New DataTable
            For Each t As String In _transType
                gv = panLog.FindControl("gv" & t)
                lbl = panLog.FindControl("lbl" & t)
                gv.Visible = False
                lbl.Visible = False
            Next
        End If
        gvSalesLedger.Columns(_gvSalesLedger_affiliateAccPos).Visible = gvSalesLedger.Columns(_gvSalesLedger_affiliatePos).Visible
        gvSalesLedger.Columns(_gvSalesLedger_affiliateAccSpacerPos).Visible = gvSalesLedger.Columns(_gvSalesLedger_affiliateSpacerPos).Visible
    End Sub
    Protected Sub bindData2()
        'If '%'(All) is selected, then show the extra columns in the SalesLedger Gridview
        _transType = New String() {"amex", "account", "affaccount", "affcc", "cc", "cheque", "ddebit", "distaccount", "distcc", "fastpay", "giro", "ideal", "paypal", "phone"}
        _currencySign = New String() {"£", "€", "$"}
        _groupType = New String() {"Paypal", "Amex", "Credit Card", "Icepay", "Accounts"}
        'If drpType.SelectedValue = "%" Or drpCountry.SelectedValue = "%" Then
        If True Then
            gvSalesLedger.Columns(_gvSalesLedger_chequePos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_chequeSpacerPos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_affiliatePos).Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_affiliateSpacerPos).Visible = True
            gvSalesLedger.Visible = False
            Dim dCurrencyTotals(_CurrencyType.size, _ChargeType.size) As Decimal
            Dim gv As GridView
            Dim lbl As Label
            Dim lblRowCurrencySign As Label
            Dim sCurrecynySign As String = ""
            Dim dt As New DataTable
            Dim bShowCountry As Boolean = False
            Dim lblCountry As Label
            Dim panNew As Panel
            Dim panCopy As Panel
            Dim tbl As Table
            Dim tRow As TableRow
            Dim tCell As TableCell
            'Code from gvSalesLedger_dataBound
            Dim iCharged As Integer = 0
            Dim iChargedCountryTotal As Integer = 0
            Dim dCredit As Decimal = 0
            Dim dCheque As Decimal = 0
            Dim dAffiliate As Decimal = 0
            Dim dAffiliateAcc As Decimal = 0
            Dim dVat As Decimal = 0
            Dim dGoods As Decimal = 0
            Dim dGoodsTotal As Decimal = 0
            Dim dGoodsCountryTotal As Decimal = 0
            Dim dGoodsVat As Decimal = 0
            Dim dGoodsVatTotal As Decimal = 0
            Dim dGoodsVatCountryTotal As Decimal = 0
            Dim dShipping As Decimal = 0
            Dim dShippingTotal As Decimal = 0
            Dim dShippingCountryTotal As Decimal = 0
            Dim dShippingVat As Decimal = 0
            Dim dShippingVatTotal As Decimal = 0
            Dim dShippingVatCountryTotal As Decimal = 0
            Dim dTotal As Decimal = 0
            Dim dCountryTotal As Decimal = 0
            Dim dVatTotal As Decimal = 0
            Dim dVatCountryTotal As Decimal = 0
            Dim lblTotal As Decimal
            Dim bShowFooter As Boolean = False
            Dim lblActionID As Label
            Dim lblVat As String
            Dim lblGoods As String
            Dim lblGoodsVat As String

            Dim lnkDay As LinkButton
            Dim type As String
            Dim fontColour As String = "black"
            Dim tblCountry As Table
            Dim bpaymentSearch As Boolean = CBool(LCase(drpSearchOn.SelectedValue) = "paymentdate")
            Dim iCountryPos As Integer = 0
            Dim iChargePos As Integer = 0
            Dim dCurrencyTotal As Decimal = 0
            Dim bShowCurrencyTable As Boolean = False
            'Loop through each payment type, if ALL countries are selected the do an outer loop of countrys
            Dim dtCountry As DataTable = loadCountryDataTable(drpCountry.SelectedValue)
            'siteInclude.debug("Ready to databind with Type=" & drpType.SelectedValue & " and Country=" & drpCountry.SelectedValue & ". Rows.Count=" & dtCountry.Rows.Count)
            For Each rowCountry As DataRow In dtCountry.Rows
                bShowCountry = False
                lblCountry = New Label
                lblCountry.EnableViewState = False
                dGoodsCountryTotal = 0
                dVatCountryTotal = 0
                dGoodsCountryTotal = 0
                dGoodsVatCountryTotal = 0
                dShippingCountryTotal = 0
                dShippingVatCountryTotal = 0
                iChargedCountryTotal = 0
                'Country name
                lblCountry.Font.Bold = True
                'lblCountry.Font.Size = "Larger"
                lblCountry.Text = "<br><br><h2>" & rowCountry("countryName") & "</h2>"
                lblCountry.Visible = False
                'Add the countryname and currency sign to a seperate table
                tblCountry = createCountryTable(rowCountry("countryName"), rowCountry("currencySign"))
                sCurrecynySign = rowCountry("currencySign")
                tblCountry.Visible = False
                panDynamicContent.Controls.Add(tblCountry)
                If drpType.SelectedValue <> "%" Then _transType = New String() {drpType.SelectedValue}
                For Each t As String In _transType
                    gv = New GridView
                    lbl = New Label
                    'Label
                    lbl.Text = "<br><b>" & UCase(t) & "</b>"
                    lbl.Visible = False
                    'Gridview
                    gv.SkinID = "GridView"
                    gv.Visible = False
                    gv.ShowFooter = True
                    tbl = New Table
                    tbl.BorderWidth = "1"
                    tbl.GridLines = GridLines.Both
                    tbl.Visible = False
                    tbl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#EFE6F7")
                    type = t
                    'Reset totals
                    dGoods = 0
                    iCharged = 0
                    dTotal = 0
                    dVatTotal = 0
                    dGoodsTotal = 0
                    dGoodsVatTotal = 0
                    dShippingTotal = 0
                    dShippingVatTotal = 0
                    dCredit = 0
                    Try
                        Dim param() As String = {"@countryCode", "@startDate", "@endDate", "@type", "@currency"}
                        'Dim paramValue() As String = {Session("EBAffEBDistributorCountryCode"), date1.getStartDate, date1.getEndDate, t}
                        Dim paramValue() As String = {rowCountry("countryCode"), date1.getStartDate, date1.getEndDate, t, drpCurrency.SelectedValue}
                        Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.DateTime, SqlDbType.DateTime, SqlDbType.VarChar, SqlDbType.VarChar}
                        Dim paramSize() As Integer = {5, 0, 0, 50, 5}
                        Dim sp As String = "procShopOrderByPaymentDateSelect"
                        If Not bpaymentSearch Then sp = "procSalesLedgerByDateSelect2"
                        'siteInclude.debug("Starting " & sp & " (" & Now().Hour & ":" & Now().Minute & ":" & Now().Second & " " & Now.Millisecond & ")")
                        dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, sp)
                        'siteInclude.debug("Finished " & sp & " (" & Now().Hour & ":" & Now().Minute & ":" & Now().Second & " " & Now.Millisecond & ")")
                        'siteInclude.debug("dt.rows=" & dt.Rows.Count)
                        If dt.Rows.Count > 0 Then
                            bShowCountry = True
                            lbl.Visible = True
                            'gv.Visible = True
                            tbl.Visible = True
                            'Style
                            tbl.Attributes.Add("style", "font-size: 0.8em;")

                            'Make header
                            tRow = New TableRow
                            tRow.Attributes.Add("style", "font-size: 0.9em;")
                            tRow.ForeColor = Drawing.Color.White
                            tRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#6487DC")
                            tRow.Font.Bold = True
                            tCell = New TableCell
                            tCell.Text = "Day"
                            addCell(tRow, tCell, True)
                            tCell.Text = "Chg/Ref"
                            addCell(tRow, tCell, True)
                            'New fields
                            tCell.Text = "Goods"
                            addCell(tRow, tCell, True)
                            tCell.Text = "Goods Vat"
                            addCell(tRow, tCell, True)
                            tCell.Text = "Carriage"
                            addCell(tRow, tCell, True)
                            tCell.Text = "Carriage Vat"
                            addCell(tRow, tCell, True)
                            'End new fields
                            tCell.Text = "Total Vat"
                            addCell(tRow, tCell, True)
                            'tCell.Text = "Credit/Debit"
                            'addCell(tRow, tCell, True)
                            'tCell.Text = "Cheque"
                            'addCell(tRow, tCell,true) - Removed 3/2/10
                            'tCell.Text = "Affiliate cc"
                            'addCell(tRow, tCell,true)
                            'tCell.Text = "Affiliate acc"
                            'addCell(tRow, tCell,true)
                            'tCell.Text = "Goods Total"
                            'addCell(tRow, tCell, True) - Removed 3/2/10
                            tCell.Text = "Total (Inc Vat)"
                            addCell(tRow, tCell, False)
                            tbl.Rows.Add(tRow)
                            For Each row As DataRow In dt.Rows
                                tRow = New TableRow
                                If row("ledgerActionID") = "1" Then lblGoods = getGoodsTotalByCountry(rowCountry("countryCode"), FormatDateTime(row("day"), DateFormat.LongDate), type, lblVat)
                                'If row("ledgerActionID") = "2" Then lblGoods = getRefundByCountry(rowCountry("countryCode"), FormatDateTime(row("day"), DateFormat.LongDate), type, lblVat)
                                If bpaymentSearch Then lblGoods = 0
                                'Make font red if row is a refund row
                                If row("ledgerActionID") = "2" Or row("ledgerActionID") = "3" Then
                                    tRow.ForeColor = Drawing.Color.Red
                                    lblGoods = row("affiliateaccount")
                                    lblVat = row("vat")
                                Else
                                    tRow.ForeColor = Drawing.Color.Black
                                End If
                                'new fields
                                dGoods = 0
                                dGoodsVat = 0
                                dShipping = 0
                                dShippingVat = 0
                                If tRow.ForeColor <> Drawing.Color.Red Then
                                    'Only set values if not null and not a refund
                                    If Not IsDBNull(row("goods")) Then dGoods = row("goods")
                                    If Not IsDBNull(row("vatGoods")) Then dGoodsVat = row("vatGoods")
                                    If Not IsDBNull(row("shipping")) Then dShipping = row("shipping")
                                    If Not IsDBNull(row("vatShipping")) Then dShippingVat = row("vatShipping")
                                End If
                                'end new fields
                                iCharged = iCharged + row("items")
                                If Not bpaymentSearch Then dCredit = dCredit + row("balance")
                                If drpType.SelectedValue = "%" Then dCheque = dCheque + row("cheque")
                                If drpType.SelectedValue = "%" Then dAffiliate = dAffiliate + row("affiliatecc")
                                If drpType.SelectedValue = "%" Then dAffiliateAcc = dAffiliateAcc + row("affiliateaccount")
                                'lblError.Text = lblError.Text & row.Cells(_gv_vatPos).Text & ","
                                'dVat = dVat + row.Cells(_gv_vatPos).Text
                                If False Then
                                    lblTotal = FormatNumber(FormatNumber(row("balance") + row("vat") + row("affiliatecc") + row("affiliateaccount") + row("cheque"), 2), 2)
                                Else
                                    lblTotal = FormatNumber(FormatNumber(row("balance") + row("affiliatecc") + row("affiliateaccount") + row("cheque"), 2), 2)
                                End If
                                If tRow.ForeColor = Drawing.Color.Red Then
                                    'Refund
                                    If Not bpaymentSearch Then lblTotal = lblTotal * -1
                                    dTotal = dTotal - lblTotal
                                    'lblGoods = (lblGoods * -1)
                                    'lblTotal = lblTotal * -1
                                    'lblVat = (lblVat * -1)
                                    'lblVat = lblGoods - (lblGoods / (vatrateoforder + 100) * 100)
                                    dVatTotal = dVatTotal - lblVat
                                    'dGoodsTotal = dGoodsTotal - row("goods")
                                    'dGoodsTotal = dGoodsTotal - row("goods")
                                    'dGoodsVatTotal = dGoodsVatTotal - row("vatGoods")
                                    'dShippingTotal = dShippingTotal - row("shipping")
                                    'dShippingVatTotal = dShippingVatTotal - row("vatShipping")
                                Else
                                    'Payment
                                    dTotal = dTotal + lblTotal
                                    dVatTotal = dVatTotal + lblVat
                                    dGoodsTotal = dGoodsTotal + dGoods
                                    dGoodsVatTotal = dGoodsVatTotal + dGoodsVat
                                    dShippingTotal = dShippingTotal + dShipping
                                    dShippingVatTotal = dShippingVatTotal + dShippingVat
                                End If
                                'dGoods = dGoods + lblGoods ??????
                                

                                Try
                                    'siteInclude.debug("salesLedgerPop.aspx?day=" & row("day") & "&countryCode=" & rowCountry("countryCode") & "&type=" & type & "'>" & FormatDateTime(row("day"), DateFormat.LongDate))
                                    tCell.Text = "<a target='_blank' href='salesLedgerPopup.aspx?day=" & FormatDateTime(row("day"), DateFormat.LongDate) & "&currency=" & drpCurrency.SelectedValue & "&countryCode=" & rowCountry("countryCode") & "&actionID=" & row("ledgerActionID") & "&type=" & type & "&bypayment=" & bpaymentSearch.ToString() & "'>" & FormatDateTime(row("day"), DateFormat.LongDate) & "</a>"
                                Catch ex As Exception
                                    siteInclude.debug(ex.ToString())
                                End Try
                                'siteInclude.debug("ok")

                                'tCell.Text = "text"
                                addCell(tRow, tCell, False)
                                tCell.Text = sCurrecynySign
                                tCell.HorizontalAlign = HorizontalAlign.Center
                                addCell(tRow, tCell, False)
                                tCell.Text = row("items")
                                tCell.HorizontalAlign = HorizontalAlign.Center
                                addCell(tRow, tCell, True)
                                'New fields
                                tCell.Text = FormatNumber(dGoods, 2)
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                addCell(tRow, tCell, True)
                                tCell.Text = FormatNumber(dGoodsVat, 2)
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                addCell(tRow, tCell, True)
                                tCell.Text = FormatNumber(dShipping, 2)
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                addCell(tRow, tCell, True)
                                tCell.Text = FormatNumber(dShippingVat, 2)
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                addCell(tRow, tCell, True)
                                'End new fields
                                tCell.Text = FormatNumber(lblVat, 2)
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                addCell(tRow, tCell, True)
                                If bpaymentSearch Then
                                    tCell.Text = "0.00"
                                Else
                                    tCell.Text = FormatNumber(row("balance"), 2)
                                End If
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                'addCell(tRow, tCell, True) - Removed 3/2/10
                                'tCell.Text = FormatNumber(row("cheque"), 2)
                                'addCell(tRow, tCell,true)
                                'tCell.Text = FormatNumber(row("affiliatecc"), 2)
                                'addCell(tRow, tCell,true)
                                'tCell.Text = FormatNumber(row("affiliateaccount"), 2)
                                'addCell(tRow, tCell,true)
                                tCell.Text = FormatNumber(lblGoods, 2)
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                'addCell(tRow, tCell, True) - Removed 3/2/10
                                tCell.Text = lblTotal
                                tCell.HorizontalAlign = HorizontalAlign.Right
                                addCell(tRow, tCell, False)
                                tbl.Rows.Add(tRow)
                            Next
                            'do footer totals
                            tRow = New TableRow
                            tRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#cccccc")
                            tRow.Font.Bold = True
                            tCell = New TableCell
                            tCell.Text = "Total"
                            tCell.HorizontalAlign = HorizontalAlign.Center
                            addCell(tRow, tCell, True)
                            tCell.Text = iCharged
                            tCell.HorizontalAlign = HorizontalAlign.Center
                            addCell(tRow, tCell, True)
                            
                            'New fields
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tCell.Text = FormatNumber(dGoodsTotal, 2)
                            addCell(tRow, tCell, True)
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tCell.Text = FormatNumber(dGoodsVatTotal, 2)
                            addCell(tRow, tCell, True)
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tCell.Text = FormatNumber(dShippingTotal, 2)
                            addCell(tRow, tCell, True)
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tCell.Text = FormatNumber(dShippingVatTotal, 2)
                            addCell(tRow, tCell, True)
                            'End new fields
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            tCell.Text = FormatNumber(dVatTotal, 2)
                            addCell(tRow, tCell, True)
                            'tCell.Text = "Credit/Debit"
                            tCell.Text = FormatNumber(dCredit, 2)
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            'addCell(tRow, tCell, True) - Removed 3/2/10
                            'tCell.Text = "Cheque"
                            'addCell(tRow, tCell,true)
                            'tCell.Text = "Affiliate cc"
                            'addCell(tRow, tCell,true)
                            'tCell.Text = "Affiliate acc"
                            'addCell(tRow, tCell,true)
                            'tCell.Text = "Goods Total"
                            tCell.Text = FormatNumber(dGoods, 2)
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            'addCell(tRow, tCell, True) - Removed 3/2/10
                            'tCell.Text = "Total (Inc Vat)"
                            tCell.Text = FormatNumber(dTotal, 2)
                            tCell.HorizontalAlign = HorizontalAlign.Right
                            addCell(tRow, tCell, False)
                            tbl.Rows.Add(tRow)
                            bShowCurrencyTable = True
                            If bShowCountry Then
                                dCountryTotal = dCountryTotal + dTotal
                                dVatCountryTotal = dVatCountryTotal + dVatTotal
                                iChargedCountryTotal = iChargedCountryTotal + iCharged
                                dGoodsCountryTotal = dGoodsCountryTotal + dGoodsTotal
                                dGoodsVatCountryTotal = dGoodsVatCountryTotal + dGoodsVatTotal
                                dShippingCountryTotal = dShippingCountryTotal + dShippingTotal
                                dShippingVatCountryTotal = dShippingVatCountryTotal + dShippingVatTotal
                            End If

                        End If
                        'gv.DataSource = dt
                        'gv.DataBind()
                    Catch ex As Exception
                        siteInclude.addError("affiliates/saledLedgerAccounts.aspx.vb", "drpType_selectedIndexChanged(type=" & t & "); " & ex.ToString())
                    Finally
                        dt.Dispose()
                    End Try
                    'AddHandler gv.DataBound, AddressOf gvSalesLedger_databound
                    panDynamicContent.Controls.Add(lbl)
                    'panDynamicContent.Controls.Add(gv)
                    panDynamicContent.Controls.Add(tbl)

                    'Add details to a new table that groups totals by Currency
                    'There are 4 groups
                    '1) Amex
                    '2) CC (All non amex cc and affcc and distcc and phone orders) (phone is uk only)
                    '3) Paypal
                    '4) Icepay (Consists of ddebit, fastpay, giro, and ideal)
                    iCountryPos = -1
                    iChargePos = -1
                    Select Case LCase(rowCountry("currencyCode"))
                        Case "gbp"
                            iCountryPos = _CurrencyType.Pound
                        Case "eur"
                            iCountryPos = _CurrencyType.Euro
                        Case "usd"
                            iCountryPos = _CurrencyType.USDollar
                    End Select
                    Select Case LCase(t)
                        Case "paypal"
                            iChargePos = _ChargeType.Paypal
                        Case "amex"
                            iChargePos = _ChargeType.Amex
                        Case "affcc"
                            iChargePos = _ChargeType.CC
                        Case "distcc"
                            iChargePos = _ChargeType.CC
                        Case "cc"
                            iChargePos = _ChargeType.CC
                        Case "phone"
                            iChargePos = _ChargeType.CC
                        Case "ddebit"
                            iChargePos = _ChargeType.Icepay
                        Case "fastpay"
                            iChargePos = _ChargeType.Icepay
                        Case "giro"
                            iChargePos = _ChargeType.Icepay
                        Case "ideal"
                            iChargePos = _ChargeType.Icepay
                        Case "account"
                            iChargePos = _ChargeType.Accounts
                        Case "affaccount"
                            iChargePos = _ChargeType.Accounts
                        Case "distaccount"
                            iChargePos = _ChargeType.Accounts
                        Case "cheque"
                            iChargePos = _ChargeType.Accounts
                    End Select
                    If iCountryPos >= 0 And iChargePos >= 0 Then dCurrencyTotals(iCountryPos, iChargePos) = dCurrencyTotals(iCountryPos, iChargePos) + dTotal
                    

                Next
                If dTotal > 0 Then
                    'siteInclude.debug("Setting lblCountry(" & lblCountry.Text & ").visible to True")
                    tbl.Visible = True
                    lbl.Visible = True
                End If

                lbl = New Label
                tbl = New Table
                tbl.BorderWidth = "1"
                tbl.GridLines = GridLines.Both
                tbl.Visible = False
                tbl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#EFE6F7")

                'Show Vat totals
                tbl = New Table
                tbl.width = unit.percentage(100)
                tbl.BorderWidth = "1"
                tbl.GridLines = GridLines.Both
                tbl.Visible = False
                tbl.BorderColor = System.Drawing.ColorTranslator.FromHtml("#EFE6F7")
                lbl.Text = "<br><b style='color: red'>SUMMARY</b>"
                lbl.Visible = False
                tRow = New TableRow
                tRow.BackColor = System.Drawing.ColorTranslator.FromHtml("#cccccc")
                tRow.Font.Bold = True
                tCell = New TableCell
                tCell.Text = "Total"
                tCell.Width = Unit.Pixel(66)
                tCell.HorizontalAlign = HorizontalAlign.Center
                addCell(tRow, tCell, False)
                tCell.HorizontalAlign = HorizontalAlign.Center
                tCell.Text = sCurrecynySign
                tCell.Width = Unit.Pixel(40)
                addCell(tRow, tCell, False)
                tCell.Text = iChargedCountryTotal
                tCell.width = unit.pixel(35)
                tCell.HorizontalAlign = HorizontalAlign.Center
                addCell(tRow, tCell, True)
                'New fields
                tCell.HorizontalAlign = HorizontalAlign.Right
                tCell.Text = FormatNumber(dGoodsCountryTotal, 2)
                tCell.width = unit.pixel(30)
                addCell(tRow, tCell, True)
                tCell.HorizontalAlign = HorizontalAlign.Right
                tCell.Text = FormatNumber(dGoodsVatCountryTotal, 2)
                tCell.width = unit.pixel(40)
                addCell(tRow, tCell, True)
                tCell.HorizontalAlign = HorizontalAlign.Right
                tCell.Text = FormatNumber(dShippingCountryTotal, 2)
                tCell.width = unit.pixel(35)
                addCell(tRow, tCell, True)
                tCell.HorizontalAlign = HorizontalAlign.Right
                tCell.Text = FormatNumber(dShippingVatCountryTotal, 2)
                tCell.width = unit.pixel(50)
                addCell(tRow, tCell, True)
                'End new fields
                tCell.HorizontalAlign = HorizontalAlign.Right
                tCell.Text = FormatNumber(dVatCountryTotal, 2)
                tCell.Width = Unit.Pixel(40)
                addCell(tRow, tCell, True)
                'tCell.Text = "Credit/Debit"
                'tCell.Text = FormatNumber(dGoodsCountryTotal, 2)
                'tCell.width = unit.pixel(55)
                'tCell.HorizontalAlign = HorizontalAlign.Right
                'addCell(tRow, tCell, True) - Removed 3/2/10
                'tCell.Text = "Cheque"
                'addCell(tRow, tCell,true)
                'tCell.Text = "Affiliate cc"
                'addCell(tRow, tCell,true)
                'tCell.Text = "Affiliate acc"
                'addCell(tRow, tCell,true)
                'tCell.Text = "Goods Total"
                'tCell.Text = FormatNumber(dGoodsCountryTotal, 2)
                'tCell.HorizontalAlign = HorizontalAlign.Right
                'addCell(tRow, tCell, True) - Removed 3/2/10
                'tCell.Text = "Total (Inc Vat)"
                tCell.Text = FormatNumber(dCountryTotal, 2)
                tCell.HorizontalAlign = HorizontalAlign.Right
                addCell(tRow, tCell, False)
                tbl.Rows.Add(tRow)
                panDynamicContent.Controls.Add(lbl)
                panDynamicContent.Controls.Add(tbl)

                If bShowCountry Then
                    'siteInclude.debug("Setting lblCountry(" & lblCountry.Text & ").visible to True")
                    tbl.Visible = True
                    lbl.Visible = True
                    tblCountry.Visible = True
                End If
            Next
            'Show the Currency Table/GV
            For iRow As Integer = 0 To _CurrencyType.size
                tRow = New TableRow
                tCell = New TableCell
                'Show currency
                tCell.Text = _currencySign(iRow)
                tCell.Font.Bold = True
                tRow.Cells.Add(tCell)
                tblCurrency.Rows.Add(tRow)
                dCurrencyTotal = 0
                For iCol As Integer = 0 To _ChargeType.size
                    tRow = New TableRow
                    tCell = New TableCell
                    tCell.Text = _groupType(iCol)
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    tCell.Width = 20
                    tCell.Text = "&nbsp;"
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    tCell.Text = FormatNumber(dCurrencyTotals(iRow, iCol), 2)
                    dCurrencyTotal = dCurrencyTotal + dCurrencyTotals(iRow, iCol)
                    tCell.HorizontalAlign = HorizontalAlign.Right
                    tRow.Cells.Add(tCell)
                    tblCurrency.Rows.Add(tRow)
                Next
                tblCurrency.Rows.Add(tRow)
                'Add total for current currency
                tRow = New TableRow
                tCell = New TableCell
                tCell.ColumnSpan = 2
                tCell.Text = "&nbsp;"
                tRow.Cells.Add(tCell)
                tCell = New TableCell
                tCell.Text = FormatNumber(dCurrencyTotal, 2)
                tCell.Font.Bold = True
                tRow.Cells.Add(tCell)
                tblCurrency.Rows.Add(tRow)
            Next
            tblCurrency.Visible = bShowCurrencyTable
        Else
            gvSalesLedger.Visible = True
            gvSalesLedger.Columns(_gvSalesLedger_chequePos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_chequeSpacerPos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_affiliatePos).Visible = False
            gvSalesLedger.Columns(_gvSalesLedger_affiliateSpacerPos).Visible = False
            'Hide all other gridviews
            Dim gv As GridView
            Dim lbl As Label
            Dim dt As New DataTable
            For Each t As String In _transType
                gv = panLog.FindControl("gv" & t)
                lbl = panLog.FindControl("lbl" & t)
                gv.Visible = False
                lbl.Visible = False
            Next
        End If
        gvSalesLedger.Columns(_gvSalesLedger_affiliateAccPos).Visible = gvSalesLedger.Columns(_gvSalesLedger_affiliatePos).Visible
        gvSalesLedger.Columns(_gvSalesLedger_affiliateAccSpacerPos).Visible = gvSalesLedger.Columns(_gvSalesLedger_affiliateSpacerPos).Visible
    End Sub
    Protected Sub addCell(ByRef row As TableRow, ByRef cell As TableCell, ByVal incSpacer As Boolean)
        row.Cells.Add(cell)
        If incSpacer Then
            cell = New TableCell
            cell.Width = Unit.Pixel(40)
            cell.Text = "&nbsp;"
            row.Cells.Add(cell)
        End If
        cell = New TableCell
    End Sub

    Protected Function getGoodsTotal(ByVal d As String, ByVal t As String) As Decimal
        Dim result As Decimal = 0
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@countryCode", "@day", "@type"}
            Dim paramValue() As String = {drpCountry.SelectedValue, d, t}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar}
            Dim paramSize() As Integer = {5, 20, 20}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procShopOrderByPaymentDateGoodsTotalSelect")
            Try
                If dt.Rows.Count > 0 Then result = dt.Rows(0)("goods")
            Catch ex As Exception
            End Try
        Catch ex As Exception
            siteInclude.addError("maintenance/salesLedgerAccounts.aspx.vb", "getGoodsTotal(d=" & d & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
        Return result
    End Function
    Protected Function getGoodsTotalByCountry(ByVal cc As String, ByVal d As String, ByVal t As String, ByRef vat As String) As Decimal
        Dim result As Decimal = 0
        vat = "0"
        Dim dt As New DataTable
        Try
            Dim sp As String = "procShopOrderByPaymentDateGoodsTotalSelect"
            If LCase(drpSearchOn.SelectedValue) = "statementdate" Then sp = "procShopOrderByScanDateGoodsTotalSelect"
            Dim param() As String = {"@countryCode", "@day", "@type"}
            Dim paramValue() As String = {cc, d, t}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar}
            Dim paramSize() As Integer = {5, 20, 20}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, sp)
            Try
                If dt.Rows.Count > 0 Then
                    result = dt.Rows(0)("goods")
                    vat = dt.Rows(0)("vat")
                End If
            Catch ex As Exception
            End Try
        Catch ex As Exception
            siteInclude.addError("maintenance/salesLedgerAccounts.aspx.vb", "getGoodsTotal(d=" & d & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
        Return result
    End Function
    Protected Function getRefundByCountry(ByVal cc As String, ByVal d As String, ByVal t As String, ByRef vat As String) As Decimal
        Dim result As Decimal = 0
        vat = "0"
        Dim dt As New DataTable
        Try
            Dim sp As String = "procSalesLedgerByPaymentDateRefundTotalSelect"
            If LCase(drpSearchOn.SelectedValue) = "statementdate" Then sp = "procSalesLedgerByScanDateRefundTotalSelect"
            Dim param() As String = {"@countryCode", "@day", "@type"}
            Dim paramValue() As String = {cc, d, t}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar}
            Dim paramSize() As Integer = {5, 20, 20}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, sp)
            Try
                If dt.Rows.Count > 0 Then
                    result = ("debit")
                    vat = dt.Rows(0)("vat")
                    siteInclude.debug("Day=" & d & ", debit=" & result & ", vat=" & vat)
                End If
            Catch ex As Exception
            End Try
        Catch ex As Exception
            siteInclude.addError("maintenance/salesLedgerAccounts.aspx.vb", "getGoodsTotal(d=" & d & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
        Return result
    End Function
    Protected Function loadCountryDataTable(ByVal cc As String) As DataTable
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@countryCode"}
            Dim paramValue() As String = {cc}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar}
            Dim paramSize() As Integer = {5}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procCountryByCountryCodeSelect")
            'dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procCountryMaintenanceSelect")

            Return dt
        Catch ex As Exception
            siteInclude.addError("affilaites/salesLedgerAccounts.aspx.vb", "loadCountryDataTable(cc=" & cc & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
        Return New DataTable
    End Function
    Protected Function createCountryTable(ByVal c As String, ByVal cs As String) As Table
        Dim tbl As New Table
        Dim tCell As New TableCell
        Dim tRow As New TableRow
        tCell.Text = "<br><br><h2>" & c & "</h2>"
        tRow.Cells.Add(tCell)
        tCell = New TableCell
        tCell.Text = "<br><br><h2>" & cs & "</h2>"
        tCell.HorizontalAlign = HorizontalAlign.Right
        tRow.Cells.Add(tCell)
        tbl.Rows.Add(tRow)
        tbl.Width = Unit.Percentage(54)
        tbl.EnableViewState = False
        Return tbl
    End Function
End Class

