<%@ Application Language="VB" %>

<script runat="server">
    Public Shared PCServiceUsername As String = "emotionalbliss"
    Public Shared PCServicePassword As String = "m111tel"
    Public Shared isDevBox As Boolean = True

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application startup
        'Assume server is Live (As live server has been showing as Dev too many times after each reboot)
        Application("isDevBox") = False
        Application("isDev") = False
        Application("cacheMenus") = True
        Application("icepayMerchantID") = "14052"  '10457
        Application("icepayMerchantCode") = "Uf4m6QWy5a7ZRs93NdJj8p3XCe6n4H9MrKz7c8EP"  'f5MSk7t4W3Lcz9G8KpDs6q4CFy8x9TRw7g5X6Eed
        Dim oConn As New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New System.Data.SqlClient.SqlCommand("procServersSelect", oConn)
        Dim da As New System.Data.SqlClient.SqlDataAdapter
        Dim ds As New System.Data.DataSet
        With oCmd
            .CommandType = Data.CommandType.StoredProcedure
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New System.Data.SqlClient.SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If CBool(ds.Tables(0).Rows(0)("isDev")) Then
                    Application("cacheMenus") = False
                    Application("isDevBox") = True
                    Application("isDev") = True
                    Application("icepayMerchantID") = "10458"
                    Application("icepayMerchantCode") = "g9P8Qcq7Y5Apa4E6Tsk3XFm7z6RHb4w8DMe5n9G3"
                End If
            End If
        Catch ex As Exception
            siteInclude.addError("global.asax", "ApplicationStart() isDevBoxCode;" & ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'Register ABCpdf key
        'Dim doc As New WebSupergoo.ABCpdf6.Doc
        'Try
        '    If Not Application("isDevBox") Then doc.SetInfo(0, "License", "393-927-439-276-6085-841")
        'Catch ex As Exception
        'Finally
        '    doc.Dispose()
        'End Try

    End Sub
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
        'Empty EBCart table - Good housekeeping, and could help prevent incompatabilites whhen ebcart.cs is updated 
        Dim oConn As New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New System.Data.SqlClient.SqlCommand("procEBCartDelete", oConn)
        oCmd.CommandType = Data.CommandType.StoredProcedure
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            'error
            Dim si As New siteInclude
            si.addError("global.asax", "ApplicationStart() Empty EBCart Code;" & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a new session is started
        'Add visitor to the visitors table
        Dim aExclusions() As String
        aExclusions = New String() {"81.149.144.46", "62.3.233.47"}
        Dim oConn As New System.Data.SqlClient.SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New System.Data.SqlClient.SqlCommand("procVisitorsInsert", oConn)
        Dim target As String = Request.ServerVariables("URL")
        Dim referer As String = Request.ServerVariables("HTTP_REFERER")
        Dim qs As String = ""
        If Request.ServerVariables("QUERY_STRING") <> "" Then qs = "?" & Request.ServerVariables("QUERY_STRING")
        If InStr(LCase(Request.ServerVariables("HTTP_HOST")), "secure.") = 0 Then
            With oCmd
                .CommandType = Data.CommandType.StoredProcedure
                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@IP", Data.SqlDbType.VarChar, 16))
                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@referer", Data.SqlDbType.VarChar, 200))
                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@target", Data.SqlDbType.VarChar, 200))
                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@country", Data.SqlDbType.VarChar, 5))
                .Parameters.Add(New System.Data.SqlClient.SqlParameter("@bot", Data.SqlDbType.Bit))
                .Parameters("@IP").Value = Request.ServerVariables("REMOTE_ADDR")
                .Parameters("@target").Value = target & qs
                .Parameters("@country").Value = getCountryFromUrl(LCase(Request.ServerVariables("SERVER_NAME")))
                .Parameters("@bot").Value = CBool(InStr(LCase(Request.ServerVariables("http_user_agent")), "bot")) Or CBool(InStr(LCase(Request.ServerVariables("http_user_agent")), "spider")) Or CBool(InStr(LCase(Request.ServerVariables("http_user_agent")), "crawl"))
            End With
            If referer = "" Then
                oCmd.Parameters("@referer").Value = DBNull.Value
            Else
                oCmd.Parameters("@referer").Value = referer
            End If
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()

                If (Not InStr(target, "shop/payment.aspx?sid=")) And isIPOK(aExclusions) Then oCmd.ExecuteNonQuery()
            Catch ex As Exception
                'error
                siteInclude.addError("global.asax", "Session_Start(); " & ex.ToString)
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Function isIPOK(ByRef arr() As String)
        Dim result As Boolean = True
        Dim usersIP As String = Request.ServerVariables("REMOTE_ADDR")
        For iLoop As Integer = 0 To UBound(arr)
            If arr(iLoop) = usersIP Then result = False
        Next
        Return result
    End Function
    Protected Function getCountryFromUrl(ByVal svr As String) As String
        Dim countryCode As String = "gb"
        If InStr(svr, "uk") > 0 Then
            countryCode = "gb"
        Else
            Dim arr As Array = Split(svr, ".")
            For i As Integer = 0 To UBound(arr)
                If Len(Replace(Replace(arr(i), "http://", ""), "https://", "")) = 2 Then countryCode = arr(i)
            Next
        End If
        Return countryCode
    End Function

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when a session ends. 
        ' Note: The Session_End event is raised only when the sessionstate mode
        ' is set to InProc in the Web.config file. If session mode is set to StateServer 
        ' or SQLServer, the event is not raised.
    End Sub

</script>