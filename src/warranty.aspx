<%@ Page Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="warranty.aspx.vb" Inherits="warranty" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="home" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="Server">
        <ContentTemplate>
                <br />
                <asp:Panel ID="pan1" runat="server" CssClass="pan">
                   <asp:Label ID="lblIntroduction" runat="server" dbResource="Introduction"></asp:Label><br /><br />
                    <table border="1" rules="none" runat="server" id="tblWarranty" width="580">
                        <tr>
                            <td>
                                <asp:Label id="lblProduct" runat="server" dbResource="Product"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:DropDownList ID="drpProduct" runat="server">
                                    <asp:ListItem Text="Chandra" Value="Chandra"></asp:ListItem>
                                    <asp:ListItem Text="Femblossom" Value="Femblossom"></asp:ListItem>
                                    <asp:ListItem Text="Isis" Value="Isis"></asp:ListItem>
                                    <asp:ListItem Text="Jasmine" Value="Jasmine"></asp:ListItem>
                                    <asp:ListItem Text="Womolia" Value="Womolia"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblPurchaseDate" runat="server" dbResource="PurchaseDate"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:DropDownList ID="drpDay" runat="server"></asp:DropDownList>&nbsp;
                                <asp:DropDownList ID="drpMonth" runat="server"></asp:DropDownList>&nbsp;
                                <asp:DropDownList ID="drpYear" runat="server"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                            <td><asp:Label ID="lblPurchaseDateError" runat="Server" ForeColor="red"></asp:Label></td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblPurchasePrice" runat="server" dbResource="PurchasePrice"></asp:Label>
                                <asp:Label ID="lblCurrencySign" runat="server" Visible="false"></asp:Label>
                            </td>
                            <td>
                                <asp:RequiredFieldValidator ID="reqTxtPrice" runat="server" ControlToValidate="txtPrice" Display="static"></asp:RequiredFieldValidator>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtPrice" runat="server" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td width="340">
                                <asp:Label id="lblShopName" runat="server" dbResource="ShopName"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:RequiredFieldValidator ID="reqPan1ShopName" runat="server" ControlToValidate="txtShopName" Display="static"></asp:RequiredFieldValidator>
                            </td>
                            <td valign="top">
                                <asp:TextBox id="txtShopName" runat="Server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblShopLocation" runat="server" dbResource="ShopLocation"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:RequiredFieldValidator ID="reqPan1ShopLoc" runat="server" ControlToValidate="txtShopLoc" Display="static"></asp:RequiredFieldValidator>
                            </td>
                            <td valign="top">
                                <asp:TextBox id="txtShopLoc" runat="Server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblShopCountry" runat="server" dbResource="ShopCountry"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:DropDownList ID="drpShopCountry" runat="Server" DataTextField="countryName" DataValueField="countryCode" DataSourceID="sqlCountry" OnDataBound="drpShopCountry_dataBound"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblFirstname" runat="server" dbResource="Firstname"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:RequiredFieldValidator ID="pan1ReqFirstname" runat="Server" ControlToValidate="txtFirstname" Display="Static"></asp:RequiredFieldValidator>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtFirstname" runat="server" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblSurname" runat="server" dbResource="Surname"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:RequiredFieldValidator ID="pan1ReqSurname" runat="Server" ControlToValidate="txtSurname" Display="Static" />
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtSurname" runat="server" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblGender" runat="server" dbResource="Gender"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:DropDownList ID="drpGender" runat="Server">
                                    <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                    <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblAddress" runat="server" dbResource="Address"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:RequiredFieldValidator ID="pan1ReqAddress" runat="Server" ControlToValidate="txtAddress" ErrorMessage="Required field " Display="Static" />
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtAddress" runat="Server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblCity" runat="server" dbResource="City"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:RequiredFieldValidator ID="pan1ReqTown" runat="Server" ControlToValidate="txtTown" ErrorMessage="Required field " Display="Static" />
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtTown" runat="Server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblPostcode" runat="server" dbResource="Postcode"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:RequiredFieldValidator ID="panReqPostcode" runat="Server" ControlToValidate="txtPostcode" ErrorMessage="Required field " Display="Static" />
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtPostcode" runat="Server" MaxLength="10"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblCountry" runat="server" dbResource="Country"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:DropDownList ID="drpCountry" runat="Server" DataTextField="countryName" DataValueField="countryCode" DataSourceID="sqlCountry" OnDataBound="drpCountry_dataBound"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblDOB" runat="server" dbResource="DateOfBirth"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:DropDownList ID="drpDOBDay" runat="server"></asp:DropDownList>&nbsp;
                                <asp:DropDownList ID="drpDOBMonth" runat="server"></asp:DropDownList>&nbsp;
                                <asp:DropDownList ID="drpDOBYear" runat="server"></asp:DropDownList>
                                <asp:Label ID="lblDOBDateError" runat="Server" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblEmail" runat="server" dbResource="EmailAddress"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:RequiredFieldValidator ID="pan1Req1Email" runat="Server" ControlToValidate="txtEmail" Display="dynamic" />
                                <asp:RegularExpressionValidator ID="regEmail" runat="server" ControlToValidate="txtEmail" Display="Dynamic" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$"></asp:RegularExpressionValidator>
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtEmail" runat="server" MaxLength="50"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label id="lblPhone" runat="server" dbResource="Telephone"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:TextBox ID="txtPhone" runat="server" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblYesIAgree" runat="server" dbResource="YesIAgree"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:CheckBox ID="chkAgree" runat="server" />
                                <asp:Label ID="lblAgreeError" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <asp:Label ID="lblAgreementNote" runat="server" dbResource="AgreementNote"></asp:Label>
                            </td>
                        </tr>
                        <tr runat="server" visible="false">
                            <td>
                                <asp:Label ID="lblPromo" runat="server" dbResource="ReceivePromotions"></asp:Label>
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:CheckBox ID="chkPromo" runat="Server" />
                            </td>
                        </tr>
                        <tr><td colspan="3">&nbsp;</td></tr>
                        <tr>
                            <td>
                                
                            </td>
                            <td>&nbsp;</td>
                            <td valign="top">
                                <asp:ImageButton ID="btnRegister" runat="server" dbResource="RegisterProduct" OnClick="btnRegister_click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel><asp:Label ID="lblComplete" runat="server"></asp:Label></ContentTemplate></atlas:UpdatePanel><table border="0" width="600">
            <tr>
                <td align="right">
                    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
                        <ProgressTemplate>
                            Please Wait....<img src="/images/loading.gif" width="16" height="16" /></ProgressTemplate></atlas:UpdateProgress>
                            <br /><br />
                    <asp:Label ID="lblCriticalError" runat="Server" ForeColor="red" dbResource="CriticalError"></asp:Label>
                    <dbResource="EmailReply"></dbResource>
                    <dbResource="CompleteMessage"></dbResource>
                    <dbResource="ShopNameConfirmationEmail"></dbResource>
                    <dbResource="ShopLocationConfirmationEmail"></dbResource>
                    <dbResource="ShopCountryConfirmationEmail"></dbResource>
                    <dbResource="ProductConfirmationEmail"></dbResource>
                    <dbResource="PurchaseDateConfirmationEmail"></dbResource>
                    <dbResource="PurchasePriceConfirmationEmail"></dbResource>                    
                    <dbResource="RegistrationDate"></dbResource>
                    <dbResource="RegistrationID"></dbResource>
                    <dbResource="CustomerInfo"></dbResource>
                </td></tr></table><asp:SqlDataSource ID="sqlCountry" runat="Server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

