Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentMaster
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            lblDeleteError.Text = ""
        End If
    End Sub
    Protected Sub txtNewType_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        addNewType()
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        addNewType()
    End Sub
    Protected Sub addNewType()
        If txtNewType.Text <> "" Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procComponentMasterInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 20))
                .Parameters("@name").Value = txtNewType.Text
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
            gvMaster.DataBind()
        End If
        txtNewType.Text = ""
    End Sub
    Protected Sub sqlProductMaster_deleting(ByVal sender As Object, ByVal e As SqlDataSourceCommandEventArgs)
        Dim cancelDeleteRequest As Boolean = True
        'Check that master has no child products associated.  Masters can only be deleted if they have no children
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentsByMasterIDListSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@masterID", SqlDbType.Int))
            .Parameters("@masterID").Value = e.Command.Parameters(0).Value
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblDeleteError.Text = "You cannot delete that Master Type, as it still has components associated with it:<br>"
                For Each rs As DataRow In ds.Tables(0).Rows
                    lblDeleteError.Text = lblDeleteError.Text & rs("componentName") & "<br>"
                Next
                lblDeleteError.Text = lblDeleteError.Text & "<br><br>"
            Else
                cancelDeleteRequest = False
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
        e.Cancel = cancelDeleteRequest
    End Sub
End Class
