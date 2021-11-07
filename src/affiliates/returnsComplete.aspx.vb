Imports system.data
Imports system.data.SqlClient

Partial Class affiliates_returnsComplete
    Inherits BasePage

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
    End Sub

    'Page Events
    Protected Sub fvReturn_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim fv As FormView = CType(sender, FormView)
        Dim lblAction As Label = fv.FindControl("lblAction")
        Dim lblDistAction As Label = fv.FindControl("lblDistAction")
        If LCase(lblDistAction.Text) = "return" Then lblAction.Text = "Returned to customer"
        If LCase(lblDistAction.Text) = "faulty" Then lblAction.Text = "Replaced/Refunded"
    End Sub

    'User Events

    'Subs
    Protected Sub fvReturn_blankIfEmpty(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label = CType(sender, Label)
        If lbl.Text = "" Then
            lbl.Visible = False
        Else
            lbl.Text = lbl.Text & "<br>"
        End If
    End Sub

    'Functions
    Protected Function showBoughtFrom(ByVal o As Object) As String
        Dim result As String = ""
        If Not IsDBNull(o) Then
            If LCase(o.ToString()) = "eb" Then
                result = "EB"
            Else
                result = "External shop"
            End If
        End If
        Return result
    End Function
End Class
