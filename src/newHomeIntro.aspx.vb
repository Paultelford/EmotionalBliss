
Imports System.Data

Partial Class newHomeIntro
    Inherits System.Web.UI.Page

    Private _htVoucherCodes As Hashtable
    Private _url As String
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim country As String = Request.QueryString("country")
        Select Case country
            Case "gb"
                Session("EBShopCountry") = "gb"
            Case "EE"
                Session("EBShopCountry") = "EE"
            Case "lt"
                Session("EBShopCountry") = "lt"
            Case "SI"
                Session("EBShopCountry") = "SI"
            Case "us"
                Session("EBShopCountry") = "us"
            Case "fi"
                Session("EBShopCountry") = "fi"
            Case "lu"
                Session("EBShopCountry") = "lu"
            Case "ZA"
                Session("EBShopCountry") = "ZA"
            Case "au"
                Session("EBShopCountry") = "au"
            Case "fr"
                Session("EBShopCountry") = "fr"
            Case "MT"
                Session("EBShopCountry") = "MT"
            Case "es"
                Session("EBShopCountry") = "es"
            Case "at"
                Session("EBShopCountry") = "at"
            Case "de"
                Session("EBShopCountry") = "de"
            Case "nl"
                Session("EBShopCountry") = "nl"
            Case "se"
                Session("EBShopCountry") = "se"
            Case "be"
                Session("EBShopCountry") = "be"
            Case "GR"
                Session("EBShopCountry") = "GR"
            Case "no"
                Session("EBShopCountry") = "no"
            Case "ch"
                Session("EBShopCountry") = "ch"
            Case "BG"
                Session("EBShopCountry") = "BG"
            Case "HU"
                Session("EBShopCountry") = "HU"
            Case "NZ"
                Session("EBShopCountry") = "NZ"
            Case "ca"
                Session("EBShopCountry") = "ca"
            Case "is"
                Session("EBShopCountry") = "is"
            Case "PL"
                Session("EBShopCountry") = "PL"
            Case "CY"
                Session("EBShopCountry") = "CY"
            Case "ie"
                Session("EBShopCountry") = "ie"
            Case "pt"
                Session("EBShopCountry") = "pt"
            Case "cz"
                Session("EBShopCountry") = "cz"
            Case "it"
                Session("EBShopCountry") = "it"
            Case "RO"
                Session("EBShopCountry") = "RO"
            Case "dk"
                Session("EBShopCountry") = "dk"
            Case "LV"
                Session("EBShopCountry") = "LV"
            Case "SK"
                Session("EBShopCountry") = "SK"
            Case Else
                If Session("EBShopCountry") = "" Then
                    Session("EBShopCountry") = "gb"
                End If
                Exit Select
        End Select

        _htVoucherCodes = New Hashtable
        _htVoucherCodes.Add("gb", "58284801")
        _htVoucherCodes.Add("nl", "69368464")
        _htVoucherCodes.Add("us", "45509222")
        lblSentOK.Text = ""
        Select Case LCase(Session("EBShopCountry"))
            Case "gb"
                lblDiscountAmount.Text = "£5"
            Case "us"
                lblDiscountAmount.Text = "$10"
            Case "ca"
                lblDiscountAmount.Text = "$10"
            Case Else
                lblDiscountAmount.Text = "€7.50"
        End Select
        _url = "http://www.emotionalbliss.com"
    End Sub
    Protected Sub lnkSend_click(ByVal sender As Object, ByVal e As EventArgs)
        _url = Request.ServerVariables("SERVER_NAME") & Request.ServerVariables("SCRIPT_NAME")
        If Request.ServerVariables("QUERY_STRING") <> "" Then _url = _url & "?" & Request.ServerVariables("QUERY_STRING")
        Dim sTo As String = txtFriendsEmail.Text
        Dim sCC As String = ""
        Dim sBCC As String = ""
        Dim sSubject As String = txtYourName.Text & " has asked us to send you the following information on their behalf"
        Dim sFrom As String = "noreply@emotionalbliss.com"
        Dim sFromName As String = txtYourName.Text
        Dim sBody As String = "Dear " & txtFriendsName.Text & ",<br><br>"
        sBody = sBody & "We have been requested by " & txtYourName.Text & " to send you the following link as it may be of interest.<br><br>"
        sBody = sBody & "Link: <a href='" & _url & "'>" & _url & "</a><br><br>"
        sBody = sBody & "As this information has been requested by " & txtYourName.Text & " it is our pleasure to enclose the following voucher code offering you a " & lblDiscountAmount.Text & " discount on any future purchases made on the " & UCase(Session("EBShopCountry")) & " website <a href='http://www.emotionalbliss.com'>http://www.emotionalbliss.com</a>.<br><br>"
        sBody = sBody & "To activate the voucher simply copy and paste the code when on the basket page.<br><br>"
        sBody = sBody & "Voucher code: " & _htVoucherCodes(LCase(Session("EBShopCountry"))) & "<br><br>"
        sBody = sBody & "Message: " & txtMessage.Text
        Try
            siteInclude.sendSQLEmail(sTo, sCC, sBCC, sSubject, sFrom, sFromName, sBody, siteInclude._emailType.emailHtml)
        Catch ex As Exception
            siteInclude.addError("msite.master.vb", "lnkSend_click::EmailNotSent(); " & ex.ToString())
        End Try

        'Add email addresses to db
        Dim sAddress As String = txtYourEmail.Text
        Dim sName As String = sFromName
        Dim sGroupID As String = "4"
        For iLoop As Integer = 1 To 2
            If iLoop = 2 Then
                sAddress = sTo
                sName = txtFriendsName.Text
                sGroupID = "5"
            End If
            Dim dt As New DataTable
            Try
                Dim param() As String = New String() {"@address", "@name", "@groupID"}
                Dim paramValue() As String = New String() {sAddress, sName, sGroupID}
                Dim paramType() As SqlDbType = New SqlDbType() {SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int}
                Dim paramSize() As Integer = New Integer() {100, 50, 0}
                siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procEmailInsert2")
            Catch ex As Exception
                siteInclude.addError("msite.master.vb", "lnkSend_click(@address=" & sAddress & ", @name=" & sName & "@groupID=" & sGroupID & "); " & ex.ToString())
            Finally
                dt.Dispose()
            End Try
        Next
        txtFriendsEmail.Text = ""
        txtFriendsName.Text = ""
        txtMessage.Text = ""
        txtYourEmail.Text = ""
        txtYourName.Text = ""
        lblSentOK.Text = "Email successfully sent.<br>Thank you."
    End Sub

    Protected Sub Unnamed_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub
End Class
