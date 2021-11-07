Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class shop_product
    Inherits BasePage
    Private _tdBasket As Object
    Private _content As ContentPlaceHolder
    Private Const _reviewMaxLength As Integer = 4000
    Private Const _gvBasket_itemPos As Integer = 1
    Private Const _gvBasket_pricePos As Integer = 3
    Private Const _maxSubMenuItems As Integer = 10
    Private Const _maxChars As Integer = 4000
    Private Const _dvReviewNewStartRow As Integer = 3
    Private Const _dvReviewNewEndRow As Integer = 10
    Private Const _dvReviewOldRow As Integer = 11

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        setBasketAjaxTrigger()
        loadDBResources()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'If basketHasItems() Then _tdBasket.visible = True
        _content = Master.FindControl("ContentPlaceHolder1")
        'Test for sesion timeout
        If Session("EBLanguage") = "" Then
            'Session has timed out, for now jus redirect to default.aspx
            Response.Redirect("default.aspx")
        End If
        showProductDetails()
        If Len(addDetails_componentCode.Value) > 2 Then
            If LCase(Left(addDetails_componentCode.Value, 2)) = "v_" Then
                'Voucher selected, show the voucher details panel
                panVoucherForm.Visible = True
            End If
        End If
        'Bind shopping basket to page
        bindBasket()
        'If Not basketHasItems() Then tblBasket.Attributes.Add("style", "display:none")
    End Sub
    Protected Sub showProductDetails()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleByIdSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@id").Value = Request.QueryString("id")
            .Parameters("@countryCode").Value = Session("EBLanguage")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'imgNext.Visible = True
                'imgNext.Attributes.Add("onclick", "navNext()")
                'imgNext.Attributes.Add("style", "cursor:hand")
                Dim rs As DataRow = ds.Tables(0).Rows(0)
                'Populate hidden fields, this will save a DB call when user adds product to basket
                addDetails_ID.Value = rs("id")
                addDetails_Name.Value = rs("saleName")
                addDetails_Price.Value = rs("saleUnitPrice")
                addDetails_Discount.Value = "0.00"
                addDetails_PriceIncDiscount.Value = rs("saleUnitPrice")
                addDetails_Vat.Value = rs("saleTaxRate")
                addDetails_DistBuyingID.Value = rs("distBuyingID")
                imgProductReview.ImageUrl = "~/images/products/" & rs("saleImageName")

                lblPrice.Text = FormatNumber(rs("finalUnitPrice"), 2)
                If Not IsDBNull(rs("componentCode")) Then addDetails_componentCode.Value = rs("componentCode")
                'Show main details on screen
                lblProdName.Text = rs("saleName")
                If Not IsDBNull(rs("saleDescription")) Then lblProdDescription.Text = rs("saleDescription")
                'If Not IsDBNull(rs("saleImageName")) Then loadImages(rs("id"), rs("saleImageName"), oConn)
                If LCase(rs("itemType")) = "bproduct" Then lnkSubMenuSensations.Visible = True
                If Not IsDBNull(rs("currencySign")) Then lblCurrencySign.Text = rs("currencySign")
                'Show the products submenu in the left hands frame
                showSubMenu(rs("id"))
                'Remove sensations link if 'showSensations=0' in the productOnSale table
                'trSensations.Visible = CBool(rs("showSensations"))
                tdButton1.Visible = CBool(rs("showSensations"))
                'imgHeatLogo.Visible = CBool(rs("showSensations"))
                'trUnderlineSensations.Visible = CBool(rs("showSensations"))
                'Remove Reviews link if 'showReviews=-' in the productOnSale table
                lnkSubMenuReviews.Visible = CBool(rs("showReviews"))
                'trUnderlineReviews.Visible = CBool(rs("showReviews"))
            Else
                lblProdName.Text = "Product cannot be found."
            End If
        Catch ex As Exception
            Try
                lblError.Text = "<font color='red'>Details for the selcted product could not be found.<br>We are sorry for any inconvenience.<br>Please try again later.<br><br>"
                Dim si As New siteInclude
                si.addError("shop/product.aspx.vb", "showProductDetail(Session(EBLanguage)=" & Session("EBLanguage") & "&id=" & Request.QueryString("id") & ");" & ex.ToString)
                si = Nothing
                tblAddToBasket.Visible = False
                'imgProduct.Visible = False
                'imgNext.Visible = False
                'tblBasket.Visible = False
            Catch ex2 As Exception
            End Try
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    
    Protected Sub btnAddToBasket_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim weight As Integer = 0
        If panVoucherForm.Visible Then
            Page.Validate()
            If Page.IsValid Then
                'create voucher in db and pass the vouchernumber to the shopping basket
                'Response.Write("Page is valid<br>")
                Dim voucherNumber As String = createVoucher()
                'Response.Write("returned voucher number=" & voucherNumber & "<br>")
                Profile.EBCart.AddItem(addDetails_ID.Value, addDetails_Name.Value, CType(addDetails_Price.Value, Decimal), CType(addDetails_Discount.Value, Decimal), CType(addDetails_PriceIncDiscount.Value, Decimal), CType(addDetails_Vat.Value, Decimal), CType(addDetails_DistBuyingID.Value, Integer), 0, voucherNumber, CType(addDetails_componentCode.Value, String))
                bindBasket()
                'tblBasket.Attributes("style") = "block"
                panVoucherForm.Visible = False
                'Response.Redirect("basket.aspx")
            End If
        Else
            If LCase(Left(addDetails_componentCode.Value, 1)) <> "v" Then weight = 500
            Profile.EBCart.AddItem(addDetails_ID.Value, addDetails_Name.Value, CType(addDetails_Price.Value, Decimal), CType(addDetails_Discount.Value, Decimal), CType(addDetails_PriceIncDiscount.Value, Decimal), CType(addDetails_Vat.Value, Decimal), CType(addDetails_DistBuyingID.Value, Integer), weight, CType(addDetails_componentCode.Value, String))
            'Dim basket As HtmlTableCell
            'basket = Me.Master.FindControl("tdBasket")
            'basket.Visible = True
            bindBasket()
            'tblBasket.Attributes("style") = "block"
        End If

    End Sub
    
    Protected Function createVoucher() As String
        'Function will  creatre a new voucher and return the unique number
        Dim voucherNumber As String = "0"
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procVoucherCreateInsert", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@credit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@recipient", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@purchaser", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@comment", SqlDbType.VarChar, 4000))
            .Parameters.Add(New SqlParameter("@recipientEmail", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@purchaserEmail", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@distributorBuyingID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@distributorID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@productOnSaleID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@shopCountry", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@coupon", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@voucherNumber", SqlDbType.VarChar, 10))
            .Parameters("@credit").Value = CType(addDetails_Price.Value, Decimal)
            .Parameters("@recipient").Value = txtRecipientName.Text
            .Parameters("@purchaser").Value = txtPurchaserName.Text
            .Parameters("@comment").Value = txtMessage.Text
            .Parameters("@recipientEmail").Value = txtRecipientEmail.Text
            .Parameters("@purchaserEmail").Value = txtPurchaserEmail.Text
            .Parameters("@currency").Value = Session("EBShopCurrency")
            .Parameters("@distributorBuyingID").Value = addDetails_DistBuyingID.Value
            .Parameters("@distributorID").Value = 0
            .Parameters("@productOnSaleID").Value = addDetails_ID.Value
            .Parameters("@shopCountry").Value = Session("EBLanguage")
            .Parameters("@coupon").Value = False
            .Parameters("@voucherNumber").Direction = ParameterDirection.Output
        End With

        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            voucherNumber = oCmd.Parameters("@voucherNumber").Value
        Catch ex As Exception
            Dim si As siteInclude
            si.addError("product.aspx.vb", "createVoucher()::" & ex.ToString)
            lblError.Text = "An error occured while creating the voucher.  Please try again later.<br>Sorry for any inconvenience."
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return voucherNumber
    End Function
    Protected Sub showSubMenu(ByVal posID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleMenuByPosIDSelect", oConn)
        Dim da As New SqlDataAdapter()
        Dim ds As New DataSet
        Dim lnkBtn As LinkButton
        Dim iPos As Integer = 1
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@posID", SqlDbType.Int))
            .Parameters("@posID").Value = posID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each rs As DataRow In ds.Tables(0).Rows
                    lnkBtn = _content.FindControl(CStr("lnkSubMenu" & iPos))
                    lnkBtn.Text = rs("menu")
                    lnkBtn.CommandArgument = rs("id")
                    iPos = iPos + 1
                Next
            End If
        Catch ex As Exception
            lblProductFrameError.Text = lblProductFrameError.Text & "iPos=" & iPos & ";  " & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub lnkSubMenuOverview_click(ByVal sender As Object, ByVal e As EventArgs)
        'Show panProductDescription1 (The products details details and overview page)
        'panProductDescription1 is the Overview
        'panProductDescription2 is the html/text driven by the productOnSaleMenu table
        'panProductDescription3 is the sensastions frame
        'panProductDescription4 is the Reviews frame
        panProductDescription1.Visible = True
        panProductDescription2.Visible = False
        panProductDescription3.Visible = False
        panProductDescription4.Visible = False
        panProductDescription4b.Visible = False
    End Sub
    Protected Sub lnkSubMenuSensations_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        panProductDescription1.Visible = False
        panProductDescription2.Visible = False
        panProductDescription3.Visible = True
        panProductDescription4.Visible = False
    End Sub
    Protected Sub lnkSubMenuReviews_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        panProductDescription1.Visible = False
        panProductDescription2.Visible = False
        panProductDescription3.Visible = False
        panProductDescription4.Visible = True
        panProductDescription4b.Visible = True
        'Get image and display thumbnail
        'imgReviewThumb.ImageURL = imgProduct.ImageUrl
        lnkAddReview.Text = "Add Review For " & lblProdName.Text
    End Sub
    Protected Sub lnkSubMenu_click(ByVal sender As Object, ByVal e As EventArgs)
        'Generic Sub for all sub menu link buttons
        'Show panel 2, and load html from productOnSaleMenu table using the productOnSaleMenuID stored in the linkbuttons CommandArgs
        Dim lnkBtn As LinkButton = CType(sender, LinkButton)
        panProductDescription1.Visible = False
        panProductDescription2.Visible = True
        lblProductDescription2.Text = getSubTextByID(lnkBtn.CommandArgument)
    End Sub
    Protected Function getSubTextByID(ByVal ID As Integer) As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleMenuByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim html As String = "No text found for this sub menu item."
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = ID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                html = ds.Tables(0).Rows(0)("html")
            End If
        Catch ex As Exception
            html = "<font color='red'>An error occured whilest retrieving product details</font><br><br>" & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return html
    End Function
    Function basketHasItems() As Boolean
        Return CType(Profile.EBCart.Items.Count > 0, Boolean)
    End Function
    Public Sub bindBasket()
        'Dim gvBasket As GridView = Master.FindControl("gvBasket")
        If Not Profile.EBCart Is Nothing Then
            Try
                gvBasket.DataSource = Profile.EBCart.Items
                gvBasket.DataBind()
            Catch ex As Exception
                Response.Write(ex.ToString)
            End Try
            
            'lblBasketTotal.Text = Profile.EBCart.TotalEx.ToString
            'lblTotal.Font.Size = 8
        End If
    End Sub
    Protected Sub gvBasket_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        For Each row As GridViewRow In gvBasket.Rows
            If row.RowType = DataControlRowType.DataRow Then
                row.Cells(_gvBasket_pricePos).Text = lblCurrencySign.Text & row.Cells(_gvBasket_pricePos).Text
            End If
        Next
        If gv.Rows.Count > 0 Then
            gv.FooterRow.Cells(_gvBasket_itemPos).Text = CType(GetLocalResourceObject("lblTotalText"), String)
            gv.FooterRow.Cells(_gvBasket_pricePos).Text = lblCurrencySign.Text & Profile.EBCart.GoodsIncVat
        End If
    End Sub
    Protected Sub setBasketAjaxTrigger()
        'AddToBasket button - ShoppingBasket GridView
        'If False Then
        Dim trigger As New AsyncPostBackTrigger()
        Dim c As LinkButton
        trigger.EventName = "Click"
        trigger.ControlID = btnAddToBasket.UniqueID
        'updateBasket.Triggers.Add(trigger)
        If False Then
            'SubMenu LinkButtons - Product Content
            trigger = New AsyncPostBackTrigger()
            trigger.EventName = "Click"
            trigger.ControlID = lnkSubMenuOverview.UniqueID
            updateProdDesc.Triggers.Add(trigger)
            For iLoop As Integer = 1 To 10
                trigger = New AsyncPostBackTrigger()
                trigger.EventName = "Click"
                c = FindControl("lnkSubMenu" & iLoop.ToString)
                trigger.ControlID = c.UniqueID
                updateProdDesc.Triggers.Add(trigger)
            Next
        End If
        'End If
    End Sub
    Protected Sub gvBasket_rowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Profile.EBCart.RemoveItem(gvBasket.DataKeys(e.RowIndex).Value)
        Dim id As String = gvBasket.DataKeys(e.RowIndex).Value
        bindBasket()
        'If Not basketHasItems() Then tblBasket.Visible = False
    End Sub
    Protected Sub btnViewProducts_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Response.Redirect("~/shopIntro.aspx")
    End Sub
    Protected Sub tblProductMenu_load(ByVal sender As Object, ByVal e As EventArgs)
        'Show extra menu options (from productMenu table)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductMenuByposIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim tRow As New TableRow
        Dim tCell As TableCell
        Dim lnk As HyperLink
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@posID", SqlDbType.Int))
            .Parameters("@posID").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    tRow = New TableRow
                    tCell = New TableCell
                    lnk = New HyperLink
                    lnk.ForeColor = Drawing.Color.Gray
                    lnk.Text = row("text")
                    lnk.NavigateUrl = row("link")
                    tCell.Controls.Add(lnk)
                    tRow.Cells.Add(tCell)
                    tblLeftMenu.Rows.Add(tRow)
                    tRow = New TableRow
                    tRow.Height = 1
                    tRow.BackColor = System.Drawing.ColorTranslator.FromHtml("lightblue")
                    tCell = New TableCell
                    tRow.Cells.Add(tCell)
                    tblLeftMenu.Rows.Add(tRow)
                Next
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("shop/product.aspx.vb", "tblProductMenu_load(id=" & Request.QueryString("id") & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Add a <br> to all items in the productOnSaleMenu list
        If lnkSubMenu1.Text = "" Then
            trSubMenu1.Visible = False
            trUnderline1.Visible = False
        End If

        If lnkSubMenu2.Text = "" Then
            trSubMenu2.Visible = False
            trUnderline2.Visible = False
        End If
        If lnkSubMenu3.Text = "" Then
            trSubMenu3.Visible = False
            trUnderline3.Visible = False
        End If
        If lnkSubMenu4.Text = "" Then
            trSubMenu4.Visible = False
            trUnderline4.Visible = False
        End If
        If lnkSubMenu5.Text = "" Then
            trSubMenu5.Visible = False
            trUnderline5.Visible = False
        End If
        If lnkSubMenu6.Text = "" Then
            trSubMenu6.Visible = False
            trUnderline6.Visible = False
        End If
        If lnkSubMenu7.Text = "" Then
            trSubMenu7.Visible = False
            trUnderline7.Visible = False
        End If
        If lnkSubMenu8.Text = "" Then
            trSubMenu8.Visible = False
            trUnderline8.Visible = False
        End If
        If lnkSubMenu9.Text = "" Then
            trSubMenu9.Visible = False
            trUnderline9.Visible = False
        End If
        If lnkSubMenu10.Text = "" Then
            trSubMenu10.Visible = False
            trUnderline10.Visible = False
        End If
    End Sub
    Protected Sub dvProductReviews_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim dv As DetailsView = CType(sender, DetailsView)
        'Load language resources
        Dim lblReviewedByText As Label
        Dim lblRating As Label
        For Each row As DetailsViewRow In dv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblReviewedByText = row.FindControl("lblReviewedByText")
                lblRating = row.FindControl("lblRating")
                'lblReviewedByText.Text = getDBResourceString("lblReviewedByText")
                showRating(lblRating.Text, row, "dv")
            End If
        Next
        'Show total number of product reviews, and avg score
        showReviewStats()
        'If current review is of the old style, then hide the new info (and vice verca)
        'It will be new style if the db field 'review'=""
        If False Then
            Dim lbl As Label = dvProductReviews.FindControl("lblReview")
            If lbl.Text = "" Then
                Dim row As DetailsViewRow
                For iLoop As Integer = _dvReviewNewStartRow To _dvReviewNewEndRow
                    row = dv.Rows(iLoop)
                    row.Visible = True
                Next
                dvProductReviews.Rows(_dvReviewOldRow).Visible = False
            Else
                Dim row As DetailsViewRow
                For iLoop As Integer = _dvReviewNewStartRow To _dvReviewNewEndRow
                    row = dv.Rows(iLoop)
                    row.Visible = False
                Next
                dvProductReviews.Rows(_dvReviewOldRow).Visible = True
            End If
        End If
        lblPager.Text = "Review " & dvProductReviews.PageIndex + 1 & " of " & dvProductReviews.PageCount
    End Sub
    Protected Sub gvReviewsLite_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        'Load language resources
        Dim lblReviewedByText As Label
        Dim lblRating As Label
        Dim lblTotalReviews As Label
        Dim lblReviewLite As Label
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblReviewedByText = row.FindControl("lblReviewedByText")
                lblRating = row.FindControl("lblRating")
                lblReviewedByText.Text = getDBResourceString("lblReviewedByText")
                lblReviewLite = row.FindControl("lblReview")
                If lblReviewLite.Text = "" Then row.Visible = False
                showRating(lblRating.Text, row, "gv")
            End If
        Next
        'Show total number of product reviews, and avg score
        showReviewStats()
    End Sub
    Protected Sub showReviewStats()
        Dim oCOnn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procReviewsByPosIDStatsSelect", oCOnn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@posID", SqlDbType.Int))
            .Parameters("@posID").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then
                'dvProductReviews.Visible = False
                lblCurrentReviewsText1.Text = getDBResourceString("lblCurrenctReviewsText1Error")
                lblCurrentReviewstext2.Text = ""
            Else
                lblCurrentReviewsText1.Text = getDBResourceString("lblCurrentReviewsText1")
                lblCurrentReviewstext2.Text = getDBResourceString("lblCurrentReviewsText2")
                lblNumberOfReviews.Text = ds.Tables(0).Rows(0)("qty")
                'lblAvgScore.Text = ds.Tables(0).Rows(0)("avgRating") / 2
                'dvProductReviews.Visible = True

            End If
        Catch ex As Exception
            lblProductFrameError.Text = "<font color='red'>An error has occured. Please try again later.</font>"
            Dim si As New siteInclude
            si.addError("shop/producta.aspx.vb", "showReviewStats(posID=" & Request.QueryString("id") & "); " & ex.ToString)
            si = Nothing
        End Try
    End Sub
    Protected Sub showRating(ByVal r As Integer, ByRef o As Object, ByVal rowType As String)
        Dim row
        If rowType = "dv" Then
            row = CType(o, DetailsViewRow)
        Else
            row = CType(o, GridViewRow)
        End If
        Dim imgStar2 As Image = row.FindControl("imgStar2")
        Dim imgStar3 As Image = row.FindControl("imgStar3")
        Dim imgStar4 As Image = row.FindControl("imgStar4")
        Dim imgStar5 As Image = row.FindControl("imgStar5")
        If r > 1 Then imgStar2.ImageUrl = "~/images/reviewStar.gif"
        If r > 2 Then imgStar3.ImageUrl = "~/images/reviewStar.gif"
        If r > 3 Then imgStar4.ImageUrl = "~/images/reviewStar.gif"
        If r > 4 Then imgStar5.ImageUrl = "~/images/reviewStar.gif"
    End Sub
    Protected Sub lnkAddReview_click(ByVal sender As Object, ByVal e As EventArgs)
        panProductDescription4b.Visible = False
        'tdQ1.Visible = True
        'lnkAddReview.Visible = False
        tblAddReviewControls.Visible = False
        tblAddReviewType.Visible = True
        'Set the product name
        Dim name As String = lblProdName.Text
        lblProduct1.Text = name
        lblProduct2.Text = name
        lblProduct3.Text = name
        lblProduct4.Text = name
        lblProduct5.Text = name
    End Sub
    Protected Sub dvAddReview_inserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
        'Check that review length is less than 4000 chars
        'Dim txtReview As TextBox = dvAddReview.FindControl("txtReview")
        'If Len(txtReview.Text) > _reviewMaxLength Then
        'Dim lblAddReviewError As Label = dvAddReview.FindControl("lblAddReviewError")
        'lblAddReviewError.Text = "<font color='red'>You review exceeds the maximum " & _reviewMaxLength & " characters. You have used " & Len(txtReview.Text) & "</font>"
        'e.Cancel = True
        'Else
        'All ok, go back to product
        'dvAddReview.Controls.Clear()
        'dvAddReview.Visible = False
        'panProductDescription1.Visible = True
        'panProductDescription2.Visible = False
        'panProductDescription3.Visible = False
        'panProductDescription4.Visible = False
        'End If
    End Sub
    Protected Sub btnNext_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As Button = CType(sender, Button)
        Dim q As Integer = Right(btn.CommandArgument, 1)
        Dim td As HtmlTableRow = tblAddReview.FindControl("tdQ" & CStr(q))
        Dim fckControl As Object = tblAddReview.FindControl("fckQ" & CStr(q))
        Dim lblError As Label = tblAddReview.FindControl("lblErrorQ" & CStr(q))
        Dim bError As Boolean = False
        'Test for null entry (or > 4000 chars)
        If fckControl.value = "" Then
            bError = True
            lblError.Text = "<font color='red'>You must enter some text.</font>"
        End If
        If Len(fckControl.Value) > _maxChars Then
            bError = True
            lblError.Text = "<font color='red'>Max " & _maxChars & " characters.  You have used " & Len(fckControl.Value) & "</font>"
        End If
        If Not bError Then
            'Hide current question and reveal the next one
            td.Visible = False
            If q < 9 Then
                td = tblAddReview.FindControl("tdQ" & CStr(q + 1))
                td.Visible = True
            End If
            lblError.Text = ""
        End If
    End Sub
    Protected Sub btnFinish_click(ByVal sender As Object, ByVal e As EventArgs)
        'check that data entered in the txtWillingToPay textbox is numeric (or empty)
        txtWillingToPay.Text = Replace(txtWillingToPay.Text, "£", "")
        If IsNumeric(txtWillingToPay.Text) Or txtWillingToPay.Text = "" Then
            'Add to database
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procReviewInsert", oConn)
            Dim bError As Boolean = False
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@posID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@rating", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@q1", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@q2", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@q3", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@q4", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@q5", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@q6", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@q7", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@q8", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@moreReviews", SqlDbType.Bit))
                .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@willingToPay", SqlDbType.VarChar, 10))
                .Parameters("@posID").Value = Request.QueryString("id")
                .Parameters("@name").Value = txtName.Text
                .Parameters("@rating").Value = drpRating.SelectedValue
                .Parameters("@q1").Value = fckQ1.Value
                .Parameters("@q2").Value = fckQ2.Value
                .Parameters("@q3").Value = fckQ3.Value
                .Parameters("@q4").Value = fckQ4.Value
                .Parameters("@q5").Value = fckQ5.Value
                .Parameters("@q6").Value = fckQ6.Value
                .Parameters("@q7").Value = fckQ7.Value
                .Parameters("@q8").Value = fckQ8.Value
                .Parameters("@countryCode").Value = Session("EBLanguage")
                .Parameters("@moreReviews").Value = radMoreReviews.SelectedValue
                .Parameters("@email").Value = txtEmail.Text
                .Parameters("@willingToPay").Value = txtWillingToPay.Text
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                bError = True
                Dim si As New siteInclude
                si.addError("shop/product.aspx.vb", "btnFinish_click(); " & ex.ToString)
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If Not bError Then
                Dim td As HtmlTableRow = tblAddReview.FindControl("tdQ8")
                td.Visible = False
                tblAddReview.Visible = False
                lblThankyou.Visible = True
                'panProductDescription4b.Visible = True
            Else
                'An error occured while storing users review.
                lblProductFrameError.Text = "<font color='red'>An error occured while storing your review. Please try later.<br>We are sorry for any inconvenience.</font>"
            End If
        Else
            lblWillingError.Text = "<font color='red'>* Numeric data only</font>"
        End If

    End Sub
    Protected Sub radMoreReviews_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim rad As RadioButtonList = CType(sender, RadioButtonList)
        Dim tbl As HtmlTable = tblAddReview.FindControl("tblEmail")
        tbl.Visible = rad.SelectedValue
        btnFinish.Visible = True
    End Sub
    Protected Function FormatReview(ByVal o As Object)
        Dim result As String = ""
        If Not IsDBNull(o) Then
            result = Replace(o.ToString, Chr(10), "<br>")
        End If
        Return result
    End Function
    Protected Sub drpReviewType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.SelectedValue = "lite" Then
            tblProductReviews.Visible = False
            gvReviewsLite.Visible = True
        Else
            tblProductReviews.Visible = True
            gvReviewsLite.Visible = False
        End If
    End Sub
    Protected Sub drpAddReviewType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        Select Case LCase(drp.SelectedValue)
            Case "full"
                tdQ1.Visible = True
                tblAddReview.Visible = True
                dvAddReview.Visible = False
            Case "lite"
                tblAddReview.Visible = False
                dvAddReview.Visible = True
        End Select
        tblAddReviewType.Visible = False
    End Sub
    Protected Sub sqlAddReview_inserted(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        dvAddReview.Visible = False
        lblThankyou.Visible = True
    End Sub
    Protected Sub hypBackToProduct_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim hyp As HyperLink = CType(sender, HyperLink)
        Dim r As New Random
        'Response.Redirect("product.aspx?id=" & Request.QueryString("id") & "&prod=" & Request.QueryString("prod") & "&r=" & r.Next)
        hyp.NavigateUrl = "product.aspx?id=" & Request.QueryString("id") & "&prod=" & Request.QueryString("prod")
    End Sub
    Protected Sub dvProductReviews_pageIndexChange(ByVal sender As Object, ByVal e As EventArgs)
        lblPager.Text = "Review " & dvProductReviews.PageIndex + 1 & " of " & dvProductReviews.PageCount
    End Sub
    Protected Sub lnkPrev_click(ByVal sender As Object, ByVal e As EventArgs)
        If dvProductReviews.PageIndex > 0 Then dvProductReviews.PageIndex = dvProductReviews.PageIndex - 1
        'lblPager.Text = "Review " & dvProductReviews.PageIndex + 1 & " of " & dvProductReviews.PageCount
        If dvProductReviews.PageIndex = 0 Then lnkPrev.Visible = False
        'Show the Next button, it should always be visible after the Prev button was clicked, as long as there is more than 1 review
        If dvProductReviews.PageCount > 1 Then lnkNext.Visible = True
    End Sub
    Protected Sub lnkNext_click(ByVal sender As Object, ByVal e As EventArgs)
        If dvProductReviews.PageIndex + 1 < dvProductReviews.PageCount Then dvProductReviews.PageIndex = dvProductReviews.PageIndex + 1
        'Show current review index
        'lblPager.Text = "Review " & dvProductReviews.PageIndex + 1 & " of " & dvProductReviews.PageCount
        'If last review is being shown, hide the 'Next' linkbutton
        If dvProductReviews.PageIndex + 1 = dvProductReviews.PageCount Then lnkNext.Visible = False
        'Show the Previous button, it should always be visible after the Next button was clicked, as long as there is more than 1 review
        If dvProductReviews.PageIndex > 0 Then lnkPrev.Visible = True
    End Sub
    Protected Sub loadDBResources()
        lblFieldsMandatory.Text = getDBResourceString("lblFieldsMandatory")
    End Sub
End Class


