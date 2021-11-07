Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_voucherCreate
    Inherits BasePage
    Private Const _gvProducts_vatPos As Integer = 6

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            'Get currency code
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procCurrencyByCountryCodeSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim currency As String = ""
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@currencyCode", SqlDbType.VarChar, 5))
                .Parameters("@currencyCode").Value = Session("EBAffCurrencyCode")
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    currency = ds.Tables(0).Rows(0)("currencySign")
                Else
                    currency = "Unknown currency"
                End If
            Catch ex As Exception
                lblError.Text = "An error occured while getting currency; " & ex.ToString
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            lblCurrencySign2.Text = currency
            lblCurrencySign3.Text = currency
            setDateDropdowns()
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        If isProductAssociated("single") Then
            lblError.Text = ""
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procVoucherCreateInsert", oConn)
            Dim voucherNumber As String = ""
            Dim bError As Boolean = False
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@credit", SqlDbType.Decimal))
                .Parameters.Add(New SqlParameter("@recipient", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@purchaser", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@comment", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@recipientEmail", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@purchaserEmail", SqlDbType.VarChar, 50))
                .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@distributorBuyingID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@productOnSaleID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@distributorID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@shopCountry", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@coupon", SqlDbType.Bit))
                .Parameters.Add(New SqlParameter("@voucherNumber", SqlDbType.VarChar, 10))
                .Parameters("@credit").Value = CType(txtAmount.Text, Decimal)
                .Parameters("@recipient").Value = txtRecipientName.Text
                .Parameters("@purchaser").Value = txtSenderName.Text
                .Parameters("@comment").Value = txtComment.Text
                .Parameters("@recipientEmail").Value = txtEmail.Text
                .Parameters("@purchaserEmail").Value = txtEmailSender.Text
                .Parameters("@currency").Value = Session("EBAffCurrencyCode")
                .Parameters("@distributorBuyingID").Value = 0
                .Parameters("@productOnSaleID").Value = 0
                .Parameters("@distributorID").Value = Session("EBAffID")
                .Parameters("@shopCountry").Value = Session("EBAffEBDistributorCountryCode")
                .Parameters("@coupon").Value = True
                .Parameters("@voucherNumber").Direction = ParameterDirection.Output
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
                voucherNumber = oCmd.Parameters("@voucherNumber").Value
            Catch ex As Exception
                lblError.Text = "An error occured while executing stored procedure; " & ex.ToString
                bError = True
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If Not bError Then
                'Assign buyable products to the newly created voucher
                linkProductsToVoucher(voucherNumber, "single")
                lblVoucherNumber.Text = voucherNumber
                lblAmount.Text = txtAmount.Text
                panCreate.Visible = False
                panComplete.Visible = True
            End If
        Else
            lblError.Text = "<font color='red'>You must associate the voucher with at least 1 product from the list abvove.</font>"
        End If
    End Sub
    Protected Sub btnSubmitMulti_click(ByVal sender As Object, ByVal e As EventArgs)
        'Test for valid date
        Dim endDate As Date
        Dim bError As Boolean = False
        lblError.Text = ""
        If Not IsDate(drpDay.SelectedValue & " " & drpMonth.SelectedValue & " " & drpYear.SelectedValue) Then
            lblErrorMulti.Text = " <font color='red'>* Invalid date</font>"
            bError = True
        Else
            lblErrorMulti.Text = ""
            endDate = CType(drpDay.SelectedValue & " " & drpMonth.SelectedValue & " " & drpYear.SelectedValue, Date)
            'Test that date has not already passed.
            If DateDiff(DateInterval.Day, Now(), endDate) < 1 Then
                lblErrorMulti.Text = " <font color='red'>* Date has passed</font>"
                bError = True
            End If
        End If
        If Not bError Then
            'Before creating voucher, make sure that at least 1 product has been associated
            If isProductAssociated("multi") Then
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procVoucherCouponCreateInsert", oConn)
                Dim voucherNumber As String = ""
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@credit", SqlDbType.Decimal))
                    .Parameters.Add(New SqlParameter("@comment", SqlDbType.VarChar, 4000))
                    .Parameters.Add(New SqlParameter("@currency", SqlDbType.VarChar, 5))
                    .Parameters.Add(New SqlParameter("@distributorBuyingID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@productOnSaleID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@distributorID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@active", SqlDbType.Bit))
                    .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
                    .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@shopCountry", SqlDbType.VarChar, 5))
                    .Parameters.Add(New SqlParameter("@voucherNumber", SqlDbType.VarChar, 10))
                    .Parameters.Add(New SqlParameter("@isPercent", SqlDbType.Bit))
                    .Parameters("@credit").Value = CType(txtAmount2.Text, Decimal)
                    .Parameters("@comment").Value = txtCommentMulti.Text
                    .Parameters("@currency").Value = Session("EBAffCurrencyCode")
                    .Parameters("@distributorBuyingID").Value = 0
                    .Parameters("@productOnSaleID").Value = 0
                    .Parameters("@distributorID").Value = Session("EBAffID")
                    .Parameters("@active").Value = CBool(drpActive.SelectedValue)
                    .Parameters("@endDate").Value = endDate
                    .Parameters("@affID").Value = CType(drpAffilaite.SelectedValue, Integer)
                    .Parameters("@shopCountry").Value = Session("EBAffEBDistributorCountryCode")
                    .Parameters("@isPercent").Value = chkIsPercent.Checked
                    .Parameters("@voucherNumber").Direction = ParameterDirection.Output
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    oCmd.ExecuteNonQuery()
                    voucherNumber = oCmd.Parameters("@voucherNumber").Value
                Catch ex As Exception
                    lblError.Text = "An error occured while creating coupon; " & ex.ToString
                    bError = True
                Finally
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
                If Not bError Then
                    'Assign buyable products to the newly created voucher
                    linkProductsToVoucher(voucherNumber, "multi")
                    'Update screen display with new info
                    lblVoucherNumber.Text = voucherNumber
                    lblAmount.Text = txtAmount.Text
                    panCreate.Visible = False
                    panCreateMulti.Visible = False
                    panComplete.Visible = True
                End If
            Else
                lblError.Text = "<font color='red'>You must associate the voucher with at least 1 product from the list abvove.</font>"
            End If
        End If
    End Sub
    Protected Sub drpType1_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        drpType2.SelectedValue = drpType1.SelectedValue
        If drpType1.SelectedValue = "0" Then
            panCreate.Visible = True
            panCreateMulti.Visible = False
        Else
            panCreate.Visible = False
            panCreateMulti.Visible = True
        End If
    End Sub
    Protected Sub drpType2_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        drpType1.SelectedValue = drpType2.SelectedValue
        If drpType1.SelectedValue = "1" Then
            panCreate.Visible = False
            panCreateMulti.Visible = True
        Else
            panCreate.Visible = True
            panCreateMulti.Visible = False
        End If
    End Sub
    Protected Sub setDateDropdowns()
        Dim y As Array = Split("Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec", ",")
        drpDay.AppendDataBoundItems = True
        drpMonth.AppendDataBoundItems = True
        drpYear.AppendDataBoundItems = True
        For iLoop As Integer = 1 To 31
            drpDay.Items.Add(New ListItem(iLoop, iLoop))
        Next
        For iLoop As Integer = 0 To 11
            drpMonth.Items.Add(New ListItem(y(iLoop), y(iLoop)))
        Next
        For iLoop As Integer = Now().Year To (Now().Year) + 5
            drpYear.Items.Add(New ListItem(iLoop, iLoop))
        Next
    End Sub
    Protected Sub gvProducts_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Dim chk As CheckBox
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If row.Cells(_gvProducts_vatPos).Text = "0.0" And LCase(Session("EBAffEBDistributorCountryCode")) <> "us" Then
                    chk = row.FindControl("chkBuyable")
                    chk.Visible = False
                End If
            End If
        Next
    End Sub
    Protected Sub linkProductsToVoucher(ByVal voucher As String, ByVal type As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As SqlCommand
        Dim chk As CheckBox
        Dim gv As GridView
        If Type = "multi" Then
            gv = panCreateMulti.FindControl("gvProducts1")
        Else
            gv = panCreate.FindControl("gvProducts")
        End If
        Try
            For Each row As GridViewRow In gv.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    chk = row.FindControl("chkBuyable")
                    If chk.Checked Then
                        oCmd = New SqlCommand("procVoucherPosLinkInsert", oConn)
                        With oCmd
                            .CommandType = CommandType.StoredProcedure
                            .Parameters.Add(New SqlParameter("@voucher", SqlDbType.VarChar, 10))
                            .Parameters.Add(New SqlParameter("@affPosID", SqlDbType.Int))
                            .Parameters("@voucher").Value = voucher
                            .Parameters("@affPosID").Value = gv.DataKeys(row.RowIndex).Value
                        End With
                        Try
                            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                            oCmd.ExecuteNonQuery()
                        Catch ex As Exception
                            lblError.Text = lblError.Text & "<br>An error occured; " & ex.ToString
                        Finally
                            oCmd.Dispose()
                        End Try
                    End If
                End If
            Next
        Catch ex As Exception
        Finally
            oConn.Dispose()
        End Try
    End Sub
    Protected Function isProductAssociated(ByVal type As String) As Boolean
        Dim result As Boolean = False
        Dim chk As CheckBox
        Dim gv As GridView
        If type = "multi" Then
            gv = panCreateMulti.FindControl("gvProducts1")
        Else
            gv = panCreate.FindControl("gvProducts")
        End If
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                chk = row.FindControl("chkBuyable")
                If chk.Checked Then
                    result = True
                End If
            End If
        Next
        Return result
    End Function
End Class
