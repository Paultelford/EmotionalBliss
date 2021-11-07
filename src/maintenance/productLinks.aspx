<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productLinks.aspx.vb" Inherits="maintenance_productLinks" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <a href='productLinks.aspx'>Refresh</a>
    <br /><br />
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td valign="top">
                        * Red font refers to products that are not linked with any other.
                        <asp:GridView ID="gvProducts" runat="server" DataSourceID="sqlProducts" DataKeyNames="id" AutoGenerateColumns="false" SkinID="GridViewRedBG" OnDataBound="gvProducts_dataBound" OnSelectedIndexChanged="gvProducts_selectedIndexChanged">
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblID" runat="server" Text='<%# Eval("id") %>'></asp:Label>&nbsp;
                                        <asp:Label ID="lblLinkID" runat="server" Text='<%# Eval("linkID") %>'></asp:Label>
                                        <asp:Label ID="lblCountryCode" runat="server" Text='<%# Eval("saleCountryCode") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Product" DataField="saleName" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Type" DataField="itemType" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:BoundField HeaderText="Country" DataField="countryName" />
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:TemplateField HeaderText="Group">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnTypeDelete" runat="Server" Text='<%# Eval("lnkName") %>' ToolTip="Click To Delete Link" CommandArgument='<%# Eval("linkID") %>' OnClick="btnTypeDelete_click"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCreate" runat="server" Text="Create Link" CommandName="select"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td valign="top" align="right">
                        <br />
                        <asp:Panel ID="panLinkTypes" runat="server" Visible="false">
                            <asp:GridView ID="gvLinkTypes" runat="server" DataSourceID="sqlLinkTypes" DataKeyNames="lnkTypeID" SkinID="GridView" Visible="true" Width="200" OnSelectedIndexChanged="gvLinkTypes_selectedIndexChanged">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select Link Type">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkTypeSelect" runat="server" Text='<%# Eval("lnkName") %>' CommandName="select"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <asp:TextBox ID="txtNewLinkType" runat="Server" MaxLength="50" ValidationGroup="new"></asp:TextBox>
                            <asp:Button ID="btnAddLinkType" runat="server" Text="Add" OnClick="btnAddLinkType_click" ValidationGroup="new"></asp:Button>
                            <br />
                            <asp:RequiredFieldValidator ID="reqTxtNewLinkType" runat="server" ControlToValidate="txtNewLinkType" ErrorMessage="* Required" ValidationGroup="new" Display="static"></asp:RequiredFieldValidator>
                            <asp:Label ID="lblNewError" runat="server"></asp:Label>
                            
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <br /><br /><br />
                        <asp:Panel ID="panDelete1" runat="Server" Visible="false">
                            Deleted old association for <br />
                            <b><asp:Label ID="lblProduct1" runat="server"></asp:Label></b><br /><br />
                        </asp:Panel>
                        <asp:Panel ID="panDelete2" runat="server" Visible="false">
                            Deleted old association for <br />
                            <b><asp:Label ID="lblProduct2" runat="server"></asp:Label></b><br /><br />
                        </asp:Panel>
                        <asp:Panel ID="panLink" runat="server" Visible="false">
                            Linked <b><asp:Label ID="lblProduct1b" runat="Server"></asp:Label></b><br />to <b><asp:Label ID="lblProduct2b" runat="server"></asp:Label></b>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblError" runat="Server"></asp:Label>    
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource id="sqlProducts" runat="server" SelectCommand="procProductOnSaleSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlLinkTypes" runat="server" SelectCommand="procProductOnSaleLinkTypeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

