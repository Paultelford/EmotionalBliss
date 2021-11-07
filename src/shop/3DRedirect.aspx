<%@ Page Language="VB" AutoEventWireup="false" CodeFile="3DRedirect.aspx.vb" Inherits="_3DRedirect"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>3D Secure Redirect</title>
    <script type="text/javascript"> function OnLoadEvent() { document.form.submit(); } </script>
</head>
<body onload="OnLoadEvent();">
<%
    Response.Write("<FORM name=""form"" action=""" & strACSURL & """ method=""POST"" target=""3DIFrame""/>")
    Response.Write("<input type=""hidden"" name=""PaReq"" value=""" & strPAReq & """/>")
    Response.Write("<input type=""hidden"" name=""TermUrl"" value=""" & strTermURL & """/>")
    Response.Write("<input type=""hidden"" name=""MD"" value=""" & strMD & """/>")
    Response.Write("<NOSCRIPT>")
    Response.Write("<center><p>Please click button below to Authenticate your card</p><input type=""submit"" value=""Go""/></p></center>")
    Response.Write("</NOSCRIPT>")
    Response.Write("</form>")
%>
</body>
</html>
