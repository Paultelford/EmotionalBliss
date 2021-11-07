Imports System.Data
Imports System.Data.SqlClient


Partial Class maintenance_componentHistoryViewComponent
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private sqlDateFrom As DateTime
    Private sqlDateTo As DateTime
    Private Const _CompBatchIDPos = 2
    Private Const _CompOrderIDPos = 3
    Private Const _QtyQInPos = 5
    Private Const _QtyQOutPos = 6
    Private Const _QtyStockInPos = 7
    Private Const _QtyStockOutPos = 8
    Private Const _QtyScrappedInPos = 9
    Private Const _QtyScrappedOutPos = 10
    Private Const _bQuaratine = 11
    Private Const _ActionIDPos = 12
    Private Const _ActionPos = 1
    Private Const _BatchPos = 2
    Private Const _ProdAssemblyIDPos = 13
    Private Const _QtyPassed = 14
    Private Const _QtyFailed = 15
    Private Const ActionID_ItemRemovedFromQuarantine = 2
    Private Const _EndDatePos = 16

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            If Not Page.IsPostBack Then
                'bindDates()
                showComponentName()
                bindGridView()
            End If
        End If
    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        bindGridView()
    End Sub
    Protected Sub bindGridView()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentBatchByCompIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@compID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
            .Parameters("@compID").Value = Convert.ToInt32(Request.QueryString("id"))
            .Parameters("@startDate").Value = date1.getStartDate()
            .Parameters("@endDate").Value = date1.getEndDate()
        End With
        Try
            If oConn.State = 0 Then oConn.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                gvHistory.DataSource = ds
                gvHistory.DataBind()
                lblDataBoundError.Text = ""
            Else
                lblDataBoundError.Text = "No results found for selected Date Range."
            End If
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
    
    Protected Sub gvHistory_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Quantities must be shunted around here (moved from Quaratine fields to Stock fields) depending on the ActionID
        Dim iQIn As Integer = 0
        Dim iQOut As Integer = 0
        Dim iStockIn As Integer = 0
        Dim iStockOut As Integer = 0
        Dim iScrappedIn As Integer = 0
        Dim iScrappedOut As Integer = 0
        Dim lblQuaratine As Label
        
        Dim fRow As GridViewRow = gvHistory.FooterRow
        For Each row As GridViewRow In gvHistory.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblQuaratine = row.Cells(_bQuaratine).FindControl("lblQuarantine")
                
                Select Case Convert.ToBoolean(lblQuaratine.Text)
                    Case True
                        'History row is Quaratine
                        'Add to Quaratine totals
                        iQIn = iQIn + Convert.ToInt16(row.Cells(_QtyQInPos).Text)
                        iQOut = iQOut + Convert.ToInt16(row.Cells(_QtyQOutPos).Text)
                        'Set Stock columns to 0
                        row.Cells(_QtyStockInPos).Text = "0"
                        row.Cells(_QtyStockOutPos).Text = "0"
                    Case False
                        'History row is Stock 
                        'Move qty's from Quaratine columns to Stock columns
                        row.Cells(_QtyStockInPos).Text = row.Cells(_QtyQInPos).Text
                        row.Cells(_QtyStockOutPos).Text = row.Cells(_QtyQOutPos).Text
                        'Set Quaratine values to 0
                        row.Cells(_QtyQInPos).Text = 0
                        row.Cells(_QtyQOutPos).Text = 0
                        'Add to stock totals
                        iStockIn = iStockIn + Convert.ToInt16(row.Cells(_QtyStockInPos).Text)
                        iStockOut = iStockOut + Convert.ToInt16(row.Cells(_QtyStockOutPos).Text)
                End Select
            End If
        Next
        fRow.Cells(_QtyQInPos).Text = "<br>" & Convert.ToString(iQIn)
        fRow.Cells(_QtyQOutPos).Text = "<br>" & Convert.ToString(iQOut)
        fRow.Cells(_QtyStockInPos).Text = "<br>" & Convert.ToString(iStockIn)
        fRow.Cells(_QtyStockOutPos).Text = "<br>" & Convert.ToString(iStockOut)
    End Sub
    Protected Sub gvHistory_dataBoundTotals(ByVal sender As Object, ByVal e As EventArgs)
        Dim iQIn As Integer = 0
        Dim iQOut As Integer = 0
        Dim iStockIn As Integer = 0
        Dim iStockOut As Integer = 0
        Dim iScrappedIn As Integer = 0
        Dim iScrappedOut As Integer = 0
        Dim iPassed As Integer = 0
        Dim iFailed As Integer = 0
        Dim lblAction As Label
        Dim prodAssemblyID As Label
        Dim lnk As HyperLink
        Dim completeText As String
        Dim lblEndDate As Label
        For Each row As GridViewRow In gvHistory.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblAction = row.FindControl("lblActionID")
                prodAssemblyID = row.Cells(_ProdAssemblyIDPos).FindControl("lblProdAssemblyID")
                Select Case Convert.ToInt16(lblAction.Text)
                    Case 1 'Items recieved into quarantine
                        lnk = row.Cells(_CompBatchIDPos).Controls(0)
                        lnk.NavigateUrl = lnk.NavigateUrl & "&" & date1.getDatesQS & "&cid=" & Request.QueryString("id") & "&source=chlog"
                    Case 2 'Moved from Quarantine to stock
                        lnk = row.Cells(_CompBatchIDPos).Controls(0)
                        lnk.NavigateUrl = lnk.NavigateUrl & "&" & date1.getDatesQS & "&cid=" & Request.QueryString("id") & "&source=chlog"
                    Case 3 'Used in product assembly
                        completeText = "(Complete)"
                        row.Cells(_ActionPos).Text = row.Cells(_ActionPos).Text & " <a href='productAssemblyView.aspx?id=" & prodAssemblyID.Text & "&" & date1.getDatesQS & "&cid=" & Request.QueryString("id") & "'>#" & prodAssemblyID.Text & "</a> "
                        lblEndDate = row.FindControl("lblEndDate")
                        If lblEndDate.Text = "" Then completeText = "(Outstanding)"
                        row.Cells(_ActionPos).Text = row.Cells(_ActionPos).Text & completeText
                    Case 4 'Recycled
                        row.Cells(_BatchPos).Text = "<a href='productAssemblyView.aspx?" & date1.getDatesQS & "&id=" & prodAssemblyID.Text & "&cid=" & Request.QueryString("id") & "'>#R" & prodAssemblyID.Text & "</a>"
                    Case 5 'Scrapped/Faulty
                        row.Cells(_ActionPos).Text = row.Cells(_ActionPos).Text & " <a href='productAssemblyView.aspx?id=" & prodAssemblyID.Text & "&cid=" & Request.QueryString("id") & "&" & date1.getDatesQS & "'>#" & prodAssemblyID.Text & "</a>"
                    Case 6 'Stock/Quarantine levels changed
                        row.Cells(_ActionPos).Text = "<a href='componentHistoryViewComponentPop.aspx?id=" & Convert.ToString(gvHistory.DataKeys(row.RowIndex).Value) & "' target='_blank'>" & row.Cells(_ActionPos).Text & "</a>"
                    Case 7 'Component details Editied
                        row.Cells(_ActionPos).Text = "<a href='componentHistoryViewComponentPop.aspx?id=" & Convert.ToString(gvHistory.DataKeys(row.RowIndex).Value) & "' target='_blank'>" & row.Cells(_ActionPos).Text & "</a>"
                    Case 9 'Warehouse consumable
                        row.Cells(_ActionPos).Text = "<a href='componentHistoryViewComponentPop.aspx?id=" & Convert.ToString(gvHistory.DataKeys(row.RowIndex).Value) & "' target='_blank'>" & row.Cells(_ActionPos).Text & "</a>"
                End Select
                If Convert.ToInt16(lblAction.Text) <> ActionID_ItemRemovedFromQuarantine Then
                    row.Cells(_QtyPassed).Text = "0" 'QtyPassed can only have a value if the current ActionID is 'Item removed from Quarantine'
                End If
                'Add to totals
                iQIn = iQIn + Convert.ToInt16(row.Cells(_QtyQInPos).Text)
                iQOut = iQOut + Convert.ToInt16(row.Cells(_QtyQOutPos).Text)
                iStockIn = iStockIn + Convert.ToInt16(row.Cells(_QtyStockInPos).Text)
                iStockOut = iStockOut + Convert.ToInt16(row.Cells(_QtyStockOutPos).Text)
                iScrappedIn = iScrappedIn + Convert.ToInt16(row.Cells(_QtyScrappedInPos).Text)
                iScrappedOut = iScrappedOut + Convert.ToInt16(row.Cells(_QtyScrappedOutPos).Text)
                'Remove 0's & Links from ComponentBatchID & ComponentOrderID Columns
                If row.Cells(_CompBatchIDPos).HasControls Then
                    lnk = row.Cells(_CompBatchIDPos).Controls(0) 'CompBatchID
                    If lnk.Text = "0" Then lnk.Text = ""
                End If
                If row.Cells(_CompOrderIDPos).HasControls Then
                    lnk = row.Cells(_CompOrderIDPos).Controls(0) 'CompOrderID
                    If lnk.Text = "0" Then lnk.Text = ""
                End If
                'Remove 0's from quantity fields
                If row.Cells(_QtyQInPos).Text = "0" Then row.Cells(_QtyQInPos).Text = ""
                If row.Cells(_QtyQOutPos).Text = "0" Then row.Cells(_QtyQOutPos).Text = ""
                If row.Cells(_QtyStockInPos).Text = "0" Then row.Cells(_QtyStockInPos).Text = ""
                If row.Cells(_QtyStockOutPos).Text = "0" Then row.Cells(_QtyStockOutPos).Text = ""
                If row.Cells(_QtyScrappedInPos).Text = "0" Then row.Cells(_QtyScrappedInPos).Text = ""
                If row.Cells(_QtyScrappedOutPos).Text = "0" Then row.Cells(_QtyScrappedOutPos).Text = ""
                If row.Cells(_QtyPassed).Text = "0" Then row.Cells(_QtyPassed).Text = ""
                If row.Cells(_QtyFailed).Text = "0" Then row.Cells(_QtyFailed).Text = ""
            End If
        Next
        Dim fRow As GridViewRow = gvHistory.FooterRow
        fRow.Cells(_QtyQInPos).Text = "<br>" & Convert.ToString(iQIn)
        fRow.Cells(_QtyQOutPos).Text = "<br>" & Convert.ToString(iQOut)
        fRow.Cells(_QtyStockInPos).Text = "<br>" & Convert.ToString(iStockIn)
        fRow.Cells(_QtyStockOutPos).Text = "<br>" & Convert.ToString(iStockOut)
        fRow.Cells(_QtyScrappedInPos).Text = "<br>" & Convert.ToString(iScrappedIn)
        fRow.Cells(_QtyScrappedOutPos).Text = "<br>" & Convert.ToString(iScrappedOut)
        fRow.Cells(_QtyPassed).Text = "<br>" & Convert.ToString(iPassed)
        fRow.Cells(_QtyFailed).Text = "<br>" & Convert.ToString(iFailed)
    End Sub
 
    Protected Sub btnDateSubmit_clicked(ByVal sender As Object, ByVal e As EventArgs)
        bindGridView()
    End Sub
    Protected Sub showComponentName()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@compID", SqlDbType.Int))
            .Parameters("@compID").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblComponent.Text = ds.Tables(0).Rows(0)("componentName")
            Else
                lblComponent.Text = "Unknown"
            End If
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
    Protected Function formatStartDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then
            result = FormatDateTime(d.ToString, DateFormat.LongDate) & " " & FormatDateTime(d.ToString, DateFormat.ShortTime)
        Else
            result = "Unknown"
        End If

        Return result
    End Function
End Class
