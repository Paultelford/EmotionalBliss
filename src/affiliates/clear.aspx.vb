Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_clear
    Inherits System.Web.UI.Page

    Protected Sub btnScan_click(ByVal sender As Object, ByVal e As EventArgs)
        runCommand("DELETE FROM scan")
        runCommand("DELETE FROM scanError")
        runCommand("DELETE FROM tracker")
        runCommand("UPDATE shopOrder SET orderStatus='placed' WHERE id=1068")
    End Sub

    Private Sub runCommand(ByVal sqlCommand As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand(sqlCommand, oConn)
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
End Class
