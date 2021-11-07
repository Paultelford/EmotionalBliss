<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="newsletter.aspx.vb" Inherits="affiliates_newsletter" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <eb:DateControl id="date1" runat="server"></eb:DateControl>
    
    <asp:GridView ID="gvNewsletter" runat="server" DataSourceID="sqlNewsletter" GridLines="None" AutoGenerateColumns="false" DataKeyNames="id">
        <Columns>
            <asp:BoundField HeaderText="Date" DataField="signup" DataFormatString="{0:dd MMM yyyy}" ReadOnly="true" />
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:BoundField HeaderText="Name" DataField="name" ReadOnly="true" />
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:BoundField HeaderText="Email" DataField="email" ReadOnly="true" />
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:BoundField HeaderText="Country" DataField="countryName" ReadOnly="true" />
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:CheckBoxField HeaderText="Active" DataField="active" ItemStyle-HorizontalAlign="Center" />
            <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
            <asp:CommandField EditText="Edit" ShowEditButton="true" />
        </Columns>
    </asp:GridView>
    
    
    <asp:SqlDataSource ID="sqlNewsletter" runat="server" SelectCommand="procNewsletterSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procNewsletterByIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="DateTime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="DateTime" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter name="id" Type="Int32" />
            <asp:Parameter Name="active" Type="Boolean" />
        </UpdateParameters>
        <DeleteParameters>
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>

