Imports SYstem.Data
Imports System.data.SqlClient



Partial Class maintenance_manufacturerDebitsPDF
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            lblDebit.Text = Request.QueryString("id")
            bindItems()
            bindDebit()
        End If

    End Sub
    Protected Sub dvToAddress_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvToAddress.Rows
            row.Cells(0).Visible = False
            If row.Cells(1).Text = "-" Then row.Visible = False
        Next
    End Sub
    Protected Sub dvBilling_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvBilling.Rows
            row.Cells(0).Visible = False 'Hide 1st column as no column headers are being shown
            If row.Cells(1).Text = "*" Then row.Visible = False
        Next
    End Sub
    Protected Sub bindItems()
        'Populate items
        Dim oConn As New SqlConnection
        Dim oCmd As New SqlCommand
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim tRow As New TableRow
        Dim tCell As New TableCell
        Dim orderTotal As Decimal = 0

        oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        oCmd = New SqlCommand("procDebitComponentsSelect", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@debitID", SqlDbType.Int))
            .Parameters("@debitID").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            ds = New DataSet
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each rs As DataRow In ds.Tables(0).Rows
                    tRow = New TableRow
                    refresh(tCell)
                    tCell.Text = rs("qty")
                    tRow.Cells.Add(tCell)
                    refresh(tCell)
                    tCell.Text = rs("componentCode")
                    tRow.Cells.Add(tCell)
                    refresh(tCell)
                    tCell.Text = rs("componentName")
                    tRow.Cells.Add(tCell)
                    refresh(tCell)
                    tCell.Text = rs("cost")
                    tRow.Cells.Add(tCell)
                    refresh(tCell)
                    tCell.Text = rs("cost") * rs("qty")
                    tRow.Cells.Add(tCell)
                    tblItems.Rows.Add(tRow)
                    orderTotal = orderTotal + Convert.ToInt32(rs("qty")) * Convert.ToDecimal(rs("cost"))
                Next
                lblSubTotal.Text = FormatNumber(orderTotal, 2)
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
    Protected Sub bindDebit()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDebitByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim rs As DataRow
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@debitID", SqlDbType.Int))
            .Parameters("@debitID").Value = CType(Request.QueryString("id"), Integer)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                rs = ds.Tables(0).Rows(0)
                lblVatRate.Text = rs("debitVatRate") & "%"
                lblVatTotal.Text = FormatNumber((rs("debitTotalClaim") / 100) * rs("debitVatRate"), 2)
                lblNetAmount.Text = FormatNumber(rs("debitTotalClaim") * (1 + (rs("debitVatRate") / 100)), 2)
                lblCurrency.Text = UCase(rs("manufacturerCurrency"))
                lblCurrency2.Text = UCase(rs("manufacturerCurrency"))
                If IsDBNull(rs("debitInstructions")) Then
                    lblInstLabel.Visible = False
                Else
                    If rs("debitInstructions") = "" Then
                        Response.Write("No1")
                        lblInstLabel.Visible = False
                    Else
                        lblInstructions.Text = Replace(rs("debitInstructions"), Chr(13), "<br>")
                    End If
                End If
            End If
        Catch ex As Exception

        End Try


    End Sub
    Protected Sub refresh(ByRef cell)
        cell = New TableCell
    End Sub
End Class
