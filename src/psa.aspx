<%@ Page Language="VB" MasterPageFile="~/m_site.master" AutoEventWireup="false" CodeFile="psa.aspx.vb" Inherits="psa" title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="sexologists" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Label ID="lblPleaseWait" runat="server" OnLoad="lblPleaseWait_load"></asp:Label>
            ....
            <asp:Image ID="imgPleaseWait" runat="server" ImageUrl="~/images/loading.gif" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="Server">
        <ContentTemplate>
            <asp:Panel ID="panQuestions" runat="Server">
                <h2>
                <asp:Literal ID="litQuestionNumber" runat="Server"></asp:Literal>
                </h2><br />
                <asp:Label ID="lblQuestion" runat="Server" CssClass="sitetext"></asp:Label>
                <br /><br />
                <asp:RadioButtonList ID="radAnswer" runat="server" CssClass="sitetext" AutoPostBack="false">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem></asp:ListItem>
                </asp:RadioButtonList>
                <asp:RequiredFieldValidator ID="reqRadAnswer" runat="server" ControlToValidate="radAnswer" Display="static" ValidationGroup="q"></asp:RequiredFieldValidator>
                <br /><br />
                <asp:Label ID="lblReply1" runat="Server" Visible="false" CssClass="sitetext"></asp:Label>
                <asp:Label ID="lblReply2" runat="Server" Visible="false" CssClass="sitetext"></asp:Label>
                <asp:Label ID="lblReply3" runat="Server" Visible="false" CssClass="sitetext"></asp:Label>
                <asp:HiddenField ID="hidQuestionNo" runat="server" />
            </asp:Panel>
            <asp:ImageButton ID="btnSubmit" runat="server" dbResource="btnSubmit" OnClick="btnSubmit_click" ValidationGroup="q" />
            <asp:ImageButton ID="btnNext" runat="server" dbResource="btnNext" Visible="false" OnClick="btnNext_click" />
            <asp:Label ID="lblEnd" runat="server" dbResources="lblEnd"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>    
    <br /><br />
    <!-- Hidden resources -->
    <asp:Label ID="lblErrorResource" runat="server" dbResource="errorRadRequired"></asp:Label>
</asp:Content>

