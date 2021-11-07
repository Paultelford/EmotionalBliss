<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="returnCreate.aspx.vb" Inherits="affiliates_returnCreate" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy id="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hidOrderID" runat="server" />
            <asp:Panel ID="pan0" runat="server">
                <b>Place of purchase</b>
                <br /><br />
                <asp:RadioButtonList ID="radPurchase" runat="server" OnSelectedIndexChanged="radPurchase_selectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Text="EB Website" Value="eb"></asp:ListItem>
                    <asp:ListItem Text="Other retail outlet" Value="ext"></asp:ListItem>
                </asp:RadioButtonList>
            </asp:Panel>
            <asp:Panel ID="pan1EB" runat="server" Visible="false">
                <table border="0">
                    <tr>
                        <td colspan="3">
                            <b>Confirm order ID</b>        
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="trPan1OrderDetails" runat="server" visible="false">
                        <td colspan="3">
                            <table border="0">
                                <tr>
                                    <td colspan="3">
                                        View Order Log
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <b>Order <asp:Label ID="lblPan1UserOrderID" runat="server"></asp:Label></b>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <b>Bill To</b><br />
                                        <asp:Label ID="lblPan1BillAdd" runat="server">
                                        </asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td valign="top">
                                        <b>Ship To</b><br />
                                        <asp:Label ID="lblPan1ShipAdd" runat="server">
                                        </asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:GridView ID="gvPan1Items" runat="server" AutoGenerateColumns="false" GridLines="none">
                                            <Columns>
                                                <asp:BoundField HeaderText="Product" DataField="itemName" />
                                                <asp:BoundField ItemStyle-Width="40" />
                                                <asp:BoundField HeaderText="Qty" DataField="itemQty" />
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Items Returned:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtItemsReturned" runat="server" Width="320"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        Reason for return:
                                    </td>
                                    <td>
                                        <asp:TextBox id="txtReason" runat="server" TextMode="multiLine" CssClass="normaltextarea" Rows="4" Columns="40"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Button ID="btnPan1Procede" runat="server" Text="Proceed with Return" OnClick="btnPan1Proceed_click" /><br />
                                        &nbsp;&nbsp;OR
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Enter Order No <font size="-2">(Excl prefix)</font>:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPan1UserOrderNo" runat="server" OnTextChanged="txtPan1UserOrderNo_textChanged"></asp:TextBox><asp:Label ID="lblCountryCode" runat="server"></asp:Label>
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:RequiredFieldValidator ID="reqPan1OrderID" runat="server" ControlToValidate="txtPan1UserOrderNo" ValidationGroup="pan1" ErrorMessage="You must enter the Order ID<br>" Display="dynamic"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="ranPan1OrderID" runat="server" ControlToValidate="txtPan1UserOrderNo" ValidationGroup="pan1" MinimumValue="0" MaximumValue="9999999" ErrorMessage="Must be a 5 digit number<br />" Display="dynamic"></asp:RangeValidator>
                            <asp:Label ID="lblPan1OrderNotFound" runat="server" ForeColor="red" Text="" Visible="false"></asp:Label>
                            <asp:Button ID="btnOrderIDSubmit" runat="server" Text="Submit" OnClick="btnOrderIDSubmit_click" ValidationGroup="pan1" />
                        </td>
                        <td>&nbsp;</td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pan1Ext" runat="server" Visible="false">
                <b>Place of purchase</b>
                <br /><br />
                <table border="0">
                    <tr>
                        <td>
                            Shop/Website Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtShop" runat="server" MaxLength="50" Width="200"></asp:TextBox><asp:RequiredFieldValidator id="reqPan1ExtShop2" runat="server" ControlToValidate="txtShop" ErrorMessage="* Required" Display="dynamic" ValidationGroup="shopname"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Affilaite:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpAffiliate" runat="server" DataSourceID="sqlPan1ExtAffilaites" DataTextField="affFirstname" DataValueField="affID" AppendDataBoundItems="true">
                                <asp:ListItem Text="None" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                   <tr>
                        <td>
                            Items Returned:
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemsReturned2" runat="server" Width="320"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Reason for return:
                        </td>
                        <td>
                            <asp:TextBox id="txtReason2" runat="server" TextMode="multiLine" Rows="4" Columns="40"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnShopSubmit" runat="server" Text="Proceed with Return" OnClick="btnShopSubmit_click" ValidationGroup="shopname" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pan1Add" runat="server" Visible="false">
                <b>Customers Return Address</b>
                <br /><br />
                <span id="spnAddress">
                    <table>
                        <tr>
                            <td>
                                Customer Name:
                            </td>
                            <td>
                                <asp:TextBox ID="txtName" runat="server" MaxLength="50" Width="200"></asp:TextBox><asp:RequiredFieldValidator ID="pan1AddName" runat="server" ControlToValidate="txtName" ErrorMessage="* Required" ValidationGroup="cn" Display="dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Address:
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdd1" runat="server" MaxLength="50" Width="200"></asp:TextBox><asp:RequiredFieldValidator ID="reqPan1Add1" runat="server" ControlToValidate="txtAdd1" ErrorMessage="* Required" ValidationGroup="cn" Display="dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdd2" runat="server" MaxLength="50" Width="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdd3" runat="server" MaxLength="50" Width="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:TextBox ID="txtAdd4" runat="server" MaxLength="50" Width="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Postcode:
                            </td>
                            <td>
                                <asp:TextBox ID="txtPostcode" runat="server" MaxLength="50" Width="200"></asp:TextBox><asp:RequiredFieldValidator ID="reqPan1Postcode" runat="server" ControlToValidate="txtPostcode" ErrorMessage="* Required" ValidationGroup="cn" Display="dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Country:
                            </td>
                            <td>
                                <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="sqlPan1AddCountry" DataTextField="countryName" DataValueField="countryCode" OnLoad="drpCountry_Load">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Email:
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50" Width="200"></asp:TextBox><asp:RequiredFieldValidator ID="reqPan1AddEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="* Required" ValidationGroup="cn" Display="dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Phone:
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="20" Width="200"></asp:TextBox><asp:RequiredFieldValidator ID="reqPan1AddPhone" runat="server" ControlToValidate="txtPhone" ErrorMessage="* Required" ValidationGroup="cn" Display="dynamic"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>&nbsp;</td>
                            <td>
                                <asp:Button ID="btnPan1AddSubmit" runat="server" Text="Create Return" OnClick="btnPan1AddSubmit_Click" ValidationGroup="cn" />
                            </td>
                        </tr>
                    </table>
                </span>
            </asp:Panel>
             <asp:Panel ID="pan3" runat="server" Visible="false">
                <b>Return Created</b>
                <br /><br />
                The return number is <asp:HyperLink ID="lnkReturnsNo" runat="server" Font-Bold="true"></asp:HyperLink>
                <br /><br />
                This part of the returns process is now complete.<br />
            </asp:Panel>
            <asp:Panel ID="pan2" runat="server" Visible="false">
                <table border="0" width="100%">
                    <tr>
                        <td valign="top">
                            <table border="0">
                                <tr>
                                    <td colspan="3">
                                        <b>Items to Return</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Table ID="tblPan2Items" runat="server" Visible="false">
                                            <asp:TableRow>
                                                <asp:TableCell Font-Bold="true">
                                                    Name
                                                </asp:TableCell>
                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                <asp:TableCell Font-Bold="true">
                                                    Qty
                                                </asp:TableCell>
                                                <asp:TableCell>&nbsp;</asp:TableCell>
                                                <asp:TableCell Font-Bold="true">
                                                    No. Returned
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                        
                                        <asp:GridView ID="gvPan2Items" runat="server" AutoGenerateColumns="false" Width="100%" GridLines="none" Visible="true">
                                            <Columns>
                                                <asp:BoundField HeaderText="Name" DataField="itemname" />
                                                <asp:BoundField ItemStyle-Width="40" />
                                                <asp:BoundField HeaderText="Qty" DataField="itemqty" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk" runat="server" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="No. Returned">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtQtyReturn" runat="server" Width="40" MaxLength="2"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblError" runat="server" ForeColor="red" Text="* Enter a value"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnPan2Proceed" runat="server" Text="Proceed with Return" OnClick="btnPan2Proceed_click" />
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        <asp:Button ID="btnPan2AddItem" runat="server" Text="Add Other Item" OnClick="btnPan2AddItem_click" Visible="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <asp:GridView ID="gvPan2ProductList" runat="server" DataSourceID="sqlPan2ProductList" Visible="false" AutoGenerateColumns="false" OnSelectedIndexChanged="gvPan2ProductList_selectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnItemAdd" runat="server" CommandName="select" Text='<%# Eval("affProductName") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Type" DataField="affProductType" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlPan2ProductList" runat="server" SelectCommand="procAffiliateProductBuyingByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" >
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlPan1ExtAffilaites" runat="Server" SelectCommand="procAffiliatesByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlPan1AddCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

