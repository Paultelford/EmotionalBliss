<%@ Page Language="VB" MasterPageFile="~/MasterPageSite.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="scanEmails.aspx.vb" Inherits="affiliates_scanEmails" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Select Email Template:&nbsp;
    <asp:DropDownList ID="drpEmailTemplate" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpEMailTemplate_selectedIndexChanged">
        <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
        <asp:ListItem Text="Standard" Value="standard"></asp:ListItem>
        <asp:ListItem Text="Return" Value="return"></asp:ListItem>
        <asp:ListItem Text="Part Complete" Value="partcomplete"></asp:ListItem>
        <asp:ListItem Text="Part Complete (Final Part)" Value="partcompletefinal"></asp:ListItem>
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblError1" runat="server"></asp:Label>
    <br /><br />
    <fckeditorv2:FCKeditor id="FCKeditor1" runat="server" BasePath="~/EBEditor/" Width="100%"  UseBROnCarriageReturn="false" EnableSourceXHTML="true" Height="300" ></fckeditorv2:FCKeditor>
    <asp:Button ID="btnSubmit" runat="server" Text="Save Changes" OnClick="btnSubmit_click" />
    &nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblError2" runat="server"></asp:Label>
                        
</asp:Content>

