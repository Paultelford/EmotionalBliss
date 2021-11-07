<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="warehouseProductAdd.aspx.vb" Inherits="maintenance_warehouseProductAdd" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>    
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Select a Boxed Product to Edit:&nbsp;
            <asp:DropDownList ID="drpBProducts" runat="server" DataTextField="FullName" EnableViewState="false" DataValueField="warehouseProductID" DataSourceID="SqlBProducts" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpBProduct_selectedIndexChanged">
                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
            </asp:DropDownList><br />
            Or, enter a new product and click 'Add'.<br /><br />
            <asp:DetailsView ID="dvBProduct" runat="server" DefaultMode="Insert" DataSourceID="SqlBProduct" DataKeyNames="warehouseProductID" GridLines="none" AutoGenerateRows="false" OnItemInserted="dvProduct_itemInserted" OnItemUpdated="dvProduct_itemUpdated">
                <HeaderStyle Font-Bold="true" VerticalAlign="top" />
                <Fields>
                    <asp:TemplateField HeaderText="BoxedProduct Name:">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtProductName" runat="server" Text='<%# Bind("warehouseProductName") %>' Width="250"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtProductName" ErrorMessage="* Required" Display="Static"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtProductName" runat="server" Text='<%# Bind("warehouseProductName") %>' Width="250"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtProductName" ErrorMessage="* Required" Display="static"></asp:RequiredFieldValidator> 
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Code:" DataField="warehouseProductCode" />
                    <asp:TemplateField HeaderText="Description:">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="60" Text='<%# Bind("warehouseProductDescription") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="5" Columns="60" Text='<%# Bind("warehouseProductDescription") %>'></asp:TextBox>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Country:">
                        <EditItemTemplate>
                            <asp:DropDownList ID="drpCountry" runat="server" selectedValue='<%# Bind("warehouseProductCountryCode") %>' DataTextField="countryName" DataValueField="countryCode" DataSourceID="SqlCountry"></asp:DropDownList>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:DropDownList ID="drpCountry" runat="server" selectedValue='<%# Bind("warehouseProductCountryCode") %>' DataTextField="countryName" DataValueField="countryCode" DataSourceID="SqlCountry"></asp:DropDownList>
                        </InsertItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="button" InsertText="Add" UpdateText="Update" ShowCancelButton="false" ShowInsertButton="true" ShowEditButton="true" />
                </Fields>
            </asp:DetailsView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="SqlBProduct" runat="server" SelectCommand="procWarehouseProductByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" InsertCommand="procWarehouseProductInsert" InsertCommandType="StoredProcedure" UpdateCommand="procWarehouseProductByIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpBProducts" Name="pid" PropertyName="selectedValue" Type="int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="warehouseProductName" Type="string" Size="50" />
            <asp:Parameter Name="warehouseProductCode" Type="string" Size="20" />
            <asp:Parameter Name="warehouseProductDescription" Type="string" Size="1000" />
            <asp:Parameter Name="warehouseProductCountryCode" Type="string" Size="5" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="warehouseProductID" Type="int32" />
            <asp:Parameter Name="warehouseProductName" Type="string" Size="50" />
            <asp:Parameter Name="warehouseProductCode" Type="string" Size="20" />
            <asp:Parameter Name="warehouseProductDescription" Type="string" Size="1000" />
            <asp:Parameter Name="warehouseProductCountryCode" Type="string" Size="5" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlBProducts" runat="server" SelectCommand="procWarehouseProductsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>


