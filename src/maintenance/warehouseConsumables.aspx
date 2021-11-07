<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Title="Untitled Page" CodeFile="warehouseConsumables.aspx.vb" Inherits="maintenance_warehouseConsumables" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvComponents" runat="server" SkinID="GridView" DataKeyNames="componentID" AutoGenerateColumns="false" GridLines="none" DataSourceID="SqlComponents" OnRowEditing="gvComponents_rowEditing" OnRowCancelingEdit="gvComponents_rowEditCancel" OnRowUpdating="gvComponents_rowUpdating">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:BoundField HeaderText="Component" DataField="componentName" ReadOnly="true" />
                    <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" ReadOnly="true" />
                    <asp:BoundField HeaderText="Stock" DataField="stock" ReadOnly="true" NullDisplayText="0" />
                    <asp:TemplateField HeaderText="Qty Used" Visible="false">
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQtyUsed" runat="server" Width="40"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtQtyUsed" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator><asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtQtyUsed" MinimumValue="1" MaximumValue="99999" ErrorMessage="Invalid Qty" Display="Dynamic"></asp:RangeValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Link" EditText="Adjust" ShowEditButton="true" />
                </Columns>    
            </asp:GridView>
            <br />
            <asp:Panel ID="pan1" runat="server" Visible="false">
                <table border="0">
                    <tr>
                        <td valign="top">
                            <b>Reason:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtReason" runat="server" TextMode="MultiLine" MaxLength="1000" Rows="4" Columns="40"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:label ID="lblError" runat="server"></asp:label>
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="SqlComponents" runat="server" SelectCommand="procComponentsByWarehouseSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    
</asp:Content>

