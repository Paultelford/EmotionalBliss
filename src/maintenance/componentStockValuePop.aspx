<%@ Page Language="VB" AutoEventWireup="false" CodeFile="componentStockValuePop.aspx.vb" Inherits="maintenance_componentStockValuePop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="tblComponent" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <b>Component:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblCompName" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Manufacturer:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblManufacturerName" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Manufacturer Currency:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Stock:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblStock" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Value:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblStockValue" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br /><br /><br />
        <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" GridLines="none" DataSourceID="SqlOrders" OnDataBound="gvOrders_dataBound">
            <HeaderStyle Font-Bold="true" HorizontalAlign="left" />
            <Columns>
                <asp:BoundField HeaderText="OrderID" DataField="compOrderID" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# formatDate(Eval("orderDate")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Price" DataField="price" HtmlEncode="false" DataFormatString="{0:n2}" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Qty Received" DataField="qtyReceived" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField ItemStyle-Width="40" />
            </Columns>
        </asp:GridView>
        
        <asp:SqlDataSource ID="SqlOrders" runat="server" SelectCommand="procComponentOrderByCompIDStockValueSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter Name="compID" QueryStringField="id" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
