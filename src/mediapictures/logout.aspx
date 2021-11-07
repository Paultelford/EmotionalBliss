<%@ Page Title="" Language="VB" MasterPageFile="~/m_site.master" AutoEventWireup="false" CodeFile="logout.aspx.vb" Inherits="mediapictures_logout" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="press" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentTop" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    You have been logged out
</asp:Content>

