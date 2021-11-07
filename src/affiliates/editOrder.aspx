<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="editOrder.aspx.vb" Inherits="affiliates_editOrder" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy id="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            Select Department:&nbsp;
            <asp:DropDownList ID="drpDepartment" runat="server" DataSourceID="sqlDepartment" DataTextField="menuType" DataValueField="menuType" AutoPostBack="true" OnSelectedIndexChanged="drpDepartment_selectedIndexChanged" AppendDataBoundItems="true">
                <asp:ListItem Text="Please choose...." Value="0"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            <table border="0">
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="gvPages" DataKeyNames="id" runat="server" GridLines="none" DataSourceID="sqlPages" AutoGenerateColumns="false" OnDataBinding="gvPages_dataBound">
                            <Columns>
                                <asp:BoundField HeaderText="Department" DataField="menuType" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Name" DataField="name" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:TemplateField HeaderText="Priority">
                                    <ItemTemplate>
                                        <asp:TextBox ID="txtPriority" runat="server" Width="40" Text='<%# Eval("priority") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="reqTxtPriority" runat="server" ControlToValidate="txtPriority" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                                        <asp:RangeValidator ID="ranTxtPriority" runat="server" ControlToValidate="txtPriority" Type="Integer" MinimumValue="1" MaximumValue="999" Display="dynamic" ErrorMessage="* Invalid value"></asp:RangeValidator>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>        
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Button ID="btnUpdate" runat="server" Text="Save Changes" OnClick="btnUpdate_click" Visible="false" />
                    </td>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <asp:Label ID="lblError" runat="server"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
            
    
    <asp:SqlDataSource ID="sqlDepartment" runat="server" SelectCommand="procSiteMenusByCountryCodeDistinctMenuTypeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    <SelectParameters>
        <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
    </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlPages" runat="server" SelectCommand="procSiteMenusByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpDepartment" PropertyName="selectedValue" Name="menuType" Type="string" Size="200" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:Parameter Name="showInEditor" Type="string" Size="1" DefaultValue="%" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

