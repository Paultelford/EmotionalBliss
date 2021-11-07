Imports System.Data
Imports System.Data.SqlClient

Partial Class static_
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
        If LCase(Request.QueryString("p")) = "newsletter" Then panNewsletter.Visible = True
    End Sub

    Protected Sub btnSignup_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procNewsletterInsert", oConn)
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 100))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@name").Value = txtName.Text
            .Parameters("@email").Value = txtEmail.Text
            .Parameters("@countryCode").Value = Session("EBShopCountry")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            siteInclude.addError("static.aspx", "btnSignup_click(Name=" & txtName.Text & ", email=" & txtEmail.Text & "); " & ex.ToString)
            lblNewsletter.Text = "&nbsp;&nbsp;&nbsp;An error occured whle adding your deatils. <br>&nbsp;&nbsp;&nbsp;Please try again later."
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try

        If Not bError Then
            tblHomeNewsletter.Visible = False
            lblNewsletter.Text = "&nbsp;&nbsp;&nbsp;You have been added to our mailing list."
        End If
    End Sub
End Class
