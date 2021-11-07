<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="quarantine.aspx.vb" Inherits="maintenance_quarantine" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    List of items in quarantine. <br />
    Can be sotred by BatchID Or OrderID<br /><br />
    <asp:GridView ID="gvQ" runat="server" DataSourceID="sqlQ" SkinID="GridView" AllowSorting="true" AutoGenerateColumns="false" GridLines="none">
        <HeaderStyle Font-Bold="true" HorizontalAlign="left" />
        <Columns>
            <asp:BoundField HeaderText="Component" DataField="componentName" SortExpression="componentName" />
            <asp:BoundField HeaderText="BatchID" DataField="compBatchID" SortExpression="compBatchID" />
            <asp:BoundField HeaderText="Qty" DataField="qtyAdded" />
            <asp:BoundField HeaderText="Purchase Order" DataField="compOrderID" SortExpression="compOrderID" />
            <asp:BoundField HeaderText="HistoryID" DataField="componentHistoryID" Visible="false" SortExpression="componentHistoryID" />
            <asp:HyperLinkField Text="Process" DataNavigateUrlFields="componentHistoryID" DataNavigateUrlFormatString="quarantineProcess.aspx?id={0}" />
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="sqlQ" runat="server" SelectCommand="procQuarantineSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>

