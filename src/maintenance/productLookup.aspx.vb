Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_productLookup
    Inherits System.Web.UI.Page
    Private Const _newIDPos As Integer = 2

    Protected Function doesExist(ByVal obj As Object) As Boolean
        Dim result As Boolean = True
        If IsDBNull(obj) Then
            result = False
        End If
        Return result
    End Function
    Protected Sub btnSubit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblExists As Label
        Dim txt As TextBox
        Dim newID As Integer
        Dim oConn As SqlConnection
        Dim oCmd As SqlCommand
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        For Each row As GridViewRow In gvList.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblExists = row.FindControl("lblValueExists")
                txt = row.FindControl("txtOldID")
                If txt.Text <> "" Then
                    If IsNumeric(txt.Text) Then
                        'All ok
                        newID = CType(row.Cells(_newIDPos).Text, Integer)
                        If CType(lblExists.Text, Boolean) Then
                            oCmd = New SqlCommand("procProductLookupUpdate", oConn)
                        Else
                            oCmd = New SqlCommand("procProductLookupInsert", oConn)
                        End If
                        With oCmd
                            .CommandType = CommandType.StoredProcedure
                            .Parameters.Add(New SqlParameter("@newID", SqlDbType.Int))
                            .Parameters.Add(New SqlParameter("@oldID", SqlDbType.Int))
                            .Parameters("@newID").Value = newID
                            .Parameters("@oldID").Value = CType(txt.Text, Integer)
                        End With
                        Try
                            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                            oCmd.ExecuteNonQuery()
                        Catch ex As Exception
                            Response.Write(ex.Message & "<br><br>")
                            Response.Write("newID=" & newID & "<br>oldID=" & txt.Text)
                            Response.End()
                        Finally
                            oCmd.Dispose()
                        End Try
                    End If
                End If
            End If
        Next
        gvList.DataBind()
    End Sub
End Class
