<%@ Page Title="" Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="email.aspx.vb" Inherits="email" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentTop" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="panIntro" runat="server" Visible="false">
       
    </asp:Panel>
    <asp:Panel ID="panCompelte" runat="server" Visible="true">
        <u><b><font size="+1">VIP VOUCHER</font></b></u>
        <br /><br />
        <b><font size="+1">Voucher Number: 21722342</font></b>
        <br /><br />
        Please copy the above voucher number to be used on the basket page to reclaim your £20 (£16 exc VAT) off your goods total for the Womolia or Femblossom Heat.
        <br /><br />
        Please click <a href="/shopIntro.aspx"><u>here</u></a> to purchase.
    </asp:Panel>
    <br /><br />
    
    
</asp:Content>

