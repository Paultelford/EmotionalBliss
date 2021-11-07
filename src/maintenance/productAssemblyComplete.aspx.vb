Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class maintenance_productAssemblyComplete
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _gvProduction_QtyPos = 4
    Private Const _gvScrap_QtyPos = 2
    Private Const _gvScrap_NamePos = 0
    Private Const _gvComplete_startDatePos = 2
    Private Const _gvComplete_endDatePos = 3
    Private Const _gvComplete_commandPos = 8
    Private Const _gvComplete_printPos = 7
    Private Const _gvDataEntry_qtyPos As Integer = 2
    Private iMasterID As Integer = 0
    Private alreadyCalculated As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            lblError.Text = ""
            panOutstanding.Visible = False
            If drpType.SelectedValue = "outstanding" Then panOutstanding.Visible = Not panOutstanding.Visible
            panComplete.Visible = Not panOutstanding.Visible
        End If
    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
        If panOutstanding.Visible = True Then
            gvProduction.DataBind()
        Else
            gvComplete.DataBind()
        End If
    End Sub
    Protected Sub gvProduction_selectedIndexChanging(ByVal sender As Object, ByVal e As GridViewSelectEventArgs)
        pan1.Visible = True
        Pan2.Visible = False
        gvProduction.Rows(e.NewSelectedIndex).BackColor = Drawing.Color.Aqua
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Go through pass and fail boxes and replace null string with 0
        If txtPass.Text = "" Then txtPass.Text = "0"
        If TxtFail.Text = "" Then TxtFail.Text = "0"
        'Add pass and fail, then check it matches productionAmount
        Dim Amount As Integer = Convert.ToInt32(gvProduction.Rows(gvProduction.SelectedIndex).Cells(_gvProduction_QtyPos).Text)
        Dim Sum As Integer = Convert.ToInt32(txtPass.Text) + Convert.ToInt32(TxtFail.Text)
        If Sum = Amount Then
            
            '################################## STILL TO DO ##############
            'Do something with the failed products, maybe ask user wether the components are to be scrapped or added back to quarantine.
            'If Convert.ToInt32(TxtFail.Text) > 0 Then
            lblFailed.Text = txtFail.Text
            lblProductAssemblyID.Text = Convert.ToString(gvProduction.SelectedValue)
            'Pan2.Visible = True
            'Else
            'Process data
            'Update productionBatch with pass/fail/endDate
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductionBatchUpdate", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@productionID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@pass", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@fail", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@user", SqlDbType.VarChar, 25))
                .Parameters("@productionID").Value = Convert.ToInt32(gvProduction.SelectedValue)
                .Parameters("@pass").Value = Convert.ToInt32(txtPass.Text)
                .Parameters("@fail").Value = Convert.ToInt32(txtFail.Text)
                .Parameters("@user").Value = Membership.GetUser.UserName
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
            'Add to productHistory
            Dim lblProdID As Label = gvProduction.SelectedRow.FindControl("lblProductID")
            Dim eb As New siteInclude
            Try
                'eb.addToProductHistory(Convert.ToInt32(lblProdID.Text), Convert.ToInt32(txtPass.Text), 0, Convert.ToInt32(txtFail.Text), 0, 1, Convert.ToInt32(gvProduction.SelectedValue), "", Membership.GetUser.UserName, 0, 0, 0, Convert.ToInt32(txtPass.Text) + Convert.ToInt32(txtFail.Text)) - This bad boy was removing products from 'In Production', which it has nothing to do with at all. 'In Production' is for Warehouse Products.
                eb.addToProductHistory(Convert.ToInt32(lblProdID.Text), Convert.ToInt32(txtPass.Text), 0, Convert.ToInt32(txtFail.Text), 0, 1, Convert.ToInt32(gvProduction.SelectedValue), "", Membership.GetUser.UserName, 0, 0, 0, 0)
            Catch ex As Exception
                Response.Write(ex)
                Response.End()
            End Try


            'Mark componets as 'Processed=True' in componentHistory table, thus removing them from 'InProduction'
            oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            oCmd = New SqlCommand("procComponentHistoryProductionUpdate", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@prodAssemblyID", SqlDbType.Int))
                .Parameters("@prodAssemblyID").Value = Convert.ToInt32(gvProduction.SelectedValue)
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

            'End If


            '#############################################################

            'All ok, hide the panel and reset the gridview (remove selection)
            'txtPass.Text = ""
            'TxtFail.Text = ""
            pan1.Visible = False
            gvProduction.DataBind()
            'open the AssemblyCompleteView.aspx popup
            ScriptManager.RegisterStartupScript(Me, Me.GetType(), "viewpopup", "self.setTimeout(openViewPop(" & lblProductAssemblyID.Text & "),100);", True)
            drpType.SelectedValue = "complete"
            panComplete.Visible = True
            panOutstanding.Visible = False
            gvComplete.DataBind()
            'If there were failures show the printout
            If Convert.ToInt32(txtFail.Text) > 0 Then ScriptManager.RegisterStartupScript(Me, Me.GetType(), "printpopup", "self.setTimeout(openPrintPop(" & lblProductAssemblyID.Text & "),200);", True)
        Else
            lblError.Text = "The Pass and Fail boxes must add up to " & gvProduction.Rows(gvProduction.SelectedIndex).Cells(_gvProduction_QtyPos).Text & " products."
        End If
    End Sub
    Protected Sub btnScrap_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim txtRecycle As TextBox
        Dim txtScrap As TextBox
        Dim iTarget As Integer
        Dim lblStockRemoved As Label
        Dim allocationError As Boolean = False
        Dim allocationErrorItems As String = ""
        Dim multipleMasterComponents As Boolean = False
        Dim oConn As SqlConnection
        Dim oCmd As SqlCommand
        For Each row As GridViewRow In gvScrap.Rows
            If row.RowType = DataControlRowType.DataRow Then
                'validate input
                txtRecycle = row.FindControl("txtRecycle")
                txtScrap = row.FindControl("txtScrap")
                If txtRecycle.Text = "" Or Not IsNumeric(txtRecycle.Text) Then txtRecycle.Text = "0"
                If txtScrap.Text = "" Or Not IsNumeric(txtScrap.Text) Then txtScrap.Text = "0"
                If InStr(row.Cells(_gvScrap_QtyPos).Text, "(") Then
                    multipleMasterComponents = True
                    'A bracket is in the qty field, use value from 'qtyStockRemoved' template field instead
                    lblStockRemoved = row.FindControl("lblStockRemoved")
                    iTarget = Convert.ToInt32(lblStockRemoved.Text)
                    If Convert.ToInt32(txtRecycle.Text) + Convert.ToInt32(txtScrap.Text) > iTarget Then
                        allocationError = True
                        allocationErrorItems = allocationErrorItems + " " & row.Cells(_gvScrap_NamePos).Text & ","
                    End If
                Else
                    iTarget = Convert.ToInt32(row.Cells(_gvScrap_QtyPos).Text)
                    'Check to qty missmatch
                    If iTarget <> Convert.ToInt32(txtRecycle.Text) + Convert.ToInt32(txtScrap.Text) Then
                        allocationError = True
                        allocationErrorItems = allocationErrorItems + " " & row.Cells(_gvScrap_NamePos).Text & ","
                    End If
                End If


            End If
        Next

        If allocationError Then
            lblAllocationError.Text = "<font color='red'>The following items do not add up...." & Left(allocationErrorItems, Len(allocationErrorItems) - 1) & "</font>"
        Else
            Dim multipleError As Boolean = False
            Dim multipleErrorItems As String = ""
            If multipleMasterComponents Then
                'All ok so far, now test for components with the same masterID
                Dim rowCount As Integer = gvScrap.Rows.Count
                Dim lblMasterID As Label
                iMasterID = 0
                For Each row As GridViewRow In gvScrap.Rows
                    If InStr(row.Cells(_gvScrap_QtyPos).Text, "(") Then
                        lblMasterID = row.FindControl("lblMasterID")
                        If Convert.ToInt32(lblMasterID.Text) <> iMasterID Then
                            'New masterID found, Call sub to check components
                            If Not addMasterComponentsOK(Convert.ToInt32(lblMasterID.Text), row.RowIndex, rowCount) Then
                                multipleError = True
                                multipleErrorItems = multipleErrorItems + " " & row.Cells(_gvScrap_NamePos).Text & ","
                            End If
                        End If
                        iMasterID = Convert.ToInt32(lblMasterID.Text)
                    End If
                Next
            End If
            If multipleError Then
                lblAllocationError.Text = "<font color='red'>You are trying to process more items than were originally used, check the values for the following ....." & Left(multipleErrorItems, Len(multipleErrorItems) - 1) & "</font>"
            Else
                'All ok, both validation methods passed.
                'Add recycled components into quarantine, and scrapped ones back to stock (as scrapped)
                Dim eb As New siteInclude
                For Each row As GridViewRow In gvScrap.Rows
                    txtRecycle = row.FindControl("txtRecycle")
                    txtScrap = row.FindControl("txtScrap")
                    If Convert.ToInt32(txtRecycle.Text) > 0 Then
                        'Add recycled components
                        'eb.addToComponentHistory(0, Convert.ToInt32(txtRecycle.Text), 0, 0, 0, 0, 0, 0, 0, 4, 0, gvScrap.DataKeys(row.RowIndex).Value, "Component Recycled", Membership.GetUser.UserName, True, lblProductAssemblyID.Text, False)
                        '22-2-07 - Recycled compoetns are now to be placed directly into stock and not quarantine.
                        eb.addToComponentHistory(0, 0, 0, Convert.ToInt32(txtRecycle.Text), 0, 0, 0, 0, 0, 4, 0, gvScrap.DataKeys(row.RowIndex).Value, "Component Recycled", Membership.GetUser.UserName, False, lblProductAssemblyID.Text, False)
                    End If
                    If Convert.ToInt32(txtScrap.Text) > 0 Then
                        'Add scrapped components
                        eb.addToComponentHistory(0, 0, 0, 0, 0, Convert.ToInt32(txtScrap.Text), 0, 0, 0, 5, 0, gvScrap.DataKeys(row.RowIndex).Value, "Component Scrapped", Membership.GetUser.UserName, False, lblProductAssemblyID.Text, True)
                    End If

                Next
                eb = Nothing
                'Update productionBatch with pass/fail/endDate
                oConn = New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                oCmd = New SqlCommand("procProductionBatchUpdate", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                    .Parameters.Add(New SqlParameter("@productionID", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@pass", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@fail", SqlDbType.Int))
                    .Parameters.Add(New SqlParameter("@user", SqlDbType.VarChar, 25))
                    .Parameters("@productionID").Value = Convert.ToInt32(gvProduction.SelectedValue)
                    .Parameters("@pass").Value = Convert.ToInt32(txtPass.Text)
                    .Parameters("@fail").Value = Convert.ToInt32(txtFail.Text)
                    .Parameters("@user").Value = Membership.GetUser.UserName
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
                'Add to Product History
                Dim lblProdID As Label = gvProduction.SelectedRow.FindControl("lblProductID")
                eb = New siteInclude
                Try
                    eb.addToProductHistory(Convert.ToInt32(lblProdID.Text), Convert.ToInt32(txtPass.Text), 0, 0, 0, 1, Convert.ToInt32(gvProduction.SelectedValue), "", Membership.GetUser.UserName, 0, 0, 0, Convert.ToInt32(txtPass.Text) + Convert.ToInt32(txtFail.Text))
                Catch ex As Exception
                    Response.Write(ex)
                    Response.End()
                End Try


                lblAllocationError.Text = ""
                txtPass.Text = ""
                txtFail.Text = ""
                Pan2.Visible = False
                gvProduction.DataBind()
            End If
        End If
    End Sub
    Protected Function addMasterComponentsOK(ByVal masterID As Integer, ByVal startRow As Integer, ByVal totalRows As Integer) As Boolean
        Dim bResult As Boolean = True
        Dim lblMasterQty As Label = gvScrap.Rows(startRow).FindControl("lblMasterQty")
        Dim lblMasterID As Label
        Dim txtRecycle As TextBox
        Dim txtScrap As TextBox
        Dim iTarget As Integer = Convert.ToInt32(lblMasterQty.Text)
        Dim iSum As Integer = 0
        For iRow As Integer = startRow To totalRows - 1
            lblMasterID = gvScrap.Rows(iRow).FindControl("lblMasterID")
            If Convert.ToInt32(lblMasterID.Text) = masterID Then
                txtRecycle = gvScrap.Rows(iRow).FindControl("txtRecycle")
                txtScrap = gvScrap.Rows(iRow).FindControl("txtScrap")
                iSum = iSum + Convert.ToInt32(txtRecycle.Text) + Convert.ToInt32(txtScrap.Text)
            End If
        Next
        If iTarget <> iSum Then bResult = False
        Return bResult
    End Function

    Protected Sub gvScrap_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim lblMax As Label = e.Row.FindControl("lblStockRemoved")
        Dim lblMasterID As Label = e.Row.FindControl("lblMasterID")
        If e.Row.RowType = DataControlRowType.DataRow Then
            If Convert.ToInt32(e.Row.Cells(_gvScrap_QtyPos).Text) > Convert.ToInt32(lblMax.Text) Then e.Row.Cells(_gvScrap_QtyPos).Text = lblMax.Text
            If iMasterID = Convert.ToInt32(lblMasterID.Text) Then
                'Same Master ID, Add brackets to Qty (and also to previous row's qty)
                e.Row.Cells(_gvScrap_QtyPos).Text = "(" & e.Row.Cells(_gvScrap_QtyPos).Text & ")"
                gvScrap.Rows(e.Row.RowIndex - 1).Cells(_gvScrap_QtyPos).Text = "(" & gvScrap.Rows(e.Row.RowIndex - 1).Cells(_gvScrap_QtyPos).Text & ")"
            End If
            iMasterID = Convert.ToInt32(lblMasterID.Text)

        End If
    End Sub
    Protected Function formatStartDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then
            result = FormatDateTime(d.ToString, DateFormat.LongDate) & " " & FormatDateTime(d.ToString, DateFormat.ShortTime)
        Else
            result = "Unknown"
        End If

        Return result
    End Function
    Protected Sub gvComplete_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim startDate As String = FormatDateTime(e.Row.Cells(_gvComplete_startDatePos).Text, DateFormat.LongDate) & " " & FormatDateTime(e.Row.Cells(_gvComplete_startDatePos).Text, DateFormat.ShortTime)
            Dim endDate As String = FormatDateTime(e.Row.Cells(_gvComplete_endDatePos).Text, DateFormat.LongDate) & " " & FormatDateTime(e.Row.Cells(_gvComplete_endDatePos).Text, DateFormat.ShortTime)
            e.Row.Cells(_gvComplete_startDatePos).Text = startDate
            e.Row.Cells(_gvComplete_endDatePos).Text = endDate
        End If
    End Sub
    Protected Sub gvComplete_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvDataEntry.DataBind()
        tblInfo.Visible = True
    End Sub
    Protected Sub gvComplete_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Check if row has already had its faulty components processed
        Dim lblProcessed As Label
        Dim lnk As HyperLink
        For Each row As GridViewRow In gvComplete.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lblProcessed = row.FindControl("lblFaultsProcessed")
                If Convert.ToBoolean(lblProcessed.Text) Then
                    'It has been processed, remove the command control from the correct cell, and replace with a link to productAssemblyView
                    row.Cells(_gvComplete_commandPos).Controls.Clear()
                    lnk = New HyperLink
                    lnk.Text = "R" & gvComplete.DataKeys(row.RowIndex).Value
                    lnk.NavigateUrl = "productAssemblyView.aspx?id=" & gvComplete.DataKeys(row.RowIndex).Value
                    lnk.Target = "_blank"
                    row.Cells(_gvComplete_commandPos).Controls.Add(lnk)
                    'remove print link
                    lnk = row.Cells(_gvComplete_printPos).Controls(0)
                    lnk.Text = "Reprint"
                    'row.Cells(_gvComplete_printPos).Controls.Clear()

                End If
            End If
        Next
    End Sub
    Protected Sub gvDataEntry_databound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim lblBuildQty As Label
        Dim lblFailures As Label
        Dim lblComponents As Label
        If e.Row.RowType = DataControlRowType.DataRow Then
            'Recordset only returns the total number of each component used in the entire batch, we only need component totals for Failed products.
            'This can be found by (TotalComponentQty / ProdBuildQty) * FailedProducts
            lblBuildQty = e.Row.FindControl("lblAmount")
            lblFailures = e.Row.FindControl("lblFailed")
            lblComponents = e.Row.FindControl("lblTotalBatchComponents")
            e.Row.Cells(_gvDataEntry_qtyPos).Text = (Convert.ToInt32(lblComponents.Text) / Convert.ToInt32(lblBuildQty.Text)) * Convert.ToInt32(lblFailures.Text)
        End If
    End Sub
    Protected Sub btnDataSubmit_click(ByVal Sender As Object, ByVal e As EventArgs)
        'Pass/Fail/Scrap details have been entered.
        'Check for invalid entry
        Dim iCompQty As Integer
        Dim txtPass As TextBox
        Dim txtFail As TextBox
        Dim txtScrap As TextBox
        Dim bAlphError As Boolean = False
        Dim bMathError As Boolean = False
        Dim iErrorRow As Integer
        Dim bRecycle As Boolean = False
        For Each row As GridViewRow In gvDataEntry.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If Not (bAlphError Or bMathError) Then
                    'Get row target qty (all boxes on this row must add up to this target)
                    iCompQty = Convert.ToInt32(row.Cells(_gvDataEntry_qtyPos).Text)
                    'Get user entry
                    txtPass = row.FindControl("txtPass")
                    txtFail = row.FindControl("txtFail")
                    txtScrap = row.FindControl("txtScrap")
                    'Set empty boxes to 0
                    If txtPass.Text = "" Then txtPass.Text = "0"
                    If txtFail.Text = "" Then txtFail.Text = "0"
                    If txtScrap.Text = "" Then txtScrap.Text = "0"
                    'Test for Non Numeric entry
                    If Not IsNumeric(txtPass.Text) Then bAlphError = True
                    If Not IsNumeric(txtFail.Text) Then bAlphError = True
                    If Not IsNumeric(txtScrap.Text) Then bAlphError = True
                    If bAlphError Then iErrorRow = row.RowIndex + 1
                    If Not bAlphError Then
                        'Test for quantities not addind up
                        If Convert.ToInt32(txtPass.Text) + Convert.ToInt32(txtFail.Text) + Convert.ToInt32(txtScrap.Text) <> iCompQty Then
                            bMathError = True
                            iErrorRow = row.RowIndex + 1
                        End If
                    End If
                    If Convert.ToInt32(txtPass.Text) > 0 Then bRecycle = True
                End If
            End If
        Next
        If bMathError Or bAlphError Then
            'Error found, display error message
            If bMathError Then
                lblError2.Text = "Row " & Convert.ToString(iErrorRow) & " does not add up."
            Else
                lblError2.Text = "Row " & Convert.ToString(iErrorRow) & " has a non-numerical entry."
            End If
        Else
            'All ok
            'If bRecycle = True Then
            If False Then
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("", oConn)
                With oCmd
                    .CommandType = CommandType.StoredProcedure
                End With
            End If
            'Add each row to componentHistory
            Dim si As siteInclude
            Dim iPass As Integer
            Dim iFail As Integer
            Dim iScrap As Integer
            Dim iComponentID As Integer
            For Each row As GridViewRow In gvDataEntry.Rows
                If row.RowType = DataControlRowType.DataRow Then
                    'Get user entry
                    txtPass = row.FindControl("txtPass")
                    txtFail = row.FindControl("txtFail")
                    txtScrap = row.FindControl("txtScrap")
                    iPass = Convert.ToInt32(txtPass.Text)
                    iFail = Convert.ToInt32(txtFail.Text)
                    iScrap = Convert.ToInt32(txtScrap.Text)
                    iComponentID = gvDataEntry.DataKeys(row.RowIndex).Value
                    Try
                        si = New siteInclude
                        If iPass > 0 Then si.addToComponentHistory(0, 0, 0, iPass, 0, 0, 0, 0, 0, 4, 0, iComponentID, "", Membership.GetUser.UserName, False, gvComplete.SelectedValue, True, 0)
                        If iFail > 0 Then si.addToComponentHistory(0, 0, 0, 0, 0, 0, 0, iFail, 0, 12, 0, iComponentID, "", Membership.GetUser.UserName, False, gvComplete.SelectedValue, True, 0)
                        If iScrap > 0 Then si.addToComponentHistory(0, 0, 0, 0, 0, iScrap, 0, 0, 0, 5, 0, iComponentID, "", Membership.GetUser.UserName, False, gvComplete.SelectedValue, True, 0)
                        si = Nothing
                    Catch ex As Exception
                        Response.Write(ex)
                        Response.End()
                    End Try
                End If
            Next
            setBatchFaultProcessed(gvComplete.SelectedValue, txtInfo.Text)
            gvDataEntry.Visible = False
            tblInfo.Visible = False
            gvComplete.SelectedIndex = -1
            gvComplete.DataBind()
            lblError2.Text = ""
        End If
    End Sub
    Protected Sub setBatchFaultProcessed(ByVal batchID As Integer, ByVal info As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductionBatchByIDFaultUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@batchID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@info", SqlDbType.VarChar, 500))
            .Parameters("@batchID").Value = batchID
            .Parameters("@info").Value = info
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
    End Sub
    Protected Sub gvProduction_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim hyp As HyperLink
        For Each row As GridViewRow In gvProduction.Rows
            If row.RowType = DataControlRowType.DataRow Then
                hyp = row.FindControl("hypLink")
                hyp.NavigateUrl = "javascript:openProdAssemblyPop(" & CStr(gvProduction.datakeys(row.RowIndex).value) & ")"
            End If
        Next
    End Sub
End Class

