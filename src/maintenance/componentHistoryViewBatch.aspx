<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentHistoryViewBatch.aspx.vb" Inherits="maintenance_componentHistoryViewBatch" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <eb:DateControl ID="date1" runat="server" OnDateChanged="date1_onDateChanged" />
    <br />
    <asp:HyperLink ID="lnkBack" runat="server"></asp:HyperLink>
    <br /><br />
    <asp:Label ID="lblBatch" runat="server" Font-Bold="true"></asp:Label>
    <br />
    
    <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="false" EmptyDataText="No results found." GridLines="none" Width="90%" OnDataBound="gvHistory_dataBoundTotals" ShowFooter="true">
        <HeaderStyle Font-Bold="true" HorizontalAlign="left" VerticalAlign="bottom" />
        <FooterStyle Font-Bold="true" HorizontalAlign="left" />
        <Columns>
            <asp:BoundField HeaderText="Component" DataField="componentName" />
            <asp:BoundField HeaderText="Action" DataField="compAction" />
            <asp:HyperLinkField HeaderText="Component<br />Order ID" DataTextField="compOrderID" DataNavigateUrlFields="compOrderID" DataNavigateUrlFormatString="componentOrderView.aspx?id={0}" />
            <asp:BoundField HeaderText="Date" DataField="dateChanged" HtmlEncode="false" DataFormatString="{0:D}" FooterText="<br>Totals" />
            <asp:BoundField HeaderText="Quarantine Qty In" DataField="qtyAdded" ItemStyle-Width="80" />
            <asp:BoundField HeaderText="Qty Out" DataField="qtyRemoved" />            
            <asp:BoundField HeaderText="Stock Qty In" ItemStyle-Width="60" DataField="qtyStockAdded" />            
            <asp:BoundField HeaderText="Qty Out" DataField="qtyStockRemoved"/>        
            <asp:BoundField HeaderText="Scrapped Qty In" ItemStyle-Width="70" DataField="qtyScrappedAdded" />            
            <asp:BoundField HeaderText="Qty Out" DataField="qtyScrappedAdded" />        
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:label ID="lblQuarantine" runat="server" Text='<%# Eval("quarantine") %>'></asp:label>
                </ItemTemplate>
            </asp:TemplateField>  
        </Columns>
    </asp:GridView>

</asp:Content>

