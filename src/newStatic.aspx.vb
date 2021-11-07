
Partial Class newStatic
    Inherits BasePage

    Protected Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim metaDescription As HtmlControl = Master.FindControl("metaDescription")
        Dim metaKeywords As HtmlControl = Master.FindControl("metaKeywords")
        Page.Title = getDBResourceString("PageTitle")
        metaDescription.Attributes.Add("content", getDBResourceString("MetaDescription"))
        metaKeywords.Attributes.Add("content", getDBResourceString("MetaKeywords"))
        lblParagraph1.Text = getDBResourceString("Paragraph1")
        lblParagraph2.Text = getDBResourceString("Paragraph2")
        lblParagraph3.Text = getDBResourceString("Paragraph3")
        lblParagraph4.Text = getDBResourceString("Paragraph4")
        lblParagraph5.Text = getDBResourceString("Paragraph5")
        lblParagraph6.Text = getDBResourceString("Paragraph6")
        lblParagraph7.Text = getDBResourceString("Paragraph7")
        lblParagraph8.Text = getDBResourceString("Paragraph8")
        lblParagraph9.Text = getDBResourceString("Paragraph9")
        lblParagraph10.Text = getDBResourceString("Paragraph10")
    End Sub
End Class
