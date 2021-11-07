<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="onSale.aspx.vb" Inherits="maintenance_onSale" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table border="0">
        <tr>
            <td width="500" valign="top">
                <table border="0" width="100%">
                    <tr>
                        <td height="40">
                            <b>Current Items On Sale</b>        
                        </td>
                        <td align="right">
                            <asp:DropDownList ID="drpOnSaleCountry" runat="server" DataSourceID="SqlCountry" DataTextField="countryName" DataValueField="countryCode" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Text="All Countrys" Value="%"></asp:ListItem>
                            </asp:DropDownList><br />
                        </td>
                    </tr>
                </table>                
                <asp:Panel ID="pan1" runat="server" BorderWidth="1" BorderColor="gray" height="464" ScrollBars="vertical" Width="96%">
                        <asp:GridView ID="gvOnSale" runat="Server" DataSourceID="SqlOnSale" AutoGenerateColumns="false" EmptyDataText="No items currently on sale for selected country" GridLines="none" ShowHeader="false" SkinID="GridView" OnSelectedIndexChanged="gvOnSale_selectedIndexChanged" Width="100%" DataKeyNames="id">
                            <HeaderStyle Font-Bold="true" />
                            <SelectedRowStyle BackColor="red" />
                            <Columns>
                                <asp:BoundField DataField="saleName" NullDisplayText="Not Set" />
                                <asp:BoundField DataField="saleUnitPrice" NullDisplayText="Not Set" />
                                <asp:BoundField DataField="saleCountryCode" />
                                <asp:BoundField DataField="itemType" />
                                <asp:CommandField SelectText="Edit" ShowSelectButton="true" ControlStyle-ForeColor="blue" ItemStyle-Width="30" />
                            </Columns>
                        </asp:GridView>
                </asp:Panel>
                <br />
                <table border="0">
                    <tr>
                        <td height="40">
                            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="panDetails" runat="server" BorderColor="gray" BorderWidth="1" BackColor="#eeeeee" Visible="false" Height="200" Width="100%">
                    <asp:FormView ID="fvDetails" runat="server" DataSourceID="SqlDetails" OnItemUpdated="fvDetails_itemUpdated" DataKeyNames="id" OnDataBound="fvDetails_dateBound">
                        <EditItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Bind("id") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblCompOrProdID" runat="server" Text='<%# Eval("itemID") %>' Visible="false"></asp:Label>
                            <br />
                            <table border="0">
                                <tr>
                                    <td align="right">
                                        Type:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblType" runat="server" Text='<%# fStr(Eval("itemType")) %>'></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        Country:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="SqlCountry" selectedValue='<%# Bind("saleCountryCode") %>' DataTextField="countryName" DataValueField="countryCode" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Not Set" Value=" "></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Sale Name:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSaleName" runat="server" Text='<%# Bind("saleName") %>'></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        Currency:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpCurrency" runat="server" DataSourceID="SqlCurrency" selectedValue='<%# Bind("saleCurrencyCode") %>' DataTextField="currencyCode" DataValueField="currencyCode" AppendDataBoundItems="true">
                                            <asp:ListItem Text="Not Set" Value=" "></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Unit Price:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUnitPrice" runat="server" Width="50" Text='<%# Bind("saleUnitPrice") %>'></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="right">
                                        Tax Rate:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVatRate" runat="server" Width="50" Text='<%# Bind("saleTaxRate") %>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" align="right">
                                        Description:
                                    </td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtDescription" runat="server" Rows="4" Columns="34" Text='<%# Bind("saleDescription") %>' TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Button ID="lnkUpdate" runat="server" Text="Save Changes" CommandName="update"></asp:Button>
                                    </td>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                    </asp:FormView>
                </asp:Panel>
            </td>
            <td width="40">&nbsp;</td>
            <td width="300" valign="top">
                <table border="0" width="100%">
                    <tr>
                        <td height="40">
                            <b>Components</b>        
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pan2" runat="server" BorderWidth="1" BorderColor="gray" Height="200" ScrollBars="vertical" Width="94%">
                    <asp:GridView ID="gvComp" runat="server" DataSourceID="SqlComp" AutoGenerateColumns="false" GridLines="none" ShowHeader="false" Width="100%" SkinID="GridView" OnSelectedIndexChanged="gvComp_selectedIndexChanged" DataKeyNames="componentID">
                        <HeaderStyle Font-Bold="true" />
                        <Columns>
                            <asp:BoundField DataField="componentName" />
                            <asp:CommandField SelectText="Add" ShowSelectButton="True" ItemStyle-Width="30" ControlStyle-ForeColor="blue" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>       
                <br />   
                <table border="0" width="100%">
                    <tr>
                        <td height="40">
                            <b>Products</b>        
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pan3" runat="server" BorderWidth="1" BorderColor="gray" Height="200" ScrollBars="vertical" Width="94%">
                    <asp:GridView ID="gvProd" runat="server" DataSourceID="SqlProd" AutoGenerateColumns="false" ShowHeader="false" GridLines="none" Width="100%" SkinID="GridView" OnSelectedIndexChanged="gvProd_seletcedIndexChanged" DataKeyNames="productID">
                        <Columns>
                            <asp:BoundField DataField="productName" />
                            <asp:CommandField SelectText="Add" ShowSelectButton="True" ItemStyle-Width="30" ControlStyle-ForeColor="blue" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <table border="0" width="100%">
                    <tr>
                        <td height="40">
                            <b>Warehouse Products</b>        
                        </td>
                        <td align="right">
                            <asp:DropDownList ID="drpBProdCountry" runat="server" DataSourceID="SqlCountry" DataTextField="countryName" DataValueField="countryCode" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Text="All Countrys" Value="%"></asp:ListItem>
                            </asp:DropDownList><br />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pan4" runat="server" BorderWidth="1" BorderColor="gray" Height="200" ScrollBars="vertical" Width="94%">
                    <asp:GridView ID="gvBProd" runat="server" DataSourceID="SqlBProd" AutoGenerateColumns="false" ShowHeader="false" GridLines="none" Width="100%" SkinID="GridView" OnSelectedIndexChanged="gvBProd_seletcedIndexChanged" DataKeyNames="warehouseProductID">
                        <Columns>
                            <asp:BoundField DataField="warehouseProductName" />
                            <asp:BoundField DataField="countryName" />
                            <asp:CommandField SelectText="Add" ShowSelectButton="True" ItemStyle-Width="30" ControlStyle-ForeColor="blue" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
    </table>
    <br /><br />

    <asp:SqlDataSource ID="SqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlOnSale" runat="server" SelectCommand="procProductOnSaleByCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpOnSaleCountry" PropertyName="selectedValue" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlComp" runat="server" SelectCommand="procComponentsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProd" runat="server" SelectCommand="procProductsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlBProd" runat="server" SelectCommand="procWarehouseProductsByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpBProdCountry" PropertyName="selectedValue" Name="countryCode" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>    
    <asp:SqlDataSource ID="SqlDetails" runat="server" SelectCommand="procProductOnSaleByIDSelect" SelectCommandType="StoredProcedure" UpdateCommand="procProductOnSaleByIDUpdate" UpdateCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvOnSale" PropertyName="selectedValue" Name="saleID" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="id" Type="Int32" />
            <asp:Parameter Name="saleCountryCode" Type="string" Size="5" />
            <asp:Parameter Name="saleName" Type="string" Size="30" />
            <asp:Parameter Name="saleCurrencyCode" Type="string" Size="5" />
            <asp:Parameter Name="saleUnitPrice" Type="decimal" />
            <asp:Parameter Name="saleTaxRate" Type="decimal" />
            <asp:Parameter Name="saleDescription" Type="string" Size="4000" />
        </UpdateParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="SqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>

