<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentList.aspx.vb" Inherits="maintenance_componentList" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:DropDownList ID="drpMaster" runat="server" AppendDataBoundItems="true" DataSourceID="sqlMasters" DataTextField="name" DataValueField="masterID" AutoPostBack="true" OnSelectedIndexChanged="drpMaster_selectedIndexChanged">
        <asp:ListItem Text="All" Value="%"></asp:ListItem>
    </asp:DropDownList>
    <br /><br />
    <atlas:ScriptManagerProxy ID="ScriptManagerProxy1" runat="server">
    </atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="Update1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvComponents" runat="server" AutoGenerateColumns="false" DataSourceID="sqlComponents" CellPadding="4" GridLines="none" EmptyDataText="No components exist for this Master Type.">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:BoundField HeaderText="Component" DataField="componentName" />
                    <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" />
                    <asp:BoundField HeaderText="MasterType" DataField="masterName" />
                    <asp:HyperLinkField Text="Edit" DataNavigateUrlFormatString="componentAdd.aspx?edit={0}" DataNavigateUrlFields="componentID" />
                    <asp:HyperLinkField Text="Stock" DataNavigateUrlFormatString="componentStock.aspx?id={0}" DataNavigateUrlFields="componentID" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
            <atlas:AsyncPostBackTrigger ControlID="drpMaster" EventName="selectedIndexChanged" />
        </Triggers>
    </atlas:UpdatePanel>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000">
        <ProgressTemplate>
            Loading....<img src="../images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
            
     
    
    <asp:SqlDataSource ID="sqlMasters" runat="server" SelectCommand="procComponentMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlComponents" runat="server" SelectCommand="procComponentsByMasterIDListSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMaster" Name="masterID" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>