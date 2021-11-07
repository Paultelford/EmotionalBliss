Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentValue
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procComponentsByManIDValueSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@manID", SqlDbType.VarChar, 5))
                .Parameters("@manID").Value = drpMan.SelectedValue
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds, "values")
                If ds.Tables("values").Rows.Count > 0 Then
                    tblValues.visible = True
                    lblError.Text = ""
                    calculateValues(ds)
                Else
                    tblValues.visible = False
                    If drpMan.selectedindex > 0 Then lblError.Text = "No items in stock."
                End If
            Catch ex As Exception
                'Dim lblErrorEx As Label = update1.FindControl("lblErrorEx")
                'lblErrorEx.Text = ex.ToString
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
    Protected Sub calculateValues(ByRef ds As DataSet)
        Dim iCurrentCompID As Integer = 0
        Dim iCompValue As Decimal = 0
        Dim bFirstComponent As Boolean = True
        Dim qtyRemain As Integer = 0
        Dim compName As String = ""
        Dim currency As String = ""
        Dim stock As String = ""
        Dim Total As Decimal = 0
        For Each Row As DataRow In ds.Tables("values").Rows
            If iCurrentCompID <> Convert.ToInt32(Row("componentID")) Then
                'New component
                If Not bFirstComponent Then
                    'Show old previous components Values
                    addRow(compName, stock, Convert.ToString(iCompValue), False, currency)
                    Total = Total + iCompValue
                End If
                'Reset totals and start calculations on new comp
                iCompValue = 0
                compName = Row("componentName")
                stock = Row("stock")
                currency = Row("currencySign")
                qtyRemain = Convert.ToInt32(Row("stock"))
                iCurrentCompID = Convert.ToInt32(Row("componentID"))
            Else
                'Same component, new row
            End If
            'Add items from current row if qtyRemaining>0
            If bFirstComponent Then bFirstComponent = False
            If qtyRemain > 0 Then
                If Convert.ToInt32(Row("qtyReceived")) <= qtyRemain Then
                    qtyRemain = qtyRemain - Convert.ToInt32(Row("qtyReceived"))
                Else
                    Row("qtyReceived") = Convert.ToInt32(Row("qtyReceived")) - (Convert.ToInt32(Row("qtyReceived")) - qtyRemain)
                    qtyRemain = 0
                End If
                iCompValue = iCompValue + Convert.ToDecimal(Row("price")) * Convert.ToInt32(Row("qtyReceived"))
            End If
        Next
        'Show old previous components Values
        addRow(compName, stock, Convert.ToString(iCompValue), False, currency)
        'Show total
        Total = Total + iCompValue
        'addRow("Total", "", Convert.ToString(Total), True)
    End Sub
    Protected Sub addRow(ByVal compName As String, ByVal stock As String, ByVal compValue As String, ByVal footer As Boolean, ByVal currency As String)
        Dim tRow = New TableRow
        Dim tCell = New TableCell
        tCell.Text = compName
        If footer Then tCell.font.bold = True
        tRow.Cells.Add(tCell)
        tCell = New TableCell
        tCell.Text = stock
        tRow.Cells.Add(tCell)
        tCell = New TableCell
        tCell.Text = currency & FormatNumber(compValue, 2)
        If footer Then tCell.font.bold = True
        tRow.Cells.Add(tCell)
        tblValues.Rows.Add(tRow)
    End Sub
End Class
