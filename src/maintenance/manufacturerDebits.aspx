<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="manufacturerDebits.aspx.vb" Inherits="maintenance_manufacturerDebits" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="../images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table>
                <tr>
                    <td valign="top">
                        Manufacturer:
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="drpMan" runat="server" DataSourceID="SqlManufacturers" AutoPostBack="true" AppendDataBoundItems="true" DataTextField="manufacturername" DataValueField="manufacturerID" OnSelectedIndexChanged="drpMan_selectedIndexChanged">
                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                        </asp:DropDownList>        
                    </td>
                    <td width="60">&nbsp;</td>
                    <td>
                        <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl>        
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gvComponents" runat="server" DataKeyNames="componentHistoryID" DataSourceID="SqlComponents" AutoGenerateColumns="false" EmptyDataText="No components found." GridLines="none" ShowFooter="True">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="Server" Text='<%# showDate(Eval("dateChanged")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Component" DataField="componentName" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Qty" DataField="qtyFailedAdded" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:HyperLinkField HeaderText="Production Batch" DataNavigateUrlFields="productAssemblyID" DataNavigateUrlFormatString="productAssemblyView.aspx?id={0}" DataTextField="productAssemblyID" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Add">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkAdd" runat="Server" />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnSubmit" runat="Server" Text="Submit" OnClick="btnSubmit_click" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblCompID" runat="server" Text='<%# Eval("componentID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br /><br />
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="SqlManufacturers" runat="server" SelectCommand="procManufacturersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlComponents" runat="server" SelectCommand="procComponentHistoryByManIDFailSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMan" PropertyName="selectedValue" Name="manID" Type="int32" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

