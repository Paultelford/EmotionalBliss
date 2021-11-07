Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_productDesign
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _qtyPos As Integer = 2
    Private Const _prodCompIDKey As Integer = 0

    Protected Sub Page_PreLoad(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreLoad
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            If Not Page.IsPostBack Then
                drpProduct.Items.Add(New ListItem("Select...", "0"))
                'gvProd.Visible = False
                'panSearch.Visible = False
                tblComponents.Visible = False
            Else
                
            End If
            lblError.Text = ""
        End If
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            '_login = Master.FindControl("logMaintenance")
            '_content = _login.FindControl("ContentPlaceHolder1")
            If Session("XSprodDesignCountry") <> "" Then
                'User has been sent back from the 'Setting Deafult Component' page, use session to populate dropdowns
                'drpCountry.SelectedValue = Session("XSprodDesignCountry")
                drpProduct.SelectedValue = Session("XSprodDesignProduct")
                tblComponents.Visible = True
                gvProdComp.DataBind()
                'Now clear session variables
                Session("XSprodDesignCountry") = ""
                Session("XSprodDesignProduct") = ""
                DataBind()
            End If
            'Check if Default Compoents have been set up.  If not show error
            If Not isDefaultComponentsOK Then lblError.Text = "Default components need setting up!"
        End If
    End Sub
  
    Protected Sub drpMaster_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        drpProduct.Items.Clear()
        drpProduct.Items.Add(New ListItem("Select...", "0"))
        drpProduct.AppendDataBoundItems = True
        'dataBindProductDrop()
        tblComponents.Visible = False
    End Sub
    Protected Sub drpProduct_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        'gvProd.Visible = True
        'panSearch.Visible = True
        tblComponents.Visible = True
    End Sub
    Protected Sub dataBindProductDrop()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductsByMasterIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@masterID", SqlDbType.Char, 5))
            .Parameters("@masterID").Value = Convert.ToInt32(drpMaster.SelectedValue)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            drpProduct.DataSource = ds
            drpProduct.DataTextField = "productName"
            drpProduct.DataValueField = "productID"
            drpProduct.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnSearch_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = tblComponents.FindControl("txtSearch")
        Dim gv As GridView = tblComponents.FindControl("gvProd")
        If txt.Text <> "" Then
            sqlProducts.FilterExpression = "Name LIKE '%" & txt.Text & "%'"
        Else
            sqlProducts.FilterExpression = ""
        End If
        gv.DataBind()
    End Sub
    Protected Sub txtSearch_textChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim txt As TextBox = tblComponents.FindControl("txtSearch")
        Dim gv As GridView = tblComponents.FindControl("gvProd")
        If txt.Text <> "" Then
            sqlProducts.FilterExpression = "Name LIKE '%" & txt.Text & "%'"
        Else
            sqlProducts.FilterExpression = ""
        End If
        gv.DataBind()
    End Sub
    Protected Sub gvProd_selectedIndexChanging(ByVal sender As Object, ByVal e As GridViewSelectEventArgs)
        'Add selected component to the current product. (After checking it doesnt already exist in current product)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductComponentsInsert", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim gv As GridView = tblComponents.FindControl("gvProd")
        Dim iCompID As Integer = gv.DataKeys(e.NewSelectedIndex).Value
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@prodID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@compMasterID", SqlDbType.Int))
            .Parameters("@prodID").Value = drpProduct.SelectedValue
            .Parameters("@compMasterID").Value = iCompID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'The component already exists, show error
                lblError.Text = "That product is already used in the current product."
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
        gvProdComp.DataBind()
        'Check if Default Compoents have been set up.  If not show error
        If Not isDefaultComponentsOK() Then lblError.Text = "Default components need setting up!"
    End Sub
    Protected Sub gvProdComp_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'lnkDefaults.NavigateUrl = "productDefault.aspx?country=" & drpCountry.SelectedValue & "&product=" & drpProduct.SelectedValue
        lnkDefaults.NavigateUrl = "productDefault.aspx?pid=" & drpProduct.SelectedValue
        If gvProdComp.Rows.Count = 0 Then
            lnkDefaults.Visible = False
        Else
            lnkDefaults.Visible = True
        End If
    End Sub
    Protected Function isDefaultComponentsOK()
        Dim result As Boolean = True
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("prodProductComponentsByProdIDNullDefaultSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@prodCode", SqlDbType.Int))
            .Parameters("@prodCode").Value = Convert.ToInt32(drpProduct.SelectedValue)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = False
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
    Protected Sub gvProdComp_rowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        Dim rowIndex As Integer = e.RowIndex
        If e.NewValues(0).ToString = "0" Then
            'Delete entry
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductComponentsByProdCompIdDelete", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@prodCompID", SqlDbType.Int))
                .Parameters("@prodCompID").Value = e.Keys(_prodCompIDKey)
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
            'e.Cancel = True
            If isDefaultComponentsOK() Then lblError.Text = ""
        End If
    End Sub
End Class
