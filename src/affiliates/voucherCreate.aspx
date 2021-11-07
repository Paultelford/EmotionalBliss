<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="voucherCreate.aspx.vb" Inherits="affiliates_voucherCreate" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="lblHeader" runat="server" text="Create Voucher"></asp:Label>
    <br /><br />
    <atlas:ScriptManagerProxy ID="smp" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panCreate" runat="server">
                <table border="0">
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="drpType1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpType1_selectedIndexChanged">
                                <asp:ListItem Text="Single use coupon" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Multiple use coupon" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>Voucher (Amount)</td>
                        <td>
                            <asp:TextBox ID="txtAmount" runat="Server" Width="100"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqTxtAmount" runat="server" Display="dynamic" ControlToValidate="txtAmount" ErrorMessage=" * Required"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="ranTxtAmount" runat="server" ControlToValidate="txtAmount" MinimumValue="0" MaximumValue="500" ErrorMessage=" * Invalid" Type="Double"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Sender Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtSenderName" runat="server" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtSenderName" runat="server" ControlToValidate="txtSenderName" ErrorMessage=" * Required" Display="dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Recipient Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtRecipientName" runat="server" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtRecipientName" runat="server" ControlToValidate="txtRecipientName" ErrorMessage=" * Required" Display="dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Sender Email:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailSender" runat="server" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtEmailSender" runat="server" ControlToValidate="txtEmailSender" ErrorMessage=" * Required" Display="dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="regTxtEmailSender" runat="server" ControlToValidate="txtEmailSender" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" ErrorMessage=" * Invalid email address"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Recipient Email:
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage=" * Required" Display="dynamic"></asp:RequiredFieldValidator><asp:RegularExpressionValidator ID="regTxtEmail" runat="server" ControlToValidate="txtEmail" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" ErrorMessage=" * Invalid email address"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Comment:
                        </td>
                        <td>
                            <asp:TextBox id="txtComment" runat="server" TextMode="MultiLine" Rows="5" Columns="60"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Products coupon can be used on:
                        </td>
                        <td>
                            <asp:GridView ID="gvProducts" runat="server" DataSourceID="sqlDistProducts" AutoGenerateColumns="false" DataKeyNames="id" OnDataBound="gvProducts_dataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="Product" DataField="saleName" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Type" DataField="affProductType" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Unit Price" DataField="saleUnitPrice" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Tax Rate" DataField="saleTaxRate" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:TemplateField HeaderText="Buyable">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBuyable" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="Create Voucher" OnClick="btnSubmit_click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panCreateMulti" runat="server" Visible="false">
                <table border="0">
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="drpType2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpType2_selectedIndexChanged">
                                <asp:ListItem Text="Single use voucher" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Multiple use voucher" Value="1"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Voucher Amount (<asp:Label ID="lblCurrencySign3" runat="server"></asp:Label>)
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmount2" runat="Server" Width="100"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtAmount2" runat="server" Display="dynamic" ControlToValidate="txtAmount2" ErrorMessage=" * Required"></asp:RequiredFieldValidator><asp:RangeValidator ID="ranTxtAmount2" runat="server" ControlToValidate="txtAmount2" MinimumValue="0" MaximumValue="500" ErrorMessage=" * Invalid" Type="Double"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Is Percent:
                        </td>
                        <td><asp:CheckBox ID="chkIsPercent" runat="server" /></td>
                    </tr>
                    <tr>
                        <td>
                            Associate Affiliate With Voucher:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpAffilaite" runat="server" AppendDataBoundItems="true" DataSourceID="sqlAffiliates" DataTextField="affCompany" DataValueField="affID">
                                <asp:ListItem Text="None" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Description:
                        </td>
                        <td>
                            <asp:TextBox id="txtCommentMulti" runat="server" TextMode="MultiLine" Rows="5" Columns="60"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            End Date:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpDay" runat="server">
                            </asp:DropDownList>&nbsp;
                            <asp:DropDownList ID="drpMonth" runat="server">
                            </asp:DropDownList>&nbsp;
                            <asp:DropDownList ID="drpYear" runat="server">
                            </asp:DropDownList>
                            <asp:Label ID="lblErrorMulti" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Set Active:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpActive" runat="server">
                                <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                                <asp:ListItem Text="No" Value="false"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Products coupon can be used on:
                        </td>
                        <td>
                            <asp:GridView ID="gvProducts1" runat="server" DataSourceID="sqlDistProducts" AutoGenerateColumns="false" DataKeyNames="id" OnDataBound="gvProducts_dataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="Product" DataField="saleName" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Type" DataField="affProductType" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Unit Price" DataField="saleUnitPrice" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Tax Rate" DataField="saleTaxRate" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:TemplateField HeaderText="Buyable">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBuyable" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnSubmitMulti" runat="server" Text="Create Voucher" OnClick="btnSubmitMulti_click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panComplete" runat="server" Visible="false">
                The voucher has been created and emailed to the recipient.
                <br /><br />
                Voucher No. <asp:Label ID="lblVoucherNumber" runat="server"></asp:Label> (<asp:Label ID="lblCurrencySign2" runat="server"></asp:Label><asp:Label ID="lblAmount" runat="server"></asp:Label>)
            </asp:Panel>
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    <asp:SqlDataSource ID="sqlAffiliates" runat="Server" SelectCommand="procAffiliatesByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDistProducts" runat="server" SelectCommand="procAffPOSByDistIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffID" Name="distID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

