<%@ Page Language="VB" AutoEventWireup="false" CodeFile="productAssemblyPDF.aspx.vb" Inherits="maintenance_productAssemblyPDF" title="Untitled Page" %>
<html>
    <head runat="server"></head>
    <body>
        <form id="frmMain" runat="server">
        <center>
            <asp:Button ID="btnPrint" runat="server" OnClientClick="printPage(this)" OnClick="btnPrint_click" Text="Print" Visible="false" UseSubmitBehavior="false" />
        </center>
        <h4>Emotional Bliss Product Assembly</h4>
        <br />
        <asp:Table ID="tblAssembly" runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <b>Start Date:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="40">&nbsp;</asp:TableCell>
                <asp:TableCell>
                    &nbsp;
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblRef" runat="server" Visible="false"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Qty:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblQty" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell Width="40">&nbsp;</asp:TableCell>
                <asp:TableCell>
                    &nbsp;
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblUser" runat="server" Visible="false"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Batch ID:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblBatch" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="3">&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Product:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblProduct" runat="server"></asp:Label>
                </asp:TableCell>
                <asp:TableCell ColumnSpan="3">&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br /><br />
        <b>Assembly Details:</b> <asp:Label id="lblComments" runat="server"></asp:Label>
        <br /><br />
        <b>Components needed for batch:</b><br /><br />
        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false" GridLines="none">
            <HeaderStyle Font-Bold="true" HorizontalAlign="left" />
            <Columns>
                <asp:BoundField HeaderText="Component" DataField="componentName" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" Visible="false" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Location Bay" DataField="locationBay" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Qty" DataField="compQty" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:TemplateField>
                    <ItemTemplate>
                        ______
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <br />
         <table>
            <tr>
                <td>
                    Completed by:
                </td>
                <td>
                    _________________
                </td>
                <td width="80">&nbsp;</td>
                <td>
                    Date:
                </td>
                <td>
                    _________________
                </td>
            </tr>
        </table>
        <b><hr /></b>
        <br /><br />
        <b>Inspection Details</b>
        <br /><br />
        <table>
            <tr>
                <td>
                    Products Passed:
                </td>
                <td>
                    ______
                </td>
            </tr>
            <tr>
                <td>
                    Products Failed:
                </td>
                <td>
                    ______
                </td>
            </tr>
        </table><br /><br />
        <table>
            <tr>
                <td>
                    Checked by:
                </td>
                <td>
                    _________________
                </td>
                <td width="80">&nbsp;</td>
                <td>
                    Date:
                </td>
                <td>
                    _________________
                </td>
            </tr>
            
        </table>
        
        </form>
        <script language="javascript">
            function printPage(e)
            {
                e.style.display="none";
                window.print();
            }
        </script>
    </body>
</html>