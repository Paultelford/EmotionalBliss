<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="purchaseManagement.aspx.vb" Inherits="affiliates_purchaseManagement" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    A bit of a menu sorta thing...
    <br /><br /><br />
    <asp:HyperLink ID="lnkSuppliers" runat="server" Text="Add/Edit Suppliers" NavigateUrl="supplierAdd.aspx"></asp:HyperLink>
    <br /><br />
    <asp:HyperLink ID="lnkComponent" runat="server" Text="Add/Edit Products" NavigateUrl="products.aspx"></asp:HyperLink>
    <br /><br />
    <asp:HyperLink ID="lnkOrders" runat="server" Text="External Orders" NavigateUrl="orders.aspx"></asp:HyperLink>
    <br /><br />
    <asp:HyperLink ID="lnkRecieved" runat="server" Text="Orders Received" NavigateUrl="ordersReceived.aspx"></asp:HyperLink>
    <br /><br />
    <asp:HyperLink ID="lnkStock" runat="server" Text="Stock" NavigateUrl="stock.aspx"></asp:HyperLink>
    <br /><br />
    <asp:HyperLink ID="lnkStockHistory" runat="server" Text="Stock History" NavigateUrl="stockHistory.aspx"></asp:HyperLink>
</asp:Content>

