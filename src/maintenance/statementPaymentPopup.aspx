<%@ Page Language="VB" AutoEventWireup="false" CodeFile="statementPaymentPopup.aspx.vb" Inherits="maintenance_statementPaymentPopup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <center>  
        <asp:Image ID="imgLetterHead" runat="server" ImageUrl="~/images/emotional bliss banner.jpg" /><br />
        <br /><br /><br /><br /><br />
        <table>
            <tr>
                <td align="left" valign="top">
                    <asp:FormView ID="fvDetails" runat="server" DataSourceID="sqlStatementDetail"  OnDataBound="fvDetails_dataBound">
                        <ItemTemplate>
                            <table border="0">
                                <tr>
                                    <td>
                                        Payment Reference:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPaymentRef" runat="server" Text='<%# uCase(Eval("linkedPrefix")) & Eval("paymentID") %>'></asp:Label>
                                        <asp:Label ID="lblAffID" runat="Server" Text='<%# Eval("affID") %>' Visible="false"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Eval("statementDate","{0:dd MMM yyyy}") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Amount:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblCurrencySign" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# formatNumber(cDec(Eval("statementDebit"))-cDec(Eval("statementCredit")),2) %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Reason:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblReason" runat="server" Text='<%# Eval("reason") %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:Label ID="lblUserOrderID" runat="server" Font-Bold="true"></asp:Label><br /><br /> 
                    <br /><br /><br /><br />
                    Related Statement Entrys<br />
                    <asp:GridView ID="gvEntrys" runat="server" DataSourceID="sqlEntrys" AutoGenerateColumns="false" SkinID="GridView" GridLines="none" EmptyDataText="No data found">
                        <Columns>
                            <asp:BoundField HeaderText="Date" DataField="statementDate" DataFormatString="{0:dd MMM yyy}" />
                            <asp:BoundField ItemStyle-Width="20" />
                            <asp:BoundField HeaderText="OrderID" DataField="userOrderID" />
                            <asp:BoundField ItemStyle-Width="20" />
                            <asp:BoundField HeaderText="Action" DataField="action" />
                            <asp:BoundField ItemStyle-Width="20" />
                            <asp:BoundField HeaderText="Credit" DataField="statementCredit" />
                            <asp:BoundField ItemStyle-Width="20" />
                            <asp:BoundField HeaderText="Debit" DataField="statementDebit" />
                        </Columns>
                    </asp:GridView>
                    <br /><br />
                    <b>Files related to this payment:</b><br />
                    <asp:GridView ID="gvFiles" runat="Server" EmptyDataText="None" AutoGenerateColumns="false" GridLines="none" DataSourceID="sqlFiles">
                        <Columns>
                            <asp:BoundField HeaderText="Upload Date" DataField="date" DataFormatString="{0:dd MMM yyyy}" />
                            <asp:BoundField ItemStyle-Width="60" />
                            <asp:HyperLinkField HeaderText="Filename" DataTextField="filename" DataNavigateUrlFormatString="~/uploads/{0}" DataNavigateUrlFields="filename" />
                        </Columns>
                    </asp:GridView>
                    <br /><br />
                    <b>Upload files</b><br />
                    <asp:FileUpload ID="upFile" runat="server" />&nbsp;
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_click" /><br />
                    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
                </td>
            </tr>
        </table>
        
    

        <asp:SqlDataSource ID="sqlStatementDetail" runat="Server" SelectCommand="procAffiliateStatementByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="sid" Name="statementID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlEntrys" runat="Server" SelectCommand="procAffiliateStatementPaymentByStatementIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="sid" Name="statementID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="sqlFiles" runat="server" SelectCommand="procAffStatementUploadByStatementIDUSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="sid" Name="statementID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    </center>
    </div>
    </form>
</body>
</html>
