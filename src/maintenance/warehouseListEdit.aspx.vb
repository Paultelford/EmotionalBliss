Imports siteInclude

Partial Class maintenance_warehouseListEdit
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private qs As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            qs = Request.QueryString("drpSel")
            lnkRefresh.NavigateUrl = "warehouseListEdit.aspx?id=" & Request.QueryString("id") & "&drpSel=" & Request.QueryString("drpSel")
        End If
    End Sub
    Protected Sub lnkbtnBack_click(ByVal sender As Object, ByVal e As EventArgs)
        Context.Items("drpSel") = qs
        Server.Transfer("warehouseList.aspx")
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add changes to productHistory table
        Dim eb As New siteInclude
        Dim iAdd As Integer = 0
        Dim iRem As Integer = 0
        Dim sAddOrRemove As String = "added"
        If rad1.SelectedValue = "add" Then
            iAdd = Convert.ToInt32(lblQty.Text)
        Else
            iRem = Convert.ToInt32(lblQty.Text)
            sAddOrRemove = "removed"
        End If
        eb.addToWarehouseHistory(Request.QueryString("id"), iAdd, iRem, 3, txtReason.Text, Membership.GetUser.UserName, 0, 0, 0, 0)
        eb = Nothing
        'Hide panel & show info
        dvDetails.DataBind()
        pan1.Visible = False
        pan2.Visible = True
    End Sub
End Class
