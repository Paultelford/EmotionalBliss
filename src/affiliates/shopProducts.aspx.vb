Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.Script
Imports AjaxControlToolkit

Partial Class affiliates_shopProducts
    Inherits BasePage
    Private Const _gvBProd_countryPos As Integer = 1
    Private Const _itemNamePos As Integer = 0
    Private Const _sqlDetails_param_imagePos As Integer = 5
    Private flagAffError As Boolean = False

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then
            Response.Redirect("default.aspx")
        Else
            If Not flagAffError Then
                lblError.Text = ""
            Else
                lblError.Text = "You must select an Affiliate before adding a component/product"
            End If
            lblCOuntryC.Text = "CurrentCountryCode=" & Session("EBAffEBDistributorCountryCode")
        End If
    End Sub

    'Page Events
    Protected Sub gvOnSale_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        panDetails.Visible = True
        tdLinks.Visible = True
        fvDetails.ChangeMode(FormViewMode.Edit)
    End Sub
    Protected Sub gvOnSale_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        If drpDept.SelectedValue = "%" Then
            gvOnSale.EmptyDataText = ""
        Else
            gvOnSale.EmptyDataText = "No items currently on sale in this department."
        End If
    End Sub
    Protected Sub fvDetails_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblType As Label = fvDetails.FindControl("lblType")
        Dim lblCompOrProdID As Label = fvDetails.FindControl("lblCompOrProdID")
        Dim chkReviews As CheckBox = fvDetails.FindControl("chkReviews")
        Dim gv As GridView
        Select Case LCase(lblType.Text)
            Case "component"
                gv = gvComp
            Case "product"
                gv = gvProd
            Case "bproduct"
                gv = gvBProd
            Case "external"
                gv = gvExt
        End Select
        For Each row As GridViewRow In gv.Rows
            If Not IsDBNull(gv.DataKeys(row.RowIndex).Value) Then
                If LCase(lblType.Text) = "external" Then
                    'The gridviews DataKey is affProductBuyingID for external items
                    If CType(gv.DataKeys(row.RowIndex).Value, Integer) = CType(lblCompOrProdID.Text, Integer) Then gv.SelectedIndex = row.RowIndex
                Else
                    'The gridviws DataKey is ID for all other items
                    If CType(gv.DataKeys(row.RowIndex).Value, Integer) = CType(lblCompOrProdID.Text, Integer) Then gv.SelectedIndex = row.RowIndex
                End If
            End If
        Next
        'Set the visible status of the ReviewLink dropdown/TableRow
        Dim trReviewLink As HtmlTableRow = fvDetails.FindControl("trReviewLink")
        'trReviewLink.Visible = chkReviews.Checked
        trReviewLink.Visible = False
        'If Image Hyperlink if empty (no image exists) then hide the delete button
        Dim lnk As HyperLink = fvDetails.FindControl("lnkSaleImageName")
        Dim lnkBtn As LinkButton = fvDetails.FindControl("lnkDeleteSaleImageName")
        If lnk.Text = "" Then lnkBtn.Visible = False

        lnk = fvDetails.FindControl("lnkSaleDeptImageName")
        lnkBtn = fvDetails.FindControl("lnkDeleteSaleDeptImageName")
        If lnk.Text = "" Then lnkBtn.Visible = False

        lnk = fvDetails.FindControl("lnkSaleImage1Small")
        lnkBtn = fvDetails.FindControl("lnkDeleteSaleImage1Small")
        If lnk.Text = "" Then lnkBtn.Visible = False
        lnk = fvDetails.FindControl("lnkSaleImage1")
        lnkBtn = fvDetails.FindControl("lnkDeleteSaleImage1")
        If lnk.Text = "" Then lnkBtn.Visible = False
        lnk = fvDetails.FindControl("lnkSaleImage2Small")
        lnkBtn = fvDetails.FindControl("lnkDeleteSaleImage2Small")
        If lnk.Text = "" Then lnkBtn.Visible = False
        lnk = fvDetails.FindControl("lnkSaleImage2")
        lnkBtn = fvDetails.FindControl("lnkDeleteSaleImage2")
        If lnk.Text = "" Then lnkBtn.Visible = False
        lnk = fvDetails.FindControl("lnkSaleImage3Small")
        lnkBtn = fvDetails.FindControl("lnkDeleteSaleImage3Small")
        If lnk.Text = "" Then lnkBtn.Visible = False
        lnk = fvDetails.FindControl("lnkSaleImage3")
        lnkBtn = fvDetails.FindControl("lnkDeleteSaleImage3")
        If lnk.Text = "" Then lnkBtn.Visible = False
    End Sub

    'User Events
    Protected Sub gvComp_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim cID As Integer = gvComp.DataKeys(gvComp.SelectedIndex).Value
        Dim name As String = gvComp.SelectedRow.Cells(_itemNamePos).Text
        Dim buyingID As Label = gvComp.SelectedRow.FindControl("lblAffProductBuyingID")
        addOnSale("component", cID, name, buyingID.Text)
        gvProd.SelectedIndex = -1
        gvBProd.SelectedIndex = -1
    End Sub
    Protected Sub gvProd_seletcedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim pID As Integer = gvProd.DataKeys(gvProd.SelectedIndex).Value
        Dim name As String = gvProd.SelectedRow.Cells(_itemNamePos).Text
        Dim buyingID As Label = gvProd.SelectedRow.FindControl("lblAffProductBuyingID")
        addOnSale("product", pID, name, buyingID.Text)
        gvComp.SelectedIndex = -1
        gvBProd.SelectedIndex = -1
    End Sub
    Protected Sub gvExt_seletcedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim pID As Integer = gvExt.DataKeys(gvExt.SelectedIndex).Value
        Dim name As String = gvExt.SelectedRow.Cells(_itemNamePos).Text
        Dim buyingID As Label = gvExt.SelectedRow.FindControl("lblAffProductBuyingID")
        addOnSale("external", pID, name, buyingID.Text)
        gvComp.SelectedIndex = -1
        gvExt.SelectedIndex = -1
    End Sub
    Protected Sub gvBProd_seletcedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim pBID As Integer = gvBProd.DataKeys(gvBProd.SelectedIndex).Value
        Dim name As String = gvBProd.SelectedRow.Cells(_itemNamePos).Text
        Dim buyingID As Label = gvBProd.SelectedRow.FindControl("lblAffProductBuyingID")
        addOnSale("bproduct", pBID, name, buyingID.Text)
        gvProd.SelectedIndex = -1
        gvComp.SelectedIndex = -1
    End Sub
    Protected Sub drpAff_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'gvOnSale.DataBind()
    End Sub
    Protected Sub SqlDetails_updated(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        clearSelected()
    End Sub
    Protected Sub gvOnSale_rowDeleted(ByVal sender As Object, ByVal e As GridViewDeletedEventArgs)
        'When a row is deleted, hide the Details Panel if its been left open, and clear the currenct selection from gvOnSale (And other panels)
        panDetails.Visible = False
        gvOnSale.SelectedIndex = -1
        clearSelected()
    End Sub
    Protected Sub lnkAddLink_click(ByVal sender As Object, ByVal e As EventArgs)
        tblAddLink.Visible = True
    End Sub
    Protected Sub btnAddLink_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("addLink")
        If Page.IsValid Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductMenuInsert", oConn)
            Dim bError As Boolean = False
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@posID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@text", SqlDbType.VarChar, 500))
                .Parameters.Add(New SqlParameter("@link", SqlDbType.VarChar, 500))
                .Parameters("@posID").Value = gvOnSale.DataKeys(gvOnSale.SelectedIndex).Value
                .Parameters("@text").Value = txtDescription.Text
                .Parameters("@link").Value = txtLink.Text
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                lblError.Text = "<font color='red'>An error occured while adding the link.</font>"
                bError = True
                Dim si As New siteInclude
                si.addError("affiliates/shopProducts.aspx.vb", "btnAddLink_click(); " & ex.ToString)
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If Not bError Then
                'Clean up
                txtDescription.Text = ""
                txtLink.Text = ""
                tblAddLink.Visible = False
                gvLinks.DataBind()
            End If
        End If
    End Sub
    Protected Sub chkReviews_checkedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox = CType(sender, CheckBox)
        Dim trReviewLink As HtmlTableRow = fvDetails.FindControl("trReviewLink")
        'trReviewLink.Visible = chk.Checked
        trReviewLink.Visible = False
    End Sub
    Protected Sub btnAdd_click(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub fvDetails_itemUpdating(ByVal sender As Object, ByVal e As FormViewUpdateEventArgs)
        Dim bError As Boolean = False
        Dim hid As HiddenField
        'Save file
        Dim file As FileUpload
        'Main image
        file = fvDetails.FindControl("f1")
        hid = fvDetails.FindControl("hidSaleImageName")
        bError = uploadFile(file, "saleImageName", hid.Value)
        file = fvDetails.FindControl("f1Dept")
        hid = fvDetails.FindControl("hidSaleDeptImageName")
        bError = uploadFile(file, "saleDeptImageName", hid.Value)
        For iLoop As Integer = 1 To 3
            'Thumbnail
            file = fvDetails.FindControl("image" & iLoop & "Small")
            hid = fvDetails.FindControl("hidImage" & iLoop & "Small")
            If Not bError Then bError = uploadFile(file, "saleImage" & iLoop & "Small", hid.Value)
            'Standard image
            file = fvDetails.FindControl("image" & iLoop)
            hid = fvDetails.FindControl("hidImage" & iLoop)
            If Not bError Then bError = uploadFile(file, "saleImage" & iLoop, hid.Value)
        Next
        e.Cancel = bError
    End Sub
    Protected Sub fvDetails_itemUpdated(ByVal sender As Object, ByVal e As FormViewUpdatedEventArgs)
        panDetails.Visible = False
        tdLinks.Visible = False
        gvOnSale.SelectedIndex = -1
        gvOnSale.DataBind()
        clearSelected()
    End Sub
    Protected Sub lnkDelete_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As LinkButton = CType(sender, LinkButton)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("UPDATE productOnSale SET " & lnk.CommandArgument & "=null WHERE [id]=" & fvDetails.DataKey.Value, oConn)
        Try
            oCmd.CommandType = CommandType.Text
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "Could not delete image"
            Dim si As New siteInclude
            si.addError("affiliates/shopProduct.aspx.vb", "lnkDelete_click(commandArg=" & lnk.CommandArgument & ", id=" & fvDetails.DataKey.Value & "); " & ex.ToString)
            si = Nothing
        End Try
        fvDetails.DataBind()
    End Sub

    'Subs
    Protected Sub addOnSale(ByVal tbl As String, ByVal tblID As Integer, ByVal name As String, ByVal distBuyingID As Integer)
        'If drpAff.SelectedValue = "0" Then
        'flagAffError = True
        'clearSelected()
        'Else
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procOnSaleInsert", oConn)
        Dim newID As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@saleRef", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@itemID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@itemType", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@saleName", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@saleCountryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@saleCurrencyCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@affProductBuyingID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
            .Parameters("@saleRef").Value = "TestRef"
            .Parameters("@itemID").Value = tblID
            .Parameters("@itemType").Value = tbl
            .Parameters("@saleName").Value = Server.HtmlDecode(name)
            .Parameters("@saleCountryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@saleCurrencyCode").Value = Session("EBAffCurrencyCode")
            .Parameters("@affProductBuyingID").Value = distBuyingID
            .Parameters("@pk").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            newID = CType(oCmd.Parameters("@pk").Value, Integer)
        Catch ex As Exception
            lblError.Text = "An error has occured."
            lblErrorTxt.Value = ex.Message
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Reset the onSale View to ALL departments, else the item that was just added may not be visible.
        drpDept.SelectedIndex = 0
        gvOnSale.DataBind()
        For Each row As GridViewRow In gvOnSale.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If CType(gvOnSale.DataKeys(row.RowIndex).Value, Integer) = newID Then gvOnSale.SelectedIndex = row.RowIndex
            End If
        Next
        panDetails.Visible = True
        fvDetails.ChangeMode(FormViewMode.Edit)
        fvDetails.DataBind()
        'End If
    End Sub
    Protected Sub clearSelected()
        gvComp.SelectedIndex = -1
        gvProd.SelectedIndex = -1
        gvBProd.SelectedIndex = -1
    End Sub

    'Functions
    Protected Function fStr(ByVal s As String) As String
        Dim firstLetter As String = Left(s, 1)
        Return UCase(firstLetter) & LCase(Right(s, Len(s) - 1))
    End Function
    Protected Function uploadFile(ByRef fileControl As FileUpload, ByVal paramName As String, ByVal hidValue As String) As Boolean
        Dim result As Boolean = False
        If fileControl.HasFile Then
            Try
                fileControl.SaveAs(Server.MapPath("~/images/products/") + fileControl.FileName)
            Catch ex As Exception
                result = True
                Dim lblError As Label = fvDetails.FindControl("lblError")
                lblError.Text = lblError.Text & "An error occured:" & ex.Message & "<br>"
            End Try
        Else
            'Load the image field with the old filename
            SqlDetails.UpdateParameters(paramName).DefaultValue = hidValue
        End If
    End Function
End Class