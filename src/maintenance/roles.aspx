<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="roles.aspx.vb" Inherits="maintenance_roles" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Add a new Role: <asp:TextBox ID="txtRole" runat="server" MaxLength="20" AutoPostBack="false"></asp:TextBox>&nbsp;
            <asp:Button ID="btnRoleSubmit" runat="server" Text="Add" OnClick="btnRoleSubmit_click" />
            <br /><br />
            <asp:GridView ID="gvRoles" runat="server" GridLines="none" AutoGenerateColumns="true" EmptyDataText="No roles exist.">
                <Columns>
                </Columns>
            </asp:GridView>
            <br /><br />
            <asp:Panel ID="pan1" runat="server">
                Add User: <asp:DropDownList ID="drpUser" runat="server" OnSelectedIndexChanged="drpUser_selectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="false"></asp:DropDownList> to Role:<asp:DropDownList ID="drpRoles" runat="server"></asp:DropDownList>&nbsp;
                <asp:Button ID="btnAddRoleSubmit" runat="server" Text="Add Role" OnClick="btnAddRoleSubmit_click" />
                <br /><br />
                <asp:GridView ID="gvUserRoles" runat="server"></asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>     
</asp:Content>

