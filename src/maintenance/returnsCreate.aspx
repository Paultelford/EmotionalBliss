<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="returnsCreate.aspx.vb" Inherits="maintenance_returnsCreate" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManagerProxy ID="smp1" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdateProgress ID="up1" runat="Server">
        <ProgressTemplate>
            Loading...<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
    <asp:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panCreate" runat="server">
                Select a Distributor to create return for:&nbsp;
                <asp:DropDownList ID="drpDistributors" runat="server" DataSourceID="sqlDistributors" DataTextField="affCompany" DataValueField="affID" AppendDataBoundItems="true" AutoPostBack="false">
                    <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                </asp:DropDownList>
                &nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnSubmit" runat="server" Text="Confirm" OnClick="btnSubmit_click" />
            </asp:Panel>
            <asp:Panel ID="panComplete" runat="server" Visible="false">
                The return has been created.<br />
                <br />
                Returns No: RMA<asp:Label ID="lblReturnsID" runat="server"></asp:Label>
                <br /><br /><br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    <br />
    <asp:Label ID="lblErrorDetail" runat="server" ForeColor="white"></asp:Label>
    <asp:SqlDataSource ID="sqlDistributors" runat="server" SelectCommand="procAffiliatesDistributorsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

