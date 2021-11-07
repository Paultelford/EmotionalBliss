<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="returnsOutstanding.aspx.vb" Inherits="affiliates_returnsOutstanding" title="Untitled Page" %>
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
                        <asp:Label ID="lblRMA" runat="server" Text='<%# Eval("rma") %>'></asp:Label>
                        <asp:Label ID="lblReturnsID" runat="server" Visible="false" Text='<%# Eval("returnsID") %>'></asp:Label>
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
            </table>
            <br /><br /><br />
        </ItemTemplate>
    </asp:FormView>
    <asp:Panel ID="panDetails" runat="server">
        <b>Please select one of the following:</b>
        <br />
        &nbsp;&nbsp;
        <asp:RadioButtonList ID="radComplete" runat="server">
            <asp:ListItem Text="&nbsp;&nbsp;The product was fine and has been returned to the customer" Value="return" />
            <asp:ListItem Text="&nbsp;&nbsp;The Product was faulty and a refund/replacement has been offered to the customer" Value="fauly" />
            <asp:ListItem Text="&nbsp;&nbsp;Customer using the 100% Guarantee offer" Value="offer" />
        </asp:RadioButtonList><br />
        <asp:RequiredFieldValidator ID="req1" runat="Server" ControlToValidate="radComplete" ErrorMessage="You must select an action!" Display="Static"></asp:RequiredFieldValidator>
        <br />
        <b>Comments/Details:</b><br /><br />
        <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" CssClass="normaltextarea" Rows="10" width="700"></asp:TextBox><br /><br />
        <span style="display:none;visiblity:hidden">
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
        </span>
        <asp:TextBox ID="txtUsername" runat="Server" MaxLength="20" Visible="false"></asp:TextBox><br /><br />
        
        <asp:Button ID="btnSubmit" runat="server" Text="Submit" onClick="btnSubmit_click" />
    </asp:Panel>
    <asp:Panel ID="panComplete" runat="Server" Visible="false">
        The return has now been set to complete.
    </asp:Panel>
    <br /><br />
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    
    <asp:SqlDataSource ID="sqlReturn" runat="server" SelectCommand="procReturnsByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="returnsID" Type="int32" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="String" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlParts" runat="server" SelectCommand="procAffiliateProductBuyingByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Type="string" Name="countryCode" Size="5" />
        </SelectParameters> 
    </asp:SqlDataSource>
</asp:Content>

