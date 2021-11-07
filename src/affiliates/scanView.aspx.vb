Imports System.data
Imports System.data.SqlClient

Partial Class affiliates_scanView
    Inherits BasePage
    Private Const _gvScans_errorMsgSpacerPos As Integer = 5
    Private Const _gvScans_errorMsgPos As Integer = 6

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        gvScans.DataBind()
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Select Case LCase(drpType.SelectedValue)
            Case "all"
                gvScans.Columns(_gvScans_errorMsgPos).Visible = False
                gvScans.Columns(_gvScans_errorMsgSpacerPos).Visible = False
            Case "failed"
                gvScans.Columns(_gvScans_errorMsgPos).Visible = True
                gvScans.Columns(_gvScans_errorMsgSpacerPos).Visible = True
        End Select
    End Sub
    Protected Sub lnkOrderID_click(ByVal sender As Object, ByVal e As EventArgs)
        'Hide gridview and show the details view for the selected scan.
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Dim scanID As Integer = CInt(lnk.CommandArgument)
        Dim orderID As Integer = 0
        Dim scanDate As String = ""
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procScanByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@scanOrderID", SqlDbType.Int))
            .Parameters("@scanOrderID").Value = scanID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblOrderID.Text = ds.Tables(0).Rows(0)("userOrderID")
                lblScanDate.Text = FormatDateTime(ds.Tables(0).Rows(0)("scanDate"), DateFormat.LongDate)
                gvFunctions.DataSource = ds
                gvFunctions.DataBind()
                gvScans.Visible = False
                drpType.Visible = False
                panDetails.Visible = True
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
            Dim si As New siteInclude
            si.addError("affiliates/scanView.aspx.vb", "lnkOrderID_click(scanID=" & scanID & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub lnkBack_click(ByVal sender As Object, ByVal e As EventArgs)
        'Revert to original page view
        gvScans.Visible = True
        drpType.Visible = True
        panDetails.Visible = False
        lblError.Text = ""
    End Sub
    Protected Sub gv_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvScans.PageIndexChanging
        Dim gv As GridView = CType(sender, GridView)
        gv.PageIndex = e.NewPageIndex
        gv.DataBind()
    End Sub
    Protected Sub gv_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvScans.DataBound
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerData(gv)
    End Sub
    Protected Sub gv_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvScans.DataBinding
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerControl(gv)
    End Sub
End Class
