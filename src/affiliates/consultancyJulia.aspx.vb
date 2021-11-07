Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_consultancyJulia
    Inherits BasePage
    Private Const juliaTextColor As String = "#ff3333"
    Private Const userTextColor As String = "#3366ff"
    Private Const _affPercentage As Decimal = 80

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        If Not Roles.IsUserInRole("Consultancy") Then Response.Redirect("default.aspx")
        If Session("EBAffID") = "" Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub gvUsers_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim rowIndex As Integer = gvUsers.SelectedIndex
        lblInfoText.Visible = True
        'Set all row backgrounds to white, then hilight selected item
        For Each row As GridViewRow In gvUsers.Rows
            row.BackColor = Drawing.Color.White
        Next
        gvUsers.Rows(rowIndex).BackColor = Drawing.Color.LightBlue
        'Show users profile/info in theinfo panel
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procConsultancyByCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 10))
            .Parameters("@code").Value = gvUsers.DataKeys(rowIndex).Value
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                fvInfo.DataSource = ds
                fvInfo.DataBind()
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
        'Show message form
        Dim lblRepliesLeft As Label = fvInfo.FindControl("lblRepliesLeft")
        If CStr(lblRepliesLeft.Text) = "0" Then
            btnSendMessage.Visible = False
            txtMessage.Visible = False
            lblMessageInstructions.Visible = False
        Else
            panForm.Visible = True
            lblMessageInstructions.Visible = True
            btnSendMessage.Visible = True
            txtMessage.Visible = True
        End If
    End Sub
    Protected Function showStatus(ByVal userWaiting As Object)
        Dim result As String = "Unknown"
        If Not IsDBNull(userWaiting) Then
            If CBool(userWaiting.ToString) Then
                result = "Waiting on your response"
            Else
                result = "Waiting for user"
            End If
            Dim lblRepliesLeft As Label = fvInfo.FindControl("lblRepliesLeft")
            If CStr(lblRepliesLeft.Text) = "0" Then
                result = "Complete"
            End If
        End If
        Return result
    End Function
    Protected Function showAge(ByVal age As Object)
        Dim result As String = "Unknown"
        If Not IsDBNull(age) Then
            If CInt(age.ToString) > 0 Then result = age.ToString
        End If
        Return result
    End Function
    Protected Function showName(ByVal n As Object)
        Dim result As String = "Anon"
        If Not IsDBNull(n) Then result = n.ToString
        Return result
    End Function
    Protected Sub gvHistory_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim logID As Integer = 0
        Dim lblDate As Label
        Dim lblUserSent As Label
        Dim trHeader As HtmlTableRow
        Dim lblMsgHeader As Label
        Dim lblMessage As Label
        Dim messageCreater As String = ""
        For Each row As GridViewRow In gvHistory.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblDate = row.FindControl("lblDate")
                lblUserSent = row.FindControl("lblUserSent")
                trHeader = row.FindControl("trHeader")
                lblMsgHeader = row.FindControl("lblMsgHeader")
                lblMessage = row.FindControl("lblMessage")
                If CBool(lblUserSent.Text) Then
                    'set font color
                    Dim lblName As Label = fvInfo.FindControl("lblName")
                    messageCreater = lblName.text
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml(userTextColor)
                Else
                    messageCreater = "You"
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml(juliaTextColor)
                End If
                If logID = gvHistory.DataKeys(row.RowIndex).Value Then
                    'Same message. Hide the headder. 
                    trHeader.Visible = False
                End If
                lblMsgHeader.Text = messageCreater & " wrote on " & FormatDateTime(lblDate.Text, DateFormat.ShortDate) & " " & FormatDateTime(lblDate.Text, DateFormat.LongTime)
                logID = gvHistory.DataKeys(row.RowIndex).Value
            End If
        Next
    End Sub
    Protected Sub btnSendMessage_click(ByVal sender As Object, ByVal e As EventArgs)
        'User has tried to send message, ask for confirmation before sending
        btnSendMessage.Visible = False
        lblConfirmText.Visible = True
        tdConfirm.Visible = True
    End Sub
    Protected Sub messageSend_click(ByVal sender As Object, ByVal e As EventArgs)
        'Create variabel called replyQty which contains the number of replys to remove from the users profile. Its value depends on which button was click.
        Dim btn As Button = CType(sender, Button)
        Dim iFirstReplyOrderID As Integer = getFirstReplyOrderID()
        'siteInclude.debug("FirstReplyOrderID=" & iFirstReplyOrderID)
        Dim replyQty As Integer = 0
        If CBool(btn.CommandArgument) Then replyQty = 1
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procConsultancyLogInsert", oConn)
        Dim logID As Integer
        Dim bError As Boolean = False
        Dim hidCode As HiddenField = fvInfo.FindControl("hidCode")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@replyQty", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar))
            .Parameters.Add(New SqlParameter("@userSent", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@logID", SqlDbType.Int))
            .Parameters("@code").Value = hidCode.Value
            .Parameters("@replyQty").Value = replyQty
            .Parameters("@status").Value = "Julia is waiting for your message"
            .Parameters("@userSent").Value = 0
            .Parameters("@logID").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            logID = CInt(oCmd.Parameters("@logID").Value)
            If iFirstReplyOrderID > 0 Then addConsultancyFeeToAffStatement(iFirstReplyOrderID)
        Catch ex As Exception
            bError = True
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Now store message in consultancyMessage table
        If Not bError Then
            Dim lblRepliesLeft As Label = fvInfo.FindControl("lblRepliesLeft")
            lblRepliesLeft.Text = CStr(CInt(lblRepliesLeft.Text) - replyQty)
            storeMessage(logID)

            If CStr(lblRepliesLeft.Text) = "0" Then
                btnSendMessage.Visible = False
                txtMessage.Visible = False
                lblMessageInstructions.Visible = False
                Response.Write("Setting to complete")
                setComplete()
                gvUsers.DataBind()
            Else
                gvUsers.DataBind()
            End If
        End If
    End Sub
    Protected Sub storeMessage(ByVal logID As Integer)
        'Split message up into chunks of 4000 characters and store in consultancyMessage table along with ID.
        'Just store text for now, split code to be added tomoz
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procConsultancyMessageInsert", oConn)
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@logID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@order", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@message", SqlDbType.VarChar, 4000))
            .Parameters("@logID").Value = logID
            .Parameters("@order").Value = 1
            .Parameters("@message").Value = txtMessage.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblMsgSendError.Text = "<font color='red'>Internal error: Your message could not be sent.<br>Please try again later, we are sorry for any inconvenience.</font>" & ex.ToString
            Try
                Dim si As New siteInclude
                si.addError("affiliates/consultancy.aspx", "storeMessage:: " & ex.ToString)
                si = Nothing
            Catch ex2 As Exception
            End Try
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            Dim lblStatus As Label = fvInfo.FindControl("lblStatus")
            lblStatus.Text = "Waiting for user"
            txtMessage.Text = ""
            tdConfirm.Visible = False
            lblConfirmText.Visible = False
            btnSendMessage.Visible = True
            gvHistory.DataBind()
        End If
    End Sub
    Protected Sub lnkShowPersonal_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim tblPersonal As HtmlTable = fvInfo.FindControl("tblPersonal")
        Dim tblPersonalExtra As HtmlTable = fvInfo.FindControl("tblPersonalExtra")
        tblPersonal.Visible = Not tblPersonal.Visible
        tblPersonalExtra.Visible = Not tblPersonal.Visible
    End Sub
    Protected Sub fvInfo_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'If the Physical/Mental issue or Disibility boxes were not filled in, then display the word 'None' as default under these sections.
        'It makes the formatting look nicer :)
        Dim lblDisability As Label = fvInfo.FindControl("lblDisability")
        Dim lblPDisability As Label = fvInfo.FindControl("lblPDisability")
        Dim lblMental As Label = fvInfo.FindControl("lblMental")
        Dim lblPMental As Label = fvInfo.FindControl("lblPMental")
        If lblDisability.Text = "" Then lblDisability.Text = "None"
        If lblPDisability.Text = "" Then lblPDisability.Text = "None"
        If lblMental.Text = "" Then lblMental.Text = "None"
        If lblPMental.Text = "" Then lblPMental.Text = "None"
        Dim lblRepliesLeft As Label = fvInfo.FindControl("lblRepliesLeft")
        If CStr(lblRepliesLeft.Text) = "0" Then
            panForm.Visible = False
            lblMessageInstructions.Visible = False
            btnSendMessage.Visible = False
        End If
    End Sub
    Protected Sub setComplete()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procConsultancyByCodeStatusUpdate", oConn)
        Dim hidCode As HiddenField = fvInfo.FindControl("hidCode")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 50))
            .Parameters("@code").Value = hidCode.Value
            .Parameters("@status").Value = "Complete"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            siteInclude.addError("affiliates/consultancyJulia.aspx,vb", "setComplete(code=" & gvUsers.SelectedDataKey.Value & "); " & ex.ToString)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Dim lblStatus As Label = fvInfo.FindControl("lblStatus")
        lblStatus.Text = "Complete"
    End Sub
    Protected Sub addConsultancyFeeToAffStatement(ByVal orderID As Integer)
        'Get the items in the order
        'siteInclude.debug("Starting addConsultancyFeeToAffStatament")
        Dim dt As New DataTable
        Dim dVoucherTotal As Decimal = 0
        Try
            Dim param() As String = {"@orderID"}
            Dim paramValue() As String = {orderID.ToString}
            Dim paramType() As SqlDbType = {SqlDbType.Int}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procShopOrderItemsByOrderIDSelect")
            For Each row As DataRow In dt.Rows
                If Left(LCase(row("saleProdCode")), 2) = "vc" Then dVoucherTotal = dVoucherTotal + CDec(row("saleUnitPrice"))
            Next
            'siteInclude.debug("dVoucherTotal=" & dVoucherTotal)
            If dVoucherTotal > 0 Then
                'Everything ok so far, this is Julia's 1st reply, the amount the customer paid has been retrieved
                'Aff currently gets 80% of the voucher unit price.
                Dim dAffEarning As Decimal = (dVoucherTotal / 100) * _affPercentage
                'siteInclude.debug("AffEarning=" & dAffEarning)
                addToStatement(Session("EBAffID"), dAffEarning, orderID)
            End If
        Catch ex As Exception
            siteInclude.addError("consultancyJulia.aspx.vb", "addConsultancyFeeToAffStatement(orderID=" & orderID & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
    End Sub
    Protected Sub addToStatement(ByVal affID As Integer, ByVal credit As Decimal, ByVal orderID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateStatementInsert", oConn)
        'siteInclude.debug("addToStatement(" & affID & ", " & credit & ", " & orderID & ") started")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@statementCredit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@statementDebit", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@extOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@linkedPrefix", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@cheque", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@reason", SqlDbType.VarChar, 2000))
            .Parameters.Add(New SqlParameter("@transDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
            .Parameters("@affID").Value = affID
            .Parameters("@statementCredit").Value = credit
            .Parameters("@statementDebit").Value = 0
            .Parameters("@orderID").Value = orderID
            .Parameters("@extOrderID").Value = 0
            .Parameters("@actionID").Value = 15 '15 is 'Consultancy Credit'
            .Parameters("@linkedPrefix").Value = ""
            .Parameters("@cheque").Value = ""
            .Parameters("@reason").Value = ""
            .Parameters("@transDate").Value = Now()
            .Parameters("@pk").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            siteInclude.addError("consultancyJulia.aspx.vb", "addToStatement(affID=" & affID & ", credit=" & credit & ", orderID=" & orderID & "); " & ex.ToString)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function getFirstReplyOrderID() As Integer
        Dim result As Integer = 0
        Dim dt As New DataTable
        Dim hidCode As HiddenField = fvInfo.FindControl("hidCode")
        Try
            Dim param() As String = {"@code"}
            Dim paramValue() As String = {hidCode.Value}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar}
            Dim paramSize() As Integer = {10}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procConsultancyByCodeNotStartedSelect")
            If dt.Rows.Count = 1 Then result = CInt(dt.Rows(0)("orderID"))
        Catch ex As Exception
            siteInclude.addError("consultancyJulia.aspx.vb", "isFirstReply(code=" & gvUsers.SelectedValue & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
        Return result
    End Function
End Class
