<%@ Page Language="VB" AutoEventWireup="false" CodeFile="statementReceipt.aspx.vb" Inherits="affiliates_statementReceipt" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <center>
    <div>
        <asp:Image ID="imgLetterHead" runat="server" ImageUrl="~/images/emotional bliss banner.jpg" /><br />
        <br /><br /><br /><br /><br />
        <table>
            <tr>
                <td align="left" valign="top">
                    <asp:FormView ID="fvAffiliate" runat="server" DataSourceID="sqlAffiliate" GridLines="none" OnDataBound="fvAffiliate_dataBound" Width="100%">
                        <ItemTemplate>
                            <asp:Label ID="lblCompany" Font-Size="medium" runat="server" Text='<%# Eval("affCompany") %>'></asp:Label><br />
                            <asp:Label ID="lblAdd1" runat="server" Font-Size="medium" Text='<%# Eval("affAdd1") %>'></asp:Label>
                            <asp:Label ID="lblAdd2" runat="server" Font-Size="medium" Text='<%# Eval("affAdd2") %>'></asp:Label>
                            <asp:Label ID="lblAdd3" runat="server" Font-Size="medium" Text='<%# Eval("affAdd3") %>'></asp:Label>
                            <asp:Label ID="lblAdd4" runat="server" Font-Size="medium" Text='<%# Eval("affAdd4") %>'></asp:Label>
                            <asp:Label ID="lblAdd5" runat="server" Font-Size="medium" Text='<%# Eval("affAdd5") %>'></asp:Label>
                            <asp:Label ID="lblPostcode" runat="server" Font-Size="medium" Text='<%# uCase(Eval("affPostcode")) %>'></asp:Label><br />
                            <asp:Label ID="lblCountry" runat="server" Font-Size="medium" Text='<%# Eval("countryName") %>'></asp:Label><br />
                            <asp:Label ID="lblAffID" runat="server" Text='<%# Eval("affID") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblCountryCode" runat="server" Text='<%# Eval("countryCode") %>' Visible="false"></asp:Label>
                            <br /><br />
                            <hr class="blueline" />
                            <br /><br />
                        </ItemTemplate>
                    </asp:FormView>
                    <asp:FormView ID="fvDetails" runat="server" DataSourceID="sqlStatementDetail"  OnDataBound="fvDetails_dataBound">
                        <ItemTemplate>
                            <table border="0">
                                <tr>
                                    <td>
                                        Payment Reference:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPaymentRef" runat="server" Text='<%# uCase(Eval("linkedPrefix")) & Eval("linkedRef") %>'></asp:Label>
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
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# Session("EBAffCurrencySign") & formatNumber(cDec(Eval("statementDebit"))-cDec(Eval("statementCredit")),2) %>'></asp:Label>
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
                    <asp:Panel ID="panRelated" runat="server">
                        Related Statement Entrys<br />
                        <asp:GridView ID="gvEntrys" runat="server" DataSourceID="sqlEntrys" AutoGenerateColumns="false" SkinID="GridView" GridLines="none" OnDataBound="gvEntrys_dataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Date" DataField="statementDate" DataFormatString="{0:dd MMM yyy}" HeaderStyle-Font-Size="Small" ItemStyle-Font-Size="Small" />
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:BoundField HeaderText="OrderID" DataField="userOrderID" HeaderStyle-Font-Size="Small" ItemStyle-Font-Size="Small" />
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:BoundField HeaderText="Action" DataField="action" HeaderStyle-Font-Size="Small" ItemStyle-Font-Size="Small" />
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:BoundField HeaderText="Credit" DataField="statementCredit" HeaderStyle-Font-Size="Small" ItemStyle-Font-Size="Small" />
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:BoundField HeaderText="Debit" DataField="statementDebit" HeaderStyle-Font-Size="Small" ItemStyle-Font-Size="Small" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <br /><br />
                    Related documents:<br />
                    <asp:Panel ID="panFiles" runat="server">
                    </asp:Panel><br /><br />
                    <asp:Panel id="panUpload" runat="server">
                        <table>
                            <tr>
                                <td>
                                    Upload a file:
                                </td>
                                <td>
                                    <asp:FileUpload ID="fu1" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_click" />
                                </td>
                            </tr>
                        </table>                         
                    </asp:Panel>
                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                    <br /><br />
                    <center>
                    Emotional Bliss<br />
                    22 Carlisle St<br />
                    Goole<br />
                    DN14 5DS<br /><br />
                    Emotional Bliss is a trading name of Pear Tree UK Ltd<br /> Registered in England No. 04080067&nbsp;&nbsp;&nbsp;&nbsp; VAT No. 764 4520 26
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>    
                </td>
            </tr>
        </table>

        <asp:SqlDataSource ID="sqlAffiliate" runat="server" SelectCommand="procAffiliateClickThroughByStatementIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="sid" Name="statementID" Type="Int32" />
            </SelectParameters>
        </asp:SqlDataSource>    
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
    </div>
    </form>
</body>
</html>
