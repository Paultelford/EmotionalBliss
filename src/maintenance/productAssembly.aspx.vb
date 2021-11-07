Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_productAssembly
    Inherits System.Web.UI.Page
    Private Const _stockPos = 1
    Private Const _defaultPos = 2
    Private Const _qtyPos = 3
    Private Const _errPos = 4
    Private Const _view2_gvComponents_QtyPos = 3
    Private Const _view2_gvComponents_stockPos = 1
    Private Const _view2_gvComponents_ComponentPos = 0
    Private Const _view2_gvComponents_ManufacturerPos = 2
    Private Const _view2_gvMasters_QtyPos = 1
    Private Const _view2_gvMasters_DefaultPos = 2
    Private Const _view2_gvMasters_MsgLnkPos = 3
    Private Const _view2_gvMasters_DefaultIDPos = 4
    Private Const _hash = "ebMaintenanceComponentList"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Session(_hash) = New Hashtable

        Else
            drpProduct.Attributes.Add("onChange", "hideContinueButton()")
            txtQty.Attributes.Add("onKeyUp", "hideContinueButton()")
            txtQtyConfirm.Attributes.Add("onKeyUp", "hideContinueButton()")
            'Response.Write(btnContinue.ClientID)
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate()
        If Page.IsValid Then
            gvComponentList.Visible = True
            tblComments.Visible = True
            btnContinue.Visible = True
        End If
    End Sub
    Protected Sub gvComponentList_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'clear err msg
        lblError.Text = ""
        Dim buildQty As Integer = txtQty.Text
        Dim bStockError As Boolean = False
        Dim lblStock As Label
        Dim lblDefault As Label
        'Test for empty table error
        If gvComponentList.Rows.Count = 0 Then
            lblError.Text = "There are no components associated with this product."
            bStockError = True
        End If
        'check qty is valid
        If Not IsNumeric(buildQty) Then
            'Error
            lblError.Text = "You must enter a valid qty."
        End If
        'calulate Qty Needed amounts (and warn user if there are not enough components)

        For Each row As GridViewRow In gvComponentList.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblStock = row.Cells(_stockPos).FindControl("lblStock")
                lblDefault = row.Cells(_defaultPos).FindControl("lblDefault")
                If String.IsNullOrEmpty(lblStock.Text) Then lblStock.Text = "0"
                If String.IsNullOrEmpty(lblDefault.Text) Then lblDefault.Text = "0"
                row.Cells(_qtyPos).Text = buildQty * Convert.ToInt32(row.Cells(_qtyPos).Text)
                If Convert.ToInt32(lblDefault.Text) < Convert.ToInt32(row.Cells(_qtyPos).Text) Then
                    row.Cells(_errPos).Text = "<font color='blue'><nobr>Low on Default!!!</nobr></font>"
                End If
                If Convert.ToInt32(lblStock.Text) < Convert.ToInt32(row.Cells(_qtyPos).Text) Then
                    row.Cells(_errPos).Text = "<font color='red'><nobr>Low Stock!!!</nobr></font>"
                    bStockError = True
                End If
            End If
        Next

        If bStockError Then btnContinue.Visible = False
    End Sub
    Protected Sub gvMasters_rowDatabound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim row As GridViewRow = e.Row
        Dim lbl As Label
        Dim lblID As Label
        If row.RowType = DataControlRowType.DataRow Then
            lbl = row.FindControl("lblDefault")
            lblID = row.FindControl("lblDefaultID")
            'Check that there are enough default components. 
            Try

            
                If Convert.ToInt32(row.Cells(_view2_gvMasters_QtyPos).Text) <= Convert.ToInt32(lbl.Text) Then
                    row.Cells(_view2_gvMasters_MsgLnkPos).Text = "Default ok"
                    Try
                        'Enough default components are available. Add them to the final list or compoents
                        Session(_hash).add(lblID.Text, row.Cells(_view2_gvMasters_QtyPos).Text)
                        'Hide row
                        row.Visible = False
                    Catch ex As Exception
                        'Duplicate found, No need to do anything as it will not be added twice.
                    End Try
                End If
            Catch ex As Exception
                Response.Write("Error occured, click <a href='productDefault.aspx?pid=" & lblProdID.Text & "'>here</a> to make sure that this product has had all its Default Components set.<br><br>")
                Response.Write("lbl.Text='" & lbl.Text & "'")
                Response.End()
            End Try
        End If
        'testGridView()
    End Sub
    Protected Sub gvMasters_rowDataboundOLD(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim row As GridViewRow = e.Row
        Dim removeLink As Boolean = False
        Dim defaultName As String = String.Empty
        If row.RowType = DataControlRowType.DataRow Then
            Dim lbl As Label = row.FindControl("lblDefault")
            If lbl.Text <> "" Then
                'cell(_view2_gvMasters_DefaultPos) contains the default components ID. Use it to retrieve that components stock Qty.
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procComponentBatchByCompIDStockSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@compID", SqlDbType.Int))
                    .Parameters("@compID").Value = Convert.ToInt32(lbl.Text)
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        If Not IsDBNull(ds.Tables(0).Rows(0)("qty")) Then
                            If Convert.ToInt32(row.Cells(_view2_gvMasters_QtyPos).Text) > Convert.ToInt32(ds.Tables(0).Rows(0)("qty")) Then
                                defaultName = ds.Tables(0).Rows(0)("componentName")
                                removeLink = True
                            End If
                        End If
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
                If removeLink Then
                    'Remove the 'click to set' link and replace it with the default component name
                    row.Cells(_view2_gvMasters_MsgLnkPos).Text = defaultName
                End If
            End If
        End If
    End Sub
    Protected Sub btnContinue_click(ByVal sender As Object, ByVal e As EventArgs)
        If mvBuild.ActiveViewIndex = 0 Then
            'Copy product ID and Qty to hidden lbl fields in the form
            lblProdID.Text = drpProduct.SelectedValue
            lblQty.Text = txtQty.Text
            lblProduct.Text = drpProduct.SelectedItem.ToString
            lblProductQty.Text = txtQty.Text
            lblRef.Text = getRef(Convert.ToInt32(lblProdID.Text))
        End If
        mvBuild.ActiveViewIndex = mvBuild.ActiveViewIndex + 1
        If mvBuild.ActiveViewIndex = 1 Then testGridView()
        btnContinue.Visible = False
    End Sub
    Protected Sub gvMasters_selectedIndexChanged(ByVal sender As Object, ByVal ByVale As EventArgs)
        Dim gv As GridView = viewSection2.FindControl("gvComponents")
        gv.Visible = True
    End Sub
    Protected Sub gvComponents_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If gvComponents.Rows.Count > 0 Then
            Dim row As GridViewRow = gvMasters.SelectedRow
            Dim footerRow As GridViewRow = gvComponents.FooterRow
            'If e.Row.RowType = DataControlRowType.Footer Then
            'e.Row.Cells(_view2_gvComponents_QtyPos).Text = row.Cells(_view2_gvMasters_QtyPos).Text
            'End If
            'Dim lbl As Label = gvComponents.FooterRow.Cells(_view2_gvComponents_QtyPos).FindControl("lblTotalQty")
            Dim lbl As Label = footerRow.FindControl("lblTotalQty")
            lbl.Text = row.Cells(_view2_gvMasters_QtyPos).Text
        End If
    End Sub
    Protected Sub btnView2Submit_click(ByVal sender As Object, ByVal e As EventArgs)
        lblError.Text = "" 'Clear error message
        Dim iTotal As Integer = 0
        Dim txtQty As TextBox
        Dim footerRow As GridViewRow = gvComponents.FooterRow
        Dim lblTotalQty As Label = footerRow.FindControl("lblTotalQty")
        For Each row As GridViewRow In gvComponents.Rows 'Add up all rows
            If row.RowType = DataControlRowType.DataRow Then 'Datarows only
                txtQty = row.FindControl("txtQty")
                If Not IsNumeric(txtQty.Text) Or txtQty.Text = "" Then txtQty.Text = "0" 'If invalid or null entry then set to 0
                iTotal = iTotal + Convert.ToInt32(txtQty.Text)
            End If
        Next
        If Convert.ToInt32(lblTotalQty.Text) > iTotal Then
            lblError.Text = "You have not used enough products."
        Else
            If Convert.ToInt32(lblTotalQty.Text) < iTotal Then
                lblError.Text = "You are trying to use too many products."
            Else
                'User has entered correct quantities. 
                'Check that they have not used more of 1 component than there is in stock
                Dim bAllocationError As Boolean = False
                For Each row As GridViewRow In gvComponents.Rows
                    If row.RowType = DataControlRowType.DataRow Then 'Datarows only
                        txtQty = row.FindControl("txtQty")
                        If Convert.ToInt32(txtQty.Text) > Convert.ToInt32(row.Cells(_view2_gvComponents_stockPos).Text) Then
                            bAllocationError = True
                            lblError.Text = "You have used more '" & row.Cells(_view2_gvComponents_ComponentPos).Text & "'(" & row.Cells(_view2_gvComponents_ManufacturerPos).Text & ") than are available."
                        End If
                    End If
                Next
                'If all ok, then store info in session based hashtable (cid,qty)
                If Not bAllocationError Then
                    Dim cID As Integer
                    For Each row As GridViewRow In gvComponents.Rows
                        If row.RowType = DataControlRowType.DataRow Then
                            txtQty = row.FindControl("txtQty")
                            cID = gvComponents.DataKeys(row.RowIndex).Value
                            If txtQty.Text <> "0" Then
                                Try
                                    Session(_hash).add(cID, txtQty.Text)
                                Catch ex As Exception

                                End Try

                            End If
                        End If
                    Next
                    'Set gvMasters table to show the current type has been set
                    gvMasters.SelectedRow.Visible = False
                    'Run test to see if all datarows are invisible, if so work here is complete.
                    testGridView()
                End If
            End If
        End If
    End Sub
    Protected Sub testGridView()
        Dim bVisibleRow As Boolean = False
        For Each row As GridViewRow In gvMasters.Rows
            If row.RowType = DataControlRowType.DataRow Then If row.Visible Then bVisibleRow = True
        Next
        'Hide gvComponent
        gvComponents.Visible = False
        If Not bVisibleRow Then

            If mvBuild.ActiveViewIndex = 1 Then
                'Production is complete
                Dim productBatchID As Integer = finalizeOrder
                makeComponentTable(productBatchID)
                mvBuild.ActiveViewIndex = mvBuild.ActiveViewIndex + 1
            End If

        End If
    End Sub
    Protected Function finalizeOrder()
        Dim result As Integer
        Dim oConn As SqlConnection
        Dim oCmd As SqlCommand
        'Add to productBatch table
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procProductionBatchInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@prodID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@prodStartDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@prodAmount", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@produser", SqlDbType.VarChar, 25))
            .Parameters.Add(New SqlParameter("@batchID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@comments", SqlDbType.VarChar, 500))
            .Parameters("@prodID").Value = Convert.ToInt32(lblProdID.Text)
            .Parameters("@prodStartDate").Value = Now()
            .Parameters("@prodAmount").Value = Convert.ToInt32(lblProductQty.Text)
            .Parameters("@prodUser").Value = Membership.GetUser.UserName
            .Parameters("@comments").Value = txtComments.Text
            .Parameters("@batchID").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            result = oCmd.Parameters("@batchID").Value
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Loop through collection of products and add the removeals from componentBatch
        For Each item As DictionaryEntry In Session(_hash)
            Dim eb As New siteInclude
            eb.addToComponentHistory(0, 0, 0, 0, Convert.ToInt32(item.Value), 0, 0, 0, 0, 3, 0, Convert.ToInt32(item.Key), "Used for Product Assembly", Membership.GetUser.UserName, False, result)
        Next
        'Show printable popup.  Popup will also create and save a PDF file.
        'Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "openPopup", "openPrintPopup(" & result & ")")
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "openPopup", "window.open('productAssemblyPDF.aspx?id=" & result & "&createPDF=true','pdfpop','toolbars=none');", True)
        lnkPopup.NavigateURL = "productAssemblyPDF.aspx?createPDF=true&id=" & result
        Return result
    End Function
    Protected Sub makeComponentTable(ByVal productionAssemblyBatchID)
        Dim tbl As Table = tblComponentList
        Dim tRow As New TableRow
        Dim tCell As New TableCell
        Dim oConn As SqlConnection
        Dim oCmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim rs As DataRow
        lblProductionBatchID.text = productionAssemblyBatchID
        For Each item As DictionaryEntry In Session(_hash)
            oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            oCmd = New SqlCommand("procComponentByIDSelect2", oConn)
            ds = New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@compID", SqlDbType.Int))
                .Parameters("@compID").Value = Convert.ToInt32(item.Key)
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    rs = ds.Tables(0).Rows(0)
                    tCell = New TableCell
                    tRow = New TableRow
                    tCell.Text = rs("componentName")
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    tCell.Text = rs("manufacturerName")
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    If Not IsDBNull(rs("locationBay")) Then tCell.Text = rs("locationBay")
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    tCell.Text = item.Value
                    tRow.Cells.Add(tCell)
                    tbl.Rows.Add(tRow)
                    rs = Nothing
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
        Next
        'Add comments to txtCom box
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procProductionBatchByIDCommentsSelect", oConn)
        da = New SqlDataAdapter
        ds = New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@assemblyID", SqlDbType.Int))
            .Parameters("@assemblyID").Value = productionAssemblyBatchID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblComments.Text = ds.Tables(0).Rows(0)("productionComments")
            Else
                lblComments.Text = ""
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
    End Sub
    Protected Function getRef(ByVal pid As Integer) As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductByIdSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = ""
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@productID", SqlDbType.Int))
            .Parameters("@productID").Value = pid
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(ds.Tables(0).Rows(0)("ref")) Then
                    result = ds.Tables(0).Rows(0)("ref")
                End If
            Else
                result = ""
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
        Return result
    End Function
    Protected Function replaceNull(ByVal obj As Object) As String
        Dim result As String
        If IsDBNull(obj) Then
            result = "0"
        Else
            result = obj.ToString
        End If
        Return result
    End Function
End Class
