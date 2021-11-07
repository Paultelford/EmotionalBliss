<%@ Page Language="VB" AutoEventWireup="false" CodeFile="salesLedgerPopup.aspx.vb" Inherits="affiliates_salesLedgerPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gv" runat="server" AutoGenerateColumns="false" GridLines="None">
            <Columns>
                <asp:HyperLinkField HeaderText="Order" DataTextField="newOrderID" DataNavigateUrlFields="id" DataNavigateUrlFormatString="/affiliates/orderView.aspx?id={0}" />
                <asp:BoundField ItemStyle-Width="20" />
                <asp:BoundField HeaderText="Transaction" DataField="balance" DataFormatString="{0:n2}" />
                <asp:BoundField ItemStyle-Width="20" />
                <asp:BoundField HeaderText="Vat" DataField="vat" DataFormatString="{0:n2}" Visible="false" />
            </Columns>
        </asp:GridView>
        
       
    </div>
    </form>
</body>
</html>
