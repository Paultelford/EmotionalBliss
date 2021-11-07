<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productDefault.aspx.vb" Inherits="maintenance_productDefault" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <asp:Table ID="tblInfo" runat="server">
        <asp:TableRow>
            <asp:TableCell>
                <b>Product:</b>
            </asp:TableCell>
            <asp:TableCell>
                <asp:Label ID="lblProduct" runat="server"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <asp:HyperLink ID="lnkBack" runat="server" Text="Back to Design"></asp:HyperLink>
    <br /><br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvComponents" runat="server" DataSourceID="sqlComponents" GridLines="None" Width="100%" AutoGenerateColumns="false" OnRowDataBound="gvComponents_dataBound" OnRowEditing="gvComponents_rowEdititng" ShowFooter="true">
                <HeaderStyle Font-Bold="true" />
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblID" runat="server" Text='<%#Bind("compMasterID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Master Component" DataField="masterName" />
                    <asp:BoundField HeaderText="Qty" DataField="qty" />
                    <asp:BoundField HeaderText="Default Component" DataField="defaultComponent" NullDisplayText="<font color='red'>Not Set</font>" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:DropDownList ID="drpComponents" runat="server" AppendDataBoundItems="true" DataTextField="componentName" DataValueField="componentID">
                                <asp:ListItem Text="Select.." Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnUpdateAll" runat="server" Text="Update All" OnClick="btnUpdateAll_click" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="link" ShowEditButton="true" EditText="Update" />
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblProdCompID" runat="server" Text='<%#Bind("prodCompID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
        
    <asp:SqlDataSource ID="sqlComponents" runat="server" SelectCommand="procProductComponentByProdIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procProductComponentByIDUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:QueryStringParameter Name="prodID" QueryStringField="pid" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="prodCompID" Type="int16" />
            <asp:Parameter Name="qty" Type="int16" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

