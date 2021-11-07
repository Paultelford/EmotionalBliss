<%@ Page Title="Product Images" Language="VB" MasterPageFile="~/m_site.master" AutoEventWireup="false" CodeFile="products.aspx.vb" Inherits="mediapictures_products" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="pressLoggedin" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentTop" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:label ID="lblInstructions" runat="server" Text="" Visible="true"></asp:label>
    <br /><br />
    <asp:Table BorderWidth="0" id="tblImages" runat="server" CellSpacing="8">
    </asp:Table>
</asp:Content>

