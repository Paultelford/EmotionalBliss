<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="tradeOverview.aspx.vb" Inherits="maintenance_tradeOverview" title="Untitled Page" %>
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
            <asp:Panel ID="panPage" runat="server">
                <asp:Table ID="tblControls" runat="server">
                    <asp:TableRow ID="tblRow1" runat="Server" Visible="false">
                        <asp:TableCell>
                            View:        
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpViewType" runat="server" OnSelectedIndexChanged="drpViewType_selectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="All Distributors" value="dist"></asp:ListItem>
                                <asp:ListItem Text="Individual Distributors Overview" Value="aff"></asp:ListItem>
                            </asp:DropDownList>        
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tblRow2" runat="server" Visible="false">
                        <asp:TableCell>
                            Distributor:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpDistributors" runat="server" DataSourceID="sqlDistributors" AutoPostBack="true" DataTextField="countryName" DataValueField="countryCode" OnSelectedIndexChanged="drpDistributors_selectedIndexChanged" AppendDataBoundItems="true">
                                <asp:ListItem Text="Select country..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <eb:DateControl id="date1" runat="server"></eb:DateControl>
                <br /><br />
            </asp:Panel>
            <asp:GridView ID="gvOverview" runat="server" DataSourceID="sqlOverview" AutoGenerateColumns="false" EmptyDataText="No data found" OnDataBound="gvOverview_dataBound" SkinID="GridView">
                <Columns>
                    <asp:BoundField HeaderText="Affiliate" DataField="affCompany" />
                    <asp:BoundField ItemStyle-Width="60" />
                    <asp:BoundField HeaderText="Country" DataField="affCountryCode" />
                    <asp:BoundField ItemStyle-Width="60" />
                    <asp:BoundField HeaderText="Credit" DataField="credit" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField ItemStyle-Width="60" />
                    <asp:BoundField HeaderText="Debit" DataField="debit" ItemStyle-HorizontalAlign="right" />
                    <asp:BoundField ItemStyle-Width="60" />
                    <asp:BoundField HeaderText="Balance" ItemStyle-HorizontalAlign="right" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlDistributors" runat="server" SelectCommand="procAffiliatesByIsDistCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:Parameter Name="isDist" Type="boolean" DefaultValue="true" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOverview" runat="server" SelectCommand="procAffiliateStatementOverviewSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="sqlOverview_selecting">
        <SelectParameters>
            <asp:Parameter Name="isDist" Type="boolean" />
            <asp:Parameter Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>