Imports System.Data
Imports System.Data.SqlClient
Imports SiteInclude
Imports System.Collections

Partial Class affiliates_scan
    Inherits BasePage
    Private Const _dvBillAdd_headerPos = 0
    Private Const _dvBillAdd_dataPos = 1
    Private Const _debug As Boolean = True
    Private Const _gvOrderItems_currentStock As Integer = 5
    Private clickThroughID As Integer
    Private bScanError As Boolean = False
    Private bScanOverride As Boolean = False

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            lblComplete.Text = ""
            ScriptManager.RegisterStartupScript(txtOrderNo, Me.GetType, "onloader", "focusElement('" & txtOrderNo.UniqueID & "');", True)
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then
            'Make sure the DefaultEmail dropdown matches the current countries default courier.

        End If
    End Sub

    'Page Events
    Protected Sub dvBillAdd_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim dv As DetailsView = CType(sender, DetailsView)
        For Each row As DetailsViewRow In dv.Rows
            'row.Cells(_dvBillAdd_headerPos).Visible = False
            If row.Cells(_dvBillAdd_dataPos).Text = "*" Then row.Visible = False
        Next
    End Sub
    Protected Sub gvOrderItems_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As GridViewRow In gvOrderItems.Rows
            If row.RowType = DataControlRowType.DataRow Then
                row.Cells(_gvOrderItems_currentStock).Text = getStock(gvOrderItems.DataKeys(row.RowIndex).Value)
            End If
        Next
    End Sub

    'User Events
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'This event is firing when enter is pressed ehile entering data into the tracker box (Probably when the scanner is used too)
        'So only continue with the prepareScan() function it was ligitimately pressed. This can be decided by checking on the visibility of the tracker box, and make sure its emtpy of all text.
        If (Not panTrackerEmail.Visible) And txtTracker.Text = "" Then prepareScan()
    End Sub
    Protected Sub btnTrackerSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Check that all validation is good (valid tracker code, and if part complete, that valid qty's have been entered)
        Page.Validate("tracker")
        If Page.IsValid Then
            If chkPartOrder.Checked Then
                'Make sure that the values entered do not exceed the qty outstanding
                If qtyOK() Then
                    'Final check - make sure there are enough items in stock 
                    If checkStockOK(True) Then
                        startScanCode(hidOrderID.Value, txtTracker.Text, True)
                        panTracker.Visible = False
                        panTrackerEmail.Visible = False
                        txtOrderNo.Text = ""
                        lblCriticalError.Text = ""
                        lblComplete.Text = "Scanning complete for order " & lblUserOrderID.Text
                        clearUserQty()
                        If CInt(drpTrackerQty.SelectedValue) > 1 Then Response.Redirect("scanExtra.aspx?id=" & hidOrderID.Value & "&qty=" & drpTrackerQty.SelectedValue)
                    End If
                End If
            Else
                'Final check - make sure there are enough items in stock 
                If checkStockOK(False) Then
                    startScanCode(hidOrderID.Value, txtTracker.Text, False)
                    panTracker.Visible = False
                    panTrackerEmail.Visible = False
                    txtOrderNo.Text = ""
                    lblCriticalError.Text = ""
                    lblComplete.Text = "Scanning complete for order " & lblUserOrderID.Text
                    If CInt(drpTrackerQty.SelectedValue) > 1 Then Response.Redirect("scanExtra.aspx?id=" & hidOrderID.Value & "&qty=" & drpTrackerQty.SelectedValue)
                End If
            End If
        End If
    End Sub
    Protected Sub btnScanAnyway_click(ByVal sender As Object, ByVal e As EventArgs)
        btnScanAnyway.Visible = False
        lblError.Text = ""
        bScanOverride = True
        prepareScan()
    End Sub
    Protected Sub chkPartOrder_changed(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox = CType(sender, CheckBox)
        If chk.Checked Then
            lblPartOrderMsg.Visible = True
            setQtyVisible(True)
        Else
            lblPartOrderMsg.Visible = False
            setQtyVisible(False)
        End If
    End Sub
    'SelectedIndexChanged
    Protected Sub drpLatestOrders_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        prepareForm() 'Clear and left over data
        'Show the tracker panel
        lblUserOrderID.Text = drpLatestOrders.SelectedItem.Text
        hidOrderID.Value = drpLatestOrders.SelectedValue
        hidPrefix.Value = getOrderPrefix(drpLatestOrders.SelectedValue)
        panTracker.Visible = True
        panTrackerEmail.Visible = True
        focusOnTracker()
    End Sub
    Protected Sub drpOutstandingQty_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        drpLatestOrders.DataBind()
    End Sub
    Protected Sub drpTrackerQty_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'User has changed the qty dropdown, pass focus back to the trackerbox
        focusOnTracker()
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If LCase(drpType.SelectedValue) = "special" Then tblEmail.Visible = True
        focusOnTracker()
    End Sub
    Protected Sub drpCourier_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@courierID"}
            Dim paramValue() As String = {drpCourier.SelectedValue.ToString()}
            Dim paramType() As SqlDbType = {SqlDbType.Int}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procCourierByIDSelect2")
            If dt.Rows.Count > 0 Then
                For Each li As ListItem In drpType.Items
                    If LCase(li.Value) = LCase(dt.Rows(0)("defaultEmail")) Then
                        drpType.SelectedValue = li.Value
                    End If
                Next
            End If
        Catch ex As Exception
            siteInclude.addError("scan.aspx.vb", "drpCourier_selectedIndexChanged(courierID=" & drpCourier.SelectedValue & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try

    End Sub

    'Subs
    Protected Sub prepareScan()
        Page.Validate("scan")
        If Page.IsValid Then
            'Check the order number for the following validation criteria
            'Must be an order from the distributors country
            'Must be a complete or part complete order.
            '------------------------------------------------------------
            prepareForm() 'Clear any left over data
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procShopOrderByNewOrderIDCountrySelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim bError As Boolean = False
            Dim sError As String = ""
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters("@newOrderID").Value = CType(parseUserOrderID(txtOrderNo.Text), Integer)
                .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    If (LCase(ds.Tables(0).Rows(0)("orderCountryCode")) = LCase(Session("EBAffEBDistributorCountryCode"))) And (CType(ds.Tables(0).Rows(0)("orderPrefix"), Integer) <> 50) Then
                        If LCase(ds.Tables(0).Rows(0)("orderStatus")) = "paid" Or bScanOverride Then
                            'All ok, show the tracker panel. - ALL hid variables must also be set in the getOrderPrefix Sub#########################
                            clickThroughID = ds.Tables(0).Rows(0)("affiliateClickThroughID")
                            hidAffClickThroughID.Value = clickThroughID
                            lblUserOrderID.Text = ds.Tables(0).Rows(0)("NID")
                            panTracker.Visible = True
                            panTrackerEmail.Visible = True
                            hidOrderID.Value = ds.Tables(0).Rows(0)("ID")
                            hidPrefix.Value = getOrderPrefix(ds.Tables(0).Rows(0)("ID"))
                            hidGoodsTotal.Value = ds.Tables(0).Rows(0)("goods")
                            hidOrderVatTotal.Value = ds.Tables(0).Rows(0)("vatTotal")
                            hidShippingTotal.Value = ds.Tables(0).Rows(0)("shippingTotal")
                            hidShippingVatRate.Value = ds.Tables(0).Rows(0)("shippingVatRate")
                            hidShipping.Value = ds.Tables(0).Rows(0)("shipping")
                            hidGoodsTotalInc.Value = ds.Tables(0).Rows(0)("goodsTotalInc")
                            hidFirstScan.Value = CStr(CType(ds.Tables(0).Rows(0)("firstScan"), Boolean))
                            hidDiscount.Value = ds.Tables(0).Rows(0)("discount")
                            If LCase(ds.Tables(0).Rows(0)("orderStatus")) = "partcomplete" Then
                                forcePartComplete()
                            End If
                            focusOnTracker()
                        Else
                            If LCase(ds.Tables(0).Rows(0)("orderStatus")) = "placed" Then
                                bError = True
                                sError = "The order has not been paid yet."
                            Else
                                bError = True
                                sError = "The order has already been scanned."
                                btnScanAnyway.Visible = True 'Give the user the chance to continue with scan even though its already been scanned
                                chkPartOrder.Visible = False 'Hide the part order box, as this will probably be a return.
                            End If
                        End If
                    Else
                        bError = True
                        sError = "Order ID belongs to a different country/distrbutor."
                    End If

                Else
                    bError = True
                    sError = "Order ID Not found"
                End If
                If bError Then
                    lblError.Text = sError
                Else
                    lblError.Text = ""

                End If

            Catch ex As Exception
                lblError.Text = ex.ToString
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Sub startScanCode(ByVal orderID As String, ByVal tracker As String, ByVal partComplete As Boolean)
        'Create connection that will be used to log all scan results
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        'Set variables needed for scan code
        'orderID - passed in
        'tracker - passed in
        Dim orderCountryCode As String = Session("EBAffEBDistributorCountryCode")
        Dim userOrderID As String = lblUserOrderID.Text
        Dim result As String
        Dim distID As Integer = Session("EBAffID")
        Dim orderPrefix As Integer = hidPrefix.Value
        Dim affClickThroughID As Integer = CType(hidAffClickThroughID.Value, Integer)
        Dim isFirstScan As Boolean = CType(hidFirstScan.Value, Boolean)
        'Check for vaild ID
        If orderID > 0 Then
            If Not orderAlreadyScanned(orderID) Then
                'Clear error msg
                lblError.Text = ""
                '####Run once####
                If isFirstScan Then
                    'Remove items from stock
                    result = removeItemsFromStock(orderID, distID, orderPrefix, partComplete)
                    addScanResult("removeItemsFromStock", result, orderID, oConn)
                    'Assign royalties
                    result = assignRoyalties(orderID)
                    addScanResult("assignRoyalties", result, orderID, oConn)
                    'Calculate Affiliates Click Through credit (if any)
                    If affClickThroughID > 0 Then
                        result = calculateAffiliateClickThrough(orderID, affClickThroughID)
                        addScanResult("calculateAffiliateClickThrough", result, orderID, oConn)
                    End If
                    'If affiliate order, then add to aff/dist statement
                    If orderPrefix = "40" Then
                        Dim si As New siteInclude
                        si.affAddToStatement(hidAffID.Value, 0, hidOrderTotal.Value, orderID, 0, 2)
                        'add to distributor statement
                        si.affAddToStatement(GetDistributorID(Session("EBAffEBDistributorCountryCode")), hidOrderTotal.Value, 0, orderID, 0, 2)
                        si = Nothing
                    End If
                    'Add order details to salesLedger table
                    result = addToSalesLedger(orderID, orderCountryCode, orderPrefix, hidGoodsTotal.Value, hidOrderVatTotal.Value, hidShipping.Value, hidShippingVatRate.Value, hidGoodsTotalInc.Value, hidDiscount.Value)
                    addScanResult("addToSalesLedger", result, orderID, oConn)
                    'Set default courier depending on countryCode
                    result = setDefaultCourier(orderID, orderCountryCode)
                    addScanResult("setDefaultCourier", result, orderID, oConn)
                End If 'isFirstScan
                '####Run on each scan####
                'Set order status to 'complete' or 'partcomplete' (Also sets firstScan=False)
                result = setOrderComplete(orderID, partComplete)
                addScanResult("setOrderComplete", result, orderID, oConn)
                'Add tracker to database
                result = addTracker(orderID, tracker)
                addScanResult("addTracker", result, orderID, oConn)
                'Add to orderlog page
                result = addToOrderLog(orderID, tracker)
                addScanResult("addToOrderLog", result, orderID, oConn)
                'If page is live (ie not in debug mode) then hide the error messages.
                'If Not _debug Then lblError.Visible = False
                If InStr(lblError.Text, "addtracker() error") Then
                    'Show critical error to alert user of scan failure
                    lblCriticalError.Text = "A critical error occured and the scan details were not saved. Please contatct support and try again later."
                End If
            Else
                'order has already been scanned, just add to order log and tracker table.
                result = addToOrderLog(orderID, tracker)
                addScanResult("addToOrderLog", result, orderID, oConn)
                'Add tracker to database
                result = addTracker(orderID, tracker)
                addScanResult("addTracker", result, orderID, oConn)
            End If
        End If
        'Clear the dropdown items and rebind so user can scan another item (This will also remove the recently scanned order from the list)
        drpLatestOrders.Items.Clear()
        drpLatestOrders.Items.Add(New ListItem("Select...", "0"))
        drpLatestOrders.DataBind()
    End Sub
    Protected Sub addToRoyaltyTransactions(ByVal orderID As Integer, ByVal distBuyingID As Integer, ByVal royalty As Decimal, ByVal affID As Integer, ByVal currency As String, ByRef oConn As SqlConnection)
        Dim oCmd As New SqlCommand("procRoyaltyTransactionsInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@distBuyingID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@credit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@debit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 3))
            .Parameters("@orderID").Value = orderID
            .Parameters("@distBuyingID").Value = distBuyingID
            .Parameters("@actionID").Value = 1
            .Parameters("@affID").Value = affID
            .Parameters("@credit").Value = royalty
            .Parameters("@debit").Value = 0
            .Parameters("@currency").Value = currency
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Dispose()
        End Try
    End Sub
    Protected Sub addScanResult(ByVal func As String, ByVal msg As String, ByVal orderID As Integer, ByRef conn As SqlConnection)
        Dim oCmd As New SqlCommand("procScanInsert", conn)
        Dim bSuccess As Boolean = False
        If msg = "0" Then bSuccess = True
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@func", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@msg", SqlDbType.VarChar, 3000))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@success", SqlDbType.Bit))
            .Parameters("@func").Value = func
            .Parameters("@msg").Value = msg
            .Parameters("@orderID").Value = orderID
            .Parameters("@success").Value = bSuccess
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = lblError.Text & "Error logged: " & func & ex.ToString & "<br>"
        Finally
            oCmd.Dispose()
        End Try
        If Not bSuccess Then bScanError = True
    End Sub
    Protected Sub setCourier(ByVal orderID As Integer, ByVal id As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderCourierByIDUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@courierID", SqlDbType.Int))
            .Parameters("@orderID").Value = orderID
            .Parameters("@courierID").Value = id
        End With
        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
        oCmd.ExecuteNonQuery()
        oCmd.Dispose()
        oConn.Dispose()
    End Sub
    Protected Sub forcePartComplete()
        'If order already partCompleted, then set the Part Complete checkbox to ticked and disable it
        chkPartOrder.Checked = True
        chkPartOrder.Enabled = False
        lblPartOrderMsg.Visible = True
        lblPartOrderMsg.Text = "<b>This order is currently Part Complete. Please enter the qty's of each item to be despactched today.</b>"
        lblItemsInOrder.Text = "<b>Items still to be Despatched</b>"
        setQtyVisible(True)
        'Affiliate clickthroughs are calculated on first scan. If page gets to this point, then its the second scan (or greater)
        hidClickThroughAlreadyDone.Value = "true"
    End Sub
    Protected Sub clearUserQty()
        Dim txt As TextBox
        For Each row As GridViewRow In gvOrderItems.Rows
            txt = row.FindControl("txtQty")
            txt.Text = "0"
        Next
        gvOrderItems.DataBind()
    End Sub
    Protected Sub prepareForm()
        lblComplete.Text = ""
        txtTracker.Text = ""
        lblError.Text = ""
        lblCriticalError.Text = ""
        chkPartOrder.Checked = False
        chkPartOrder.Enabled = True
        lblItemsInOrder.Text = "<b>Items in Order</b>"
        lblPartOrderMsg.Text = "<b>Please enter the qty's of each item to be despactched today</b>"
        lblPartOrderMsg.Visible = False
        drpCourier.DataBind()
        drpCourier_selectedIndexChanged(New Object, New EventArgs)
    End Sub
    Protected Sub setQtyVisible(ByVal vis As Boolean)
        Dim txt As TextBox
        For Each row As GridViewRow In gvOrderItems.Rows
            If row.RowType = DataControlRowType.DataRow Then
                txt = row.FindControl("txtQty")
                txt.Visible = vis
            End If
        Next
    End Sub
    Protected Sub focusOnTracker()
        ScriptManager.RegisterStartupScript(txtTracker, Me.GetType, "onloader", "focusElement('" & txtTracker.UniqueID & "');", True)
    End Sub

    'Functions
    Protected Function checkStockOK(ByVal partComplete As Boolean) As Boolean
        Dim stockOK As Boolean = True
        Dim stockToRemove As Integer
        Dim lbl As Label
        Dim txt As TextBox
        Dim currentStock As Integer
        For Each row As GridViewRow In gvOrderItems.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If stockOK Then
                    'Get the qty to be removed from stock
                    If partComplete Then
                        txt = row.FindControl("txtQty")
                        stockToRemove = CType(txt.Text, Integer)
                    Else
                        lbl = row.FindControl("lblQty")
                        stockToRemove = CType(lbl.Text, Integer)
                    End If
                    'Get current stock level
                    currentStock = CType(row.Cells(_gvOrderItems_currentStock).Text, Integer)
                    'Compare the qty to be removed with the curent stock level
                    If stockToRemove > currentStock Then stockOK = False
                End If
            End If
        Next
        If Not stockOK Then
            'Show error message
            lblCriticalError.Text = "You do not have enough stock to scan this order."
        End If
        Return stockOK
    End Function
    Protected Function getStock(ByVal affProductBuyingID As Integer) As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateDistributorStockByAffProductBuyingIDStockSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affProductBuyingID", SqlDbType.Int))
            .Parameters("@affProductBuyingID").Value = affProductBuyingID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If (Not IsDBNull(ds.Tables(0).Rows(0)("stockQty"))) Then result = ds.Tables(0).Rows(0)("stockQty")
            End If
        Catch ex As Exception
            lblError.Text = lblError.Text & ex.ToString
        End Try
        Return result
    End Function
    Protected Function orderAlreadyScanned(ByVal id As Integer) As Boolean
        Dim result As Boolean = False
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procTrackerByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = True
        Catch ex As Exception
            lblError.Text = ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function GetDistributorID(ByVal countryCode As String) As Integer
        'Returns a specified countrys DistributorID
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliatesDistByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0)("affID")
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function getDistributorsProductBuyingID(ByVal saleID As Integer) As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        'Dim oCmd As New SqlCommand("", oConn)
    End Function
    Protected Function parseUserOrderID(ByVal d As String)
        'Makes sure user entry is an integer.
        'If user enters country code then it gets stripped
        Dim result As Integer
        If Not IsNumeric(d) Then
            Dim str As String = ""
            For iLoop As Integer = 0 To Len(d) - 1
                If IsNumeric(d.Substring(iLoop, 1)) Then str = str & d.Substring(iLoop, 1)
            Next
            Try
                result = CType(str, Integer)
            Catch ex As Exception
                Response.Write("Input='" & d & "'<br>Error=" & ex.ToString)
            End Try
        Else
            result = CType(d, Integer)
        End If
        Return result
    End Function
    Protected Function getOrderPrefix(ByVal id As Integer) As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByIDPrefixSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As Integer = 0
        Dim clickThroughID As String = ""
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0)("orderPrefix")
                clickThroughID = ds.Tables(0).Rows(0)("affiliateClickThroughID")
                hidAffClickThroughID.Value = clickThroughID
                hidFirstScan.Value = CStr(CType(ds.Tables(0).Rows(0)("firstScan"), Boolean))
                hidGoodsTotal.Value = ds.Tables(0).Rows(0)("goods")
                'hidGoodsVatRate.Value = ds.Tables(0).Rows(0)("goodsVatRate")
                hidOrderVatTotal.Value = ds.Tables(0).Rows(0)("vatTotal")
                hidShippingTotal.Value = ds.Tables(0).Rows(0)("shippingTotal")
                hidShipping.Value = ds.Tables(0).Rows(0)("shipping")
                hidShippingVatRate.Value = ds.Tables(0).Rows(0)("shippingVatRate")
                hidGoodsTotalInc.Value = ds.Tables(0).Rows(0)("goodsTotalInc")
                hidOrderTotal.Value = ds.Tables(0).Rows(0)("orderTotal")
                hidAffID.Value = ds.Tables(0).Rows(0)("affiliateID")
                hidDiscount.Value = ds.Tables(0).Rows(0)("discount")
                If LCase(ds.Tables(0).Rows(0)("orderStatus")) = "partcomplete" Then
                    gvOrderItems.DataBind()
                    forcePartComplete()
                End If
            End If

        Catch ex As Exception
            lblError.Text = "Error retrieving orderPrefix<br>" & ex.Message
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function qtyOK()
        Dim result As Boolean = True
        Dim txt As TextBox
        Dim lbl As Label
        For Each row As GridViewRow In gvOrderItems.Rows
            txt = row.FindControl("txtQty")
            lbl = row.FindControl("lblQty")
            If CType(txt.Text, Integer) > CType(lbl.Text, Integer) Then result = False
        Next
        If Not result Then lblCriticalError.Text = lblCriticalError.Text & "You are trying to despatch more items than have been ordered.<br>"
        Return result
    End Function


    '******* Start of scan functions *******
    Protected Function setOrderComplete(ByVal id As Integer, ByVal partComplete As Boolean) As String
        Dim result As String = "0"
        Try
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procShopOrderByIDStatusScanUpdate", oConn)
            Dim txtQty As TextBox
            Dim lblQty As Label
            Dim setPartCompleteToFullyComplete As Boolean = True
            If partComplete Then
                For Each row As GridViewRow In gvOrderItems.Rows
                    txtQty = row.FindControl("txtQty")
                    lblQty = row.FindControl("lblQty")
                    If lblQty.Text <> txtQty.Text Then setPartCompleteToFullyComplete = False 'This will stay true if max qtys are despatched (ie order status should be changed from partComplete to complete)
                Next
            End If
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@distStatus", SqlDbType.VarChar, 20))
                .Parameters("@orderID").Value = id
                .Parameters("@status").Value = "complete"
                .Parameters("@distStatus").Value = "despatched"
            End With
            If partComplete And Not setPartCompleteToFullyComplete Then
                oCmd.Parameters("@status").Value = "partcomplete"
                oCmd.Parameters("@distStatus").Value = "partdespatched"
            End If
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw (ex)
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Catch ex As Exception
            result = ex.ToString
            lblError.Text = lblError.Text & "setOrderComplete() error occured.<br>"
        End Try
        Return result
    End Function
    Protected Function removeItemsFromStock(ByVal id As Integer, ByVal distID As String, ByVal orderprefix As String, ByVal partComplete As Boolean) As String
        Dim result As String = "0"
        Try
            If orderprefix <> 50 Then
                Dim si As New siteInclude
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procShopOrderItemsByIDSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                Dim txtQty As TextBox
                Dim ht As New Hashtable
                Dim partComplete_complete As Boolean = True
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                    .Parameters("@orderID").Value = id
                End With
                If partComplete Then
                    'Loop though all qty boxes and add to a hash table with the distBuyingID as the key
                    For Each row As GridViewRow In gvOrderItems.Rows
                        txtQty = row.FindControl("txtQty")
                        ht.Add(gvOrderItems.DataKeys(row.RowIndex).Value, txtQty.Text)
                    Next
                End If
                Try
                    'Get dataset of items in order
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        For Each row As DataRow In ds.Tables(0).Rows
                            If partComplete Then
                                'If more then 0 items are to be removed from stock, then go ahead and make the update.
                                If CType(ht(row("distBuyingID")), Integer) > 0 Then
                                    si.affStockUpdate(row("distBuyingID"), distID, id, 0, 0, CType(ht(row("distBuyingID")), Integer), 4, False, "")
                                End If
                            Else
                                si.affStockUpdate(row("distBuyingID"), distID, id, 0, 0, row("qty"), 4, False, "")
                            End If
                        Next
                        si = Nothing
                    End If
                Catch ex As Exception
                    Throw (ex)
                Finally
                    ds.Dispose()
                    da.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                    ht.Clear()
                    ht = Nothing
                End Try
            End If
        Catch ex As Exception
            result = ex.ToString
            lblError.Text = lblError.Text & "removeItemsFromStock() error occured.<br>"
        End Try

        Return result
    End Function
    Protected Function addTracker(ByVal id As Integer, ByVal tracker As String) As String
        Dim result As String = "0"
        Try
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procTrackerInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@tracker", SqlDbType.VarChar, 30))
                .Parameters.Add(New SqlParameter("@emailType", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@emailBody", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@courierID", SqlDbType.Int))
                .Parameters("@orderID").Value = id
                .Parameters("@tracker").Value = tracker
                .Parameters("@emailType").Value = drpType.SelectedValue
                .Parameters("@emailBody").Value = txtEmail.Text
                .Parameters("@courierID").Value = drpCourier.SelectedValue
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw ex
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Catch ex As Exception
            result = ex.ToString
            lblError.Text = lblError.Text & "addTracker() error occured.<br>"
        End Try
        Return result
    End Function
    Protected Function calculateAffiliateClickThroughOld(ByVal orderID As Integer, ByVal affClickThroughID As Integer) As String
        Dim result As String = "0"
        If Not CType(hidClickThroughAlreadyDone.Value, Boolean) Then
            Try
                'Get affiliates percentage, and order goods subtotal
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procShopOrderByIDClickThroughSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                    .Parameters("@orderID").Value = orderID
                    .Parameters("@affID").Value = affClickThroughID
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        Dim goodsExcVat As Decimal = ds.Tables(0).Rows(0)("goods")
                        Dim percentage As Decimal = ds.Tables(0).Rows(0)("affClickThrough")
                        Dim royalty As Decimal = FormatNumber((goodsExcVat / 100) * percentage, 2)
                        Dim si As New siteInclude
                        si.affAddToStatement(affClickThroughID, royalty, 0, orderID, 0, 13)
                        si = Nothing
                    End If
                Catch ex As Exception
                    Throw ex
                Finally
                    ds.Dispose()
                    da.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            Catch ex As Exception
                result = ex.ToString
                lblError.Text = lblError.Text & "calculateAffiliateClickThrough() error occured.<br>" & ex.ToString
            End Try
        End If
        Return result
    End Function
    Protected Function calculateAffiliateClickThrough(ByVal orderID As Integer, ByVal affClickThroughID As Integer) As String
        Dim result As String = "0"
        If Not CType(hidClickThroughAlreadyDone.Value, Boolean) Then
            Try
                'Get affiliates percentage, and order goods subtotal
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procAffiliateClickThroughByOrderIDEarningSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                Dim earning As Decimal = 0
                Dim percentage As Decimal = 0
                Dim totalEarning As Decimal = 0
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                    .Parameters("@orderID").Value = orderID
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        For Each row As DataRow In ds.Tables(0).Rows
                            earning = 0
                            percentage = 0
                            If Not IsDBNull(row("earning")) Then earning = CDec(row("earning"))
                            If Not IsDBNull(row("percentage")) Then percentage = CDec(row("percentage"))
                            totalEarning = totalEarning + earning
                            addToAffiliateClickThroughLog(orderID, affClickThroughID, earning, row("affProductName"), row("qty"), row("price"), percentage)
                        Next
                        Dim si As New siteInclude
                        si.affAddToStatement(affClickThroughID, totalEarning, 0, orderID, 0, 13)
                        si = Nothing
                    End If
                Catch ex As Exception
                    Throw ex
                Finally
                    ds.Dispose()
                    da.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            Catch ex As Exception
                result = ex.ToString
                lblError.Text = lblError.Text & "calculateAffiliateClickThrough() error occured.<br>" & ex.ToString
            End Try
        End If
        Return result
    End Function
    Protected Sub addToAffiliateClickThroughLog(ByVal orderID As Integer, ByVal affID As Integer, ByVal earning As Decimal, ByVal product As String, ByVal qty As Integer, ByVal price As Decimal, ByVal percentage As Decimal)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateClickThroughLogInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@product", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@amount", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@percentage", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@unitPrice", SqlDbType.Decimal))
            .Parameters("@affID").Value = affID
            .Parameters("@orderID").Value = orderID
            .Parameters("@product").Value = product
            .Parameters("@qty").Value = qty
            .Parameters("@amount").Value = earning
            .Parameters("@percentage").Value = percentage
            .Parameters("@unitPrice").Value = price
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("scan", ex.ToString)
            Throw ex
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function addToSalesLedger(ByVal id As Integer, ByVal countryCode As String, ByVal orderPrefix As Integer, ByVal goods As Decimal, ByVal vatTotal As Decimal, ByVal shipping As Decimal, ByVal shippingVat As Decimal, ByVal goodsInc As Decimal, ByVal voucherDiscount As Decimal) As String
        Dim result As String = "0"
        Dim si As New siteInclude
        si.addToSalesLedger(id, 0, 1, countryCode, orderPrefix, goods + shipping + voucherDiscount, 0, vatTotal, 0)
        si = Nothing
        Return result
    End Function
    Protected Function addToSalesLedgerOld(ByVal id As Integer, ByVal countryCode As String, ByVal orderPrefix As Integer, ByVal goods As Decimal, ByVal vatTotal As Decimal, ByVal shipping As Decimal, ByVal shippingVat As Decimal, ByVal goodsInc As Decimal) As String
        Dim result As String = "0"
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSalesLedgerInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@returnID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@orderPrefix", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@ledgerCredit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@ledgerDebit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@ledgerVatCredit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@ledgerVatDebit", SqlDbType.Decimal))
            .Parameters("@orderID").Value = id
            .Parameters("@returnID").Value = 0
            .Parameters("@actionID").Value = 1
            .Parameters("@countryCode").Value = countryCode
            .Parameters("@orderPrefix").Value = orderPrefix
            .Parameters("@ledgerCredit").Value = goods + shipping
            .Parameters("@ledgerDebit").Value = 0
            '.Parameters("@ledgerVatCredit").Value = (goodsInc - goods) + (shipping * (shippingVat / 100))
            .Parameters("@ledgerVatCredit").Value = vatTotal
            .Parameters("@ledgerVatDebit").Value = 0
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            result = ex.ToString
            lblError.Text = lblError.Text & "addToSalesLedger() error occured.<br>" & ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function setDefaultCourier(ByVal orderID As Integer, ByVal orderCountryCode As String)
        Dim result As String = "0"
        Dim courierID As Integer = 1
        'Grab default courier for countrycode from couriers table. If nothing exists just set to DHL. It can be changed/set before dispatch.
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCourierByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = orderCountryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                ID = CInt(ds.Tables(0).Rows(0)("courierID"))
                setCourier(orderID, ID)
            End If
        Catch ex As Exception
            result = ex.ToString
            lblError.Text = lblError.Text & "setDefaultCourier() error occured, default courier has been set.<br>" & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function addToOrderLog(ByVal orderid As Integer, ByVal tracker As String) As String
        Dim result As String = "0"
        Dim countryCode As String = LCase(Session("EBAffEBDistributorCountryCode"))
        Dim msg As String = "Scan completed successfully"
        'Add message that customer cannot view
        Try
            If bScanError Then msg = "Scan completed with errors"
            siteInclude.AddToOrderLog(orderid, msg, Membership.GetUser.UserName, False)
        Catch ex As Exception
            result = ex.ToString
        End Try
        'Add message for customer tracking
        Try
            msg = drpCourier.SelectedItem.Text & " has collected your order.<br><br>"
            'msg = msg & "Please click <a href='http://track.dhl.co.uk/tracking/wrd/run/wt_xtrack_pw.entrypoint' target='_blank'>here</a> and enter the following tracker reference number in the 'Ident Code' column: " & txtTracker.Text & "<br><br>"
            Select Case countryCode
                Case "us"
                    msg = msg & "Tracker reference: <a target='_blank' href='" & siteInclude.getTrackerURL(tracker, countryCode) & "'>" & tracker & "</a><br>"
                    'msg = msg & "Tracker reference: <a target='_blank' href='http://www.dhl.co.uk/publish/gb/en/eshipping/international_air.high.html?pageToInclude=RESULTS&type=trackindex&awb_hidden=" & tracker & "+&brand=DHL'>" & tracker & "</a><br>"
                Case "ca"
                    'msg = msg & "Tracker reference: <a target='_blank' href='http://www.dhl.co.uk/publish/gb/en/eshipping/international_air.high.html?pageToInclude=RESULTS&type=trackindex&awb_hidden=" & tracker & "+&brand=DHL'>" & tracker & "</a><br>"
                    msg = msg & "Tracker reference: <a target='_blank' href='" & siteInclude.getTrackerURL(tracker, countryCode) & "'>" & tracker & "</a><br>"
                Case Else
                    'msg = msg & "Tracker reference: <a target='_blank' href='http://track.dhl.co.uk/tracking/wrd/run/wt_xhistory_pw.execute?PCL_NO=" & tracker & "&PCL_INST=1&COLLDATE=&CNTRY=" & UCase(Session("EBAffEBDistributorCountryCode")) & "'>" & tracker & "</a><br>"
                    msg = msg & "Tracker reference: <a target='_blank' href='" & siteInclude.getTrackerURL(tracker, countryCode) & "'>" & tracker & "</a><br>"
            End Select
            msg = msg & "(Click the tracker number to view details)<br><br>"
            msg = msg & "Please note the information will not be entered into the " & drpCourier.SelectedItem.Text & " tracker system until approximately 6pm on the day of despatch."
            siteInclude.AddToOrderLog(orderid, msg, Membership.GetUser.UserName, True)
        Catch ex As Exception
            If result <> "0" Then result = result & ex.ToString
        End Try
        Return result
    End Function
    Protected Function assignRoyalties(ByVal orderID As Integer) As String
        Dim result As String = "0"
        'Get recordset of earners royalties
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procRoyaltyEarningsByOrderIDRoyaltySelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim ht As New Hashtable
        Dim totalRoyalty As Decimal = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@retail", SqlDbType.Bit))
            .Parameters("@orderID").Value = orderID
            .Parameters("@retail").Value = True
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            For Each row As DataRow In ds.Tables(0).Rows
                addToRoyaltyTransactions(orderID, row("distBuyingID"), row("royalty"), row("affID"), row("orderCurrency"), oConn)
                'Keep track of each royalty earners total, as items are returned seperately
                If ht.ContainsKey("aff" & row("affID")) Then
                    'Add to
                    ht("aff" & row("affID")) = ht("aff" & row("affID")) + row("royalty")

                Else
                    'Create new
                    ht.Add("aff" & row("affID"), row("royalty"))
                End If

                totalRoyalty = totalRoyalty + CDec(row("royalty"))
            Next
            'Add Earner Totals to aff statement
            For Each item As DictionaryEntry In ht
                addToAffStatement(Replace(item.Key, "aff", ""), item.Value, orderID)
            Next

        Catch ex As Exception
            Throw ex
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Sub addToAffStatement(ByVal affID As Integer, ByVal royaltyAmount As Decimal, ByVal orderID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateStatementInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@statementCredit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@statementDebit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@extOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@linkedPrefix", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@cheque", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@reason", SqlDbType.VarChar, 2000))
            .Parameters.Add(New SqlParameter("@transDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
            .Parameters("@affID").Value = affID
            .Parameters("@actionID").Value = 14
            .Parameters("@statementCredit").Value = royaltyAmount
            .Parameters("@statementDebit").Value = 0
            .Parameters("@orderID").Value = orderID
            .Parameters("@extOrderID").Value = 0
            .Parameters("@linkedPrefix").Value = ""
            .Parameters("@cheque").Value = ""
            .Parameters("@reason").Value = ""
            .Parameters("@transDate").Value = Now()
            .Parameters("@pk").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    '*************************************
    '******* End of scan functions *******
    '*************************************





End Class

