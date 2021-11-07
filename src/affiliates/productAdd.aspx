<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/maffs.master" CodeFile="productAdd.aspx.vb" Inherits="maintenance_productAdd" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>        
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Choose a product to Edit:&nbsp;
            <asp:DropDownList ID="drpComp" runat="server" DataSourceID="sqlComponentsList" DataTextField="affProductName" DataValueField="affProductBuyingID" OnSelectedIndexChanged="drpComp_selectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true">
                <asp:ListItem Text="Please choose...." Value="0" />
            </asp:DropDownList>
            <br />
            Or enter a new product below and then click 'Add'.
            <br /><br />
            <asp:DetailsView ID="dvComp" runat="server" DataSourceID="sqlComponent" BorderWidth="0" GridLines="none" DataKeyNames="affProductBuyingID" AutoGenerateRows="false" DefaultMode="insert" OnItemInserted="dvComp_itemInserted" OnItemUpdated="dvComp_itemUpdated">
                <FieldHeaderStyle VerticalAlign="top" />
                <Fields>
                    <asp:BoundField HeaderText="Name:" DataField="affProductName" ControlStyle-Width="200" />
                    <asp:TemplateField HeaderText="Manufacturer:">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpManu" runat="server" DataSourceID="sqlExistingSuppliers" DataTextField="supplierName" DataValueField="supplierID"></asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="drpManu" runat="server" DataSourceID="sqlExistingSuppliers" DataTextField="supplierName" DataValueField="supplierID" selectedValue='<%# Bind("supplierID") %>' ></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Unit Price" DataField="affUnitPrice" ControlStyle-Width="60" />
                    <asp:BoundField HeaderText="Tax Rate" DataField="affTaxRate" ControlStyle-Width="60" />
                    <asp:CheckBoxField HeaderText="Active" DataField="affProductActive" />
                    <asp:CommandField ButtonType="Button" ShowEditButton="true" EditText="Edit" UpdateText="Update" ShowInsertButton="true" InsertText="Add" ShowCancelButton="false" />
                </Fields>
            </asp:DetailsView>
        </ContentTemplate>
    </atlas:UpdatePanel>
            
    <asp:SqlDataSource ID="sqlComponentsList" runat="server" SelectCommand="procAffiliateProductBuyingByAffIDExtSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlExistingSuppliers" runat="server" SelectCommand="procSuppliersByAffIDActiveSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlMasterTypes" runat="server" SelectCommand="procComponentMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlComponent" runat="server" SelectCommand="procAffiliateProductBuyingByAffProductBuyingIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" InsertCommand="procAffiliateProductBuyingInsert3" InsertCommandType="StoredProcedure" UpdateCommand="procAffiliateProductBuyingByIDUpdate2" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter Name="affProductBuyingID" Type="int32" ControlID="drpComp" PropertyName="selectedValue" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="affProductName" Type="string" Size="50" />
            <asp:Parameter Name="supplierID" Type="int32" />
            <asp:Parameter Name="affUnitPrice" type="decimal" />
            <asp:Parameter Name="affTaxRate" Type="decimal" />
            <asp:Parameter Name="affProductActive" Type="boolean" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="affProductName" Type="string" Size="50" />
            <asp:Parameter Name="supplierID" Type="int32" />
            <asp:Parameter Name="affUnitPrice" type="decimal" />
            <asp:Parameter Name="affTaxRate" Type="decimal" />
            <asp:Parameter Name="affProductActive" Type="boolean" />
            <asp:Parameter Name="affProductType" Type="string" Size="10" DefaultValue="external" />
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" /> 
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>