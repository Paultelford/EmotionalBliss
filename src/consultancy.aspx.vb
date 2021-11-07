Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class consultancy
    Inherits BasePage
    Private Const juliaTextColor As String = "#ff3333"
    Private Const userTextColor As String = "#3366ff"
    Private Const maxLength As Integer = 4000
    Private iAge As Integer
    Private iPAge As Integer
    Private iYear As Integer
    Private iMonth As Integer
    Private iChildren As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Clear errors
        lblLoginError.Text = ""
        lblMsgSendError.Text = ""
        lblTextOverflowError.Text = ""
        If Request.QueryString("code") <> "" Then
            txtCode.Text = Request.QueryString("code")
            Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('focusElement(""" & txtSurname.ClientID & """)',200);", True)
        Else
            Me.ClientScript.RegisterStartupScript(Me.GetType, "onload", "setTimeout('focusElement(""" & txtCode.ClientID & """)',200);", True)
        End If

    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        'Dim lnkSection As HyperLink = Master.FindControl("lnkSection")
        'Dim lnkSubSection As HyperLink = Master.FindControl("lnkSubSection")
        'lnkSection.Text = "Consultancy"
        'lnkSubSection.Text = ""
    End Sub
    Protected Sub btnLogin_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("login")
        If Page.IsValid Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procConsultancyByCodeSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 10))
                .Parameters("@code").Value = txtCode.Text
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    If surnameMatches(ds.Tables(0).Rows(0)("cardname")) Then
                        'All matched up, user is valid.
                        panLogin.Visible = False
                        'If it is users 1st visit, then ask some personal questions rather than showing the main Form
                        If CBool(ds.Tables(0).Rows(0)("firstVisit")) Then
                            '1st visit, show a quesionaire panel (User can enter ersonal data or skip this step. With choice will set firstVisit=0 in the db)
                            panFirstTime.Visible = True
                            fvForm.DataSource = ds
                            fvForm.DataBind()
                        Else
                            'Not 1st visit, show the Form and databind all info
                            panForm.Visible = True
                            fvForm.DataSource = ds
                            fvForm.DataBind()
                            displayMostRecentMessage()
                            panMostRecent.Visible = True
                        End If
                    Else
                        'The surname did not match. Display error
                        lblLoginError.Text = "<font color='red'>The Surname you entered did not match the surname used to buy the Consultancy Voucher.<br>It must match the surname on the Credit/Debit Card used to buy this voucher.</font>"
                    End If
                    Else
                        lblLoginError.Text = "<font color='red'>Consultancy code not found.</font>"
                    End If
            Catch ex As Exception
                lblLoginError.Text = "<font>Consultancy is currently unavailable.<br>We are sorry for the inconvenience. Please try later.</font>"
                Try
                    Dim si As New siteInclude
                    si.addError("consultancy.aspx", "btnLogin_click(voucher number " & txtCode.Text & "); " & ex.ToString)
                    si = Nothing
                Catch ex2 As Exception
                End Try
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Function surnameMatches(ByVal fullName As String) As Boolean
        'Split name up into array (Unless it contains no spaces, in which case assume it is the surname)
        Dim bResult As Boolean = False
        Try
            Dim aName() As String = Split(fullName, " ")
            Dim surname As String
            If UBound(aName) > 0 Then
                'Spaces found
                surname = aName(UBound(aName))
            Else
                'No spaces, assume surname only was entered
                surname = fullName
            End If
            If LCase(surname) = LCase(txtSurname.Text) Then bResult = True
        Catch ex As Exception
            Try
                Dim si As New siteInclude
                si.addError("consultancy.aspx", "surnameMatches(" & fullName & "); " & ex.ToString)
                si = Nothing
            Catch ex2 As Exception
            End Try
        End Try
        Return bResult
    End Function
    Protected Sub drpAge_init(ByVal sender As Object, ByVal e As EventArgs)
        'Bind to partners age control
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.Items.Count = 0 Then
            drp.Items.Clear()
            drp.Items.Add(New ListItem("Rather not say", "0"))
            For iLoop As Integer = 18 To 100
                drp.Items.Add(New ListItem(iLoop, iLoop))
            Next
        End If
    End Sub
    Protected Sub sqlPersonal_inserted(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        'First time Personal data has been update, and firstVisit set to 0
        'Display the main Form
        panFirstTime.Visible = False
        panForm.Visible = True
        panMostRecent.Visible = True
        displayMostRecentMessage()
    End Sub
    Protected Sub btnSendMessage_click(ByVal sender As Object, ByVal e As EventArgs)
        'Check for text being entered. 
        If Len(txtMessage.Text) > 0 Then
            'Add to consultancyLog and return the logID
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim ocmd As New SqlCommand("procConsultancyLogInsert", oConn)
            Dim logID As Integer
            Dim bError As Boolean = False
            With ocmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@replyQty", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar))
                .Parameters.Add(New SqlParameter("@userSent", SqlDbType.Bit))
                .Parameters.Add(New SqlParameter("@logID", SqlDbType.Int))
                .Parameters("@code").Value = txtCode.Text
                .Parameters("@replyQty").Value = 0
                .Parameters("@status").Value = "Awaiting reply from Julia"
                .Parameters("@userSent").Value = 1
                .Parameters("@logID").Direction = ParameterDirection.Output
            End With
            Try
                If ocmd.Connection.State = 0 Then ocmd.Connection.Open()
                ocmd.ExecuteNonQuery()
                logID = ocmd.Parameters("@logID").Value
            Catch ex As Exception
                bError = True
                lblMsgSendError.Text = "<font color='red'>Internal error: Your message could not be sent.<br>Please try again later, we are sorry for any inconvenience.</font>" & ex.ToString
                Try
                    Dim si As New siteInclude
                    si.addError("consultancy.aspx", "btnSendMessageClick::procConsultancyLogInsert; " & ex.ToString)
                    si = Nothing
                Catch ex2 As Exception
                End Try
            Finally
                ocmd.Dispose()
                oConn.Dispose()
            End Try
            'Now store message in consultancyMessage table
            If Not bError Then storeMessage(logID)
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
            .Parameters.Add(New SqlParameter("@message", SqlDbType.VarChar, -1))
            .Parameters("@logID").Value = logID
            .Parameters("@order").Value = 1
            .Parameters("@message").Value = txtMessage.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            siteInclude.debug(Len(txtMessage.Text))
            If Len(txtMessage.Text) > 10000 Then
                bError = True
                lblError.Text = "Maximum charcters exceeded. You have used " & Len(txtMessage.Text) & ", please reduce this to 10,000."
            Else
                oCmd.ExecuteNonQuery()
            End If
        Catch ex As Exception
            bError = True
            lblMsgSendError.Text = "<font color='red'>Internal error: Your message could not be sent.<br>Please try again later, we are sorry for any inconvenience.</font>" & ex.ToString
            Try
                Dim si As New siteInclude
                si.addError("consultancy.aspx", "storeMessage:: " & ex.ToString)
                si = Nothing
            Catch ex2 As Exception
            End Try
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            Dim lblStatus As Label = fvForm.FindControl("lblStatus")
            lblStatus.Text = "Awaiting reply from Julia"
            lblMostRecent.Text = txtMessage.Text
            lblMostRecent.ForeColor = Drawing.Color.Blue
            txtMessage.Text = ""
            gvHistory.DataBind()
        End If
    End Sub
    Protected Sub displayMostRecentMessage()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procConsultancyMessageByOrderSelectLatest", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@code", SqlDbType.VarChar, 10))
            .Parameters("@code").Value = txtCode.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblMostRecent.Text = ""
                For Each row As DataRow In ds.Tables(0).Rows
                    lblMostRecent.Text = lblMostRecent.Text & row("message")
                Next
                If CBool(ds.Tables(0).Rows(0)("userSent")) Then
                    lblMostRecent.ForeColor = System.Drawing.ColorTranslator.FromHtml(userTextColor)
                Else
                    lblMostRecent.ForeColor = System.Drawing.ColorTranslator.FromHtml(juliaTextColor)
                End If
            End If
        Catch ex As Exception
            Try
                Dim si As New siteInclude
                si.addError("consultancy.aspx", "displayMostRecentMessage:: " & ex.ToString)
                si = Nothing
            Catch ex2 As Exception
            End Try
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub lnkShowHistory_click(ByVal sender As Object, ByVal e As EventArgs)
        panHistory.Visible = Not panHistory.Visible
        panMostRecent.Visible = Not panHistory.Visible
    End Sub
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
                    messageCreater = "You"
                    lblMessage.ForeColor = System.Drawing.ColorTranslator.FromHtml(userTextColor)
                Else
                    messageCreater = "Julia"
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
    Protected Sub drpYears_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.Items.Count = 0 Then
            drp.Items.Clear()
            For iLoop As Integer = 0 To 50
                drp.Items.Add(New ListItem(iLoop, iLoop))
            Next
        End If
    End Sub
    Protected Sub drpMonths_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.Items.Count = 0 Then
            drp.Items.Clear()
            For iLoop As Integer = 0 To 12
                drp.Items.Add(New ListItem(iLoop, iLoop))
            Next
        End If
    End Sub
    Protected Sub drpChildren_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.Items.Count = 0 Then
            drp.Items.Clear()
            For iLoop As Integer = 0 To 10
                drp.Items.Add(New ListItem(iLoop, iLoop))
            Next
        End If
    End Sub
    Protected Sub dvPersonal_inserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
        'Make sure data entered is not more than the maximim allowed
        Dim txtDisability As TextBox = dvPersonal.FindControl("txtDisability")
        Dim txtPDisability As TextBox = dvPersonal.FindControl("txtPDisability")
        Dim txtMental As TextBox = dvPersonal.FindControl("txtMental")
        Dim txtPMental As TextBox = dvPersonal.FindControl("txtPMental")
        Dim bError As Boolean = False
        Dim tmp As String = ""
        Dim lbl As Label = dvPersonal.FindControl("lblDisabilityUsed")
        lbl.Text = ""
        If Len(txtDisability.Text) > maxLength Then
            bError = True
            lbl.Text = "<font color='red'>  You have used " & Len(txtDisability.Text) & "</font>"
        End If
        lbl = dvPersonal.FindControl("lblPDisabilityUsed")
        lbl.Text = ""
        If Len(txtPDisability.Text) > maxLength Then
            bError = True
            lbl.Text = "<font color='red'>  You have used " & Len(txtPDisability.Text) & "</font>"
        End If
        lbl = dvPersonal.FindControl("lblMentalUsed")
        lbl.Text = ""
        If Len(txtMental.Text) > maxLength Then
            bError = True
            lbl.Text = "<font color='red'>  You have used " & Len(txtMental.Text) & "</font>"
        End If
        lbl = dvPersonal.FindControl("lblPMentalUsed")
        lbl.Text = ""
        If Len(txtPMental.Text) > maxLength Then
            tmp = txtPMental.Text
            bError = True
            lbl.Text = "<font color='red'>  You have used " & Len(txtPMental.Text) & "</font>"
        End If
        If bError Then lblTextOverflowError.Text = "<font color='red'>You have used too many characters in one of the above input boxes.</font>"
        e.Cancel = bError
    End Sub
    Protected Sub lnkProfile_click(ByVal Sender As Object, ByVal e As EventArgs)
        panHistory.Visible = False
        panForm.Visible = False
        panMostRecent.Visible = False
        lblFirstTimeOnlyText.Visible = False
        panFirstTime.Visible = True
        dvPersonal.ChangeMode(DetailsViewMode.Edit)
    End Sub
    Protected Sub fvForm_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblReplysLeft As Label = fvForm.FindControl("lblReplysLeft")
        If CStr(lblReplysLeft.Text) = "0" Then lblFormInstructions.Visible = False
    End Sub
End Class
