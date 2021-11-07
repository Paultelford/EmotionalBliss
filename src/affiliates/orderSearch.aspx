<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" Theme="winxp_blue" CodeFile="orderSearch.aspx.vb" Inherits="affiliates_orderSearch" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
   
            <table>
                <tr>
                    <td>
                        Search Query:
                    </td>
                    <td colspan="2">    
                        <nobr>
                            <asp:TextBox ID="txtSearch" runat="Server" MaxLength="20"></asp:TextBox>
                        </nobr>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:RadioButtonList ID="radCriteria" runat="server">
                            <asp:ListItem Text="Order ID" Value="orderid" Selected="true"></asp:ListItem>
                            <asp:ListItem Text="Name(Bill)" Value="namebill"></asp:ListItem>
                            <asp:ListItem Text="Name(Ship)" Value="nameship"></asp:ListItem>
                            <asp:ListItem Text="Part of address(Bill)" Value="addbill"></asp:ListItem>
                            <asp:ListItem Text="Part of address(Ship)" Value="addship"></asp:ListItem>
                            <asp:ListItem Text="Email" Value="email"></asp:ListItem>
                            <asp:ListItem Text="Order Date" Value="orderdate"></asp:ListItem>
                            <asp:ListItem Text="Cheque Acc No" Value="chequenum"></asp:ListItem>
                            <asp:ListItem Text="Tracker No" Value="trackernum"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td valign="top">
                        <asp:button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_click" />
                        
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:GridView ID="gvResults" runat="server" Width="100%" AutoGenerateColumns="false" EmptyDataText="No results found!" SkinID="GridView" OnPageIndexChanging="gvResults_pageIndexChanging"  AllowPaging="true" PagerSettings-Visible="false" PageSize="20">
                <Columns>
                    <asp:HyperLinkField HeaderText="Order ID" DataTextField="userOrderID" DataNavigateUrlFields="id" DataNavigateUrlFormatString="~/affiliates/orderView.aspx?id={0}" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Order Date">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderDate" runat="server" Text='<%# showDate(Eval("orderDate")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Customer Name" DataField="billName" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Order Total" DataField="orderTotal" DataFormatString="{0:c2}" HtmlEncode="false" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Order Status" DataField="orderStatus" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Country" DataField="billCountry" />
                </Columns>
            </asp:GridView>
            <eb:Pager id="pager1" runat="server" showTextBox="true" gv="gvResults"></eb:Pager>
            <asp:Label ID="lblError" runat="server"></asp:Label>

</asp:Content>

