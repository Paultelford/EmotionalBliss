Imports WebSupergoo.ABCpdf6
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentOrderLog
    Inherits System.Web.UI.Page
    Private Const _orderIDPos As Integer = 0
    Private Const _statusPos As Integer = 4
    Private Const _reCreatePos As Integer = 6

    Protected Sub gvOrders_dateBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnkDebit As HyperLink
        Dim lnk As HyperLink
        Dim lnkCancel As LinkButton
        For Each row As GridViewRow In gvOrders.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If LCase(row.Cells(_statusPos).Text) = "complete" Then
                    lnk = row.Cells(_orderIDPos).Controls(0)
                    lnkDebit = New HyperLink
                    lnkDebit.Text = "DebitNote"
                    lnkDebit.NavigateUrl = "../pdfs/PeartreeDebitNote" & lnk.Text & ".pdf"
                    row.Cells(_reCreatePos).Text = ""
                    row.Cells(_reCreatePos).Controls.Add(lnkDebit)
                End If
                If LCase(row.Cells(_statusPos).Text) = "cancelled" Then
                    lnkCancel = row.FindControl("lnkCancel")
                    lnkCancel.Text = ""
                End If
            End If
        Next
    End Sub
    Protected Sub gvOrders_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnkDebit As HyperLink
        Dim lnk As HyperLink = gvOrders.SelectedRow.Cells(_orderIDPos).Controls(0)
        gvOrders.SelectedRow.Cells(_reCreatePos).Text = SetComplete(Convert.ToInt32(lnk.Text))
        'lnk.NavigateUrl = ""
        gvOrders.SelectedIndex = -1
        For Each row As GridViewRow In gvOrders.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If row.Cells(_statusPos).Text = "Complete" Then
                    lnkDebit = New HyperLink
                    lnkDebit.Text = "DebitNote"
                    lnkDebit.NavigateUrl = "../pdfs/PeartreeDebitNote" & lnk.Text & ".pdf"
                    row.Cells(_reCreatePos).Text = ""
                    row.Cells(_reCreatePos).Controls.Add(lnkDebit)
                End If
            End If
        Next
    End Sub
    Protected Function SetComplete(ByVal orderID As Integer)
        SavePDF(orderID)
        Return "PDF Complete"
    End Function
    Protected Sub SavePDF(ByVal orderID)
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
        Dim rc As New Random
        Dim file As String = "http://" & Request.ServerVariables("HTTP_HOST") & "/maintenance/componentOrderPDF.aspx?id=" & orderID & "&rnd=" & rc.Next
        'Dim file As String = "http://google.com"
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
                Dim fs As New FileStream("c:\inetpub\wwwroot\emotionalbliss2k11\pdfs\PeartreePurchaseOrder" & orderID & ".pdf", FileMode.Create, FileAccess.Write)
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
    Protected Sub btnCancel_click(ByVal sender As Object, ByVal e As EventArgs)
        'Set order to cancelled
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentOrderStatusUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@compOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
            .Parameters("@compOrderID").Value = CInt(lblOrderIDCancel.Text)
            .Parameters("@status").Value = "Cancelled"
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = "An error occured while setting order status to 'Cancelled'."
            lblErrorDetail.Text = ex.ToString
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        gvOrders.DataBind()
        panCancel.Visible = False
    End Sub
    Protected Sub btnNo_click(ByVal sender As Object, ByVal e As EventArgs)
        panCancel.Visible = False
    End Sub
    Protected Sub lnkCancel_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnkBtn As LinkButton = CType(sender, LinkButton)
        panCancel.Visible = True
        lblOrderIDCancel.Text = lnkBtn.CommandArgument
    End Sub
End Class
