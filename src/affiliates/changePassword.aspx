<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="changePassword.aspx.vb" Inherits="affiliates_changePassword" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table runat="server" id="tblChangePassword">
        <tr>
            <td>
                Current Password:
            </td>
            <td width="60">&nbsp;</td>
            <td>
                <asp:TextBox ID="txtCurrentPassword" runat="server" maxlength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqTxtCurrentPassword" runat="server" ControlToValidate="txtCurrentPassword" ErrorMessage="* Required" Display="Static"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                New Password:
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqTxtNewPassword" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="* Required" Display="Static"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td>
                Confirm New Password:
            </td>
            <td>&nbsp;</td>
            <td>
                <asp:TextBox ID="txtConfirm" runat="server" MaxLength="20"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqTxtConfirm" runat="server" ControlToValidate="txtConfirm" ErrorMessage="* Required" Display="Static"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
            <td>
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_click" />
            </td>
        </tr>
    </table>
    <asp:Label ID="lblComplete" runat="server"></asp:Label>
</asp:Content>

