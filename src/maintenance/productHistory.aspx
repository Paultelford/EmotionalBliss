<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productHistory.aspx.vb" Inherits="maintenance_productHistory" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Component Master: 
            <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="SqlMasters" DataTextField="Name" DataValueField="masterID" AutoPostBack="true" AppendDataBoundItems="true">
                <asp:ListItem Text="All" Value="%"></asp:ListItem>
            </asp:DropDownList>    
            <br /><br />
            <asp:GridView ID="gvProducts" runat="server" DataSourceID="SqlProducts" SkinID="GridView" DataKeyNames="productID" AutoGenerateColumns="false" GridLines="none">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:HyperLinkField HeaderText="Product" DataTextField="productName" DataNavigateUrlFields="productID" DataNavigateUrlFormatString="productHistoryView.aspx?id={0}" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Code" DataField="ref" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
       
    
    <asp:SqlDataSource ID="SqlMasters" runat="server" SelectCommand="procProductMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProducts" runat="server" SelectCommand="procProductsByMasterIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMaster" PropertyName="selectedValue" Name="masterID" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

