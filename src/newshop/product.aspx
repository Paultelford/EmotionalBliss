<%@ Page Language="VB" AutoEventWireup="false" Trace="false" ValidateRequest="false" MasterPageFile="~/m_shop.master" Theme="WinXP_Blue" CodeFile="product.aspx.vb" Inherits="shop_product" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<%@ MasterType virtualPath="~/m_shop.master"%>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBShopMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="shop" master="m_shop"></menu:EBMenu>
    <br /><br />
    <table border="0" runat="server" id="tblAddToBasket">
        <tr>
            <td align="left">
                <!-- old currency/price placeholder -->
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:Label ID="lblVatText" runat="server" dbResources="lblVatText" Font-Italic="true" Visible="false" />
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <!-- BASKET -->
                <atlas:UpdatePanel ID="updateBasket" runat="server">
                <ContentTemplate>
                    <table border="0" width="100%" cellpadding="0" cellspacing="0">
                        <tr>
                            <td width="209" height="59">
                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td background="/design/shop/images/basket-box1.jpg" width="209" height="59">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="150" width="209" background="/design/shop/images/basket-box2.jpg" valign="top" align="center">
                                            <table border="0" cellpadding="0" width="86%">
                                                <tr>
                                                    <td>
                                                        <asp:GridView ID="gvBasket" runat="server" DataKeyNames="id" Font-Size="XX-Small" GridLines="none" BorderWidth="0" ShowFooter="true" AutoGenerateColumns="false" ShowHeader="false" Width="100%" OnDataBound="gvBasket_dataBound" OnRowDeleting="gvBasket_rowDeleting">
                                                            <Columns>
                                                                <asp:CommandField Visible="false" DeleteText="Delete" DeleteImageUrl="~/images/x.gif" ShowDeleteButton="true" ButtonType="Image" ItemStyle-VerticalAlign="top" />
                                                                <asp:HyperLinkField ItemStyle-VerticalAlign="top" ItemStyle-HorizontalAlign="left" DataTextField="Name" DataNavigateUrlFields="id" DataNavigateUrlFormatString="product.aspx?id={0}" ItemStyle-Width="60%" FooterStyle-HorizontalAlign="left" />
                                                                <asp:BoundField />
                                                                <asp:BoundField DataField="itemPriceInc" DataFormatString="{0:n2}" ItemStyle-VerticalAlign="top" />
                                                                <asp:TemplateField ItemStyle-VerticalAlign="top">
                                                                    <ItemTemplate>
                                                                        x
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField DataField="Qty" ItemStyle-HorizontalAlign="right" ItemStyle-VerticalAlign="top" />
                                                                <asp:BoundField />
                                                            </Columns>
                                                        </asp:GridView>  
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td background="/design/shop/images/basket-box3.jpg" with="209" height="18"></td>
                                    </tr>
                                    <tr>
                                        <td height="1" background="/design/shop/images/basket-box4a.jpg">
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center">
                                                        <asp:HyperLink ID="lnkCheckout" runat="server" ImageUrl="/design/shop/images/btn_checkout_gb.jpg" Font-Bold="true" NavigateUrl="~/shop/basket.aspx"></asp:HyperLink>
                                                    </td>
                                                </tr>
                                             </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="28" width="209" background="/design/shop/images/basket-box5.jpg">
                                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td align="center">
                                                        
                                                    </td>
                                                </tr>    
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                        </tr>
                    </table>
                    <br /><br />
                </ContentTemplate>
                
            </atlas:UpdatePanel>
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">AC_FL_RunContent = 0;</script>
    <script type="text/javascript" src="/AC_RunActiveContent.js" language="javascript"></script>
    <!-- Left hand Info panel -->
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
            <table border="0" cellpadding="0" cellspacing="0" width="615">
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
                                                            <div id="pricesleft">
                                                                <h4>
                                                                    <asp:Literal ID="lblCurrencySign" runat="server"></asp:Literal><asp:Literal ID="lblPrice" runat="server"></asp:Literal>&nbsp;(inc vat) 
                                                                </h4>
                                                            </div>
                                                            <asp:Label ID="lblProdDescription" runat="server"></asp:Label>
                                                
                                                <table>        
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Table ID="panVoucherForm" runat="server" Visible="false">
                                                                <asp:TableRow>
                                                                    <asp:TableCell>
                                                                                <br /><br />
                                                                        <asp:Label id="lblFieldsMandatory" runat="server" dbResources="lblFieldsMandatory"></asp:Label>
                                                                        <br /><br />
                                                                        <table border="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblMessage" runat="server" dbResources="lblMessage"></asp:Label>
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="5" Columns="30"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormReqTxtMessage" runat="server" ControlToValidate="txtMessage" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblRecipient" runat="server" dbResources="lblRecipient"></asp:Label>
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtRecipientName" runat="Server" MaxLength="50"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormReqTxtRecipientName" runat="server" ControlToValidate="txtRecipientName" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblYourName" runat="server" dbResources="lblYourName"></asp:Label>
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtPurchaserName" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormReqPurchaseName" runat="server" ControlToValidate="txtPurchaserName" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblRecipientEmail" runat="server" dbResources="lblRecipientEmail"></asp:Label>
                                                                                    <br />
                                                                                    <asp:TextBox ID="txtRecipientEmail" runat="server" MaxLength="50"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="panVoucherFormTxtRecipientEmail" runat="server" ControlToValidate="txtRecipientEmail" dbResources="errorRequired" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                                                                    <asp:RegularExpressionValidator ID="regTxtRecipientEmail" runat="server" ControlToValidate="txtRecipientEmail" dbResources="errorInvalidEmail" Display="dynamic" ValidationGroup="add" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$"></asp:RegularExpressionValidator>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <br />
                                                                                    <asp:Label id="lblAnonymous" runat="server" dbResources="lblAnonymous"></asp:Label>
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label id="lblYourEmail" runat="server" dbResources="lblYourEmail"></asp:Label>
                                                                                    <br />
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
		                                                    'height', '330',
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
		                                                    'bgcolor', '#84abd2',
		                                                    'name', 'Graph',
		                                                    'menu', 'true',
		                                                    'allowFullScreen', 'false',
		                                                    'allowScriptAccess','sameDomain',
		                                                    'movie', 'Graph',
		                                                    'salign', ''
		                                                    ); //end AC code
                                                    }
                                                </script>
                                                <noscript>
                                                    <object classid="clsid:d27cdb6e-ae6d-11cf-96b8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0" width="600" height="330" id="Graph" align="middle">
                                                    <param name="allowScriptAccess" value="sameDomain" />
                                                    <param name="allowFullScreen" value="false" />
                                                    <param name="movie" value="Graph.swf" /><param name="quality" value="high" /><param name="bgcolor" value="#84abd2" />	<embed src="Graph.swf" quality="high" bgcolor="#84abd2" width="600" height="330" name="Graph" align="middle" allowScriptAccess="sameDomain" allowFullScreen="false" type="application/x-shockwave-flash" pluginspage="http://www.macromedia.com/go/getflashplayer" />
                                                    </object>
                                                </noscript>
                                            </asp:Panel>
                                        </ContentTemplate>
                                        <Triggers>
                                            
                                        </Triggers>
                                    </atlas:UpdatePanel>
                                    <!-- REVIEWS -->
                                    <asp:Panel ID="panProductDescription4" runat="server" Visible="false">
                                        <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <table border="0" id="tblAddReviewControls" runat="server">
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:Label ID="lblCurrentReviewsText1" runat="server" dbResource="lblCurrentReviewsText1"></asp:Label>
                                                                <asp:Label ID="lblNumberOfReviews" runat="server"></asp:Label>
                                                                <asp:Label ID="lblCurrentReviewstext2" runat="server" dbResource="lblCurrentReviewsText12"></asp:Label><br />        
                                                                <br />
                                                                <asp:LinkButton ID="lnkAddReview" runat="server" Text="Add Review" OnClick="lnkAddReview_click"></asp:LinkButton>
                                                            </td>
                                                        </tr>                                                
                                                    </table>
                                                    <table border="0" id="tblAddReviewType" runat="server" visible="false">
                                                        <tr>
                                                            <td>
                                                                Which type of review would you like to leave:            
                                                            </td>
                                                            <td>
                                                                <asp:DropDownList ID="drpAddReviewType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpAddReviewType_selectedIndexChanged">
                                                                    <asp:ListItem Text="Please Choose" Value="0"></asp:ListItem>
                                                                    <asp:ListItem Text="Full EB Review" Value="full"></asp:ListItem>
                                                                    <asp:ListItem Text="Short Review" Value="lite"></asp:ListItem>
                                                                </asp:DropDownList>&nbsp;    
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <table border="0" id="tblAddReview" runat="server" width="540" visible="false">
                                                        <tr id="tdQ1" runat="server" visible="false">
                                                            <td>
                                                                <b>What was your first impression of the <asp:label ID="lblProduct1" runat="server" />?</b><br /><br />
                                                                <FCKeditorV2:FCKeditor id="fckQ1" runat="server" BasePath="~/EBEditor/" Width="100%" ToolbarStartExpanded="false" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="160" />
                                                                <asp:Button ID="btnQ1Next" runat="server" Text="Next >>" OnClick="btnNext_click" CommandArgument="q1" />                                                            
                                                                <br />
                                                                <asp:Label ID="lblErrorQ1" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdQ2" runat="server" visible="false">
                                                            <td>
                                                                <b>What was it like in action?</b><br /><br />
                                                                <FCKeditorV2:FCKeditor id="fckQ2" runat="server" BasePath="~/EBEditor/" Width="100%" ToolbarStartExpanded="false" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="160" />
                                                                <asp:Button ID="btnQ2Next" runat="server" Text="Next >>" OnClick="btnNext_click" CommandArgument="q2" />
                                                                <br />
                                                                <asp:Label ID="lblErrorQ2" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdQ3" runat="server" visible="false">
                                                            <td>
                                                                <b>Did it make you feel sexy?</b><br /><br />
                                                                <FCKeditorV2:FCKeditor id="fckQ3" runat="server" BasePath="~/EBEditor/" Width="100%" ToolbarStartExpanded="false" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="160" />
                                                                <asp:Button ID="btnQ3Next" runat="server" Text="Next >>" OnClick="btnNext_click" CommandArgument="q3" />
                                                                <br />
                                                                <asp:Label ID="lblErrorQ3" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdQ4" runat="server" visible="false">
                                                            <td>
                                                                <b>What reaction did the <asp:label ID="lblProduct2" runat="server" /> acheive - personally to you or with other parties?</b><br /><br />
                                                                <FCKeditorV2:FCKeditor id="fckQ4" runat="server" BasePath="~/EBEditor/" Width="100%" ToolbarStartExpanded="false" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="160" />
                                                                <asp:Button ID="btnQ4Next" runat="server" Text="Next >>" OnClick="btnNext_click" CommandArgument="q4" />
                                                                <br />
                                                                <asp:Label ID="lblErrorQ4" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdQ5" runat="server" visible="false">
                                                            <td>
                                                                <b>Any other comments?</b><br /><br />
                                                                <FCKeditorV2:FCKeditor id="fckQ5" runat="server" BasePath="~/EBEditor/" Width="100%" ToolbarStartExpanded="false" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="160" />
                                                                <asp:Button ID="btnQ5Next" runat="server" Text="Next >>" OnClick="btnNext_click" CommandArgument="q5" />
                                                                <br />
                                                                <asp:Label ID="lblErrorQ5" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdQ6" runat="server" visible="false">
                                                            <td>
                                                                <b>How could the <asp:label ID="lblProduct3" runat="server" /> be improved?</b><br /><br />
                                                                <FCKeditorV2:FCKeditor id="fckQ6" runat="server" BasePath="~/EBEditor/" Width="100%" ToolbarStartExpanded="false" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="160" />
                                                                <asp:Button ID="btnQ6Next" runat="server" Text="Next >>" OnClick="btnNext_click" CommandArgument="q6" />
                                                                <br />
                                                                <asp:Label ID="lblErrorQ6" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdQ7" runat="server" visible="false">
                                                            <td>
                                                                <b>Would you recomendit to a friend?</b><br /><br />
                                                                <FCKeditorV2:FCKeditor id="fckQ7" runat="server" BasePath="~/EBEditor/" Width="100%" ToolbarStartExpanded="false" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="160" />
                                                                <asp:Button ID="btnQ7Next" runat="server" Text="Next >>" OnClick="btnNext_click" CommandArgument="q7" />
                                                                <br />
                                                                <asp:Label ID="lblErrorQ7" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdQ8" runat="server" visible="false">
                                                            <td>
                                                                <b>Do you have any hot tips when using the <asp:label ID="lblProduct4" runat="server" />?</b><br /><br />
                                                                <FCKeditorV2:FCKeditor id="fckQ8" runat="server" BasePath="~/EBEditor/" Width="100%" ToolbarStartExpanded="false" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="160" />
                                                                <asp:Button ID="btnQ8Next" runat="server" Text="Next >>" OnClick="btnNext_click" CommandArgument="q8" />
                                                                <br />
                                                                <asp:Label ID="lblErrorQ8" runat="server"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr id="tdQ9" runat="server" visible="false">
                                                            <td>    
                                                                <table>
                                                                    <tr>
                                                                        <td>
                                                                            <b>Your Name:</b>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtName" runat="server" MaxLength="50"></asp:TextBox>
                                                                            <asp:RequiredFieldValidator ID="reqTxtName" runat="server" ControlToValidate="txtName" ErrorMessage="* Required" Display="dynamic" ValidationGroup="finish"></asp:RequiredFieldValidator>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <b>Product Rating:</b>
                                                                        </td>
                                                                        <td>
                                                                            <asp:DropDownList ID="drpRating" runat="server">
                                                                                <asp:ListItem Text="1 - (Poor)" Value="1"></asp:ListItem>
                                                                                <asp:ListItem Text="2 - (Average)" Value="2"></asp:ListItem>
                                                                                <asp:ListItem Text="3 - (Good)" Value="3" Selected="true"></asp:ListItem>
                                                                                <asp:ListItem Text="4 - (Very Good)" Value="4"></asp:ListItem>
                                                                                <asp:ListItem Text="5 - (Excellent)" Value="5"></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <b>How much would you be willing<br /> to pay for <asp:label ID="lblProduct5" runat="server" />:</b>
                                                                        </td>
                                                                        <td valign="baseline">
                                                                            <asp:TextBox ID="txtWillingToPay" runat="server"></asp:TextBox>
                                                                            <asp:Label ID="lblWillingError" runat="Server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <b>Would you like to participate in more reviews for Emotional Bliss ?</b><br />
                                                                <asp:RadioButtonList ID="radMoreReviews" runat="server" AutoPostBack="true" OnSelectedIndexChanged="radMoreReviews_selectedIndexChanged">
                                                                    <asp:ListItem Text="Yes" Value="True"></asp:ListItem>
                                                                    <asp:ListItem Text="No" Value="False"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                                <br />
                                                                <table id="tblEmail" runat="server" visible="false">
                                                                    <tr>
                                                                        <td>
                                                                            Email Address:&nbsp;
                                                                            <br /><br />
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" ValidationGroup="finish"></asp:TextBox>&nbsp;
                                                                            <asp:RequiredFieldValidator ID="reqTxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="* Required" Display="dynamic" ValidationGroup="finish"></asp:RequiredFieldValidator>
                                                                            <asp:RegularExpressionValidator ID="regTxtEmail" runat="server" ControlToValidate="txtEMail" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$" ErrorMessage="* Invalid email" Display="dynamic" ValidationGroup="finish"></asp:RegularExpressionValidator>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <asp:Button ID="btnFinish" runat="server" Text="Finish" OnClick="btnFinish_click" ValidationGroup="finish" Visible="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:DetailsView ID="dvAddReview" runat="Server" GridLines="none" DataSourceID="sqlAddReview" Visible="false" DefaultMode="insert" OnItemInserting="dvAddReview_inserting">
                                                    <Fields>
                                                        <asp:TemplateField HeaderText="Your Name:" HeaderStyle-Width="180">
                                                            <InsertItemTemplate>
                                                                <asp:TextBox ID="txtName" runat="Server" MaxLength="40" Text='<%# Bind("name") %>'></asp:TextBox>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Score:" >
                                                            <InsertItemTemplate>
                                                                <asp:DropDownList ID="drpRating" runat="server" selectedvalue='<%# Bind("rating") %>'>
                                                                    <asp:ListItem Text="1 - (Poor)" Value="1"></asp:ListItem>
                                                                    <asp:ListItem Text="2 - (Average)" Value="2"></asp:ListItem>
                                                                    <asp:ListItem Text="3 - (Good)" Value="3"></asp:ListItem>
                                                                    <asp:ListItem Text="4 - (Very Good)" Value="4"></asp:ListItem>
                                                                    <asp:ListItem Text="5 - (Excellent)" Value="5" Selected="true"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="<nobr>How much would you</nobr><nobr> be willing to pay ?</nobr>" ItemStyle-VerticalAlign="bottom">
                                                            <InsertItemTemplate>
                                                                <asp:TextBox ID="txtCost" runat="server" Text='<%# Bind("willingToPay") %>' MaxLength="50"></asp:TextBox>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Review:" HeaderStyle-VerticalAlign="top">
                                                            <InsertItemTemplate>
                                                                <asp:TextBox ID="txtReview" runat="Server" TextMode="multiLine" Rows="6" Columns="60" Text='<%# Bind("review") %>'></asp:TextBox><br />
                                                                <asp:RequiredFieldValidator ID="reqTxtReview" ValidationGroup="add" runat="server" ControlToValidate="txtReview" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <InsertItemTemplate>
                                                                <asp:Label ID="lblAddReviewInstructionsText" runat="Server">
                                                                    All product reviews submitted are reviewed by Emotional Bliss prior to being posted on the site. Once your form has been completed you will be directed back to the product your were viewing.
                                                                </asp:Label><br />
                                                                <asp:ImageButton ID="btnAdd" runat="server" CommandName="insert" ValidationGroup="add" ImageUrl="~/images/navImages/btn_submit_gb.jpg" /><br />
                                                                <asp:Label ID="lblAddReviewError" runat="server"></asp:Label>
                                                            </InsertItemTemplate>
                                                        </asp:TemplateField>
                                                    </Fields>                                                    
                                                </asp:DetailsView>

                                                <asp:Label ID="lblThankyou" runat="server" Visible="false">
                                                    Thankyou for filling out the EmotionalBliss review. <br />
                                                    All submitted reviews will be checked by Emotional Bliss before being displayed on the website.<br /><br />
                                                    <asp:HyperLink ID="hypBackToProduct" runat="server" Text="Back to product" OnLoad="hypBackToProduct_load"></asp:HyperLink>
                                                </asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Image id="imgReviewThumb" runat="server" Width="200" Height="144" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                    <br /><br />
                                    <atlas:UpdateProgress ID="up2" runat="server" AssociatedUpdatePanelID="update2">
                                        <ProgressTemplate>
                                            <asp:Label ID="lblPleaseWait" runat="server" Text="Please Wait"></asp:Label>
                                            ....
                                            <asp:Image ID="imgPleaseWait" runat="server" ImageUrl="~/images/loading.gif" />
                                        </ProgressTemplate>
                                    </atlas:UpdateProgress>
                                    <atlas:UpdatePanel ID="update2" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="panProductDescription4b" runat="server" Visible="false">
                                                Which type of review would you like to view: <asp:DropDownList ID="drpReviewType" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="drpReviewType_selectedIndexChanged">
                                                    <asp:ListItem Text="Short Review" Value="lite"></asp:ListItem>
                                                    <asp:ListItem Text="Full EB Review" Value="full"></asp:ListItem>
                                                </asp:DropDownList>
                                                <asp:GridView ID="gvReviewsLite" runat="server" DataSourceID="sqlReviews" SkinID="gridview" Width="100%" AutoGenerateColumns="true" OnDataBound="gvReviewsLite_dataBound" ShowHeader="false" EmptyDataText="The are currently no short reviews for this product">
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
                                                        <asp:BoundField ItemStyle-Width="40" DataField="willingToPay" />
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
                                                <table runat="server" id="tblProductReviews" visible="false" width="100%">
                                                    <tr>
                                                        <td>
                                                            <asp:DetailsView ID="dvProductReviews" runat="server" GridLines="none" HeaderStyle-Width="15%" HeaderStyle-Font-Bold="true" SkinID="DetailsView" Width="100%" AutoGenerateRows="false" DataSourceID="sqlReviewsFull" AllowPaging="true" OnDataBound="dvProductReviews_dataBound" Visible="true" EmptyDataText="There are currently no EB reviews for this product" PagerSettings-NextPageText="" PagerSettings-PreviousPageText="" PagerSettings-Mode="NextPrevious" OnPageIndexChanged="dvProductReviews_pageIndexChange">
                                                                <Fields>
                                                                    <asp:BoundField HeaderText="Review Date:" DataField="dateAdded" HeaderStyle-Font-Bold="true" DataFormatString="{0:dd MMM yyyy}" />
                                                                    <asp:BoundField HeaderText="Review By:" DataField="name"  HeaderStyle-Font-Bold="true" />
                                                                    <asp:TemplateField HeaderText="Rating:"  HeaderStyle-Font-Bold="true" ItemStyle-Width="85%">
                                                                        <ItemTemplate>
                                                                            <asp:Image id="imgStar1" runat="Server" ImageUrl="~/images/reviewStar.gif" />
                                                                            <asp:Image id="imgStar2" runat="Server" ImageUrl="~/images/reviewStarGray.gif" />
                                                                            <asp:Image id="imgStar3" runat="Server" ImageUrl="~/images/reviewStarGray.gif" />
                                                                            <asp:Image id="imgStar4" runat="Server" ImageUrl="~/images/reviewStarGray.gif" />
                                                                            <asp:Image id="imgStar5" runat="Server" ImageUrl="~/images/reviewStarGray.gif" />
                                                                            <asp:Label ID="lblRating" runat="server" Visible="false" Text='<%# Eval("rating") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <b>What was your first impression of the item sent to you?</b><br />
                                                                            <asp:Label ID="lblQ1" runat="server" Font-Bold="false" Text='<%# Eval("q1") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <b>What was the item like in action?</b><br />
                                                                            <asp:Label ID="lblQ2" runat="server" Text='<%# Eval("q2") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <b>Did the item make you feel sexy?</b><br />
                                                                            <asp:Label ID="lblQ3" runat="server" Text='<%# Eval("q3") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <b>What reaction did the item acheive - personally to you or with other parties?</b><br />
                                                                            <asp:Label ID="lblQ4" runat="server" Text='<%# Eval("q4") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <b>Any other comments?</b><br />
                                                                            <asp:Label ID="lblQ5" runat="server" Text='<%# Eval("q5") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <b>How could this item be improved?</b><br />
                                                                            <asp:Label ID="lblQ6" runat="server" Text='<%# Eval("q6") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <b>Would you recomend this item to a friend?</b><br />
                                                                            <asp:Label ID="lblQ7" runat="server" Text='<%# Eval("q7") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <b>Do you have any hot tips when using this item?</b><br />
                                                                            <asp:Label ID="lblQ8" runat="server" Text='<%# Eval("q8") %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField HeaderStyle-VerticalAlign="top" HeaderText="Review:" HeaderStyle-Font-Bold="true">
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblReview" runat="server" Text='<%# FormatReview(Eval("review")) %>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <FooterTemplate>
                                                                            
                                                                        </FooterTemplate>
                                                                    </asp:TemplateField>
                                                                </Fields>
                                                            </asp:DetailsView>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center">
                                                            <asp:LinkButton ID="lnkPrev" runat="Server" Text="Prev" Font-Bold="true" OnClick="lnkPrev_click"></asp:LinkButton>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Label ID="lblPager" runat="server" Text=""></asp:Label>
                                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:LinkButton ID="lnkNext" runat="Server" Text="Next" Font-Bold="true" OnClick="lnkNext_click"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Label id="lblProductFrameError" runat="server" foreColor="red"></asp:Label>
                                        </ContentTemplate>
                                    </atlas:UpdatePanel>
                                </td>
                            </tr>
                        </table>    
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="addDetails_ID" runat="server" />
            <asp:HiddenField ID="addDetails_Name" runat="server" />
            <asp:HiddenField ID="addDetails_Price" runat="server" />
            <asp:HiddenField ID="addDetails_Discount" runat="server" />
            <asp:HiddenField ID="addDetails_PriceIncDiscount" runat="server" />
            <asp:HiddenField ID="addDetails_Vat" runat="server" />
            <asp:HiddenField ID="addDetails_DistBuyingID" runat="server" />
            <asp:HiddenField ID="addDetails_componentCode" runat="server" />
            <br /><br />    
            <asp:SqlDataSource ID="sqlReviews" runat="server" SelectCommand="procReviewsByPosIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
                <SelectParameters>
                    <asp:QueryStringParameter QueryStringField="ID" name="posID" Type="int32" />
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
            <table width="100%" id="tblButtons" runat="Server">
                <tr>
                    <td id="tdButton1" runat="Server" align="center">
                        <asp:ImageButton ID="lnkSubMenuSensations" runat="server" ImageUrl="/design/shop/images/sensations-graph-btn.jpg" OnClick="lnkSubMenuSensations_click" Visible="false" />        
                        <asp:ImageButton ID="lnkBack1" runat="server" ImageUrl="/design/shop/images/backtoprod_gb.jpg" OnClick="lnkSubMenuOverview_click" Visible="false" CommandArgument="lnkSubMenuSensations" />
                    </td>
                    <td id="tdButton2" runat="Server" align="center">
                        <asp:ImageButton ID="lnkSubMenuReviews" runat="server" ImageUrl="/design/shop/images/customer-review-btn.jpg" OnClick="lnkSubMenuReviews_click" />
                        <asp:ImageButton ID="lnkBack2" runat="server" ImageUrl="/design/shop/images/backtoprod_gb.jpg" OnClick="lnkSubMenuOverview_click" Visible="false" CommandArgument="lnkSubMenuReviews" />
                    </td>
                    <td id="tdButton3" runat="Server" align="center">
                        <asp:ImageButton ID="btnAddToBasket" runat="server" ImageUrl="/design/shop/images/add-basket-btn3.jpg" OnClick="btnAddToBasket_click" ValidationGroup="add" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hidSensationsVisible" runat="Server" />
            <asp:HiddenField ID="hidReviewsVisible" runat="Server" />
   <script type="text/JavaScript">
    <!--
    function MM_preloadImages() { //v3.0
      var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
        var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
        if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
    }

    function MM_swapImgRestore() { //v3.0
      var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
    }

    function MM_findObj(n, d) { //v4.01
      var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
        d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
      if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
      for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
      if(!x && d.getElementById) x=d.getElementById(n); return x;
    }

    function MM_swapImage() { //v3.0
      var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
       if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
    }
    //-->
    </script>
    <SCRIPT LANGUAGE="JavaScript">
    /*
    on precharge les images de substitution
    */
    i1 = new Image;
    i1.src = "/design/shop/images/womolia-info-pic3.jpg";
    i2 = new Image;
    i2.src = "/design/shop/images/womolia-info-pic2.jpg";
    i3 = new Image;
    i3.src = "/design/shop/images/womolia-info-pic4.jpg";
    </SCRIPT> 
</asp:Content>


