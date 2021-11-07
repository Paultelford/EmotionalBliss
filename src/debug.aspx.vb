Imports System.Data
Imports System.Data.SqlClient

Partial Class debug
    Inherits System.Web.UI.Page

    Protected Sub btnClear_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDebugDelete", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        gvDebugLog.DataBind()
    End Sub
End Class
