<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="orderView.aspx.vb" Inherits="affiliates_orderView" title="Untitled Page" Theme="WinXP_Blue" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	
	<atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
	<atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
		<ProgressTemplate>
			Please Wait....<img src="/images/loading.gif" width="16" height="16" />
		</ProgressTemplate>
	</atlas:UpdateProgress>
	<atlas:UpdatePanel ID="update1" runat="server">
		<ContentTemplate>
		<asp:Label ID="lblDebug" runat="server" ForeColor="Green"></asp:Label>
			<asp:Label ID="lblDenied" runat="server"></asp:Label>
			<asp:Label ID="lblPaymentPanelName" runat="server" Visible="false"></asp:Label>
			<asp:FormView ID="fvOrder" runat="server" DataSourceID="sqlOrder" OnDataBound="fvOrder_dataBound" Width="100%">
				<ItemTemplate>
					<asp:HiddenField ID="hidOrderID" runat="server" Value='<%# Eval("ID") %>' />
					<asp:HiddenField ID="hidOrderPrefix" runat="server" Value='<%# Eval("orderPrefix") %>' />
					<asp:HiddenField ID="hid3dTransaction" runat="server" Value='<%# Eval("3dTransaction") %>' />
					<table border="0" width="100%">
						<tr>
							<td align="left">
								<span onclick="window.print();" style="cursor:pointer;color:blue;">Print Page</span>
							</td>
							<td>
								<a href="orderList.aspx">Back to OrderList</a>
							</td>
							<td align="right">
								<span onclick="toggleOrderLog()" id="spanOrderLog" style="cursor:pointer;color:blue;">View Order Log</span>
							</td>
						</tr>
						<tr>
							<td id="tdOrderLog" style="display:none;" colspan="3">
								<asp:Panel ID="panLog" runat="server" Width="100%" BorderWidth="1">
									<asp:GridView ID="gvTrace" runat="Server" AutoGenerateColumns="false" DataSourceID="sqlOrderLog" SkinID="GridView" EmptyDataText="The order log is empty.<br> There must have been an error while placing the order." Width="100%">
										<Columns>
											<asp:BoundField HeaderText="Date" ItemStyle-VerticalAlign="top" DataField="date" ItemStyle-Width="20%" />
											<asp:TemplateField HeaderText="Action" ItemStyle-VerticalAlign="top">
												<ItemTemplate>
													<asp:Label ID="lblMessage" runat="server" Text='<%# Replace(Eval("message"),chr(10),"<br>") %>'></asp:Label>
												</ItemTemplate>
											</asp:TemplateField>
											<asp:BoundField HeaderText="Customer Contacted" DataField="contact" ItemStyle-Width="10%" />
											<asp:CheckBoxField DataField="customerVisible" HeaderText="Customer Visible" ItemStyle-VerticalAlign="top" ItemStyle-Width="10%" />
											<asp:BoundField HeaderText="User" DataField="userName" ItemStyle-VerticalAlign="top" ItemStyle-Width="10%" />
										</Columns>
									</asp:GridView>
									<table border="0" width="100%">
										<tr>
											<td valign="top" width="186">
												New Message:                                    
											</td>
											<td>
												<asp:TextBox ID="txtNewMessage" runat="server" CssClass="normaltextarea" TextMode="MultiLine" Rows="10" width="600"></asp:TextBox>
											</td>
										</tr>
										<tr>
											<td valign="top">
												Customer Can<br/>see comment:
											</td>
											<td>
												<asp:CheckBox ID="chkCustomerVisible" runat="server" />
											</td>
										</tr>
										<tr>
											<td valign="top">
												Customer<br />Contacted:
											</td>
											<td>
												<asp:DropDownList ID="drpContact" runat="server" AutoPostBack="false">
													<asp:ListItem Text="N/A" Value="N/A"></asp:ListItem>
													<asp:ListItem Text="Phoned" Value="Phoned"></asp:ListItem>
													<asp:ListItem Text="Emailed" Value="Emailed"></asp:ListItem>
												</asp:DropDownList>
											</td>
										</tr>
										<tr>
											<td>
												&nbsp;
											</td>
											<td>
												<asp:Button ID="btnMsgSubmit" runat="server" Text="Add Message" OnClick="btnMsgSubmit_click" />
											</td>
										</tr>
									</table>
								</asp:Panel>
							</td>
						</tr>
						</table>
					<table>
						<tr>            
							<td>
								<b>Order ID:</b>
							</td>
							<td>
								<asp:Label ID="lblOrderID" runat="Server" Text='<%# Eval("userOrderID") %>'></asp:Label>
							</td>
						</tr>
						<tr>
							
							<td>
								<b>Date:</b>
							</td>
							<td>
								<asp:Label ID="lblOrderDate" runat="server" Text='<%# showDate(Eval("orderDate")) %>'></asp:Label>
							</td>
						</tr>
						<tr>
							
							<td>
								<b>Currency:</b>
							</td>
							<td>
								<asp:Label ID="lblCurrency" runat="server" Text='<%# uCase(Eval("orderCurrency")) %>'></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								<b>Affiliate PO:</b>
							</td>
							<td valign="top">
								<asp:Label ID="Label4" runat="server" Text='<%# Eval("purchaseOrder") %>'></asp:Label>
							</td>
						</tr>
						<tr>
							<td>
								<b>Affiliate<br />ClickThrough:</b>
							</td>
							<td valign="top">
								<asp:Label ID="lblAffiliate" runat="server" Text='<%# Eval("affClick") %>'></asp:Label>
							</td>
						</tr>
						<tr>                    
							<td valign="top">
								<b>Bill To:</b>
							</td>
							<td valign="top">
								<asp:label id="lblBillName" runat="server" Text='<%# Eval("billName") & "<br>" %>'></asp:label>
								<asp:label id="lblBillAdd1" runat="server" Text='<%# Eval("billAdd1") & "<br>" %>'></asp:label>
								<asp:label id="lblBillAdd2" runat="server" Text='<%# Eval("billAdd2") & "<br>" %>'></asp:label>
								<asp:label id="lblBillAdd3" runat="server" Text='<%# Eval("billAdd3") & "<br>" %>'></asp:label>
								<asp:label id="lblBillAdd4" runat="server" Text='<%# Eval("billAdd4") & "<br>" %>'></asp:label>
								<asp:label id="lblBillAdd5" runat="server" Text='<%# Eval("billAdd5") & "<br>" %>'></asp:label>
								<asp:label id="lblBillPostcode" runat="server" Text='<%# Eval("billPostcode") & "<br>" %>'></asp:label>
								<asp:label id="lblBillCountry" runat="server" Text='<%# Eval("billCountry") & "<br>" %>'></asp:label>
								<asp:Label ID="lblPhone" runat="server" Text='<%# Eval("phone") & "<br>" %>'></asp:Label>
								<asp:HyperLink ID="lnkEmail" runat="server" Text='<%# Eval("email")%>' NavigateUrl='<%# "mailto:" & eval("email") %>'></asp:HyperLink><br />
								<asp:Label ID="lblFiscal" runat="server" Text='<%# Eval("fiscal") & "<br>" %>'></asp:Label>
								<asp:Label ID="lblDOB" runat="server" Text='<%# Eval("dob") %>'></asp:Label><br />
								<asp:Label ID="lblGender" runat="server" Text='<%# Eval("gender") %>'></asp:Label>
								
							</td>
							<td>&nbsp;</td>                    
							<td valign="top">
								<b>Ship To:</b>
							</td>
							<td valign="top">
								<asp:label id="lblShipName" runat="server" Text='<%# Eval("shipName") & "<br>" %>'></asp:label>
								<asp:label id="lblShipAdd1" runat="server" Text='<%# Eval("shipAdd1") & "<br>" %>'></asp:label>
								<asp:label id="lblShipAdd2" runat="server" Text='<%# Eval("shipAdd2") & "<br>" %>'></asp:label>
								<asp:label id="lblShipAdd3" runat="server" Text='<%# Eval("shipAdd3") & "<br>" %>'></asp:label>
								<asp:label id="lblShipAdd4" runat="server" Text='<%# Eval("shipAdd4") & "<br>" %>'></asp:label>
								<asp:label id="lblShipAdd5" runat="server" Text='<%# Eval("shipAdd5") & "<br>" %>'></asp:label>
								<asp:label id="lblShipPostcode" runat="server" Text='<%# Eval("shipPostcode") & "<br>" %>'></asp:label>
								<asp:label id="lblShipCountry" runat="server" Text='<%# Eval("shipCountry") & "<br>" %>'></asp:label>
							</td>

							<td>
								<table border="0">
									<tr>
										<td>
											<b>Status:</b>        
										</td>
										<td>
											<asp:Label ID="lblStatus" runat="server" Text='<%# Eval("orderStatus") %>' Visible="false"></asp:Label>
											<asp:DropDownList ID="drpStatus" runat="server" OnSelectedIndexChanged="drpStatus_selectedIndexChanged" AutoPostBack="true">
												<asp:ListItem Text="Cancelled" Value="cancelled"></asp:ListItem>
												<asp:ListItem Text="Placed" Value="Placed"></asp:ListItem>
												<asp:ListItem Text="Paid" Value="Paid"></asp:ListItem>
												<asp:ListItem Text="Deferred" Value="deferred"></asp:ListItem>
												<asp:ListItem Text="Failed" Value="Failed"></asp:ListItem>
												<asp:ListItem Text="Payment Pending" Value="PaymentPending"></asp:ListItem>
												<asp:ListItem Text="Complete" Value="Complete"></asp:ListItem>
											</asp:DropDownList>
											<asp:Label ID="lblStatusUpdate" runat="server" ForeColor="red"></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<b>Placed By:</b>
										</td>
										<td>
											<asp:Label ID="lblPlacedBy" runat="server" Text='<%# formatSource(Eval("orderSource")) %>'></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<b>Payment:</b>
										</td>
										<td>
											<asp:Label ID="lblPaymentMethod" runat="Server" Text='<%# formatPayment(Eval("paymentMethod")) %>'></asp:Label>
										</td>
									</tr>
									<tr runat="server" visible="false">
										<td>
											<b>Type:</b>        
										</td>
										<td>
											<asp:Label ID="lblType" runat="server" Text='<%# Eval("orderType") %>'></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											<b>Courier:</b>
										</td>
										<td>
											<asp:Label ID="lblCourier" runat="server" Text='<%# Eval("courier") %>'></asp:Label>
										</td>
									</tr>
									<tr>
										<td>
											&nbsp;
										</td>
										<td>
											<asp:HyperLink ID="lnkEdit" runat="server" Text="Edit Details"></asp:HyperLink>
										</td>
									</tr>
								</table>
							</td>
						</tr>
						<tr>            
							<td>
								<b>Instructions:</b>
							</td>
							<td>
								<asp:Label ID="lblDelivery" runat="Server" Text='<%# Eval("delivery") %>'></asp:Label>
							</td>
						</tr>
					</table>
					<asp:Panel ID="panCardDetails" runat="server" Visible="false">
						<hr>            	
						<b>Card Details:-<br>
						<table Border="0">
							<tr><td>Card Name:</td><td class="admin"><asp:Label ID="lblCardName" runat="Server" Text='<%# Eval("billName") %>'></asp:Label></td></tr>
							<tr><td>Card Type:</td> <td class='admin'><asp:Label ID="lblCardType" runat="server" Text='<%# Eval("cardType") %>'></asp:Label></td></tr>
							<tr><td>Card Num:</td>  <td class='admin'><asp:Label ID="lblCardNo" runat="server" Text='<%# Eval("cardNo") %>'></asp:Label>&nbsp;<asp:Label ID="lblEEE" runat="server" Text='<%# encryptCard(Eval("cardNo")) %>'></asp:Label>&nbsp;<asp:Button ID="btnUpdateCCEnc" runat="server" Text="Update CC Details" OnClick="btnUpdateCCEnc_click" /></td></tr>
							<tr><td visible="false" runat="server">Enc CC (ccEnc):</td>  <td class='admin' runat="server" visible="false"><asp:Label ID="Label1" runat="server" Text='<%# Eval("ccEnc") %>'></asp:Label></td></tr>
							<tr><td visible="false" runat="server">Decrypted (ccEnc):</td>  <td class='admin' runat="server" visible="false"><asp:Label ID="Label2" runat="server" Text='<%# decryptCardNo(Eval("ccEnc")) %>'></asp:Label></td></tr>
							<tr><td>Valid From:</td>  <td class='admin'><asp:Label ID="lblValidFrom" runat="server" Text='<%# Eval("cardStart") %>'></asp:Label></td></tr>
							<tr><td>Expires End:</td>  <td class='admin'><asp:Label ID="lblExpires" runat="server" Text='<%# Eval("cardExp") %>'></asp:Label></td></tr>
							<tr><td>Iss. Num:</td> <td class="admin"><asp:Label ID="lblIssue" runat="server" Text='<%# Eval("cardIssue") %>'></asp:Label></td></tr>
							<tr><td>Cv2:</td> <td class="admin"><asp:Label ID="lblCV2" runat="server" Text='<%# Eval("cardCV2") %>'></asp:Label></tr>
							<tr><td>3dSecure:</td> <td class="admin"><asp:Label ID="Label3" runat="server" Text='<%# showIs3DSecure(Eval("3dTransaction")) %>'></asp:Label></tr>
							<tr><td>&nbsp;</td><td></td></tr>
						</table>
					</asp:Panel>
					<asp:Panel ID="panEditShipping" runat="server" Visible="false">
						The 'Edit Shipping' feature is not yet implemented.
					</asp:Panel>
					<hr />
					<asp:Panel ID="panProducts" runat="server">
						<asp:GridView ID="gvProducts" runat="server">
							<Columns>
							</Columns>
						</asp:GridView>
					</asp:Panel>
					<asp:Panel ID="panTransHistory" runat="Server" Visible="false">
						<asp:GridView ID="gvHistory" runat="server" DataSourceID="sqlHistory" DataKeyNames="protxid" AutoGenerateColumns="false" SkinID="GridViewRedBG" OnDataBound="gvHistory_dataBound" OnSelectedIndexChanged="gvHistory_selectedIndexChanged" OnDataBinding="gvHistory_dataBound">
							<Columns>
								<asp:BoundField HeaderText="Date" DataField="transactionDate" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy hh-mm}" />
								<asp:BoundField ItemStyle-Width="40" />
								<asp:BoundField Headertext="Amount" DataField="amount" HtmlEncode="false" DataFormatString="{0:n2}" />
								<asp:BoundField ItemStyle-Width="40" />
								<asp:BoundField HeaderText="Currency" DataField="currency" />
								<asp:BoundField ItemStyle-Width="40" />
								<asp:BoundField HeaderText="Status" DataField="status" />
								<asp:BoundField ItemStyle-Width="40" />
								<asp:BoundField HeaderText="Tx Code" DataField="vendortxCode" />
								<asp:BoundField ItemStyle-Width="40" />
								<asp:TemplateField>
									<ItemTemplate>
										<asp:Button ID="btnRefund" runat="server" Text="Refund" CommandName="select" />
									</ItemTemplate>
								</asp:TemplateField>
							</Columns>
						</asp:GridView>
					</asp:Panel>
					<asp:Table ID="tblRefund" runat="server" Width="100%" Visible="false">
						<asp:TableRow>
							<asp:TableCell HorizontalAlign="right">
								Enter amount to refund: <asp:TextBox ID="txtRefund" runat="server" MaxLength="10" ValidationGroup="ref"></asp:TextBox>
								<asp:Button ID="btnConfirmRefund" runat="server" Text="Process Refund" OnClick="btnConfirmRefund_click" ValidationGroup="ref" /><br />
								<asp:RangeValidator ID="ranTxtRefund" runat="server" ControlToValidate="txtRefund" ValidationGroup="ref" ErrorMessage="* Invalid amount" Display="dynamic" Type="double" MinimumValue="0.01" MaximumValue="100000"></asp:RangeValidator>
								<asp:HiddenField ID="hidVPSTxId" runat="server" />
								<asp:HiddenField ID="hidSecurityKey" runat="server" />
								<asp:HiddenField ID="hidTxAuthNo" runat="server" />
								<asp:Label ID="lblRefundError" runat="server"></asp:Label>
							</asp:TableCell>
						</asp:TableRow>
					</asp:Table>
					<asp:Panel ID="panPaymentResults" runat="Server" Visible="true">
						<asp:Label ID="lblCCResults" runat="server" ForeColor="red"></asp:Label>
					</asp:Panel>
					<asp:panel ID="panPaymentCC" runat="server" Visible="false">
						<table Border="0">
							<tr>
								<td runat="server" id="tdProtxRelease" visible="false">
									<asp:Button ID="btnProtxRelease" runat="server" Text="Release Funds" OnClick="btnProtxRelease_click" />
									PRESSING THIS BUTTON WILL TAKE&nbsp;
									<font color="red">
									<asp:Label ID="lblCurrencySign2" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblOrderTotalProtxRelease" runat="server" Text='<%# Eval("orderTotal","{0:n2}") %>'></asp:Label>
									</font>&nbsp;
									FROM CUSTOMER ACCOUNT                        
								</td>
							</tr>
							<tr>
								<td runat="server" id="tdProtx">
									<asp:Button ID="btnProtx" runat="server" Text="Pay PROTX" OnClick="btnProtx_click" /><br />
									PRESSING THIS BUTTON WILL TAKE&nbsp;
									<font color="red">
									<asp:Label ID="lblCurrencySign" runat="server" Text='<%# Eval("currencySign") %>'></asp:Label><asp:Label ID="lblOrderTotalProtx" runat="server" Text='<%# Eval("orderTotal","{0:n2}") %>'></asp:Label>
									</font>&nbsp;
									FROM CUSTOMER ACCOUNT                        
								</td>
							</tr>
							<tr>
								<td>
									
								</td>
							</tr>
						</table>   
						<asp:Label ID="lblProtxError" runat="server" ForeColor="Red"></asp:Label>
									<asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>        		
						<table Border="0">
							<tr>
								<td runat="server" id="tdProtxExtra" visible="false">
									
									<asp:TextBox ID="txtPartAmount" runat="server" MaxLength="10" ValidationGroup="extraPayment"></asp:TextBox>&nbsp;
									<asp:Button ID="btnProtxExtra" runat="server" Text="Pay Protx Extra" OnClick="btnProtxExtra_click" ValidationGroup="extrapayment" /><br />
									<asp:DropDownList id="drpExtraVat" runat="server">
										<asp:ListItem Text="Is the extra charge VAT applicable ?" Value=""></asp:ListItem>
										<asp:ListItem Text="Yes" Value="true"></asp:ListItem>
										<asp:ListItem Text="No" Value="false"></asp:ListItem>
									</asp:DropDownList>
									<asp:RequiredFieldValidator ID="reqDrpExtraVat" runat="server" ControlToValidate="drpExtraVat" ValidationGroup="extrapayment" Display="static" ErrorMessage="* Required"></asp:RequiredFieldValidator>
									<br />
									<asp:RequiredFieldValidator ID="reqTxtPartAmount" runat="server" ControlToValidate="txtPartAmount" ErrorMessage="* Required<br>" Display="dynamic" ValidationGroup="extrapayment"></asp:RequiredFieldValidator>
									<asp:RangeValidator ID="ranTxtPartAmount" runat="server" ControlToValidate="txtPartAmount" ErrorMessage="* Invalid<br>" Display="dynamic" Type="Double" MinimumValue="1" MaximumValue="9999" ValidationGroup="extrapayment"></asp:RangeValidator>
									PRESSING THIS BUTTON TAKES EXTRA AMOUNT <font color="red"><asp:Literal ID="litCurrencyCode" runat="server" Text='<%# uCase(Eval("currencyCode")) %>'></asp:Literal></font> FROM CUSTOMER ACCOUNT!!
								</td>
							</tr>    
							   
						</table>
					</asp:panel>
					<asp:Panel ID="panPaymentCheque" runat="server" Visible="false">
						<asp:Label id="lblChequeNumber" runat="server" tetx="Cheque Number: "></asp:Label>
						<asp:TextBox ID="txtCheque" runat="server" MaxLength="20" Visible="false"></asp:TextBox>&nbsp;
						<asp:Label ID="lblChequeNo" runat="server" Text='<%# Eval("chequeNo") %>'></asp:Label>
						<asp:Button ID="btnChequeSubmit" runat="server" Text="Submit" OnClick="btnChequeSubmit_click" Visible="false" />
						<asp:Button ID="btnChequeCleared" runat="server" Text="Set Payment Cleared" OnClick="btnChequeCleared_click" Visible="false" />
					</asp:Panel>
					<asp:Panel ID="panPaymentAccount" runat="server" Visible="false">
						Account No:&nbsp;
						<asp:Label ID="lblAccountNo" runat="server" Text='<%# Eval("accountNo") %>'></asp:Label>
						<asp:Button ID="btnAccountSubmit" runat="server" Text="Set Payment Complete" OnClick="btnAccountSubmit_click" Visible="false" />
					</asp:Panel>
					<asp:Panel ID="panPaymentPaypal" runat="server" Visible="false">
						
					</asp:Panel>
					<asp:Panel ID="panPaymentIDeal" runat="server" Visible="false">
						iDeal Number:&nbsp;
						<asp:TextBox ID="txtIDeal" runat="server" MaxLength="20" Visible="false" />&nbsp;
						<asp:Button id="btnIDealSubmit" runat="server" Text="Submit" OnClick="btnIDealSubmit_click" Visible="false" />
						<asp:Label ID="lblIDealNo" runat="server" Text='<%# Eval("iDeal") %>'></asp:Label>
						<asp:Button ID="btnIDealPaymentCleared" runat="server" Text="Set Payment Cleared" OnClick="btnIDealPaymentCleared_click" Visible="false" />
					</asp:Panel>
					<asp:Panel ID="panAffAccountPayment" runat="server" Visible="false">
						<asp:Label ID="lblAffReadyToScan" runat="server" Text="Affiliate Order ready to scan." Visible="false"></asp:Label>
						<asp:Button ID="btnAffAccountSubmit" runat="server" Text="Auth Affiliate Order" OnClick="btnAffAccountSubmit_click" Visible="false" />
					</asp:Panel>
					<asp:Panel ID="panRefunds" runat="server" Visible="false">
						<br /><br />
						Refunds
						<table border="1">
							<tr>
								<td class='admin'>
									Transaction Code
								</td>
								<td class='admin'>
									Transaction Code Refunded Against
								</td>
								<td class='admin'>
									Status
								</td>
								<td class='admin'>
									Status Detail
								</td>
								<td class='admin'>
									Transaction Date
								</td>
								<td class='admin'>
									Amount
								</td>
							</tr>
						</table>
					</asp:Panel>    	    
					<br>
					<table cellpadding="0" cellspacing="0">
						<tr>
							<td>
								 <asp:GridView ID="gvOrderItems" runat="server" GridLines="none" DataKeyNames="orderItemID" DataSourceID="sqlOrderItems" AutoGenerateColumns="false" Width="562" OnRowUpdating="gvOrderItems_updating" OnRowUpdated="gvOrderItems_updated">
									<EditRowStyle HorizontalAlign="right" />
									<HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
									<Columns>
										<asp:BoundField HeaderText="Product" DataField="affProductName" ItemStyle-HorizontalAlign="left" ReadOnly="true" />
										<asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
										<asp:BoundField HeaderText="Qty" DataField="qty" ControlStyle-Width="40" />
										<asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
										<asp:BoundField HeaderText="Vat Rate" DataField="vatRate" ControlStyle-Width="60" />
										<asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
										<asp:BoundField HeaderText="Unit Price" DataField="price" ControlStyle-Width="60" HtmlEncode="false" DataFormatString="{0:n2}" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right" />
										<asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
										<asp:CommandField EditText="Edit" ShowEditButton="true" ShowCancelButton="false" ItemStyle-Width="60" />
									</Columns>
								</asp:GridView>     
								<asp:Label ID="lblErrorItems" runat="server"></asp:Label>
								<br />
							</td>
						</tr>
						<tr>
							<td>
							</td>
						</tr>
					</table>
					<table width="560" cellspacing="0" border="0">                
						<tr>
							<td>
								Subtotal:
							</td>
							<td align="right">
								<asp:Label ID="lblOrderGoodsTotal" runat="server" Text='<%# Eval("goods","{0:n2}") %>' ></asp:Label>
							</td>
							<td colspan="2" width="99">&nbsp;</td>
						</tr>
						<tr>
							<td>
								Shipping:
							</td>
							<td align="right">
								<asp:Label ID="lblShipping" runat="server" Text='<%# Eval("shipping","{0:n2}") %>'></asp:Label>
							</td>
							<td width="43">&nbsp;</td>
							<td width="56">
								<asp:LinkButton ID="LinkButton1" runat="server" Text="Edit" OnClick="lnkShippingEdit_click"></asp:LinkButton>
							</td>
						</tr>
						<tr id="trShipping" runat="server" visible="false">
							<td>
								<font color='blue'>New Shipping Value:</font>
							</td>
							<td align="right">
								<asp:textbox ID="txtShipping" runat="server" Width="40" Visible="true" ValidationGroup="shippingUpdate"></asp:textbox>
								<asp:RequiredFieldValidator ID="reqTxtShipping" runat="server" ControlToValidate="txtShipping" ErrorMessage="<br>Invalid" Display="dynamic" ValidationGroup="shippingUpdate"></asp:RequiredFieldValidator>
								<asp:RangeValidator ID="ranTxtShipping" runat="server" ControlToValidate="txtShipping" ErrorMessage="<br>Invalid" Type="double" MinimumValue="0" MaximumValue="9999" ValidationGroup="shippingUpdate" Display="dynamic"></asp:RangeValidator>
							</td>
							<td width="43">&nbsp;</td>
							<td width="56">
								<asp:LinkButton ID="btnShippingUpdate" runat="server" Text="Update" OnClick="btnShippingUpdate_click" ValidationGroup="shippingUpdate"></asp:LinkButton>
							</td>
						</tr>
						<tr>
							<td>
								Tax:
							</td>
							<td align="right">
								<asp:Label ID="lblTax" runat="server" Text='<%# Eval("vat","{0:n2}") %>'></asp:Label>
							</td>
							<td>&nbsp;</td>
							<td>
								<asp:LinkButton ID="lnkTaxEdit" runat="server" Text="Edit" OnClick="lnkTaxEdit_click"></asp:LinkButton>
							</td>
						</tr>
						<tr id="trTax" runat="server" visible="false">
							<td>
								<font color='blue'>New Goods Tax Value:</font>
							</td>
							<td align="right">
								<asp:textbox ID="txtTax" runat="server" Width="40" Visible="true" ValidationGroup="TaxUpdate"></asp:textbox>
								<asp:RequiredFieldValidator ID="reqTxtTax" runat="server" ControlToValidate="txtTax" ErrorMessage="<br>Invalid" Display="dynamic" ValidationGroup="TaxUpdate"></asp:RequiredFieldValidator>
								<asp:RangeValidator ID="ranTxtTax" runat="server" ControlToValidate="txtTax" ErrorMessage="<br>Invalid" Type="double" MinimumValue="0" MaximumValue="100" ValidationGroup="TaxUpdate" Display="dynamic"></asp:RangeValidator>
							</td>
							<td width="43">&nbsp;</td>
							<td width="56">
								
							</td>
						</tr>
						<tr id="trTax2" runat="server" visible="false">
							<td>
								<font color='blue'>New Shipping Tax Value:</font>
							</td>
							<td align="right">
								<asp:textbox ID="txtTaxShip" runat="server" Width="40" Visible="true" ValidationGroup="TaxUpdate"></asp:textbox>
								<asp:RequiredFieldValidator ID="reqTxtTaxShip" runat="server" ControlToValidate="txtTaxShip" ErrorMessage="<br>Invalid" Display="dynamic" ValidationGroup="TaxUpdate"></asp:RequiredFieldValidator>
								<asp:RangeValidator ID="ranTxtTaxShip" runat="server" ControlToValidate="txtTaxShip" ErrorMessage="<br>Invalid" Type="double" MinimumValue="0" MaximumValue="100" ValidationGroup="TaxUpdate" Display="dynamic"></asp:RangeValidator>
							</td>
							<td width="43">&nbsp;</td>
							<td width="56">
								<asp:LinkButton ID="btnTaxUpdate" runat="server" Text="Update" OnClick="btnTaxUpdate_click" ValidationGroup="TaxUpdate"></asp:LinkButton>
							</td>
						</tr>
						<tr>
							<td>
								Voucher Discount: <asp:HyperLink ID="lnkVoucher" runat="server" Target="_blank" Text='<%# Eval("voucherNumber") %>' OnLoad="lnkVoucher_load"></asp:HyperLink>
							</td>
							<td align="right">
								<asp:Label ID="lblDiscount" runat="server" Text='<%# Eval("discount","{0:n2}") %>'></asp:Label>
							</td>
							<td colspan="2">&nbsp;</td>
						</tr>
						<tr>
							<td colspan="4">&nbsp;</td>
						</tr>
						<tr>
							<td>
								<b>Total:</b>
							</td>
							<td align="right">
								<asp:Label ID="lblOrderTotal" runat="server" Text='<%# Eval("orderTotal","{0:n2}") %>'></asp:Label>
							</td>
							<td colspan="2">&nbsp;</td>
						</tr>
						<tr>
							<td colspan="2" align="right">
								<asp:button ID="btnEditShipping" Visible="false" runat="server" Text="Edit Subtotal/Shipping/VAT" OnClick="btnEditShipping_click" />
							</td>
							<td colspan="2">&nbsp;</td>
						</tr>
					</table>
				</ItemTemplate>
			</asp:FormView>
			<asp:Label ID="lblTest" runat="server"></asp:Label>
		</ContentTemplate>
	</atlas:UpdatePanel>
	
	<asp:SqlDataSource ID="sqlOrder" runat="server" SelectCommand="procShopOrderDetailsByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
		<SelectParameters>
			<asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="int32" />
			<asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlOrderItems" runat="server" SelectCommand="procShopOrderItemByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procShopOrderItemByItemIDUpdate" UpdateCommandType="StoredProcedure">
		<SelectParameters>
			<asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="int32" />
		</SelectParameters>
		<UpdateParameters>
			<asp:Parameter Name="orderItemID" Type="int32" />
			<asp:Parameter Name="qty" Type="int16" />
			<asp:Parameter Name="vatRate" Type="decimal" />
			<asp:Parameter Name="price" Type="decimal" />
		</UpdateParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlHistory" runat="server" SelectCommand="procProtxByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
		<SelectParameters>
			<asp:QueryStringParameter QueryStringField="id" Name="orderid" Type="int32" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlOrderLog" runat="server" SelectCommand="procOrderLogByOrderIDSelectAll" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
		<SelectParameters>
			<asp:QueryStringParameter QueryStringField="id" Name="id" Type="int32" />
		</SelectParameters>
	</asp:SqlDataSource>
	<asp:SqlDataSource ID="sqlOrderTotals" runat="server" SelectCommand="procShopOrderByIDCostsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="" UpdateCommandType="StoredProcedure">
		<SelectParameters>
			<asp:QueryStringParameter QueryStringField="id" Name="id" Type="int32" />
		</SelectParameters>
	</asp:SqlDataSource>
	
	<script language="javascript" type="text/javascript">
		function toggleOrderLog()
		{
			if(document.getElementById("tdOrderLog").style.display=="none")
			{
				document.getElementById("tdOrderLog").style.display="";
				document.getElementById("spanOrderLog").innerHTML="Hide Order Log";
			}else{
				document.getElementById("tdOrderLog").style.display='none';
				document.getElementById("spanOrderLog").innerHTML="View Order Log";
			}
		}
	</script>
</asp:Content>

