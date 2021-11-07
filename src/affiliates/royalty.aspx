<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="royalty.aspx.vb" Inherits="affiliates_royalty" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Current royalty earners<br /><br />
    <atlas:ScriptManagerProxy id="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="gvEarners" runat="server" DataSourceID="sqlEarners" AutoGenerateColumns="false" DataKeyNames="affID" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvEarners_selectedIndexChanged" EmptyDataText="No earners found.">
                <Columns>
                    <asp:HyperLinkField HeaderText="Company" DataTextField="affCompany" DataNavigateUrlFormatString="~/affiliates/royaltyTrans.aspx?aid={0}" DataNavigateUrlFields="affID" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("affFirstName") & " " & Eval("affSurname") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:CommandField SelectText="Edit Amounts" ShowSelectButton="true" ShowCancelButton="false" ItemStyle-ForeColor="blue" />
                </Columns>
            </asp:GridView>
            <br /><br />
            <asp:Label ID="lblError" runat="server"></asp:Label><br />
            <asp:Label ID="lblComplete" runat="server" />
            <asp:GridView ID="gvAmounts" runat="server" DataSourceID="sqlAmounts" AutoGenerateColumns="false" SkinID="GridView" ShowFooter="true">
                <Columns>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("saleName") %>'></asp:Label>
                            <asp:Label ID="lblDistBuyingID" runat="server" Text='<%# Eval("distBuyingID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Button ID="btnSubmit" runat="server" Text="Save Changes" OnClick="btnSubmit_click" ValidationGroup="amount" />
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:TemplateField HeaderText="Earning(Per product)">
                        <ItemTemplate>
                            <asp:TextBox ID="txtEarning" runat="Server" MaxLength="5" Width="40" Text='<%# showAmount(Eval("amount")) %>' ValidationGroup="amount"></asp:TextBox>%
                            <asp:RangeValidator ID="ranTxtEarning" runat="server" ControlToValidate="txtEarning" Type="Double" MinimumValue="0" MaximumValue="99.99" ValidationGroup="amount" ErrorMessage="* Invalid"></asp:RangeValidator>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="sqlEarners" runat="server" SelectCommand="procAffiliateByCountryCodeRoyaltySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlAmounts" runat="server" SelectCommand="procRoyaltyEarningsByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvEarners" PropertyName="selectedValue" Name="affID" Type="int32" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

