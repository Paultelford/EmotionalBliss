Imports System.Data

Partial Class affiliates_newsView
    Inherits BasePage

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        'If Session("ebMediaAccess") <> "true" Then ebMenu1.menuName = "Press"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("nid") = "" Then
                'No default ID has been passed
                'Grab the latest headline by default
                Dim dt As New DataTable
                Dim viewable As String = "public"
                viewable = "mediab2b"
                Try
                    Dim param() As String = {"@viewable", "@active"}
                    Dim paramValue() As String = {viewable, "true"}
                    Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.Bit}
                    Dim paramSize() As Integer = {10, 0}
                    dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procNewsByDateSelectTop1")
                    dvNews.DataSourceID = ""
                    dvNews.DataSource = dt
                    dvNews.DataBind()
                Catch ex As Exception
                    siteInclude.addError("news.aspx.vb", "Page_Load(); " & ex.ToString())
                Finally
                    dt.Dispose()
                End Try
            End If
        Else

        End If
    End Sub

    'Page
    Protected Sub sqlNewsHeadlines_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        'If InStr(LCase(Request.ServerVariables("QUERY_STRING")), "media") Then
        'e.Command.Parameters("@viewable").Value = "media"
        'Else
        'e.Command.Parameters("@viewable").Value = "public"
        'End If
    End Sub
End Class
