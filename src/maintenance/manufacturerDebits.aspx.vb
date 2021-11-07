Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_manufacturerDebits
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _qtyPos As Integer = 4

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            If drpMan.SelectedValue = "0" Then
                gvComponents.Visible = False
            Else
                gvComponents.Visible = True
            End If
        End If
    End Sub
    Protected Sub drpMan_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblError.Text = ""
        gvComponents.EmptyDataText = "No components found for <font color='gray'>" & Convert.ToString(drpMan.SelectedItem) & "</font>"
    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblError.Text = ""
    End Sub
    Protected Function showDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then
            result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        End If
        Return result
    End Function
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox
        Dim lbl As Label
        Dim bDataExists As Boolean = False
        Dim iDebitID As Integer
        'Loop through all checkboxes to make sure user has selected at least one
        For Each row As GridViewRow In gvComponents.Rows
            If row.RowType = DataControlRowType.DataRow Then
                chk = row.FindControl("chkAdd")
                If chk.Checked Then bDataExists = True
            End If
        Next
        If bDataExists Then
            'Create new debit note
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procDebitInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@manID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@debitID", SqlDbType.Int))
                .Parameters("@manID").Value = Convert.ToInt32(drpMan.SelectedValue)
                .Parameters("@debitID").Direction = ParameterDirection.Output
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
                iDebitID = oCmd.Parameters("@debitID").Value
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            Finally
                oCmd.Dispose()
            End Try
            For Each row As GridViewRow In gvComponents.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    chk = row.FindControl("chkAdd")
                    If chk.Checked Then
                        'Add item to debitNote
                        lbl = row.FindControl("lblCompID")
                        addItemToDebit(oConn, iDebitID, gvComponents.DataKeys(row.RowIndex).Value, Convert.ToInt32(row.Cells(_qtyPos).Text), Convert.ToInt32(lbl.Text))
                    End If
                End If
            Next
            'Clear up
            oConn.Dispose()
            lblError.Text = "Debit Note #" & iDebitID & " created."
            drpMan.SelectedIndex = 0
            gvComponents.DataBind()
        End If
    End Sub
    Protected Sub addItemToDebit(ByRef oConn As SqlConnection, ByVal debitID As Integer, ByVal compHistoryID As Integer, ByVal qty As Integer, ByVal compID As Integer)
        Dim oCmd As New SqlCommand("procDebitItemImsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@debitID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@componentID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@compHistoryID", SqlDbType.Int))
            .Parameters("@debitID").Value = debitID
            .Parameters("@componentID").Value = compID
            .Parameters("@qty").Value = qty
            .Parameters("@compHistoryID").Value = compHistoryID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
        End Try
    End Sub
End Class
