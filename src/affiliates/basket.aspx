<%@ Page Language="VB" Trace="false" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="basket.aspx.vb" Inherits="affiliates_basket" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    
            <asp:GridView ID="gvBasket" runat="Server" Width="100%" AutoGenerateColumns="false" EmptyDataText="Your basket is empty" SkinID="GridView" ShowFooter="true" DataKeyNames="ID" OnRowDeleting="gvBasket_rowDeleting">
                <HeaderStyle Font-Bold="True" />
                <RowStyle Font-Size="X-Small" />
                <AlternatingRowStyle Font-Size="X-Small" />
                <Columns>
                    <asp:BoundField HeaderText="Item" DataField="Name" HtmlEncode="false" />
                    <asp:TemplateField HeaderText="Unit Price">
                        <ItemTemplate>
                            <asp:Label ID="lblCurrencySign1" runat="server" Text='<%# getCurrencyCode() %>'></asp:Label>
                            <asp:Label ID="lblUnitPrice" runat="server" Text='<%# Eval("Price") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Tax Rate" DataField="Vat" />
                    <asp:TemplateField HeaderText="Total" Visible="false">
                        <ItemTemplate>
                            <asp:label id="lblTotal" runat="server" Text='<%# calcTotal(Eval("Price"),Eval("Vat")) %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Qty">
                        <ItemTemplate>
                            <asp:TextBox ID="txtQty" runat="Server" Text='<%# Bind("Qty") %>' Width="40" Height="18" Font-Size="XX-Small" AutoPostBack="true" OnTextChanged="txtQty_updateRow"></asp:TextBox>
                            <asp:Label ID="lblAffProductBuyingID" runat="server" Text='<%# Eval("ID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Row Total (Inc Tax)">
                        <ItemTemplate>
                            <asp:Label ID="lblCurrencySign2" runat="server" Text='<%# getCurrencyCode() %>'></asp:Label>
                            <asp:label id="lblRowTotal" runat="server" Text='<%# Eval("RowPriceInc") %>'></asp:label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lblCurrencySign3" runat="server" Text='<%# getCurrencyCode() %>'></asp:Label>
                            <asp:Label ID="lblOrderTotal" runat="Server" Font-Bold="true"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove">
                        <ItemTemplate>
                            <asp:Button ID="btnDeleteItem" runat="server" Text=" X " CommandName="Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <table border="0" width="100%">
                <tr>
                    <td>
                        
                    </td>
                    <td align="right">
                        <asp:Button ID="btnUpdate" runat="Server" Text="Update Basket" OnClick="btnUpdate_click" />
                        &nbsp;
                        <input type="button" runat="server" id="btnCheckout" value="Checkout" onclick="document.location='payment.aspx';" />
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:DropDownList ID="drpProducts" runat="Server" DataSourceID="SqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged" OnDataBound="drpProducts_dataBound">
                <asp:ListItem Text="Add product to basket..." Value="0"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <asp:Label ID="lblError" runat="Server" ForeColor="red" Font-Bold="true"></asp:Label>
            <asp:label ID="lblErrorMsg" runat="server" />
     
    
    <asp:SqlDataSource ID="SqlProducts" runat="Server" SelectCommand="procAffiliateProductBuyingByAffIDSelectNOTNULL" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="SqlProducts_selecting">
        <SelectParameters>
            <asp:Parameter Name="affID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

