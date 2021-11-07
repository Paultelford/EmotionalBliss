<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="returnsLog.aspx.vb" Inherits="maintenance_returnsLog" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:scriptmanagerproxy id="smp1" runat="server"></asp:scriptmanagerproxy>
    <asp:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table border="0" width="100%">
                <tr>
                    <td>
                        <eb:DateControl id="date1" runat="server"></eb:DateControl>        
                    </td>
                    <td align="right" valign="top">
                        Quick Link: <asp:TextBox ID="txtQuick" runat="server" AutoPostBack="false" ValidationGroup="quick"></asp:TextBox><asp:Button ID="btnQuickSubmit" runat="server" Text="Proceed" OnClick="btnQuickSubmit_click" ValidationGroup="quick" /><br />
                        <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtQuick" ErrorMessage="* Required" Display="dynamic" ValidationGroup="quick"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtQuick" ErrorMessage="* Numeric only" Display="dynamic" MinimumValue="0" MaximumValue="999999999" ValidationGroup="quick"></asp:RangeValidator>
                        <asp:Label ID="lblQuickError" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
            </table>    
            <br /><br />
            <table border="0">
                <tr>
                    <td>
                        View:        
                    </td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDate_selectedIndexChanged">
                            <asp:ListItem Text="Outstanding" Value="created"></asp:ListItem>  
                            <asp:ListItem Text="Pending" Value="recieved"></asp:ListItem>  
                            <asp:ListItem Text="Checked" Value="checked"></asp:ListItem>  
                            <asp:ListItem Text="Complete" Value="complete"></asp:ListItem>  
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Order By:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpOrder" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOrder_selectedIndexChanged">
                            <asp:ListItem Text="Date return was created" Value="created"></asp:ListItem>
                            <asp:ListItem Text="Date items recieved by EB" Value="recieved"></asp:ListItem>  
                            <asp:ListItem Text="Date items were checked" Value="checked"></asp:ListItem>  
                            <asp:ListItem Text="Date return was set complete" Value="complete"></asp:ListItem>  
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            
            <asp:GridView ID="gvReturns" runat="server" DataSourceID="sqlReturns" AutoGenerateColumns="false" SkinID="GridView" OnDataBound="gvReturns_dataBound" EmptyDataText="No returns found for the specified date range.">
                <Columns>
                    <asp:BoundField HeaderText="Return Code" DataField="rma" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Distributor" DataField="affName" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Country" DataField="countryName" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Email" DataField="affEmail" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <asp:HyperLink ID="lnkStatus" runat="server" Text='<%# Eval("status") %>'></asp:HyperLink>
                            <asp:Label ID="lblReturnsID" runat="server" Visible="false" Text='<%# Eval("returnsID") %>'></asp:Label>
                            <asp:Label ID="lblCountryCode" runat="server" Visible="false" Text='<%# Eval("countryCode1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="D" DataField="oDate" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
    <asp:SqlDataSource ID="sqlReturns" runat="server" SelectCommand="procReturnsByDateDistSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="dateField" Type="string" Size="10" />
            <asp:ControlParameter ControlID="drpOrder" PropertyName="selectedValue" Name="orderField" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

