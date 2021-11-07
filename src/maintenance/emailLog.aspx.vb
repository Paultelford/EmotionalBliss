Imports System.Configuration

Partial Class maintenance_emailLog
    Inherits System.Web.UI.Page

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        gvProfile.DataBind()
    End Sub

    Protected Sub drpProfile_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        siteInclude.addItemToDropdown(CType(sender, DropDownList))
    End Sub
    'SelectedIndexChanged
    Protected Sub gvProfile_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        scrollBrowserAjax(dvEmail)
    End Sub
    'Selecting
    Protected Sub sqlEmail_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        e.Command.Parameters(0).Value = ConfigurationManager.AppSettings("databaseEmailProfileID")
    End Sub
    Protected Sub scrollBrowserAjax(ByVal ctrl As Control)
        'Dim cs As ClientScriptManager = smp.
        'cs.RegisterStartupScript(Me.GetType, "pageScroll", "self.setTimeout(""document.location='#step" & s & "';"",200);", True)
        ScriptManager.RegisterStartupScript(ctrl, Me.GetType, "onloader", "self.setTimeout(""document.location='#m';"",200);", True)
    End Sub
    'Pager subs
    Protected Sub gv_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvProfile.PageIndexChanging
        Dim gv As GridView = CType(sender, GridView)
        gv.PageIndex = e.NewPageIndex
        gv.DataBind()
    End Sub
    Protected Sub gv_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvProfile.DataBound
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerData(gv)
    End Sub
    Protected Sub gv_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvProfile.DataBinding
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerControl(gv)
    End Sub
End Class
