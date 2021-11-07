<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="warehouseAssembly.aspx.vb" Inherits="maintenance_warehouseAssembly" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Select a boxed product and quantity to build:<br />
            <table>
                <tr>
                    <td>
                        Country:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="SqlCountry" DataTextField="countryName" DataValueField="countryCode" AppendDataBoundItems="true" AutoPostBack="true" CausesValidation="true" OnSelectedIndexChanged="drpCountry_selectedIndexChanged">
                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Product:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpBProduct" runat="server" DataSourceID="SqlBPRoducts" DataTextField="FullName" DataValueField="warehouseProductID" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="drpBProduct_selectedIndexChanged" CausesValidation="false">
                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Qty:
                    </td>
                    <td>
                        <asp:TextBox ID="txtQty" Width="40" runat="server" ValidationGroup="check"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server"  ControlToValidate="txtQty" ErrorMessage="* Required" Display="dynamic" ValidationGroup="check"></asp:RequiredFieldValidator><asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtQty" MinimumValue="1" MaximumValue="99999" ErrorMessage="* Invalid qty" Display="dynamic" ValidationGroup="check"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Qty Confirm:
                    </td>
                    <td>
                        <asp:TextBox ID="txtQtyConfirm" Width="40" runat="server" ValidationGroup="check"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  ControlToValidate="txtQty" ErrorMessage="* Required" Display="dynamic" ValidationGroup="check"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtQty" MinimumValue="1" MaximumValue="99999" ErrorMessage="* Invalid qty" Display="dynamic" ValidationGroup="check"></asp:RangeValidator><asp:CompareValidator ID="com1" runat="server" ControlToValidate="txtQty" ControlToCompare="txtQtyConfirm" ErrorMessage="* Quantities must match" Display="dynamic" EnableClientScript="false"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnCheckStock" runat="server" text="Check Stock" OnClick="btnCheckStock_click" Visible="false" ValidationGroup="check" />
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:Panel ID="pan1" runat="server" Visible="false">
                Required Item List:<br />
                <asp:GridView ID="gvList" runat="server" DataSourceID="SqlItemList" GridLines="None" AutoGenerateColumns="false" OnDataBound="gvList_dataBound" Width="500">
                    <RowStyle VerticalAlign="top" />
                    <Columns>
                        <asp:TemplateField HeaderText="Item">
                            <ItemTemplate>
                                <asp:Label ID="lblItem" runat="server" Text='<%# Eval("componentName") & Eval("productName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Stock">
                            <ItemTemplate>
                                <asp:Label ID="lblStock" runat="server" Text='<%# Eval("compStock") & Eval("prodStock") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Needed" DataField="contentQty" />
                    </Columns>
                </asp:GridView>
                <br />
                <br />
                <table>
                    <tr>
                        <td valign="top">
                            <asp:label ID="lblComments" runat="server" Text="Comments:"></asp:label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="4" Columns="40" MaxLength="500" Visible="false"></asp:TextBox><br />
                            <asp:Label ID="lblError" runat="server" Text="There are not enough components/products to complete the request." Visible="false"></asp:Label>        
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnContinue" runat="server" Text="Continue >" OnClick="btnContinue_click" Visible="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblComplete" runat="server"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>

    <asp:SqlDataSource ID="SqlBProducts" runat="server" SelectCommand="procWarehouseProductsByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpCountry" PropertyName="selectedValue" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlItemList" runat="server" SelectCommand="procWarehouseBoxContentByProductIDStockSelect" SelectCommandType="storedProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpBProduct" Name="bProdID" Type="int32" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

