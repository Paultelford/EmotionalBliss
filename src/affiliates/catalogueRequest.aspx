<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="catalogueRequest.aspx.vb" Inherits="affiliates_catalogueRequest" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:DetailsView ID="dvRequest" runat="server" DataSourceID="sqlRequest" DefaultMode="insert" GridLines="none" OnItemInserted="dvRequest_inserted">
                <Fields>
                    <asp:BoundField HeaderText="Name:" DataField="name" />
                    <asp:BoundField HeaderText="Address:" DataField="add1" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="add2" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="add3" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="add4" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="add5" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="Postcode:" DataField="postcode" ControlStyle-Width="140" />
                    <asp:BoundField HeaderText="Phone:" DataField="phone" ControlStyle-Width="140" />
                    <asp:CheckBoxField HeaderText="Does customer want future<br>catalogues to be sent ?" DataField="maillist" />
                    <asp:CommandField HeaderText="" InsertText="Add Request" ShowCancelButton="false" ShowInsertButton="true" ButtonType="Button" ItemStyle-HorizontalAlign="right" />
                </Fields>
            </asp:DetailsView>
            <asp:Label ID="lblComplete" runat="server" ForeColor="red" Visible="false" Text="Request successfully added."></asp:Label><br />
            <asp:LinkButton ID="lnkAgain" runat="server" Text="Add another" OnClick="lnkAgain_click" Visible="false"></asp:LinkButton>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlRequest" runat="server" InsertCommand="procCatalogueRequestInsert" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <InsertParameters>
            <asp:Parameter Name="name" Type="string" Size="50" />
            <asp:Parameter Name="add1" Type="string" Size="50" />
            <asp:Parameter Name="add2" Type="string" Size="50" />
            <asp:Parameter Name="add3" Type="string" Size="50" />
            <asp:Parameter Name="add4" Type="string" Size="50" />
            <asp:Parameter Name="add5" Type="string" Size="50" />
            <asp:Parameter Name="postcode" Type="string" Size="20" />
            <asp:Parameter Name="phone" Type="string" Size="20" />
            <asp:Parameter Name="maillist" Type="Boolean" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="country" Type="string" Size="5" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>

