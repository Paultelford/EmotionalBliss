<%@ Page Language="VB" MasterPageFile="~/maffs.master" Trace="false" ValidateRequest="false" AutoEventWireup="false" CodeFile="shopProducts.aspx.vb" Inherits="affiliates_shopProducts" Title="Untitled Page" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%--<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>--%>
<%@ Register Assembly="FredCK.FCKeditorV2" Namespace="FredCK.FCKeditorV2" TagPrefix="FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <br />
    <asp:Label ID="lblCOuntryC" runat="server" Visible="false"></asp:Label>
    <table border="0">
        <tr>
            <td width="500" valign="top" align="left">
                <table border="0" width="100%">
                    <tr>
                        <td height="40" align="left">
                            <b>Items Available to Customers:</b>
                        </td>
                        <td align="right">Show:
                                    <asp:DropDownList ID="drpDept" runat="server" DataSourceID="SqlDept" DataTextField="deptName" DataValueField="deptID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpAff_selectedIndexChanged" Visible="true">
                                        <asp:ListItem Text="All" Value="%"></asp:ListItem>
                                    </asp:DropDownList><br />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pan1" runat="server" BorderWidth="1" BorderColor="gray" Height="464" ScrollBars="vertical" Width="100%">
                    <asp:GridView ID="gvOnSale" runat="Server" DataSourceID="SqlOnSale" AutoGenerateColumns="false" GridLines="none" ShowHeader="false" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvOnSale_selectedIndexChanged" Width="96%" DataKeyNames="id" OnDataBinding="gvOnSale_dataBinding" OnRowDeleted="gvOnSale_rowDeleted">
                        <HeaderStyle Font-Bold="true" />
                        <SelectedRowStyle BackColor="red" />
                        <Columns>
                            <asp:BoundField DataField="saleName" NullDisplayText="Not Set" />
                            <asp:BoundField DataField="saleUnitPrice" NullDisplayText="Not Set" />
                            <asp:BoundField DataField="saleTaxRate" />
                            <asp:BoundField DataField="itemType" />
                            <asp:CommandField SelectText="Edit" ShowSelectButton="true" ControlStyle-ForeColor="blue" ItemStyle-Width="30" />
                            <asp:CommandField DeleteText="X" ShowDeleteButton="true" ControlStyle-ForeColor="red" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <table border="0">
                    <tr>
                        <td height="40">
                            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
                            <asp:HiddenField ID="lblErrorTxt" runat="server" Value="" />
                        </td>
                    </tr>
                </table>
            </td>
            <td width="40">&nbsp;</td>
            <td width="300" valign="top" align="left">
                <table border="0" width="100%">
                    <tr>
                        <td height="40">
                            <b>Components</b>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pan2" runat="server" BorderWidth="1" BorderColor="gray" Height="200" ScrollBars="vertical" Width="100%">
                    <asp:GridView ID="gvComp" runat="server" DataSourceID="SqlComp" AutoGenerateColumns="false" GridLines="none" ShowHeader="false" Width="94%" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvComp_selectedIndexChanged" DataKeyNames="ID">
                        <HeaderStyle Font-Bold="true" />
                        <Columns>
                            <asp:BoundField DataField="Name" />
                            <asp:CommandField SelectText="Add" ShowSelectButton="True" ItemStyle-Width="30" ControlStyle-ForeColor="blue" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAffProductBuyingID" runat="server" Text='<%# Eval("affProductBuyingID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <br />
                <table border="0" width="100%" runat="server" visible="false">
                    <tr>
                        <td height="40">
                            <b>Products</b>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pan3" runat="server" BorderWidth="1" BorderColor="gray" Visible="false" Height="200" ScrollBars="vertical" Width="100%" HorizontalAlign="Left">
                    <asp:GridView ID="gvProd" runat="server" DataSourceID="SqlProd" AutoGenerateColumns="false" ShowHeader="false" GridLines="none" Width="94%" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvProd_seletcedIndexChanged" DataKeyNames="">
                        <Columns>
                            <asp:BoundField DataField="Name" />
                            <asp:CommandField SelectText="Add" ShowSelectButton="True" ItemStyle-Width="30" ControlStyle-ForeColor="blue" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAffProductBuyingID" runat="server" Text='<%# Eval("affProductBuyingID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <table id="Table1" border="0" width="100%" runat="server" visible="true">
                    <tr>
                        <td height="40">
                            <b>External Products</b>
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="Panel1" runat="server" BorderWidth="1" BorderColor="gray" Visible="true" Height="200" ScrollBars="vertical" Width="100%">
                    <asp:GridView ID="gvExt" runat="server" DataSourceID="SqlExt" AutoGenerateColumns="false" ShowHeader="false" GridLines="none" Width="94%" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvExt_seletcedIndexChanged" DataKeyNames="affProductBuyingID">
                        <Columns>
                            <asp:BoundField DataField="Name" />
                            <asp:CommandField SelectText="Add" ShowSelectButton="True" ItemStyle-Width="30" ControlStyle-ForeColor="blue" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAffProductBuyingID" runat="server" Text='<%# Eval("affProductBuyingID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <table border="0" width="100%">
                    <tr>
                        <td height="40">
                            <b>Warehouse Products</b>
                        </td>
                        <td align="right" visible="false" id="tdWarehouseCountryDrop" runat="server">
                            <asp:DropDownList ID="drpBProdCountry" runat="server" DataSourceID="SqlCountry" DataTextField="countryName" DataValueField="countryCode" AppendDataBoundItems="true" AutoPostBack="true">
                                <asp:ListItem Text="All Countrys" Value="%"></asp:ListItem>
                            </asp:DropDownList><br />
                        </td>
                    </tr>
                </table>
                <asp:Panel ID="pan4" runat="server" BorderWidth="1" BorderColor="gray" Height="200" ScrollBars="vertical" Width="100%">
                    <asp:GridView ID="gvBProd" runat="server" DataSourceID="SqlBProd" AutoGenerateColumns="false" ShowHeader="false" GridLines="none" Width="94%" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvBProd_seletcedIndexChanged" DataKeyNames="ID">
                        <Columns>
                            <asp:BoundField DataField="Name" />
                            <asp:CommandField SelectText="Add" ShowSelectButton="True" ItemStyle-Width="30" ControlStyle-ForeColor="blue" />
                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblAffProductBuyingID" runat="server" Text='<%# Eval("affProductBuyingID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="3" runat="server" align="left">
                <asp:Panel ID="panDetails" runat="server" BorderColor="gray" BorderWidth="1" BackColor="#eeeeee" Visible="false" Height="964" Width="100%">
                    <asp:FormView ID="fvDetails" runat="server" DataSourceID="SqlDetails" OnItemUpdated="fvDetails_itemUpdated" OnItemUpdating="fvDetails_itemUpdating" DataKeyNames="id" OnDataBound="fvDetails_dataBound">
                        <EditItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>' Visible="false"></asp:Label>
                            <asp:Label ID="lblCompOrProdID" runat="server" Text='<%# Eval("itemID") %>' Visible="false"></asp:Label>
                            <br />
                            <br />
                            <table border="0">
                                <tr>
                                    <td align="right">Type:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblType" runat="server" Text='<%# fStr(Eval("itemType")) %>'></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="left" id="tdTargetCountry" runat="server">Country:
                                    </td>
                                    <td id="tdTargetCountry2" runat="server">
                                        <asp:Label ID="lblCountryCode" runat="server" Text='<%# Eval("countryName") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">PID:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPID" runat="server" Text='<%# Eval("ID") %>'></asp:Label>
                                    </td>
                                    <td colspan="3">&nbsp;</td>
                                </tr>
                                <tr>
                                    <td align="right">Sale Name:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblSaleName" runat="server" Text='<%# Eval("saleName") %>'></asp:Label>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="left" id="tdTargetCurrency" runat="server">Currency:
                                    </td>
                                    <td id="tdTargetCurrency2" runat="Server">
                                        <asp:Label ID="lblCurrencyCode" runat="Server" Text='<%# Eval("saleCurrencyCode") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">Unit Price:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtUnitPrice" runat="server" Width="50" Text='<%# Bind("saleUnitPrice") %>'></asp:TextBox>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="left">Tax Rate:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVatRate" runat="server" Width="50" Text='<%# Bind("saleTaxRate") %>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">Discount(%):
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDiscount" runat="server" Width="50" Text='<%# Bind("saleDiscount") %>'></asp:TextBox>
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                    <td>Unit Price After Discount:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblUnitPriceAfterDiscount" runat="server" Text='<%# Eval("unitPriceAfterDiscount") %>'></asp:Label>&nbsp;
                                                (<asp:Label ID="lbbUnitPriceAfterDiscountIncVat" runat="server" Text='<%# Eval("unitPriceAfterDiscountIncVat") %>'></asp:Label>
                                        Inc Vat)
                                    </td>
                                </tr>
                                <tr>
                                    <td>Prod Code:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtProdCode" runat="server" MaxLength="20" Text='<%# Bind("saleProdCode") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqTxtProdCode" runat="server" ControlToValidate="txtProdCode" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="left">Dept:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpDept" runat="Server" SelectedValue='<%# Bind("saleDeptID") %>' DataSourceID="SqlDept" AppendDataBoundItems="true" DataTextField="deptName" DataValueField="deptID">
                                            <asp:ListItem Value="0">Select..</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">Description:
                                    </td>
                                    <td colspan="4">
                                        <fckeditorv2:FCKeditor ID="txtDescription" runat="server" BasePath="~/EBEditor/" ToolbarStartExpanded="false" Width="680" Height="400" Value='<%# Bind("saleDescription") %>'></fckeditorv2:FCKeditor>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Product Image:
                                    </td>
                                    <td valign="top">
                                        <asp:FileUpload ID="f1" runat="server" FileName='<%# Bind("saleImageName") %>' />
                                        <asp:HiddenField ID="hidSaleImageName" runat="server" Value='<%# Eval("saleImageName") %>' />
                                        <br />
                                        <asp:HyperLink ID="lnkSaleImageName" runat="server" Text='<%# Eval("saleImageName") %>' NavigateUrl='<%# "/images/products/" & Eval("saleImageName") %>' Target="_blank"></asp:HyperLink>
                                        &nbsp;
                                                <asp:LinkButton ID="lnkDeleteSaleImageName" runat="server" ForeColor="Red" Text="X" OnClick="lnkDelete_click" CommandArgument="saleImageName"></asp:LinkButton>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="left">Deptartment Image:
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="f1Dept" runat="server" FileName='<%# Bind("saleDeptImageName") %>' />
                                        <asp:HiddenField ID="hidSaleDeptImageName" runat="server" Value='<%# Eval("saleDeptImageName") %>' />
                                        <br />
                                        <asp:HyperLink ID="lnkSaleDeptImageName" runat="server" Text='<%# Eval("saleDeptImageName") %>' NavigateUrl='<%# "/images/products/" & Eval("saleDeptImageName") %>' Target="_blank"></asp:HyperLink>
                                        &nbsp;
                                                <asp:LinkButton ID="lnkDeleteSaleDeptImageName" runat="server" ForeColor="Red" Text="X" OnClick="lnkDelete_click" CommandArgument="saleDeptImageName"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Extra Thumbnail 1:
                                    </td>
                                    <td valign="top">
                                        <asp:FileUpload ID="image1small" runat="server" FileName='<%# Bind("saleImage1Small") %>' />
                                        <asp:HiddenField ID="hidImage1Small" runat="server" Value='<%# Eval("saleImage1Small") %>' />
                                        <br />
                                        <asp:HyperLink ID="lnkSaleImage1Small" runat="server" NavigateUrl='<%# "/images/products/" & Eval("saleImage1Small") %>' Text='<%# Eval("saleImage1Small") %>' Target="_blank"></asp:HyperLink>
                                        &nbsp;
                                                <asp:LinkButton ID="lnkDeleteSaleImage1Small" runat="server" ForeColor="Red" Text="X" OnClick="lnkDelete_click" CommandArgument="saleImage1Small"></asp:LinkButton>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="left">Extra Image 1:
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="image1" runat="server" FileName='<%# Bind("saleImage1") %>' />
                                        <asp:HiddenField ID="hidImage1" runat="server" Value='<%# Eval("saleImage1") %>' />
                                        <br />
                                        <asp:HyperLink ID="lnkSaleImage1" runat="server" NavigateUrl='<%# "/images/products/" & Eval("saleImage1") %>' Text='<%# Eval("saleImage1") %>' Target="_blank"></asp:HyperLink>
                                        &nbsp;
                                                <asp:LinkButton ID="lnkDeleteSaleImage1" runat="server" ForeColor="Red" Text="X" OnClick="lnkDelete_click" CommandArgument="saleImage1"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Extra Thumbnail 2:
                                    </td>
                                    <td valign="top">
                                        <asp:FileUpload ID="image2small" runat="server" FileName='<%# Bind("saleImage2Small") %>' />
                                        <asp:HiddenField ID="hidImage2Small" runat="server" Value='<%# Eval("saleImage2Small") %>' />
                                        <br />
                                        <asp:HyperLink ID="lnkSaleImage2Small" runat="server" NavigateUrl='<%# "/images/products/" & Eval("saleImage2Small") %>' Text='<%# Eval("saleImage2Small") %>' Target="_blank"></asp:HyperLink>
                                        &nbsp;
                                                <asp:LinkButton ID="lnkDeleteSaleImage2Small" runat="server" ForeColor="Red" Text="X" OnClick="lnkDelete_click" CommandArgument="saleImage2Small"></asp:LinkButton>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="left">Extra Image 2:
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="image2" runat="server" FileName='<%# Bind("saleImage2") %>' />
                                        <asp:HiddenField ID="hidImage2" runat="server" Value='<%# Eval("saleImage2") %>' />
                                        <br />
                                        <asp:HyperLink ID="lnkSaleImage2" runat="server" NavigateUrl='<%# "/images/products/" & Eval("saleImage2") %>' Text='<%# Eval("saleImage2") %>' Target="_blank"></asp:HyperLink>
                                        &nbsp;
                                                <asp:LinkButton ID="lnkDeleteSaleImage2" runat="server" ForeColor="Red" Text="X" OnClick="lnkDelete_click" CommandArgument="saleImage2"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Extra Thumbnail 3:
                                    </td>
                                    <td valign="top">
                                        <asp:FileUpload ID="image3small" runat="server" FileName='<%# Bind("saleImage3Small") %>' />
                                        <asp:HiddenField ID="hidImage3Small" runat="server" Value='<%# Eval("saleImage3Small") %>' />
                                        <br />
                                        <asp:HyperLink ID="lnkSaleImage3Small" runat="server" NavigateUrl='<%# "/images/products/" & Eval("saleImage3Small") %>' Text='<%# Eval("saleImage3Small") %>' Target="_blank"></asp:HyperLink>
                                        &nbsp;
                                                <asp:LinkButton ID="lnkDeleteSaleImage3Small" runat="server" ForeColor="Red" Text="X" OnClick="lnkDelete_click" CommandArgument="saleImage3Small"></asp:LinkButton>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td align="left">Extra Image 3:
                                    </td>
                                    <td>
                                        <asp:FileUpload ID="image3" runat="server" FileName='<%# Bind("saleImage3") %>' />
                                        <asp:HiddenField ID="hidImage3" runat="server" Value='<%# Eval("saleImage3") %>' />
                                        <br />
                                        <asp:HyperLink ID="lnkSaleImage3" runat="server" NavigateUrl='<%# "/images/products/" & Eval("saleImage3") %>' Text='<%# Eval("saleImage3") %>' Target="_blank"></asp:HyperLink>
                                        &nbsp;
                                                <asp:LinkButton ID="lnkDeleteSaleImage3" runat="server" ForeColor="Red" Text="X" OnClick="lnkDelete_click" CommandArgument="saleImage3"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Display thumbnails:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpAlign" runat="server" SelectedValue='<%# Bind("extraImageVertical") %>'>
                                            <asp:ListItem Text="Horizontally" Value="False"></asp:ListItem>
                                            <asp:ListItem Text="Verically" Value="True"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>&nbsp;</td>
                                    <td>Automatically show images:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chAutoImages" runat="server" Checked='<%# Bind("useAutoImages") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Short Description:
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtShortDescription" runat="server" MaxLength="1000" Width="220" Text='<%# Bind("saleShortDescription") %>'></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Show Sensations:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkSensations" runat="server" Checked='<%# Bind("showSensations") %>' />
                                    </td>
                                    <td></td>
                                    <td>Show Product on Shop Intro:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkShowOnIntro" runat="server" Checked='<%# Bind("showOnIntro") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Show Reviews:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkReviews" runat="server" Checked='<%# Bind("showReviews") %>' />
                                    </td>
                                    <td></td>
                                    <td>'Only' product (ie no charger):
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkOnly" runat="server" Checked='<%# Bind("onlyProduct") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td>Show on Affiliate Link Page:
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkAffLink" runat="server" Checked='<%# Bind("allowAffLink") %>' />
                                    </td>
                                    <td></td>
                                    <td>&nbsp;
                                    </td>
                                    <td>&nbsp;
                                    </td>
                                </tr>
                                <tr id="trReviewLink" runat="server" visible="false">
                                    <td valign="top">Link Review: (Reviews added for this product will also be added to the product below)
                                    </td>
                                    <td valign="top">
                                        <asp:DropDownList ID="drpReviewLink" runat="server">
                                        </asp:DropDownList>&nbsp;
                                                <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_click" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                Existing product links:&nbsp;
                                                <asp:Label ID="lblProductLinks" runat="Server"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td colspan="4">
                                        <asp:Button ID="lnkUpdate" runat="server" Text="Save Changes" CommandName="update"></asp:Button><br />
                                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">&nbsp;</td>
                                    <td colspan="2">
                                        <asp:RangeValidator ID="ran1" runat="server" ControlToValidate="drpDept" MinimumValue="1" MaximumValue="999999" ErrorMessage="You must select a Department" Display="dynamic"></asp:RangeValidator>
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                    </asp:FormView>
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td colspan="3" runat="server" visible="false" id="tdLinks">
                <br />
                <br />
                <b>External/Internal links for this product:</b><br />
                <br />
                <asp:Panel ID="panLinks" runat="server" Visible="true" Width="100%" Height="300" BorderWidth="1" BorderColor="gray" ScrollBars="vertical">
                    <asp:GridView ID="gvLinks" runat="server" EmptyDataText="No links exist yes" SkinID="GridView" DataSourceID="sqlLinks" Width="98%" AutoGenerateColumns="false" ShowFooter="false" DataKeyNames="productMenuID">
                        <Columns>
                            <asp:BoundField HeaderText="Description" ControlStyle-Width="200" DataField="text" />
                            <asp:BoundField ItemStyle-Width="60" ReadOnly="true" />
                            <asp:BoundField HeaderText="URL/Link" ControlStyle-Width="200" DataField="link" />
                            <asp:BoundField ItemStyle-Width="60" ReadOnly="true" />
                            <asp:CheckBoxField HeaderText="Active" DataField="active" />
                            <asp:BoundField ItemStyle-Width="60" ReadOnly="true" />
                            <asp:CommandField EditText="Edit" ShowEditButton="true" ShowCancelButton="false" />
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />
                    <asp:LinkButton ID="lnkAddLink" runat="server" Text="Add New Link" OnClick="lnkAddLink_click"></asp:LinkButton>
                    <br />
                    <asp:Table ID="tblAddLink" runat="server" Visible="false">
                        <asp:TableRow>
                            <asp:TableCell>
                                        <b>Name:</b>
                            </asp:TableCell>
                            <asp:TableCell>
                                        &nbsp;
                            </asp:TableCell>
                            <asp:TableCell>
                                        <b>Link:</b>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell>
                                <asp:TextBox ID="txtDescription" runat="server" MaxLength="500" ValidationGroup="addLink" Width="300"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqTxtDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="<br />Required" Display="dynamic" ValidationGroup="addLink"></asp:RequiredFieldValidator>
                            </asp:TableCell>
                            <asp:TableCell>
                                        &nbsp;
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="txtLink" runat="server" MaxLength="500" Width="300"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="reqTxtLink" runat="server" ControlToValidate="txtLink" ErrorMessage="<br />Required" Display="dynamic" ValidationGroup="addLink"></asp:RequiredFieldValidator>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell ColumnSpan="3" HorizontalAlign="right">
                                <asp:Button ID="btnAddSubmit" runat="server" Text="Submit Link" ValidationGroup="addLink" OnClick="btnAddLink_click" />
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </asp:Panel>
            </td>
        </tr>
    </table>


    <asp:SqlDataSource ID="SqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlOnSale" runat="server" SelectCommand="procProductOnSaleByCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" DeleteCommand="procProductOnSaleByIDDelete" DeleteCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpDept" PropertyName="selectedValue" Name="deptID" Type="string" Size="5" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="ID" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlComp" runat="server" SelectCommand="procAffiliateProductBuyingByDistIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="distID" Type="int32" />
            <asp:Parameter Name="productType" DefaultValue="component" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProd" runat="server" SelectCommand="procAffiliateProductBuyingByDistIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="distID" Type="int32" />
            <asp:Parameter Name="productType" DefaultValue="product" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlExt" runat="server" SelectCommand="procAffiliateProductBuyingByDistIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="distID" Type="int32" />
            <asp:Parameter Name="productType" DefaultValue="external" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlBProd" runat="server" SelectCommand="procAffiliateProductBuyingByDistIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="distID" Type="int32" />
            <asp:Parameter Name="productType" DefaultValue="bproduct" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDetails" runat="server" SelectCommand="procProductOnSaleByIdSelect" SelectCommandType="StoredProcedure" UpdateCommand="procProductOnSaleByIDUpdate" UpdateCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnUpdated="SqlDetails_updated">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvOnSale" PropertyName="selectedValue" Name="id" Type="int32" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="id" Type="int32" />
            <asp:Parameter Name="saleUnitPrice" Type="decimal" />
            <asp:Parameter Name="saleTaxRate" Type="decimal" />
            <asp:Parameter Name="saleDiscount" Type="decimal" />
            <asp:Parameter Name="saleProdCode" Type="string" Size="50" />
            <asp:Parameter Name="saleDescription" Type="string" Size="-1" />
            <asp:Parameter Name="saleImageName" Type="string" Size="100" />
            <asp:Parameter Name="saleDeptImageName" Type="string" Size="100" />
            <asp:Parameter Name="saleImage1Small" Type="string" Size="100" />
            <asp:Parameter Name="saleImage1" Type="string" Size="100" />
            <asp:Parameter Name="saleImage2Small" Type="string" Size="100" />
            <asp:Parameter Name="saleImage2" Type="string" Size="100" />
            <asp:Parameter Name="saleImage3Small" Type="string" Size="100" />
            <asp:Parameter Name="saleImage3" Type="string" Size="100" />
            <asp:Parameter Name="extraImageVertical" Type="Boolean" />
            <asp:Parameter Name="useAutoImages" Type="Boolean" />
            <asp:Parameter Name="saleShortDescription" Type="string" Size="1000" />
            <asp:Parameter Name="saleDeptID" Type="int32" />
            <asp:Parameter Name="showOnIntro" Type="boolean" />
            <asp:Parameter Name="showSensations" Type="boolean" />
            <asp:Parameter Name="showReviews" Type="boolean" />
            <asp:Parameter Name="onlyProduct" Type="boolean" />
            <asp:Parameter Name="allowAffLink" Type="boolean" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlAff" runat="Server" SelectCommand="procAffiliatesByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDept" runat="server" SelectCommand="procDeptByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlLinks" runat="server" ConnectionString="<%$ ConnectionStrings:connectionString %>" SelectCommand="procProductMenuByposIDSelect" SelectCommandType="StoredProcedure" UpdateCommand="procProductMenuByIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvOnSale" PropertyName="selectedValue" Name="posID" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="productMenuID" Type="int32" />
            <asp:Parameter Name="text" Type="string" Size="500" />
            <asp:Parameter Name="link" Type="string" Size="500" />
            <asp:Parameter Name="active" Type="boolean" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

