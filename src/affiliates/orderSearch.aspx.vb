Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_orderSearch
    Inherits BasePage
    Private pgIndex As Integer = 0

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        If Page.IsPostBack Then
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Membership.Provider.ApplicationName = "ebAffProvider"
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then
            Response.Redirect("default.aspx")
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        runSearch(pgIndex)
    End Sub
    Protected Sub btnSearch_click(ByVal sender As Object, ByVal e As EventArgs)
        runSearch(0)
    End Sub
    Protected Sub runSearch(ByVal pIndex As Integer)
        Dim sp As String = ""
        Dim searchText As String = txtSearch.Text
        Select Case radCriteria.SelectedValue
            Case "orderid"
                sp = "procShopOrderSearchOrderIDSelect"
            Case "namebill"
                sp = "procShopOrderSearchNameBillSelect"
            Case "nameship"
                sp = "procShopOrderSearchNameShipSelect"
            Case "addbill"
                sp = "procShopOrderSearchAddBillSelect"
            Case "addship"
                sp = "procShopOrderSearchAddShipSelect"
            Case "email"
                sp = "procShopOrderSearchEmailSelect"
            Case "orderdate"
                sp = "procShopOrderSearchOrderDateSelect"
                If IsDate(searchText) Then searchText = make2dp(Day(searchText)) & "." & make2dp(Month(searchText)) & "." & CStr(Year(searchText))
            Case "ccnum"
                sp = "procShopOrderSearchCCNumSelect"
            Case "chequenum"
                sp = "procShopOrderSearchChequeNumSelect"
            Case "trackernum"
                sp = "procShopOrderSearchTrackerNumSelect"
        End Select
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand(sp, oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@searchText", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@searchText").Value = searchText
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If pIndex > 0 Then gvResults.PageIndex = pIndex
            gvResults.DataSource = ds
            gvResults.DataBind()
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
    Function make2dp(ByVal s As String)
        Dim result As String = s
        If Len(s) = 1 Then result = "0" & CStr(s)
        Return result
    End Function
    Protected Function showDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d, DateFormat.LongDate)
        Return result
    End Function
    Protected Sub gvResults_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs)
        pgIndex = e.NewPageIndex
    End Sub
    Protected Sub gv_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvResults.DataBound
        pager1.registerData(gvResults)
    End Sub
    Protected Sub gv_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvResults.DataBinding
        pager1.registerControl(gvResults)
    End Sub
End Class
