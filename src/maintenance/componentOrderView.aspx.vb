Imports System.Data
Imports System.Data.SqlClient
Imports WebSupergoo.ABCpdf6
Imports System.IO

Partial Class maintenance_componentOrderView
    Inherits System.Web.UI.Page
    Private Const _componentNamePos = 0
    Private Const _qtyOrderedPos = 2
    Private Const _qtyOutstanding = 4
    Private Const _qtyReceived = 6
    Private Const _qtyRejected = 8
    Private Const _manIDPos = 10
    Private Const _compIDPos = 12

    Protected Function showOutstanding(ByVal qty As Integer, ByVal received As Integer) As Integer
        Return qty - received
    End Function
    Protected Sub gvComponents_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblOutstanding As Label = e.Row.Cells(_qtyOutstanding).FindControl("lblOutstanding")
            Dim pan1 As Panel = e.Row.Cells(_qtyReceived).FindControl("pan1")
            Dim pan2 As Panel = e.Row.Cells(_qtyReceived).FindControl("pan2")
            If lblOutstanding.Text = "0" Then
                pan1.Visible = False
                pan2.Visible = False
                gvComponents.HeaderRow.Cells(_qtyReceived).Text = ""
                gvComponents.HeaderRow.Cells(_qtyRejected).Text = ""
            End If
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Validate input
        Page.Validate()
        If Page.IsValid Then
            Dim bCreateBatch As Boolean = False
            Dim bErrorFound As Boolean = False
            Dim lblOutstanding As Label
            Dim txtReceived As TextBox
            Dim txtRejected As TextBox
            Dim oConn As SqlConnection
            Dim oCmd As SqlCommand
            For Each row As GridViewRow In gvComponents.Rows
                If Not bErrorFound Then
                    lblOutstanding = row.Cells(_qtyOutstanding).FindControl("lblOutstanding")
                    txtReceived = row.Cells(_qtyReceived).FindControl("txtReceived")
                    txtRejected = row.Cells(_qtyReceived).FindControl("txtRejected")
                    If Convert.ToInt32(lblOutstanding.Text) > 0 Then
                        If Not (IsNumeric(txtReceived.Text)) And (Not txtReceived.Text = "") Then
                            'Test for invalid input
                            bErrorFound = True
                            lblError.Text = "Invalid Qty Received in one of the input boxes."
                        Else
                            'Test Quantity Outstanding compared to value user entered
                            If txtReceived.Text = "" Then txtReceived.Text = "0"
                            If txtRejected.Text = "" Then txtRejected.Text = "0"
                            If Convert.ToInt32(txtReceived.Text) + Convert.ToInt32(txtRejected.Text) > Convert.ToInt16(lblOutstanding.Text) Then
                                bErrorFound = True
                                lblError.Text = "You cannot have received " & txtReceived.Text & " x " & row.Cells(_componentNamePos).Text & ", as only " & lblOutstanding.Text & " are outstanding."
                            End If
                        End If
                    End If
                End If
            Next
            If (Not bErrorFound) Then
                'Clear error msg
                lblError.Text = ""
                'Get details needed for the ComponentHistory table
                Dim iBatchID As Integer
                Dim lblMan As Label = gvComponents.Rows(0).Cells(_manIDPos).FindControl("lblMan")
                Dim lblComp As Label
                Dim iComponentID As Integer
                iBatchID = getNextComponentBatchID()
                'Add changes to DB (and add to new Component Batch)
                Dim compOrderItemID As Integer
                For Each row As GridViewRow In gvComponents.Rows
                    txtReceived = row.Cells(_qtyReceived).FindControl("txtReceived")
                    txtRejected = row.Cells(_qtyRejected).FindControl("txtRejected")
                    'Response.Write("a)" & txtReceived.Text & "<br>")
                    compOrderItemID = gvComponents.DataKeys(row.RowIndex).Value
                    lblComp = row.Cells(_compIDPos).FindControl("lblComp")
                    iComponentID = Convert.ToInt16(lblComp.Text)
                    'Response.Write(IsNumeric(txtReceived.Text) & " - " & Not txtReceived.Text = "")
                    'If (IsNumeric(txtReceived.Text)) And (Not txtReceived.Text = "") Then
                    If Convert.ToInt32(txtReceived.Text) > 0 Then
                        setAsReceived(Convert.ToInt32(compOrderItemID), Convert.ToInt32(txtReceived.Text), 1, Convert.ToInt32(lblComp.Text), iBatchID, 0)
                    End If
                    setAsReceived(Convert.ToInt32(compOrderItemID), 0, 11, Convert.ToInt32(lblComp.Text), iBatchID, Convert.ToInt32(txtRejected.Text))
                Next
            End If
            'If All ok, refresh screen
            If Not bErrorFound Then
                Dim bComplete As Boolean = True
                gvComponents.DataBind()
                'Update order ststus to Complete or PartComplete.
                'If any received textboxes are visible, then it should be partComplete, else complete.
                For Each row As GridViewRow In gvComponents.Rows
                    If row.RowType = DataControlRowType.DataRow Then
                        txtReceived = row.Cells(_qtyReceived).FindControl("txtReceived")
                        If txtReceived.Visible Then bComplete = False
                    End If
                Next
                oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                oCmd = New SqlCommand("procComponentOrderByOrderIDStatusUpdate", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@compOrderID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@status", SqlDbType.VarChar, 20))
                    .Parameters("@compOrderID").Value = Convert.ToInt32(Request.QueryString("id"))
                    .Parameters("@status").Value = "PartComplete"
                End With

                If bComplete Then
                    oCmd.Parameters("@status").Value = "Complete"
                    SavePDF(Convert.ToInt32(Request.QueryString("id")))
                End If

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
            End If
        End If 'page.isValid
    End Sub
    Protected Sub setAsReceived(ByVal compOrderItem As Integer, ByVal qty As Integer, ByVal actionID As Integer, ByVal compID As Integer, ByVal batchID As Integer, ByVal rejectedQty As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentOrderItemByIDReceivedUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@compOrderItemID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
            .Parameters("@compOrderItemID").Value = compOrderItem
            .Parameters("@qty").Value = Convert.ToInt32(qty + rejectedQty)
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
        'Now add this change of 'goods received' to the corresponding Batch/History.
        Try
            siteInclude.addToComponentHistory(batchID, qty, 0, 0, 0, 0, 0, rejectedQty, 0, actionID, Request.QueryString("id"), compID, "", Membership.GetUser.UserName, True)
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        End Try
    End Sub
    Protected Function getNextComponentBatchID() As Integer
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procComponentHistoryNextBatchIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As Integer = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If Not IsDBNull(ds.Tables(0).Rows(0)("compBatchID")) Then result = ds.Tables(0).Rows(0)("compBatchID")
            End If
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result + 1
    End Function
    Protected Sub SavePDF(ByVal orderID As Integer)
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
        Dim file As String = "http://" & Request.ServerVariables("HTTP_HOST") & "/maintenance/componentOrderDebitNotePDF.aspx?id=" & orderID
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
                Dim fs As New FileStream("c:\inetpub\wwwroot\emotionalbliss\pdfs\PeartreeDebitNote" & orderID & ".pdf", FileMode.Create, FileAccess.Write)
                Dim bw As New BinaryWriter(fs)
                bw.Write(pdfData)
                bw.Close()
                bw = Nothing
                fs.Dispose()
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
