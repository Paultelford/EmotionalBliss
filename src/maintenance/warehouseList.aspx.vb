
Partial Class maintenance_warehouseList
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _linkPos As Integer = 8
    Private Const _countryPos As Integer = 2
    Private _countryCode As DropDownList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            _countryCode = _login.FindControl("drpMainCountry")
            _countryCode.Visible = True
            'Response.Write("DS=" & Context.Items("drpSel"))
            If Context.Items("drpSel") <> "" Then
                'drpCountry.SelectedIndex = Convert.ToInt32(Context.Items("drpSel"))
                'DataBind()
            End If
            'If drpCountry.SelectedValue <> "0" And drpCountry.SelectedValue <> "%" Then
            'gvProducts.Columns(_countryPos).Visible = False
            'Else
            'gvProducts.Columns(_countryPos).Visible = True
            'End If
        End If
    End Sub
    Protected Sub gvProducts_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim lnk As HyperLink = e.Row.Cells(_linkPos).Controls(0)
        lnk.NavigateUrl = lnk.NavigateUrl & "&drpSel=" & _countryCode.SelectedIndex
    End Sub
    Protected Function makeAdjustURL(ByVal id As Integer) As String
        Return "warehouseListEdit.aspx?id=" & id & "&drpSel=" & _countryCode.SelectedIndex
    End Function
End Class

