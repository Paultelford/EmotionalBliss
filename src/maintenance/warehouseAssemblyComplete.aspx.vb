Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class maintenance_warehouseAssemblyComplete
    Inherits System.Web.UI.Page
    Private Const _qtyPos As Integer = 4


    Protected Sub gvBatches_selectedInxedChanging(ByVal sender As Object, ByVal e As GridViewSelectEventArgs)
        Dim index As Integer = e.NewSelectedIndex
        For Each row As GridViewRow In gvBatches.Rows
            row.BackColor = Drawing.Color.White
        Next
        gvBatches.Rows(index).BackColor = Drawing.Color.LightGray
        pan1.Visible = True
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Check for valid input
        Dim userTotal As Integer = 0
        Dim bError As Boolean = False
        Dim batchTotal As Integer = Convert.ToInt32(gvBatches.Rows(gvBatches.SelectedIndex).Cells(_qtyPos).Text)
        If txtPass.Text = "" Then txtPass.Text = "0"
        If txtFail.Text = "" Then txtFail.Text = "0"
        If IsNumeric(txtPass.Text) And IsNumeric(txtFail.Text) Then
            userTotal = Convert.ToInt16(txtPass.Text) + Convert.ToInt32(txtFail.Text)
            If userTotal <> batchTotal Then
                bError = True
                lblError.Text = "<font color='red'>Quantities entered do not match the batch total.</font>"
            End If
        Else
            'non-numerical gubbins entered
            lblError.Text = "<font color='red'>Non numerical data entered.</font>"
            bError = True
        End If
        If Not bError Then
            'All ok, process
            'Update warehouseBatch table
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("warehouseBatchByIDUpdate", oConn)
            Dim warehouseID As Integer = Convert.ToInt32(gvBatches.DataKeys(gvBatches.SelectedIndex).Value)
            Dim lblProductID As Label = gvBatches.Rows(gvBatches.SelectedIndex).FindControl("lblProductID")
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@warehouseID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@pass", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@fail", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 25))
                .Parameters("@warehouseID").Value = warehouseID
                .Parameters("@pass").Value = Convert.ToInt32(txtPass.Text)
                .Parameters("@fail").Value = Convert.ToInt32(txtFail.Text)
                .Parameters("@username").Value = Membership.GetUser.UserName
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            'Add details to productHistory
            Dim eb As New siteInclude
            removeProductsFromStock(eb, Convert.ToInt32(lblProductID.Text), warehouseID, Convert.ToInt32(txtPass.Text))
            'Add details to warehouseHistory
            eb.addToWarehouseHistory(Convert.ToInt32(lblProductID.Text), Convert.ToInt32(txtPass.Text), 0, 1, "", Membership.GetUser.UserName, 0, 0, Convert.ToInt32(txtPass.Text), warehouseID)
            eb = Nothing
            lblError.Text = ""
            pan1.Visible = False
            gvBatches.DataBind()
        End If
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
    Protected Sub removeProductsFromStock(ByRef eb As siteInclude, ByVal warehouseProductID As Integer, ByVal warehouseID As Integer, ByVal pass As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procWarehouseBoxContentByBProductIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@warehouseProductID", SqlDbType.Int))
            .Parameters("@warehouseProductID").Value = warehouseProductID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each rs As DataRow In ds.Tables(0).Rows
                    If rs("contentIsProduct") Then eb.addToProductHistory(rs("contentProductID"), 0, 0, 0, 0, 2, 0, "Batch complete", Membership.GetUser.UserName, False, warehouseID, 0, Convert.ToInt32(txtPass.Text) * Convert.ToInt32(rs("contentQty")))
                Next
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
End Class

