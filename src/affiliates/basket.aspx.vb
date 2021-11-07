Imports System.Data
Imports System.Data.SqlClient


Partial Class affiliates_basket
    Inherits BasePage
    Dim lblOrderTotal As Label

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("EBAffID") <> "" Then
            'User is logged in
        Else
            Cache.Remove("EBImageMap" & Session("EBLanguage"))
            Response.Redirect("default.aspx")
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then bindBasket()
        Dim drp As DropDownList = Master.FindControl("drpAffmenu")
        drp.SelectedValue = "order"
    End Sub
    Protected Sub drpProducts_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpProducts.SelectedValue <> "0" Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procAffiliateProductBuyingByIDSelectNOTNULL", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
                .Parameters("@ID").Value = CType(drpProducts.SelectedValue, Integer)
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    addItemToCart(ds.Tables(0).Rows(0))
                Else
                    'Error, product not found (should never happen)
                    lblError.Text = "The product you selected could not be found. Please try again."
                    'Rebind dropdown, which should hopefully remove faulty product
                    drpProducts.DataBind()
                End If
            Catch ex As Exception
                lblError.Text = "An error has occured. Please try again."
                lblErrorMsg.Text = ex.ToString
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
        bindBasket()
    End Sub
    Protected Sub addItemToCart(ByRef rs As datarow)
        Dim weight As Integer = 500
        Profile.EBAffCart.AddItem(rs("affProductBuyingID"), rs("affProductName"), CType(rs("affUnitPrice"), Decimal), 0, CType(rs("affUnitPrice"), Decimal), CType(rs("affTaxRate"), Decimal), CType(rs("distBuyingID"), Integer), weight, "")
        If LCase(Session("EBAffEBDistributor")) = "true" Then
            Profile.EBAffCart.CurrencyCode = rs("affCurrencyCodeBuys")
        Else
            Profile.EBAffCart.CurrencyCode = rs("affCurrencyCode")
        End If
        bindBasket()
        drpProducts.SelectedIndex = 0
    End Sub
    Protected Function calcTotal(ByVal unitPrice As Decimal, ByVal tax As Decimal) As Decimal
        Return FormatNumber(unitPrice * (1 + (tax / 100)), 2)
    End Function
    Protected Function showRowTotal(ByVal unitPrice As Decimal, ByVal tax As Decimal, ByVal Qty As Integer) As Decimal
        Return FormatNumber(unitPrice * (1 + (tax / 100)), 2) * qty
    End Function
    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        updateAll()
    End Sub
    Protected Sub txtQty_updateRow(ByVal sender As Object, ByVal e As EventArgs)
        updateAll()
    End Sub
    Protected Sub updateAll()
        Dim iQty As Integer
        Dim ID As Label
        Dim txt As TextBox
        For Each row As GridViewRow In gvBasket.Rows
            If row.RowType = DataControlRowType.DataRow Then
                txt = row.FindControl("txtQty")
                If IsNumeric(txt.Text) And txt.Text <> "" Then
                    'Ok, value must be integer - update basket qty
                    Try
                        iQty = CType(txt.Text, Integer)
                        ID = row.FindControl("lblAffProductBuyingID")
                        Profile.EBAffCart.UpdateItem(iQty, CType(ID.Text, Integer))
                    Catch ex As Exception
                        lblError.Text = lblError.Text & "An error occured whilest updating basket item.<br>"
                        lblErrorMsg.Text = lblErrorMsg.Text & ex.ToString
                    End Try
                End If
            End If
        Next
        bindBasket()
    End Sub
    Protected Sub btnCheckout_click(ByVal sender As Object, ByVal e As EventArgs)
        Response.Redirect("test.aspx")
    End Sub
    Protected Sub gvBasket_rowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs)
        Profile.EBAffCart.RemoveItem(CType(gvBasket.DataKeys(e.RowIndex).Value, String))
        bindBasket()
        If Profile.EBAffCart.Items.Count = 0 Then btnCheckout.Visible = False
    End Sub
    Protected Sub bindBasket()
        gvBasket.DataSource = Profile.EBAffCart.Items
        gvBasket.DataBind()
        If Profile.EBAffCart.Items.Count > 0 Then
            lblOrderTotal = gvBasket.FooterRow.FindControl("lblOrderTotal")
            'lblOrderTotal.Text = Profile.EBAffCart.TotalEx.ToString 'Was not adding VAT
            lblOrderTotal.Text = addRowTotals()
            btnCheckout.Visible = True
        Else
            btnCheckout.Visible = False
        End If
    End Sub
    Protected Function addRowTotals() As String
        Dim t As Decimal = 0
        Dim lbl As Label
        For Each row As GridViewRow In gvBasket.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lbl = row.FindControl("lblRowTotal")
                t = t + CType(lbl.Text, Decimal)
                'Now format teh rowTotal to 2DP for display purposes
                lbl.Text = CStr(FormatNumber(CType(lbl.Text, Decimal), 2))
            End If
        Next
        Return CType(FormatNumber(t, 2), String)
    End Function
    Protected Sub drpProducts_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        For Each i As ListItem In drp.Items
            i.Text = Replace(i.Text, "&quot;", """")
        Next
    End Sub
    Protected Sub SqlProducts_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        Dim id As Integer = Session("EBDistID")
        If isUserAffiliate() Then id = Session("EBAffID")
        e.Command.Parameters(0).Value = id
    End Sub
    Protected Function isUserAffiliate() As Boolean
        Dim result As Boolean = False
        If Not (Session("EBAffEBDistributor") Or Session("EBAffEBUser")) Then result = True
        Return result
    End Function
    Protected Function getCurrencyCode() As String
        Dim si As New siteInclude
        Dim result As String = ""
        Try
            result = si.getCurrencySignByCurrencyCode(Profile.EBAffCart.CurrencyCode)
        Catch ex As Exception
        End Try
        si = Nothing
        Return result
    End Function
End Class
