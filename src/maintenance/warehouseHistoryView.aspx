<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="warehouseHistoryView.aspx.vb" Inherits="maintenance_warehouseHistoryView" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="update1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="../images/loading.gif" width="16" height="16" />
        </ProgressTemplate>              
    </atlas:UpdateProgress>
    <br />
    <asp:Label ID="lblProduct" Text="Not set" runat="server" Font-Bold="true"></asp:Label><br /><br />
    <atlas:UpdatePanel ID="updateDates" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
        </ContentTemplate>
    </atlas:UpdatePanel>    
    <br />
    <atlas:UpdatePanel ID="updateHistory" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvHistory" runat="server" DataSourceID="SqlHistory" AutoGenerateColumns="false" CellPadding="4" CellSpacing="0" GridLines="vertical" ShowFooter="true" OnDataBound="gvHistory_dataBound" EmptyDataText="No data found for sleected date range.">
                <HeaderStyle Font-Bold="true" />
                <FooterStyle Font-Bold="true" />
                <RowStyle Font-Size="Small" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# formatStartDate(Eval("dateChanged")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Action" DataField="Action" />
                    <asp:BoundField HeaderText="Info" />
                    <asp:BoundField HeaderText="Stock Added" DataField="qtyAdded" NullDisplayText="0" />
                    <asp:BoundField HeaderText="Stock Removed" DataField="qtyRemoved" NullDisplayText="0" />
                    <asp:BoundField HeaderText="Production(In)" DataField="qtyProductionAdded" NullDisplayText="0" />
                    <asp:BoundField HeaderText="Production(Out)" DataField="qtyProductionRemoved" NullDisplayText="0" />
                    <asp:BoundField HeaderText="User" DataField="username" />
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("warehouseActionID") %>'></asp:Label>
                            <asp:Label ID="lblOrderID" runat="server" Text='<%# Eval("orderID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <atlas:AsyncPostBackTrigger ControlID="date1" EventName="DateChanged" />
        </Triggers>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="SqlHistory" runat="server" SelectCommand="procWarehouseHistoryByProductIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="ProductID" QueryStringField="id" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>




