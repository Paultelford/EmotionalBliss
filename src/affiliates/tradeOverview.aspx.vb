
Partial Class affiliates_tradeOverview
    Inherits BasePage
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private _page As Panel
    Private Const _gvOverview_affilaitePos As Integer = 0
    Private Const _gvOverview_countryPos As Integer = 2
    Private Const _gvOverview_countrySpacerPos As Integer = 3
    Private Const _gvOverview_creditPos As Integer = 4
    Private Const _gvOverview_debitPos As Integer = 6
    Private Const _gvOverview_balancePos As Integer = 8

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Not (Session("EBAffEBDistributor") Or Session("EBAffEBUser")) Then Response.Redirect("default.aspx")
        If InStr(Request.ServerVariables("URL"), "maintenance") Then
            'Get main panel
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceholder1")
            _page = _content.FindControl("panPage")
            'User is in Peartree maintenance
            'Show peartree controls
            Dim tblRow1 As TableRow = _page.FindControl("tblRow1")
            tblRow1.Visible = True
        Else
            'User is logged in as distributor
            'Check permissions
            If Session("EBAffID") = "" Then Response.Redirect("default.aspx")
            'Get main panel
            _page = panPage
        End If
    End Sub
    Protected Sub drpViewType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        Dim tblRow2 As TableRow = _page.FindControl("tblRow2")
        Dim gv As GridView = _page.FindControl("gvOverview")
        Dim drpDistributors As DropDownList = _page.FindControl("drpDistributors")
        If drp.SelectedValue = "aff" Then
            tblRow2.Visible = True
        Else
            tblRow2.Visible = False 'Hide the distributor dropdown and reset it
            drpDistributors.SelectedIndex = 0
            gv.DataBind() 'Rebind the gv to show all distributors
        End If
    End Sub
    Protected Sub drpDistributors_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Peartree user has selected a distributor to view.  Databind the overview gridview to show the new results
        Dim gv As GridView = _page.FindControl("gvOverview")
        gvOverview.DataBind()
    End Sub
    Protected Sub sqlOverview_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        'Load datasource parameters with correct values
        Dim sql As SqlDataSourceView = CType(sender, SqlDataSourceView)
        Dim countryCode As String = "%"
        Dim isDist As Boolean = True
        Dim drpViewType As DropDownList = _page.FindControl("drpViewType")
        Dim drpDistributors As DropDownList = _page.FindControl("drpDistributors")
        Dim gv As GridView = _page.FindControl("gvOverview")
        If InStr(Request.ServerVariables("URL"), "maintenance") Then
            'User is in peartree maintenance
            If drpViewType.SelectedValue = "aff" Then
                'User is wainting to look at a distributors's overview
                isDist = False
                countryCode = drpDistributors.SelectedValue
            End If
        Else
            'User distributor in the distributor back end
            isDist = False
            countryCode = Session("EBAffEBDistributorCountryCode")
        End If
        e.Command.Parameters(0).Value = isDist
        e.Command.Parameters(1).Value = countryCode
    End Sub
    Protected Sub gvOverview_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Gridview has been databound, calculate each rows balance (credit-debit)
        Dim gv As GridView = CType(sender, GridView)
        For Each row As GridViewRow In gv.rows
            If row.RowType = DataControlRowType.DataRow Then
                row.Cells(_gvOverview_balancePos).Text = FormatNumber(CDec(row.Cells(_gvOverview_creditPos).Text) - CDec(row.Cells(_gvOverview_debitPos).Text), 2)
            End If
        Next
        'Show correct header for the 1st column (To distinguish between affilaites or distributors)
        Dim isDist As Boolean = True
        Dim showCountry As Boolean = False
        Dim drpViewType As DropDownList = _page.FindControl("drpViewType")
        Dim drpDistributors As DropDownList = _page.FindControl("drpDistributors")
        Dim header As String = ""
        If drpViewType.SelectedValue <> "dist" And drpDistributors.SelectedIndex <> 0 Then isDist = True
        If isDist Then
            header = "Affilaites"
            'No need to show the country column, as all affiliates must be from the same country as the distributor
            showCountry = False
        Else
            header = "Distributors"
        End If
        If gv.Rows.Count > 0 Then gv.HeaderRow.Cells(_gvOverview_affilaitePos).Text = header 'Change header if gridview has been drawn
        gv.Columns(_gvOverview_countryPos).Visible = showCountry
        gv.Columns(_gvOverview_countrySpacerPos).Visible = showCountry
    End Sub
End Class
