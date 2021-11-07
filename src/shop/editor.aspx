<%@ Page Language="VB" MasterPageFile="~/MasterPageSite.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="editor.aspx.vb" Inherits="editor" title="Untitled Page" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <center>
        
                <table border="0" width="930">
                    <tr>
                        <td align="left">
                            <FCKeditorV2:FCKeditor id="FCKeditor1" runat="server" BasePath="~/EBEditor/" Width="100%" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="300" />
                            <asp:Label ID="lblEditing" runat="server"></asp:Label>
                            <br />
	                        <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
	                        <br />
	                        <asp:Panel ID="panControls" runat="server" Visible="true">
	                            <table border="0" width="100%">
	                                <tr>
	                                    <td valign="top">
	                                        <table border="0">
	                                        <tr>
	                                            <td>
	                                                <b>PageName:</b>
	                                            </td>
	                                            <td colspan="2">
	                                                <asp:DropDownList ID="drpPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPage_selectedIndexChanged">
	                                                </asp:DropDownList>
	                                            </td>
	                                        </tr>
	                                        <tr>
	                                            <td colspan="3" id="tdButtons2" runat="server" align="center">
	                                                <asp:Button id="btnSubmit" runat="server" Text="Save Changes" onclick="btnSubmit_click" Visible="false" /><br />
	                                                <asp:Label ID="lblComplete" runat="server" Font-Bold="true"></asp:Label><br /><br />
	                                                <asp:Button ID="btnView" runat="server" Text="Preview Page" OnClick="btnView_click" /><br />
	                                                <asp:Button ID="btnViewUser" runat="server" Text="Preview User Data" OnClick="btnViewUser_click" /><br />
	                                                <asp:Button ID="btnNavigae" runat="server" Text="Navigate To Page" OnClick="btnNavigate_click" />
	                                            </td>
	                                        </tr>
	                                        </table>
	                                    </td>
	                                    <td valign="top" align="right">
	                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
	                                            <tr>
	                                                <td align="left" width="100%">    
	                                                    <b><asp:Label ID="lblPageName" runat="server"></asp:Label></b>
	                                                    <asp:Panel ID="panHTML" runat="server" BorderWidth="1" Visible="false">
	                                                        <table border="0" width="100%" cellpadding="6">
	                                                            <tr>
	                                                                <td width="100%">
	                                                                    <asp:Label ID="lblHTML" runat="server"></asp:Label>        
	                                                                </td>
	                                                            </tr>
	                                                        </table>			                                        
	                                                    </asp:Panel>       
	                                                </td>
	                                            </tr>
	                                        </table>
	                                    </td>
	                                </tr>
	                            </table>
	                        </asp:Panel>
                        </td>
                    </tr>
                </table>
           
    </center>        
</asp:Content>

