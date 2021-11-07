<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentValue.aspx.vb" Inherits="maintenance_componentValue" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    Manufacturer:&nbsp;
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:DropDownList ID="drpMan" runat="server" DataSourceID="SqlManufacturers" DataTextField="manufacturerName" DataValueField="manufacturerID" AutoPostBack="true" AppendDataBoundItems="true">
                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                <asp:ListItem Text="All..." Value="%"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:Table ID="tblValues" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        <b>Component</b>
                    </asp:TableCell>
                    <asp:TableCell>
                        <b>Stock</b>
                    </asp:TableCell>
                    <asp:TableCell>
                        <b>Value</b>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <br /><br />
            <asp:Label ID="lblErrorEx" runat="server" ForeColor="red"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="SqlManufacturers" runat="server" SelectCommand="procManufacturersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

