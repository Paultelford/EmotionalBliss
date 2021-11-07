<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="productsSold.aspx.vb" Inherits="affiliates_productsSold" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
     
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <asp:DropDownList ID="drpProduct" runat="server" DataSourceID="sqlProducts" AutoPostBack="true" AppendDataBoundItems="true" DataTextField="affProductName" DataValueField="affProductbuyingID">
                <asp:ListItem Text="All Products" Value="%"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:GridView ID="gvSales" runat="server" DataSourceID="sqlProductRange" SkinID="GridViewRedBG" AutoGenerateColumns="false" EmptyDataText="No sales found for selected date range.">
                <Columns>
                    <asp:BoundField HeaderText="Product" DataField="affProductName" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Qty Sold" DataField="items" />
                </Columns>
            </asp:GridView>
            
            
    <asp:SqlDataSource ID="sqlProducts" runat="server" SelectCommand="procAffiliateProductBuyingByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlProductRange" runat="server" SelectCommand="procShopOrderItemByDateCountryCodeStatusSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" Name="startDate" Type="datetime" PropertyName="getStartDate" />
            <asp:ControlParameter ControlID="date1" Name="endDate" Type="datetime" PropertyName="getEndDate" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpProduct" PropertyName="selectedValue" Name="affProductBuyingID" Type="string" Size="6" />
            <asp:Parameter Name="status" Type="string" Size="20" DefaultValue="complete" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

