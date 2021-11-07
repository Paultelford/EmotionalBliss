<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/m_shop.master" CodeFile="basket.aspx.vb" Inherits="shop_basket"  %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBShopMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
     <!-- Checkout Stage Indicator -->
    <div id="basketmenu">
        <span class="title3">Shopping Basket</span><br />
        <img src="/design/shop/images/basket-menu-activ.gif" alt="basket" width="209" height="55" /><br />
        Payment Method<br />
        <img src="/design/shop/images/basket-menu.gif" alt="payment" width="209" height="50" /><br />
        Delivery Address<br />
        <img src="/design/shop/images/basket-menu.gif" alt="dispatch" width="209" height="50" /><br />
        Confirmation
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <asp:Label ID="lblHeader" runat="server" CssClass="lightbluehead" dbResource="lblShoppingBasket" Visible="false"></asp:Label>
    <img src="/design/shop/images/shop-basket-ttl.gif" alt="Shopping Basket" width="600" height="50" /><br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <span style='visibility:hidden;'>
            Total basket weight: <asp:Label ID="lblTotalWeight" runat="server"></asp:Label>
            </span>
            <asp:GridView ID="gvBasket" runat="server" DataKeyNames="id" CssClass="table" BorderColor="black" GridLines="none" EnableTheming="false" AutoGenerateColumns="false" SkinID="GridView" OnLoad="gvBasket_load" ShowFooter="true" OnRowDeleting="gvBasket_rowDeleting" Width="605" OnDataBound="gvBasket_dataBound" EmptyDataText="You have no products in your basket.">
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
                            <asp:ImageButton ID="btnDeleteItem" runat="server" CommandName="Delete" ImageUrl="/design/shop/images/remove-btn.jpg" />
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
			<div id="paymentbuttons">
			    <div id="productbuttons">
			        <a style="display: none;" href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image16','','/design/shop/images/continue-shop-btn2.jpg',1)"><img src="/design/shop/images/continue-shop-btn.jpg" alt="Continue Shopping" name="Image16" width="142" height="45" border="0" id="Image16" />
			        </a>
			        <asp:ImageButton ID="btnContinueShopping" runat="server" ImageUrl="/design/shop/images/continue-shop-btn.jpg" OnClick="btnContinueShopping_click"  />
			    </div>
			    <div id="updatepay">
			        <a style="display: none;" href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image18','','/design/shop/images/update-basket-btn2.jpg',1)"><img src="/design/shop/images/update-basket-btn.jpg" alt="Update Basket" name="Image18" width="120" height="45" border="0" id="Image18" />
			        </a>
			        <asp:ImageButton ID="btnUpdate" runat="server" onClick="btnUpdate_click" ImageUrl="/design/shop/images/update-basket-btn.jpg" />
			        &nbsp;&nbsp;
			        <a style="display: none;" href="payment1.html" target="_self" onmouseover="MM_swapImage('Image17','','/design/shop/images/proceed-btn2.jpg',1)" onmouseout="MM_swapImgRestore()"><img src="/design/shop/images/proceed-btn.jpg" alt="Proceed Payment" name="Image17" width="120" height="45" border="0" id="Image17" />
			        </a>
			        <asp:ImageButton ID="btnContinue" runat="server" ImageUrl="/design/shop/images/proceed-btn.jpg" OnClick="btnContinue_click" />
			    </div>
			</div>
			<br class="clear" />
			<table width="605" cellspacing="0" cellpadding="0" class="table" id="tblVoucher" runat="server">
                <tr>
                    <th colspan="3" valign="middle" class="thvoucher">&nbsp;&nbsp;&nbsp;Do you have a gift voucher or promotional claim code?&nbsp;
                    </th>  
                </tr>
                <tr>
                    <td class="tdvoucher">
                        &nbsp;&nbsp;&nbsp;Enter Voucher No: GB 
                    </td>
                    <td>
                        <asp:TextBox ID="txtVoucherNumber" runat="server" Width="200" Visible="true" MaxLength="8" AutoPostBack="false" ValidationGroup="voucher"></asp:TextBox>&nbsp;
                    </td>
                    <td width="200" class="tdvoucher">
                        <asp:ImageButton ID="btnVoucherSubmit" runat="server" ImageUrl="~/design/shop/images/submit-btn.jpg" Visible="true" ValidationGroup="voucher" OnClick="btnVoucherSubmit_click" />&nbsp;&nbsp;
                    </td>
                </tr>
            </table>
            <asp:RequiredFieldValidator ID="reqTxtVoucherNumber" runat="server" ControlToValidate="txtVoucherNumber" ValidationGroup="voucher" dbResource="errRequired" Display="Dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="regTxtVoucherNumber" runat="server" ControlToValidate="txtVoucherNumber" ValidationExpression="^[0-9]{8}" Display="dynamic" dbResource="errVocuherWrongLength" ValidationGroup="voucher"></asp:RegularExpressionValidator>    
            <asp:Label ID="lblVoucherError" runat="Server"></asp:Label>
            <asp:Label ID="lblError" runat="server"></asp:Label>

            <!-- Hidden resources -->
            <asp:Label ID="headerItem" runat="server" dbResource="headerItem"></asp:Label>
            <asp:Label ID="headerQty" runat="server" dbResource="headerQty"></asp:Label>
            <asp:Label ID="headerPrice" runat="Server" dbResource="headerPrice"></asp:Label>
            <asp:Label ID="headerSubTotal" runat="server" dbResource="headerSubTotal"></asp:Label>
            <asp:Label ID="headerVat" runat="server" dbResource="headerVat"></asp:Label>
            <asp:Label ID="headerTotal" runat="server" dbResource="headerTotal"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
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