<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="mainMenu.aspx.vb" Inherits="affiliates_mainMenu" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <br />
            <asp:GridView ID="gvMenu" runat="server" DataSourceID="SqlMenu" DataKeyNames="mapid" AutoGenerateColumns="false" SkinID="GridView" Width="100%" OnRowDataBound="gvMenu_rowDataBound">
                <Columns>
                    <asp:BoundField HeaderText="Link Description" DataField="navText" />
                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                    <asp:BoundField HeaderText="URL" DataField="url" ItemStyle-Width="50%" ControlStyle-Width="400" />
                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                    <asp:BoundField HeaderText="Menu Text" DataField="menuName" />
                    <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                    <asp:CommandField ShowEditButton="true" ShowCancelButton="true" EditText="Edit" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>    
    
    <asp:SqlDataSource ID="SqlMenu" runat="server" SelectCommand="procImageMapsByCountryCodeMenuSelect" SelectCommandType="StoredProcedure" UpdateCommand="procImageMapsByIDUpdate" UpdateCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="mapid" Type="int32" />
            <asp:Parameter Name="navText" Type="string" size="150" />
            <asp:Parameter Name="url" Type="string" size="150" />
            <asp:Parameter Name="menuName" Size="150" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

