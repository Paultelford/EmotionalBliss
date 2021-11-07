<%@ Page Language="VB" Trace="false" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="static.aspx.vb" Inherits="static_" title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="ContentTop" ContentPlaceHolderID="contentTop" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <table border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>&nbsp;</td>
            <td>
                <asp:Label ID="lblMetaDescription" runat="server" dbResource="metaDescription"></asp:Label>
                <asp:Label ID="lblMetaKeywords" runat="server" dbResource="metaKeywords"></asp:Label>
                <asp:Label ID="PageTitle" runat="server" dbResource="PageTitle"></asp:Label>
                <asp:Label ID="lblParagraph1" runat="server" dbResource="Paragraph1"></asp:Label>
                <asp:Label ID="lblParagraph2" runat="server" dbResource="Paragraph2"></asp:Label>
                <asp:Label ID="lblParagraph3" runat="server" dbResource="Paragraph3"></asp:Label>
                <asp:Label ID="lblParagraph4" runat="server" dbResource="Paragraph4"></asp:Label>
                <asp:Label ID="lblParagraph5" runat="server" dbResource="Paragraph5"></asp:Label>
                <asp:Label ID="lblParagraph6" runat="server" dbResource="Paragraph6"></asp:Label>
                <asp:Label ID="lblParagraph7" runat="server" dbResource="Paragraph7"></asp:Label>
                <asp:Label ID="lblParagraph8" runat="server" dbResource="Paragraph8"></asp:Label>
                <asp:Label ID="lblParagraph9" runat="server" dbResource="Paragraph9"></asp:Label>
                <asp:Label ID="lblParagraph10" runat="server" dbResource="Paragraph10"></asp:Label>
                
                <!-- Newsletter controls -->
                <asp:Panel ID="panNewsletter" runat="server" Visible="false">
                    <table border="0" cellpadding="0" cellspacing="0" id="tblHomeNewsletter" runat="server"> 
                        <tr>
                            <td width="15"></td>
                            <td>
                                Name:&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" MaxLength="50" ValidationGroup="email" Height="12" Width="110" Font-Size="Smaller"></asp:TextBox>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td width="15"></td>
                            <td>
                                Email:&nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" ValidationGroup="email" Height="12" Width="110" Font-Size="Smaller"></asp:TextBox>
                            </td>
                            <td>
                                <asp:RegularExpressionValidator ID="regTxtEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="email" ErrorMessage="*" Display="Dynamic" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$"></asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr height="6"><td></td></tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                            <td align="right">
                                <asp:LinkButton ID="lnkSignup" runat="server" OnClick="btnSignup_click" ValidationGroup="email" CssClass="SubmitRollover" ToolTip="Submit" dbResource="cssSubmit"></asp:LinkButton>
                                <br />
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <asp:RequiredFieldValidator ID="reqTxtEmail" runat="server" ControlToValidate="txtEmail" ValidationGroup="email"></asp:RequiredFieldValidator>
                    <asp:Label ID="lblNewsletter" runat="server"></asp:Label>
                </asp:Panel>
            </td>
        </tr>     
     </table>
</asp:Content>