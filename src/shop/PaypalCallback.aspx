<%@ Page Language="VB" AutoEventWireup="false" CodeFile="PaypalCallback.aspx.vb" Inherits="shop_PaypalCallback" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    OrderID: <asp:Label ID="lblOrderID" runat="server"></asp:Label><br />
    Returned status=<asp:Label ID="lblStatus" runat="server"></asp:Label><br />
    Returned Status Detail=<asp:Label ID="lblStatusDetail" runat="server"></asp:Label><br />
    Returned Protocol=<asp:Label ID="lblProtocol" runat="server"></asp:Label><br /><br />
    If status is 'PAYPALOK' then click <asp:LinkButton ID="lnkPaypalOK" runat="server" Text="here" OnClick="lnkPaypalOK_click"></asp:LinkButton><br /><br />
    If status is 'OK' then click <asp:LinkButton ID="lnkOK" runat="server" Text="here" OnClick="lnkOK_click"></asp:LinkButton><br /><br />
    If it failed then click <asp:LinkButton ID="lnkFail" runat="server" Text="here" OnClick="lnkFail_click"></asp:LinkButton> to go back to the payment page (order should get deleted)
    </div>
    </form>
</body>
</html>
