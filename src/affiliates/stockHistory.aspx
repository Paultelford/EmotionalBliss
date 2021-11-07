<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="stockHistory.aspx.vb" Inherits="affiliates_stockHistory" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <table border="0" width="100%">
        <tr>
            <td>
                <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
                    <ProgressTemplate>
                        Please Wait....<img src="/images/loading.gif" width="16" height="16" />
                    </ProgressTemplate>
                </atlas:UpdateProgress>        
            </td>
            <td align="right">
                <asp:HyperLink ID="lnkPurchaseManagement" runat="server" Text="Back" Font-Bold="true" NavigateUrl="~/affiliates/purchaseManagement.aspx"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <br />
            <asp:GridView ID="gvStatement" runat="server" DataSourceID="sqlStatement" AutoGenerateColumns="false" Width="100%" SkinID="GridView" AllowPaging="true" PageSize="20" PagerSettings-Visible="false">
                <Columns>
                    <asp:BoundField HeaderText="Date" DataField="affDate" HtmlEncode="false" DataFormatString="{0:D}" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Product" DataField="affProductName" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Action" DataField="action" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Added" DataField="qtyAdd" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Removed" DataField="qtyRemove" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:HyperLinkField HeaderText="EBOrderID" DataNavigateUrlFields="id" DataTextField="userOrderID" DataNavigateUrlFormatString="orderView.aspx?id={0}" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="ExtOrderID" DataField="userExtOrderID" />
                </Columns>
            </asp:GridView>
            <eb:Pager id="pager1" runat="server" showTextBox="true"></eb:Pager>
        </ContentTemplate>
    </atlas:UpdatePanel>


    <asp:SqlDataSource ID="sqlStatement" runat="server" SelectCommand="procAffiliateEBDistributorStockByAffIDDateSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>