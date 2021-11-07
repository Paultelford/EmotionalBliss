<%@ Page Language="VB" MasterPageFile="~/mshop.master" AutoEventWireup="false" CodeFile="receipt.aspx.vb" Inherits="shop_receipt" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
<!-- Use jQuery to do the Checkout rollover image -->
    <script type='text/javascript' src='/jquery.js'></script>
    <script type='text/javascript' src='/jquery.preload.js'></script>
    <script type='text/javascript' src='/jquery.rollover.js'></script>
    <script language="javascript" type="text/javascript">
        //Init rollover
        $(function() { Sys.Application.add_load(wireEvents); });
    </script>    
    <!-- Google Code for Sale Conversion Page -->
    <script type="text/javascript">
    <!--
        var google_conversion_id = 1038821070;
        var google_conversion_language = "en";
        var google_conversion_format = "3";
        var google_conversion_color = "ffffff";
        var google_conversion_label = "EdcrCPLctAEQzs2s7wM";
        var google_conversion_value = 0;
    //-->
    </script>
    <script type="text/javascript" src="https://www.googleadservices.com/pagead/conversion.js">
    </script>
    <noscript>
    <div style="display:inline;">
    <img height="1" width="1" style="border-style:none;" alt="" src="https://www.googleadservices.com/pagead/conversion/1038821070/?label=EdcrCPLctAEQzs2s7wM&amp;guid=ON&amp;script=0"/>
    </div>
    </noscript>
    <!-- Checkout Stage Indicator -->
    <div id="basketmenu">
    <asp:Label id="lblShoppingBasketText" runat="server"></asp:Label>
        <!--
        Shopping Basket<br />
        <img src="/design/shop/images/basket-menu.gif" alt="basket" width="209" height="50" /><br />
        Payment Method<br />
        <img src="/design/shop/images/basket-menu.gif" alt="payment" width="209" height="50" /><br />
        Delivery Address<br />
        <img src="/design/shop/images/basket-menu-activ_bottom.gif" alt="dispatch" width="209" height="55" /><br />
        <span class="title3">Confirmation</span>-->
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<dbResource="lblShoppingBasketText" />
    <asp:Label ID="lblYourReceiptText" runat="server" dbResource="lblYourReceiptText"><h3>Your Receipt</h3></asp:Label>
    <br />
    <div id="receiptBody">
        <div id="paymenttextreceipt">
            <p>    
            <table border="0" width="100%">
                <tr>
                    <td width="35%">
                        <asp:Label ID="lblOrderNo" runat="server" dbResource="lblOrderNo"></asp:Label> <asp:Label ID="lblOrderID" runat="server" Font-Bold="true"></asp:Label>        
                    </td>
                    <td align="left">
                        <asp:Label id="lblDateText" runat="server" dbResource="lblDateText"></asp:Label> <asp:Label ID="lblDate" runat="server" Font-Bold="true"></asp:Label>        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblBillingText" runat="server" dbResource="lblBillingText" Font-Underline="true"></asp:Label>
                        <br />
                        <asp:label id="lblBillAddress" runat="server"></asp:label>
                    </td>
                    <td>
                        <asp:Label ID="lblShippingText" runat="server" dbResource="lblShippingText" Font-Underline="true"></asp:Label>
                        <br />
                        <asp:label id="lblShipAddress" runat="server"></asp:label>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblPaymentResults" runat="server"></asp:Label>
            </p>
            <br />
        </div>
        <br class="clear" />
        <asp:GridView ID="gvBasket" runat="server" Visible="true" GridLines="none" Width="605" AutoGenerateColumns="false" OnLoad="gvBasket_Load" ShowFooter="true" OnDataBound="gvBasket_dataBound" CssClass="table">
            <Columns>               
                <asp:TemplateField HeaderText="Item" HeaderStyle-CssClass="th3" ItemStyle-Width="260">
                    <ItemTemplate>
                        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("productCode") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Qty" DataField="qty" HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" ItemStyle-Width="45" HeaderStyle-CssClass="th4a" />
                <asp:TemplateField HeaderText="Unit Price" HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" ItemStyle-Width="65" HeaderStyle-CssClass="th4a">
                    <ItemTemplate>
                        <asp:Label ID="lblRowPrice" runat="server" Text='<%# Profile.EBCart.CurrencySign & Eval("price") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="140" HeaderStyle-CssClass="th4">
                    <FooterTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                             <tr id="trCoupon" runat="server" visible="false">
                                <td class="td4a">
                                    <asp:Label ID="lblCouponDiscountText" runat="server" Font-Bold="false" Visible="false" dbResource="lblCouponDiscountText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="td5">
                                    <asp:Label ID="lblShipping" runat="server" Font-Bold="false" dbResource="lblShipping"></asp:Label>
                                </td>
                            </tr>                           
                            <tr>
                                <td class="td4a">
                                    <asp:Label ID="lblVat" runat="server" Font-Bold="false" dbResource="lblVAT"></asp:Label><br />
                                </td>
                            </tr>
                            <tr id="trVoucher" runat="server" visible="false">
                                <td class="td4a">
                                    <asp:Label ID="lblVoucherDiscountText" runat="server" Font-Bold="false" Visible="false" dbResource="lblVoucherDiscountText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="td5">
                                    <asp:Label ID="lblTotal" runat="server" Font-Bold="true" dbResource="lblTotal"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Row Price" HeaderStyle-HorizontalAlign="right" ItemStyle-Width="65" HeaderStyle-CssClass="th4">
                    <ItemStyle HorizontalAlign="right" />
                    <ItemTemplate>
                        <asp:Label ID="lblRowPrice2" runat="server" Text='<%# Profile.EBCart.CurrencySign & Eval("rowPriceEx") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="right" />
                    <FooterTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr id="trCouponAmount" runat="server" visible="false">
                                <td class="td4a" align="right">
                                    <asp:Label ID="lblCouponDiscount" runat="server"></asp:Label>
                                    <asp:Label ID="lblShippingCostInc" runat="server" Visible="false"></asp:Label>                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="td5" align="right">
                                    <asp:Label ID="lblShippingCostEx" runat="server"></asp:Label><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="td4a" align="right">    
                                    <asp:Label ID="lblVatCost" runat="server"></asp:Label><br />
                                </td>
                            </tr>
                            <tr id="trVoucherAmount" runat="server" visible="false">
                                <td class="td4a" align="right">    
                                    <asp:Label ID="lblVoucherDiscount" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="td5" align="right">    
                                    <asp:Label ID="lblTotalCost" runat="server"></asp:Label>        
                                </td>
                            </tr>
                        </table>                        
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <div id="productbuttonsbox">
            <span class="BlueOyster">
                <asp:Label ID="lblComplete" runat="server" dbResource="lblComplete"></asp:Label>
                <asp:Label ID="lblComplete1" runat="server" dbResource="lblComplete1"></asp:Label>
                <asp:Label ID="lblComplete2" runat="server" dbResource="lblComplete2"></asp:Label>
                <asp:Label ID="lblComplete3" runat="server" dbResource="lblComplete3"></asp:Label>
                <asp:Label ID="lblComplete4" runat="server" dbResource="lblComplete4"></asp:Label>
                <asp:Label ID="lblComplete5" runat="server" dbResource="lblComplete5"></asp:Label>
                <asp:Label ID="lblCompleteVoucher" runat="server" dbResource="lblCompleteVoucher"></asp:Label>
                <br />
                <asp:Label ID="lblVoucher" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblConsultancy" runat="server"></asp:Label>
                <br />
                <asp:Label ID="lblReceipt" runat="Server"></asp:Label>    
            
                <asp:ScriptManagerProxy ID="smp1" runat="server"></asp:ScriptManagerProxy>
                <asp:UpdatePanel ID="up1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="panSurvey" runat="server">
                            Where did you hear about us:&nbsp;
                            <asp:DropDownList ID="drpWhere" runat="server" DataSourceID="sqlMag" DataTextField="magName" DataValueField="magName" AutoPostBack="true" OnSelectedIndexChanged="drpWhere_selectedIndexChanged" OnDataBound="drpWhere_dataBound">
                            </asp:DropDownList>
                        </asp:Panel>
                        <asp:Label ID="lblThanks" runat="server">
                            
                        </asp:Label>
                        <asp:HiddenField ID="hidNewOrderID" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br /><br />
                
            </span>
            <!-- Hidden elements -->
            <asp:Label ID="lblProduct" runat="server" dbResource="lblProduct"></asp:Label>
            <asp:Label ID="lblQty" runat="server" dbResource="lblQty"></asp:Label>
            <asp:Label ID="lblUnitPrice" runat="server" dbResource="lblUnitPrice"></asp:Label>
            <asp:Label ID="lblRowPrice" runat="server" dbResource="lblRowPrice"></asp:Label>
        </div>
    </div>
    
    <div id="productbuttons2">
        <table border="0">
            <tr>
                <td width="140">
                    <div id="button"><asp:HyperLink ID="lnkBackHome" runat="server" dbResource="lnkBackHome" CssClass="sideNav" NavigateUrl="/newHomeIntro.aspx"></asp:HyperLink></div>
                </td>
                <td width="140">
                    <div id="button"><asp:LinkButton ID="lnkPrint" runat="server" dbResource="lnkPrint" CssClass="sideNav" OnClientClick="openPrintPop(1);"></asp:LinkButton></div>
                </td>
            </tr>
        </table>
        
        <asp:image ID="imgBackHome" runat="server" ImageUrl="/design/shop/images/back-home-btn.jpg" CssClass="rollover" Visible="false" dbResource="imgBackHome" />&nbsp;&nbsp;&nbsp;
        <asp:Image ID="imgPrint" runat="server" ImageUrl="/design/shop/images/print-receipt-btn.jpg" CssClass="rollover" dbResource="imgPrint" Visible="false" />
        
    </div>                
    
    
       
    <script language="Javascript" type="text/javascript">
        var win;
        function openPrintPop(oID)
        {
            win=window.open("receiptPrint.aspx","receiptPrintPop","toolbars=none");
        }
        function startDataTransfer()
        {
            return document.getElementById("receiptBody").innerHTML;
        }
    </script>
    
    <asp:SqlDataSource ID="sqlMag" runat="server" SelectCommand="procCallcentreMagazineSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>


