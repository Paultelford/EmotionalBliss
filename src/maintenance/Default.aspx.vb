Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_Default
    Inherits System.Web.UI.Page
    Protected Sub Pag_OnInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        Membership.Provider.ApplicationName = "ebProvider"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Membership.GetUser Is Nothing Then Response.Redirect("login.aspx")
        'Only show error log if user is in the 'Dev' role
        Dim user As MembershipUser = Membership.GetUser
        If Not Roles.IsUserInRole(user.UserName, "Dev") Then
            gvErrors.Visible = False
            gvScanErrors.Visible = False
            btnDeleteAll.Visible = False
            lblErrors.Visible = False
            lblScanErrors.Visible = False
        End If
    End Sub
    Protected Sub gvErrors_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If gvErrors.Rows.Count = 0 Then
            lblErrors.Visible = False
        End If
    End Sub
    Protected Sub gvScanErrors_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If gvScanErrors.Rows.Count = 0 Then
            lblScanErrors.Visible = False
        End If
    End Sub
    Protected Function showDateTime(ByVal d As Date) As String
        Return FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
    End Function
    Protected Sub lnkClearAll_click(ByVal sender As Object, ByVal e As EventArgs)
        'clear all errors
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procErrorsUpdate", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("maintenance/default.aspx.vb", "lnkClearAll_click(); " & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        gvErrors.DataBind()
    End Sub
End Class
