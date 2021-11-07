Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Drawing.Imaging

Partial Class homeIntro
    Inherits BasePage
    Protected Sub page_init(ByVal sender As Object, ByVal e As EventArgs)Handles Me.Init
        Server.Transfer("/newHomeIntro.aspx")

    End Sub
    Protected Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Server.Transfer("/newHomeIntro.aspx")

        Dim metaDescription As HtmlControl = Master.FindControl("metaDescription")
        Dim metaKeywords As HtmlControl = Master.FindControl("metaKeywords")
        Page.Title = getDBResourceString("PageTitle")
        metaDescription.Attributes.Add("content", getDBResourceString("MetaDescription"))
        metaKeywords.Attributes.Add("content", getDBResourceString("MetaKeywords"))
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
        'Update product url's using shopCountry
        If Not Page.IsPostBack Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductOnSaleByCountryHomeIntroCodeSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim qs As String = ""
            If LCase(Session("EBShopCountry")) = "nl" Or LCase(Session("EBShopCountry")) = "be" Then
                qs = "&m=Kopen"
            Else
                qs = "&m=shop"
            End If

            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters("@countryCode").Value = Session("EBShopCountry")
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In ds.Tables(0).Rows
                        'If LCase(row("saleName")) = "femblossom ""heat""" Then lnkFemblossom.NavigateUrl = "~/shop/product.aspx?id=" & row("id") & "&prod=Femblossom" & qs
                        'If LCase(row("saleName")) = "womolia ""heat""" Then lnkWomolia.NavigateUrl = "~/shop/product.aspx?id=" & row("id") & "&prod=Womolia" & qs
                        'If LCase(row("saleName")) = "chandra" Then lnkChandra.NavigateUrl = "~/shop/product.aspx?id=" & row("id") & "&prod=Chandra" & qs
                    Next
                End If
            Catch ex As Exception
                Response.Write(ex.ToString())
            End Try
            'Hide Left Menu
            Dim m As msite = CType(Master, msite)
            m.LeftMenu.Visible = False
        End If
    End Sub
End Class
