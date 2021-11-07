Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_emailSetup
    Inherits System.Web.UI.Page
    Private Const _gvEmail_groupsPos As Integer = 2

    Protected Sub page_load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'add to email_groups table
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEmailGroupsInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@groupname", SqlDbType.VarChar, 30))
            .Parameters("@groupname").Value = txtNewGroup.Text
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
        drpGroups.DataBind()
        txtNewGroup.Text = ""
        lblError.Text = ""
    End Sub
    Protected Sub drpGroups_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        drpGroups.Items.Clear()
        drpGroups.Items.Add(New ListItem("Select...", "0"))
    End Sub
    Protected Sub drpGroups_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If drpGroups.Items.Count = 0 Then
            drpGroups.Items.Add("No groups exist")
            drpGroups.Items(0).Value = 0
        End If
    End Sub

    Protected Sub gvEmail_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim drp As DropDownList = e.Row.FindControl("drpGroupList")
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procEmailGroupsByEmailIDSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@emailID", SqlDbType.Int))
                .Parameters("@emailID").Value = gvEmail.DataKeys(e.Row.RowIndex).Value
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    'drp.DataSource = ds
                    'drp.DataBind()
                    For Each rs As DataRow In ds.Tables(0).Rows
                        e.Row.Cells(_gvEmail_groupsPos).Text = e.Row.Cells(_gvEmail_groupsPos).Text & rs("groupname") & ","
                    Next
                Else
                    e.Row.Cells(_gvEmail_groupsPos).Text = "None,"
                End If
                'Remove last comma
                e.Row.Cells(_gvEmail_groupsPos).Text = Left(e.Row.Cells(_gvEmail_groupsPos).Text, Len(e.Row.Cells(_gvEmail_groupsPos).Text) - 1)
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
    End Sub
    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add checked address to selected group
        Dim groupID As Integer = Convert.ToInt32(drpGroups.SelectedValue)
        Dim chk As CheckBox
        If groupID <> 0 Then 'Only run if the dropdowns selecteValue <> 0
            Dim oConn As SqlConnection
            Dim oCmd As SqlCommand
            For Each row As GridViewRow In gvEmail.Rows
                chk = row.FindControl("chkUpdate")
                If chk.Checked Then
                    oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                    oCmd = New SqlCommand("procEmailLinkInsert", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@emailID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@groupID", SqlDbType.Int))
                        .Parameters("@emailID").Value = Convert.ToInt32(gvEmail.DataKeys(row.RowIndex).Value)
                        .Parameters("@groupID").Value = groupID
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
                End If
            Next
            gvEmail.DataBind()
            lblError.Text = ""
        End If
    End Sub
    Protected Sub btnDelete_click(ByVal sender As Object, ByVal e As EventArgs)
        'Remove checked address from selcted group
        Dim groupID As Integer = Convert.ToInt32(drpGroups.SelectedValue)
        Dim chk As CheckBox
        If groupID <> 0 Then 'Only run if the dropdowns selecteValue <> 0
            Dim oConn As SqlConnection
            Dim oCmd As SqlCommand
            For Each row As GridViewRow In gvEmail.Rows
                chk = row.FindControl("chkUpdate")
                If chk.Checked Then
                    oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
                    oCmd = New SqlCommand("procEmailLinkDelete", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@emailID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@groupID", SqlDbType.Int))
                        .Parameters("@emailID").Value = Convert.ToInt32(gvEmail.DataKeys(row.RowIndex).Value)
                        .Parameters("@groupID").Value = groupID
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
                End If
            Next
            gvEmail.DataBind()
            lblError.Text = ""
        End If
    End Sub
    Protected Sub btnEmailSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Add email address to email table
        Dim oConn As SqlConnection
        Dim oCmd As SqlCommand
        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        oCmd = New SqlCommand("procEmailInsert", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        With oCmd
            .Parameters.Add(New SqlParameter("@address", SqlDbType.VarChar, 100))
            .Parameters.Add(New SqlParameter("@alreadyExists", SqlDbType.Bit))
            .Parameters("@address").Value = txtNewEmail.Text
            .Parameters("@alreadyExists").Direction = ParameterDirection.Output
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
            If Convert.ToBoolean(oCmd.Parameters("@alreadyExists").Value) Then
                lblError.Text = "Email address already exists!"
            Else
                lblError.Text = ""
            End If
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        txtNewEmail.Text = ""
        gvEmail.DataBind()
    End Sub
    Protected Sub lnkActive_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim dcfc As DataControlFieldCell = CType(sender, LinkButton).Parent
        Dim row As GridViewRow = dcfc.Parent
        Dim hidEmail As HiddenField = row.FindControl("hidEmail")
        Dim hidActive As HiddenField = row.FindControl("hidActive")
        Try
            Dim param() As String = {"@email", "@subscribe"}
            Dim paramValue() As String = {hidEmail.Value, (Not (CBool(hidActive.Value))).ToString()}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.Bit}
            Dim paramSize() As Integer = {100, 0}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procEmailsActiveUpdate")
            gvEmail.DataBind()
        Catch ex As Exception
            siteInclude.addError("emailSetup.aspx.vb", "lnkActive_click(email='" & hidEmail.Value & "',currentactive=" & hidActive.Value & "); " & ex.ToString())
        End Try
    End Sub
    Protected Sub drpGroups_selectedInxedChanged(ByVal sender As Object, ByVal e As EventArgs)
        showCurrentGroup()
    End Sub
    Protected Sub gvEmail_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        showCurrentGroup()
    End Sub
    Protected Sub showCurrentGroup()
        'The groups dropdown has been changed.   Hilight all email addresses withing this group
        Dim groupID As Integer = Convert.ToInt32(drpGroups.SelectedValue)
        Dim drp As DropDownList = drpGroups
        Dim arr As Array
        Dim groupMember As Boolean
        For Each row As GridViewRow In gvEmail.Rows
            If row.RowType = DataControlRowType.DataRow Then
                groupMember = False
                'drp = row.FindControl("drpGroupList")
                arr = Split(row.Cells(_gvEmail_groupsPos).Text, ",")
                'For Each li As ListItem In drp.Items
                'If Convert.ToInt32(li.Value) = groupID And groupID <> 0 Then groupMember = True
                'Next
                For Each s As String In arr
                    If drp.SelectedItem.Text = s Then groupMember = True
                Next
                If groupMember Then
                    row.BackColor = Drawing.Color.LightGray
                Else
                    row.BackColor = Drawing.Color.White
                End If

            End If
        Next
    End Sub
    Protected Function getActivateText(ByVal o As Object) As String
        Dim result As String = ""
        If Not IsDBNull(o) Then
            If CBool(o.ToString()) Then
                result = "Deactivate"
            Else
                result = "Activate"
            End If
        End If
        Return result
    End Function
End Class

