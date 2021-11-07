<%@ Page Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="homeIntro.aspx.vb" Inherits="homeIntro" title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="home" master="msite"></menu:EBMenu>
</asp:Content>
<asp:Content ID="ContentTop" ContentPlaceHolderID="contentTop" runat="server">

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="welcometext">
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
	</div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" runat="server" visible="false">
        <tr valign="top">
            <td height="40" colspan="4" style="background-image: url(images/line_dashHorizontal.gif);
                background-repeat: repeat-x; background-position: bottom;">
                <h3>
                    Objects of Desire...</h3>
            </td>
        </tr>
        <tr>
            <td>
                <!--Womolia-->
                <a href="/shop/product.aspx?id=39">
                    <img src="/images2011/home_womolia_deepLilac.jpg" alt="Womolia" width="230" height="154"
                        border="0"></a>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="padding-left: 5px;">
                            <h4>
                                Womolia Heat<br>
                            </h4>
                            Deep Lilac
                        </td>
                        <td width="110" valign="bottom">
                            <a href="product.html" class="MoreInfoRollover" title="Womolia Heat More Info"></a>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <!--Femblossom-->
                <a href="product.html">
                    <img src="/images2011/home_femblossom_lightJade.jpg" alt="Womolia" width="230" height="154"
                        border="0"></a>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="padding-left: 5px;">
                            <h4>
                                Femblossom Heat<br>
                            </h4>
                            Light Jade
                        </td>
                        <td width="110" valign="bottom">
                            <a href="product.html" class="MoreInfoRollover" title="Femblossom Heat More Info">
                            </a>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <!--Chandra-->
                <a href="product.html">
                    <img src="/images2011/home_chandra_zenGreen.jpg" alt="Womolia" width="230" height="154"
                        border="0"></a>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="padding-left: 5px;">
                            <h4>
                                Chandra<br>
                            </h4>
                            Zen Green
                        </td>
                        <td width="110" valign="bottom">
                            <a href="product.html" class="MoreInfoRollover" title="Chandra More Info"></a>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
                <!--Isis-->
                <a href="product.html">
                    <img src="/images2011/home_isis_warmOrannge.jpg" alt="Womolia" width="230" height="154"
                        border="0"></a>
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td style="padding-left: 5px;">
                            <h4>
                                Isis<br>
                            </h4>
                            Warm Orange
                        </td>
                        <td width="110" valign="bottom">
                            <a href="product.html" class="MoreInfoRollover" title="Chandra More Info"></a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr valign="top">
            <td height="20" colspan="4" style="background-image: url(images/line_dashHorizontal.gif);
                background-repeat: repeat-x; background-position: bottom;">
                &nbsp;
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" runat="server" visible="false">
        <tr>
            <td width="279" valign="top">
                <!-- Submenu-->
                <div id="RoundedCornerContent">
                    <a class="sideNav" href="xxx.html">Satisfaction Guaranteed</a>
                    <!--div id="DashedLineHorizontal">
                    </div>
                    <a class="sideNav" href="xxx.html">The History of Emotional Bliss</a><div id="DashedLineHorizontal">
                    </div>
                    <a class="sideNav" href="xxx.html">Our Goal</a><div id="DashedLineHorizontal">
                    </div>
                    <a class="sideNav" href="xxx.html">FAQ</a><div id="DashedLineHorizontal">
                    </div>
                    <a class="sideNav" href="xxx.html">Banking and Security</a><div id="DashedLineHorizontal">
                    </div>
                    <a class="sideNav" href="xxx.html">Delivery</a><div id="DashedLineHorizontal">
                    </div>
                    <a class="sideNav">Download Catalogue</a><div id="DashedLineHorizontal">
                    </div>
                    <a class="sideNav">Product Registration</a><div id="DashedLineHorizontal">
                    </div-->
                </div>
                <div id="lowerWoman">
                    <img src="/images2011/woman1.jpg" width="270" height="365"></div>
            </td>
            <td valign="top" class="mainContent">
                <h3>
                    Welcome</h3>
                <br>
                At Emotional Bliss, we believe that all women are entitled to achieve sexual wellbeing.
                That's why we've spent years perfecting an innovative and stylish range of intimate
                massagers, perfectly in tune with the way a woman's body works.<br>
                <br>
                Shocked by research suggesting that up to 75 per cent of women have never had an
                orgasm, we set about looking for the reasons why. From our extensive research comes
                a range of products scientifically proven to give women the pleasure and sexual
                fulfillment so far missing from their lives.<br>
                <br>
                Working with Julia Cole, a leading psychosexual therapist, and consulting with female
                focus groups, we've perfected the optimum levels of heat and vibration required
                for women to achieve orgasm. We guarantee our intimate massagers work for all women,
                regardless of age, or any medical conditions which might have previously limited
                their libido or ability to achieve orgasm.<br>
                <br>
                Even if you're one of the lucky 25 per cent of women who have experienced orgasm,
                Emotional Bliss promises you even more pleasure, and more often. You can use our
                intimate massagers on your own or with a partner, adding an exciting new dimension
                to your love life.<br>
                <br>
                As you will see, Emotional Bliss intimate massagers are subtle yet very effective.
                They have been produced with leading-edge technology using medical grade anti-bacterial
                materials. No wonder they've won the backing of leading sexual wellbeing experts,
                and due to their positive effect on women's sexual wellbeing, are even attracting
                interest from public health organizations too.<br>
                <br>
                <h4>
                    We know you'll enjoy your experience with Emotional Bliss.<br>
                </h4>
            </td>
        </tr>
    </table>
</asp:Content>
    