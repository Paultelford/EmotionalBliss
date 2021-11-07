<%@ Control Language="C#" AutoEventWireup="true" CodeFile="pager.ascx.cs" Inherits="pager" %>
<asp:Table ID="tblPager" runat="server" Width="100%">
    <asp:TableRow>
        <asp:TableCell HorizontalAlign="center">
            <asp:LinkButton ID="lnkPrev" runat="server" Text="Prev" OnClick="lnkPrev_click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:Label ID="lblPageCurrent" runat="Server" Text="1"></asp:Label>
            &nbsp;of&nbsp;
            <asp:Label ID="lblPageTotal" runat="Server"></asp:Label>
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:LinkButton ID="lnkNext" runat="server" Text="Next" OnClick="lnkNext_click"></asp:LinkButton>
            &nbsp;&nbsp;&nbsp;&nbsp;            
            <asp:TextBox ID="txtGoto" runat="server" Width="30" OnTextChanged="txtGoto_textChanged" Visible="false" AutoPostBack="true" ValidationGroup="goto"></asp:TextBox>
            <asp:RangeValidator ID="ranTxtGoto" runat="server" ControlToValidate="txtGoto" Type="Integer" MinimumValue="1" MaximumValue="1000" ValidationGroup="goto" Display="dynamic" ErrorMessage="*"></asp:RangeValidator>
            &nbsp;&nbsp;&nbsp;&nbsp;            
            <asp:DropDownList ID="drpPageSize" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPageSize_selectedIndexChanged">
                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                <asp:ListItem Text="20" Value="20" Selected="true"></asp:ListItem>
                <asp:ListItem Text="50" Value="50"></asp:ListItem>
                <asp:ListItem Text="1000" Value="1000"></asp:ListItem>
            </asp:DropDownList>
        </asp:TableCell>
    </asp:TableRow>
</asp:Table>
<asp:Label ID="lblPagerError" runat="Server"></asp:Label>