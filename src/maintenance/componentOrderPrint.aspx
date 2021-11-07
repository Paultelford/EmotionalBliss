<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentOrderPrint.aspx.vb" Inherits="maintenance_componentOrderPrint" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Please <span onclick="printPage()" style="color:Blue;cursor:hand">print</span> a hard copy.<br /><br />
    <asp:Panel Width="500" ID="panMain" runat="server">
        <span id="orderHTML">
        <b>Purchase Order:</b> <asp:Label ID="lblPurchaseOrder" runat="server"></asp:Label><br />
        <b>Date:</b> <%=formatDateTime(now(),DateFormat.LongDate) %><br /><br />
        <asp:Table runat="server" ID="tblAddresses" Width="80%">
            <asp:TableRow>
                <asp:TableCell Width="160" VerticalAlign="top" ColumnSpan="2">
                    <b>To:</b>
                    <asp:DetailsView ID="dvToAddress" runat="server" AutoGenerateRows="false" GridLines="none" OnDataBound="dvToAddress_dataBound">
                        <Fields>
                            <asp:BoundField DataField="manufacturerName" NullDisplayText="-" />
                            <asp:BoundField DataField="manufacturerAdd1" NullDisplayText="-" />
                            <asp:BoundField DataField="manufacturerAdd2" NullDisplayText="-" />
                            <asp:BoundField DataField="manufacturerAdd3" NullDisplayText="-" />
                            <asp:BoundField DataField="manufacturerAdd4" NullDisplayText="-" />
                            <asp:BoundField DataField="manufacturerAdd5" NullDisplayText="-" />
                            <asp:BoundField DataField="manufacturerAdd6" NullDisplayText="-" />
                        </Fields>
                    </asp:DetailsView>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="2">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="160" VerticalAlign="top">
                    <b>Ship To:</b><br />
                    <asp:DetailsView ID="dvShipping" runat="server" DataSourceID="sqlShippingAddress" GridLines="none" AutoGenerateRows="false" OnDataBound="dvShipping_dataBound">
                        <Fields>
                            <asp:BoundField DataField="company" NullDisplayText="*" />
                            <asp:BoundField DataField="add1" NullDisplayText="*" />
                            <asp:BoundField DataField="add2" NullDisplayText="*" />
                            <asp:BoundField DataField="add3" NullDisplayText="*" />
                            <asp:BoundField DataField="add4" NullDisplayText="*" />
                            <asp:BoundField DataField="add5" NullDisplayText="*" />
                        </Fields>
                    </asp:DetailsView>
                </asp:TableCell>
                <asp:TableCell Width="160" VerticalAlign="top">
                    <b>Bill To:</b>
                    <asp:DetailsView ID="dvBilling" runat="server" DataSourceID="sqlBillingAddress" GridLines="none" AutoGenerateRows="false" EmptyDataText="*" OnDataBound="dvBilling_dataBound">
                        <Fields>
                            <asp:BoundField DataField="company" NullDisplayText="*" />
                            <asp:BoundField DataField="add1" NullDisplayText="*" />
                            <asp:BoundField DataField="add2" NullDisplayText="*" />
                            <asp:BoundField DataField="add3" NullDisplayText="*" />
                            <asp:BoundField DataField="add4" NullDisplayText="*" />
                            <asp:BoundField DataField="add5" NullDisplayText="*" />
                        </Fields>
                    </asp:DetailsView>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table><br /><br />
        <asp:Table ID="tblItems" runat="server" Width="80%" BorderWidth="1" CellSpacing="0" GridLines="Both">
            <asp:TableHeaderRow>
                <asp:TableCell HorizontalAlign="center" Width="15%">
                    <b>Item No.</b>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="center" Width="50%">
                    <b>Description</b>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="center" Width="15%">
                    <b>Qty</b>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="center" Width="20%">
                    <b>Unit Price</b>
                </asp:TableCell>
            </asp:TableHeaderRow>
        </asp:Table>
        <asp:Table ID="tblTotals" runat="server" Width="80%" CellSpacing="0">
            <asp:TableRow>
                <asp:TableCell Width="15%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="50%" HorizontalAlign="right">
                    <b>Delivery:</b>
                </asp:TableCell>
                <asp:TableCell Width="15%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="20%">
                    <asp:Label ID="lblDelivery" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="15%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="50%" HorizontalAlign="right">
                    <b>SubTotal:</b>
                </asp:TableCell>
                <asp:TableCell Width="15%">&nbsp;</asp:TableCell>
                <asp:TableCell Width="20%">
                    <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        </span><br />
        <asp:Label ID="lblPDFLink" runat="server"></asp:Label>
    </asp:Panel>
    
    <asp:SqlDataSource ID="sqlBillingAddress" runat="server" SelectCommand="procComponentOrderByIDBillingSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="compOrderID" QueryStringField="id" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlShippingAddress" runat="server" SelectCommand="procComponentOrderByIDShippingSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="compOrderID" QueryStringField="id" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <script language="Javascript" type="text/javascript">
    var win;
    function printPage()
    {
        win=window.open("componentOrderPrintPop.aspx","compOrderPrintPop");
    }
    function remoteCall()
    {
        win.receiveData(document.getElementById("orderHTML").innerHTML);
    }
    </script>
</asp:Content>



