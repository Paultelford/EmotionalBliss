Imports System.Data
Partial Class email
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'lblEmail.Text = Request.QueryString("email")
        Try
            Dim param() As String = {"@email", "@mailshotID"}
            Dim paramValue() As String = {Request.QueryString("email"), "1"}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.Int}
            Dim paramSize() As Integer = {200, 0}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procMailshotInsert")
        Catch ex As Exception
            siteInclude.addError("", "(); " & ex.ToString())
        End Try
    End Sub

    'User - Click
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'lblComplete.Text = "Your voucher has been despatched to "
        'lblEmail.ForeColor = Drawing.Color.Red
        panIntro.Visible = False
        panCompelte.Visible = True
        'Dim dummyInt As Integer = siteInclude.sendSQLEmail(Request.QueryString("email"), "", "", "Your EB Voucher Code", "noreply@emotionalbliss.co.uk", "Emotional Bliss", getBody(), siteInclude._emailType.emailHtml)
        Try
            Dim param() As String = {"@email", "@mailshotID"}
            Dim paramValue() As String = {Request.QueryString("email"), "1"}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.Int}
            Dim paramSize() As Integer = {200, 0}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procMailshotInsert")
        Catch ex As Exception
            siteInclude.addError("", "(); " & ex.ToString())
        End Try
    End Sub

    'Functions
    Protected Function getBody() As String
        Dim html As String = ""
        html = html & "<br><br>Here is your voucher from Emotional Bliss.<br><br><br><font color='red'>EB00ABCDE</font><br><br><br>"
        html = html & "To take advantage of this offer, visit <a href='http://www.emotionalbliss.com'>Emotional Bliss</a> and add a product to your basket.<br>Goto the checkout page then enter your discount code to receive 10% off."
        Return html
    End Function
End Class
