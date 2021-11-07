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
    Private Const _defaultWeight As Integer = 2

    'System Events
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        testBasketCountry()
        If Request.QueryString("product") <> "" Then autoAddToBasket()
        setBasketAjaxTrigger()
        loadDBResources()
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'If basketHasItems() Then _tdBasket.visible = True
        '_content = Master.FindControl("ContentPlaceHolder1")
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
        If Request.QueryString("r") = "1" And Not Page.IsPostBack Then
            showReview()
        End If
        If Request.QueryString("r") = "1" Then
            'Now hide Sensations & Back to Product buttons, but show Add To Basket
            btnAddToBasket.Visible = True
            btnAddToBasket2.Visible = True
            'lnkSubMenuSensations.Visible = False
            panSensationsButton.Visible = False
            lnkBack2.Visible = False
        End If
        '###################################
        'Product was showing voucher form. Ot was to do with the procProductOnSaleByID SP linking to the component table and wrongly assuming the product is a voucher.
        'Try
        'siteInclude.debug("panVoucherForm.Visible=" & panVoucherForm.Visible & ", componentCode=" & addDetails_componentCode.Value)
        'Catch ex As Exception
        'siteInclude.debug("panVoucherForm error")
        'End Try
        '###################################
        ''Add the jQuery startup code to apply the style to the 'Add to Basket' button
        Dim cs As ClientScriptManager = Page.ClientScript
        'cs.RegisterStartupScript(Me.GetType(), "loade", "self.setTimeout(""$('#" & btnAddToBasket.ClientID & "').button().click(function() { $('#dialog-form').dialog('open'); });"",0);", True)
        If Not Page.IsPostBack Then
            'Low stock warning
            'If Not (LCase(Session("EBShopCountry")) = "nl" Or LCase(Session("EBShopCountry")) = "be") Then lblNote.Text = "Please note: due to the high level of demand we are currently workings to a 10 working days delivery from receipt of order. We apologise for any inconvenience and thank you for your support."
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        panRelatedDropdown.Visible = Not panBack1.Visible
        panAddToBasket.Visible = Not panBack1.Visible
    End Sub
    Protected Sub hypBackToProduct_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim hyp As HyperLink = CType(sender, HyperLink)
        Dim r As New Random
        hyp.NavigateUrl = "product.aspx?id=" & Request.QueryString("id") & "&prod=" & Request.QueryString("prod")
    End Sub
    Protected Sub panPrice_load(ByVal sender As Object, ByVal e As EventArgs)
        CType(sender, Panel).Attributes.Add("style", "margin-left: 60px;")
    End Sub

    'User Control Events
    Protected Sub lnkAddToBasket_click(ByVal sender As Object, ByVal e As EventArgs)
        btnAddToBasket_click(sender, New ImageClickEventArgs(0, 0))
    End Sub

    Protected Sub btnAddToBasket_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim weight As Integer = 0
        Dim componentCode As String = Convert.ToString(addDetails_componentCode.Value)
        'The Belgium Chandra gets incorrectly reported as a voucher, so use pid's instead of component codes for BE orders
        If LCase(Session("EBShopCountry")) = "be" Then componentCode = addDetails_ID.Value
        If panVoucherForm.Visible Then
            Page.Validate()
            If Page.IsValid Then
                'create voucher in db and pass the vouchernumber to the shopping basket
                'Response.Write("Page is valid<br>")
                Dim voucherNumber As String = createVoucher()
                'Response.Write("returned voucher number=" & voucherNumber & "<br>")
                Profile.EBCart.AddItem(addDetails_ID.Value, addDetails_Name.Value, CType(addDetails_Price.Value, Decimal), CType(addDetails_Discount.Value, Decimal), CType(addDetails_PriceIncDiscount.Value, Decimal), CType(addDetails_Vat.Value, Decimal), CType(addDetails_DistBuyingID.Value, Integer), 0, voucherNumber, componentCode)
                'Add basket image
                Profile.EBCart.UpdateBasketImage(addDetails_ID.Value, addDetails_BasketImageName.Value)
                bindBasket()
                updateBasket.Update()
                'tblBasket.Attributes("style") = "block"
                panVoucherForm.Visible = False
                'Response.Redirect("basket.aspx")
            End If
        Else
            If LCase(Left(addDetails_componentCode.Value, 1)) <> "v" Then weight = _defaultWeight
            Profile.EBCart.AddItem(addDetails_ID.Value, addDetails_Name.Value, CType(addDetails_Price.Value, Decimal), CType(addDetails_Discount.Value, Decimal), CType(addDetails_PriceIncDiscount.Value, Decimal), CType(addDetails_Vat.Value, Decimal), CType(addDetails_DistBuyingID.Value, Integer), weight, componentCode)
            'Dim basket As HtmlTableCell
            'basket = Me.Master.FindControl("tdBasket")
            'basket.Visible = True
            'Add basket image
            Profile.EBCart.UpdateBasketImage(addDetails_ID.Value, addDetails_BasketImageName.Value)
            bindBasket()
            'tblBasket.Attributes("style") = "block"
            updateBasket.Update()
        End If
        'siteInclude.debug("Added component code '" & componentCode & "'")
        'Hide the button being stored in currentViewState (As when the Add To Basket button was clicked, it because visible again for some unknown reason.)
        Select Case LCase(currentViewState.Value)
            Case "sensations"
                'lnkSubMenuSensations.Visible = False
                panSensationsButton.Visible = False
            Case "reviews"
                'lnkSubMenuReviews.Visible = False
                panReviewsButton.Visible = False
        End Select
        'siteInclude.debug("isVibe=" & isVibe(addDetails_Name.Value))
        'If isVibe(addDetails_Name.Value) Then
        'Dim bHeatProduct As Boolean = False
        'Dim bOnlyProduct As Boolean = False
        'If InStr(LCase(addDetails_Name.Value), "heat") > 0 Then bHeatProduct = True
        'If InStr(LCase(addDetails_Name.Value), "only") > 0 Then bOnlyProduct = True
        ''Get the 150ml Water & Silicon Lube pID's and place in form
        'Dim dt As New DataTable
        'Dim rowCount As Integer = 0
        'Try
        'Dim param() As String = {"@countryCode", "@free"}
        'Dim paramValue() As String = {Session("EBShopCountry"), CBool(bHeatProduct And Not bOnlyProduct).ToString}
        'Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.Bit}
        'Dim paramSize() As Integer = {5, 0}
        'dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procProductOnSaleLube150Select")
        'rowCount = dt.Rows.Count
        'siteInclude.debug("rowCount=" & rowCount)
        'For Each row As DataRow In dt.Rows
        'If InStr(LCase(row("saleName")), "water") > 0 Then
        'water_ID.Value = row("id")
        'water_Name.Value = row("saleName")
        'water_Price.Value = row("saleUnitPrice")
        'water_Discount.Value = row("saleDiscount")
        'water_PriceIncDiscount.Value = row("saleUnitPrice") - row("saleDiscount")
        'water_Vat.Value = row("saleTaxRate")
        'water_Currency.Value = row("saleCurrencyCode")
        'water_DistBuyingID.Value = row("distBuyingID")
        'water_ComponentCode.Value = zeroReplaceNull(row("componentCode"))
        'water_UnitPriceAfterDiscountIncVat.Value = row("unitPriceAfterDiscountIncVat")
        ''Live throws error if componentCode is null, Set it to 0 if NULL is returned
        'If water_ComponentCode.Value = "" Then water_ComponentCode.Value = "0"
        'imgLubeWater.ImageUrl = "/images/products/" & row("saleImageName")
        'lblLubeWaterPrice.Text = FormatNumber(row("unitPriceAfterDiscountIncVat"), 2)
        'ElseIf InStr(LCase(row("saleName")), "silicon") > 0 Then
        'silicon_ID.Value = row("id")
        'silicon_Name.Value = row("saleName")
        'silicon_Price.Value = row("saleUnitPrice")
        'silicon_Discount.Value = row("saleDiscount")
        'silicon_PriceIncDiscount.Value = row("saleUnitPrice") - row("saleDiscount")
        'silicon_Vat.Value = row("saleTaxRate")
        'silicon_Currency.Value = row("saleCurrencyCode")
        'silicon_DistBuyingID.Value = row("distBuyingID")
        'silicon_ComponentCode.Value = zeroReplaceNull(row("componentCode"))
        'silicon_UnitPriceAfterDiscountIncVat.Value = row("unitPriceAfterDiscountIncVat")
        ''Live throws error if componentCode is null, Set it to 0 if NULL is returned
        'If silicon_ComponentCode.Value = "" Then silicon_ComponentCode.Value = "0"
        'imgLubeSilicon.ImageUrl = "/images/products/" & row("saleImageName")
        'lblLubeSiliconPrice.Text = FormatNumber(row("unitPriceAfterDiscountIncVat"), 2)
        'End If
        'Next
        ''If bHeatProduct And Not bOnlyProduct Then
        ''Set the price to 0
        ''water_Price.Value = "0"
        ''water_PriceIncDiscount.Value = "0"
        ''silicon_Price.Value = "0"
        ''silicon_PriceIncDiscount.Value = "0"
        ''End If
        'Catch ex As Exception
        'siteInclude.debug(ex.ToString())
        'siteInclude.addError("product.aspx", "btnAddToBasket_click" & ex.ToString())
        'Finally
        'dt.Dispose()
        'End Try
        'If rowCount = 2 Then
        'Dim cs As ClientScriptManager = Page.ClientScript
        'Dim sNoThanksBtn As String = "No Thanks"
        'If LCase(Session("EBLanguage")) = "be" Or LCase(Session("EBLanguage")) = "nl" Then sNoThanksBtn = "Nee, bedankt"
        'cs.RegisterStartupScript(Me.GetType(), "loade", "self.setTimeout(""setSiliconPrice('" & FormatNumber(silicon_UnitPriceAfterDiscountIncVat.Value, 2) & "');setWaterPrice('" & FormatNumber(water_UnitPriceAfterDiscountIncVat.Value, 2) & "');setLubeText('" & Session("EBLanguage") & "'," & LCase(CBool(bHeatProduct And Not bOnlyProduct)) & ");$('#dialog-form').dialog({ autoOpen: true, height: 420, width: 500, modal: true, buttons: {'" & sNoThanksBtn & "':function() { $(this).dialog('close');}} }); $('#tblModal').removeClass('product-table-modal');"",0);", True)
        'End If
        'End If
        Try
            Dim m As mshop = CType(Master, mshop)
            m.updateBasket()
        Catch ex As Exception

        End Try


    End Sub
    Protected Sub btnAddReview_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        'panProductDescription4b.Visible = False
        dvAddReview.Visible = True
        gvReviewsLite.Visible = False
        'lnkSubMenuReviews.Visible = False
        panReviewsButton.Visible = False
        'tdQ1.Visible = True
        'lnkAddReview.Visible = False
        'tblAddReviewControls.Visible = False
        'tblAddReviewType.Visible = True
        'Set the product name
        'Dim name As String = lblProdName.Text
        'lblProduct1.Text = name
        'lblProduct2.Text = name
        'lblProduct3.Text = name
        'lblProduct4.Text = name
        'lblProduct5.Text = name
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
        'Dim btn As Button = CType(sender, Button)
        'Dim q As Integer = Right(btn.CommandArgument, 1)
        'Dim td As HtmlTableRow = tblAddReview.FindControl("tdQ" & CStr(q))
        'Dim fckControl As Object = tblAddReview.FindControl("fckQ" & CStr(q))
        'Dim lblError As Label = tblAddReview.FindControl("lblErrorQ" & CStr(q))
        'Dim bError As Boolean = False
        'Test for null entry (or > 4000 chars)
        'If fckControl.value = "" Then
        'bError = True
        'lblError.Text = "<font color='red'>You must enter some text.</font>"
        'End If
        'If Len(fckControl.Value) > _maxChars Then
        'bError = True
        'lblError.Text = "<font color='red'>Max " & _maxChars & " characters.  You have used " & Len(fckControl.Value) & "</font>"
        'End If
        'If Not bError Then
        'Hide current question and reveal the next one
        'td.Visible = False
        'If q < 9 Then
        'td = tblAddReview.FindControl("tdQ" & CStr(q + 1))
        'td.Visible = True
        'End If
        'lblError.Text = ""
        'End If
    End Sub
    Protected Sub sqlAddReview_inserted(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        dvAddReview.Visible = False
        lblThankyou.Visible = True
    End Sub
    Protected Sub lnkSubMenuOverview_click(ByVal sender As Object, ByVal e As EventArgs)
        'Show panProductDescription1 (The products details details and overview page)
        'panProductDescription1 is the Overview
        'panProductDescription2 is the html/text driven by the productOnSaleMenu table
        'panProductDescription3 is the sensastions frame
        'panProductDescription4 is the Reviews frame
        'Dim lnkBtn As LinkButton = CType(sender, LinkButton)
        'Dim lnkBtnSubMenu As ImageButton = tblButtons.FindControl(lnkBtn.CommandArgument)
        'lnkBtn.Visible = False
        panBack1.Visible = False
        'lnkBtnSubMenu.Visible = True
        panProductDescription1.Visible = True
        panProductDescription2.Visible = False
        panProductDescription3.Visible = False
        panProductDescription4.Visible = False
        panProductDescription4b.Visible = False
        'Show Add to basket button and the Choose Your Product dropdown
        panRelated.Visible = True
        btnAddToBasket.Visible = True
        panSensationsButton.Visible = True
        panAddToBasket.Visible = True
        panRelatedDropdown.Visible = True
        'Reset from currentViewState
        currentViewState.Value = ""
        siteInclude.debug("Set VIS=True")
    End Sub
    Protected Sub lnkSubMenuSensations_click(ByVal sender As Object, ByVal e As EventArgs)
        panProductDescription1.Visible = False
        panProductDescription2.Visible = False
        panProductDescription3.Visible = True
        panProductDescription4.Visible = False
        'lnkSubMenuSensations.Visible = False 'Hide button that was just clicked
        panSensationsButton.Visible = False
        'lnkBack1.Visible = True 'Show the back to product button
        panBack1.Visible = True
        'Hide the other back to product button and make sure the reviews button is visible - Only if sensations are available for this product
        If CBool(hidReviewsVisible.Value) Then
            'lnkSubMenuReviews.Visible = True
            'panReviewsButton.Visible = True
            lnkBack2.Visible = False
        End If
        'While viewing the Sensations, remove the Add To Basket button from the updateButtons Async Postback list 13-10-08
        '(Else when clicked the atlas postback returns an empty window causing the pasge to look odd and the basket to get split across frame, pretty it aint.)
        Dim t As New PostBackTrigger()
        t.ControlID = "btnAddToBasket"
        updateButtons.Triggers.Add(t)
        'Set the viewstate to 'sensations' so we know which button to hide when the addToBasket button is clicks (As this. button will reappear for some reason when Add To Basket is clicked)
        currentViewState.Value = "sensations"
    End Sub
    Protected Sub lnkSubMenuReviews_click(ByVal sender As Object, ByVal e As EventArgs)
        showReview()
    End Sub
    Protected Sub lnkSubMenu_click(ByVal sender As Object, ByVal e As EventArgs)
        'Generic Sub for all sub menu link buttons
        'Show panel 2, and load html from productOnSaleMenu table using the productOnSaleMenuID stored in the linkbuttons CommandArgs
        Dim lnkBtn As LinkButton = CType(sender, LinkButton)
        panProductDescription1.Visible = False
        panProductDescription2.Visible = True
        lblProductDescription2.Text = getSubTextByID(lnkBtn.CommandArgument)
    End Sub
    Protected Sub lnkBack_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)

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
    Protected Sub lnkLube_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        Dim dSiliconCode As Integer = 65
        Dim dWaterCode As Integer = 66
        'siteInclude.debug("id=" & silicon_ID.Value)
        'siteInclude.debug("name=" & silicon_Name.Value)
        'siteInclude.debug("price=" & silicon_Price.Value)
        'siteInclude.debug("discount=" & silicon_Discount.Value)
        'siteInclude.debug("priceIncDiscount=" & silicon_PriceIncDiscount.Value)
        'siteInclude.debug("vat=" & silicon_Vat.Value)
        'siteInclude.debug("distBuyingID=" & silicon_DistBuyingID.Value)
        'siteInclude.debug("componentCode=" & silicon_ComponentCode.Value)
        Select Case LCase(btn.CommandArgument)
            Case "silicon"
                Profile.EBCart.AddItem(silicon_ID.Value, silicon_Name.Value, CDec(silicon_Price.Value), CDec(silicon_Discount.Value), CDec(silicon_PriceIncDiscount.Value), CDec(silicon_Vat.Value), silicon_DistBuyingID.Value, 0, silicon_ComponentCode.Value)
            Case "water"
                Profile.EBCart.AddItem(water_ID.Value, water_Name.Value, CDec(water_Price.Value), CDec(water_Discount.Value), CDec(water_PriceIncDiscount.Value), CDec(water_Vat.Value), water_DistBuyingID.Value, 0, water_ComponentCode.Value)
        End Select
        'Rebind basket to show added lube
        bindBasket()
    End Sub
    'SelectedIndexChanged
    Protected Sub drpRelated_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.SelectedIndex > 0 Then Response.Redirect(drp.SelectedValue & "&m=" & Request.QueryString("m"))
    End Sub

    'Databinding Events
    Protected Sub gvBasket_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Dim lblProductCode As Label
        For Each row As GridViewRow In gvBasket.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'If lblCurrencySign.Text = "$" Then
                '   row.Cells(_gvBasket_pricePos).Text = "US" & lblCurrencySign.Text & row.Cells(_gvBasket_pricePos).Text
                'Else
                row.Cells(_gvBasket_pricePos).Text = lblCurrencySign.Text & row.Cells(_gvBasket_pricePos).Text
                ' End If
                'If the basket item's SaleProductCOde is 'CustomCharge' then make sure the item cannot be deleted from basket (also dont display as a link, use normal text)
                lblProductCode = row.FindControl("lblProductCode")
                If LCase(lblProductCode.Text = "customs") Then
                    row.Cells(_gvBasket_itemPos).Controls.Clear()
                    row.Cells(_gvBasket_itemPos).Text = "Customs Charge"
                End If
            End If
        Next
        If gv.Rows.Count > 0 Then
            'gv.FooterRow.Cells(_gvBasket_itemPos).Text = CType(GetLocalResourceObject("lblTotalText"), String)
            gv.FooterRow.Cells(_gvBasket_itemPos).Text = "Total:"
            'If lblCurrencySign.Text = "$" Then
            'gv.FooterRow.Cells(_gvBasket_pricePos).Text = "US" & lblCurrencySign.Text & Profile.EBCart.GoodsIncVat
            'Else
            gv.FooterRow.Cells(_gvBasket_pricePos).Text = lblCurrencySign.Text & Profile.EBCart.GoodsIncVat
            'End If

        End If
    End Sub
    Protected Sub dvProductReviews_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Dim dv As DetailsView = CType(sender, DetailsView)
        'Load language resources
        'Dim lblReviewedByText As Label
        'Dim lblRating As Label
        'For Each row As DetailsViewRow In dv.Rows
        'If row.RowType = DataControlRowType.DataRow Then
        'lblReviewedByText = row.FindControl("lblReviewedByText")
        'lblRating = row.FindControl("lblRating")
        'lblReviewedByText.Text = getDBResourceString("lblReviewedByText")
        'showRating(lblRating.Text, row, "dv")
        'End If
        'Next
        'Show total number of product reviews, and avg score
        'showReviewStats()
        'If current review is of the old style, then hide the new info (and vice verca)
        'It will be new style if the db field 'review'=""
        'If False Then
        'Dim lbl As Label = dvProductReviews.FindControl("lblReview")
        'If lbl.Text = "" Then
        ' Dim row As DetailsViewRow
        ' For iLoop As Integer = _dvReviewNewStartRow To _dvReviewNewEndRow
        'row = dv.Rows(iLoop)
        'row.Visible = True
        'Next
        'dvProductReviews.Rows(_dvReviewOldRow).Visible = False
        'Else
        'Dim row As DetailsViewRow
        'For iLoop As Integer = _dvReviewNewStartRow To _dvReviewNewEndRow
        'row = dv.Rows(iLoop)
        'row.Visible = False
        'Next
        'dvProductReviews.Rows(_dvReviewOldRow).Visible = True
        'End If
        'End If
        'lblPager.Text = "Review " & dvProductReviews.PageIndex + 1 & " of " & dvProductReviews.PageCount
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
    Protected Sub drpRelated_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.Items.Count < 2 Then panRelated.Visible = False
        drp.Items(0).Text = addDetails_Name.Value
    End Sub


    'Subs & Functions
    Protected Sub showReview()
        panProductDescription1.Visible = False
        panProductDescription2.Visible = False
        panProductDescription3.Visible = False
        panProductDescription4.Visible = True
        panProductDescription4b.Visible = True
        gvReviewsLite.Visible = True
        dvAddReview.Visible = False
        'Hide Add to basket button and the Choose Your Product dropdown
        panRelated.Visible = False
        btnAddToBasket.Visible = False
        'lnkSubMenuReviews.Visible = False 'Hide button that was just clicked
        panReviewsButton.Visible = False
        lnkBack2.Visible = True 'Show the back to product button
        'Hide the other back to product button and make sure the sensations button is visible - Only if sensations are available for this product
        If CBool(hidSensationsVisible.Value) Then
            'lnkSubMenuSensations.Visible = True
            panSensationsButton.Visible = True
            'lnkBack1.Visible = False
            panBack1.Visible = False
        End If
        'Set the viewstate to 'sensations' so we know which button to hide when the addToBasket button is clicks (As this. button will reappear for some reason when Add To Basket is clicked)
        currentViewState.Value = "reviews"
        'loadDBResources for all controls that sit within the dvAddReview
        Dim btnAdd As ImageButton = dvAddReview.FindControl("btnSubmitReview")
        Dim lblAddReviewInstructionsText As Label = dvAddReview.FindControl("lblAddReviewInstructionsText")
        Dim drpRating As DropDownList = dvAddReview.FindControl("drpRating")
        lblAddReviewInstructionsText.Text = getDBResourceString("lblAddReviewInstructionsText")
        drpRating.Items(0).Text = getDBResourceString("Review_Poor")
        drpRating.Items(1).Text = getDBResourceString("Review_Average")
        drpRating.Items(2).Text = getDBResourceString("Review_Good")
        drpRating.Items(3).Text = getDBResourceString("Review_VeryGood")
        drpRating.Items(4).Text = getDBResourceString("Review_Excellent")
        dvAddReview.Rows(siteInclude.getDVRowByHeader(dvAddReview, "Your Name:")).Cells(0).Text = getDBResourceString("Review_YourName")
        dvAddReview.Rows(siteInclude.getDVRowByHeader(dvAddReview, "Score:")).Cells(0).Text = getDBResourceString("Review_Score")
        dvAddReview.Rows(siteInclude.getDVRowByHeader(dvAddReview, "Your Age:")).Cells(0).Text = getDBResourceString("Review_YourAge")
        dvAddReview.Rows(siteInclude.getDVRowByHeader(dvAddReview, "Review:")).Cells(0).Text = getDBResourceString("Review_Review")
        Dim tmp As String = ""
        tmp = trimCrap(getDBResourceString("imgSubmitReview"))
        If tmp <> "" Then btnAdd.ImageUrl = tmp 'Submit
        tmp = trimCrap(getDBResourceString("imgAddReview"))
        If tmp <> "" Then btnAddReview.ImageUrl = tmp 'Add
        gvReviewsLite.EmptyDataText = getDBResourceString("NoReviewsFound")
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
            .Parameters("@currency").Value = addDetails_Currency.Value
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
                    'lnkBtn = _content.FindControl(CStr("lnkSubMenu" & iPos))
                    lnkBtn = FindControl(CStr("lnkSubMenu" & iPos))
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
                lblCurrentReviewsText2.Text = ""
            Else
                lblCurrentReviewsText1.Text = getDBResourceString("lblCurrentReviewsText1")
                lblCurrentReviewsText2.Text = getDBResourceString("lblCurrentReviewsText2")
                lblNumberOfReviews.Text = ds.Tables(0).Rows(0)("qty")
                If lblNumberOfReviews.Text = "0" Then
                    lblCurrentReviewsText1.Visible = False
                    lblCurrentReviewsText2.Visible = False
                    lblNumberOfReviews.Visible = False
                End If
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
            .Parameters("@countryCode").Value = Session("EBShopCountry")
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
                'Populate unique Page Title and meta tags for selected product
                If Not IsDBNull(rs("pageTitle")) Then If rs("pageTitle") <> "" Then Page.Title = rs("pageTitle")
                Dim metaDescription As HtmlControl = Master.FindControl("metaDescription")
                Dim metaKeywords As HtmlControl = Master.FindControl("metaKeywords")
                If Not IsDBNull(rs("metaDescription")) Then If rs("metaDescription") <> "" Then metaDescription.Attributes.Add("content", rs("metaDescription"))
                If Not IsDBNull(rs("metaKeywords")) Then If rs("metaKeywords") <> "" Then metaKeywords.Attributes.Add("content", rs("metaKeywords"))
                'Populate hidden fields, this will save a DB call when user adds product to basket
                addDetails_ID.Value = rs("id")
                addDetails_Name.Value = rs("saleName")
                If Not IsDBNull(rs("saleImage1Small")) Then addDetails_BasketImageName.Value = rs("saleImage1Small")
                addDetails_Price.Value = rs("unitPriceAfterDiscount")
                addDetails_Discount.Value = "0.00"
                addDetails_PriceIncDiscount.Value = rs("unitPriceAfterDiscount")
                addDetails_Vat.Value = rs("saleTaxRate")
                addDetails_Currency.Value = rs("saleCurrencyCode")
                addDetails_DistBuyingID.Value = rs("distBuyingID")
                litProductNameReview.Text = rs("saleName")
                If Not CBool(rs("extraImageVertical")) Then lblShortDescription.Text = "<br>"
                lblShortDescription.Text = lblShortDescription.Text & rs("saleShortDescription")
                If Not IsDBNull(rs("reviewImageName")) Then
                    'Some older products had an image directly added to the db, if it exists then use it
                    imgProductReview.ImageUrl = rs("reviewImageName")
                Else
                    'Otherwise use the products Deartment Image
                    imgProductReview.ImageUrl = "~/images/products/" & rs("saleImageName")
                End If
                lblPrice.Text = FormatNumber(rs("finalUnitPrice"), 2)
                lblPriceAfterDiscountIncVat.Text = rs("unitPriceAfterDiscountIncVat")
                If lblPrice.Text > lblPriceAfterDiscountIncVat.Text Then
                    'Show the discount price if item has a discount
                    panDiscount.Visible = True
                    litNowText.Visible = True
                End If
                If Not IsDBNull(rs("componentCode")) Then addDetails_componentCode.Value = rs("componentCode")
                'Show main details on screen
                lblProdName.Text = rs("saleName")
                If Not IsDBNull(rs("saleDescription")) Then lblProdDescription.Text = rs("saleDescription")
                'If Not IsDBNull(rs("saleImageName")) Then loadImages(rs("id"), rs("saleImageName"), oConn)
                'If LCase(rs("itemType")) = "bproduct" Then
                'lnkSubMenuSensations.Visible = CBool(rs("showSensations"))
                panSensationsButton.Visible = CBool(rs("showSensations"))
                hidSensationsVisible.Value = CStr(rs("showSensations"))
                If Not IsDBNull(rs("currencySign")) Then
                    If rs("currencySign") = "$" Then
                        lblCurrencySign.Text = "US" & rs("currencySign")
                        lblCurrencySign2.Text = "US" & rs("currencySign")
                        'lblCurrencySign3.Text = "US" & rs("currencySign")
                        'lblCurrencySign4.Text = "US" & rs("currencySign")
                    Else
                        lblCurrencySign.Text = rs("currencySign")
                        lblCurrencySign2.Text = rs("currencySign")
                        'lblCurrencySign3.Text = rs("currencySign")
                        'lblCurrencySign4.Text = rs("currencySign")
                    End If
                End If
                'Show the products submenu in the left hands frame
                'showSubMenu(rs("id"))
                'Remove sensations link if 'showSensations=0' in the productOnSale table
                'trSensations.Visible = CBool(rs("showSensations"))
                'imgHeatLogo.Visible = CBool(rs("showSensations"))
                'trUnderlineSensations.Visible = CBool(rs("showSensations"))
                'Remove Reviews link if 'showReviews=-' in the productOnSale table
                'lnkSubMenuReviews.Visible = CBool(rs("showReviews"))
                'panReviewsButton.Visible = CBool(rs("showReviews"))
                hidReviewsVisible.Value = CStr(rs("showReviews"))
                'trUnderlineReviews.Visible = CBool(rs("showReviews"))
                'If vat is 0% then hide the '(inc x% vat)' text
                If addDetails_Vat.Value = "0.0" Then
                    lblVatText1.Visible = False
                    lblVatText2.Visible = False
                Else
                    lblVatRate.Text = Replace(addDetails_Vat.Value, ".0", "") & "%"
                End If
                litDiscount.Text = FormatNumber(rs("saleDiscount"))
                'Add the main image in a meta tag for facebook
                Try
                    'Dim m As mshop = CType(Master, mshop)
                    'm.AddToHeader("http://emotionalbliss.co.uk/images/products/" & rs("saleImageName"))
                Catch ex As Exception
                    siteInclude.debug(ex.ToString())
                End Try
                'Show product images
                If CBool(rs("useAutoImages")) Then
                    'Automatically display the images the user has uploaded and make a javascript roll over effect
                    imgMain.ImageUrl = "~/images/products/" & rs("saleImageName")

                    Dim imgThumb1 As Image
                    Dim imgThumb2 As Image
                    Dim imgThumb3 As Image
                    If CBool(rs("extraImageVertical")) Then
                        'Show Vertical images
                        imgThumb1 = imgImage1SmallV
                        imgThumb2 = imgImage2SmallV
                        imgThumb3 = imgImage3SmallV
                    Else
                        'Show Horizontal images
                        imgThumb1 = imgImage1SmallH
                        imgThumb2 = imgImage2SmallH
                        imgThumb3 = imgImage3SmallH
                    End If
                    'Point image holders to image locaiton
                    If Not IsDBNull(rs("saleImage1Small")) And Not IsDBNull(rs("saleImage1")) Then
                        imgThumb1.ImageUrl = "~/images/products/" & rs("saleImage1Small")
                        imgThumb1.Visible = True
                        imgThumb1.Attributes.Add("onMouseOver", imgMain.ClientID & ".src=i1.src")
                        imgThumb1.Attributes.Add("onMouseOut", imgMain.ClientID & ".src='/images/products/" & rs("saleImageName") & "'")
                    End If
                    If Not IsDBNull(rs("saleImage2Small")) And Not IsDBNull(rs("saleImage2")) Then
                        imgThumb2.ImageUrl = "~/images/products/" & rs("saleImage2Small")
                        imgThumb2.Visible = True
                        imgThumb2.Attributes.Add("onMouseOver", imgMain.ClientID & ".src=i2.src")
                        imgThumb2.Attributes.Add("onMouseOut", imgMain.ClientID & ".src='/images/products/" & rs("saleImageName") & "'")
                    End If
                    If Not IsDBNull(rs("saleImage3Small")) And Not IsDBNull(rs("saleImage3")) Then
                        imgThumb3.ImageUrl = "~/images/products/" & rs("saleImage3Small")
                        imgThumb3.Visible = True
                        imgThumb3.Attributes.Add("onMouseOver", imgMain.ClientID & ".src=i3.src")
                        imgThumb3.Attributes.Add("onMouseOut", imgMain.ClientID & ".src='/images/products/" & rs("saleImageName") & "'")
                    End If
                    'Init Javascript image code
                    Dim cs As ClientScriptManager = Page.ClientScript
                    Dim js As String = "i1 = new Image;i1.src = '/images/products/" & rs("saleImage1") & "';" & Chr(10) & "i2 = new Image;i2.src = '/images/products/" & rs("saleImage2") & "';" & Chr(10) & "i3 = new Image;i3.src = '/images/products/" & rs("saleImage3") & "';"
                    cs.RegisterStartupScript(Me.GetType(), "imgload", js, True)
                Else
                    'Images will be controlled direcdtly by user in the html/wysiwyg
                    spanAutoImages.Visible = False
                End If
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
                Dim si As New siteInclude
                si.addError("shop/product.aspx.vb", "bindBasket(); " & ex.ToString)
                si = Nothing
            End Try
        End If
    End Sub
    Protected Function FormatReview(ByVal o As Object)
        Dim result As String = ""
        If Not IsDBNull(o) Then
            result = Replace(o.ToString, Chr(10), "<br>")
        End If
        Return result
    End Function
    Protected Sub loadDBResources()
        Dim tmp As String = ""
        lblThankyou.Text = getDBResourceString("lblThankyou")
        lblFieldsMandatory.Text = getDBResourceString("lblFieldsMandatory")
        lblMessage.Text = getDBResourceString("lblMessage")
        lblRecipient.Text = getDBResourceString("lblRecipient")
        lblYourName.Text = getDBResourceString("lblYourName")
        lblRecipientEmail.Text = getDBResourceString("lblRecipientEmail")
        lblAnonymous.Text = getDBResourceString("lblAnonymous")
        lblYourEmail.Text = getDBResourceString("lblYourEmail")
        lblVatText1.Text = getDBResourceString("lblVatText1")
        lblVatText2.Text = trimCrap(getDBResourceString("lblVatText2"))
        lblVatText3.Text = getDBResourceString("lblVatText1")
        lblVatText4.Text = trimCrap(getDBResourceString("lblVatText2"))
        litWasText.Text = getDBResourceString("litWasText")
        litDiscountText.Text = getDBResourceString("litDiscountText")
        litNowText.Text = getDBResouceString("litNowText")
        tmp = trimCrap(getDBResourceString("cssAddToBasket"))
        If tmp <> "" Then btnAddToBasket.CssClass = tmp
        If tmp <> "" Then lnkAddToBasket.CssClass = tmp
        tmp = trimHTML(getDBResourceString("lnkSubMenuReviews"))
        If tmp <> "" Then lnkSubMenuReviews.Text = tmp
        tmp = trimHTML(getDBResourceString("lnkSubMenuSensations"))
        If tmp <> "" Then lnkSubMenuSensations.Text = tmp
        tmp = trimCrap(getDBResourceString("imgAddToBasket"))
        'If tmp <> "" Then btnAddToBasket.ImageUrl = tmp
        If tmp <> "" Then
            btnAddToBasket2.ImageUrl = tmp
        End If
        'tmp = trimCrap(getDBResourceString("imgSubMenuReviews"))
        'If tmp <> "" Then lnkSubMenuReviews.ImageUrl = tmp
        'tmp = trimCrap(getDBResourceString("imgSubMenuSensations"))
        'If tmp <> "" Then lnkSubMenuSensations.Text = tmp
        tmp = trimCrap(getDBResourceString("imgBackToProd"))
        'If tmp <> "" Then lnkBack1.ImageUrl = tmp
        'If tmp <> "" Then lnkBack2.ImageUrl = tmp
        tmp = trimCrap(getDBResourceString("lnkCheckout"))
        If tmp <> "" Then litCheckout.Text = tmp
        'Validators
        panVoucherFormReqTxtMessage.ErrorMessage = getDBResourceString("errorRequired")
        panVoucherFormReqTxtRecipientName.ErrorMessage = getDBResourceString("errorRequired")
        panVoucherFormReqPurchaseName.ErrorMessage = getDBResourceString("errorRequired")
        panVoucherFormTxtRecipientEmail.ErrorMessage = getDBResourceString("errorRequired")
        regTxtRecipientEmail.ErrorMessage = getDBResourceString("errorInvalidEmail")
        panVoucherFormTxtPurchserEmail.ErrorMessage = getDBResourceString("errorRequired")
        regTxtPurchaserEmail.ErrorMessage = getDBResourceString("errorInvalidEmail")
    End Sub
    Protected Sub setBasketAjaxTrigger()
        'AddToBasket button - ShoppingBasket GridView
        If False Then
            Dim trigger As New AsyncPostBackTrigger()
            Dim c As LinkButton
            trigger.EventName = "Click"
            trigger.ControlID = btnAddToBasket.UniqueID
            'updateBasket.Triggers.Add(trigger)
            If False Then
                'SubMenu LinkButtons - Product Content
                trigger = New AsyncPostBackTrigger()
                trigger.EventName = "Click"
                'trigger.ControlID = lnkSubMenuOverview.UniqueID
                updateProdDesc.Triggers.Add(trigger)
                For iLoop As Integer = 1 To 10
                    trigger = New AsyncPostBackTrigger()
                    trigger.EventName = "Click"
                    c = FindControl("lnkSubMenu" & iLoop.ToString)
                    trigger.ControlID = c.UniqueID
                    updateProdDesc.Triggers.Add(trigger)
                Next
            End If
        End If
    End Sub
    Protected Sub testBasketCountry()
        'Set cart/shop country
        If Session("EBShopCountry") <> Profile.EBCart.ShopCountry Then
            Profile.EBCart.emptyBasket()
            Profile.EBCart.ShopCountry = Session("EBShopCountry")
            'Norway and Switzerland have a £25 Customs Tax, add to the basket as an item.

            Dim d As New DataTable
            Try
                Dim param() As String = {"@saleProdCode", "@countryCode"}
                Dim paramValue() As String = {"customs", Session("EBShopCountry")}
                Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.VarChar}
                Dim paramSize() As Integer = {50, 5}
                d = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procProductOnSaleByProductCodeCountryCodeSelect")
                If d.Rows.Count > 0 Then
                    'Profile.EBCart.AddItem(d.Rows(0)("id"), "Customs Charge", 25, 0, 25, 0, 0, 0, "customs")
                End If
            Catch ex As Exception
                siteInclude.addError("shop/product.aspx", "testBasketCountry(Session(EBShopCountry)=" & Session("EBShopCountry") & ")" & ex.ToString())
            Finally
                d.Dispose()
            End Try
        End If
        'Now check that the id passed actually belongs to that country - If it doesnt set the basket/shop to the country belonging to the productID
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@id"}
            Dim paramValue() As String = {Request.QueryString("id").ToString}
            Dim paramType() As SqlDbType = {SqlDbType.Int}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procProductOnSaleByIdCountrySelect")
            If dt.Rows.Count > 0 Then
                If LCase(Profile.EBCart.ShopCountry) <> LCase(dt.Rows(0)("saleCountryCode")) Then
                    Profile.EBCart.emptyBasket()
                    Profile.EBCart.ShopCountry = dt.Rows(0)("saleCountryCode")
                    Session("EBShopCountry") = dt.Rows(0)("saleCountryCode")
                End If
            End If
        Catch ex As Exception
            siteInclude.addError("shop/product.aspx.vb", "testBasketCountry(); " & ex.ToString)
        Finally
            dt.Dispose()
        End Try
    End Sub
    Protected Sub autoAddToBasket()
        'Customer has come from external site with a product link. Add and redirect to basket.
        If IsNumeric(Request.QueryString("product")) And Request.QueryString("lang") <> "" And Request.QueryString("shop") <> "" Then
            Profile.EBCart.emptyBasket()
            Session("EBLanguage") = Request.QueryString("lang")
            Session("EBShopCountry") = Request.QueryString("shop")
            Dim dt As New DataTable
            Dim weight As Integer = _defaultWeight
            If Request.QueryString("product") = "588" Then weight = 1
            Try
                Dim param() As String = {"@id", "@countryCode"}
                Dim paramValue() As String = {Request.QueryString("product"), Request.QueryString("shop")}
                Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar}
                Dim paramSize() As Integer = {0, 5}
                dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procProductOnSaleByIdSelect")
                If dt.Rows.Count > 0 Then
                    Dim row As DataRow = dt.Rows(0)
                    addDetails_ID.Value = row("id")
                    addDetails_Name.Value = row("saleName")
                    addDetails_Price.Value = row("unitPriceAfterDiscount")
                    addDetails_Discount.Value = "0.00"
                    addDetails_PriceIncDiscount.Value = row("unitPriceAfterDiscount")
                    addDetails_Vat.Value = row("saleTaxRate")
                    addDetails_Currency.Value = row("saleCurrencyCode")
                    addDetails_DistBuyingID.Value = row("distBuyingID")
                    Profile.EBCart.AddItem(addDetails_ID.Value, addDetails_Name.Value, CType(addDetails_Price.Value, Decimal), CType(addDetails_Discount.Value, Decimal), CType(addDetails_PriceIncDiscount.Value, Decimal), CType(addDetails_Vat.Value, Decimal), CType(addDetails_DistBuyingID.Value, Integer), weight, 0)
                    Response.Redirect("basket.aspx")
                Else

                End If
            Catch ex As Exception
                siteInclude.addError("", "" & ex.ToString())
            Finally
                dt.Dispose()
            End Try
        End If
    End Sub
    Protected Function trimCrap(ByVal fck As String) As String
        Dim result As String = Replace(fck, "<p>", "")
        result = Replace(result, "</p>", "")
        Return result
    End Function
    Protected Function trimHTML(ByVal fck As String) As String
        Return Regex.Replace(fck, "<(.|\n)*?>", String.Empty)
    End Function
    Protected Function isVibe(ByVal productName As String) As Boolean
        Dim bResult As Boolean = False
        If InStr(LCase(productName), "womolia") > 0 Then bResult = True
        If InStr(LCase(productName), "femblossom") > 0 Then bResult = True
        If InStr(LCase(productName), "isis") > 0 Then bResult = True
        If InStr(LCase(productName), "chandra") > 0 Then bResult = True
        Return bResult
    End Function
    Protected Function zeroReplaceNull(ByVal o As Object) As String
        Dim result As String = "0"
        If Not IsDBNull(o) Then result = o.ToString
        Return result
    End Function

    'Properties

End Class



