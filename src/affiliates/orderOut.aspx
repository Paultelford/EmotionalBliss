<%@ Page Title="Outstanding Orders" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="orderOut.aspx.vb" Inherits="orderOut" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <center>
    <br />
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false" AssociatedUpdatePanelID="update1">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>    
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            <asp:Table ID="tblOut" runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                        Order Type:&nbsp;
                        <asp:DropDownList ID="drpStatus" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="Deferred" Value="deferred"></asp:ListItem>
                            <asp:ListItem Text="Placed" Value="placed"></asp:ListItem>
                            <asp:ListItem Text="Paid" Value="paid" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Failed" Value="failed"></asp:ListItem>
                        </asp:DropDownList><br /><br />
                        <asp:GridView ID="gvOrders" runat="server" DataSourceID="sqlOrders" AutoGenerateColumns="false" GridLines="None" EmptyDataText="None found">
                            <Columns>
                                <asp:BoundField HeaderText="Country" DataField="countryname" />
                                <asp:BoundField ItemStyle-Width="80" />
                                <asp:BoundField HeaderText="Orders" DataField="orders" />
                            </Columns>
                        </asp:GridView>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </ContentTemplate>
    </atlas:UpdatePanel>
    </center>
    <asp:SqlDataSource id="sqlOrders" runat="server" SelectCommand="procShopOrderOutstandingSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpStatus" PropertyName="selectedValue" Name="status" Type="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

