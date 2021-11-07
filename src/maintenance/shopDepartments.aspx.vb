Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_shopDepartments
    Inherits System.Web.UI.Page

    Protected Sub gvCountrys_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As SqlConnection
        Dim oCmd As SqlCommand
        Dim da As SqlDataAdapter
        Dim ds As DataSet
        Dim gv As GridView
        For Each row As GridViewRow In gvCountrys.Rows
            If row.RowType = DataControlRowType.DataRow Then
                oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                oCmd = New SqlCommand("procShopDeptCountrysSelect", oConn)
                da = New SqlDataAdapter
                ds = New DataSet
                gv = row.FindControl("gvLink")
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                    .Parameters("@countryCode").Value = gvCountrys.DataKeys(row.RowIndex).Value
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        gv.DataSource = ds
                        gv.DataBind()
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
            End If
        Next
    End Sub
    Protected Sub gvCountrys_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim countryCode As String = gvCountrys.DataKeys(gvCountrys.SelectedIndex).Value
        Dim gv As GridView = gvCountrys.SelectedRow.FindControl("gvLink")
        Dim chk As CheckBox
        Dim deptID As Integer
        'delete all links for selected country
        removeLinks(countryCode)
        'Go through all departments and assign Active links
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                chk = row.FindControl("chkActive")
                deptID = gv.DataKeys(row.RowIndex).Value
                If chk.Checked Then createLink(countryCode, deptID)
            End If
        Next
        gvCountrys.DataBind()
    End Sub
    Protected Sub removeLinks(ByVal cc As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopDeptLinkByCountryCodeDelete", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = cc
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
    End Sub
    Protected Sub createLink(ByVal cc As String, ByVal deptID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopDeptLinkInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@deptID", SqlDbType.Int))
            .Parameters("@countryCode").Value = cc
            .Parameters("@deptID").Value = deptID
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
    End Sub
    Protected Function isActive(ByVal id As Object) As Boolean
        Dim result = True
        If IsDBNull(id) Then result = False
        Return result
    End Function
    Protected Sub gvDepartments_rowUpdated(ByVal sender As Object, ByVal e As GridViewUpdatedEventArgs)
        gvCountrys.DataBind()
    End Sub
    Protected Sub btnAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        If txtNewDept.Text <> "" Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procShopDeptInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@deptName", SqlDbType.VarChar, 50))
                .Parameters("@deptName").Value = txtNewDept.Text
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
            txtNewDept.Text = ""
            gvDepartments.DataBind()
            gvCountrys.DataBind()
        End If
    End Sub
End Class
