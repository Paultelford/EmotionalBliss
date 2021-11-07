Imports System.data
Imports System.data.SqlClient

Partial Class affiliates_statementPaymentPopup
    Inherits BasePage
    Protected Sub fvDetails_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Check that affilaite is looking at correct details and hasnt messed with QS.
    End Sub
    Protected Sub btnUpload_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim berror As Boolean = False
        lblError.Text = ""
        If upFile.HasFile Then
            'Save file
            upFile.SaveAs("c:/inetpub/wwwroot/emotionalbliss2k8/uploads/" & upFile.FileName)
            'Add to database
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procAffStatementUploadInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@statementID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@filename", SqlDbType.VarChar, 100))
                .Parameters("@statementID").Value = Request.QueryString("sid")
                .Parameters("@filename").Value = upFile.FileName
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                berror = True
                lblError.Text = "An error occured while uploading the file. Please try again later.<br>Sorry for any inconvenience."
                Dim si As New siteInclude
                si.addError("maintenance/statementPaymentPop.aspx", "btnUpload_click(sid=" & Request.QueryString("sid") & ", filename=" & upFile.FileName & "); " & ex.ToString)
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If Not berror Then
                'Clean up and show success message
                lblError.Text = "Upload successful"
                gvFiles.DataBind()
            End If
        Else
            lblError.Text = "You must select a file to upload."
        End If
    End Sub
End Class
