Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentHistoryViewOrder
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            If Not Page.IsPostBack Then
                bindDates()
                showBatchTitle()
            End If
            setSQLDates()
            bindGridView()
        End If
    End Sub
    Protected Sub bindGridView()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentHistoryByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@compOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
            .Parameters("@compOrderID").Value = Convert.ToInt32(Request.QueryString("id"))
            .Parameters("@startDate").Value = getStartDate()
            .Parameters("@endDate").Value = getEndDate()
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
    Protected Sub bindDates()
        Dim aMonth As New ArrayList
        Dim aYear As New ArrayList
        Dim tnow As DateTime = DateAndTime.Now
        aMonth.Add("January")
        aMonth.Add("February")
        aMonth.Add("March")
        aMonth.Add("April")
        aMonth.Add("May")
        aMonth.Add("June")
        aMonth.Add("July")
        aMonth.Add("August")
        aMonth.Add("September")
        aMonth.Add("October")
        aMonth.Add("November")
        aMonth.Add("December")
        For iLoop As Integer = 1 To 31
            drpDayFrom.Items.Add(New ListItem(iLoop, iLoop))
            drpDayTo.Items.Add(New ListItem(iLoop, iLoop))
        Next
        For iLoop As Integer = 2006 To tnow.Year
            drpYearFrom.Items.Add(New ListItem(iLoop, iLoop))
            drpYearTo.Items.Add(New ListItem(iLoop, iLoop))
        Next
        For iLoop As Integer = 1 To 12
            drpMonthFrom.Items.Add(New ListItem(aMonth(iLoop - 1), iLoop))
            drpMonthTo.Items.Add(New ListItem(aMonth(iLoop - 1), iLoop))
        Next
        drpDayTo.SelectedValue = DateTime.DaysInMonth(tnow.Year, tnow.Month)
        drpMonthFrom.SelectedValue = tnow.Month
        drpMonthTo.SelectedValue = tnow.Month
        drpYearFrom.SelectedValue = tnow.Year
        drpYearTo.SelectedValue = tnow.Year
    End Sub
    Protected Sub gvHistory_dataBoundTotals(ByVal sender As Object, ByVal e As EventArgs)
        Dim iQIn As Integer = 0
        Dim iQOut As Integer = 0
        Dim iStockIn As Integer = 0
        Dim iStockOut As Integer = 0
        Dim iScrappedIn As Integer = 0
        Dim iScrappedOut As Integer = 0
        'Try
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
        ' Catch ex As Exception
        'Response.Write(ex)
        'Response.End()
        'End Try
    End Sub
    Protected Sub setSQLDates()
        sqlDateFrom = Convert.ToDateTime(Convert.ToString(drpDayFrom.SelectedItem) & " " & Convert.ToString(drpMonthFrom.SelectedValue) & " " & Convert.ToString(drpYearFrom.SelectedValue) & " 0:00")
        sqlDateTo = Convert.ToDateTime(Convert.ToString(drpDayTo.SelectedItem) & " " & Convert.ToString(drpMonthTo.SelectedValue) & " " & Convert.ToString(drpYearTo.SelectedValue) & " 23:59:59")

    End Sub
    Protected Function getStartDate() As Date
        Return sqlDateFrom
    End Function
    Protected Function getEndDate() As Date
        Return sqlDateTo
    End Function
    Protected Sub btnDateSubmit_clicked(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub showBatchTitle()
        lblOrderID.Text = "Component Order #" & Request.QueryString("id")
    End Sub
End Class
