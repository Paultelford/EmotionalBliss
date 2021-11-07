
Partial Class affiliates_survey
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
    End Sub

    'Page
    Protected Sub gvSurvey_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Dim totalOrders As Integer = 0
        Dim lnk As LinkButton
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                totalOrders = totalOrders + row.Cells(siteInclude.getGVRowByHeader(gv, "Qty")).Text
                'Make the link button inactive for the 'No answer..' item
                lnk = row.FindControl("lnkSelect")
                If InStr(lnk.Text, "...") > 0 Then lnk.Enabled = False
            End If
        Next
        lblCount.Text = totalOrders
    End Sub

    'User
    Protected Sub drpOrderType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvSurvey1.SelectedIndex = -1

    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvSurvey1.SelectedIndex = -1
    End Sub

    'Functions
    Protected Function showResult(ByVal o As Object) As String
        Dim result As String = "No answer..."
        If Not IsDBNull(o) Then result = o.ToString
        Return result
    End Function
End Class
