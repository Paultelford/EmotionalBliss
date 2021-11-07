Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class affiliates_orders
    Inherits BasePage
    Private Const iMaxLines As Integer = 25

    Protected Sub Page_Load(ByVal Sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        'Assign default orderID
        If Not Page.IsPostBack Then
            txtOrderID.Text = Now.Year.ToString & Now.Month.ToString & Now.Day.ToString & "-" & Now.TimeOfDay.Hours.ToString & Now.TimeOfDay.Minutes.ToString
        End If
    End Sub
    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreLoad

    End Sub
    Protected Sub drpSuppliers_selectedIndexChanged(ByVal Sender As Object, ByVal e As EventArgs)
        'New supplier has been selected, clear all product dropdowns and hide all apart from the 1st
        Dim drp As DropDownList
        Dim txt As TextBox
        Dim tr As HtmlTableRow
        For i As Integer = 1 To iMaxLines
            drp = tblOrder.FindControl("drpProducts" & CType(i, String))
            txt = tblOrder.FindControl("txtQty" & CType(i, String))
            tr = tblOrder.FindControl("tr_drpProducts" & CType(i, String))
            drp.Items.Clear()
            drp.Items.Add(New ListItem("Select...", "0"))
            txt.Text = ""
            If i > 1 Then tr.Visible = False
        Next
    End Sub
    Protected Sub lnkAddRow_click(ByVal Sender As Object, ByVal e As EventArgs)
        If tr_drpProducts24.Visible Then tr_drpProducts25.Visible = True
        If tr_drpProducts23.Visible Then tr_drpProducts24.Visible = True
        If tr_drpProducts22.Visible Then tr_drpProducts23.Visible = True
        If tr_drpProducts21.Visible Then tr_drpProducts22.Visible = True
        If tr_drpProducts20.Visible Then tr_drpProducts21.Visible = True
        If tr_drpProducts19.Visible Then tr_drpProducts20.Visible = True
        If tr_drpProducts18.Visible Then tr_drpProducts19.Visible = True
        If tr_drpProducts17.Visible Then tr_drpProducts18.Visible = True
        If tr_drpProducts16.Visible Then tr_drpProducts17.Visible = True
        If tr_drpProducts15.Visible Then tr_drpProducts16.Visible = True
        If tr_drpProducts14.Visible Then tr_drpProducts15.Visible = True
        If tr_drpProducts13.Visible Then tr_drpProducts14.Visible = True
        If tr_drpProducts12.Visible Then tr_drpProducts13.Visible = True
        If tr_drpProducts11.Visible Then tr_drpProducts12.Visible = True
        If tr_drpProducts10.Visible Then tr_drpProducts11.visible = True
        If tr_drpProducts9.Visible Then tr_drpProducts10.visible = True
        If tr_drpProducts8.Visible Then tr_drpProducts9.visible = True
        If tr_drpProducts7.Visible Then tr_drpProducts8.visible = True
        If tr_drpProducts6.Visible Then tr_drpProducts7.visible = True
        If tr_drpProducts5.Visible Then tr_drpProducts6.visible = True
        If tr_drpProducts4.Visible Then tr_drpProducts5.visible = True
        If tr_drpProducts3.Visible Then tr_drpProducts4.Visible = True
        If tr_drpProducts2.Visible Then tr_drpProducts3.visible = True
        If tr_drpProducts1.Visible Then tr_drpProducts2.Visible = True
    End Sub
    Protected Sub btnSubmit_click(ByVal sener As Object, ByVal e As EventArgs)
        Page.Validate()
        If Not Page.IsValid Then

        Else
            'Save order detials to the database
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procDistributorExtOrderInsert", oConn)
            Dim iOrderPK As Integer
            Dim totalExc As Decimal = getGoodsTotal(False)
            Dim totalInc As Decimal = getGoodsTotal(True)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@orderID", SqlDbType.VarChar, 20))
                .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@supplierID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 10))
                .Parameters.Add(New SqlParameter("@goodsTotalExc", SqlDbType.Decimal))
                .Parameters.Add(New SqlParameter("@goodsTotalInc", SqlDbType.Decimal))
                .Parameters.Add(New SqlParameter("@shipping", SqlDbType.Decimal))
                .Parameters.Add(New SqlParameter("@shippingVatRate", SqlDbType.Decimal))
                .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
                .Parameters("@orderID").Value = txtOrderID.Text & "_" & Session("EBDistID")
                .Parameters("@affID").Value = CType(Session("EBDistID"), Integer)
                .Parameters("@supplierID").Value = CType(drpSuppliers.SelectedValue, Integer)
                .Parameters("@status").Value = "Placed"
                .Parameters("@goodsTotalExc").Value = totalExc
                .Parameters("@goodsTotalInc").Value = totalInc
                .Parameters("@shipping").Value = CType(txtShipping.Text, Decimal)
                .Parameters("@shippingVatRate").Value = CType(txtShippingVat.Text, Decimal)
                .Parameters("@pk").Direction = ParameterDirection.Output
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
                iOrderPK = CType(oCmd.Parameters("@pk").Value, Integer)
            Catch ex As Exception
                siteInclude.addError("affiliates/order.aspx.vb", "btnSubmit(); " & ex.ToString())
                Response.End()
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            'Add detilas to the Distributor Statement
            siteInclude.affAddToStatement(Session("EBDistID"), 0, calcOrderTotal(totalInc, CType(txtShipping.Text, Decimal), CType(txtShippingVat.Text, Decimal)), 0, iOrderPK, 3)
            'Save each Item into the distributorOrderItem table
            Dim drp As DropDownList
            Dim txt As TextBox
            Dim txtUnitPrice As TextBox
            Dim txtTaxRate As TextBox
            For i As Integer = 1 To iMaxLines
                drp = tblOrder.FindControl("drpProducts" & CType(i, String))
                txt = tblOrder.FindControl("txtQty" & CType(i, String))
                txtUnitPrice = tblOrder.FindControl("txtUnitPrice" & CType(i, String))
                txtTaxRate = tblOrder.FindControl("txtTaxRate" & CType(i, String))
                If drp.SelectedValue <> "0" Then
                    'Product has been selected in current dropdown, add to table
                    oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                    oCmd = New SqlCommand("procDistributorExtOrderItemInsert", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@distOrderID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@affProductBuyingID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@unitPrice", SqlDbType.Decimal))
                        .Parameters.Add(New SqlParameter("@taxRate", SqlDbType.Decimal))
                        .Parameters("@distOrderID").Value = iOrderPK
                        .Parameters("@affProductBuyingID").Value = drp.SelectedValue
                        .Parameters("@qty").Value = CType(txt.Text, Integer)
                        .Parameters("@unitPrice").Value = CType(txtUnitPrice.Text, Decimal)
                        .Parameters("@taxRate").Value = CType(txtTaxRate.Text, Decimal)
                    End With
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        siteInclude.addError("affiliates/order.aspx.vb", "btnSubmit(); " & ex.ToString())
                        Response.End()
                    Finally
                        oCmd.Dispose()
                        oConn.Dispose()
                    End Try
                End If
            Next
            'Order complete, all OK
            lblOrderMessage.Text = "Order Complete, click <a href='orders.aspx'>here</a> to place another."
            pan1.Visible = False
        End If
    End Sub
    Protected Sub drpProducts_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        Dim id As Integer = Replace(drp.ID, "drpProducts", "")
        Dim txtUnitPrice As TextBox = tblOrder.FindControl("txtUnitPrice" & id)
        Dim txtTaxRate As TextBox = tblOrder.FindControl("txtTaxRate" & id)
        setPrice(CType(drp.SelectedValue, Integer), txtUnitPrice, txtTaxRate)
    End Sub
    Protected Sub setPrice(ByVal affProdBuyID As Integer, ByRef txtUnitPrice As TextBox, ByRef txtTaxRate As TextBox)
        'Sets Row Lables with a products unit price and tax rate
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateProductBuyingByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = affProdBuyID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                txtUnitPrice.Text = CType(ds.Tables(0).Rows(0)("affUnitPrice"), String)
                txtTaxRate.Text = CType(ds.Tables(0).Rows(0)("affTaxRate"), String)
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
    Protected Function calcOrderTotal(ByVal g As Decimal, ByVal s As Decimal, ByVal sv As Decimal) As Decimal
        Return g + (s * ((sv / 100) + 1))
    End Function
    Function getGoodsTotal(ByVal incVat As Boolean) As Decimal
        'Returns 2 digit total, inc or excl VAT
        Dim drp As DropDownList
        Dim txt As TextBox
        Dim txtUnitPrice As TextBox
        Dim txtTaxRate As TextBox
        Dim Total As Decimal = 0
        For i As Integer = 1 To iMaxLines
            drp = tblOrder.FindControl("drpProducts" & CType(i, String))
            txt = tblOrder.FindControl("txtQty" & CType(i, String))
            txtUnitPrice = tblOrder.FindControl("txtUnitPrice" & CType(i, String))
            txtTaxRate = tblOrder.FindControl("txtTaxRate" & CType(i, String))
            If drp.SelectedValue <> "0" Then
                If incVat Then
                    Total = Total + CType(txtUnitPrice.Text, Decimal) * ((CType(txtTaxRate.Text, Decimal) / 100) + 1)
                Else
                    Total = Total + CType(txtUnitPrice.Text, Decimal)
                End If
            End If
        Next
        Return FormatNumber(Total, 2)
    End Function
End Class
