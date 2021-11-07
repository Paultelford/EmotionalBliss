<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="orderViewLite.aspx.vb" Inherits="affiliates_orderViewLite" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:DetailsView ID="dvCustomer" runat="server" DataSourceID="SqlCustomer" AutoGenerateRows="false" GridLines="None" OnDataBound="dvCustomer_dataBound">
        <FieldHeaderStyle Font-Bold="true" Width="160" />
        <Fields>
            <asp:BoundField HeaderText="Customer:" DataField="billName" />
            <asp:BoundField HeaderText="Address:" DataField="billAdd1" />
            <asp:BoundField HeaderText="" DataField="billAdd2" />
            <asp:BoundField HeaderText="" DataField="billAdd3" />
            <asp:BoundField HeaderText="" DataField="billAdd4" />
            <asp:BoundField HeaderText="" DataField="billAdd5" />
            <asp:BoundField HeaderText="Postcode:" DataField="billPostcode" />
            <asp:BoundField HeaderText="Country:" DataField="billCountry" />
            <asp:BoundField HeaderText="Email:" DataField="email" />
            <asp:BoundField HeaderText="Order Type:" DataField="orderType" />
        </Fields>
    </asp:DetailsView>
    <br /><br />
    <asp:GridView ID="gvOrderItems" runat="server" GridLines="none" DataKeyNames="orderItemID" DataSourceID="sqlOrderItems" AutoGenerateColumns="false" OnDataBound="gvItems_dataBound" ShowFooter="true">
        <EditRowStyle HorizontalAlign="right" />
        <Columns>
            <asp:BoundField HeaderText="Product" DataField="affProductName" ItemStyle-HorizontalAlign="left" ReadOnly="true" FooterText="Total" FooterStyle-Font-Bold="true" />
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:BoundField HeaderText="Qty" DataField="qty" ControlStyle-Width="40" />
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:BoundField HeaderText="Vat Rate" DataField="vatRate" ControlStyle-Width="60" />
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:TemplateField HeaderText="Unit Price" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblCurrencySign" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblPrice" runat="server" Text='<%# Eval("price","{0:n2}") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lblCurrencySign1" runat="server" Font-Bold="true"></asp:Label><asp:Label ID="lblPriceTotal" runat="server" Font-Bold="true"></asp:Label>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:TemplateField HeaderText="Royalty" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right">
                <ItemTemplate>
                    <asp:Label ID="lblCurrencySign2" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblRoyalty" runat="server" Text='<%# Eval("credit","{0:n2}") %>'></asp:Label>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Label ID="lblCurrencySign3" runat="server" Font-Bold="true"></asp:Label><asp:Label ID="lblRoyaltyTotal" runat="server" Font-Bold="true"></asp:Label>
                </FooterTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>     
    <asp:Label ID="lblErrorItems" runat="server"></asp:Label>
    <br /><br />
    
    <asp:SqlDataSource ID="SqlCustomer" runat="server" SelectCommand="procShopCustomerByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBTmpRedirect_orderID" Name="orderID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOrderItems" runat="server" SelectCommand="procShopOrderItemByOrderIDSelect2" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBTmpRedirect_orderID" Name="orderID" Type="int32" />
            <asp:SessionParameter SessionField="EBAffID" Name="affID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

