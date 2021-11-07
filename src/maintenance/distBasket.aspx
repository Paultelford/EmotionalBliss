<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="distBasket.aspx.vb" Inherits="maintenance_distBasket" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update" runat="server">
        <ContentTemplate>
            <asp:DropDownList ID="drpDistributors" runat="server" AutoPostBack="true" AppendDataBoundItems="true" DataSourceID="sqlDistributors" DataTextField="affCompany" DataValueField="affID" OnSelectedIndexChanged="drpDistributors_selectedIndexChanged">
                <asp:ListItem Text="Select Distributor" Value="0"></asp:ListItem>
            </asp:DropDownList><br /><br />
            
            <asp:Panel id="panOrderForm" runat="server" Visible="false">
                <asp:GridView ID="gvBasket" runat="Server" Width="100%" AutoGenerateColumns="false" EmptyDataText="Your basket is empty" SkinID="GridView" ShowFooter="true" DataKeyNames="ID" OnRowDeleting="gvBasket_rowDeleting">
                    <HeaderStyle Font-Bold="True" />
                    <RowStyle Font-Size="X-Small" />
                    <AlternatingRowStyle Font-Size="X-Small" />
                    <Columns>
                        <asp:BoundField HeaderText="Item" DataField="Name" HtmlEncode="false" />
                        <asp:BoundField HeaderText="Unit Price" DataField="Price" />
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
                                <asp:label id="lblRowTotal" runat="server" Text='<%# Eval("RowPriceInc") %>'></asp:label>
                            </ItemTemplate>
                            <FooterTemplate>
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
                            <asp:Button ID="btnCheckout" runat="server" Text="Checkout" onClick="btnCheckout_click" />
                        </td>
                    </tr>
                </table>
                <br /><br />
                <asp:DropDownList ID="drpProducts" runat="Server" DataSourceID="SqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged" OnDataBound="drpProducts_dataBound" OnDataBinding="drpProducts_dataBinding">
                    <asp:ListItem Text="Add product to basket..." Value="0"></asp:ListItem>
                </asp:DropDownList>
                <br /><br />
                <asp:Label ID="lblError" runat="Server" ForeColor="red" Font-Bold="true"></asp:Label>
                <asp:label ID="lblErrorMsg" runat="server" />
            </asp:Panel> 
        </ContentTemplate>
    </atlas:UpdatePanel>    
    
    <asp:SqlDataSource ID="sqlDistributors" runat="server" SelectCommand="procAffiliatesDistributorsSelect" SelectCommandType="storedProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProducts" runat="Server" SelectCommand="procAffiliateProductBuyingByAffIDSelectNOTNULL" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter PropertyName="selectedValue" ControlID="drpDistributors" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

