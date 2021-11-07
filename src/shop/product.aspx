<%@ Page Language="VB" AutoEventWireup="false" Trace="false" ValidateRequest="false" MasterPageFile="~/mshop.master" CodeFile="product.aspx.vb" Inherits="shop_product" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<%@ MasterType virtualPath="~/mshop.master"%>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBShopMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">

	
	
    <script language="javascript">

        function setSiliconPrice(price) {
            if (price=='0.00'){
                document.getElementById('sLubePriceSilicon').innerHTML = "";
                document.getElementById('sLubePriceTextSilicon').innerHTML = "";
            }else{
                document.getElementById('sLubePriceSilicon').innerHTML = price;
            }
        }
        function setWaterPrice(price) {
            if (price == '0.00') {
                document.getElementById('sLubePriceWater').innerHTML = "";
                document.getElementById('sLubePriceTextWater').innerHTML = "";
            } else {
                document.getElementById('sLubePriceWater').innerHTML = price;
            }
        }
        function setLubeText(lang, free) {
            switch(lang.toString().toLowerCase()){
                case 'nl':
                    if(free){
                        document.getElementById('dialog-form').title = "EB intieme stimulators zijn ontworpen voor externe stimulatie";
                        document.getElementById('sLubeText').innerHTML = "Als een gewaardeerde klant van Emotional Bliss willen wij bieden u een gratis fles glijmiddel.<br><br>Kies uit de volgende om uw 30ml fles glijmiddel aan uw bestelling toe te voegen.";
                    }else{
                        document.getElementById('dialog-form').title = "EB intieme stimulators zijn ontworpen voor externe stimulatie";
                        document.getElementById('sLubeText').innerHTML = "Om de meest bevredigende resultaten te bereiken hebben wij twee uitstekende  sensatieverhogende glijmiddelen gecreëerd, die uw Emotional Bliss ervaring complementeren.<br><br>Kies uit de volgende om uw 30ml fles glijmiddel aan uw bestelling toe te voegen.";
                    }
                    break;
                case 'be':
                    if (free) {
                        document.getElementById('dialog-form').title = "EB intieme stimulators zijn ontworpen voor externe stimulatie";
                        document.getElementById('sLubeText').innerHTML = "Als een gewaardeerde klant van Emotional Bliss willen wij bieden u een gratis fles glijmiddel.<br><br>Kies uit de volgende om uw 30ml fles glijmiddel aan uw bestelling toe te voegen.";
                    } else {
                        document.getElementById('dialog-form').title = "EB intieme stimulators zijn ontworpen voor externe stimulatie";
                        document.getElementById('sLubeText').innerHTML = "Om de meest bevredigende resultaten te bereiken hebben wij twee uitstekende  sensatieverhogende glijmiddelen gecreëerd, die uw Emotional Bliss ervaring complementeren.<br><br>Kies uit de volgende om uw 30ml fles glijmiddel aan uw bestelling toe te voegen.";
                    }
                    break;
                default:
                    if(free){
                        document.getElementById('dialog-form').title = "EB Intimate Massagers are designed for external stimulation";
                        document.getElementById('sLubeText').innerHTML = "As a valued customer of Emotional Bliss we would like to offer you a complimentary bottle of lubricant.<br /><br />Please select from the following to add your FREE 30ml bottle of Lubricant to your order.";
                    }else{
                        document.getElementById('dialog-form').title = "EB Intimate Massagers are designed for external stimulation";
                        document.getElementById('sLubeText').innerHTML = "To achieve the most satisfying results we have created two superb lubricants to increase the level of sensation to compliment your emotional bliss experience.<br /><br />Please select from the following to add your 30ml bottle of Lubricant to your order.";
                    }
                    break;
            }
        }        
    </script>   
    <table border="0" cellpadding="0" cellspacing="0" width="230">
        <tr>
            <td>
                <menu:EBMenu ID="ebMenu1" runat="server" menuName="shop" master="mshop"></menu:EBMenu>
            </td>
        </tr>
    </table>
    <br /><br />
    
    <div style="float: left;">
    <table border="0" runat="server" id="tblAddToBasket" width="230">
        <tr>
            <td align="left">
                <!-- old currency/price placeholder -->
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="lblVatText" runat="server" dbResources="lblVatText" Font-Italic="true" Visible="false" />&nbsp;                
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <!-- New Basket -->
                <asp:UpdatePanel ID="updateBasket" runat="Server" ChildrenAsTriggers="false" UpdateMode="conditional">
                    <ContentTemplate>
                        <div id="RoundedCornerContent">
                            <div id="basketIcon">
                                <img src="/images/icon_basket.png" width="18" height="17"></div>
                                Shopping Basket
                                <div id="DashedLineHorizontal">
                                </div>
                                <asp:GridView ID="gvBasket" runat="server" DataKeyNames="id" Font-Size="XX-Small" GridLines="none" BorderWidth="0" ShowFooter="true" AutoGenerateColumns="false" ShowHeader="false" Width="100%" OnDataBound="gvBasket_dataBound" OnRowDeleting="gvBasket_rowDeleting">
                                    <Columns>
                                        <asp:CommandField Visible="false" DeleteText="Delete" DeleteImageUrl="~/images/x.gif"
                                            ShowDeleteButton="true" ButtonType="Image" ItemStyle-VerticalAlign="top" />
                                        <asp:HyperLinkField ItemStyle-VerticalAlign="top" ItemStyle-HorizontalAlign="left"
                                            DataTextField="Name" DataNavigateUrlFields="id" DataNavigateUrlFormatString="product.aspx?id={0}"
                                            ItemStyle-Width="60%" FooterStyle-HorizontalAlign="left" />
                                        <asp:BoundField />
                                        <asp:BoundField DataField="itemPriceInc" DataFormatString="{0:n2}" ItemStyle-VerticalAlign="top" />
                                        <asp:TemplateField ItemStyle-VerticalAlign="top">
                                            <ItemTemplate>
                                                x
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Qty" ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="top" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblProductCode" runat="server" Visible="false" Text='<%# Eval("ProductCode") %>'></asp:Label>&nbsp;
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                <div id="DashedLineHorizontal">
                                </div>
                                <a class="sideNav" href="/shop/basket.aspx"><asp:Literal ID="litCheckout" runat="server">Go to Checkout</asp:Literal></a><div id="DashedLineHorizontal">
                            </div>
                        </div>
                    </ContentTemplate>
                    <Triggers>
                    </Triggers>
                </asp:UpdatePanel>
            </td>
        </tr>
        <tr>
            <td align="center">
            </td>
        </tr>
        <tr id="Tr1" runat="server" visible="false">
            <td align="left">
                <!-- Placeholders for sub menu buttons-->
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">AC_FL_RunContent = 0;</script>
    <script type="text/javascript" src="/AC_RunActiveContent.js" language="javascript"></script>
    <!-- Left hand Info panel -->
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td width="100%">
                        <table border="0" width="100%" valign="top">
                            <tr>
                                <td width="100%" valign="top">
                                    <atlas:UpdatePanel id="updateProdDesc" runat="server">
                                        <ContentTemplate>
                                            <asp:Label ID="lblError" runat="server"></asp:Label>
                                            <asp:Panel ID="panProductDescription1" runat="server" Visible="true">
                                                <asp:Label ID="lblProdName" runat="server" CssClass="lightbluehead" Visible="false"></asp:Label>                                           
                                                    <!-- @Old prices splaceholder -->
                                                    <!--div id="pricesleft">
                                                        <h4 align="center">
                                                            <!-- Images Placeholder>
                                                            <span id="spanAutoImages" runat="server">
                                                                <asp:HiddenField ID="hidSaleImageName" runat="server" />
                                                                <asp:HiddenField ID="hidImage1Small" runat="server" />
                                                                <asp:HiddenField ID="hidImage1" runat="server" />
                                                                <asp:HiddenField ID="hidImage2Small" runat="server" />
                                                                <asp:HiddenField ID="hidImage2" runat="server" />
                                                                <asp:HiddenField ID="hidImage3Small" runat="server" />
                                                                <asp:HiddenField ID="hidImage3" runat="server" />
                                                                <table border="0">
                                                                    <tr>
                                                                        <td>
                                                                            <!-- Main Image -->
                                                                            <asp:Image id="imgMain" runat="server" />
                                                                        </td>
                                                                        <td>
                                                                            <!-- Placeholder for vertical thumbnails -->
                                                                            <asp:Image ID="imgImage1SmallV" runat="server" Visible="false" /><br />
                                                                            <table border="0" height="8"></table>
                                                                            <asp:Image ID="imgImage2SmallV" runat="server" Visible="false" /><br />
                                                                            <table border="0" height="8"></table>
                                                                            <asp:Image ID="imgImage3SmallV" runat="server" Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            <!-- Placeholder for horizontal thumbnails -->
                                                                            <asp:Image ID="imgImage1SmallH" runat="server" Visible="false" />&nbsp;&nbsp;
                                                                            <asp:Image ID="imgImage2SmallH" runat="server" Visible="false" />&nbsp;&nbsp;
                                                                            <asp:Image ID="imgImage3SmallH" runat="server" Visible="false" />
                                                                        </td>
                                                                        <td></td>
                                                                    </tr>
                                                                </table>
                                                            </span>
                                                              <asp:Label ID="lblShortDescription" runat="server" Font-Bold="true"></asp:Label>
                                                        </h4>
                                                   </div-->                                                          
                                                <!-- New Imageless Description-->  
                                                <table border="0" width="67%">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:LinkButton ID="lnkAddToBasket" runat="server"  ValidationGroup="add" OnClick="lnkAddToBasket_click" CssClass="AddToBasketRollover" dbResource="cssAddToBasket"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>                                              
                                                <asp:Label ID="lblProdDescription" runat="server"></asp:Label>
                                                <div style="display: none;">
                                                    <asp:Panel ID="panDiscount" runat="server" Visible="false" OnLoad="panPrice_load">
                                                        <asp:Label ID="litWasText" runat="server" dbResource="litWasText">Was</asp:Label>
                                                        <asp:Label ID="lblCurrencySign" runat="server"></asp:Label><asp:Label ID="lblPrice" runat="server"></asp:Label>&nbsp;
                                                        <asp:Label ID="lblVatText1" runat="server" dbResource="lblVatText1" Font-Italic="false" Visible="false" />
                                                        <asp:Label ID="lblVatText2" runat="server" dbResource="lblVatText2" Font-Italic="false" Visible="false" /> <br />  
                                                        <asp:Label ID="litDiscount" runat="server"></asp:Label>%&nbsp;
                                                        <asp:Label ID="litDiscountText" runat="server" dbResource="litDiscountText">Discount</asp:Label>
                                                    </asp:Panel>
                                                    <h4>
                                                        <asp:Panel ID="panPrice" runat="server" OnLoad="panPrice_load">
                                                            <asp:Label ID="litNowText" runat="server" dbResource="litNowText" Visible="false">Now</asp:Label>                                                                    
                                                            &nbsp;
                                                            <asp:Label ID="lblVatText3" runat="server" dbResource="lblVatText1" Font-Italic="false" Visible="true" />
                                                            <!-- <asp:Label ID="lblVatRate" runat="server"></asp:Label>&nbsp; -->
                                                            <asp:Label ID="lblVatText4" runat="server" dbResource="lblVatText2" Font-Italic="false" Visible="true" />    
                                                        </asp:Panel>                                                                
                                                    </h4><br />
                                                    <table border="0">
                                                        <tr>
                                                            <td width="12"></td>
                                                            <td>
                                                                <div id="fb-root"></div>
                                                                <script src="https://connect.facebook.net/en_US/all.js#appId=241262055913847&amp;xfbml=1"></script><fb:like href="" send="false" layout="button_count" show_faces="false" action="like" font="arial"></fb:like>
                                                            </td>
                                                            <td>
                                                                <g:plusone size="medium"></g:plusone>
                                                                <!-- Place this tag after the last plusone tag -->
                                                                <script type="text/javascript">
                                                                    window.___gcfg = { lang: 'en-GB' };

                                                                    (function() {
                                                                        var po = document.createElement('script'); po.type = 'text/javascript'; po.async = true;
                                                                        po.src = 'https://apis.google.com/js/plusone.js';
                                                                        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
                                                                    })();
                                                                </script>
                                                            </td>                                                            
                                                            <td>
                                                                <a href="https://twitter.com/share" class="twitter-share-button" data-count="none">Tweet</a><script type="text/javascript" src="https://platform.twitter.com/widgets.js"></script>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <table width="100%">        
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Table ID="panVoucherForm" runat="server" Visible="false" Width="100%">
                                                                <asp:TableRow>
                                                                    <asp:TableCell>
                                                                                <br /><br />
                                                                        <asp:Label id="lblFieldsMandatory" CssClass="sitetext" runat="server" dbResources="lblFieldsMandatory"></asp:Label>
                                                                        <br /><br />
                                                                        <table border="0">
                                                                            <tr>
                                                                                <td width="180" valign="top">
                                                                                    <asp:Label id="lblMessage" runat="server" CssClass="sitetext" dbResources="lblMessage"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="normaltextarea" Rows="5" Columns="30"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormReqTxtMessage" runat="server" ControlToValidate="txtMessage" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblRecipient" runat="server" CssClass="sitetext" dbResources="lblRecipient"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtRecipientName" runat="Server" MaxLength="50"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormReqTxtRecipientName" runat="server" ControlToValidate="txtRecipientName" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblYourName" runat="server" CssClass="sitetext" dbResources="lblYourName"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtPurchaserName" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormReqPurchaseName" runat="server" ControlToValidate="txtPurchaserName" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblRecipientEmail" runat="server" CssClass="sitetext" dbResources="lblRecipientEmail"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtRecipientEmail" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormTxtRecipientEmail" runat="server" ControlToValidate="txtRecipientEmail" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <asp:RegularExpressionValidator ID="regTxtRecipientEmail" runat="server" ControlToValidate="txtRecipientEmail" dbResources="errorInvalidEmail" Display="dynamic" ValidationGroup="add" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$"></asp:RegularExpressionValidator>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>&nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <br />
                                                                                    <asp:Label id="lblAnonymous" runat="server" CssClass="sitetext" dbResources="lblAnonymous"></asp:Label>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblYourEmail" runat="server" CssClass="sitetext" dbResources="lblYourEmail"></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtPurchaserEmail" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormTxtPurchserEmail" runat="server" ControlToValidate="txtPurchaserEmail" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <asp:RegularExpressionValidator ID="regTxtPurchaserEmail" runat="server" ControlToValidate="txtPurchaserEmail" dbResources="errorInvalidEmail" Display="dynamic" ValidationGroup="add" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$"></asp:RegularExpressionValidator>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                                                                
                                            </asp:Panel>
                                            <asp:Panel ID="panProductDescription2" runat="server" Visible="false">
                                            <asp:Label ID="lblProductDescription2" runat="server"></asp:Label>
                                            </asp:Panel>
                                            <asp:Panel id="panProductDescription3" runat="server" Visible="false">
                                                <script language="javascript">
                                                    if (AC_FL_RunContent == 0) {
                                                        alert("This page requires AC_RunActiveContent.js.");
                                                    } else {
                                                        AC_FL_RunContent(
			                                                'codebase', 'http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0',
			                                                'width', '600',
			                                                'height', '400',
			                                                'src', 'Graph',
			                                                'quality', 'high',
			                                                'pluginspage', 'http://www.macromedia.com/go/getflashplayer',
			                                                'align', 'middle',
			                                                'play', 'true',
			                                                'loop', 'true',
			                                                'scale', 'showall',
			                                                'wmode', 'window',
			                                                'devicefont', 'false',
			                                                'id', 'Graph',
			                                                'bgcolor', '#fff',
			                                                'name', 'Graph',
			                                                'menu', 'true',
			                                                'allowFullScreen', 'false',
			                                                'allowScriptAccess', 'sameDomain',
			                                                'movie', 'Graph',
			                                                'salign', ''
			                                                ); //end AC code
                                                                                                    }
                                                </script>
                                                <noscript>
	                                                <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0" width="600" height="330" id="Graph" align="middle">
	                                                <param name="allowScriptAccess" value="sameDomain" />
	                                                <param name="allowFullScreen" value="false" />
	                                                <param name="movie" value="Graph.swf" /><param name="quality" value="high" /><param name="bgcolor" value="#fff" />	<embed src="Graph.swf" quality="high" bgcolor="#84abd2" width="600" height="400" name="Graph" align="middle" allowScriptAccess="sameDomain" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
	                                                </object>
                                                </noscript>
                                                 
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            <atlas:PostBackTrigger ControlID="lnkAddToBasket" />
                                        </Triggers>
                                    </atlas:UpdatePanel>
                                    <!-- REVIEWS -->
                                     <atlas:UpdateProgress ID="up2" runat="server" AssociatedUpdatePanelID="update2">
                                        <ProgressTemplate>
                                            <asp:Label ID="lblPleaseWait" runat="server" Text="Please Wait"></asp:Label>
                                            ....
                                            <asp:Image ID="imgPleaseWait" runat="server" ImageUrl="~/images/loading.gif" />
                                        </ProgressTemplate>
                                    </atlas:UpdateProgress>
                                    <atlas:UpdatePanel ID="update2" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="panProductDescription4" runat="server" Visible="false">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td  height="260" valign="top">
                                                        <div id="productinfocontainer">
                                                            <div id="productinfopic">
                                                                <asp:Image ID="imgProductReview" runat="server" />
                                                                <div id="productreviewbtn">
                                                                    <table>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:ImageButton ID="btnAddReview" runat="server" AlternateText="Add Review" ImageUrl="/design/shop/images/add-review-btn.jpg" OnClick="btnAddReview_click" dbResource="imgAddReview" />
                                                                            </td>
                                                                            <td>
                                                                                <asp:ImageButton ID="btnAddToBasket2" runat="server" CssClass="rollover" ImageUrl="/design/shop/images/add-basket-btn.jpg" OnClick="btnAddToBasket_click" ValidationGroup="add" Visible="false"/>
                                                                            </td>
                                                                        </tr>
                                                                    </table>                                                                   
                                                                </div>
                                                            </div>
                                                            <div id="producttext2">
                                                                <h2>
                                                                    <asp:Literal ID="litProductNameReview" runat="Server"></asp:Literal>
                                                                </h2>
                                                                <br />
                                                                <div id="pricescontainer">
                                                                    <div id="newrangelogo">
                                                                        <!--img src="/design/shop/images/new-heat-range.jpg" alt="New Heat Range" width="93" height="93" /-->
                                                                    </div>
                                                                </div>
                                                                <div id="paymentselect"><br />
		                                                            <asp:Label ID="lblCurrentReviewsText1" runat="server" dbResource="lblCurrentReviewsText1"></asp:Label>
		                                                            <asp:Label ID="lblNumberOfReviews" runat="server" Font-Bold="true"></asp:Label>
		                                                            <asp:Label ID="lblCurrentReviewsText2" runat="server" dbResource="lblCurrentReviewsText2"></asp:Label><br />   
		                                                            <br />
		                                                        </div>
		                                                    </div>
			                                            </div>
                                                    </td>
                                                </tr>
                                                </table>
                                                <br /><br />
                                                <dbResource="Review_YourName" />
                                                <dbResource="Review_Score" />
                                                <dbResource="Review_YourAge" />
                                                <dbResource="Review_Review" />
                                                <asp:DetailsView ID="dvAddReview" CssClass="sitetext" CellPadding="8" CellSpacing="8" BorderWidth="0" runat="Server" Width="608" GridLines="none" DataSourceID="sqlAddReview" Visible="false" DefaultMode="insert" OnItemInserting="dvAddReview_inserting">
                                                    <Fields>
                                                        <asp:TemplateField HeaderText="Your Name:" HeaderStyle-Width="300">
                                                            <InsertItemTemplate>
                                                                <asp:TextBox ID="txtName" runat="Server" MaxLength="40" Text='<%# Bind("name") %>'></asp:TextBox>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Score:" >
                                                            <InsertItemTemplate>
                                                                <asp:DropDownList ID="drpRating" runat="server" selectedvalue='<%# Bind("rating") %>'>
                                                                    <asp:ListItem Text="1 - (Poor)" Value="1" dbResource="Review_Poor"></asp:ListItem>
                                                                    <asp:ListItem Text="2 - (Average)" Value="2" dbResource="Review_Average"></asp:ListItem>
                                                                    <asp:ListItem Text="3 - (Good)" Value="3" dbResource="Review_Good"></asp:ListItem>
                                                                    <asp:ListItem Text="4 - (Very Good)" Value="4" dbResource="Review_VeryGood"></asp:ListItem>
                                                                    <asp:ListItem Text="5 - (Excellent)" Value="5" Selected="true" dbResource="Review_Excellent"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Your Age:" ItemStyle-VerticalAlign="bottom">
                                                            <InsertItemTemplate>
                                                                <asp:TextBox ID="txtCost" runat="server" Text='<%# Bind("willingToPay") %>' MaxLength="50"></asp:TextBox>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Review:" HeaderStyle-VerticalAlign="top" ItemStyle-VerticalAlign="top">
                                                            <InsertItemTemplate>
                                                                <asp:TextBox ID="txtReview" runat="Server" TextMode="multiLine" CssClass="normaltextarea" Rows="6" Width="280" Text='<%# Bind("review") %>'></asp:TextBox><br />
                                                                <asp:RequiredFieldValidator ID="reqTxtReview" ValidationGroup="add" runat="server" ControlToValidate="txtReview" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <InsertItemTemplate>
                                                                <asp:Label ID="lblAddReviewInstructionsText" CssClass="sitetext" runat="Server" dbResource="lblAddReviewInstructionsText">
                                                                    
                                                                </asp:Label><br />
                                                                <asp:ImageButton ID="btnSubmitReview" runat="server" CommandName="insert" ValidationGroup="add" dbResource="imgSubmitReview" /><br />
                                                                <asp:Label ID="lblAddReviewError" runat="server"></asp:Label>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                    </Fields>                                                    
                                                </asp:DetailsView>

                                                <asp:Label ID="lblThankyou" runat="server" Visible="false" dbResource="lblThankyou"></asp:Label>
                                                <br /><br />
                                                
                                                <asp:HyperLink ID="hypBackToProduct" Visible="false" runat="server" Text="Back to product" OnLoad="hypBackToProduct_load"></asp:HyperLink>
                                            </asp:Panel>
                                            <br /><br />
                                   
                                            <asp:Panel ID="panProductDescription4b" runat="server" Visible="false">
                                                <asp:GridView ID="gvReviewsLite" runat="server" DataSourceID="sqlReviews" SkinID="gridview" Width="100%" AutoGenerateColumns="false" OnDataBound="gvReviewsLite_dataBound" ShowHeader="false" dbResource="NoReviewsFound">
                                                    <Columns>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblReviewedByText" runat="server" dbResource="lblReviewedByText" Font-Bold="true"></asp:Label>&nbsp;
                                                                <asp:Label ID="lblName" runat="Server" Text='<%# Eval("name") %>'></asp:Label>
                                                                <br />
                                                                <asp:Label ID="lblReview" runat="server" Text='<%# formatReview(Eval("review")) %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <HeaderTemplate>
                                                                <asp:Label ID="lblTotalReviews" runat="server" Text='<%# Eval("qty") %>'></asp:Label>
                                                                <asp:Label ID="lblAvgReview" runat="server" Text='<%# Eval("rating") %>'></asp:Label>
                                                            </HeaderTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField ItemStyle-Width="40" HeaderText="Age" />
                                                        <asp:TemplateField ItemStyle-Width="90">
                                                            <ItemTemplate>
                                                                <asp:Image id="imgStar1" runat="Server" ImageUrl="~/images/reviewStar.gif" />
                                                                <asp:Image id="imgStar2" runat="Server" ImageUrl="~/images/reviewStarGray.gif" />
                                                                <asp:Image id="imgStar3" runat="Server" ImageUrl="~/images/reviewStarGray.gif" />
                                                                <asp:Image id="imgStar4" runat="Server" ImageUrl="~/images/reviewStarGray.gif" />
                                                                <asp:Image id="imgStar5" runat="Server" ImageUrl="~/images/reviewStarGray.gif" />
                                                                <asp:Label ID="lblRating" runat="server" Visible="false" Text='<%# Eval("rating") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                </asp:GridView>
                                                
                                            </asp:Panel>
                                            <asp:Label id="lblProductFrameError" runat="server" foreColor="red"></asp:Label>
                                        </ContentTemplate>
                                    </atlas:UpdatePanel>
                                    <asp:TextBox Visible="false" ID="txtWillingToPay" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                        </table>    
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="addDetails_ID" runat="server" />
            <asp:HiddenField ID="addDetails_Name" runat="server" />
            <asp:HiddenField ID="addDetails_BasketImageName" runat="server" />
            <asp:HiddenField ID="addDetails_Price" runat="server" />
            <asp:HiddenField ID="addDetails_Discount" runat="server" />
            <asp:HiddenField ID="addDetails_PriceIncDiscount" runat="server" />
            <asp:HiddenField ID="addDetails_Vat" runat="server" />
            <asp:HiddenField ID="addDetails_Currency" runat="server" />
            <asp:HiddenField ID="addDetails_DistBuyingID" runat="server" />
            <asp:HiddenField ID="addDetails_componentCode" runat="server" />
            <asp:HiddenField ID="water_ID" runat="server" />
            <asp:HiddenField ID="water_Name" runat="server" />
            <asp:HiddenField ID="water_Price" runat="server" />
            <asp:HiddenField ID="water_Discount" runat="server" />
            <asp:HiddenField ID="water_PriceIncDiscount" runat="server" />
            <asp:HiddenField ID="water_Vat" runat="server" />
            <asp:HiddenField ID="water_Currency" runat="server" />
            <asp:HiddenField ID="water_DistBuyingID" runat="server" />
            <asp:HiddenField ID="water_ComponentCode" runat="server" />
            <asp:HiddenField ID="water_UnitPriceAfterDiscountIncVat" runat="server" />
            <asp:HiddenField ID="silicon_ID" runat="server" />
            <asp:HiddenField ID="silicon_Name" runat="server" />
            <asp:HiddenField ID="silicon_Price" runat="server" />
            <asp:HiddenField ID="silicon_Discount" runat="server" />
            <asp:HiddenField ID="silicon_PriceIncDiscount" runat="server" />
            <asp:HiddenField ID="silicon_Vat" runat="server" />
            <asp:HiddenField ID="silicon_Currency" runat="server" />
            <asp:HiddenField ID="silicon_DistBuyingID" runat="server" />
            <asp:HiddenField ID="silicon_ComponentCode" runat="server" />
            <asp:HiddenField ID="silicon_UnitPriceAfterDiscountIncVat" runat="server" />
            <br /><br />    
            <asp:SqlDataSource id="sqlRelated" runat="server" SelectCommand="procProductMenuByposIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="id" Name="posID" Type="int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="sqlReviews" runat="server" SelectCommand="procReviewsByPosIDLanguageSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="ID" name="posID" Type="int32" />
                    <asp:SessionParameter SessionField="EBLanguage" Name="lang" Type="String" Size="5" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="sqlReviewsFull" runat="server" SelectCommand="procReviewsByPosIDSelectFull" FilterExpression="review is null or review=''" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="ID" name="posID" Type="int32" />
                </SelectParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="sqlAddReview" runat="server" InsertCommand="procReviewInsertLite" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnInserted="sqlAddReview_inserted">
                <InsertParameters>
                    <asp:Parameter Name="name" Type="string" Size="40" />
                    <asp:Parameter Name="rating" Type="int16" />
                    <asp:Parameter Name="willingToPay" Type="string" Size="50" />
                    <asp:Parameter Name="review" Type="string" Size="4000" />
                    <asp:QueryStringParameter QueryStringField="id" Name="posID" Type="int32" />
                    <asp:SessionParameter SessionField="EBLanguage" Name="countryCode" Type="string" Size="5" />
                </InsertParameters>
            </asp:SqlDataSource>
            
            <!-- BUTTONS -->
            <div style="margin-top: -100px;">
                <table border="0" width="100%">
                    <tr>
                        <td></td>
                        <td colspan="2">
                        <asp:Label ID="lblNote" runat="server" ForeColor="Red" ></asp:Label>
                        </td>
                        <td colspan="3"></td>
                    </tr>
                    <tr>
                        <td width="30">&nbsp;</td>
                        <td valign="top" align="left">&nbsp;<!--shoop-->
                            <asp:Panel ID="panRelatedDropdown" runat="server">
                            <div style="padding-top: 3px;">
                                <span style="font-size:18px;"><asp:Label ID="lblCurrencySign2" runat="server"></asp:Label><asp:Label ID="lblPriceAfterDiscountIncVat" runat="server"></asp:Label></span><br>
                                <asp:DropDownList ID="drpRelated" runat="server" DataTextField="text" DataValueField="link" OnSelectedIndexChanged="drpRelated_selectedIndexChanged" DataSourceID="sqlRelated" AppendDataBoundItems="true" OnDataBound="drpRelated_dataBound" AutoPostBack="true">
                                    <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            </asp:Panel>
                        </td>
                        <td align="right" valign="top">&nbsp;<!--shoop-->
                            <asp:Panel ID="panAddToBasket" runat="server">
                                <div style="padding-top: 26px;">
                                <asp:LinkButton ID="btnAddToBasket" runat="server"  ValidationGroup="add" OnClick="lnkAddToBasket_click" CssClass="AddToBasketRollover" dbResource="cssAddToBasket"></asp:LinkButton>
                                </div>
                            </asp:Panel>
                        </td>
                        <td width="15">&nbsp;</td>
                        <td align="left" valign="top" width="200" runat="server" visible="true" style="padding-top: 36px;">
                            <asp:Panel ID="panSensationsButton" runat="server" Visible="false" Width="200"><div id="button"><asp:LinkButton ID="lnkSubMenuSensations" runat="server" CssClass="sideNav" OnClick="lnkSubMenuSensations_click" dbResource="lnkSubMenuSensations">Sensations Graph</asp:LinkButton></div></asp:Panel>
                           <asp:Panel ID="panReviewsButton" runat="server" Visible="false" Width="200"><div id="button"><asp:LinkButton ID="lnkSubMenuReviews" runat="server" CssClass="sideNav" OnClick="lnkSubMenuReviews_click" dbResource="lnkSubMenuReviews">Customer Reviews</asp:LinkButton></div></asp:Panel>
                           <asp:Panel ID="panBack1" runat="server" Visible="false" Width="200"><div id="button"><asp:LinkButton ID="lnkBack1" runat="server"  CssClass="sideNav" OnClick="lnkSubMenuOverview_click" CommandArgument="lnkSubMenuSensations">Back to Product</asp:LinkButton></div></asp:Panel>
                        </td>
                        <td width="10">&nbsp;</td>
                    </tr>
                    
                </table>
                
                
            </div>
            <atlas:UpdatePanel ID="updateButtons" runat="Server">
                <ContentTemplate>
                    
                    <!-- Show related links/products -->
                    <asp:Panel ID="panRelated" runat="server" Visible="true">
                    <span class="sitetext" style="display: none;">Choose your product: </span>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    </asp:Panel>
                    <asp:HiddenField ID="currentViewState" runat="server" />
                    <table width="100%" id="tblButtons" runat="Server">
                        <tr>
                            <td>&nbsp;</td>
                            <td colspan="2" align="right">
                                
                            </td>
                        </tr>
                        <tr>
                            <td id="tdButton1" runat="Server" align="center" valign="bottom">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left">
                                            
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td id="tdButton2" runat="Server" align="center" valign="bottom">
                                <asp:Panel ID="panBack2" runat="server" Visible="false" Width="200"><div id="button"><asp:LinkButton ID="lnkBack2" runat="server"  CssClass="sideNav" OnClick="lnkSubMenuOverview_click" Visible="false" CommandArgument="lnkSubMenuReviews">Back to Product</asp:LinkButton></div></asp:Panel>
                                
                            </td>
                            <td id="tdButton3" runat="Server" align="center" valign="bottom">
                                
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="hidSensationsVisible" runat="Server" />
                    <asp:HiddenField ID="hidReviewsVisible" runat="Server" />
                </ContentTemplate>
                <Triggers>
                    <atlas:PostBackTrigger ControlID="lnkSubMenuSensations" />
                    <atlas:PostBackTrigger ControlID="btnAddToBasket" />
                    <atlas:PostBackTrigger ControlID="lnkBack1" />
                </Triggers>
            </atlas:UpdatePanel>  
            <dbResource="imgCheckout"></dbResource>
</asp:Content>



