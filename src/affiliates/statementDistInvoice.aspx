<%@ Page Language="VB" AutoEventWireup="false" CodeFile="statementDistInvoice.aspx.vb" Inherits="affiliates_statementDistInvoice" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="imgLetterHead" runat="server" ImageUrl="~/images/emotional bliss banner.jpg" /><br />
        <br /><br /><br /><br /><br />
        <asp:Label ID="lblError" runat="server"></asp:Label>
        <table border="0">
            <tr>
                <td width="80">&nbsp;</td>
                <td>
                    <asp:FormView ID="fvAffiliate" runat="server" DataSourceID="sqlAffiliate" GridLines="none" OnDataBound="fvAffiliate_dataBound" Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="lblCompany" Font-Size="medium" runat="server" Text='<%# Eval("affCompany") %>'></asp:Label><br />
                            <asp:Label ID="lblAdd1" runat="server" Font-Size="medium" Text='<%# Eval("affAdd1") %>'></asp:Label>
                            <asp:Label ID="lblAdd2" runat="server" Font-Size="medium" Text='<%# Eval("affAdd2") %>'></asp:Label>
                            <asp:Label ID="lblAdd3" runat="server" Font-Size="medium" Text='<%# Eval("affAdd3") %>'></asp:Label>
                            <asp:Label ID="lblAdd4" runat="server" Font-Size="medium" Text='<%# Eval("affAdd4") %>'></asp:Label>
                            <asp:Label ID="lblAdd5" runat="server" Font-Size="medium" Text='<%# Eval("affAdd5") %>'></asp:Label>
                            <asp:Label ID="lblPostcode" runat="server" Font-Size="medium" Text='<%# uCase(Eval("affPostcode")) %>'></asp:Label><br />
                            <asp:Label ID="lblCountry" runat="server" Font-Size="medium" Text='<%# Eval("countryName") %>'></asp:Label><br />
                            <asp:Label ID="lblAffID" runat="server" Text='<%# Eval("affID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblCountryCode" runat="server" Text='<%# Eval("orderCountryCode") %>' Visible="false"></asp:Label>
                            <br /><br />
                            <hr class="blueline" />
                            <br /><br />
                            <asp:Label id="lblOrderIDText" runat="server" Font-Size="medium" Text="Pear Tree Order No: "></asp:Label>
                            <asp:Label ID="lblOrderID" runat="server" Font-Size="medium" Text='<%# showOrderCode(Eval("newOrderID"),Eval("orderCountryCode")) %>'></asp:Label><br />
                            <asp:Label id="lblPurchaseOrderText" runat="server" Font-Size="medium" Text="Order Reference: "></asp:Label>
                            <asp:Label ID="lblPurchaseOrder" runat="server" Font-Size="medium" Text='<%# Eval("purchaseOrder") %>'></asp:Label><br />
                            <asp:Label id="lblVatNoText" runat="server" Font-Size="medium" Text="VAT No: "></asp:Label>
                            <asp:Label ID="lblVATNo" runat="server" Font-Size="medium"></asp:Label><br />
                            <asp:Label id="lblOrderDateText" runat="server" Font-Size="medium" Text="Order Date: "></asp:Label>
                            <asp:Label id="lblOrderDate" runat="server" Font-Size="medium" Text='<%# Eval("orderDate","{0:dd MMM yyyy}") %>'></asp:Label><br />
                            <asp:Label id="lblTrackerDateText" runat="server" Font-Size="medium" Text="Despatch Date: "></asp:Label>
                            <asp:Label ID="lblTrackerDate" runat="server" Font-Size="medium" Text='<%# Eval("trackerDate","{0:dd MMM yyyy}") %>'></asp:Label>
                            <asp:HiddenField ID="hidAffiliateID" runat="Server" Value='<%# Eval("affiliateID") %>' />
                        </ItemTemplate>
                    </asp:FormView>
                    <br /><br />
                    <table border="0" width="701" cellpadding="0">
                        <tr>
                            <td>
                                <asp:GridView ID="gvOrderItems" runat="server" GridLines="none" DataKeyNames="orderItemID" DataSourceID="sqlOrderItems" AutoGenerateColumns="false" Width="100%">
                                    <EditRowStyle HorizontalAlign="right" />
                                    <HeaderStyle Font-Size="medium" />
                                    <RowStyle Font-Size="medium" />
                                    <Columns>
                                        <asp:BoundField HeaderText="Product" DataField="affProductName" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" ReadOnly="true" />
                                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                        <asp:BoundField HeaderText="Qty" DataField="qty" ControlStyle-Width="40" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="UnitPrice" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="right" ItemStyle-Width="80">
                                            <ItemTemplate>
                                                <nobr>
                                                <asp:Label ID="lblCurrencySign" runat="server" Text='<%# trim(getCurrencySign(Eval("orderCurrency"))) %>'></asp:Label><asp:Label id="lblUnitPrice" runat="server" Text='<%# trim(Eval("price","{0:n2}")) %>'></asp:Label>
                                                </nobr>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrencySign2" runat="server" Text='<%# getCurrencySign(Eval("orderCurrency")) %>'></asp:Label><asp:label id="lblUnitPrice" runat="server" Text='<%# Eval("rowtotal","{0:n2}") %>'></asp:label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>    
                            </td>
                        </tr>
                    </table>
                    
                    <br />
                    <asp:FormView ID="fvTotals" runat="server" DataSourceID="SqlAffiliate">
                        <ItemTemplate>
                            <table width="700">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblGoodsExcText" runat="server" runat="server" Font-Size="medium">
                                            Subtotal
                                        </asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblGoodsExc" runat="server" Font-Size="medium" Text='<%# getCurrencySign(Eval("orderCurrency")) & Eval("goods") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblShippingExText" runat="server" runat="server" Font-Size="medium">                                        
                                            Shipping
                                        </asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblShippingExc" runat="server" Font-Size="medium" Text='<%# getCurrencySign(Eval("orderCurrency")) & Eval("shipping") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblVatText" runat="server" runat="server" Font-Size="medium">
                                            VAT
                                        </asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lblVat" runat="server" Font-Size="medium" Text='<%# getCurrencySign(Eval("orderCurrency")) & calcVat(Eval("goodsVat"),Eval("shipping"),Eval("shippingTotal")) %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td width="120" align="right">
                                        <asp:Label ID="lblTotalText" runat="server" runat="server" Font-Size="medium">
                                            Total
                                        </asp:Label>
                                    </td>
                                    <td align="right" width="120">
                                        <asp:Label ID="lblTotal" runat="server" Font-Size="medium" Text='<%# getCurrencySign(Eval("orderCurrency")) & formatNumber(Eval("orderTotal"),2) %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView> 
                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <br /><br />
                    <center>
                    Pear Tree UK Ltd<br />
                    Stonecroft<br />
                    17 Garnet Lane<br />
                    Tadcaster<br />
                    LS24 9LD<br /><br />
                    Registered in England No. 04080067&nbsp;&nbsp;&nbsp;&nbsp; VAT No. 764 4520 26
                    </center>
                    
                </td>
            </tr>
        </table>
    </div>
    </form>
    
    <asp:SqlDataSource ID="sqlAffiliate" runat="server" SelectCommand="procAffiliateByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOrderItems" runat="server" SelectCommand="procShopOrderItemByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</body>
</html>

