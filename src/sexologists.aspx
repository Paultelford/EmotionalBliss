<%@ Page Language="VB" MasterPageFile="~/MasterPageSite.master" AutoEventWireup="false" CodeFile="sexologists.aspx.vb" Inherits="sexologists" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    
    <table border="0" width="470">
        <tr>
            <td width="300" valign="top">
                <asp:Label ID="lblPanelEditor1" runat="server" Text="Section 1" Visible="false"></asp:Label>
                <asp:Panel ID="panEditor1" runat="server" Width="100%">
                </asp:Panel>
            </td>
            <td width="170" valign="top">
                <img src="images/cate1.jpg" width="167" height="250" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblPanelEditor2" runat="server" Text="Section 2" Visible="false"></asp:Label>
                <asp:Panel ID="panEditor2" runat="server" Width="100%">        
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>

