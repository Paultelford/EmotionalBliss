<%@ Page Title="Emails" Language="VB" MasterPageFile="~/maffs.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="emails.aspx.vb" Inherits="affiliates_emails" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false" AssociatedUpdatePanelID="update1">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>    
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            Edit existing email:&nbsp;
            <asp:DropDownList ID="drpEmails" runat="server" DataSourceID="sqlEmails" AutoPostBack="true" DataTextField="emailType" DataValueField="emailMasterID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpEmails_selectedIndexChanged">
                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Button ID="btnAdd" runat="server" Text="Clear Form" OnClick="btnAdd_click" />
            <br /><br />
            <asp:Panel ID="panEmail">
                <table width="100%">
                    <tr>
                        <td valign="top">
                            <asp:FormView ID="fvEmail" runat="server" DefaultMode="Insert">
                                <InsertItemTemplate>
                                    <table>
                                        <tr runat="server" visible="false">
                                            <td>
                                                Email Name:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="30" ValidationGroup="add" Width="200" Enabled="false"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqTxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="* Required" Display="Static" ValidationGroup="add"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Subject:
                                            </td>
                                            <td>
                                                <asp:TextBox id="txtSubject" runat="server" MaxLength="100" ValidationGroup="add" Width="300"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqTxtSubject" runat="server" ControlToValidate="txtSubject" ErrorMessage="* Required" Display="Static" ValidationGroup="add"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                Body:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBody" CssClass="normaltextarea" runat="server" TextMode="MultiLine" Width="500" Height="300"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btnInsert" runat="server" Text="Add Email" OnClick="btnInsert_click" Visible="false" />
                                                <asp:Button ID="btnUpdate" runat="server" Text="Update Email" OnClick="btnUpdate_click" Visible="false" />
                                            </td>
                                        </tr>
                                    </table>
                                </InsertItemTemplate>
                            </asp:FormView>
                        </td>
                        <td valign="top">
                            <b><u>Special Commands</u></b>
                            <br /><br />
                            <table border="0">
                                <tr>
                                    <td colspan="3">
                                        <u>Order Confirmation</u>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @orderID
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the customers name.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @date
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the order date.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @items
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows a list of items in table format.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @totalPrice
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the total price payable by the customer.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @billingAdd
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the customers Billing Address.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @shippingAdd
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the customers Shipping Address.
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <u>Voucher Confirmation</u>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @recipient
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the recipients name.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @purchaser
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the orders name.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @recipientEmail
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the recipients email address.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @purchaserEmail
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the orders name email address.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @voucherAmount
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the value of the voucher.
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        @voucherNumber
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the unique voucher number.
                                    </td>
                                </tr>
                                 <tr>
                                    <td valign="top">
                                        @comment
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Shows the purchasers mssage to the recipient.
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                    </tr>
                </table>                
                <asp:Label ID="lblErrorInsert" runat="server" ForeColor="Red"></asp:Label>
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
        
    <asp:SqlDataSource ID="sqlEmails" runat="server" SelectCommand="procEmailMasterByCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="String" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
   
    
</asp:Content>

