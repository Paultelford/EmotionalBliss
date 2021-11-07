<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="customerOrders.aspx.vb" Inherits="maintenance_customerOrders" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td>
                        Order Type        
                    </td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpType_selectedIndexChanged">
                            <asp:ListItem Text="All" Value="%"></asp:ListItem>
                            <asp:ListItem Text="Outstanding" Value="outstanding" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Cancelled" Value="cancelled"></asp:ListItem>
                            <asp:ListItem Text="Complete" Value="complete"></asp:ListItem>
                        </asp:DropDownList>        
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        Country    
                    </td>
                    <td>
                        <asp:DropDownList id="drpCountry" runat="server" DataSourceID="sqlCountry" DataTextField="countryName" DataValueField="countryCode" AppendDataBoundItems="true" AutoPostBack="true">
                            <asp:ListItem Text="All" Value="%"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <table border="0" width="100%">
                <tr>
                    <td valign="top">
                        <asp:GridView ID="gvOrders" runat="server" DataSourceID="sqlOrders" AllowPaging="true" PageSize="30" SkinID="GridView">
                            <Columns>
                                <asp:BoundField HeaderText="OrderID" DataField="userOrderID" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Date" DataField="orderDate" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Country" DataField="countryName" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Status" DataField="orderStatus" />
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td width="80">&nbsp;</td>
                    <td valign="top" align="center">
                        <asp:Panel ID="panList" runat="server" BorderWidth="1">
                            <asp:GridView ID="gvList" runat="server" DataSourceID="sqlList" AutoGenerateColumns="false" GridLines="none">
                                <Columns>
                                    <asp:BoundField ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" HeaderText="Country" DataField="countryName" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Total Orders" DataField="qty" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlCountry" runat="server" SelectCommand="procCountryByOrderTypeDistinctSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="status" Type="string" Size="20" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOrders" runat="server" SelectCommand="procShopOrderOutstandingCustomerByStatusCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="status" Size="20" Type="string" />
            <asp:ControlParameter ControlID="drpCountry" PropertyName="selectedValue" Name="countryCode" Size="5" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlList" runat="server" SelectCommand="procShopOrdersByStatusTotalSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="status" Type="string" Size="20" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

