<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="shopDepartments.aspx.vb" Inherits="maintenance_shopDepartments" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="lblHeader" runat="server">
        Existing shop departments
    </asp:Label>
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <table border="0" width="100%">
        <tr>
            <td valign="top">
                <asp:GridView ID="gvDepartments" runat="server" DataSourceID="sqlDepartments" AutoGenerateColumns="false" DataKeyNames="shopDeptID" SkinID="GridView" OnRowUpdated="gvDepartments_rowUpdated">
                    <Columns>
                        <asp:BoundField HeaderText="Dept Name" DataField="deptName" />
                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                        <asp:CommandField UpdateText="Edit" ShowEditButton="true" />
                    </Columns>
                </asp:GridView>        
                <asp:Label ID="lblAdd" runat="Server" Text="Add new Dept. " Visible="false"></asp:Label>
                <asp:TextBox ID="txtNewDept" runat="server" Width="100"></asp:TextBox><asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="btnAdd_click" />
            </td>
            <td valign="top" align="right">
                <asp:GridView ID="gvCountrys" runat="server" GridLines="none" DataSourceID="sqlCountrys" AutoGenerateColumns="false" DataKeyNames="countryCode" OnDataBound="gvCountrys_dataBound" ShowHeader="false" OnSelectedIndexChanged="gvCountrys_selectedIndexChanged">
                    <Columns>
                        <asp:BoundField DataField="countryName" ItemStyle-Font-Bold="true" ItemStyle-VerticalAlign="top" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:GridView ID="gvLink" runat="server" AutoGenerateColumns="false" DataKeyNames="shopDeptID" SkinID="GridView">
                                    <Columns>
                                        <asp:BoundField HeaderText="Department" DataField="deptName" />
                                        <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                        <asp:TemplateField HeaderText="Active">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkActive" runat="server" Checked='<%# isActive(Eval("shopDeptLinkID")) %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField SelectText="Save" ShowSelectButton="true" ItemStyle-VerticalAlign="top" />
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
            
    <asp:SqlDataSource ID="sqlDepartments" runat="server" SelectCommand="procShopDeptSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procShopDeptByIDUpdate" UpdateCommandType="StoredProcedure">
        <UpdateParameters>
            <asp:Parameter Name="shopDeptID" Type="int32" />
            <asp:Parameter Name="deptName" Type="string" Size="50" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCountrys" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

