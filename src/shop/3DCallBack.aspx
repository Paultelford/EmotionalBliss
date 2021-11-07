<%@ Page Language="VB" AutoEventWireup="false" CodeFile="3DCallBack.aspx.vb" Inherits="_3DCallBack"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>3D CallBack</title>
    <script type="text/javascript"> function OnLoadEvent() { document.form.submit(); } </script>
</head>
<body onload="OnLoadEvent();">
    <asp:Label ID="lblWait" runat="server" Text="Please wait while authorising...."></asp:Label>
<%
    Response.Write("<FORM name=""form"" action=""3DComplete.aspx"" method=""POST"" target=""_top""/>")
    response.write("<input type=""hidden"" name=""PaRes"" value=""" & strPaRes & """/>")
    response.write("<input type=""hidden"" name=""MD"" value=""" & strMD & """/>")
    response.write("<NOSCRIPT>")
    response.write("<center><p>Please click button below to Authorise your card</p><input type=""submit"" value=""Go""/></p></center>")
    response.write("</NOSCRIPT>")
    response.write("</form>")
%>
</body>
</html>