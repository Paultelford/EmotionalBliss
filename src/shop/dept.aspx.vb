Imports System.Data
Imports System.Data.SqlClient


Partial Class shop_dept
    Inherits BasePage
    Private Const _productsPerRow As Integer = 2
    Private Const _imageWidths As Integer = 275

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            loadDBResources()
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        showProducts()
    End Sub

    'Subs
    Protected Sub showProducts()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleByDeptIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim tRow As New TableRow
        Dim tblProducts As New Table
        tblProducts.Width = Unit.Percentage(100)
        tblProducts.BorderWidth = 0
        tblProducts.CellPadding = 0
        tblProducts.CellSpacing = 0
        Dim iRowPos As Int16 = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@deptID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@deptID").Value = Request.QueryString("id")
            .Parameters("@countryCode").Value = Session("EBShopCountry")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'Populate unique Page Title and meta tags for selected product
                If Not IsDBNull(ds.Tables(0).Rows(0)("pageTitle")) Then If ds.Tables(0).Rows(0)("pageTitle") <> "" Then Page.Title = ds.Tables(0).Rows(0)("pageTitle")
                Dim metaDescription As HtmlControl = Master.FindControl("metaDescription")
                Dim metaKeywords As HtmlControl = Master.FindControl("metaKeywords")
                If Not IsDBNull(ds.Tables(0).Rows(0)("metaDescription")) Then If ds.Tables(0).Rows(0)("metaDescription") <> "" Then metaDescription.Attributes.Add("content", ds.Tables(0).Rows(0)("metaDescription"))
                If Not IsDBNull(ds.Tables(0).Rows(0)("metaKeywords")) Then If ds.Tables(0).Rows(0)("metaKeywords") <> "" Then metaKeywords.Attributes.Add("content", ds.Tables(0).Rows(0)("metaKeywords"))
                'Show department and its description
                If Not IsDBNull(ds.Tables(0).Rows(0)("deptName")) Then lblDeptName.Text = ds.Tables(0).Rows(0)("deptName")
                If Not IsDBNull(ds.Tables(0).Rows(0)("deptDescription")) Then lblDeptDescription.Text = ds.Tables(0).Rows(0)("deptDescription")
                'Build Table
                For Each row As DataRow In ds.Tables(0).Rows
                    addCell(row, tRow)
                    iRowPos = iRowPos + 1
                    If iRowPos = _productsPerRow Then
                        tblProducts.Rows.Add(tRow)
                        tRow = New TableRow
                        lblProductTables.Controls.Add(tblProducts)
                        tblProducts = New Table
                        tblProducts.Width = Unit.Percentage(100)
                        tblProducts.CellPadding = 0
                        tblProducts.CellSpacing = 0
                        iRowPos = 0
                    End If
                Next
                If iRowPos > 0 Then
                    tblProducts.Rows.Add(tRow) 'Add row if it has cells
                    lblProductTables.Controls.Add(tblProducts)
                End If
                tblProducts = Nothing
                tRow = Nothing
            Else
                lblDeptDescription.Text = CType(GetLocalResourceObject("lblNoproducts"), String)
            End If
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub addCell(ByVal rs As DataRow, ByRef tblRow As TableRow)
        Dim tCell As New TableCell
        tCell.Controls.Add(addProduct(rs))
        tCell.HorizontalAlign = HorizontalAlign.Center
        tCell.VerticalAlign = VerticalAlign.Top
        tblRow.Cells.Add(tCell)
    End Sub
    Protected Sub loadDBResources()
        'litShopText.Text = getDBResourceString("litShopText")
    End Sub

    'Functions
    Protected Function addProduct(ByVal rs As DataRow) As Table
        Dim tCell As New TableCell
        Dim lnk As New HyperLink
        Dim lblDesc As New Label
        Dim lblName As New Label
        Dim lblShortDesc As New Label
        Dim lblPrice As New Label
        Dim tblSingleProduct As New Table
        Dim row As New TableRow
        Dim btnMoreInfo As New ImageButton
        Dim lnkMoreInfo As New HyperLink
        Dim sImgName As String
        If IsDBNull(rs("saleDeptImageName")) Then
            If IsDBNull(rs("saleImageName")) Then
                sImgName = "~/images/products/productMissing.gif"
            Else
                sImgName = "~/images/products/" & rs("saleImageName")
            End If
        Else
            sImgName = "~/images/products/" & rs("saleDeptImageName")
        End If
        tblSingleProduct.CellSpacing = 0
        tblSingleProduct.CellPadding = 0
        lblName.Text = "<table border='0'><tr><td width='8'>&nbsp;</td><td><font size='+1'><b>" & rs("saleName") & "</b></font></td><td width='8'>&nbsp;</td></tr></table><br>"
        If rs("saleDiscount") > 0 Then
            lblPrice.Text = "<table border='0'><tr><td width='8'>&nbsp;</td><td><font size='-1'>" & CType(GetLocalResourceObject("_cbWas"), String) & " "
            lblPrice.Text = lblPrice.Text & rs("currencySign") & FormatNumber(rs("saleUnitPrice") + ((rs("saleUnitPrice") / 100) * rs("saleTaxRate")), 2)
            lblPrice.Text = lblPrice.Text & "&nbsp;&nbsp;" & Format(rs("saleDiscount"), "0.#") & "% " & CType(GetLocalResourceObject("_cbOff"), String)
            lblPrice.Text = lblPrice.Text & "<br></font><font size='+0'><b>" & CType(GetLocalResourceObject("_cbNow"), String) & " " & rs("currencySign") & FormatNumber(rs("unitPriceAfterDiscountIncVat"), 2)
            lblPrice.Text = lblPrice.Text & "</b></font></td><td width='8'>&nbsp;</td></tr></table><br>"
        Else
            lblPrice.Text = "<table border='0'><tr><td width='8'>&nbsp;</td><td><font size='+0'><b>" & CType(GetLocalResourceObject("_cbPrice"), String) & "</b> "
            lblPrice.Text = lblPrice.Text & rs("currencySign") & FormatNumber(rs("saleUnitPrice") + ((rs("saleUnitPrice") / 100) * rs("saleTaxRate")), 2) & "</font></td><td width='8'>&nbsp;</td></tr></table><br>"
        End If

        lblDesc.Text = "<table border='0'><tr><td width='8'>&nbsp;</td><td align='left'>" & rs("saleDeptInfo") & "</td><td width='8'>&nbsp;</td></tr></table>"
        lblShortDesc.Text = "<table border='0'><tr><td width='8'>&nbsp;</td><td><font size='-1'><b>" & rs("saleShortDescription") & "</b></font></td><td width='8'>&nbsp;</td></tr></table><br>"
        lnk.ImageUrl = sImgName
        lnk.Text = rs("saleName")
        lnk.NavigateUrl = "product.aspx?id=" & rs("id") & "&prod=" & rs("saleName")
        If Request.QueryString("m") <> "" Then lnk.NavigateUrl = lnk.NavigateUrl & "&m=" & Request.QueryString("m")
        btnMoreInfo.AlternateText = CType(GetLocalResourceObject("_cbMoreInfo"), String)
        btnMoreInfo.ImageAlign = ImageAlign.Middle
        btnMoreInfo.ImageUrl = CType(GetLocalResourceObject("_cbImgMoreInfo"), String)
        btnMoreInfo.PostBackUrl = "product.aspx?id=" & rs("id") & "&prod=" & rs("saleName")
        lnkMoreInfo.ToolTip = CType(GetLocalResourceObject("_cbMoreInfo"), String)
        lnkMoreInfo.NavigateUrl = "product.aspx?id=" & rs("id") & "&prod=" & rs("saleName")
        lnkMoreInfo.CssClass = "MoreInfoRollover"

        'btnMoreInfo.OnClientClick = "browseTo('product.aspx?id=" & rs("id") & "&prod=" & rs("saleName") & "')"

        tCell.Width = _imageWidths
        tCell.Controls.Add(lnk)
        'tCell.Controls.Add(lblName)
        tCell.Controls.Add(lblShortDesc)
        tCell.Controls.Add(lblPrice)
        'tCell.Controls.Add(btnMoreInfo) - Replaced with lnkMoreInfo
        tCell.Controls.Add(lnkMoreInfo)
        tCell.Controls.Add(lblDesc)
        tCell.VerticalAlign = VerticalAlign.Top
        tCell.HorizontalAlign = HorizontalAlign.Center
        'tblRow.Cells.Add(tCell)
        'tCell = Nothing
        row.Cells.Add(tCell)
        tblSingleProduct.Rows.Add(row)
        Return tblSingleProduct
    End Function
End Class