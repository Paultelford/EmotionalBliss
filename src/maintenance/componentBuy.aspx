<%@ Page Language="VB" Debug="true" Trace="false" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentBuy.aspx.vb" Inherits="maintenance_componentBuy" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Fill out the order form and click 'Submit'
    <br /><br />
    <asp:Table ID="tblOrder" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                Purchase Order:
            </asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:Label ID="lblOrderID" runat="server"></asp:Label>                
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Order Date:
            </asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:Label ID="lblDate" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                Manufacturer:
            </asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:DropDownList ID="drpMan" runat="server" DataSourceID="sqlExistingManufacturers" DataTextField="manufacturerName" DataValueField="manufacturerID" AutoPostBack="true" OnSelectedIndexChanged="drpMan_selectedIndexChanged" EnableViewState="true" AppendDataBoundItems="true">
                    <asp:ListItem Text="Select Manufacturer..." Value="0"></asp:ListItem>
                </asp:DropDownList>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableFooterRow>
            <asp:TableCell>
                Billing Address:
            </asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:DropDownList ID="drpBilling" runat="server" DataTextField="displayAdd" DataValueField="billingAddID"></asp:DropDownList> <asp:Button ID="btnAddBilling" runat="server" Text="Add" OnClick="btnAddBilling_click" />
            </asp:TableCell>
        </asp:TableFooterRow>
        <asp:TableFooterRow>
            <asp:TableCell>
                Shipping Address:
            </asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:DropDownList ID="drpShipping" runat="server" DataTextField="displayAdd" DataValueField="shippingAddID"></asp:DropDownList> <asp:Button ID="btnAddShipping" runat="server" Text="Add" OnClick="btnAddShipping_click" />
            </asp:TableCell>
        </asp:TableFooterRow>
        <asp:TableRow>
            <asp:TableCell>
                Delivery Charge:
            </asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:TextBox ID="txtDelivery" runat="server" Width="40"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtDelivery" runat="server" ControlToValidate="txtDelivery" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>
                VAT Rate(%):
            </asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:TextBox id="txtVAT" runat="server" Width="40"></asp:TextBox>
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell VerticalAlign="top">
                Special Instructions:
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="txtInstructions" runat="server" MaxLength="1000" TextMode="MultiLine" Rows="3" Columns="40"></asp:TextBox>
                <asp:Label ID="lblCurrency" runat="server" Visible="false"></asp:Label>
                <asp:Label ID="lblCurrencyID" Visible="false" runat="server"></asp:Label>
            </asp:TableCell>            
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="3">
                &nbsp;
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell ColumnSpan="3">
                <asp:Table ID="tblItems" runat="server" >
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell>
                            &nbsp;
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            Component
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            Qty
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell>
                            Unit Cost
                        </asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            Component 1:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem1" runat="server" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem1_indexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty1" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost1" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow2" Visible="false">
                        <asp:TableCell>
                            Component 2:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem2" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem2_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty2" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost2" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow3" Visible="false">
                        <asp:TableCell>
                            Component 3:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem3" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem3_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty3" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost3" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow4" Visible="false">
                        <asp:TableCell>
                            Component 4:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem4" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem4_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty4" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost4" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow5" Visible="false">
                        <asp:TableCell>
                            Component 5:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem5" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem5_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty5" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost5" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow6" Visible="false">
                        <asp:TableCell>
                            Component 6:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem6" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem6_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty6" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost6" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow7" Visible="false">
                        <asp:TableCell>
                            Component 7:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem7" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem7_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty7" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost7" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow8" Visible="false">
                        <asp:TableCell>
                            Component 8:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem8" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem8_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty8" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost8" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow9" Visible="false">
                        <asp:TableCell>
                            Component 9:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem9" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem9_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty9" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost9" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow ID="tRow10" Visible="false">
                        <asp:TableCell>
                            Component 10:
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpItem10" runat="server" AutoPostBack="true" DataSourceID="sqlComponents" EnableViewState="false" DataTextField="componentName" DataValueField="componentID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpItem10_indexChanged">
                                <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtQty10" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtCost10" runat="server" Width="40"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
        
    <asp:Button ID="btnCompleteOrder" runat="server" Text="Place Order" OnClick="btnCompleteOrder_click" />&nbsp;&nbsp;
    <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
    <asp:SqlDataSource ID="sqlExistingManufacturers" runat="server" SelectCommand="procManufacturersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlComponents" runat="server" SelectCommand="procComponentByManIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter Name="manid" ControlID="drpMan" PropertyName="selectedValue" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

