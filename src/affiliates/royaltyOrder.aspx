<%@ Page Language="VB" AutoEventWireup="false" CodeFile="royaltyOrder.aspx.vb" Inherits="affiliates_royaltyOrder" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Royalty Earnings</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Image ID="imgLetterHead" runat="server" ImageUrl="~/images/emotional bliss banner.jpg" /><br />
        <br /><br /><br /><br /><br />
        <table width="80%">
            <tr>
                <td width="80">&nbsp;</td>
                <td align="left">
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
                            <asp:Label id="lblOrderIDText" runat="server" Font-Size="medium" Text="Emotional Bliss Order No: "></asp:Label>
                            <asp:Label ID="lblOrderID" runat="server" Font-Size="medium" Text='<%# Eval("userOrderID") %>'></asp:Label><br />
                            <asp:Label id="lblOrderDateText" runat="server" Font-Size="medium" Text="Order Date: "></asp:Label>
                            <asp:Label id="lblOrderDate" runat="server" Font-Size="medium" Text='<%# Eval("orderDate","{0:dd MMM yyyy}") %>'></asp:Label><br />
                            <asp:Label ID="lblCompleteDateText" runat="server" Font-Size="medium" Text="Complete Date: "></asp:Label>
                            <asp:Label id="lblCompleteDate" runat="server" Font-Size="medium" Text='<%# Eval("transDate","{0:dd MMM yyyy}") %>'></asp:Label><br />
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <br /><br />
                    <asp:GridView ID="gvItems" runat="server" DataSourceID="sqlItems" GridLines="none" AutoGenerateColumns="false" ShowFooter="true" OnDataBound="gvItems_dataBound">
                        <Columns>
                            <asp:TemplateField HeaderText="Product">
                                <ItemTemplate>
                                    <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("saleName") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalText" runat="Server" Text="Total" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="40" />
                            <asp:BoundField HeaderText="Qty" DataField="qty" />
                            <asp:BoundField ItemStyle-Width="40" />
                            <asp:BoundField HeaderText="Unit Price" DataField="saleUnitPrice" DataFormatString="{0:n2}" />
                            <asp:BoundField ItemStyle-Width="40" />
                            <asp:BoundField HeaderText="Royalty %" DataField="amount" />
                            <asp:BoundField ItemStyle-Width="40" />
                            <asp:TemplateField HeaderText="Earning" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right">
                                <ItemTemplate>
                                    <asp:Label ID="lblRoyalty" runat="server" Text='<%# Session("EBAffCurrencySign") & Eval("credit","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotal" runat="Server" Font-Bold="true"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="40" />
                        </Columns>
                    </asp:GridView>
                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <br /><br />
                    <center>
                    Emotional Bliss<br />
                    22 Carlisle St<br />
                    Goole<br />
                    DN14 5DS<br /><br />
                    Emotional Bliss is a trading name of Pear Tree UK Ltd<br /> Registered in England No. 04080067&nbsp;&nbsp;&nbsp;&nbsp; VAT No. 764 4520 26
                </td>
            </tr>
        </table>
        
        
        <asp:SqlDataSource ID="sqlAffiliate" runat="server" SelectCommand="procAffiliateRoyaltyByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="int32" />
                <asp:QueryStringParameter QueryStringField="aid" Name="affID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlItems" runat="server" SelectCommand="procShopOrderItemByOrderIDAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="int32" />
                <asp:QueryStringParameter QueryStringField="aid" Name="affID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
