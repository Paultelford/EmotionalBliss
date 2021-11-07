<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="news.aspx.vb" Inherits="affiliates_news" ValidateRequest="false" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2>Add/Edit News Articles</h2>
    <br /><br />
    <asp:Label ID="lblInitText" runat="server" CssClass="text">
        Enter a new story below and click 'Save'<br />
        Or select a story to edit from the dropdown:
    </asp:Label>
    &nbsp;
    <asp:DropDownList ID="drpNews" runat="server" DataSourceID="sqlHeadlines" DataTextField="headline" DataValueField="id" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="drpNews_selectedIndexChanged">
        <asp:ListItem Text="Select..." Value=""></asp:ListItem>
    </asp:DropDownList><br /><br />
    <table border="0" width="100%">
        <tr>
            <td width="70">
                <asp:Label ID="lblHeadlineText" runat="server" Text="Headline:" Font-Bold="true" CssClass="text"></asp:Label>&nbsp;        
            </td>
            <td>
                <asp:TextBox ID="txtHeadline" runat="server" MaxLength="200" Width="400"></asp:TextBox>
                <asp:RequiredFieldValidator ID="reqTxtHeadline" runat="server" ControlToValidate="txtHeadline" ErrorMessage="* Required"></asp:RequiredFieldValidator>
            </td>
            <td align="right">
                <asp:CheckBox ID="chkActive" runat="server" Text="Active" />&nbsp;
                <asp:CheckBox ID="chkFront" runat="server" Text="Show on front page" Visible="false" />
                <asp:DropDownList ID="drpPublic" runat="server">
                    <asp:ListItem Text="Viewable by all" Value="all"></asp:ListItem>
                    <asp:ListItem Text="Media Only" Value="media"></asp:ListItem>
                    <asp:ListItem Text="Public Only" Value="public"></asp:ListItem>
                    <asp:ListItem Text="Media & B2B" Value="mediab2b"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td colspan="2">
                <FCKeditorV2:FCKeditor id="FCKeditor1" runat="server" BasePath="~/EBEditor/" UseBROnCarriageReturn="true" EnableSourceXHTML="true" height="500"></FCKeditorV2:FCKeditor>        
                <div style="float: left;">
                    <asp:Label ID="lblError" runat="server" CssClass="text" ForeColor="red"></asp:Label>
                </div>
                <div style="float: right;">
                    <asp:Button ID="btnSave" runat="server" Text="Save Article" OnClick="btnSave_click" />
                    <asp:Button ID="btnEdit" runat="server" Text="Save Changes" OnClick="btnEdit_click" Visible="false" />
                </div>
            </td>
        </tr>
    </table>
        
    
    <asp:SqlDataSource ID="sqlHeadlines" runat="server" SelectCommand="procNewsHeadlinesSelectAll" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

