<%@ Page Language="VB" AutoEventWireup="false" CodeFile="statementEarnings.aspx.vb" Inherits="affiliates_statementEarnings" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <center>
    <div>
        <asp:Image ID="imgLetterHead" runat="server" ImageUrl="~/images/emotional bliss banner.jpg" /><br />
        <br /><br /><br /><br /><br />
        <table>
            <tr>
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
                            <asp:Label ID="lblOrderID" runat="server" Font-Size="medium" Text='<%# showOrderCode(Eval("userOrderID"),Eval("orderCountryCode")) %>'></asp:Label><br />
                            <asp:Label id="lblPurchaseOrderText" runat="server" Font-Size="medium" Text="Order Reference: "></asp:Label><br />
                            <asp:Label id="lblOrderDateText" runat="server" Font-Size="medium" Text="Order Date: "></asp:Label>
                            <asp:Label id="lblOrderDate" runat="server" Font-Size="medium" Text='<%# Eval("orderDate","{0:dd MMM yyyy}") %>'></asp:Label><br />
                            <asp:Label id="lblVatNumberText" runat="server" Font-Size="medium" Text='Vat Number:'></asp:Label>
                            <asp:Label id="lblVatNumber" runat="server" Font-Size="medium" Text='<%# Eval("affVat") %>'></asp:Label><br />
                        </ItemTemplate>
                    </asp:FormView>
                    <br /><br />
                    <asp:GridView ID="gvItems" runat="server" DataSourceID="sqlItems" AutoGenerateColumns="false" GridLines="none" OnDataBound="gvItems_dataBound" ShowFooter="true">
                        <HeaderStyle HorizontalAlign="right" />
                        <RowStyle HorizontalAlign="right" VerticalAlign="top" />
                        <FooterStyle HorizontalAlign="right" Font-Bold="true" />
                        <Columns>
                            <asp:BoundField HeaderText="Product" HeaderStyle-HorizontalAlign="left" ItemStyle-HorizontalAlign="left" DataField="product" />
                            <asp:BoundField ItemStyle-Width="40" />
                            <asp:BoundField HeaderText="Unit Price" DataField="unitprice" NullDisplayText="0" />
                            <asp:BoundField ItemStyle-Width="40" />
                            <asp:BoundField HeaderText="Qty" DataField="qty" NullDisplayText="0" />
                            <asp:BoundField ItemStyle-Width="40" />
                            <asp:BoundField HeaderText="Percentage" NullDisplayText="0" DataField="Percentage" DataFormatString="{0:N1}%" FooterText="<br>Total" />
                            <asp:BoundField ItemStyle-Width="40" />
                            <asp:TemplateField HeaderText="Earnings">
                                <ItemStyle VerticalAlign="top" />
                                <FooterStyle VerticalAlign="top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblEarnings" runat="server" text='<%# showEarnings(Eval("amount")) %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <br />
                                    <asp:Label ID="lblEarningsTotal" runat="server"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
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
        <asp:Label ID="lblError" runat="server"></asp:Label>
    </div>
    </center>

    <asp:SqlDataSource ID="sqlAffiliate" runat="server" SelectCommand="procAffiliateClickThroughByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>    
    <asp:SqlDataSource ID="sqlItems" runat="server" SelectCommand="procAffiliateClickThroughLogByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="ID" Name="orderID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
