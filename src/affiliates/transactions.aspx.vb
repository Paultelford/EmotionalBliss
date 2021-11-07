Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class affiliates_transactions
    Inherits System.Web.UI.Page
    Private Const _gvStatement_orderPos As Integer = 2
    Private Const _gvStatement_actionPos As Integer = 4
    Private Const _gvStatement_AffiliatePos As Integer = 6
    Private Const _gvStatement_paymentReceiptPos As Integer = 7
    Private Const _gvStatement_affiliateNameSpacerPos As Integer = 8
    Private Const _gvStatement_trackerPos As Integer = 9
    Private Const _gvStatement_paymentRefPos As Integer = 11
    Private Const _gvStatement_creditPos As Integer = 13
    Private Const _gvStatement_debitPos As Integer = 15
    Private Const _gvStatement_checkPos As Integer = 16
    Private Const _gvStatement_balancePos As Integer = 17
    Private Const _actionID_credit As Integer = 5
    Private Const _actionID_debit As Integer = 6
    Private Const _actionID_paymentIn As Integer = 7
    Private Const _actionID_paymentOut As Integer = 8
    Private Const _allTransactions As Integer = 0
    Private Const _EBTransactions As Integer = 1
    Private Const _affiliateTransactions As Integer = 2
    Private Const _customerTransactions As Integer = 3
    Private Const _actionID_affClickThrough As Integer = 13
    Private Const _actionID_affPaymentOut As Integer = 12
    Private Const _actionID_affPaymentIn As Integer = 11
    Private Const _actionID_distOrder As Integer = 1
    Private _currencySign As String = ""

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffEBDistributorCountryCode") = "" Then
            Response.Redirect("default.aspx")
        Else
            If Not (Session("EBAffEBDistributor") Or Session("EBAffEBUser")) Then
                If Not Page.IsPostBack Then
                    'Affiliate is logged in, not distributor. Hide dropdown, and default selection to show affiliateTransactions only
                    tdView.Visible = False
                    gvStatement.DataSourceID = "sqlStatementAffOnly"
                    sqlStatement.SelectCommand = "procAffiliateAffStatementByAffIDSelect"
                End If
            Else
                If Page.IsPostBack Then
                    'Distributor is logged in.
                    'sqlDataSource keeps on dropping its select command. So reload it on each page refresh.
                    'setDataSourceAndSelectCommand()
                End If
            End If
        End If

    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        showClickThroughs()
        Dim drp As DropDownList = Master.FindControl("drpAffmenu")
        drp.SelectedValue = "statement"
        gvStatement.DataBind()
    End Sub

    'Page Events
    Protected Sub gvStatement_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvStatement.PageIndexChanging
        Dim gv As GridView = CType(sender, GridView)
        gv.PageIndex = e.NewPageIndex
        gv.DataBind()
    End Sub
    Protected Sub gvStatement_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvStatement.DataBinding
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerControl(gv)
    End Sub
    Protected Sub gvStatement_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvStatement.DataBound
        Dim gv As GridView = CType(sender, GridView)
        Dim s As New siteInclude
        Dim cr As Decimal = 0
        Dim dr As Decimal = 0
        Dim balance As Decimal = 0
        Dim chk As CheckBox
        Dim lbl As Label
        Dim lblActionID As Label
        Dim lnkAction As HyperLink
        Dim lblOrderID As Label 'User order ID inc country code
        Dim lnkOrderID As HyperLink
        Dim lblStatementID As Label
        Dim lblPaymentID As Label
        Dim lblCredit As Label
        Dim lblDebit As Label
        Dim lblBalance As Label
        Dim lblID As Label 'ShopOrder PK field
        If _currencySign = "" Then _currencySign = s.getCurrencySignByCurrencyCode(drpCurrency.SelectedValue)
        s = Nothing
        'Add each row up / and hide the checkbox if a paymentRef already exists
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'get currency
                'get reference to checkbox and paymentRef
                lbl = row.FindControl("lblPaymentRef")
                chk = row.FindControl("chkAdd")
                lblOrderID = row.FindControl("lblOrderID")
                lnkOrderID = row.FindControl("lnkInvoice")
                lblActionID = row.FindControl("lblActionID")
                lnkAction = row.FindControl("lnkAction")
                lblID = row.FindControl("lblID")
                lblPaymentID = row.FindControl("lblPaymentID")
                lblStatementID = row.FindControl("lblStatementID")
                lblCredit = row.FindControl("lblCredit")
                lblDebit = row.FindControl("lblDebit")
                lblBalance = row.FindControl("lblBalance")
                'If user is distributor, show link to orderView page
                If Session("EBAffEBDistributor") Then
                    lblOrderID.Visible = False
                    lnkOrderID.Visible = True
                Else
                    lblOrderID.Visible = True
                    lnkOrderID.Visible = False
                End If
                'Add to total
                'cr = cr + CType(row.Cells(_gvStatement_creditPos).Text, Decimal)
                'dr = dr + CType(row.Cells(_gvStatement_debitPos).Text, Decimal)
                cr = cr + CType(lblCredit.Text, Decimal)
                dr = dr + CType(lblDebit.Text, Decimal)
                'If Not (Session("EBAffEBDistributor")) Then row.Cells(_gvStatement_balancePos).Text = row.Cells(_gvStatement_balancePos).Text * -1
                If Not (Session("EBAffEBDistributor")) Then lblBalance.Text = lblBalance.Text * -1
                balance = balance + CType(lblBalance.Text, Decimal)
                'If there is a (Then hide the '/0' orderID)
                If lnkOrderID.Text = "/0" Or lnkOrderID.Text = "0" Then
                    lnkOrderID.Text = ""
                Else
                    'Remove country code from orderID
                    lnkOrderID.Text = Replace(lnkOrderID.Text, "ZZ", "") 'Replace ZZ
                    lnkOrderID.Text = Replace(lnkOrderID.Text, UCase(Session("EBAffEBDistributorCountryCode")), "") 'Replace Aff Countrycode
                    'Update Each rows Balance
                    'row.Cells(_gvStatement_balancePos).Text = CType(FormatNumber(balance, 2), String)
                    lblBalance.Text = CType(FormatNumber(balance, 2), String)
                End If
                If row.Cells(_gvStatement_paymentRefPos).Text <> "*" Then
                    chk.Visible = False
                Else
                    row.Cells(_gvStatement_paymentRefPos).Text = ""
                End If
                If Not (lbl.Text = String.Empty) Then
                    chk.Visible = False
                End If
                'If there is no order ID to show, then display the credit/debit/payment ID instead
                If lnkOrderID.Text = "/" Then lnkOrderID.Text = lbl.Text
                'Specific code for each Action Type
                Select Case CInt(lblActionID.Text)
                    Case _actionID_distOrder
                        'Dist orders can contain multiple trackers. Grab now
                        If IsNumeric(lblID.Text) Then getTrackers(lblID.Text, row)
                        'Change order ID into a link, and remove the ZZ country code
                        row.Cells(_gvStatement_orderPos).Text = "<a target='_blank' href='statementDistInvoice.aspx?id=" & lblID.Text & "'>" & Replace(LCase(lblOrderID.Text), "zz", "") & "</a>"
                    Case _actionID_affClickThrough
                        'Click through order, show a link to a popup showing the affiliate what they earnt for that order
                        'lblOrderID.Visible = False
                        'lnkOrderID.Visible = True
                        'lnkOrderID.Text = lblOrderID.Text
                        lnkAction.NavigateUrl = "statementEarnings.aspx?id=" & lblID.Text
                    Case _actionID_paymentOut
                        'Move the Payment Ref field to the action column
                        row.Cells(_gvStatement_orderPos).Text = row.Cells(_gvStatement_orderPos).Text & " <a target='_blank' href='statementPaymentPopup.aspx?sid=" & lblStatementID.Text & "'>PO" & lblPaymentID.Text & "</a>"
                        row.Cells(_gvStatement_paymentRefPos).Text = ""
                    Case _actionID_paymentIn
                        'Move the Payment Ref field to the action column
                        row.Cells(_gvStatement_orderPos).Text = row.Cells(_gvStatement_orderPos).Text & " <a target='_blank' href='statementPaymentPopup.aspx?sid=" & lblStatementID.Text & "'>PI" & lblPaymentID.Text & "</a>"
                        row.Cells(_gvStatement_paymentRefPos).Text = ""
                    Case _actionID_credit
                        row.Cells(_gvStatement_orderPos).Text = row.Cells(_gvStatement_orderPos).Text & " <a target='_blank' href='statementPaymentPopup.aspx?sid=" & lblStatementID.Text & "'>CR" & lblPaymentID.Text & "</a>"
                        row.Cells(_gvStatement_paymentRefPos).Text = ""
                    Case _actionID_debit
                        row.Cells(_gvStatement_orderPos).Text = row.Cells(_gvStatement_orderPos).Text & " <a target='_blank' href='statementPaymentPopup.aspx?sid=" & lblStatementID.Text & "'>DR" & lblPaymentID.Text & "</a>"
                        row.Cells(_gvStatement_paymentRefPos).Text = ""
                End Select

            End If
        Next
        'Set toals in footer row
        If cr > 0 Or dr > 0 Then
            gv.FooterRow.Cells(_gvStatement_creditPos).Text = _currencySign & CType(FormatNumber(cr, 2), String)
            gv.FooterRow.Cells(_gvStatement_debitPos).Text = _currencySign & CType(FormatNumber(dr, 2), String)
            gv.FooterRow.Cells(_gvStatement_balancePos).Text = _currencySign & CType(FormatNumber(balance, 2), String)
            'Depending on wether user is affilaite or ditsributr, hide certain fields from the statement gridview
            If CBool(Session("EBAffEBDistributor")) Then

            Else
                'Affilite. Hide affiliate column, no need to show them their own name, show the payment receipt instead
                gvStatement.Columns(_gvStatement_AffiliatePos).Visible = False
                gvStatement.Columns(_gvStatement_affiliateNameSpacerPos).Visible = False
                gvStatement.Columns(_gvStatement_paymentReceiptPos).Visible = True
            End If
        End If
        pager1.registerData(gv)
    End Sub
    Protected Sub drpCurrency_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If drpCurrency.Items.Count < 2 Then panCurrency.Visible = False
        For Each li As ListItem In drpCurrency.Items
            If IsDBNull(li.Value) Then li.Enabled = False
            If li.Value = "" Then li.Enabled = False
        Next
        gvStatement.DataBind()
    End Sub

    'User Events
    Protected Sub drpAffiliate_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Only show the Make Transaction button if an affiliate is selected.
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.SelectedIndex = 0 Then
            lnkMakeTransaction.Visible = False
            panTrans.Visible = False
        Else
            lnkMakeTransaction.Visible = True
        End If
    End Sub
    Protected Sub drpView_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As DropDownList = CType(sender, DropDownList)
        Dim storedProcedure As String = ""
        drpAffiliate.Visible = False
        lnkMakeTransaction.Visible = False
        panTrans.Visible = False
        clearTransTable()
        gvStatement.DataSourceID = "sqlStatement"
        Select Case drpView.SelectedValue
            Case _allTransactions
                storedProcedure = "procAffiliateStatementByAffIDSelect"
            Case _EBTransactions
                storedProcedure = "procAffiliateEBStatementByAffIDSelect"
            Case _affiliateTransactions
                storedProcedure = "procAffiliateAffStatementByAffIDSelect"
                drpAffiliate.Visible = True 'Show affiliate Dropdown
                gvStatement.Columns(_gvStatement_AffiliatePos).Visible = True 'If 
                drpAffiliate.SelectedIndex = 0
                gvStatement.DataSourceID = "sqlStatementAff"
            Case _customerTransactions
                storedProcedure = "procAffiliateCustomerStatementByAffIDSelect"
        End Select
        sqlStatement.SelectCommand = storedProcedure
        gvStatement.DataBind()
    End Sub
    Protected Sub lnkMakeTransaction_click(ByVal Sender As Object, ByVal e As EventArgs)
        panTrans.Visible = True
        txtTransDate.Text = FormatDateTime(Now(), DateFormat.LongDate)
    End Sub
    Protected Sub btnTransfer_click(ByVal Sender As Object, ByVal e As EventArgs)
        Page.Validate("trans")
        If Page.IsValid And IsDate(txtTransDate.Text) Then
            Dim si As New siteInclude
            Dim cr As Decimal = 0
            Dim dr As Decimal = 0
            Dim prefix As String = ""
            Dim actionID As Integer = CType(radType.SelectedValue, Integer)
            Dim affStatementID As Integer
            Dim distStatementID As Integer
            Dim paymentID As Integer
            Dim bTickboxesNeedTesting As Boolean = False
            Select Case actionID
                Case _actionID_credit
                    cr = CType(txtAmount.Text, Decimal)
                    prefix = "CR"
                Case _actionID_Debit
                    dr = CType(txtAmount.Text, Decimal)
                    prefix = "DR"
                Case _actionID_paymentIn
                    cr = CType(txtAmount.Text, Decimal)
                    prefix = "PI"
                    bTickboxesNeedTesting = True
                Case _actionID_paymentOut
                    dr = CType(txtAmount.Text, Decimal)
                    prefix = "PO"
                    bTickboxesNeedTesting = True
            End Select
            If atLeastOneCheckboxHasBeenTicked() Or (Not bTickboxesNeedTesting) Then
                lblTransactionUserError.Text = ""
                'Add to affiliate statement (returns pk)
                affStatementID = si.affAddToStatement(getSelectedAffID, cr, dr, 0, 0, actionID, prefix, txtCheque.Text, txtReason.Text, FormatDateTime(txtTransDate.Text, DateFormat.LongDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime))
                'Add to affilaiteStatementPayment (and return pk)
                paymentID = insertStatementPayment(affStatementID)
                'Link statement entry to paymentID
                linkStatementToPayment(affStatementID, paymentID, prefix)
                'Add paymentID to all checked statement entries (as paymentRef)
                linkToPayment(paymentID, prefix)
                'Now add to Distributor statement(return pk)
                distStatementID = si.affAddToStatement(Session("EBDistID"), dr, cr, 0, 0, actionID, prefix, txtCheque.Text, txtReason.Text, FormatDateTime(txtTransDate.Text, DateFormat.LongDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime))
                'Add to affilaiteStatementPayment
                insertDistStatementPayment(distStatementID, paymentID)
                'Link statement entry to dist paymentID
                linkStatementToPayment(distStatementID, paymentID, prefix)

                'Rebind gridview to show changes
                gvStatement.DataBind()
                'Hide checkboxes
                gvStatement.Columns(_gvStatement_checkPos).Visible = False
                'Hide transaction table
                panTrans.Visible = False
                clearTransTable()
            Else
                lblTransactionUserError.Text = lblTransactionUserError.Text & "You must tick at least one box"
            End If
        Else
            'Validation failed
            lblTransactionUserError.Text = "Please complete all the fields"
            If Not IsDate(txtTransDate.Text) Then lblTransDateError.Text = "<font color='red'>Invalid date entered</font>"
        End If
    End Sub
    Protected Sub radType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If CType(radType.SelectedValue, Integer) = _actionID_paymentIn Or CType(radType.SelectedValue, Integer) = _actionID_paymentOut Then
            'Make the checkboxes visible
            gvStatement.Columns(_gvStatement_checkPos).Visible = True
        Else
            'Hide the checkboxes
            gvStatement.Columns(_gvStatement_checkPos).Visible = False
        End If
    End Sub
    Protected Sub btnAffCreditSubmit_click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    'Subs
    Protected Sub insertDistStatementPayment(ByVal affStatementID As Integer, ByVal paymentID As Integer)
        Dim pk As Integer = 0
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateStatementDistPaymentInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affStatementID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@paymentID", SqlDbType.Int))
            .Parameters("@affStatementID").Value = affStatementID
            .Parameters("@paymentID").Value = paymentID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblTransactionUserError.Text = "Error while storing the distributors payment reference.<br>" & ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub linkStatementToPayment(ByVal affStatementID As Integer, ByVal paymentID As Integer, ByVal prefix As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As SqlCommand
        oCmd = New SqlCommand("procAffiliateStatementByIDPaymentUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@statementID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@linkedPrefix", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@linkedRef", SqlDbType.Int))
            .Parameters("@statementID").Value = affStatementID
            .Parameters("@linkedPrefix").Value = prefix
            .Parameters("@linkedRef").Value = paymentID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblTransactionUserError.Text = lblTransactionUserError.Text & "An error occured while assinging the payment refernce of '" & prefix & "/" & paymentID & "' to the selected entries.<br>" & ex.ToString
        End Try
    End Sub
    Protected Sub clearTransTable()
        txtAmount.Text = ""
        radType.SelectedIndex = 0
        txtCheque.Text = ""
        txtReason.Text = ""
    End Sub
    Protected Sub getTrackers(ByVal orderID As Integer, ByRef gvRow As GridViewRow)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procTrackerByOrderIDSelectALL", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim lnk As HyperLink
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = orderID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    lnk = New HyperLink
                    lnk.NavigateUrl = "http://track.dhl.co.uk/tracking/wrd/run/wt_xhistory_pw.execute?PCL_NO=" & row("trackerCode") & "&PCL_INST=1&COLLDATE=&CNTRY=" & UCase(Session("EBAffEBDistributorCountryCode"))
                    lnk.Text = row("trackerCode") & "<br>"
                    lnk.Target = "_blank"
                    gvRow.Cells(_gvStatement_trackerPos).Controls.Add(lnk)
                Next
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("affiliates/transactions.aspx.vb", "getTrackers(orderID=" & orderID & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub linkToPayment(ByVal pid As Integer, ByVal p As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand
        Dim chk As CheckBox
        Try
            For Each row As GridViewRow In gvStatement.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    chk = row.FindControl("chkAdd")
                    If chk.Checked Then
                        'Add payment reference to current item
                        oCmd = New SqlCommand("procAffiliateStatementByOrderIDPaymentUpdate", oConn)
                        With oCmd
                            .CommandType = CommandType.StoredProcedure
                            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@linkedPrefix", SqlDbType.VarChar, 5))
                            .Parameters.Add(New SqlParameter("@linkedRef", SqlDbType.Int))
                            .Parameters("@orderID").Value = CType(gvStatement.DataKeys(row.RowIndex).Value, Integer)
                            .Parameters("@linkedPrefix").Value = p
                            .Parameters("@linkedRef").Value = pid
                        End With
                        Try
                            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                            oCmd.ExecuteNonQuery()
                        Catch ex As Exception
                            lblTransactionUserError.Text = lblTransactionUserError.Text & "An error occured while assinging the payment refernce of '" & p & "/" & pid & "' to the selected entries.<br>" & ex.ToString
                        End Try
                    End If
                End If
            Next
        Catch ex As Exception
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub setDataSourceAndSelectCommand()
        gvStatement.DataSourceID = "sqlStatement"
        gvStatement.Columns(_gvStatement_AffiliatePos).Visible = True
        gvStatement.Columns(_gvStatement_paymentReceiptPos).Visible = False
        Select Case drpView.SelectedValue
            Case _allTransactions
                sqlStatement.SelectCommand = "procAffiliateStatementByAffIDSelect"
                drpAffiliate.SelectedIndex = 0
            Case _EBTransactions
                sqlStatement.SelectCommand = "procAffiliateEBStatementByAffIDSelect"
                drpAffiliate.SelectedIndex = 0
            Case _affiliateTransactions
                sqlStatement.SelectCommand = "procAffiliateAffStatementByAffIDSelect"
                drpAffiliate.Visible = True 'Show affiliate Dropdown
                If drpAffiliate.Visible And drpAffiliate.SelectedIndex > 0 Then 'If an affilaite has been chosen from the dropdown then hide the affiliate name field and show CC receipt instead
                    gvStatement.Columns(_gvStatement_AffiliatePos).Visible = False
                    gvStatement.Columns(_gvStatement_paymentReceiptPos).Visible = True
                End If

                gvStatement.DataSourceID = "sqlStatementAff"
            Case _customerTransactions
                sqlStatement.SelectCommand = "procAffiliateCustomerStatementByAffIDSelect"
                drpAffiliate.SelectedIndex = 0
        End Select
    End Sub
    Protected Sub showClickThroughs()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateLogByAffIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim dayCount As Long = DateDiff("d", date1.getStartDate, date1.getEndDate)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
            .Parameters("@affID").Value = Session("EBAffID")
            .Parameters("@startDate").Value = date1.getStartDate
            .Parameters("@endDate").Value = date1.getEndDate
        End With
        'If distributor is logged in, then use the selected affilaiteID from the dropdown, rather than from session.
        If drpAffiliate.SelectedValue <> "%" Then oCmd.Parameters("@affID").Value = drpAffiliate.SelectedValue
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'lblClickTotal.Text = ds.Tables(0).Rows(0)("total")
                'lblClickDate.Text = ds.Tables(0).Rows(0)("range")
                lblClickTotal.Text = ds.Tables(0).Rows(0)("range")
                lblClickDate.Text = dayCount
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
            Dim si As New siteInclude
            si.addError("affiliates/statement.aspx", "showClickThroughs(affID=" & Session("EBAffID") & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub

    'Functions
    Protected Function showOrderID(ByVal extID As Integer, ByVal extUserOrderID As Object, ByVal ebPrefix As Object, ByVal ebOrderID As Object, ByVal ebCountryCode As Object) As String
        Dim result As String
        If extID = 0 Then
            result = ebOrderID.ToString & UCase(ebCountryCode.ToString)
        Else
            result = Parse(extUserOrderID.ToString)
        End If
        Return result
    End Function
    Protected Function getActionURL(ByVal extID As Integer, ByVal extUserOrderID As Object, ByVal ebPrefix As Object, ByVal ebOrderID As Object, ByVal ebCountryCode As Object, ByVal action As Object) As String
        Dim result As String
        Select Case CInt(action.ToString)
            Case 13 'Affiliate Click Through

        End Select
    End Function
    Protected Function showDate(ByVal d As Object)
        Dim result As String = "Unknown"
        If Not IsDBNull(d) Then
            result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        End If
        Return result
    End Function
    Protected Function getSelectedAffID() As Integer
        Dim result As Integer = 0
        'If drpAff.SelectedIndex <> 0 Then result = CType(drpAff.SelectedValue, Integer)
        'If drpDist.SelectedIndex <> 0 Then result = CType(drpDist.SelectedValue, Integer)
        result = CType(drpAffiliate.SelectedValue, Integer)
        Return result
    End Function
    Protected Function Parse(ByVal s As String) As String
        'Removes the affID from the end of the extOrderID
        Dim a As Array = Split(s, "_")
        Return a(0)
    End Function
    Protected Function atLeastOneCheckboxHasBeenTicked()
        Dim result As Boolean = False
        Dim chk As CheckBox
        For Each row As GridViewRow In gvStatement.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'If checkbox has been ticked, set the result to TRUE
                chk = row.FindControl("chkAdd")
                If chk.Checked Then result = True
            End If
        Next
        Return result
    End Function
    Protected Function insertStatementPayment(ByVal affStatementID As Integer) As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateStatementPaymentInsert", oConn)
        Dim pk As Integer
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affStatementID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
            .Parameters("@affStatementID").Value = affStatementID
            .Parameters("@pk").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            pk = CType(oCmd.Parameters("@pk").Value, Integer)
        Catch ex As Exception
            lblTransactionUserError.Text = "Error while storing the affiliates payment reference.<br>" & ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return pk
    End Function
    Protected Function userSelected()
        Dim result As Boolean = True
        If drpAffiliate.SelectedIndex = 0 Then result = False
        Return result
    End Function
    Protected Function makeInvoiceURL(ByVal actionID As Integer, ByVal orderID As Integer)
        Dim r As String = ""
        If actionID = _affiliateTransactions Then r = "statementInvoice.aspx?id=" & orderID
        Return r
    End Function
End Class
