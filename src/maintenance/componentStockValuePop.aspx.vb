Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentStockValuePop
    Inherits System.Web.UI.Page
    Private Const _pricePos As Integer = 4
    Private Const _qtyReceivedPos As Integer = 6
    Private Const _valuePos As Integer = 8
    Private qtyRemain As Integer
    Private stockValue As Decimal = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentHistoryByCompIDStockValueSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@compID", SqlDbType.Int))
            .Parameters("@compID").Value = Convert.ToInt32(Request.QueryString("id"))
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds, "details")
            If ds.Tables("details").Rows.Count > 0 Then
                Dim rs As DataRow = ds.Tables("details").Rows(0)
                lblCompName.Text = rs("componentName")
                lblManufacturerName.Text = rs("manufacturerName")
                lblStock.Text = rs("stock")
                lblCurrency.Text = rs("manufacturerCurrency")
                lblCurrency.Font.Bold = True
                qtyRemain = Convert.ToInt32(rs("stock"))
            Else
                'Error
                Response.Write("Data not found.")
            End If
        Catch ex As Exception
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub gvOrders_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If gvOrders.Rows.Count > 0 Then
            For Each row As GridViewRow In gvOrders.Rows
                If qtyRemain > 0 Then
                    If Convert.ToInt32(row.Cells(_qtyReceivedPos).Text) <= qtyRemain Then
                        qtyRemain = qtyRemain - Convert.ToInt32(row.Cells(_qtyReceivedPos).Text)
                    Else
                        row.Cells(_qtyReceivedPos).Text = Convert.ToInt32(row.Cells(_qtyReceivedPos).Text) - (Convert.ToInt32(row.Cells(_qtyReceivedPos).Text) - qtyRemain)
                        qtyRemain = 0
                    End If
                    row.Cells(_valuePos).Text = Convert.ToDecimal(row.Cells(_pricePos).Text) * Convert.ToInt32(row.Cells(_qtyReceivedPos).Text)
                    stockValue = stockValue + Convert.ToDecimal(row.Cells(_valuePos).Text)
                Else
                    row.Visible = False
                End If
            Next
            lblStockValue.Text = Convert.ToString(FormatNumber(stockValue, 2))
        End If
    End Sub
    Protected Function formatDate(ByVal d As Object) As String
        Dim result As String = "Unknown"
        If Not IsDBNull(d) Then
            result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        End If
        Return result
    End Function
End Class
