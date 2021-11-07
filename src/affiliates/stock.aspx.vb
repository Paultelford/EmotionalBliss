Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_stock
    Inherits BasePage
    Protected Const _gvStock_codePos As Integer = 1

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then
            Response.Redirect("default.aspx")
        Else
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add or remove stock
        Dim iAdd As Integer = 0
        Dim iRem As Integer = 0
        Dim addReason As Boolean = False
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateEBDistributorStockInsert", oConn)
        Select Case drpAddRem.SelectedValue
            Case "add"
                iAdd = CType(txtQty.Text, Integer)
            Case "rem"
                iRem = CType(txtQty.Text, Integer)
        End Select
        If Len(Trim(txtReason.Text)) > 0 Then addReason = True
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affProductBuyingID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@actionID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@qtyAdd", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@qtyRemove", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@reason", SqlDbType.VarChar, 2000))
            .Parameters.Add(New SqlParameter("@addReason", SqlDbType.Bit))
            .Parameters("@affProductBuyingID").Value = CType(gvStock.DataKeys(gvStock.SelectedIndex).Value, Integer)
            .Parameters("@affID").Value = Session("EBDistID")
            .Parameters("@actionID").Value = 3
            .Parameters("@qtyAdd").Value = iAdd
            .Parameters("@qtyRemove").Value = iRem
            .Parameters("@reason").Value = txtReason.Text
            .Parameters("@addReason").Value = addReason
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Tidy up by resetting the 2 txt boxes, and remove the selected row from the stock gridview.
        gvStock.SelectedIndex = -1
        txtQty.Text = "0"
        txtReason.Text = ""
        pan1.Visible = False
        gvStock.DataBind()
    End Sub
    Protected Sub gvStock_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        pan1.Visible = True
    End Sub
    Protected Sub gvStock_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblWarehouse As Label
        Dim lblComponent As Label
        Dim lblType As Label
        For Each row As GridViewRow In gvStock.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblWarehouse = row.FindControl("lblWarehouseProductCode")
                lblComponent = row.FindControl("lblComponentCode")
                lblType = row.FindControl("lblType")
                If LCase(lblType.Text = "bproduct") Then
                    row.Cells(_gvStock_codePos).Text = lblWarehouse.Text
                Else
                    row.Cells(_gvStock_codePos).Text = lblComponent.Text
                End If
            End If
        Next
    End Sub
End Class

