<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="departments.aspx.vb" Inherits="affiliates_departments" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>

            Current Departments:<br />
            <asp:Panel ID="pan1" runat="server" Width="99%" BorderWidth="1">
                <asp:GridView ID="gvDepts" runat="server" DataSourceID="SqlDepartments" AutoGenerateColumns="false" Width="100%" DataKeyNames="deptID" OnRowUpdating="gvDepts_rowUpdating">
                    <HeaderStyle Font-Bold="true" />
                    <RowStyle VerticalAlign="top" />
                    <EditRowStyle VerticalAlign="Top" />
                    <Columns>
                        <asp:TemplateField HeaderText="Dept Name">
                            <ItemTemplate>
                                <asp:Label ID="lblDeptName" runat="server" Text='<%# Eval("deptName") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDeptName" runat="server" Text='<%# Bind("deptName") %>' MaxLength="50"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description" ItemStyle-VerticalAlign="Top">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("deptDescription") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate><asp:TextBox ID="txtDescription" runat="server" Text='<%# Bind("deptDescription") %>' Rows="6" Width="220" TextMode="MultiLine"></asp:TextBox></EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CheckBoxField HeaderText="Active" DataField="deptActive" ItemStyle-HorizontalAlign="center" />
                        <asp:TemplateField HeaderText="Image Filename (Visible on ShopIntro page)">
                            <ItemTemplate>
                                <asp:Label ID="lblImage" runat="server" Text='<%# Eval("deptImage") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                            <EditItemTemplate>
                                <asp:FileUpload ID="f1" runat="server" FileName='<%# Bind("deptImage") %>' /><br />
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Order" DataField="deptPriority" ControlStyle-Width="40" />
                        <asp:CommandField EditText="Edit" ShowEditButton="true" />
                        <asp:CommandField DeleteText="X" ControlStyle-ForeColor="red" ShowDeleteButton="true" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btnInsert" runat="server" Text="Insert New Department" OnClick="btnInsert_click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:Panel ID="panAdd" runat="server" Visible="false">
                <b>New Deaprtment:</b><br />
                <table border="0" cellspacing="8">
                    <tr>
                        <td>
                            Name:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpPeartreeDepartments" runat="server" DataSourceID="sqlDepts" DataTextField="deptName" DataValueField="shopDeptID" Visible="false"></asp:DropDownList>
                            <asp:TextBox ID="txtName" runat="server" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtName" runat="server" ControlToValidate="txtName" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Description:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" CssClass="normaltextarea" TextMode="multiLine" Rows="6" Columns="40" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Image Name:
                        </td>
                        <td>
                            <asp:TextBox ID="txtImage" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Order Priority:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPriority" runat="server" Width="40"></asp:TextBox><asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtPriority" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator><asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtPriority" MinimumValue="1" MaximumValue="999" ErrorMessage="* Enter a number between 1 - 999" Display="dynamic"></asp:RangeValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="2">
                            <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_click" /><br />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblError" runat="server"></asp:Label> 
    
    <asp:SqlDataSource ID="SqlDepartments" runat="server" SelectCommand="procDeptByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" DeleteCommand="procDepyByIDDelete" DeleteCommandType="StoredProcedure" UpdateCommand="procDeptByIDDistUpdate" UpdateCommandType="StoredProcedure" OnUpdated="sqlDepartments_updated" OnDeleted="sqlDepartments_deleted">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="deptID" Type="int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="deptID" Type="int32" />
            <asp:Parameter Name="deptName" Type="string" Size="500" />
            <asp:Parameter Name="deptDescription" Type="string" Size="4000" />
            <asp:Parameter Name="deptActive" Type="boolean" />
            <asp:Parameter Name="deptImage" Type="string" Size="100" />
            <asp:Parameter Name="deptPriority" Type="int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDepts" runat="server" SelectCommand="procShopDeptsByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

