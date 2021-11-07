<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="orderView.aspx.vb" Inherits="maintenance_orderView" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hidVatRate" runat="server" />
            <asp:FormView ID="fvOrder" runat="Server" DataSourceID="SqlOrder" OnDataBound="fvOrder_dataBound" Width="100%">
                <ItemTemplate>
                    <table border="0" width="100%">
                        <tr>
                            <td>
                                <b>OrderID:</b>
                            </td>
                            <td>
                                <asp:Label ID="lblOrderID" runat="Server" Text='<%# Eval("neworderID") %>'></asp:Label>
                            </td>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <b>Date:</b>
                            </td>
                            <td>
                                <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("orderDate")) %>'></asp:Label>
                            </td>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td>
                                <b>Affiliate:</b>
                            </td>
                            <td>
                                <asp:Label ID="lblAffiliate" runat="server" Text='<%# Eval("affCompany") %>'></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td>
                                <b>Status</b>
                            </td>
                            <td>
                                <asp:Label ID="lblStatus" runat="Server" Text='<%# Eval("distributorStatus") %>'></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <b>Bill To:</b>
                            </td>
                            <td>
                                <asp:DetailsView ID="dvBillAdd" runat="Server" DataSourceID="SqlOrder" AutoGenerateRows="false" GridLines="none">
                                    <Fields>
                                        <asp:BoundField DataField="billName" NullDisplayText="*" />
                                        <asp:BoundField DataField="billAdd1" NullDisplayText="*" />
                                        <asp:BoundField DataField="billAdd2" NullDisplayText="*" />
                                        <asp:BoundField DataField="billAdd3" NullDisplayText="*" />
                                        <asp:BoundField DataField="billAdd4" NullDisplayText="*" />
                                        <asp:BoundField DataField="billAdd5" NullDisplayText="*" />
                                        <asp:BoundField DataField="billPostcode" NullDisplayText="*" />
                                        <asp:BoundField DataField="billCountry" NullDisplayText="*" />
                                    </Fields>
                                </asp:DetailsView>                        
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <b>Ship To:</b>
                            </td>
                            <td>
                                <asp:DetailsView ID="dvShipAdd" runat="Server" DataSourceID="SqlOrder" AutoGenerateRows="false" GridLines="none">
                                    <Fields>
                                        <asp:BoundField DataField="shipName" NullDisplayText="*" />
                                        <asp:BoundField DataField="shipAdd1" NullDisplayText="*" />
                                        <asp:BoundField DataField="shipAdd2" NullDisplayText="*" />
                                        <asp:BoundField DataField="shipAdd3" NullDisplayText="*" />
                                        <asp:BoundField DataField="shipAdd4" NullDisplayText="*" />
                                        <asp:BoundField DataField="shipAdd5" NullDisplayText="*" />
                                        <asp:BoundField DataField="shipPostcode" NullDisplayText="*" />
                                        <asp:BoundField DataField="shipCountry" NullDisplayText="*" />
                                    </Fields>
                                </asp:DetailsView> 
                            </td>
                        </tr>
                        <tr id="trCard" runat="server">
                            <td colspan="5">
                                <table>
                                    <tr>
                                        <td colspan="2">
                                            <b>Card Details :-</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Card Name:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCardName" runat="server" Text='<%# Eval("billName") %>'></asp:Label>
                                        </td>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Card Type:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCardType" runat="server" Text='<%# Eval("cardType") %>'></asp:Label>
                                        </td>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Card Number:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCardNo" runat="server" Text='<%# Eval("cardNo") %>'></asp:Label>
                                        </td>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Valid From:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblValidFrom" runat="server" Text='<%# Eval("cardStart") %>'></asp:Label>
                                        </td>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Exp:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblExp" runat="server" Text='<%# Eval("cardExp") %>'></asp:Label>
                                        </td>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Issue:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblIssue" runat="server" Text='<%# Eval("cardIssue") %>'></asp:Label>
                                        </td>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            CV2:
                                        </td>
                                        <td>
                                            <asp:Label ID="lblCV2" runat="server" Text='<%# Eval("cardCv2") %>'></asp:Label>
                                        </td>
                                        <td colspan="3">&nbsp;</td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        
                        <tr>
                            <td colspan="5">&nbsp;</td>
                        </tr>
                        <tr id="trAccount" runat="server">
                            <td>
                                <b>Account Order:</b>
                            </td>
                            <td>
                                <asp:label ID="lblAccount" runat="server" Text='<%# Eval("purchaseOrder") %>'></asp:label>
                            </td>
                            <td colspan="3">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:GridView ID="gvBasket" runat="server" Width="100%" BorderWidth="0" DataKeyNames="orderItemID,vatRate" DataSourceID="SqlBasket" AutoGenerateColumns="false" GridLines="none" OnRowDataBound="gvBasket_rowDataBound" ShowFooter="true" OnRowUpdated="gvBasket_rowUpdated" OnDataBound="gvBasket_dataBound">
                                <HeaderStyle Font-Bold="true" />
                                <Columns>
                                    <asp:BoundField HeaderText="Product" DataField="affProductname" ReadOnly="true" />
                                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Code" DataField="warehouseProductCode" ReadOnly="true" />
                                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Qty" DataField="qty" ReadOnly="false" />
                                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Outstanding">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOutstanding" runat="server" ForeColor="black" Text='<%# showOutstanding(Eval("qty"),Eval("qtyDespatched")) %>'></asp:Label>
                                            <asp:Label ID="lblItemVatRate" runat="server" Text='<%# Eval("vatRate") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Stock" DataField="stock" ReadOnly="true" />
                                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Unit Price" HeaderStyle-HorizontalAlign="right" ItemStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("price","{0:n2}") %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtUnitPrice" runat="server" Text='<%# Bind("price","{0:n2}") %>' Width="60"></asp:TextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="DespatchQty" Visible="false">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtDespatch" runat="server" Width="40"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ItemStyle-HorizontalAlign="right" EditText="Edit" ShowEditButton="true" ShowCancelButton="false" />
                                </Columns>
                                </asp:GridView>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td width="91%">
                                            <asp:DetailsView ID="dvCosts" runat="server" DataSourceID="sqlCosts" AutoGenerateRows="false" GridLines="none" Width="100%">
                                            <RowStyle HorizontalAlign="right" />
                                            <HeaderStyle HorizontalAlign="left" />
                                            <Fields>
                                                <asp:BoundField HeaderText="Goods Subtotal" DataField="goods" DataFormatString="{0:n2}" ReadOnly="true" />
                                                <asp:BoundField HeaderText="Goods Vat" DataField="goodsVat" DataFormatString="{0:n2}" ReadOnly="true" />
                                                <asp:TemplateField HeaderText="Shipping">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblShipping" runat="server" Text='<%# Eval("shipping","{0:n2}") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtShipping" runat="server" Text='<%# Eval("shipping","{0:n2}") %>' Width="60" AutoPostBack="false" OnTextChanged="txtShipping_textChanged"></asp:TextBox>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="Shipping Vat" DataField="shippingVat" DataFormatString="{0:n2}" ReadOnly="true" />
                                                <asp:BoundField HeaderText="Total" DataField="orderTotal" DataFormatString="{0:n2}" ReadOnly="true" />
                                            </Fields>
                                        </asp:DetailsView>        
                                        </td>
                                        <td valign="top" align="right">
                                            <table border="0">
                                                <tr height="32"><td>&nbsp;</td></tr>
                                            </table>
                                            <asp:LinkButton ID="btnEditShipping" runat="server" Text="Edit" OnClick="btnEditShipping_click"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5" align="right">
                                <br />
                                <asp:Button ID="btnPaymentComplete" runat="server" Text="Set Payment Complete" OnClick="btnPaymentComplete_click" Visible="false" />
                            </td>
                        </tr>
                    </table>
                    <asp:Label ID="lblError" runat="server"></asp:Label>
                    <asp:HiddenField ID="hidUseBillAdd" runat="server" Value='<%# Eval("useBillAdd") %>' />
                </ItemTemplate>
            </asp:FormView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="SqlOrder" runat="Server" SelectCommand="procShopOrderByIDViewSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlBasket" runat="server" SelectCommand="procShopOrderItemsByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procShopOrderItemByItemIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="orderItemID" Type="int32" />
            <asp:Parameter Name="vatRate" Type="decimal" />
            <asp:Parameter Name="qty" Type="int32" />
            <asp:Parameter Name="price" Type="decimal" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCosts" runat="server" SelectCommand="procShopOrderByIDCostsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="ID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

