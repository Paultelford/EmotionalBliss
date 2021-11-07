Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_menuPermissions
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _DescPos As Integer = 0
    Private Const _URLPos As Integer = 1
    Private Const _AccessPos As Integer = 2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            If Not Page.IsPostBack Then
                _login = Master.FindControl("logMaintenance")
                _content = _login.FindControl("ContentPlaceHolder1")
                For Each user As MembershipUser In Membership.GetAllUsers
                    drpUsers.Items.Add(New ListItem(user.UserName, user.UserName))
                Next
            End If
            If Not Roles.IsUserInRole("Administrator") Then
                pan1.Visible = False
                pan2.Visible = True
            End If
        End If
    End Sub
    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim user As String = drpUsers.SelectedValue
        Dim chk As CheckBox
        If user <> "0" Then 'Make sure valid user has been selected from dropdown
            'Remove all accociations to selected user
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procMenuPermissionsByUsernameDelete", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 25))
                .Parameters("@username").Value = user
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
            For Each row As GridViewRow In gvMenu.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    chk = row.FindControl("chkHasAccess")
                    If chk.Checked Then
                        addPermission(user, Convert.ToInt32(gvMenu.DataKeys(row.RowIndex).Value))
                    End If
                End If
            Next
        End If
        drpUsers.SelectedIndex = 0
        gvMenu.Visible = False
    End Sub
    Protected Sub addPermission(ByVal user As String, ByVal menuID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procMenuPermissionsInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@username", SqlDbType.VarChar, 25))
            .Parameters.Add(New SqlParameter("@menuID", SqlDbType.Int))
            .Parameters("@username").Value = user
            .Parameters("@menuID").Value = menuID
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
    Protected Sub drpUsers_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpUsers.SelectedValue = "0" Then
            gvMenu.Visible = False
        Else
            gvMenu.Visible = True
        End If
    End Sub
    Protected Sub gvMenu_dataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.Cells(_URLPos).Text = "*" Then
            Dim chk As CheckBox
            e.Row.Cells(_DescPos).Style.Add(HtmlTextWriterStyle.FontWeight, "800")
            e.Row.Cells(_URLPos).Text = ""
            'chk = e.Row.FindControl("chkHasAccess")
            'chk.Visible = False
        End If
    End Sub
    Protected Function populateChkBox(ByVal val As Object) As Boolean
        Dim returnVal As Boolean = False
        If val.ToString = "True" Then returnVal = True
        Return returnVal
    End Function
End Class
