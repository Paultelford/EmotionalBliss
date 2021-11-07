<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" Theme="WinXP_Blue" CodeFile="emailLog.aspx.vb" Inherits="affiliates_emailLog" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManagerProxy id="smp1" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <br />
    
            <asp:Panel ID="panLog" runat="server">
                <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl>
                <br /><br />
                <asp:GridView ID="gvEmailLog" runat="server" AutoGenerateColumns="false" DataKeyNames="column1" DataSourceID="sqlEmailLog" EmptyDataText="No data found for selected dates." SkinID="GridView" OnSelectedIndexChanged="gvEmailLog_selectedIndexChamged">
                    <Columns>
                        <asp:TemplateField HeaderText="Date">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnk" runat="server" Text='<%# showDate(Eval("column1")) %>' CommandName="select"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="No of Emails" DataField="items" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="panDetails" runat="server" Visible="false">
                <table border="0" width="500">
                    <tr>
                        <td>
                            <asp:Label ID="lblDateHeader" runat="server"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:LinkButton ID="lnkBack" runat="server" Text="Back" Font-Bold="true" OnClick="lnkBack_click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="gvDetails" runat="server" DataSourceID="sqlEmailDetails" AutoGenerateColumns="false" SkinID="GridView" DataKeyNames="trackerID" OnRowUpdating="gvDetails_rowUpdating">
                    <Columns>
                        <asp:HyperLinkField HeaderText="Order ID" DataTextField="userOrderID" DataNavigateUrlFormatString="orderView.aspx?id={0}" DataNavigateUrlFields="trackerOrderID" />
                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                        <asp:BoundField HeaderText="Tracker" DataField="trackerCode" ReadOnly="true" />
                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                        <asp:BoundField HeaderText="Email" DataField="trackerEmail" ReadOnly="true" />
                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                        <asp:TemplateField HeaderText="UPS Invoice" ItemStyle-Width="90">
                            <ItemTemplate>
                                <asp:Label ID="lblUPS" runat="server" Text='<%# Eval("trackerups") %>'></asp:Label>
                                <asp:HiddenField ID="hidOrderID" runat="server" Value='<%# Eval("trackerorderID") %>' />
                            </ItemTemplate>          
                            <EditItemTemplate>
                                <asp:TextBox ID="txtUPS" runat="server" Text='<%# Bind("trackerups") %>' Width="80" OnLoad="txtUPS_load" ValidationGroup="ups"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqUPS" runat="server" Text="* Rquired" ControlToValidate="txtUPS" Display="Dynamic" ValidationGroup="ups"></asp:RequiredFieldValidator>
                                <asp:HiddenField ID="hidOrderID" runat="server" Value='<%# Eval("trackerorderID") %>' />
                            </EditItemTemplate>                  
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                        <asp:CheckBoxField HeaderText="Sent" DataField="trackerSent" ReadOnly="true" />
                        <asp:CommandField EditText="Edit UPS" ShowEditButton="true" ValidationGroup="ups" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        
    
    <asp:SqlDataSource ID="sqlEmailLog" runat="server" SelectCommand="procTrackerByDateSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="orderCountryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlEmailDetails" runat="server" SelectCommand="procTrackerByDaySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procTrackerByIDUPSUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvEmailLog" PropertyName="selectedValue" Name="trackerDate" Type="datetime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="orderCountryCode" Type="string" Size="5" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="trackerID" Type="Int32" />
            <asp:Parameter Name="trackerUPS" Type="String" Size="50" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

