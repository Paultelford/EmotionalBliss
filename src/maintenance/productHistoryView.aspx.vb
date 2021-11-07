Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_productHistoryView
    Inherits System.Web.UI.Page
    Protected Const _qtyStockAddedPos As Integer = 3
    Protected Const _qtyStockRemovedPos As Integer = 4
    Protected Const _qtyProdAddedPos As Integer = 5
    Protected Const _qtyProdRemovedPos As Integer = 6
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            If Not Page.IsPostBack Then
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procProductByIDSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@productID", SqlDbType.Int))
                    .Parameters("@productID").Value = Request.QueryString("id")
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        lblProduct.Text = ds.Tables(0).Rows(0)("productName")
                    Else
                        lblProduct.Text = "Unknown Product"
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
            End If
        End If
    End Sub
    Protected Sub gvHistory_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim iStockAddedTotal As Integer = 0
        Dim iStockRemovedTotal As Integer = 0
        Dim iProdAddedTotal As Integer = 0
        Dim iProdRemovedTotal As Integer = 0
        For Each Row As GridViewRow In gvHistory.Rows
            If Row.RowType = DataControlRowType.DataRow Then
                'Add row values to totals
                iStockAddedTotal = iStockAddedTotal + Convert.ToInt32(Row.Cells(_qtyStockAddedPos).Text)
                iStockRemovedTotal = iStockRemovedTotal + Convert.ToInt32(Row.Cells(_qtyStockRemovedPos).Text)
                iProdAddedTotal = iProdAddedTotal + Convert.ToInt32(Row.Cells(_qtyProdAddedPos).Text)
                iProdRemovedTotal = iProdRemovedTotal + Convert.ToInt32(Row.Cells(_qtyProdRemovedPos).Text)
            End If
            If Row.RowType = DataControlRowType.Footer Then
                'Add totals to Footer
                gvHistory.FooterRow.Cells(_qtyStockAddedPos).Text = iStockAddedTotal
                gvHistory.FooterRow.Cells(_qtyStockRemovedPos).Text = iStockRemovedTotal
                gvHistory.FooterRow.Cells(_qtyProdAddedPos).Text = iProdAddedTotal
                gvHistory.FooterRow.Cells(_qtyProdRemovedPos).Text = iProdRemovedTotal
            End If
        Next
    End Sub
    Protected Function formatStartDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then
            result = FormatDateTime(d.ToString, DateFormat.LongDate) & " " & FormatDateTime(d.ToString, DateFormat.ShortTime)
        Else
            result = "Unknown"
        End If

        Return result
    End Function
End Class

