<%@ Page Language="VB" AutoEventWireup="false" CodeFile="warehouseAssemblyPDF.aspx.vb" Inherits="maintenance_warehouseAssemblyPDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <asp:Button ID="btnPrint" runat="server" Text="Print" OnClientClick="printPage(this)" OnClick="btnPrint_click" Visible="false" UseSubmitBehavior="false" />
        </center>
        <h4>Emotional Bliss Final Product Assembly</h4>
        <br />
        <asp:Table ID="tblDetails" runat="server">
            <asp:TableRow>            
                <asp:TableCell>
                    <b>BatchID:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:label id="lblBatchID" runat="server"></asp:label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Boxed Product:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblProduct" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>            
            <asp:TableCell>
                    <b>Date:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblDate" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Country:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblCountry" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>            
            <asp:TableCell>
                    <b>Code:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblCode" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>            
            <asp:TableCell>
                    <b>Build Qty:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblQty" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>            
            <asp:TableCell>
                    <b>User:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblUser" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br /><br />
        <b>Items needed for batch:</b>
        <br /><br />
        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false" GridLines="none">
            <HeaderStyle Font-Bold="true" HorizontalAlign="left" />
            <Columns>
                <asp:TemplateField HeaderText="Item">
                    <ItemTemplate>
                        <asp:label id="lblItem" runat="server" Text='<%# getItemName(Eval("productName"),Eval("componentName")) %>'></asp:label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" NullDisplayText="EB" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Location Bay" DataField="locationBay" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Qty" DataField="qty"/>
                <asp:BoundField ItemStyle-Width="40" />
                <asp:TemplateField>
                    <ItemTemplate>
                        ______
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        <b>Comments:</b> <asp:Label ID="lblComments" runat="server"></asp:Label>
        <br /><br /><br />
        <table>
            <tr>
                <td>
                    Completed By:        
                </td>
                <td>
                    _________________________
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>
                    Date:
                </td>
                <td>
                    _________________________
                </td>
            </tr>
            </tr>
            <tr>
                <td colspan="2"><br /><br /></td>
            </tr>
            <tr>
                <td>
                    Entered By:        
                </td>
                <td>
                    _________________________
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
            <tr>
                <td>
                    Date:
                </td>
                <td>
                    _________________________
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
<script langauge="javascript">
    function printPage(e)
    {
        e.style.display="none";
        window.print();
    }
</script>
</html>
