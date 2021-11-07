<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="royaltyTrans.aspx.vb" Inherits="affiliates_royaltyTrans" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <asp:DropDownList ID="drpEarners" runat="server" Visible="false" DataSourceID="sqlEarners" DataTextField="name" DataValueField="affID" AutoPostBack="true" OnSelectedIndexChanged="drpEarners_selectedIndexChanged">
            </asp:DropDownList>
            <br /><br />
            <asp:GridView ID="gvStatement" runat="server" DataSourceID="sqlStatement" AutoGenerateColumns="false" DataKeyNames="orderid" SkinID="GridView" OnDataBound="gvStatement_dataBound">
                <Columns>
                    <asp:BoundField HeaderText="Date" DataField="transDate" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:HyperLinkField HeaderText="Order ID" DataTextField="userOrderID" DataNavigateUrlFields="orderid,affID" Target="_blank" DataNavigateUrlFormatString="royaltyOrder.aspx?id={0}&aid={1}" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Action" DataField="action" DataFormatString="{0:n2}" NullDisplayText="0.00" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Credit" DataField="credit" DataFormatString="{0:n2}" NullDisplayText="0.00" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Debit" DataField="debit" DataFormatString="{0:n2}" NullDisplayText="0.00" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Balance">
                        <ItemTemplate>
                            <asp:Label ID="lblBalance" runat="Server"></asp:Label>
                            <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("actionID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlEarners" runat="server" SelectCommand="procAffiliateByCountryCodeRoyaltySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlStatement" runat="server" SelectCommand="procRoyaltyTransactionsByAffIDDateSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="sqlStatement_selecting">
        <SelectParameters>
            <asp:Parameter Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

