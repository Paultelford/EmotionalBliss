
Partial Class affiliates_reviewNewPop
    Inherits System.Web.UI.Page


    Protected Function getProductName(ByVal o As Object) As String
        Dim result As String = ""
        If Not isdbnull(o) Then
            result = left(ucase(o.ToString()), 1)
            result = result & right(lcase(o.ToString()), len(o.ToString()) - 1)
        End If
        Return result
    End Function
    Protected Function showDate(ByVal d As Object) As String
        Dim result As String = ""
        If Not IsDBNull(d) Then result = FormatDateTime(d.ToString, DateFormat.LongDate)
        Return result
    End Function
End Class
