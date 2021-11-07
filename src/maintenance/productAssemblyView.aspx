<%@ Page Language="VB" AutoEventWireup="false" CodeFile="productAssemblyView.aspx.vb" Inherits="maintenance_productAssemblyView" title="Untitled Page" %>
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
    <asp:HyperLink ID="lnkBack" runat="server" Text="Back" Visible="false"></asp:HyperLink>
    <br /><br />
    <asp:FormView ID="fvProdAss" runat="server" DataSourceID="sqlProductionBatch" OnDataBound="fvProdAss_dataBound">
        <ItemTemplate>
            <table>
                <tr>
                    <td>
                        <b>Production ID:</b>
                    </td>
                    <td>
                        <asp:Label ID="lblProdAssemblyID" runat="server" Text='<%# Eval("productionID") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Product:</b>
                    </td>
                    <td>
                        <asp:Label ID="lblProdProduct" runat="server" Text='<%# Eval("productName") %>'></asp:Label>
                    </td>                   
                </tr>
                <tr>
                    <td>
                        <b>Production Start Date:</b>
                    </td>
                    <td>
                        <asp:Label ID="lblProdStartDate" runat="server" Text='<%# Eval("productionStartDate","{0:D}") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Production Complete Date:</b>
                    </td>
                    <td>
                        <asp:Label ID="lblProdEndDate" runat="server" Text='<%# Eval("productionEndDate","{0:D}") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Batch Qty:</b>
                    </td>
                    <td>
                        <asp:Label ID="lblProdAmount" runat="server" Text='<%# Eval("productionAmount") %>'></asp:Label> <asp:Label ID="lblPassFail" runat="server" Text='<%# showPassFail(Eval("productionPassed"),Eval("productionFailed")) %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Created By:</b>
                    </td>
                    <td>
                        <asp:Label ID="lblProdUser" runat="server" Text='<%# Eval("productionUser") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Completed By:</b>
                    </td>
                    <td>
                        <asp:Label ID="lblprodUserComplete" runat="server" Text='<%# Eval("productionUserComplete") %>'></asp:Label><asp:Label ID="lblIncomplete" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Components Used:</b>
                    </td>
                    <td>
                        <asp:GridView ID="gvComp" runat="server" DataSourceID="sqlComp" AutoGenerateColumns="false" GridLines="none">
                            <HeaderStyle Font-Bold="true" />
                            <Columns>
                                <asp:BoundField HeaderText="Component" DataField="componentName" />
                                <asp:BoundField />
                                <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" />
                                <asp:BoundField />
                                <asp:BoundField HeaderText="Qty" DataField="qtyStockRemoved" /> 
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Comments:</b>
                    </td>
                    <td>
                        <asp:Label id="lblProdComments" runat="server" Width="400" Text='<%# Eval("productionComments") %>'></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblProductionInfo" runat="server" Text='<%# Eval("productionInfo") %>'></asp:Label>
        </ItemTemplate>
    </asp:FormView>
    <br /><br />
    <b>Component History:</b>
    <asp:Panel ID="panFailDetails" runat="Server">
        <asp:GridView ID="gvFailDetails" runat="server" GridLines="none" DataSourceID="SqlFailDetails" AutoGenerateColumns="false">
            <HeaderStyle Font-Bold="true" />
            <RowStyle VerticalAlign="top" />
            <Columns>
                <asp:TemplateField HeaderText="Date">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("dateChanged")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Component" DataField="componentName" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Qty" DataField="qty" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="Action" DataField="compAction" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:BoundField HeaderText="User" DataField="changedBy" />
                
                
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <br />
    <b>Failure Report Comments</b>
    <br /><br />
    <asp:Label ID="lblProductionInfoCopy" runat="server"></asp:Label>

    <asp:SqlDataSource ID="sqlComp" runat="server" SelectCommand="procComponentHistoryByProductAssemblyIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="productionID" QueryStringField="id" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlproductionBatch" runat="server" SelectCommand="procProductionBatchByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="productionID" QueryStringField="id" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlFailDetails" runat="server" SelectCommand="procComponentHistoryByBatchIDDetailsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="batchID" QueryStringField="id" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </div>
    <script language="Javascript">
            function printWin()
            {
                document.form1.btnPrint.style.display='none';
                window.print();
            }
        </script>
</form>
</body>
</html>
