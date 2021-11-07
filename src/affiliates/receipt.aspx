<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="receipt.aspx.vb" Inherits="shop_receipt" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblReceipt" runat="server" CssClass="lightbluehead" Text="<%$ Resources:lblReceipt %>"></asp:Label>
            </td>
            <td align="right">
                <span style="color:Blue;cursor:pointer;visibility:hidden;" onclick="openPrintPop()">Print Receipt</span>
            </td>
        </tr>
    </table>    
    <br /><br />
    <span id="receiptBody" style="border: 10px 10px 10px 10px;">
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblOrderNo" runat="server" Text="<%$ Resources:lblOrderNo %>"></asp:Label>:
                </td>
                <td>
                     <asp:Label ID="lblPrefix" Font-Bold="true" runat="server"></asp:Label><asp:Label ID="lblDash" runat="server" Text="/" Font-Bold="true" Visible="false"></asp:Label><asp:Label ID="lblOrderID" runat="server" Font-Bold="true"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Purchase Order:
                </td>
                <td>
                    <asp:Label ID="lblPurchaseOrder" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
        <table width="100%">
            <tr>
                <td width="20%">
                    <div style='float: left;'>
                    <asp:Label ID="lblBillingText" runat="server" dbResource="lblBillingText" Font-Underline="true">Billing Address:</asp:Label>
                    <br />
                    <asp:label id="lblBillAddress" runat="server"></asp:label>
                    </div>
                </td>
                <td width="5%"></td>
                <td>
                    <div style='float: left;'>
                    <asp:Label ID="lblShippingText" runat="server" dbResource="lblShippingText" Font-Underline="true">Shipping Address:</asp:Label>
                    <br />
                    <asp:label id="lblShipAddress" runat="server"></asp:label>
                    </div>
                </td>
            </tr>
        </table>
        <asp:Label ID="lblPaymentResults" runat="server"></asp:Label>
        <br /><br />
        <asp:GridView ID="gvBasket" runat="server" Visible="true" GridLines="none" Width="100%" AutoGenerateColumns="false" OnLoad="gvBasket_Load" ShowFooter="true" OnDataBound="gvBasket_dataBound">
            <Columns>
                
                <asp:TemplateField HeaderText="Product" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                        <asp:Label ID="lblProductCode" runat="server" Text='<%# Eval("productCode") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Qty" DataField="qty" HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" ItemStyle-Width="25%" />
                <asp:TemplateField HeaderText="Unit Price" HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <asp:Label id="lblCurrencySign1" runat="server" Text='<%# getCurrencyCode() %>'></asp:Label>
                        <asp:Label ID="lblRowPrice" runat="server" Text='<%# Eval("price") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Row Price" HeaderStyle-HorizontalAlign="right" ItemStyle-Width="25%">
                    <ItemStyle HorizontalAlign="right" />
                    <ItemTemplate>
                        <asp:Label id="lblCurrencySign2" runat="server" Text='<%# getCurrencyCode() %>'></asp:Label>
                        <asp:Label ID="lblRowPrice2" runat="server" Text='<%# Eval("rowPriceEx") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterStyle HorizontalAlign="right" />
                    <FooterTemplate>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblShipping" runat="server" Font-Bold="false" Text="<%$ Resources:_cbLblShipping %>"></asp:Label><br />
                                    <asp:Label ID="lblCouponDiscountText" runat="server" Font-Bold="false" Visible="false" Text='<%$ Resources:lblCouponDiscountText %>'></asp:Label>
                                    <asp:Label ID="lblVat" runat="server" Font-Bold="false" Text="<%$ Resources:_cbLblVAT %>"></asp:Label><br />
                                    <asp:Label ID="lblVoucherDiscountText" runat="server" Font-Bold="false" Visible="false" Text='<%$ Resources:lblVoucherDiscountText %>'></asp:Label>
                                    <asp:Label ID="lblTotal" runat="server" Font-Bold="true" Text="<%$ Resources:_cbLblTotal %>"></asp:Label>
                                </td>
                                <td width="20"></td>
                                <td align="right">
                                    <asp:Label ID="lblShippingCostEx" runat="server"></asp:Label><br />
                                    <asp:Label ID="lblShippingCostInc" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lblCouponDiscount" runat="server"></asp:Label>
                                    <asp:Label ID="lblVatCost" runat="server"></asp:Label><br />
                                    <asp:Label ID="lblVoucherDiscount" runat="server"></asp:Label>
                                    <asp:Label ID="lblTotalCost" runat="server"></asp:Label>        
                                </td>
                            </tr>
                        </table>                        
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br /><br />
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
        <input type="button" value="Print Receipt" onclick="openPrintPop();this.style.visibility='hidden'" />
    </span>
    <script language="Javascript" type="text/javascript">
        var win;
        function openPrintPop()
        {
            win=window.open("receiptPrint.aspx","receiptPrintPop","toolbars=none");
        }
        function startDataTransfer()
        {
            return document.getElementById("receiptBody").innerHTML;
        }
    </script>
</asp:Content>


