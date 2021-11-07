<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" Theme="WinXP_Blue" CodeFile="orderList.aspx.vb" Inherits="affiliates_orderList" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>

    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblTest" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl>
            <table border="0">
                <tr>
                    <td>
                        Order Type:        
                    </td>
                    <td>
                        <asp:DropDownList ID="drpOrderPrefix" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOrderPrefix_selectedIndexChanged" Visible="false">
                            <asp:ListItem Value="%" Text="All"></asp:ListItem>
                            <asp:ListItem Value="20" Text="Web"></asp:ListItem>
                            <asp:ListItem Value="25" Text="Post"></asp:ListItem>
                            <asp:ListItem Value="30" Text="Phone"></asp:ListItem>
                            <asp:ListItem Value="40" Text="Affiliate"></asp:ListItem>
                        </asp:DropDownList>      
                        <asp:DropDownList ID="drpOrderSource" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOrderSource_selectedIndexChanged">
                            <asp:ListItem Value="%" Text="All"></asp:ListItem>
                            <asp:ListItem Value="affiliate" Text="Affiliate"></asp:ListItem>
                            <asp:ListItem Value="callcentre" Text="Call Centre"></asp:ListItem>
                            <asp:ListItem Value="distributor" Text="Distributor"></asp:ListItem>
                            <asp:ListItem Value="shopper" Text="Shopper"></asp:ListItem>
                        </asp:DropDownList>  
                    </td>
                </tr>
                <tr>
                    <td>
                        Payment Method:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpPaymentMethod" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPaymentMethod_selectedIndexChanged">
                            <asp:ListItem Value="%" Text="All"></asp:ListItem>
                            <asp:ListItem Text="Account" Value="account"></asp:ListItem>
                            <asp:ListItem Text="Bank Account" Value="bankaccount"></asp:ListItem>
                            <asp:ListItem Text="Credit Card" Value="cc"></asp:ListItem>
                            <asp:ListItem Text="Cheque" Value="cheque"></asp:ListItem>
                            <asp:ListItem Text="iDeal" Value="ideal"></asp:ListItem>
                            <asp:ListItem Text="Paypal" Value="paypal"></asp:ListItem>
                            <asp:ListItem Text="Direct Debit" Value="ddebit"></asp:ListItem>
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
                            <asp:ListItem Value="deferred" Text="Deferred"></asp:ListItem>
                            <asp:ListItem Value="cancelled" Text="Cancelled"></asp:ListItem>
                            <asp:ListItem Value="placed" Text="Placed"></asp:ListItem>
                            <asp:ListItem Value="paid" Text="Paid"></asp:ListItem>
                            <asp:ListItem Value="paymentpending" Text="Payment Pending"></asp:ListItem>
                            <asp:ListItem Value="complete" Text="Complete"></asp:ListItem>
                            <asp:ListItem Value="partcomplete" Text="Part Complete"></asp:ListItem>
                            <asp:ListItem Value="failed" Text="Failed"></asp:ListItem>
                            <asp:ListItem Value="deleted" Text="Deleted*"></asp:ListItem>
                        </asp:DropDownList>
                        &nbsp;&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnShowAll" runat="Server" Text="Show All" OnClick="btnShowAll_click" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvOrders" runat="server" Width="100%" AutoGenerateColumns="false" SkinID="GridView" AllowPaging="true" PageSize="20" EmptyDataText="No orders found" PagerSettings-Visible="false">
                <Columns>
                    <asp:HyperLinkField HeaderText="Order ID" DataTextField="customerOrderID" DataNavigateUrlFields="id" DataNavigateUrlFormatString="orderView.aspx?id={0}" DataTextFormatString="{0}" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# ShowDate(Eval("orderDate")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Shopper">
                        <ItemTemplate>
                            <asp:Label ID="lblShopper" runat="server" Text='<%# Eval("billName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Total" DataField="orderTotal" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderSource" runat="Server" Text='<%# formatSource(Eval("orderSource")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Payment">
                        <ItemTemplate>
                            <asp:Label ID="lblPaymentMethod" runat="Server" Text='<%# formatPayment(Eval("paymentMethod")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("orderStatus") %>' ForeColor='<%# getStatusColor(Eval("orderStatus")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <eb:Pager id="pager1" runat="server" showTextBox="true"></eb:Pager>
        </ContentTemplate>
    </atlas:UpdatePanel>
        
    <asp:SqlDataSource ID="sqlOrders" runat="server" SelectCommand="procShopOrderByCountryDateSelectNew" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="sqlOrders_selecting">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:Parameter Name="startDate" Type="DateTime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="DateTime" />
            <asp:ControlParameter ControlID="drpOrderSource" PropertyName="selectedValue" Name="orderSource" Type="string" Size="20" />
            <asp:ControlParameter ControlID="drpPaymentMethod" PropertyName="selectedValue" Name="paymentMethod" Type="string" Size="20" />
            <asp:ControlParameter ControlID="drpOrderStatus" PropertyName="selectedValue" Name="orderStatus" Type="string" Size="20" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

