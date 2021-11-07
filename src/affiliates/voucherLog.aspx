<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="voucherLog.aspx.vb" Inherits="affiliates_voucherLog" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy id="smp" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <br />
            View voucher type:&nbsp;
            <asp:DropDownList id="drpType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpType_selectedIndexChanged">
                <asp:listitem text="Single use voucher" value="0"></asp:listitem>
                <asp:listitem text="Multiple use coupon" value="1"></asp:listitem>
            </asp:DropDownList>
            <br /><br />
            <asp:Panel id="panSingleUse" runat="server">
                Gray background shows voucher was created by Distributor<br />
                <asp:GridView ID="gvLog" runat="server" DataSourceID="sqlVoucherOrders" AutoGenerateColumns="false" SkinID="GridView" OnRowDataBound="gvLog_rowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="Order Date" DataField="purchaseDate" HtmlEncode="false" DataFormatString="{0:dd MMMM yyyy}" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:HyperLinkField HeaderText="Order ID" DataTextField="userOrderID" DataNavigateUrlFields="id" DataNavigateUrlFormatString="~/affiliates/orderView.aspx?id={0}" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Qty" DataField="qty" NullDisplayText="1" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Price" DataField="price" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField HeaderText="Order Status">
                            <ItemTemplate>
                                <asp:Label id="lblStatus" runat="server" Text='<%# formatCapString(Eval("orderStatus"))%>'></asp:Label>
                                <asp:Label ID="lblPS" runat="server" Visible="false" Text='<%# Eval("productOnSaleID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Voucher Used?">
                            <ItemTemplate>
                                <asp:label ID="lblVoucherUsed" runat="server" Text='<%# isOrderUsed(Eval("userUsedOrderID"),Eval("usedOrderID")) %>'></asp:label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Number" DataField="number" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            Gray background shows coupon has expired or is inactive
            <asp:Panel id="panMultiUse" runat="server" visible="false">
                <asp:GridView ID="gvMultiLog" runat="server" DataSourceID="sqlCoupons" DataKeyNames="number" AutoGenerateColumns="false" SkinID="GridViewRedBG" EmptyDataText="No orders found for the selected voucher.">
                    <RowStyle VerticalAlign="top" />
                    <Columns>
                        <asp:BoundField HeaderText="Created" DataField="purchaseDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Expires" DataField="endDate" DataFormatString="{0:dd MMM yyyy}" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="Voucher No">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkNumber" runat="server" CommandName="select" Text='<%# Eval("number") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Details" DataField="comment" />
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnActive" runat="server" Text='<%# Eval("active") %>' OnClick="btnActive_click" CommandArgument='<%# Eval("voucherID") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Amount" DataField="credit" />
                        <asp:BoundField HeaderText="Affiliate" DataField="affCompany" NullDisplayText="None" />
                    </Columns>
                </asp:GridView>
                <br /><br />
                <asp:Label ID="lblSelectedVoucher" runat="server"></asp:Label><br />
                <asp:GridView ID="gvCoupon" runat="server" DataSourceID="sqlCouponOrders" AutoGenerateColumns="false" SkinID="GridView">
                    <Columns>
                        <asp:HyperLinkField HeaderText="Order" DataNavigateUrlFields="id" DataTextField="userOrderID" DataNavigateUrlFormatString="orderView.aspx?id={0}" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Order Date" DataField="orderDate" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Order Total" DataField="orderTotal" DataFormatString="{0:n2}" />                        
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Label id="lblError" runat="server"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>               
    

    <asp:SqlDataSource ID="sqlVoucherOrders" runat="server" SelectCommand="procVouchersByDateCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCoupons" runat="server" SelectCommand="procVouchersCouponByDateCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCouponOrders" runat="server" SelectCommand="procShopOrderByVoucherNumberSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvMultiLog" PropertyName="selectedValue" Name="voucherNumber" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

