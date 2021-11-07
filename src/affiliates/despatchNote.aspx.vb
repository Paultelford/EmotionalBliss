Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_despatchNote
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Membership.Provider.ApplicationName = "ebAffProvider"
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        lblError.Text = ""
    End Sub
    Protected Sub drpOrderNo_selcectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        If drp.SelectedValue <> "0" Then
            registerStartupJavascript(drp.SelectedValue, sender)
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Check for valid orderid
        'If a '/' was entered then parse it
        Dim arr() As String = Split(txtOrderNo.Text, "/")
        Dim orderNo As String = ""
        If UBound(arr) = 1 Then
            ' / found
            orderNo = arr(1)
        Else
            orderNo = txtOrderNo.Text
        End If
        'Remove countrycode if present
        orderNo = Replace(LCase(orderNo), LCase(Session("EBAffEBDistributorCountryCode")), "")
        'orderNo should now contain the User order number (5 digits, all numeric)
        If IsNumeric(orderNo) Then
            If Len(orderNo) > 4 And Len(orderNo) < 7 Then
                'All ok, check to see if number exists in database
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procShopOrderIDByNewOrderIDSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                    .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.Int))
                    .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
                    .Parameters("@newOrderID").Value = CInt(orderNo)
                End With
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        'Success
                        registerStartupJavascript(ds.Tables(0).Rows(0)("id"), sender)
                    Else
                        'Not found, invalid order number was entered by user
                        lblError.Text = "Order No not found."
                    End If

                Catch ex As Exception
                    lblError.Text = ex.ToString
                Finally
                    ds.Dispose()
                    da.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            Else
                'Invalid length
                lblError.Text = "Invalid Order No entered."
            End If
        Else
            lblError.Text = "Invalid data entry"
        End If
    End Sub
    Protected Sub sqlRecentOrders_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        drpOrderNo.Items.Clear()
        drpOrderNo.Items.Add(New ListItem("Select...", "0"))
    End Sub
    Protected Sub registerStartupJavascript(ByVal id As String, ByRef ctrl As Control)
        ScriptManager.RegisterStartupScript(ctrl, Me.GetType, "onloader", "self.setTimeout(""openPopup('" & id & "')"",200);", True)
    End Sub
    Protected Sub drpOutstandingQty_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        drpOrderNo.DataBind()
    End Sub
End Class
