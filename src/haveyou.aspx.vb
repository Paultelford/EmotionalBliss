
Partial Class haveyou
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.Title = getDBResourceString("PageTitle")
            Dim metaDescription As HtmlControl = Master.FindControl("metaDescription")
            Dim metaKeywords As HtmlControl = Master.FindControl("metaKeywords")
            Dim sYes, sNo As String
            metaDescription.Attributes.Add("content", getDBResourceString("MetaDescription"))
            metaKeywords.Attributes.Add("content", getDBResourceString("MetaKeywords"))
            lblIntro.Text = getDBResourceString("Intro")
            lblParagraph1.Text = getDBResourceString("Paragraph1")
            lblParagraph2.Text = getDBResourceString("Paragraph2")
            lblParagraph3.Text = getDBResourceString("Paragraph3")
            lblParagraph4.Text = getDBResourceString("Paragraph4")
            lblParagraph5.Text = getDBResourceString("Paragraph5")
            lblParagraph6.Text = getDBResourceString("Paragraph6")
            lblParagraph7.Text = getDBResourceString("Paragraph7")
            lblParagraph8.Text = getDBResourceString("Paragraph8")
            'If getDBResourceString("submit") <> "" Then btnSubmit.ImageUrl = siteInclude.trimCrap(getDBResouceString("submit"))
            If getDBResourceString("yes") <> "" Then
                sYes = getDBResourceString("yes")
                radQ1Yes.Text = sYes
                radQ2Yes.Text = sYes
                radQ3Yes.Text = sYes
                radQ4Yes.Text = sYes
                radQ5Yes.Text = sYes
                radQ6Yes.Text = sYes
                radQ7Yes.Text = sYes
                radQ8Yes.Text = sYes
            End If
            If getDBResourceString("no") <> "" Then
                sNo = Replace(getDBResourceString("no"), "<p>", "")
                radQ1No.Text = sNo
                radQ2No.Text = sNo
                radQ3No.Text = sNo
                radQ4No.Text = sNo
                radQ5No.Text = sNo
                radQ6No.Text = sNo
                radQ7No.Text = sNo
                radQ8No.Text = sNo
            End If
            Dim tmp As String = ""
            tmp = getDBResourceString("cssSubmit")
            If tmp <> "" Then lnkSubmit.CssClass = tmp
            tmp = getDBResourceString("ttSubmit")
            If tmp <> "" Then lnkSubmit.ToolTip = tmp
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim result As String = ""
        result = getResults()
        Select Case LCase(result)
            Case "true|true|false|false|false|false|false|false|"
                If siteInclude.trimCrap(getDBResourceString("AnswerA")) <> "" Then
                    lbLResults.Text = getDBResourceString("AnswerA")
                Else
                    lbLResults.Text = "You have reached the initial arousal phase, but not achieved orgasm. You need more stimulation, by hand or with a vibrator."
                End If
            Case "true|true|true|true|true|true|true|true|"
                If siteInclude.trimCrap(getDBResourceString("AnswerB")) <> "" Then
                    lbLResults.Text = getDBResourceString("AnswerB")
                Else
                    lbLResults.Text = "You have definitely had an orgasm. You have had the intense arousal needed for orgasm and experienced the powerful sense of wellbeing that satisfying sexual climax can give."
                End If
            Case Else
                If siteInclude.trimCrap(getDBResourceString("Answerc")) <> "" Then
                    lbLResults.Text = getDBResourceString("AnswerC")
                Else
                    lbLResults.Text = "It is possible that you have had an orgasm, but it is unlikely to have been particularly intense or satisfying. You may also be at the ‘plateau’ phase of arousal, with more arousal needed to enjoy orgasm. You need more stimulation, particularly if you are attempting to integrate your orgasm with penetrative sex. Ask your partner to continue stimulating your clitoris as he penetrates you, or caress yourself as he thrusts into you to give you a more intense orgasmic sensation."
                End If
        End Select
        panQuestions.Visible = False
    End Sub
    Protected Function getResults()
        Dim result As String = ""
        result = result & CStr(radQ1Yes.Checked) & "|"
        result = result & CStr(radQ2Yes.Checked) & "|"
        result = result & CStr(radQ3Yes.Checked) & "|"
        result = result & CStr(radQ4Yes.Checked) & "|"
        result = result & CStr(radQ5Yes.Checked) & "|"
        result = result & CStr(radQ6Yes.Checked) & "|"
        result = result & CStr(radQ7Yes.Checked) & "|"
        result = result & CStr(radQ8Yes.Checked) & "|"
        Return result
    End Function

End Class
