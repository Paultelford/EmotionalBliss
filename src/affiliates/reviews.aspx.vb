Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_reviews
    Inherits BasePage

    Protected Sub Page_Load(ByVal sener As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffEBDistributorCountryCode") = "" Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        gvReviews.DataBind()
    End Sub
    Protected Sub gvReviews_editing(ByVal sener As Object, ByVal e As GridViewEditEventArgs)
        Dim id As Integer = gvReviews.DataKeys(e.NewEditIndex).Value
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procReviewNewByIDActiveUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@reviewID", SqlDbType.Int))
            .Parameters("@reviewID").Value = id
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
        e.Cancel = True
        gvReviews.DataBind()
    End Sub
    Protected Sub gvReviews_deleting(ByVal sener As Object, ByVal e As GridViewDeleteEventArgs)
        Dim id As Integer = gvReviews.DataKeys(e.RowIndex).Value
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procReviewByIDDeleteUpdate", oConn)
        Dim deleted As Integer = 1
        If LCase(drpType.SelectedValue) = "delete" Then deleted = 0
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@reviewID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@deleted", SqlDbType.Bit))
            .Parameters("@reviewID").Value = id
            .Parameters("@deleted").Value = deleted
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
        e.Cancel = True
        gvReviews.DataBind()
    End Sub
    Protected Function showDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d.ToString, DateFormat.LongDate)
        Return result
    End Function
    Protected Function showSmallReview(ByVal txt As Object, ByVal id As Integer) As String
        Dim r As String = ""
        If Not IsDBNull(txt) Then
            If Len(txt.ToString) > 100 Then
                r = Left(txt.ToString, 100) & ".... <span onclick='showReviewPopup(" & id & ")' style='color:blue;cursor: pointer;'>View More</span>"
            Else
                r = txt.ToString
            End If
        End If

        Return r
    End Function
    Protected Sub drpProducts_dataBound(ByVal sener As Object, ByVal e As EventArgs)
        If drpProducts.Items.Count = 1 Then
            lblNoReviewsText.Text = "There are currently no reviews."
            panProductSelect.Visible = False
        End If
    End Sub
    Protected Sub sqlReviews_selecting(ByVal sender As Object, ByVal e As SqlDataSourceSelectingEventArgs)
        Dim activeParam As Integer = 1
        Dim deletedParam As Integer = 2
        Dim active As String = ""
        Dim deleted As String = "0"
        Select Case LCase(drpType.SelectedValue)
            Case "all"
                active = "%"
            Case "auth"
                active = "1"
            Case "unauth"
                active = "0"
            Case "delete"
                active = "%"
                deleted = "1"
        End Select
        e.Command.Parameters(activeParam).Value = active
        e.Command.Parameters(deletedParam).Value = deleted
    End Sub
    Protected Sub drpType_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvReviews.DataBind()
    End Sub
    Protected Sub gv_pageIndexChanging(ByVal sender As Object, ByVal e As GridViewPageEventArgs) Handles gvReviews.PageIndexChanging
        Dim gv As GridView = CType(sender, GridView)
        gv.PageIndex = e.NewPageIndex
        gv.DataBind()
    End Sub
    Protected Sub gv_dataBound(ByVal sender As Object, ByVal e As EventArgs) Handles gvReviews.DataBound
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerData(gv)
    End Sub
    Protected Sub gv_dataBinding(ByVal sender As Object, ByVal e As EventArgs) Handles gvReviews.DataBinding
        Dim gv As GridView = CType(sender, GridView)
        pager1.registerControl(gv)
    End Sub

    Protected Function getProductName(ByVal o As Object) As String
        Dim result As String = ""
        If Not isdbnull(o) Then
            result = left(ucase(o.ToString()), 1)
            result = result & right(lcase(o.ToString()), len(o.ToString()) - 1)
        End If
        Return result
    End Function
End Class
