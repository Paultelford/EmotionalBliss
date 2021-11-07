<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="affLinks.aspx.vb" Inherits="affiliates_affLinks" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    Your default link:<br />
    <asp:Label ID="lblDefault" runat="server" BackColor="#eeeeee"></asp:Label>
    <br /><br />
    Product links:<br />
    <asp:Literal ID="litProductLinks" runat="server"></asp:Literal>
    <asp:Panel ID="panOther" runat="server" Visible="false">
        Other Products:<br />
        <asp:DropDownList ID="drpOtherProducts" runat="server" OnSelectedIndexChanged="drpOtherProducts_selectedIndexChanged" DataTextField="saleName" DataValueField="id" DataSourceID="sqlOther" OnDataBinding="drpOtherProducts_dataBinding" AppendDataBoundItems="true" AutoPostBack="true">
        
        </asp:DropDownList><br />
        <asp:Label ID="lblOther" runat="server" BackColor="#eeeeee"></asp:Label>
    </asp:Panel>
    <br /><br />
    
    
    <asp:SqlDataSource ID="sqlOther" runat="server" SelectCommand="procProductOnSaleByNOTItemTypeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffCountryCode" Name="countryCode" Type="String" Size="5" />
            <asp:Parameter Name="itemType" Type="String" Size="30" DefaultValue="bproduct" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

