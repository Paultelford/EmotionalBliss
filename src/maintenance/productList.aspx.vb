
Partial Class maintenance_productList
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _linkPos As Integer = 6

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            'Response.Write("DS=" & Context.Items("drpSel"))
            If Context.Items("drpSel") <> "" Then
                drpMasters.SelectedIndex = Convert.ToInt32(Context.Items("drpSel"))
                DataBind()
            End If
        End If
    End Sub
    Protected Sub gvProducts_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim lnk As HyperLink = e.Row.Cells(_linkPos).Controls(0)
        lnk.NavigateUrl = lnk.NavigateUrl & "&drpSel=" & drpMasters.SelectedIndex
    End Sub
    Protected Function makeAdjustURL(ByVal id As Integer) As String
        Return "productListEdit.aspx?id=" & id & "&drpSel=" & drpMasters.SelectedIndex
    End Function
End Class
