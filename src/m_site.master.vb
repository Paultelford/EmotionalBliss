Imports System.Data
Imports System.Data.SqlClient

Partial Class m_site
    Inherits System.Web.UI.MasterPage
    Protected bMDebug As Boolean = False
    Private Const _affType_affiliate As String = "1"
    Private Const _affType_wholesaler As String = "3"

    'System Events
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        'Language and Shop settings
        'QueryString settings override all others
        If Request.QueryString("country") <> "" Then
            Dim EBLanguage As String = ""
            Dim EBShop As String = ""
            Dim EBCountryName As String = ""
            getCountryDefaults(Request.QueryString("country"), EBLanguage, EBShop, EBCountryName)
            If bMDebug Then Response.Write("Setting EBLanguage to '" & EBLanguage & "' as country default<br>")
            If bMDebug Then Response.Write("Setting EBShop to '" & EBShop & "' as country default<br>")
            Session("EBLanguage") = EBLanguage
            Session("EBShopCountry") = EBShop
            Session("EBCountryName") = EBCountryName
            Profile.EBLanguage = EBLanguage
            Profile.EBShop = EBShop
            Profile.EBCountryName = EBCountryName
            'siteInclude.debug(EBLanguage & "/" & EBShop & "/" & EBCountryName)
        End If
        If Request.QueryString("lang") <> "" Then
            Session("EBLanguage") = Request.QueryString("lang")
            Profile.EBLanguage = Request.QueryString("lang")
        End If
        If Request.QueryString("shop") <> "" Then
            Session("EBShopCountry") = Request.QueryString("shop")
            Profile.EBShop = Request.QueryString("shop")
            Profile.EBCountryName = getCountryName(Request.QueryString("shop"))
        End If
        'If Session is empty, then load values from profile
        If Session("EBLanguage") = "" Then
            If Profile.EBLanguage <> "" Then
                If bMDebug Then Response.Write("Setting Language session to match profile<br>")
                Session("EBLanguage") = Profile.EBLanguage
            Else
                'No Session and no profile values exist, default to gb
                If bMDebug Then Response.Write("Profile not found, setting Language session to gb<br>")
                Session("EBLanguage") = "gb"
                Profile.EBLanguage = "gb"
            End If
        End If
        If Session("EBShopCountry") = "" Then
            If Profile.EBShop <> "" Then
                If bMDebug Then Response.Write("Setting Shop session to match profile<br>")
                Session("EBShopCountry") = Profile.EBShop
            Else
                'No Session and no profile values exist, default to gb
                If bMDebug Then Response.Write("Profile not found, setting Shop session to gb<br>")
                Session("EBShopCountry") = "gb"
                Profile.EBShop = "gb"
                Profile.EBCountryName = "United Kingdom"
            End If
        End If
        If Session("EBCountryName") = "" Then
            If Profile.EBCountryName <> "" Then
                If bMDebug Then Response.Write("Setting Shop session to match profile<br>")
                Session("EBCountryName") = Profile.EBCountryName
            Else
                'No Session and no profile values exist, default to gb
                If bMDebug Then Response.Write("Profile not found, setting CountryName session to United Kingdom<br>")
                Profile.EBCountryName = "United Kingdom"
            End If
        End If
        'Show langauge and shopCountry in hidden form so it can always be seen by Viewing Source in a browser
        hidL.Value = Session("EBLanguage")
        hidS.Value = Session("EBShopCountry")
        Membership.Provider.ApplicationName = "EBAffProvider"
        If Profile.country = "" Then Profile.country = Session("EBLanguage")
        'Check quesrystring for news menuIndex (This will occur if user has come from another site via an affilaite link and may be heading directly to a static page)
        'If Request.QueryString("m") <> "" Then updateMenuIndexFromDB(Request.QueryString("m"))
        If bMDebug Then Response.Write("EBLanguage=" & Session("EBLanguage") & "<br>EBShopCountry=" & Session("EBShopCountry") & "<br>Profile.country=" & Profile.country & "<br>")
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Only run on 1st visit
            'imgFlag1.ImageUrl = "~/images/flags/" & Session("EBShopCountry") & ".png"
            imgCurrentFlag.ImageUrl = "~/images/flags/" & Session("EBShopCountry") & ".png"

            Dim dt As DataTable = loadMenuTable(Session("EBLanguage"))
            'If user is logged in as an Affiliate, then point the 'Buy Now' menu item to affiliates/basket.aspx
            If Session("EBAffTypeID") = _affType_affiliate Or Session("EBAffTypeID") = _affType_wholesaler Then
                For Each row As DataRow In dt.Rows
                    Try
                        If LCase(row("navText")) = "shop intro" Then
                            row.Item("url") = "/affiliates/basket.aspx"
                        End If
                    Catch ex As Exception
                    End Try
                Next
            End If
            dlMenu.DataSource = dt
            dlMenu.DataBind()
            'Set MasterLinks below left menu
            rotateLinks()
            'Set the top header background image depending on Session(EBLanguage)
            setBackgroundImage()
            'Show dist menu if logged in
            If LCase(Session("EBAffEBDistributor")) = "true" Or LCase(Session("EBAffEBUser")) = "true" Then
                'drpAffMenu.Visible = False
                'drpDistMenu.Visible = True
                showDistributorTopMenu()
                distmenu.Visible = True
            Else
                'drpDistMenu.Visible = False
                'drpAffMenu.Visible = True
                'lblQuickSearch.Visible = False
                'txtQuickSearch.Visible = False
            End If
            'If user is on homepage (homeIntro.aspx) then show the header animation sequence
            Dim js As String
            If InStr(LCase(Request.ServerVariables("SCRIPT_NAME")), "homeintro.aspx") Then
                js = "var _lofmain = $('lofslidecontent45');" & Chr(10)
                js = js & "var _lofscmain = _lofmain.getElement('.lof-main-wapper');" & Chr(10)
                js = js & "var _lofnavigator = _lofmain.getElement('.lof-navigator-outer');" & Chr(10)
                js = js & "var object = new LofFlashContent(_lofscmain, _lofnavigator,{ fxObject: { transition: Fx.Transitions.Quad.easeInOut, duration: 400 },interval: 4000,layoutStyle: 'opacity'}).start(true, _lofmain.getElement('.preload'));" & Chr(10)
                If LCase(Session("EBLanguage")) = "nl" Or LCase(Session("EBLanguage")) = "be" Then
                    js = js & "document.getElementById('imgFlip1').src='/images/slide1_nl.jpg';" & Chr(10)
                    js = js & "document.getElementById('imgFlip2').src='/images/slide2_nl.jpg';" & Chr(10)
                    js = js & "document.getElementById('imgFlip3').src='/images/slide3_nl.jpg';" & Chr(10)
                End If
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "imageFlipper", js, True)
            Else
                'show random image
                'Dim imgArr() As String
                Dim imgName As String = "slide?.jpg"
                Dim r As New Random(Environment.TickCount)
                Dim i As Integer = r.Next(1, 6)
                If LCase(Session("EBLanguage")) = "nl" Or LCase(Session("EBLanguage")) = "be" Then imgName = "slide?_nl.jpg"
                i = r.Next(1, 6)
                'imgArr = New String() {"slide1.jpg", "slide2.jpg", "slide3.jpg"}
                If i = 1 Or i = 2 Then imgHeaderRnd.ImageUrl = Replace("/images/" & imgName, "?", "1")
                If i = 3 Or i = 4 Then imgHeaderRnd.ImageUrl = Replace("/images/" & imgName, "?", "2")
                If i = 5 Or i = 6 Then imgHeaderRnd.ImageUrl = Replace("/images/" & imgName, "?", "3")
                r = Nothing
            End If

        End If
        If Application("isDevBox") Then
            Dim si As New siteInclude
            lblSiteFooter.Text = "EB Dev Box - GA - " & si.getGoogleRef(Session("EBShopCountry"))
            si = Nothing
        Else
            lblSiteFooter.Text = "Copyright © 2002 - " & Year(Now()) & " Emotional Bliss"
            Dim js As String = ""
            Dim si As New siteInclude
            'js = "<script type=""text/javascript"" src=""https://cetrk.com/pages/scripts/0008/9681.js""> </script>" & Chr(10)
            js = js & "<script type=""text/javascript"">" & Chr(10)
            js = js & "var gaJsHost = ((""https:"" == document.location.protocol) ? ""https://ssl."" : ""http://www."");" & Chr(10)
            js = js & "document.write(unescape(""%3Cscript src='"" + gaJsHost + ""google-analytics.com/ga.js' type='text/javascript'%3E%3C/script%3E""));" & Chr(10)
            js = js & "</script>" & Chr(10)
            js = js & "<script type=""text/javascript"">" & Chr(10)
            js = js & "var pageTracker = _gat._getTracker(""" & si.getGoogleRef(Session("EBShopCountry")) & """);" & Chr(10)
            js = js & "pageTracker._initData();" & Chr(10)
            js = js & "pageTracker._trackPageview();" & Chr(10)
            js = js & "</script>" & Chr(10)
            si = Nothing
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "trackerJS", js, False)
        End If
        If Request.UserAgent.Contains("AppleWebKit") Then Request.Browser.Adapters.Clear()
        'Show country name
        lblCurrentCountryName.Text = Profile.EBCountryName
        loadDbResources()
    End Sub

    'Page Events
    Protected Sub lofnavigator_load(ByVal sender As Object, ByVal e As EventArgs)
        If Not InStr(LCase(Request.ServerVariables("SCRIPT_NAME")), "homeintro.aspx") > 0 Then CType(sender, HtmlControl).Attributes.Add("style", "display: none;")
    End Sub
    Protected Sub rndnavigator_load(ByVal sender As Object, ByVal e As EventArgs)
        If InStr(LCase(Request.ServerVariables("SCRIPT_NAME")), "homeintro.aspx") > 0 Then CType(sender, HtmlControl).Attributes.Add("style", "display: none;")
    End Sub
    Protected Sub dlMenu_itemDataBound(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        Dim lnkMenu As HyperLink
        Dim hidAction As HiddenField
        If LCase(Session("EBShopCountry")) <> "gb" Then
            'Hide Blog menu entries if viewing from outside GB
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                lnkMenu = CType(e.Item.FindControl("lnkMenu"), HyperLink)
                If InStr(LCase(lnkMenu.NavigateUrl), "blog") Then
                    Dim lblSpacePre As Label = CType(e.Item.FindControl("lblSpacePre"), Label)
                    Dim lblSpaceAft As Label = CType(e.Item.FindControl("lblSpaceAft"), Label)
                    lnkMenu.Visible = False
                    lblSpacePre.Visible = False
                    lblSpaceAft.Visible = False
                End If
            End If
        End If
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            'If the 'action' field in the datatable is set to 'popup' then make sure menu item opens in a popup.
            hidAction = CType(e.Item.FindControl("hidAction"), HiddenField)
            lnkMenu = CType(e.Item.FindControl("lnkMenu"), HyperLink)
            If hidAction.Value = "popup" Then lnkMenu.Target = "_blank"
        End If
    End Sub
    Protected Sub imgCurrentFlag_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim img As Image = CType(sender, Image)
        img.Attributes.Add("style", "margin-top: 2px;")
    End Sub

    'Subs
    Public Sub passUpperTableData(ByVal html As String)
        litTopMenu.Text = html
    End Sub
    Protected Sub rotateLinks()
        If False Then
            Dim dt As DataTable = getLinks()
            If Not dt Is Nothing Then
                Dim r As New Random
                Dim val1 As Integer = 0
                Dim val2 As Integer = 0
                Dim val3 As Integer = 0
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
        Else
            Select Case LCase(Session("EBLanguage"))
                Case "nl"
                    lnkMasterLink1.ImageUrl = "~/design/images/Button_FAQs_nl.png"
                    lnkMasterLink1.ToolTip = "veel gestelde vragen"
                    lnkMasterLink1.NavigateUrl = "~/static.aspx?p=faq&m=Home"
                    lnkMasterLink2.ImageUrl = "~/design/images/Button_Catalogue_nl.png"
                    lnkMasterLink2.ToolTip = "Brochure"
                    lnkMasterLink2.NavigateUrl = "/static.aspx?p=Download_Brochure&m=Home"
                    lnkMasterLink3.ImageUrl = "~/design/images/Button_IntimateMassagers_nl.png"
                    lnkMasterLink3.ToolTip = "Intieme Massagers"
                    lnkMasterLink3.NavigateUrl = "~/shopIntro.aspx?m=kopen"
                Case Else
                    'For now keep links static, as they are different sizes
                    lnkMasterLink1.ImageUrl = "~/design/images/Button_FAQs.png"
                    lnkMasterLink1.ToolTip = "FAQ"
                    lnkMasterLink1.NavigateUrl = "~/static.aspx?p=faq&m=Home"
                    lnkMasterLink2.ImageUrl = "~/design/images/Button_Sexologists.png"
                    lnkMasterLink2.ToolTip = "Sexologists - Talk to our expert"
                    lnkMasterLink2.NavigateUrl = "~/sexologistsIntro.aspx"
                    lnkMasterLink3.ImageUrl = "~/design/images/Button_IntimateMassagers.png"
                    lnkMasterLink3.ToolTip = "Intimate Massagers - View the range"
                    lnkMasterLink3.NavigateUrl = "~/shopIntro.aspx"
            End Select
        End If
    End Sub
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
    Protected Sub getCountryDefaults(ByVal country As String, ByRef lang As String, ByRef shop As String, ByRef countryName As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = country
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then
                'Country Code not found. Default to GB
                lang = "gb"
                shop = "gb"
            Else
                'Country was found
                lang = ds.Tables(0).Rows(0)("defaultLanguage")
                shop = ds.Tables(0).Rows(0)("countryCode")
                countryName = ds.Tables(0).Rows(0)("countryName")
            End If
        Catch ex As Exception
            siteInclude.addError("m_affs.master.vb", "getCountryDefaults(country=" & country & "); " & ex.ToString())
            'If error occured, make sure that language and shop get set to gb as default
            lang = "gb"
            shop = "gb"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub setBackgroundImage()
        'Dim imageName As String = GetLocalResourceObject("imgBackgroundHeader")
        'backheader.Attributes.Add("style", "background-image:  url('/design/images/" & imageName & "'); width: 950px; height: 385px; margin: 0; padding: 0; border: 0; float: left;")
    End Sub
    Public Sub showFacebookLink()
        'lnkFacebook.Visible = True
        'lnkFacebook.Attributes.Add("style", "padding-top: 10px;")
    End Sub
    Protected Sub loadDbResources()
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@countryCode", "@pagename"}
            Dim paramValue() As String = {Session("EBLanguage"), "m_site_master"}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.VarChar}
            Dim paramSize() As Integer = {5, 50}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procDbResourcesByPageSelect")
            For Each row As DataRow In dt.Rows
                Select Case LCase(row("name"))
                    Case "lblshoppingbasket"
                        lblShoppingBasket.Text = row("value")
                    Case "lblcontactus"
                        lblContactUs.Text = row("value")
                    Case "lblmailinglist"
                        lblMailingList.Text = row("value")
                    Case "lbltrackorder"
                        lblTrackOrder.Text = row("value")
                End Select
            Next
        Catch ex As Exception
            siteInclude.addError("m_site.master.vb", "loadDbResources(); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
    End Sub

    'Functions
    Protected Function loadMenuTable(ByVal lang As String) As DataTable
        Dim dt As DataTable
        If Cache("EBImageMap" & lang) Is Nothing Or Not Application("cacheMenus") Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procImageMapByIndexMenuSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim dep As SqlCacheDependency
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@section", SqlDbType.VarChar, 10))
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
                Cache.Add("EBImageMap" & lang, dt, Nothing, Caching.Cache.NoAbsoluteExpiration, tsExpires, CacheItemPriority.Normal, Nothing)
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
            dt = CType(Cache("EBImageMap" & lang), DataTable)
        End If
        Return dt
    End Function
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
                siteInclude.addError("MasterPageSite.master.vb", "getLinks()::" & ex.ToString)
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
    Protected Function getCountryName(ByVal cc As String) As String
        Dim result As String = ""
        Dim dt As New DataTable
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
        oCmd.Parameters("@countryCode").Value = cc
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            dt = ds.Tables(0)
            If dt.Rows.Count > 0 Then result = dt.Rows(0)("countryName")
            'Dim dep As New SqlCacheDependency("emotionalbliss", "affiliateMenu")
            'Cache.Add("EBDistMenuDT", dt, dep, Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(10), CacheItemPriority.Normal, Nothing)
        Catch ex As Exception
            siteInclude.debug(ex.ToString())
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
End Class