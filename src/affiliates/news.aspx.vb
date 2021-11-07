Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_news
    Inherits BasePage

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblError.Text = ""
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
    End Sub

    'User Events
    Protected Sub btnSave_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procNewsInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@headline", SqlDbType.VarChar, 200))
            .Parameters.Add(New SqlParameter("@news", SqlDbType.VarChar, -1))
            .Parameters.Add(New SqlParameter("@active", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@front", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@viewable", SqlDbType.VarChar, 10))
            .Parameters("@headline").Value = txtHeadline.Text
            .Parameters("@news").Value = FCKeditor1.Value
            .Parameters("@active").Value = chkActive.Checked
            .Parameters("@front").Value = chkFront.Checked
            .Parameters("@viewable").Value = drpPublic.SelectedValue
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
        End Try
        'Clear up and rebind dropdown to include new story
        FCKeditor1.Value = ""
        txtHeadline.Text = ""
        drpNews.Items.Clear()
        drpNews.Items.Add(New ListItem("Select...", ""))
        drpNews.DataBind()
        'Show Success message 
        lblError.Text = "Story successfully added."
    End Sub
    Protected Sub btnEdit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procNewsByIdUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@headline", SqlDbType.VarChar, 200))
            .Parameters.Add(New SqlParameter("@news", SqlDbType.VarChar, -1))
            .Parameters.Add(New SqlParameter("@date", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@active", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@front", SqlDbType.Bit))
            .Parameters.Add(New SqlParameter("@viewable", SqlDbType.VarChar, 10))
            .Parameters("@id").Value = drpNews.SelectedValue
            .Parameters("@headline").Value = txtHeadline.Text
            .Parameters("@news").Value = FCKeditor1.Value
            .Parameters("@date").Value = Now()
            .Parameters("@active").Value = chkActive.Checked
            .Parameters("@front").Value = chkFront.Checked
            .Parameters("@viewable").Value = drpPublic.SelectedValue
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
        End Try
        'Clear up and rebind dropdown to include new story
        FCKeditor1.Value = ""
        txtHeadline.Text = ""
        drpNews.Items.Clear()
        drpNews.Items.Add(New ListItem("Select...", ""))
        drpNews.DataBind()
        'Show Success message 
        lblError.Text = "Story successfully updated."
    End Sub
    Protected Sub drpNews_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpNews.SelectedValue <> "" Then
            btnSave.Visible = False
            btnEdit.Visible = True
            'Get story and load contents into the editor
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procNewsByIdSelect2", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@id", SqlDbType.Int))
                .Parameters("@id").Value = drpNews.SelectedValue
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    FCKeditor1.Value = ds.Tables(0).Rows(0)("news")
                    txtHeadline.Text = ds.Tables(0).Rows(0)("headline")
                    chkActive.Checked = CBool(ds.Tables(0).Rows(0)("active"))
                    chkFront.Checked = CBool(ds.Tables(0).Rows(0)("showOnFrontPage"))
                    drpPublic.SelectedValue = ds.Tables(0).Rows(0)("viewable")
                End If
            Catch ex As Exception
                Response.Write(ex)

            End Try
        Else
            'Put back into Add mode
            txtHeadline.Text = ""
            FCKeditor1.Value = ""
            btnSave.Visible = True
            btnEdit.Visible = False
        End If
    End Sub

    'Subs
End Class
