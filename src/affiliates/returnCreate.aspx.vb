Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_returnCreate
    Inherits BasePage
    Private Const _gvItems_namePos As Integer = 0
    Private Const _gvItems_qtyPos As Integer = 2
    Private Const _gvItems_checkPos As Integer = 3
    Private Const _gvItems_returnQtyPos As Integer = 4
    Private Const _gvItems_errorMsgPos As Integer = 5

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            'Set country code from session
            lblCountryCode.Text = UCase(Session("EBAffEBDistributorCountryCode"))
        End If
    End Sub
    '#################### PANEL 1
    Protected Sub btnOrderIDSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        pan1Submit()
    End Sub
    Protected Sub txtPan1UserOrderNo_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("pan1")
        If Page.IsValid Then pan1Submit()
    End Sub

    Protected Sub pan1Submit()
        'User has entered an order number, lookup in the db and find the proper orderID and order details and ask user to confirm their selection.
        trPan1OrderDetails.Visible = False
        lblPan1OrderNotFound.Visible = False
        btnPan1Procede.Visible = True
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByNumCountrySelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderNo", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@orderCountryCode", SqlDbType.VarChar, 5))
            .Parameters("@orderNo").Value = txtPan1UserOrderNo.Text
            .Parameters("@orderCountryCode").Value = Session("EBAffEBDistributorCountryCode")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If LCase(ds.Tables(0).Rows(0)("orderStatus")) <> "complete" Then
                    lblPan1OrderNotFound.Text = "That order is not yet complete.<br>"
                    lblPan1OrderNotFound.Visible = True
                    btnPan1Procede.Visible = False
                Else
                    hidOrderID.Value = ds.Tables(0).Rows(0)("id")
                    trPan1OrderDetails.Visible = True
                    lblPan1UserOrderID.Text = ds.Tables(0).Rows(0)("userOrderID") 'Show Full order ID
                    showAddress("bill", ds.Tables(0).Rows(0))
                    showAddress("ship", ds.Tables(0).Rows(0))
                    bindOrderItems(ds.Tables(0).Rows(0)("id"))
                End If
            Else
                lblPan1OrderNotFound.Text = "The order number does not exist<br>"
                lblPan1OrderNotFound.Visible = True
            End If
        Catch ex As Exception
            lblError.Text = "<font color='red'>An error occured while getting order details.</font>"
            Dim si As New siteInclude
            si.addError("affiliates/returnCreate.aspx.vb", "pan1Submit(txtPan1UserOrderNo=" & txtPan1UserOrderNo.Text & ", countrycode=" & Session("EBAffEBDistributorCountryCode") & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnPan1Proceed_click(ByVal sender As Object, ByVal e As EventArgs)
        'User has confirmed that selected order ID is correct. The orderID has been placed in hidOrderID for later use.
        'Hide panel 1 and show panel 2
        pan1EB.Visible = False
        pan1Ext.Visible = False
        pan1Add.Visible = True
        'Populate customers shipping address

        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopCustomerShipAddByUserOrderNumSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim dr As DataRow
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@userOrderNum", SqlDbType.Int))
        oCmd.Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
        oCmd.Parameters("@userOrderNum").Value = txtPan1UserOrderNo.Text
        oCmd.Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'Fill table
                dr = ds.Tables(0).Rows(0)
                txtName.Text = dr("shipname")
                txtAdd1.Text = dr("shipadd1")
                txtAdd2.Text = dr("shipadd2")
                txtAdd3.Text = dr("shipadd3")
                txtAdd4.Text = dr("shipadd4")
                If Trim(txtAdd4.Text) = "," Then txtAdd4.Text = ""
                txtPostcode.Text = dr("shipPostcode")
                Try
                    drpCountry.SelectedItem.Text = dr("shipCountry")
                Catch ex As Exception
                    'Do nothing, as country was set to current distributors country when the dropdown loaded. This should be right 99% of the time
                End Try
                txtEmail.Text = dr("email")
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
    Protected Sub btnShopSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        pan1Ext.Visible = False
        pan1Add.Visible = True
    End Sub
    '#################### PANEL 2
    Protected Sub btnPan2AddItem_click(ByVal sender As Object, ByVal e As EventArgs)
        gvPan2ProductList.Visible = True
    End Sub
    Protected Sub drpCountry_Load(ByVal sender As Object, ByVal e As EventArgs)
        drpCountry.SelectedValue = Session("EBAffEBDistributorCountryCode")
    End Sub
    Protected Sub btnPan1AddSubmit_Click(ByVal sender As Object, ByVal e As EventArgs)
        'Populate the 'spnAddressTarget' span field in 'pan3' with customers address.
        'lblAdd.Text = txtName.Text & "<br>"
        'If txtAdd1.Text <> "" Then lblAdd.Text = lblAdd.Text & txtAdd1.Text & "<br>"
        'If txtAdd2.Text <> "" Then lblAdd.Text = lblAdd.Text & txtAdd2.Text & "<br>"
        'If txtAdd3.Text <> "" Then lblAdd.Text = lblAdd.Text & txtAdd3.Text & "<br>"
        'If txtAdd4.Text <> "" Then lblAdd.Text = lblAdd.Text & txtAdd4.Text & "<br>"
        'If txtPostcode.Text <> "" Then lblAdd.Text = lblAdd.Text & txtPostcode.Text & "<br>"
        'lblAdd.Text = lblAdd.Text & drpCountry.SelectedItem.Text & "<br>"
        pan1Add.Visible = False
        pan3.Visible = True
        Dim iReturnsNo As Integer = 0
        'Create return and show return number
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procReturnInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add4", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@postcode", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@phone", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@shop", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@distributorID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@itemsReturned", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@returnReason", SqlDbType.VarChar, 4000))
            .Parameters.Add(New SqlParameter("@purchasePlace", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@returnsID", SqlDbType.Int))
            .Parameters("@name").Value = txtName.Text
            .Parameters("@add1").Value = txtAdd1.Text
            .Parameters("@add2").Value = txtAdd2.Text
            .Parameters("@add3").Value = txtAdd3.Text
            .Parameters("@add4").Value = txtAdd4.Text
            .Parameters("@postcode").Value = txtPostcode.Text
            .Parameters("@countryCode").Value = drpCountry.SelectedValue
            .Parameters("@email").Value = txtEmail.Text
            .Parameters("@phone").Value = txtPhone.Text
            If hidOrderID.Value = "" Then
                .Parameters("@orderID").Value = 0
                .Parameters("@affID").Value = drpAffiliate.SelectedValue
            Else
                .Parameters("@orderID").Value = hidOrderID.Value
                .Parameters("@affID").Value = 0
            End If
            .Parameters("@shop").Value = txtShop.Text
            .Parameters("@distributorID").Value = 0
            .Parameters("@itemsReturned").Value = txtItemsReturned.Text
            .Parameters("@returnReason").Value = txtReason.Text
            .Parameters("@purchasePlace").Value = radPurchase.SelectedValue
            If txtItemsReturned.Text = "" And txtReason.Text = "" Then
                'Must have used the other none eb form, grab values from there instead
                .Parameters("@itemsReturned").Value = txtItemsReturned2.Text
                .Parameters("@returnReason").Value = txtReason2.Text
            End If
            .Parameters("@returnsID").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            iReturnsNo = oCmd.Parameters("@returnsID").Value
            'Add the returns number to the order log (If eb order)
            If radPurchase.SelectedValue = "eb" Then
                Dim si As New siteInclude
                si.AddToOrderLog(hidOrderID.Value, "Return <a href='returnFind.aspx?id=" & iReturnsNo & "'>" & iReturnsNo & "</a> created.", Membership.GetUser.UserName)
                si = Nothing
            End If
            lnkReturnsNo.Text = iReturnsNo
            lnkReturnsNo.NavigateUrl = "returnsOutstanding.aspx?id=" & iReturnsNo
        Catch ex As Exception
            For Each c As SqlParameter In oCmd.Parameters
                If Not IsDBNull(c.Value) Then Response.Write(c.XmlSchemaCollectionName & ": " & c.Value & c.Size & "-" & Len(CStr(c.Value)) & "<br>")
            Next
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnPan2Proceed_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox
        Dim txt As TextBox
        Dim iQty As Integer
        Dim bError As Boolean = False
        Dim bItemAdded As Boolean = False
        For Each row As GridViewRow In gvPan2Items.Rows
            If row.RowType = DataControlRowType.DataRow Then
                iQty = CInt(row.Cells(_gvItems_qtyPos).Text)
                chk = row.FindControl("chk")
                txt = row.FindControl("txtQtyReturn")
                If chk.Checked Then
                    'User selected this row, make sure the data entered is valid
                    If txt.Text = "" Or Not IsNumeric(txt.Text) Then
                        'Invalid or null entry
                        row.Cells(_gvItems_errorMsgPos).Visible = True
                        Response.Write("Set error visible")
                        bError = True
                    Else
                        bItemAdded = True
                    End If
                End If
            End If
        Next
        'If valid entry, then proceed to Panel 3
        If bItemAdded And Not bError Then
            pan2.Visible = False
            pan3.Visible = True
        Else

        End If
    End Sub
    Protected Sub gvPan2ProductList_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim tr As New TableRow
        Dim td As New TableCell
        Dim chk As CheckBox
        Dim txt As TextBox
        td.Text = "TestItem1"
        tr.Cells.Add(td)
        td = New TableCell
        td.Text = "&nbsp;"
        tr.Cells.Add(td)
        td = New TableCell
        td.Text = "4"
        tr.Cells.Add(td)
        td = New TableCell
        chk = New CheckBox
        td.Controls.Add(chk)
        tr.Cells.Add(td)
        td = New TableCell
        txt = New TextBox
        txt.Width = "40"
        td.Controls.Add(txt)
        tr.Cells.Add(td)
        tblPan2Items.Rows.Add(tr)
    End Sub


    '############
    Protected Sub showAddress(ByVal type As String, ByRef row As DataRow)
        'Type will be 'bill' or 'ship', row contains the customers address details
        'Function will show address from shopCustomer and affilaite tables, as customer can never be both, correct address should show.
        Dim lbl As Label = pan1EB.FindControl("lblPan1" & type & "Add")
        Dim affType As String = ""
        Select Case type
            Case "bill"
                affType = "aff"
            Case "ship"
                affType = "affTo"
        End Select
        lbl.Text = ""
        'Shop orders
        If Not IsDBNull(row(type & "Name")) Then If Not row(type & "Name") = "" Then lbl.Text = lbl.Text & row(type & "Name") & "<br>"
        If Not IsDBNull(row(type & "Add1")) Then If Not row(type & "Add1") = "" Then lbl.Text = lbl.Text & row(type & "Add1") & "<br>"
        If Not IsDBNull(row(type & "Add2")) Then If Not row(type & "Add2") = "" Then lbl.Text = lbl.Text & row(type & "Add2") & "<br>"
        If Not IsDBNull(row(type & "Add3")) Then If Not row(type & "Add3") = "" Then lbl.Text = lbl.Text & row(type & "Add3") & "<br>"
        If Not IsDBNull(row(type & "Add4")) Then If Not row(type & "Add4") = "" Then lbl.Text = lbl.Text & row(type & "Add4") & "<br>"
        If Not IsDBNull(row(type & "Add5")) Then If Not row(type & "Add5") = "" Then lbl.Text = lbl.Text & row(type & "Add5") & "<br>"
        If Not IsDBNull(row(type & "Postcode")) Then If Not row(type & "Postcode") = "" Then lbl.Text = lbl.Text & row(type & "Postcode")
        'Affiliate orders
        If Not IsDBNull(row(affType & "Name")) Then If Not row(affType & "Name") = "" Then lbl.Text = lbl.Text & row(affType & "Name") & "<br>"
        If Not IsDBNull(row(affType & "Add1")) Then If Not row(affType & "Add1") = "" Then lbl.Text = lbl.Text & row(affType & "Add1") & "<br>"
        If Not IsDBNull(row(affType & "Add2")) Then If Not row(affType & "Add2") = "" Then lbl.Text = lbl.Text & row(affType & "Add2") & "<br>"
        If Not IsDBNull(row(affType & "Add3")) Then If Not row(affType & "Add3") = "" Then lbl.Text = lbl.Text & row(affType & "Add3") & "<br>"
        If Not IsDBNull(row(affType & "Add4")) Then If Not row(affType & "Add4") = "" Then lbl.Text = lbl.Text & row(affType & "Add4") & "<br>"
        If Not IsDBNull(row(affType & "Add5")) Then If Not row(affType & "Add5") = "" Then lbl.Text = lbl.Text & row(affType & "Add5") & "<br>"
        If Not IsDBNull(row(affType & "Postcode")) Then If Not row(affType & "Postcode") = "" Then lbl.Text = lbl.Text & row(affType & "Postcode")
    End Sub
    Protected Sub bindOrderItems(ByVal oID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderItemByIDSelect2", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = oID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            gvPan1Items.DataSource = ds
            gvPan1Items.DataBind()
            gvPan2Items.DataSource = ds
            gvPan2Items.DataBind()
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
    Protected Sub testTable()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderItemByIDSelect2", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim tr As New TableRow
        Dim td As New TableCell
        Dim chk As CheckBox
        Dim txt As TextBox
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = hidOrderID.Value
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    tr = New TableRow
                    td = New TableCell
                    td.Text = row("itemName")
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = "&nbsp;"
                    td.Width = "40"
                    tr.Cells.Add(td)
                    td = New TableCell
                    td.Text = row("itemQty")
                    tr.Cells.Add(td)
                    td = New TableCell
                    chk = New CheckBox
                    td.Controls.Add(chk)
                    tr.Cells.Add(td)
                    td = New TableCell
                    txt = New TextBox
                    txt.Width = "40"
                    td.Controls.Add(txt)
                    tr.Cells.Add(td)
                    tblPan2Items.Rows.Add(tr)
                Next
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
    Protected Sub radPurchase_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim rad As RadioButtonList = CType(sender, RadioButtonList)
        pan0.Visible = False
        If rad.SelectedValue = "eb" Then
            pan1EB.visible = True
        Else
            pan1Ext.visible = True
        End If
    End Sub
End Class
