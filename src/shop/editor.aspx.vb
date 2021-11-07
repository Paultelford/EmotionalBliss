Imports System.IO
Imports System.data
Imports System.net
Imports System.Xml
Imports System.data.SqlClient

Partial Class editor
    Inherits System.Web.UI.Page
    Private currentDirectory As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or (Not Session("EBTextEdit")) Then
            'Response.Write("You are not logged in.<br>Click <a href='affiliates/default.aspx'>here</a> to log in.")
            'Response.End()
        End If
        populatePageDropdown()
        If Not Page.IsPostBack Then
            If Request.QueryString("p") <> "" And Request.QueryString("l") <> "" And Request.QueryString("pg") <> "" Then
                'User has clicked a paragraph of text that they want to edit
                FCKeditor1.Value = getDBResouceString(Request.QueryString("p"), Request.QueryString("l"), Replace(Request.QueryString("pg"), ".", "_"), False)
                'Set the page dropdown to match the passed pagename
                drpPage.SelectedValue = Request.QueryString("pg")
                'Make the preview visible
                previewUserData()
                'Show the Save Changes button
                btnSubmit.Visible = True
                'Display the current element name thats being edited
                lblEditing.Text = "Currently Edititing: <b>" & Request.QueryString("p") & "</b>"
            End If
        End If
    End Sub
    
    Protected Sub drpPage_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Automatically show the 'Preview User Data' gubbins
        previewUserData()
        'Hide the Save Changes button, and the 'Currently Edititng' label.
        btnSubmit.Visible = False
        lblEditing.Text = ""
        'Clear the FCK editor window
        FCKeditor1.Value = ""
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Save text to database if Length test is successful
        Dim bError As Boolean = False
        If Len(FCKeditor1.Value) <= 4000 Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procDBResourcesInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@page", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@string", SqlDbType.VarChar, 4000))
                .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
                .Parameters("@page").Value = Replace(drpPage.SelectedValue, ".", "_")
                .Parameters("@name").Value = Request.QueryString("p")
                .Parameters("@string").Value = FCKeditor1.Value
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                lblError.Text = "An error occures while saving the text; " & ex.ToString
                bError = True
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If Not bError Then lblError.Text = "<font color='red'>Save Successful</font>"
        Else
            lblError.Text = "Text too long. Max 4000 characters"
        End If
        previewUserData()
    End Sub
    Protected Sub btnView_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim html As String
        Dim displayHTML As String
        'Get the HTML for the currently selected page and language (Session will need setting to selected language)
        'Session("EBLanguage") = drpLanguage.SelectedValue
        'html = getPageHTML("editorRedirect.aspx?target=" & drpPage.SelectedValue & "&lang=" & drpLanguage.SelectedValue)
        html = getPageHTML(drpPage.SelectedValue & "?forceLang=" & Session("EBAffEBDistributorCountryCode"))
        displayHTML = parseHTML(html) 'Remove the header/menu/top images/footer
        panHTML.Visible = True
        lblHTML.Text = displayHTML
        lblPageName.Text = drpPage.Text & " - Preview"
    End Sub
    Protected Function parseHTML(ByVal html As String) As String
        'This function recieves the full page html, then removed the header/menu/footer
        Dim startPos As Integer = InStr(html, "<EBContentTag>")
        Dim endPos As Integer = InStr(html, "</EBContentTag>")
        Dim result As String = "Page cannot be displayed"
        If startPos > 0 And endPos > startPos Then
            'Tags found - trim the gubbins
            result = Mid(html, startPos, endPos - startPos)
        End If
        Return result
    End Function
    Protected Function getPageHTML(ByVal page As String) As String
        Dim rand As New Random()
        'Dim uri As New Uri("http://" & Request.ServerVariables("HTTP_HOST") & "/" & page & "&" & CStr(rand.Next))
        Dim uri As New Uri("http://" & Request.ServerVariables("HTTP_HOST") & "/editorRedirect.aspx?lang=" & Session("EBAffEBDistributorCountryCode") & "&target=" & page & "&" & CStr(rand.Next))
        'Response.Write(uri.ToString)
        'Response.End()
        Dim req As HttpWebRequest = HttpWebRequest.Create(uri)
        req.Method = WebRequestMethods.Http.Get
        'Response.Write(uri)
        'Response.End()
        Dim r As HttpWebResponse
        Dim html As String
        Try
            r = req.GetResponse
            Dim reader As New StreamReader(r.GetResponseStream)
            html = reader.ReadToEnd
        Catch ex As Exception
            Response.Write(" URL=" & uri.AbsoluteUri)
            Response.End()
        Finally
            r.Close()
        End Try
        Return html
    End Function
    Protected Sub btnViewUser_click(ByVal sender As Object, ByVal e As EventArgs)
        'Show page data
        previewUserData()
    End Sub
    Protected Sub btnNavigate_click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(currentDirectory & drpPage.SelectedValue)
    End Sub
    Protected Sub previewUserData()
        lblHTML.Text = ""
        panHTML.Visible = True
        makeEditableHTML()
        lblPageName.Text = drpPage.Text & " - Editable Data"
    End Sub
    Protected Sub makeEditableHTML()
        Dim sr As StreamReader
        Dim line As String
        Dim bProcessHTML As Boolean = False
        sr = File.OpenText(Server.MapPath(drpPage.SelectedValue))
        While sr.Peek() <> -1
            line = sr.ReadLine()
            If InStr(line, "ContentPlaceHolder1") Then bProcessHTML = True
            If InStr(line, "/asp:Content") Then bProcessHTML = False
            If bProcessHTML And InStr(LCase(line), "dbresource") Then processHTML(line)
        End While
        sr.Close()
        sr.Dispose()
        'File.SetAttributes(Server.MapPath(drpPage.SelectedItem.Text), FileAttributes.Normal)
    End Sub
    Protected Sub processHTML(ByVal h As String)
        Dim ElementName As String = parseElementName(h)
        Dim lnk As New HyperLink
        'Add element name
        Dim lblElement As New Label
        lblElement.Text = "<table cellpadding='0' cellspacing='0' width='100%'><tr><td><font color='red'>" & ElementName & "</font></td><td width='100%'><hr></td></tr></table>"
        lblHTML.Controls.Add(lblElement)
        'Add element data
        'lnk.Text = Replace(LCase(Replace(LCase(getTextByElementName(drpPage.SelectedValue, ElementName, Session("EBAffEBDistributorCountryCode"))), "<a", "&lt;a")), "</a", "&lt;a")
        lnk.Text = getDBResouceString(ElementName, Session("EBAffEBDistributorCountryCode"), Replace(drpPage.SelectedValue, ".", "_"), True)
        lnk.NavigateUrl = getPageName() & "?l=" & Session("EBAffEBDistributorCountryCode") & "&pg=" & drpPage.SelectedValue & "&p=" & ElementName
        lblHTML.Controls.Add(lnk)
        'Add line break
        Dim lblBreak As New Label
        lblBreak.Text = "<br><br>"
        lblHTML.Controls.Add(lblBreak)
    End Sub
    Protected Function parseElementName(ByVal e As String) As String
        Dim result As String = ""
        Try
            Dim iPos As Integer = InStr(LCase(e), "dbresource")
            Dim newString As String = Right(e, (Len(e) - iPos) - 9)
            Dim firstQuote As Integer = InStr(newString, """")
            Dim tmpStr As String = Right(newString, Len(newString) - firstQuote)
            Dim secondQuote As Integer = InStr(tmpStr, """")
            result = Left(tmpStr, secondQuote - 1)
        Catch ex As Exception
            result = "Bad Element Name"
        End Try
        Return result
    End Function
    Protected Function getPageName() As String
        Dim result As String
        Dim a() As String = Split(Request.ServerVariables("PATH_TRANSLATED"), "\")
        result = a(UBound(a))
        Return result
    End Function
    Protected Sub populatePageDropdown()
        Dim dirArr = Directory.GetFiles(Server.MapPath("\" & currentDirectory))
        For Each file As String In dirArr
            'Add pagenames to dropdown
            addToPageDropdown(file)
        Next
    End Sub
    Protected Sub addToPageDropdown(ByVal filepath As String)
        Dim arr1() As String = Split(filepath, "\")
        Dim fullFilename As String = arr1(UBound(arr1))
        If LCase(Right(fullFilename, 4)) = "aspx" Then drpPage.Items.Add(New ListItem(Left(fullFilename, Len(fullFilename) - 5), fullFilename))
    End Sub
    Protected Function getDBResouceString(ByVal name As String, ByVal language As String, ByVal page As String, ByVal returnErrors As Boolean) As String
        Dim result As String = ""
        If returnErrors Then result = "<font color='red'>_dbResource:" & name & "</font> -> Resource not found for current langauge<br>"
        If name = "" Then Return "<font color='red'>_dbResource:" & name & "</font> -> Resource name not specified<br>"
        If language = "" Then Return "<font color='red'>_dbResource:" & name & "</font> -> Language not specified<br>"
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDBResourcesByNameSelect", oConn)
        Dim da As New SqlDataAdapter()
        Dim ds As New DataSet()
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
        oCmd.Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
        oCmd.Parameters.Add(New SqlParameter("@pagename", SqlDbType.VarChar, 50))
        oCmd.Parameters("@name").Value = name
        oCmd.Parameters("@countryCode").Value = language
        oCmd.Parameters("@pagename").Value = page
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = ds.Tables(0).Rows(0)("value")
        Catch ex As Exception
            'Not much we can do about it really, return the error message for now
            result = ex.ToString()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
End Class
