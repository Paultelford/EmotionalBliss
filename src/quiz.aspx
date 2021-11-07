<%@ Page Language="VB" MasterPageFile="~/MasterPageSite.master" AutoEventWireup="false" CodeFile="quiz.aspx.vb" Inherits="quiz" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label><br />
    <asp:Label ID="lblPanelEditor1" runat="server" Text="Section 1" Visible="false"></asp:Label>
    <asp:Panel ID="panEditor1" runat="server">
    </asp:Panel>
</asp:Content>

