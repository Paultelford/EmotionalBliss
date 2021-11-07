Imports System.Data
Imports System.Data.SqlClient

Partial Class m_affs
    Inherits System.Web.UI.MasterPage
    Private bStaticWidth As Boolean = False
    Private Const _maxImages As Integer = 9
    Private Const _setAffiliatePricePos As Integer = 2
    Private Const _setShopPricePos As Integer = 3
    Private Const _setClickThroughPos As Integer = 3
    Private Const _departmentsPos As Integer = 5
    Private Const _purchaseManagementPos As Integer = 6
    Private Const _scanPos As Integer = 7
    Private Const _affLinkPos As Integer = 8
    Private Const _affApprovePos As Integer = 9
    Private Const _drpAffMenu_consultancyIndex As Integer = 2
    Private Const _affType_affiliate As String = "1"
    Private Const _affType_wholesaler As String = "3"
    Protected bMDebug As Boolean = False

    'System Events
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        'Language and Shop settings
        If Request.UserAgent.IndexOf("AppleWebKit") > 0 Then
            Request.Browser.Adapters.Clear()
        End If
        'QueryString settings override all others
        If Request.QueryString("country") <> "" Then
            Dim EBLanguage As String = ""
            Dim EBShop As String = ""
            Dim EBCountryName As String = ""
            getCountryDefaults(Request.QueryString("country"), EBLanguage, EBShop)
            If bMDebug Then Response.Write("Setting EBLanguage to '" & EBLanguage & "' as country default<br>")
            If bMDebug Then Response.Write("Setting EBShop to '" & EBShop & "' as country default<br>")
            Session("EBLanguage") = EBLanguage
            Session("EBShopCountry") = EBShop
            Session("EBCountryName") = EBCountryName
            Profile.EBLanguage = EBLanguage
            Profile.EBShop = EBShop
            Profile.EBCountryName = EBCountryName
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
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Only run on 1st visit
            imgFlag1.ImageUrl = "~/images/flags/" & Session("EBShopCountry") & ".png"
            imgFlag2.ImageUrl = "~/images/flags/" & Session("EBAffCountryCode") & ".png"
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
            'Show dist menu if logged in
            If LCase(Session("EBAffEBDistributor")) = "true" Or LCase(Session("EBAffEBUser")) = "true" Then distmenu.Visible = True
            'Set the top header background image depending on Session(EBLanguage)
            setBackgroundImage()
        End If
        'Hide flasgs for Affiliates set to 'hideCountry'
        If Session("EBAffHideCountry") Then imgFlag2.Visible = False
        'Dist/Aff menus
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
            Page.Title = "Emotional Bliss Affiliate Section"
            'Make the affiliates menu/header section visible
            tblAffiliate.Visible = True
            lblUsername.Text = Session("EBAffUsername")
            'Retrieve affMenu dropdown state Session
            'Is user Affiliate or Distributor
            If LCase(Session("EBAffEBDistributor")) = "true" Or LCase(Session("EBAffEBUser")) = "true" Then
                drpAffMenu.Visible = False
                'drpDistMenu.Visible = True
                showDistributorTopMenu()
            Else
                'drpDistMenu.Visible = False
                drpAffMenu.Visible = True
                lblQuickSearch.Visible = False
                txtQuickSearch.Visible = False
            End If
        End If
        'Show the consultancy menu option for affilaites that have access to consultancy
        If Not Membership.GetUser Is Nothing Then
            If Roles.IsUserInRole(Membership.GetUser.UserName, "Consultancy") Then drpAffMenu.Items(_drpAffMenu_consultancyIndex).Enabled = True
            If lcase(Session("EBAffUsername")) = "penlan" Then drpAffMenu.Items(_drpAffMenu_consultancyIndex).Enabled = True
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
            'js = js & "var printWin;" & Chr(10)
            'js = js & "function printData(){ printWin=window.open('printPop.aspx'); }"
            'js = js & "function remoteCall(){ printWin.passData( document.getElementById('" & panTextBody.ClientID & "').innerHTML); }"
            js = js & "</script>" & Chr(10)
            si = Nothing
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "trackerJS", js, False)
        End If
        If Request.UserAgent.Contains("AppleWebKit") Then Request.Browser.Adapters.Clear()
        'Show country name
        lblCurrentCountryName.Text = Profile.EBCountryName
    End Sub

    'Page Events
    Protected Sub dlMenu_itemDataBound(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        If lCase(Session("EBShopCountry")) <> "gb" Then
            'Hide Blog menu entries if viewing from outside GB
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim lnkMenu As HyperLink = CType(e.Item.FindControl("lnkMenu"), HyperLink)
                If InStr(lCase(lnkMenu.NavigateUrl), "blog") Then
                    Dim lblSpacePre As Label = CType(e.Item.FindControl("lblSpacePre"), Label)
                    Dim lblSpaceAft As Label = CType(e.Item.FindControl("lblSpaceAft"), Label)
                    lnkMenu.Visible = False
                    lblSpacePre.Visible = False
                    lblSpaceAft.Visible = False
                End If
            End If
        End If
    End Sub
    Protected Sub imgCurrentFlag_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim img As Image = CType(sender, Image)
        img.Attributes.Add("style", "margin-top: 2px;")
    End Sub

    'Subs
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
    Protected Sub drpAffMenu_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Response.Write("Setting Session to Index of " & drpAffMenu.SelectedIndex & "<br>")
        'Session("EBAffMenuIndex") = drpAffMenu.SelectedIndex
        Session("EBAffMenuValue") = drpAffMenu.SelectedValue
        Select Case LCase(drpAffMenu.SelectedValue)
            Case "password"
                Response.Redirect("~/affiliates/changePassword.aspx")
            Case "order"
                Response.Redirect("~/affiliates/basket.aspx")
            Case "statement"
                Response.Redirect("~/affiliates/statement.aspx")
            Case "orders"
                Response.Redirect("~/affiliates/affOrderView.aspx")
            Case "consultancy"
                Response.Redirect("~/affiliates/consultancyJulia.aspx")
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
            Case "banners"
                'Response.Redirect("~/static.aspx?p=Banner_Links&m=B2B")
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), "printpopup", "self.setTimeout(window.open('/banners.html'),200);", True)
                'Dim cs As ClientScriptManager = Page.ClientScript
                'cs.RegisterStartupScript(Me.GetType(), "loade", "self.setTimeout(""focusElement('" & txtUsername.ClientID & "')"",200);", True)
            Case "media"
                Response.Redirect("~/affiliates/mediaPictures.aspx")
            Case "links"
                Response.Redirect("~/affiliates/affLinks.aspx")
            Case "contact"
                Response.Redirect("~/ebcontact.aspx")
            Case "overview"
                Response.Redirect("~/affiliates/default.aspx")
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
    Protected Sub getCountryDefaults(ByVal country As String, ByRef lang As String, ByRef shop As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.Varchar, 5))
            .Parameters("@countryCode").Value = country
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then
                'Country Code not found. Default to GB
                lang = "gb"
                shop = "gb"
            Else
                'Country was found
                lang = ds.Tables(0).Rows(0)("defaultLanguage")
                shop = ds.Tables(0).Rows(0)("countryCode")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.AddError("m_affs.master.vb", "getCountryDefaults(country=" & country & "); " & ex.ToString())
            si = Nothing
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
        Dim imageName As String = GetLocalResourceObject("imgBackgroundHeader")
        'backheader.Attributes.Add("style", "background-image:  url('/design/images/" & imageName & "'); width: 950px; height: 385px; margin: 0; padding: 0; border: 0; float: left;")
    End Sub

    'Functions
    Protected Function loadMenuTable(ByVal lang As String) As DataTable
        Dim dt As DataTable
        If Cache("EBImageMap" & lang) Is Nothing Then
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
        'If Cache("EBDistMenuDT") Is Nothing Then
        If True Then
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

