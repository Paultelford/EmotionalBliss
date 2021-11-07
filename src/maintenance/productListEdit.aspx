<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productListEdit.aspx.vb" Inherits="maintenance_productListEdit" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DetailsView ID="dvDetails" runat="server" GridLines="none" DataSourceID="SqlDetails" AutoGenerateRows="false">
        <HeaderStyle Font-Bold="true" />
        <Fields>
            <asp:BoundField HeaderText="Product:" DataField="productName" />
            <asp:BoundField HeaderText="Stock:" DataField="stock" />
            <asp:BoundField HeaderText="In Production:" DataField="production" />
        </Fields>
    </asp:DetailsView>
    <br />
    <asp:Panel ID="pan1" runat="server">
        <asp:RadioButtonList ID="rad1" runat="server">
            <asp:ListItem Selected="true" Text="Add" Value="add"></asp:ListItem>
            <asp:ListItem Text="Remove" Value="rem"></asp:ListItem>
        </asp:RadioButtonList>
        <br />
        <table>
            <tr>
                <td>
                    <b>Quantity:</b>
                </td>
                <td>
                    <asp:TextBox ID="lblQty" runat="server" Width="40"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="lblQty" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator><asp:RangeValidator ID="range1" runat="server" ControlToValidate="lblQty" MinimumValue="1" MaximumValue="999999" ErrorMessage="* Invalid quantity" Display="Dynamic"></asp:RangeValidator>
                </td>
            </tr>
            <tr>
                <td valign="top">
                    <b>Reason:</b>
                </td>
                <td>
                    <asp:TextBox TextMode="MultiLine" ID="txtReason" runat="server" Columns="50" Rows="5"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Apply" OnClick="btnSubmit_click" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <asp:Panel ID="pan2" runat="server" Visible="false">
        Details Added.<br />
        Click <asp:LinkButton ID="lnkbtnBack" runat="server" Text="here" OnClick="lnkbtnBack_click"></asp:LinkButton> to go back to Stock.<br />
        &nbsp;&nbsp;&nbsp;Or<br />
        Make another <asp:HyperLink ID="lnkRefresh" runat="server" Text="Adjustment"></asp:HyperLink>
        
    </asp:Panel>

    <asp:SqlDataSource ID="SqlDetails" runat="server" SelectCommand="procProductStockByProductIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter Name="productID" QueryStringField="id" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

