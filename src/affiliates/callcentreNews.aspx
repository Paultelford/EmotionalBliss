<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="callcentreNews.aspx.vb" Inherits="affiliates_callcentreNews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:DetailsView ID="dvArticle" runat="server" DefaultMode="Insert" DataSourceID="sqlArticle" GridLines="None" DataKeyNames="newsID" AutoGenerateRows="false" OnItemInserted="dvArticle_itemInserted" OnItemUpdated="dvArticle_itemUpdated">
        <Fields>
            <asp:TemplateField HeaderText="Headline" HeaderStyle-Width="100" ItemStyle-Height="30">
                <ItemTemplate>
                    <asp:TextBox ID="txtHeadline" runat="server" MaxLength="200" Width="400" Text='<%# Bind("headline") %>'></asp:TextBox><asp:RequiredFieldValidator ID="reqHeadline" runat="server" ControlToValidate="txtHeadline" ErrorMessage="* Required" Display="Dynamic" ValidationGroup="art"></asp:RequiredFieldValidator>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Message" ItemStyle-VerticalAlign="Top" HeaderStyle-VerticalAlign="Top">
                <ItemTemplate>
                    <asp:TextBox ID="txtText" runat="server" Width="600" Height="300" CssClass="normaltextarea" TextMode="MultiLine" Text='<%# Bind("text") %>'></asp:TextBox><asp:RequiredFieldValidator ID="redText" runat="server" ControlToValidate="txtText" ErrorMessage="* Required" Display="Dynamic" ValidationGroup="art"></asp:RequiredFieldValidator>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField HeaderText="Active" DataField="active" ItemStyle-Height="30"></asp:CheckBoxField>
            <asp:TemplateField>
                <InsertItemTemplate>
                    <asp:Button ID="btnInsert" runat="server" CommandName="insert" Text="Add" ValidationGroup="art" />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:Button ID="btnUpdate" runat="server" CommandName="update" Text="Update" ValidationGroup="art" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>
    </asp:DetailsView>
    <br /><br />
    <asp:GridView ID="gvNews" runat="server" DataKeyNames="newsID" DataSourceID="sqlNews" GridLines="None" AutoGenerateColumns="false" Width="100%" HeaderStyle-VerticalAlign="Top" OnSelectedIndexChanged="gvNews_selectedIndexChanged">
        <Columns>
            <asp:TemplateField HeaderText="Headline">
                <ItemTemplate>
                    <asp:LinkButton ID="lnkHeadline" runat="server" Text='<%# Eval("headline") %>' CommandName="select"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="40" />
            <asp:BoundField HeaderText="Date Added" DataField="dateadded" DataFormatString="{0:dd MMM yyyy}" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:BoundField HeaderText="URL" DataField="url" Visible="false" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:CheckBoxField HeaderText="Active" DataField="active" />
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="sqlNews" runat="server" SelectCommand="procNewsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlArticle" runat="server" SelectCommand="procNewsByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" InsertCommand="procNewsInsert2" InsertCommandType="StoredProcedure" UpdateCommand="procNewsByIDUpdate2" UpdateCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvNews" PropertyName="selectedValue" Name="newsID" Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="headline" Type="String" Size="200" />
            <asp:Parameter Name="text" Type="String" Size="-1" />
            <asp:Parameter Name="active" Type="Boolean" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="newsID" Type="Int32" />
            <asp:Parameter Name="headline" Type="String" Size="200" />
            <asp:Parameter Name="text" Type="String" Size="-1" />
            <asp:Parameter Name="active" Type="Boolean" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

