<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="emailSetup.aspx.vb" Inherits="maintenance_emailSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
                <table border="0">
                    <tr>
                        <td>
                            Existing groups:&nbsp;
                            <asp:DropDownList ID="drpGroups" runat="server" DataSourceID="SqlGroups" DataTextField="groupName" DataValueField="groupID" OnDataBound="drpGroups_dataBound" AutoPostBack="true" OnSelectedIndexChanged="drpGroups_selectedInxedChanged" AppendDataBoundItems="true" OnDataBinding="drpGroups_dataBinding">
                            </asp:DropDownList>      <br />
                            <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true">
                                <asp:ListItem Text="View active email addresses" Value="true"></asp:ListItem>
                                <asp:ListItem Text="View inactive email addresses" Value="false"></asp:ListItem>
                            </asp:DropDownList>                              
                        </td>
                        <td width="60">
                        </td>
                        <td>
                            Add new group: 
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewGroup" runat="server" Width="120" AutoPostBack="false" MaxLength="30" ValidationGroup="group1"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtNewGroup" ErrorMessage="*" Display="Dynamic" ValidationGroup="group1"></asp:RequiredFieldValidator> <asp:Button ID="btnSubmit" runat="server" Text="Add" ValidationGroup="group1" OnClick="btnSubmit_click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">&nbsp;</td>
                        <td>
                            Add new address:
                        </td>
                        <td>
                            <asp:TextBox ID="txtNewEmail" runat="server" Width="120" AutoPostBack="false" MaxLength="100" ValidationGroup="group2"></asp:TextBox><asp:RegularExpressionValidator ID="regEx1" runat="server" ControlToValidate="txtNewEmail" ErrorMessage="*" ValidationGroup="group2" ValidationExpression="^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$" Display="dynamic"></asp:RegularExpressionValidator><asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtNewEmail" ErrorMessage="*" Display="dynamic" ValidationGroup="group2"></asp:RequiredFieldValidator> <asp:Button ID="btnEmailSubmit" runat="server" Text="Add" ValidationGroup="group2" OnClick="btnEmailSubmit_click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                </table>
                
                <br /><br />
                <asp:GridView ID="gvEmail" runat="server" DataKeyNames="emailID" ShowFooter="true" CssClass="text" GridLines="none" AutoGenerateColumns="false" DataSourceID="SqlEmail" OnRowDataBound="gvEmail_rowDataBound" OnDataBound="gvEmail_dataBound" EmptyDataText="No results found.">
                    <HeaderStyle Font-Bold="true" HorizontalAlign="Left" />
                    <RowStyle VerticalAlign="top" />
                    <Columns>
                        <asp:TemplateField HeaderText="Address">
                            <ItemTemplate>
                                <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("address") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtAddress" runat="server" Text='<%# Bind("address") %>' Width="240" ValidationGroup="email"></asp:TextBox><asp:RequiredFieldValidator ID="reqAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="* Required" Display="Dynamic" ValidationGroup="email"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="regAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="* Invalid email." ValidationGroup="email" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$" Display="Dynamic"></asp:RegularExpressionValidator>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                        <asp:BoundField HeaderText="Groups" ItemStyle-Font-Size="Smaller" ReadOnly="true" />
                        <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Update" HeaderStyle-HorizontalAlign="center">
                            <ItemStyle HorizontalAlign="center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chkUpdate" runat="server" />                                
                            </ItemTemplate>
                            <EditItemTemplate></EditItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnUpdate" runat="server" CausesValidation="false" Text="Update  Selected" OnClick="btnUpdate_click" /><br />
                                <asp:Button ID="btnDelete" runat="server" CausesValidation="false" Text="Remove Selected" OnClick="btnDelete_click" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                        <asp:CommandField DeleteText="Delete" ShowDeleteButton="true" ItemStyle-ForeColor="blue" EditText="Edit" ShowEditButton="true" />
                        <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkActivate" runat="server" Text='<%# getActivateText(Eval("active")) %>' OnClick="lnkActive_click"></asp:LinkButton>
                                <asp:HiddenField ID="hidEmail" runat="server" Value='<%# Eval("address") %>' />
                                <asp:HiddenField ID="hidActive" runat="server" Value='<%# Eval("active") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>
                </asp:GridView>
            
    </asp:Panel>
    
    
    <asp:SqlDataSource ID="SqlGroups" runat="server" SelectCommand="procEmailGroupsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlEmail" runat="server" SelectCommand="procEmailByActiveSelect" SelectCommandType="StoredProcedure" DeleteCommand="procEmailDelete" DeleteCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procEmailByIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpType" Name="active" Type="Boolean"  />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="emailID" Type="int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="emailID" Type="Int32" />
            <asp:Parameter Name="address" Type="String" Size="100" />
        </UpdateParameters>        
    </asp:SqlDataSource>
</asp:Content>

