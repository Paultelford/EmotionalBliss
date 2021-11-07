<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="manufacturerAdd.aspx.vb" Inherits="maintenance_manufacturerAdd" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Select a Manufacturer to Edit:&nbsp;
            <asp:DropDownList ID="drpMan" runat="server" DataSourceID="sqlExistingManufacturers" DataTextField="manufacturerName" DataValueField="manufacturerID" AutoPostBack="true" OnSelectedIndexChanged="drpMan_selectedIndexChanged" AppendDataBoundItems="true">
                <asp:ListItem Text="Please choose...." Value="0"></asp:ListItem>    
            </asp:DropDownList><br />
            Or add new Manufacturer details below and click 'Add', or<br />
            
            <br /><br />
            <asp:DetailsView ID="dvManufacturer" runat="server" AutoGenerateRows="false" DataKeyNames="manufacturerID" BorderWidth="0" GridLines="none" DataSourceID="sqlManufacturers" DefaultMode="insert" OnItemInserted="dvManufacturer_itemInserted" OnItemUpdated="dvManufacurer_itemUpdated">
                <Fields>
                    <asp:BoundField HeaderText="Name:" DataField="manufacturerName" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="Address:" DataField="manufacturerAdd1" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="manufacturerAdd2" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="manufacturerAdd3" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="manufacturerAdd4" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="manufacturerAdd5" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="" DataField="manufacturerAdd6" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="Tel:" DataField="manufacturerTel" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="Website:" DataField="manufacturerWebsite" ControlStyle-Width="200" />
                    <asp:BoundField HeaderText="Email:" DataField="manufacturerEmail" ControlStyle-Width="200" />
                    <asp:TemplateField HeaderText="Currency:">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpCurrency" runat="server" DataSourceID="sqlCurrency" selectedValue='<%# Bind("manufacturerCurrency") %>' DataTextField="currencyCode" DataValueField="currencyCode"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("manufacturerActive") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Button" ShowEditButton="true" EditText="Edit" UpdateText="Update" ShowInsertButton="true" InsertText="Add" ShowCancelButton="false" />
                </Fields>
            </asp:DetailsView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlExistingManufacturers" runat="server" SelectCommand="procManufacturersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlManufacturers" runat="server" UpdateCommand="procManufacturerByIDUpdate" UpdateCommandType="StoredProcedure" InsertCommand="procManufacturerInsert" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" SelectCommandType="StoredProcedure" SelectCommand="procManufacturerByIDSelect">
        <InsertParameters>
            <asp:Parameter Name="manufacturerName" Type="string" Size="30" />
            <asp:Parameter Name="manufacturerAdd1" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd2" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd3" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd4" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd5" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd6" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerTel" Type="string" Size="30" />
            <asp:Parameter Name="manufacturerWebsite" Type="string" Size="100" />
            <asp:Parameter Name="manufacturerEmail" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerCurrency" Type="string" Size="5" />
            <asp:Parameter Name="manufacturerActive" Type="boolean" />
        </InsertParameters>    
        <UpdateParameters>
            <asp:Parameter Name="manufacturerName" type="string" Size="30" />
            <asp:Parameter Name="manufacturerAdd1" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd2" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd3" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd4" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd5" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerAdd6" Type="string" Size="50" />  
            <asp:Parameter Name="manufacturerTel" Type="string" Size="30" />
            <asp:Parameter Name="manufacturerWebsite" Type="string" Size="100" />          
            <asp:Parameter Name="manufacturerEmail" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerCurrency" Type="string" Size="5" />
            <asp:Parameter Name="manufacturerActive" Type="boolean" />
        </UpdateParameters>
        <SelectParameters>
            <asp:ControlParameter Name="manID" ControlID="drpMan" Type="int16" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCurrency" runat="server" SelectCommand="procCurrencySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    
</asp:Content>

