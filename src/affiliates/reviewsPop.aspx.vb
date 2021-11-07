
Partial Class affiliates_reviewsPop
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        If Page.IsPostBack Then
        Else
        End If
    End Sub
    Protected Function showDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d.ToString, DateFormat.LongDate)
        Return result
    End Function
    Protected Sub sqlReviewDetails_updated(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        lblComplete.Text = "<font color='red'>The review has been updated.  <br>Changes will not show in the main page unless it is refreshed.</font>"
    End Sub
    Protected Function formatReview(ByVal o As Object)
        Dim result As String = ""
        If Not IsDBNull(0) Then
            result = Replace(o.ToString, Chr(10), "<br>")
            result = Replace(result, "<p>", "")
            result = Replace(result, "</p>", "")
        End If
        Return result
    End Function
    Protected Sub fvReview_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If fvReview.CurrentMode = FormViewMode.Edit Then
            'hide new or old review, depending on review type.
            Dim txtReview As TextBox = fvReview.FindControl("txtReview")
            If txtReview.Text <> "" Then
                'old review text exists, hide new table
                Dim lbl As Label
                Dim txt As TextBox
                For iLoop As Integer = 1 To 8
                    lbl = fvReview.FindControl("lblQ" & iLoop & "Text")
                    txt = fvReview.FindControl("lblQ" & iLoop)
                    lbl.Visible = False
                    txt.Visible = False
                Next
            Else
                'New review, hide the old review textbox
                txtReview.Visible = False
            End If
        Else
            Dim lblReview As Label = fvReview.FindControl("lblReview")
            If lblReview.Text <> "" Then
                Dim tblReview As HtmlTable = fvReview.FindControl("tblReview")
                tblReview.Visible = False
            End If
        End If
    End Sub
End Class
