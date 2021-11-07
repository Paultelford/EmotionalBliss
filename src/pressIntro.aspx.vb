Imports System.Drawing
Imports System.Drawing.Imaging

Partial Class pressIntro
    Inherits BasePage
    Protected Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Page.Title = getDBResourceString("PageTitle")
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
        'Header image
        'Dim imgURL As String = getDBResourceString("imgPageHeader")
        'tblHeader.Attributes.Add("style", "background-image:url('" & Replace(imgURL, "~/", "") & "');")
        'Dim bmp As New Bitmap(Page.MapPath(imgURL), False)
        'tdLeftMenu.Attributes.Add("height", bmp.Height - 1)
        'bmp.Dispose()
        'Hide main content text window
        Dim pan As Panel = Master.FindControl("panTextBody")
        pan.Visible = False
    End Sub
End Class
