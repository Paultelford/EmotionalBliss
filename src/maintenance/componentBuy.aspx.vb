Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentBuy
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreLoad
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            If Not Page.IsPostBack Then
                'Set date in table
                lblDate.Text = FormatDateTime(Now(), DateFormat.LongDate)
                'get orderID(purchase order)
                lblOrderID.Text = getNewOrderID()
                'Clear error
                lblError.Text = ""
                BindAddressDropDowns()
            End If
        End If
    End Sub
    Protected Function getNewOrderID()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentOrderSelectID", oConn)
        Dim result As Integer
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        oCmd.CommandType = CommandType.StoredProcedure
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0)("compOrderID")
            Else
                result = 0
            End If

        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oConn.Dispose()
            oCmd.Dispose()
        End Try
        Return result + 1
    End Function
    Protected Sub drpMan_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Set Currency dropdown with selected manufacturers default currency
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procManufacturerByIdSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@manID", SqlDbType.Int))
            .Parameters("@manID").Value = drpMan.SelectedValue
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'drpCurrency.ClearSelection()
                'For Each item As ListItem In drpCurrency.Items
                'If Convert.ToInt32(item.Value) = Convert.ToInt32(ds.Tables(0).Rows(0)("currencyID").ToString) Then item.Selected = True
                'Next
                lblCurrencyID.Text = ds.Tables(0).Rows(0)("currencyID")
                lblCurrency.Text = ds.Tables(0).Rows(0)("currencyCode")
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
    End Sub
    Protected Sub btnCompleteOrder_click(ByVal sender As Object, ByVal e As EventArgs)
        'Do some validating..
        Dim bProductSelected As Boolean = False
        Dim bError As Boolean = False
        If Not (Len(txtDelivery.text) > 0 And IsNumeric(txtDelivery.text)) Then bError = True
        If bError Then lblError.Text = "Invalid delivery amount."
        If Not bError Then
            If Convert.ToInt32(drpItem1.Text) > 0 Or Convert.ToInt32(drpItem2.Text) > 0 Or Convert.ToInt32(drpItem3.Text) > 0 Or Convert.ToInt32(drpItem4.Text) > 0 Or Convert.ToInt32(drpItem5.Text) > 0 Or Convert.ToInt32(drpItem6.Text) > 0 Or Convert.ToInt32(drpItem7.Text) > 0 Or Convert.ToInt32(drpItem8.Text) > 0 Or Convert.ToInt32(drpItem9.Text) > 0 Or Convert.ToInt32(drpItem10.Text) > 0 Then bProductSelected = True
            If bProductSelected Then
                'Go through results
                Dim unitTotal As Decimal = 0
                Dim vatTotal As Decimal = 0
                Dim orderID As Integer
                If Convert.ToInt32(drpItem1.SelectedValue) > 0 Then
                    If isInvalid(txtQty1.Text) Or isInvalid(txtCost1.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty1.Text) * Convert.ToDecimal(txtCost1.Text)
                End If
                If Convert.ToInt32(drpItem2.SelectedValue) > 0 Then
                    If isInvalid(txtQty2.Text) Or isInvalid(txtCost2.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty2.Text) * Convert.ToDecimal(txtCost2.Text)
                End If
                If Convert.ToInt32(drpItem3.SelectedValue) > 0 Then
                    If isInvalid(txtQty3.Text) Or isInvalid(txtCost3.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty3.Text) * Convert.ToDecimal(txtCost3.Text)
                End If
                If Convert.ToInt32(drpItem4.SelectedValue) > 0 Then
                    If isInvalid(txtQty4.Text) Or isInvalid(txtCost4.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty4.Text) * Convert.ToDecimal(txtCost4.Text)
                End If
                If Convert.ToInt32(drpItem5.SelectedValue) > 0 Then
                    If isInvalid(txtQty5.Text) Or isInvalid(txtCost5.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty5.Text) * Convert.ToDecimal(txtCost5.Text)
                End If
                If Convert.ToInt32(drpItem6.SelectedValue) > 0 Then
                    If isInvalid(txtQty6.Text) Or isInvalid(txtCost6.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty6.Text) * Convert.ToDecimal(txtCost6.Text)
                End If
                If Convert.ToInt32(drpItem7.SelectedValue) > 0 Then
                    If isInvalid(txtQty7.Text) Or isInvalid(txtCost7.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty7.Text) * Convert.ToDecimal(txtCost7.Text)
                End If
                If Convert.ToInt32(drpItem8.SelectedValue) > 0 Then
                    If isInvalid(txtQty8.Text) Or isInvalid(txtCost8.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty8.Text) * Convert.ToDecimal(txtCost8.Text)
                End If
                If Convert.ToInt32(drpItem9.SelectedValue) > 0 Then
                    If isInvalid(txtQty9.Text) Or isInvalid(txtCost9.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty9.Text) * Convert.ToDecimal(txtCost9.Text)
                End If
                If Convert.ToInt32(drpItem10.SelectedValue) > 0 Then
                    If isInvalid(txtQty10.Text) Or isInvalid(txtCost10.Text) Then bError = True
                    If Not bError Then unitTotal = unitTotal + Convert.ToInt32(txtQty10.Text) * Convert.ToDecimal(txtCost10.Text)
                End If
                If Not bError Then


                    'Add data to componentOrder table
                    Dim vat As Decimal
                    If txtVAT.Text = "" Or Not IsNumeric(txtVAT.Text) Then
                        vat = 0
                    Else
                        vat = Convert.ToDecimal(txtVAT.Text)
                    End If
                    Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                    Dim oCmd As New SqlCommand("procComponentOrderInsert", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@date", SqlDbType.DateTime))
                        .Parameters.Add(New SqlParameter("@manid", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@orderTotal", SqlDbType.Decimal))
                        .Parameters.Add(New SqlParameter("@currencyID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@billingID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@shippingID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@instructions", SqlDbType.VarChar, 1000))
                        .Parameters.Add(New SqlParameter("@vat", SqlDbType.Decimal))
                        .Parameters.Add(New SqlParameter("@delivery", SqlDbType.Decimal))
                        .Parameters.Add(New SqlParameter("@orderid", SqlDbType.Int))
                        .Parameters("@date").Value = lblDate.Text & " " & FormatDateTime(Now(), DateFormat.LongTime)
                        .Parameters("@manid").Value = drpMan.SelectedValue
                        .Parameters("@orderTotal").Value = unitTotal
                        .Parameters("@currencyID").Value = Convert.ToInt32(lblCurrencyID.Text)
                        .Parameters("@billingID").Value = Convert.ToInt32(drpBilling.SelectedValue)
                        .Parameters("@shippingID").Value = Convert.ToInt32(drpShipping.SelectedValue)
                        .Parameters("@instructions").Value = txtInstructions.Text
                        .Parameters("@vat").Value = vat
                        .Parameters("@delivery").Value = txtDelivery.text
                        .Parameters("@orderid").Direction = ParameterDirection.Output
                    End With
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                        orderID = oCmd.Parameters("@orderid").Value
                    Catch ex As Exception
                        Response.Write(ex)
                        Response.End()
                    Finally
                        oCmd.Dispose()
                        oConn.Dispose()
                    End Try

                    'Add Each product to componentOrderItem table
                    If Convert.ToInt32(drpItem1.SelectedValue) > 0 Then addItem(drpItem1.SelectedValue, txtQty1.Text, txtCost1.Text, orderID)
                    If Convert.ToInt32(drpItem2.SelectedValue) > 0 Then addItem(drpItem2.SelectedValue, txtQty2.Text, txtCost2.Text, orderID)
                    If Convert.ToInt32(drpItem3.SelectedValue) > 0 Then addItem(drpItem3.SelectedValue, txtQty3.Text, txtCost3.Text, orderID)
                    If Convert.ToInt32(drpItem4.SelectedValue) > 0 Then addItem(drpItem4.SelectedValue, txtQty4.Text, txtCost4.Text, orderID)
                    If Convert.ToInt32(drpItem5.SelectedValue) > 0 Then addItem(drpItem5.SelectedValue, txtQty5.Text, txtCost5.Text, orderID)
                    If Convert.ToInt32(drpItem6.SelectedValue) > 0 Then addItem(drpItem6.SelectedValue, txtQty6.Text, txtCost6.Text, orderID)
                    If Convert.ToInt32(drpItem7.SelectedValue) > 0 Then addItem(drpItem7.SelectedValue, txtQty7.Text, txtCost7.Text, orderID)
                    If Convert.ToInt32(drpItem8.SelectedValue) > 0 Then addItem(drpItem8.SelectedValue, txtQty8.Text, txtCost8.Text, orderID)
                    If Convert.ToInt32(drpItem9.SelectedValue) > 0 Then addItem(drpItem9.SelectedValue, txtQty9.Text, txtCost9.Text, orderID)
                    If Convert.ToInt32(drpItem10.SelectedValue) > 0 Then addItem(drpItem10.SelectedValue, txtQty10.Text, txtCost10.Text, orderID)

                    'Order complete
                    'Transfer to the order edit page where the total/shipping can be seen.
                    Server.Transfer("componentOrderPrint.aspx?id=" & orderID)
                    'Server.Transfer("componentOrderView.aspx?id=" & orderID)
                Else
                    lblError.Text = "You must fill in the Quantity and Cost fields for each component you have selected."
                End If
            Else
                lblError.Text = "You must pick at least 1 component before pressing Submit"
            End If
        End If
    End Sub
    Protected Sub addItem(ByVal item As String, ByVal qty As String, ByVal cost As String, ByVal id As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentOrderItemInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@compID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@price", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@compID").Value = Convert.ToInt32(item)
            .Parameters("@qty").Value = Convert.ToInt32(qty)
            .Parameters("@price").Value = Convert.ToDecimal(cost)
            .Parameters("@orderID").Value = Convert.ToInt32(id)
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
    End Sub
    Protected Function isInvalid(ByVal txt As String) As Boolean
        Dim result As Boolean = False
        If txt = "" Or Not (IsNumeric(txt)) Then result = True
        Return result
    End Function
    Protected Sub BindAddressDropDowns()
        Dim oConn As SqlConnection
        Dim oCmd As SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        'Do Billing
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procEBBillingSelect", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            ds = New DataSet
            da.Fill(ds)
            drpBilling.DataSource = ds
            drpBilling.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Do shipping
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procEBShippingSelect", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            ds = New DataSet
            da.Fill(ds)
            drpShipping.DataSource = ds
            drpShipping.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnAddBilling_click(ByVal sender As Object, ByVal e As EventArgs)
        Server.Transfer("componentBuyAddress.aspx?type=bill")
    End Sub
    Protected Sub btnAddShipping_click(ByVal sender As Object, ByVal e As EventArgs)
        Server.Transfer("componentBuyAddress.aspx?type=ship")
    End Sub
    Protected Sub drpItem1_databound(ByVal sender As Object, ByVal e As EventArgs)
        Response.Write("drp1 databound<br>")
    End Sub
    Protected Sub drpItem2_databound(ByVal sender As Object, ByVal e As EventArgs)
        Response.Write("drp2 databound<br>")
    End Sub
    Protected Sub drpItem3_databound(ByVal sender As Object, ByVal e As EventArgs)
        Response.Write("drp3 databound<br>")
    End Sub
    Protected Sub drpItem4_databound(ByVal sender As Object, ByVal e As EventArgs)
        Response.Write("drp4 databound<br>")
    End Sub
    Protected Sub drpItem5_databound(ByVal sender As Object, ByVal e As EventArgs)
        Response.Write("drp5 databound<br>")
    End Sub
    Protected Sub drpDummy_databound(ByVal sender As Object, ByVal e As EventArgs)
        Response.Write("dummy databound<br>")
    End Sub

    Protected Sub drpItem1_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow2.visible = True
    End Sub
    Protected Sub drpItem2_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow3.visible = True
    End Sub
    Protected Sub drpItem3_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow4.visible = True
    End Sub
    Protected Sub drpItem4_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow5.visible = True
    End Sub
    Protected Sub drpItem5_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow6.visible = True
    End Sub
    Protected Sub drpItem6_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow7.visible = True
    End Sub
    Protected Sub drpItem7_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow8.visible = True
    End Sub
    Protected Sub drpItem8_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow9.visible = True
    End Sub
    Protected Sub drpItem9_indexChanged(ByVal sender As Object, ByVal e As EventArgs)
        tRow10.visible = True
    End Sub
    Protected Sub drpItem10_indexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
End Class
