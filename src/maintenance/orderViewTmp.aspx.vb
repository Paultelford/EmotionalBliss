
Partial Class maintenance_orderViewTmp
    Inherits System.Web.UI.Page
    Protected Function showDate(ByVal d As Object)
        Dim result As String = ""
        If Not IsDBNull(d) Then
            result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        End If
        Return result
    End Function
End Class
