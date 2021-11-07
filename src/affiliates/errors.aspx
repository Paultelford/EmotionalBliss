<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="errors.aspx.vb" Inherits="affiliates_errors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
        <tr>
            <td>
                <asp:Label ID="lblErrors" runat="server" Text="Recent Errors" Font-Bold="true"></asp:Label>
            </td>
            <td align="right">
                <asp:LinkButton ID="btnDeleteAll" runat="Server" Text="Clear All Errors" OnClick="lnkClearAll_click"></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:GridView ID="gvErrors" runat="server" Visible="true" DataSourceID="sqlErrors" AutoGenerateColumns="false" OnDataBound="gvErrors_dataBound" DataKeyNames="errorID" Width="100%" EmptyDataText="No errors found">
                    <Columns>
                        <asp:BoundField HeaderText="Page Name" DataField="errorPage" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField HeaderText="Error Occured">
                            <ItemTemplate>
                                <asp:label id="lblErrorDate" runat="server" Text='<%# showDateTime(Eval("errorDate")) %>'></asp:label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Error Message" DataField="errorMsg" ItemStyle-Width="60%" />
                        <asp:CommandField DeleteText="Dismiss" ShowDeleteButton="true" ShowCancelButton="false" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>     
    
    <asp:SqlDataSource ID="sqlErrors" runat="server" SelectCommand="procErrorsByActiveSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" DeleteCommand="procErrosByIDUpdate" DeleteCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="errorID" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>

