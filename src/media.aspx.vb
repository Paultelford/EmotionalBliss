Imports System.Data

Partial Class media
    Inherits System.Web.UI.Page
    Private Const _itemsPerRow As Integer = 3
    Private _item As Integer = 1
    Private tRow As TableRow

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim dt As New DataTable
        tRow = New TableRow
        Try
            Dim param() As String = {"@active"}
            Dim paramValue() As String = {"true"}
            Dim paramType() As SqlDbType = {SqlDbType.Bit}
            Dim paramSize() As Integer = {0}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procMediaByActiveSelect")
            'Response.Write("Rowcount=" & dt.Rows.Count)
            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    'Response.Write(row("title") & "<br>")

                    showArticle(row)
                Next
                If _item > 1 Then
                    'incomplete row exists, add dummy <td> to fill the row, then add row to table
                    Dim tCell As TableCell
                    For iLoop As Integer = _item To _itemsPerRow
                        tCell = New TableCell
                        tCell.Text = "<table width='200' cellpadding='0' cellspacing='0'><tr><td>&nbsp;</td></tr></table>"
                        tRow.Cells.Add(tCell)
                    Next
                    tblMedia.Rows.Add(tRow)
                End If
            End If
        Catch ex As Exception
            'siteInclude.addError("media.asp.vb", "Page_Load(); " & ex.ToString)
            Response.Write(ex.ToString)
        Finally
            dt.Dispose()
        End Try
        'Show paypal link
        Dim m As msite = CType(Master, msite)
        m.showFacebookLink()
    End Sub
    Protected Sub showArticle(ByRef row As DataRow)
        Dim html As String = hidCell.Value
        html = Replace(html, "@title", row("title"))
        html = Replace(html, "@date", smallMonth(FormatDateTime(row("date"), DateFormat.LongDate)))
        html = Replace(html, "@headline", row("description"))
        html = Replace(html, "@thumb", row("thumb"))
        html = Replace(html, "@button", makeJSButton(row))
        Dim tCell As New TableCell
        tCell.Text = html
        tRow.Cells.Add(tCell)
        If _item = _itemsPerRow Then
            'End of row reached, goto next row
            tblMedia.Rows.Add(tRow)
            tRow = New TableRow
            _item = 0
        End If
        _item = _item + 1
    End Sub
    Protected Function makeJSButton(ByRef row As DataRow) As String
        Dim result As String = ""
        Select Case Convert.ToInt16(row("type"))
            Case 1
                'Image popup
                result = "<a href='/images/media/" & row("image") & "' onclick='return hs.expand(this)'>"
            Case 2
                'Url
                result = "<a href='" & row("url") & "' class='highslide' target='new'>"
            Case 3
                'Full Html Frame
                result = "<a href='press/press.aspx?id=" & row("id") & "' onclick=""return hs.htmlExpand(this, { objectType: 'iframe' } )"">"
        End Select
        Return result
    End Function
    Protected Function smallMonth(ByVal d As String) As String
        Dim result As String = d
        result = Replace(result, "January", "Jan")
        result = Replace(result, "February", "Feb")
        result = Replace(result, "March", "Mar")
        result = Replace(result, "April", "Apr")
        result = Replace(result, "June", "Jun")
        result = Replace(result, "July", "Jul")
        result = Replace(result, "August", "Aug")
        result = Replace(result, "September", "Sep")
        result = Replace(result, "October", "Oct")
        result = Replace(result, "November", "Nov")
        result = Replace(result, "December", "Dec")
        Return result
    End Function

End Class
