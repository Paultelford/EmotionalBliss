Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_cataloguePrintPop
    Inherits BasePage
    Private Const _maxAddressesPerPage As Integer = 14

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            showData()
        End If
    End Sub
    Protected Sub showData()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCatalogueRequestByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim iCount As Integer = 0
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
        oCmd.Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
        tbl.Rows.Clear()
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                Dim tr As New TableRow
                Dim td As TableCell
                Dim col1 As Boolean = True
                For Each row As DataRow In ds.Tables(0).Rows
                    iCount = iCount + 1
                    If iCount <= _maxAddressesPerPage Then
                        If col1 Then tr = New TableRow
                        hidIDs.Value = hidIDs.Value & row("id") & ","
                        td = New TableCell
                        td.Height = "130"
                        td.Width = Unit.Percentage(50)
                        td.HorizontalAlign = HorizontalAlign.Left
                        td.Attributes.Add("onclick", "editAddress(" & row("id") & ")")
                        td.Attributes.Add("style", "cursor:hand;")
                        If Not IsDBNull(row("name")) Then td.Text = td.Text & row("name") & "<br>"
                        If Not IsDBNull(row("add1")) Then td.Text = td.Text & row("add1") & "<br>"
                        If Not IsDBNull(row("add2")) Then td.Text = td.Text & row("add2") & "<br>"
                        If Not IsDBNull(row("add3")) Then td.Text = td.Text & row("add3") & "<br>"
                        If Not IsDBNull(row("add4")) Then td.Text = td.Text & row("add4") & "<br>"
                        If Not IsDBNull(row("add5")) Then td.Text = td.Text & row("add5") & "<br>"
                        If Not IsDBNull(row("postcode")) Then td.Text = td.Text & row("postcode") & "<br>"
                        tr.Cells.Add(td)
                        'Response.Write("added<br>")
                        If Not col1 Then
                            tbl.Rows.Add(tr)
                            col1 = True
                        Else
                            col1 = False
                        End If
                    End If
                Next
                If iCount > 0 Then hidIDs.Value = Left(hidIDs.Value, Len(hidIDs.Value) - 1)
                If Not col1 Then
                    'Table ended on an odd record count, and final row was not added to table. Add it now
                    td = New TableCell
                    td.Text = "&nbsp;"
                    tr.Cells.Add(td)
                    tbl.Rows.Add(tr)
                End If
            Else
                lblError.Text = "No catalogue requests found"
                btnSend.Visible = False
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnSend_click(ByVal sender As Object, ByVal e As EventArgs)
        'hidIDs contains a comma seperated list of IDs that appear on page, and should now be set to sent=1 in the db
        Dim aID() As String = Split(hidIDs.Value, ",")
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Try
            For iLoop As Integer = 0 To UBound(aID)
                sendCatalogue(aID(iLoop), oConn)
            Next
        Catch ex As Exception
            lblError.Text = lblError.Text & ex.ToString & "<br><br>"
        Finally
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub sendCatalogue(ByVal id As String, ByRef oConn As SqlConnection)
        Dim oCmd As New SqlCommand("procCatalogueRequestByIDUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters("@id").Value = CInt(id)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = lblError.Text & ex.ToString & "<br><br>"
        Finally
            oCmd.Dispose()
        End Try
    End Sub
End Class
