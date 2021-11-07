Imports System.Data
Imports System.Data.SqlClient
Imports WebSupergoo.ABCpdf6
Imports System.IO

Partial Class maintenance_componentOrderPrint
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private orderID As Integer

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            orderID = Request.QueryString("id")
            Dim orderTotal As Decimal = 0
            Dim delivery As Decimal = 0
            'Dim lblSub
            lblPurchaseOrder.Text = orderID
            'Populate the address of company the order is being placed with
            Dim oConn As SqlConnection
            Dim oCmd As SqlCommand
            Dim da As SqlDataAdapter
            Dim ds As New DataSet
            oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            oCmd = New SqlCommand("procManufacturerByOrderIDAddressSelect", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                .Parameters("@orderID").Value = orderID
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                dvToAddress.DataSource = ds
                dvToAddress.DataBind()
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            'Populate items
            Dim tRow As New TableRow
            Dim tCell As New TableCell
            oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            oCmd = New SqlCommand("procComponentOrderItemsByOrderIDSelect", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
                .Parameters("@orderID").Value = orderID
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
                        tCell.Text = rs("componentCode")
                        tRow.Cells.Add(tCell)
                        refresh(tCell)
                        tCell.Text = rs("componentName")
                        tRow.Cells.Add(tCell)
                        refresh(tCell)
                        tCell.Text = rs("qty")
                        tRow.Cells.Add(tCell)
                        refresh(tCell)
                        tCell.Text = rs("price")
                        tRow.Cells.Add(tCell)
                        tblItems.Rows.Add(tRow)
                        orderTotal = orderTotal + Convert.ToInt32(rs("qty")) * Convert.ToDecimal(rs("price"))
                        delivery = Convert.ToDecimal(rs("delivery"))
                    Next
                    lblSubTotal.Text = FormatNumber(orderTotal + delivery, 2)
                    lblDelivery.text = FormatNumber(delivery, 2)
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
        End If
        SavePDF()
    End Sub
    Protected Sub dvToAddress_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvToAddress.Rows
            row.Cells(0).Visible = False
            If row.Cells(1).Text = "-" Then row.Visible = False
        Next
    End Sub
    Protected Sub refresh(ByRef cell)
        cell = New TableCell
    End Sub
    Protected Sub SavePDF()
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
        Dim file As String = "http://" & Request.ServerVariables("HTTP_HOST") & "/maintenance/componentOrderPDF.aspx?id=" & orderID
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
                Dim fs As New FileStream("c:\inetpub\wwwroot\" & getWebsite & "\pdfs\PeartreePurchaseOrder" & orderID & ".pdf", FileMode.Create, FileAccess.Write)
                Dim bw As New BinaryWriter(fs)
                bw.Write(pdfData)
                bw.Close()
                bw = Nothing
                fs.Dispose()
                lblPDFLink.Text = "PDF available here - <a href='..\pdfs\PeartreePurchaseOrder" & orderID & ".pdf' target='_blank'>PeartreePurchaseOrder" & orderID & ".pdf</a><br>"
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
    Protected Function getWebsite()
        Dim result As String = ""
        Dim arr As Array = Split(Request.ServerVariables("PATH_TRANSLATED"), "\")
        Try
            result = arr(3)
        Catch ex As Exception
            result = "emotionalbliss2k8"
        End Try
        Return result
    End Function
    Protected Sub dvShipping_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvShipping.Rows
            row.Cells(0).Visible = False 'Hide 1st column as no column headers are being shown
            If row.Cells(1).Text = "*" Then row.Visible = False
        Next
    End Sub
    Protected Sub dvBilling_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As DetailsViewRow In dvBilling.Rows
            row.Cells(0).Visible = False 'Hide 1st column as no column headers are being shown
            If row.Cells(1).Text = "*" Then row.Visible = False
        Next
    End Sub
End Class
