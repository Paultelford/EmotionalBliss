<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="scanView.aspx.vb" Inherits="affiliates_scanView" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false" AssociatedUpdatePanelID="update1">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>    
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server" ></eb:DateControl><br />
            <asp:DropDownList ID="drpType" runat="Server" AutoPostBack="true" OnSelectedIndexChanged="drpType_selectedIndexChanged">
                <asp:ListItem Text="Failed Scans" Value="failed"></asp:ListItem>
                <asp:ListItem Text="All Scans" Value="all"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:GridView ID="gvScans" runat="server" DataSourceID="sqlScans" AutoGenerateColumns="false" SkinID="GridView" Width="100%" AllowPaging="true" PageSize="20" PagerSettings-Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="OrderID">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkOrderID" runat="Server" Text='<%# Eval("userOrderID") %>' CommandArgument='<%# Eval("scanOrderID") %>' OnClick="lnkOrderID_click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Scan Date" DataField="scanDate" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Function" DataField="scanFunction" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Error" DataField="scanMsg" />
                </Columns>
            </asp:GridView>
            <eb:Pager id="pager1" runat="server" showTextBox="true"></eb:Pager>
            <asp:Panel ID="panDetails" runat="server" Visible="false">
                <asp:Table ID="tblDetails" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            OrderID:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblOrderID" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell>&nbsp;</asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            Scan Date:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:Label ID="lblScanDate" runat="Server"></asp:Label>
                        </asp:TableCell>
                        <asp:TableCell Width="200" HorizontalAlign="right">
                            <asp:LinkButton ID="lnkBack" runat="Server" Text="Previous View" OnClick="lnkBack_click"></asp:LinkButton>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:GridView ID="gvFunctions" runat="server" AutoGenerateColumns="false" SkinID="GridView">
                    <Columns>
                        <asp:BoundField HeaderText="Function" DataField="scanFunction" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:CheckBoxField HeaderText="Success" DataField="scanSuccess" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>    
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
            
            <asp:SqlDataSource ID="sqlScans" runat="server" SelectCommand="procScanByDateCountryTypeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
                <SelectParameters>
                    <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
                    <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
                    <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
                    <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="type" Type="string" Size="6" />
                </SelectParameters>
            </asp:SqlDataSource>
</asp:Content>

