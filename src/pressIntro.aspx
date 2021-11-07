<%@ Page Language="VB" MasterPageFile="~/m_site.master" AutoEventWireup="false" CodeFile="pressIntro.aspx.vb" Inherits="pressIntro" title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="press release" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="ContentTop" ContentPlaceHolderID="contentTop" runat="server">
<img src="/images/navimages/_introgfx_4_gb.gif" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <table border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td>&nbsp;</td>
            <td>
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
            </td>
        </tr>
     
     </table>
</asp:Content>

