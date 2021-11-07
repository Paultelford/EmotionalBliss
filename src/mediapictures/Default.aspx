<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" MasterPageFile="~/m_site.master" Inherits="DefaultMedia" Theme="WinXP_Blue" Title="Emotional Bliss Media Pictures" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="pressLoggedin" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="panLoggedin" runat="server">
        <asp:Label ID="lblIntro" runat="server" dbResource="lblIntro"></asp:Label>
     </asp:Panel>
     <asp:Panel ID="panLogin" runat="server">
        <table>
            <tr>
                <td align="center">
                    Welcome to the Press Area.<br /><br />
                </td>
            </tr>
            <tr>
                <td>
                    If you require a Username and Password please contact our Press Enquiries via <a href='/ebcontact.aspx?dept=3'>contact us</a><br /><br />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td width="80">&nbsp;</td>
                <td>
                    Username:
                </td>
                <td>
                    <asp:TextBox ID="txtUsername" runat="server" MaxLength="20" ValidationGroup="login"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqUsername" runat="server" ControlToValidate="txtUsername" Display="Static" ErrorMessage="* Required" ValidationGroup="login"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td width="80">&nbsp;</td>
                <td>
                    Password:
                </td>
                <td>
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="20" ValidationGroup="login"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword" Display="Static" ErrorMessage="* Required" ValidationGroup="login"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td width="80">&nbsp;</td>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_click" ValidationGroup="login"></asp:Button>
                </td>
            </tr>
            <tr>
                <td width="80">&nbsp;</td>
                <td colspan="2">
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                </td>
            </tr>
        </table>
     </asp:Panel>
</asp:Content>