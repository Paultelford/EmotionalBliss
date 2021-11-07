<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="returnsPending.aspx.vb" Inherits="maintenance_returnsPending" title="Untitled Page" %>
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
            </table>
            <br /><br /><br />
        </ItemTemplate>
    </asp:FormView>
    <asp:Panel ID="panDetails" runat="server">
        <b>Comments/Details:</b><br /><br />
        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="10" Columns="80"></asp:TextBox><br /><br />
        <b>Parts Returned:</b><br /><br />
        <asp:GridView ID="gvParts" runat="server" DataSourceID="sqlParts" AutoGenerateColumns="false" GridLines="none">
            <Columns>
                <asp:BoundField HeaderText="Name" DataField="affProductName" HeaderStyle-VerticalAlign="top" />
                <asp:BoundField ItemStyle-Width="40" />
                <asp:TemplateField HeaderText="Qty Returned">
                    <ItemTemplate>
                        <asp:TextBox ID="txtQty" runat="server" Width="50"></asp:TextBox><asp:RangeValidator ID="ranQty" runat="server" ControlToValidate="txtQty" MinimumValue="1" MaximumValue="999" ErrorMessage=" Invalid" Display="Dynamic"></asp:RangeValidator>
                        <asp:Label ID="lblAffProductBuyingID" runat="server" Text='<%# Eval("affProductBuyingID") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblType" runat="server" Text='<%# Eval("affProductType") %>' Visible="false"></asp:Label>                        
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView><br /><br />
        <b>Username:</b><br /><br />
        <asp:TextBox ID="txtUsername" runat="Server" MaxLength="20"></asp:TextBox><br /><br />
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" onClick="btnSubmit_click" />
    </asp:Panel>
    <asp:Panel ID="panComplete" runat="Server" Visible="false">
        The return has now been set to complete.
    </asp:Panel>
    <br /><br />
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    
    <asp:SqlDataSource ID="sqlReturn" runat="server" SelectCommand="procReturnsByIDMaintenanceSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="returnsID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlParts" runat="server" SelectCommand="procAffiliateProductBuyingByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="distCountryCode" Type="string" Name="countryCode" Size="5" />
        </SelectParameters> 
    </asp:SqlDataSource>
</asp:Content>

