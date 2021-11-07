
Partial Class maintenance_componentList
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            If Page.IsPostBack Then
                _login = Master.FindControl("logMaintenance")
                _content = _login.FindControl("ContentPlaceHolder1")
            End If
        End If
    End Sub
    Protected Sub drpMaster_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'System.Threading.Thread.Sleep(2000)
        'gvComponents.DataBind()
    End Sub
End Class
