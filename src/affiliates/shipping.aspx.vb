Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class affiliates_shipping
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            showCountryName()
            showVatRate()
            If txtVatRate.Text = "0" Then
                'Vat has not been set, hide everything else
                gvShipping.Visible = False
            End If
        End If
        lblUpdateComplete.Text = ""
    End Sub
    Protected Sub showCountryName()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then
                lblCountryName.Text = "Unknown"
            Else
                lblCountryName.Text = ds.Tables(0).Rows(0)("countryName")
            End If
        Catch ex As Exception
            lblError.Text = "An error occured; " & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub showVatRate()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShippingVatByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim vatRate As Decimal = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then vatRate = ds.Tables(0).Rows(0)("vatRate")
            txtVatRate.Text = vatRate
        Catch ex As Exception
            lblError.Text = "An error occured displaying tax rate; " & ex.ToString
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnVatUpdate_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShippingVatInsert", oConn)
        If gvShipping.Visible Then oCmd.CommandText = "procShippingVatUpdate"
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@vatRate", SqlDbType.Decimal))
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
            .Parameters("@vatRate").Value = txtVatRate.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblError.Text = "An error occured while setting new vatRate; " & ex.ToString
            Dim si As New siteInclude
            si.addError("affilaites/shipping.aspx.vb", "Session(EBAffEBDistributorCountryCode)=" & Session("EBAffEBDistributorCountryCode") & "; ")
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            gvShipping.Visible = True
            lblUpdateComplete.Text = "<font color='red'>Update Successful</font>"
        End If

    End Sub
    Protected Sub lnkAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        panAdd.Visible = True
    End Sub
    Protected Sub fvAdd_itemInserted(ByVal sender As Object, ByVal e As FormViewInsertedEventArgs)
        panAdd.Visible = False
        gvShipping.DataBind()
    End Sub
End Class
