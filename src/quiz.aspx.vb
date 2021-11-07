Imports System.Data
Imports System.Data.SqlClient


Partial Class quiz
    Inherits System.Web.UI.Page
    Private _content As ContentPlaceHolder
    Private bDebug As Boolean = False

    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        Dim lbl As Label
        Dim pageName As String = "quiz"
        _content = Master.FindControl("ContentPlaceHolder1")
        'Clear all text boxes & errors
        lblError.Text = ""
        For iLoop As Integer = 1 To 20
            lbl = _content.FindControl("lblEditor" & CType(iLoop, String))
            Try
                lbl.Text = ""
            Catch ex As Exception
            End Try
        Next
        'Retrieve text from DB depending on country in Session("EBLanguage")
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procEditorByPageSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@page", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@languageCode", SqlDbType.VarChar, 5))
            .Parameters("@page").Value = pageName
            .Parameters("@languageCode").Value = Session("EBLanguage")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'Fill label controls with data from DB
                For Each rs As DataRow In ds.Tables(0).Rows
                    showText(rs("text"), rs("paragraph"), rs("section"), rs("id"), pageName)
                Next
            Else
                'Show error in 1st textbox
                lblError.Text = "<font color='red'>No text has been defined for this language yet!</font>"
            End If
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        'If user is logged in as affiliate and has set Session("EBTextEdit") to TRUE, then show editable details.
        If Session("EBTextEdit") Or bDebug Then
            Dim pan As Panel
            Dim lblPan As Label
            For iLoop As Integer = 1 To 20
                pan = _content.FindControl("panEditor" & CType(iLoop, String))
                lblPan = _content.FindControl("lblPanelEditor" & CType(iLoop, String))
                Try
                    pan.BorderWidth = "1"
                    lblPan.ForeColor = Drawing.Color.Aqua
                    lblPan.Font.Bold = True
                    lblPan.Visible = True
                Catch ex As Exception
                End Try
            Next
        End If
    End Sub
    Protected Sub showText(ByRef text As String, ByVal para As Integer, ByVal section As Integer, ByVal id As Integer, ByVal page As String)
        Dim lbl As New Label
        Dim lnk As New HyperLink
        Dim pan As Panel = _content.FindControl("panEditor" & section) 'Get reference to panel. The text will be added to this panel
        lbl.Text = text 'Assign text to the newly created Label
        lnk.Text = "....Edit<br>" 'Set the Edit links Text
        lnk.NavigateUrl = "~/editor.aspx?id=" & id & "&page=" & page 'Set the link url to the edit page
        pan.Controls.Add(lbl) 'Add the newly created label to the panal on page
        If Session("EBTextEdit") Or bDebug Then pan.Controls.Add(lnk) 'Add the Edit link if Distributor has enabled editing
    End Sub
End Class
