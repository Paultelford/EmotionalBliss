Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_statementDistInvoice
    Inherits BasePage
    Private _currency As String = "£"

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCurrencyByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = Request.QueryString("id")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                _currency = ds.Tables(0).Rows(0)("currencySign")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("affilaites/statementInvoice.aspx.vb", "Page_Init(orderid=" & Request.QueryString("id") & "); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function showOrderCode(ByVal nID As Object, ByVal cc As Object) As String
        Dim result As String = ""
        If Not IsDBNull(nID) Then result = nID.ToString
        'If Not IsDBNull(cc) Then result = result & UCase(cc.ToString)
        Return result
    End Function
    Protected Function showName(ByVal f As Object, ByVal s As Object) As String
        Dim first As String = ""
        Dim second As String = ""
        If Not IsDBNull(f) Then first = formatText(f.ToString)
        If Not IsDBNull(s) Then second = formatText(s.ToString)
        Return first & " " & second
    End Function
    Protected Function calcVat(ByVal goodsVat As Decimal, ByVal shipping As Decimal, ByVal shippingTotal As Decimal) As String
        Return FormatNumber(goodsVat + (shippingTotal - shipping), 2)
    End Function
    Protected Sub fvAffiliate_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Check that affiliate is looking at their own order (stop em fiddling with the orderid querystring)
        Dim hidAffiliateID As HiddenField = fvAffiliate.FindControl("hidAffiliateID")
        If CBool(Session("EBAffEBDistributor")) Then
            'User is Distributor, just check on country code
            Dim lblCountryCode As Label = fvAffiliate.FindControl("lblCountryCode")
            'If LCase(lblCountryCode.Text) <> LCase(Session("EBAffEbDistributorCountryCode")) And hidAffiliateID.Value <> Session("EBAffID") Then showAccessDenied()
            If Not (lcase(lblCountryCode.Text) = "zz" And hidAffiliateID.Value = Session("EBDistID")) Then showAccessDenied()
        Else
            'User is affiliate, check affID matches
            Try
                Dim lblAffID As Label = fvAffiliate.FindControl("lblAffID")
                If lblAffID.Text <> Session("EBDistID") Then showAccessDenied()
            Catch ex As Exception
                'Order must not be an affiliate order, and has thrown an exception, show access deinied error
                showAccessDenied()
            End Try
        End If
        'Add <br>'s to address lines in the formview
        Dim lbl As Label
        For iLoop As Integer = 1 To 5
            lbl = fvAffiliate.FindControl("lblAdd" & iLoop)
            If lbl.Text <> "" Then lbl.Text = lbl.Text & "<br>"
        Next
    End Sub
    Protected Sub showAccessDenied()
        fvAffiliate.Visible = False
        gvOrderItems.Visible = False
        fvTotals.Visible = False
        lblError.Text = "<font color='red'>Unable to retrieve details for this order.</font>"
    End Sub
    Protected Function formatText(ByVal text As String) As String
        'Formats the word to start with a capital letter and the rest lowercase
        Dim result As String = ""
        If Len(text) > 1 Then
            result = UCase(Left(text, 1))
            result = result & LCase(Right(text, Len(text) - 1))
        Else
            result = UCase(text)
        End If
        Return result
    End Function
    Protected Function showCurrency() As String
        Return _currency
    End Function
    Protected Function getCurrencySign(ByVal o As Object) As String
        Dim result As String = ""
        Dim si As New siteInclude
        Try
            result = si.getCurrencySignByCurrencyCode(o.ToString())
        Catch ex As Exception
        End Try
        Return result
    End Function
End Class
