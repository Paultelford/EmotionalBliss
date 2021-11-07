
Partial Class maintenance_productAssemblyView
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            '_login = Master.FindControl("logMaintenance")
            '_content = _login.FindControl("ContentPlaceHolder1")
        End If
        lnkBack.NavigateUrl = "componentHistoryViewComponent.aspx?id=" & Request.QueryString("cid") & "&startDate=" & Request.QueryString("startDate") & "&endDate=" & Request.QueryString("endDate")
    End Sub
    Protected Function showPassFail(ByVal pass As Object, ByVal fail As Object) As String
        Dim retVal As String
        If IsDBNull(pass) Or IsDBNull(fail) Then
            retVal = ""
            Dim lbl As Label = fvProdAss.FindControl("lblIncomplete")
            lbl.Text = "<font color='red'>Production Incomplete!</font>"
        Else
            retVal = "(" & pass.ToString & " Passed, " & fail.ToString & " Failed)"
        End If
        Return retVal
    End Function
    Protected Function showDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then
            result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        End If
        Return result
    End Function
    Protected Sub fvProdAss_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Move the Failure Report Comments to the bottom of the page
        Dim lblProductionInfo As Label = fvProdAss.FindControl("lblProductionInfo")
        lblProductionInfoCopy.Text = lblProductionInfo.Text
        lblProductionInfo.Text = ""
    End Sub
End Class
