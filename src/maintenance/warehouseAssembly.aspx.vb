Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class maintenance_warehouseAssembly
    Inherits System.Web.UI.Page
    Private Const _gvList_stockPos = 1
    Private Const _gvList_neededPos = 2
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            'txtQty.Attributes.Add("onKeyUp", "document.getElementById('ctl00$logMaintenance$ContentPlaceHolder1$btnContinue').style.display='none';")
            txtQty.Attributes.Add("onKeyUp", "if(document.getElementById('" & btnContinue.ClientID & "'))document.getElementById('" & btnContinue.ClientID & "').style.display='none';")
            txtQtyConfirm.Attributes.Add("onKeyUp", "if(document.getElementById('" & btnContinue.ClientID & "'))document.getElementById('" & btnContinue.ClientID & "').style.display='none';")
        End If
    End Sub
    Protected Sub gvList_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'loop though table rows and mutiple Needed cell by qty of products to be assembled
        Dim lblStock As Label
        Dim bError As Boolean = False
        For Each row As GridViewRow In gvList.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblStock = row.FindControl("lblStock")
                If IsDBNull(lblStock.Text) Or lblStock.Text = "" Then
                    lblStock.Text = "0"
                Else

                End If
                row.Cells(_gvList_neededPos).Text = Convert.ToInt32(row.Cells(_gvList_neededPos).Text) * Convert.ToInt32(txtQty.Text)
                If Convert.ToInt32(row.Cells(_gvList_neededPos).Text) > Convert.ToInt32(lblStock.Text) Then
                    row.ForeColor = Drawing.Color.Red
                    bError = True
                End If
            End If
        Next
        If bError Then
            lblError.Visible = True
        Else
            lblError.Visible = False
        End If
        If gvList.Rows.Count = 0 Then
            lblError.Text = "There are no compoents/products associated with this boxed product. Goto <a href='warehouseDesign.aspx'>Warehouse Design</a> to add some."
            lblError.Visible = True
        End If
        txtComments.Visible = Not lblError.Visible
        btnContinue.Visible = Not lblError.Visible
        lblComments.Visible = Not lblError.Visible
    End Sub
    Protected Sub drpBProduct_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        pan1.Visible = False
        btnCheckStock.Visible = True
    End Sub
    Protected Sub btnCheckStock_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate()
        If Page.IsValid Then
            pan1.Visible = True
            gvList.DataBind()
        End If
    End Sub
    Protected Sub btnContinue_click(ByVal sender As Object, ByVal e As EventArgs)
        'All checks should be complete, so create Assembly DB entries
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procWarehouseBatchInsert", oConn)
        Dim iBatchID As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@productID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@warehouseComments", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@warehouseUser", SqlDbType.VarChar, 25))
            .Parameters.Add(New SqlParameter("@warehouseAmount", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@warehouseBatchID", SqlDbType.Int))
            .Parameters("@productID").Value = Convert.ToInt32(drpBProduct.SelectedValue)
            .Parameters("@warehouseComments").Value = txtComments.Text
            .Parameters("@warehouseUser").Value = Membership.GetUser.UserName
            .Parameters("@warehouseAmount").Value = Convert.ToInt32(txtQty.Text)
            .Parameters("@warehouseBatchID").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            iBatchID = oCmd.Parameters("@warehouseBatchID").Value
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Add to productHistory - 
        'Get recordset of all componetns/products used, and remove them from stock and change to In Production.
        Dim eb As New siteInclude
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procWarehouseBoxContentByBProductIDSelect", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@warehouseProductID", SqlDbType.Int))
            .Parameters("@warehouseProductID").Value = Convert.ToInt32(drpBProduct.SelectedValue)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    If row("contentIsProduct") Then
                        eb.addToProductHistory(Convert.ToInt32(row("contentProductID")), 0, Convert.ToInt32(txtQty.Text) * row("contentQty"), 0, 0, 3, 0, "Warehouse Assembly In Progress", Membership.GetUser.UserName, False, iBatchID, Convert.ToInt32(txtQty.Text) * row("contentQty"), 0)
                    Else
                        eb.addToComponentHistory(0, 0, 0, 0, Convert.ToInt32(txtQty.Text) * row("contentQty"), 0, 0, 0, 0, 10, 0, Convert.ToInt32(row("contentComponentID")), "Being Boxed/In Production", Membership.GetUser.UserName, False, 0, False, iBatchID)
                    End If
                Next
            End If
            eb.addToWarehouseHistory(Convert.ToInt32(drpBProduct.SelectedValue), 0, 0, 2, "", Membership.GetUser.UserName, 0, Convert.ToInt32(txtQty.Text), 0, iBatchID)
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        pan1.Visible = False
        lblComplete.Text = "Batch <b>" & iBatchID & "</b> was successfully created."
        btnCheckStock.Visible = False
        ScriptManager.RegisterStartupScript(update1, Me.GetType(), "test", "window.open('warehouseAssemblyPDF.aspx?id=" & iBatchID & "&createPDF=true','warehouseAssPop','toolbars=none');", True)
    End Sub
    Protected Sub drpCountry_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        drpBProduct.Items.Clear()
        drpBProduct.Items.Add(New ListItem("Select...", "0"))
    End Sub
End Class
