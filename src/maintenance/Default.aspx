<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="maintenance_Default" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="../images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    Welcome to the EB Maintenance Section.<br /><br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
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
        </ContentTemplate>
    </atlas:UpdatePanel>
    <br /><br /><br />
    <asp:Label ID="lblScanErrors" runat="server" Text="Recent Scan Errors" Font-Bold="true"></asp:Label><br />
    <asp:GridView ID="gvScanErrors" runat="server" Visible="true" DataSourceID="sqlScanErrors" OnDataBound="gvScanErrors_dataBound" AutoGenerateColumns="false" DataKeyNames="scanErrorID" Width="80%">
        <Columns>
            <asp:BoundField HeaderText="OrderID" DataField="userOrderID" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:TemplateField HeaderText="Scan Date">
                <ItemTemplate>
                    <asp:label id="lblScanDate" runat="server" Text='<%# showDateTime(Eval("scanDate")) %>'></asp:label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="40" />
            <asp:BoundField HeaderText="Scan Function" DataField="scanFunction" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:BoundField HeaderText="Error Message" DataField="scanMsg" ItemStyle-Width="60%" />
            <asp:CommandField DeleteText="Dismiss" ShowDeleteButton="true" ShowCancelButton="false" />
        </Columns>
    </asp:GridView>
    
    
    <asp:SqlDataSource ID="sqlErrors" runat="server" SelectCommand="procErrorsByActiveSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" DeleteCommand="procErrosByIDUpdate" DeleteCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="errorID" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlScanErrors" runat="server" SelectCommand="procScanErrorsByActiveSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" DeleteCommand="procScanErrosByIDDelete" DeleteCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="scanErrorID" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content> 

