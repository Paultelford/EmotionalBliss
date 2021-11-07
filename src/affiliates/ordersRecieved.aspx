<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="ordersRecieved.aspx.vb" Inherits="affiliates_ordersRecieved" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <table width="100%">
                <tr>
                    <td valign="top">
                        <asp:GridView ID="gvOrders" runat="server" DataSourceID="sqlOrders" ShowFooter="true" AutoGenerateColumns="false" DataKeyNames="date" SkinID="GridViewRedBG" OnDataBound="gvOrders_dataBound" EmptyDataText="No orders found for selected date range">
                            <Columns>
                                <asp:TemplateField HeaderText="Order Date">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDate" runat="server" Text='<%# formatDateTime(Eval("date"),DateFormat.LongDate) %>' CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lblTotalText" runat="server" Font-Bold="true" Text="Totals"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>                                
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:BoundField HeaderText="Orders" DataField="items" ReadOnly="true" />
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Value">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotals" runat="server" Text='<%# Session("EBAffCurrencySign") & Eval("total","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:BoundField HeaderText="Visitors" DataField="visitors" Visible="false" ReadOnly="true" />
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" Visible="false" />
                                <asp:TemplateField HeaderText="Google Visitors">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGVisitors" runat="server" Text='<%# Eval("gvisitors") %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtGVisitors" CssClass="editbox" runat="server" Width="40" Text='<%# Bind("gvisitors") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Avg Customers Per Order" ItemStyle-HorizontalAlign="center" Visible="false">
                                    <FooterStyle HorizontalAlign="center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrdersPerCustomer" runat="server" Text='<%# FormatNumber(Eval("visitors")/Eval("items"),2)%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" Visible="false" />
                                <asp:TemplateField HeaderText="Avg Google Per Order" ItemStyle-HorizontalAlign="center">
                                    <FooterStyle HorizontalAlign="center" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGOrdersPerCustomer" runat="server" Text='<%# FormatNumber(Eval("gvisitors")/Eval("items"),2)%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:CommandField EditText="Edit Google Visitors" ShowCancelButton="false" ShowEditButton="true" />
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td valign="top" align="right">
                        <asp:GridView ID="gvDetails" runat="server" DataSourceID="sqlDetails" AutoGenerateColumns="false" GridLines="none" OnDataBound="gvDetails_dataBound" ShowFooter="true">
                            <Columns>
                                <asp:BoundField HeaderText="Product" DataField="affProductName" ItemStyle-HorizontalAlign="left" HeaderStyle-HorizontalAlign="left" FooterStyle-HorizontalAlign="left" FooterText="<br><b>Total</b>" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:TemplateField HeaderText="Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQty" runat="server" Text='<%# Eval("items") %>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <br />
                                        <asp:Label ID="lblTotal" runat="server"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <script src="/jQuery.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    function focusEditBox(c)
    {
        if(__nonMSDOMBrowser)
            document.getElementById(c).select();
        else
           self.setTimeout("document.all['" + c + "'].select();",200);
    }
    </script>
    <asp:SqlDataSource ID="sqlOrders" runat="server" SelectCommand="procShopOrderTotalsByDateSelect" UpdateCommand="procVisitorsGoogleByDateUpdate" UpdateCommandType="StoredProcedure" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" Name="startDate" PropertyName="getStartDate" Type="DateTime" />
            <asp:ControlParameter ControlID="date1" Name="endDate" PropertyName="getEndDate" Type="DateTime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="gvisitors" Type="int32" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="country" Type="String" Size="5" />
        </UpdateParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="sqlDetails" runat="server" SelectCommand="procShopOrderItemByDayCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvOrders" PropertyName="selectedValue" Name="date" Type="datetime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

