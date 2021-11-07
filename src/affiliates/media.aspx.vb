Imports System.Data

Partial Class affiliates_media
    Inherits System.Web.UI.Page

    'System
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            
        End If
        lblError.Text = ""
    End Sub

    'Page
    Protected Sub drpArticle_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        drpArticle.Items.Clear()
        drpArticle.Items.Add(New ListItem("Select...", ""))
    End Sub
    Protected Sub dvArticle_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = dvArticle.FindControl("drpLinkType")
        Dim hid As HiddenField = dvArticle.FindControl("hidHtml")
        Select Case Convert.ToString(drp.SelectedValue)
            Case "1"
                'Image
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "New Image:")).Visible = True
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "URL:")).Visible = False
                tdEditor.Visible = False
            Case "2"
                'URL
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "New Image:")).Visible = False
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "URL:")).Visible = True
                tdEditor.Visible = False
            Case "3"
                'HTML
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "New Image:")).Visible = False
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "URL:")).Visible = False
                tdEditor.Visible = True
                fck1.Value = hid.Value
            Case Else
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "New Image:")).Visible = False
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "URL:")).Visible = False
                tdEditor.Visible = False
        End Select
    End Sub

    'User
    Protected Sub drpArticle_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpArticle.SelectedIndex > 0 Then
            dvArticle.ChangeMode(DetailsViewMode.Edit)
        Else
            dvArticle.ChangeMode(DetailsViewMode.Insert)
        End If
    End Sub
    Protected Sub sqlArticel_inserted(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        'Database entry has been created, now upload the Thumbnail (and Image if one was selected)
        Dim bError As Boolean = False
        Dim fThumb As FileUpload = dvArticle.FindControl("fThumb")
        Dim fImage As FileUpload
        Try
            Dim id As Integer = CInt(e.Command.Parameters("@id").Value)
            bError = uploadImage(id, fThumb, False)
            If Not bError Then
                If CInt(e.Command.Parameters("@type").Value) = 1 Then
                    'Upload Image too
                    fImage = dvArticle.FindControl("fImage")
                    bError = uploadImage(id, fImage, True)
                End If
            End If
        Catch ex As Exception
            lblError.Text = "An error occured whle inserting the Article."
            siteInclude.addError("affiliates.media.aspx.vb", "sqlArticle_inserted(id=" & e.Command.Parameters("@id").Value & "); " & ex.ToString)
        End Try
        If Not bError Then
            lblError.Text = "Article successfully added."
            drpArticle.DataBind()
            drpArticle.SelectedIndex = 0
            fck1.Value = ""
        End If
    End Sub
    Protected Sub sqlArticle_updated(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        'Database entry has been updated, now upload the Thumbnail (and Image if one was selected)
        Dim bError As Boolean = False
        Dim fThumb As FileUpload = dvArticle.FindControl("fThumb")
        Dim fImage As FileUpload
        Try
            Dim id As Integer = CInt(e.Command.Parameters("@id").Value)
            bError = uploadImage(id, fThumb, False)
            If Not bError Then
                If CInt(e.Command.Parameters("@type").Value) = 1 Then
                    'Upload Image too
                    fImage = dvArticle.FindControl("fImage")
                    bError = uploadImage(id, fImage, True)
                End If
            End If
        Catch ex As Exception
            bError = True
            lblError.Text = "An error occured whle updating the Article."
            siteInclude.addError("affiliates.media.aspx.vb", "sqlArticle_updated(id=" & e.Command.Parameters("@id").Value & "); " & ex.ToString)
        End Try
        If Not bError Then
            drpArticle.SelectedIndex = 0
            drpArticle.DataBind()
            lblError.Text = "Article successfully updated."
            fck1.Value = ""
        End If
    End Sub
    Protected Sub drpLinkType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'Hide certain fields depending on Link Type
        Dim drp As DropDownList = CType(sender, DropDownList)
        Select Case Convert.ToString(drp.SelectedValue)
            Case "1"
                'Image
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "New Image:")).Visible = True
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "URL:")).Visible = False
                tdEditor.Visible = False
            Case "2"
                'URL
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "New Image:")).Visible = False
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "URL:")).Visible = True
                tdEditor.Visible = False
            Case "3"
                'HTML
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "New Image:")).Visible = False
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "URL:")).Visible = False
                tdEditor.Visible = True
            Case Else
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "New Image:")).Visible = False
                dvArticle.Rows(siteInclude.getDVRowByHeader(dvArticle, "URL:")).Visible = False
                tdEditor.Visible = False
        End Select
    End Sub

    'Subs

    'Functions
    Protected Function uploadImage(ByVal articleID As Integer, ByVal up As FileUpload, ByVal keepFilename As Boolean) As Boolean
        'Upload image
        Dim fileName As String = ""
        Dim fileExt As String = ""
        Dim bError As Boolean = False
        Try
            If up.HasFile Then
                fileExt = Right(up.FileName, Len(up.FileName) - InStr(up.FileName, "."))
                If keepFilename Then
                    fileName = up.FileName
                Else
                    fileName = CStr(articleID) & "." & fileExt
                End If
                up.SaveAs(Server.MapPath("~/images/media/") + fileName)
                'Update the media table with the image name
                Try
                    Dim param() As String = {"@id", "@filename", "@isImage"}
                    Dim paramValue() As String = {articleID.ToString(), fileName, keepFilename.ToString()}
                    Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar, SqlDbType.Bit}
                    Dim paramSize() As Integer = {0, 100, 0}
                    siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procMediaByIDImageUpdate")
                Catch ex As Exception
                    bError = True
                    lblError.Text = "Could not write image file to disk."
                    siteInclude.addError("affiliates/media.asp.vb", "uploadImage2(articleID=" & articleID & ", filename=" & up.FileName & ", keepName=" & keepFilename & "); " & ex.ToString)
                Finally
                End Try
            End If
        Catch ex As Exception
            bError = True
            lblError.Text = "Could not write image file to disk."
            siteInclude.addError("affiliates/media.asp.vb", "uploadImage(articleID=" & articleID & ", filename=" & up.FileName & ", keepName=" & keepFilename & "); " & ex.ToString)
        End Try
        
        Return bError
    End Function
End Class
