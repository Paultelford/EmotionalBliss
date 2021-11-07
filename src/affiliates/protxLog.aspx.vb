Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_protxLog
    Inherits BasePage
    Private Const _gvLog_DayPos As Integer = 0
    Private Const _gvLog_TSPos As Integer = 2
    Private Const _gvLog_TSAPos As Integer = 4
    Private Const _gvLog_RSPos As Integer = 6
    Private Const _gvLog_RSAPos As Integer = 8
    Private Const _gvLog_TFPos As Integer = 10
    Private Const _gvLog_TFAPos As Integer = 12
    Private Const _gvLog_RFPos As Integer = 14
    Private Const _gvLog_RFAPos As Integer = 16
    Private Const _gvDay_vendorTxCodePos As Integer = 2
    Private _DayTot As Integer = 0
    Private _TSTot As Integer = 0
    Private _TSATot As Decimal = 0
    Private _RSTot As Integer = 0
    Private _RSATot As Decimal = 0
    Private _TFTot As Integer = 0
    Private _TFATot As Decimal = 0
    Private _RFTot As Integer = 0
    Private _RFATot As Decimal = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub gvLog_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvDay.Visible = True
    End Sub
    Protected Sub gvLog_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProtxDayByCountryDateSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim lnk As LinkButton
            lnk = e.Row.FindControl("lnkDate")
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@date", SqlDbType.VarChar, 10))
                .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
                .Parameters("@date").Value = FormatDateTime(lnk.Text, DateFormat.ShortDate)
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                For Each row As DataRow In ds.Tables(0).Rows
                    If row("transSuccess") = 0 And IsDBNull(row("transSuccessAmount")) And row("refSuccess") = 0 And IsDBNull(row("refSuccessAmount")) And row("transFailed") = 0 And IsDBNull(row("transFailedAmount")) And row("refFailed") = 0 And IsDBNull(row("refFailedAmount")) Then
                        'Hide row as no data was found for specific date (errors must be in the table for current date)
                        e.Row.Visible = False
                    Else
                        If Not IsDBNull(row("transSuccess")) Then If (row("transSuccess")) <> 0 Then e.Row.Cells(_gvLog_TSPos).Text = row("transSuccess")
                        If Not IsDBNull(row("transSuccessAmount")) Then e.Row.Cells(_gvLog_TSAPos).Text = FormatNumber(row("transSuccessAmount"), 2)
                        If Not IsDBNull(row("refSuccess")) Then If (row("refSuccess")) <> 0 Then e.Row.Cells(_gvLog_RSPos).Text = row("refSuccess")
                        If Not IsDBNull(row("refSuccessAmount")) Then e.Row.Cells(_gvLog_RSAPos).Text = FormatNumber(row("refSuccessAmount"), 2)
                        If Not IsDBNull(row("transFailed")) Then If (row("transFailed")) <> 0 Then e.Row.Cells(_gvLog_TFPos).Text = row("transFailed")
                        If Not IsDBNull(row("transFailedAmount")) Then e.Row.Cells(_gvLog_TFAPos).Text = FormatNumber(row("transFailedAmount"), 2)
                        If Not IsDBNull(row("refFailed")) Then If (row("refFailed")) <> 0 Then e.Row.Cells(_gvLog_RFPos).Text = row("refFailed")
                        If Not IsDBNull(row("refFailedAmount")) Then e.Row.Cells(_gvLog_RFAPos).Text = FormatNumber(row("refFailedAmount"), 2)
                    End If
                Next

            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Sub gvLog_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As GridViewRow In gvLog.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If row.Cells(_gvLog_TSPos).Text <> "" Then _TSTot = _TSTot + row.Cells(_gvLog_TSPos).Text
                If row.Cells(_gvLog_TSAPos).Text <> "" Then _TSATot = _TSATot + row.Cells(_gvLog_TSAPos).Text
                If row.Cells(_gvLog_RSPos).Text <> "" Then _RSTot = _RSTot + row.Cells(_gvLog_RSPos).Text
                If row.Cells(_gvLog_RSAPos).Text <> "" Then _RSATot = _RSATot + row.Cells(_gvLog_RSAPos).Text
                If row.Cells(_gvLog_TFPos).Text <> "" Then _TFTot = _TFTot + row.Cells(_gvLog_TFPos).Text
                If row.Cells(_gvLog_TFAPos).Text <> "" Then _TFATot = _TFATot + row.Cells(_gvLog_TFAPos).Text
                If row.Cells(_gvLog_RFPos).Text <> "" Then _RFTot = _RFTot + row.Cells(_gvLog_RFPos).Text
                If row.Cells(_gvLog_RFAPos).Text <> "" Then _RFATot = _RFATot + row.Cells(_gvLog_RFAPos).Text
            End If
        Next
        If _TSTot <> 0 Then gvLog.FooterRow.Cells(_gvLog_TSPos).Text = _TSTot
        If _TSATot <> 0 Then gvLog.FooterRow.Cells(_gvLog_TSAPos).Text = _TSATot
        If _RSTot <> 0 Then gvLog.FooterRow.Cells(_gvLog_RSPos).Text = _RSTot
        If _RSATot <> 0 Then gvLog.FooterRow.Cells(_gvLog_RSAPos).Text = _RSATot
        If _TFTot <> 0 Then gvLog.FooterRow.Cells(_gvLog_TFPos).Text = _TFTot
        If _TFATot <> 0 Then gvLog.FooterRow.Cells(_gvLog_TFAPos).Text = _TFATot
        If _RFTot <> 0 Then gvLog.FooterRow.Cells(_gvLog_RFPos).Text = _RFTot
        If _RFATot <> 0 Then gvLog.FooterRow.Cells(_gvLog_RFAPos).Text = _RFATot
    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvLog.SelectedIndex = -1
        gvDay.Visible = False
    End Sub
    Protected Sub gvDay_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'If the row is a refund, then show it in red ink
        For Each row As GridViewRow In gvDay.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If LCase(Left(row.Cells(_gvDay_vendorTxCodePos).Text, 5)) = "ebref" Then row.ForeColor = Drawing.Color.Red
            End If
        Next
    End Sub
End Class
