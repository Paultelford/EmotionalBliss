Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_scanExtra
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            lblQtyLeft.Text = CStr(CInt(Request.QueryString("qty")) - 1)
        Else
            focusOnTracker()
        End If
    End Sub
    Protected Sub txtTracker_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Add tracker
        addTracker(Request.QueryString("id"), txtTracker.Text)
        lblScanned.Text = lblScanned.Text & txtTracker.Text & " Added to tarcker table.<br>"
        lblQtyLeft.Text = CStr(CInt(lblQtyLeft.Text) - 1)
        'Hide input box when there are no more tarckers to add
        txtTracker.Text = ""
        If lblQtyLeft.Text = "0" Then
            txtTracker.Visible = False
            lblTrackerText.Visible = False
            lnkBack.Visible = True
        Else
            focusOnTracker()
        End If
    End Sub
    Protected Sub focusOnTracker()
        ScriptManager.RegisterStartupScript(txtTracker, Me.GetType, "onloader", "focusElement('" & txtTracker.UniqueID & "');", True)
    End Sub
    Protected Sub addTracker(ByVal id As Integer, ByVal tracker As String)
        Try
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procTrackerInsertExtra", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@tracker", SqlDbType.VarChar, 30))
                .Parameters.Add(New SqlParameter("@emailType", SqlDbType.VarChar, 50))
                .Parameters("@orderID").Value = id
                .Parameters("@tracker").Value = tracker
                .Parameters("@emailType").Value = "extra"
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
                siteInclude.AddToOrderLog(id, "Extra tracker scanned: <a href='" & siteInclude.getTrackerURL(tracker, Session("EBAffEBDistributorCountryCode")) & "'>" & tracker & "</a>", Membership.GetUser.UserName, True)
            Catch ex As Exception
                Throw ex
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Catch ex As Exception
            lblError.Text = lblError.Text & "<font color='red'>addTracker() error occured.</font><br>"
            siteInclude.addError("affiliates/scanExtra.aspx.vb", "addTracker(id=" & id & ",tracker=" & tracker & "); " & ex.ToString)

        End Try
    End Sub
End Class
