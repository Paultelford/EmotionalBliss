<%@ Page Language="VB" MasterPageFile="~/MasterPageSite.master" AutoEventWireup="false" CodeFile="sensations.aspx.vb" Inherits="sensations" title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="SexologistsMenu" Src="~/sexMenu.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" width="130">
                <!-- Left Menu -->
                <font size="-2" color='lightblue'><b><asp:Label ID="lblHelp" runat="server" dbResources="lblHelp"></asp:Label></b></font><br />
                <hr />
                <menu:SexologistsMenu id="sex1" runat="server" menuName="sexologists"></menu:SexologistsMenu>
            </td>
            <td valign="top" width="800">
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
