Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing.Text

Partial Class affiliates_orderList
    Inherits BasePage

    'System Events
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        If Page.IsPostBack Then

        Else
            'On 1st visit check for the last drpOrderStatus in the seesion, if it exists the set the dropdown to users previous selected value
            If Session("EBAffOrderList_drpOrderStatus") <> "" Then drpOrderStatus.SelectedValue = Session("EBAffOrderList_drpOrderStatus")
            'If Session("EBAffOrderList_drpOrderPrefix") <> "" Then drpOrderPrefix.SelectedValue = Session("EBAffOrderList_drpOrderPrefix")
            If Session("EBAffOrderList_drpOrderSource") <> "" Then drpOrderSource.SelectedValue = Session("EBAffOrderList_drpOrderSource")
            If Session("EBAffOrderList_drpPaymentMethod") <> "" Then drpPaymentMethod.SelectedValue = Session("EBAffOrderList_drpPaymentMethod")
            Dim fonts As New InstalledFontCollection

            'grdFonts.DataSource = fonts.Families
            'grdFonts.DataBind()

        End If

    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        ordersDataBind()
    End Sub

    'Page Events
    Protected Sub sqlOrders_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)

    End Sub

    'User Events
    Protected Sub drpOrderStatus_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Session("EBAffOrderList_drpOrderStatus") = drpOrderStatus.SelectedValue
    End Sub
    Protected Sub drpOrderPrefix_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Session("EBAffOrderList_drpOrderPrefix") = drpOrderPrefix.SelectedValue
    End Sub
    Protected Sub btnShowAll_click(ByVal sender As Object, ByVal e As EventArgs)
        lblTest.Text = "all"

    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblTest.Text = ""
    End Sub
    Protected Sub drpOrderSource_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drpOrderSource As DropDownList = CType(sender, DropDownList)
        Session("EBAffOrderList_drpOrderSource") = drpOrderSource.SelectedValue
    End Sub
    Protected Sub drpPaymentMethod_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drpPaymentMethod As DropDownList = CType(sender, DropDownList)
        Session("EBAffOrderList_drpPaymentMethod") = drpPaymentMethod.SelectedValue
    End Sub

    'Subs    
    Protected Sub ordersDataBind()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByCountryDateSelectNew", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim Dates1() As String = Split(date1.getStartDate, " ")
        Dim Dates2() As String = Split(date1.getEndDate, " ")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@orderSource", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@paymentMethod", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@orderStatus", SqlDbType.VarChar, 20))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            'This line commited by shailesh parmar date on 
            '.Parameters("@startDate").Value = FormatDateTime(date1.getStartDate, DateFormat.LongDate) & " 00:00"
            .Parameters("@startDate").Value = DateTime.ParseExact(Dates1(0), "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            'FormatDateTime(date1.getStartDate, DateFormat.LongDate) & " 00:00"
            'This line code change by shailesh parmar 21122020 "1 " + months[DateTime.Today.Month-1] + " " + DateTime.Today.Year
            '.Parameters("@endDate").Value = DateTime.Now.Date & " 23:59:59" 'FormatDateTime(date1.getEndDate, DateFormat.LongDate) & " 23:59:59"
            .Parameters("@endDate").Value = DateTime.ParseExact(Dates2(0) & " 11:59:59 PM", "dd/MM/yyyy hh:mm:ss tt", System.Globalization.DateTimeFormatInfo.InvariantInfo)
            .Parameters("@orderSource").Value = drpOrderSource.SelectedValue
            .Parameters("@paymentMethod").Value = drpPaymentMethod.SelectedValue
            .Parameters("@orderStatus").Value = drpOrderStatus.SelectedValue
        End With
        If lblTest.Text = "all" Then oCmd.Parameters("@startDate").Value = "1 January 2008"
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            gvOrders.DataSource = ds
            gvOrders.DataBind()
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured, please conatct tech support.</font>"
            Dim si As New siteInclude
            si.addError("affiliates/orderList.aspx.vb", "ordersDataBind(startDate=" & oCmd.Parameters("@startDate").Value & ",endDate=" & oCmd.Parameters("@endDate").Value & ",countryCode=" & oCmd.Parameters("@countryCode").Value & ",orderSource=" & oCmd.Parameters("@orderSource").Value & ",paymentMethod=" & oCmd.Parameters("@paymentMethod").Value & ",orderStatus=" & oCmd.Parameters("@orderStatus").Value & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub

    'Functions
    Protected Function ShowDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then
            result = FormatDateTime(d.ToString, DateFormat.LongDate) & " " & FormatDateTime(d.ToString, DateFormat.ShortTime)
        End If
        Return result
    End Function
    Protected Function formatSource(ByVal o As Object) As String
        Dim result As String = ""
        If Not IsDBNull(o) Then
            Select Case o.ToString
                Case "shopper"
                    result = "Web Shopper"
                Case "affiliate"
                    result = "Affiliate"
                Case "distributor"
                    result = "Distributor"
                Case "callcentre"
                    result = "Call Centre"
            End Select
        End If
        Return result
    End Function
    Protected Function formatPayment(ByVal o As Object) As String
        Dim result As String = ""
        If Not IsDBNull(o) Then
            Select Case o.ToString
                Case "account"
                    result = "EB Account"
                Case "bankaccount"
                    result = "Bank Account"
                Case "cc"
                    result = "Credit Card"
                Case "cheque"
                    result = "Cheque"
                Case "ideal"
                    result = "iDeal"
                Case "ddebit"
                    result = "Direct Debit"
                Case "paypal"
                    result = "Paypal"
                Case "fastpay"
                    result = "FastPay"
            End Select
        End If
        Return result
    End Function
    Function getStatusColor(ByVal s As Object) As Drawing.Color
        Dim result As Drawing.Color = Drawing.ColorTranslator.FromHtml("black")
        If Not IsDBNull(s) Then If LCase(s.ToString()) = "cancelled" Then result = Drawing.ColorTranslator.FromHtml("red")
        Return result
    End Function

    'Pager functions
    Protected Sub gvOrders_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvOrders.PageIndexChanging
        Dim gv As GridView = CType(sender, GridView)
        gv.PageIndex = e.NewPageIndex
        gv.DataBind()
    End Sub
    Protected Sub gv_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvOrders.DataBound
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerData(gv)
    End Sub
    Protected Sub gv_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvOrders.DataBinding
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerControl(gv)
    End Sub
End Class
