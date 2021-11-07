Imports System.Data
Imports System.Data.SqlClient

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Private Const ebmcc As String = "EBMaintenanceCountryCode"
    Private drpMainCountry As DropDownList
    Private lblMainCountry As Label

    'WORK TO DO################################
    'When country dropdown from master page is changed, selectedIndexChagnged needs to be handles on this page so that drpDepartments can be rebound to show the departments for the selected country.
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        If Not Page.IsPostBack Then
            Membership.Provider.ApplicationName = "EBProvider"
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If Not Membership.GetUser Is Nothing Then
            Dim mapNav As ImageMap = logMaintenance.FindControl("mapNav")
            'See if user has Security Permissions to view this page

            'Get users countrys and create the EBMaintenance Country Dropdown.
            'Each time a new maintenance page is visited. Not postback
            drpMainCountry = logMaintenance.FindControl("drpMainCountry")
            lblMainCountry = logMaintenance.FindControl("lblMainCountry")

            If Page.IsPostBack Then
                If Session("EBPeartreeIndex") = "" Then Session("EBPeartreeIndex") = "1"
                mapNav.ImageUrl = "~/images/NavImages/Nav_Peartree" & Session("EBPeartreeIndex") & "_GB.jpg"
                'preCacheImages()
                applyMapFromDB(Session("EBPeartreeIndex"), "GB")
            Else
                If Session("EBPeartreeIndex") = "" Then preCacheImages()
                '13-11-07 - Get menu hotspots from db and set image depending on page in session
                If Session("EBPeartreeIndex") = "" Then Session("EBPeartreeIndex") = "1"
                mapNav.ImageUrl = "~/images/NavImages/Nav_Peartree" & Session("EBPeartreeIndex") & "_GB.jpg"
                applyMapFromDB(Session("EBPeartreeIndex"), "GB")
            End If
        Else
            Response.Redirect("~/maintenance/login.aspx")
        End If
    End Sub
    Protected Sub drpMainCountry_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'This runs each time a new maintenance page is visited (Not a postback)
        Dim drpMainCountry As DropDownList = logMaintenance.FindControl("drpMainCountry")
        If Session(ebmcc) = "" Then 'This runs when maintenance is visited for the 1st time 
            Session(ebmcc) = drpMainCountry.SelectedValue
        End If
        drpMainCountry.SelectedValue = Session(ebmcc)
    End Sub
    Protected Sub setCountry()
        'This runs each time a new maintenance page is visited (Not a postback)
        Dim drpMainCountry As DropDownList = logMaintenance.FindControl("drpMainCountry")
        If Session(ebmcc) = "" Then 'This runs when maintenance is visited for the 1st time 
            Session(ebmcc) = drpMainCountry.SelectedValue
        End If
        drpMainCountry.SelectedValue = Session(ebmcc)
    End Sub
    Protected Sub drpMainCountry_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Each time a new country is selected, it gets added to the session here.
        Dim drpMainCountry As DropDownList = logMaintenance.FindControl("drpMainCountry")
        Session(ebmcc) = drpMainCountry.SelectedValue
    End Sub
    Protected Sub applyMapFromDB(ByVal i As String, ByVal lang As String)
        Dim dt As DataTable = loadMapDataTable(i, lang)
        Dim mapNav As ImageMap = logMaintenance.FindControl("mapNav")
        Dim lblTest22 As Label = logMaintenance.FindControl("lblTest22")
        If dt.Rows.Count > 0 Then
            mapNav.HotSpots.Clear() 'Remove all hotspots
            For Each row As DataRow In dt.Rows
                Dim rect As New RectangleHotSpot
                If LCase(row("type")) = "parent" Then
                    'Add aprent as postback
                    rect.HotSpotMode = HotSpotMode.PostBack
                    rect.PostBackValue = row("index")
                Else
                    'Add child as navigate   
                    rect.HotSpotMode = HotSpotMode.Navigate
                    rect.NavigateUrl = row("url")
                End If
                'lbltest22.text=lbltest22.Text & 
                rect.Left = row("left")
                rect.Bottom = row("bottom")
                rect.Right = row("right")
                rect.Top = row("top")
                mapNav.HotSpots.Add(rect)
            Next
        End If
    End Sub
    Protected Function loadMapDataTable(ByVal i As String, ByVal lang As String) As DataTable
        Dim dt As New DataTable
        If Cache("EBImageMapPeartree" & CStr(i) & lang) Is Nothing Then
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
                .Parameters("@section").Value = "peartree"
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                dep = New SqlCacheDependency("emotionalbliss", "imageMaps")
                dt = ds.Tables(0)
                Dim tsExpires As New TimeSpan(0, 0, 5, 0)
                Cache.Add("EBImageMapPeartree" & CStr(i) & lang, dt, Nothing, Caching.Cache.NoAbsoluteExpiration, tsExpires, CacheItemPriority.Normal, Nothing)
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
            dt = CType(Cache("EBImageMapPeartree" & i & lang), DataTable)
        End If
        Return dt
    End Function
    Protected Sub mapNav_click(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ImageMapEventArgs)
        'The navigation panel was clicked, update the map with the required image.
        Dim mapNav As ImageMap = logMaintenance.FindControl("mapNav")
        Dim menuItem As Integer = e.PostBackValue
        mapNav.ImageUrl = "~/images/NavImages/Nav_Peartree" & menuItem & "_GB.jpg"
        Session("EBPeartreeIndex") = CStr(menuItem)
        applyMapFromDB(Session("EBPeartreeIndex"), "GB")
    End Sub
    Protected Sub preCacheImages()
        'Pre loads navigation images into browser on users first visit
        Dim img As Image
        For iloop As Integer = 1 To 6
            img = FindControl("imgNav" & iloop)
            img.ImageUrl = "~/images/NavImages/Nav_Peartree" & iloop & "_GB.jpg"
        Next
    End Sub
End Class


