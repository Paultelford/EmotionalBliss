<%@ Page Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" Theme="WinXP_Blue" CodeFile="application.aspx.vb" Inherits="application" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="b2b" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="PageTitle" runat="server" dbResources="PageTitle" Visible="false"></asp:Label>
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
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
                    <td colspan="3">
                        <asp:Label ID="lblInvoiceAddress" runat="server" Font-Bold="true" dbResources="lblInvoiceAddress"></asp:Label>
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
                <tr id="trPayToName" runat="server" visible="false">
                    <td>&nbsp;</td>
                    <td align="right">
                    <nobr>
                        <asp:Label ID="lblPayToName" runat="server" dbResources="lblPayToName"></asp:Label>
                        <asp:RequiredFieldValidator ID="reqPayToName" runat="server" ControlToValidate="txtPayToName" Display="dynamic"></asp:RequiredFieldValidator>
                    </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPayToName" runat="server" MaxLength="50"></asp:TextBox>
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
                            <asp:RequiredFieldValidator ID="reqPostcode" runat="server" ControlToValidate="txtPostcode" Display="dynamic"></asp:RequiredFieldValidator>
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
                <tr>
                    <td colspan="3">
                        <asp:Label ID="lblSiteInfo" runat="server" dbResources="lblSiteInfo" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <tr id="trSiteName" runat="server" visible="false">
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblSiteName" runat="server" dbResources="lblSiteName"></asp:Label>
                            <asp:RequiredFieldValidator ID="reqSiteName" runat="server" ControlToValidate="txtSiteName" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>                
                    </td>
                    <td>
                        <asp:TextBox ID="txtSiteName" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblURL" runat="server" dbResources="lblURL"></asp:Label>
                            <asp:RequiredFieldValidator ID="reqURL" runat="server" ControlToValidate="txtURL" Display="dynamic"></asp:RequiredFieldValidator>
                        </nobr>                
                    </td>
                    <td>
                        <asp:TextBox ID="txtURL" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
                <tr id="trCategory" runat="server" visible="false">
                    <td colspan="3">
                        <asp:Label ID="lblCategory" runat="server" Font-Bold="true" dbResources="lblCategory"></asp:Label>
                    </td>
                </tr>
                <tr id="trRelated" runat="server" visible="false">
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblRelated" runat="server" dbResources="lblRelated"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:RadioButton ID="radRelated" runat="server" GroupName="category" Checked="true"></asp:RadioButton>
                    </td>
                </tr>
                <tr id="trNonRelated" runat="server" visible="false">
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblNonRelated" runat="server" dbResources="lblNonRelated"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:RadioButton ID="radNonRelated" runat="server" GroupName="category" ></asp:RadioButton>
                    </td>
                </tr>
                <tr id="trNonProfit" runat="server" visible="false">
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblNonProfit" runat="server" dbResources="lblNonProfit"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:RadioButton ID="radNonProfit" runat="server" GroupName="category" ></asp:RadioButton>
                    </td>
                </tr>
                <tr id="trCharity" runat="server" visible="false">
                    <td>&nbsp;</td>
                    <td align="right">
                        <nobr>
                            <asp:Label ID="lblCharity" runat="server" dbResources="lblCharity"></asp:Label>
                        </nobr>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCharity" runat="server" MaxLength="50"></asp:TextBox>
                    </td>
                </tr>
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
                        <br />
                        <asp:Label ID="lblAgreement" runat="server" dbResources="lblAgreement"></asp:Label>
                        <asp:panel ID="panErrors" runat="server">
                            <asp:Label ID="lblErrorUsername" runat="server" dbResources="lblErrorUsername" ForeColor="red" Visible="false"></asp:Label>
                            <asp:Label ID="lblErrorPassword" runat="server" dbResources="lblErrorPassword" ForeColor="red" Visible="false"></asp:Label>
                        </asp:panel>
                        <br />
                        <asp:ImageButton ID="btnAgree" runat="server" dbResources="btnAgree" OnClick="btnAgree_click" /><br /><br />
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
        </ContentTemplate>
    </atlas:UpdatePanel>    
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <asp:SqlDataSource ID="sqlCountry" runat="server" SelectCommand="procCountrySelectNoEEC" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>

