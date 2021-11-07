<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="transactions.aspx.vb" Inherits="affiliates_transactions" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:label id="lblTransactionUserError" runat="server" ForeColor="red"></asp:label>
            <table>
                <tr>
                    <td>
                        <eb:DateControl id="date1" runat="server"></eb:DateControl>        
                        <asp:Panel ID="panCurrency" runat="server">
                            Curreny: <asp:DropDownList ID="drpCurrency" runat="server" DataSourceID="sqlCurrency" AutoPostBack="true" DataTextField="currency" DataValueField="currency" OnDataBound="drpCurrency_dataBound"></asp:DropDownList>
                        </asp:Panel>
                    </td>
                    <td width="40">&nbsp;</td>
                    <td id="tdView" runat="server" valign="bottom" align="right">
                        <asp:DropDownList ID="drpView" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpView_selectedIndexChanged" Visible="false">
                            <asp:ListItem Text="All Orders/Transactions" Value="0"></asp:ListItem>
                            <asp:ListItem Text="EB Transactions" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Affiliate Orders" Value="2" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Customer Orders" Value="3"></asp:ListItem>
                        </asp:DropDownList><br />
                        <asp:DropDownList ID="drpAffiliate" runat="server" DataSourceID="sqlAffiliates" DataTextField="affName" DataValueField="affID" AppendDataBoundItems="true" AutoPostBack="true" Visible="false" OnSelectedIndexChanged="drpAffiliate_selectedIndexChanged">
                            <asp:ListItem Text="All Affiliates" Value="%"></asp:ListItem>
                        </asp:DropDownList><br />
                        <asp:LinkButton ID="lnkMakeTransaction" runat="server" Text="Make Transaction" OnClick="lnkMakeTransaction_click" Visible="false"></asp:LinkButton>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td colspan="2">
                        <b>Website ClickThroughs</b><br />
                        <table border="0">
                            <tr>
                                <td>
                                    Number of Days:
                                </td>
                                <td width="20">&nbsp;</td>
                                <td>
                                    <asp:Label ID="lblClickDate" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Visitors:
                                </td>
                                <td width="20">&nbsp;</td>
                                <td>
                                    <asp:Label ID="lblClickTotal" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <td id="Td1" runat="server" visible="false">
                            <asp:DropDownList ID="drpGiveTake" runat="server">
                                <asp:ListItem Text="Give/Credit" Value="9"></asp:ListItem>
                                <asp:ListItem Text="Take/Debit" Value="10"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="txtCredits" runat="server" MaxLength="10" ValidationGroup="affCredit"></asp:TextBox>&nbsp;
                            <asp:RequiredFieldValidator ID="reqTxtCredits" runat="server" ErrorMessage="* Required" Display="Dynamic" ControlToValidate="txtCredits" ValidationGroup="affCredit"></asp:RequiredFieldValidator><br />
                            <asp:Label ID="lblToFrom" runat="server" Text="To:"></asp:Label>
                            <asp:DropDownList ID="drpAffiliates" runat="server" DataSourceID="sqlAffiliates" AppendDataBoundItems="true" AutoPostBack="false" DataTextField="affName" DataValueField="affID"></asp:DropDownList>&nbsp;
                            <asp:RequiredFieldValidator ID="reqDrpAffiliates" runat="server" ControlToValidate="drpAffiliates" ErrorMessage="* Required" Display="dynamic" ValidationGroup="affCredit"></asp:RequiredFieldValidator><br />
                            <asp:Button ID="btnAffCreditSubmit" runat="server" Text="Make Transaction" OnClick="btnAffCreditSubmit_click" />
                        </td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:GridView id="gvStatement" runat="server" DataKeyNames="orderID" DataSourceID="sqlStatement" AutoGenerateColumns="false" SkinID="GridView" ShowFooter="true" Width="100%" AllowPaging="true" PageSize="20" PagerSettings-Visible="false"> 
                            <Columns>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("statementDate")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:TemplateField HeaderText="Order ID">
                                    <ItemTemplate>
                                        <asp:Label ID="lblOrderID" runat="server" Visible="false" Text='<%# showOrderID(Eval("extOrderID"),Eval("extUserOrderID"),Eval("orderPrefix"),Eval("newOrderID"),Eval("orderCountryCode")) %>'></asp:Label>
                                        <asp:HyperLink ID="lnkInvoice" runat="server" Target="_blank" Text='<%# showOrderID(Eval("extOrderID"),Eval("extUserOrderID"),Eval("orderPrefix"),Eval("newOrderID"),Eval("orderCountryCode")) %>' NavigateUrl='<%# "statementInvoice.aspx?id=" & Eval("id") %>'></asp:HyperLink>
                                        <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("actionID") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                                        <asp:Label ID="lblStatementID" runat="server" Text='<%# Eval("statementID") %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkAction" runat="server" Target="_blank" Text='<%# Eval("action") %>' NavigateUrl='<%# makeInvoiceURL(Eval("actionID"),Eval("orderID")) %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:TemplateField HeaderText="Affiliate" Visible="True">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAffiliate" runat="server" Text='<%# Eval("affName") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Paymet Receipt" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaymentReceipt" runat="server" Text='<%# Eval("vendorTxCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:TemplateField HeaderText="Tacker Info">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkTracker" runat="Server" Text='' Target="_blank" NavigateUrl='<%# "http://track.dhl.co.uk/tracking/wrd/run/wt_xhistory_pw.execute?PCL_NO=#&PCL_INST=1&COLLDATE=&CNTRY=" & uCase(Session("EBAffEBDistributorCountryCode")) %>'></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:BoundField HeaderText="PaymentRef" DataField="paymentRef" NullDisplayText="*" />
                                <asp:BoundField ItemStyle-Width="20" />
                                
                                <asp:TemplateField HeaderText="Credit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencySign1" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblCredit" runat="server" Text='<%#Eval("statementCredit")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        crF
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="20" />
                                <asp:TemplateField HeaderText="Debit">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencySign2" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblDebit" runat="server" Text='<%#Eval("statementDebit")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        crF
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Add" Visible="false">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkAdd" runat="server" />
                                        <asp:Label ID="lblPaymentRef" runat="server" Visible="false" Text='<%# Eval("paymentPrefixAndID") %>'></asp:Label>
                                        <asp:Label ID="lblPaymentID" runat="server" Text='<%# Eval("paymentID") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Balance">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencySign3" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblBalance" runat="server" Text='<%#Eval("balance")%>'></asp:Label>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        crF
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <eb:Pager id="pager1" runat="server" showTextBox="true"></eb:Pager>
                    </td>
                </tr>
            </table>    
            <asp:Panel ID="panTrans" runat="Server" Visible="false">
                <table border="0" id="tblTransaction" runat="server" visible="true">
                    <tr>
                        <td>
                            Amount(£):
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="server" Width="80" ValidationGroup="trans"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="reqAmount" runat="server" ControlToValidate="txtAmount" ErrorMessage="* Required" Display="dynamic" ValidationGroup="trans"></asp:RequiredFieldValidator><asp:RangeValidator ID="ranAmount" runat="server" ControlToValidate="txtAmount" MinimumValue="0.01" MaximumValue="999999" ErrorMessage="* Invalid Amount" Display="dynamic" ValidationGroup="trans"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Transaction Date:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransDate" runat="server" MaxLength="30" ValidationGroup="trans"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="reqTxtTransDate" runat="Server" ControlToValidate="txtTransDate" ErrorMessage="* Required" Display="dynamic" ValidationGroup="trans"></asp:RequiredFieldValidator>
                            <asp:Label ID="lblTransDateError" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Transaction Type:
                        </td>
                        <td>
                            <asp:RadioButtonList ID="radType" runat="server" OnSelectedIndexChanged="radType_selectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Credit" Value="9" Selected></asp:ListItem>
                                <asp:ListItem Text="Debit" Value="10"></asp:ListItem>
                                <asp:ListItem Text="Payment(In)" Value="11"></asp:ListItem>
                                <asp:ListItem Text="Payment(Out)" Value="12"></asp:ListItem>
                            </asp:RadioButtonList>
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
                            <asp:Button ID="btnTransfer" runat="server" Text="Transfer Funds" OnClick="btnTransfer_click" ValidationGroup="trans"/>
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
            </asp:Panel>
            <asp:Label ID="lblTemp" runat="server" Text=""></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel> 

    <asp:SqlDataSource ID="sqlCurrency" runat="server" SelectCommand="procTransactionByAffIDDistinctCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatement" runat="server" SelectCommand="procAffiliateStatementByAffIDSelect2" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            <asp:ControlParameter ControlID="drpCurrency" PropertyName="selectedValue" Name="currency" Type="String" Size="3" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatementOld" runat="server" SelectCommand="procAffiliateStatementByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatementAff" runat="server" SelectCommand="procAffiliateAffStatementByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBaffID" Name="distID" Type="int32" />
            <asp:ControlParameter ControlID="drpAffiliate" Name="affID" Type="string" Size="5" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatementAffOnly" runat="server" SelectCommand="procAffiliateAffStatementByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:Parameter Name="distID" DefaultValue="0" Type="int32" />
            <asp:SessionParameter SessionField="EBAffID" Name="affID" Type="string" Size="5" />
            <asp:SessionParameter SessionField="EBAffCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlAffiliates" runat="server" SelectCommand="procAffiliatesByDistIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>

