<%@ Page Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="sexologistsIntro.aspx.vb" Inherits="sexologistsIntro" title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="experts" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="ContentTop" ContentPlaceHolderID="contentTop" runat="server">
    
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" language="javascript">AC_FL_RunContent = 0;</script>
    <script type="text/javascript" src="AC_RunActiveContent.js" language="javascript"></script>
    <script language="javascript">
        if (AC_FL_RunContent == 0) {
            alert("This page requires AC_RunActiveContent.js.");
        } else {
            AC_FL_RunContent( 'codebase','http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0','width','675','height','340','id','waitingroom','align','middle','src','waitingroom','quality','high','bgcolor','#ffffff','name','waitingroom','allowscriptaccess','sameDomain','allowfullscreen','false','pluginspage','http://www.macromedia.com/go/getflashplayer','movie','waitingroom' ); //end AC code
        }
    </script>
    <noscript>
        <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0"
            width="675" height="340" id="waitingroom" align="middle">
            <param name="allowScriptAccess" value="sameDomain" />
            <param name="allowFullScreen" value="false" />
            <param name="movie" value="waitingroom.swf" />
            <param name="quality" value="high" />
            <param name="bgcolor" value="#ffffff" />
            <embed src="waitingroom.swf" quality="high" bgcolor="#ffffff" width="675" height="340"
                name="waitingroom" align="middle" allowscriptaccess="sameDomain" allowfullscreen="false"
                type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
        </object>
    </noscript>
    <asp:Label ID="PageTitle" runat="server" dbResource="PageTitle"></asp:Label>
    <asp:Label ID="lblMetaKeywords" runat="server" dbResource="metaKeywords"></asp:Label>
    <asp:Label ID="lblMetaDescription" runat="server" dbResource="metaDescription"></asp:Label>
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
</asp:Content>

