<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="products.aspx.vb" Inherits="affiliates_products" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <table border="0" width="100%">
        <tr>
            <td>
                <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
                    <ProgressTemplate>
                        Please Wait....<img src="/images/loading.gif" width="16" height="16" />
                    </ProgressTemplate>
                </atlas:UpdateProgress>        
            </td>
            <td align="right">
                <asp:HyperLink ID="lnkPurchaseManagement" runat="server" Text="Back" Font-Bold="true" NavigateUrl="~/affiliates/purchaseManagement.aspx"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Choose a Product to Edit:&nbsp;
            <asp:DropDownList ID="drpProd" runat="server" DataSourceID="sqlOnSale" DataTextField="affProductName" DataValueField="affproductBuyingID" OnSelectedIndexChanged="drpProd_selectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true">
                <asp:ListItem Text="Please choose...." Value="0" />
            </asp:DropDownList>
            <br />
            Or enter a new product below and then click 'Add'.
            <br /><br />
            <asp:DetailsView ID="dvProd" runat="server" DataSourceID="sqlProduct" BorderWidth="0" GridLines="none" DataKeyNames="affproductBuyingID" AutoGenerateRows="false" DefaultMode="insert" OnItemInserted="dvProd_itemInserted" OnItemUpdated="dvProd_itemUpdated" OnDataBound="dvProd_dataBound">
                <Fields>
                    <asp:BoundField HeaderText="Product Name:" DataField="affProductName" ControlStyle-Width="260" />
                    <asp:TemplateField HeaderText="Supplier:">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpManu" runat="server" DataSourceID="sqlExistingSuppliers" DataTextField="supplierName" DataValueField="supplierID"></asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="drpManu" runat="server" DataSourceID="sqlExistingSuppliers" DataTextField="supplierName" DataValueField="supplierID" SelectedValue='<%# Bind("supplierID") %>'></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit Price:">
                        <ItemTemplate>
                            <asp:TextBox Width="40" ID="txtUnitPrice" runat="server" Text='<%# Bind("affUnitPrice") %>'></asp:TextBox>&nbsp;
                            <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tax Rate:">
                        <ItemTemplate>
                            <asp:TextBox Width="40" ID="txtTaxRate" runat="server" Text='<%# Bind("affTaxRate") %>'></asp:TextBox>&nbsp;
                            %
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Button" UpdateText="Update" InsertText="Add" ShowInsertButton="true" ShowCancelButton="false" ShowEditButton="true" />
                </Fields>
            </asp:DetailsView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlExistingSuppliers" runat="server" SelectCommand="procSuppliersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlOnSale" runat="server" SelectCommand="procAffiliateProductBuyingByAffIDProdSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProduct" runat="server" SelectCommand="procAffiliateProductBuyingByIDProdSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procAffiliateProductBuyingByIDProdUpdate" UpdateCommandType="StoredProcedure" InsertCommand="procAffiliateProductBuyingByIDProdInsert" InsertCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpProd" PropertyName="selectedValue" Name="id" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="affProductBuyingID" Type="int32" />
            <asp:Parameter Name="affProductName" Type="string" Size="30" />
            <asp:Parameter Name="supplierID" Type="int32" />
            <asp:Parameter Name="affUnitPrice" Type="decimal" />
            <asp:Parameter Name="affTaxRate" Type="decimal" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="affProductName" Type="string" Size="30" />
            <asp:Parameter Name="supplierID" Type="int32" />
            <asp:Parameter Name="affUnitPrice" Type="decimal" />
            <asp:Parameter Name="affTaxRate" Type="decimal" />
            <asp:SessionParameter SessionField="EBAffID" Name="affID" Type="int32" />
        </InsertParameters>
    </asp:SqlDataSource>    
</asp:Content>

