<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentHistoryViewOrder.aspx.vb" Inherits="maintenance_componentHistoryViewOrder" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="panDateRange" runat="server">
        <asp:Table ID="tblDateRange" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label id="lblFrom" runat="server" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="drpDayFrom" runat="server"></asp:DropDownList>        
                </asp:TableCell>
                <asp:TableCell> 
                    <asp:DropDownList ID="drpMonthFrom" runat="server"></asp:DropDownList>            
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="drpYearFrom" runat="server"></asp:DropDownList>        
                </asp:TableCell>     
                <asp:TableCell>&nbsp;</asp:TableCell>           
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label id="lblTo" runat="server" />
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="drpDayTo" runat="server"></asp:DropDownList>        
                </asp:TableCell>
                <asp:TableCell> 
                    <asp:DropDownList ID="drpMonthTo" runat="server"></asp:DropDownList>            
                </asp:TableCell>
                <asp:TableCell>
                    <asp:DropDownList ID="drpYearTo" runat="server"></asp:DropDownList>        
                </asp:TableCell>                
                <asp:TableCell>
                    <asp:Button ID="btnDateSubmit" runat="server" Text="Update" UseSubmitBehavior="true" />
                </asp:TableCell>           
            </asp:TableRow>
        </asp:Table>
    </asp:Panel>
    <br />
    <asp:Label ID="lblOrderID" runat="server" Font-Bold="true"></asp:Label>
    <br />
    
    <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="false" EmptyDataText="No results found." GridLines="none" Width="90%" OnDataBound="gvHistory_dataBoundTotals" ShowFooter="true">
        <HeaderStyle Font-Bold="true" HorizontalAlign="left" VerticalAlign="bottom" />
        <FooterStyle Font-Bold="true" HorizontalAlign="left" />
        <Columns>
            <asp:BoundField HeaderText="Component" DataField="componentName" />
            <asp:BoundField HeaderText="Action" DataField="compAction" />
            <asp:HyperLinkField HeaderText="Component<br />Batch ID" DataTextField="compBatchID" DataNavigateUrlFields="compBatchID" DataNavigateUrlFormatString="componentHistoryViewBatch.aspx?id={0}" />
            <asp:BoundField HeaderText="Date" DataField="dateChanged" HtmlEncode="false" DataFormatString="{0:D}" FooterText="<br>Totals" />
            <asp:BoundField HeaderText="Quarantine Qty In" DataField="qtyAdded" ItemStyle-Width="80" />
            <asp:BoundField HeaderText="Qty Out" DataField="qtyRemoved" />            
            <asp:BoundField HeaderText="Stock Qty In" ItemStyle-Width="60" DataField="qtyStockAdded" />            
            <asp:BoundField HeaderText="Qty Out" DataField="qtyStockRemoved"/>        
            <asp:BoundField HeaderText="Scrapped Qty In" ItemStyle-Width="70" DataField="qtyScrappedAdded" />            
            <asp:BoundField HeaderText="Qty Out" DataField="qtyScrappedAdded" />
        </Columns>
    </asp:GridView>
</asp:Content>

