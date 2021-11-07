Imports System.Data
Imports System.Data.SqlClient
Imports WebSupergoo.ABCpdf6
Imports System.IO

Partial Class maintenance_productAssemblyPDF
    Inherits System.Web.UI.Page
    'Private _login As LoginView
    'Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            '_login = Master.FindControl("logMaintenance")
            '_content = _login.FindControl("ContentPlaceHolder1")
            If Request.QueryString("showPrint") = "true" Then btnPrint.Visible = True
            'Populate assembly info
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductionBatchByBatchIDDetailsSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@batchID", SqlDbType.Int))
                .Parameters("@batchID").Value = Request.QueryString("id")
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds, "assembly")
                If ds.Tables("assembly").Rows.Count > 0 Then
                    Dim rs As DataRow = ds.Tables("assembly").Rows(0)
                    If Not IsDBNull(rs("productionID")) Then lblBatch.Text = rs("productionID")
                    If Not IsDBNull(rs("productName")) Then lblProduct.Text = rs("productName")
                    If Not IsDBNull(rs("productionComments")) Then lblComments.Text = rs("productionComments")
                    If Not IsDBNull(rs("productionStartDate")) Then lblDate.Text = FormatDateTime(rs("productionStartDate"), DateFormat.LongDate) & " " & FormatDateTime(rs("productionStartDate"), DateFormat.ShortTime)
                    If Not IsDBNull(rs("productionAmount")) Then lblQty.Text = rs("productionAmount")
                    If Not IsDBNull(rs("ref")) Then lblRef.Text = rs("ref")
                    If Not IsDBNull(rs("productionUser")) Then lblUser.Text = rs("productionUser")
                    fillGridView(ds)
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
        'Create PDF if needed
        If Request.QueryString("createPDF") = "true" Then
            'createPDF()
            btnPrint.Visible = True
        End If

    End Sub
    Protected Sub createPDF()
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
        doc.Rect.SetRect(40, 72, 520, 640)
        'Add webpage
        Dim ID As Integer
        Dim file As String = "http://" & Request.ServerVariables("HTTP_HOST") & "/maintenance/productAssemblyPDF.aspx?id=" & Request.QueryString("id")
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
                Dim fs As New FileStream("c:\inetpub\wwwroot\emotionalbliss\pdfs\prodAssembly" & Request.QueryString("id") & ".pdf", FileMode.Create, FileAccess.Write)
                Dim bw As New BinaryWriter(fs)
                bw.Write(pdfData)
                bw.Close()
                bw = Nothing
                fs.Dispose()
                'lblPDFLink.Text = "PDF available here - <a href='..\pdfs\compOrder" & Request.QueryString("id") & ".pdf' target='_blank'>compOrder" & Request.QueryString("id") & ".pdf</a><br>"
                'Register JS to show an alert, informing the user that a pdf has been created
                'Page.ClientScript.RegisterStartupScript(Me.GetType(), "myAlert", "alert('A PDF has been created. Please click the Print button to make a hard copy.');")
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
    Protected Sub fillGridView(ByRef ds As DataSet)
        'For Each row As DataRow In ds.Tables("assembly").Rows
        'Next
        gvItems.DataSource = ds
        gvItems.DataBind()
    End Sub
    Protected Sub btnPrint_click(ByVal sender As Object, ByVal e As EventArgs)
        btnPrint.Visible = False
    End Sub
End Class
