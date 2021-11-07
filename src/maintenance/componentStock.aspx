<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentStock.aspx.vb" Inherits="maintenance_componentStockView" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
            Master Type:
            <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMasterTypes" DataTextField="name" DataValueField="masterid" AppendDataBoundItems="true" EnableViewState="true" AutoPostBack="true" OnSelectedIndexChanged="drpMaster_dataBound">
                <asp:ListItem Text="Select Master..." Value="0"></asp:ListItem> 
                <asp:ListItem Text="All" Value="%"></asp:ListItem> 
            </asp:DropDownList>
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            Manufacturer:    
            <asp:DropDownList ID="drpMan" runat="server" DataSourceID="sqlMan" DataTextField="manufacturerName" DataValueField="manufacturerID" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="drpMan_dataBound">
                <asp:ListItem Text="All" Value="%"></asp:ListItem> 
            </asp:DropDownList>
            <br /><br />
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:GridView ID="gvComponents" runat="server" DataSourceID="sqlComponents" DataKeyNames="componentID" Visible="false" GridLines="Vertical" AutoGenerateColumns="false" EmptyDataText="No components found." OnRowDataBound="gvComponents_rowDataBound" OnRowUpdating="gvComponents_rowUpdating" OnDataBound="gvComponents_dataBound">
                            <HeaderStyle Font-Bold="true" Wrap="false" />
                            <RowStyle VerticalAlign="top" />
                            <Columns>           
                                <asp:HyperLinkField HeaderText="Name" DataTextField="componentName" DataNavigateUrlFields="componentID" DataNavigateUrlFormatString="componentHistoryView.aspx?cid={0}&type=component" />
                                <asp:BoundField HeaderText="Code" DataField="componentCode" />
                                <asp:BoundField HeaderText="Description" DataField="componentDescription" ReadOnly="true" />
                                <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Stock">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStock" runat="server" Text='<%# show(Eval("componentStock")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtStock" runat="server" Width="40" Text='<%#Bind("componentStock") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="InProduction">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProduction" runat="server" Text='<%# show(Eval("productionStock")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Quarantine">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuarantine" runat="server" Text='<%# show(Eval("quarantineStock")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtQuarantine" runat="server" Width="40" Text='<%#Bind("quarantineStock") %>'></asp:TextBox>
                                    </EditItemTemplate>
                                </asp:TemplateField>                                
                                <asp:BoundField Visible="false" HeaderText="Total" DataField="totalStock" NullDisplayText="0" ReadOnly="true" />
                                <asp:BoundField HeaderText="OnOrder" />
                                <asp:TemplateField HeaderText="ReOrder">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReorder" runat="server" Text='<%# show(Eval("reorderLevel")) %>'></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblReorder" runat="server" Text='<%# Eval("reorderLevel") %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink Text="Adjust" ID="lnkAdjust" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td>
                        <asp:GridView ID="gvOutstanding" runat="server" DataSourceID="SqlOutstanding" Visible="true" GridLines="none" AutoGenerateColumns="false" OnRowDataBound="gvOutstanding_rowDataBound" OnDataBound="gvOutstanding_dataBound">
                            <HeaderStyle Font-Bold="true" />
                            <RowStyle VerticalAlign="top" />
                            <Columns>
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOutstanding" runat="server" Text='<%# showOnOrder(Eval("onorder")) %>' Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>                            
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Label ID="lblError" runat="server"></asp:Label>
        
    
              
    <asp:SqlDataSource ID="sqlMasterTypes" runat="server" SelectCommand="procComponentMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlMan" runat="server" SelectCommand="procManufacturersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlComponents" runat="server" SelectCommand="procComponentsByMasterIDStockSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMaster" Name="masterID" PropertyName="selectedValue" Type="String" Size="10" />
            <asp:ControlParameter ControlID="drpMan" Name="manID" PropertyName="selectedValue" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlOutstanding" runat="server" SelectCommand="procComponentsByMasterIDOnOrderSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMaster" Name="masterID" PropertyName="selectedValue" Type="String" Size="10" />
            <asp:ControlParameter ControlID="drpMan" Name="manID" PropertyName="selectedValue" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>


