<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeFile="componentAdd.aspx.vb" Inherits="maintenance_componentAdd" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:ScriptManagerProxy id="sm" runat="server" />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table border="0">
                <tr>
                    <td>
                        Master Type:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMasterTypes" DataTextField="name" DataValueField="masterID" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpMaster_selectedIndexChanged">
                            <asp:ListItem Text="Please choose...." Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        Choose a component to Edit:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpComp" runat="server" DataSourceID="sqlComponentsList" DataTextField="componentName" DataValueField="componentID" OnSelectedIndexChanged="drpComp_selectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true">
                            <asp:ListItem Text="Please choose...." Value="0" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br />
            Or enter a new component below and then click 'Add'.
            <br /><br />            
            <asp:DetailsView ID="dvComp" runat="server" DataSourceID="sqlComponent" BorderWidth="0" GridLines="none" DataKeyNames="componentID" AutoGenerateRows="false" DefaultMode="insert" OnItemInserted="dvComp_itemInserted" OnItemUpdating="dvComp_itemUpdated">
                <FieldHeaderStyle VerticalAlign="top" />
                <Fields>
                    <asp:BoundField HeaderText="Name:" DataField="componentName" ControlStyle-Width="200" />
                    <asp:TemplateField HeaderText="Manufacturer:">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpManu" runat="server" DataSourceID="sqlExistingManufacturers" DataTextField="manufacturerName" DataValueField="manufacturerID"></asp:DropDownList>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="drpManu" runat="server" DataSourceID="sqlExistingManufacturers" DataTextField="manufacturerName" DataValueField="manufacturerID" SelectedValue='<%# Bind("manufacturerID") %>'></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Master Type:">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMasterTypes" DataTextField="name" DataValueField="masterID">
                            </asp:DropDownList>
                            <asp:RangeValidator id="ranDrpMaster" runat="server" ControlToValidate="drpMaster" Type="Integer" MinimumValue="1" MaximumValue="9999999" ErrorMessage="* Required"></asp:RangeValidator>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMasterTypes" DataTextField="name" DataValueField="masterID" SelectedValue='<%# Bind("masterID") %>' AppendDataBoundItems="true">
                                <asp:ListItem Text="Please choose...." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RangeValidator id="ranDrpMaster" runat="server" ValidationGroup="testGroup" ControlToValidate="drpMaster" Type="Integer" MinimumValue="1" MaximumValue="9999999" ErrorMessage="* Required"></asp:RangeValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Code:">
                        <ItemTemplate>
                            <asp:TextBox ID="txtComponentCode" runat="server" Text='<%# Bind("componentCode") %>'></asp:TextBox><asp:RequiredFieldValidator ID="reqCompCode" ValidationGroup="testGroup" runat="server" ControlToValidate="txtComponentCode" ErrorMessage="* Required field" Display="dynamic"></asp:RequiredFieldValidator>
                            <asp:Label ID="lblCompCodeError" runat="server" ForeColor="red"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Reorder:">
                        <ItemTemplate>
                            <asp:TextBox ID="txtReorder" runat="server" MaxLength="6"></asp:TextBox><asp:RangeValidator ID="r1a" runat="server" MinimumValue="0" MaximumValue="99999" ControlToValidate="txtReorder" Display="Dynamic" ErrorMessage="You must enter a numerical value."></asp:RangeValidator><asp:RequiredFieldValidator ID="req1a" runat="server" ControlToValidate="txtReorder" ErrorMessage=" Required field" Display="dynamic"></asp:RequiredFieldValidator>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtReorder" runat="server" MaxLength="6" Text='<%# Bind("reorderLevel") %>'></asp:TextBox><asp:RangeValidator ID="r1b" runat="server" ValidationGroup="testGroup" MinimumValue="0" MaximumValue="99999" ControlToValidate="txtReorder" Display="Dynamic" ErrorMessage="You must enter a numerical value."></asp:RangeValidator><asp:RequiredFieldValidator ID="req1b" runat="server" ValidationGroup="testGroup" ControlToValidate="txtReorder" ErrorMessage="Required field" Display="dynamic"></asp:RequiredFieldValidator>
                            <asp:Label ID="lblReorderError" runat="server" ForeColor="red"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Min Order:" DataField="minimumOrder" />
                    <asp:BoundField HeaderText="Box Qty:" DataField="boxQty" />
                    <asp:BoundField HeaderText="Lead Time:" DataField="leadTime" />
                    <asp:BoundField HeaderText="Location Bay:" DataField="locationBay" />
                    <asp:CheckBoxField HeaderText="Warehouse Edit:" DataField="warehouseEdit" />
                    <asp:CheckBoxField HeaderText="Component Active:" DataField="active" />
                    <asp:TemplateField HeaderText="Description:">
                        <ItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="1000" Rows="5" columns="40" TextMode="multiLine"></asp:TextBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDescription" runat="server" MaxLength="1000" Rows="5" columns="40" TextMode="multiLine" Text='<%# Bind("componentDescription") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="More Info:">
                        <ItemTemplate>
                            <asp:TextBox ID="txtMoreInfo" runat="server" MaxLength="1000" TextMode="multiLine"></asp:TextBox>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtMoreInfo" runat="server" MaxLength="1000" Rows="5" columns="40" TextMode="multiLine" Text='<%# Bind("moreInfo") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Button" ShowEditButton="true" EditText="Edit" ValidationGroup="testGroup" UpdateText="Update" ShowInsertButton="true" InsertText="Add" ShowCancelButton="false" />
                </Fields>
            </asp:DetailsView>
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlComponentsList" runat="server" SelectCommand="procComponentsByMasterIDListSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMaster" Name="masterID" Type="int32" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlExistingManufacturers" runat="server" SelectCommand="procManufacturersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlMasterTypes" runat="server" SelectCommand="procComponentMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlComponent" runat="server" SelectCommand="procComponentByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" InsertCommand="procComponentInsert" InsertCommandType="StoredProcedure" UpdateCommand="procComponentByIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter Name="compID" Type="int32" ControlID="drpComp" PropertyName="selectedValue" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="componentName" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerID" Type="int16" />
            <asp:Parameter Name="masterID" type="int16" />
            <asp:Parameter Name="componentCode" Type="string" size="20" />
            <asp:Parameter Name="reorderLevel" Type="int16" />
            <asp:Parameter Name="minimumOrder" Type="int16" />
            <asp:Parameter Name="boxQty" Type="int16" />
            <asp:Parameter Name="leadTime" Type="string" Size="20" />
            <asp:Parameter Name="componentDescription" Type="string" Size="1000" />
            <asp:Parameter Name="moreInfo" Type="string" Size="1000" />
            <asp:Parameter Name="locationBay" Type="string" Size="50" />
            <asp:Parameter Name="warehouseEdit" Type="boolean" />
            <asp:Parameter Name="active" Type="boolean" />
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="componentName" Type="string" Size="50" />
            <asp:Parameter Name="manufacturerID" Type="int16" />
            <asp:Parameter Name="masterID" type="int16" />
            <asp:Parameter Name="componentCode" Type="string" size="20" />
            <asp:Parameter Name="reorderLevel" Type="int16" />
            <asp:Parameter Name="minimumOrder" Type="int16" />
            <asp:Parameter Name="boxQty" Type="int16" />
            <asp:Parameter Name="leadTime" Type="string" Size="20" />
            <asp:Parameter Name="componentDescription" Type="string" Size="1000" />
            <asp:Parameter Name="moreInfo" Type="string" Size="1000" />
            <asp:Parameter Name="locationBay" Type="string" Size="50" />
            <asp:Parameter Name="warehouseEdit" Type="boolean" />
            <asp:Parameter Name="active" Type="boolean" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>