<%@ Page Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="ebcontact.aspx.vb" Inherits="ebcontact" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="home" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" width="100%">
        <tr>
            <td>
                <dbResource="FirstName" />
                <dbResource="LastName" />
                <dbResource="EmailAddress" />
                <dbResource="Telephone" />
                <dbResource="Department" />
                <dbResource="SelectDepartment" />
                <dbResource="CustomerService" />
                <dbResource="AffiliateEnquiry" />
                <dbResource="PressEnquiry" />
                <dbResource="Accounts" />
                <dbResource="Returns" />
                <dbResource="WebsiteFeedback" />
                <dbResource="ProductInformation" />
                <dbResource="WholesaleEnquiries" />
                <dbResource="DistributionEnquiries" />
                <dbResource="CountryRepresentative" />
                <dbResource="B2BEnquiry" />
                <dbResource="PTMD" />
                <dbResource="JasmineFeedback" />
                <dbResource="Subject" />
                <dbResource="OrderNumber" />
                <dbResource="Message" />
                <asp:DetailsView ID="dvContact" runat="server" CellSpacing="4" DataSourceID="sqlContact" DefaultMode="insert" GridLines="none" OnItemInserting="dvContact_inserting" AutoGenerateRows="False">
                    <Fields>
                        <asp:BoundField HeaderText="First Name:" DataField="firstName" />
                        <asp:BoundField HeaderText="Last Name:" DataField="firstName" />
                        <asp:TemplateField HeaderText="Email Address:">
                            <InsertItemTemplate>
                                <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="regTxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="* Invalid Email" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$" Display="dynamic" ValidationGroup="norm"></asp:RegularExpressionValidator>
                                <asp:RequiredFieldValidator ID="reqTxtEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="* Required" Display="dynamic" ValidationGroup="norm"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Telephone:" DataField="firstName" />
                        <asp:TemplateField HeaderText="Department:">
                            <InsertItemTemplate>
                                <asp:DropDownList ID="drpDepartment" runat="Server">
                                    <asp:ListItem Text="- Please select a department" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="Customer Services" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Affiliate Enquiry" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="Press Enquiries" Value="3"></asp:ListItem>
                                    <asp:ListItem Text="Accounts" Value="4"></asp:ListItem>
                                    <asp:ListItem Text="Returns" Value="5"></asp:ListItem>
                                    <asp:ListItem Text="Website Feedback / Problem" Value="6"></asp:ListItem>
                                    <asp:ListItem Text="Product Information" Value="7"></asp:ListItem>
                                    <asp:ListItem Text="Wholesale Enquiries" Value="8"></asp:ListItem>
                                    <asp:ListItem Text="Distributor Enquiries" Value="9"></asp:ListItem>
                                    <asp:ListItem Text="Country Representative Enquiries" Value="10"></asp:ListItem>
                                    <asp:ListItem Text="Product Feedback" Value="13"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RangeValidator ID="ranDrpDepartment" runat="server" ControlToValidate="drpDepartment" Type="Integer" MinimumValue="1" MaximumValue="20" ErrorMessage="* Required" ValidationGroup="norm"></asp:RangeValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Subject:" DataField="firstName" />
                        <asp:BoundField HeaderText="Order Number(Optional):" DataField="firstName" />
                        <asp:TemplateField HeaderText="Message:" HeaderStyle-VerticalAlign="top">
                            <InsertItemTemplate>
                                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="normaltextarea" Rows="8" width="320"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqTxtMessage" runat="server" ControlToValidate="txtMessage" Display="dynamic" ErrorMessage="Please enter a message" ValidationGroup="norm"></asp:RequiredFieldValidator>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <InsertItemTemplate>
                                <div id="button"><asp:LinkButton ID="lnkSendEmail" runat="server" CssClass="sideNav" CommandName="insert" ValidationGroup="norm" dbResource="lnkSendEmail">Send Enquiry</asp:LinkButton></div>
                            </InsertItemTemplate>
                        </asp:TemplateField>
                    </Fields>
                </asp:DetailsView>        
            </td>
            <td valign="top" align="right">
                <%--<asp:Panel ID="panAddress" runat="server">
                    Emotional Bliss<br />
                    The Electric Salvage Building<br />
                    Andersen Road<br />
                    Goole<br />
                    DN14 6UD<br />
                    <asp:Label ID="lblCountryName" runat="server" Text=""></asp:Label>
                    <nobr>Tel: <asp:Label id="lblEBPhone" runat="server" text="08700 410 022"></asp:Label></nobr>
                </asp:Panel>--%>
                <asp:Panel ID="panNLAddress" runat="server" Visible="false">
                    Emotional Bliss<br />
                    <nobr>Overschieseweg 12-D</nobr><br />
                    3044EE Rotterdam<br />                             
                    Postbus 28020<br />
                    3003KA Rotterdam<br />
                    T&nbsp;&nbsp;&nbsp;010 340 0550<br />
                </asp:Panel>
            </td>
        </tr>
    </table>
        
    <asp:Label ID="lblError" runat="server"></asp:Label>
    
    <asp:SqlDataSource ID="sqlContact" runat="server" InsertCommand="" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <InsertParameters>
        
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>

