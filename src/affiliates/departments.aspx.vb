Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class affiliates_departments
    Inherits BasePage
    Private Const _gvDepts_departmentPos As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            'Remove items from the PeartreeDepartment dropdown if they already exist in the 
            'For Each row As GridViewRow In gvDepts.Rows
            'removeFromPeartreeDropdown(row.Cells(_gvDepts_departmentPos).Text)
            'Next
            lblError.Text = ""
        Else
            Response.Redirect("default.aspx")
        End If
    End Sub
    Protected Sub btnInsert_click(ByVal sender As Object, ByVal e As EventArgs)
        panAdd.Visible = True
        btnInsert.Visible = False
    End Sub
    Protected Sub btnAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        'Check that the selected item from the dropdown is valid
        Page.Validate()
        If Page.IsValid Then
            btnInsert.Visible = True
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procDeptDistInsert", oConn)
            Dim pk As Integer = 0
            Dim menuType As String
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@deptName", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@deptDescription", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@deptCountry", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@deptImage", SqlDbType.VarChar, 30))
                .Parameters.Add(New SqlParameter("@deptActive", SqlDbType.Bit))
                .Parameters.Add(New SqlParameter("@deptPriority", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
                .Parameters("@deptName").Value = txtName.text
                .Parameters("@deptDescription").Value = txtDescription.Text
                .Parameters("@deptCountry").Value = Session("EBAffEBDistributorCountryCode")
                .Parameters("@deptImage").Value = txtImage.Text
                .Parameters("@deptActive").Value = True
                .Parameters("@deptPriority").Value = CType(txtPriority.Text, Integer)
                .Parameters("@pk").Direction = ParameterDirection.Output
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
                pk = oCmd.Parameters("@pk").Value
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            'Add to siteMenus
            menuType = getMenuType()
            If menuType <> "" Then
                oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                oCmd = New SqlCommand("procSiteMenusInsert2", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                    .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@menuType", SqlDbType.VarChar, 20))
                    .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@showOnMenu", SqlDbType.Bit))
                    .Parameters.Add(New SqlParameter("@priority", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@static", SqlDbType.Bit))
                    .Parameters.Add(New SqlParameter("@showInEditor", SqlDbType.Bit))
                    .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
                    .Parameters("@name").Value = txtName.Text
                    .Parameters("@menuType").Value = menuType
                    .Parameters("@url").Value = "~/shop/dept.aspx?id=" & pk
                    .Parameters("@showOnMenu").Value = True
                    .Parameters("@priority").Value = CInt(txtPriority.Text)
                    .Parameters("@static").Value = False
                    .Parameters("@showInEditor").Value = False
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    lblError.Text = "<font color='red'>An error occured while adding new department to the Shop Menu.</font>"
                Finally
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            Else
                'MenuType was returned as ""
                'lblError.Text = "No MenuType could be found for this country. Please contact tech support."
                'Error is now handled and logged by getMenuType
            End If
            'Hide the add panel now its been used
            panAdd.Visible = False
            'If anything was being edited, exit edit mode
            gvDepts.EditIndex = -1
            'Rebind data, this will add the new entry to the gridview
            gvDepts.DataBind()
            'Clear all of the Add New Dept fields, so they are clear incase the user wants to add more
            'txtDeptName.Text = ""
            'drpPeartreeDepartments.SelectedIndex = 0
            txtDescription.Text = ""
            txtImage.Text = ""
            txtPriority.Text = ""
        Else
            lblError.Text = "<font color='red'>That department already exist, please choose another from the dropdown.</font>"
        End If
    End Sub
    Protected Sub gvDepts_rowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim row As GridViewRow = gvDepts.Rows(gvDepts.EditIndex)
        Dim bError As Boolean = False
        'Save file
        Dim fileupload As FileUpload = row.FindControl("f1")
        If fileupload.HasFile Then
            Try
                fileupload.SaveAs(Server.MapPath("~/design/shop/images/") + fileupload.FileName)
            Catch ex As Exception
                bError = True
                Dim lblError As Label = row.FindControl("lblError")
                lblError.Text = "An error occured:<br>" & ex.Message
            End Try
        End If
        e.Cancel = bError
    End Sub
    Protected Sub removeFromPeartreeDropdown(ByVal txt As String)
        For Each item As ListItem In drpPeartreeDepartments.Items
            If item.Text = txt Then item.Value = "x"
        Next
    End Sub
    Protected Sub sqlDepartments_updated(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        'Keep siteMenus(showOnMenu)property the same as 'deptActive' in the dept table -also update Priority Order
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByURLShowOnMenuUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@showOnMenu", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@priority", SqlDbType.Int))
            .Parameters("@url").Value = "~/shop/dept.aspx?id=" & e.Command.Parameters("@deptID").Value
            .Parameters("@showOnMenu").Value = e.Command.Parameters("@deptActive").Value
            .Parameters("@name").Value = e.Command.Parameters("@deptName").Value
            .Parameters("@priority").Value = e.Command.Parameters("@deptPriority").Value
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub sqlDepartments_deleted(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        'Delete entry from siteMenus table doo
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procSiteMenusByUrlDelete", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 500))
            .Parameters("@url").Value = "~/shop/dept.aspx?id=" & e.Command.Parameters("@deptID").Value
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Try
                Dim si As New siteInclude
                si.addError("Affilaites/departments.aspx.vb", "sqlDepartments_deleted(deptID=" & e.Command.Parameters("@deptID").Value & "); " & ex.ToString)
                si = Nothing
            Catch ex2 As Exception
            End Try
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function getMenuType() As String
        Dim d As New siteInclude
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procImageMapsByCountryCodeURLMenuTypeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = ""
        d.debug("Starting getMenutype")
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@url", SqlDbType.VarChar, 50))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@url").Value = "~/shopIntro.aspx%"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If Not validDataExists(ds.Tables(0).Rows(0)) Then
                    lblError.Text = "<font color='red'>No reference to shopInclude exists in the 'ImageMaps' table.<br>This department will not be shown in the Shop Department.<br>Please contact tech Support."
                    Try
                        Dim si As New siteInclude
                        si.addError("affilaites/departments.aspx.vb", "a shopInclude link needs adding to imageMaps for " & UCase(Session("EBAffEBDistributorCountryCode")) & ".")
                        si = Nothing
                    Catch ex As Exception
                    End Try
                Else
                    result = ds.Tables(0).Rows(0)("menuName")
                End If
            Else
                lblError.Text = "<font color='red'>No reference to shopInclude exists in the 'ImageMpas' table.<br>This department will not be shown in the Shop Department.<br>Please contact tech Support."
            End If
        Catch ex As Exception
            lblError.Text = "An error occured while creating the department. It may not show in the Shop menu.<br>Please contact Tech Support for further info."
            Dim si As New siteInclude
            si.addError("affilaites/departments.aspx.vb", "getMenuType(Session(EBAffEBDistributorCountryCode)= " & UCase(Session("EBAffEBDistributorCountryCode")) & ");" & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function validDataExists(ByRef row As datarow) As Boolean
        Dim result As Boolean = True
        'Set result to false if the data is null or =""
        If IsDBNull(row("menuName")) Then result = False
        If result Then If row("menuName") = "" Then result = False
        Return result
    End Function
End Class
