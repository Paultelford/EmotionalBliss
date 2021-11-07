<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="maintenance_login" title="Maintenance Login" %>
<html>
<head runat="server">
</head>
<body>
    <form id="frmLogin" runat="server" >
        <center>
            <asp:Login ID="login1" runat="server" OnAuthenticate="loginAff_authenticate" DestinationPageUrl="~\maintenance\default.aspx" SkinID="LoginView"></asp:Login>
        </center>
    </form>
</body>
</html>
<script language="javascript">
function focusLogin(e)
{
    document.getElementById(e).focus();
}
</script>
