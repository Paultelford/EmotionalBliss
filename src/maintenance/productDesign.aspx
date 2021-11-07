<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productDesign.aspx.vb" Inherits="maintenance_productDesign" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>    
    <table border="0">
        <tr>
            <td>
                Select Product:
            </td>
            <td>
                <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMaster" DataTextField="name" DataValueField="masterID" AppendDataBoundItems="true" EnableViewState="true" AutoPostBack="true" CausesValidation="false" OnSelectedIndexChanged="drpMaster_selectedIndexChanged">
                    <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
                <atlas:UpdatePanel ID="update1" runat="server">
                    <ContentTemplate>
                        <asp:DropDownList ID="drpProduct" runat="server" DataSourceID="SqlProductsByMasterID" DataTextField="productName" DataValueField="productID" AppendDataBoundItems="true" OnSelectedIndexChanged="drpProduct_selectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </ContentTemplate>
                    <Triggers>
                        <atlas:AsyncPostBackTrigger ControlID="drpMaster" EventName="selectedIndexChanged" />
                    </Triggers>
                </atlas:UpdatePanel>
            </td>
        </tr>
    </table>
    <br />
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" AssociatedUpdatePanelID="update2">
        <ProgressTemplate>
            Loading...<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <br />
    <atlas:UpdatePanel ID="update2" runat="server">
        <ContentTemplate>
            <asp:Table ID="tblComponents" runat="server" Width="100%">
                <asp:TableRow>
                    <asp:TableCell>
                        <b>Components used in Product:</b>
                    </asp:TableCell>
                    <asp:TableCell>
                        <b>All Components:</b>
                    </asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell VerticalAlign="top">
                        <asp:GridView ID="gvProdComp" runat="server" GridLines="none" AutoGenerateColumns="false" DataKeyNames="prodCompID" DataSourceID="sqlProdComp" EmptyDataText="No components added yet." OnDataBound="gvProdComp_dataBound" OnRowUpdating="gvProdComp_rowUpdating">
                            <Columns>
                                <asp:BoundField HeaderText="Component" DataField="masterName" ReadOnly="true" />
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:BoundField HeaderText="Quantity" DataField="qty" />
                                <asp:CommandField EditText="Edit" ShowEditButton="true" ShowCancelButton="false" />
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:HyperLink ID="lnkDefaults" runat="server" Text="Set Default Components" ></asp:HyperLink><br />
                        <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
                    </asp:TableCell>
                    <asp:TableCell VerticalAlign="top">
                        <asp:Panel ID="panSearch" runat="server">
                            <b>Search:</b> <asp:TextBox ID="txtSearch" runat="server" MaxLength="20" AutoPostBack="false" OnTextChanged="txtSearch_textChanged"></asp:TextBox>&nbsp;<asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_click" Text="go" Visible="false" />
                        </asp:Panel>
                        <asp:Panel ID="panList" runat="server">
                            <asp:GridView ID="gvProd" runat="server" GridLines="none" DataSourceID="sqlProducts" DataKeyNames="masterID" AutoGenerateColumns="false" ShowHeader="false" OnSelectedIndexChanging="gvProd_selectedIndexChanging">
                                <Columns>
                                    <asp:CommandField SelectText="Add" ShowSelectButton="true" />
                                    <asp:BoundField ItemStyle-Width="30" />
                                    <asp:BoundField DataField="name" />
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </ContentTemplate>
        <Triggers>
            <atlas:AsyncPostBackTrigger ControlID="drpProduct" EventName="selectedIndexChanged" />
        </Triggers>
    </atlas:UpdatePanel>       
   
    <asp:SqlDataSource ID="SqlProductsByMasterID" runat="server" SelectCommand="procProductsByMasterIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMaster" PropertyName="selectedValue" Name="masterID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlProducts" runat="server" SelectCommand="procComponentMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlProdComp" runat="server" SelectCommand="procProductComponentByProdIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procProductComponentByIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpProduct" PropertyName="selectedValue" Name="prodID" Type="int16" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="prodCompID" Type="int16" />
            <asp:Parameter Name="qty" Type="int16" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlMaster" runat="server" SelectCommand="procProductMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>

