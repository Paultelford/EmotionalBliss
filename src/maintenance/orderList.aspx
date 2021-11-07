<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="orderList.aspx.vb" Inherits="maintenance_orderList" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <table>
                <tr>
                    <td>
                        Order Type:        
                    </td>
                    <td>
                        <asp:DropDownList ID="drpOrderType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOrderPrefix_selectedIndexChanged">
                            <asp:ListItem Value="%" Text="All"></asp:ListItem>
                            <asp:ListItem Value="distaccount" Text="Account"></asp:ListItem>
                            <asp:ListItem Value="distcc" Text="CC"></asp:ListItem>
                        </asp:DropDownList>        
                    </td>
                </tr>
                <tr>
                    <td>
                        Status: 
                    </td>
                    <td>
                        <asp:DropDownList ID="drpOrderStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOrderStatus_selectedIndexChanged">
                            <asp:ListItem Value="%" Text="All"></asp:ListItem>
                            <asp:ListItem Value="cancelled" Text="Cancelled"></asp:ListItem>
                            <asp:ListItem Value="placed" Text="Placed"></asp:ListItem>
                            <asp:ListItem Value="paid" Text="Paid"></asp:ListItem>
                            <asp:ListItem Value="complete" Text="Complete"></asp:ListItem>
                            <asp:ListItem Value="partcomplete" Text="Part Complete"></asp:ListItem>
                            <asp:ListItem Value="failed" Text="Failed"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnShowAll" runat="Server" Text="Show All" OnClick="btnShowAll_click" Visible="false" />
                    </td>
                </tr>
            </table>
            <table border="0" width="100%">
                <tr>
                    <td width="90%">
                        <asp:GridView ID="gvOrders" runat="Server" DataSourceID="SqlOrders" AutoGenerateColumns="false" GridLines="both" SkinID="GridView" OnDataBound="gvOrders_dataBound" EmptyDataText="No orders found for selected date range" AllowPaging="true" PageSize="5" Width="100%" PagerSettings-NextPageText="Next" PagerSettings-PreviousPageText="Prev" PagerSettings-Mode="NextPrevious">
                            <Columns>
                                <asp:HyperLinkField HeaderText="Order ID" DataTextField="newOrderID" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="orderView.aspx?id={0}" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Date Placed" DataField="orderDate" DataFormatString="{0:D}" HtmlEncode="false" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Type" DataField="orderType" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Status" DataField="orderStatus" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Order Total" DataField="orderTotal" DataFormatString="{0:n2}" HtmlEncode="false" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Country" DataField="distCountryName" />
                            </Columns>
                        </asp:GridView>
                        <center>
                        Page <asp:Label ID="lblIndexLo" runat="server"></asp:Label> of <asp:Label ID="lblIndexHi" runat="server"></asp:Label>
                        </center>
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="SqlOrders" runat="server" SelectCommand="procShopOrderByCountryDateDistSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:Parameter Name="countryCode" Type="string" Size="5" DefaultValue="zz" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="DateTime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="DateTime" />
            <asp:ControlParameter ControlID="drpOrderType" PropertyName="selectedvalue" Name="ordertype" Type="string" Size="15" />
            <asp:ControlParameter ControlID="drpOrderStatus" PropertyName="selectedValue" Name="orderStatus" Type="string" Size="20" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

