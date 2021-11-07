<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="warehouseList.aspx.vb" Inherits="maintenance_warehouseList" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000">
        <ProgressTemplate>
            Loading...<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
        View stock for:&nbsp;
            <asp:DropDownList ID="drpMainCountry" runat="server" DataSourceID="sqlCountrys" DataTextField="countryName" DataValueField="countryCode" AppendDataBoundItems="true" AutoPostBack="true">
                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:GridView ID="gvProducts" runat="server" SkinID="GridView" DataSourceID="SqlProducts" AutoGenerateColumns="false" GridLines="none" >
                <Columns>
                    <asp:BoundField HeaderText="Product" DataField="warehouseProductName" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Country" DataField="countryName" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Stock Qty" DataField="stock" NullDisplayText="0" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="In Production" DataField="production" NullDisplayText="0" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkAdjust" runat="server" Text="Adjust" NavigateUrl='<%# makeAdjustURL(Eval("warehouseProductID")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
        
    <asp:SqlDataSource ID="SqlCountrys" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProducts" runat="server" SelectCommand="procWarehouseStockByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMainCountry" Name="countryCode" Type="string" Size="5" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>


