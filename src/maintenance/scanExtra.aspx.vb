Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_scanExtra
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        _login = Master.FindControl("logMaintenance")
        _content = _login.FindControl("ContentPlaceholder1")
        If Not Page.IsPostBack Then
            lblQtyLeft.Text = CStr(CInt(Request.QueryString("qty")) - 1)
            Dim txt As TextBox = _content.FindControl("txtTracker")
            focusOnTracker(txt)
        End If
    End Sub
    Protected Sub txtTracker_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = CType(sender, TextBox)
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
            focusOnTracker(txt)
        End If
    End Sub
    Protected Sub focusOnTracker(ByRef txt As TextBox)
        ScriptManager.RegisterStartupScript(txt, Me.GetType, "onloader", "focusElement('" & txt.UniqueID & "');", True)
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
                Dim si As New siteInclude
                si.AddToOrderLog(id, "Extra tracker scanned: <a href='http://track.dhl.co.uk/tracking/wrd/run/wt_xhistory_pw.execute?PCL_NO=" & tracker & "&PCL_INST=1&COLLDATE=&CNTRY=" & getDistCountryCode(id) & "'>" & tracker & "</a>", Membership.GetUser.UserName, True)
            Catch ex As Exception
                Throw ex
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Catch ex As Exception
            lblError.Text = lblError.Text & "<font color='red'>addTracker() error occured.</font><br>"
            Dim si As New siteInclude
            si.addError("affiliates/scanExtra.aspx.vb", "addTracker(id=" & id & ",tracker=" & tracker & "); " & ex.ToString)
            si = Nothing
        End Try
    End Sub
    Protected Function getDistCountryCode(ByVal id As Integer) As String
        Dim result As String = ""
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByOrderIDCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters("@id").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = ds.Tables(0).Rows(0)("affCountryCode")
        Catch ex As Exception
            lblError.Text = lblError.Text & "<font color='red'>Could not add new tracker to Order Log.</font><br>"
            Throw ex
        End Try
        Return result
    End Function
End Class
