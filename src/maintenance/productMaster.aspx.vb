Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_productMaster
    Inherits System.Web.UI.Page

    Protected Sub btnAddEdit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As SqlCommand
        If btnAddEdit.Text = "Add" Then
            'Add new master product
            oCmd = New SqlCommand("procProductMasterInsert", oConn)
            oCmd.Parameters.Add(New SqlParameter("@master", SqlDbType.VarChar, 50))
            oCmd.Parameters("@master").Value = txtMaster.Text
        Else
            'Edit currently selected master product
            oCmd = New SqlCommand("procProductMasterUpdate", oConn)
            oCmd.Parameters.Add(New SqlParameter("@master", SqlDbType.VarChar, 50))
            oCmd.Parameters("@master").Value = txtMaster.Text
            oCmd.Parameters.Add(New SqlParameter("@masterID", SqlDbType.Int))
            oCmd.Parameters("@masterID").Value = Convert.ToInt32(drpMaster.SelectedValue)
            'Edit finished, reset drop down and button
            drpMaster.SelectedIndex = 0
            btnAddEdit.Text = "Add"
        End If
        oCmd.CommandType = CommandType.StoredProcedure
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
        txtMaster.Text = ""
        drpMaster.Items.Clear()
        drpMaster.Items.Add(New ListItem("Select...", "0"))
        drpMaster.DataBind()
    End Sub
    Protected Sub drpMaster_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpMaster.SelectedIndex = 0 Then
            txtMaster.Text = ""
            btnAddEdit.Text = "Add"
        Else
            txtMaster.Text = drpMaster.SelectedItem.ToString
            btnAddEdit.Text = "Edit"
        End If
    End Sub
End Class
