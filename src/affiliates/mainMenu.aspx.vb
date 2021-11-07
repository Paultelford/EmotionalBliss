
Partial Class affiliates_mainMenu
    Inherits BasePage
    Private Const _gvMenu_urlPos As Integer = 2
    Private Const _gvMenu_commandPos As Integer = 6

    Protected Sub gvMenu_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim ctrl
        If lcase(e.Row.Cells(_gvMenu_urlPos).text) = "~/shopintro.aspx" Then
            ctrl = e.Row.Cells(_gvMenu_commandPos).Controls(0)
            ctrl.Visible = False
        End If

    End Sub
End Class
