<%@ Page Language="VB" AutoEventWireup="false" CodeFile="royaltyUpdate.aspx.vb" Inherits="maintenance_royaltyUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnUpdateTest" runat="server" Text="Update Royalties (Test)" OnClick="btnUpdate_click" CommandArgument="test" /><br />
        <asp:Button ID="btnUpdateRetail" runat="server" Text="Update Royalties Retail Orders" OnClick="btnUpdate_click" CommandArgument="true" />
        <asp:Button ID="btnUpdateDist" runat="server" Text="Update Royalties Distributor Orders" OnClick="btnUpdate_click" CommandArgument="false" />
    </div>
    </form>
</body>
</html>
