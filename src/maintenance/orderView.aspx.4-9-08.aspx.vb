Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_orderView
    Inherits System.Web.UI.Page
    Private Const _gvBasket_qtyPos As Integer = 4
    Private Const _gvBasket_stockPos As Integer = 8
    Private Const _gvBasket_editPos As Integer = 13
    Private Const _dvCosts_shippingPos As Integer = 2

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        'If orderstatus is 'Paid' then remove the edit facility
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        If LCase(lblStatus.Text) = "paid" Then
            'hide edit buttons
            Dim btnEditShipping As LinkButton = fvOrder.FindControl("btnEditShipping")
            btnEditShipping.Visible = False
            Dim gv As GridView = fvOrder.FindControl("gvBasket")
            gv.Columns(_gvBasket_editPos).Visible = False
            gv.Width = Unit.Percentage(95)
        End If
    End Sub
    Protected Sub fvOrder_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim hidUseBillAdd As HiddenField = fvOrder.FindControl("hidUseBillAdd")
        Dim dvBill As DetailsView = fvOrder.FindControl("dvBillAdd")
        Dim dvShip As DetailsView = fvOrder.FindControl("dvShipAdd")
        If CType(hidUseBillAdd.Value, Boolean) Then
            For Each row As DetailsViewRow In dvBill.Rows
                dvShip.Rows(row.RowIndex).Cells(1).Text = row.Cells(1).Text 'Assign new value to Shipping Address Row
            Next
        End If
        hideAddressBlanks(dvBill) 'Remove blank address fields
        hideAddressBlanks(dvShip) 'Remove blank address fields
        'If No Affiliate, the show 'None'
        Dim lblAff As Label = fvOrder.FindControl("lblAffiliate")
        If lblAff.Text = "" Then lblAff.Text = "None"
        'If order is not an NL account order, then hide the account table row
        Dim lblAccount As Label = fvOrder.FindControl("lblAccount")
        Dim tr As Object = fvOrder.FindControl("trAccount")
        Dim trCard As Object = fvOrder.FindControl("trCard")
        If lblAccount.Text = "" Then
            tr.visible = False
            trCard.Visible = True
        Else
            tr.visible = True
            trCard.Visible = False
        End If
        'Hide the header row on the Address DetailsView's
        dvBill.HeaderRow.Visible = False
        dvShip.HeaderRow.Visible = False
        'If distributor status is 'placed', then show the Take Payment button
        Dim lblStatus As Label = fvOrder.FindControl("lblStatus")
        Dim btnPaymentComplete As Button = fvOrder.FindControl("btnPaymentComplete")
        If LCase(lblStatus.Text) = "placed" Then btnPaymentComplete.Visible = True
        'Bind shipping
        'bindShipping()
        'showTotal()
    End Sub
    Protected Function showDate(ByVal d As Date) As String
        Return FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
    End Function
    Protected Sub hideAddressBlanks(ByRef dv As DetailsView)
        For Each row As DetailsViewRow In dv.Rows
            If Trim(row.Cells(1).Text) = "" Or row.Cells(1).Text = "*" Then row.Visible = False
        Next
    End Sub
    Protected Function showOutstanding(ByVal quantity As Object, ByVal despatched As Object) As String
        Dim qty As Integer = 0
        Dim qtyDespatched As Integer = 0
        If Not IsDBNull(quantity) Then qty = CInt(quantity.ToString)
        If Not IsDBNull(despatched) Then qtyDespatched = CInt(despatched.ToString)
        Return CStr(qty - qtyDespatched)
    End Function
    Protected Function showDespatchQty(ByVal outstanding As Integer) As String
        Dim gv As GridView = fvOrder.FindControl("gvBasket")
        Dim stock As Integer = CInt(gv.Rows(gv.Rows.Count).Cells(_gvBasket_stockPos).Text)
        Dim result As Integer = outstanding
        If stock < outstanding Then result = stock
        Return CStr(result)
    End Function
    Protected Sub gvBasket_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim result As Integer = 0
            Dim txtDespatch As TextBox = e.Row.FindControl("txtDespatch")
            Try
                Dim lblOutstanding As Label = e.Row.FindControl("lblOutstanding")
                Dim iOutstanding As Integer = CInt(lblOutstanding.Text)
                Dim iStock As Integer = CInt(e.Row.Cells(_gvBasket_stockPos).Text)
                result = iOutstanding
                If iStock < iOutstanding Then result = iStock
            Catch ex As Exception
            End Try
            txtDespatch.Text = CStr(result)
        End If
    End Sub
    Protected Sub btnPaymentComplete_click(ByVal sender As Object, ByVal e As EventArgs)
        'Update order to show as 'paid' on the Peartree side
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDDistStatusUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters("@orderID").Value = Request.QueryString("id")
            .Parameters("@status").Value = "Paid"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        fvOrder.DataBind()
    End Sub
    Protected Sub bindShipping()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDCostsSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim gv As GridView = fvOrder.FindControl("gvBasket")
        Dim lblShippingVat As Label = gv.FooterRow.FindControl("lblShippingVat")
        Dim lblShipping As Label = gv.FooterRow.FindControl("lblShipping")
        Dim txtShippingVat As TextBox = gv.FooterRow.FindControl("txtShippingVat")
        Dim txtShipping As TextBox = gv.FooterRow.FindControl("txtShipping")
        Dim lblVat As Label = gv.FooterRow.FindControl("lblVat")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim row As DataRow = ds.Tables(0).Rows(0)
                lblShippingVat.Text = FormatNumber(row("shippingVatRate"), 1)
                lblShipping.Text = FormatNumber(row("shipping"), 2)
                lblVat.Text = FormatNumber(row("goodsVat"), 1)
                txtShippingVat.Text = lblShippingVat.Text
                txtShipping.Text = lblShipping.Text
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub showTotal()
        Dim gv As GridView = fvOrder.FindControl("gvBasket")
        Dim total As Decimal = 0
        Dim goodsVat As Decimal = 0
        Dim shipping As Decimal = 0
        Dim qty As Integer = 0
        Dim lblShipping As Label = gv.FooterRow.FindControl("lblShipping")
        Dim lblShippingVat As Label = gv.FooterRow.FindControl("lblShippingVat")
        Dim lblItemVatRate As Label
        Dim lblUnitPrice As Label
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblItemVatRate = row.FindControl("lblItemVatRate")
                qty = row.Cells(_gvBasket_qtyPos).Text
                lblUnitPrice = row.FindControl("lblUnitPrice")
                total = total + lblUnitPrice.Text * qty
                goodsVat = goodsVat + FormatNumber((lblUnitPrice.Text / 100) * lblItemVatRate.Text, 2) * qty
            End If
        Next
        Dim lblTotal As Label = gv.FooterRow.FindControl("lblTotal")
        shipping = lblShipping.Text * (1 + (lblShippingVat.Text / 100))
        lblTotal.Text = FormatNumber(total + shipping + goodsVat, 2)
    End Sub
    Protected Sub gvBasket_rowUpdated(ByVal sender As Object, ByVal e As GridViewUpdatedEventArgs)
        'Item row has been updated in shopOrderItem table.
        'Calculate new goodsTotal/goodsVatTotal and update shopOrder table
        Dim dv As DetailsView = fvOrder.FindControl("dvCosts")
        calcTotal()
        dv.DataBind()
    End Sub
    Protected Sub calcTotal()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderItemByOrderIDTotalsSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim gv As GridView = fvOrder.FindControl("gvBasket")
        Dim dv As DetailsView = fvOrder.FindControl("dvCosts")
        Dim itemPriceIncVat = 0
        Dim goods As Decimal = 0
        Dim goodsVat As Decimal = 0
        Dim lblShipping As Label = dv.FindControl("lblShipping")
        Dim shipping As Decimal = CDec(lblShipping.Text)
        Dim shippingVatRate As Decimal = hidVatRate.Value
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    itemPriceIncVat = FormatNumber(row("price") * (1 + (row("vatRate") / 100)), 2)
                    goods = goods + row("price") * row("qty")
                    goodsVat = goodsVat + FormatNumber((itemPriceIncVat - row("price")) * row("qty"), 2)
                Next
                'Update shopOrder
                oCmd = New SqlCommand("procShopOrderByIDCostsUpdate2", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@goods", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@goodsVat", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@shipping", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@shippingVatRate", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@shippingTotal", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@orderTotal", SqlDbType.Decimal))
                    .Parameters("@ID").Value = Request.QueryString("id")
                    .Parameters("@goods").Value = goods
                    .Parameters("@goodsVat").Value = goodsVat
                    .Parameters("@shipping").Value = shipping
                    .Parameters("@shippingVatRate").Value = hidVatRate.Value
                    .Parameters("@shippingTotal").Value = FormatNumber(shipping + ((shippingVatRate / 100) * shipping))
                    .Parameters("@orderTotal").Value = goods + goodsVat + oCmd.Parameters("@shippingTotal").Value
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    Throw ex
                End Try
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("maintenance/orderView.aspx.vb", "calcTotal(orderID=" & Request.QueryString("id") & "); " & ex.ToString)
            si = Nothing
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub gvBasket_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Update the VatRate hidden field with the vate rate from any row
        Dim gv As GridView = CType(sender, GridView)
        Dim lbl As Label
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lbl = row.FindControl("lblItemVatRate")
                hidVatRate.Value = lbl.Text
                Exit For
            End If
        Next
    End Sub
    Protected Sub btnEditShipping_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim dv As DetailsView = fvOrder.FindControl("dvCosts")
        Dim btnEditShipping As LinkButton = CType(sender, LinkButton)
        btnEditShipping.Visible = False
        dv.ChangeMode(DetailsViewMode.Edit)
    End Sub
    Protected Sub txtShipping_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Update shopOrder with the new shipping and shipingRate values. (By default Distributor orders have 0% for the shipping rate, so running this update will grab the current vatrate and update the order with it)
        Dim txt As TextBox = CType(sender, TextBox)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDShippingUpdate", oConn)
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@shipping", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingVatRate", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@shippingTotal", SqlDbType.Decimal))
            .Parameters("@id").Value = Request.QueryString("id")
            .Parameters("@shipping").Value = CDec(txt.Text)
            .Parameters("@shippingVatRate").Value = CDec(hidVatRate.Value)
            .Parameters("@shippingTotal").Value = FormatNumber((CDec(txt.Text) / 100) * CDec(hidVatRate.Value), 2)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            Dim lblError As Label = fvOrder.FindControl("lblError")
            lblError.Text = "An error occured while updating the shipping."
            Dim si As New siteInclude
            si.addError("maintenance/orderView.aspx", "txtShipping_textChanged(id=" & Request.QueryString("id") & "); " & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            Dim btnEditShipping As LinkButton = fvOrder.FindControl("btnEditShipping")
            btnEditShipping.Visible = True
            Dim dv As DetailsView = fvOrder.FindControl("dvCosts")
            dv.ChangeMode(DetailsViewMode.ReadOnly)
            dv.DataBind()
            calcTotal()
            dv.DataBind()
        End If
    End Sub
End Class
