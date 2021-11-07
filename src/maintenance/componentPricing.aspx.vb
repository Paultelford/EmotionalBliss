Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentPricing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            If Page.IsPostBack Then

            End If
        End If
    End Sub
    Protected Sub gvComponents_rowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim bErrorFound As Boolean = False
        If e.NewValues("price") = "" Then e.NewValues("price") = "0"
        If e.NewValues("vatRate") = "" Then e.NewValues("vatRate") = "0"

        If Not IsNumeric(e.NewValues("price")) Then
            'error
            lblError.Text = "Invalid price."
            bErrorFound = True
        End If
        If Not IsNumeric(e.NewValues("vatRate")) Then
            'error
            lblError.Text = "Invalid Vat Rate."
            bErrorFound = True
        End If
        e.Cancel = bErrorFound
        If Not bErrorFound Then lblError.Text = ""
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        drpMaster.Visible = Not CType(drpType.SelectedValue = "0", Boolean)
        If drpType.SelectedValue <> "0" Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As SqlCommand
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            drpMaster.Items.Clear()
            drpMaster.Items.Add(New ListItem("Select...", "0"))
            drpMaster.Items.Add(New ListItem("All", "%"))
            Select Case drpType.SelectedValue
                Case "master"
                    oCmd = New SqlCommand("procComponentMastersSelect", oConn)
                Case "manu"
                    oCmd = New SqlCommand("procManufacturersSelect", oConn)
            End Select
            oCmd.CommandType = CommandType.StoredProcedure
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                drpMaster.DataSource = ds
                If ds.Tables(0).Rows.Count > 0 Then
                    Select Case drpType.SelectedValue
                        Case "master"
                            drpMaster.DataTextField = "name"
                            drpMaster.DataValueField = "masterid"
                        Case "manu"
                            drpMaster.DataTextField = "manufacturerName"
                            drpMaster.DataValueField = "manufacturerID"
                    End Select
                End If
                drpMaster.DataBind()
            Catch ex As Exception
                lblError.Text = ex.Message
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Sub drpMaster_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If False Then
            gvComponents.Visible = Not CType(drpMaster.SelectedValue = "0", Boolean)
            If drpMaster.SelectedValue <> "0" Then
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As SqlCommand
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                Select Case drpType.SelectedValue
                    Case "master"
                        oCmd = New SqlCommand("procComponentsByMasterIDPriceSelect", oConn)
                        oCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.VarChar, 10))
                    Case "manu"
                        oCmd = New SqlCommand("procComponentsByManufacturerIDPriceSelect", oConn)
                        oCmd.Parameters.Add(New SqlParameter("@ID", SqlDbType.VarChar, 10))
                End Select
                oCmd.Parameters("@ID").Value = drpMaster.SelectedValue
                oCmd.CommandType = CommandType.StoredProcedure
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    gvComponents.DataSource = ds
                    gvComponents.DataBind()
                Catch ex As Exception
                    lblError.Text = ex.Message
                Finally
                    ds.Dispose()
                    da.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            End If
        End If
        gvComponents.Visible = Not CType(drpMaster.SelectedValue = "0", Boolean)
        Select Case drpType.SelectedValue
            Case "master"
                SqlComponents.SelectCommand = "procComponentsByMasterIDPriceSelect"
            Case "manu"
                SqlComponents.SelectCommand = "procComponentsByManufacturerIDPriceSelect"
        End Select
    End Sub
    Protected Sub drpMaster_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Select Case drpType.SelectedValue
            Case "master"
                SqlComponents.SelectCommand = "procComponentsByMasterIDPriceSelect"
            Case "manu"
                SqlComponents.SelectCommand = "procComponentsByManufacturerIDPriceSelect"
        End Select
        gvComponents.DataBind()
    End Sub
    Protected Sub gvComponents_rowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        Select Case drpType.SelectedValue
            Case "master"
                SqlComponents.SelectCommand = "procComponentsByMasterIDPriceSelect"
            Case "manu"
                SqlComponents.SelectCommand = "procComponentsByManufacturerIDPriceSelect"
        End Select
    End Sub
    Protected Sub gvComponents_rowUpdated(ByVal sender As Object, ByVal e As GridViewUpdatedEventArgs)
        Select Case drpType.SelectedValue
            Case "master"
                SqlComponents.SelectCommand = "procComponentsByMasterIDPriceSelect"
            Case "manu"
                SqlComponents.SelectCommand = "procComponentsByManufacturerIDPriceSelect"
        End Select
    End Sub
End Class
