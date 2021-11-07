Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_errors
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        'gvErrors.Visible = False
        'btnDeleteAll.Visible = False
        'lblErrors.Visible = False

    End Sub
    Protected Sub gvErrors_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If gvErrors.Rows.Count = 0 Then
            lblErrors.Visible = False
        End If
    End Sub
    Protected Function showDateTime(ByVal d As Date) As String
        Return FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
    End Function
    Protected Sub lnkClearAll_click(ByVal sender As Object, ByVal e As EventArgs)
        'clear all errors
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procErrorsUpdate", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            siteInclude.addError("maintenance/default.aspx.vb", "lnkClearAll_click(); " & ex.ToString)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        gvErrors.DataBind()
    End Sub
End Class
