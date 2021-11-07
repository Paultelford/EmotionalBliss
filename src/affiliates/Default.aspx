<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="affiliates_Default" Title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server">
    </atlas:ScriptManagerProxy>
    <table width="100%">
        <tr>
            <td width="30%">
                &nbsp;</td>
            <td align="left">
                <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
                    <ProgressTemplate>
                        Please Wait....<img src="/images/loading.gif" width="16" height="16" />
                    </ProgressTemplate>
                </atlas:UpdateProgress>
                <atlas:UpdatePanel ID="update1" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="panLogin" runat="server">
                            <asp:Label ID="lblHeader" runat="Server" CssClass="lightbluehead" Text="Account Login"></asp:Label>
                            <br />
                            <br />
                            <asp:Login ID="loginAff" runat="server" OnAuthenticate="loginAff_authenticate" RememberMeSet="false">
                            </asp:Login>
                            <asp:LinkButton ID="lnkForgot" runat="server" Text="I forgot my password" OnClick="lnkForgot_click"></asp:LinkButton>
                            <br />
                            <br />
                            <asp:Panel ID="panForgot" runat="server" Visible="false" Width="100%">
                                <table border="0" width="100%">
                                    <tr>
                                        <td width="160" align="left">
                                            &nbsp;</td>
                                        <td colspan="2" align="left">
                                            Please enter your username and email address.<br />
                                            If a match is found, your password will be emailed to you.<br />
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="160" align="left">
                                            Username:
                                        </td>
                                        <td colspan="2" align="left">
                                            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="regTxtUsername" runat="server" ControlToValidate="txtUsername"
                                                Display="dynamic" ErrorMessage="* Required" ValidationGroup="forgot"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="160" align="left">
                                            Email:
                                        </td>
                                        <td colspan="2" align="left">
                                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqTxtEmail" runat="server" ControlToValidate="txtEmail"
                                                Display="dynamic" ErrorMessage="* Required" ValidationGroup="forgot"></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="regTxtEmail" runat="server" ControlToValidate="txtEmail"
                                                ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$"
                                                Display="dynamic" ErrorMessage="* Invalid email address" ValidationGroup="forgot"></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="160">
                                            &nbsp;</td>
                                        <td colspan="2" align="left">
                                            <asp:Button ID="btnForgotSubmit" runat="server" Text="Email my password" ValidationGroup="forgot"
                                                OnClick="btnForgotSubmit_click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </asp:Panel>
                    </ContentTemplate>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    <table width="100%">
        <tr>
            <td width="40">
            
            </td>
            <td valign="top">
                <asp:Panel ID="panOverview" runat="server" Visible="false">
                    <!-- AFFILIATE DETAILS -->
                    <b>Your Details</b><br /><br />
                    <asp:DetailsView ID="dvAffiliate" runat="server" DataKeyNames="affID" GridLines="None" DataSourceID="sqlAffiliate" SkinID="" DefaultMode="ReadOnly" AutoGenerateDeleteButton="false" AutoGenerateRows="false" OnDataBound="dvAffiliate_dataBound">
                        <HeaderStyle ForeColor="Black" />
                        <Fields>
                            <asp:BoundField HeaderText="Company" DataField="affCompany" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Postition In Company" DataField="affCompanyPosition" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Title" DataField="affTitle" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Firstname" DataField="affFirstname" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Surname" DataField="affSurname" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Email" DataField="affEmail" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Address" DataField="affAdd1" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="" DataField="affAdd2" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="City" DataField="affAdd3" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Postcode" DataField="affPostcode" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Country" DataField="affCountryCode" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Site Name" DataField="affSiteName" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Site URL" DataField="affSiteURL" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="VAT Number" DataField="affVat" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="How Many Stores" DataField="wholesaleStores" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Products Sold Other" DataField="wholesaleSell" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Customer Base" DataField="customerBase" ControlStyle-Width="300" NullDisplayText="*" />
                            <asp:BoundField HeaderText="Username" DataField="affUsername" ReadOnly="true" ControlStyle-Width="300" Visible="false" />
                            <asp:BoundField HeaderText="Password" DataField="affPassword" ControlStyle-Width="300" Visible="false" />   
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />
                                </EditItemTemplate>
                            </asp:TemplateField>                        
                        </Fields>
                    </asp:DetailsView>
                </td>
                <td width="80">&nbsp;</td>
                <td valign="top">
                    <!-- AFFILIATE LOG -->
                    <b>History</b><br /><br />
                    <asp:GridView ID="gvTrace" runat="Server" AutoGenerateColumns="false" DataSourceID="sqlAffiliateLog" SkinID="GridView" EmptyDataText="The affiliate log is empty." Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="Date" ItemStyle-VerticalAlign="top" DataField="date" DataFormatString="{0:dd MMM yyy}" ItemStyle-Width="20%" />
                            <asp:TemplateField HeaderText="Action" ItemStyle-VerticalAlign="top">
                                <ItemTemplate>
                                    <asp:Label ID="lblMessage" runat="server" Text='<%# Replace(Eval("message"),chr(10),"<br>") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Customer Contacted" DataField="contact" ItemStyle-Width="10%" Visible="false" />
                            <asp:CheckBoxField DataField="affVisible" HeaderText="Customer Visible" ItemStyle-VerticalAlign="top" ItemStyle-Width="10%" Visible="false" />
                            <asp:BoundField HeaderText="User" DataField="userName" ItemStyle-VerticalAlign="top" ItemStyle-Width="10%" Visible="false" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>


    <asp:SqlDataSource ID="sqlAffiliate" runat="server" SelectCommand="procAffiliateByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffID" Name="affid" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlAffiliateLog" runat="server" SelectCommand="procAffiliateContactLogByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" FilterExpression="affVisible=1">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffID" Name="affid" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>    
        
    <script language="Javascript" type="text/javascript">
        function focusElement(e)
        {
            document.getElementById(e).focus();
        }
    </script>

</asp:Content>
