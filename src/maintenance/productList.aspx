<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productList.aspx.vb" Inherits="maintenance_productList" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    View completed products: <asp:DropDownList ID="drpMasters" runat="server" DataTextField="name" DataValueField="masterID" DataSourceID="SqlMasters" AppendDataBoundItems="true" AutoPostBack="true">
        <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
        <asp:ListItem Text="All" Value="%"></asp:ListItem>
    </asp:DropDownList>
    <br /><br /><br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvProducts" runat="server" SkinID="GridView" DataSourceID="SqlProducts" AutoGenerateColumns="false" GridLines="none" >
                <Columns>
                    <asp:BoundField HeaderText="Product" DataField="productName" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Stock Qty" DataField="stock" NullDisplayText="0" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="In Production" DataField="production" NullDisplayText="0" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkAdjust" runat="server" Text="Adjust" NavigateUrl='<%# makeAdjustURL(Eval("productID")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <atlas:AsyncPostBackTrigger ControlID="drpMasters" EventName="selectedIndexChanged" />
        </Triggers>
    </atlas:UpdatePanel>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000">
        <ProgressTemplate>
            Loading...<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    
    <asp:SqlDataSource ID="SqlMasters" runat="server" SelectCommand="procProductMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProducts" runat="server" SelectCommand="procProductStockByMasterIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMasters" Name="masterID" Type="string" Size="5" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>


