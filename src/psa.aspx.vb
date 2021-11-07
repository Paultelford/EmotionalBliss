Imports System.Data
Imports System.Data.SqlClient

Partial Class psa
    Inherits BasePage

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            loadResources()
            If hidQuestionNo.Value = "" Then hidQuestionNo.Value = "1"
            showQuestion()
        End If
    End Sub

    'User Events
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Page.Validate()
        If Page.IsValid Then
            btnSubmit.Visible = False
            btnNext.Visible = True
            Dim lblReply As Label
            lblReply = panQuestions.FindControl("lblReply" & CInt(radAnswer.SelectedValue))
            lblReply.Visible = True
        End If
    End Sub
    Protected Sub btnNext_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        btnNext.Visible = False
        btnSubmit.Visible = True
        radAnswer.SelectedIndex = -1
        hidQuestionNo.Value = CInt(hidQuestionNo.Value) + 1
        showQuestion()
        lblReply1.Visible = False
        lblReply2.Visible = False
        lblReply3.Visible = False
    End Sub

    'Databind Events
    Protected Sub lblPleaseWait_load(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label = CType(sender, Label)
        lbl.Text = getDBResourceString("lblPleaseWait", "global")
    End Sub

    'Subs
    Protected Sub showQuestion()
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procPSAByQNoSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim row As DataRow
        Dim lblReply As Label
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@questionNo", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@questionNo").Value = CInt(hidQuestionNo.Value)
            .Parameters("@countryCode").Value = Session("EBLanguage")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'Show question and possible answers
                row = ds.Tables(0).Rows(0)
                litQuestionNumber.Text = "Question " & row("qNo")
                lblQuestion.Text = row("q")
                'show the 3 answers
                For Each aRow As DataRow In ds.Tables(0).Rows
                    radAnswer.Items(CInt(aRow("aNo")) - 1).Text = " " & aRow("a")
                    radAnswer.Items(CInt(aRow("aNo")) - 1).Value = " " & aRow("aNo")
                    lblReply = panQuestions.FindControl("lblReply" & aRow("aNo"))
                    lblReply.Text = "<b>Julia Says -</b><br>" & aRow("response") & "<br><br>"
                Next
            Else
                'End of questionnaire
                panQuestions.Visible = False
                btnSubmit.Visible = False
                lblEnd.Text = getDBResourceString("lblEnd")
            End If
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("psa.aspx.vb", "showQuestion(); " & ex.ToString)
            si = Nothing
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub loadResources()
        reqRadAnswer.ErrorMessage = getDBResourceString("errorRadRequired")
        btnSubmit.ImageUrl = getDBResourceString("btnSubmit")
        btnNext.ImageUrl = getDBResouceString("btnNext")
    End Sub
End Class
