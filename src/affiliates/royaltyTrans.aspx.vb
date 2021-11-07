
Partial Class affiliates_royaltyTrans
    Inherits BasePage
    Private Const _gvStatement_creditPos As Integer = 6
    Private Const _gvStatement_debitPos As Integer = 8
    Private Const _gvStatement_orderIDPos As Integer = 2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            If Session("EBAffEBDistributor") Or Session("EBAffEBUser") Then drpEarners.Visible = True
            If Request.QueryString("aid") <> "" Then
                Try
                    drpEarners.SelectedValue = Request.QueryString("aid")
                Catch ex As Exception
                End Try
            End If
        End If
    End Sub
    Protected Sub sqlStatement_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        'If user is distributor/EBUser then use the affID from the dropdown. Otherwise use affilaites ID
        Dim affID As Integer = 0
        If Session("EBAffEBDistributor") Or Session("EBAffEBUser") Then
            affID = drpEarners.SelectedValue
        Else
            affID = Session("EBAffID")
        End If
        e.Command.Parameters(0).Value = affID
    End Sub
    Protected Sub drpEarners_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvStatement.DataBind()
    End Sub
    Protected Sub gvStatement_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblActionID As Label
        Dim lblBalance As Label
        Dim balance As Decimal = 0
        For Each row As GridViewRow In gvStatement.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblActionID = row.FindControl("lblActionID")
                lblBalance = row.FindControl("lblBalance")
                Select Case CInt(lblActionID.Text)
                    Case 0
                        'Opening balance
                        balance = CDec(row.Cells(_gvStatement_creditPos).Text) - CDec(row.Cells(_gvStatement_debitPos).Text)
                        lblBalance.Text = Session("EBAffCurrencySign") & FormatNumber(balance, 2)
                        'Hide credit and debit fields
                        row.Cells(_gvStatement_creditPos).Text = ""
                        row.Cells(_gvStatement_debitPos).Text = ""
                        row.Cells(_gvStatement_orderIDPos).Text = ""
                    Case Else
                        balance = balance + CDec(row.Cells(_gvStatement_creditPos).Text)
                        balance = balance - CDec(row.Cells(_gvStatement_debitPos).Text)
                        lblBalance.Text = Session("EBAffCurrencySign") & FormatNumber(balance, 2)
                        'Add currency signs to credit/debit columns
                        row.Cells(_gvStatement_creditPos).Text = Session("EBAffCurrencySign") & row.Cells(_gvStatement_creditPos).Text
                        row.Cells(_gvStatement_debitPos).Text = Session("EBAffCurrencySign") & row.Cells(_gvStatement_debitPos).Text
                End Select
            End If
        Next
    End Sub
End Class
