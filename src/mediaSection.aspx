<%@ Page Title="" Language="VB" MasterPageFile="~/m_site.master" AutoEventWireup="false" CodeFile="mediaSection.aspx.vb" Inherits="mediaSection" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="press" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>Media Coverage</h2>
    <asp:Table width="100%" cellspacing="8" runat="server" id="tblMedia">
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Left">
                <table width="286" height="144" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style='background-color: #cccccc;' align="center">
                            <table border="0" cellpadding="0" cellspacing="0" width="280" height="138">
                                <tr>
                                    <td width="100">
                                        <img src="/images/media/al.jpg" />
                                    </td>
                                    <td width="190" style='background-color: White;' valign="top">
                                        <table border="0" width="100%" height="100%">
                                        <tr>
                                            <td>
                                                <h2>Avril Lavigne</h2><br />
                                                <i>Tastier than a Ploughmans Lunch but not quite as common...</i>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <img src='/images/view.gif' />
                                            </td>
                                        </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>  
            </asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <table width="286" height="144" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style='background-color: #cccccc;' align="center">
                            <table border="0" cellpadding="0" cellspacing="0" width="280" height="138">
                                <tr>
                                    <td width="100" align="left">
                                        <img src="/images/media/anl.jpg" />
                                    </td>
                                    <td width="190" align="left" style='background-color: White;' valign="top">
                                        <table border="0" cellspacing="0" width="100%" height="100%">
                                        <tr>
                                            <td>
                                                <h2>Neil Young</h2><br />
                                                <i>Still rocking in the free world...</i>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                <img src='/images/view.gif' />
                                            </td>
                                        </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>                
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
</asp:Content>

