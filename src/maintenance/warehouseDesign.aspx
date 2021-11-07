<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="warehouseDesign.aspx.vb" Inherits="maintenance_warehouseDesign" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    Select a Boxed Product:&nbsp;
    <asp:DropDownList ID="drpBProduct" runat="server" DataSourceID="SqlBProducts" DataTextField="FullName" DataValueField="warehouseProductID" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="drpBProduct_selectedIndexChanged">
        <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
    </asp:DropDownList>
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <br /><br />    
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table id="tblMain" runat="server" visible="false">
                <tr>
                    <td valign="top">
                        <b>Box contents</b>
                        <asp:Panel ID="panContent" runat="server" BorderWidth="1" ScrollBars="auto" Width="400" Height="440" BorderColor="lightgray">
                            <asp:GridView ID="gvContentList" runat="server" GridLines="none" DataKeyNames="contentID" DataSourceID="SqlContentList" AutoGenerateColumns="false" EmptyDataText="None assigned yet" Width="100%">
                                <Columns>
                                    <asp:BoundField HeaderText="Product" DataField="productName" ReadOnly="true" />
                                    <asp:BoundField HeaderText="Component" DataField="componentName" ReadOnly="true" />
                                    <asp:TemplateField HeaderText="Qty">
                                        <EditItemTemplate>
                                            <asp:TextBox ID="txtQty" runat="server" Width="30" Text='<%# Bind("contentQty") %>'></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtQty" ErrorMessage="Invalid" Display="dynamic"></asp:RequiredFieldValidator><asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtQty" MinimumValue="1" MaximumValue="99999" Display="Dynamic" ErrorMessage="Invalid"></asp:RangeValidator>
                                        </EditItemTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("contentQty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:CommandField EditText="Edit" ShowEditButton="true" />
                                    <asp:CommandField DeleteText="D" ShowDeleteButton="true" />                                    
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </td>
                    <td valign="top" id="selection" runat="server">
                        <b>Component List</b>
                        <asp:Panel ID="pan1" runat="server" BorderWidth="1" Height="200" ScrollBars="auto" Width="300" BorderColor="lightgray">
                            Master Type:&nbsp;
                            <asp:DropDownList ID="drpCompMaster" runat="server" DataSourceID="SqlCompMaster" DataTextField="name" DataValueField="masterID" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="drpCompMaster_selectedIndexChanged"></asp:DropDownList>
                            <asp:GridView ID="gvCompList" runat="server" GridLines="none" DataKeyNames="componentID" DataSourceID="SqlCompList" AutoGenerateColumns="false" OnSelectedIndexChanging="gvCompList_selectedIndexChanging">
                                <Columns>
                                    <asp:CommandField ButtonType="link" SelectText="Add" ShowSelectButton="true" ControlStyle-Width="40" />
                                    <asp:BoundField HeaderText="Component" DataField="componentName" />
                                    <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel><br />
                        <b>Product List</b>
                        <asp:Panel ID="pan2" runat="server" BorderWidth="1" Height="200" ScrollBars="auto" Width="300" BorderColor="lightgray">                   
                        <asp:DropDownList ID="drpProdMaster" runat="server" DataSourceID="SqlProdMaster" DataTextField="name" DataValueField="masterID" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="drpProdMaster_selectedIndexChanged"></asp:DropDownList>
                            <asp:GridView ID="gvProdList" runat="server" GridLines="none" DataKeyNames="productID" DataSourceID="SqlProdList" AutoGenerateColumns="false" OnSelectedIndexChanging="gvProdList_selectedIndexChanging">
                                <Columns>
                                    <asp:CommandField ButtonType="link" SelectText="Add" ShowSelectButton="true" ControlStyle-Width="40" />
                                    <asp:BoundField HeaderText="Product" DataField="ProductName" />
                                    <asp:BoundField HeaderText="Code" DataField="ref" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <atlas:AsyncPostBackTrigger ControlID="drpBProduct" EventName="selectedIndexChanged" />
        </Triggers>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="SqlBProducts" runat="server" SelectCommand="procWarehouseProductsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlContentList" runat="server" SelectCommand="procWarehouseBoxContentByBProductIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" DeleteCommand="procWarehouseBoxContentDelete" DeleteCommandType="StoredProcedure" UpdateCommand="warehouseBoxContentByIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpBProduct" name="warehouseProductID" Type="int32" PropertyName="selectedValue" />
        </SelectParameters>
        <DeleteParameters>
            <asp:Parameter Name="contentID" Type="int32" />
        </DeleteParameters>
        <UpdateParameters>
            <asp:Parameter Name="contentID" Type="int32" />
            <asp:Parameter Name="contentQty" Type="int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCompMaster" runat="server" SelectCommand="procComponentMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCompList" runat="server" SelectCommand="procComponentsByMasterIDSelect2" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpCompMaster" name="masterID" Type="int32" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProdList" runat="server" SelectCommand="procProductsByMasterIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpProdMaster" name="masterID" Type="int32" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlProdMaster" runat="server" SelectCommand="procProductMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    
</asp:Content>


