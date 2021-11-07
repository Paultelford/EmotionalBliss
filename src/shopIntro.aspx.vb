Imports System.Data

Partial Class shop_shopIntro
    Inherits BasePage
    Protected _lastDeptID As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            loadDBResources()
            bindProducts()
            'Low stock Warning
            'If Not (LCase(Session("EBShopCountry")) = "nl" Or LCase(Session("EBShopCountry")) = "be") Then lblNote.Text = "Please note: due to the high level of demand we are currently workings to a 10 working days delivery from receipt of order. We apologise for any inconvenience and thank you for your support."
        End If
    End Sub

    'Subs
    Protected Sub loadDBResources()
        litShopText.Text = getDBResourceString("litShopText")
    End Sub
    Protected Sub bindProducts()
        'Get list of departments and all products within that department that are set in POS table as showOnIntro=True
        Dim dt As New DataTable
        Try
            Dim param() As String = New String() {"@countryCode"}
            Dim paramValue() As String = New String() {Session("EBShopCountry")}
            Dim paramType() As SqlDbType = New SqlDbType() {SqlDbType.VarChar}
            Dim paramSize() As Integer = New Integer() {5}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procProductOnSaleWithDeptByCountryCodeSelect")
            Dim tbl As New Table
            tbl.Width = Unit.Percentage(100)
            Dim tRow As TableRow
            Dim tCell As TableCell
            Dim lit As Literal
            Dim lnk As HyperLink
            Dim iCol As Integer = 0
            For Each row As DataRow In dt.Rows
                If _lastDeptID <> CInt(row("deptID")) Then
                    If Not tRow Is Nothing Then tbl.Rows.Add(tRow)
                    'New section - Line break
                    tRow = New TableRow
                    tCell = New TableCell
                    tCell.ColumnSpan = 2
                    tCell.Text = row("deptName") & "<br><div id='DashedLineHorizontal'></div>"
                    tRow.Cells.Add(tCell)
                    tbl.Rows.Add(tRow)
                    tRow = New TableRow
                    _lastDeptID = CInt(row("deptID"))
                    iCol = 0
                End If
                tCell = New TableCell
                tCell.HorizontalAlign = HorizontalAlign.Center
                lit = New Literal
                lnk = New HyperLink
                lit.Text = "<table width='230' border='0' cellpadding='0' cellspacing='0'><tr><td><h4>" & ShortName(row("salename")) & "</h4></td><td width='110' align='right'><a href='/shop/product.aspx?id=" & row("id") & "' class='MoreInfoRollover'></a></td></tr></table>"
                lnk.ImageUrl = "~/images/products/" & row("saleImageName")
                lnk.NavigateUrl = "~/shop/product.aspx?id=" & row("id")
                If Request.QueryString("m") <> "" Then lnk.NavigateUrl = lnk.NavigateUrl & "&m=" & Request.QueryString("m")
                lnk.ToolTip = row("saleName")
                tCell.Controls.Add(lnk)
                tCell.Controls.Add(lit)
                tRow.Cells.Add(tCell)
                iCol = iCol + 1
                If iCol = 2 Then
                    'New line
                    tbl.Rows.Add(tRow)
                    tRow = New TableRow
                    iCol = 0 'Reset iCol
                End If
            Next
            If Not tRow Is Nothing Then tbl.Rows.Add(tRow)
            panTest.Controls.Add(tbl)
        Catch ex As Exception
            siteInclude.addError("", "(); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try

    End Sub

    'Functions 
    Protected Function ShortName(ByVal s As String) As String
        Dim a As Array = Split(s, "-")
        Return a(0)
    End Function
End Class
