
Partial Class _3DRedirect
    Inherits System.Web.UI.Page
    '**************************************************************************************************
    ' VSP Direct Kit 3D Redirection inline frame
    '**************************************************************************************************
    Public strACSURL As String
    Public strPAReq As String 
    Public strMD As String
    Public strVendorTxCode As String
    Public strTermURL As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strACSURL = Session("ACSURL")
        strPAReq = Session("PAReq")
        strMD = Session("MD")
        strVendorTxCode = Session("VendorTxCode")
        strTermURL = "http://www.eb-dev.com:8888/shop/3DCallback.aspx?VendorTxCode=" & strVendorTxCode
        If Not Application("isDevBox") Then strTermURL = "https://" & Request.ServerVariables("HTTP_HOST") & "/shop/3DCallback.aspx?vendorTxCode=" & strVendorTxCode
        Session("PAReq") = ""
    End Sub
    Public Shared Function URLEncode(ByVal strString As String) As String
        Return HttpUtility.UrlEncode(strString)
    End Function
End Class
