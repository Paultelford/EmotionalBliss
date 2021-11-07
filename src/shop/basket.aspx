<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/mshop.master" CodeFile="basket.aspx.vb" Inherits="shop_basket"  %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBShopMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
	<!-- Use jQuery to do the Checkout rollover image -->
	<script type='text/javascript' src='/jquery.js'></script>
	<script type='text/javascript' src='/jquery.preload.js'></script>
	<script type='text/javascript' src='/jquery.rollover.js'></script>
	<script language="javascript">
		//Init rollover
		$(function(){Sys.Application.add_load(wireEvents);});
	</script>    
	 <!-- Checkout Stage Indicator -->
	<div id="basketmenu">
	<asp:Label id="lblShoppingBasketText" runat="server"></asp:Label>
		<!--<span class="title3">Shopping Basket</span><br />
		<img src="/design/shop/images/basket-menu-activ.gif" alt="basket" width="209" height="55" /><br />
		Payment Method<br />
		<img src="/design/shop/images/basket-menu.gif" alt="payment" width="209" height="50" /><br />
		Delivery Address<br />
		<img src="/design/shop/images/basket-menu.gif" alt="dispatch" width="209" height="50" /><br />
		Confirmation -->
		
	</div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
	<dbResource="lblShoppingBasketText" />
	<atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
	<atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
		<ProgressTemplate>
			Please Wait....<img src="/images/loading.gif" width="16" height="16" />
		</ProgressTemplate>
	</atlas:UpdateProgress>
			<menu:EBMenu ID="ebMenu1" runat="server" menuName="shop" master="mshop"></menu:EBMenu>
			<span style='visibility:hidden;'>
			Total basket weight: <asp:Label ID="lblTotalWeight" runat="server"></asp:Label>
			</span>
			<dbResource="EmptyBasket" />
			<asp:Label ID="lblHeader" runat="server" CssClass="lightbluehead" dbResource="lblShoppingBasket" Visible="false"></asp:Label>
			<h3><asp:Label ID="lblBasketNote" runat="server" dbResource="lblBasketNote"></asp:Label></h3><br />
			<div id="DashedLineHorizontal"></div>
			<asp:ListView ID="lvBasket" runat="server" DataKeyNames="id" OnItemDeleting="lvBasket_rowDeleting" OnDataBound="lvBasket_dataBound" OnItemUpdating="lvBasket_itemUpdating">
				<LayoutTemplate>
					<table width="100%" border="0" cellspacing="0" cellpadding="0">
						<tr>
							<td class="cellUnderline">Item</td>
							<td class="cellUnderline">&nbsp;</td>
							<td class="cellUnderline">Price</td>
							<td align="center" class="cellUnderline">Qty</td>
							<td align="center" class="cellUnderline">SubTotal</td>
							<td align="center" class="cellUnderline" id="colVat">Vat(%)</td>
							<td align="center" class="cellUnderline" id="colTotal">Total</td>
							<td align="center" class="cellUnderline">Remove</td>
						</tr>
						<asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
						<tr>
							<td class="cellUnderline"></td>
							<td class="cellUnderline"></td>
							<td class="cellUnderline"></td>
							<td class="cellUnderline"></td>
							<td class="cellUnderline"></td>
							<td class="cellUnderline" id="colVat"></td>
							<td class="cellUnderline" id="colTotal"></td>
							<td class="cellUnderline"></td>
						</tr>
						<tr>
							<td colspan="5" align="right" class="cellNoUnderline" valign="top">
								
								<asp:Panel ID="panDiscount1" runat="server" Visible="false">
									<asp:Label ID="lblCouponDiscount" runat="server" Font-Size="Small" Visible="false" dbResource="lblCouponDiscount"></asp:Label>
									<asp:Label ID="lblVoucherDiscount" runat="server" Font-Size="small" Visible="false" dbResource="lblVoucherDiscount"></asp:Label>
								</asp:Panel>
								<asp:Panel ID="panTotals1" runat="server">
								
								</asp:Panel>                                
								<asp:Label id="lblShipping" runat="server" Font-Size="Small" dbResource="lblShipping"></asp:Label><br />
								<asp:Label Visible="false" id="lblVAT" runat="server" font-size="small" dbResource="lblVAT"></asp:Label>
								<asp:Label id="lblVATRate" Visible="false" runat="server"></asp:Label>
								<asp:Label id="lblTotal" runat="server" font-size="small" dbResource="lblTotal"></asp:Label>
								<asp:Label ID="lblCouponOrderVat" runat="server" Font-Size="small" Visible="false" dbResource="lblCouponOrderVat"></asp:Label><br />
							</td>
							<td class="cellNoUnderline" align="right" valign="top">
								
								<asp:Panel ID="panDiscount2" runat="server" Visible="false">
									<asp:Label ID="lblTotalIncVatCoupon" font-size="small" runat="server" Text="0" Visible="false"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
								</asp:Panel>
								<asp:Panel ID="panTotals2" runat="server">
									
								</asp:Panel>
								<asp:Label ID="lblShippingCost" font-size="small" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
								<span style="">
								&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<br />
								
								</span>
							</td>
							<td class="cellNoUnderline" align="right" valign="top">
								<table width="100%" cellpadding="0" cellspacing="0">
									<tr>
										<td align="right">

											<asp:Panel ID="panDiscount3" runat="server" Visible="false">
												<asp:Label ID="lblCouponDiscountCost" Font-Size="small" runat="server" Text="0" Visible="false"></asp:Label>
												<asp:Label ID="lblVoucherDiscountCost" Font-Size="small" runat="server" Text="0" Visible="false"></asp:Label><br />
											</asp:Panel>
											<asp:Panel ID="panTotals3" runat="server">
											
											</asp:Panel>
											<asp:Label ID="lblShippingTotalCost" font-size="small" runat="server"></asp:Label><br />
											<asp:Label ID="lblCouponVatTotal" Font-Size="small" runat="server" Visible="false"></asp:Label>
											<asp:Label ID="lblVoucherLineBreak1" runat="server" Text="<br />" Visible="false"></asp:Label>
											<asp:Label ID="lblVoucherLineBreak2" runat="server" Text="<br />" Visible="false"></asp:Label>
											<asp:Label ID="lblTotalIncVat" runat="server" Text='<%# Eval("TotalInc") %>' Font-Size="small"></asp:Label>
										</td>
										<td width="16"></td>
									</tr>
								</table>
								
							</td>
							<td class="cellNoUnderline">&nbsp;</td>
						</tr>
						<tr>
							<td colspan="5" align="right" class="cellUnderline"><blockquote>
								<blockquote>
								
								</blockquote>
								</blockquote>
							</td>
							<td class="cellUnderline">&nbsp;</td>
							<td class="cellUnderline" align="right">
								<asp:Label ID="lblTotalIncVat2" runat="server" Font-Size="small"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
							</td>
							<td class="cellUnderline">&nbsp;</td>
					   </tr>
					   <tr>
							<td colspan="8" class="cellUnderline">      
								<table width="100%" border="0" cellspacing="0" cellpadding="0">
									<tr>
										<td>
											<dbResource="ttContinueShopping"></dbResource>
											<asp:LinkButton ID="lnkContinueShopping" runat="server" OnClick="lnkContinueShopping_click" dbResource="cssContinueShopping" CssClass="ContinueShoppingRollover" ToolTip="Continue Shopping"></asp:LinkButton>
										</td>
										<td width="118" align="right">
											<asp:LinkButton ID="lnkUpdate" runat="server" CssClass="Update" Visible="false" CommandName="update"></asp:LinkButton>                                        
										</td>
										<td width="118" align="right">
											<dbResource="ttProceed"></dbResource>
											<asp:LinkButton ID="lnkProceed" runat="server" dbResource="cssProceed" OnClick="btnContinue_click" CssClass="ProceedRollover" ToolTip="Proceed to checkout"></asp:LinkButton>
										</td>
									</tr>
								</table>
							</td>
						</tr>
					</table>
				</LayoutTemplate>
				<ItemTemplate>
					<tr>
						<td align="left" class="cellNoUnderline" >
							<div class="sideImagesThumbs">
								<asp:Image ID="impThumb" runat="server" ImageUrl='<%# "/images2011/" & Eval("BasketImageName") %>' Width="90" Height="68" /><br>
							</div>
						</td>
						<td  class="cellNoUnderline">
							<asp:Literal ID="lblItem" runat="server" Text='<%# Eval("Name") %>'></asp:Literal>
							<asp:Label ID="lblPearTreeProductCode" runat="server" Text='<%# Eval("productCode") %>' Visible="false"></asp:Label>
						</td>
						<td  class="cellNoUnderline">
							<asp:Literal ID="litPrice" runat="Server" Text='<%# Eval("price","{0:n2}") %>'></asp:Literal>
						</td>
						<td align="center" class="cellNoUnderline">
							<asp:TextBox ID="txtQty" runat="server" Font-Size="X-Small" Text='<%# Eval("qty") %>' Width="40" ValidationGroup="num"></asp:TextBox>
							<asp:RequiredFieldValidator ID="regQty" runat="server" ControlToValidate="txtQty" ErrorMessage="<br />* Required" ForeColor="Red" ValidationGroup="num" Display="Dynamic"></asp:RequiredFieldValidator>
							<asp:RangeValidator ID="ranQty" runat="server" ControlToValidate="txtQty" ErrorMessage="<br />* Invalid" Type="Integer" MinimumValue="0" MaximumValue="99999" ValidationGroup="num" ForeColor="Red" Display="Dynamic"></asp:RangeValidator>
						</td>
						<td align="center" class="cellNoUnderline">
							<asp:Label ID="lblSubTotal" runat="server" Text='<%# Eval("rowPriceEx","{0:n2}") %>'></asp:Label>
							
						</td>
						<td align="right" class="cellNoUnderline" id="colVat">
							<asp:Literal ID="lblVatRate" runat="server" text='<%# Eval("vat") %>'></asp:Literal>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</td>
						<td align="right" class="cellNoUnderline" id="colTotal">
							<asp:Label ID="lblRowTotal" runat="server" Text='<%# Eval("RowPriceInc","{0:n2}") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
						</td>
						<td  class="cellNoUnderline" width="100">
							<dbResource="ttUpdate" />
							<asp:LinkButton ID="lnkUpdate" runat="server" CommandArgument='<%# Eval("id") %>' CommandName="update" OnLoad="lnkUpdate_load" dbResource="cssUpdate" CssClass="UpdateRollover" ToolTip="Update Quantities" ValidationGroup="num"></asp:LinkButton>
						</td>
					</tr>
				</ItemTemplate>
			</asp:ListView>

			<br class="clear" />
			<div id="Message">
				<table width="100%" border="0" cellspacing="0" cellpadding="0" id="tblVoucher" runat="server">
					<tr>
						<td width="360" valign="top">
							<asp:Label ID="lblPromoText" runat="server" dbResource="lblPromoText"></asp:Label>
						</td>
						<td valign="top">
							<%= uCase(Session("EBShopCountry")) %>
							<asp:TextBox ID="txtVoucherNumber" runat="server" Width="170" Visible="true" MaxLength="8" AutoPostBack="false" ValidationGroup="voucher"></asp:TextBox>&nbsp;
							<asp:RequiredFieldValidator ID="reqTxtVoucherNumber" runat="server" ControlToValidate="txtVoucherNumber" ValidationGroup="voucher" dbResource="errRequired" Display="Dynamic"></asp:RequiredFieldValidator>
						</td>
						<td width="118" valign="top">
							<dbResource="ttSubmit" />
							<asp:LinkButton ID="lnkVoucherSubmit" runat="server" OnClick="lnkVoucherSubmit_click" ValidationGroup="voucher" CssClass="SubmitRollover" ToolTip="Submit voucher code" dbResource="cssSubmit"></asp:LinkButton>
						</td>
					</tr>
				</table>
			</div>
			
			
			<asp:Label ID="lblVoucherError" runat="Server"></asp:Label>
			<asp:Label ID="lblError" runat="server"></asp:Label>
			<br /><br /><br /><br /><br /><br />
			<asp:GridView ID="gvBasket" runat="server" DataKeyNames="id" CssClass="table" BorderColor="black" GridLines="none" EnableTheming="false" Visible="false" AutoGenerateColumns="false" SkinID="GridView" OnLoad="gvBasket_load" ShowFooter="true" OnRowDeleting="gvBasket_rowDeleting" Width="605" OnDataBound="gvBasket_dataBound">
				<HeaderStyle BackColor="#5987b6" />
				<Columns>
					<asp:TemplateField ItemStyle-Width="235" HeaderStyle-CssClass="th1" ItemStyle-CssClass="td1" HeaderText="Item" HeaderStyle-HorizontalAlign="center">
						<ItemTemplate>
							<asp:Literal ID="lblItem" runat="server" Text='<%# Eval("Name") %>'></asp:Literal>
							<asp:Label ID="lblPearTreeProductCode" runat="server" Text='<%# Eval("productCode") %>' Visible="false"></asp:Label>
						</ItemTemplate>
						<HeaderTemplate>
							Item
						</HeaderTemplate>
						<FooterStyle HorizontalAlign="right" VerticalAlign="top" />
						
					</asp:TemplateField>
					<asp:TemplateField ItemStyle-HorizontalAlign="right" ItemStyle-Width="50" HeaderStyle-CssClass="th2" ItemStyle-CssClass="td2">
						<FooterStyle VerticalAlign="top"  />
						<ItemTemplate>
							<asp:Literal ID="litPrice" runat="Server" Text='<%# Eval("price","{0:n2}") %>'></asp:Literal>
						</ItemTemplate>
					</asp:TemplateField>
					<asp:TemplateField HeaderText="Qty" ItemStyle-Width="55" HeaderStyle-CssClass="th2" ItemStyle-CssClass="td2" ItemStyle-HorizontalAlign="center" >
						<FooterStyle HorizontalAlign="right" VerticalAlign="top" />
						<ItemTemplate>
							<asp:TextBox ID="txtQty" runat="server" Font-Size="X-Small" Text='<%# Eval("qty") %>' Width="30"></asp:TextBox>
						</ItemTemplate>
						<FooterTemplate>
							<asp:Label id="lblShipping" runat="server" Font-Size="Small" dbResource="lblShipping"></asp:Label>
							<br />
							<asp:Label ID="lblVoucherDiscount" runat="server" Font-Size="small" Visible="false" dbResource="lblVoucherDiscount"></asp:Label>
							<asp:Label Visible="false" id="lblVAT" runat="server" font-size="small" dbResource="lblVAT"></asp:Label>
							<asp:Label id="lblVATRate" Visible="false" runat="server"></asp:Label>
							
							<asp:Label ID="lblCouponDiscount" runat="server" Font-Size="Small" Visible="false" dbResource="lblCouponDiscount"></asp:Label>
							<asp:Label ID="lblCouponOrderVat" runat="server" Font-Size="small" Visible="false" dbResource="lblCouponOrderVat"></asp:Label>
							<asp:Label id="lblTotal" runat="server" font-size="small" dbResource="lblTotal"></asp:Label>
						</FooterTemplate>
					</asp:TemplateField>            
					<asp:TemplateField HeaderText="Subtotal" ItemStyle-Width="55" HeaderStyle-CssClass="th2" ItemStyle-CssClass="td2" FooterStyle-CssClass="td4">
						<ItemStyle HorizontalAlign="right" />
						<ItemTemplate>
							<asp:Label ID="lblSubTotal" runat="server" Text='<%# Eval("rowPriceEx","{0:n2}") %>'></asp:Label>
						</ItemTemplate>
						<FooterStyle VerticalAlign="top" HorizontalAlign="right" />
						<FooterStyle />
						<FooterTemplate>

							<asp:Label ID="lblShippingCost" font-size="small" runat="server"></asp:Label><br />                            
							<asp:Label ID="lblCouponDiscountCost" Font-Size="small" runat="server" Text="0" Visible="false"></asp:Label><br />
							<asp:Label ID="lblCouponVatTotal" Font-Size="small" runat="server" Visible="false"></asp:Label><br />
							<asp:Label ID="lblTotalIncVatCoupon" font-size="small" runat="server" Text="0" Visible="false"></asp:Label><br />
						</FooterTemplate>
					</asp:TemplateField>      
					<asp:TemplateField HeaderText="Vat(%)" ItemStyle-Width="55" HeaderStyle-CssClass="th2" ItemStyle-CssClass="td2" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" FooterStyle-CssClass="td4">
						<FooterStyle />
						<ItemTemplate>
							<asp:Literal ID="lblVatRate" runat="server" text='<%# Eval("vat") %>'></asp:Literal>
						</ItemTemplate>
						<FooterStyle VerticalAlign="top" />
						<FooterTemplate>
							<asp:Label ID="lblShippingVatRate" runat="server" Font-Size="small"></asp:Label>
						</FooterTemplate>
					</asp:TemplateField>              
					<asp:TemplateField HeaderText="Total" ItemStyle-Width="55" HeaderStyle-CssClass="th2" ItemStyle-CssClass="td2" FooterStyle-CssClass="td4">
						<ItemStyle HorizontalAlign="right" />
						<ItemTemplate>
							<asp:Label ID="lblRowTotal" runat="server" Text='<%# Eval("RowPriceInc","{0:n2}") %>'></asp:Label>
						</ItemTemplate>
						<FooterStyle VerticalAlign="top" HorizontalAlign="right" />
						<FooterStyle />
						<FooterTemplate>
							<asp:Label ID="lblShippingTotalCost" runat="server" Font-Size="small"></asp:Label><br />
							<asp:Label ID="lblVoucherDiscountCost" Font-Size="small" runat="server" Text="0" Visible="false"></asp:Label>
							<asp:Label ID="lblVoucherLineBreak1" runat="server" Text="<br />" Visible="false"></asp:Label>
							<asp:Label ID="lblVoucherLineBreak2" runat="server" Text="<br />" Visible="false"></asp:Label>
							<asp:Label ID="lblTotalIncVat" runat="server" Text='<%# Eval("TotalInc") %>' Font-Size="small"></asp:Label>
						</FooterTemplate>
					</asp:TemplateField>
					<asp:TemplateField ItemStyle-HorizontalAlign="Right" ItemStyle-CssClass="td3" FooterStyle-CssClass="td4">
						<ItemTemplate>
							<asp:ImageButton ID="btnDeleteItem" runat="server" CssClass="rollover" CommandName="Delete" ImageUrl="/design/shop/images/remove-btn.jpg" OnLoad="btnDeleteItem_load" dbResource="imgRemove" />
						</ItemTemplate>
						<FooterStyle />
					</asp:TemplateField>
					<asp:TemplateField Visible="false">
						<ItemTemplate>
							<asp:Label ID="lblOldQty" runat="server" Text='<%# Eval("qty") %>'></asp:Label>
						</ItemTemplate>
						<FooterStyle />
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
			<br class="clear" />
			
			

			<!-- Hidden resources -->
			<asp:Label ID="headerItem" runat="server" dbResource="headerItem"></asp:Label>
			<asp:Label ID="headerQty" runat="server" dbResource="headerQty"></asp:Label>
			<asp:Label ID="headerPrice" runat="Server" dbResource="headerPrice"></asp:Label>
			<asp:Label ID="headerSubTotal" runat="server" dbResource="headerSubTotal"></asp:Label>
			<asp:Label ID="headerVat" runat="server" dbResource="headerVat"></asp:Label>
			<asp:Label ID="headerTotal" runat="server" dbResource="headerTotal"></asp:Label>
		
	<script type="text/JavaScript">
	<!--
	function MM_preloadImages() { //v3.0
	  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
		var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
		if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
	}

	function MM_findObj(n, d) { //v4.01
	  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
		d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
	  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
	  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
	  if(!x && d.getElementById) x=d.getElementById(n); return x;
	}

	function MM_swapImgRestore() { //v3.0
	  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
	}

	function MM_swapImage() { //v3.0
	  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
	   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
	}
	//-->
	</script>

</asp:Content>