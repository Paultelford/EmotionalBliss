<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="returnsOutstanding.aspx.vb" Inherits="maintenance_returnsOutstanding" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:FormView id="fvReturn" runat="server" DataSourceID="sqlReturn">
        <ItemTemplate>
            <table border="0">
                <tr>
                    <td>
                        <b>Returns ID:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblReturnsID" runat="server" Text='<%# Eval("returnsID") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Distributor:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblDistributor" runat="server" Text='<%# Eval("affName") & ", " & Eval("affCompany") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Return Created:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblDateCreated" runat="server" Text='<%# Eval("dateCreated","{0:dd MMM yyyy}") %>'></asp:Label>
                    </td>
                </tr>
            </table>
            <br /><br /><br />
            To print out the item list for this return, click <asp:LinkButton ID="lnkOpenPopup" runat="server" Text="HERE" OnClick="lnkOpenPopup_click"></asp:LinkButton>.<br />
            The return will not be processed until you click the 'Print' button at the top of the popup.<br /><br /><br />
            <asp:Panel ID="panComplete" runat="server" Visible="false">
                Click <asp:HyperLink ID="hypBack" runat="server" Text="here" NavigateUrl="~/maintenance/returnsLog.aspx"></asp:HyperLink> to go back to the Returns Log.
            </asp:Panel>
            
        </ItemTemplate>
    </asp:FormView>
    
    <asp:SqlDataSource ID="sqlReturn" runat="server" SelectCommand="procReturnsByIDDistSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="returnsID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <script language="javascript" type="text/javascript">
        function openPrintPop(id)
        {
            var win = window.open("returnsOutstandingPop.aspx?id="+id,"returnsOutstandingPop","toolbars=none,width=800,height=850");
        }
    </script>
</asp:Content>

