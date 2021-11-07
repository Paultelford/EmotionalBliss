Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_scanEmails
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        lblError1.Text = ""
        lblError2.Text = ""
    End Sub
    Protected Sub drpEMailTemplate_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        FCKeditor1.Value = ""
        If drpEmailTemplate.SelectedValue <> "0" Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procScanEmailByCountryCodeTypeSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 50))
                .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
                .Parameters("@type").Value = drpEmailTemplate.SelectedValue
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    'FCKeditor1.Value = ds.Tables(0).Rows(0)("body")
                    FCKeditor1.Value = Replace(ds.Tables(0).Rows(0)("body"), Chr(13), "<br>")
                End If
            Catch ex As Exception
                lblError1.Text = "<font color='red'>" & ex.ToString & "</font>"
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        If drpEmailTemplate.SelectedValue <> "0" Then
            'Test for >4000 characters
            If Len(FCKeditor1.Value) > 4000 Then
                lblError2.Text = "<font color='red'>Max email length is 4000 characters, it is currently " & Len(FCKeditor1.Value) & " characters.</font>"
            Else
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procScanEmailByCountryCodeTypeUpdate", oConn)
                Dim bError As Boolean = False
                Dim body As String = Replace(FCKeditor1.Value, "<br>", Chr(13))
                body = Replace(body, "<br />", "")
                body = Replace(body, "<p>", "")
                body = Replace(body, "</p>", "")
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                    .Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 50))
                    .Parameters.Add(New SqlParameter("@body", SqlDbType.VarChar, 4000))
                    .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
                    .Parameters("@type").Value = drpEmailTemplate.SelectedValue
                    .Parameters("@body").Value = body
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                Catch ex As Exception
                    bError = True
                    lblError1.Text = "<font color='red'>" & ex.ToString & "</font>"
                Finally
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
                If Not bError Then
                    'Clean up
                    FCKeditor1.Value = ""
                    drpEmailTemplate.SelectedIndex = 0
                End If
            End If
        Else
            lblError1.Text = "<font color='red'>You must select a Template before saving.</font>"
        End If
    End Sub

End Class
