<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="returnsComplete.aspx.vb" Inherits="maintenance_returnsComplete" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    This return is complete.
    <br /><br />
    <asp:FormView id="fvReturn" runat="server" DataSourceID="sqlReturn">
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
                        <b>Distributor:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("affname") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblAdd1" runat="server" Text='<%# Eval("affadd1") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblAdd2" runat="server" Text='<%# Eval("affadd2") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblAdd3" runat="server" Text='<%# Eval("affadd3") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblAdd4" runat="server" Text='<%# Eval("affadd4") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblPostcode" runat="server" Text='<%# Eval("affpostcode") %>' OnDataBinding="fvReturn_blankIfEmpty"></asp:Label>
                        <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("countryName") %>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
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
            <asp:Parameter Name="countryCode" DefaultValue=" " Type="String" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlParts" runat="server" SelectCommand="procReturnsItemByRMACountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="rma" Type="int32" />
            <asp:Parameter Name="countryCode" DefaultValue=" " Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

