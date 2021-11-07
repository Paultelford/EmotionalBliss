<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="affiliateClick.aspx.vb" Inherits="affiliates_affiliateClick" title="Untitled Page" %>
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
            <table border="0" width="100%">
                <tr>
                    <td valign="top">
                        <asp:GridView ID="gvAffs" runat="server" DataSourceID="sqlAffs" AutoGenerateColumns="false" DataKeyNames="affID" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvAffs_selectedIndexChanged">
                            <Columns>
                                <asp:BoundField HeaderText="Affiliate" DataField="affCompany" ReadOnly="true" />
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:TemplateField HeaderText="Contact">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAff" runat="server" Text='<%# getAffFullName(Eval("affFirstName"),Eval("affSurname")) %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:CommandField SelectText="Edit" ShowSelectButton="true" />
                            </Columns>
                        </asp:GridView>
                    </td>
                    <td valign="top" align="right">
                        <asp:Panel ID="panComponents" runat="server" BorderWidth="0" BorderColor="gray" Height="200" ScrollBars="vertical" HorizontalAlign="right" Visible="false">
                            <table>
                                <tr></tr>
                                    <td align="left">
                                        <table border="0" width="100%">
                                            <td>
                                                <asp:Label ID="lblErrorComponent" runat="server"></asp:Label>
                                                <asp:Label ID="lblComponentsText" runat="server" Text="Components" Font-Bold="true"></asp:Label>    
                                            </td>
                                            <td align="right">
                                                <asp:LinkButton ID="lnkSaveComponentList" runat="server" Text="Save Changes" OnClick="lnkSaveComponentList_click" ValidationGroup="comp"></asp:LinkButton>
                                                <asp:Label ID="lblSaveCompleteComponent" runat="server" Text="<font color='red'>Save Complete</font>" Visible="false"></asp:Label>
                                            </td>
                                        </table>
                                        <asp:GridView ID="gvComponent" runat="server" DataSourceID="SqlComp" AutoGenerateColumns="false" GridLines="none" Width="300" ShowHeader="false" SkinID="GridView" DataKeyNames="affProductBuyingID">
                                            <HeaderStyle Font-Bold="true" />
                                            <Columns>
                                                <asp:BoundField DataField="Name" />
                                                <asp:BoundField ItemStyle-Width="40" />
                                                <asp:TemplateField HeaderText="Percentage" ItemStyle-HorizontalAlign="right">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPercentage" runat="server" Text='<%# showPercentage(Eval("percentage")) %>' Width="35" Height="13"></asp:TextBox>%
                                                        <asp:RequiredFieldValidator ID="reqTxtPercentage" runat="server" ControlToValidate="txtPercentage" ErrorMessage="*" Display="dynamic" ValidationGroup="comp"></asp:RequiredFieldValidator><asp:RangeValidator ID="ranTxtPercentage" runat="server" ControlToValidate="txtPercentage" ErrorMessage="*" Display="Dynamic" ValidationGroup="comp" Type="Double" MinimumValue="0" MaximumValue="100"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>    
                                    </td>
                                </tr>
                            </table>                            
                        </asp:Panel> 
                        <br /><br />
                        <asp:Panel ID="panBProducts" runat="server" BorderWidth="0" BorderColor="gray" Height="200" ScrollBars="vertical" HorizontalAlign="right" Visible="false">
                            <table>
                                <tr>
                                    <td align="left">
                                        <table border="0" width="100%">
                                            <td>
                                                <asp:Label ID="lblErrorProduct" runat="server"></asp:Label>
                                                <asp:Label ID="lblBProductsText" runat="server" Text="Boxed Products" Font-Bold="true"></asp:Label>    
                                            </td>
                                            <td align="right">
                                                <asp:LinkButton ID="lnkSaveProductList" runat="server" Text="Save Changes" OnClick="lnkSaveProductList_click" ValidationGroup="prod"></asp:LinkButton>
                                                <asp:Label ID="lblSaveCompleteProduct" runat="server" Text="<font color='red'>Save Complete</font>" Visible="false"></asp:Label>
                                            </td>
                                        </table>
                                        <asp:GridView ID="gvProduct" runat="server" DataSourceID="SqlBProd" AutoGenerateColumns="false" ShowHeader="false" width="300" GridLines="none" SkinID="GridView" DataKeyNames="affProductBuyingID">
                                            <Columns>
                                                <asp:BoundField DataField="Name" />
                                                <asp:BoundField ItemStyle-Width="40" />
                                                <asp:TemplateField HeaderText="Percentage" ItemStyle-HorizontalAlign="right">
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="txtPercentage" runat="server" Text='<%# showPercentage(Eval("percentage")) %>' Width="35" Height="13"></asp:TextBox>%
                                                        <asp:RequiredFieldValidator ID="reqTxtPercentage" runat="server" ControlToValidate="txtPercentage" ErrorMessage="*" Display="dynamic" ValidationGroup="prod"></asp:RequiredFieldValidator><asp:RangeValidator ID="ranTxtPercentage" runat="server" ControlToValidate="txtPercentage" ErrorMessage="*" Display="Dynamic" ValidationGroup="prod" Type="Double" MinimumValue="0" MaximumValue="100"></asp:RangeValidator>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </atlas:UpdatePanel>            
        
    
    <asp:SqlDataSource ID="sqlAffs" runat="Server" SelectCommand="procAffiliatesByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procAffiliateByAffIDClickThroughUpdate" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter Name="countryCode" Type="string" Size="5" SessionField="EBAffEBDistributorCountryCode" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlComp" runat="server" SelectCommand="procAffiliateProductBuyingClickThroughByAffIDDistIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvAffs" PropertyName="selectedValue" Name="affID" Type="int32" />
            <asp:SessionParameter SessionField="EBAffID" Name="distID" Type="int32" />
            <asp:Parameter Name="productType" DefaultValue="component" type="string" size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlBProd" runat="server" SelectCommand="procAffiliateProductBuyingClickThroughByAffIDDistIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvAffs" PropertyName="selectedValue" Name="affID" Type="int32" />
            <asp:SessionParameter SessionField="EBAffID" Name="distID" Type="int32" />
            <asp:Parameter Name="productType" DefaultValue="bproduct" type="string" size="10" />
        </SelectParameters>
    </asp:SqlDataSource> 
</asp:Content>

