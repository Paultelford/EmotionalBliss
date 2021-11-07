Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_quarantineProcess
    Inherits System.Web.UI.Page
    Private Const _batchIDPos = 2
    Private Const _orderIDPos = 3
    Private Const _compIDPos = 7
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim bErrorFound As Boolean = False
        'Check for invalid entries
        Dim txtPassed As TextBox = dvComponent.Rows(4).Cells(1).FindControl("txtPassed")
        Dim txtFailed As TextBox = dvComponent.Rows(5).Cells(1).FindControl("txtFailed")
        If txtPassed.Text = "" Then txtPassed.Text = "0"
        If txtFailed.Text = "" Then txtFailed.Text = "0"
        If Not (txtFailed.Text = "0" And txtPassed.Text = "0") Then
            'Check quantities
            If (Not IsNumeric(txtPassed.Text)) Or (Not IsNumeric(txtFailed.Text)) Then
                'Error, none numberic data has been entered
                bErrorFound = True
                lblError.Text = "Invalid quantity entered."
            Else
                'Do the math !!
                Dim iTotal As Integer = Convert.ToInt32(txtPassed.Text) + Convert.ToInt32(txtFailed.Text)
                Dim iQtyInQuarantine As Integer = dvComponent.Rows(1).Cells(1).Text
                If iTotal <> iQtyInQuarantine Then
                    bErrorFound = True
                    lblError.Text = "You must enter a combined total of " & iQtyInQuarantine & " products."
                End If
            End If
        End If
        If Not bErrorFound Then
            'Process (Set current batch entry to 'Processed=True' and add a new entry for Passed/Failed items)
            processComponentBatchID(Request.QueryString("id"))
            'addNewBatchEntry(getBatchID, Convert.ToInt32(txtPassed.Text), Convert.ToInt32(txtFailed.Text))
            siteInclude.addToComponentHistory(getDVData(dvComponent, _batchIDPos), 0, Convert.ToInt32(txtPassed.Text) + Convert.ToInt32(txtFailed.Text), Convert.ToInt32(txtPassed.Text), 0, 0, 0, Convert.ToInt32(txtFailed.Text), 0, 2, getDVData(dvComponent, _orderIDPos), getDVLabel(dvComponent, _compIDPos, "lblComponentID"), "", "no user", False)
            lblError.Text = ""
            Server.Transfer("quarantine.aspx")
        End If
    End Sub
    Protected Sub processComponentBatchID(ByVal batchid As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentBatchByIDProcessedUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@batchid", SqlDbType.Int))
            .Parameters("@batchid").Value = batchid
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
    End Sub
    Protected Function getDVData(ByRef dv As DetailsView, ByVal pos As Integer) As Integer
        Return dv.Rows(pos).Cells(1).Text
    End Function
    Protected Function getDVLabel(ByRef dv As DetailsView, ByVal pos As Integer, ByVal ctrl As String) As Integer
        Dim c As Label = dv.Rows(pos).Cells(1).FindControl(ctrl)
        Return Convert.ToInt32(c.Text)
    End Function
End Class
