<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Trace="false" AutoEventWireup="false" CodeFile="manager.aspx.vb" Inherits="maintenance_manager" title="User management" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblAccessDenied" runat="server" Visible="false">
                You do not have authorization to view this page.
            </asp:Label>
            <table border="0" width="100%">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkNewUser" runat="server" Text="Add new user" OnClick="lnkNewUser_click"></asp:LinkButton>    
                    </td>
                    <td align="right">
                        <asp:LinkButton id="lnkReActivate" runat="server" Text="Re-Activate Affiliates/Distributors" OnClick="lnkReActivate_click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="panUsers" runat="server" BorderWidth="1">
                <table border="0" width="100%">
                    <tr>
                        <td>
                            Current Maintenance Users:
                        </td>
                        <td width="80">&nbsp;</td>
                        <td>
                            Current&nbsp;
                            <asp:DropDownList id="drpType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpType_selectedIndexChanged">
                                <asp:ListItem Text="Distributors" Value="distributors"></asp:ListItem>
                                <asp:ListItem Text="Affiliates" Value="affiliates"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:GridView ID="gvMaintenance" runat="server" AutoGenerateColumns="false" DataKeyNames="username" OnLoad="gvMaintenance_load" GridLines="none" OnRowDataBound="gvMaintenance_rowDataBound" OnSelectedIndexChanged="gvMaintenence_selectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="Username">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkUsername" runat="server" CommandName="select" Text='<%# Eval("username") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:TemplateField HeaderText="Locked Out">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLockedOut" runat="server" Checked='<%# Eval("isLockedOut") %>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:TemplateField HeaderText="Last Login">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastLogin" runat="server" Text='<%# showDate(Eval("lastLoginDate")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                        <td>&nbsp;</td>
                        <td valign="top">
                            <asp:GridView ID="gvDistributors" runat="server" AutoGenerateColumns="false" DataKeyNames="username" OnLoad="gvDistributors_load" GridLines="none" OnRowDataBound="gvDistributors_rowDataBound" OnSelectedIndexChanged="gvDistributors_selectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="Username">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkUsername" runat="server" CommandName="select" Text='<%# Eval("username") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:TemplateField HeaderText="Locked Out">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkLockedOut" runat="server" Checked='<%# Eval("isLockedOut") %>' Enabled="false" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:TemplateField HeaderText="Last Login">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastLogin" runat="server" Text='<%# showDate(Eval("lastLoginDate")) %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </asp:Panel><br />
            <asp:Panel ID="panUserEdit" runat="server" Visible="false" BorderWidth="1">
                <table border="1" width="100%" cellspacing="0" cellspacing="0">
                    <tr>    
                        <td width="55%" valign="top">
                            Click a checkbox to alter permissions:
                            <table border="0" width="100%">
                                <tr>
                                    <td width="20%">
                                        <b>Username:</b>
                                    </td>
                                    <td width="10%">&nbsp;</td>
                                    <td width="20%">
                                        <asp:Label ID="lblUsername" runat="server"></asp:Label>
                                    </td>
                                    <td width="50%">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <b>Password:</b>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td colspan="2">
                                        <asp:LinkButton ID="lnkShowPassword" runat="server" Text="Show" OnClick="btnShowPassword_click"></asp:LinkButton>
                                        <asp:Table ID="tblPassword" runat="server" Visible="false">
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:Label ID="lblPassword" runat="server"></asp:Label>    
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell>
                                                    <asp:LinkButton ID="lnkChangePassword" runat="server" Text="Change Password" OnClick="lnkChangePassword_click"></asp:LinkButton>
                                                        <asp:Table ID="tblNewPasssword" runat="server" Visible="false">
                                                            <asp:TableRow>
                                                                <asp:TableCell>
                                                                    New Password: <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="20" ValidationGroup="newPass"></asp:TextBox> <asp:RegularExpressionValidator ID="regTxtNewPassword" runat="server" ControlToValidate="txtNewPassword" ValidationGroup="newPass" ValidationExpression="[a-zA-Z-0-9]{5,20}" ErrorMessage="* Password must be 5 - 20 characters" Display="dynamic"></asp:RegularExpressionValidator><br />
                                                                    <asp:Button ID="btnConfirmNewPass" runat="server" Text="Confirm" OnClick="btnConfirmNewPass_click" ValidationGroup="newPass" />
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                        <table id="tblResetPassword" runat="Server" visible="false" width="100%" bgcolor="#eeeeee">
                                            <tr>
                                                <td valign="top">
                                                    <b>Password cannot be retrieved</b><br />
                                                    Please enter new password (min 5 chars):<br />
                                                    Confirm new password:
                                                </td>
                                                <td valign="top">
                                                    <br />
                                                    <asp:TextBox ID="txtResetPassword" runat="Server" MaxLength="20"></asp:TextBox> <asp:RegularExpressionValidator ID="regTxtResetPassword" runat="server" ControlToValidate="txtResetPassword" ValidationExpression="^[a-zA-Z0-9]{5,20}$" ValidationGroup="resetPass" ErrorMessage="Must be 5 - 20 alphanumeric characters" Display="dynamic"></asp:RegularExpressionValidator><asp:RequiredFieldValidator ID="reqTxtResetPassword" runat="server" ControlToValidate="txtResetPassword" ErrorMessage="Required" Display="dynamic" ValidationGroup="resetPass"></asp:RequiredFieldValidator><br />
                                                    <asp:TextBox ID="txtResetPasswordCompare" runat="server" MaxLength="20"></asp:TextBox> <asp:RegularExpressionValidator ID="regTxtResetPasswordConfirm" runat="server" ControlToValidate="txtResetPasswordCompare" ValidationExpression="^[a-zA-Z0-9]{5,20}$" ValidationGroup="resetPass" ErrorMessage="Must be 5 - 20 alphanumeric characters" Display="dynamic"></asp:RegularExpressionValidator><asp:CompareValidator ID="comTxtResetPassword" runat="server" Display="dynamic" ControlToCompare="txtResetPassword" ControlToValidate="txtResetPasswordCompare" ValidationGroup="resetPass" ErrorMessage="Passwords do not match"></asp:CompareValidator><asp:RequiredFieldValidator ID="reqTxtResetPasswordConpare" runat="server" ControlToValidate="txtResetPasswordCompare" ErrorMessage="Required" Display="dynamic" ValidationGroup="resetPass"></asp:RequiredFieldValidator><br />
                                                    <asp:Button ID="btnResetPasswordSubmit" runat="server" Text="Submit" OnClick="btnResetPasswordSubmit_click" ValidationGroup="resetPass" />
                                                </td>
                                                <td></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b>Locked Out:</b>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:CheckBox ID="chkLockedOut" runat="server" OnCheckedChanged="chkLockedOut_checkedChanged" AutoPostBack="true" />
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="btnDelete" runat="server" Text="Delete User" OnClick="btnDelete_click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">&nbsp;</td>
                                </tr>   
                                <tr>
                                    <td colspan="4">
                                        User is in the following Roles:
                                    </td>
                                </tr>   
                                <tr>
                                    <td colspan="4">
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    Administrator
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRoleAdmin" runat="server" OnCheckedChanged="chkRoleAdmin_checkedChanged" AutoPostBack="true" />
                                                </td>
                                                <td width="20">&nbsp;</td>
                                                <td>
                                                    Distributor
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRoleDistributor" runat="server" OnCheckedChanged="chkRoleDistributor_checkedChanged" AutoPostBack="true" />
                                                </td>
                                                <td width="20">&nbsp;</td>
                                                <td>
                                                    Consultancy
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRoleConsultancy" runat="server" OnCheckedChanged="chkRoleConsultancy_checkedChanged" AutoPostBack="true" />
                                                </td>
                                                <td>&nbsp;</td>
                                                <td>
                                                    Royalty
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkRoleRoyalty" runat="server" OnCheckedChanged="chkRoleRoyalty_checkedChanged" AutoPostBack="true" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>   
                            </table>
                        </td>
                        <td width="0"></td>
                        <td>
                            <asp:DetailsView ID="dvAff" runat="server" DataSourceID="SqlAffs" DataKeyNames="affID" AutoGenerateRows="false" Width="100%" GridLines="none" DefaultMode="edit" OnItemUpdated="dvAff_itemUpdated">
                                <Fields>
                                    <asp:BoundField HeaderText="Company" DataField="affCompany" />
                                    <asp:TemplateField HeaderText="First Name">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtFirstName" runat="server" Text='<%# Bind("affFirstName") %>' ValidationGroup="editAff" MaxLength="50"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="reqTxtFirstName" runat="server" ControlToValidate="txtFirstname" ValidationGroup="editAff" ErrorMessage="* Required" Display="static"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Surname">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtSurname" runat="server" Text='<%# Bind("affSurname") %>' MaxLength="50" ValidationGroup="editAff"></asp:TextBox>&nbsp;
                                            <asp:RequiredFieldValidator ID="reqTxtSurname" runat="server" ControlToValidate="txtSurname" ErrorMessage="* Required" ValidationGroup="editAff" Display="static"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Country">
                                        <EditItemTemplate>
                                            <asp:Label ID="lblHidCountry" runat="server" Text='<%# Eval("affCountryCode") %>' Visible="false" />
                                            <asp:Label ID="lblHidCurrency" runat="server" Text='<%# Eval("affCurrencyCode") %>' Visible="false" />
                                            <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="SqlCountry" DataTextField="CountryName" DataValueField="countryCode" SelectedValue='<%# Bind("affCountryCode") %>'></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sells in">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="drpCurrency" runat="server" DataSourceID="SqlCurrency" DataTextField='currencyCode' DataValueField="currencyCode" SelectedValue='<%# Bind("affCurrencyCode") %>'></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Buys in">
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="drpCurrency2" runat="server" DataSourceID="SqlCurrency" DataTextField='currencyCode' DataValueField="currencyCode" SelectedValue='<%# Bind("affCurrencyCodeBuys") %>'></asp:DropDownList>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CheckBoxField HeaderText="Distributor" DataField="affEBDistributor" ReadOnly="true" />
                                    <asp:CheckBoxField HeaderText="User" DataField="affEBUser" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Address" DataField="affAdd1" />
                                    <asp:BoundField HeaderText="" DataField="affAdd2" />
                                    <asp:BoundField HeaderText="" DataField="affAdd3" />
                                    <asp:BoundField HeaderText="" DataField="affAdd4" />
                                    <asp:BoundField HeaderText="" DataField="affAdd5" />
                                    <asp:BoundField HeaderText="Postcode" DataField="affPostcode" />
                                    <asp:BoundField HeaderText="Email" DataField="affEmail" />
                                    <asp:BoundField HeaderText="Phone" DataField="affPhone" />
                                    <asp:BoundField HeaderText="ClickThrough Amount"  DataField="affClickThrough" />
                                    <asp:BoundField HeaderText="Website Link" DataField="affLink" />
                                    <asp:BoundField HeaderText="WEbsite Params" DataField="affLinkParam" />
                                    <asp:BoundField HeaderText="Pay To Name" DataField="affPayToName" />
                                    <asp:BoundField HeaderText="Pay To Address" DataField="affToAdd1" />
                                    <asp:BoundField HeaderText="" DataField="affToAdd2" />
                                    <asp:BoundField HeaderText="" DataField="affToAdd3" />
                                    <asp:BoundField HeaderText="Pay To Postcode" DataField="affToPostcode" />
                                    <asp:TemplateField>
                                        <EditItemTemplate>
                                            <asp:Button ID="btnUpdateAff" runat="server" Text="Save Changes" CommandName="update" ValidationGroup="editAff" />
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Fields>
                            </asp:DetailsView>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panUserAdd" runat="server" Visible="false" BorderWidth="1">
                Enter the users details then click Create.
                <table border="0">
                    <tr>
                        <td>
                            <b>Type:</b>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <table border="0">
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="drpAddType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpAddType_selectedIndexChanged">
                                            <asp:ListItem Text="Distributor" Value="distributor"></asp:ListItem>
                                            <asp:ListItem Text="User" Value="user"></asp:ListItem>
                                            <asp:ListItem Text="Peartree User" Value="peartree"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="SqlCountryUnique" AutoPostBack="false" DataTextField="countryName" DataValueField="countryCode" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCurrency" runat="server" DataSourceID="SqlCurrency" DataTextField='currencyCode' DataValueField="currencyCode" OnDataBound="drpCurrency2_dataBound"></asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            Cant find the country/currency your looking for, <asp:LinkButton ID="lnkNewCountry" runat="server" Text="Add one" ValidationGroup="none" OnClick="lnkNewCountry_click"></asp:LinkButton>.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Username:</b>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtUsername" runat="server" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqTxtUsername" ControlToValidate="txtUsername" runat="server" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b>Password:</b>
                        </td>
                        <td>&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" MaxLength="20"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqTxtPassword" ControlToValidate="txtPassword" runat="server" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table border="0">
                                <tr>
                                    <td>
                                        Administrator
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAddAdministrator" runat="server" AutoPostBack="false" />
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Distributor
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAddDistributor" runat="server" AutoPostBack="false" />
                                    </td>
                                    <td width="20">&nbsp;</td>
                                    <td>
                                        Consultancy
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAddConsultancy" runat="server" AutoPostBack="false" />
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        Royalty
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAddRoyalty" runat="server" AutoPostBack="false" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>   
                    <tr>
                        <td>
                            <asp:Button ID="btnCreate" runat="server" Text="Create" OnClick="btnCreate_click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panNewCountry" runat="Server" Visible="false" Width="100%">
                <table border="0" width="100%">
                    <tr>
                        <td>
                            <table border="0" width="100%">
                                <tr>
                                    <td>
                                        Country Name:&nbsp;
                                        <asp:TextBox ID="txtNewCountryName" runat="server" MaxLength="20" ValidationGroup="ncc" OnTextChanged="txtNewCountryName_textChanged"></asp:TextBox>        
                                    </td>
                                    <td>
                                        Country Code (2 digits):&nbsp;
                                        <asp:TextBox ID="txtNewCountryCode" runat="server" MaxLength="2" ValidationGroup="ncc" OnTextChanged="txtNewCountryCode_textChanged" Width="40"></asp:TextBox>
                                    </td>
                                    <td>
                                        Currency:&nbsp;
                                        <asp:DropDownList ID="drpNewCountryCurrency" runat="server" DataSourceID="SqlCurrency" DataTextField="currencyCode" DataValueField="currencyCode" OnDataBound="drpCurrency2_dataBound"></asp:DropDownList>
                                    </td>
                                    <td>
                                        Default Language:&nbsp;
                                        <asp:DropDownList ID="drpNewCountryLanguage" runat="server" DataSourceID="SqlCountry" DataTextField="countryCode" DataValueField="countryCode" OnDataBound="drpCurrency2_dataBound"></asp:DropDownList>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnAddCountry" runat="server" ValidationGroup="ncc" Text="Add Country" OnClick="btnAddCountry_click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ID="reqTxtNewCountryName" runat="server" ValidationGroup="ncc" ControlToValidate="txtNewCountryName" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                    <td colspan="4">
                                        <asp:RequiredFieldValidator ID="reqTxtNewCountryCode" runat="server" ValidationGroup="ncc" ControlToValidate="txtNewCountryCode" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>                            
                        </td>
                    </tr>
                    <tr>
                        <td><hr /></td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" width="100%">
                                <tr>
                                    <td>
                                        Currency Code:&nbsp;
                                        <asp:TextBox ID="txtNewCurrencyCode" runat="server" ValidationGroup="ncu" MaxLength="3" OnTextChanged="txtNewCurrencyCode_textChanged"></asp:TextBox>        
                                    </td>
                                    <td>
                                        Currency Sign (eg £,$):&nbsp;
                                        <asp:TextBox ID="txtNewCurrencySign" runat="server" ValidationGroup="ncu" MaxLength="1" Width="40"></asp:TextBox>
                                    </td>
                                    <td align="right">
                                        <asp:Button ID="btnNewCurrency" runat="server" Text="Add Currency" ValidationGroup="ncu" OnClick="btnNewCurrency_click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RequiredFieldValidator ID="reqTxtNewCurrencyCode" runat="server" ControlToValidate="txtNewCurrencyCode" ErrorMessage="* Required" Display="dynamic" ValidationGroup="ncu"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="reqTxtNewCurrencySign" runat="server" ControlToValidate="txtNewCurrencySign" ErrorMessage="* Required" Display="dynamic" ValidationGroup="ncu"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                </tr>
                            </table>   
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panReActivate" runat="server" Visible="false">
                The following users exist in the Database as Inactive<br />
                <asp:GridView ID="gvReactive" runat="server" DataKeyNames="affID" AutoGenerateColumns="false" DataSourceID="SqlReActivate" GridLines="none" SelectedRowStyle-BackColor="aqua" OnSelectedIndexChanged="gvReactivate_selectedIndexChanged">
                    <Columns>
                        <asp:BoundField HeaderText="Username" DataField="affUsername" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Company" DataField="affCompany" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField HeaderText="Name">
                            <ItemTemplate>
                                <asp:Label ID="lblName" runat="server" Text='<%# Eval("affFirstName") & " " & Eval("affSurname") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Country" DataField="countryName" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField HeaderText="User Type">
                            <ItemTemplate>
                                <asp:Label ID="lblUserType" runat="server" Text='<%# showUserType(Eval("affEBDistributor"),Eval("affEBUser")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:CommandField ShowSelectButton="true" SelectText="Set Active" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="panReActivateDetails" runat="server" Visible="false">
            <br /><br />
                <table>
                    <tr>
                        <td>
                            You need to create a password for this user: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewPass" runat="server" ValidationGroup="np" MaxLength="20"></asp:TextBox>&nbsp;
                            <asp:RequiredFieldValidator ID="reqTxtNewPass" runat="server" ValidationGroup="np" ControlToValidate="txtNewPass" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regTxtNewPass" runat="server" ControlToValidate="txtNewPass" ValidationGroup="np" ValidationExpression="[a-zA-Z-0-9]{5,20}" ErrorMessage="* Password must be 5 - 20 characters" Display="dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnReActivateSubmit" runat="server" Text="Re Activate" OnClick="btnReActivateSubmit_click" ValidationGroup="np" />
                        </td>
                    </tr>
                </table>
                
                
            </asp:Panel>
            <br />
            <asp:Label ID="lblMessage" runat="server" ForeColor="red" Text="" OnLoad="lblMessage_load"></asp:Label>
            <asp:HiddenField ID="hidProvider" runat="server" Visible="true" />
            <asp:HiddenField ID="hidUser" runat="server" />
        </ContentTemplate>
    </atlas:UpdatePanel>              
    <asp:SqlDataSource ID="sqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCountryUnique" runat="server" SelectCommand="procCountryDistUnusedSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlReActivate" runat="server" SelectCommand="procAffiliateByActiveSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:Parameter Name="active" Type="boolean" DefaultValue="false" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlAffs" runat="Server" SelectCommand="procAffiliateByUsernameSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procAffiliateByIdUpdate2" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvDistributors" PropertyName="selectedValue" Name="affUsername" Type="string" Size="20" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter name="affID" Type="int32" />
            <asp:Parameter Name="affCompany" Type="string" Size="100" />
            <asp:Parameter Name="affTitle" Type="String" size="20" />
            <asp:Parameter Name="affFirstName" Type="string" Size="50" />
            <asp:Parameter Name="affSurname" Type="string" Size="50" />
            <asp:Parameter Name="affCountryCode" Type="string" Size="5" />
            <asp:Parameter Name="affCurrencyCode" Type="string" Size="3" />
            <asp:Parameter Name="affCurrencyCodeBuys" Type="string" Size="3" />
            <asp:Parameter Name="affAdd1" Type="string" Size="50" />
            <asp:Parameter Name="affAdd2" Type="string" Size="50" />
            <asp:Parameter Name="affAdd3" Type="string" Size="50" />
            <asp:Parameter Name="affAdd4" Type="string" Size="50" />
            <asp:Parameter Name="affAdd5" Type="string" Size="50" />
            <asp:Parameter Name="affPostcode" Type="string" Size="10" />
            <asp:Parameter Name="affEmail" Type="string" Size="100" />
            <asp:Parameter Name="affPhone" Type="string" Size="30" />
            <asp:Parameter Name="affClickThrough" Type="decimal" />
            <asp:Parameter Name="affLink" Type="string" Size="50" />
            <asp:Parameter Name="affLinkParam" Type="string" Size="50" />
            <asp:Parameter Name="affPayToName" Type="string" Size="50" />
            <asp:Parameter Name="affToAdd1" Type="string" Size="50" />
            <asp:Parameter Name="affToAdd2" Type="string" Size="50" />
            <asp:Parameter Name="affToAdd3" Type="string" Size="50" />
            <asp:Parameter Name="affToPostcode" Type="string" Size="10" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>

