Imports System.Data
Imports System.Data.SqlClient
Imports WebSupergoo.ABCpdf6
Imports System.IO

Partial Class maintenance_manufacturerDebitsPending
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _gvPending_vatPos As Integer = 8
    Private Const _gvPending_claimPos As Integer = 10
    Private Const _gvPending_completeDatePos As Integer = 12
    Private Const _gvPending_paidDatePos As Integer = 14
    Private Const _gvPending_commandPos As Integer = 16
    Private Const _gvComponent_qtyPos As Integer = 2


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")

        End If
    End Sub

    Protected Function showDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then
            result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        End If
        Return result
    End Function
    Protected Sub gvPending_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Hide certain columns if debit note is pending
        Dim gv As GridView = CType(sender, GridView)
        gv.Columns(_gvPending_claimPos).Visible = Not Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue))
        gv.Columns(_gvPending_claimPos + 1).Visible = Not Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue)) 'Hide spacer column
        gv.Columns(_gvPending_completeDatePos).Visible = Not Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue))
        gv.Columns(_gvPending_completeDatePos + 1).Visible = Not Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue)) 'Hide spacer column
        gv.Columns(_gvPending_paidDatePos).Visible = Not Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue))
        gv.Columns(_gvPending_paidDatePos + 1).Visible = Not Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue)) 'Hide spacer column
        gv.Columns(_gvPending_vatPos).Visible = Not Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue))
        gv.Columns(_gvPending_vatPos + 1).Visible = Not Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue)) 'Hide spacer column

        gv.Columns(_gvPending_commandPos).Visible = Convert.ToBoolean(Convert.ToInt16(drpType.SelectedValue))

    End Sub
    Protected Sub gvPending_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        panPrices.visible = True
        gvComponents.DataBind()
    End Sub
    Protected Sub btnSubmit_submit(ByVal sender As Object, ByVal e As EventArgs)
        'Add data to debit tables
        Dim iQty As Integer
        Dim dUnitPrice As Decimal
        Dim dVatRate As Decimal
        Dim dOrderTotal As Decimal = 0
        Dim txt As TextBox
        Dim debitID As String

        'Update DebitItem items
        For Each row As GridViewRow In gvComponents.Rows
            txt = row.FindControl("txtPrice")
            dUnitPrice = CType(txt.Text, Decimal)
            iQty = CType(row.Cells(_gvComponent_qtyPos).Text, Integer)
            dOrderTotal = dOrderTotal + CType(iQty * dUnitPrice, Decimal)
            updateItem(CType(gvComponents.DataKeys(row.RowIndex).Value, Integer), dUnitPrice, CType(gvPending.SelectedValue, Integer))
        Next
        'Update debit table
        txt = gvComponents.FooterRow.FindControl("txtVat")
        dVatRate = CType(txt.Text, Decimal)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDebitByIDUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@debitID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@debitVatRate", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@debitTotalClaim", SqlDbType.Decimal))
            .Parameters.Add(New SqlParameter("@debitInstructions", SqlDbType.VarChar, 1000))
            .Parameters("@debitID").Value = CType(gvPending.SelectedValue, Integer)
            .Parameters("@debitVatRate").Value = dVatRate
            .Parameters("@debitTotalClaim").Value = dOrderTotal
            .Parameters("@debitInstructions").Value = txtInstructions.text
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
        'Create/Save PDF
        SavePDF(gvPending.DataKeys(gvPending.SelectedIndex).Value)
        'clean up
        gvPending.SelectedIndex = -1
        gvPending.DataBind()
        gvComponents.DataBind()
        panPrices.Visible = False
    End Sub
    Protected Sub updateItem(ByVal componentID As Integer, ByVal unitPrice As Decimal, ByVal debitID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procDebitItemByIDUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@debitID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@componentID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@cost", SqlDbType.Decimal))
            .Parameters("@debitID").Value = debitID
            .Parameters("@componentID").Value = componentID
            .Parameters("@cost").Value = unitPrice
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
    End Sub
    Protected Sub SavePDF(ByVal debitID As String)
        Dim doc As New Doc
        doc.HtmlOptions.PageCacheEnabled = False
        doc.HtmlOptions.AddForms = False
        doc.HtmlOptions.AddLinks = False
        doc.HtmlOptions.AddMovies = False
        doc.HtmlOptions.FontEmbed = False
        doc.HtmlOptions.UseResync = False
        doc.HtmlOptions.UseVideo = False
        'doc.HtmlOptions.UseJava = False
        'doc.HtmlOptions.UseActiveX = False
        doc.HtmlOptions.UseScript = False
        doc.HtmlOptions.HideBackground = False
        doc.HtmlOptions.Timeout = 1000
        doc.HtmlOptions.LogonName = ""
        doc.HtmlOptions.LogonPassword = ""
        doc.Rect.SetRect(40, 72, 540, 720)
        'Add webpage
        Dim ID As Integer
        Dim file As String = "http://" & Request.ServerVariables("HTTP_HOST") & "/maintenance/manufacturerDebitsPDF.aspx?id=" & debitID
        Try
            ID = doc.AddImageUrl(file)
            'Now save it
            Try
                Dim pdfData() As Byte = doc.GetData()
                'Response.ContentType = "application/pdf"
                'Response.AddHeader("content-length", pdfData.Length.ToString())
                'Response.AddHeader("content-disposition", "attachment; filename=MyPDF.PDF")
                'pdfData now needs writing to a binary data stream
                'Response.BinaryWrite(pdfData)
                Dim fs As New FileStream("c:\inetpub\wwwroot\emotionalbliss\pdfs\PeartreeDebit" & debitID & ".pdf", FileMode.Create, FileAccess.Write)
                Dim bw As New BinaryWriter(fs)
                bw.Write(pdfData)
                bw.Close()
                bw = Nothing
                fs.Dispose()
                'lblPDFLink.Text = "PDF available here - <a href='..\pdfs\PeartreePurchaseOrder" & orderID & ".pdf' target='_blank'>PeartreePurchaseOrder" & orderID & ".pdf</a><br>"
            Catch ex As Exception
                Response.Write("Exception occured while trying to save the PFD Document.<br>" & ex.ToString)
            End Try
        Catch ex As Exception
            Response.Write("Exception occured while trying to attach webpage to PDF Maker.<br>" & ex.ToString)
            Response.Write("<br>" & file)
        Finally
            doc.Dispose()
        End Try
    End Sub
End Class
