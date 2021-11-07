Imports System.Data

Partial Class press_press
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@id"}
            Dim paramValue() As String = {Request.QueryString("id").ToString}
            Dim paramType() As SqlDbType = {SqlDbType.Int}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procMediaHTMLByIdSelect")
            If dt.Rows.Count > 0 Then
                litHTML.Text = dt.Rows(0)("html")
            End If
        Catch ex As Exception
            siteInclude.addError("press/press.aspx.vb", "PageLoad(id=" & Request.QueryString("id") & "); " & ex.ToString)
        Finally
            dt.Dispose()
        End Try
    End Sub
End Class
