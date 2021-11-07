Imports System.Data
Imports System.Data.SqlClient

Partial Class reviews
    Inherits BasePage
    Protected Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Page.Title = getDBResourceString("PageTitle")
            Dim metaDescription As HtmlControl = Master.FindControl("metaDescription")
            Dim metaKeywords As HtmlControl = Master.FindControl("metaKeywords")
            metaDescription.Attributes.Add("content", getDBResourceString("MetaDescription"))
            metaKeywords.Attributes.Add("content", getDBResourceString("MetaKeywords"))
            getFeaturedProducts()
            'If Session("EBShopCountry") = "gb" Then panGBOnly.Visible = True
            'Set correct image depending on countrycode
            imgDept.ImageUrl = "/design/shop/images/reviews_" & Session("EBLanguage") & ".gif"
            imgDept.Attributes.Add("style", "padding-right: 4px;")
        End If
    End Sub
    Protected Sub getFeaturedProducts()
        'Returns 2 Heat products, 2 finger products, and gets department buttons from dbResources
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleByCountryIntroSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim heatIndex As Integer = 1
        Dim fingerIndex As Integer = 1
        Dim lnk As HyperLink
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
                    If row("isHeat") Then
                        lnk = panTest.FindControl("lnkHeat" & heatIndex)
                        lnk.ImageUrl = "~/images/products/" & row("saleImageName")
                        lnk.NavigateUrl = "~/shop/product.aspx?r=1&id=" & row("id")
                        If Request.QueryString("m") <> "" Then lnk.NavigateUrl = lnk.NavigateUrl & "&m=" & Request.QueryString("m")
                        lnk.ToolTip = row("saleName")
                        heatIndex = 2
                    Else
                        lnk = panTest.FindControl("lnkFinger" & fingerIndex)
                        lnk.ImageUrl = "~/images/products/" & row("saleImageName")
                        lnk.NavigateUrl = "~/shop/product.aspx?r=1&id=" & row("id")
                        If Request.QueryString("m") <> "" Then lnk.NavigateUrl = lnk.NavigateUrl & "&m=" & Request.QueryString("m")
                        lnk.ToolTip = row("saleName")
                        fingerIndex = 2
                    End If
                Next
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shopIntro.aspx.vb,1", "getFeaturedProducts(country=" & Session("EBShopCountry") & "); " & ex.ToString())
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Show/get departments
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procDeptCountryIntroSelect", oConn)
        da = New SqlDataAdapter
        ds = New DataSet
        Dim deptIndex As Integer = 1
        Dim img As Image
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
                    lnk = panTest.FindControl("lnkDept" & deptIndex)
                    img = panTest.FindControl("imgDept" & CStr(deptIndex))
                    deptIndex = deptIndex + 1
                    If Not IsDBNull(row("deptImage")) Then img.ImageUrl = row("deptImage")
                    lnk.NavigateUrl = "~/shop/dept.aspx?id=" & row("deptID")
                Next
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shopIntro.aspx.vb,2", "getFeaturedProducts(country=" & Session("EBShopCountry") & "); " & ex.ToString())
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
End Class
