<%@ Page Language="VB" AutoEventWireup="false" CodeFile="clearCache.aspx.vb" Inherits="clearCache" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        Select the cache and enter the countrycode (if applicable) then click Clear to do the biz.<br />
        <asp:DropDownList ID="drpCacheName" runat="server">
            <asp:ListItem Text="Choose Cache to clear...." Value=""></asp:ListItem>
            <asp:ListItem Text="Main Menu" Value="EBImageMap"></asp:ListItem>
        </asp:DropDownList>
        <asp:TextBox ID="txtCountryCode" runat="server" MaxLength="5" Width="20"></asp:TextBox>
        <asp:Button ID="btnSubmit" runat="server" Text="Clear" OnClick="btnSubmit_click" />
        <br /><br />
        <asp:Label ID="lblComplete" runat="server" ForeColor="Red"></asp:Label>
    </div>
    </form>
</body>
</html>
