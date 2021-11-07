Imports System.Data
Imports System.Data.SqlClient

Partial Class MasterPageShop
    Inherits System.Web.UI.MasterPage
    Private bSetResTo1024 As Boolean = False
    Private bStaticWidth As Boolean = False
    Private Const _maxImages As Integer = 8
    Private Const _setAffiliatePricePos As Integer = 2
    Private Const _setShopPricePos As Integer = 3
    Private Const _setClickThroughPos As Integer = 3
    Private Const _departmentsPos As Integer = 5
    Private Const _purchaseManagementPos As Integer = 6
    Private Const _scanPos As Integer = 7
    Private Const _affLinkPos As Integer = 8
    Private Const _affApprovePos As Integer = 9
    Private Const _gvBasket_itemPos As Integer = 1
    Private Const _gvBasket_pricePos As Integer = 3

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init

        If Request.QueryString("forceLang") <> "" Then Session("EBLanguage") = Request.QueryString("forceLang")
        If Request.QueryString("language") <> "" Then Session("EBLanguage") = Request.QueryString("language")
        If Session("EBLanguage") = "" Then
            If Profile.country <> "" Then
                Session("EBLanguage") = Profile.country
                Session("EBShopCountry") = Profile.country
            Else
                Session("EBlanguage") = "gb"
                Session("EBShopCountry") = "gb"
            End If
        End If
        Membership.Provider.ApplicationName = "EBAffProvider"
        If Profile.country = "" Then Profile.country = Session("EBLanguage")
        If CStr(Session("EBMenuIndex")) = "" Then Session("EBMenuIndex") = "1"
        'Check quesrystring for news menuIndex (This will occur if user has come from another site via an affilaite link and may be heading directly to a static page)
        If Request.QueryString("m") <> "" Then updateMenuIndexFromDB(Request.QueryString("m"))
        If Session("EBShopCurrency") = "" Then
            Session("EBShopCurrency") = "gbp"
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblTester.Text = "Language=" & Session("EBLanguage") & ", Shop=" & Session("EBShopCountry") & ", Currency=" & Session("EBShopCurrency")
        Page.Title = "Emotional Bliss Online Shop"
        'Make top navigation map (for Home/Shopping Basket/Contact Us/Track Order
        mapNav.ImageUrl = "~/images/NavImages/Nav_Menu_" & Session("EBLanguage") & ".jpg"
        applyMapFromDB(Session("EBMenuIndex"), Session("EBLanguage"))
        preCacheImages()
        If Page.IsPostBack Then
        Else
            If Session("EBAffMenu") = "" Then Session("EBAffMenu") = "order"
        End If
        'Retrieve Language dropdown state Session
        'If no menu dept session exists, create one
        If CType(Session("EBMenuDept"), String) = "" Then Session("EBMenuDept") = "1"
        'If user is an affiliate then show the Affiliate Menu
        If Session("EBAffID") <> "" Then
            'Set affiliate page title for all affiliates
            'Page.Title = "Emotional Bliss Affiliate Section"
            'Make the affiliates menu/header section visible
            tblAffiliate.Visible = True
            lblUsername.Text = Session("EBAffUsername")
            'Retrieve affMenu dropdown state Session
            drpAffMenu.SelectedValue = Session("EBAffMenuValue")
            'Is user Affiliate or Distributor
            If CType(Session("EBAffEBDistributor"), Boolean) Or CType(Session("EBAffEBUser"), Boolean) Then
                drpAffMenu.Visible = False
                'drpDistMenu.Visible = True
                'Make Distributor TopNavbar
                'makeTopNavbar()
                'If Session("EBDistMenuValue") <> "" Then loadDistNavbar(Session("EBDistMenuValue"))
                showDistributorTopMenu()
            Else
                'drpDistMenu.Visible = False
                drpAffMenu.Visible = True
                lblQuickSearch.Visible = False
                txtQuickSearch.Visible = False
            End If
        End If
        If Not Page.IsPostBack Then
            rotateLinks()
        End If
        If Application("isDevBox") Then
            lblSiteFooter.Text = "EB Dev Box"
        Else
            lblSiteFooter.Text = "Copyright " & Year(Now()) & " Emotional Bliss"
            Dim js As String = ""
            js = "<script type=""text/javascript"" src=""https://cetrk.com/pages/scripts/0008/9681.js""> </script>" & Chr(10)
            js = js & "<script type=""text/javascript"">" & Chr(10)
            js = js & "var gaJsHost = ((""https:"" == document.location.protocol) ? ""https://ssl."" : ""http://www."");" & Chr(10)
            js = js & "document.write(unescape(""%3Cscript src='"" + gaJsHost + ""google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E""));" & Chr(10)
            js = js & "</script>" & Chr(10)
            js = js & "<script type=""text/javascript"">" & Chr(10)
            js = js & "var pageTracker = _gat._getTracker(""UA-4553866-1"");" & Chr(10)
            js = js & "pageTracker._initData();" & Chr(10)
            js = js & "pageTracker._trackPageview();" & Chr(10)
            js = js & "</script>" & Chr(10)
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "trackerJS", js, False)
        End If
        If Not Page.IsPostBack Then If LCase(Session("EBShopCountry")) = "gb" Or LCase(Session("EBShopCountry")) = "nl" Or LCase(Session("EBShopCountry")) = "ie" Then drpShopCountry.SelectedValue = LCase(Session("EBShopCountry")) & "," & LCase(Session("EBShopCurrency"))
    End Sub
    Protected Function getPageName() As String
        Dim sResult As String
        Dim arr As Array = Split(Request.ServerVariables("HTTP_REFERER"), "/")
        sResult = LCase(arr(UBound(arr)))
        Return sResult
    End Function
    Private Function getCurrency(ByVal countryCode As String) As String
        Dim result As String = "gbp"
        Dim oConn As New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New System.Data.SqlClient.SqlCommand("procCountryByCountryCodeCurrencySelect", oConn)
        Dim da As New System.Data.SqlClient.SqlDataAdapter
        Dim ds As New System.Data.DataSet
        With oCmd
            .CommandType = Data.CommandType.StoredProcedure
            .Parameters.Add(New System.Data.SqlClient.SqlParameter("@countryCode", Data.SqlDbType.VarChar))
            .Parameters("@countryCode").Value = countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New System.Data.SqlClient.SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0)("currencyCode")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("MasterPageShop.aspx.vb", "getCurrency(" & countryCode & ")::" & ex.ToString)
            si = Nothing
        End Try
        Return result
    End Function
    Protected Sub applyMapFromDB(ByVal i As String, ByVal lang As String)
        Dim dt As DataTable = loadMapDataTable(i, lang)
        Dim tCell As TableCell
        Dim tRow As New TableRow
        Dim lnk As LinkButton
        Dim bFirst As Boolean = True
        Dim iLoop As Integer = 1
        tRow.Height = 22
        If dt.Rows.Count > 0 Then
            mapNav.HotSpots.Clear() 'Remove all hotspots
            For Each row As DataRow In dt.Rows
                Dim rect As New RectangleHotSpot
                If LCase(row("type")) = "parent" Then
                    'Add aprent as postback
                    rect.HotSpotMode = HotSpotMode.PostBack
                    rect.PostBackValue = row("index") & "|" & row("url") & "|" & row("navText") & "|main"
                    'Added 30-5-08 for new text based menu
                    'Show Leftmenu img (if menuItem is selected)
                    If CStr(iLoop) = Session("EBMenuIndex") Then
                        tCell = New TableCell
                        tCell.Width = "25"
                        tCell.Text = "<img src='../images/menuL.gif'>"
                        tRow.Cells.Add(tCell)
                    Else
                        If bFirst Then
                            tCell = New TableCell
                            tCell.Width = "25"
                            tCell.Text = "&nbsp;"
                            tRow.Cells.Add(tCell)
                        End If
                    End If
                    'Show menuitem text and link
                    lnk = New LinkButton
                    If Not IsDBNull(row("menuName")) Then lnk.Text = UCase(row("menuName"))
                    lnk.CommandArgument = row("url") & "|" & iLoop
                    lnk.CssClass = "menuitem"
                    lnk.Font.Bold = True
                    If CStr(iLoop) = Session("EBMenuIndex") Then
                        lnk.ForeColor = Drawing.Color.White
                    Else
                        lnk.ForeColor = Drawing.Color.Gray
                    End If
                    AddHandler lnk.Click, AddressOf menuItem_Click
                    tCell = New TableCell
                    tCell.Controls.Add(lnk)
                    tCell.HorizontalAlign = HorizontalAlign.Center
                    If CStr(iLoop) = Session("EBMenuIndex") Then tCell.BackColor = System.Drawing.ColorTranslator.FromHtml("#9c9c9c")
                    tRow.Cells.Add(tCell)
                    'Show rightmenu img (if menuitem is slected, else just show blank space)
                    If CStr(iLoop) = Session("EBMenuIndex") Then
                        tCell = New TableCell
                        tCell.Width = "25"
                        tCell.Text = "<img src='../images/menuR.gif'>"
                        tRow.Cells.Add(tCell)
                    Else
                        If CStr(iLoop) <> Session("EBMenuIndex") - 1 Then
                            tCell = New TableCell
                            tCell.Width = "25"
                            tCell.Text = "&nbsp;"
                            tRow.Cells.Add(tCell)
                        End If
                    End If
                    iLoop = iLoop + 1
                    bFirst = False
                Else
                    'Add chid as navigate   
                    rect.HotSpotMode = HotSpotMode.PostBack
                    If Not (IsDBNull(row("index")) Or IsDBNull(row("url")) Or IsDBNull(row("navText"))) Then rect.PostBackValue = row("index") & "|" & row("url") & "|" & row("navText") & "|sub"
                End If
                rect.Left = row("left")
                rect.Bottom = row("bottom")
                rect.Right = row("right")
                rect.Top = row("top")
                mapNav.HotSpots.Add(rect)
            Next
            'Add print link
            lnk = New LinkButton
            lnk.OnClientClick = "printMainFrm();return false;"
            lnk.Text = "PRINT PAGE"
            lnk.CssClass = "menuitem"
            lnk.Font.Bold = True
            lnk.ForeColor = Drawing.Color.Gray
            tCell = New TableCell
            tCell.Controls.Add(lnk)
            tRow.Cells.Add(tCell)
            tblMenu.Rows.Add(tRow)
        End If
        'Show navText
        If Session("EBMenuSection") = "" Then
            'Nowt in session, Initiallize to default page
            Session("EBMenuURL") = "default.aspx"
            'Session("EBMenuSubURL") = "default.aspx"
            Session("EBMenuSection") = "Home"
            'Session("EBMenuSubSection") = "About EB"
        End If
        If Session("EBMenuSubURL") <> "" Then lblMenuSeperator.Visible = True
        'lnkSection.NavigateUrl = Session("EBMenuURL")
        lnkSection.Text = Session("EBMenuSection")
        'lnkSubSection.NavigateUrl = Session("EBMenuSubURL")
        lnkSubSection.Text = Session("EBMenuSubSection")
    End Sub
    Protected Sub menuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        'User has clicked a main menu item
        Dim lnkBtn As LinkButton = CType(sender, LinkButton)
        Dim menuIndex As Integer
        Dim url As String
        Dim aTmp As Array = Split(lnkBtn.CommandArgument, "|")
        menuIndex = aTmp(1)
        url = aTmp(0)
        'Set the session with the index of the menu item
        Session("EBMenuIndex") = CStr(menuIndex)
        Response.Redirect(url)
    End Sub
    Protected Function loadMapDataTable(ByVal i As String, ByVal lang As String) As DataTable
        Dim dt As DataTable
        If Cache("EBImageMap" & CStr(i) & lang) Is Nothing Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procImageMapByIndexSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim dep As SqlCacheDependency
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@Index", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@section", SqlDbType.VarChar, 10))
                .Parameters("@Index").Value = CType(i, Integer)
                .Parameters("@countryCode").Value = lang
                .Parameters("@section").Value = "menu"
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                dep = New SqlCacheDependency("emotionalbliss", "imageMaps")
                dt = ds.Tables(0)
                Dim tsExpires As New TimeSpan(0, 0, 5, 0)
                Cache.Add("EBImageMap" & CStr(i) & lang, dt, Nothing, Caching.Cache.NoAbsoluteExpiration, tsExpires, CacheItemPriority.Normal, Nothing)
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Else
            dt = CType(Cache("EBImageMap" & i & lang), DataTable)
        End If
        Return dt
    End Function
    Protected Sub drpAffMenu_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Response.Write("drpAffMenu databound()<br>")
    End Sub
    Protected Sub lnkButtonChangeDept(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Session("EBMenuDept") = CType(lnk.CommandArgument, String)
        Response.Redirect("/" & lnk.CommandName)
    End Sub
    Protected Sub lnkLogout_click(ByVal sender As Object, ByVal e As EventArgs)
        logAffiliateOut()
    End Sub
    Protected Sub drpAffMenu_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Write("Setting Session to Index of " & drpAffMenu.SelectedIndex & "<br>")
        'Session("EBAffMenuIndex") = drpAffMenu.SelectedIndex
        Session("EBAffMenuValue") = drpAffMenu.SelectedValue
        Select Case drpAffMenu.SelectedValue
            Case "password"
                Response.Redirect("~/affiliates/changePassword.aspx")
            Case "order"
                Response.Redirect("~/affiliates/basket.aspx")
            Case "statement"
                Response.Redirect("~/affiliates/statement.aspx")
            Case "orders"
                Response.Redirect("~/affiliates/affOrderView.aspx")
            Case "setprice"
                Response.Redirect("~/affiliates/affiliateProducts.aspx")
            Case "setshop"
                Response.Redirect("~/affiliates/shopProducts.aspx")
            Case "depts"
                Response.Redirect("~/affiliates/departments.aspx")
            Case "po"
                Response.Redirect("~/affiliates/purchaseManagement.aspx")
            Case "scan"
                Response.Redirect("~/affiliates/scan.aspx")
            Case "link"
                Response.Redirect("~/affiliates/affLink.aspx")
            Case "click"
                Response.Redirect("~/affiliates/affiliateClick.aspx")
            Case "app"
                Response.Redirect("~/affiliates/affiliateList.aspx")
            Case "logout"
                Response.Redirect("~/affiliates/logout.aspx")
        End Select
    End Sub
    Protected Sub drpAffMenu_load(ByVal sender As Object, ByVal e As EventArgs)
        drpAffMenu.SelectedValue = Session("EBAffMenuValue")
        'Response.Write("Running load<br>")
        'Response.Write("Current index is " & Session("EBAffMenuIndex") & "<br>")
        'drpAffMenu.SelectedIndex = Session("EBAffMenuIndex")
        'Response.Write("Running load<br>")
    End Sub
    Protected Sub logAffiliateOut()
        Session("EBAffUsername") = Nothing
        Session("EBAffID") = Nothing
        Session("EBAffCountryCode") = Nothing
        Session("EBAffCurrencyCode") = Nothing
        Session("EBAffEBDistributor") = Nothing
        Session("EBAffEBDistributorCountryCode") = Nothing
        Session("EBAffMenuValue") = Nothing
        Session("EBDistMenuValue") = ""
        Dim myCookie As New HttpCookie("affSetting")
        myCookie.Expires = DateTime.Now.AddDays(-1D)
        Response.Cookies.Add(myCookie)
        Response.Redirect("~/affiliates/logout.aspx")
    End Sub
    Protected Sub mapNav_click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ImageMapEventArgs)
        'The navigation panel was clicked, update the map with the required image.
        'Eith a Main category was selected, or a Submenu item.
        Try
            Dim commandargs As Array = Split(e.PostBackValue, "|")
            Dim menuItem As String = commandargs(0)
            Dim menuURL As String = commandargs(1)
            Dim navText As String = commandargs(2)
            Dim type As String = commandargs(3)
            'Dim menuItem As Integer = e.PostBackValue
            If type = "main" Then
                'Main category selected
                mapNav.ImageUrl = "~/images/NavImages/Nav_Menu" & menuItem & "_" & Session("EBLanguage") & ".jpg"
                Session("EBMenuIndex") = CStr(menuItem)
                Session("EBMenuSection") = navText
                Session("EBMenuURL") = menuURL
                applyMapFromDB(Session("EBMenuIndex"), Session("EBLanguage"))
                'Now redirect to categorys default page
                Response.Redirect(menuURL)
            Else
                'Sub category selected
                Session("EBMenuSubSection") = navText
                Session("EBMenuSubURL") = menuURL
                Response.Redirect(menuURL)
            End If
        Catch ex As Exception
            lblError1.text = ex.ToString
            Response.End()
        End Try
    End Sub
    Protected Sub preCacheImages()
        'Pre loads navigation images into browser on users first visit
        Dim img As Image
        For iloop As Integer = 1 To _maxImages
            img = FindControl("imgNav" & iloop)
            img.ImageUrl = "~/images/NavImages/Nav_Menu" & iloop & "_" & Session("EBLanguage") & ".jpg"
        Next
    End Sub
    Protected Sub countryChange_RefreshPage(ByVal sender As Object, ByVal e As EventArgs)
        Session("EBLanguage") = drpLanguage.SelectedValue
        Server.Transfer(Request.Path)
    End Sub
    Function basketHasItems() As Boolean
        Return CType(Profile.EBCart.Items.Count > 0, Boolean)
    End Function
    Protected Sub rotateLinks()
        Dim dt As DataTable = getLinks()
        Dim r As New Random
        Dim val1 As Integer = 0
        Dim val2 As Integer = 0
        Dim val3 As Integer = 0
        If Not dt Is Nothing Then
            val1 = CStr(r.Next(0, dt.Rows.Count))
            Do
                val2 = CStr(r.Next(0, dt.Rows.Count))
            Loop While val1 = val2
            Do
                val3 = CStr(r.Next(0, dt.Rows.Count))
            Loop While val1 = val3 Or val2 = val3
            lnkMasterLink1.ImageUrl = dt.Rows(val1)("linkImage")
            lnkMasterLink1.NavigateUrl = dt.Rows(val1)("linkURL")
            lnkMasterLink2.ImageUrl = dt.Rows(val2)("linkImage")
            lnkMasterLink2.NavigateUrl = dt.Rows(val2)("linkURL")
            lnkMasterLink3.ImageUrl = dt.Rows(val3)("linkImage")
            lnkMasterLink3.NavigateUrl = dt.Rows(val3)("linkURL")
            dt.Dispose()
        End If
    End Sub
    Protected Function getLinks() As DataTable
        Dim dt As DataTable
        If Cache("EBLinksDT" & Session("EBLanguage")) Is Nothing Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procImageLinksByCountryCodeSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim dep As SqlCacheDependency
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@CountryCode", SqlDbType.VarChar, 5))
                .Parameters("@countryCode").Value = Session("EBLanguage")
            End With
            If oCmd.Parameters("@countryCode").Value = "" Then oCmd.Parameters("@countryCode").Value = "gb"
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    dt = ds.Tables(0)
                    'Add to cache
                    dep = New SqlCacheDependency("emotionalbliss", "imageLinks")
                    Cache.Add("EBLinksDT" & CStr(Session("EBLanguage")), dt, dep, Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5), CacheItemPriority.Normal, Nothing)
                End If
            Catch ex As Exception
                Dim si As New siteInclude
                si.addError("MasterPageSite.master.vb", "getLinks()::" & ex.ToString)
                si = Nothing
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Else
            'Table cached, grab from session
            dt = CType(Cache("EBLinksDT" & Session("EBLanguage")), DataTable)
        End If
        Return dt
    End Function
    Protected Sub showDistributorTopMenu()
        Dim dt As DataTable = loadDistributorMenu()
        Dim currentDept As String = ""
        Dim menuName As String
        Dim menuLink As String
        Dim menuDept As String
        Dim miMainDept As MenuItem
        If dt.Rows.Count > 0 Then
            'Clear menu 1st
            menuDistMenu.Items.Clear()
            For Each row As DataRow In dt.Rows
                menuName = row("affMenuName")
                If IsDBNull(row("affMenuLink")) Then
                    menuLink = ""
                Else
                    menuLink = row("affMenuLink")
                End If
                menuDept = row("affMenuName1")
                If menuDept <> currentDept Then
                    'New dept
                    If currentDept <> "" Then menuDistMenu.Items.Add(miMainDept)
                    miMainDept = New MenuItem(menuDept)
                    miMainDept.ChildItems.Add(New MenuItem(menuName, "", "", menuLink))
                    currentDept = menuDept
                Else
                    'Same dept
                    miMainDept.ChildItems.Add(New MenuItem(menuName, "", "", menuLink))
                End If
            Next
            menuDistMenu.Items.Add(miMainDept)
        End If
    End Sub
    Protected Function loadDistributorMenu() As DataTable
        Dim dt As DataTable
        If True Then
            'If Cache("EBDistMenuDT") Is Nothing Then
            'Not found in cache, load from datatbase
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procAffiliateMenuSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            oCmd.CommandType = CommandType.StoredProcedure
            If Session("EBAffEBUser") Then
                oCmd.CommandText = "procAffiliateUserMenuSelect"
                oCmd.Parameters.Add(New SqlParameter("@affUserID", SqlDbType.Int))
                oCmd.Parameters("@affUserID").Value = Session("EBAffID")
            End If
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                dt = ds.Tables(0)
                'Dim dep As New SqlCacheDependency("emotionalbliss", "affiliateMenu")
                'Cache.Add("EBDistMenuDT", dt, dep, Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10), CacheItemPriority.Normal, Nothing)
            Catch ex As Exception
                lblError1.Text = ex.ToString
                Response.Write(ex.ToString)
                Response.End()
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Else
            dt = CType(Cache("EBDistMenuDT"), DataTable)
        End If
        Return dt
    End Function
    Protected Sub txtQuickSearch_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Look for existing order
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByNewOrderIDCountrySelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim iID As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@newOrderID").Value = txtQuickSearch.Text
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then iID = ds.Tables(0).Rows(0)("id")
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("masterPageSite.master.vb", "txtQuickSearch_textChanged(txtQuickSearch=" & txtQuickSearch.Text & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If iID > 0 Then
            Response.Redirect("~/affiliates/orderView.aspx?id=" & iID)
        Else
            lblOrderNotFound.Text = "<font color='red' size='-2'>Order not found</font>"
        End If
    End Sub
    Protected Sub updateMenuIndexFromDB(ByVal menuName As String)
        'Lookup supplied menuName, if it exists update the menuIndex in session
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procImageMapByMenuNameSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@menuName", SqlDbType.VarChar, 150))
            .Parameters("@menuName").Value = menuName
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'Menu found, do something
                Session("EBMenuIndex") = ds.Tables(0).Rows(0)("index")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("masterPageSite.aspx.vb", "updateMenuUndexFromDB(menuName=" & menuName & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub drpShopCountry_load(ByVal sender As Object, ByVal e As EventArgs)
        
    End Sub
    Protected Sub drpShopCountry_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Selected Value contains shop country and currency seperated by a comma
        Dim drp As DropDownList = CType(sender, DropDownList)
        Dim country As String = ""
        Dim currency As String = ""
        Dim commaPos As Integer = InStr(drp.SelectedValue, ",")
        country = Left(drp.SelectedValue, commaPos - 1)
        currency = Right(drp.SelectedValue, Len(drp.SelectedValue) - commaPos)
        Session("EBShopCountry") = country
        Session("EBShopCurrency") = currency
        lblTester.Text = "Language=" & Session("EBLanguage") & ", Shop=" & Session("EBShopCountry") & ", Currency=" & Session("EBShopCurrency")
    End Sub
End Class

