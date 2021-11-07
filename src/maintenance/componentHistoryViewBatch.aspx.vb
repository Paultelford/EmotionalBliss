Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentHistoryViewBatch
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private sqlDateFrom As DateTime
    Private sqlDateTo As DateTime
    Private _QtyQInPos = 4
    Private _QtyQOutPos = 5
    Private _QtyStockInPos = 6
    Private _QtyStockOutPos = 7
    Private _QtyScrappedInPos = 8
    Private _QtyScrappedOutPos = 9

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            If Not Page.IsPostBack Then
                'bindDates()
                showBatchTitle()
                If Request.QueryString("source") = "chlog" Then
                    'Add back button to take user back to ComponentHistoryLog, pass dates and cid
                    lnkBack.Text = "Back to History Log"
                    lnkBack.NavigateUrl = "componentHistoryViewComponent.aspx?id=" & Request.QueryString("cid") & "&startDate=" & Request.QueryString("startDate") & "&endDate=" & Request.QueryString("endDate")
                End If
            End If
            'setSQLDates()
            bindGridView()
        End If
    End Sub
    Protected Sub date1_onDateChanged(ByVal sender As Object, ByVal e As EventArgs)
        bindGridView()
    End Sub
    Protected Sub bindGridView()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentHistoryByBatchIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@batchID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
            .Parameters("@batchID").Value = Convert.ToInt32(Request.QueryString("id"))
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
    Protected Sub gvHistory_dataBoundTotals(ByVal sender As Object, ByVal e As EventArgs)
        Dim iQIn As Integer = 0
        Dim iQOut As Integer = 0
        Dim iStockIn As Integer = 0
        Dim iStockOut As Integer = 0
        Dim iScrappedIn As Integer = 0
        Dim iScrappedOut As Integer = 0
        For Each row As GridViewRow In gvHistory.Rows
            If row.RowType = DataControlRowType.DataRow Then
                iQIn = iQIn + Convert.ToInt16(row.Cells(_QtyQInPos).Text)
                iQOut = iQOut + Convert.ToInt16(row.Cells(_QtyQOutPos).Text)
                iStockIn = iStockIn + Convert.ToInt16(row.Cells(_QtyStockInPos).Text)
                iStockOut = iStockOut + Convert.ToInt16(row.Cells(_QtyStockOutPos).Text)
                iScrappedIn = iScrappedIn + Convert.ToInt16(row.Cells(_QtyScrappedInPos).Text)
                iScrappedOut = iScrappedOut + Convert.ToInt16(row.Cells(_QtyScrappedOutPos).Text)
            End If
        Next
        Dim fRow As GridViewRow = gvHistory.FooterRow
        fRow.Cells(_QtyQInPos).Text = "<br>" & Convert.ToString(iQIn)
        fRow.Cells(_QtyQOutPos).Text = "<br>" & Convert.ToString(iQOut)
        fRow.Cells(_QtyStockInPos).Text = "<br>" & Convert.ToString(iStockIn)
        fRow.Cells(_QtyStockOutPos).Text = "<br>" & Convert.ToString(iStockOut)
        fRow.Cells(_QtyScrappedInPos).Text = "<br>" & Convert.ToString(iScrappedIn)
        fRow.Cells(_QtyScrappedOutPos).Text = "<br>" & Convert.ToString(iScrappedOut)
    End Sub
    Protected Sub showBatchTitle()
        lblBatch.Text = "Component Batch #" & Request.QueryString("id")
    End Sub
End Class
