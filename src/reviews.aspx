<%@ Page Title="" Language="VB" MasterPageFile="~/m_shop.master" AutoEventWireup="false" CodeFile="reviews.aspx.vb" Inherits="reviews" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBShopMenu.ascx" %>
<asp:Content ID="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="buy now" master="m_site">
    </menu:EBMenu>
    <br /><br />
    <a href="http://www.facebook.com/home.php?#/pages/Emotional-Bliss/84827783715" target="_blank"><img style="padding-top: 10px;" src="/blog/wp-content/themes/eb-theme/blog-images/facebook-join.jpg" border="0"></a>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:Label ID="PageTitle" runat="server" dbResource="PageTitle"></asp:Label>
    <asp:Label ID="lblMetaKeywords" runat="server" dbResource="metaKeywords"></asp:Label>
    <asp:Label ID="lblMetaDescription" runat="server" dbResource="metaDescription"></asp:Label>
    <span style="height: 82px;">
    <table height="82" width="100%">
        <tr>
            <td>
            <asp:Image ID="imgDept" runat="server" ImageAlign="Right" ImageUrl="/design/shop/images/reviews_gb.gif" Height="81" />
            </td>
        </tr>
    </table>
            
            
            
        </span>
    <asp:Panel ID="panTest" runat="server">
        
        <img src="/design/shop/images/intimate-massag-ttl.gif" alt="Intimate Massagers" width="600" height="50" /><br />
        <div id="shopcontainer">
            <div id="buttonshop1">
                <asp:HyperLink ID="lnkHeat1" runat="server"></asp:HyperLink>
                </div>
            <div id="buttonshop2">
                <asp:HyperLink ID="lnkHeat2" runat="server"></asp:HyperLink>
            </div>
        </div>
        <!--<div id="shopcontainer">
            <div id="buttonshop1">
                <img src="/design/shop/images/shop-jasmine-btn.jpg" alt="Intimate Massagers - Jasmine Heat" width="275" height="180" />
            </div>
            <div id="buttonshop2">
            </div>
        </div>-->
        <br class="clear" />
        <img src="/design/shop/images/finger-massag-ttl.gif" alt="'Finger' Massagers" width="600" height="50" />
        <div id="shopcontainer">
            <div id="buttonshop1">
            <asp:HyperLink ID="lnkFinger1" runat="server"></asp:HyperLink>
            </div>
            <div id="buttonshop2">
            <asp:HyperLink ID="lnkFinger2" runat="server"></asp:HyperLink>
            </div>
        </div>
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <br class="clear" />
        <asp:Panel ID="panGBOnly" runat="server" Visible="false">
        <img src="/design/shop/images/other-products-ttl.gif" alt="Other products" width="600" height="50" />
        <div id="otherproducts"><asp:HyperLink ID="lnkDept1" runat="server" dbResource="lnkDept1"><asp:Image ID="imgDept1" runat="server" CssClass="rollover" /></asp:HyperLink><asp:HyperLink ID="lnkDept2" runat="server" dbResource="lnkDept2"><asp:Image ID="imgDept2" runat="server" CssClass="rollover" /></asp:HyperLink><asp:HyperLink ID="lnkDept3" runat="server" dbResource="lnkDept3"><asp:Image ID="imgDept3" runat="server" CssClass="rollover" /></asp:HyperLink><asp:HyperLink ID="lnkDept4" runat="server" dbResource="lnkDept4"><asp:Image ID="imgDept4" runat="server" CssClass="rollover" /></asp:HyperLink><asp:HyperLink ID="lnkDept5" runat="server" dbResource="lnkDept5"><asp:Image ID="imgDept5" runat="server" CssClass="rollover" /></asp:HyperLink></div>
        </asp:Panel>
    </asp:Panel>
</asp:Content>
