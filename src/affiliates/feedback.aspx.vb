Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_feedback
    Inherits BasePage
    Private bHeatProduct As Boolean = False
    Private Const _maxLength As Integer = 4000

    Protected Sub page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        tblFeedback.Visible = True
        lblError2A.Text = ""
        lblError2B.Text = ""
        lblError3A.Text = ""
        lblError3B.Text = ""
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        If Page.IsPostBack Then

        Else
            showFeedback("%")
            'If drpType.SelectedValue = "all" Then showRangeFeedback() 'Show min/max/avg values for questions that arent multiple choice
        End If
    End Sub
    Protected Sub gvLog_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        showFeedback(gvLog.SelectedDataKey.Value)
        'If drpType.SelectedValue = "all" Then showRangeFeedback() 'Show min/max/avg values for questions that arent multiple choice
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal E As EventArgs)
        If drpType.SelectedValue = "all" Then
            'gvLog.Visible = False
            lblName.Visible = False
            showFeedback("%")
            drpUser.Visible = False
            drpUser.SelectedIndex = 0
            tbl2.Visible = True
            tbl3.Visible = True
            td2TextA.Visible = False
            td2TextB.Visible = False
            td3TextA.Visible = False
            td3TextB.Visible = False
        Else
            'gvLog.Visible = True
            tblFeedback.Visible = False
            tbl2.Visible = False
            tbl3.Visible = False
            drpUser.Visible = True
            td2TextA.Visible = True
            td2TextB.Visible = True
            td3TextA.Visible = True
            td3TextB.Visible = True
            up2.visible = False
        End If
    End Sub
    Protected Sub showFeedback(ByVal id As String)
        clearResults()
        tblFeedback.Visible = True
        tbl2.Visible = True
        tbl3.Visible = True
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductFeedbackByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.VarChar, 4))
            .Parameters("@id").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then processResults(ds.Tables(0))
        Catch ex As Exception
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub showCosts(ByVal q As String)
        Dim dt As DataTable = getPriceRange(q)
        Dim lbl As Label
        Try
            For Each row As DataRow In dt.Rows
                lbl = tblFeedback.FindControl("lbl" & row("name"))
                lbl.Text = FormatNumber(row("min"), 2) & " / " & FormatNumber(row("avg"), 2) & " / " & FormatNumber(row("max"), 2)
            Next
        Catch ex As Exception
            Response.Write(ex)
        Finally
            dt.Dispose()
        End Try
    End Sub
    Protected Function getPriceRange(ByVal q As String) As DataTable
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductFeedbackCostsSelect", oConn)
        Dim da As New SqlDataAdapter()
        Dim ds As New DataSet
        Dim dt As New DataTable
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@question", SqlDbType.VarChar, 10))
            .Parameters("@question").Value = q
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            dt = ds.Tables(0)
        Catch ex As Exception
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return dt
    End Function
    Protected Function showPostage(ByVal q As String) As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductFeedbackPostageSelect", oConn)
        Dim da As New SqlDataAdapter()
        Dim ds As New DataSet
        Dim row As DataRow
        Dim result As String = ""
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@question", SqlDbType.VarChar, 10))
            .Parameters("@question").Value = q
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                row = ds.Tables(0).Rows(0)
                result = FormatNumber(row("min"), 2) & " / " & FormatNumber(row("avg"), 2) & " / " & FormatNumber(row("max"), 2)
            Else
                result = "N/A"
            End If
        Catch ex As Exception
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function showColors(ByVal q As String) As String
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductFeedbackColorsSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim result As String = ""
        Dim iCount As Integer = 0
        oCmd.CommandType = CommandType.StoredProcedure
        oCmd.Parameters.Add(New SqlParameter("@question", SqlDbType.VarChar, 10))
        oCmd.Parameters("@question").Value = q
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each row As DataRow In ds.Tables(0).Rows
                    result = result & row("color") & "(" & row("qty") & "), "
                    iCount = iCount + CInt(row("qty"))
                Next
                result = result & "<b>Total(" & iCount & ")"
            End If
        Catch ex As Exception
            Response.Write(ex)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Sub drpUser_selectedIndexChanged(ByVal sender As Object, ByVal E As EventArgs)
        showFeedback(drpUser.SelectedValue)
        lblName.Visible = True
        up2.Visible = True
    End Sub
    Protected Sub processResults(ByRef dt As DataTable)
        Try
            Dim bSingleResults As Boolean = False
            If drpType.SelectedValue = "individual" Then bSingleResults = True
            Dim iCount As Integer = 0
            For Each row As DataRow In dt.Rows
                iCount = iCount + 1
                bHeatProduct = Not (LCase(row("1_8")) = "i")
                If bSingleResults Then
                    tbl2.Visible = bHeatProduct
                    tbl3.Visible = Not bHeatProduct
                    lblName.Text = "<b>Name: </b>" & row("name")
                End If
                doRow("1_1", row)
                doRow("1_2", row)
                doRow("1_3", row)
                doRow("1_4", row)
                doRow("1_5a", row)
                doRow("1_5b", row)
                doRow("1_6", row)
                doRow("1_7", row)
                doRow("1_8", row)
                If bHeatProduct Then
                    doRow("2_1", row)
                    doRow("2_2a", row)
                    doRow("2_2b", row)
                    doRow("2_2c", row)
                    doRow("2_2d", row)
                    doRow("2_2e", row)
                    doRow("2_2f", row)
                    doRow("2_2g", row)
                    doRow("2_2h", row)
                    doRow("2_2i", row)
                    doRow("2_3a", row)
                    doRow("2_3b", row)
                    doRow("2_3c", row)
                    doRow("2_3d", row)
                    doRow("2_3e", row)
                    doRow("2_3f", row)
                    doRow("2_3g", row)
                    doRow("2_3h", row)
                    doRow("2_3i", row)
                    doRow("2_4a", row)
                    doRow("2_4b", row)
                    doRow("2_4c", row)
                    doRow("2_4d", row)
                    doRow("2_4e", row)
                    doRow("2_4f", row)
                    doRow("2_4g", row)
                    doRow("2_4h", row)
                    doRow("2_4i", row)
                    doBit("2_5", row)
                    If bSingleResults Then
                        doText("2_6", row)
                        doText("2_7a", row)
                        doText("2_7b", row)
                        doText("2_7c", row)
                        doText("2_7d", row)
                        doText("2_7e", row)
                    End If
                    doBit("2_8", row)
                    If bSingleResults Then doText("2_9", row)
                    doBit("2_10", row)
                    If bSingleResults Then
                        doTextBox("2_11", row)
                        doTextBox("2_12", row)
                    End If
                Else
                    doRow("3_1a", row)
                    doRow("3_1b", row)
                    doRow("3_2a", row)
                    doRow("3_2b", row)
                    doRow("3_3a", row)
                    doRow("3_3b", row)
                    doBit("3_5", row)
                    If bSingleResults Then
                        doText("3_6", row)
                        doText("3_7a", row)
                        doText("3_7b", row)
                        doText("3_7c", row)
                        doText("3_7d", row)
                        doText("3_7e", row)
                    End If
                    doBit("3_8", row)
                    If bSingleResults Then doText("3_9", row)
                    doBit("3_10", row)
                    If bSingleResults Then
                        doTextBox("3_11", row)
                        doTextBox("3_12", row)
                    End If
                End If
            Next
            'Run once/range results
            If Not bSingleResults Then
                lbl2_6.Text = showColors("2_6")
                lbl3_6.Text = showColors("3_6")
                showCosts("2_7")
                showCosts("3_7")
                lbl2_9.Text = showPostage("2_9")
                lbl3_9.Text = showPostage("3_9")
                lbl2_7Pre.Text = "£(Minimum / Average / Maximum)"
                lbl3_7Pre.Text = "£(Minimum / Average / Maximum)"
                lbl2_9Pre.Text = "£(Minimum / Average / Maximum)"
                lbl3_9Pre.Text = "£(Minimum / Average / Maximum)"
            Else
                lbl2_7Pre.Text = ""
                lbl3_7Pre.Text = ""
                lbl2_9Pre.Text = ""
                lbl3_9Pre.Text = ""
            End If
            lblReports.Text = iCount
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Protected Sub doRow(ByVal e As String, ByRef r As DataRow)
        If r(e) <> "0" Then
            Dim lbl As Label = tblFeedback.FindControl("lbl" & e & "_" & r(e))
            lbl.Text = lbl.Text + 1
        End If
    End Sub
    Protected Sub doBit(ByVal e As String, ByRef r As DataRow)
        Dim lbl As Label
        If CBool(r(e)) Then
            lbl = tblFeedback.FindControl("lbl" & e & "_1")
        Else
            lbl = tblFeedback.FindControl("lbl" & e & "_0")
        End If
        lbl.Text = lbl.Text + 1
    End Sub
    Protected Sub doText(ByVal e As String, ByRef r As DataRow)
        Dim lbl As Label = tblFeedback.FindControl("lbl" & e)
        lbl.Text = r(e)
    End Sub
    Protected Sub doTextBox(ByVal e As String, ByRef r As DataRow)
        Dim lbl As TextBox = tblFeedback.FindControl("lbl" & e)
        lbl.Text = r(e)
    End Sub
    Protected Sub btnSubmit2_click(ByVal sender As Object, ByVal E As EventArgs)
        'Test for 4000 characters in textboxes
        Dim bError As Boolean = False
        If Len(lbl2_11.Text) > _maxLength Then
            lblError2A.Text = "<font color='red'>* Max " & _maxLength & " characters, you have used " & Len(lbl2_11.Text) & "</font>"
            bError = True
        Else
            lblError2A.Text = ""
        End If
        If Len(lbl2_12.Text) > _maxLength Then
            lblError2B.Text = "<font color='red'>* Max " & _maxLength & " characters, you have used " & Len(lbl2_12.Text) & "</font>"
            bError = True
        Else
            lblError2B.Text = ""
        End If

        If Not bError Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductFeedbackByIDHeatUpdate", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@2_11", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@2_12", SqlDbType.VarChar, 4000))
                .Parameters("@id").Value = drpUser.SelectedValue
                .Parameters("@2_11").Value = lbl2_11.Text
                .Parameters("@2_12").Value = lbl2_12.Text
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                bError = True
                Dim si As New siteInclude
                si.addError("affiliates/feedback.aspx", "btnSubmit2_click(); " & ex.ToString)
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If Not bError Then lblError2B.Text = "<font color='red'>Updated successfully</font>"
        End If
    End Sub
    Protected Sub btnSubmit3_click(ByVal sender As Object, ByVal E As EventArgs)
        'Test for 4000 characters in textboxes
        Dim bError As Boolean = False
        If Len(lbl3_11.Text) > _maxLength Then
            lblError3A.Text = "<font color='red'>* Max " & _maxLength & " characters, you have used " & Len(lbl3_11.Text) & "</font>"
            bError = True
        Else
            lblError2A.Text = ""
        End If
        If Len(lbl3_12.Text) > _maxLength Then
            lblError3B.Text = "<font color='red'>* Max " & _maxLength & " characters, you have used " & Len(lbl3_12.Text) & "</font>"
            bError = True
        Else
            lblError3B.Text = ""
        End If

        If Not bError Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procProductFeedbackByIDFingerUpdate", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@3_11", SqlDbType.VarChar, 4000))
                .Parameters.Add(New SqlParameter("@3_12", SqlDbType.VarChar, 4000))
                .Parameters("@id").Value = drpUser.SelectedValue
                .Parameters("@3_11").Value = lbl3_11.Text
                .Parameters("@3_12").Value = lbl3_12.Text
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                bError = True
                Dim si As New siteInclude
                si.addError("affiliates/feedback.aspx", "btnSubmit3_click(); " & ex.ToString)
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
            If Not bError Then lblError3B.Text = "<font color='red'>Updated successfully</font>"
        End If
    End Sub
    Protected Sub clearResults()
        lbl1_1_1.Text = "0"
        lbl1_1_2.Text = "0"
        lbl1_1_3.Text = "0"
        lbl1_1_4.Text = "0"
        lbl1_1_5.Text = "0"
        lbl1_2_1.Text = "0"
        lbl1_2_2.Text = "0"
        lbl1_2_3.Text = "0"
        lbl1_2_4.Text = "0"
        lbl1_2_5.Text = "0"
        lbl1_3_1.Text = "0"
        lbl1_3_2.Text = "0"
        lbl1_3_3.Text = "0"
        lbl1_3_4.Text = "0"
        lbl1_3_5.Text = "0"
        lbl1_4_1.Text = "0"
        lbl1_4_2.Text = "0"
        lbl1_5a_1.Text = "0"
        lbl1_5a_2.Text = "0"
        lbl1_5a_3.Text = "0"
        lbl1_5a_4.Text = "0"
        lbl1_5a_5.Text = "0"
        lbl1_5b_1.Text = "0"
        lbl1_5b_2.Text = "0"
        lbl1_5b_3.Text = "0"
        lbl1_5b_4.Text = "0"
        lbl1_5b_5.Text = "0"
        lbl1_6_1.Text = "0"
        lbl1_6_2.Text = "0"
        lbl1_6_3.Text = "0"
        lbl1_6_4.Text = "0"
        lbl1_6_5.Text = "0"
        lbl1_7_1.Text = "0"
        lbl1_7_2.Text = "0"
        lbl1_7_3.Text = "0"
        lbl1_7_4.Text = "0"
        lbl1_7_5.Text = "0"
        lbl1_8_J.Text = "0"
        lbl1_8_W.Text = "0"
        lbl1_8_F.Text = "0"
        lbl1_8_I.Text = "0"
        lbl2_1_1.Text = "0"
        lbl2_1_2.Text = "0"
        lbl2_1_3.Text = "0"
        lbl2_1_4.Text = "0"
        lbl2_1_5.Text = "0"
        lbl2_2a_1.Text = "0"
        lbl2_2a_2.Text = "0"
        lbl2_2a_3.Text = "0"
        lbl2_2a_4.Text = "0"
        lbl2_2a_5.Text = "0"
        lbl2_2b_1.Text = "0"
        lbl2_2b_2.Text = "0"
        lbl2_2b_3.Text = "0"
        lbl2_2b_4.Text = "0"
        lbl2_2b_5.Text = "0"
        lbl2_2c_1.Text = "0"
        lbl2_2c_2.Text = "0"
        lbl2_2c_3.Text = "0"
        lbl2_2c_4.Text = "0"
        lbl2_2c_5.Text = "0"
        lbl2_2d_1.Text = "0"
        lbl2_2d_2.Text = "0"
        lbl2_2d_3.Text = "0"
        lbl2_2d_4.Text = "0"
        lbl2_2d_5.Text = "0"
        lbl2_2e_1.Text = "0"
        lbl2_2e_2.Text = "0"
        lbl2_2e_3.Text = "0"
        lbl2_2e_4.Text = "0"
        lbl2_2e_5.Text = "0"
        lbl2_2f_1.Text = "0"
        lbl2_2f_2.Text = "0"
        lbl2_2f_3.Text = "0"
        lbl2_2f_4.Text = "0"
        lbl2_2f_5.Text = "0"
        lbl2_2g_1.Text = "0"
        lbl2_2g_2.Text = "0"
        lbl2_2g_3.Text = "0"
        lbl2_2g_4.Text = "0"
        lbl2_2g_5.Text = "0"
        lbl2_2h_1.Text = "0"
        lbl2_2h_2.Text = "0"
        lbl2_2h_3.Text = "0"
        lbl2_2h_4.Text = "0"
        lbl2_2h_5.Text = "0"
        lbl2_2i_1.Text = "0"
        lbl2_2i_2.Text = "0"
        lbl2_2i_3.Text = "0"
        lbl2_2i_4.Text = "0"
        lbl2_2i_5.Text = "0"
        lbl2_3a_1.Text = "0"
        lbl2_3a_2.Text = "0"
        lbl2_3a_3.Text = "0"
        lbl2_3a_4.Text = "0"
        lbl2_3a_5.Text = "0"
        lbl2_3b_1.Text = "0"
        lbl2_3b_2.Text = "0"
        lbl2_3b_3.Text = "0"
        lbl2_3b_4.Text = "0"
        lbl2_3b_5.Text = "0"
        lbl2_3c_1.Text = "0"
        lbl2_3c_2.Text = "0"
        lbl2_3c_3.Text = "0"
        lbl2_3c_4.Text = "0"
        lbl2_3c_5.Text = "0"
        lbl2_3d_1.Text = "0"
        lbl2_3d_2.Text = "0"
        lbl2_3d_3.Text = "0"
        lbl2_3d_4.Text = "0"
        lbl2_3d_5.Text = "0"
        lbl2_3e_1.Text = "0"
        lbl2_3e_2.Text = "0"
        lbl2_3e_3.Text = "0"
        lbl2_3e_4.Text = "0"
        lbl2_3e_5.Text = "0"
        lbl2_3f_1.Text = "0"
        lbl2_3f_2.Text = "0"
        lbl2_3f_3.Text = "0"
        lbl2_3f_4.Text = "0"
        lbl2_3f_5.Text = "0"
        lbl2_3g_1.Text = "0"
        lbl2_3g_2.Text = "0"
        lbl2_3g_3.Text = "0"
        lbl2_3g_4.Text = "0"
        lbl2_3g_5.Text = "0"
        lbl2_3h_1.Text = "0"
        lbl2_3h_2.Text = "0"
        lbl2_3h_3.Text = "0"
        lbl2_3h_4.Text = "0"
        lbl2_3h_5.Text = "0"
        lbl2_3i_1.Text = "0"
        lbl2_3i_2.Text = "0"
        lbl2_3i_3.Text = "0"
        lbl2_3i_4.Text = "0"
        lbl2_3i_5.Text = "0"
        lbl2_4a_1.Text = "0"
        lbl2_4a_2.Text = "0"
        lbl2_4a_3.Text = "0"
        lbl2_4a_4.Text = "0"
        lbl2_4a_5.Text = "0"
        lbl2_4b_1.Text = "0"
        lbl2_4b_2.Text = "0"
        lbl2_4b_3.Text = "0"
        lbl2_4b_4.Text = "0"
        lbl2_4b_5.Text = "0"
        lbl2_4c_1.Text = "0"
        lbl2_4c_2.Text = "0"
        lbl2_4c_3.Text = "0"
        lbl2_4c_4.Text = "0"
        lbl2_4c_5.Text = "0"
        lbl2_4d_1.Text = "0"
        lbl2_4d_2.Text = "0"
        lbl2_4d_3.Text = "0"
        lbl2_4d_4.Text = "0"
        lbl2_4d_5.Text = "0"
        lbl2_4e_1.Text = "0"
        lbl2_4e_2.Text = "0"
        lbl2_4e_3.Text = "0"
        lbl2_4e_4.Text = "0"
        lbl2_4e_5.Text = "0"
        lbl2_4f_1.Text = "0"
        lbl2_4f_2.Text = "0"
        lbl2_4f_3.Text = "0"
        lbl2_4f_4.Text = "0"
        lbl2_4f_5.Text = "0"
        lbl2_4g_1.Text = "0"
        lbl2_4g_2.Text = "0"
        lbl2_4g_3.Text = "0"
        lbl2_4g_4.Text = "0"
        lbl2_4g_5.Text = "0"
        lbl2_4h_1.Text = "0"
        lbl2_4h_2.Text = "0"
        lbl2_4h_3.Text = "0"
        lbl2_4h_4.Text = "0"
        lbl2_4h_5.Text = "0"
        lbl2_4i_1.Text = "0"
        lbl2_4i_2.Text = "0"
        lbl2_4i_3.Text = "0"
        lbl2_4i_4.Text = "0"
        lbl2_4i_5.Text = "0"
        lbl2_5_0.Text = "0"
        lbl2_5_1.Text = "0"
        lbl2_6.Text = ""
        lbl2_7a.Text = ""
        lbl2_7b.Text = ""
        lbl2_7c.Text = ""
        lbl2_7d.Text = ""
        lbl2_7e.Text = ""
        lbl2_8_0.Text = "0"
        lbl2_8_1.Text = "0"
        lbl2_9.Text = ""
        lbl2_10_0.Text = "0"
        lbl2_10_1.Text = "0"
        lbl2_11.Text = ""
        lbl2_12.Text = ""
        lbl3_1a_1.Text = "0"
        lbl3_1a_2.Text = "0"
        lbl3_1a_3.Text = "0"
        lbl3_1a_4.Text = "0"
        lbl3_1a_5.Text = "0"
        lbl3_1b_1.Text = "0"
        lbl3_1b_2.Text = "0"
        lbl3_1b_3.Text = "0"
        lbl3_1b_4.Text = "0"
        lbl3_1b_5.Text = "0"
        lbl3_2a_1.Text = "0"
        lbl3_2a_2.Text = "0"
        lbl3_2a_3.Text = "0"
        lbl3_2a_4.Text = "0"
        lbl3_2a_5.Text = "0"
        lbl3_2b_1.Text = "0"
        lbl3_2b_2.Text = "0"
        lbl3_2b_3.Text = "0"
        lbl3_2b_4.Text = "0"
        lbl3_2b_5.Text = "0"
        lbl3_3a_1.Text = "0"
        lbl3_3a_2.Text = "0"
        lbl3_3a_3.Text = "0"
        lbl3_3a_4.Text = "0"
        lbl3_3a_5.Text = "0"
        lbl3_3b_1.Text = "0"
        lbl3_3b_2.Text = "0"
        lbl3_3b_3.Text = "0"
        lbl3_3b_4.Text = "0"
        lbl3_3b_5.Text = "0"
        lbl3_5_1.Text = "0"
        lbl3_5_0.Text = "0"
        lbl3_6.Text = ""
        lbl3_7a.Text = ""
        lbl3_7b.Text = ""
        lbl3_7c.Text = ""
        lbl3_7d.Text = ""
        lbl3_7e.Text = ""
        lbl3_8_1.Text = "0"
        lbl3_8_0.Text = "0"
        lbl3_9.Text = ""
        lbl3_10_1.Text = "0"
        lbl3_10_0.Text = "0"
        lbl3_11.Text = ""
        lbl3_12.Text = ""

    End Sub
End Class
