<%@ Page Language="VB" AutoEventWireup="false" CodeFile="manufacturerDebitsPDF.aspx.vb" Inherits="maintenance_manufacturerDebitsPDF" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Debit Note</title>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Panel ID="panMain" runat="server">
        <img src="../images/peartreePOlogo.jpg" width="792" /><br />
        <span id="orderHTML">
        <asp:Table runat="server" ID="tblAddresses" Width="792" BorderWidth="0">
            <asp:TableRow>
                <asp:TableCell VerticalAlign="top">
                    <asp:DetailsView ID="dvToAddress" runat="server" DataSourceID="SqlAddress" AutoGenerateRows="false" GridLines="none" OnDataBound="dvToAddress_dataBound">
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
                <asp:TableCell HorizontalAlign="right" VerticalAlign="top">
                    <table border="0">
                        <tr>
                            <td align="left">
                                PearTree UK<br />
                                22 Carlisle St<br />
                                Goole<br />
                                DN14 5DU<br />                              
                                England
                                <asp:DetailsView ID="dvBilling" runat="server"  GridLines="none" AutoGenerateRows="false" EmptyDataText="*" OnDataBound="dvBilling_dataBound">
                                <Fields>
                                    <asp:BoundField DataField="company" NullDisplayText="*" />
                                    <asp:BoundField DataField="add1" NullDisplayText="*" />
                                    <asp:BoundField DataField="add2" NullDisplayText="*" />
                                    <asp:BoundField DataField="add3" NullDisplayText="*" />
                                    <asp:BoundField DataField="add4" NullDisplayText="*" />
                                    <asp:BoundField DataField="add5" NullDisplayText="*" />
                                </Fields>
                            </asp:DetailsView>
                            </td>
                        </tr>
                    </table>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />        
        <table border="0" width="792">
            <tr>
                <td align="center">
                    <h1 style="color:black;"><b>Debit Note</b></h1>
                </td>
            </tr>
        </table>    
        <br />    
        <asp:Table id="tblPO" runat="server" BorderWidth="0" Width="792" CellPadding="0" CellSpacing="0">
            <asp:TableRow>
                <asp:TableCell Width="140">
                    <b>Debit No:</b>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblDebit" runat="server" Text="YOU ARE NOT LOGGED IN"></asp:Label>
                </asp:TableCell>
                <asp:TableCell>&nbsp;</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <b>Date:</b>        
                </asp:TableCell>
                <asp:TableCell>
                    <%=formatDateTime(now(),DateFormat.LongDate) %>       
                </asp:TableCell>
                <asp:TableCell>&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>         
        <br />
        
        <asp:Table ID="tblItems" runat="server" Width="792" BorderWidth="0" CellSpacing="0" GridLines="Both">
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableHeaderRow>
                <asp:TableCell HorizontalAlign="left" Width="10%">
                    <b>Qty</b>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" Width="20%">
                    <b>Ref.</b>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" Width="50%">
                    <b>Description</b>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" Width="10%">
                    <b>Unit Price</b>
                </asp:TableCell>
                <asp:TableCell HorizontalAlign="left" Width="10%">
                    <b>Amount</b>
                </asp:TableCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="5"><hr /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell ColumnSpan="4">&nbsp;</asp:TableCell>
                <asp:TableCell>
                    <asp:Label ID="lblCurrency" runat="server"></asp:Label>
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <table border="0" width="792">
            <tr>
                <td colspan="3"><hr /></td>
            </tr>
            <tr>
                <td align="center">
                    <b>SUB TOTAL</b>
                </td>
                <td align="center">
                    <b>VAT TOTAL (<asp:Label ID="lblVatRate" runat="server"></asp:Label>)</b>
                </td>
                <td align="center">
                    <b>TOTAL</b>
                </td>
            </tr>
            <tr>
                <td align="center">
                    <asp:Label ID="lblSubTotal" runat="server"></asp:Label>
                </td>
                <td align="center">
                    <asp:Label ID="lblVatTotal" Text="0" runat="server"></asp:Label>
                </td>
                <td align="center">
                    <asp:Label ID="lblCurrency2" runat="server"></asp:Label>&nbsp;<asp:Label ID="lblNetAmount" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="3"><hr /></td>
            </tr>
        </table>
        <br /><br />
        <table border="0" width="700">
            <tr>
                <td valign="top" width="110">
                    <b><asp:label ID="lblInstLabel" runat="server" Text="Instructions:&nbsp;&nbsp;&nbsp;"></asp:label></b>
                </td>
                <td>
                    <asp:Label ID="lblInstructions" runat="Server"></asp:Label>
                </td>
            </tr>
        </table>
        
             
        </span>
    </asp:Panel>
                    
    <asp:SqlDataSource ID="sqlAddress" runat="server" SelectCommand="procManufacturerByDebitIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="debitID" QueryStringField="id" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
