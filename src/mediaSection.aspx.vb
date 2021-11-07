Imports System.Data

Partial Class mediaSection
    Inherits System.Web.UI.Page
    Private Const _columns As Integer = 2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        loadMedia()
    End Sub

    'Subs
    Protected Sub loadMedia()
        Dim dt As New DataTable
        Try
            Dim param() As String = {}
            Dim paramValue() As String = {}
            Dim paramType() As SqlDbType = {}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "proc")
        Catch ex As Exception
            siteInclude.addError()
        Finally
            dt.Dispose()
        End Try
    End Sub
End Class
