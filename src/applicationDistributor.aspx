<%@ Page Title="" Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="applicationDistributor.aspx.vb" Inherits="applicationDistributor" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="b2b" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentTop" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            <asp:Label ID="lblIntro" runat="server" dbResources="lblIntro"></asp:Label>
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
                            <asp:label ID="lblTitle" runat="server" dbResources="lblTitle" />
                            <asp:RequiredFieldValidator ID="reqTitle" runat="server" ControlToValidate="txtTitle" dbResources="text_Required" Display="dynamic"></asp:RequiredFieldValidator></nobr></td>
                    <td width="160">
                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="20"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:label ID="lblFirstName" runat="server" dbResources="lblFirstName" />
                            <asp:RequiredFieldValidator ID="reqFirstName" runat="server" ControlToValidate="txtFirstName" Display="dynamic"></asp:RequiredFieldValidator>
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
                            <asp:RequiredFieldValidator ID="reqLastName" runat="server" ControlToValidate="txtLastName" Display="dynamic"></asp:RequiredFieldValidator>
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
                            <asp:RequiredFieldValidator ID="reqPhoneNo" runat="server" ControlToValidate="txtPhoneNo" Display="dynamic"></asp:RequiredFieldValidator>
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
                            <asp:RequiredFieldValidator ID="reqEmail" runat="server" ControlToValidate="txtEmail" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right" valign="top">
                        <nobr>
                            Where does your your main customer base lie?
                        </nobr>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radCustomerBase" runat="server" AutoPostBack="false">
                            <asp:ListItem Text="Drugstores"></asp:ListItem>
                            <asp:ListItem Text="Lingerie Boutiques"></asp:ListItem>
                            <asp:ListItem Text="Medical Supply"></asp:ListItem>
                            <asp:ListItem Text="Adult Retail"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqCustomerBase" runat="server" ControlToValidate="radCustomerBase" Display="Dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right" valign="top">
                        <nobr>
                            What kind of products do you mostly carry?
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
                    <td>&nbsp;</td>
                    <td align="right" valign="top">
                        <nobr>
                            What geographical area does your main customer base cover ?
                        </nobr>
                        <td>
                            <asp:TextBox ID="txtArea" runat="server" MaxLength="50"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqArea" runat="server" ControlToValidate="txtArea" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblCompany" runat="server" dbResources="lblCompany"></asp:Label>
                            <asp:RequiredFieldValidator ID="reqCompany" runat="server" ControlToValidate="txtCompany" Display="dynamic"></asp:RequiredFieldValidator>
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
                            <asp:RequiredFieldValidator ID="reqAdd1" runat="server" ControlToValidate="txtAdd1" Display="dynamic"></asp:RequiredFieldValidator>
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
                            <asp:Label ID="lblVatNumber" runat="Server" dbResources="lblVatNumber"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVatNumber" runat="server" MaxLength="50"></asp:TextBox>
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
                            <asp:RequiredFieldValidator ID="reqUsername" runat="server" ControlToValidate="txtUsername" Display="dynamic"></asp:RequiredFieldValidator>
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
                            <asp:RequiredFieldValidator ID="reqPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="* Must be more than 4 characters" Display="dynamic" ValidationExpression="[a-zA-Z0-9]{5,19}"></asp:RegularExpressionValidator>
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
                            <asp:RequiredFieldValidator ID="reqConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
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
                        <asp:LinkButton ID="lnkAgree" runat="server" OnClick="btnAgree_click" CssClass="SubmitRollover" ToolTip="Submit application" dbResource="cssSubmit"></asp:LinkButton>
                        <dbResource="ttSubmit"></dbResource>
                        <br />
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

