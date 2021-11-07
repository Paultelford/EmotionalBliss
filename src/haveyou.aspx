<%@ Page Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="haveyou.aspx.vb" Inherits="haveyou" title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu id="sex1" runat="server" menuName="sexologists" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="PageTitle" runat="server" dbResource="PageTitle"></asp:Label>
    <asp:Label ID="lblMetaKeywords" runat="server" dbResource="metaKeywords"></asp:Label>
    <asp:Label ID="lblMetaDescription" runat="server" dbResource="metaDescription"></asp:Label>
    <table border="0" cellpadding="0" cellspacing="0" width="100%"  id="tblHeader" runat="server">
        <tr>
            <td valign="top" width="800">   
                <asp:Panel ID="panQuestions" runat="server">
                    <asp:Label ID="lblIntro" runat="server" dbResource="Intro"></asp:Label>
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblParagraph1" runat="server" dbResource="Paragraph1"></asp:Label>
                    <br /><br />
                    <asp:RadioButton ID="radQ1Yes" runat="server" Text="Yes" GroupName="q1" /><br />
                    <asp:RadioButton ID="radQ1No" runat="server" Text="No" GroupName="q1" />
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblParagraph2" runat="server" dbResource="Paragraph2"></asp:Label>
                    <br /><br />
                    <asp:RadioButton ID="radQ2Yes" runat="server" Text="Yes" GroupName="q2" /><br />
                    <asp:RadioButton ID="radQ2No" runat="server" Text="No" GroupName="q2" />
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblParagraph3" runat="server" dbResource="Paragraph3"></asp:Label>
                    <br /><br />
                    <asp:RadioButton ID="radQ3Yes" runat="server" Text="Yes" GroupName="q3" /><br />
                    <asp:RadioButton ID="radQ3No" runat="server" Text="No" GroupName="q3" />
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblParagraph4" runat="server" dbResource="Paragraph4"></asp:Label>
                    <br /><br />
                    <asp:RadioButton ID="radQ4Yes" runat="server" Text="Yes" GroupName="q4" /><br />
                    <asp:RadioButton ID="radQ4No" runat="server" Text="No" GroupName="q4" />
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblParagraph5" runat="server" dbResource="Paragraph5"></asp:Label>
                    <br /><br />
                    <asp:RadioButton ID="radQ5Yes" runat="server" Text="Yes" GroupName="q5" /><br />
                    <asp:RadioButton ID="radQ5No" runat="server" Text="No" GroupName="q5" />
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblParagraph6" runat="server" dbResource="Paragraph6"></asp:Label>
                    <br /><br />
                    <asp:RadioButton ID="radQ6Yes" runat="server" Text="Yes" GroupName="q6" /><br />
                    <asp:RadioButton ID="radQ6No" runat="server" Text="No" GroupName="q6" />
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblParagraph7" runat="server" dbResource="Paragraph7"></asp:Label>
                    <br /><br />
                    <asp:RadioButton ID="radQ7Yes" runat="server" Text="Yes" GroupName="q7" /><br />
                    <asp:RadioButton ID="radQ7No" runat="server" Text="No" GroupName="q7" />
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblParagraph8" runat="server" dbResource="Paragraph8"></asp:Label>
                    <br /><br />
                    <asp:RadioButton ID="radQ8Yes" runat="server" Text="Yes" GroupName="q8" /><br />
                    <asp:RadioButton ID="radQ8No" runat="server" Text="No" GroupName="q8" />
                    <br /><br /><hr class='blueline' /><br />
                    <asp:Label ID="lblError" runat="server"></asp:Label><br />
                    <asp:LinkButton ID="lnkSubmit" runat="server" OnClick="btnSubmit_click" CssClass="SubmitRollover" ToolTip="Submit" dbResource="cssSubmit"></asp:LinkButton>
                    <dbResource="ttSubmit"></dbResource>
                </asp:Panel>
                <asp:Literal ID="lbLResults" runat="server"></asp:Literal>
                <dbResource="Yes" />
                <dbResource="No" />
                <dbResource="AnswerA" />
                <dbResource="AnswerB" />
                <dbResource="AnswerC" />
            </td>
        </tr>
    </table>

</asp:Content>

