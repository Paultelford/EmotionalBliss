Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_onSale
    Inherits System.Web.UI.Page
    Private Const _gvBProd_countryPos As Integer = 1

    Protected Sub gvOnSale_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        panDetails.Visible = True
        fvDetails.ChangeMode(FormViewMode.Edit)
        gvOnSale.SelectedRow.BackColor = Drawing.Color.LightPink
    End Sub
    Protected Sub fvDetails_itemUpdated(ByVal sender As Object, ByVal e As FormViewUpdatedEventArgs)
        panDetails.Visible = False
        gvOnSale.SelectedIndex = -1
        gvOnSale.DataBind()
        clearSelected()
    End Sub
    Protected Sub gvComp_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim cID As Integer = gvComp.DataKeys(gvComp.SelectedIndex).Value
        addOnSale("component", cID, " ")
        gvComp.SelectedRow.BackColor = Drawing.Color.LightPink
    End Sub
    Protected Sub gvProd_seletcedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim pID As Integer = gvProd.DataKeys(gvProd.SelectedIndex).Value
        addOnSale("product", pID, " ")
        gvProd.SelectedRow.BackColor = Drawing.Color.LightPink
    End Sub
    Protected Sub gvBProd_seletcedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim pBID As Integer = gvProd.DataKeys(gvBProd.SelectedIndex).Value
        addOnSale("bproduct", pBID, gvProd.Rows(gvProd.SelectedIndex).Cells(_gvBProd_countryPos).Text)
        gvBProd.SelectedRow.BackColor = Drawing.Color.LightPink
    End Sub
    Protected Sub fvDetails_dateBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblType As Label = fvDetails.FindControl("lblType")
        Dim lblCompOrProdID As Label = fvDetails.FindControl("lblCompOrProdID")
        Dim gv As GridView
        Select Case LCase(lblType.Text)
            Case "component"
                gv = gvComp
            Case "product"
                gv = gvProd
            Case "bproduct"
                gv = gvBProd
        End Select
        For Each row As GridViewRow In gv.Rows
            If CType(gv.DataKeys(row.RowIndex).Value, Integer) = CType(lblCompOrProdID.text, Integer) Then gv.SelectedIndex = row.RowIndex
        Next
        gv.SelectedRow.BackColor = Drawing.Color.LightPink
    End Sub
    Protected Sub addOnSale(ByVal tbl As String, ByVal tblID As Integer, ByVal countryCode As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductOnSaleInsert", oConn)
        Dim newID As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@saleRef", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@itemID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@itemType", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@saleCountryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
            .Parameters("@saleRef").Value = "TestRef"
            .Parameters("@itemID").Value = tblID
            .Parameters("@itemType").Value = tbl
            If countryCode <> "" Then
                .Parameters("@saleCountryCode").Value = countryCode
            Else
                .Parameters("@saleCountryCode").Value = DBNull.Value
            End If
            .Parameters("@pk").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            newID = CType(oCmd.Parameters("@pk").Value, Integer)
        Catch ex As Exception
            lblError.Text = ex.Message
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        gvOnSale.DataBind()
        For Each row As GridViewRow In gvOnSale.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If CType(gvOnSale.DataKeys(row.RowIndex).Value, Integer) = newID Then gvOnSale.SelectedIndex = row.RowIndex
            End If
        Next
        panDetails.Visible = True
        fvDetails.ChangeMode(FormViewMode.Edit)
        fvDetails.DataBind()
    End Sub
    Protected Function fStr(ByVal s As String) As String
        Dim firstLetter As String = Left(s, 1)
        Return UCase(firstLetter) & LCase(Right(s, Len(s) - 1))
    End Function
    Protected Sub clearSelected()
        gvComp.SelectedIndex = -1
        gvProd.SelectedIndex = -1
        gvBProd.SelectedIndex = -1
    End Sub
End Class
