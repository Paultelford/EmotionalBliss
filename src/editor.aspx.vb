Imports System.IO
Imports System.data
Imports System.net
Imports System.Xml
Imports System.data.SqlClient
Imports System.Text.RegularExpressions

Partial Class editor
    Inherits System.Web.UI.Page
    Private currentDirectory As String = ""

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or (Not Session("EBTextEdit")) Then
            Response.Write("You are not logged in.<br>Click <a href='affiliates/default.aspx'>here</a> to log in.")
            Response.Write("<br>Session(EBTextEdit)=" & Session("EBTextEdit"))
            Response.Write("<br>Session(affEBUser)=" & Session("affEBUser"))
            Response.End()
        End If
        If Not Page.IsPostBack Then
            populatePageDropdown()
            If Request.QueryString("type") = "individual" Then
                'Set the NewControls panel as the visible one
                panControls.Visible = False
                panNewControls.Visible = True
                'Also set the Type dropdown to Individual.
                drpPageType.SelectedValue = "individual"
            End If
            If Request.QueryString("p") <> "" And Request.QueryString("l") <> "" And Request.QueryString("pg") <> "" Then
                'User has clicked a paragraph of text that they want to edit
                FCKeditor1.Value = getDBResouceString(Request.QueryString("p"), Request.QueryString("l"), getSQLPagename(Request.QueryString("pg")), False)
                'Set the page dropdown to match the passed pagename
                If panControls.Visible Then
                    drpPage.SelectedValue = Request.QueryString("pg")
                Else
                    drpIndividualPage.SelectedValue = Request.QueryString("pg")
                End If
                'Make the preview visible
                previewUserData()
                'Show the Save Changes button
                btnSubmit.Visible = True
                'Display the current element name thats being edited
                lblEditing.Text = "Currently Edititing: <b>" & Request.QueryString("p") & "</b>"
                trEdit1.Visible = True
            End If
            Else
                'Page is postback
            End If
            lblError.Text = ""
        lblPageSearchError.Text = ""
        lblComplete.Text = ""
    End Sub

    'Page Events
    Protected Sub drpIndividualPage_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        'Dim drp As DropDownList = CType(sender, DropDownList)
        'drp.Items.Clear()
        'drp.Items.Add(New ListItem("Please Choose....", "0"))
    End Sub

    'User Events
    Protected Sub drpPageType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.SelectedValue = "static" Then
            panNewControls.Visible = False
            'previewUserData()
        Else
            panNewControls.Visible = True
        End If
        panControls.Visible = Not panNewControls.Visible
        'Each time this dropdown is changed, reset all on screen data
        panEdit.Visible = False 'Hide Edit panel
        FCKeditor1.Value = "" 'Clear editor window
        drpPage.SelectedIndex = 0 'Reset page dropdown
        drpIndividualPage.SelectedIndex = 0 'Reset individual page dropdown
        trEdit1.Visible = False 'Hide panCcontrols' internal controls
        trEdit2.Visible = False
        trEdit3.Visible = False
        tdButtons2.Visible = False
        tdNewPage.Visible = False
    End Sub
    Protected Sub drpPage_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpPage.SelectedValue <> "0" Then
            'Automatically show the 'Preview User Data' gubbins
            previewUserData()
            'Hide the Save Changes button, and the 'Currently Edititng' label.
            btnSubmit.Visible = False
            lblEditing.Text = ""
            trEdit1.Visible = True
            trEdit2.Visible = True
            trEdit3.Visible = True
        Else
            'If user selected the 'Please choose....' option from dropdown, then reset page to look like it does on 1st visit.
            lblPageName.Visible = False
            panHTML.Visible = False
            lblEditing.Visible = False
            btnSubmit.Visible = False
            lblComplete.Visible = False
            trEdit1.Visible = False
            trEdit2.Visible = False
            trEdit3.Visible = False
        End If
        'Clear the FCK editor window
        FCKeditor1.Value = ""
        'Reset the Page Name Edit
        txtNewPageNameEdit.Text = ""
        tblEditName.Visible = False
        lnkBtnEditPageName.Visible = True
    End Sub
    Protected Sub drpIndividualPage_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.SelectedValue <> "0" Then
            previewUserData()
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Save text to database if Length test is successful
        Dim bError As Boolean = False
        If Len(FCKeditor1.Value) <= 8000 Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procDBResourcesInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@page", SqlDbType.VarChar, 500))
                .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 500))
                .Parameters.Add(New SqlParameter("@string", SqlDbType.VarChar, 8000))
                .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
                .Parameters("@page").Value = getSQLPagename(drpPage.SelectedValue)
                .Parameters("@name").Value = Request.QueryString("p")
                .Parameters("@string").Value = FCKeditor1.Value
            End With
            If Not panControls.Visible Then oCmd.Parameters("@page").Value = getSQLPagename(drpIndividualPage.SelectedValue)
            If LCase(Request.QueryString("p")) = "pagetitle" Or LCase(Request.QueryString("p")) = "metadescription" Or LCase(Request.QueryString("p")) = "metakeywords" Then oCmd.Parameters("@string").Value = formatData(FCKeditor1.Value)
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
            If Not bError Then lblComplete.Text = "<font color='red'>Save Successful</font>"
            If hidMenuType.Value <> drpCurrentMenu.SelectedValue Then
                'updateMenuType()
                'lblError.Text = "drpCurrentMenu.SelectedValue=" & drpCurrentMenu.SelectedValue
            End If
        Else
            lblError.Text = "Text too long. Max 8000 characters"
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
    Protected Sub lnkBtnEditPageName_click(ByVal sender As Object, ByVal e As EventArgs)
        lnkBtnEditPageName.Visible = False
        tblEditName.Visible = True
        txtNewPageNameEdit.Text = drpPage.SelectedItem.Text
        lblPageName.Visible = False
    End Sub
    Protected Sub btnSubmitNewPageName_click(ByVal sender As Object, ByVal e As EventArgs)
        'Make sure new pagename doesnt already exist.
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim ocmd As New SqlCommand("procSiteMenusByCountryNameSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim bError As Boolean = False
        With ocmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 500))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@name").Value = txtNewPageNameEdit.Text
        End With
        Try
            If ocmd.Connection.State = 0 Then ocmd.Connection.Open()
            da = New SqlDataAdapter(ocmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then
                'New pagename does not exist
                bError = Not updatePageNameOK() 'Returns true if all is ok
            Else
                'Error, pagename already exists
                lblError.Text = "<font color='red'>That pagename already exists, please choose another.</font>"
                bError = True
            End If
        Catch ex As Exception
            Response.Write(ex)
            lblError.Text = "<font color='red'>An error occured while changing the pagename.</font>"
            bError = True
        Finally
            ds.Dispose()
            da.Dispose()
            ocmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            'Success, tidy up
            'Reset the Page Name Edit
            Dim tmpPagename As String = txtNewPageNameEdit.Text
            txtNewPageNameEdit.Text = ""
            tblEditName.Visible = False
            lnkBtnEditPageName.Visible = True
            drpPage.DataBind()
            drpPage.SelectedItem.Text = tmpPagename
            previewUserData()
        End If
    End Sub
    Protected Sub drpCurrentMenu_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'menuType has just been changed, update selected page with the new type
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByURLMenuTypeUpdate", oConn)
        Dim bError As Boolean
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@menuType", SqlDbType.VarChar, 200))
            .Parameters("@url").Value = "~/" & drpPage.SelectedValue
            .Parameters("@menuType").Value = drpCurrentMenu.SelectedValue
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "An error occured while changing the new Menu Type."
            bError = True
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            lblError.Text = "Menu Type has been updated."
            previewUserData()
        End If
    End Sub
    Protected Sub chkShowOnMenuEdit_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        'showOnMenu has just been changed. Update siteMenus table
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByURLShowOnMenuUpdate", oConn)
        Dim bError As Boolean
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@showOnMenu", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@priority", SqlDbType.Int))
            .Parameters("@url").Value = "~/" & drpPage.SelectedValue
            .Parameters("@showOnMenu").Value = chkShowOnMenuEdit.Checked
            .Parameters("@name").Value = drpPage.SelectedItem.Text
            .Parameters("@priority").Value = 0
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("editor.aspx.vb", "chkShowOnMenuEdit_checkChanged(); " & ex.ToString)
            si = Nothing
            lblError.Text = "An error occured while updating the ShowOnMenu option."
            bError = True
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            If chkShowOnMenuEdit.Checked Then
                lblError.Text = "Page will now show on the front menu."
            Else
                lblError.Text = "Page will not show on the front menu."
            End If
            previewUserData()
        End If
    End Sub
    Protected Sub btnNewPageSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Make sure new pagename doesnt already exist.
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim ocmd As New SqlCommand("procSiteMenusByCountryURLSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With ocmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 500))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@url").Value = "~/" & Replace(Trim(txtNewPageName.Text), " ", "_")
        End With
        Try
            If ocmd.Connection.State = 0 Then ocmd.Connection.Open()
            da = New SqlDataAdapter(ocmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then
                'All ok, add new page
                If createPage(txtNewPageName.Text, Session("EBAffEBDistributorCountryCode"), drpMenuType.SelectedValue) Then
                    'Success, point to new page
                    populatePageDropdown()
                    drpPage.SelectedValue = Replace(Trim(txtNewPageName.Text), " ", "_")
                    'Automatically show the 'Preview User Data' gubbins
                    previewUserData()
                    'Hide the Save Changes button, and the 'Currently Edititng' label.
                    btnSubmit.Visible = False
                    lblEditing.Text = ""
                    lblPageName.Text = drpPage.Text & " - Editable Data"
                    lblPageName.Visible = True
                    btnAdd.Visible = True
                    FCKeditor1.Value = ""
                    txtNewPageName.Text = ""
                    lblComplete.Text = ""
                    tdNewPage.Visible = False
                End If
            Else
                'Something was found matching supplied url with the countrycode
                lblNewPageError.Text = "That page already exists in the database.<br>Please try another name."
            End If
        Catch ex As Exception
            lblNewPageError.Text = ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            ocmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub upArrow_click(ByVal sender As Object, ByVal e As EventArgs)
        previewUserData()
        Dim btn As Button = CType(sender, Button)
        'swapParagraphs(btn.CommandArgument, CInt(btn.CommandArgument) - 1)
        Response.Write("upArrow_click(" & btn.CommandArgument & "," & CInt(btn.CommandArgument) - 1 & ")")
    End Sub
    Protected Sub downArrow_click(ByVal sender As Object, ByVal e As EventArgs)
        previewUserData()
        Dim btn As Button = CType(sender, Button)
        'swapParagraphs(btn.CommandArgument, CInt(btn.CommandArgument) + 1)
        Response.Write("downArrow_click(" & btn.CommandArgument & "," & CInt(btn.CommandArgument) + 1 & ")")
    End Sub
    Protected Sub btnViewUser_click(ByVal sender As Object, ByVal e As EventArgs)
        'Show page data
        previewUserData()
    End Sub
    Protected Sub btnNavigate_click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect(currentDirectory & drpPage.SelectedValue)
    End Sub
    Protected Sub btnAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        'Show the add new page controls
        tdNewPage.Visible = True
        btnAdd.Visible = False
        lblPageName.Visible = False
        panHTML.Visible = False
        lblCurrentMenuText.Visible = False
        drpCurrentMenu.Visible = False
    End Sub
    Protected Sub txtPageSearch_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim bFileFound As Boolean = True
        If txtPageSearch.Text = "" Then
            'Do nowt, no need to display error, user probably clicked on the textbox, then onto anothre control.
        Else
            'Some text was entered. Do a lookup.
            Dim sr As StreamReader
            Dim bEditable As Boolean = False
            Try
                sr = File.OpenText(Server.MapPath(txtPageSearch.Text))
            Catch ex As Exception
                bFileFound = False
                'Page not found, show error and user help
                lblPageSearchError.Text = "The pagename you entered could not be found.<br>"
                If Not instr(txtPageSearch.Text, ".aspx") Then
                    lblPageSearchError.Text = lblPageSearchError.Text & "Always include the '.aspx' file extension.<br>"
                Else
                    lblPageSearchError.Text = lblPageSearchError.Text & "If the page you are trying to edit exisint inside a directory, then please add that.<br>"
                    lblPageSearchError.Text = lblPageSearchError.Text & "For example, if the page you want to edit is 'http://www.emotionalbliss.com/shop/basket.aspx' Then type 'shop/basket.aspx' into the textbox."
                End If
            End Try
            If bFileFound Then
                'File found ok, test for editable content

                Dim line As String
                While sr.Peek() <> -1
                    line = sr.ReadLine()
                    If instr(lcase(line), "dbresource") Then bEditable = True
                    If bEditable Then Exit While
                End While
                If bEditable Then
                    'File can be edited, add to the database for future inclusion to the dropdown
                    addPage(txtPageSearch.Text)
                    'Load page up for user to view and edit
                    panEdit.Visible = True
                    lblHTML.Text = ""
                    panHTML.Visible = True
                    lblPageName.Text = txtPageSearch.Text & " - Editable Data"
                    makeEditableHTML(txtPageSearch.Text)
                    'Rebind individualPage dropdown and clear the input box (if successful)
                    populatePageDropdown()
                    drpIndividualPage.SelectedValue = txtPageSearch.Text
                    txtPageSearch.Text = ""
                Else
                    lblPageSearchError.Text = "The pagename you entered is not editable."
                End If
            End If
        If Not sr Is Nothing Then
            sr.Close()
            sr.Dispose()
        End If
        End If
    End Sub

    'Subs
    Protected Sub previewUserData()
        Dim drp As DropDownList
        If panControls.Visible Then
            drp = drpPage
            btnAdd.Visible = True
        Else
            drp = drpIndividualPage
            btnAdd.Visible = False
        End If
        panEdit.Visible = True
        lblHTML.Text = ""
        panHTML.Visible = True
        makeEditableHTML(drp.SelectedValue)
        lblPageName.Text = drp.SelectedValue & " - Editable Data"
    End Sub
    Protected Sub makeEditableHTML(ByVal pagename As String)
        Dim sr As StreamReader
        Dim line As String
        Dim bProcessHTML As Boolean = False
        Try
            sr = File.OpenText(Server.MapPath(pagename))
        Catch ex As Exception
            'Page cannot be found, default to static.aspx instead
            sr = File.OpenText(Server.MapPath("~/static.aspx"))
        End Try

        While sr.Peek() <> -1
            line = sr.ReadLine()
            If InStr(line, "ContentPlaceHolder1") Then bProcessHTML = True
            If InStr(line, "/asp:Content") Then bProcessHTML = False
            If bProcessHTML And InStr(LCase(line), "dbresource") Then processHTML(line, pagename)
        End While
        getDetails() 'This grabs the page's isStatic, Menu and showOnMenu details
        sr.Close()
        sr.Dispose()
        'File.SetAttributes(Server.MapPath(drpPage.SelectedItem.Text), FileAttributes.Normal)
    End Sub
    Protected Sub processHTML(ByVal h As String, ByVal pagename As String)
        Dim ElementName As String = parseElementName(h)
        Dim lnk As New HyperLink
        Dim url As String = getPageName() & "?l=" & Session("EBAffEBDistributorCountryCode") & "&pg=" & pagename & "&p=" & ElementName
        If Not panControls.Visible Then url = url & "&type=individual" 'So the page knows to show the correct ControlsPanel if the user is editing Individual pages.
        'Add element name
        Dim lblElement As New Label
        lblElement.Text = "<table cellpadding='0' cellspacing='0' width='100%'><tr><td><font color='red'><a href='" & url & "'>" & ElementName & "</a></font></td><td width='96%'><hr></td><td>"
        If lcase(Session("EBAffEBDistributorCountryCode")) <> "gb" Then
            'Show an English translation, to help foreign distributors know wht to put in each section
            lblElement.Text = lblElement.Text & "<acronym title=""" & removeHTMLTags(getDBResouceString(ElementName, "gb", getSQLPagename(pagename), False)) & """>T</acronym>"
        End If
        lblElement.Text = lblElement.Text & "</td></tr></table>"
        lblHTML.Controls.Add(lblElement)
        'Create a html table control, 1st td will contain the link control, second td will contain the up and down arrow controls (only if text exists)
        Dim tbl As New Table
        Dim tRow As New TableRow
        Dim tCell As New TableCell
        tbl.Width = Unit.Percentage(100)
        'Add element data
        'lnk.Text = Replace(LCase(Replace(LCase(getTextByElementName(drpPage.SelectedValue, ElementName, Session("EBAffEBDistributorCountryCode"))), "<a", "&lt;a")), "</a", "&lt;a")
        lnk.Text = getDBResouceString(ElementName, Session("EBAffEBDistributorCountryCode"), getSQLPagename(pagename), False)
        lnk.NavigateUrl = url
        tCell.Controls.Add(lnk)
        tRow.Cells.Add(tCell)
        If lnk.Text <> "" And LCase(ElementName) <> "pagetitle" Then
            tCell = New TableCell
            tCell.Width = 20
            tCell.Controls.Add(upArrow(ElementName))
            tCell.Controls.Add(downArrow(ElementName))
            'tRow.Cells.Add(tCell) Removed due to lack of dynamic event handling :(
        End If
        tbl.Rows.Add(tRow)
        lblHTML.Controls.Add(tbl)
        'Add line break
        Dim lblBreak As New Label
        lblBreak.Text = "<br><br>"
        lblHTML.Controls.Add(lblBreak)
    End Sub
    Protected Sub populatePageDropdown()
        'Dim dirArr = Directory.GetFiles(Server.MapPath("\" & currentDirectory))
        'For Each file As String In dirArr
        'Add pagenames to dropdown
        'addToPageDropdown(File)
        'Next
        drpPage.Items.Clear()
        drpPage.Items.Add(New ListItem("Please Choose....", "0"))
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@menuType", SqlDbType.VarChar, 200))
            .Parameters.Add(New SqlParameter("@showInEditor", SqlDbType.VarChar, 1))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@menuType").Value = "%"
            .Parameters("@showInEditor").Value = "1"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    addToPageDropdown(row("name"), row("url"))
                Next
            End If
        Catch ex As Exception
            lblError.Text = "An error occured while building pagelist; " & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Do the same for the Individual Dropdown
        drpIndividualPage.Items.Clear()
        drpIndividualPage.Items.Add(New ListItem("Please Choose....", "0"))
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procSiteMenusPageSelect", oConn)
        da = New SqlDataAdapter
        ds = New DataSet
        oCmd.CommandType = CommandType.StoredProcedure
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            drpIndividualPage.DataSource = ds.Tables(0)
            drpIndividualPage.DataBind()
        Catch ex As Exception
            lblError.Text = "An error occured while building individual pagelist; " & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub addToPageDropdown(ByVal fileDescription As Object, ByVal filePath As Object)
        'Dim arr1() As String = Split(filepath, "/")
        'Dim fullFilename As String = arr1(UBound(arr1))
        If Not IsDBNull(fileDescription) Then
            If Not IsDBNull(filePath) Then drpPage.Items.Add(New ListItem(fileDescription.ToString, Right(filePath.ToString, Len(filePath.ToString) - 2)))
        End If

    End Sub
    Protected Sub swapParagraphs(ByVal p1 As String, ByVal p2 As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDBResourcesByParaSwapUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@page", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@para1", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@para2", SqlDbType.VarChar, 500))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@page").Value = Replace(drpPage.SelectedValue, ".", "_")
            .Parameters("@para1").Value = "paragraph" & p1
            .Parameters("@para2").Value = "paragraph" & p2
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured while swapping elements " & p1 & " and " & p2 & "; " & ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        previewUserData()
    End Sub
    Protected Sub getDetails()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByCountryURLSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 500))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@url").Value = "~/" & Replace(Trim(drpPage.SelectedValue), " ", "_")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                hidStatic.Value = CBool(ds.Tables(0).Rows(0)("static"))
                hidMenuType.Value = ds.Tables(0).Rows(0)("menuType")
                hidName.Value = ds.Tables(0).Rows(0)("name")
                hidURL.Value = ds.Tables(0).Rows(0)("url")
                drpCurrentMenu.SelectedValue = ds.Tables(0).Rows(0)("menuType")
                chkShowOnMenuEdit.Checked = CBool(ds.Tables(0).Rows(0)("showOnMenu"))
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub updateMenuType()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByCountryURLUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@menuType", SqlDbType.VarChar, 200))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@url").Value = "~/" & Replace(drpPage.SelectedValue, ".", "_")
            .Parameters("@menuType").Value = drpCurrentMenu.SelectedValue
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub addPage(ByVal pg As String)
        Dim oConn As New sqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New sqlCommand("procSiteMenusPageInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@page", SqlDbType.VarChar, 200))
            .Parameters("@page").Value = pg
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.AddError("editor.aspx.vb", "addPage(pg=" & pg & "); " & ex.ToString())
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub

    'Functions    
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
    Protected Function upArrow(ByVal e As String) As Button
        Dim btn As New Button
        Dim iParagraph As Integer = getParagraphNumber(e)
        btn.Text = "^"
        btn.CommandArgument = iParagraph
        AddHandler btn.Click, AddressOf upArrow_click
        'btn.Attributes.Add("onClick", "document.forms[0].ctl00_ContentPlaceHolder1_hidUpClick.value='" & iParagraph & "';document.forms[0].submit();")
        If iParagraph = 0 Or iParagraph = 1 Then btn.Visible = False
        Return btn
    End Function
    Protected Function downArrow(ByVal e As String) As Button
        Dim btn As New Button
        Dim iParagraph As Integer = getParagraphNumber(e)
        btn.Text = "v"
        btn.CommandArgument = iParagraph
        AddHandler btn.Click, AddressOf downArrow_click
        If iParagraph = 0 Or iParagraph = 10 Then btn.Visible = False
        Return btn
    End Function
    Protected Function getParagraphNumber(ByVal t As String) As Integer
        Dim result As Integer = 0
        Dim lastChar As String = Right(t, 1)
        If IsNumeric(lastChar) Then
            'Lookin good so far
            If lastChar = "0" Then
                result = 10
            Else
                result = CInt(lastChar)
            End If
        End If
        Return result
    End Function
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
        oCmd.Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 500))
        oCmd.Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
        oCmd.Parameters.Add(New SqlParameter("@pagename", SqlDbType.VarChar, 500))
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
    Protected Function formatData(ByVal s As String)
        Dim result As String
        result = Replace(s, "<p>", "")
        result = Replace(result, "<P>", "")
        result = Replace(result, "</P>", "")
        result = Replace(result, "</p>", "")
        Return result
    End Function
    Protected Function createPage(ByVal page As String, ByVal cc As String, ByVal type As String) As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusInsert", oConn)
        Dim result As Boolean = True
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@menuType", SqlDbType.VarChar, 200))
            .Parameters.Add(New SqlParameter("@showOnMenu", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@static", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@showInEditor", SqlDbType.Bit))
            .Parameters("@countryCode").Value = cc
            .Parameters("@name").Value = page
            .Parameters("@url").Value = "~/" & Replace(Trim(txtNewPageName.Text), " ", "_")
            .Parameters("@menuType").Value = type
            .Parameters("@showOnMenu").Value = chkShowOnMenu.Checked
            .Parameters("@static").Value = True
            .Parameters("@showInEditor").Value = True
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblNewPageError.Text = ex.ToString
            result = False
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function updatePageNameOK() As Boolean
        Dim resultOK As Boolean = True
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByCountryNameUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@menuType", SqlDbType.VarChar, 200))
            .Parameters.Add(New SqlParameter("@newName", SqlDbType.VarChar, 500))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@name").Value = hidName.Value
            .Parameters("@menuType").Value = hidMenuType.Value
            .Parameters("@newName").Value = txtNewPageNameEdit.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured whlie updating the new name.</font>"
            resultOK = False
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return resultOK
    End Function
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
    Protected Function getSQLPagename(ByVal s As String) As String
        Dim result As String
        result = replace(s, ".", "_")
        result = replace(result, "/", "_")
        result = replace(result, "\", "_")
        Return result
    End Function
    Protected Function removeHTMLTags(ByVal s As String) As String
        Return Regex.Replace(s, "<[^>]+?>", "")
    End Function
End Class
