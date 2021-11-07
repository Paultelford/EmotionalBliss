<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="returnsComplete.aspx.vb" Inherits="affiliates_returnsComplete" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    This return is complete.<br />
    <a href='returnsLog.aspx'>Back to returns log</a>
    <br /><br />
    <asp:FormView id="fvReturn" runat="server" DataSourceID="sqlReturn" OnDataBound="fvReturn_dataBound">
        <ItemTemplate>
            <table border="0">
                <tr>
                    <td>
                        <b>Returns ID:</b>
                    </td>
                    <td width="80">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblReturnsID" runat="server" Text='<%# Eval("rma") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Details:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("name") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblAdd1" runat="server" Text='<%# Eval("add1") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblAdd2" runat="server" Text='<%# Eval("add2") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblAdd3" runat="server" Text='<%# Eval("add3") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblAdd4" runat="server" Text='<%# Eval("add4") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblPostcode" runat="server" Text='<%# Eval("postcode") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("countryName") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Bought From:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblBoughtFrom" runat="server" Text='<%# showBoughtFrom(Eval("purchasePlace")) %>'></asp:Label> <asp:HyperLink ID="lnkOrder" runat="server" Text='<%# Eval("userOrderID") %>' NavigateUrl='<%# "orderView.aspx?id=" & Eval("OrderID") %>'></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Returned Items:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:GridView ID="gvParts" runat="server" DataSourceID="sqlParts" AutoGenerateColumns="false" GridLines="none">
                            <Columns>
                                <asp:BoundField HeaderText="Product" DataField="name" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Qty" DataField="qty" />
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Action:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblAction" runat="server"></asp:Label>
                        <asp:Label ID="lblDistAction" runat="server" Text='<%# Eval("distAction") %>' Visible="false"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Comments/Details:</b>
                    </td>   
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblComments" runat="server" Text='<%# Eval("comments") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>User:</b>
                    </td>   
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblUsername" runat="server" Text='<%# Eval("username") %>'></asp:Label>
                    </td>
                </tr>
            </table>
            <br /><br /><br />
        </ItemTemplate>
    </asp:FormView>
    
    
    <asp:SqlDataSource ID="sqlReturn" runat="server" SelectCommand="procReturnsByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="returnsID" Type="int32" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="String" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlParts" runat="server" SelectCommand="procReturnsItemByRMACountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="rma" Type="int32" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

