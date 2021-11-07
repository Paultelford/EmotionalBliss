<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="userManagement.aspx.vb" Inherits="affiliates_userManagement" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="lblHeader" runat="server" Font-Bold="true">User maintenance for</asp:Label> <asp:Label ID="lblCountryCode" runat="server" Font-Bold="true"></asp:Label>
    <br />
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
               Please Wait....<img src='../images/loading.gif' />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panCurrentUsers" runat="server">
                <asp:Label ID="lblCurrenctUsers" runat="server" Text="Current Users:"></asp:Label><br />
                <asp:GridView ID="gvCurrentUsers" runat="server" DataSourceID="sqlCurrentUsers" AutoGenerateColumns="false" DataKeyNames="affID" GridLines="none" ShowFooter="true" OnSelectedIndexChanged="gvCurrentUsers_selectedIndexChanged" OnRowDataBound="gvCurrentUsers_rowDatabound" OnRowDeleted="gvCurrentUsers_rowDeleted" OnRowEditing="gvCurrenctUsers_rowEditing">
                    <Columns>
                        <asp:BoundField HeaderText="User" DataField="affName" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Login Name" DataField="affUsername" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField HeaderText="Password">
                            <ItemTemplate>
                                <asp:LinkButton id="lnkShowPassword" runat="server" CommandName="edit" Text="Show PW"></asp:LinkButton>
                                <asp:Label id="lblPassword" runat="server" text='<%# Eval("affPassword") %>' visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Date Added" DataField="affDateAdded" DataFormatString="{0:dd MMM yyy}" HtmlEncode="false" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Active" DataField="affActive" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton id="lnkEdit" runat="server" Text="Edit" CommandName="select" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:CommandField DeleteText="Delete" ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>
                <asp:Button ID="btnAdd" runat="server" Text="Add New User" OnClick="btnAdd_click" />
                <asp:Label ID="lblError" runat="server"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="panEditPermissions" runat="server" Visible="false">
                <table border="0">
                    <tr>
                        <td colspan="3">
                            <asp:Label ID="lblEditName" runat="server" Font-Bold="true">Permissions for </asp:Label><asp:Label ID="lblEditUsersFullname" runat="Server" Font-Bold="true"></asp:Label>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblPW" runat="Server" Text="Users Password: "></asp:Label>
                            <asp:Label ID="lblUserPassword" runat="Server"></asp:Label>
                            <asp:GridView ID="gvPermissions" runat="server" AutoGenerateColumns="false" GridLines="none" DataKeyNames="affMenuID1">
                                <Columns>
                                    <asp:BoundField HeaderText="Page" DataField="affMenuname" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Section" DataField="affMenuName1" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:TemplateField HeaderText="Has Acceess">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkAccess" runat="server" Checked='<%# getUserAccess(Eval("permissionID")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>        
                        </td>
                    </tr>
                    <tr>
                        <td width="260">
                            <asp:Button ID="btnEditUpdate" runat="server" Text="Save Changes" OnClick="btnEditUpdate_click" />
                        </td>
                        <td coslapn="2">
                            <asp:Button ID="btnSelectAll" runat="server" Text="All" OnClick="btnSelectAll_click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnSelectNone" runat="server" Text="None" OnClick="btnSelectNone_click" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" OnClick="btnCancel_click" />
                        </td>
                    </tr>
                </table>                
                <br />
                <asp:Label ID="lblEditError" runat="server"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="panAddUser" runat="server" Visible="false">
                <table border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblFirstname" runat="server" Text="Users Firstname:"></asp:Label>
                        </td>
                        <td>
                            <asp:textbox ID="txtFirstname" runat="server" MaxLength="50" ValidationGroup="add"></asp:textbox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="reqTxtFirstname" runat="server" ValidationGroup="add" ControlToValidate="txtFirstname" ErrorMessage="* Required" Display="static"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSurname" runat="server" Text="Users Surname:"></asp:Label>
                        </td>
                        <td>
                            <asp:textbox ID="txtSurname" runat="server" MaxLength="50" ValidationGroup="add"></asp:textbox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="reqTxtSurname" runat="server" ValidationGroup="add" ControlToValidate="txtSurname" ErrorMessage="* Required" Display="static"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblUsername" runat="server" Text="Desired Login Name:"></asp:Label>
                        </td>
                        <td>
                            <asp:textbox ID="txtUsername" runat="server" MaxLength="20" ValidationGroup="add"></asp:textbox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="reqTxtUsername" runat="server" ValidationGroup="add" ControlToValidate="txtUsername" ErrorMessage="* Required" Display="static"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                        </td>
                        <td>
                            <asp:textbox ID="txtPassword" runat="server" MaxLength="20" ValidationGroup="add"></asp:textbox>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="reqTxtPassword" runat="server" ValidationGroup="add" ControlToValidate="txtPassword" ErrorMessage="* Required" Display="static"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regTxtPassword" runat="server" ValidationGroup="add" ValidationExpression="[A-Za-z0-9]{5,20}" ControlToValidate="txtPassword" ErrorMessage="* Must be at least 5 alphanumeric characters" Display="static"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblActive" runat="server" Text="Active:"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkActive" runat="Server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:Button ID="btnAddUserSubmit" runat="server" ValidationGroup="add" Text="Create User" OnClick="btnAddUserSubmit_click" /><br />
                            <asp:Button ID="btnAddUserCancel" runat="Server" Text="Cancel" OnClick="btnAddUserCance_click" />
                            <br /><br />
                            <asp:Label ID="lblAddUserError" runat="server" ForeColor="red"></asp:Label>                    
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlCurrentUsers" runat="server" SelectCommand="procAffiliatesUsersByCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" DeleteCommand="procAffiliateByIDDelete" DeleteCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="affID" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>

