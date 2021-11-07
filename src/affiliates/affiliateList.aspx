<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="affiliateList.aspx.vb" Inherits="affiliates_affiliateList" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
   
            <asp:HiddenField ID="hidPassword" runat="server" />
            <asp:HiddenField ID="hidUsername" runat="server" />
            <table>
                <tr>
                    <td>
                        Affiliate Type:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="Affiliates" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Distributors" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Wholesalers" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Counry Reps" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </tr>
            <tr>
                <td>
                    Status:
                </td>
                <td>
                    <asp:DropDownList ID="drpStatus" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpStatus_selectedIndexChanged">
                        <asp:ListItem Text="All"></asp:ListItem>
                        <asp:ListItem Text="Active"></asp:ListItem>
                        <asp:ListItem Text="Inactive"></asp:ListItem>
                        <asp:ListItem Text="Rejected"></asp:ListItem>
                    </asp:DropDownList>
                </td>
            </tr>
            </table>
            <br />
            <asp:Label ID="lblEditText" runat="server">
                Click on the Affiliate to edit details
            </asp:Label><br /><br />
            <asp:GridView ID="gvAffiliates" runat="server" EmptyDataText="No results found." DataSourceID="sqlAffiliates" Width="100%" AutoGenerateColumns="false" OnSelectedIndexChanged="gvAffiliates_selectedIndexChanged" DataKeyNames="affID" SkinID="GridView" AllowPaging="true" PageSize="20" OnRowEditing="gvAffiliates_rowEditing" PagerSettings-Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="Affiliate">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" Text='<%# Eval("affCompany") %>' CommandName="edit"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="10" />
                    <asp:BoundField HeaderText="Contact" DataField="affiliateName" SortExpression="affiliateName" />
                    <asp:BoundField ItemStyle-Width="10" />
                    <asp:BoundField HeaderText="Email" DataField="affEmail" />
                    <asp:BoundField ItemStyle-Width="10" />
                    <asp:TemplateField HeaderText="Password" Visible="false">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnChangePassword" runat="Server" Text='<%# Eval("affPassword") %>' OnClick="btnChangePassword_click" CommandArgument='<%# Eval("affUsername") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="10" Visible="false" />
                    <asp:TemplateField HeaderText="Special">
                        <ItemTemplate>
                            <asp:Label ID="lblSpecial" runat="server" Text='<%# showSpecial(Eval("affConsultancy"),Eval("affRoyalty")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="10" Visible="true" />
                    <asp:BoundField HeaderText="Vat" DataField="affVat" />
                    <asp:BoundField ItemStyle-Width="10" Visible="true" />
                    <asp:BoundField HeaderText="Signup Date" DataField="affDateAdded" DataFormatString="{0:dd MMM yy HH:mm}" />
                    <asp:BoundField ItemStyle-Width="10" Visible="true" />
                    <asp:CheckBoxField HeaderText="Active" DataField="affActive" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkActivate" runat="server" CommandName="select" Text="Toggle Active"></asp:LinkButton>
                            <asp:Label ID="lblUsername" runat="server" Visible="false" Text='<%# Eval("affUsername") %>'></asp:Label>
                            <asp:Label ID="lblPassword" runat="server" Visible="false" Text='<%# Eval("affPassword") %>'></asp:Label>
                            <asp:CheckBox ID="chkFirstTime" runat="server" Visible="false" Checked='<%# Eval("affFirstTime") %>' />
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("affEmail") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <eb:Pager id="pager1" runat="server" showTextBox="true"></eb:Pager>
            <br />
            <asp:Panel ID="panPassword" runat="Server" Visible="false">
                <table>
                    <tr>
                        <td>
                            Enter new password:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="Server" MaxLength="20" ValidationGroup="pass"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqTxtPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="pass" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Confirm password:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPasswordConfirm" runat="Server" MaxLength="20" ValidationGroup="pass"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqTxtPasswordConfirm" runat="server" ControlToValidate="txtPasswordConfirm" ValidationGroup="pass" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSubmitPassword" runat="server" Text="Submit" ValidationGroup="pass" OnClick="btnSubmitPassword_click" />
                        </td>
                    </tr>
                </table>
                <asp:Label ID="lblErrorPassword" runat="Server"></asp:Label>
            </asp:Panel>
            <asp:DetailsView ID="dvAffiliateEditOld" runat="server" DataKeyNames="affID"  DefaultMode="edit" AutoGenerateRows="false" SkinID="DetailsView" OnItemUpdated="dvAffiliateEdit_itemUpdated" OnItemUpdating="dvAffiliateEdit_itemUpdating">
                <Fields>
                    <asp:BoundField HeaderText="Company" DataField="affCompany" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Contact Firstname" DataField="affFirstname" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Contact Surname" DataField="affSurname" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Address" DataField="affAdd1" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="" DataField="affAdd2" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="" DataField="affAdd3" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="" DataField="affAdd4" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="" DataField="affAdd5" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Postcode" DataField="affPostcode" />
                    <asp:BoundField HeaderText="Email" DataField="affEmail" ControlStyle-Width="300" />   
                    <asp:CheckBoxField HeaderText="Consultancy" DataField="affConsultancy" />
                    <asp:CheckBoxField HeaderText="Royalty" DataField="affRoyalty" />
                    <asp:BoundField HeaderText="Username" DataField="affUsername" ReadOnly="true" />
                    <asp:BoundField HeaderText="Password" DataField="affPassword" ControlStyle-Width="300" />   
                    <asp:TemplateField>
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />
                        </EditItemTemplate>
                    </asp:TemplateField>            
                </Fields>
            </asp:DetailsView>
            <asp:DetailsView ID="dvAffiliateEdit" runat="server" DataKeyNames="affID" GridLines="None" DataSourceID="sqlAffiliateEdit" SkinID="" DefaultMode="Edit" AutoGenerateDeleteButton="false" AutoGenerateRows="false" OnItemUpdated="dvAffiliateEdit_itemUpdated" OnItemUpdating="dvAffiliateEdit_itemUpdating" OnDataBound="dvAffiliateEdit_dataBound">
                <HeaderStyle ForeColor="Black" Width="200" />
                <Fields>
                    <asp:BoundField HeaderText="Company" DataField="affCompany" ControlStyle-Width="300" HeaderStyle-Width="160" />
                    <asp:BoundField HeaderText="Postition In Company" DataField="affCompanyPosition" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Title" DataField="affTitle" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Firstname" DataField="affFirstname" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Surname" DataField="affSurname" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Email" DataField="affEmail" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Phone" DataField="affPhone" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Address" DataField="affAdd1" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="" DataField="affAdd2" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="" DataField="affAdd3" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Postcode" DataField="affPostcode" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Country" DataField="affCountryCode" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Pay To Name" DataField="affPayToName" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Pay To Address" DataField="affToAdd1" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="" DataField="affToAdd2" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="" DataField="affToAdd3" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Pay To Postcode" DataField="affToPostcode" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Pay To Country" DataField="affToCountryCode" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Site Name" DataField="affSiteName" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Site URL" DataField="affSiteURL" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="VAT Number" DataField="affVat" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="How Many Stores" DataField="wholesaleStores" ControlStyle-Width="300" />
                    <asp:TemplateField HeaderText="Products Sold" ControlStyle-Width="300" HeaderStyle-VerticalAlign="Top">
                        <EditItemTemplate>
                            <asp:GridView ID="gvProductsSold" runat="server" DataSourceID="sqlProductsSold" AutoGenerateColumns="false" GridLines="None" ShowHeader="false">
                                <Columns>
                                    <asp:BoundField DataField="item" />
                                </Columns>
                            </asp:GridView>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Products Sold Other" DataField="wholesaleSell" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Customer Base" DataField="customerBase" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Customer Geographic Area" DataField="customerBaseArea" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Username" DataField="affUsername" ReadOnly="true" ControlStyle-Width="300" />
                    <asp:BoundField HeaderText="Password" DataField="affPassword" ControlStyle-Width="300" />   
                    <asp:CheckBoxField HeaderText="Order With CreditCard" DataField="canOrderWithCC" />
                    <asp:CheckBoxField HeaderText="Order On Account" DataField="canOrderWithAccount" />
                    <asp:CheckBoxField HeaderText="Hide Country" DataField="hideCountry" />
                    <asp:TemplateField>
                        <EditItemTemplate>
                            <asp:Button ID="btnUpdate" runat="server" CommandName="Update" Text="Update" />
                        </EditItemTemplate>
                    </asp:TemplateField>                        
                </Fields>
            </asp:DetailsView>
            <br />
            <asp:Panel ID="pan1" runat="server" Visible="false" BorderWidth="1" Width="499" HorizontalAlign="center">
                <br />
                Are you sure you want to make <asp:Label ID="lblSelectedAffiliate" runat="server" Font-Bold="true"></asp:Label> an active affiliate ?<br />
                This will create them a login, and send an acceptance Email.<br /><br />
                <asp:Button ID="btnAccept" runat="Server" Text="Yes, go ahead" OnClick="btnAccept_click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnCancel" runat="server" Text="No, Cancel" OnClick="btnCancel_click" />
                <br /><br />
            </asp:Panel>
            <br /><br />
            <asp:Label ID="lblComplete" runat="Server"></asp:Label><br />
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
            <asp:Panel ID="panLog" runat="server" Width="100%" BorderWidth="1" Visible="false">
                <asp:GridView ID="gvTrace" runat="Server" AutoGenerateColumns="false" DataSourceID="sqlAffiliateLog" SkinID="GridView" EmptyDataText="The affiliate log is empty." Width="100%">
                    <Columns>
                        <asp:BoundField HeaderText="Date" ItemStyle-VerticalAlign="top" DataField="date" ItemStyle-Width="20%" />
                        <asp:TemplateField HeaderText="Action" ItemStyle-VerticalAlign="top">
                            <ItemTemplate>
                                <asp:Label ID="lblMessage" runat="server" Text='<%# Replace(Eval("message"),chr(10),"<br>") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Customer Contacted" DataField="contact" ItemStyle-Width="10%" />
                        <asp:CheckBoxField DataField="affVisible" HeaderText="Customer Visible" ItemStyle-VerticalAlign="top" ItemStyle-Width="10%" />
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
                            Affiliate Can<br/>see comment:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkCustomerVisible" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Affiliate<br />Contacted:
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
            
    
    <asp:SqlDataSource ID="sqlAffiliates" runat="server" SelectCommand="procAffiliatesListByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" name="affTypeID" Type="Int16" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlAffiliateEdit" runat="server" SelectCommand="procAffiliateByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procAffiliateByIDUpdate3" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:Parameter Name="affID" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="affID" Type="int32" />
            <asp:Parameter Name="affCompany" Type="string" Size="100" />
            <asp:Parameter Name="affCompanyPosition" Type="string" Size="50" />
            <asp:Parameter Name="affTitle" Type="string" Size="20" />
            <asp:Parameter Name="affFirstname" Type="string" Size="50" />
            <asp:Parameter Name="affSurname" Type="string" Size="50" />
            <asp:Parameter Name="affEmail" Type="string" Size="100" />
            <asp:Parameter Name="affPhone" Type="string" Size="30" />
            <asp:Parameter Name="affAdd1" Type="string" size="50" />
            <asp:Parameter Name="affAdd2" Type="string" size="50" />
            <asp:Parameter Name="affAdd3" Type="string" size="50" />
            <asp:Parameter Name="affPostcode" Type="string" size="10" />
            <asp:Parameter Name="affCountryCode" Type="string" size="5" />
            <asp:Parameter Name="affPayToName" Type="string" Size="50" />
            <asp:Parameter Name="affToAdd1" Type="string" size="50" />
            <asp:Parameter Name="affToAdd2" Type="string" size="50" />
            <asp:Parameter Name="affToAdd3" Type="string" size="50" />
            <asp:Parameter Name="affToPostcode" Type="string" size="10" />
            <asp:Parameter Name="affToCountryCode" Type="string" size="5" />            
            <asp:Parameter Name="affSiteName" Type="string" Size="50" />
            <asp:Parameter Name="affSiteUrl" Type="string" Size="50" />
            <asp:Parameter Name="affVat" Type="string" size="50" />       
            <asp:Parameter Name="wholesaleStores" Type="string" size="50" />       
            <asp:Parameter Name="wholesaleSell" Type="string" size="200" />  
            <asp:Parameter Name="customerBase" Type="string" size="50" />  
            <asp:Parameter Name="customerBaseArea" Type="string" size="50" />  
            <asp:Parameter Name="affPassword" Type="string" size="20" />  
            <asp:Parameter Name="canOrderWithCC" Type="Boolean" />    
            <asp:Parameter Name="canOrderWithAccount" Type="Boolean" />    
            <asp:Parameter Name="hideCountry" Type="Boolean" />    
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProductsSold" runat="server" SelectCommand="procAffiliateSellsByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="dvAffiliateEdit" PropertyName="selectedValue" Name="affID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlAffiliateLog" runat="server" SelectCommand="procAffiliateContactLogByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="dvAffiliateEdit" PropertyName="selectedValue" Name="affid" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

