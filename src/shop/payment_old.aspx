<%@ Page Language="VB" Trace="false" AutoEventWireup="false" MasterPageFile="~/m_shop.master" Theme="" CodeFile="payment_old.aspx.vb" Inherits="shop_payment" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <!-- Use jQuery to do the Checkout rollover image -->
    <script type='text/javascript' src='/jquery.js'></script>
    <script type='text/javascript' src='/jquery.preload.js'></script>
    <script type='text/javascript' src='/jquery.rollover.js'></script>
    <script language="javascript" type="text/javascript">
        $(function(){
            $("div.radios").show();
            Sys.Application.add_load(wireEvents);
        });
        function showPleaseWaitMsg(state)
        {
            if (__nonMSDOMBrowser)
            {
                document.getElementById("plsWait").style.display=state;
            }else{
                document.all["plsWait"].style.display=state;
            }            
        }
    </script>
    <!-- Checkout Stage Indicator -->
    <atlas:UpdatePanel ID="updateTree" runat="Server" ChildrenAsTriggers="false" UpdateMode="conditional">
    <ContentTemplate>
        <div id="basketmenu">
            Shopping Basket<br />
            <img src="/design/shop/images/basket-menu.gif" alt="basket" width="209" height="50" /><br />
            <asp:Literal ID="litPaymentText" runat="server"><span class="title3">Payment Method</span></asp:Literal>
            <br />
            <asp:Image ID="imgMiddleSection" runat="Server" ImageUrl="/design/shop/images/basket-menu-activ.gif" Width="209" Height="55" /><br />
            <asp:Literal ID="litDeliveryText" runat="server">Delivery Address</asp:Literal> 
            <br />
            <asp:Image ID="imgBottomSection" runat="Server" ImageUrl="/design/shop/images/basket-menu.gif" Width="209" Height="50" /><br />
            Confirmation
        </div>
    </ContentTemplate>
    </atlas:UpdatePanel>
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

    <asp:TextBox ID="txtBillAdd5" runat="server" Visible="false"></asp:TextBox>
    <asp:TextBox id="txtShipAdd5" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
    <asp:Label ID="lblShipPostcodeError" runat="server" ForeColor="red" Visible="false"></asp:Label>
    <asp:Label ID="lblBillPostcodeError" runat="Server"></asp:Label>
    <div id="productinfocontainer">
        <div id="paymenttext"><asp:Label ID="lblHowPay" runat="server" dbResource="lblHowPay"></asp:Label></div>
        <div id="paymentselect">
            <div class="radios" style="display:none;">
                <asp:RadioButton ID="chkCC" runat="server" dbResource="lblChkCC" OnCheckedChanged="chkCC_checkedChanged" AutoPostBack="true" GroupName="type" /><br />
                <asp:RadioButton ID="chkPost" runat="server" dbResource="lblPost" OnCheckedChanged="chkPost_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" /><br />
                <asp:RadioButton ID="chkIDeal" runat="server" dbResource="lblIDeal" OnCheckedChanged="chkIDeal_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
                <asp:RadioButton ID="chkAccount" runat="server" dbResource="lblAccount" OnCheckedChanged="chkAccount_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
            </div>
        </div>
    </div>
    <table width="100%">
        <tr>
            <td valign="middle">
                <img src="/design/shop/images/credit-card-logos.gif" alt="Visa, Mastercard, Maestro, Delta" width="280" height="41" border="0" />            
            </td>
            <td valign="middle" align="right">
                <script type="text/javascript" src="https://seal.XRamp.com/seal.asp?type=H"></script>    &nbsp;&nbsp;&nbsp;&nbsp;        
            </td>
        </tr>
    </table>    
    <br /><br />
    <atlas:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <div id="productinfocontainer">
                    <div id="paymenttext">
                        <asp:Panel ID="panCard" runat="Server" Visible="false">
		                    <div id="creditcards">		                    
		                        <asp:Literal ID="litCardDetails" runat="server"></asp:Literal>
		                    </div>
		                    <br class="clear" />
		                    <fieldset>
		                        <p>
    		                        <label for="name" class="text">
    		                            <asp:Label ID="lblCardType" runat="server" dbResource="lblCardType" Font-Bold="true"></asp:Label>
    		                        </label>
    		                        <asp:DropDownList ID="drpCardType" runat="server">
                                        <asp:ListItem Text="Visa" Value="VISA"></asp:ListItem>
                                        <asp:ListItem Text="Visa Debit/Delta" Value="DELTA"></asp:ListItem>
                                        <asp:ListItem Text="Visa Electron" Value="UKE"></asp:ListItem>
                                        <asp:ListItem Text="Mastercard" Value="MC"></asp:ListItem>
                                        <asp:ListItem Text="Switch/Maestro" Value="SWITCH"></asp:ListItem>
                                        <asp:ListItem Text="Solo" Value="SOLO"></asp:ListItem>
                                    </asp:DropDownList>
		                        </p>
		                        <p>
		                            <label for="name" class="text">
		                                <asp:Label ID="lblCardNo" runat="server" dbResource="lblCardNo" Font-Bold="true"></asp:Label>
		                            </label>
		                            <asp:TextBox ID="txtCard" runat="server" MaxLength="26" Width="140" ValidationGroup="add"></asp:TextBox><span class="title3">*</span><asp:RegularExpressionValidator id="regex1" runat="server" ControlToValidate="txtCard" ValidationExpression="^\d{13,23}$" Display="dynamic" ValidationGroup="add" /><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCard" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
		                        </p>
		                        <p>
		                            <label for="name" class="text">
		                                <asp:Label ID="lblStartDate" runat="server" dbResource="lblStartDate" Font-Bold="true"></asp:Label><br />
		                                <asp:Label ID="lblSwitchOnly" runat="server" dbResource="lblSwitchOnly"></asp:Label>
		                            </label>
		                            <asp:DropDownList ID="drpStartMonth" runat="Server" OnLoad="bindMonths" AppendDataBoundItems="true">
                                        <asp:ListItem Text=" " Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp;/&nbsp;
                                    <asp:DropDownList ID="drpStartYear" runat="server" OnLoad="drpStartYear_load" AppendDataBoundItems="true"> 
                                        <asp:ListItem Text=" " Value=""></asp:ListItem>
                                    </asp:DropDownList>
		                        </p>
		                        <p>
		                            <label for="name" class="text">
		                                <asp:Label ID="lblEndDate" runat="server" dbResource="lblEndDate" Font-Bold="true"></asp:Label>
		                            </label>
		                            <asp:DropDownList ID="drpEndMonth" runat="Server" OnLoad="bindMonths" OnDataBound="drpEndMonth_dataBound">
                                    </asp:DropDownList>
                                    &nbsp;/&nbsp;
                                    <asp:DropDownList ID="drpEndYear" runat="server" OnLoad="drpEndYear_load"> 
                                    </asp:DropDownList><span class="title3">*</span>
		                        </p>
		                        <p>
		                            <label for="name" class="text">
		                                <asp:Label ID="lblIssueNo" runat="server" dbResource="lblIssueNo" Font-Bold="true"></asp:Label><br />
		                                <asp:Label ID="lblSwitchOnly2" runat="server"></asp:Label>
		                            </label>
		                            <asp:TextBox ID="txtIssue" runat="server" MaxLength="3" Width="36"></asp:TextBox>
		                        </p>
		                        <p>
		                            <label for="name" class="text">
		                                <nobr><asp:Label ID="lblCV2" runat="server" dbResource="lblCV2" Font-Bold="true"></asp:Label></nobr><br />
		                                <asp:Label ID="lblCV2Exp" runat="server" Font-Size="smaller" dbResource="lblCV2Exp"></asp:Label>
		                            </label>
                                    <asp:TextBox ID="txtCV2" runat="server" MaxLength="4" Width="48"></asp:TextBox><span class="title3">*</span>                    
                                    <asp:RegularExpressionValidator id="regTxtCV2" runat="server" ControlToValidate="txtCV2" ValidationExpression="^[0-9]{3,4}$" Display="dynamic" ValidationGroup="add" />
                                    <asp:RequiredFieldValidator ID="reqTxtCV2" runat="server" ControlToValidate="txtCV2" ValidationGroup="add" Display="dynamic"></asp:RequiredFieldValidator>
		                        </p>
                            </fieldset>
                        </asp:Panel>
                    </div>
                
                    <div id="paymentselect">
                        <asp:Panel ID="panBill" runat="Server" Visible="false">
	                         <div id="delivery">
	                            <asp:Literal ID="litDelivery" runat="server"></asp:Literal>
	                         </div>
	                         <br class="clear" />
	                         <fieldset>
	                            <p>
	                                <label for="name" class="text">
	                                    <asp:label ID="lblBillName" runat="server" Text="Name on card:" Font-Bold="true"></asp:label>
	                                </label>
	                                <asp:TextBox ID="txtBillName" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtBillName" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
	                            </p>
	                            <p>
	                                <label for="name" class="text">
	                                    <asp:Label ID="lblEmail" runat="server" dbResource="lblEmail" Font-Bold="true"></asp:Label>
	                                </label>
	                                <asp:Textbox id="txtEmail" runat="server" MaxLength="50"></asp:Textbox>
                                    <asp:RegularExpressionValidator ID="regTxtEmail" runat="server" ControlToValidate="txtEmail" Display="dynamic" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$" ValidationGroup="add"></asp:RegularExpressionValidator>
	                            </p>
	                            <table border="0" cellpadding="0" cellspacing="0" id="tblLookup1" runat="server" visible="false" width="100%">
	                                <tr>
	                                    <td>
	                                        <p>
	                                            <label for="name" class="text">
                                                    <asp:Label ID="lblHouseNo" runat="server" dbResource="lblHouseNo" Font-Bold="true"></asp:Label>
                                                </label>
                                                <asp:TextBox ID="txtLookupBillHouse" runat="server" MaxLength="30" ValidationGroup="billLookup"></asp:TextBox>&nbsp;
                                                <asp:RequiredFieldValidator ID="reqBillLookupHouse" runat="server" ControlToValidate="txtLookupBillHouse" Display="dynamic" ValidationGroup="billLookup"></asp:RequiredFieldValidator>
                                            </p>
                                            <p>
	                                            <label for="name" class="text">
	                                                <asp:Label ID="lblPostcode" runat="server" dbResource="lblPostcode" Font-Bold="true"></asp:Label>
	                                            </label>
	                                            <asp:TextBox ID="txtLookupBillPostcode" runat="server" MaxLength="10" ValidationGroup="billLookup"></asp:TextBox>&nbsp;
                                                <asp:RequiredFieldValidator ID="reqBillLookupPostcode" runat="server" ControlToValidate="txtLookupBillPostcode" Display="dynamic" ValidationGroup="billLookup"></asp:RequiredFieldValidator>
                                            </p>
                                            <asp:ImageButton ID="imgLookupBill" runat="server" AlternateText="Lookup Address" CssClass="rollover" ImageUrl="/design/shop/images/lookup-address-btn.jpg" OnCommand="lnkLookup_click" CommandName="bill" ValidationGroup="billLookup" />
	                                    </td>
	                                </tr>
	                            </table>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblAddress" runat="server" dbResource="lblAddress" Font-Bold="true"></asp:Label>
                                    </label>
                                    <asp:TextBox id="txtBillAdd1" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtBillAdd1" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                                </p>
                                <p>
                                    <label for="name" class="text"></label>
                                    <asp:TextBox id="txtBillAdd2" runat="server" MaxLength="50"></asp:TextBox>
                                </p>
                                <p>
                                    <label for="name" class="text"></label>
                                    <asp:TextBox id="txtBillAdd3" runat="server" MaxLength="50"></asp:TextBox>
                                </p>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblStateProvince" runat="server" dbResource="lblStateProvince" Text="State/Province:" Visible="false" Font-Bold="true"></asp:Label>
                                    </label>
                                    <asp:TextBox id="txtBillAdd4" runat="server" MaxLength="50"></asp:TextBox>
                                </p>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblPostcode2" runat="server" dbResource="lblPostcode" Font-Bold="true"></asp:Label>    
                                    </label>
                                    <asp:TextBox id="txtBillPostcode" runat="server" MaxLength="10"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="txtBillPostcode" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
                                </p>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblPhone" runat="server" Text="Phone No:" Font-Bold="true"></asp:Label> 
                                    </label>
                                    <asp:TextBox id="txtPhone" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="reqTxtPhone" runat="server" ControlToValidate="txtPhone" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
                                </p>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblCountry" runat="server" dbResource="lblCountry" Font-Bold="true"></asp:Label>
                                    </label>
                                    <asp:Label id="txtBillCountry" runat="server" OnLoad="txtBillCountry_load"></asp:Label>
                                </p>
                                <p id="trAccount" runat="server" visible="false">
                                    <label for="name" class="text">
                                        <asp:Label ID="lblAccount" runat="server" dbResource="lblAccountNo" Font-Bold="true"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtAccount" runat="server" MaxLength="12"></asp:TextBox>
                                </p>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblDeliveryInstructions" runat="server" Text="Special Delivery<br>Instructions:" Font-Bold="true"></asp:Label>
                                    </label>
                                    <asp:TextBox ID="txtDelivery" runat="server" MaxLength="200" CssClass="normaltextarea" TextMode="MultiLine" Rows="2" Columns="30"></asp:TextBox>
                                    <asp:Label ID="lblDeliveryError" runat="server"></asp:Label>
                                </p>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblDOB" runat="server" dbResource="lblDOB" Font-Bold="true"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="drpDay" runat="server">
                                    </asp:DropDownList>&nbsp;&nbsp;
                                    <asp:DropDownList ID="drpMonth" runat="server">
                                    </asp:DropDownList>&nbsp;&nbsp;
                                    <asp:DropDownList ID="drpYear" runat="server">
                                    </asp:DropDownList>    
                                </p>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblGender" runat="server" dbResource="lblGender" Font-Bold="true"></asp:Label>
                                    </label>
                                    <asp:DropDownList ID="drpGender" runat="server">
                                        <asp:ListItem dbResource="drpSelect" Value=""></asp:ListItem>
                                        <asp:ListItem dbResource="drpMale" Value="Male"></asp:ListItem>
                                        <asp:ListItem dbResource="drpFemale" Value="Female"></asp:ListItem>
                                    </asp:DropDownList>
                                </p>
                                <p>
                                    <label for="name" class="text">
                                        <asp:Label ID="lblDeliverTo" runat="server" dbResource="lblDeliverTo" Font-Bold="true"></asp:Label>
                                    </label>
                                    <asp:RadioButton ID="radYes" runat="server" dbResource="radYes" AutoPostBack="true" OnCheckedChanged="radYes_checkedChanged" GroupName="bill" />&nbsp;&nbsp;&nbsp;<asp:RadioButton ID="radNo" runat="server" dbResource="radNo" OnCheckedChanged="radNo_checkedChanged" GroupName="bill" AutoPostBack="true" />
                                </p>
	                        </fieldset>      
	                    </asp:Panel>
	                </div>
	            
                
        	    
        <!-- **************************************************************** -->


	            <asp:Panel ID="panShip" runat="Server" Visible="false">
	                <div id="paymentselect">
	                     <div id="delivery">
	                        <asp:Literal ID="litShippingAddress" runat="Server"></asp:Literal><br />
	                     </div>
	                     <br class="clear" />
	                     <fieldset>
	                        <p>
	                            <label for="name" class="text">
	                                <asp:Label ID="lblName" runat="server" dbResource="lblName"></asp:Label>
	                            </label>
	                            <asp:TextBox ID="txtShipName" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtShipName" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
	                        </p>
	                        <table border="0" cellpadding="0" cellspacing="0" id="tblLookup2" runat="server" visible="false" width="100%">
                                <tr>
                                    <td>
                                        <p>
                                            <label for="name" class="text">
                                                <asp:Label ID="lblHouseNo2" runat="server"></asp:Label>
                                            </label>
                                            <asp:TextBox ID="txtLookupShipHouse" runat="server" MaxLength="30" ValidationGroup="billLookup"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="reqLookupShipHouse" runat="server" ControlToValidate="txtLookupShipHouse" Display="dynamic" ValidationGroup="shipLookup"></asp:RequiredFieldValidator>
                                        </p>
                                        <p>
                                            <label for="name" class="text">
                                                <asp:Label ID="lblPostcode3" runat="server"></asp:Label>
                                            </label>
                                            <asp:TextBox ID="txtLookupShipPostcode" runat="server" MaxLength="10" ValidationGroup="billLookup"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="reqLookupShipPostcode" runat="server" ControlToValidate="txtLookupShipPostcode" Display="dynamic" ValidationGroup="shipLookup"></asp:RequiredFieldValidator>
                                        </p>
                                        <asp:ImageButton ID="btnLookupAddress" runat="server" CssClass="rollover" ImageUrl="/design/shop/images/lookup-address-btn.jpg" OnCommand="lnkLookup_click" CommandName="ship" ValidationGroup="shipLookup" />
                                    </td>
                                </tr>
                            </table>
                            <p>
                                <label for="name" class="text">
                                    <asp:Label ID="lblAddress2" runat="server"></asp:Label>
                                </label>
                                <asp:TextBox id="txtShipAdd1" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtShipAdd1" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                            </p>
                            <p>
                                <label for="name" class="text">
                                </label>
                                <asp:TextBox id="txtShipAdd2" runat="server" MaxLength="50"></asp:TextBox>
                            </p>
                            <p>
                                <label for="name" class="text">
                                </label>
                                <asp:TextBox id="txtShipAdd3" runat="server" MaxLength="50"></asp:TextBox>
                            </p>
                            <p>
                                <label for="name" class="text">
                                    <asp:Label ID="lblStateProvince2" runat="server" Text="State/Province:" Visible="false" Font-Bold="true"></asp:Label>
                                </label>
                                <asp:TextBox id="txtShipAdd4" runat="server" MaxLength="50"></asp:TextBox>
                            </p>
                            <p>
                                <label for="name" class="text">    
                                    <asp:Label ID="lblPostcode4" runat="server"></asp:Label>
                                </label>                            
                                <asp:TextBox id="txtShipPostcode" runat="server" MaxLength="10"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtShipPostcode" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
                            </p>
                            <p>
                                <label for="name" class="text">
                                    <asp:Label ID="lblCountry2" runat="server"></asp:Label>
                                </label>
                                <asp:Label id="txtShipCountry" runat="server"></asp:Label>
                            </p>
                            <p>
                                <label for="name" class="text">
                                </label>
                                <asp:LinkButton id="btnViewBilling" Visible="false" runat="server" dbResource="btnViewBilling" onClick="btnViewBilling_click"></asp:LinkButton>
                            </p>
	                    </fieldset>      
	                </div>
	            </asp:Panel>
	            
            </div>
            <div id="buttoncomplete">
                <asp:ImageButton ID="btnSubmitShipBill" runat="server" CssClass="rollover" ImageUrl="/design/shop/images/Btn_CompleteOrder_new_GB.jpg" OnClientClick='javascript:if(Page_ClientValidate("add")){this.style.display="none";showPleaseWaitMsg("block");}' Visible="false" ValidationGroup="add" OnClick="btnSubmitShipBill_click" />
                <asp:ImageButton ID="btnSubmitBill" runat="server" CssClass="rollover" ImageUrl="/design/shop/images/Btn_CompleteOrder_new_GB.jpg" OnClientClick='javascript:if(Page_ClientValidate("add")){this.style.display="none";showPleaseWaitMsg("block");}' Visible="false" ValidationGroup="add" OnClick="btnSubmitBill_click" />        <br />
                <span style="display:none;" id="plsWait" class="title3">
                    <asp:Literal ID="litPleaseWait" runat="server" OnLoad="litPleaseWait_load" Visible="true"></asp:Literal>....
                    <asp:Image ID="imgPleaseWait2" runat="server" ImageUrl="~/images/loading.gif" />
                </span>
            </div>
            <br />
            <asp:label ID="lblError" runat="server" ForeColor="red"></asp:label>
         </ContentTemplate>
        <Triggers>
            <atlas:AsyncPostBackTrigger ControlID="chkCC" EventName="CheckedChanged" />
            <atlas:AsyncPostBackTrigger ControlID="chkPost" EventName="CheckedChanged" />
            <atlas:AsyncPostBackTrigger ControlID="chkIdeal" EventName="CheckedChanged" />
            <atlas:AsyncPostBackTrigger ControlID="chkAccount" EventName="CheckedChanged" />
        </Triggers>
    </atlas:UpdatePanel>   
    <!-- Hidden resources -->
    <asp:Label ID="lblCardDetails" runat="server" dbResource="lblCardDetails" Visible="false"></asp:Label>
    <asp:Label ID="lblBillingDetails" runat="server" dbResource="lblBillingDetails" Visible="false"></asp:Label>
    <asp:Label ID="lblShippingAddress" runat="server" dbResource="lblShippingAddress" Visible="false"></asp:Label>
    
    
    
                
</asp:Content>