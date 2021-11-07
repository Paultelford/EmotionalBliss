Imports System.Data

Partial Class affiliates_statementReceipt
    Inherits BasePage
    Private Const _imgPath As String = "userfiles\statementfiles\"

    'System
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        showFiles()
        lblError.Text = ""
    End Sub

    'Page
    Protected Sub fvDetails_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'If transaction is CR or DR then hide the Related Statement Entrys
        Dim fv As FormView = CType(sender, FormView)
        Dim lblPaymentRef As Label = fv.FindControl("lblPaymentRef")
        If Left(LCase(lblPaymentRef.Text), 2) = "cr" Or Left(LCase(lblPaymentRef.Text), 2) = "dr" Then panRelated.Visible = False
    End Sub
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
    Protected Sub gvEntrys_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As GridViewRow In gvEntrys.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If row.Cells(siteInclude.getGVRowByHeader(gvEntrys, "Credit")).Text = "0.00" And row.Cells(siteInclude.getGVRowByHeader(gvEntrys, "Debit")).Text = "0.00" Then row.Visible = False
            End If
        Next
    End Sub

        'User
    Protected Sub btnUpload_click(ByVal sender As Object, ByVal e As EventArgs)
        If fu1.HasFile Then
            Try
                'save file
                fu1.SaveAs(Request.PhysicalApplicationPath & _imgPath & fu1.FileName)
                'success, add to db
                Try
                    Dim param() As String = {"@statementID", "@filename"}
                    Dim paramValue() As String = {Request.QueryString("sid"), fu1.FileName}
                    Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar}
                    Dim paramSize() As Integer = {0, 100}
                    siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procAffiliateStatamentFilesInsert")
                    showFiles()
                Catch ex As Exception
                    siteInclude.addError("affiliates/statementReceipt.aspx.vb", "btnUplaod_click(filename=" & fu1.FileName & ", statementID=" & Request.QueryString("sid") & "); " & ex.ToString())
                End Try
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
        Else
            lblError.Text = "Please choose a file before clicking Upload."
        End If
    End Sub

    'Subs
    Protected Sub showFiles()
        'Clear the panel and grab list of files from db
        panFiles.Controls.Clear()
        Dim dt As New DataTable
        Dim lnk As HyperLink
        Dim lbl As New Label
        lbl.Text = "<br>"
        Try
            Dim param() As String = {"@statementID"}
            Dim paramValue() As String = {Request.QueryString("sid")}
            Dim paramType() As SqlDbType = {SqlDbType.Int}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procAffiliateFilesByStatementIDSelect")
            For Each row As DataRow In dt.Rows
                lnk = New HyperLink
                lnk.Text = row("filename") & "<br>"
                lnk.Target = "_blank"
                lnk.NavigateUrl = "http://" & Request.ServerVariables("SERVER_NAME")
                If Application("isDevBox") Then lnk.NavigateUrl = lnk.NavigateUrl & ":8888"
                lnk.NavigateUrl = lnk.NavigateUrl & "/" & Replace(_imgPath, "\", "/") & row("filename")
                panFiles.Controls.Add(lnk)
                'panFiles.Controls.Add(lbl)
            Next
        Catch ex As Exception
            siteInclude.addError("affiliates/statementReceipt.aspx.vb", "Page_Load()")
        Finally
            dt.Dispose()
        End Try
    End Sub
    Protected Function showOrderCode(ByVal nID As Object, ByVal cc As Object) As String
        Dim result As String = ""
        If Not IsDBNull(nID) Then result = nID.ToString
        If Not IsDBNull(cc) Then result = result & UCase(cc.ToString)
        Return result
    End Function
    Protected Sub showAccessDenied()
        fvAffiliate.Visible = False
        fvDetails.Visible = False
        'fvTotals.Visible = False
        lblError.Text = "<font color='red'>Unable to retrieve details for this order.</font>"
    End Sub
End Class
