<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentHistory.aspx.vb" Inherits="maintenance_componentHistory" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    View By:
    <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true">
        <asp:ListItem Text="Components" Value="component"></asp:ListItem>    
        <asp:ListItem Text="Batches" Value="batch"></asp:ListItem>    
        <asp:ListItem Text="Component Orders" Value="order"></asp:ListItem>    
    </asp:DropDownList>
    <br /><br />
    <asp:GridView ID="gvList" runat="server" GridLines="none" AutoGenerateColumns="false" DataKeyNames="fieldID">
        <HeaderStyle HorizontalAlign="left" Font-Bold="true" />
        <RowStyle VerticalAlign="top" />
        <Columns>
            <asp:HyperLinkField HeaderText="Component" DataTextField="field1" DataNavigateUrlFields="fieldID,fieldType" DataNavigateUrlFormatString="componentHistoryView.aspx?cid={0}&type={1}" />
            <asp:BoundField HeaderText="Description" DataField="field2" />
        </Columns>
    </asp:GridView>
    
</asp:Content>

