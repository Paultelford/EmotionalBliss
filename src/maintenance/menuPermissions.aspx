<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="menuPermissions.aspx.vb" Inherits="maintenance_menuPermissions" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <asp:Panel id="pan1" runat="server">
        Select a user from the dropdown list, then un/tick the boxes to assign permissions.<br /><br />
        <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
            <ProgressTemplate>
                Please Wait....<img src='../images/loading.gif' width="16" height="16" />
            </ProgressTemplate>            
        </atlas:UpdateProgress> 
        <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
                <asp:DropDownList ID="drpUsers" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="drpUsers_selectedIndexChanged">
                    <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
                <br /><br />
                <asp:GridView ID="gvMenu" runat="server" DataSourceID="SqlMenu" AutoGenerateColumns="false" DataKeyNames="menuID" OnRowDataBound="gvMenu_dataBound" Visible="false" ShowFooter="true">
                    <HeaderStyle Font-Bold="true" />
                    <Columns>
                        <asp:BoundField HeaderText="Description" DataField="text" />
                        <asp:BoundField HeaderText="Page" DataField="url" NullDisplayText="*" />
                        <asp:TemplateField HeaderText="Has Access">
                            <ItemTemplate>
                                <asp:CheckBox id="chkHasAccess" runat="server" Checked='<%# populateChkBox(Eval("hasAccess")) %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Button ID="btnUpdate" runat="server" Text="Update" OnClick="btnUpdate_click" />
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>    
            </ContentTemplate>
    </atlas:UpdatePanel>
    </asp:Panel>
    <asp:Panel ID="pan2" runat="server" Visible="false">
        Only Administrators have access to this page.
    </asp:Panel>
    
    <asp:SqlDataSource ID="SqlMenu" runat="server" SelectCommand="procMenuPermissionsByUsernameSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpUsers" PropertyName="selectedValue" Name="username" Type="string" Size="25" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

