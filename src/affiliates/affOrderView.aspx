<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="affOrderView.aspx.vb" Inherits="affiliates_affOrderView" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Show:&nbsp;
            <asp:DropDownList ID="drpStatus" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="drpStatus_selectedIndexChanged">
                <asp:ListItem Text="All" Value="%"></asp:ListItem>
                <asp:ListItem Text="Placed" Value="placed"></asp:ListItem>
                <asp:ListItem Text="Paid" Value="paid" Enabled="false"></asp:ListItem>
                <asp:ListItem Text="Complete" Value="complete"></asp:ListItem>
                <asp:ListItem Text="Cancelled" Value="cancelled"></asp:ListItem>
            </asp:DropDownList>
            <table border="0" width="100%">
                <tr>
                    <td valign="top">
                        <asp:GridView ID="gvOrders" runat="server" DataSourceID="sqlOrders" AutoGenerateColumns="false" SkinID="GridViewredBG" DataKeyNames="id" EmptyDataText="No orders match this criteria.">
                            <Columns>
                                <asp:TemplateField HeaderText="OrderID">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkOrderID" runat="server" Text='<%# Eval("userOrderID") %>' CommandName="Select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Order Date" DataField="orderDate" DataFormatString="{0:dd MMM yyyy}" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Order Total" DataField="orderTotal" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Order Status" DataField="orderStatus" />
                            </Columns>
                        </asp:GridView>        
                    </td>
                    <td valign="top" align="right">
                        <asp:FormView ID="fvOrder" runat="server" DataSourceID="sqlOrder">
                            <ItemTemplate>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="right">
                                            <asp:GridView ID="gvItems" runat="server" DataSourceID="sqlItems" AutoGenerateColumns="false" GridLines="none">
                                                <Columns>
                                                    <asp:BoundField HeaderText="Product" DataField="affProductName" />
                                                    <asp:BoundField ItemStyle-Width="40" />
                                                    <asp:TemplateField HeaderText="Unit Price">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblUnitPrice" runat="Server" Text='<%# Session("EBAffCurrencySign") & Eval("price","{0:n2}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField ItemStyle-Width="40" />
                                                    <asp:BoundField HeaderText="Qty" DataField="qty" />
                                                    <asp:BoundField ItemStyle-Width="40" />
                                                    <asp:TemplateField HeaderText="Subtotal">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSubtotal" runat="server" Text='<%# Session("EBAffCurrencySign") & Eval("qty")*Eval("price") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>        
                                            <table>
                                                <tr>
                                                    <td>
                                                        Goods
                                                    </td>
                                                    <td width="100">
                                                        <asp:Label ID="lblGoods" runat="server" Text='<%# Session("EBAffCurrencySign") & Eval("goods","{0:n2}") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Shipping
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblShipping" runat="server" Text='<%# Session("EBAffCurrencySign") & Eval("shipping","{0:n2}") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        VAT
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblVat" runat="server" Text='<%# Session("EBAffCurrencySign") & Eval("vatTotal","{0:n2}") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Total
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblTotal" runat="server" Text='<%# Session("EBAffCurrencySign") & Eval("orderTotal","{0:n2}") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>                        
                            </ItemTemplate>
                        </asp:FormView>    
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    
    <asp:SqlDataSource ID="sqlOrders" runat="server" SelectCommand="procShopOrdesrByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffID" Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="drpStatus" Name="status" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOrder" runat="server" SelectCommand="procShopOrderByIDCostsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvOrders" PropertyName="selectedValue" Name="ID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlItems" runat="Server" SelectCommand="procShopOrderItemsByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvOrders" PropertyName="selectedValue" Name="orderID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

