<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/masterPageShop.master" CodeFile="default.aspx.vb" Inherits="shop_default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    Welcome to the eb shop
    <br />
    Here's the welcome blurb..............
    <br /><br /><br /><br />
    <asp:Label ID="lblDeptEmpty" runat="server" ForeColor="red"></asp:Label>
    <asp:Table ID="tblDept" runat="server">
    </asp:Table>
</asp:Content>