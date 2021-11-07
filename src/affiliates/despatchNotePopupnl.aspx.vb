Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_despatchNotePopupnl
    Inherits System.Web.UI.Page

    'System
    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreInit
        If LCase(Session("EBAffEBDistributorCountryCode")) <> "nl" And LCase(Session("EBAffEBDistributorCountryCode")) <> "be" Then Response.Redirect("/affiliates/default.aspx")
        Page.Theme = "EBTheme"
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByOrderIDSelect", oConn)
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
        oCmd.Parameters("@id").Value = Request.QueryString("id")
        Dim da As New SqlDataAdapter(oCmd)
        Dim ds As New DataSet
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                lblOrderID.Text = ds.Tables(0).Rows(0)("newOrderID") & UCase(ds.Tables(0).Rows(0)("orderCountryCode"))
                lblOrderDate.Text = FormatDateTime(ds.Tables(0).Rows(0)("orderDate"), DateFormat.LongDate)
            End If
        Catch ex As Exception
            Response.Write(ex.ToString())
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        

    End Sub
    Public Overrides Property StyleSheetTheme() As String
        Get
            Return "EBTheme"
        End Get
        Set(ByVal value As String)
        End Set
    End Property
    'Page
    Protected Sub dv_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Hide the 1st colum (headers)
        Dim dv As DetailsView = CType(sender, DetailsView)
        dv.HeaderRow.Visible = False
        For Each row As DetailsViewRow In dv.Rows
            If row.Cells(1).Text = "@@" Then row.Visible = False
            row.Cells(0).Visible = False
        Next
    End Sub
    Protected Sub gvItems_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As GridViewRow In gvItems.Rows
            If row.RowType = DataControlRowType.DataRow Then
                row.Cells(1).Text = CStr(row.RowIndex + 1)
            End If
        Next
    End Sub
    Protected Function showTotal(ByVal goods As Object, ByVal shipping As Object, ByVal vat As Object) As Decimal
        Dim g As Decimal = 0
        Dim s As Decimal = 0
        Dim v As Decimal = 0
        If Not IsDBNull(goods) Then g = CType(goods, Decimal)
        If Not IsDBNull(shipping) Then s = CType(shipping, Decimal)
        If Not IsDBNull(vat) Then v = CType(vat, Decimal)
        Return FormatNumber(g + s + v, 2)
    End Function
    Protected Sub litDate_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lit As Literal = CType(sender, Literal)
        lit.Text = Day(Now()) & "/" & Month(Now()) & "/" & Year(Now())
    End Sub
    
    Protected Function showCurrency() As String
        Return Session("EBAffCurrencySign")
    End Function
    Protected Function formatAddress(ByVal a1 As Object, ByVal a2 As Object, ByVal a3 As Object, ByVal a4 As Object, ByVal a5 As Object) As String
        Dim result As String = ""
        If Not IsDBNull(a1) Then If a1.ToString() <> "" Then result = result & a1.ToString() & ","
        If Not IsDBNull(a2) Then If a2.ToString() <> "" Then result = result & a2.ToString() & ","
        If Not IsDBNull(a3) Then If a3.ToString() <> "" Then result = result & a3.ToString() & ","
        If Not IsDBNull(a4) Then If a4.ToString() <> "" Then result = result & a4.ToString() & ","
        If Not IsDBNull(a5) Then If a5.ToString() <> "" Then result = result & a5.ToString() & ","
        result = Left(result, Len(result) - 1)
        Dim s As Array = Split(result, ",")
        result = ""
        For iLoop As Integer = 0 To UBound(s) - 1
            If iLoop > 0 Then result = result & "<br>"
            result = result & s(iLoop)
        Next
        If result = "" Then If Not IsDBNull(a1) Then If a1.ToString() <> "" Then result = result & a1.ToString()
        Return result
    End Function
    Protected Function formatPostcode(ByVal pc As Object, ByVal a2 As Object, ByVal a3 As Object, ByVal a4 As Object, ByVal a5 As Object) As String
        Dim result As String = ""
        Dim bFound As Boolean = False

        If Not IsDBNull(pc) Then result = result & pc.ToString() & ", "
        If Not IsDBNull(a5) Then
            If a5.ToString() <> "" Then
                result = result & a5.ToString() & ","
                bFound = True
            End If
        End If
        If Not (IsDBNull(a4)) And Not bFound Then
            If a4.ToString() <> "" Then
                result = result & a4.ToString() & ","
                bFound = True
            End If
        End If
        If Not (IsDBNull(a3)) And Not bFound Then
            If a3.ToString() <> "" Then
                result = result & a3.ToString() & ","
                bFound = True
            End If
        End If
        If Not (IsDBNull(a2)) And Not bFound Then
            If a2.ToString() <> "" Then
                result = result & a2.ToString() & ","
                bFound = True
            End If
        End If
        If Right(result, 2) = ", " Then
            result = Left(result, Len(result) - 2)
        Else
            result = Left(result, Len(result) - 1)
        End If

        Return result
    End Function
End Class
