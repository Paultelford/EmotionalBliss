<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="protxLog.aspx.vb" Inherits="affiliates_protxLog" title="Untitled Page" Theme="WinXP_Blue" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="Server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl><br /><br />
            <asp:GridView ID="gvLog" runat="server" DataSourceID="sqlLog" DataKeyNames="day" AutoGenerateColumns="true" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvLog_selectedIndexChanged" OnRowDataBound="gvLog_rowDataBound" OnDataBound="gvLog_dataBound" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Date" FooterText="Totals">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDate" runat="Server" Text='<%# formatDateTime(Eval("day"),DateFormat.ShortDate) %>' CommandName="select"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Trans Successful" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Trans Successful Amount" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Refunds Successful" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Refunds Successful Amount" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Trans Failed" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Trans Failed Amount" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Refunds Failed" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Refunds Failed Amount" />
                </Columns>
            </asp:GridView>
            <br /><br />
            <asp:GridView ID="gvDay" runat="server" DataSourceID="sqlDay" DataKeyNames="id" AutoGenerateColumns="false" Visible="false" SkinID="GridView" OnDataBound="gvDay_dataBound" EmptyDataText="No data found for selected day">
                <Columns>
                    <asp:HyperLinkField HeaderText="Order ID" DataNavigateUrlFields="id" DataNavigateUrlFormatString="orderView.aspx?id={0}" DataTextField="userOrderID" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Protx Code" DataField="VendorTxCode" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Status" DataField="status" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Detail" DataField="statusDetail" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Amount" DataField="amount" HtmlEncode="false" DataFormatString="{0:n2}" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Order Status" DataField="orderStatus" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
            
            
    <asp:SqlDataSource ID="sqlLog" runat="server" SelectCommand="procProtxByCountryDateSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDay" runat="server" SelectCommand="procProtxByDaySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="gvLog" PropertyName="selectedValue" Name="date" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

