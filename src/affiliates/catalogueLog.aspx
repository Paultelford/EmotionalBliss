<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="catalogueLog.aspx.vb" Theme="WinXP_Blue" Inherits="affiliates_catalogueLog" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>

            <table border="0" width="100%">
                <tr>
                    <td width="80">&nbsp;</td>
                    <td>
                        <eb:DateControl id="date1" runat="server"></eb:DateControl>
                        By:&nbsp;
                        <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="Request Date" Value="request"></asp:ListItem>
                            <asp:ListItem Text="Despatch Date" Value="despatch"></asp:ListItem>
                        </asp:DropDownList>
                        <br /><br />
                        <asp:GridView ID="gvCatlog" runat="server" DataSourceID="sqlCatlog" DataKeyNames="date" AutoGenerateColumns="false" GridLines="none" OnSelectedIndexChanged="gvCatalogue_selectedIndexChanged" SkinID="GridViewRedBG">
                            <Columns>
                                <asp:TemplateField HeaderText="Date">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDate" runat="server" CommandName="select" Text='<%# formatDateTime(Eval("day"),DateFormat.LongDate) %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="80" />
                                <asp:BoundField HeaderText="Requests" DataField="items" />
                            </Columns>
                        </asp:GridView>        
                        <br /><br />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvDay" runat="server" Visible="false" DataSourceID="sqlDay" AutoGenerateColumns="false" SkinID="GridView" Width="100%" EmptyDataText="No data found">
                <Columns>
                    <asp:BoundField HeaderText="Name" DataField="name" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Add1" DataField="add1" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Add2" DataField="add2" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Add3" DataField="add3" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Postcode" DataField="postcode" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Request Date" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" DataField="requestDate" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Despatch Date" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" DataField="despatched" />
                </Columns>
            </asp:GridView>
           
            
    
    <asp:SqlDataSource ID="sqlCatlog" runat="Server" SelectCommand="procCatalogueRequestByDateSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="type" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlDay" runat="Server" SelectCommand="procCatalogueRequestByDaySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvCatlog" PropertyName="selectedValue" Name="day" Type="datetime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="type" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

