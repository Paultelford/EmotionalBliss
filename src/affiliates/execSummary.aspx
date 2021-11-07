<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="execSummary.aspx.vb" Inherits="affiliates_execSummary" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy id="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
            
            <asp:Panel ID="panSummary" runat="server">
                <eb:DateControl id="date1" runat="server"></eb:DateControl>
                <asp:DropDownList ID="drpCountryOverview" runat="server" Visible="false" AutoPostBack="true" OnSelectedIndexChanged="drpCountryOverview_selectedIndexChanged">
                    <asp:ListItem Text="Just GB" Value="gb" Selected="true"></asp:ListItem>
                    <asp:ListItem Text="All Countrys" Value="%"></asp:ListItem>
                </asp:DropDownList>
                <br /><br />
                <asp:GridView ID="gvSummary" runat="server" DataSourceID="sqlSummary" AutoGenerateColumns="false" ShowFooter="true" SkinID="GridView" OnDataBound="gvSummary_dataBound">
                    <FooterStyle Font-Bold="true" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%# Eval("prefix") & Eval("actionID") %>'></asp:Label>
                                <asp:Label ID="lblActionID" runat="server" Text='<%# Eval("actionID") %>' Visible="false"></asp:Label>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lblTotalText" runat="Server" Text="Totals" Font-Bold="true"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="" DataField="actionID" Visible="false" />
                        <asp:BoundField HeaderText="EC Sales" DataField="ec" Visible="false" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="" DataField="ec_amount" Visible="false" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="center" />
                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                        <asp:BoundField HeaderText="Non-EC Sales" DataField="nec" Visible="false" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="" DataField="nec_amount" Visible="false" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="center" />
                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                        <asp:BoundField HeaderText="Other Sales" DataField="other" Visible="false" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="" DataField="o_amount" Visible="false" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="center" />
                        <asp:BoundField ItemStyle-Width="40" Visible="false" />
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRegionalVat" runat="Server" Text='<%# Eval("regionalVat") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="" DataField="salesItems" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="" DataField="sales_amount" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="center" />
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblLocalVat" runat="server" Text='<%# Eval("salesVat") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Total Sales" ItemStyle-BackColor="#eeeeee" ItemStyle-HorizontalAlign="center" FooterStyle-HorizontalAlign="center" />
                        <asp:BoundField HeaderText="Total Vat" ItemStyle-BackColor="#eeeeee" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" />
                        <asp:BoundField HeaderText="Total Inc Vat" ItemStyle-BackColor="#eeeeee" ItemStyle-HorizontalAlign="right" FooterStyle-HorizontalAlign="right" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            
            
            
        <asp:SqlDataSource ID="sqlSummary" runat="server" SelectCommand="procSalesLedgerByCountryCodeDateSummarySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
                <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
                <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
            </SelectParameters>
        </asp:SqlDataSource>
</asp:Content>

