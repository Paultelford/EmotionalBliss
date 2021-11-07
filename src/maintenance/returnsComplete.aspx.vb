Imports system.data
Imports system.data.SqlClient

Partial Class maintenance_returnsComplete
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub fvReturn_blankIfEmpty(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label = CType(sender, Label)
        If lbl.Text = "" Then
            lbl.Visible = False
        Else
            lbl.Text = lbl.Text & "<br>"
        End If
    End Sub
    
End Class
