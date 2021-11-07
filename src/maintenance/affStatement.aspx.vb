
Partial Class maintenance_affStatement
    Inherits System.Web.UI.Page
    Private Const _gvStatement_creditPos As Integer = 6
    Private Const _gvStatement_debitPos As Integer = 8

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
    End Sub
    Protected Function showOrderID(ByVal extID As Integer, ByVal extUserOrderID As Object, ByVal ebPrefix As Object, ByVal ebOrderID As Object, ByVal ebCountryCode As Object) As String
        Dim result As String
        If extID = 0 Then
            result = ebPrefix.ToString & "/" & ebOrderID.ToString & UCase(ebCountryCode.ToString)
        Else
            result = Parse(extUserOrderID.ToString)
        End If
        Return result
    End Function
    Protected Function showDate(ByVal d As Object)
        Dim result As String = "Unknown"
        If Not IsDBNull(d) Then
            result = FormatDateTime(d, DateFormat.LongDate) & " " & FormatDateTime(d, DateFormat.ShortTime)
        End If
        Return result
    End Function
    Protected Sub gvStatement_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Dim cr As Decimal = 0
        Dim dr As Decimal = 0
        'Add each row up
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                cr = cr + CType(row.Cells(_gvStatement_creditPos).Text, Decimal)
                dr = dr + CType(row.Cells(_gvStatement_debitPos).Text, Decimal)
            End If
        Next
        'Set toals in footer row
        gv.FooterRow.Cells(_gvStatement_creditPos).Text = CType(FormatNumber(cr, 2), String)
        gv.FooterRow.Cells(_gvStatement_debitPos).Text = CType(FormatNumber(dr, 2), String)
    End Sub
    Protected Function Parse(ByVal s As String) As String
        'Removes the affID from the end of the extOrderID
        Dim a As Array = Split(s, "_")
        Return a(0)
    End Function
End Class
