Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_affiliateProducts
    Inherits System.Web.UI.Page
    Private Const _gvBProd_countryPos As Integer = 1
    Private Const _itemNamePos As Integer = 0
    Private flagAffError As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Not Membership.GetUser Is Nothing Then
            If Not flagAffError Then
                lblError.Text = ""
            Else
                lblError.Text = "You must select an Affiliate before adding a component/product"
            End If
        End If
    End Sub
    Protected Sub gvOnSale_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        panDetails.Visible = True
        fvDetails.ChangeMode(FormViewMode.Edit)
    End Sub
    Protected Sub fvDetails_itemUpdated(ByVal sender As Object, ByVal e As FormViewUpdatedEventArgs)
        panDetails.Visible = False
        gvOnSale.SelectedIndex = -1
        gvOnSale.DataBind()
        clearSelected()
    End Sub
    Protected Sub gvComp_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim cID As Integer = gvComp.DataKeys(gvComp.SelectedIndex).Value
        Dim name As String = gvComp.SelectedRow.Cells(_itemNamePos).Text
        addOnSale("component", cID, name)
        gvProd.SelectedIndex = -1
        gvBProd.SelectedIndex = -1
    End Sub
    Protected Sub gvProd_seletcedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim pID As Integer = gvProd.DataKeys(gvProd.SelectedIndex).Value
        Dim name As String = gvProd.SelectedRow.Cells(_itemNamePos).Text
        addOnSale("product", pID, name)
        gvComp.SelectedIndex = -1
        gvBProd.SelectedIndex = -1
    End Sub
    Protected Sub gvBProd_seletcedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim pBID As Integer = gvBProd.DataKeys(gvBProd.SelectedIndex).Value
        Dim name As String = gvBProd.SelectedRow.Cells(_itemNamePos).Text
        addOnSale("bproduct", pBID, name)
        gvProd.SelectedIndex = -1
        gvComp.SelectedIndex = -1
    End Sub
    Protected Sub fvDetails_dataBound(ByVal sender As Object, ByVal e As EventArgs)
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
            If CType(gv.DataKeys(row.RowIndex).Value, Integer) = CType(lblCompOrProdID.Text, Integer) Then gv.SelectedIndex = row.RowIndex
        Next
    End Sub
    Protected Sub addOnSale(ByVal tbl As String, ByVal tblID As Integer, ByVal name As String)
        If drpAff.SelectedValue = "0" Then
            flagAffError = True
            clearSelected()
        Else
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procAffiliateProductBuyingInsert", oConn)
            Dim newID As Integer = 0
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@affProductID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@affProductType", SqlDbType.VarChar, 30))
                .Parameters.Add(New SqlParameter("@affProductName", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
                .Parameters("@affID").Value = CType(drpAff.SelectedValue, Integer)
                .Parameters("@affProductType").Value = tbl
                .Parameters("@affProductID").Value = tblID
                .Parameters("@affProductName").Value = name
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
        End If
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
    Protected Sub gvOnSale_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        If drpAff.SelectedValue = "0" Then
            gvOnSale.EmptyDataText = ""
        Else
            gvOnSale.EmptyDataText = "No items currently on sale to the selected Affiliate"
        End If
    End Sub
    Protected Sub drpAff_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'gvOnSale.DataBind()
    End Sub
    Protected Sub SqlDetails_updated(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        clearSelected()
    End Sub
End Class
