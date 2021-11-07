Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentHistory
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            bindGridView()
        End If
    End Sub
    Protected Sub bindGridView()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim drp As DropDownList
        _login = Master.FindControl("logMaintenance")
        _content = _login.FindControl("ContentPlaceHolder1")
        drp = _content.FindControl("drpType")
        oCmd.CommandType = CommandType.StoredProcedure
        Select Case drp.SelectedValue
            Case "component"
                oCmd = New SqlCommand("procComponentsSelectCH", oConn)
            Case "batch"
                oCmd = New SqlCommand("procComponentBatchesSelectCH", oConn)
            Case "order"
                oCmd = New SqlCommand("procComponentOrdersSelectCH", oConn)
        End Select
        Try
            If oConn.State = 0 Then oConn.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            gvList.DataSource = ds
            gvList.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Change headers
        Select Case drpType.SelectedValue
            Case "component"
                setGVHeader("Component", "Description")
            Case "batch"
                setGVHeader("BatchID", "")
            Case "order"
                setGVHeader("OrderID", "Order Date")
                'Apply cell formatting for the date field
                For Each row As GridViewRow In gvList.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        row.Cells(1).Text = FormatDateTime(row.Cells(1).Text, DateFormat.LongDate)
                    End If
                Next
        End Select
    End Sub
    Protected Sub setGVHeader(ByVal col0 As String, ByVal col1 As String)
        Dim head As GridViewRow = gvList.HeaderRow
        head.Cells(0).Text = col0
        head.Cells(1).Text = col1
    End Sub
End Class
