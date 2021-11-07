<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="warehouseHistory.aspx.vb" Inherits="maintenance_warehouseHistory" title="Untitled Page" %>
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
            <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="SqlCountrys" DataTextField="countryName" DataValueField="countryCode" AutoPostBack="true" AppendDataBoundItems="true">
                <asp:ListItem Text="All" Value="%"></asp:ListItem>
            </asp:DropDownList><br /><br />
            <asp:GridView ID="gvProducts" runat="server" SkinID="GridView" DataSourceID="SqlProducts" DataKeyNames="warehouseProductID" AutoGenerateColumns="false" GridLines="none" EmptyDataText="No products found for selected country.">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:HyperLinkField HeaderText="Product" DataTextField="warehouseProductName" DataNavigateUrlFields="warehouseProductID" DataNavigateUrlFormatString="warehouseHistoryView.aspx?id={0}" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Code" DataField="warehouseProductCode" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Country" DataField="countryName" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
       
    
    <asp:SqlDataSource ID="SqlCountrys" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProducts" runat="server" SelectCommand="procWarehouseProductsByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpCountry" PropertyName="selectedValue" Name="countryCode" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

