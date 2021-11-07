<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="salesLedgerLogAccounts.aspx.vb" Inherits="affiliates_salesLedgerLogAccounts" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<atlas:ScriptManagerProxy id="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panLog" runat="server">
                <table>
                    <tr>
                        <td align="center">
                            <h2>Sales Ledger</h2><br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table border="0">
                                <tr>
                           
                                    <td>
                                        <eb:DateControl id="date1" runat="server" ></eb:DateControl>        
                                    </td>
                                    <td width="40">&nbsp;</td>
                                    <td valign="bottom">
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTypeText" runat="server" Text="Type:"></asp:Label>                                    
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="bottom">
                                        <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true" DataTextField="orderType" DataValueField="orderType" DataSourceID="sqlOrderTypes" AppendDataBoundItems="true" OnSelectedIndexChanged="drpType_selectedIndexChanged">
                                            <asp:ListItem Text="All" Value="%" />
                                            <asp:ListItem Text="Web+Phone" Value="webphone" />
                                        </asp:DropDownList>
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:HyperLink id="lnkShowComplete" runat="server" Text="Show Complete Orders" Visible="false" Target="_blank" />
                                    </td>
                                </tr>
                            </table>
                            <br /><br />
                            <asp:GridView ID="gvSalesLedger" runat="server" ShowFooter="true" DataKeyNames="day" DataSourceID="sqlSalesLedger" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" EmptyDataText="No orders found.">
                                <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Day">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                            <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle  HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField HeaderText="Vat" ItemStyle-Width="50" DataField="balanceVat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField HeaderText="Vat" ItemStyle-Width="50" DataField="chequeVat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField HeaderText="Vat" ItemStyle-Width="50" DataField="affiliateccVat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField HeaderText="Vat" ItemStyle-Width="50" DataField="affiliateaccountVat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                    <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                    <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("balanceVat") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateccVat") + Eval("affiliateaccount") + Eval("affiliateaccountVat") + Eval("cheque") + Eval("chequeVat"),2) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Panel ID="panDetails" runat="server" Visible="false">
                            <asp:LinkButton ID="lnkBack" runat="Server" Text="Back" OnClick="lnkBack_click" Font-Bold="true"></asp:LinkButton><br />
                            <asp:Label ID="lblDay" runat="Server" Font-Bold="true"></asp:Label><br /><br />
                            Note: Affiliate Account orders are highlighted in red
                            <asp:GridView ID="gvSalesLedgerDay" runat="server" datasourceid="sqlSalesLedgerDay" AutoGenerateColumns="false" SkinID="GridView" ShowFooter="true" OnDataBound="gvSalesLedgerDay_dataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Order ID">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="lnkUserOrderID" runat="server" Text='<%# Eval("userOrderID") %>' NavigateUrl='<%# "~/affiliates/orderView.aspx?id=" & Eval("orderID") %>'></asp:HyperLink>
                                            <asp:Label ID="lblOrderType" runat="server" Text='<%# Eval("orderType") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Goods" DataField="balance" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Vat" DataField="vat" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Affiliate" DataField="affCompany" />
                                    <asp:BoundField ItemStyle-Width="90" ItemStyle-HorizontalAlign="center" />
                                    <asp:BoundField HeaderText="Tracker" DataField="trackerCode" NullDisplayText="N/A" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlOrderTypes" runat="server" SelectCommand="procShopOrderTypeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSalesLedger" runat="server" SelectCommand="procSalesLedgerByDateSelectAccounts" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="type" Type="string" Size="50" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSalesLedgerDay" runat="server" SelectCommand="procSalesLedgerByDaySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="sqlSalesLedgerDay_selecting">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvSalesLedger" name="ledgerDay" Type="datetime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="type" Type="string" Size="50" />
            <asp:Parameter Name="actionID" Type="int16" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>

