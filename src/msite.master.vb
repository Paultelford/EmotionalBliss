Imports System.Data
Imports System.Data.SqlClient

Partial Class msite
    Inherits System.Web.UI.MasterPage
    Protected bMDebug As Boolean = False
    Private Const _affType_affiliate As String = "1"
    Private Const _affType_wholesaler As String = "3"
    Private _htVoucherCodes As Hashtable
    Private _url As String

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
        If InStr(LCase(Request.ServerVariables("SCRIPT_NAME")), "productreviews") > 0 Then LeftMenu.Visible = False
        If InStr(LCase(Request.ServerVariables("SCRIPT_NAME")), "email.aspx") > 0 Then LeftMenu.Visible = False
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

        Dim lnkMenu As hyperlink

        dlMenu.DataSource = dt
        dlMenu.DataBind()

        Dim dliTellAFriend As datalistitem
        
        'Dim h As htmlliteral
        For Each dli As DataListItem In dlMenu.Items
            lnkMenu = dli.FindControl("lnkMenu")
            siteinclude.debug(lnkMenu.Text)
            If LCase(lnkMenu.Text) = "tell a friend" Then
                Dim l As New HyperLink
                l.Text = "Test link"
                'l.Attributes.add("class", "trigger")
                l.NavigateUrl = "#"
                l.CssClass = "trigger"
                'lnkMenu.Attributes.Remove("class")
                lnkMenu.CssClass = "trigger"
                'lnkMenu.NavigateUrl = "#"

                'lnkMenu.Text = "XXXX"
            End If
        Next

        'dliTellAFriend.CssClass = "trigger"
        lblTopItems.Text = Profile.EBCart.Items.Count.ToString()
        lblTopCurrency.Text = "£"
        lblTopOrderTotal.Text = FormatNumber(Profile.EBCart.GoodsIncVat, 2)
        If Not Page.IsPostBack Then
            imgCurrentFlag.ImageUrl = "~/images/flags/" & Session("EBShopCountry") & ".png"
            lblCurrentCountryName.Text = Profile.EBCountryName
        End If
        'Voucher codes for Tell A Friend
        _htVoucherCodes = New Hashtable
        _htVoucherCodes.Add("gb", "58284801")
        _htVoucherCodes.Add("nl", "69368464")
        _htVoucherCodes.Add("us", "45509222")
        lblSentOK.Text = ""
        Select Case LCase(Session("EBShopCountry"))
            Case "gb"
                lblDiscountAmount.Text = "£5"
            Case "us"
                lblDiscountAmount.Text = "$10"
            Case "ca"
                lblDiscountAmount.Text = "$10"
            Case Else
                lblDiscountAmount.Text = "€7.50"
        End Select
        _url = "http://www.emotionalbliss.com"
        If Application("isDevBox") Then
            Dim si As New siteInclude
            'lblSiteFooter.Text = "EB Dev Box - GA - " & si.getGoogleRef(Session("EBShopCountry"))
            si = Nothing
        Else
            'lblSiteFooter.Text = "Copyright © 2002 - " & Year(Now()) & " Emotional Bliss"
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
    End Sub

    'Page - ItemDataBound
    Protected Sub dlMenu_itemDataBound(ByVal sender As Object, ByVal e As DataListItemEventArgs)
        If False Then
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
        End If
    End Sub
    'Load
    Protected Sub imgCurrentFlag_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim img As Image = CType(sender, Image)
        img.Attributes.Add("style", "margin-top: 2px;")
    End Sub

    'User - Click
    Protected Sub lnkSend_click(ByVal sender As Object, ByVal e As EventArgs)
        _url = Request.ServerVariables("SERVER_NAME") & Request.ServerVariables("SCRIPT_NAME")
        If Request.ServerVariables("QUERY_STRING") <> "" Then _url = _url & "?" & Request.ServerVariables("QUERY_STRING")
        Dim sTo As String = txtFriendsEmail.Text
        Dim sCC As String = ""
        Dim sBCC As String = ""
        Dim sSubject As String = txtYourName.Text & " has asked us to send you the following information on their behalf"
        Dim sFrom As String = "noreply@emotionalbliss.com"
        Dim sFromName As String = txtYourName.Text
        Dim sBody As String = "Dear " & txtFriendsName.Text & ",<br><br>"
        sBody = sBody & "We have been requested by " & txtYourName.Text & " to send you the following link as it may be of interest.<br><br>"
        sBody = sBody & "Link: <a href='" & _url & "'>" & _url & "</a><br><br>"
        sBody = sBody & "As this information has been requested by " & txtYourName.Text & " it is our pleasure to enclose the following voucher code offering you a " & lblDiscountAmount.Text & " discount on any future purchases made on the " & UCase(Session("EBShopCountry")) & " website <a href='http://www.emotionalbliss.com'>http://www.emotionalbliss.com</a>.<br><br>"
        sBody = sBody & "To activate the voucher simply copy and paste the code when on the basket page.<br><br>"
        sBody = sBody & "Voucher code: " & _htVoucherCodes(LCase(Session("EBShopCountry"))) & "<br><br>"
        sBody = sBody & "Message: " & txtMessage.Text
        Try
            siteInclude.sendSQLEmail(sTo, sCC, sBCC, sSubject, sFrom, sFromName, sBody, siteInclude._emailType.emailHtml)
        Catch ex As Exception
            siteInclude.addError("msite.master.vb", "lnkSend_click::EmailNotSent(); " & ex.ToString())
        End Try

        'Add email addresses to db
        Dim sAddress As String = txtYourEmail.Text
        Dim sName As String = sFromName
        Dim sGroupID As String = "4"
        For iLoop As Integer = 1 To 2
            If iLoop = 2 Then
                sAddress = sTo
                sName = txtFriendsName.Text
                sGroupID = "5"
            End If
            Dim dt As New DataTable
            Try
                Dim param() As String = New String() {"@address", "@name", "@groupID"}
                Dim paramValue() As String = New String() {sAddress, sName, sGroupID}
                Dim paramType() As SqlDbType = New SqlDbType() {SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int}
                Dim paramSize() As Integer = New Integer() {100, 50, 0}
                siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procEmailInsert2")
            Catch ex As Exception
                siteInclude.addError("msite.master.vb", "lnkSend_click(@address=" & sAddress & ", @name=" & sName & "@groupID=" & sGroupID & "); " & ex.ToString())
            Finally
                dt.Dispose()
            End Try
        Next
        txtFriendsEmail.Text = ""
        txtFriendsName.Text = ""
        txtMessage.Text = ""
        txtYourEmail.Text = ""
        txtYourName.Text = ""
        lblSentOK.Text = "Email successfully sent.<br>Thank you."
    End Sub

    'Subs
    Protected Sub rotateLinks()
        Select Case LCase(Session("EBLanguage"))
            Case "nl"
                'lnkMasterLink1.ImageUrl = "~/design/images/Button_FAQs_nl.png"
                'lnkMasterLink1.ToolTip = "veel gestelde vragen"
                'lnkMasterLink1.NavigateUrl = "~/static.aspx?p=faq&m=Home"
                'lnkMasterLink2.ImageUrl = "~/design/images/Button_Catalogue_nl.png"
                'lnkMasterLink2.ToolTip = "Brochure"
                'lnkMasterLink2.NavigateUrl = "/static.aspx?p=Download_Brochure&m=Home"
                'lnkMasterLink3.ImageUrl = "~/design/images/Button_IntimateMassagers_nl.png"
                'lnkMasterLink3.ToolTip = "Intieme Massagers"
                'lnkMasterLink3.NavigateUrl = "~/shopIntro.aspx?m=kopen"
            Case Else
                'For now keep links static, as they are different sizes
                'lnkMasterLink1.ImageUrl = "~/design/images/Button_FAQs.png"
                'lnkMasterLink1.ToolTip = "FAQ"
                'lnkMasterLink1.NavigateUrl = "~/static.aspx?p=faq&m=Home"
                'lnkMasterLink2.ImageUrl = "~/design/images/Button_Sexologists.png"
                'lnkMasterLink2.ToolTip = "Sexologists - Talk to our expert"
                'lnkMasterLink2.NavigateUrl = "~/sexologistsIntro.aspx"
                'lnkMasterLink3.ImageUrl = "~/design/images/Button_IntimateMassagers.png"
                'lnkMasterLink3.ToolTip = "Intimate Massagers - View the range"
                'lnkMasterLink3.NavigateUrl = "~/shopIntro.aspx"
        End Select
    End Sub
    Public Sub passUpperTableData(ByVal html As String)
        litTopMenu.Text = html
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
    Public Sub showFaceBookLink()
        'To be copied from m_site
    End Sub

    'Functions
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
    Protected Function formatTextUL(ByVal s As String) As String
        Dim a As Array = Split(s, " ")
        Dim result As String = ""
        For Each x As String In a
            Select Case x
                Case "B2B"
                    result = result & "B2B"
                Case Else
                    result = result & UCase(Left(x, 1))
                    If Len(x) > 1 Then
                        result = result & LCase(Right(x, Len(x) - 1)) & " "
                    End If
            End Select
        Next
        If Right(result, 1) = " " Then result = Left(result, Len(result) - 1)
        Return result
    End Function

    'Properties
    Public ReadOnly Property LeftMenu() As HtmlTableCell
        Get
            Return tdLeftMenu
        End Get
    End Property
End Class

