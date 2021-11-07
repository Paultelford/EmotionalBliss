Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_despatchNotePopup
    Inherits System.Web.UI.Page
    Private Const _dvOrder_userOrderCodePos As Integer = 0
    Private Const _dvOrder_weightRow As Integer = 3
    Private Const _gvItems_productNamePos As Integer = 4

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        If LCase(Session("EBAffEBDistributorCountryCode")) = "nl" Or LCase(Session("EBAffEBDistributorCountryCode")) = "be" Then Server.Transfer("despatchNotePopupnl.aspx?id=" & Request.QueryString("id"))
    End Sub
    Protected Sub gvItems_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'remove 1 '<br />' for each item in the gridview, this keeps the gridviews height static no matter how many items it has
        For Each row As GridViewRow In gvItems.Rows
            'lblSpacer.Text = Left(lblSpacer.Text, Len(lblSpacer.Text) - 6)
            'Show 'ONLY@ product in bold
            If InStr(LCase(row.Cells(_gvItems_productNamePos).Text), "only") Then row.Font.Bold = True
        Next
        'Response.Write(gvItems.Rows.Count)
    End Sub
    Protected Sub dv_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Hide the 1st colum (headers)
        Dim dv As DetailsView = CType(sender, DetailsView)
        dv.HeaderRow.Visible = False
        For Each row As DetailsViewRow In dv.Rows
            If row.Cells(1).Text = "@@" Then row.Visible = False
        Next
        'Get additional delivery instructions
        'Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionstring").ConnectionString)
        'Dim oCmd As New SqlCommand("", oConn)
        'Dim lblHidDelivery As Label = dv.Rows(1).FindControl("lblHidDelivery")
        'lblDelivery.Text = lblHidDelivery.Text
    End Sub
    Protected Function showTotal(ByVal goods As Object, ByVal shipping As Object, ByVal vat As Object) As Decimal
        Dim g As Decimal = 0
        Dim s As Decimal = 0
        Dim v As Decimal = 0
        If Not IsDBNull(goods) Then g = CType(goods, Decimal)
        If Not IsDBNull(shipping) Then s = CType(shipping, Decimal)
        If Not IsDBNull(vat) Then v = CType(vat, Decimal)
        Return FormatNumber(g + s + v, 2)
    End Function
    Protected Sub litDate_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lit As Literal = CType(sender, Literal)
        lit.Text = Day(Now()) & "/" & Month(Now()) & "/" & Year(Now())
    End Sub
    Protected Sub dvOrder_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblOrderType As Label = dvOrder.FindControl("lblOrderType")
        If LCase(lblOrderType.Text) = "affcc" Or LCase(lblOrderType.Text) = "affaccount" Then dvCosts.Visible = False

    End Sub
    Protected Function showCurrency() As String
        Return Session("EBAffCurrencySign")
    End Function
End Class
