<%@ Page Title="" Language="VB" MasterPageFile="~/m_site.master" AutoEventWireup="false" CodeFile="news.aspx.vb" Inherits="news" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="pressLoggedin" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="contentTop" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0" width="100%">
        <tr>
            <td valign="top" width="70%">
                <asp:DetailsView ID="dvNews" runat="server" DataSourceID="sqlNews" AutoGenerateRows="false" GridLines="None" Width="100%">
                    <Fields>
                        <asp:BoundField DataField="headline" ItemStyle-Font-Bold="true" />
                        <asp:BoundField DataField="date" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField />
                        <asp:BoundField DataField="news" HtmlEncode="false" />
                    </Fields>
                </asp:DetailsView>
            </td>
            <td valign="top" align="right">
                <table border="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblLatestText" runat="server" Text="Latest Headlines -"></asp:Label>
                            <asp:GridView ID="gvHeadlines" runat="server" DataSourceID="sqlNewsHeadlines" AutoGenerateColumns="false" GridLines="None">
                                <Columns>
                                    <asp:HyperLinkField DataTextField="headline" DataNavigateUrlFields="id" DataNavigateUrlFormatString="news.aspx?nid={0}" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    
        

    <asp:SqlDataSource ID="sqlNews" runat="server" SelectCommand="procNewsByIDSelect2" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="nid" Name="id" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlNewsHeadlines" runat="server" SelectCommand="procNewsByDateSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="sqlNewsHeadlines_selecting">
        <SelectParameters>
            <asp:Parameter Name="viewable" Type="String" Size="10" DefaultValue="all" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

