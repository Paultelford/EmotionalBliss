Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentOrderPDFNew
    Inherits System.Web.UI.Page
    Private orderID As Integer


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        orderID = Request.QueryString("id")
        Dim orderTotal As Decimal = 0
        'Dim lblSub
        lblPurchaseOrder.Text = orderID
        'Populate the address of company the order is being placed with
        Dim oConn As SqlConnection
        Dim oCmd As SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procManufacturerByOrderIDAddressSelect", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = orderID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            dvToAddress.DataSource = ds
            dvToAddress.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Populate items
        Dim tRow As New TableRow
        Dim tCell As New TableCell
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procComponentOrderItemsByOrderIDSelect", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = orderID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            ds = New DataSet
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each rs As DataRow In ds.Tables(0).Rows
                    tRow = New TableRow
                    refresh(tCell)
                    tCell.Text = rs("qty")
                    tRow.Cells.Add(tCell)
                    refresh(tCell)
                    tCell.Text = rs("componentCode")
                    tRow.Cells.Add(tCell)
                    refresh(tCell)
                    tCell.Text = rs("componentName")
                    tRow.Cells.Add(tCell)
                    refresh(tCell)
                    tCell.Text = rs("price")
                    tRow.Cells.Add(tCell)
                    refresh(tCell)
                    tCell.Text = rs("price") * rs("qty")
                    tRow.Cells.Add(tCell)
                    tblItems.Rows.Add(tRow)
                    orderTotal = orderTotal + Convert.ToInt32(rs("qty")) * Convert.ToDecimal(rs("price"))
                Next
                lblSubTotal.Text = FormatNumber(orderTotal, 2)
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
        bindLabels()
    End Sub
    Protected Sub dvToAddress_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvToAddress.Rows
            row.Cells(0).Visible = False
            If row.Cells(1).Text = "-" Then row.Visible = False
        Next
    End Sub
    Protected Sub refresh(ByRef cell)
        cell = New TableCell
    End Sub
    Protected Sub dvShipping_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvShipping.Rows
            row.Cells(0).Visible = False 'Hide 1st column as no column headers are being shown
            If row.Cells(1).Text = "*" Then row.Visible = False
        Next
    End Sub
    Protected Sub dvBilling_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvBilling.Rows
            row.Cells(0).Visible = False 'Hide 1st column as no column headers are being shown
            If row.Cells(1).Text = "*" Then row.Visible = False
        Next
    End Sub
    Protected Sub bindLabels()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCurrencyByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With (oCmd)
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = orderID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblCurrency.Text = ds.Tables(0).Rows(0)("currencyCode")
                lblCurrency2.Text = ds.Tables(0).Rows(0)("currencyCode")
                lblInstructions.Text = ds.Tables(0).Rows(0)("instructions")
                lblNetAmount.Text = Convert.ToDecimal(lblSubTotal.Text) + Convert.ToDecimal(lblVatTotal.Text)
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
End Class
