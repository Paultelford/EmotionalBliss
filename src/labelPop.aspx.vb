
Partial Class labelPop
    Inherits System.Web.UI.Page
    Private Const _cellWidth As Integer = 190
    Private Const _cellHeight As Integer = 19

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim tCell As New TableCell
        Dim tRow As New TableRow
        Dim num As String = Request.QueryString("num")
        For iRow As Integer = 1 To 48
            tRow = New TableRow
            For iCol As Integer = 1 To 4
                tCell = New TableCell
                tCell.Text = num
                tCell.Width = _cellWidth
                tCell.Height = _cellHeight
                tCell.HorizontalAlign = HorizontalAlign.Center
                tCell.VerticalAlign = VerticalAlign.Middle
                tRow.Cells.Add(tCell)
            Next
            tbl.Rows.Add(tRow)
        Next
    End Sub
End Class
