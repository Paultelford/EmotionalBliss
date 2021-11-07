Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_deptAdd
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private drpMainCountry As DropDownList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            drpMainCountry = _login.FindControl("drpMainCountry")
            drpMainCountry.Visible = True
            If Page.IsPostBack Then
                If dvDept.CurrentMode = DetailsViewMode.Insert Then
                    'Response.Write("cleared<br>")
                    'drpDepartments.Items.Clear()
                    'drpDepartments.Items.Add(New ListItem("Add New", "0"))
                    'drpDepartments.DataBind()
                End If
            Else
                'bindDrpCountry()
            End If
        End If
    End Sub
    Protected Sub SqlDepartments_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        'If LCase(drpMainCountry.Items(0).Text) = "all" Then
        'e.Command.Parameters(0).Value = "%"
        'Else
        e.Command.Parameters(0).Value = drpMainCountry.SelectedValue
        'End If
    End Sub
    Protected Sub drpDepartment_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpDepartments.SelectedValue = "0" Then
            dvDept.ChangeMode(DetailsViewMode.Insert)
        Else
            dvDept.ChangeMode(DetailsViewMode.Edit)
        End If
    End Sub
    Protected Sub drpCountryCode_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        'Clear dropdown
        drp.Items.Clear()
        'Try and create dropdown with same countrys as in users drpMainCountry dropdown (excl. All)
        For Each item As ListItem In drpMainCountry.Items
            If item.Text <> "All" Then drp.Items.Add(item)
        Next
        'Automatically select same country as the one thats selected in drpMainCountry. Only for insert. Edit will have its own value brought back from db
        If drpDepartments.SelectedValue = "0" Then
            If drpMainCountry.SelectedValue <> "%" Then drp.SelectedValue = drpMainCountry.SelectedValue
        End If
    End Sub
    Protected Sub SqlCountrys_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        'Insert countrycode from drpMainCountry
        e.Command.Parameters(0).Value = drpMainCountry.SelectedValue
    End Sub
    Protected Sub dvDept_inserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        drpDepartments.Items.Clear()
        drpDepartments.Items.Add(New ListItem("Add New", "0"))
        drpDepartments.DataBind()
    End Sub
    Protected Sub bindDrpCountry()
        Dim drp As DropDownList = dvDept.Rows(2).Cells(1).FindControl("drpCountryCode")
        'Clear dropdown
        drp.Items.Clear()
        'Try and create dropdown with same countrys as in users drpMainCountry dropdown (excl. All)
        For Each item As ListItem In drpMainCountry.Items
            If item.Text <> "All" Then drp.Items.Add(item)
        Next
        'Automatically select same country as the one thats selected in drpMainCountry. Only for insert. Edit will have its own value brought back from db
        If drpDepartments.SelectedValue = "0" Then
            If drpMainCountry.SelectedValue <> "%" Then drp.SelectedValue = drpMainCountry.SelectedValue
        End If
    End Sub
    Protected Sub dvDept_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If dvDept.CurrentMode = DetailsViewMode.Insert Then
            Dim drp As DropDownList = dvDept.Rows(2).Cells(1).FindControl("drpCountryCode")
            'Clear dropdown
            drp.Items.Clear()
            If drpMainCountry.Items(0).Text = "All" Then
                'Grab all from DB
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procCountryMaintenanceSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                oCmd.CommandType = CommandType.StoredProcedure
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    For Each rs As DataRow In ds.Tables(0).Rows
                        If LCase(rs("countryName")) <> "all" Then drp.Items.Add(New ListItem(rs("countryName"), rs("countryCode")))
                        If drpMainCountry.SelectedValue <> "%" Then drp.SelectedValue = drpMainCountry.SelectedValue
                    Next
                Catch ex As Exception
                    Response.Write(ex.Message)
                    Response.End()
                Finally
                    ds.Dispose()
                    da.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            Else
                'Use countrys from main maintenance dropdown

                'Try and create dropdown with same countrys as in users drpMainCountry dropdown (excl. All)
                For Each item As ListItem In drpMainCountry.Items
                    If item.Text <> "All" Then drp.Items.Add(item)
                Next
                'Automatically select same country as the one thats selected in drpMainCountry. Only for insert. Edit will have its own value brought back from db
                If drpDepartments.SelectedValue = "0" Then
                    If drpMainCountry.SelectedValue <> "%" Then drp.SelectedValue = drpMainCountry.SelectedValue
                End If
            End If
        End If
    End Sub
    Protected Sub dvDept_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        drpDepartments.SelectedIndex = 0
    End Sub
End Class
