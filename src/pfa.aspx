<%@ Page Language="VB" MasterPageFile="~/m_site.master" AutoEventWireup="false" CodeFile="pfa.aspx.vb" Inherits="pfa" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="press release" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="ContentTop" ContentPlaceHolderID="contentTop" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%"  id="tblHeader" runat="server">
        <tr>
            <td valign="top" width="100%"> 
                <center>
                    <p>Click on the photograph below to see a bigger version</p>
                    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
                    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
                        <ProgressTemplate>
                            <asp:Label ID="lblPleaseWait" runat="server" Text="Image loading"></asp:Label>
                            ....
                            <asp:Image ID="imgPleaseWait" runat="server" ImageUrl="~/images/loading.gif" />
                        </ProgressTemplate>
                    </atlas:UpdateProgress>
                    <atlas:UpdatePanel ID="update1" runat="server">
                        <ContentTemplate>
                            <table border="0" width="40" height="300" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="100%" height="100%">
                                        <asp:Image ID="imgPFA" runat="server" AlternateText="Click to enlarge" /><br />
                                    </td>
                                </tr>
                            </table><br />
                            <asp:ImageButton ID="btnBack" runat="server" OnClick="btnBack_click" ImageUrl="~/images/navImages/btn_prev_gb.jpg" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnNext" runat="server" OnClick="btnNext_click" ImageUrl="~/images/navImages/btn_next_gb.jpg" />
                            <asp:HiddenField ID="hidPos" runat="server" />
                            <asp:HiddenField ID="hidMax" runat="server" />
                            <br /><br />
                            <asp:Label ID="lblError" runat="server"></asp:Label>
                            <br />
                            <asp:Label ID="lblCache" runat="server" Visible="true"></asp:Label>
                        </ContentTemplate>
                    </atlas:UpdatePanel>
                </center>
            </td>
        </tr>
    </table>
</asp:Content>

