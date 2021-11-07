<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="quarantineProcess.aspx.vb" Inherits="maintenance_quarantineProcess" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Enter the number of components that have passed the Quality Control. (These will be put into stock)
    <br /><br />
    <asp:DetailsView ID="dvComponent" runat="server" DataSourceID="sqlComponent" AutoGenerateRows="false" GridLines="none">
        <HeaderStyle Font-Bold="true" />
        <Fields>
            <asp:BoundField HeaderText="Component" DataField="componentName" />
            <asp:BoundField HeaderText="Qty" DataField="qtyAdded" />
            <asp:BoundField HeaderText="BatchID" DataField="compBatchID" />
            <asp:BoundField HeaderText="OrderID" DataField="compOrderID" />
            <asp:TemplateField HeaderText="Qty Passed">
                <ItemTemplate>
                    <asp:TextBox ID="txtPassed" runat="server" MaxLength="6" Width="40"></asp:TextBox>
                </ItemTemplate>                
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Qty Failed">
                <ItemTemplate>
                    <asp:TextBox ID="txtFailed" runat="server" MaxLength="6" Width="40"></asp:TextBox>
                </ItemTemplate>                
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnSubmit" runat="server" text="Submit" onClick="btnSubmit_click" />
                </ItemTemplate>                
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Label ID="lblComponentID" runat="server" Text='<%# Eval("componentID") %>' Visible="false"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    
    <asp:SqlDataSource ID="sqlComponent" runat="server" SelectCommand="procComponentBatchByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="batchID" Type="int32" QueryStringField="id" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

