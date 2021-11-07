Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_royaltyUpdate
    Inherits System.Web.UI.Page

    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As button = CType(sender, button)
        Dim bUpdateDB As Boolean = False
        Dim bRetail As Boolean = False
        Dim strSQL As String = ""
        If btn.CommandArgument <> "test" Then
            bUpdateDB = True
            If LCase(btn.CommandArgument) = "true" Then bRetail = True
        End If

        'Set 1st sql query (All order that are complete, all countrys)
        strSQL = "SELECT SO.id FROM shopOrder SO WHERE SO.orderStatus='complete'"
        If bRetail Then
            strSQL = strSQL & " AND orderCountryCode<>'zz' order by so.id"
        Else
            strSQL = strSQL & " AND orderCountryCode='zz' order by so.id"
        End If

        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand(strSQL, oConn)
        Dim dr As SqlDataReader
        oCmd.CommandType = CommandType.Text
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            dr = oCmd.ExecuteReader
            While dr.Read
                Response.Write(dr("id") & "<br>")
                If bUpdateDB Then
                    Response.Write(assignRoyalties(dr("id"), bRetail) & "<br><br>")
                End If
            End While
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            If Not dr.IsClosed Then dr.Close()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub

    Protected Sub addToAffStatement(ByVal affID As Integer, ByVal royaltyAmount As Decimal, ByVal orderID As Integer, ByVal d As Date)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateStatementInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@statementCredit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@statementDebit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@extOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@linkedPrefix", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@cheque", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@reason", SqlDbType.VarChar, 2000))
            .Parameters.Add(New SqlParameter("@transDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
            .Parameters("@affID").Value = affID
            .Parameters("@actionID").Value = 14
            .Parameters("@statementCredit").Value = royaltyAmount
            .Parameters("@statementDebit").Value = 0
            .Parameters("@orderID").Value = orderID
            .Parameters("@extOrderID").Value = 0
            .Parameters("@linkedPrefix").Value = ""
            .Parameters("@cheque").Value = ""
            .Parameters("@reason").Value = ""
            .Parameters("@transDate").Value = d
            .Parameters("@pk").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub addToRoyaltyTransactions(ByVal orderID As Integer, ByVal distBuyingID As Integer, ByVal royalty As Decimal, ByVal affID As Integer, ByRef oConn As SqlConnection, ByVal d As Date)
        Dim oCmd As New SqlCommand("procRoyaltyTransactionsWithDateInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@distBuyingID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@credit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@debit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@transDate", SqlDbType.DateTime))
            .Parameters("@orderID").Value = orderID
            .Parameters("@distBuyingID").Value = distBuyingID
            .Parameters("@actionID").Value = 1
            .Parameters("@affID").Value = affID
            .Parameters("@credit").Value = royalty
            .Parameters("@debit").Value = 0
            .Parameters("@transDate").Value = d
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Dispose()
        End Try
    End Sub
    Protected Sub addToRoyaltyTransactions(ByVal orderID As Integer, ByVal distBuyingID As Integer, ByVal royalty As Decimal, ByVal affID As Integer, ByVal currency As String, ByRef oConn As SqlConnection)
        Dim oCmd As New SqlCommand("procRoyaltyTransactionsInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@distBuyingID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@credit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@debit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 3))
            .Parameters("@orderID").Value = orderID
            .Parameters("@distBuyingID").Value = distBuyingID
            .Parameters("@actionID").Value = 1
            .Parameters("@affID").Value = affID
            .Parameters("@credit").Value = royalty
            .Parameters("@debit").Value = 0
            .Parameters("@currency").Value = currency
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Dispose()
        End Try
    End Sub
    Protected Sub addRoyaltyToAffStatement(ByVal affID As Integer, ByVal royaltyAmount As Decimal, ByVal currency As String, ByVal orderID As Integer, ByVal scandate As DateTime)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateStatementInsert3", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@statementCredit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@statementDebit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@extOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@linkedPrefix", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@cheque", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@reason", SqlDbType.VarChar, 2000))
            .Parameters.Add(New SqlParameter("@transDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 3))
            .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
            .Parameters("@affID").Value = affID
            .Parameters("@actionID").Value = 14
            .Parameters("@statementCredit").Value = royaltyAmount
            .Parameters("@statementDebit").Value = 0
            .Parameters("@orderID").Value = orderID
            .Parameters("@extOrderID").Value = 0
            .Parameters("@linkedPrefix").Value = ""
            .Parameters("@cheque").Value = ""
            .Parameters("@reason").Value = ""
            .Parameters("@transDate").Value = scandate
            .Parameters("@currency").Value = currency
            .Parameters("@pk").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub

    Protected Function getScanDate(ByVal id As Integer, ByRef oConn As SqlConnection) As Date
        Dim oCmd As New SqlCommand("SELECT TOP 1 scanDate FROM scan WHERE scanOrderID=" & id & " ORDER BY scanDate DESC", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As Date = Now()
        oCmd.CommandType = CommandType.Text
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = CType(ds.Tables(0).Rows(0)("scanDate"), Date)
        Catch ex As Exception
            Response.Write(ex.Message & "<br>")
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
        End Try
        Return result
    End Function
    Protected Function assignRoyalties(ByVal orderID As Integer, ByVal bRetail As Boolean) As String
        Dim result As String = "0"
        'Get recordset of earners royalties
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procRoyaltyEarningsByOrderIDRoyaltySelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim ht As New Hashtable
        Dim currency As String = ""
        Dim scanDate As Date = getScanDate(orderID, oConn)
        Response.Write(scanDate & "<br>")
        Dim totalRoyalty As Decimal = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@retail", SqlDbType.Bit))
            .Parameters("@orderID").Value = orderID
            .Parameters("@retail").Value = bRetail
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            For Each row As DataRow In ds.Tables(0).Rows
                currency = row("orderCurrency")
                If bRetail Then
                    addToRoyaltyTransactions(orderID, row("distBuyingID"), row("royalty"), row("affID"), oConn, scanDate)
                Else
                    addToRoyaltyTransactions(orderID, row("distBuyingID"), row("royalty"), row("affID"), row("orderCurrency"), oConn)
                End If
                'Keep track of each royalty earners total, as items are returned seperately
                If ht.ContainsKey("aff" & row("affID")) Then
                    'Add to
                    ht("aff" & row("affID")) = ht("aff" & row("affID")) + row("royalty")

                Else
                    'Create new
                    ht.Add("aff" & row("affID"), row("royalty"))
                End If

                totalRoyalty = totalRoyalty + CDec(row("royalty"))
            Next
            'Add Earner Totals to aff statement
            For Each item As DictionaryEntry In ht
                If bRetail Then
                    addToAffStatement(Replace(item.Key, "aff", ""), item.Value, orderID, scanDate)
                Else
                    addRoyaltyToAffStatement(Replace(item.Key, "aff", ""), item.Value, currency, orderID, scanDate)
                End If

            Next

        Catch ex As Exception
            Throw ex
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
End Class
