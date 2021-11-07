<%@ Page Language="VB" Trace="false" AutoEventWireup="false" MasterPageFile="~/m_shop.master" CodeFile="payment.aspx.vb" Inherits="shop_payment" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <!-- Checkout Stage Indicator -->
    <div id="basketmenu">
        Shopping Basket<br />
        <img src="/design/shop/images/basket-menu.gif" alt="basket" width="209" height="55" /><br />
        <span class="title3">Payment Method</span><br />
        <img src="/design/shop/images/basket-menu-activ.gif" alt="payment" width="209" height="50" /><br />
        Delivery Address<br />
        <img src="/design/shop/images/basket-menu.gif" alt="dispatch" width="209" height="50" /><br />
        Confirmation
    </div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Label ID="lblPleaseWait" runat="server" OnLoad="lblPleaseWait_load"></asp:Label>
            ....
            <asp:Image ID="imgPleaseWait" runat="server" ImageUrl="~/images/loading.gif" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <asp:HiddenField ID="hidVoucherPurchase" runat="server" Value="false" />
    <img src="/design/shop/images/payment-ttl.gif" alt="Payment Method" width="600" height="50" /><br />
    <table border="0" width="100%">
        <tr>
            <td valign="top" width="310">
                <asp:Label ID="lblHowPay" runat="server" dbResource="lblHowPay"></asp:Label>
            </td>
            <td valign="top" width="310">
                <asp:RadioButton ID="chkCC" runat="server" dbResource="lblChkCC" OnCheckedChanged="chkCC_checkedChanged" AutoPostBack="true" GroupName="type" /><br />
                <asp:RadioButton ID="chkPost" runat="server" dbResource="lblPost" OnCheckedChanged="chkPost_checkedChanged" AutoPostBack="true" GroupName="type" /><br />
                <asp:RadioButton ID="chkIDeal" runat="server" dbResource="lblIDeal" OnCheckedChanged="chkIDeal_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
                <asp:RadioButton ID="chkAccount" runat="server" dbResource="lblAccount" OnCheckedChanged="chkAccount_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
            </td>
            <td>
                <script type="text/javascript" src="https://seal.XRamp.com/seal.asp?type=H"></script>
            </td>
        </tr>
    </table>
    <img src="/design/shop/images/credit-card-logos.gif" alt="Visa, Mastercard, Maestro, Delta" width="280" height="41" border="0" />
    <br /><br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table border="0" width="100%">
                <tr>
                    <td id="tdCardDetails" runat="server" visible="false" width="400" valign="top">
                        <asp:Label ID="lblCardDetails" runat="server" dbResource="lblCardDetails" font-bold="true"></asp:Label>
                        <br />
                        <table border="0">
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblCardType" runat="server" dbResource="lblCardType"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpCardType" runat="server">
                                        <asp:ListItem Text="Visa" Value="VISA"></asp:ListItem>
                                        <asp:ListItem Text="Visa Debit/Delta" Value="DELTA"></asp:ListItem>
                                        <asp:ListItem Text="Visa Electron" Value="UKE"></asp:ListItem>
                                        <asp:ListItem Text="Mastercard" Value="MC"></asp:ListItem>
                                        <asp:ListItem Text="Switch" Value="SWITCH"></asp:ListItem>
                                        <asp:ListItem Text="Solo" Value="SOLO"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblCardNo" runat="server" dbResource="lblCardNo"></asp:Label>
                                </td>
                                <td>
                                    <nobr>
                                    <asp:TextBox ID="txtCard" runat="server" MaxLength="26" Width="180" ValidationGroup="add"></asp:TextBox> <asp:RegularExpressionValidator id="regex1" runat="server" ControlToValidate="txtCard" ValidationExpression="^\d{13,23}$" Display="dynamic" ValidationGroup="add" /><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCard" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                    </nobr>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblStartDate" runat="server" dbResource="lblStartDate"></asp:Label>
                                    <br />
                                    <font size='-2'>
                                        <asp:Label ID="lblSwitchOnly" runat="server" dbResource="lblSwitchOnly"></asp:Label>
                                    </font>
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
                                    <asp:Label ID="lblEndDate" runat="server" dbResource="lblEndDate"></asp:Label>
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
                                    <asp:Label ID="lblIssueNo" runat="server" dbResource="lblIssueNo"></asp:Label>
                                    <br />
                                    <font size='-2'>
                                        <asp:Label ID="lblSwitchOnly2" runat="server" dbResource="lblSwitchOnly"></asp:Label>
                                    </font>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIssue" runat="server" MaxLength="3" Width="26"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont" valign="top">
                                    <asp:Label ID="lblCV2" runat="server" dbResource="lblCV2"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCV2" runat="server" MaxLength="4" Width="32"></asp:TextBox> <font color="red"><b>*</b></font>
                                    <asp:Label ID="lblCV2Exp" runat="server" Font-Size="smaller" dbResource="lblCV2Exp"></asp:Label>
                                    <asp:RegularExpressionValidator id="regTxtCV2" runat="server" ControlToValidate="txtCV2" ValidationExpression="^[0-9]{3,4}$" Display="dynamic" ValidationGroup="add" />
                                    <asp:RequiredFieldValidator ID="reqTxtCV2" runat="server" ControlToValidate="txtCV2" ValidationGroup="add"></asp:RequiredFieldValidator>
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
<!-- BILLING ADDRESS -->
                    <td valign="top" id="tdBillAddress" runat="server" visible="false">
                        <table border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblBillingDetails" runat="server" dbResource="lblBillingDetails" font-bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton id="btnViewShipping" runat="server" dbResource="btnViewShipping" visible="false" onClick="btnViewShipping_click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont" width="200">
                                    <asp:label ID="lblBillName" runat="server" Text="Name on card:"></asp:label>
                                </td>
                                <td>
                                <nobr>
                                    <asp:TextBox ID="txtBillName" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtBillName" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                    </nobr>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblEmail" runat="server" dbResource="lblEmail"></asp:Label>
                                </td>
                                <td class="smfont">
                                    <nobr>
                                    <asp:Textbox id="txtEmail" runat="server" MaxLength="50"></asp:Textbox>
                                    <asp:RegularExpressionValidator ID="regTxtEmail" runat="server" ControlToValidate="txtEmail" Display="dynamic" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$" ValidationGroup="add"></asp:RegularExpressionValidator>
                                    </nobr>
                                </td>
                            </tr>
                            <tr id="trAddSearch1" runat="server" visible="false">
                                <td class="smfont">
                                    <asp:Label ID="lblHouseNo" runat="server" dbResource="lblHouseNo"></asp:Label>
                                </td>
                                <td>
                                    <nobr>
                                    <asp:TextBox ID="txtLookupBillHouse" runat="server" MaxLength="30" ValidationGroup="billLookup"></asp:TextBox>&nbsp;
                                    <asp:RequiredFieldValidator ID="reqBillLookupHouse" runat="server" ControlToValidate="txtLookupBillHouse" Display="dynamic" ValidationGroup="billLookup"></asp:RequiredFieldValidator>
                                    </nobr>
                                </td>
                            </tr>
                            <tr id="trAddSearch2" runat="server" visible="false">
                                <td class="smfont">
                                    <asp:Label ID="lblPostcode" runat="server" dbResource="lblPostcode"></asp:Label>
                                </td>
                                <td>
                                    <nobr>
                                    <asp:TextBox ID="txtLookupBillPostcode" runat="server" MaxLength="10" ValidationGroup="billLookup"></asp:TextBox>&nbsp;
                                    <asp:RequiredFieldValidator ID="reqBillLookupPostcode" runat="server" ControlToValidate="txtLookupBillPostcode" Display="dynamic" ValidationGroup="billLookup"></asp:RequiredFieldValidator>
                                    </nobr>
                                </td>
                            </tr>
                            <tr id="trAddSearch3" runat="server" visible="false">
                                <td colspan="2" align="right">
                                    <asp:Button ID="lnkLookup" Visible="false" runat="server" Text="<%$ Resources:btnLookupAddress %>" onCommand="lnkLookup_click" ValidationGroup="billLookup" CommandName="bill"></asp:Button>
                                    <asp:ImageButton ID="imgLookupBill" runat="server" ImageUrl="~/images/navImages/Btn_LookupAddress_GB.jpg" OnCommand="lnkLookup_click" CommandName="bill" ValidationGroup="billLookup" />
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblAddress" runat="server" dbResource="lblAddress"></asp:Label>
                                </td>
                                <td>
                                    <nobr>
                                    <asp:TextBox id="txtBillAdd1" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtBillAdd1" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                    </nobr>
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
                                    <asp:Label ID="lblBillPostcodeError" runat="server" ForeColor="red"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox id="txtBillAdd5" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblPostcode2" runat="server" dbResource="lblPostcode"></asp:Label>
                                </td>
                                <td>
                                    <nobr>
                                    <asp:TextBox id="txtBillPostcode" runat="server" MaxLength="10"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="txtBillPostcode" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
                                    </nobr>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblPhone" runat="server" Text="Phone No:"></asp:Label>
                                </td>
                                <td>
                                    <nobr>
                                    <asp:TextBox id="txtPhone" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="reqTxtPhone" runat="server" ControlToValidate="txtPhone" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
                                    </nobr>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblCountry" runat="server" dbResource="lblCountry"></asp:Label>
                                </td>
                                <td class="smfont">
                                    <asp:Label id="txtBillCountry" runat="server" OnLoad="txtBillCountry_load"></asp:Label>
                                </td>
                            </tr>
                            
                            <tr id="trAccount" runat="server" visible="false">
                                <td>
                                    <asp:Label ID="lblAccount" runat="server" dbResource="lblAccountNo"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtAccount" runat="server" MaxLength="12"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
<!-- SHIPPING ADDRESS-->
                    <td valign="top" id="tdShipAddress" runat="server" visible="false">
                        <br />
                        <table border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lblShippingAddress" runat="server" dbResource="lblShippingAddress" font-bold="true"></asp:Label>
                                </td>
                                <td>
                                    <asp:LinkButton id="btnViewBilling" Visible="false" runat="server" dbResource="btnViewBilling" onClick="btnViewBilling_click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont" width="200">
                                    <asp:Label ID="lblName" runat="server" dbResource="lblName"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtShipName" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtShipName" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" height="24">&nbsp;</td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblHouseNo2" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <nobr>
                                    <asp:TextBox ID="txtLookupShipHouse" runat="server" MaxLength="30" ValidationGroup="billLookup"></asp:TextBox>&nbsp;
                                    <asp:RequiredFieldValidator ID="reqLookupShipHouse" runat="server" ControlToValidate="txtLookupShipHouse" Display="dynamic" ValidationGroup="shipLookup"></asp:RequiredFieldValidator>
                                    </nobr>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblPostcode3" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <nobr>
                                    <asp:TextBox ID="txtLookupShipPostcode" runat="server" MaxLength="10" ValidationGroup="billLookup"></asp:TextBox>&nbsp;
                                    <asp:RequiredFieldValidator ID="reqLookupShipPostcode" runat="server" ControlToValidate="txtLookupShipPostcode" Display="dynamic" ValidationGroup="shipLookup"></asp:RequiredFieldValidator>
                                    <nobr>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" align="right">
                                    <asp:Button ID="lnkButton" Visible="false" runat="server" Text="<%$ Resources:btnLookupAddress %>" onCommand="lnkLookup_click" ValidationGroup="shipLookup" CommandName="ship"></asp:Button>
                                    <asp:ImageButton ID="btnLookupAddress" runat="server" ImageUrl="~/images/navImages/Btn_LookupAddress_GB.jpg" OnCommand="lnkLookup_click" CommandName="ship" ValidationGroup="shipLookup" />
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblAddress2" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipAdd1" runat="server" MaxLength="50"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtShipAdd1" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
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
                                    <asp:Label ID="lblShipPostcodeError" runat="server" ForeColor="red"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipAdd5" runat="server" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblPostcode4" runat="server"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox id="txtShipPostcode" runat="server" MaxLength="10"></asp:TextBox> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtShipPostcode" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblCountry2" runat="server"></asp:Label>
                                </td>
                                <td class="smfont">
                                    <asp:Label id="txtShipCountry" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
<!-- PERSONAL INFO -->
            <table>
                <tr>
                    <td width="400" id="tdPersonalSpacer" runat="server">
                        &nbsp;
                    </td>
                    <td id="tdPersonalInfo" runat="server" visible="false">
                        <table>
                            <tr>
                                <td class="smfont" valign="top">
                                    <asp:Label ID="lblDeliveryInstructions" runat="server" Text="Special Delivery<br>Instructions:"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDelivery" runat="server" MaxLength="200" TextMode="MultiLine" Rows="2" Columns="30"></asp:TextBox>
                                    <asp:Label ID="lblDeliveryError" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont" width="200">
                                    <asp:Label ID="lblDOB" runat="server" dbResource="lblDOB"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpDay" runat="server">
                                    </asp:DropDownList>&nbsp;&nbsp;
                                    <asp:DropDownList ID="drpMonth" runat="server">
                                    </asp:DropDownList>&nbsp;&nbsp;
                                    <asp:DropDownList ID="drpYear" runat="server">
                                    </asp:DropDownList>                                    
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblGender" runat="server" dbResource="lblGender"></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="drpGender" runat="server">
                                        <asp:ListItem dbResource="drpSelect" Value=""></asp:ListItem>
                                        <asp:ListItem dbResource="drpMale" Value="Male"></asp:ListItem>
                                        <asp:ListItem dbResource="drpFemale" Value="Female"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="smfont">
                                    <asp:Label ID="lblDeliverTo" runat="server" dbResource="lblDeliverTo"></asp:Label>
                                </td>
                                <td class="smfont">
                                    <asp:RadioButton ID="radYes" runat="server" dbResource="radYes" AutoPostBack="true" OnCheckedChanged="radYes_checkedChanged" GroupName="bill" />&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="radNo" runat="server" dbResource="radNo" OnCheckedChanged="radNo_checkedChanged" GroupName="bill" AutoPostBack="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:ImageButton ID="btnSubmitShipBill" runat="server" ImageUrl="~/images/navImages/Btn_CompleteOrder_GB.jpg" Visible="false" ValidationGroup="add" OnClick="btnSubmitShipBill_click" OnClientClick="this.style.display='none';" />
                                    <asp:ImageButton ID="btnSubmitBill" runat="server" ImageUrl="~/images/navImages/Btn_CompleteOrder_GB.jpg" Visible="false" ValidationGroup="add" OnClick="btnSubmitBill_click" OnClientClick="this.style.display='none';" />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />
            <asp:label ID="lblError" runat="server" ForeColor="red"></asp:label>
            </ContentTemplate>
    </atlas:UpdatePanel>
    
</asp:Content>