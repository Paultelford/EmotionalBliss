Imports System.Data
Imports System.Data.SqlClient

Partial Class notfound
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Grab requred server variable in order to determin country code
        Dim svrName As String = Request.ServerVariables("HTTP_HOST")
        Dim qs As String = Request.ServerVariables("QUERY_STRING")
        Dim loc As String = LCase(Replace(qs, "404;http://", ""))
        Dim bDataBaseMatch As Boolean = False
        'Test for country code passed, if so set country and language session variables
        loc = Replace(loc, svrName, "")
        loc = Replace(loc, ":80", "")
        loc = Replace(loc, "/", "")
        loc = Replace(loc, "callcentre", "")
        'loc now contains everything after 'http:xxx.ssssssssss.ccc/'
        If Len(loc) = 2 Then
            '2 digits found, could be country code. Lookup in database and return countrys default langugae if it exists
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters("@countryCode").Value = loc
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    bDataBaseMatch = True
                    Session("EBLanguage") = ds.Tables(0).Rows(0)("defaultLanguage")
                    Session("EBShopCountry") = ds.Tables(0).Rows(0)("countryCode")
                End If
            Catch ex As Exception
                Response.Write(ex.ToString)
                Response.End()
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If bDataBaseMatch Then
                If InStr(qs, "callcentre") Then
                    Server.Transfer("~/callcentre/default.aspx")
                Else
                    Server.Transfer("~/default.aspx")
                End If
            End If

        End If

    End Sub
End Class
