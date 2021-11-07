<%@ Page Title="" Language="VB" MasterPageFile="~/mshop.master" AutoEventWireup="false" CodeFile="shopIntro.aspx.vb" Inherits="shop_shopIntro" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBShopMenu.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentTop" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="shop" master="mshop">
    </menu:EBMenu>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h3><asp:Label ID="litShopText" runat="server" dbResource="litShopText">Shop</asp:Label></h3>
    <div style="padding-left: 80px;">
        <asp:Label ID="lblNote" runat="server" ForeColor="Red"></asp:Label>
    </div>
    <div id="DashedLineHorizontal"></div>
    <asp:Label ID="PageTitle" runat="server" dbResource="PageTitle"></asp:Label>
    <asp:Label ID="lblMetaKeywords" runat="server" dbResource="metaKeywords"></asp:Label>
    <asp:Label ID="lblMetaDescription" runat="server" dbResource="metaDescription"></asp:Label>
    <asp:Panel ID="panTest" runat="server">
    </asp:Panel>
</asp:Content>

