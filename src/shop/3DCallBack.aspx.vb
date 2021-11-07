Imports includes
Partial Class _3DCallBack
    Inherits System.Web.UI.Page
    Public strACSURL As String = ""
    Public strPaRes As String = ""
    Public strMD As String = ""
    Public strVendorTxCode As String = ""
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        strPaRes = Request.Form("PaRes")
        strMD = Request.Form("MD")
        strVendorTxCode = Request.QueryString("VendorTxCode")
        Session("VendorTxCode") = strVendorTxCode
    End Sub
    Public Shared Function URLEncode(ByVal strString As String) As String
        Return HttpUtility.UrlEncode(strString)
    End Function
End Class
