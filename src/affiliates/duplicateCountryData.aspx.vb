Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_duplicateCountryData
    Inherits BasePage

    Protected Sub btnGo_click(ByVal sender As Object, ByVal e As eventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("SELECT id, name, value, type, countryCode, page FROM dbResources WHERE countryCode = '" & drpSource.SelectedValue & "'", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    duplicateData(row, drpDestination.SelectedValue)
                Next
            End If
            lblOutput.Text = lblOutput.Text & "Job complete."
        Catch ex As Exception
            lblError.Text = lblError.Text & "main:" & ex.message & "<br>"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub duplicateData(ByRef row As DataRow, ByVal cc As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("INSERT INTO dbResources (name,value,type,countryCode,page) VALUES ('" & makeSafe(row("name")) & "','" & makeSafe(row("value")) & "','" & makeSafe(row("type")) & "','" & cc & "','" & makeSafe(row("page")) & "')", oConn)
        oCmd.CommandType = CommandType.Text
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            lblOutput.Text = lblOutput.Text & "Copying item (" & row("name") & "," & row("type") & "," & row("page") & ")...... "
            oCmd.ExecuteNonQuery()
            lblOutput.Text = lblOutput.Text & "SUCCESS<br>"
        Catch ex As Exception
            lblOutput.Text = lblOutput.Text & "FAILED<br>"
            lblError.Text = lblError.Text & "sub:" & ex.message & "<br>"
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function makeSafe(ByVal txt As Object)
        If isDBNull(txt) Then
            Return txt
        Else
            Return Replace(txt.ToString(), "'", "''")
        End If

    End Function
End Class
