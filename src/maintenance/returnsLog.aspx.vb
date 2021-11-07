Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_returnsLog
    Inherits System.Web.UI.Page
    Private Const _gvReturns_returnsIDPos As Integer = 0
    Private Const _gvReturns_orderDatePos As Integer = 10

    Protected Sub drpDate_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub drpOrder_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub gvReturns_dataBound(ByVal ssender As Object, ByVal e As EventArgs)
        Dim lblReturnsID As Label
        Dim lnk As HyperLink
        Dim lnkStatus As HyperLink
        Dim lblCountryCode As Label
        For Each row As GridViewRow In gvReturns.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'Create link to EB returns (link to OrderView.aspx page)
                lblReturnsID = row.FindControl("lblReturnsID")
                lblCountryCode = row.FindControl("lblCountryCode")
                'Create Status link. This is used to 'Proceed' to the next stage of the return.
                lnkStatus = row.FindControl("lnkStatus")
                If Not (lnkStatus.Text = "" Or LCase(lnkStatus.Text = "complete")) Then
                    'Only update link if status<>Complete
                    If LCase(lnkStatus.Text) = "complete" Then
                        lnkStatus.NavigateUrl = "returns" & lnkStatus.Text & ".aspx?id=" & row.Cells(_gvReturns_returnsIDPos).Text & "&distCountryCode=" & lblCountryCode.Text
                    Else
                        lnkStatus.NavigateUrl = "returns" & lnkStatus.Text & ".aspx?id=" & lblReturnsID.Text & "&distCountryCode=" & lblCountryCode.Text
                    End If

                End If
            End If
        Next
        'Set the header to the date column in the Gridview, depending on the 'Order By' date the user has selected
        If gvReturns.Rows.Count > 0 Then gvReturns.HeaderRow.Cells(_gvReturns_orderDatePos).Text = drpOrder.SelectedItem.Text
    End Sub
    Protected Sub btnQuickSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'User enters a returns number, this sub looks up the Status then redirects to the correct page in order to continue with the return.
        Page.Validate("quick")
        If Page.IsValid Then
            If IsNumeric(txtQuick.Text) Then
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procReturnsByIDSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                Dim returnsID As Integer = CType(txtQuick.Text, Integer)
                Dim target As String = ""
                Dim bError As Boolean = False
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@returnsID", SqlDbType.Int))
                    .Parameters("@returnsID").Value = txtQuick.Text
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        lblQuickError.Text = ""
                        target = "returns" & ds.Tables(0).Rows(0)("status") & ".aspx?id=" & returnsID
                    Else
                        bError = True
                        lblQuickError.Text = "Return ID does not exist."
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
                If Not bError Then Response.Redirect(target)
            Else
                ran1.IsValid = False
            End If 'isNumeric
        Else
            lblQuickError.Text = ""
        End If
    End Sub

End Class
