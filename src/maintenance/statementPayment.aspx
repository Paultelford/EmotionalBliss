<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="statementPayment.aspx.vb" Inherits="maintenance_statementPayment" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    
            <asp:Table ID="tblStatementMain" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="top">
                        View Statements / Make Payemnts
                        <br /><br />
                        <table border="0">
                            <tr id="trAffiliates" runat="server" visible="false">
                                <td>
                                    Affiliates:
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpAff" runat="server" DataSourceID="sqlAffiliates" DataTextField="affName" DataValueField="affID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpAff_selectedIndexChanged">
                                        <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>   
                            </tr>
                            <tr>
                                <td>
                                    Distributors:
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpDist" runat="server" DataSourceID="sqlDistributors" DataTextField="affName" DataValueField="affID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpDist_selectedIndexChanged">
                                        <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>   
                            </tr>
                        </table>
                        <br />
                        <asp:LinkButton id="btnTransaction" runat="server" Text="Make Transaction" OnClick="btnTransaction_click"></asp:LinkButton><br />
                        <asp:Label ID="lblTransactionUserError" runat="server" ForeColor="red"></asp:Label>
                        <table border="0" id="tblTransaction" runat="server" visible="false">
                            <tr>
                                <td>
                                    Amount:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAmount" runat="server" Width="80"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="reqAmount" runat="server" ControlToValidate="txtAmount" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator><asp:RangeValidator ID="ranAmount" runat="server" ControlToValidate="txtAmount" MinimumValue="0.01" MaximumValue="999999" ErrorMessage="* Invalid Amount" Display="dynamic"></asp:RangeValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Currency:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCurrency" runat="server" DataSourceID="sqlCurrency" DataTextField="currency" DataValueField="currency">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Transaction Type:
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="radType" runat="server" OnSelectedIndexChanged="radType_selectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Credit" Value="5" Selected></asp:ListItem>
                                        <asp:ListItem Text="Debit" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="Payment(In)" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="Payment(Out)" Value="8"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Transaction Date:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Cheque Number:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCheque" runat="server" Width="175" MaxLength="30"></asp:TextBox><br /><asp:Label ID="lblChequeMsg" runat="server" Text="Cheque payments only" Font-Size="Smaller"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    Reason:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="4" Columns="20"></asp:TextBox>
                                </td>                               
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:Button ID="btnTransfer" runat="server" Text="Transfer Funds" OnClick="btnTransfer_click" />
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    Select the transactions to be included in this <br />
                                    Payment by selecting the checkboxes in the<br />
                                    table to the right:
                                </td>
                            </tr>
                        </table>
                        
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="top">
                        <asp:Panel ID="panAffStatement" runat="server" Visible="false" BorderWidth="1">
                            <table border="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <asp:label ID="lblAffiliateHeader" runat="server" Font-Size="Large" Font-Bold="true"></asp:label><br />
                                        <asp:Label ID="lblAffiliateName" runat="server" Font-Size="Large"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <eb:DateControl id="dateAff" runat="server"></eb:DateControl>
                                        <asp:Label ID="lblStatementCurrencyText" runat="server" Text="Curreny: ">
                                        </asp:Label><asp:DropDownList ID="drpStatementCurrency" runat="server" DataSourceID="sqlStatementCurrency" AutoPostBack="true" DataTextField="currency" DataValueField="currency" OnDataBound="drpStatementCurrency_dataBound"></asp:DropDownList>    
                                    </td>
                                </tr>
                            </table>
                            
                            <asp:GridView id="gvStatement" runat="server" Width="100%" DataSourceID="sqlStatement" DataKeyNames="statementID" AutoGenerateColumns="false" SkinID="GridView" ShowFooter="true" OnDataBound="gvStatement_dataBound"> 
                                <Columns>
                                    <asp:TemplateField HeaderText="Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("statementDate")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:TemplateField HeaderText="Order ID">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrderID" runat="server" Text='<%# showOrderID(Eval("extOrderID"),Eval("extUserOrderID"),Eval("orderPrefix"),Eval("newOrderID"),Eval("orderCountryCode")) %>'></asp:Label>
                                            <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("actionID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblStatementID" runat="server" Text='<%# Eval("statementID") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("orderID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:BoundField HeaderText="Action" DataField="action" />
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:BoundField HeaderText="PaymentRef" DataField="paymentRef" NullDisplayText="*" />
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:TemplateField HeaderText="Credit">
                                        <ItemStyle HorizontalAlign="right" />
                                        <FooterStyle HorizontalAlign="right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrencySign1" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblCredit" runat="server" Text='<%# Eval("statementCredit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            crF
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:TemplateField HeaderText="Debit">
                                        <ItemStyle HorizontalAlign="right" />
                                        <FooterStyle HorizontalAlign="right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrencySign2" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblDebit" runat="server" Text='<%# Eval("statementDebit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            drF
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Add" Visible="false">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAdd" runat="server" />
                                            <asp:Label ID="lblPaymentRef" runat="server" Visible="false" Text='<%# Eval("paymentPrefixAndID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Balance">
                                        <ItemStyle HorizontalAlign="right" />
                                        <FooterStyle HorizontalAlign="right" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrencySign3" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblBalance" runat="server" Text='<%# Eval("balance") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:Label ID="lblTest" runat="server"></asp:Label>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        

    <asp:SqlDataSource ID="sqlCurrency" runat="server" SelectCommand="procCurrencyByAffIDDefaultSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpDist" PropertyName="selectedValue" Name="affID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatementCurrency" runat="server" SelectCommand="procTransactionByAffIDDistinctCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpDist" PropertyName="selectedValue" Name="affID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlAffiliates" runat="server" SelectCommand="procAffilaitesByIsDistSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:Parameter Name="isDist" DefaultValue="false" Type="boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDistributors" runat="server" SelectCommand="procAffilaitesByIsDistSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:Parameter Name="isDist" DefaultValue="true" Type="boolean" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatement" runat="server" SelectCommand="procAffiliateEBStatementByAffIDSelect2" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="dateAff" PropertyName="getStartDate" name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="dateAff" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            <asp:ControlParameter ControlID="drpStatementCurrency" PropertyName="selectedValue" Name="currency" Type="String" Size="3" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

