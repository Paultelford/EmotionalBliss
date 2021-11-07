<%@ Page Language="VB" Trace="false" AutoEventWireup="false" MasterPageFile="~/mshop.master" Theme="" CodeFile="payment_20210829.aspx.vb" Inherits="shop_payment2" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBShopMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
	<!-- Use jQuery to do the Checkout rollover image -->
	<script type='text/javascript' src='/jquery.js'></script>
	<script type='text/javascript' src='/jquery.preload.js'></script>
	<script type='text/javascript' src='/jquery.rollover.js'></script>
	<script language="javascript" type="text/javascript">
		$(function () {
			$("div.radios").show();
			Sys.Application.add_load(wireEvents);
		});
		function showPleaseWaitMsg(state) {
			if (__nonMSDOMBrowser) {
				document.getElementById("plsWait").style.display = state;
			} else {
				document.all["plsWait"].style.display = state;
			}
		}
	</script>
	<!-- Checkout Stage Indicator -->
	<atlas:UpdatePanel ID="updateTree" runat="Server" ChildrenAsTriggers="false" UpdateMode="conditional">
	<ContentTemplate>
		<div id="basketmenu">
		<asp:Label id="lblShoppingBasketText" runat="server"></asp:Label>
		<!--
			Shopping Basket<br />
			<img src="/design/shop/images/basket-menu.gif" alt="basket" width="209" height="50" /><br />
			<asp:Literal ID="litPaymentText" runat="server"><span class="title3">Payment Method</span></asp:Literal>
			<br />
			<asp:Image ID="imgMiddleSection" runat="Server" ImageUrl="/design/shop/images/basket-menu-activ.gif" Width="209" Height="55" /><br />
			<asp:Literal ID="litDeliveryText" runat="server">Delivery Address</asp:Literal> 
			<br />
			<asp:Image ID="imgBottomSection" runat="Server" ImageUrl="/design/shop/images/basket-menu.gif" Width="209" Height="50" /><br />
			Confirmation-->
		</div>
	</ContentTemplate>
	</atlas:UpdatePanel>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<dbResource="lblShoppingBasketText" />
	<atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
	<atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
		<ProgressTemplate>
			<asp:Label ID="lblPleaseWait" runat="server" OnLoad="lblPleaseWait_load"></asp:Label>
			....
			<asp:Image ID="imgPleaseWait" runat="server" ImageUrl="~/images/loading.gif" />
		</ProgressTemplate>
	</atlas:UpdateProgress>
	<asp:HiddenField ID="hidVoucherPurchase" runat="server" Value="false" />
	<h3>Shopping Basket</h3><br>
	<div id="DashedLineHorizontal"></div>

	<asp:TextBox ID="txtBillAdd5" runat="server" Visible="false"></asp:TextBox>
	<asp:TextBox id="txtShipAdd5" runat="server" MaxLength="50" Visible="false"></asp:TextBox>
	<asp:Label ID="lblShipPostcodeError" runat="server" ForeColor="red" Visible="false"></asp:Label>
	<asp:Label ID="lblBillPostcodeError" runat="Server"></asp:Label>
	
	<asp:Label ID="lblHowPay" runat="server" dbResource="lblHowPay"></asp:Label>
	<table width="100%" border="0">
		<tr>
			<td valign="middle">
				<img src="/design/shop/images/credit-card-logos.gif" alt="Visa, Mastercard, Maestro, Delta" width="280" height="41" border="0" />            
			</td>
			<td align="right">
				<table border="0">
					<tr>
						<td align="left">
							<div class="radios" style="display:none;">
								<asp:RadioButton ID="chkCC" runat="server" dbResource="lblChkCC" OnCheckedChanged="chkCC_checkedChanged" AutoPostBack="true" GroupName="type" /><%--<br />--%>
								<asp:RadioButton ID="chkPost" runat="server" dbResource="lblPost" OnCheckedChanged="chkPost_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
								<asp:Label ID="lblPostBreak" runat="server" Text=""></asp:Label>
								<asp:RadioButton ID="chkIDeal" runat="server" dbResource="lblIDeal" OnCheckedChanged="chkIDeal_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
								<asp:Label ID="lblIDealBreak" runat="server" Text=""></asp:Label>
								<asp:RadioButton ID="chkDDebit" runat="server" dbResource="lblDDebit" OnCheckedChanged="chkDDebit_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
								<asp:Label ID="lblDDebitBreak" runat="server" Text=""></asp:Label>
								<asp:RadioButton ID="chkEDebit" runat="server" dbResource="lblEDebit" OnCheckedChanged="chkEDebit_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
								<asp:RadioButton ID="chkAccount" runat="server" dbResource="lblAccount" OnCheckedChanged="chkAccount_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
								<asp:RadioButton ID="chkGiro" runat="server" dbResource="lblGiro" OnCheckedChanged="chkGiro_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
								<asp:Label ID="lblPaypalBreak" runat="server" Text=""></asp:Label>
								<asp:RadioButton ID="chkPaypal" runat="server" dbResource="lblPaypal" OnCheckedChanged="chkPaypal_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
								<asp:Label ID="lblFastpayBreak" runat="server" Text=""></asp:Label>
								<asp:RadioButton ID="chkFastpay" runat="server" dbResource="lblFastpay" OnCheckedChanged="chkFastpay_checkedChanged" AutoPostBack="true" Visible="false" GroupName="type" />
							</div>
						</td>
						<td width="40">&nbsp;</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td valign="middle" align="left" colspan="2">
			<table border="0">
				<tr>
					<td>
						<img src="Graphs/ICEPAY.png" width="53" height="45" />
						<%--<img src="https://secure.emotionalbliss.com/design/shop/images/icepay_logo.gif" />--%>
					</td>
					<td runat="server" id="tdPaypalLogo" visible="false">
						<!-- PayPal Logo -->
							<table border="0" cellpadding="10" cellspacing="0" align="center"><tr><td align="center"></td></tr>
							<tr><td align="center"><a href="#" onclick="javascript:window.open('https://www.paypal.com/cgi-bin/webscr?cmd=xpt/Marketing/popup/OLCWhatIsPayPal-outside','olcwhatispaypal','toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=yes, resizable=yes, width=400, height=350');"><img  src="https://www.paypal.com/en_US/i/logo/PayPal_mark_60x38.gif" border="0" alt="Acceptance Mark"></a></td></tr></table>
						<!-- PayPal Logo -->
					</td>
					<td>
						<img src="Graphs/AMEX.png" width="53" height="45" />
						<%--<img src="/design/shop/images/amex.gif" width="53" height="45" />--%>
					</td>
				</tr>
				<tr>
					<td colspan="3">
						<span id="siteseal"><script type="text/javascript" src="https://seal.godaddy.com/getSeal?sealID=QbIyELEjFibSysF9RVq6G8Sn4akVoKBMHNLZaOsZ8RPMGfaaznL2OWzEFIMF"></script></span> 
					</td>
				</tr>
			</table>
				
			</td>
		</tr>
	</table>    


	<div id="card-number"></div>
	<div id="card-expiry"></div>
	<div id="card-cvc"></div>

	<asp:Label ID="lblDeliveryText" runat="server" dbResource="lblDeliveryText"></asp:Label>
	<atlas:UpdatePanel ID="update1" runat="server">
		<ContentTemplate>    
		<table width="100%" cellpadding="0" cellspacing="0" border="0">
			<tr>
				<td align="left">
					<asp:label ID="lblError" runat="server" ForeColor="red"></asp:label>
					<div id="productinfocontainer">
						<table width="100%">
							<tr>
								<td width="80">&nbsp;</td>
								<td>
									<asp:Panel ID="pan3DSecure" runat="server" Visible="false">
										<iframe src="3DRedirect.aspx" name="3DIFrame" width="100%" height="500" frameborder="0">
										</iframe>
									</asp:Panel>
								</td>
								<td width="80">&nbsp;</td>
							</tr>
						</table>                     
							<div id="paymenttext">
								<asp:Panel ID="panCard" runat="Server" Visible="false">
									<div id="creditcards">		                    
										<b><asp:Literal ID="litCardDetails" runat="server"></asp:Literal></b>
									</div><br />

						
									

							        <table width="100%" border="0" cellpadding="5" cellspacing="5">
										<tr>
											<td>
													<asp:Label ID="lblCardType" runat="server" dbResource="lblCardType" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:DropDownList ID="drpCardType" runat="server">
													<asp:ListItem Text="Visa" Value="VISA"></asp:ListItem>
													<asp:ListItem Text="Visa Debit/Delta" Value="DELTA"></asp:ListItem>
													<asp:ListItem Text="Visa Electron" Value="UKE"></asp:ListItem>
													<asp:ListItem Text="Mastercard" Value="MC"></asp:ListItem>
													<asp:ListItem Text="Switch/Maestro" Value="SWITCH"></asp:ListItem>
													<asp:ListItem Text="Solo" Value="SOLO"></asp:ListItem>
													<asp:ListItem Text="American Express" Value="AMEX"></asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblCardNo" runat="server" dbResource="lblCardNo" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtCard" runat="server" MaxLength="26" Width="140" ValidationGroup="add"></asp:TextBox><span class="title3">*</span><asp:RegularExpressionValidator id="regex1" runat="server" ControlToValidate="txtCard" ValidationExpression="^\d{13,23}$" Display="dynamic" ValidationGroup="add" /><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="Server" ControlToValidate="txtCard" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblStartDate" runat="server" dbResource="lblStartDate" Font-Bold="true"></asp:Label><br />
												<asp:Label ID="lblSwitchOnly" runat="server" dbResource="lblSwitchOnly"></asp:Label>
											</td>
											<td>
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
											<td>
												<asp:Label ID="lblEndDate" runat="server" dbResource="lblEndDate" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:DropDownList ID="drpEndMonth" runat="Server" OnLoad="bindMonths" OnDataBound="drpEndMonth_dataBound">
												</asp:DropDownList>
												&nbsp;/&nbsp;
												<asp:DropDownList ID="drpEndYear" runat="server" OnLoad="drpEndYear_load"> 
												</asp:DropDownList><span class="title3">*</span>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblIssueNo" runat="server" dbResource="lblIssueNo" Font-Bold="true"></asp:Label><br />
												<asp:Label ID="lblSwitchOnly2" runat="server"></asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtIssue" runat="server" MaxLength="3" Width="36"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<nobr><asp:Label ID="lblCV2" runat="server" dbResource="lblCV2" Font-Bold="true"></asp:Label></nobr><br />
											</td>
											<td>
												<asp:TextBox ID="txtCV2" runat="server" MaxLength="4" Width="48"></asp:TextBox><span class="title3">*</span>                    
												<asp:RegularExpressionValidator id="regTxtCV2" runat="server" ControlToValidate="txtCV2" ValidationExpression="^[0-9]{3,4}$" Display="dynamic" ValidationGroup="add" />
												<asp:RequiredFieldValidator ID="reqTxtCV2" runat="server" ControlToValidate="txtCV2" ValidationGroup="add" Display="dynamic"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td colspan="2">
												<asp:Label ID="lblCV2Exp" runat="server" Font-Size="smaller" dbResource="lblCV2Exp"></asp:Label>
											</td>
										</tr>
									</table>
								</asp:Panel>
							</div>                        
							<div id="paymentselect">
								<asp:Panel ID="panBill" runat="Server" Visible="false">
									 <div id="delivery">
										<b><asp:Literal ID="litDelivery" runat="server"></asp:Literal></b>
									 </div>
									 <br class="clear" />
									 <table width="100%" border="0" cellpadding="5" cellspacing="5">
										<tr>
											<td>    
												<asp:label ID="lblBillName" runat="server" Text="Name on card:" dbResource="lblNameOnCard" Font-Bold="true"></asp:label>
												
											</td>
											<td>
												<asp:TextBox ID="txtBillName" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtBillName" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblEmail" runat="server" dbResource="lblEmail" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:Textbox id="txtEmail" runat="server" MaxLength="50"></asp:Textbox><span class="title3">*</span><asp:RequiredFieldValidator ID="req3" runat="server" ControlToValidate="txtEmail" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
												<asp:RegularExpressionValidator ID="regTxtEmail" runat="server" ControlToValidate="txtEmail" Display="dynamic" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$" ValidationGroup="add"></asp:RegularExpressionValidator>
												<dbResource="regTxtEmail"></dbResource>
											</td>
										</tr>
										<tr id="ES_Fiscal" runat="server" visible="false">
											<td>
												<asp:label ID="lblFiscal" runat="server" Text="VAT/NI Fiscal No" Font-Bold="true"></asp:label>
											</td>
											<td>
												<asp:TextBox ID="txtFiscal" runat="server" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtBillName" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>                                    
											</td>
										</tr>
										<tr>
											<td colspan="2">
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
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblAddress" runat="server" dbResource="lblAddress" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:TextBox id="txtBillAdd1" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtBillAdd1" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
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
												<asp:Label ID="lblCity" runat="server" dbResource="lblCity" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:TextBox id="txtBillAdd3" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="reqTxtBillAdd3" runat="server" ControlToValidate="txtBillAdd3" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblStateProvince" runat="server" dbResource="lblStateProvince" Text="State/Province:" Visible="false" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:DropDownList ID="drpState" runat="server" DataSourceID="sqlState" DataTextField="stateCode" DataValueField="state" Visible="false"></asp:DropDownList>
												<asp:TextBox id="txtBillAdd4" runat="server" MaxLength="50"></asp:TextBox>
											</td>
										</tr>
										<tr id="pBill" runat="server">
											<td>    
												<asp:Label ID="lblPostcode2" runat="server" dbResource="lblPostcode" Font-Bold="true"></asp:Label>    
											</td>
											<td>
												<asp:TextBox id="txtBillPostcode" runat="server" MaxLength="10"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="reqBillPostcode" runat="server" ControlToValidate="txtBillPostcode" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblPhone" runat="server" Text="Phone No:" dbResource="lblPhone" Font-Bold="true"></asp:Label> 
											</td>
											<td>
												<asp:TextBox id="txtPhone" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="reqTxtPhone" runat="server" ControlToValidate="txtPhone" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblCountry" runat="server" dbResource="lblCountry" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:Label id="txtBillCountry" runat="server" OnLoad="txtBillCountry_load"></asp:Label>
												<asp:DropDownList ID="drpEUState" runat="server" DataSourceID="sqlEUState" AppendDataBoundItems="true" DataTextField="stateEU" DataValueField="stateEU_ISO" AutoPostBack="false" Visible="false" OnDataBinding="dropdown_dataBinding">
												</asp:DropDownList>
												<asp:RequiredFieldValidator ID="reqEUState" runat="server" ValidationGroup="add" InitialValue="0" ControlToValidate="drpEUState" ErrorMessage="* EURequired" Display="Dynamic"></asp:RequiredFieldValidator>
											</td>
										</tr>
										<tr id="trAccount" runat="server" visible="false">
											<td>
												<asp:Label ID="lblAccount" runat="server" dbResource="lblAccountNo" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtAccount" runat="server" MaxLength="12"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblDeliveryInstructions" runat="server" Text="Special Delivery<br>Instructions:" dbResource="lblDeliveryInstructions" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:TextBox ID="txtDelivery" runat="server" MaxLength="200" CssClass="normaltextarea" TextMode="MultiLine" Rows="2" Columns="17"></asp:TextBox>
												<asp:Label ID="lblDeliveryError" runat="server"></asp:Label>
											</td>
										</tr>
										<tr>
											<td>
												<asp:Label ID="lblDOB" runat="server" dbResource="lblDOB" Font-Bold="true"></asp:Label>
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
											<td>
												<asp:Label ID="lblGender" runat="server" dbResource="lblGender" Font-Bold="true"></asp:Label>
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
											<td>
												<asp:Label ID="lblDeliverTo" runat="server" dbResource="lblDeliverTo" Font-Bold="true"></asp:Label>
											</td>
											<td>
												<asp:RadioButton ID="radYes" runat="server" dbResource="radYes" AutoPostBack="true" OnCheckedChanged="radYes_checkedChanged" GroupName="bill" />&nbsp;&nbsp;&nbsp;
												<asp:RadioButton ID="radNo" runat="server" dbResource="radNo" OnCheckedChanged="radNo_checkedChanged" GroupName="bill" AutoPostBack="true" />
											</td>
										</tr>
									 </table>
								</asp:Panel>
							
				<!-- **************************************************************** -->
							<asp:Panel ID="panShip" runat="Server" Visible="false">	                
								 <div id="delivery">
									<b><asp:Literal ID="litShippingAddress" runat="Server"></asp:Literal></b><br />
								 </div>
								 <br class="clear" />
								 <table width="100%" border="0" cellpadding="5" cellspacing="5">
									<tr>
										<td>    
											<asp:Label ID="lblName" runat="server" dbResource="lblName"></asp:Label>
											
										</td>
										<td>
											<asp:TextBox ID="txtShipName" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtShipName" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
										</td>
									</tr>
									<tr>
										<td colspan="2">
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
										</td>
									</tr>
									<tr>
										<td>
											<asp:Label ID="lblAddress2" runat="server"></asp:Label>
										</td>
										<td>
											<asp:TextBox id="txtShipAdd1" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtShipAdd1" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
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
											<asp:Label ID="lblCity2" runat="server"></asp:Label>
										</td>
										<td>
											<asp:TextBox id="txtShipAdd3" runat="server" MaxLength="50"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="reqTxtShipAdd3" runat="server" ControlToValidate="txtShipAdd3" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
										</td>	                                
									</tr>
									<tr>
										<td>
											<asp:Label ID="lblStateProvince2" runat="server" Text="State/Province:" Visible="false" Font-Bold="true"></asp:Label>
										</td>
										<td>
											<asp:DropDownList ID="drpState2" runat="server" DataSourceID="sqlState" DataTextField="stateCode" DataValueField="state" Visible="false"></asp:DropDownList>
											<asp:TextBox id="txtShipAdd4" runat="server" MaxLength="50"></asp:TextBox>
										</td>	                                
									</tr>
									<tr id="pShip" runat="server">
										<td>
											<asp:Label ID="lblPostcode4" runat="server"></asp:Label>
										</td>
										<td>
											<asp:TextBox id="txtShipPostcode" runat="server" MaxLength="10"></asp:TextBox><span class="title3">*</span><asp:RequiredFieldValidator ID="reqShipPostcode" runat="server" ControlToValidate="txtShipPostcode" Display="dynamic" ValidationGroup="add"></asp:RequiredFieldValidator><br />
										</td>	                                
									</tr>
									<tr>
										<td>
											<asp:Label ID="lblCountry2" runat="server"></asp:Label>
										</td>
										<td>
											<asp:Label id="txtShipCountry" runat="server"></asp:Label>
											<asp:DropDownList ID="drpEUState2" runat="server" DataSourceID="sqlEUState" AppendDataBoundItems="true" DataTextField="stateEU" DataValueField="stateEU_ISO" ValidationGroup="add" AutoPostBack="false" Visible="false" OnDataBinding="dropdown_dataBinding">
											</asp:DropDownList>
											<asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="drpEUState2" InitialValue="0" ErrorMessage="* Required" Display="Static" ValidationGroup="add"></asp:RequiredFieldValidator>
										</td>	                                
									</tr>
									<tr>
										<td>
										
										</td>
										<td>
											<asp:LinkButton id="btnViewBilling" Visible="false" runat="server" dbResource="btnViewBilling" onClick="btnViewBilling_click"></asp:LinkButton>
										</td>	                                
									</tr>
								</table>
							
						</asp:Panel>
						</div>
						<asp:Panel ID="panIDealBank" runat="server" Visible="false">
					<br /><br />
					<asp:Label ID="lblIDealBank" runat="server" dbResource="lblIDealBank"></asp:Label>
					<asp:DropDownList ID="drpIDealBank" runat="server" AutoPostBack="false" ValidationGroup="add">
						<asp:ListItem Text="" Value=""></asp:ListItem>
						<asp:ListItem Text="ABNAMRO" Value="ABNAMRO"></asp:ListItem>
						<asp:ListItem Text="FORTIS" Value="FORTIS"></asp:ListItem>
						<asp:ListItem Text="ING" Value="ING"></asp:ListItem>
						<asp:ListItem Text="RABOBANK" Value="RABOBANK"></asp:ListItem>
						<asp:ListItem Text="SNSBANK" Value="SNSBANK"></asp:ListItem>
					</asp:DropDownList>
					<asp:RequiredFieldValidator ID="reqDrpIDealBank" runat="server" ControlToValidate="drpIDealBank" ValidationGroup="add"></asp:RequiredFieldValidator>
				</asp:Panel>
				</div>
				</td>
			</tr>
			<tr>
				<td align="right">
					<table id="tblButtons" runat="server" visible="false">
						<tr>
							<td width="140">
									<div id="button">
										<asp:LinkButton ID="btnSubmitShipBill" runat="server" CssClass="sideNav" dbResource="lnkSubmitShipBill" OnClick="btnSubmitShipBill_click" Visible="false" ValidationGroup="add" OnClientClick='javascript:if(Page_ClientValidate("add")){this.style.display="none";showPleaseWaitMsg("block");}'>Complete Order</asp:LinkButton>
										<asp:LinkButton ID="btnSubmitBill" runat="server" CssClass="sideNav" dbResource="lnkSubmitBill" OnClick="btnSubmitBill_click" Visible="false" ValidationGroup="add" OnClientClick='javascript:if(Page_ClientValidate("add")){this.style.display="none";showPleaseWaitMsg("block");}'>Complete Order</asp:LinkButton>
										<asp:LinkButton ID="btnIcePay" runat="server" CssClass="sideNav" dbResource="lnkIcePay" OnClick="btnIcePay_click" Visible="false" ValidationGroup="add" OnClientClick='javascript:if(Page_ClientValidate("add")){this.style.display="none";showPleaseWaitMsg("block");}'>Complete Order</asp:LinkButton>
									</div>
								</div>
							</td>
							<td width="150">&nbsp;</td>
						</tr>
					</table>
					<span style="display:none;" id="plsWait">
						<asp:Literal ID="litPleaseWait" runat="server" OnLoad="litPleaseWait_load" Visible="true"></asp:Literal>....
						<asp:Image ID="imgPleaseWait2" runat="server" ImageUrl="~/images/loading.gif" />
					</span>
				</td>
			</tr>
		</table>
			
				
			
				
		  
	   </ContentTemplate>
		<Triggers>
			<atlas:AsyncPostBackTrigger ControlID="chkCC" EventName="CheckedChanged" />
			<atlas:AsyncPostBackTrigger ControlID="chkPost" EventName="CheckedChanged" />
			<atlas:AsyncPostBackTrigger ControlID="chkIdeal" EventName="CheckedChanged" />
			<atlas:AsyncPostBackTrigger ControlID="chkAccount" EventName="CheckedChanged" />
			<atlas:AsyncPostBackTrigger ControlID="chkDDebit" EventName="CheckedChanged" />
			<atlas:AsyncPostBackTrigger ControlID="chkEDebit" EventName="CheckedChanged" />
		</Triggers>
	</atlas:UpdatePanel>
	<asp:SqlDataSource ID="sqlState" runat="server" SelectCommand="procStatesSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlEUState" runat="server" SelectCommand="procStateEUSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
	<!-- Hidden resources -->
	<asp:Label ID="lblCardDetails" runat="server" dbResource="lblCardDetails" Visible="false"></asp:Label>
	<asp:Label ID="lblBillingDetails" runat="server" dbResource="lblBillingDetails" Visible="false"></asp:Label>
	<asp:Label ID="lblShippingAddress" runat="server" dbResource="lblShippingAddress" Visible="false"></asp:Label>
</asp:Content>