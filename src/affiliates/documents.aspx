<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="documents.aspx.vb" Inherits="affiliates_documents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />Right click on a document name and choose Save Target As to download.<br /><br />
    <asp:GridView ID="gvDocs" runat="server" DataSourceID="sqlDocs" AutoGenerateColumns="false" EmptyDataText="There are currently no documents available" GridLines="None">
        <Columns>
            <asp:HyperLinkField HeaderText="Document Name" DataTextField="filename" DataNavigateUrlFields="filename" DataNavigateUrlFormatString="~/uploads/documents/{0}" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:BoundField HeaderText="Description" DataField="description" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:BoundField HeaderText="Update Date" DataField="uploadDate" DataFormatString="{0:dd MMM yyyy}" />
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="sqlDocs" runat="server" SelectCommand="procDocumentsByTypeIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffTypeID" Name="affTypeID" Type="Int32" />
            <asp:SessionParameter SessionField="EBAffCountryCode" Name="countryCode" Type="String" Size="5" />
        </SelectParameters>    
    </asp:SqlDataSource>
</asp:Content>

