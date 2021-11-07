<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="chequeLog.aspx.vb" Inherits="affiliates_chequeLog" title="Untitled Page" Theme="WinXP_Blue" %>
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
            <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl><br />
            Search By:&nbsp;
            <asp:DropDownList ID="drpSearchField" runat="server" AutoPostBack="true">
                <asp:ListItem Text="Order Date" Value="orderDate"></asp:ListItem>
                <asp:ListItem Text="Cheque Recieved Date" Value="chequeDate"></asp:ListItem>
                <asp:ListItem Text="Cheque Cleared Date" Value="chequeClearedDate"></asp:ListItem>
            </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Panel ID="pan1" runat="server">
                <asp:GridView ID="gvList" runat="server" DataSourceID="sqlList" DataKeyNames="day" AutoGenerateColumns="false" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvList_selectedIndexChanged" Width="300" AllowPaging="true" PageSize="20" PagerSettings-Visible="false">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDateSelect" runat="server" Text='<%# formatDateTime(Eval("day"),vbLongDate) %>' CommandName="select"></asp:LinkButton>
                            </ItemTemplate>                             
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="80" />
                        <asp:BoundField HeaderText="No of Orders" DataField="items" />
                    </Columns>
                </asp:GridView>
                <eb:Pager id="pager1" runat="server" tableWidth="300" showTextBox="false"></eb:Pager>
            </asp:Panel>
            <br /><br />
            <asp:Panel ID="pan2" runat="server" Visible="false">
                Order By:&nbsp;
                <asp:DropDownList ID="drpOrderField" runat="Server" AutoPostBack="true">
                    <asp:ListItem Text="Order Date" Value="orderDate"></asp:ListItem>
                    <asp:ListItem Text="Cheque Recieved Date" Value="chequeDate"></asp:ListItem>
                    <asp:ListItem Text="Cheque Cleared Date" Value="chequeClearedDate"></asp:ListItem>
                </asp:DropDownList><br />
                <asp:GridView ID="gvLog" runat="server" DataSourceID="sqlDay" AutoGenerateColumns="false" SkinID="GridView">
                    <Columns>
                        <asp:HyperLinkField HeaderText="Order ID" DataNavigateUrlFields="ID" DataTextField="userOrderID" DataNavigateUrlFormatString="orderView.aspx?id={0}" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Status" DataField="orderStatus" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Order Date" DataField="chOrder" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Cheque Date" DataField="chDate" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Cleared Date" DataField="chCleared" HtmlEncode="false" DataFormatString="{0:dd/MM/yyyy}" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlList" runat="server" SelectCommand="procShopCustomerByCountryDateChequeLogDaySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="String" Size="5" />
            <asp:ControlParameter ControlID="drpSearchField" PropertyName="selectedValue" Name="searchField" Type="String" Size="20" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDay" runat="server" SelectCommand="procShopCustomerByCountryDateChequeLogSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvList" PropertyName="selectedValue" Name="day" Type="String" Size="10" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="String" Size="5" />
            <asp:ControlParameter ControlID="drpSearchField" PropertyName="selectedValue" Name="searchField" Type="String" Size="20" />
            <asp:ControlParameter ControlID="drpOrderField" PropertyName="selectedValue" Name="orderField" Type="String" Size="20" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

