<%@ Page Language="VB" AutoEventWireup="false" CodeFile="despatchNotePopupnl.aspx.vb" Inherits="affiliates_despatchNotePopupnl" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        hr{color: Black;}
        body 
        {
            font-family:Verdana;
            font-size: 14px;
            color:#555555;
         }   
    </style>
</head>
<body style="padding-left: 80px; padding-top: 80px;">
    <form id="form1" runat="server">
    
    <div>
        <table width="800" cellpadding="0" border="0" cellspacing="0">
            <tr>
                <td width="60%" valign="top">
                    <table border="0" width="100%" height="132">
                        <tr>
                            <td valign="top">
                                <!-- logo -->
                                <img src="/images/logo2.jpg" />
                            </td>
                        </tr>
                    </table>
                    <asp:DetailsView ID="dvBill2" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0" DataSourceID="sqlBilling" AutoGenerateRows="false" GridLines="none" OnDataBound="dv_dataBound">
                        <Fields>
                            <asp:BoundField DataField="billName" NullDisplayText="@@" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdd1" runat="server" Text='<%# formatAddress(Eval("billadd1"),Eval("billadd2"),Eval("billadd3"),Eval("billadd4"),Eval("billadd5")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdd2" runat="server" Text='<%# formatPostcode(Eval("billPostcode"),Eval("billadd2"),Eval("billadd3"),Eval("billadd4"),Eval("billadd5")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 
                            <asp:BoundField DataField="billCountry" NullDisplayText="@@" />
                        </Fields>
                    </asp:DetailsView>
                </td>
                <td valign="top" width="40%" align="right">
                    <table>
                        <tr>
                            <td>
                                Emotional Bliss<br />
                                Overschieseweg 12-D<br />
                                3044EE Rotterdam<br /><br />                                
                                Postbus 28020<br />
                                3003KA Rotterdam<br /><br />
                                T&nbsp;&nbsp;&nbsp;010 340 0550<br />
                                E&nbsp;&nbsp;&nbsp;info@emotionalbliss.nl<br />
                                W&nbsp;&nbsp;www.emotionalbliss.nl
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br /><br /><br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <!-- OrderID & Date -->
                    <h2 style="color: Black;">FACTUUR</h2><br /><br />
                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td width="50%">
                                <b>Factuurnummer:</b> <asp:Label ID="lblOrderID" runat="server"></asp:Label>
                            </td>
                            <td width="50%">
                                <b>Factuurdatum:</b> <asp:Label ID="lblOrderDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                    </table>                  
                    <br /><br />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <!-- Addresses -->
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td valign="top" width="50%">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top"><b>Factuuradres:</b></td>
                                        <td>
                                            <br />
                                            <asp:DetailsView ID="dvBill" runat="server" BorderWidth="0" CellPadding="0" CellSpacing="0" DataSourceID="sqlBilling" AutoGenerateRows="false" GridLines="none" OnDataBound="dv_dataBound">
                                                <Fields>
                                                    <asp:BoundField DataField="billName" NullDisplayText="@@" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAdd1" runat="server" Text='<%# formatAddress(Eval("billadd1"),Eval("billadd2"),Eval("billadd3"),Eval("billadd4"),Eval("billadd5")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAdd2" runat="server" Text='<%# formatPostcode(Eval("billPostcode"),Eval("billadd2"),Eval("billadd3"),Eval("billadd4"),Eval("billadd5")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                    <asp:BoundField DataField="billCountry" NullDisplayText="@@" />
                                                </Fields>
                                            </asp:DetailsView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top" width="50%">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top"><b>Verzendadres:</b></td>
                                        <td>
                                            <br />
                                            <asp:DetailsView ID="dvShipping" runat="server" DataSourceID="sqlShipping" AutoGenerateRows="false" GridLines="none" OnDataBound="dv_dataBound">
                                                <Fields>
                                                    <asp:BoundField DataField="shipName" NullDisplayText="@@" />
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAdd1" runat="server" Text='<%# formatAddress(Eval("shipadd1"),Eval("shipadd2"),Eval("shipadd3"),Eval("shipadd4"),Eval("shipadd5")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblAdd2" runat="server" Text='<%# formatPostcode(Eval("shipPostcode"),Eval("shipadd2"),Eval("shipadd3"),Eval("shipadd4"),Eval("shipadd5")) %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField> 
                                                    <asp:BoundField DataField="shipCountry" NullDisplayText="@@" />
                                                    <asp:BoundField DataField="phone" NullDisplayText="@@" Visible="false" />
                                                    <asp:BoundField DataField="email" NullDisplayText="@@" Visible="false" />
                                                </Fields>
                                            </asp:DetailsView>
                                        </td>
                                    </tr>
                                </table>
                            </td>                
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <!-- Spacer -->
                    <table border="0" width="95%"><tr><td><hr /></td></tr></table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="gvItems" runat="server" Width="98%" AutoGenerateColumns="false" DataSourceID="sqlItems" GridLines="None">
                        <HeaderStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderText="Aantal" ItemStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label ID="lblQty" runat="server" Text='<%# Eval("qty") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Artikelen" ItemStyle-Width="72%" DataField="affProductName" />
                            
                            <asp:TemplateField HeaderText="Prijs" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblPrice" runat="server" Text='<%# showCurrency & FormatNumber(Eval("price"),2) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bedrag" ItemStyle-Width="10%" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <asp:Label ID="lblRowPrice" runat="server" Text='<%# showCurrency & FormatNumber(Eval("lineTotal"),2) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView><br /><br />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="right">
                    <!-- Totals -->
                    <asp:DetailsView ID="dvCosts" runat="server" DataSourceID="sqlCosts" AutoGenerateRows="false" GridLines="None" BorderColor="LightBlue" BorderWidth="1" Width="250">
                        <RowStyle Height="28" />
                        <Fields>
                            <asp:TemplateField HeaderText="Subtotaal ex. BTW" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150">
                                <ItemTemplate>  
                                    <asp:Literal ID="litGoods" runat="server" Text='<%# showCurrency & FormatNumber(Eval("goods"),2) %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Transport" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150">
                                <ItemTemplate >
                                    <asp:Literal ID="litShipping" runat="server" Text='<%# showCurrency & FormatNumber(Eval("shipping"),2) %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Korting" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Literal ID="litDiscount" runat="server" Text='<%# showCurrency & FormatNumber(Eval("discount"),2) %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="19% BTW" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="150">
                                <ItemTemplate>
                                    <asp:Literal ID="litVat" runat="server" Text='<%# showCurrency & FormatNumber(Eval("vatTotal"),2) %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-BackColor="lightblue" ItemStyle-HorizontalAlign="Right" HeaderStyle-BackColor="LightBlue" HeaderText="Totaal" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Font-Bold="true">
                                <ItemTemplate>
                                   <asp:Literal ID="lblTotal" runat="server" Text='<%# showCurrency & Eval("orderTotal") %>'></asp:Literal>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                    <br /><br /><br /><br /><br /><br /><br /><br />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center">
                    <i style="color: #33bbff; font-size: 14pt;">Bedankt voor uw bestelling en veel plezier met uw aankoop!</i><br />
                    <font style="font-size: 13px;">Emotional Bliss is een handelsnaam van Care Design B.V.</font>
                    <br /><br /><br /><br /><br />
                    <font style="font-size: 14px;">KVK 50730436 • BTW NL822897878B01 • ING BANK 6629814 • IBAN NL41INGB0006629814 • BIC INGBNL2A</font> 
                </td>
            </tr>
        </table>
    </div>
    <asp:SqlDataSource ID="sqlBilling" runat="server" SelectCommand="procShopCustomerBillAddByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlShipping" runat="server" SelectCommand="procShopCustomerShipAddByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlItems" runat="server" SelectCommand="procShopOrderItemByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCosts" runat="server" SelectCommand="procShopOrderByIDCostsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="ID" type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
