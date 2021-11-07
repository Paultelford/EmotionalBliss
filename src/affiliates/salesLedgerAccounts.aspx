<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="salesLedgerAccounts.aspx.vb" Inherits="affiliates_salesLedgerAccounts" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy id="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>

            <asp:Panel ID="panLog" runat="server">
                <table>
                    <tr>
                        <td align="center">
                            <h2>Sales Ledger&nbsp;</h2>                            
                            <span style="display: none;">
                            <asp:Label ID="lblCountryCode" runat="server" font-bold="true"></asp:Label>&nbsp;/
                            <asp:Label ID="lblCurrency" runat="server" Font-Bold="true"></asp:Label>
                            </span>
                            <br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table border="0">
                                <tr>                           
                                    <td valign="top">
                                        <asp:Label ID="lblDateError" runat="server" ForeColor="Red"></asp:Label>
                                        <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl>        
                                    </td>
                                    <td width="40">&nbsp;</td>
                                    <td valign="bottom">
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    Search By:                                                    
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpSearchOn" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCountry_selectedIndexChanged">
                                                        <asp:ListItem Text="Scan Date" Value="statementDate"></asp:ListItem>
                                                        <asp:ListItem Text="Payment Date" Value="paymentDate"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblTypeText" runat="server" Text="Type:"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true" DataTextField="orderType" DataValueField="orderType" DataSourceID="sqlOrderTypes" AppendDataBoundItems="true" OnSelectedIndexChanged="drpType_selectedIndexChanged" OnDataBinding="drpType_dataBinding">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Country:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpCountry" runat="server" AutoPostBack="true" DataTextField="countryName" DataValueField="countryCode" DataSourceID="sqlCountry" AppendDataBoundItems="false" OnDataBound="drpCountry_dataBound" OnSelectedIndexChanged="drpCountry_selectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Currency:
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="drpCurrency" runat="server" AutoPostBack="true" DataTextField="currencyCodeUpper" DataValueField="currencyCode" DataSourceID="sqlCurrency" AppendDataBoundItems="true" OnSelectedIndexChanged="drpCurrency_selectedIndexChanged">
                                                        <asp:ListItem Text="All" Value="%"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="bottom">
                                        <asp:HyperLink id="lnkShowComplete" runat="server" Text="Show Complete Orders" Visible="false" Target="_blank" />
                                    </td>
                                </tr>
                            </table>
                            <br /><br />
                            <asp:GridView ID="gvSalesLedger" runat="server" ShowFooter="true" DataKeyNames="day" DataSourceID="sqlSalesLedger" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" EmptyDataText="No orders found." width="100%">
                                <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                            <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                        </ItemTemplate>
                                        <FooterStyle  HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-Width="40" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRowCurrencySign" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                    <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                    <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
<!-- Multi Gridviews -->                    
                            <asp:Panel ID="panGridviews" runat="server">
                                <asp:Label ID="lblCountry" runat="server" Font-Bold="true" Font-Size="Large"></asp:Label><br />
                                <asp:Label ID="lblAMEX" runat="server" Text="<br />AMEX" Visible="false"></asp:Label>
                                <asp:GridView ID="gvAMEX" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblAccount" runat="server" Text="<br />Account" Visible="false"></asp:Label>
                                <asp:GridView ID="gvAccount" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblAffAccount" runat="server" Text="<br />AffAccount" Visible="false"></asp:Label>
                                <asp:GridView ID="gvAffAccount" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblAffCC" runat="server" Text="<br />Aff CC" Visible="false"></asp:Label>
                                <asp:GridView ID="gvAffCC" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblCC" runat="server" Text="<br />CC" Visible="false"></asp:Label>
                                <asp:GridView ID="gvCC" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblCheque" runat="server" Text="<br />Cheque" Visible="false"></asp:Label>
                                <asp:GridView ID="gvCheque" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblDDebit" runat="server" Text="<br />DDebit" Visible="false"></asp:Label>
                                <asp:GridView ID="gvDDebit" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblDistAccount" runat="server" Text="<br />Dist Account" Visible="false"></asp:Label>
                                <asp:GridView ID="gvDistAccount" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblDistCC" runat="server" Text="<br />Dist CC" Visible="false"></asp:Label>
                                <asp:GridView ID="gvDistCC" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblFastpay" runat="server" Text="<br />Fastpay" Visible="false"></asp:Label>
                                <asp:GridView ID="gvFastpay" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblGiro" runat="server" Text="<br />Giro" Visible="false"></asp:Label>
                                <asp:GridView ID="gvGiro" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblIdeal" runat="server" Text="<br />iDeal" Visible="false"></asp:Label>
                                <asp:GridView ID="gvIdeal" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                
                                <asp:Label ID="lblPhone" runat="server" Text="<br />Phone" Visible="false"></asp:Label>
                                <asp:GridView ID="gvPhone" runat="server" ShowFooter="true" DataKeyNames="day" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSalesLedger_selectedIndexChanged" SkinID="GridView" OnDataBound="gvSalesLedger_databound" width="100%">
                                    <FooterStyle Font-Bold="true" HorizontalAlign="right" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Day" ItemStyle-Width="120">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkDay" runat="server" CommandName="select" Text='<%# showDate(Eval("day")) %>'></asp:LinkButton>
                                                <asp:Label ID="lblActionID" runat="Server" Text='<%# Eval("ledgerActionID") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <FooterStyle  HorizontalAlign="Center" />
                                            <FooterTemplate>
                                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Chg./Ref" DataField="items" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Credit/Debit" DataField="balance" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Cheque" DataField="cheque" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate cc" DataField="affiliatecc" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Affiliate acc" DataField="affiliateaccount" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:TemplateField HeaderText="Goods Total" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblGoodsTotal" runat="server" Text="0.00"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField ItemStyle-Width="40" />
                                        <asp:BoundField HeaderText="Vat" DataField="vat" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" Visible="false" />
                                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                                        <asp:TemplateField HeaderText="Total (Inc Vat)" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalIncVat" runat="Server" Text='<%# formatNumber(Eval("balance") + Eval("vat") + Eval("affiliatecc") + Eval("affiliateaccount") + Eval("cheque"),2) %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <table>
                                <tr>
                                    <td valign="top">
                                        <asp:Panel ID="panDynamicContent" runat="server">
                                        </asp:Panel>
                                    </td>
                                    <td width="100">&nbsp;</td>
                                    <td valign="top">
                                        <br /><br />
                                        <asp:Table ID="tblCurrency" runat="server" Visible="false"></asp:Table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panDetails" runat="server" Visible="false">
                <asp:LinkButton ID="lnkBack" runat="Server" Text="Back" OnClick="lnkBack_click" Font-Bold="true"></asp:LinkButton><br />
                <asp:Label ID="lblDay" runat="Server" Font-Bold="true"></asp:Label><br /><br />
                Note: Affiliate Account orders are highlighted in red
                <asp:GridView ID="gvSalesLedgerDay" runat="server" datasourceid="sqlSalesLedgerDay" AutoGenerateColumns="false" SkinID="GridView" ShowFooter="true" OnDataBound="gvSalesLedgerDay_dataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Order ID">
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkUserOrderID" runat="server" Text='<%# Eval("userOrderID") %>' NavigateUrl='<%# "~/affiliates/orderView.aspx?id=" & Eval("orderID") %>'></asp:HyperLink>
                                <asp:Label ID="lblOrderType" runat="server" Text='<%# Eval("orderType") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalText" runat="server" Text="Total" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Total(Ex Vat)" DataField="balance" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Vat" DataField="vat" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Affiliate" DataField="affCompany" />
                        <asp:BoundField ItemStyle-Width="90" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="Tracker" DataField="trackerCode" NullDisplayText="N/A" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Label ID="lblError" runat="server"></asp:Label>
 
    
    <asp:SqlDataSource ID="sqlOrderTypes" runat="server" SelectCommand="procShopOrderTypeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSalesLedger" runat="server" SelectCommand="procSalesLedgerByPaymentDateSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            <asp:ControlParameter ControlID="drpCountry" PropertyName="selectedValue" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="type" Type="string" Size="50" />
            <asp:ControlParameter ControlID="drpSearchOn" PropertyName="selectedValue" Name="searchOn" Type="String" Size="20" />
            <asp:ControlParameter ControlID="drpCurrency" PropertyName="selectedValue" Name="currency" Type="String" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSalesLedgerDay" runat="server" SelectCommand="procSalesLedgerByPaymentDaySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="sqlSalesLedgerDay_selecting">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpCountry" PropertyName="selectedValue" Name="countryCode" Type="string" Size="5" />
            <asp:Parameter name="type" Type="String" Size="50" />
            <asp:Parameter Name="actionID" Type="int16" />
            <asp:Parameter Name="ledgerDay" Type="DateTime" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

