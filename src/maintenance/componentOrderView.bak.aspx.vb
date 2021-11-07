Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentOrderView
    Inherits System.Web.UI.Page
    Protected Function showOutstanding(ByVal qty As Integer, ByVal qtyReceived As Integer) As Integer
        Return qty - qtyReceived
    End Function
    Protected Sub gvComponents_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lbl As Label = e.Row.Cells(2).FindControl("lblOutstanding")
            If Convert.ToInt16(lbl.Text) = 0 Then e.Row.Cells(4).Text = ""
        End If
    End Sub

    Protected Sub gvComponents_editing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvComponents.Rows(e.NewEditIndex).Cells(3).Visible = True
        gvComponents.Columns(3).Visible = True
    End Sub
    Protected Sub gvComponents_rowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        'Error check
        Dim txtReceived As TextBox = gvComponents.Rows(e.RowIndex).Cells(3).FindControl("txtReceived")
        Dim lblOutstanding As Label = gvComponents.Rows(e.RowIndex).Cells(2).FindControl("lblOutstanding")
        If Not (IsNumeric(txtReceived.Text)) Or txtReceived.Text = "" Then
            lblError.Text = "Invalid input in the Received box."
        Else
            If Convert.ToInt16(txtReceived.Text) > Convert.ToInt16(lblOutstanding.Text) Then
                lblError.Text = "Too many items"
            Else
                'clear error text
                lblError.Text = ""
                'Run SQL to update the items received then set the grid back to normal mode
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procComponentOrderItemByIDReceivedUpdate", oConn)
                Dim compOrderItemID As Integer = gvComponents.DataKeys(e.RowIndex).Value
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@compOrderItemID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
                    .Parameters("@compOrderItemID").Value = compOrderItemID
                    .Parameters("@qty").Value = Convert.ToInt16(txtReceived.Text)
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
                    gvComponents.EditIndex = -1
                    gvComponents.Columns(3).Visible = False
                    e.Cancel = True
                End Try
            End If
        End If
        e.Cancel = True
    End Sub
End Class
