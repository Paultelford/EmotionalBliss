Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_statementEarnings
    Inherits BasePage
    Private _affDiscount As Decimal = 0
    Private Const _gvItems_unitPricePos As Integer = 2
    Private Const _gvItems_qtyPos As Integer = 4
    Private Const _gvItems_discountPos As Integer = 6

    Protected Sub Page_Load(ByVal Sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Get affiliates discount
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procAffiliateClickThroughByOrderIDSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim bError As Boolean = False
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
                    'Make sure user is not messing around with querystrings
                    If Session("EBAffEBDistributor") Then
                        'Check country codes match
                        If LCase(Session("EBAffEBDistributorCountryCode")) <> ds.Tables(0).Rows(0)("orderCountryCode") Then bError = True
                    Else
                        'Check that clickthrough is for this affiliate
                        If Session("EBAffID") <> ds.Tables(0).Rows(0)("affID") Then bError = True
                    End If
                    If Not bError Then
                        _affDiscount = ds.Tables(0).Rows(0)("affClickThrough")
                    Else
                        lblError.Text = "<font color='red'>You do not have permission to view this page.</font>"
                        gvItems.Visible = False
                    End If
                End If
            Catch ex As Exception
                Dim si As siteInclude
                si.addError("affiliates/statementEarnings.aspx.vb", "Page_Load(); " & ex.ToString)
                lblError.Text = "<font color='red'>An error occured whlie retrieving details. Sorry for any inconvenience.</font>"
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Sub gvItems_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lblEarnings As Label
        Dim total As Decimal = 0
        Dim bRowsFound As Boolean = False
        Dim lblEarningsTotal As Label
        For Each row As GridViewRow In gvItems.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If Not bRowsFound Then lblEarningsTotal = gvItems.FooterRow.FindControl("lblEarningsTotal")
                bRowsFound = True
                lblEarnings = row.FindControl("lblEarnings")
                total = total + FormatNumber(lblEarnings.Text, 2)
                'Show currency 
                row.Cells(_gvItems_unitPricePos).Text = Session("EBAffCurrencySign") & row.Cells(_gvItems_unitPricePos).Text
                lblEarnings.Text = Session("EBAffCurrencySign") & lblEarnings.Text
            End If
        Next
        If bRowsFound Then
            lblEarningsTotal.Text = Session("EBAffCurrencySign") & CStr(FormatNumber(total, 2)) 'Show overall earnings
        Else
            lblError.Text = "<font color='red'>Details of this transaction do not exist. We are sorry for any inconvenience.</font>"
        End If

    End Sub
    Protected Function showEarnings(ByRef o As Object) As String
        Dim result As Decimal = "0"
        If Not IsDBNull(o) Then result = FormatNumber(o.ToString, 2)
        Return FormatNumber(result, 2)
    End Function
    Protected Sub fvAffiliate_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Check that affiliate is looking at their own order (stop em fiddling with the orderid querystring)
        If CBool(Session("EBAffEBDistributor")) Then
            'User is Distributor, just check on country code
            Dim lblCountryCode As Label = fvAffiliate.FindControl("lblCountryCode")
            'If LCase(lblCountryCode.Text) <> LCase(Session("EBAffEbDistributorCountryCode")) Then showAccessDenied()
        Else
            'User is affiliate, check affID matches
            Try
                Dim lblAffID As Label = fvAffiliate.FindControl("lblAffID")
                If lblAffID.Text <> Session("EBAffID") Then showAccessDenied()
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
    Protected Function showOrderCode(ByVal nID As Object, ByVal cc As Object) As String
        Dim result As String = ""
        If Not IsDBNull(nID) Then result = nID.ToString
        If Not IsDBNull(cc) Then result = result & UCase(cc.ToString)
        Return result
    End Function
    Protected Sub showAccessDenied()
        fvAffiliate.Visible = False
        gvItems.Visible = False
        'fvTotals.Visible = False
        lblError.Text = "<font color='red'>Unable to retrieve details for this order.</font>"
    End Sub
End Class
