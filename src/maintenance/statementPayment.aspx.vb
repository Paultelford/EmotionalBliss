Imports siteInclude
Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_statementPayment
    Inherits System.Web.UI.Page
    Private Const _gvStatement_orderPos As Integer = 2
    Private Const _gvStatement_paymentRefPos As Integer = 6
    Private Const _gvStatement_creditPos As Integer = 8
    Private Const _gvStatement_debitPos As Integer = 10
    Private Const _gvStatement_checkPos As Integer = 11
    Private Const _gvStatement_balancePos As Integer = 12
    Private Const _distOrder As Integer = 1
    Private Const _addCredit As Integer = 5
    Private Const _addDebit As Integer = 6
    Private Const _addPaymentIn As Integer = 7
    Private Const _addPaymentOut As Integer = 8

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            'dateAff.setStartDate = "1 july 2007"
        Else
            Response.Redirect("default.aspx")
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then txtDate.Text = FormatDateTime(Now(), DateFormat.LongDate)
    End Sub
    
    Protected Sub clearErrors()
        lblTransactionUserError.Text = ""
        txtAmount.Text = ""
        radType.SelectedIndex = 0
        txtReason.Text = ""
        txtCheque.Text = ""
    End Sub
    Protected Function getSelectedID() As Integer
        Dim result As Integer = 0
        If drpAff.SelectedIndex <> 0 Then result = CType(drpAff.SelectedValue, Integer)
        If drpDist.SelectedIndex <> 0 Then result = CType(drpDist.SelectedValue, Integer)
        Return result
    End Function

    'Page Events
    Protected Sub gvStatement_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Dim cr As Decimal = 0
        Dim dr As Decimal = 0
        Dim balance As Decimal = 0
        Dim chk As CheckBox
        Dim lbl As Label
        Dim lblID As Label
        Dim lblOrderID As Label
        Dim lblActionID As Label
        Dim lblStatementID As Label
        Dim lblCredit As Label
        Dim lblDebit As Label
        Dim lblBalance As Label
        Dim lblCurrencySign As Label
        Dim currencySign As String = ""
        'Add each row up / and hide the checkbox if a paymentRef already exists
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'Grab currency sign so it can be used in the footer
                lblCurrencySign = row.FindControl("lblCurrencySign1")
                currencySign = lblCurrencySign.Text
                'get reference to checkbox and paymentRef
                lbl = row.FindControl("lblPaymentRef")
                chk = row.FindControl("chkAdd")
                lblActionID = row.FindControl("lblActionID")
                lblOrderID = row.FindControl("lblOrderID")
                lblStatementID = row.FindControl("lblStatementID")
                lblCredit = row.FindControl("lblCredit")
                lblDebit = row.FindControl("lblDebit")
                lblBalance = row.FindControl("lblBalance")
                lblID = row.FindControl("lblID")
                'Add to total
                cr = cr + CType(lblCredit.Text, Decimal)
                dr = dr + CType(lblDebit.Text, Decimal)
                balance = balance + CType(lblBalance.Text, Decimal)
                'If there is a (Then hide the '/0' orderID)
                If lblOrderID.Text = "/0" Then
                    lblOrderID.Text = ""
                Else
                    'Update Each rows Balance
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
                lblOrderID = row.FindControl("lblOrderID")
                If lblOrderID.Text = "/" Then lblOrderID.Text = lbl.Text
                Select Case CInt(lblActionID.Text)
                    Case _distOrder
                        'Make link to orderView.aspx page
                        lblOrderID.Text = "<a href='orderView.aspx?id=" & lblID.Text & "'>" & lblOrderID.Text & "</a>"
                    Case _addPaymentIn
                        'Make a link to the Peartree Payment popup
                        lblOrderID.Text = "<a href='statementPaymentPopup.aspx?sid=" & lblStatementID.Text & "' target='_blank'>" & lbl.Text & "</a>"
                    Case _addPaymentOut
                        'Make a link to the Peartree Payment popup
                        lblOrderID.Text = "<a href='statementPaymentPopup.aspx?sid=" & lblStatementID.Text & "' target='_blank'>" & lbl.Text & "</a>"
                    Case _addCredit
                        'Make a link to the Peartree Payment popup
                        lblOrderID.Text = "<a href='statementPaymentPopup.aspx?sid=" & lblStatementID.Text & "' target='_blank'>" & lbl.Text & "</a>"
                    Case _addDebit
                        'Make a link to the Peartree Payment popup
                        lblOrderID.Text = "<a href='statementPaymentPopup.aspx?sid=" & lblStatementID.Text & "' target='_blank'>" & lbl.Text & "</a>"
                End Select
            End If
        Next
        'Set toals in footer row
        If cr > 0 Or dr > 0 Then
            gv.FooterRow.Cells(_gvStatement_creditPos).Text = currencySign & CType(FormatNumber(cr, 2), String)
            gv.FooterRow.Cells(_gvStatement_debitPos).Text = currencySign & CType(FormatNumber(dr, 2), String)
            gv.FooterRow.Cells(_gvStatement_balancePos).Text = currencySign & CType(FormatNumber(balance, 2), String)
        End If
    End Sub
    Protected Sub drpStatementCurrency_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim d As New siteInclude
        If drpStatementCurrency.Items.Count < 2 Then
            lblStatementCurrencyText.Visible = False
            drpStatementCurrency.Visible = False
        End If
        For Each li As ListItem In drpStatementCurrency.Items
            If IsDBNull(li.Value) Then li.Enabled = False
            If li.Value = "" Then li.Enabled = False
        Next
        gvStatement.DataBind()
    End Sub

    'User Events
    Protected Sub drpAff_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If CType(drpAff.SelectedValue, Integer) <> 0 Then
            sqlStatement.SelectParameters("affID").DefaultValue = drpAff.SelectedValue 'Load selected affiliateID into the statement SELECT parameter.
            panAffStatement.Visible = True
            lblAffiliateHeader.Text = "Affiliate Statement"
            lblAffiliateName.Text = drpAff.SelectedItem.Text
            'Reset other dropdowns
            drpDist.SelectedIndex = 0
            clearErrors()
        Else
            'Hide statement
            panAffStatement.Visible = False
        End If
        tblTransaction.Visible = False
    End Sub
    Protected Sub drpDist_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If CType(drpDist.SelectedValue, Integer) <> 0 Then
            sqlStatement.SelectParameters("affID").DefaultValue = drpDist.SelectedValue 'Load selected affiliateID into the statement SELECT parameter.
            panAffStatement.Visible = True
            lblAffiliateHeader.Text = "Distributor Statement"
            lblAffiliateName.Text = drpDist.SelectedItem.Text
            'Reset other dropdowns
            drpAff.SelectedIndex = 0
            clearErrors()
        Else
            'Hide statement
            panAffStatement.Visible = False
        End If
        tblTransaction.Visible = False
    End Sub
    
    'Functions
    Protected Function Parse(ByVal s As String) As String
        'Removes the affID from the end of the extOrderID
        Dim a As Array = Split(s, "_")
        Return a(0)
    End Function
    Protected Function showOrderID(ByVal extID As Integer, ByVal extUserOrderID As Object, ByVal ebPrefix As Object, ByVal ebOrderID As Object, ByVal ebCountryCode As Object) As String
        Dim result As String
        If extID = 0 Then
            'result = ebPrefix.ToString & "/" & ebOrderID.ToString & UCase(ebCountryCode.ToString)
            result = ebOrderID.ToString
        Else
            result = Parse(extUserOrderID.ToString)
        End If
        Return result
    End Function
    Protected Function showDate(ByVal d As Object)
        Dim result As String = "Unknown"
        If Not IsDBNull(d) Then
            result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        End If
        Return result
    End Function



    '######################## Transaction Functions ########################
    Protected Sub btnTransaction_click(ByVal sender As Object, ByVal e As EventArgs)
        'Check that a user has been selected form one of the dropdowns
        If userSelected() Then
            'Clear any previous values
            clearErrors()
            'Make visible
            tblTransaction.Visible = True
        Else
            lblTransactionUserError.Text = "You must make a selection from one of the dropdowns before making a transaction."
        End If
    End Sub
    Protected Sub radType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If radType.SelectedValue = "7" Or radType.SelectedValue = "8" Then
            'Make the checkboxes visible
            gvStatement.Columns(_gvStatement_checkPos).Visible = True
        Else
            'Hide the checkboxes
            gvStatement.Columns(_gvStatement_checkPos).Visible = False
        End If
    End Sub
    Protected Sub btnTransfer_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim si As New siteInclude
        Dim cr As Decimal = 0
        Dim dr As Decimal = 0
        Dim prefix As String = ""
        Dim actionID As Integer = CType(radType.SelectedValue, Integer)
        Dim affStatementID As Integer
        Dim paymentID As Integer
        Dim bTickboxesNeedTesting As Boolean = False
        Select Case actionID
            Case _addCredit
                cr = CType(txtAmount.Text, Decimal)
                prefix = "CR"
            Case _addDebit
                dr = CType(txtAmount.Text, Decimal)
                prefix = "DR"
            Case _addPaymentIn
                cr = CType(txtAmount.Text, Decimal)
                prefix = "PI"
                bTickboxesNeedTesting = True
            Case _addPaymentOut
                dr = CType(txtAmount.Text, Decimal)
                prefix = "PO"
                bTickboxesNeedTesting = True
        End Select

        If IsDate(txtDate.Text) Then
            If atLeastOneCheckboxHasBeenTicked() Or (Not bTickboxesNeedTesting) Then
                lblTransactionUserError.Text = ""
                'Add to affiliate statement (returns pk)
                Try
                    affStatementID = si.affAddToStatementDistBuyingWithCurrency(getSelectedID, cr, dr, 0, 0, actionID, prefix, txtCheque.Text, txtReason.Text, drpCurrency.SelectedValue, FormatDateTime(txtDate.Text, DateFormat.LongDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime))
                Catch ex As Exception

                End Try

                'Add to affilaiteStatementPayment (and return pk)
                paymentID = insertStatementPayment(affStatementID)
                'Add paymentID to all checked statement entries (as paymentRef)
                linkToPayment(paymentID, prefix)
                'Rebind gridview to show changes
                gvStatement.DataBind()
                'Hide checkboxes
                gvStatement.Columns(_gvStatement_checkPos).Visible = False
                'Hide transaction table
                tblTransaction.Visible = False
            Else
                lblTransactionUserError.Text = lblTransactionUserError.Text & "You must tick at least one box."
            End If
        Else
            lblTransactionUserError.Text = lblTransactionUserError.Text & "Invalid Transaction Date entered."
        End If
    End Sub
    Protected Function insertStatementPayment(ByVal affStatementID) As Integer
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
            lblTransactionUserError.Text = "Error while storing the payment reference.<br>" & ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return pk
    End Function
    Protected Function userSelected()
        Dim result As Boolean = True
        If drpAff.SelectedIndex = 0 And drpDist.SelectedIndex = 0 Then result = False
        Return result
    End Function
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
                        oCmd = New SqlCommand("procAffiliateStatementByIDPaymentUpdate", oConn)
                        With oCmd
                            .CommandType = CommandType.StoredProcedure
                            .Parameters.Add(New SqlParameter("@statementID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@linkedPrefix", SqlDbType.VarChar, 5))
                            .Parameters.Add(New SqlParameter("@linkedRef", SqlDbType.Int))
                            .Parameters("@statementID").Value = CType(gvStatement.DataKeys(row.RowIndex).Value, Integer)
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
End Class
