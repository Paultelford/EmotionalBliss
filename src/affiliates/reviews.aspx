<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" Theme="WinXP_Blue" CodeFile="reviews.aspx.vb" Inherits="affiliates_reviews" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>   
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <asp:UpdatePanel runat="server" ID="update1">
        <ContentTemplate>
            <asp:Panel ID="panProductSelect" runat="server">
                <table width="100%">
                    <tr>
                        <td width="30%">
                            Select a product:&nbsp;
                            <asp:DropDownList ID="drpProducts" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBound="drpProducts_dataBound">
                                <asp:ListItem Text="All" Value="%"></asp:ListItem>
                                <asp:ListItem Text="Chandra" Value="Chandra"></asp:ListItem>
                                <asp:ListItem Text="Femblossom" Value="Femblossom"></asp:ListItem>
                                <asp:ListItem Text="Isis" Value="Isis"></asp:ListItem>
                                <asp:ListItem Text="Womolia" Value="Womolia"></asp:ListItem>
                            </asp:DropDownList>&nbsp;&nbsp;        
                        </td>
                        <td width="30%">
                            Language:&nbsp;
                            <asp:DropDownList runat="server" ID="drpLanguage" AutoPostBack="True">
                                <asp:ListItem Text="All" Value="%"></asp:ListItem>
                                <asp:ListItem Text="GB" Value="gb"></asp:ListItem>
                                <asp:ListItem Text="NL" Value="nl"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </td>
                        <td>
                            View:&nbsp;
                            <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpType_selectedIndexChanged">
                                <asp:ListItem Text="All" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                
                <br />
                Click 'View Details' to see the whole review.
            </asp:Panel>
            <asp:Label ID="lblNoReviewsText" runat="server"></asp:Label>            
            <br /><br />
            <asp:GridView ID="gvReviews" runat="Server" DataSourceID="sqlReviewNew" DataKeyNames="reviewID" AutoGenerateColumns="false" SkinID="GridView" OnRowEditing="gvReviews_editing" OnRowDeleting="gvReviews_deleting" Width="100%" AllowPaging="true" PageSize="20" PagerSettings-Visible="false">
                <Columns>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label ID="lblProduct" runat="server" Text='<%# getProductName(Eval("product")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:TemplateField HeaderText="Date Added">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="Server" Text='<%# showDate(Eval("dateAdded")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Rating" DataField="score" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:TemplateField HeaderText="User Comments">
                        <ItemTemplate>
                            <asp:label ID="lblUserComment" runat="Server" Text='<%# showSmallReview(eval("review"),eval("reviewID")) %>'></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:CheckBoxField HeaderText="Active" DataField="active" ItemStyle-HorizontalAlign="Center" />
                    <asp:CommandField EditText="De/Activate" Visible="true" ShowDeleteButton="false" ShowCancelButton="false" ShowEditButton="true" />
                    <asp:TemplateField Visible="False">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="delete" ForeColor="black" Text="Un/Delete" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <eb:Pager id="pager1" runat="server" showTextBox="true"></eb:Pager>
        </ContentTemplate>
    </asp:UpdatePanel>       
    
    <asp:SqlDataSource ID="sqlProducts" runat="Server" SelectCommand="procProductsByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffID" Name="affID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlReviewProducts" runat="server" SelectCommand="procProductOnSaleByCountryCodeReviewSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlReviews" runat="server" SelectCommand="procReviewsByPosIDTypeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="sqlReviews_selecting">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpProducts" PropertyName="selectedValue" Name="posID" Type="string" Size="5" />
            <asp:Parameter Name="active" Type="string" Size="1" />
            <asp:Parameter Name="deleted" Type="string" Size="1" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="String" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource runat="server" ID="sqlReviewNew" SelectCommand="procReviewNewByProductActiveSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
          <SelectParameters>
            <asp:ControlParameter ControlID="drpProducts" PropertyName="selectedValue" Name="product" Type="string" Size="20" />
            <asp:ControlParameter ControlID="drpLanguage" PropertyName="selectedValue" Name="language" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpType" Name="active" Type="Int16" />
          </SelectParameters>
    </asp:SqlDataSource>
    
    <script language="javascript" type="text/javascript">
        function showReviewPopup(rID)
        {
            var win=window.open("reviewNewPop.aspx?id="+rID,"reviewpop","height=576,width=980,scrollbars=1,resizable=1");
        }
    </script>
</asp:Content>

