<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productAdd.aspx.vb" Inherits="maintenance_productAdd" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Choose a Product to Edit:&nbsp;
    <asp:DropDownList ID="drpProduct" runat="server" DataSourceID="sqlExistingProducts" DataTextField="productName" DataValueField="productID" AutoPostBack="true" OnSelectedIndexChanged="drpProduct_selectedIndexChanged" AppendDataBoundItems="true">
        <asp:ListItem Value="0" Text="Please choose...."></asp:ListItem>
    </asp:DropDownList><br />
    Or enter new Product Details below and click 'Add'.
    <br /><br />
    
    <asp:DetailsView ID="dvProduct" runat="server" DefaultMode="insert" DataKeyNames="productID" DataSourceID="sqlProduct" GridLines="none" AutoGenerateRows="false" OnItemInserted="dvProduct_itemInserted" OnItemUpdated="dvProduct_itemUpdated">
        <HeaderStyle VerticalAlign="top" />
        <Fields>
            <asp:BoundField HeaderText="Name:" DataField="productName" />
            <asp:TemplateField HeaderText="Master Type:">
                <ItemTemplate>
                    <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMasterTypes" DataTextField="name" DataValueField="masterID"></asp:DropDownList>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMasterTypes" DataTextField="name" DataValueField="masterID" selectedValue='<%# Bind("masterID") %>'></asp:DropDownList>
                </EditItemTemplate>
                <InsertItemTemplate>
                    <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMasterTypes" DataTextField="name" DataValueField="masterID" selectedValue='<%# Bind("masterID") %>'></asp:DropDownList>
                </InsertItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Ref#:" DataField="ref" ControlStyle-Width="100" />
            <asp:TemplateField HeaderStyle-VerticalAlign="top" HeaderText="Description:">
                <ItemTemplate>
                    <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Columns="40" Rows="6" MaxLength="1000"></asp:TextBox>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtDesc" runat="server" TextMode="MultiLine" Columns="40" Rows="6" MaxLength="1000" Text='<%# Bind("productDescription") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:CommandField ButtonType="Button" ShowEditButton="true" EditText="Edit" UpdateText="Update" ShowInsertButton="true" InsertText="Add" ShowCancelButton="false" />
        </Fields>    
    </asp:DetailsView>
    
    <asp:SqlDataSource ID="sqlProduct" runat="server" SelectCommand="procProductByIDSelect" SelectCommandType="StoredProcedure" UpdateCommand="procProductByIDUpdate" UpdateCommandType="StoredProcedure" InsertCommand="procProductInsert" InsertCommandType="StoredProcedure" ConnectionString='<%$ ConnectionStrings:connectionString %>'>
        <SelectParameters>
            <asp:ControlParameter Name="productID" Type="int16" ControlID="drpProduct" PropertyName="selectedValue" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="productName" Type="string" />
            <asp:Parameter Name="masterID" Type="int16" />
            <asp:Parameter Name="ref" Type="string" />
            <asp:Parameter Name="productDescription" type="string" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="productName" Type="string" />
            <asp:Parameter Name="masterID" Type="int16" />
            <asp:Parameter Name="ref" Type="string" />
            <asp:Parameter Name="productDescription" type="string" />
        </InsertParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlMasterTypes" runat="server" SelectCommand="procProductMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlExistingProducts" runat="server" SelectCommand="procProductsSelect" SelectCommandType="StoredProcedure" ConnectionString='<%$ ConnectionStrings:connectionString %>'></asp:SqlDataSource>
</asp:Content>

