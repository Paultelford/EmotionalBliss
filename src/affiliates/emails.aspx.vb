Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_emails
    Inherits BasePage
    Private _maxMessageLength As Integer = 4000

    Protected Sub btnAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        fvEmail.DefaultMode = FormViewMode.Insert
        Dim txtType As TextBox = fvEmail.FindControl("txtEmail")
        Dim txtSubject As TextBox = fvEmail.FindControl("txtSubject")
        Dim txtBody As TextBox = fvEmail.FindControl("txtBody")
        Dim btnInsert As Button = fvEmail.FindControl("btnInsert")
        Dim btnUpdate As Button = fvEmail.FindControl("btnUpdate")
        txtType.Text = ""
        txtSubject.Text = ""
        txtBody.text = ""
        btnInsert.Visible = False
        btnUpdate.Visible = False
        drpEmails.SelectedIndex = 0
    End Sub
    Protected Sub btnInsert_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailMasterInsert", oConn)
        Dim txtSubject As TextBox = fvEmail.FindControl("txtSubject")
        Dim masterID As Integer = 0
        Dim txtBody As TextBox = fvEmail.FindControl("txtBody")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@emailType", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@subject", SqlDbType.VarChar, 100))
            .Parameters.Add(New SqlParameter("@masterID", SqlDbType.Int))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@emailtype").Value = drpEmails.SelectedItem.Text
            .Parameters("@subject").Value = txtSubject.Text
            .Parameters("@masterID").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            masterID = oCmd.Parameters("@masterID").Value
        Catch ex As Exception
            lblErrorInsert.Text = "An error occured while adding the new email. Please try later<br>"
            Dim si As New siteInclude
            si.AddError("affiliates/emails.aspx", "btnInsert_click(); " & ex.toString())
            si = Nothing
        End Try
        'Now add the email body to the emailMessage table
        prepareMessage(masterID, txtBody.Text, 1)
        txtSubject.Text = ""
        txtBody.text = ""
        drpEmails.items.clear()
        drpEmails.items.add(New listItem("Select...", "0"))
        drpEmails.DataBind()
        fvEmail.DataBind()
    End Sub
    Protected Sub prepareMessage(ByVal masterID As Integer, ByVal body As String, ByVal priority As Integer)
        If Len(body) > _maxMessageLength Then
            'store 1st 4000 characters then run again
            storeMessage(masterID, left(body, _maxMessageLength), priority)
            prepareMessage(masterID, right(body, len(body) - _maxMessageLength), priority + 1)
        Else
            'store the lot in 1 go
            storeMessage(masterID, body, priority)
        End If
    End Sub
    Protected Sub storeMessage(ByVal masterID As Integer, ByVal body As String, ByVal priority As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailMessageInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@emailMasterID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@order", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@text", SqlDbType.VarChar, _maxMessageLength))
            .Parameters("@emailMasterID").Value = masterID
            .Parameters("@order").Value = priority
            .Parameters("@text").Value = body
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblErrorInsert.Text = "An error occured while adding the new email. Please try later<br>"
            Dim si As New siteInclude
            si.AddError("affiliates/emails.aspx", "storeMessage(masterID=" & masterID & ", order=" & priority & "bodyLength=" & len(body) & "); " & ex.toString())
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailMasterByIDUpdate", oConn)
        Dim txtType As TextBox = fvEmail.FindControl("txtEmail")
        Dim txtSubject As TextBox = fvEmail.FindControl("txtSubject")
        Dim txtBody As TextBox = fvEmail.FindControl("txtBody")
        Dim bError As Boolean = False
        deleteMessages(drpEmails.SelectedValue)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@emailMasterID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@emailType", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@emailSubject", SqlDbType.VarChar, 100))
            .Parameters("@emailMasterID").Value = drpEmails.SelectedValue
            .Parameters("@emailType").Value = txtType.Text
            .Parameters("@emailSubject").Value = txtSubject.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblErrorInsert.Text = "An error occured while updating email. Please try later<br>"
            Dim si As New siteInclude
            si.AddError("affiliates/emails.aspx", "storeMessage(masterID=" & drpEmails.SelectedValue & "); " & ex.toString())
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            prepareMessage(drpEmails.SelectedValue, txtBody.Text, 1)
            'Clear up
            drpEmails.items.clear()
            drpEmails.items.add(New listItem("Select...", "0"))
            drpEmails.DataBind()
            txtType.Text = ""
            txtSubject.Text = ""
            txtBody.Text = ""
        End If
    End Sub
    Protected Sub drpEmails_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtType As TextBox = fvEmail.FindControl("txtEmail")
        Dim txtSubject As TextBox = fvEmail.FindControl("txtSubject")
        Dim txtBody As TextBox = fvEmail.FindControl("txtBody")
        Dim btnInsert As Button = fvEmail.FindControl("btnInsert")
        Dim btnUpdate As Button = fvEmail.FindControl("btnUpdate")
        txtBody.Text = ""
        If drpEmails.SelectedValue <> "0" Then
            If drpEmails.SelectedValue <> "" Then
                'Populate fields
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procEmailMasterByIDSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                Dim bError As Boolean = False
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@emailMasterID", SqlDbType.Int))
                    .Parameters("@emailMasterID").Value = drpEmails.SelectedValue
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        txtType.Text = ds.Tables(0).Rows(0)("emailType")
                        txtSubject.Text = ds.Tables(0).Rows(0)("emailSubject")
                        showBody(ds.Tables(0).Rows(0)("emailMasterID"), txtBody)
                    End If
                Catch ex As Exception
                    lblErrorInsert.Text = "An error occured while retreiving email. Please try later<br>"
                    Dim si As New siteInclude
                    si.AddError("affiliates/emails.aspx", "drpEmails_selectedIndexChanged(); " & ex.toString())
                    si = Nothing
                    bError = True
                Finally
                    da.Dispose()
                    ds.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
                If Not bError Then
                    btnInsert.Visible = False
                    btnUpdate.Visible = True
                End If
            Else
                'Doesnt exist in database for this country, show Insert button
                btnInsert.Visible = True
                btnUpdate.Visible = False
            End If
        Else
            'Just clear all data from form
            txtType.Text = ""
            txtSubject.Text = ""
            txtBody.text = ""
            btnInsert.Visible = False
            btnUpdate.Visible = False
        End If
    End Sub
    Protected Sub showBody(ByVal id As Integer, ByRef txt As textbox)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailMessageByMasterIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@emailMasterID", SqlDbType.Int))
            .Parameters("@emailMasterID").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    txt.Text = txt.Text & row("text")
                Next
            End If
        Catch ex As Exception
            lblErrorInsert.Text = "An error occured while retreiving email message. Please try later<br>"
            Dim si As New siteInclude
            si.AddError("affiliates/emails.aspx", "showBody(masterID=" & id & "); " & ex.toString())
            si = Nothing
        Finally
            da.Dispose()
            ds.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub deleteMessages(ByVal masterID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailMessageByMasterIDDelete", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@emailMasterID", SqlDbType.Int))
            .Parameters("@emailMasterID").Value = masterID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblErrorInsert.Text = "An error occured while retreiving email message. Please try later<br>"
            Dim si As New siteInclude
            si.AddError("affiliates/emails.aspx", "deleteMessages(masterID=" & masterID & "); " & ex.toString())
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
End Class

