Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class affiliates_ordersReceived
    Inherits BasePage
    Private _content As ContentPlaceHolder
    Private Const _gvEBOrders_statusPos As Integer = 4
    Private Const _gvEBOrders_cancelBtnPos As Integer = 7
    Private Const _gvEXOrders_statusPos As Integer = 4
    Private Const _gvEXOrders_cancelBtnPos As Integer = 7

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _content = FindControl("ContentPlaceHolder1")
        Else
            Response.Redirect("default.aspx")
        End If
    End Sub
    Protected Sub gvEBOrders_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Get details from DB and show in pan3
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDViewSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim orderID As Integer = CType(gvEBOrders.DataKeys(gvEBOrders.SelectedIndex).Value, Integer)
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
                lblOrderDetails.Text = "<b>EB Order ID:</b> " & ds.Tables(0).Rows(0)("newOrderID") & UCase(Session("EBAffEBDistributorCountryCode"))
                lblOrderDate.Text = FormatDateTime(ds.Tables(0).Rows(0)("orderDate"), DateFormat.LongDate)
                'lblOrderTotal.Text = FormatNumber(ds.Tables(0).Rows(0)("goodsTotal") * (1 + ((ds.Tables(0).Rows(0)("goodsVatRate") / 100)))) 'Total = Goods Inc VAT
                'lblOrderTotal.Text = FormatNumber(ds.Tables(0).Rows(0)("goodsTotalInc") + (ds.Tables(0).Rows(0)("shippingTotal") * ((ds.Tables(0).Rows(0)("shippingVatRate") / 100) + 1)), 2)
                lblOrderTotal.Text = FormatNumber(ds.Tables(0).Rows(0)("orderTotal"), 2)
            End If

        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Make details visible
        pan3.Visible = True
        'Get dataset of goods ordered
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procShopOrderItemByIDSelect2", oConn)
        ds = New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = orderID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            gvItems.DataSource = ds
            gvItems.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        hidOrderType.Value = "eb"
        hidOrderID.Value = orderID
    End Sub
    Protected Sub gvEXOrders_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Get details from DB and show in pan3
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDistributorExtOrderByIDViewSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim orderID As Integer = CType(gvEXOrders.DataKeys(gvEXOrders.SelectedIndex).Value, Integer)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = orderID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblOrderDetails.Text = "<b>Ext Order ID:</b> " & formatExtOrderID(ds.Tables(0).Rows(0)("OrderID"))
                lblOrderDate.Text = FormatDateTime(ds.Tables(0).Rows(0)("orderDate"), DateFormat.LongDate)
                lblOrderTotal.Text = "Not yet known"
            End If

        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Make details visible
        pan3.Visible = True
        'Get dataset of goods ordered
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procDistributorExtOrderItemByIDSelect", oConn)
        ds = New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = orderID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            gvItems.DataSource = ds
            gvItems.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        hidOrderType.Value = "ex"
        hidOrderID.Value = orderID
    End Sub
    Protected Sub btnProcess_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtReceived As TextBox
        Dim lblOutstanding As Label
        Dim bError As Boolean = False
        'Loop through Received each entry made by the user.
        For Each Row As GridViewRow In gvItems.Rows
            'Validate the entry
            If Row.RowType = DataControlRowType.DataRow Then
                lblOutstanding = Row.FindControl("lblOutstanding")
                txtReceived = Row.FindControl("txtReceived")
                If txtReceived.Text = "" Then txtReceived.Text = "0"
                If Not IsNumeric(txtReceived.Text) Then
                    'Non numberic text entered
                    lblError.Text = "Invalid entry. Only numerical data is accepted."
                    bError = True
                Else
                    'Check for an entry that is more than the qty of outstanding items
                    If CType(lblOutstanding.Text, Integer) < CType(txtReceived.Text, Integer) Then
                        bError = True
                        lblError.Text = "Some Received quantities entered are greater than the Qty Outstanding."
                    End If
                End If
            End If
        Next
        If Not bError Then
            'no validation errors.
            Dim orderID As Integer
            Dim extOrderID As Integer
            Dim actionID As Integer
            Dim affProductBuyingID As HiddenField
            Select Case hidOrderType.Value
                Case "eb"
                    orderID = CType(hidOrderID.Value, Integer)
                    extOrderID = 0
                    actionID = 1
                Case "ex"
                    extOrderID = CType(hidOrderID.Value, Integer)
                    orderID = 0
                    actionID = 2
            End Select
            Dim oConn As SqlConnection
            Dim oCmd As SqlCommand
            Dim bDataUpdated As Boolean = False
            For Each row As GridViewRow In gvItems.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    txtReceived = row.FindControl("txtReceived")
                    affProductBuyingID = row.FindControl("hidProductBuyingID")
                    'Only add stock if non zero qty was entered
                    If CType(txtReceived.Text, Integer) > 0 Then
                        bDataUpdated = True 'The Order Status will be updated if this is true
                        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                        oCmd = New SqlCommand("procEBDistributorStockInsert", oConn)
                        With oCmd
                            .CommandType = CommandType.StoredProcedure
                            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@affProductBuyingID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@qtyAdd", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@extOrderID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
                            .Parameters("@affID").Value = Session("EBAffID")
                            .Parameters("@affProductBuyingID").Value = CType(affProductBuyingID.Value, Integer)
                            .Parameters("@qtyAdd").Value = CType(txtReceived.Text, Integer)
                            .Parameters("@orderID").Value = orderID
                            .Parameters("@extOrderID").Value = extOrderID
                            .Parameters("@actionID").Value = actionID
                        End With
                        Try
                            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                            oCmd.ExecuteNonQuery()
                        Catch ex As Exception
                            Response.Write(ex)
                            Response.End()
                        Finally
                            oCmd.Dispose()
                            oConn.Dispose()
                        End Try
                    End If
                End If
            Next
            'Now update the orders Status
            If bDataUpdated Then
                oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                If orderID > 0 Then
                    'EB Order
                    oCmd = New SqlCommand("procShopOrderByOrderByIDStatusUpdate", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
                        .Parameters("@ID").Value = orderID
                    End With
                Else
                    'Ext Order
                    oCmd = New SqlCommand("procDistributorExtOrderByIDStatusUpdate", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
                        .Parameters("@ID").Value = extOrderID
                    End With
                End If
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    Response.Write(ex)
                    Response.End()
                Finally
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            End If
            'Final step - Hide items (panel 3) and update either EB Order Panel, or Ext Order Panel to reflect the orders new Status
            If orderID > 0 Then
                'EB Order
                gvEBOrders.DataBind()
            Else
                'Ext Order
                gvEXOrders.DataBind()
            End If
            pan3.Visible = False
        End If
    End Sub
    Protected Sub gvItems_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblOutstanding As Label
        Dim txt As TextBox
        For Each row As GridViewRow In gvItems.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'If orderQty and receivedQty are equal, then hide the input box.
                lblOutstanding = row.FindControl("lblOutstanding")
                txt = row.FindControl("txtReceived")
                If CType(lblOutstanding.Text, Integer) = 0 Then txt.Visible = False
            End If
        Next
    End Sub
    Protected Function subtract(ByVal a As Object, ByVal b As Object)
        Return CType(a.ToString, Integer) - CType(b.ToString, Integer)
    End Function
    Protected Function formatExtOrderID(ByVal id As String) As String
        Dim array As Array = Split(id.ToString, "_")
        Return array(0)
    End Function
    Protected Function calcTotal(ByVal goods As Object, ByVal shipping As Object, ByVal shippingVatRate As Object) As String
        Dim result As String
        Dim g As Decimal
        Dim s As Decimal
        Dim sv As Decimal
        If Not (IsDBNull(goods) Or IsDBNull(shipping) Or IsDBNull(shippingVatRate)) Then
            g = CType(goods.ToString, Decimal)
            s = CType(shipping.ToString, Decimal)
            sv = CType(shippingVatRate, Decimal)
            result = FormatNumber(g + (shipping * ((shippingVatRate / 100) + 1)), 2)
        Else
            result = "unknown"
        End If
        Return CType(result, String)
    End Function
    Protected Sub gvEBOrders_rowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByOrderByIDStatussUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@distributorStatus", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@orderStatus", SqlDbType.VarChar, 20))
            .Parameters("@id").Value = gvEBOrders.DataKeys(e.NewEditIndex).Value
            .Parameters("@distributorStatus").Value = "Cancelled"
            .Parameters("@orderStatus").Value = "Cancelled"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "A SQL error occured while Cancelling the order. Please try again later."
            lblErrorDetail.text = ex.ToString
            Dim si As New siteInclude
            si.addError("affiliates/ordersRecieved.aspx.vb", "gvEBOrders_rowEditing;" & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        e.Cancel = True
        gvEBOrders.DataBind()
    End Sub
    Protected Sub gvEBOrders_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If LCase(e.Row.Cells(_gvEBOrders_statusPos).Text) = "despatched" Or LCase(e.Row.Cells(_gvEBOrders_statusPos).Text) = "cancelled" Or LCase(e.Row.Cells(_gvEBOrders_statusPos).Text) = "partdespatched" Or LCase(e.Row.Cells(_gvEBOrders_statusPos).Text) = "complete" Then e.Row.Cells(_gvEBOrders_cancelBtnPos).Controls.Clear()
        End If
    End Sub
    Protected Sub gvEXOrders_rowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDistributorOrderByByIDStatussUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters("@id").Value = gvEXOrders.DataKeys(e.NewEditIndex).Value
            .Parameters("@status").Value = "Cancelled"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "A SQL error occured while Cancelling the order. Please try again later."
            lblErrorDetail.text = ex.ToString
            Dim si As New siteInclude
            si.addError("affiliates/ordersRecieved.aspx.vb", "gvEXOrders_rowEditing;" & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        e.Cancel = True
        gvEXOrders.DataBind()
    End Sub
    Protected Sub gvEXOrders_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If LCase(e.Row.Cells(_gvEXOrders_statusPos).Text) = "cancelled" Or LCase(e.Row.Cells(_gvEXOrders_statusPos).Text) = "partcomplete" Or LCase(e.Row.Cells(_gvEXOrders_statusPos).Text) = "complete" Then e.Row.Cells(_gvEXOrders_cancelBtnPos).Controls.Clear()
        End If
    End Sub
End Class
