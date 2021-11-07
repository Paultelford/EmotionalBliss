<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="payment.aspx.vb" Inherits="affiliates_payment" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
            <table border="0">
                <tr>
                    <td valign="top">
                        How would you like to pay for your goods:        
                    </td>
                    <td>
                        <asp:RadioButton ID="chkCC" runat="server" Text="Credit Card<br />" OnCheckedChanged="chkCC_checkedChanged" AutoPostBack="true" GroupName="type" />
                        <asp:RadioButton ID="chkAccount" runat="server" Text="Account Order" OnCheckedChanged="chkAccount_checkedChanged" AutoPostBack="true" GroupName="type" /><br />
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
            <table border="0">
                <tr>
                    <td id="tdCardDetails" runat="server" visible="false" width="340">
                        Credit Card Details -<br />
                        <table border="0">
                            <tr>
                                <td class="smfont">
                                    Card Type:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCardType" runat="server">
                                        <asp:ListItem Text="Visa" Value="VISA"></asp:ListItem>
                                        <asp:ListItem Text="Visa Debit/Delta" Value="DELTA"></asp:ListItem>
                                        <asp:ListItem Text="Visa Electron" Value="UKE"></asp:ListItem>
                                        <asp:ListItem Text="Mastercard" Value="MC"></asp:ListItem>
                                        <asp:ListItem Text="Switch" Value="SWITCH"></asp:ListItem>
                                        <asp:ListItem Text="Solo" Value="SOLO"></asp:ListItem>
                                        <asp:ListItem Text="American Express" Value="AMEX"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    Card No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCard" runat="server" MaxLength="26" Width="180"></asp:TextBox> <font color="red"><b>*</b></font> <asp:RegularExpressionValidator id="regex1" runat="server" ControlToValidate="txtCard" ValidationExpression="^\d{13,23}$" ErrorMessage="Invalid" Display="dynamic" /><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCard" ErrorMessage="Required" Display="dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    Start Date:<br /><font size='-2'>(Switch only)</font>
                                </td>
                                <td valign="top">
                                    <asp:DropDownList ID="drpStartMonth" runat="Server" OnLoad="bindMonths" AppendDataBoundItems="true">
                                        <asp:ListItem Text=" " Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;/&nbsp;
                                    <asp:DropDownList ID="drpStartYear" runat="server" OnLoad="drpStartYear_load" AppendDataBoundItems="true"> 
                                        <asp:ListItem Text=" " Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    End Date:
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpEndMonth" runat="Server" OnLoad="bindMonths" OnDataBound="drpEndMonth_dataBound">
                                    </asp:DropDownList>
                                    &nbsp;/&nbsp;
                                    <asp:DropDownList ID="drpEndYear" runat="server" OnLoad="drpEndYear_load"> 
                                    </asp:DropDownList> <font color="red"><b>*</b></font>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    Issue No:<br /><font size='-2'>(Switch only)</font>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIssue" runat="server" MaxLength="3" Width="26"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    CV2:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCV2" runat="server" MaxLength="4" Width="26"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqCV2" runat="server" ControlToValidate="txtCV2" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2" id="tdBreak" runat="server" visible="false">
                        <hr /><br />
                    </td>
                </tr>
                <tr>
                    <td valign="top" id="tdBillAddress" runat="server" visible="false">
                        Cardholders Details -
                        <table border="0" width="340">
                            <tr>
                                <td class="smfont">
                                    <asp:label ID="lblBillName" runat="server" Text="Name on Card:"></asp:label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBillName" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtBillName" ErrorMessage="Required" Display="dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    Address:
                                </td>
                                <td>
                                    <asp:TextBox id="txtBillAdd1" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtBillAdd1" ErrorMessage="Required" Display="dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                </td>
                                <td>
                                    <asp:TextBox id="txtBillAdd2" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                </td>
                                <td>
                                    <asp:TextBox id="txtBillAdd3" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                </td>
                                <td>
                                    <asp:TextBox id="txtBillAdd4" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    City:
                                </td>
                                <td>
                                    <asp:TextBox id="txtBillAdd5" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    Postcode:
                                </td>
                                <td>
                                    <asp:TextBox id="txtBillPostcode" runat="server" MaxLength="10"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="txtBillPostcode" ErrorMessage="Required" Display="dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trCountry" runat="server">
                                <td class="smfont">
                                    Country:
                                </td>
                                <td class="smfont">
                                    <asp:Label id="txtBillCountry" runat="server" ></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Email:      
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Required" Display="dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Purchase Order No:      
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPurchaseOrderNo" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trAccount" runat="server" visible="false">
                                <td>
                                    Account No:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAccount" runat="server" MaxLength="12"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    Deliver to this address  
                                </td>
                                <td class="smfont">
                                    <asp:RadioButton ID="radYes" runat="server" Text="Yes" AutoPostBack="true" OnCheckedChanged="radYes_checkedChanged" GroupName="bill" />&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="radNo" runat="server" Text="No" OnCheckedChanged="radNo_checkedChanged" GroupName="bill" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont" valign="top">
                                    <asp:Label ID="lblDeliveryInstructions" runat="server" Text="Special Delivery<br>Instructions:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDelivery" runat="server" MaxLength="200" CssClass="normaltextarea" TextMode="MultiLine" Rows="3" width="160"></asp:TextBox>
                                    <asp:Label ID="lblDeliveryError" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" id="tdShipAddress" runat="server" visible="false">
                        Shipping address -<br />
                        <table border="0">
                            <tr>
                                <td class="smfont">
                                    Name:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShipName" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtShipName" ErrorMessage="Required" Display="dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    Address:
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipAdd1" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtShipAdd1" ErrorMessage="Required" Display="dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipAdd2" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipAdd3" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipAdd4" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipAdd5" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    Postcode:
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipPostcode" runat="server" MaxLength="10"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtShipPostcode" ErrorMessage="Required" Display="dynamic"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr id="trCountry2" runat="server"> 
                                <td class="smfont">
                                    Country:
                                </td>
                                <td class="smfont">
                                    <asp:Label id="txtShipCountry" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:Button ID="btnSubmitShipBill" runat="server" Text="Confirm Order" OnClick="btnSubmitShipBill_click" Visible="false" />            
            <asp:Button ID="btnSubmitBill" runat="server" Text="Confirm Order" OnClick="btnSubmitBill_click" Visible="false" />            
            <br /><br />
   
</asp:Content>



