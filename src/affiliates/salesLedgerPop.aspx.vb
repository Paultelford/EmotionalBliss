Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_salesLedgerPop
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Show complete orders for date range
        Dim startDate As String = Request.QueryString("startDate")
        Dim endDate As String = Request.QueryString("endDate")
        Dim type As String = Request.QueryString("type")
        If Not (startDate = "" Or endDate = "") Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procShopOrderByScanDateCountryCodeStatusSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
                .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
                .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 20))
                .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
                .Parameters("@startDate").Value = startDate
                .Parameters("@endDate").Value = endDate
                .Parameters("@status").Value = "complete"
                .Parameters("@type").Value = Type
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                Response.Write("Complete orders between <b>" & FormatDateTime(startDate, DateFormat.LongDate) & "</b> and <b>" & FormatDateTime(endDate, DateFormat.LongDate) & "</b> for <b>" & Replace(type, "%", "all") & "</b> orders<br><br>")
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In ds.Tables(0).Rows
                        Response.Write(row("userOrderID") & "<br>")
                    Next
                Else
                    Response.Write("No complete orders found for date range.")
                End If
            Catch ex As Exception
                Response.Write(ex.ToString)
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Else
            Response.Write("Dates not passed.")
        End If
    End Sub
End Class
