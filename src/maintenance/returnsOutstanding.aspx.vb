
Partial Class maintenance_returnsOutstanding
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("login.aspx")
    End Sub
    Protected Sub fvReturn_blankIfEmpty(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label = CType(sender, Label)
        If lbl.Text = "" Then
            lbl.Visible = False
        Else
            lbl.Text = lbl.Text & "<br>"
        End If
    End Sub
    Protected Sub lnkOpenPopup_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim pan As Panel = fvReturn.FindControl("panComplete")
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), "printpopup", "self.setTimeout(openPrintPop(" & Request.QueryString("id") & "),200);", True)
        pan.Visible = True
    End Sub
End Class
