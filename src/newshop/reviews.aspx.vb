
Partial Class shop_reviews
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblCurrentReviewsText1.Text = getDBResouceString("lblCurrentReviewsText1")
        lblCurrentReviewstext2.Text = getDBResouceString("lblCurrentReviewsText2")

    End Sub
End Class
