<%@ Page Language="VB" AutoEventWireup="false" CodeFile="productAssemblyCompletePrint.aspx.vb" Inherits="maintenance_productAssemblyCompletePrint" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Product Assembly Faulty Components</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <center>
            <input type="button" value="Print" id="btnPrint" name="btnPrint" onclick="printWin()" />
        </center>
        <B><font size="+1">Product Assembly Details</font></B>
        <br /><br />
        <asp:FormView ID="fvComplete" runat="server" DataSourceID="SqlComplete">  
            <ItemTemplate>
                <table border="0">
                    <tr>
                        <td>
                            <b>Start Date:</b>
                        </td>
                        <td>    
                            <asp:Label ID="lblStart" runat="server" Text='<%# showDate(Eval("productionStartDate")) %>'></asp:Label>
                        </td>
                        <td width="100">&nbsp;</td>
                        <td>
                            <b>Completed:</b>
                        </td>
                        <td>
                            <asp:Label ID="lblEndDate" runat="server" Text='<%# showDate(Eval("productionEndDate")) %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Qty</b>
                        </td>
                        <td>
                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("productionAmount") %>'></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <b>Passed:</b>
                        </td>
                        <td>
                            <asp:Label ID="lblQtyPassed" runat="server" Text='<%# Eval("productionPassed") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Batch ID:</b>
                        </td>
                        <td>
                            <asp:Label ID="lblBatchID" runat="server" Text='<%# Eval("productionID") %>'></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <b>Failed:</b>
                        </td>
                        <td>
                            <asp:Label ID="lblQtyFailed" runat="server" Text='<%# Eval("productionFailed") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Product:</b>
                        </td>
                        <td>
                            <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("productName") %>'></asp:Label>
                        </td>
                        <td colspan="3">&nbsp;</td>                        
                    </tr>
                    <tr>
                        <td colspan="5">
                            <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <b>Assembly Details:</b>
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        <asp:Label id="lblAssDetails" runat="Server" Text='<%# Eval("productionComments") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
                <br /><br />
            </ItemTemplate>
        </asp:FormView>
        <br /><br />
        <b><font size="+1">Faulty Product Component List</font></b>
        <br /><br />
        <asp:GridView ID="gvComponents" runat="server" DataSourceID="SqlComponents" AutoGenerateColumns="False" GridLines="none" Width="660" OnRowDataBound="gvComponents_dataBound">
            <HeaderStyle Font-Bold="true" />
            <Columns>
                <asp:BoundField HeaderText="Component" DataField="componentName" />
                <asp:BoundField HeaderText="Location" DataField="locationBay" />
                <asp:BoundField HeaderText="Qty" DataField="compQty" />
                <asp:BoundField HeaderText="Pass" DataField="compQty" DataFormatString="_____" ItemStyle-Width="60" />
                <asp:BoundField HeaderText="Fail" DataField="compQty" DataFormatString="_____" ItemStyle-Width="60" />
                <asp:BoundField HeaderText="Scrap" DataField="compQty" DataFormatString="_____" ItemStyle-Width="60" />
            </Columns>
        </asp:GridView>
        <br /><br />
        
        <b>Failure Report</b><br /><br />
        <table border="0">
            <tr>
                <td>
                    Qty:
                </td>
                <td>
                    ______
                </td>
                <td>
                    Report:
                </td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td>
                    Qty:
                </td>
                <td>
                    ______
                </td>
                <td>
                    Report:
                </td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td>
                    Qty:
                </td>
                <td>
                    ______
                </td>
                <td>
                    Report:
                </td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td>
                    Qty:
                </td>
                <td>
                    ______
                </td>
                <td>
                    Report:
                </td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td>
                    Qty:
                </td>
                <td>
                    ______
                </td>
                <td>
                    Report:
                </td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
            <tr>
                <td colspan="3">&nbsp;</td>
                <td>
                    ______________________________________________________________________________________
                </td>
            </tr>
        </table>
        
        
        
        <asp:SqlDataSource id="SqlComplete" runat="server" SelectCommand="procProductionBatchByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="id" Name="productionID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="SqlComponents" runat="server" SelectCommand="procProductionBatchByBatchIDComponentsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="id" Name="batchID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        
        <script language="Javascript">
            function printWin()
            {
                document.form1.btnPrint.style.display='none';
                window.print();
            }
        </script>
    </div>
    </form>
</body>
</html>
