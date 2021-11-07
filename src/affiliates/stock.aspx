<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="stock.aspx.vb" Inherits="affiliates_stock" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <table border="0" width="100%">
        <tr>
            <td>
                <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
                    <ProgressTemplate>
                        Please Wait....<img src="/images/loading.gif" width="16" height="16" />
                    </ProgressTemplate>
                </atlas:UpdateProgress>        
            </td>
            <td align="right">
                <asp:HyperLink ID="lnkPurchaseManagement" runat="server" Text="Back" Font-Bold="true" NavigateUrl="~/affiliates/purchaseManagement.aspx"></asp:HyperLink>
            </td>
        </tr>
    </table>
    <br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvStock" runat="server" DataSourceID="SqlStock" AutoGenerateColumns="false" DataKeyNames="affProductBuyingID" Width="500" SkinID="GridView" OnSelectedIndexChanged="gvStock_selectedIndexChanged" OnDataBound="gvStock_dataBound">
                <Columns>
                    <asp:BoundField HeaderText="Product" DataField="affProductName" />
                    <asp:BoundField HeaderText="Product Code" />
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblWarehouseProductCode" runat="server" Visible="false" text='<%# Eval("warehouseProductCode") %>'></asp:Label>
                            <asp:Label ID="lblComponentCode" runat="server" Visible="false" text='<%# Eval("componentCode") %>'></asp:Label>
                            <asp:Label ID="lblType" runat="server" Visible="false" text='<%# Eval("affProductType") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Current Stock" DataField="stock" />
                    <asp:CommandField SelectText="Adjust" ShowSelectButton="true" ControlStyle-ForeColor="blue" />
                </Columns>
            </asp:GridView>
            <br /><br />
            <asp:Panel ID="pan1" runat="server" Width="500" Visible="false" BorderWidth="1">
                <table border="0" cellpadding="6" cellspacing="6" width="100%">
                    <tr>
                        <td>
                            <asp:DropDownList ID="drpAddRem" runat="server">
                                <asp:ListItem Text="Add" Value="add"></asp:ListItem>
                                <asp:ListItem Text="Remove" Value="rem"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="txtQty" runat="server" Text="0" MaxLength="5" Width="40"></asp:TextBox>
                            &nbsp;
                            units to/from stock
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Reason:
                        </td>
                        <td>
                            <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblError1" runat="server" ForeColor="red"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblError2" runat="server" ForeColor="red"></asp:Label>
                            <asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtQty" MinimumValue="1" MaximumValue="99999" ErrorMessage="Invalid Qty." Display="dynamic"></asp:RangeValidator>
                            <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtQty" ErrorMessage="Invalid Qty." Display="static"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>                

    
    <asp:SqlDataSource ID="SqlStock" runat="server" SelectCommand="procAffiliateEBDistributorStockByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

