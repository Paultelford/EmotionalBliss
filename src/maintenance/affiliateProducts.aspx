<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Trace="false" AutoEventWireup="false" CodeFile="affiliateProducts.aspx.vb" Inherits="maintenance_affiliateProducts" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress><br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table border="0">
                <tr>
                    <td width="500" valign="top">
                        <table border="0" width="100%">
                            <tr>
                                <td height="40">
                                    <b>Current Items Associated with Affiliate:</b>        
                                </td>
                                <td align="right">
                                    <asp:DropDownList ID="drpAff" runat="server" DataSourceID="SqlAff" DataTextField="affCompany" DataValueField="affID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpAff_selectedIndexChanged">
                                        <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                                    </asp:DropDownList><br />
                                </td>
                            </tr>
                        </table>                
                        <asp:Panel ID="pan1" runat="server" BorderWidth="1" BorderColor="gray" height="464" ScrollBars="vertical" Width="96%">
                                <asp:GridView ID="gvOnSale" runat="Server" DataSourceID="SqlOnSale" AutoGenerateColumns="false" GridLines="none" ShowHeader="false" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvOnSale_selectedIndexChanged" Width="100%" DataKeyNames="affProductBuyingID" OnDataBinding="gvOnSale_dataBinding">
                                    <HeaderStyle Font-Bold="true" />
                                    <SelectedRowStyle BackColor="red" />
                                    <Columns>
                                        <asp:BoundField DataField="affProductName" NullDisplayText="Not Set" />
                                        <asp:BoundField DataField="affUnitPrice" NullDisplayText="Not Set" />
                                        <asp:BoundField DataField="affTaxRate" />
                                        <asp:BoundField DataField="affProductType" />
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
                            <asp:FormView ID="fvDetails" runat="server" DataSourceID="SqlDetails" OnItemUpdated="fvDetails_itemUpdated" DataKeyNames="affProductBuyingID" OnDataBound="fvDetails_dataBound">
                                <EditItemTemplate>
                                    <asp:Label ID="lblID" runat="server" Text='<%# Eval("affProductBuyingID") %>' Visible="false"></asp:Label>
                                    <asp:Label ID="lblCompOrProdID" runat="server" Text='<%# Eval("affproductID") %>' Visible="false"></asp:Label>
                                    <br /><br />
                                    <table border="0">
                                        <tr>
                                            <td align="right">
                                                Type:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblType" runat="server" Text='<%# fStr(Eval("affProductType")) %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td align="right" id="tdTargetCountry" runat="server">
                                                Country:
                                            </td>
                                            <td id="tdTargetCountry2" runat="server">
                                                <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="SqlCountry" selectedValue='<%# Eval("affCountryCode") %>' DataTextField="countryName" DataValueField="countryCode" AppendDataBoundItems="true" Enabled="false">
                                                    <asp:ListItem Text="Not Set" Value=" "></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Sale Name:
                                            </td>
                                            <td>
                                                <asp:Label ID="lblSaleName" runat="server" Text='<%# Eval("affProductName") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td align="right" id="tdTargetCurrency" runat="server">
                                                Target Currency:
                                            </td>
                                            <td id="tdTargetCurrency2" runat="Server">
                                                <asp:DropDownList ID="drpCurrency" runat="server" DataSourceID="SqlCurrency" selectedValue='<%# Eval("affCurrencyCode") %>' DataTextField="currencyCode" DataValueField="currencyCode" AppendDataBoundItems="true" Enabled="false">
                                                    <asp:ListItem Text="Not Set" Value=" "></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="right">
                                                Unit Price:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtUnitPrice" runat="server" Width="50" Text='<%# Bind("affUnitPrice") %>'></asp:TextBox>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td align="right">
                                                Tax Rate:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtVatRate" runat="server" Width="50" Text='<%# Bind("affTaxRate") %>'></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr id="trDescription" runat="server" visible="false">
                                            <td valign="top" align="right">
                                                Description:
                                            </td>
                                            <td colspan="4">
                                                <asp:TextBox ID="txtDescription" runat="server" Enabled="false" Rows="4" Columns="34" TextMode="MultiLine"></asp:TextBox>
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
                            <asp:GridView ID="gvComp" runat="server" DataSourceID="SqlComp" AutoGenerateColumns="false" GridLines="none" ShowHeader="false" Width="100%" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvComp_selectedIndexChanged" DataKeyNames="componentID">
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
                            <asp:GridView ID="gvProd" runat="server" DataSourceID="SqlProd" AutoGenerateColumns="false" ShowHeader="false" GridLines="none" Width="100%" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvProd_seletcedIndexChanged" DataKeyNames="productID">
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
                            <asp:GridView ID="gvBProd" runat="server" DataSourceID="SqlBProd" AutoGenerateColumns="false" ShowHeader="false" GridLines="none" Width="100%" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvBProd_seletcedIndexChanged" DataKeyNames="warehouseProductID">
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
        </ContentTemplate>
    </atlas:UpdatePanel>
    <br /><br />

    <asp:SqlDataSource ID="SqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlOnSale" runat="server" SelectCommand="procAffilaiteProductBuyingByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpAff" PropertyName="selectedValue" Name="affID" Type="int32" />
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
    <asp:SqlDataSource ID="SqlDetails" runat="server" SelectCommand="procAffiliateProductBuyingByIDSelect" SelectCommandType="StoredProcedure" UpdateCommand="procAffiliateProductBuyingByIDUpdate" UpdateCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnUpdated="SqlDetails_updated">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvOnSale" PropertyName="selectedValue" Name="id" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="affProductBuyingID" Type="int32" />
            <asp:Parameter Name="affUnitPrice" Type="decimal" />
            <asp:Parameter Name="affTaxRate" Type="decimal" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlAff" runat="Server" SelectCommand="procAffiliatesSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>
