
Partial Class affiliates_chequeLog
    Inherits BasePage

    Protected Sub Page_Load(ByVal sener As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        gvList.DataBind()
    End Sub
    Protected Sub gv_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvList.PageIndexChanging
        Dim gv As GridView = CType(sender, GridView)
        gv.PageIndex = e.NewPageIndex
        gv.DataBind()
    End Sub
    Protected Sub gv_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvList.DataBound
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerData(gv)
    End Sub
    Protected Sub gv_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvList.DataBinding
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerControl(gv)
    End Sub
    Protected Sub gvList_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        pan2.Visible = True
    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        pan2.Visible = False
    End Sub
End Class
