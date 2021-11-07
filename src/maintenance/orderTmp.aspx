<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="orderTmp.aspx.vb" Inherits="maintenance_orderTmp" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy> 
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait.... <img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="Server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server" />&nbsp; <asp:LinkButton ID="lnkViewAll" runat="server" Text="View All Orders" OnClick="lnkViewAll_click"></asp:LinkButton>
            <br /><br />
            <asp:GridView ID="gvOrders" runat="Server" DataKeyNames="" SkinID="GridView" AllowPaging="true" PageSize="25" AllowSorting="true" DataSourceID="SqlOrders" AutoGenerateColumns="false" OnDataBound="gvOrders_dataBound" EmptyDataText="No orders exist within the selected date range.">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:HyperLinkField HeaderText="order ID" SortExpression="orderid" DataNavigateUrlFormatString="orderViewTmp.aspx?id={0}" DataTextField="orderid" DataNavigateUrlFields="orderid" />
                    <asp:BoundField itemstyle-width="40" />
                    <asp:TemplateField HeaderText="Complete Date" SortExpression="orderdate">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("orderDate")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>        
                    <asp:BoundField itemstyle-width="40" />
                    <asp:BoundField HeaderText="Country" DataField="country" SortExpression="country" />
                    <asp:BoundField itemstyle-width="40" />
                    <asp:BoundField HeaderText="Type" DataField="prefix" SortExpression="prefix" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    

    <asp:SqlDataSource ID="SqlOrders" runat="server" SelectCommand="procOldOrdersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Type="datetime" Name="startDate" />        
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Type="datetime" Name="endDate" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

