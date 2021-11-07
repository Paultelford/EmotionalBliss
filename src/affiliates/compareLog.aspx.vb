
Partial Class affiliates_compareLog
    Inherits BasePage
    Private Const _gvList_orderIDPos As Integer = 0
    Private Const _gvList_AmountPos As Integer = 4
    Private Const _gvList_scanAmountPos As Integer = 6
    Private Const _gvList_protxAmountPos As Integer = 9
    Private Const _gvList_discrepencyPos As Integer = 13

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
    End Sub
    Protected Function isFound(ByVal o As Object)
        Dim result As Boolean = True
        If IsDBNull(o) Then result = False
        Return result
    End Function
    Protected Sub gvList_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim chkSales As CheckBox
        Dim chkProtx As CheckBox
        Dim chkMissing As CheckBox
        Dim lblScannedDate As Label
        Dim lblProtxDate As Label
        Dim lblID As Label
        Dim lblTxCode As label
        Dim currentID As Integer
        Dim lastID As Integer = 0
        Dim bColor As Boolean = True
        Dim orderTotal As Decimal = 0
        Dim scanTotal As Decimal = 0
        Dim protxTotal As Decimal = 0
        Dim scanOrderTotal As Decimal = 0
        Dim protxOrderTotal As Decimal = 0
        Dim discrepencyTotal As Decimal = 0
        For Each row As GridViewRow In gvList.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblID = row.FindControl("lblID")
                currentID = CInt(lblID.Text)
                lblScannedDate = row.FindControl("lblSales")
                lblProtxDate = row.FindControl("lblProtxDate")
                lblTxCode = row.FindControl("lblTxCode")
                If Trim(LCase(lblScannedDate.Text)) = "scanned" Then lblScannedDate.Text = ""
                If Trim(LCase(lblProtxDate.Text)) = "taken" Then lblProtxDate.Text = ""
                'If protx entry is a refund, then change the figure in the protxAmount column to a negative
                If lCase(lblTxCode.Text) = "ebref" Then
                    row.cells(_gvList_protxAmountPos).Text = CDec(row.cells(_gvList_protxAmountPos).Text * -1)
                End If
                If currentID = lastID Then
                    If bColor Then
                        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#e6effe")
                    Else
                        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
                    End If
                    If IsNumeric(row.Cells(_gvList_scanAmountPos).Text) Then scanOrderTotal = scanOrderTotal + CDec(row.Cells(_gvList_scanAmountPos).Text)
                    If IsNumeric(row.Cells(_gvList_protxAmountPos).Text) Then protxOrderTotal = protxOrderTotal + CDec(row.Cells(_gvList_protxAmountPos).Text)
                Else
                    'If amounts dont match, then hilight the previous row
                    If scanOrderTotal <> protxOrderTotal Then
                        gvList.Rows(row.RowIndex - 1).Cells(_gvList_discrepencyPos).BackColor = Drawing.Color.LightSalmon
                        gvList.Rows(row.RowIndex - 1).Cells(_gvList_discrepencyPos).Text = FormatNumber(scanOrderTotal - protxOrderTotal, 2)
                        discrepencyTotal = discrepencyTotal + (scanOrderTotal - protxOrderTotal)
                    End If
                    scanOrderTotal = 0
                    protxOrderTotal = 0
                    bColor = Not bColor
                    If bColor Then
                        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#e6effe")
                    Else
                        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffffff")
                    End If
                    'compare current order totals
                    If IsNumeric(row.Cells(_gvList_scanAmountPos).Text) Then scanOrderTotal = CDec(row.Cells(_gvList_scanAmountPos).Text)
                    If IsNumeric(row.Cells(_gvList_protxAmountPos).Text) Then protxOrderTotal = CDec(row.Cells(_gvList_protxAmountPos).Text)
                End If
                'Keep track of totals
                If IsNumeric(row.Cells(_gvList_AmountPos).Text) Then orderTotal = orderTotal + CDec(row.Cells(_gvList_AmountPos).Text)
                If IsNumeric(row.Cells(_gvList_scanAmountPos).Text) Then scanTotal = scanTotal + CDec(row.Cells(_gvList_scanAmountPos).Text)
                If IsNumeric(row.Cells(_gvList_protxAmountPos).Text) Then protxTotal = protxTotal + CDec(row.Cells(_gvList_protxAmountPos).Text)
                lastID = currentID
                'Old code
                If False Then
                    chkSales = row.FindControl("chkSales")
                    chkProtx = row.FindControl("chkProtx")
                    chkMissing = row.FindControl("chkMissing")
                    If Not (chkSales.Checked And chkProtx.Checked) Then row.Cells(_gvList_orderIDPos).Font.Bold = True
                    If chkMissing.Checked Then row.Cells(10).BackColor = Drawing.Color.LightSalmon
                End If
            End If
        Next
        'Show totals
        gvList.FooterRow.Cells(_gvList_discrepencyPos).Text = FormatNumber(discrepencyTotal, 2)
        gvList.FooterRow.Cells(_gvList_scanAmountPos).Text = FormatNumber(scanTotal, 2)
        gvList.FooterRow.Cells(_gvList_protxAmountPos).Text = FormatNumber(protxTotal, 2)
    End Sub
    Protected Function showDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d.ToString(), DateFormat.ShortDate)
        Return result
    End Function
    Protected Function showAmount(ByVal c As Object)
        Dim result As String = ""
        If Not IsDBNull(c) Then result = FormatNumber(c.ToString(), 2)
        Return result
    End Function
End Class
