<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="supplierAdd.aspx.vb" Inherits="maintenance_supplierAdd" title="Untitled Page" %>
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
    <atlas:UpdatePanel ID="update1" runat="Server">
        <ContentTemplate>
            Select a Supplier to Edit:&nbsp;
            <asp:DropDownList ID="drpMan" runat="server" DataSourceID="sqlExistingSuppliers" DataTextField="supplierName" DataValueField="supplierID" AutoPostBack="true" OnSelectedIndexChanged="drpMan_selectedIndexChanged" AppendDataBoundItems="true">
                <asp:ListItem Text="Please choose...." Value="0"></asp:ListItem>    
            </asp:DropDownList><br />
            Or add new supplier details below and click 'Add', or<br />
            
            <br /><br />
            <asp:DetailsView ID="dvSupplier" runat="server" AutoGenerateRows="false" DataKeyNames="supplierID" BorderWidth="0" GridLines="none" DataSourceID="SqlSuppliers" DefaultMode="insert" OnItemInserted="dvSupplier_itemInserted" OnItemUpdated="dvSupplier_itemUpdated">
                <Fields>
                    <asp:BoundField HeaderText="Name:" DataField="supplierName" />
                    <asp:BoundField HeaderText="Address:" DataField="supplierAdd1" />
                    <asp:BoundField HeaderText="" DataField="supplierAdd2" />
                    <asp:BoundField HeaderText="" DataField="supplierAdd3" />
                    <asp:BoundField HeaderText="" DataField="supplierAdd4" />
                    <asp:BoundField HeaderText="" DataField="supplierAdd5" />
                    <asp:BoundField HeaderText="" DataField="supplierAdd6" />
                    <asp:BoundField HeaderText="Tel:" DataField="supplierTel" />
                    <asp:BoundField HeaderText="Website:" DataField="supplierWebsite" />
                    <asp:BoundField HeaderText="Email:" DataField="supplierEmail" />
                    <asp:TemplateField HeaderText="Currency:">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpCurrency" runat="server" DataSourceID="sqlCurrency" selectedValue='<%# Bind("supplierCurrency") %>' DataTextField="currencyCode" DataValueField="currencyCode" OnDataBound="drpCurrency_dataBound"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CheckBoxField HeaderText="Active" DataField="supplierActive" />
                    
                    <asp:CommandField ButtonType="Button" ShowEditButton="true" EditText="Edit" UpdateText="Update" ShowInsertButton="true" InsertText="Add" ShowCancelButton="false" />
                </Fields>
            </asp:DetailsView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="sqlExistingSuppliers" runat="server" SelectCommand="procSuppliersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSuppliers" runat="server" UpdateCommand="procSupplierByIDUpdate" UpdateCommandType="StoredProcedure" InsertCommand="procSupplierInsert" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" SelectCommandType="StoredProcedure" SelectCommand="procSupplierByIDSelect">
        <InsertParameters>
            <asp:Parameter Name="supplierName" Type="string" Size="30" />
            <asp:Parameter Name="supplierAdd1" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd2" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd3" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd4" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd5" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd6" Type="string" Size="50" />
            <asp:Parameter Name="supplierTel" Type="string" Size="30" />
            <asp:Parameter Name="supplierWebsite" Type="string" Size="100" />
            <asp:Parameter Name="supplierEmail" Type="string" Size="50" />
            <asp:Parameter Name="supplierCurrency" Type="string" Size="5" />
            <asp:Parameter Name="supplierActive" Type="boolean" />
            <asp:SessionParameter SessionField="EBDistID" Name="affID" Type="int32" />
        </InsertParameters>    
        <UpdateParameters>
            <asp:Parameter Name="supplierName" type="string" Size="30" />
            <asp:Parameter Name="supplierAdd1" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd2" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd3" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd4" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd5" Type="string" Size="50" />
            <asp:Parameter Name="supplierAdd6" Type="string" Size="50" />  
            <asp:Parameter Name="supplierTel" Type="string" Size="30" />
            <asp:Parameter Name="supplierWebsite" Type="string" Size="100" />          
            <asp:Parameter Name="supplierEmail" Type="string" Size="50" />
            <asp:Parameter Name="supplierCurrency" Type="string" Size="5" />
            <asp:Parameter Name="supplierActive" Type="boolean" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter Name="manID" ControlID="drpMan" Type="int16" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    
</asp:Content>

