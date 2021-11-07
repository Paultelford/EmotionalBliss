<%@ Page Title="" Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="applicationWholesale.aspx.vb" Inherits="applicationWholesale" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="b2b" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentTop" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <asp:Label ID="lblIntro" runat="server" dbResources="lblIntro" Visible="true"></asp:Label>
            <table border="0" width="600" id="tblApplicationForm" runat="server">
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblContactDetails" runat="server" dbResources="lblContactDetails" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td width="40">&nbsp;</td>
                    <td align="right" width="400">
                        <nobr>
                            <asp:label ID="lblCompanyPosition" runat="server" dbResources="lblCompanyPosition" />
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqCompanyPosition" runat="server" ControlToValidate="txtCompanyPosition" dbResources="text_Required" Display="dynamic"></asp:RequiredFieldValidator></nobr></td>
                    <td width="160">
                        <asp:TextBox ID="txtCompanyPosition" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td width="40">&nbsp;</td>
                    <td align="right" width="400">
                        <nobr>
                            <asp:label ID="lblTitle" runat="server" dbResources="lblTitle" />
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqTitle" runat="server" ControlToValidate="txtTitle" dbResources="text_Required" Display="dynamic"></asp:RequiredFieldValidator></nobr></td>
                    <td width="160">
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:label ID="lblFirstName" runat="server" dbResources="lblFirstName" />
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqFirstName" runat="server" ControlToValidate="txtFirstName" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <norb>
                            <asp:Label ID="lblLastname" runat="server" dbResources="lblLastname"></asp:Label>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqLastName" runat="server" ControlToValidate="txtLastName" Display="dynamic"></asp:RequiredFieldValidator>
                        </norb>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLastName" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblPhoneNo" runat="server" dbResources="lblPhoneNo"></asp:Label>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqPhoneNo" runat="server" ControlToValidate="txtPhoneNo" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPhoneNo" runat="server" MaxLength="30"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:label id="lblEmail" runat="server" dbResources="lblEmail"></asp:label>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqEmail" runat="server" ControlToValidate="txtEmail" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblCompanyDetails" runat="server" Font-Bold="true" dbResources="lblCompanyDetails">Company Details</asp:Label>
                    </td>
                </tr>
                <tr id="trPayToName" runat="server" visible="false">
                    <td>&nbsp;</td>
                    <td align="right">
                    <nobr>
                        <asp:Label ID="lblPayToName" runat="server" dbResources="lblPayToName"></asp:Label>
                        <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqPayToName" runat="server" ControlToValidate="txtPayToName" Display="dynamic"></asp:RequiredFieldValidator>
                    </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPayToName" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right" valign="top">
                        <nobr>
                            How many stores does your company have?
                        </nobr>
                        <td>
                            <asp:RadioButtonList id="radStores" runat="server" AutoPostBack="false">
                                <asp:ListItem Text="1 or 2"></asp:ListItem>
                                <asp:ListItem Text="2 - 5"></asp:ListItem>
                                <asp:ListItem Text="5 - 10"></asp:ListItem>
                                <asp:ListItem Text="10 - 40"></asp:ListItem>
                                <asp:ListItem Text="40 - 100"></asp:ListItem>
                                <asp:ListItem Text="100 - 250"></asp:ListItem>
                                <asp:ListItem Text="250+"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqStores" runat="server" ControlToValidate="radStores" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right" valign="top">
                        <nobr>
                            What kind of products does your retail outlet mostly sell?
                        </nobr>
                        <td>
                            <asp:CheckBoxList ID="chkSell" runat="server">
                                <asp:ListItem Text="Intimate Apparal"></asp:ListItem>
                                <asp:ListItem Text="Toys"></asp:ListItem>
                                <asp:ListItem Text="Massagers"></asp:ListItem>
                                <asp:ListItem Text="Books"></asp:ListItem>
                                <asp:ListItem Text="DVDs"></asp:ListItem>
                                <asp:ListItem Text="Feminine Hygiene Products"></asp:ListItem>
                                <asp:ListItem Text="Condoms"></asp:ListItem>
                                <asp:ListItem Text="Incontinence Materials or Aids"></asp:ListItem>
                                <asp:ListItem Text="Medical Supplies"></asp:ListItem>
                                <asp:ListItem Text="Make-Up"></asp:ListItem>
                                <asp:ListItem Text="Body Lotions"></asp:ListItem>
                                <asp:ListItem Text="Lubricants"></asp:ListItem>
                                <asp:ListItem Text="Other"></asp:ListItem>                                
                            </asp:CheckBoxList>
                            <table><tr><td><font size='-2'>(Please explain. Max 200 chars)</font></td></tr></table>
                            <asp:TextBox ID="txtSellOther" runat="server" TextMode="MultiLine" Width="200" Height="100" CssClass="normaltextarea"></asp:TextBox>
                        </td>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblInvoiceAddress" runat="server" Font-Bold="true" dbResources="lblInvoiceAddress"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblCompany" runat="server" dbResources="lblCompany"></asp:Label>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red"  ID="reqCompany" runat="server" ControlToValidate="txtCompany" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>                
                    </td>
                    <td>
                        <asp:TextBox ID="txtCompany" runat="server" MaxLength="100"></asp:TextBox>
                    </td>
                </tr>
                 <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblAdd1" runat="Server" dbResources="lblAdd1"></asp:Label>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqAdd1" runat="server" ControlToValidate="txtAdd1" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAdd1" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblAdd2" runat="Server" dbResources="lblAdd2"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAdd2" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblAdd3" runat="Server" dbResources="lblAdd3"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAdd3" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblPostcode" runat="Server" dbResources="lblPostcode"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPostcode" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblSiteURL" runat="server" dbResources="lblSiteURL"></asp:Label>
                        </nobr>                
                    </td>
                    <td>
                        <asp:TextBox ID="txtSiteURL" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblCountry" runat="Server" dbResources="lblCountry"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="sqlCountry" DataTextField="countryName" DataValueField="countryCode"></asp:DropDownList>
                    </td>
                </tr>              
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblVat" runat="server" dbResources="lblVat"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVat" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblDeliveryAddress" runat="server" Font-Bold="true" dbResources="lblDeliveryAddress"></asp:Label><br />
                        <asp:CheckBox ID="chkSameDelivery" runat="server" AutoPostBack="false" />
                        <asp:Label ID="lblDeliveryInstructions" runat="server" Font-Bold="true" dbResources="lblDeliveryInstructions"></asp:Label>                
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <asp:Label ID="lblToAdd1" runat="server" dbResources="lblToAdd1"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToAdd1" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <asp:Label ID="lblToAdd2" runat="server" dbResources="lblToAdd2"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToAdd2" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <asp:Label ID="lblToAdd3" runat="server" dbResources="lblToAdd3"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToAdd3" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <asp:Label ID="lblToPostcode" runat="server" dbResources="lblToPostcode"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtToPostcode" runat="server" MaxLength="10"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <asp:Label ID="lblToCountry" runat="server" dbResources="lblToCountry"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpToCountry" runat="server" DataSourceID="sqlCountry" DataTextField="countryName" DataValueField="countryCode"></asp:DropDownList>
                    </td>
                </tr>
                <!-- Credentials -->
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblCredentials" runat="server" Font-Bold="true" dbResources="lblCredentials"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblUsername" runat="Server" dbResources="lblUsername"></asp:Label>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqUsername" runat="server" ControlToValidate="txtUsername" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtUsername" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblPassword" runat="Server" dbResources="lblPassword"></asp:Label>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regPassword" runat="server" ValidationGroup="app" ControlToValidate="txtPassword" ErrorMessage="* Must be more than 4 characters" Display="dynamic" ValidationExpression="[a-zA-Z0-9]{5,19}"></asp:RegularExpressionValidator>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPassword" runat="server" MaxLength="20" TextMode="password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblConfirmPassword" runat="Server" dbResources="lblConfirmPassword"></asp:Label>
                            <asp:RequiredFieldValidator ValidationGroup="app" ForeColor="Red" ID="reqConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtConfirmPassword" runat="server" MaxLength="20" TextMode="password"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblAgreement" runat="server" dbResources="lblAgreement" Visible="false"></asp:Label>
                        <asp:panel ID="panErrors" runat="server">
                            <asp:Label ID="lblErrorUsername" runat="server" dbResources="lblErrorUsername" ForeColor="red" Visible="false"></asp:Label>
                            <asp:Label ID="lblErrorPassword" runat="server" dbResources="lblErrorPassword" ForeColor="red" Visible="false"></asp:Label>
                        </asp:panel>
                        <br />
                        <asp:ImageButton ID="btnAgree" runat="server" dbResources="btnAgree" OnClick="btnAgree_click" ValidationGroup="app" /><br />
                        <asp:Label ID="lblAgree" runat="server" dbResources="lblAgree"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblComplete" runat="Server" dbResources="lblComplete" Visible="false"></asp:Label>
            <script language="javascript" type="text/javascript">
                function copyData(ad1,ad2,ad3,adPostcode,adDrop,a1,a2,a3,aPostcode,aDrop)
                {
                    document.getElementById(ad1).value=document.getElementById(a1).value;
                    document.getElementById(ad2).value=document.getElementById(a2).value;
                    document.getElementById(ad3).value=document.getElementById(a3).value;
                    document.getElementById(adPostcode).value=document.getElementById(aPostcode).value;
                    document.getElementById(adDrop).value=document.getElementById(aDrop).value;
                }
            </script>

    <asp:SqlDataSource ID="sqlCountry" runat="server" SelectCommand="procCountrySelectNoEEC" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>

