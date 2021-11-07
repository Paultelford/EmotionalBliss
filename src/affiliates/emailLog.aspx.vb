
Partial Class affiliates_emailLog
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Membership.GetUser Is Nothing) Or Session("EBAffEBDistributorCountryCode") = "" Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub txtUPS_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = CType(sender, TextBox)
        If txt.Visible Then
            txt.Focus()
        End If
    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvEmailLog.DataBind()
    End Sub
    Protected Sub gvEmailLog_selectedIndexChamged(ByVal sender As Object, ByVal e As EventArgs)
        panLog.Visible = False
        panDetails.Visible = True
        lblDateHeader.Text = "Tracker Details for " & FormatDateTime(gvEmailLog.DataKeys(gvEmailLog.SelectedIndex).Value, DateFormat.LongDate)
        gvDetails.DataBind()
    End Sub
    Protected Sub gvDetails_rowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        'Dim txt As TextBox = gvDetails.Rows(e.NewValues(0)).FindControl("txtUPS")
        'siteInclude.debug("Row editing, id= " & txt.ClientID & ", value=" & txt.Text)
        'txt.Focus()
        Dim hid As HiddenField = gvDetails.Rows(e.RowIndex).FindControl("hidOrderID")
        'siteInclude.debug("e.new(0)=" & e.NewValues(0).ToString)
        siteInclude.AddToOrderLog(hid.Value, "EmailLog UPS Invoice Updated with " & e.NewValues(0).ToString, Session("EBAffUsername"), False, "N/A")
    End Sub
    Protected Sub lnkBack_click(ByVal sender As Object, ByVal e As EventArgs)
        panLog.Visible = True
        panDetails.Visible = False
        gvEmailLog.SelectedIndex = -1
    End Sub
    Protected Function showDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d.ToString, DateFormat.LongDate)
        Return result
    End Function

End Class
