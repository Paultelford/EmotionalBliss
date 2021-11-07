Imports System.Drawing
Imports System.Drawing.Imaging

Partial Class termsIntro
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
        'Header image
        'Dim imgURL As String = getDBResouceString("imgPageHeader")
        'tblHeader.Attributes.Add("style", "background-image:url('" & Replace(imgURL, "~/", "") & "');")
        'Dim bmp As New Bitmap(Page.MapPath(imgURL), False)
        'tdLeftMenu.Attributes.Add("height", bmp.Height - 1)
        'bmp.Dispose()
        'Hide main content text window
        Dim pan As Panel = Master.FindControl("panTextBody")
        'pan.Visible = False
    End Sub
End Class
