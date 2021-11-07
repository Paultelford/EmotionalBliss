<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentOrderView.aspx.vb" Inherits="maintenance_componentOrderView" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    This page will show the complete order including the totals and shipping/vat.<br />
    You will also be able to edit these values and print out a hard copy.
    <br /><br />
    <asp:GridView ID="gvComponents" runat="server" AutoGenerateColumns="false" GridLines="none" DataKeyNames="compOrderItemID" DataSourceID="sqlComponents" OnRowDataBound="gvComponents_rowDataBound">
        <HeaderStyle Font-Bold="true" HorizontalAlign="left" />
        <RowStyle VerticalAlign="top" />
        <Columns>
            <asp:BoundField HeaderText="Component" DataField="componentName" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:BoundField HeaderText="Qty Ordered" DataField="qty" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:TemplateField HeaderText="Qty Outstanding">
                <ItemTemplate>
                    <asp:Label ID="lblOutstanding" runat="server" Text='<%# showOutstanding(Eval("qty"),Eval("qtyReceived")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="40" />
            <asp:TemplateField HeaderText="Qty Received OK">
                <ItemTemplate>
                    <asp:Panel ID="pan1" runat="server">
                        <asp:TextBox ID="txtReceived" runat="server" Width="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="req1" EnableClientScript="false" runat="server" ControlToValidate="txtReceived" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="com1" runat="server" EnableClientScript="false" ControlToValidate="txtReceived" ControlToCompare="txtReceivedCon" ErrorMessage="*Does not match" Display="dynamic"></asp:CompareValidator>
                        <br />
                        <asp:TextBox ID="txtReceivedCon" runat="server" Width="50"></asp:TextBox> (Confirm)
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="40" />
            <asp:TemplateField HeaderText="Qty Received Fail">
                <ItemTemplate>
                    <asp:Panel ID="pan2" runat="server">
                        <asp:TextBox ID="txtRejected" runat="server" Width="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="req2" runat="server" EnableClientScript="false" ControlToValidate="txtRejected" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="com2" runat="server" EnableClientScript="false" ControlToValidate="txtRejected" ControlToCompare="txtRejectedCon" ErrorMessage="*Does not match" Display="dynamic"></asp:CompareValidator>
                        <br />
                        <asp:TextBox ID="txtRejectedCon" runat="server" Width="50"></asp:TextBox> (Confirm)
                    </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="40" />
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblMan" runat="server" Text='<%#Eval("manid") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="40" />
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblComp" runat="server" Text='<%#Eval("compID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <asp:Button ID="btnSubmit" runat="server" Text="Update" onClick="btnSubmit_click" /><br />
    *Items received at the same time will be part of the same 'Component Received Batch'.
    <br /><br />
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    <asp:SqlDataSource ID="sqlComponents" runat="server" SelectCommand="procComponentOrderItemsByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="orderid" Type="int32" QueryStringField="id" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

