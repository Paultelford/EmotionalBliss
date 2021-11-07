Imports System.Data

Partial Class unsubscribe
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("email") <> "" Then
                Dim dt as New DataTable
                try
                    Dim param() As String = {"@email"}
                    Dim paramValue() As String = {Request.QueryString("email")}
                    Dim paramType() As SqlDbType = {SqlDbType.VarChar}
                    Dim paramSize() As Integer = {200}
                    dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procEmailUnsubscribeInsert")
                Catch ex As Exception
                    siteInclude.addError("unsubscribe.aspx.vb", "PageLoad(email=" & Request.QueryString("email") & "); " & ex.ToString())
                Finally
                    dt.Dispose()
                End Try
            End If
        End If
    End Sub
End Class
