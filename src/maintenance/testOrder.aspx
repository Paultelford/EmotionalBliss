<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Trace="false" AutoEventWireup="false" CodeFile="testOrder.aspx.vb" Inherits="maintenance_testOrder" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
        <asp:Panel ID="panHide" runat="server" Visible="false">
            <b>Last Order Added:</b> <asp:Label ID="lblPreamble" runat="server"></asp:Label>&nbsp;(<asp:label ID="lblPreambleDate" runat="server"></asp:label>)
            <br /><br />
        </asp:Panel>
            <table border="0">
                <tr>
                    <td>
                        <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl>
                    </td>
                    <td>
                        <asp:Button ID="BtnSubmit" runat="server" Text="Generate Orders" OnClick="BtnSubmit_click" /><br />
                    </td>
                </tr>
            </table>       
            <br /><br />
            <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
                <ProgressTemplate>
                    Transfering Results....<img src="../images/loading.gif" width="16" height="16" />
                </ProgressTemplate>
            </atlas:UpdateProgress>
            <br /><br />
            <asp:Label ID="lblResult" runat="server"></asp:Label>
            <br /><br />
            <asp:Label ID="lblOutput" runat="Server"></asp:Label>
            <br /><br />
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    
</asp:Content>

