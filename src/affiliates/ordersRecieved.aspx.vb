
Partial Class affiliates_ordersRecieved
    Inherits BasePage
    Private Const bDebug As Boolean = False
    Private Const _gvDetails_qtyPos As Integer = 2
    Private Const _gvOrders_ordersPos As Integer = 2
    Private Const _gvOrders_valuePos As Integer = 4
    Private Const _gvOrders_visitorsPos As Integer = 6
    Private Const _gvOrders_GvisitorsPos As Integer = 8
    Private Const _gvOrders_visitorsPerOrderPos As Integer = 10
    Private Const _gvOrders_GvisitorsPerOrderPos As Integer = 12

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
    End Sub

    'Page Events
    Protected Sub gvDetails_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim iTotal As Integer = 0
        Dim lblQty As Label
        Dim lblTotal As Label
        For Each row As GridViewRow In gvDetails.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblQty = row.FindControl("lblQty")
                iTotal = iTotal + CInt(lblQty.Text)
            End If
        Next
        If gvDetails.Rows.Count > 0 Then
            lblTotal = gvDetails.FooterRow.FindControl("lblTotal")
            lblTotal.Text = iTotal
        End If
    End Sub
    Protected Sub gvOrders_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim orders As Integer = 0
        Dim value As Decimal = 0
        Dim visitors As Integer = 0
        Dim gvisitors As Integer = 0
        Dim lblTotals As Label
        Dim lblGOrdersPerCustomer As Label
        Dim lblGVisitors As Label
        For Each row As GridViewRow In gvOrders.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If Not bDebug Then
                    If row.RowIndex <> gvOrders.EditIndex Then
                        lblTotals = row.FindControl("lblTotals")
                        lblGVisitors = row.FindControl("lblGVisitors")
                        lblGOrdersPerCustomer = row.FindControl("lblGOrdersPerCustomer")
                        orders = orders + CInt(row.Cells(_gvOrders_ordersPos).Text)
                        Try
                            value = value + CDec(right(lblTotals.Text, len(lblTotals.text) - 1))
                        Catch ex As exception
                        End Try
                        'visitors = visitors + CInt(row.Cells(_gvOrders_visitorsPos).Text)
                        gvisitors = gvisitors + CInt(lblGVisitors.Text)
                        'Replace the Google Visiters Per Order value of NaN with 0
                        If LCase(lblGOrdersPerCustomer.Text) = "nan" Then lblGOrdersPerCustomer.Text = "0.0"
                    End If
                End If
            End If
        Next
        If Not bDebug Then
            If gvOrders.Rows.Count > 0 Then
                'Show totals
                gvOrders.FooterRow.Cells(_gvOrders_ordersPos).Text = CStr(orders)
                gvOrders.FooterRow.Cells(_gvOrders_valuePos).Text = CStr(Session("EBAffCurrencySign") & FormatNumber(value, 2))
                'gvOrders.FooterRow.Cells(_gvOrders_visitorsPos).Text = CStr(visitors)
                'gvOrders.FooterRow.Cells(_gvOrders_visitorsPerOrderPos).Text = CStr(FormatNumber((visitors / orders), 2))
                gvOrders.FooterRow.Cells(_gvOrders_GvisitorsPos).Text = CStr(gvisitors)
                gvOrders.FooterRow.Cells(_gvOrders_GvisitorsPerOrderPos).Text = CStr(FormatNumber((gvisitors / orders), 2))
            End If
        End If

        If gvOrders.EditIndex <> -1 Then
            Dim ctrl As TextBox = gvOrders.Rows(gvOrders.EditIndex).FindControl("txtGVisitors")
            ScriptManager.RegisterStartupScript(gvOrders, Me.GetType, "onloader", "self.setTimeout(""focusEditBox('" & ctrl.ClientID & "')"",200);", True)
        End If
    End Sub

    'USer Events
    Protected Sub gvOrders_rowEdited(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        Dim ctrl As TextBox = gvOrders.Rows(e.NewEditIndex).FindControl("txtGVisitors")
        'ctrl = gvOrders.Rows(e.NewEditIndex).Cells(_gvOrders_GvisitorsPerOrderPos).Controls(0)
        ScriptManager.RegisterStartupScript(gvOrders, Me.GetType, "onloader", "self.setTimeout(""focusEditBox('" & ctrl.ClientID & "')"",200);", True)
    End Sub
End Class
