<%@ Page Language="VB" MasterPageFile="~/MasterPageSite.master" AutoEventWireup="false" CodeFile="affStatement.aspx.vb" Inherits="maintenance_affStatement" title="Affiliate Statement" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <br />
            
            <asp:GridView id="gvStatement" runat="server" DataSourceID="sqlStatement" AutoGenerateColumns="false" SkinID="GridView" ShowFooter="true" OnDataBound="gvStatement_dataBound"> 
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# showDate(Eval("statementDate")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:TemplateField HeaderText="Order ID">
                        <ItemTemplate>
                            <asp:Label ID="lblOrderID" runat="server" Text='<%# showOrderID(Eval("extOrderID"),Eval("extUserOrderID"),Eval("orderPrefix"),Eval("newOrderID"),Eval("orderCountryCode")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Action" DataField="action" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Credit" DataField="statementCredit" FooterText="crF" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Debit" DataField="statementDebit" FooterText="drF" />
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>


    <asp:SqlDataSource ID="sqlStatement" runat="server" SelectCommand="procAffiliateStatementByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffID" Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

