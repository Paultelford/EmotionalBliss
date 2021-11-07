<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="orderViewTmp.aspx.vb" Inherits="maintenance_orderViewTmp" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DetailsView ID="dvOrder" runat="server" DataSourceID="SqlOrder" AutoGenerateRows="false" GridLines="none">
        <HeaderStyle Font-Bold="true" />
        <Fields>
            <asp:BoundField HeaderText="Order ID:" DataField="orderid" />
            <asp:TemplateField HeaderText="Complete Date:&nbsp;&nbsp;&nbsp;&nbsp;">
                <ItemTemplate>
                    <asp:Label ID="lblDate" runat="server" text='<%# showDate(Eval("orderDate")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Country" DataField="country" />
        </Fields>
    </asp:DetailsView>
    <br /><br />
    <asp:GridView ID="gvItems" runat="server" DataSourceID="SqlItems" AutoGenerateColumns="false" SkinID="GridView">
        <HeaderStyle Font-Bold="true" />
        <Columns>
            <asp:BoundField HeaderText="PF ID" DataField="productID" />
            <asp:BoundField HeaderText="Product Name" DataField="warehouseProductName" NullDisplayText="Not assigned" />
            <asp:BoundField HeaderText="Qty" DataField="qty" />
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="SqlOrder" runat="server" SelectCommand="procOldOrdersByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderid" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlItems" runat="server" SelectCommand="procOldOrdersItemByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderid" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

