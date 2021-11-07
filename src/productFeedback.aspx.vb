Imports System.Data
Imports System.Data.SqlClient

Partial Class productFeedback
    Inherits System.Web.UI.Page
    Private Const _maxLength As Integer = 4000

    Protected Sub btnNext1_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate()
        If Page.IsValid Then
            Select Case LCase(rad1_8.SelectedValue)
                Case "womolia"
                    mvFeedback.SetActiveView(vPage2)
                Case "femblossom"
                    mvFeedback.SetActiveView(vPage2)
                Case "jasmine"
                    mvFeedback.SetActiveView(vPage2)
                Case "isis & chandra"
                    mvFeedback.SetActiveView(vPage3)
            End Select
        End If
    End Sub
    Protected Sub btnComplete2_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim bError As Boolean = False
        Page.Validate()
        If Page.IsValid Then
            'Test for 4000 characters in textboxes
            If Len(txt2_11.Text) > _maxLength Then
                lblInfo2.Text = "<font color='red'>* Max " & _maxLength & " characters, you have used " & Len(txt2_11.Text) & "</font>"
                bError = True
            Else
                lblInfo2.Text = ""
            End If
            If Len(txt2_12.Text) > _maxLength Then
                lblReview2.Text = "<font color='red'>* Max " & _maxLength & " characters, you have used " & Len(txt2_12.Text) & "</font>"
                bError = True
            Else
                lblReview2.Text = ""
            End If
        Else
            bError = True
        End If
        If Page.IsValid And Not bError Then commitData("heat")
    End Sub
    Protected Sub btnComplete3_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim bError As Boolean = False
        Page.Validate()
        If Page.IsValid Then
            'Test for 4000 characters in textboxes
            If Len(txt3_11.Text) > _maxLength Then
                lblInfo3.Text = "<font color='red'>* Max " & _maxLength & " characters, you have used " & Len(txt3_11.Text) & "</font>"
                bError = True
            Else
                lblInfo3.Text = ""
            End If
            If Len(txt3_12.Text) > _maxLength Then
                lblReview3.Text = "<font color='red'>* Max " & _maxLength & " characters, you have used " & Len(txt3_11.Text) & "</font>"
                bError = True
            Else
                lblReview3.Text = ""
            End If
        Else
            bError = True
        End If
        If Page.IsValid And Not bError Then commitData("kit")
    End Sub
    Protected Sub commitData(ByVal prod As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procProductFeedbackInsert", oConn)
        Dim bError As Boolean = False
        oCmd.CommandType = CommandType.StoredProcedure
        With oCmd
            .Parameters.Add(New SqlParameter("@postcode", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@name", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@1_1", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@1_2", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@1_3", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@1_4", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@1_5a", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@1_5b", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@1_6", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@1_7", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@1_8", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@2_1", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2a", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2b", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2c", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2d", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2e", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2f", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2g", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2h", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_2i", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3a", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3b", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3c", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3d", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3e", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3f", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3g", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3h", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_3i", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4a", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4b", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4c", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4d", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4e", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4f", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4g", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4h", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_4i", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@2_5", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@2_6", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@2_7a", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@2_7b", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@2_7c", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@2_7d", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@2_7e", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@2_8", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@2_9", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@2_10", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@2_11", SqlDbType.VarChar, 4000))
            .Parameters.Add(New SqlParameter("@2_12", SqlDbType.VarChar, 4000))
            .Parameters.Add(New SqlParameter("@3_1a", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@3_1b", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@3_2a", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@3_2b", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@3_3a", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@3_3b", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@3_5", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@3_6", SqlDbType.VarChar, 500))
            .Parameters.Add(New SqlParameter("@3_7a", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@3_7b", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@3_7c", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@3_7d", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@3_7e", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@3_8", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@3_9", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@3_10", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@3_11", SqlDbType.VarChar, 4000))
            .Parameters.Add(New SqlParameter("@3_12", SqlDbType.VarChar, 4000))

            .Parameters("@postcode").Value = txtPostcode.Text
            .Parameters("@name").Value = txtName.Text
            .Parameters("@1_1").Value = rad1_1.SelectedValue
            .Parameters("@1_2").Value = rad1_2.SelectedValue
            .Parameters("@1_3").Value = rad1_3.SelectedValue
            .Parameters("@1_4").Value = rad1_4.SelectedValue
            .Parameters("@1_5a").Value = rad1_5a.SelectedValue
            .Parameters("@1_5b").Value = rad1_5b.SelectedValue
            .Parameters("@1_6").Value = rad1_6.SelectedValue
            .Parameters("@1_7").Value = rad1_7.SelectedValue
            .Parameters("@1_8").Value = rad1_8.SelectedValue
        End With
        If prod = "kit" Then
            oCmd.Parameters("@3_1a").Value = rad3_1a.SelectedValue
            oCmd.Parameters("@3_1b").Value = rad3_1b.SelectedValue
            oCmd.Parameters("@3_2a").Value = rad3_2a.SelectedValue
            oCmd.Parameters("@3_2b").Value = rad3_2b.SelectedValue
            oCmd.Parameters("@3_3a").Value = rad3_3a.SelectedValue
            oCmd.Parameters("@3_3b").Value = rad3_3b.SelectedValue
            oCmd.Parameters("@3_5").Value = rad3_5.SelectedValue
            oCmd.Parameters("@3_6").Value = txt3_6.Text
            oCmd.Parameters("@3_7a").Value = txt3_7a.Text
            oCmd.Parameters("@3_7b").Value = txt3_7a.Text
            oCmd.Parameters("@3_7c").Value = txt3_7a.Text
            oCmd.Parameters("@3_7d").Value = txt3_7a.Text
            oCmd.Parameters("@3_7e").Value = txt3_7a.Text
            oCmd.Parameters("@3_8").Value = rad3_8.SelectedValue
            oCmd.Parameters("@3_9").Value = txt3_9.Text
            oCmd.Parameters("@3_10").Value = rad3_10.SelectedValue
            oCmd.Parameters("@3_11").Value = txt3_11.Text
            oCmd.Parameters("@3_12").Value = txt3_12.Text
            'Set all others to 0
            oCmd.Parameters("@2_1").Value = 0
            oCmd.Parameters("@2_2a").Value = 0
            oCmd.Parameters("@2_2b").Value = 0
            oCmd.Parameters("@2_2c").Value = 0
            oCmd.Parameters("@2_2d").Value = 0
            oCmd.Parameters("@2_2e").Value = 0
            oCmd.Parameters("@2_2f").Value = 0
            oCmd.Parameters("@2_2g").Value = 0
            oCmd.Parameters("@2_2h").Value = 0
            oCmd.Parameters("@2_2i").Value = 0
            oCmd.Parameters("@2_3a").Value = 0
            oCmd.Parameters("@2_3b").Value = 0
            oCmd.Parameters("@2_3c").Value = 0
            oCmd.Parameters("@2_3d").Value = 0
            oCmd.Parameters("@2_3e").Value = 0
            oCmd.Parameters("@2_3f").Value = 0
            oCmd.Parameters("@2_3g").Value = 0
            oCmd.Parameters("@2_3h").Value = 0
            oCmd.Parameters("@2_3i").Value = 0
            oCmd.Parameters("@2_4a").Value = 0
            oCmd.Parameters("@2_4b").Value = 0
            oCmd.Parameters("@2_4c").Value = 0
            oCmd.Parameters("@2_4d").Value = 0
            oCmd.Parameters("@2_4e").Value = 0
            oCmd.Parameters("@2_4f").Value = 0
            oCmd.Parameters("@2_4g").Value = 0
            oCmd.Parameters("@2_4h").Value = 0
            oCmd.Parameters("@2_4i").Value = 0
            oCmd.Parameters("@2_5").Value = ""
            oCmd.Parameters("@2_6").Value = ""
            oCmd.Parameters("@2_7a").Value = ""
            oCmd.Parameters("@2_7b").Value = ""
            oCmd.Parameters("@2_7c").Value = ""
            oCmd.Parameters("@2_7d").Value = ""
            oCmd.Parameters("@2_7e").Value = ""
            oCmd.Parameters("@2_8").Value = ""
            oCmd.Parameters("@2_9").Value = ""
            oCmd.Parameters("@2_10").Value = ""
            oCmd.Parameters("@2_11").Value = ""
            oCmd.Parameters("@2_12").Value = ""
        Else
            oCmd.Parameters("@2_1").Value = rad2_1.SelectedValue
            oCmd.Parameters("@2_2a").Value = rad2_2a.SelectedValue
            oCmd.Parameters("@2_2b").Value = rad2_2b.SelectedValue
            oCmd.Parameters("@2_2c").Value = rad2_2c.SelectedValue
            oCmd.Parameters("@2_2d").Value = rad2_2d.SelectedValue
            oCmd.Parameters("@2_2e").Value = rad2_2e.SelectedValue
            oCmd.Parameters("@2_2f").Value = rad2_2f.SelectedValue
            oCmd.Parameters("@2_2g").Value = rad2_2g.SelectedValue
            oCmd.Parameters("@2_2h").Value = rad2_2h.SelectedValue
            oCmd.Parameters("@2_2i").Value = rad2_2i.SelectedValue
            oCmd.Parameters("@2_3a").Value = rad2_3a.SelectedValue
            oCmd.Parameters("@2_3b").Value = rad2_3b.SelectedValue
            oCmd.Parameters("@2_3c").Value = rad2_3c.SelectedValue
            oCmd.Parameters("@2_3d").Value = rad2_3d.SelectedValue
            oCmd.Parameters("@2_3e").Value = rad2_3e.SelectedValue
            oCmd.Parameters("@2_3f").Value = rad2_3f.SelectedValue
            oCmd.Parameters("@2_3g").Value = rad2_3g.SelectedValue
            oCmd.Parameters("@2_3h").Value = rad2_3h.SelectedValue
            oCmd.Parameters("@2_3i").Value = rad2_3i.SelectedValue
            oCmd.Parameters("@2_4a").Value = rad2_4a.SelectedValue
            oCmd.Parameters("@2_4b").Value = rad2_4b.SelectedValue
            oCmd.Parameters("@2_4c").Value = rad2_4c.SelectedValue
            oCmd.Parameters("@2_4d").Value = rad2_4d.SelectedValue
            oCmd.Parameters("@2_4e").Value = rad2_4e.SelectedValue
            oCmd.Parameters("@2_4f").Value = rad2_4f.SelectedValue
            oCmd.Parameters("@2_4g").Value = rad2_4g.SelectedValue
            oCmd.Parameters("@2_4h").Value = rad2_4h.SelectedValue
            oCmd.Parameters("@2_4i").Value = rad2_4i.SelectedValue
            oCmd.Parameters("@2_5").Value = rad2_5.SelectedValue
            oCmd.Parameters("@2_6").Value = txt2_6.Text
            oCmd.Parameters("@2_7a").Value = txt2_7a.Text
            oCmd.Parameters("@2_7b").Value = txt2_7b.Text
            oCmd.Parameters("@2_7c").Value = txt2_7c.Text
            oCmd.Parameters("@2_7d").Value = txt2_7d.Text
            oCmd.Parameters("@2_7e").Value = txt2_7e.Text
            oCmd.Parameters("@2_8").Value = rad2_8.SelectedValue
            oCmd.Parameters("@2_9").Value = txt2_9.Text
            oCmd.Parameters("@2_10").Value = rad2_10.SelectedValue
            oCmd.Parameters("@2_11").Value = txt2_11.Text
            oCmd.Parameters("@2_12").Value = txt2_12.Text
            'Set all others to 0
            oCmd.Parameters("@3_1a").Value = 0
            oCmd.Parameters("@3_1b").Value = 0
            oCmd.Parameters("@3_2a").Value = 0
            oCmd.Parameters("@3_2b").Value = 0
            oCmd.Parameters("@3_3a").Value = 0
            oCmd.Parameters("@3_3b").Value = 0
            oCmd.Parameters("@3_5").Value = ""
            oCmd.Parameters("@3_6").Value = ""
            oCmd.Parameters("@3_7a").Value = ""
            oCmd.Parameters("@3_7b").Value = ""
            oCmd.Parameters("@3_7c").Value = ""
            oCmd.Parameters("@3_7d").Value = ""
            oCmd.Parameters("@3_7e").Value = ""
            oCmd.Parameters("@3_8").Value = ""
            oCmd.Parameters("@3_9").Value = ""
            oCmd.Parameters("@3_10").Value = ""
            oCmd.Parameters("@3_11").Value = ""
            oCmd.Parameters("@3_12").Value = ""
        End If
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            bError = True
            lblError.Text = "<font color='red'>An error occured, please try again later.</font>"
            Dim si As New siteInclude
            si.addError("productFeedbasck.aspx.vb", "commitData(prod=" & prod & "); " & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If Not bError Then
            mvFeedback.Visible = False
            lblError.Text = "Thankyou for your feedback."
        End If
    End Sub
End Class
