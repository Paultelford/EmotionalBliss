
Partial Class affiliates_orderViewVoucher
    Inherits BasePage

    Protected Function showType(ByVal b As Boolean)
        Dim result As String = "Single Use"
        If b Then result = "Coupon"
        Return result
    End Function
End Class
