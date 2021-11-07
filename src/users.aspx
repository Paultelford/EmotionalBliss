<%@ Page Language="VB" AutoEventWireup="false" CodeFile="users.aspx.vb" Inherits="users" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <br /><br />
        Select Provider:&nbsp;
        <asp:DropDownList ID="drpProviders" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="drpProviders_selectedIndexChanged">
            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
            <asp:ListItem Text="EBProvider" Value="ebProvider"></asp:ListItem>
            <asp:ListItem Text="EBAffprovider" Value="ebAffProvider"></asp:ListItem>
        </asp:DropDownList>
        <br /><br />
        Users:<br />
        <asp:Panel ID="pan1" runat="server" BorderWidth="1" Width="300">
            <asp:GridView ID="gvUsers" runat="Server" AutoGenerateColumns="true">
            </asp:GridView>
            <br />
            Add user to selected Profile: <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox> <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_click" />
        </asp:Panel>
        <br /><br />
        Roles:
        <asp:Panel ID="pan2" runat="server" BorderWidth="1" Width="300">
            <asp:GridView ID="gvRoles" runat="Server" AutoGenerateColumns="true">
            </asp:GridView>
            <br />
            Add new Role: <asp:TextBox ID="txtNewRole" runat="server"></asp:TextBox> <asp:Button ID="btnRoleAdd" runat="server" Text="Add" OnClick="btnRoleAdd_click" />
        </asp:Panel>
        <br /><br />
        <asp:Panel ID="pan5" runat="server">
            Create user <asp:TextBox ID="txtCreate" runat="server" MaxLength="20"></asp:TextBox> <asp:Button id="btnCreate" runat="server" Text="Create" OnClick="btnCreate_click" />
            <br />
            <asp:Label ID="lblErrorCreate" runat="server" ForeColor="Red"></asp:Label>
        </asp:Panel>
        <asp:Panel ID="pan3" runat="server">
            Add user <asp:TextBox ID="txtUser" runat="Server"></asp:TextBox> to role <asp:TextBox ID="txtRole" runat="server"></asp:TextBox> <asp:Button ID="btnRoleAssign" runat="Server" Text="Add" OnClick="btnRoleAssign_click" />
            <br />
            <asp:Label ID="lblError" runat="Server" ForeColor="red"></asp:Label>
        </asp:Panel>
        <br /><br />
        <asp:Panel ID="pan4" runat="server">
            Change user <asp:TextBox ID="txtUser_Pass" runat="Server"></asp:TextBox> password to <asp:TextBox ID="txtNewPass" runat="server"></asp:TextBox> <asp:Button ID="btnChange" runat="Server" Text="Change" OnClick="btnChange_click" />
            <br />
            <asp:Label ID="lblError2" runat="Server" ForeColor="red"></asp:Label>
        </asp:Panel>
        <br /><br />
        <asp:Panel ID="Panel1" runat="server">
            Delete User <asp:TextBox ID="txtDelete" runat="Server"></asp:TextBox> <asp:Button ID="btnDelete" runat="Server" Text="Delete" OnClick="btnDelete_clicked" />
            <br />
            <asp:Label ID="lblError3" runat="Server" ForeColor="red"></asp:Label>
        </asp:Panel>
        <br /><br />
        <asp:Panel ID="Panel2" runat="server">
            Get password for <asp:TextBox ID="txtGetPassword" runat="Server"></asp:TextBox> <asp:Button ID="btnGetPassword" runat="Server" Text="Get" OnClick="btnGetPassword_click" />
            <br />
            <asp:Label ID="lblPW" runat="Server" ForeColor="red"></asp:Label>
        </asp:Panel>
        <br /><br />
        <asp:Panel ID="panApprove" runat="server">
            Approve user <asp:TextBox ID="txtApprove" runat="server"></asp:TextBox> <asp:Button ID="btnApprove" runat="server" Text="Approve" OnClick="txtApprove_click" />
            <br />
            <asp:Label ID="lblApproveError" runat="server" ForeColor="Red"></asp:Label>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
