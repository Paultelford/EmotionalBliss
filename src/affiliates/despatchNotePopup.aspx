<%@ Page Language="VB" AutoEventWireup="false" CodeFile="despatchNotePopup.aspx.vb" Inherits="affiliates_despatchNotePopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <style type="text/css">
        #despatch td{
            font-size:11px;  
        }
        #add td{
            font-size:11px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <!--<center><input type='button' value='Print' id='printBtn' onclick='printScr()' /></center>-->
    <div id="despatch" style="margin: 0 0 0 10px;">
        <font size="4"><br /></font>        
                    <table border="0" width="750">
                        <tr>
                            <td valign="top" width="300">
                                <table border="0" cellpadding="0" cellspacing="0"><tr height="25"><td></td></tr></table>
                                <asp:GridView ID="gvItems" runat="server" GridLines="none" ShowHeader="false" AutoGenerateColumns="false" DataSourceID="sqlItems" Width="450" OnDataBound="gvItems_dataBound">
                                    <RowStyle Height="22" />
                                    <Columns>
                                        <asp:BoundField DataField="qty" ItemStyle-Width="10"/>
                                        <asp:BoundField ItemStyle-Width="20" />
                                        <asp:BoundField DataField="saleProdCode" ItemStyle-Width="40" />
                                        <asp:BoundField ItemStyle-Width="20" />
                                        <asp:BoundField DataField="affProductName" ItemStyle-Width="120" />
                                        <asp:BoundField ItemStyle-Width="20" />
                                        <asp:TemplateField ItemStyle-Width="50">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPrice" runat="server" Text='<%# showCurrency & Eval("price") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                     
                                        <asp:BoundField ItemStyle-Width="20" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                            <td width="130">&nbsp;</td>
                            <td valign="top">
                                <asp:DetailsView id="dvOrder" runat="server" DataSourceID="sqlOrder" GridLines="none" AutoGenerateRows="false" OnDataBound="dvOrder_dataBound">
                                    <RowStyle Height="17" />
                                    <Fields>
                                        <asp:BoundField DataField="userOrderID" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Literal ID="litDate" runat="server" Text='<%# formatDateTime(Eval("orderDate"), DateFormat.LongDate) %>'></asp:Literal>
                                                <asp:Label ID="lblOrderType" runat="server" Text='<%# Eval("orderType") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>                            
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                1 of 1
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                1kg
                                            </ItemTemplate>
                                        </asp:TemplateField>                           
                                        <asp:BoundField DataField="totalQty" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                UPS
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                    </Fields>
                                </asp:DetailsView>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblSpacer" runat="Server" Text="<br /><br /><br /><br /><br />"></asp:Label>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="200">
                                            Additional delivery instructions:        
                                        </td>
                                        <td align="left">
                                            <asp:GridView ID="gvDelivery" runat="server" DataSourceID="sqlOrder" AutoGenerateColumns="false" ShowHeader="false" GridLines="none">
                                                <Columns>
                                                    <asp:BoundField DataField="delivery" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>                                
                                <br />
                            </td>
                        </tr>


                    </table>                    
                    <table border="0" id="add">
                        <tr>
                            <td width="30"></td>
                            <td width="220" valign="top">
                                <!-- Billing Address -->
                                <asp:DetailsView ID="dvBill" runat="server" DataSourceID="sqlBilling" AutoGenerateRows="false" GridLines="none" OnDataBound="dv_dataBound">
                                    <Fields>
                                        <asp:BoundField DataField="billName" NullDisplayText="@@" />
                                        <asp:BoundField DataField="billAdd1" NullDisplayText="@@" />
                                        <asp:BoundField DataField="billAdd2" NullDisplayText="@@" />
                                        <asp:BoundField DataField="billAdd3" NullDisplayText="@@" />
                                        <asp:BoundField DataField="billAdd4" NullDisplayText="@@" />
                                        <asp:BoundField DataField="billAdd5" NullDisplayText="@@" />
                                        <asp:BoundField DataField="billPostcode" NullDisplayText="@@" />
                                        <asp:BoundField DataField="billCountry" NullDisplayText="@@" />
                                        <asp:BoundField DataField="fiscal" NullDisplayText="@@" />
                                    </Fields>
                                </asp:DetailsView>
                            </td>
                            <td width="10">&nbsp;</td>
                            <td width="270" valign="top">
                                <!-- Costs -->
                                <asp:DetailsView ID="dvCosts" runat="server" DataSourceID="sqlCosts" AutoGenerateRows="false" GridLines="none">
                                    <RowStyle Height="18" />
                                    <Fields>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Literal ID="litGoods" runat="server" Text='<%# showCurrency & FormatNumber(Eval("goods"),2) %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Literal ID="litShipping" runat="server" Text='<%# showCurrency & FormatNumber(Eval("shipping"),2) %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Literal ID="litVat" runat="server" Text='<%# showCurrency & FormatNumber(Eval("vatTotal"),2) %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Literal ID="lblTotal" runat="server" Text='<%# showCurrency & Eval("orderTotal") %>'></asp:Literal>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Fields>
                                </asp:DetailsView>
                            </td>
                            <td width="240" valign="top">
                                <!-- Shipping Address -->
                                <asp:DetailsView ID="dvShipping" runat="server" DataSourceID="sqlShipping" AutoGenerateRows="false" GridLines="none" OnDataBound="dv_dataBound">
                                    <Fields>
                                        <asp:BoundField DataField="shipName" NullDisplayText="@@" />
                                        <asp:BoundField DataField="shipAdd1" NullDisplayText="@@" />
                                        <asp:BoundField DataField="shipAdd2" NullDisplayText="@@" />
                                        <asp:BoundField DataField="shipAdd3" NullDisplayText="@@" />
                                        <asp:BoundField DataField="shipAdd4" NullDisplayText="@@" />
                                        <asp:BoundField DataField="shipAdd5" NullDisplayText="@@" />
                                        <asp:BoundField DataField="shipPostcode" NullDisplayText="@@" />
                                        <asp:BoundField DataField="shipCountry" NullDisplayText="@@" />
                                        <asp:BoundField DataField="phone" NullDisplayText="@@" />
                                        <asp:BoundField DataField="email" NullDisplayText="@@" />
                                    </Fields>
                                </asp:DetailsView>
                            </td>
                        </tr>
                    </table>
              
        
    </div> 
    
    <asp:SqlDataSource ID="sqlItems" runat="server" SelectCommand="procShopOrderItemsByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderid" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
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
    <asp:SqlDataSource ID="sqlCosts" runat="server" SelectCommand="procShopOrderByIDCostsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="ID" type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOrder" runat="server" SelectCommand="procShopOrderDespatchDetailsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <script language="javascript" type="text/javascript">
    function printScr()
    {
        document.getElementById("printBtn").style.visibility='hidden';
        window.print();
    }
    </script>
    </form>
</body>
</html>

