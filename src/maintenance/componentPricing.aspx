<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentPricing.aspx.vb" Inherits="maintenance_componentPricing" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy> 
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait.... <img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table border="0">
                <tr>
                    <td>
                        &nbsp;       
                    </td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpType_selectedIndexChanged">
                            <asp:ListItem Text="View By..." Selected="true" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Master Type" Value="master"></asp:ListItem>
                            <asp:ListItem Text="Manufacturer" Value="manu"></asp:ListItem>
                        </asp:DropDownList>        
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:DropDownList Visible="false" ID="drpMaster" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="drpMaster_selectedIndexChanged" OnDataBound="drpMaster_dataBound">
                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                            <asp:ListItem Text="All" Value="%"></asp:ListItem>
                        </asp:DropDownList>        
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:gridview ID="gvComponents" Visible="false" runat="server" SkinID="GridView" AllowSorting="true" DataSourceID="SqlComponents" AutoGenerateColumns="true" DataKeyNames="componentID" OnRowUpdating="gvComponents_rowUpdating" EmptyDataText="No components found." OnRowUpdated="gvComponents_rowUpdated" OnRowEditing="gvComponents_rowEditing">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:BoundField HeaderText="Component" DataField="componentName" ReadOnly="true" SortExpression="componentName" />
                    <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                    <asp:BoundField HeaderText="Manufacturer" DataField="manufacturername" SortExpression="manufacturername" ReadOnly="true" />
                    <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                    <asp:CheckBoxField HeaderText="SaleItem" DataField="saleItem" SortExpression="saleItem" />
                    <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                    <asp:BoundField HeaderText="Price" DataField="price" ControlStyle-Width="40" />
                    <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                    <asp:BoundField HeaderText="Vat Rate" DataField="vatRate" ControlStyle-Width="40" />
                    <asp:BoundField ItemStyle-Width="20" ReadOnly="true" />
                    <asp:CommandField EditText="Edit" ShowEditButton="true" ShowCancelButton="true" />
                </Columns>
            </asp:gridview>
            <br /><br />
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="SqlComponents" runat="server" SelectCommand="procComponentsByMasterIDPriceSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procComponentsByMasterIDPriceUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMaster" PropertyName="selectedValue" Name="id" Type="string" Size="10" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="componentID" Type="int32" />
            <asp:Parameter Name="saleItem" Type="boolean" />
            <asp:Parameter Name="price" Type="decimal" />
            <asp:Parameter Name="vatRate" Type="decimal" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

