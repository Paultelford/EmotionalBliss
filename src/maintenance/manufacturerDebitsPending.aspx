<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="manufacturerDebitsPending.aspx.vb" Inherits="maintenance_manufacturerDebitsPending" title="Untitled Page" %>
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
   
                
            <table border="0">
                <tr>
                    <td valign="top">
                        Type of Debit Note:        
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="Pending" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Complete" Value="0"></asp:ListItem>
                        </asp:DropDownList>       
                    </td>
                    <td width="60">&nbsp;</td>
                    <td>
                        <eb:DateControl id="date1" runat="server"></eb:DateControl>
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:GridView ID="gvPending" runat="server" DataSourceID="SqlPending" DataKeyNames="debitID" GridLines="none" AutoGenerateColumns="false" EmptyDataText="No Debit Notes found for selected Date Range." OnDataBound="gvPending_dataBound" OnSelectedIndexChanged="gvPending_selectedIndexChanged">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:BoundField HeaderText="Debit Note #" DataField="debitID" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("debitDate")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Items" DataField="qty" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="VatRate" DataField="debitVatRate" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Total Claim" DataField="debitTotalClaim" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Complete Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("debitCompleteDate")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Paid Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("debitPaidDate")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:CommandField SelectText="Set Prices" ShowSelectButton="true" />
                </Columns>
            </asp:GridView>
            <br /><br />
            <asp:Panel ID="panPrices" runat="server" Visible="false" BorderWidth="1" Width="600">
                <table border="0">
                    <tr>
                        <td valign="top">
                            <asp:GridView ID="gvComponents" runat="server" AutoGenerateColumns="false" DataKeyNames="componentID" DataSourceID="SqlComponents" ShowFooter="true" GridLines="none">
                                <HeaderStyle Font-Bold="true" />
                                <FooterStyle VerticalAlign="top" />
                                <Columns>
                                    <asp:TemplateField HeaderText="Component">
                                        <ItemTemplate>
                                            <asp:Label ID="lblComponent" runat="server" Text='<%# Eval("componentName") %>'></asp:Label>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            VAT Rate<br />
                                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_submit" />
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Qty" DataField="qty" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:TemplateField HeaderText="Unit Price">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtPrice" runat="server" Width="40"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="Server" ControlToValidate="txtPrice" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator><asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtPrice" MinimumValue="0" MaximumValue="99999" ErrorMessage="* Invalid" Display="dynamic"></asp:RangeValidator>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:TextBox ID="txtVat" runat="server" Width="40"></asp:TextBox>%<asp:RequiredFieldValidator ID="req2" runat="Server" ControlToValidate="txtVat" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator><asp:RangeValidator ID="ran2" runat="server" ControlToValidate="txtVat" MinimumValue="0" MaximumValue="99999" ErrorMessage="* Invalid" Display="dynamic"></asp:RangeValidator>
                                        </FooterTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>        
                        </td>
                        <td width="40">&nbsp;</td>
                        <td valign="top" align="right">
                            <b>Instructions:&nbsp;&nbsp;</b><br />
                            <asp:TextBox ID="txtInstructions" runat="server" TextMode="MultiLine" Rows="4" Columns="28"></asp:TextBox>        
                        </td>
                    </tr>
                </table>
            </asp:Panel>                

    
    <asp:SqlDataSource ID="SqlPending" runat="server" SelectCommand="procDebitPendingSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpType" PropertyName="selectedValue" Name="pending" Type="int16" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="SqlComponents" runat="server" SelectCommand="procDebitComponentsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvPending" PropertyName="selectedValue" Name="debitID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

