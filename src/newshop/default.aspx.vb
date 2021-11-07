Imports System.Data
Imports System.Data.SqlClient


Partial Class shop_default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        showDeptByCountry()
        testForFirstVisit() 'Empty basket if session has expired
    End Sub
    Protected Sub showDeptByCountry()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDeptByCountryCodeActiveSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim tRow As New TableRow
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = Session("EBShopCountry")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblDeptEmpty.Text = ""
                For Each rs As DataRow In ds.Tables(0).Rows
                    AddDept(rs, tRow)
                Next
                tblDept.Rows.Add(tRow)
            Else
                lblDeptEmpty.Text = "There are currently no Departments for your selected country.<br>Please choose another country from the dropdown menu."
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
    End Sub
    Protected Sub AddDept(ByVal rs As DataRow, ByRef tblRow As TableRow)
        Dim tCell As New TableCell
        Dim lnk As New HyperLink
        Dim sImgName As String
        lnk.Text = rs("deptName")
        If IsDBNull(rs("deptImage")) Then
            sImgName = "~/images/departments/deptMissing.gif"
        Else
            sImgName = "~/images/departments/" & rs("deptImage")
        End If
        lnk.ImageUrl = sImgName
        lnk.NavigateUrl = "dept.aspx?id=" & rs("deptID")
        tCell.Controls.Add(lnk)
        tblRow.Cells.Add(tCell)
    End Sub
    Protected Sub testForFirstVisit()
        If Not Session("EBCustomerPreambleComplete") Then
            'This is Customers 1st visit, or the session has expired
            'Empty the shopping basket
            Profile.EBCart.emptyBasket()
            Session("EBCustomerPreambleComplete") = True
        End If
    End Sub
End Class


