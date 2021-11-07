Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_productDefault
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _compIDPos = 0
    Private Const _defaultCompPos = 3
    Private Const _compDrpPos = 4
    Private Const _dropdownPos = 5
    Private Const _prodCompIDPos = 6

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            If Not Page.IsPostBack Then
                _login = Master.FindControl("logMaintenance")
                _content = _login.FindControl("ContentPlaceHolder1")
                'lblCountry.Text = getCountry(Request.QueryString("country"))
                lblProduct.Text = getProduct(Convert.ToString(Request.QueryString("pid")))
            End If
            lnkBack.NavigateUrl = "productDesign.aspx"
            Session("XSprodDesignCountry") = Request.QueryString("country")
            Session("XSprodDesignProduct") = Request.QueryString("product")
        End If
    End Sub
    Protected Sub gvComponents_dataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim row As GridViewRow = e.Row
            Dim drp As DropDownList = row.FindControl("drpComponents")
            Dim compMasterLabel As Label = row.FindControl("lblID")
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductComponentByMasterIDSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@compMasterID", SqlDbType.Int))
                .Parameters("@compMasterID").Value = Convert.ToInt32(compMasterLabel.Text)
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                drp.DataSource = ds
                drp.DataBind()
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
    Protected Sub gvComponents_rowEdititng(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        Dim rowIndex As Integer = e.NewEditIndex
        Dim drp As DropDownList = gvComponents.Rows(rowIndex).FindControl("drpComponents")
        Dim lblProdCompID As Label = gvComponents.Rows(rowIndex).FindControl("lblProdCompID")
        If drp.SelectedValue.ToString <> "0" Then 'Test to make sure user has selected something
            gvComponents.Rows(rowIndex).Cells(_defaultCompPos).Text = drp.SelectedItem.ToString
            'Update productComponents table with new default product
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductComponentsByProdCompIDUpdate", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@prodCompID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@defaultCompID", SqlDbType.Int))
                .Parameters("@prodCompID").Value = Convert.ToInt32(lblProdCompID.Text)
                .Parameters("@defaultCompID").Value = Convert.ToInt32(drp.SelectedValue)
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
        drp.SelectedIndex = 0
        e.Cancel = True
    End Sub
    Protected Function getCountry(ByVal countryCode As String) As String
        Dim result As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = countryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0)("countryName")
            Else
                result = "#Error"
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
        Return result
    End Function

    Protected Function getProduct(ByVal pid As Integer) As String
        Dim result As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductByPIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@pid", SqlDbType.Int))
            .Parameters("@pid").Value = pid
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = ds.Tables(0).Rows(0)("productName")
            Else
                result = "#Error"
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
        Return result
    End Function
    Protected Sub btnUpdateAll_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList
        Dim lblProdCompID As Label
        For Each row As GridViewRow In gvComponents.Rows
            If row.RowType = DataControlRowType.DataRow Then
                drp = row.FindControl("drpComponents")
                lblProdCompID = row.FindControl("lblProdCompID")
                If drp.SelectedIndex > 0 Then
                    row.Cells(_defaultCompPos).Text = drp.SelectedItem.ToString
                    'Update productComponents table with new default product
                    Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                    Dim oCmd As New SqlCommand("procProductComponentsByProdCompIDUpdate", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@prodCompID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@defaultCompID", SqlDbType.Int))
                        .Parameters("@prodCompID").Value = Convert.ToInt32(lblProdCompID.Text)
                        .Parameters("@defaultCompID").Value = Convert.ToInt32(drp.SelectedValue)
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
            End If
        Next
    End Sub
End Class
