<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="orders.aspx.vb" Inherits="affiliates_orders" title="Untitled Page" %>
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
    <atlas:UpdatePanel runat="server" ID="update1">
        <ContentTemplate>
            <asp:Panel ID="pan1" runat="server" BorderWidth="1">
                <asp:Table ID="tblOrder" runat="server">
                    <asp:TableRow>
                        <asp:TableCell>
                            <b>Order ID:</b>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:TextBox ID="txtOrderID" runat="server" MaxLength="20"></asp:TextBox>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <b>Suppliers:</b>
                        </asp:TableCell>
                        <asp:TableCell>
                            <asp:DropDownList ID="drpSuppliers" runat="server" DataSourceID="sqlExistingSuppliers" DataTextField="supplierName" DataValueField="supplierID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpSuppliers_selectedIndexChanged">
                                    <asp:ListItem Text="Please choose...." Value=""></asp:ListItem>    
                                </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell VerticalAlign="top">
                            <b>Products:</b>
                        </asp:TableCell>
                        <asp:TableCell>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <b>Product</b>
                                    </td>
                                    <td width="30">&nbsp;</td>
                                    <td>
                                        <b>Qty</b>
                                    </td>
                                    <td width="30">&nbsp;</td>
                                    <td>
                                        <b>Unit Price</b>
                                    </td>
                                    <td width="30">&nbsp;</td>
                                    <td>
                                        <b>Tax Rate</b>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts1" runat="server">
                                    <td>
                                        <asp:DropDownList ID="drpProducts1" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtQty1" runat="server" Width="40"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtQty1" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtQty1" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="ran2" runat="server" ControlToValidate="drpProducts1" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice1" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice1" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqUnitPrice1" runat="server" ControlToValidate="txtUnitPrice1" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="ranUnitPrice1" runat="server" ControlToValidate="txtUnitPrice1" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate1" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate1" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator48" runat="server" ControlToValidate="txtTaxRate1" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator72" runat="server" ControlToValidate="txtTaxRate1" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts2" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts2" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtQty2" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtQty2" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtQty2" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator5" runat="server" ControlToValidate="drpProducts2" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice2" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice2" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="txtUnitPrice2" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator48" runat="server" ControlToValidate="txtUnitPrice2" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate2" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate2" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator49" runat="server" ControlToValidate="txtTaxRate2" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator73" runat="server" ControlToValidate="txtTaxRate2" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts3" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts3" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtQty3" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtQty3" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator2" runat="server" ControlToValidate="txtQty3" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator6" runat="server" ControlToValidate="drpProducts3" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice3" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice3" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="txtUnitPrice3" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator49" runat="server" ControlToValidate="txtUnitPrice3" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate3" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate3" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator50" runat="server" ControlToValidate="txtTaxRate3" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator74" runat="server" ControlToValidate="txtTaxRate3" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts4" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts4" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtQty4" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtQty4" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator3" runat="server" ControlToValidate="txtQty4" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator7" runat="server" ControlToValidate="drpProducts4" MinimumValue="1" MaximumValue="999999" ErrorMessage="* Select a product" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice4" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice4" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="txtUnitPrice4" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator50" runat="server" ControlToValidate="txtUnitPrice4" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate4" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate4" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator51" runat="server" ControlToValidate="txtTaxRate4" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator75" runat="server" ControlToValidate="txtTaxRate4" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts5" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts5" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty5" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtQty5" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator4" runat="server" ControlToValidate="txtQty5" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator8" runat="server" ControlToValidate="drpProducts5" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice5" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice5" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="txtUnitPrice5" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator51" runat="server" ControlToValidate="txtUnitPrice5" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate5" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate5" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator52" runat="server" ControlToValidate="txtTaxRate5" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator76" runat="server" ControlToValidate="txtTaxRate5" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts6" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts6" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty6" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtQty6" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator9" runat="server" ControlToValidate="txtQty6" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator10" runat="server" ControlToValidate="drpProducts6" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice6" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice6" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="txtUnitPrice6" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator52" runat="server" ControlToValidate="txtUnitPrice6" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate6" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate6" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator53" runat="server" ControlToValidate="txtTaxRate6" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator77" runat="server" ControlToValidate="txtTaxRate6" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts7" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts7" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty7" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator7a" runat="server" ControlToValidate="txtQty7" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator11" runat="server" ControlToValidate="txtQty7" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator12" runat="server" ControlToValidate="drpProducts7" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice7" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice7" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="txtUnitPrice7" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator53" runat="server" ControlToValidate="txtUnitPrice7" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate7" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate7" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator54" runat="server" ControlToValidate="txtTaxRate7" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator78" runat="server" ControlToValidate="txtTaxRate7" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts8" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts8" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty8" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtQty8" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator13" runat="server" ControlToValidate="txtQty8" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator14" runat="server" ControlToValidate="drpProducts8" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice8" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice8" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="txtUnitPrice8" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator54" runat="server" ControlToValidate="txtUnitPrice8" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate8" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate8" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator55" runat="server" ControlToValidate="txtTaxRate8" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator79" runat="server" ControlToValidate="txtTaxRate8" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts9" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts9" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty9" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtQty9" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator19" runat="server" ControlToValidate="txtQty9" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator16" runat="server" ControlToValidate="drpProducts9" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice9" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice9" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator31" runat="server" ControlToValidate="txtUnitPrice9" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator55" runat="server" ControlToValidate="txtUnitPrice9" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate9" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate9" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator56" runat="server" ControlToValidate="txtTaxRate9" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator80" runat="server" ControlToValidate="txtTaxRate9" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts10" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts10" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty10" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtQty10" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator17" runat="server" ControlToValidate="txtQty10" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator18" runat="server" ControlToValidate="drpProducts10" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice10" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice10" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator32" runat="server" ControlToValidate="txtUnitPrice10" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator56" runat="server" ControlToValidate="txtUnitPrice10" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate10" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate10" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator57" runat="server" ControlToValidate="txtTaxRate10" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator81" runat="server" ControlToValidate="txtTaxRate10" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts11" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts11" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty11" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtQty11" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator1a" runat="server" ControlToValidate="txtQty11" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator20" runat="server" ControlToValidate="drpProducts11" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice11" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice11" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator33" runat="server" ControlToValidate="txtUnitPrice11" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator57" runat="server" ControlToValidate="txtUnitPrice11" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate11" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate11" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator58" runat="server" ControlToValidate="txtTaxRate11" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator82" runat="server" ControlToValidate="txtTaxRate11" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts12" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts12" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty12" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtQty12" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator21" runat="server" ControlToValidate="txtQty12" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator22" runat="server" ControlToValidate="drpProducts12" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice12" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice12" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator34" runat="server" ControlToValidate="txtUnitPrice12" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator58" runat="server" ControlToValidate="txtUnitPrice12" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate12" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate12" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator59" runat="server" ControlToValidate="txtTaxRate12" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator83" runat="server" ControlToValidate="txtTaxRate12" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts13" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts13" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty13" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtQty13" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator15" runat="server" ControlToValidate="txtQty13" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator23" runat="server" ControlToValidate="drpProducts13" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice13" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice13" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator35" runat="server" ControlToValidate="txtUnitPrice13" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator59" runat="server" ControlToValidate="txtUnitPrice13" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate13" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate13" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator60" runat="server" ControlToValidate="txtTaxRate13" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator84" runat="server" ControlToValidate="txtTaxRate13" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts14" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts14" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty14" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtQty14" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator24" runat="server" ControlToValidate="txtQty14" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator25" runat="server" ControlToValidate="drpProducts14" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice14" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice14" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator36" runat="server" ControlToValidate="txtUnitPrice14" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator60" runat="server" ControlToValidate="txtUnitPrice14" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate14" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate14" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator61" runat="server" ControlToValidate="txtTaxRate14" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator85" runat="server" ControlToValidate="txtTaxRate14" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts15" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts15" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty15" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtQty15" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator26" runat="server" ControlToValidate="txtQty15" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator27" runat="server" ControlToValidate="drpProducts15" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice15" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice15" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator37" runat="server" ControlToValidate="txtUnitPrice15" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator61" runat="server" ControlToValidate="txtUnitPrice15" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate15" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate15" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator62" runat="server" ControlToValidate="txtTaxRate15" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator86" runat="server" ControlToValidate="txtTaxRate15" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts16" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts16" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty16" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txtQty16" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator28" runat="server" ControlToValidate="txtQty16" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator29" runat="server" ControlToValidate="drpProducts16" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice16" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice16" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator38" runat="server" ControlToValidate="txtUnitPrice16" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator62" runat="server" ControlToValidate="txtUnitPrice16" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate16" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate16" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator63" runat="server" ControlToValidate="txtTaxRate16" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator87" runat="server" ControlToValidate="txtTaxRate16" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts17" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts17" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty17" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txtQty17" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator30" runat="server" ControlToValidate="txtQty17" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator31" runat="server" ControlToValidate="drpProducts17" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice17" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice17" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator39" runat="server" ControlToValidate="txtUnitPrice17" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator63" runat="server" ControlToValidate="txtUnitPrice17" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate17" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate17" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator64" runat="server" ControlToValidate="txtTaxRate17" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator88" runat="server" ControlToValidate="txtTaxRate17" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts18" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts18" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty18" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtQty18" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator32" runat="server" ControlToValidate="txtQty18" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator33" runat="server" ControlToValidate="drpProducts18" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice18" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice18" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator40" runat="server" ControlToValidate="txtUnitPrice18" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator64" runat="server" ControlToValidate="txtUnitPrice18" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate18" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate18" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator65" runat="server" ControlToValidate="txtTaxRate18" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator89" runat="server" ControlToValidate="txtTaxRate18" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts19" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts19" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty19" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="txtQty19" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator34" runat="server" ControlToValidate="txtQty19" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator35" runat="server" ControlToValidate="drpProducts19" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice19" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice19" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator41" runat="server" ControlToValidate="txtUnitPrice19" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator65" runat="server" ControlToValidate="txtUnitPrice19" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate19" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate19" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator66" runat="server" ControlToValidate="txtTaxRate19" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator90" runat="server" ControlToValidate="txtTaxRate19" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts20" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts20" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty20" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="txtQty20" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator36" runat="server" ControlToValidate="txtQty20" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator37" runat="server" ControlToValidate="drpProducts20" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice20" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice20" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator42" runat="server" ControlToValidate="txtUnitPrice20" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator66" runat="server" ControlToValidate="txtUnitPrice20" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate20" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate20" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator67" runat="server" ControlToValidate="txtTaxRate20" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator91" runat="server" ControlToValidate="txtTaxRate20" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts21" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts21" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty21" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="txtQty21" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator38" runat="server" ControlToValidate="txtQty21" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator39" runat="server" ControlToValidate="drpProducts21" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice21" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice21" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator43" runat="server" ControlToValidate="txtUnitPrice21" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator67" runat="server" ControlToValidate="txtUnitPrice21" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate21" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate21" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator68" runat="server" ControlToValidate="txtTaxRate21" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator92" runat="server" ControlToValidate="txtTaxRate21" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts22" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts22" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty22" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="txtQty22" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator40" runat="server" ControlToValidate="txtQty22" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator41" runat="server" ControlToValidate="drpProducts22" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice22" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice22" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator44" runat="server" ControlToValidate="txtUnitPrice22" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator68" runat="server" ControlToValidate="txtUnitPrice22" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate22" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate22" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator69" runat="server" ControlToValidate="txtTaxRate22" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator93" runat="server" ControlToValidate="txtTaxRate22" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts23" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts23" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty23" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="txtQty23" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator42" runat="server" ControlToValidate="txtQty23" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator43" runat="server" ControlToValidate="drpProducts23" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice23" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice23" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator45" runat="server" ControlToValidate="txtUnitPrice23" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator69" runat="server" ControlToValidate="txtUnitPrice23" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate23" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate23" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator70" runat="server" ControlToValidate="txtTaxRate23" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator94" runat="server" ControlToValidate="txtTaxRate23" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts24" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts24" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty24" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="txtQty24" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator44" runat="server" ControlToValidate="txtQty24" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator45" runat="server" ControlToValidate="drpProducts24" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice24" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice24" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator46" runat="server" ControlToValidate="txtUnitPrice24" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator70" runat="server" ControlToValidate="txtUnitPrice24" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate24" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate24" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator71" runat="server" ControlToValidate="txtTaxRate24" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator95" runat="server" ControlToValidate="txtTaxRate24" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr id="tr_drpProducts25" runat="server" visible="false">
                                    <td>
                                        <asp:DropDownList ID="drpProducts25" runat="server" DataSourceID="sqlProducts" DataTextField="affProductName" DataValueField="affProductBuyingID" AutoPostBack="true" AppendDataBoundItems="true" Visible="true" OnSelectedIndexChanged="drpProducts_selectedIndexChanged">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>    
                                        <asp:TextBox ID="txtQty25" runat="server" Width="40" Visible="true"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="txtQty25" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator><asp:RangeValidator ID="RangeValidator46" runat="server" ControlToValidate="txtQty25" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" EnableClientScript="false"></asp:RangeValidator><asp:RangeValidator ID="RangeValidator47" runat="server" ControlToValidate="drpProducts25" MinimumValue="1" MaximumValue="999999" ErrorMessage="*" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblUnitPrice25" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtUnitPrice25" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator47" runat="server" ControlToValidate="txtUnitPrice25" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator71" runat="server" ControlToValidate="txtUnitPrice25" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblTaxRate25" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtTaxRate25" runat="server" MaxLength="6" Width="60"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator72" runat="server" ControlToValidate="txtTaxRate25" ErrorMessage="*" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="RangeValidator96" runat="server" ControlToValidate="txtTaxRate25" Type="Double" Display="Dynamic" ErrorMessage="*" MinimumValue="0" MaximumValue="999999"></asp:RangeValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:linkButton ID="lnkAddRow" runat="server" Text="Add item" ForeColor="blue" Font-Size="X-Small" OnClick="lnkAddRow_click"></asp:linkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Shipping
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        1
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtShipping" runat="server" Width="60" MaxLength="8"></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:TextBox ID="txtShippingVat" runat="server" Width="60" MaxLength="8"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>&nbsp;</asp:TableCell>
                        <asp:TableCell>
                            <asp:RequiredFieldValidator ID="reqShip" runat="server" ControlToValidate="txtShipping" ErrorMessage="Enter a shipping cost.<br>" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator>
                            <asp:RequiredFieldValidator ID="reqShipVat" runat="server" ControlToValidate="txtShippingVat" ErrorMessage="Enter a shipping vat rate.<br />" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RequiredFieldValidator>
                            <asp:RangeValidator ID="ranShip" runat="server" ControlToValidate="txtShipping" MinimumValue="0" Type="Double" MaximumValue="99999" ErrorMessage="Invalid shipping cost.<br>" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                            <asp:RangeValidator ID="ranShipVat" runat="server" ControlToValidate="txtShippingVat" MinimumValue="0" Type="Double" MaximumValue="100" ErrorMessage="Invalid shipping vat rate.<br>" Display="dynamic" ValidationGroup="main" EnableClientScript="false"></asp:RangeValidator>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                            <asp:Button ID="btnSubmit" runat="server" Text="Place order" OnClick="btnSubmit_click" ValidationGroup="main" />
                        </asp:TableCell>
                        <asp:TableCell>&nbsp;</asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </asp:Panel>
            <asp:Label ID="lblOrderMessage" runat="server"></asp:Label>
            <br /><br />
            <asp:Panel ID="pan2" runat="server">
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>
        
        
    
    <asp:SqlDataSource ID="sqlExistingSuppliers" runat="server" SelectCommand="procSuppliersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProducts" runat="server" SelectCommand="procAffiliateProductBuyingBySupplierIDProdSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procAffiliateProductBuyingByIDProdUpdate" UpdateCommandType="StoredProcedure" InsertCommand="procAffiliateProductBuyingByIDProdInsert" InsertCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="drpSuppliers" PropertyName="selectedValue" Name="supplierID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

