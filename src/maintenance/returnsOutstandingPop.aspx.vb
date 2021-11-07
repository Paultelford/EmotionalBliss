Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_returnsOutstandingPop
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblReturnsID.Text = Request.QueryString("id")
        If Page.IsPostBack Then
            btnPrint.Visible = False
        End If
    End Sub
    Protected Sub btnPrint_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procReturnsByIdStatusUpdate", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@returnsID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters("@returnsID").Value = Request.QueryString("id")
            .Parameters("@status").Value = "Pending"
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
End Class
