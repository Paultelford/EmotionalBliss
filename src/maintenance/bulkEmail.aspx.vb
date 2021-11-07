Imports System.Net.Mail
Imports System.Net.Mail.SmtpClient
Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_bulkEmail
    Inherits System.Web.UI.Page
    Private Const _emailToSend As Integer = 100

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        btnSend.Text = "Send " & _emailToSend & " emails"
        refreshTotals()
    End Sub
    Protected Sub refreshTotals()
        lblTotalEmails.Text = getEmailTotal()
        lblSentEmails.Text = getEmailSent()
        lblLeftEmails.Text = getEmailLeft()
    End Sub
    Protected Sub btnSend_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim msg As MailMessage
        Dim chk As CheckBox
        Dim plainView As AlternateView
        Dim htmlView As AlternateView
        Dim logo As LinkedResource
        Dim emailSendError As Boolean = False
        Dim emailAddError As Boolean = False
        Dim eAdd As String = ""
        If FCKeditor1.Value <> "" And txtSubject.Text <> "" Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procOldEmailsSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            oCmd.CommandType = CommandType.StoredProcedure
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each row As DataRow In ds.Tables(0).Rows
                        If (Not emailSendError) And (Not emailAddError) And Not IsDBNull(row("email")) Then
                            eAdd = row("email")
                            msg = New MailMessage
                            msg.To.Add(replace(row("email"), " ", ""))
                            msg.From = New MailAddress("noreply@emotionalbliss.com")
                            msg.Subject = txtSubject.Text
                            msg.IsBodyHtml = True
                            plainView = AlternateView.CreateAlternateViewFromString(convertBrackets(FCKeditor1.Value) & "<br><br>Copyright ©" & Now.Year & " Emotional Bliss. All rights reserved.", Nothing, "text/plain")
                            htmlView = AlternateView.CreateAlternateViewFromString(convertBrackets(Replace(FCKeditor1.Value, Chr(13), "")) & "<br><br><a href='http://heat.emotionalbliss.co.uk'><img src=cid:companylogo border='0'></a><br><br><font color='#777777' size='-2'>Copyright ©" & Now.Year & " Emotional Bliss. All rights reserved.</font>", Nothing, "text/html")
                            logo = New LinkedResource(Server.MapPath("~/images/400.jpg"))
                            logo.ContentId = "companylogo"
                            htmlView.LinkedResources.Add(logo)
                            msg.AlternateViews.Add(plainView)
                            msg.AlternateViews.Add(htmlView)
                            'set address to false
                            emailSendError = setEmailToSent(row("email"))
                            emailAddError = addEmailToResults(row("email"))
                            Dim client As New SmtpClient
                            client.Send(msg)
                            msg.Dispose()
                        End If
                    Next
                End If
            Catch ex As Exception
                Response.Write("Error sending to '" & eAdd & "'<br>")
                'Response.Write(ex.ToString)
            End Try
        End If
        refreshTotals()
    End Sub
    Protected Function convertBrackets(ByVal txt)
        Dim returnVal As String
        returnVal = Replace(txt, "[", "<")
        returnVal = Replace(returnVal, "]", ">")
        Return returnVal
    End Function
    Protected Function setEmailToSent(ByVal e As String) As Boolean
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procOldEmailsUpdate", oConn)
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 500))
            .Parameters("@email").Value = e
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.ToString)
            bError = True
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return bError
    End Function
    Protected Function addEmailToResults(ByVal e As String) As Boolean
        Dim bError As Boolean = False
        Try
            Dim tCell As New TableCell
            Dim tRow As New TableRow
            tCell.Text = e
            tRow.Cells.Add(tCell)
            tblResults.Rows.Add(tRow)
        Catch ex As Exception
            Response.Write(ex.ToString)
            bError = True
        End Try
        Return bError
    End Function
    Protected Function getEmailTotal() As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procOldEmailsStatusTotalSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = "Err"
        With oCmd
            .CommandType = CommandType.StoredProcedure
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim rs As DataRow = ds.Tables(0).Rows(0)
                result = rs("qty")
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function getEmailSent() As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procOldEmailsStatusSentSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = "Err"
        With oCmd
            .CommandType = CommandType.StoredProcedure
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim rs As DataRow = ds.Tables(0).Rows(0)
                result = rs("qty")
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function getEmailLeft() As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procOldEmailsStatusLeftSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = "Err"
        With oCmd
            .CommandType = CommandType.StoredProcedure
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim rs As DataRow = ds.Tables(0).Rows(0)
                result = rs("qty")
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
End Class
