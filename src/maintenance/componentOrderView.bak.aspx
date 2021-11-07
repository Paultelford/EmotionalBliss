<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentOrderView.aspx.vb" Inherits="maintenance_componentOrderView" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    This page will show the complete order including the totals and shipping/vat.<br />
    You will also be able to edit these values and print out a hard copy.
    <br /><br />
    <asp:GridView ID="gvComponents" runat="server" DataSourceID="sqlComponents" DataKeyNames="compOrderItemID" AutoGenerateColumns="false" OnRowDataBound="gvComponents_rowDataBound" OnRowEditing="gvComponents_editing" OnRowUpdating="gvComponents_rowUpdating">
        <Columns>
            <asp:BoundField HeaderText="Component" ReadOnly="true" DataField="componentName" />
            <asp:BoundField HeaderText="Ordered" ReadOnly="true" DataField="qty" />
            <asp:TemplateField HeaderText="Outstanding">
                <ItemTemplate>
                    <asp:Label ID="lblOutstanding" runat="server" Text='<%# showOutstanding(Eval("qty"),Eval("qtyReceived")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Received" Visible="false">
                <EditItemTemplate>
                    <asp:TextBox ID="txtReceived" runat="server"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ShowEditButton="true" ShowCancelButton="false" EditText="Enter recieved items" />
        </Columns>
    </asp:GridView>
    <br /><br />
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    <asp:SqlDataSource ID="sqlComponents" runat="server" SelectCommand="procComponentOrderItemsByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="orderid" Type="int32" QueryStringField="id" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

