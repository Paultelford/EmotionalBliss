Imports System.Data

Partial Class productReviews
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim bWriteMode As Boolean = False
        tblFront.Visible = CBool(Request.QueryString("product") = "")
        If InStr(Request.ServerVariables("QUERY_STRING"), "write") > 0 Then
            panReviews.Visible = False
            panWriteReview.Visible = True
        Else
            panReviews.Visible = Not tblFront.Visible
        End If


        litTopDash.Visible = False
        imgProductBig.Visible = True
        Select Case LCase(Request.QueryString("product"))
            Case "femblossom"
                'lblHeader.Text = "Femblossom Heat Reviews"
                imgProductBig.ImageUrl = "/images/header_review_femblossom.jpg"
                lnkAddReview.NavigateUrl = "/productReviews.aspx?product=femblossom&write"
            Case "isis"
                'lblHeader.Text = "Isis Reviews"
                imgProductBig.ImageUrl = "/images/header_review_isis.jpg"
                lnkAddReview.NavigateUrl = "/productReviews.aspx?product=isis&write"
            Case "womolia"
                'lblHeader.Text = "Womolia Heat Reviews"
                imgProductBig.ImageUrl = "/images/header_review_womolia.jpg"
                lnkAddReview.NavigateUrl = "/productReviews.aspx?product=womolia&write"
            Case "chandra"
                'lblHeader.Text = "Chandra Reviews"
                imgProductBig.ImageUrl = "/images/header_review_chandra.jpg"
                lnkAddReview.NavigateUrl = "/productReviews.aspx?product=chandra&write"
            Case Else
                lblHeader.Text = "Reviews"
                litTopDash.Visible = True
                imgProductBig.Visible = False
        End Select
        If panReviews.Visible Then
            Dim dt As DataTable = getReviews(Request.QueryString("product"))
            lvReview.DataSource = dt
            lvReview.DataBind()
        End If
    End Sub
    'Page
    Protected Sub lvReview_itemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs)
        Dim hidScore As HiddenField = e.Item.FindControl("hidScore")
        For iLoop As Integer = 1 To 5
            CType(e.Item.FindControl("imgStar" & iLoop.ToString()), HtmlImage).Visible = CBool(CInt(hidScore.Value) >= iLoop)
        Next

    End Sub

    'User - Click
    Protected Sub lnkReviewInsert_click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim param() As String = New String() {"@product", "@review", "@language", "@name", "@score"}
            Dim paramValue() As String = New String() {Request.QueryString("product").ToLower(), txtReviewText.Text, Session("ebLanguage"), txtReviewName.Text, drpScore.SelectedValue}
            Dim paramType() As SqlDbType = New SqlDbType() {SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.Int}
            Dim paramSize() As Integer = New Integer() {20, -1, 5, 100, 0}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procReviewNewInsertLite")
        Catch ex As Exception
            siteInclude.addError("productReviews.aspx.vb", "lnkReviewInsert_click(); " & ex.ToString())
        End Try
        lblReviewInsert.Text = "Your review has been submitted. Once validated it will be shown here."
        txtReviewName.Text = ""
        txtReviewText.Text = ""
        drpScore.SelectedIndex = 2
        panWriteReview.Visible = False
    End Sub


    'Functions
    Protected Function getReviews(ByVal product As String) As DataTable
        If product <> "" Then
            Dim dt as New DataTable
            try
                Dim param() As String = {"@product", "@language"}
                Dim paramValue() As String = {product, Session("ebLanguage").ToString()}
                Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.VarChar}
                Dim paramSize() As Integer = {20, 5}
                dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procReviewNewByProductSelect")
                Return dt
            Catch ex As Exception
                siteInclude.addError("", "(); " & ex.ToString())
            Finally
                dt.Dispose()
            End Try
        End If
        Return New DataTable
    End Function
End Class
