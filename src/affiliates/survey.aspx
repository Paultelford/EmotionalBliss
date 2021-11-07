<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="survey.aspx.vb" Inherits="affiliates_survey" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        
    <eb:DateControl ID="date1" runat="server" OnDateChanged="date1_dateChanged" />
    <asp:DropDownList ID="drpOrderType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOrderType_selectedIndexChanged">
        <asp:ListItem Text="All Orders" Value="%"></asp:ListItem>
        <asp:ListItem Text="Call Centre" Value="callcentre"></asp:ListItem>
        <asp:ListItem Text="Web" Value="other"></asp:ListItem>
    </asp:DropDownList>
    <br /><br />
    Number of Call Centre orders: <asp:Label ID="lblCount" runat="server"></asp:Label>
    <br /><br />
    <b>Where did you hear about us:</b><br />
    <asp:GridView ID="gvSurvey1" runat="server" DataSourceID="sqlSurvey1" DataKeyNames="surveyQ1" GridLines="None" AutoGenerateColumns="false" OnDataBound="gvSurvey_dataBound" EmptyDataText="No results found for selected date range." SkinID="GridViewRedBG">
        <Columns>
            <asp:TemplateField HeaderText="Source">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkSelect" CommandName="select" runat="server" Text='<%# showResult(eval("surveyQ1")) %>'></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="60" />
            <asp:BoundField HeaderText="Qty" DataField="items" />
        </Columns>
    </asp:GridView>
    <br /><br />
    <asp:GridView ID="gvOrders" runat="server" DataKeyNames="id" DataSourceID="sqlOrders" AutoGenerateColumns="false" EmptyDataText="No results found" GridLines="Vertical" SkinID="GridView">
        <Columns>
            <asp:TemplateField HeaderText="OrderID">
                <ItemTemplate>
                    <asp:HyperLink ID="lnkOrder" runat="server" Text='<%# Eval("newOrderID") & uCase(Eval("orderCountryCode")) %>' NavigateUrl='<%# "orderView.aspx?id=" & Eval("id") %>'></asp:HyperLink>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="20" />
            <asp:BoundField HeaderText="Date" DataField="orderDate" DataFormatString="{0:dd MMMM yyyy}" />
            <asp:BoundField ItemStyle-Width="20" />
            <asp:BoundField HeaderText="Shopper" DataField="billName" />
            <asp:BoundField ItemStyle-Width="20" />
            <asp:BoundField HeaderText="Total" DataField="orderTotal" DataFormatString="{0:n2}" />
            <asp:BoundField ItemStyle-Width="20" />
            <asp:BoundField HeaderText="Type" DataField="orderType" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="sqlSurvey1" runat="server" ConnectionString="<%$ ConnectionStrings:connectionString %>" SelectCommand="procShopOrderSurveyQ1ByDateSelect" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="DateTime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="DateTime" />
            <asp:ControlParameter ControlID="drpOrderType" PropertyName="selectedValue" Name="orderType" Type="String" Size="20" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOrders" runat="server" ConnectionString="<%$ ConnectionStrings:connectionString %>" SelectCommand="procShopOrderSurveyQ1ByDateMagazineSelect" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="DateTime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="DateTime" />
            <asp:ControlParameter ControlID="drpOrderType" PropertyName="selectedValue" Name="orderType" Type="String" Size="20" />
            <asp:ControlParameter ControlID="gvSurvey1" PropertyName="selectedValue" Name="magazineName" Type="String" Size="50" />
        </SelectParameters>
    </asp:SqlDataSource>
    
</asp:Content>

