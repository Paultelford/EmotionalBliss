<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="compareLog.aspx.vb" Inherits="affiliates_compareLog" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false" AssociatedUpdatePanelID="update1">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>    
            
            
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <br /><br />
            <table>
                <tr>
                    <td valign="top">
                        <asp:GridView ID="gvList" runat="server" DataSourceID="sqlList" AutoGenerateColumns="false" SkinID="GridView" OnDataBound="gvList_dataBound" ShowFooter="true">
                            <Columns>
                                <asp:HyperLinkField HeaderText="OrderID" DataNavigateUrlFields="id" DataTextField="userOrderID" DataNavigateUrlFormatString="~/affiliates/orderView.aspx?id={0}" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Status" DataField="orderstatus" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="SO Amount" DataField="amount" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="SL Amount" DataField="scanamount" />
                                <asp:TemplateField HeaderText="Sales">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSales" runat="server" Enabled="false" Visible="false" />
                                        <asp:Label ID="lblSales" runat="server" Text='<%# "Scanned " & showDate(Eval("scanDate")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="P Amount" DataField="protxAmount" />
                                <asp:TemplateField HeaderText="Protx">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkProtx" runat="server" Enabled="false" Visible="false" />
                                        <asp:Label ID="lblProtxDate" runat="server" Text='<%# "taken " & showDate(Eval("protxDate")) %>'></asp:Label>
                                        <asp:Label id="lblID" runat="Server" Visible="false" Text='<%# Eval("id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:TemplateField HeaderText="Missing" Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkMissing" runat="server" Enabled="false" />
                                        <asp:Label ID="lblTxCode" runat="server" Visible="false" Text='<%# Eval("txCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Discrepency" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            
            
            
            
    <asp:SqlDataSource ID="sqlList" runat="server" SelectCommand="procSalesLedgerProtxByCountryDateSelectNew" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

