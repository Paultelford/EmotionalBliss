Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_warehouseDesign
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
        End If
    End Sub
    Protected Sub drpCompMaster_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub drpBProduct_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpBProduct.SelectedIndex = 0 Then
            tblMain.Visible = False
        Else
            tblMain.Visible = True
        End If

    End Sub
    Protected Sub drpProdMaster_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)

    End Sub
    Protected Sub gvCompList_selectedIndexChanging(ByVal sender As Object, ByVal e As GridViewSelectEventArgs)
        addToBox(Convert.ToInt32(drpBProduct.SelectedValue), 0, Convert.ToInt32(gvCompList.DataKeys(e.NewSelectedIndex).Value), False, 1)
    End Sub
    Protected Sub gvProdList_selectedIndexChanging(ByVal sender As Object, ByVal e As GridViewSelectEventArgs)
        addToBox(Convert.ToInt32(drpBProduct.SelectedValue), Convert.ToInt32(gvProdList.DataKeys(e.NewSelectedIndex).Value), 0, True, 1)
    End Sub
    Protected Sub addToBox(ByVal bProductID As Integer, ByVal productID As Integer, ByVal componentID As Integer, ByVal isProduct As Boolean, ByVal qty As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procWarehouseBoxContentInsert", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        With oCmd
            .Parameters.Add(New SqlParameter("@bProductID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@productID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@componentID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@isProduct", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
            .Parameters("@bProductID").Value = bProductID
            .Parameters("@productID").Value = productID
            .Parameters("@componentID").Value = componentID
            .Parameters("@isProduct").Value = isProduct
            .Parameters("@qty").Value = qty
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
        gvContentList.DataBind()
    End Sub
End Class
