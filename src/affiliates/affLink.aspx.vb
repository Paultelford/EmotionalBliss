Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_affLink
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            drpLanguage.SelectedValue = Session("EBAffCountryCode")
            lblURL.Text = "~/default.aspx"
            lblParam.Text = "?country=" & Session("EBAffCountryCode") & "&language=" & Session("EBAffCountryCode")
            showCurrentDefaultPage()
        End If
    End Sub
    Protected Sub drpPage_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        Dim url As String = ""
        Dim param As Boolean = False
        Dim custom As Boolean = False
        Dim language As Boolean = True
        Select Case drp.SelectedValue
            Case "welcome"
                url = "default.aspx"
            Case "shop"
                url = "shop/default.aspx"
            Case "product"
                url = "shop/product.aspx"
                param = True
                rebindProducts()
            Case "sex"
                url = "sexologists.aspx"
            Case "custom"
                custom = True
        End Select
        trParam.Visible = param
        trCustom.Visible = custom
        trLanguage.Visible = Not custom
        lblURL.Text = url
    End Sub
    Protected Sub drpLanguage_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblParam.Text = "?country=" & drpLanguage.SelectedValue & "&language=" & drpLanguage.SelectedValue
        If drpPage.SelectedValue = "product" Then rebindProducts()
    End Sub
    Protected Sub drpParam_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblParam.Text = "?country=" & drpLanguage.SelectedValue & "&language=" & drpLanguage.SelectedValue & "&id=" & drpParam.SelectedValue
    End Sub
    Protected Sub rebindProducts()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procWarehouseProductsByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = drpLanguage.SelectedValue
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            drpParam.DataSource = ds
            drpParam.DataBind()
        Catch ex As Exception
            lblError.Text = lblError.Text & "Error occured during productDatabinding:" & ex.ToString & "<br>"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        lblParam.Text = "?country=" & drpLanguage.SelectedValue & "&language=" & drpLanguage.SelectedValue & "&pid=" & drpParam.SelectedValue
    End Sub
    Protected Sub btnSubmit_saveChanges(ByVal sender As Object, ByVal e As EventArgs)
        Dim page As String = ""
        Dim param As String = ""
        If drpPage.SelectedValue = "custom" Then
            setVars(page, param)
        Else
            page = removeLeadingCharacters(lblURL.Text)
            param = lblParam.Text
        End If
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateLinkByIDUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@page", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@param", SqlDbType.VarChar, 50))
            .Parameters("@affID").Value = Session("EBAffID")
            .Parameters("@page").Value = "~/" & page
            .Parameters("@param").Value = param
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = lblError.Text & "Error whilest saving changes: " & ex.ToString & "<br>"
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        tblMain.visible = False
        lblMsg.text = "You changes have been saved."
    End Sub
    Protected Sub setVars(ByRef url, ByRef param)
        If InStr(txtCustom.Text, "?") Then
            'User has entered URL & Parameters
            Dim arr = Split(txtCustom.Text, "?")
            url = removeLeadingCharacters(arr(0))
            param = "?" & arr(1)
        Else
            'URL Only
            url = removeLeadingCharacters(txtCustom.Text)
        End If
    End Sub
    Protected Function removeLeadingCharacters(ByVal str As String) As String
        'Removes the 1 character of a string if it isnt an alphabetical character
        'Function runs recursively to remove all non-alpha chars 1 at a time
        Dim c As String = Left(str, 1)
        If (Asc(c) > 96 And Asc(c) < 123) Or Len(str) < 2 Then
            Return str
        Else
            Return removeLeadingCharacters(Right(str, Len(str) - 1))
        End If
    End Function
    Protected Sub showCurrentDefaultPage()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByIDTargetSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters("@affID").Value = Session("EBAffID")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblCurrent.Text = ds.Tables(0).Rows(0)("fullURL")
            Else
                lblCurrent.Text = "Error, aff session timed out or affiliate not found."
            End If
        Catch ex As Exception
            lblError.Text = "Error occured whiles getting affiliates current default page: " & ex.ToString & "<br>"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
End Class
