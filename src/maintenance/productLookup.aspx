<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productLookup.aspx.vb" Inherits="maintenance_productLookup" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Full list of new products:<br />
    <asp:GridView ID="gvList" runat="server" DataKeyNames="warehouseProductID" DataSourceID="SqlList" AutoGenerateColumns="false" ShowFooter="true">
        <HeaderStyle Font-Bold="true" />
        <Columns>
            <asp:BoundField HeaderText="Product Name" DataField="warehouseProductName" />
            <asp:BoundField HeaderText="Country" DataField="warehouseProductCountryCode" />
            <asp:BoundField HeaderText="New ID" DataField="warehouseProductID" />
            <asp:TemplateField HeaderText="EB2k5 ID">
                <ItemTemplate>
                    <asp:TextBox ID="txtOldID" runat="Server" Text='<%# Eval("oldid") %>' Width="54"></asp:TextBox>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:Button ID="btnSubmit" runat="server" Text="Update" OnClick="btnSubit_click" />
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblValueExists" runat="server" Text='<%# doesExist(Eval("oldID")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="SqlList" runat="server" SelectCommand="procWarehouseProductListSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

