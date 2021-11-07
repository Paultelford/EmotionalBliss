
Partial Class saletc
    Inherits BasePage
    Protected Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Page.Title = getDBResouceString("PageTitle")
        lblParagraph1.Text = getDBResouceString("Paragraph1")
        lblParagraph2.Text = getDBResouceString("Paragraph2")
        lblParagraph3.Text = getDBResouceString("Paragraph3")
        lblParagraph4.Text = getDBResouceString("Paragraph4")
        lblParagraph5.Text = getDBResouceString("Paragraph5")
        lblParagraph6.Text = getDBResouceString("Paragraph6")
        lblParagraph7.Text = getDBResouceString("Paragraph7")
        lblParagraph8.Text = getDBResouceString("Paragraph8")
        lblParagraph9.Text = getDBResouceString("Paragraph9")
        lblParagraph10.Text = getDBResouceString("Paragraph10")
    End Sub
End Class
