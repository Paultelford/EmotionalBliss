<%@ Page Title="" Language="VB" MasterPageFile="~/mshop.master" AutoEventWireup="false" CodeFile="PaymentSuccess.aspx.vb" Inherits="shop_PaymentSuccess" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentTop" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <table>
       <caption><asp:Label runat="server" ID="lblPaymentMsg"></asp:Label></caption>
       <tr>
           <td><asp:Label runat="server" ID="lblPaymentID"></asp:Label></td>
       </tr>
       <tr>
           <td><asp:Label runat="server" ID="lblTokenID"></asp:Label></td>
       </tr>
       <tr>
           <td><asp:Label runat="server" ID="lblPayerID"></asp:Label></td>
       </tr>
   </table>
    <asp:Button ID="btnbacktohome"  runat="server" Text="Back to Home"/>
</asp:Content>

