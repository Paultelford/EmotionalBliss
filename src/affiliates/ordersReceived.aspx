<%@ Page Language="VB" MasterPageFile="~/maffs.master" Trace="false" AutoEventWireup="true" CodeFile="ordersReceived.aspx.vb" Inherits="affiliates_ordersReceived" title="Untitled Page" %>
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
            <table border="0">
                <tr>
                    <td valign="top">
                        <table border="0" width="378">
                            <tr>
                                <td>
                                    Peartree Orders:        
                                    &nbsp;
                                    <asp:DropDownList ID="drpEBStatus" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="All" Value="%"></asp:ListItem>
                                        <asp:ListItem Text="Outstanding" Value="placed"></asp:ListItem>
                                        <asp:ListItem Text="Despatched" Value="despatched"></asp:ListItem>
                                        <asp:ListItem Text="Part Complete" Value="partcomp"></asp:ListItem>
                                        <asp:ListItem Text="Complete" Value="complete"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pan1" runat="server" BorderWidth="1">
                            <asp:GridView ID="gvEBOrders" runat="server" DataSourceID="SqlEBOrders" DataKeyNames="id" AutoGenerateColumns="false" Width="100%" EmptyDataText="None found." OnSelectedIndexChanged="gvEBOrders_selectedIndexChanged" Font-Size="smaller" AllowPaging="true" PageSize="15" OnRowEditing="gvEBOrders_rowEditing" OnRowDataBound="gvEBOrders_rowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="OrderID">
                                        <ItemTemplate>
                                            <asp:linkButton ID="btnSelectOrder" runat="Server"  Text='<%# Eval("newOrderID") & "Peartree" %>' CommandName="select" CommandArgument='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="10" />
                                    <asp:BoundField HeaderText="Date" DataField="orderDate" DataFormatString="{0:D}" HtmlEncode="false" />
                                    <asp:BoundField ItemStyle-Width="10" />
                                    <asp:BoundField HeaderText="Status" DataField="distributorStatus" />
                                    <asp:BoundField ItemStyle-Width="10" />
                                    <asp:TemplateField HeaderText="Order Total">
                                        <ItemTemplate>
                                            <asp:Label ID="lblGoods" runat="server" Text='<%# eval("orderTotal") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField ShowEditButton="true" EditText="Cancel" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                    <td width="20">&nbsp;</td>
                    <td valign="top">
                        <table border="0" width="378">
                            <tr>
                                <td>
                                    External Orders:        
                                    &nbsp;
                                    <asp:DropDownList ID="drpExStatus" runat="server" AutoPostBack="true">
                                        <asp:ListItem Text="All" Value="%"></asp:ListItem>
                                        <asp:ListItem Text="Outstanding" Value="placed"></asp:ListItem>
                                        <asp:ListItem Text="Part Complete" Value="partcomp"></asp:ListItem>
                                        <asp:ListItem Text="Complete" Value="complete"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="pan2" runat="server" BorderWidth="1">
                            <asp:GridView ID="gvEXOrders" runat="server" DataSourceID="SqlEXOrders" DataKeyNames="id" AutoGenerateColumns="false" Width="100%" EmptyDataText="None found." OnRowEditing="gvEXOrders_rowEditing" OnRowDataBound="gvEXOrders_rowDataBound" OnSelectedIndexChanged="gvEXOrders_selectedIndexChanged" Font-Size="smaller" AllowPaging="true" PageSize="15">
                                <Columns>
                                    <asp:TemplateField HeaderText="OrderID">
                                        <ItemTemplate>
                                            <asp:linkButton ID="btnSelectOrder" runat="Server" Text='<%# formatExtOrderID(Eval("OrderID")) %>' CommandName="select" CommandArgument='<%# Eval("ID") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:BoundField HeaderText="Date" DataField="orderDate" DataFormatString="{0:D}" HtmlEncode="false" />
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:BoundField HeaderText="Status" DataField="status" />
                                    <asp:BoundField ItemStyle-Width="20" />
                                    <asp:BoundField HeaderText="Supplier" DataField="supplierName" />
                                    <asp:CommandField ShowEditButton="true" EditText="Cancel" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" width="100%">
                        <asp:Panel ID="pan3" runat="server" Width="100%" BorderWidth="1" Visible="false">
                        <asp:HiddenField ID="hidOrderType" runat="server" />
                        <asp:HiddenField ID="hidOrderID" runat="server" />
                            <table border="0" cellpadding="6" cellspacing="6" width="100%">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblOrderDetails" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <b>Order Date:</b> <asp:Label ID="lblOrderDate" runat="server"></asp:Label>
                                    </td>
                                    <td>
                                        <b>Order Total:</b> <asp:Label ID="lblOrderTotal" runat="server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td width="100%" colspan="3">
                                        <asp:GridView ID="gvItems" runat="server" AutoGenerateColumns="false" Width="100%" BorderWidth="0" ShowFooter="true" DataKeyNames="itemID" OnDataBound="gvItems_dataBound">
                                            <Columns>
                                                <asp:BoundField HeaderText="Name" DataField="itemName" />
                                                <asp:BoundField HeaderText="Qty Ordered" DataField="itemQty" />
                                                <asp:TemplateField HeaderText="Outstanding">
                                                    <ItemTemplate>
                                                        <asp:label id="lblOutstanding" runat="server" Text='<%# subtract(Eval("itemQty"),Eval("itemReceived")) %>'></asp:label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Received">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtReceived" runat="server" Width="40"></asp:TextBox>
                                                        <asp:HiddenField ID="hidProductBuyingID" runat="server" Value='<%# Eval("itemID") %>' />
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <asp:Button ID="btnProcess" runat="server" Text="Process" OnClick="btnProcess_click" />
                                                    </FooterTemplate>
                                                </asp:TemplateField>
                                                
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
            <asp:Label ID="lblErrorDetail" Visible="false" runat="server"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="SqlEBOrders" runat="Server" SelectCommand="procShopOrdersByStatusSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpEBStatus" PropertyName="selectedValue" Name="status" Type="string" Size="10" />
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlEXOrders" runat="Server" SelectCommand="procDistributorExtOrdersByStatusSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpEXStatus" PropertyName="selectedValue" Name="status" Type="string" Size="10" />
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

