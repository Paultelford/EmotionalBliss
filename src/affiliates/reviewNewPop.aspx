<%@ Page Language="VB" AutoEventWireup="false" CodeFile="reviewNewPop.aspx.vb" Inherits="affiliates_reviewNewPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DetailsView runat="server" ID="dvReview" DataSourceID="sqlReview" AutoGenerateRows="false" GridLines="None" Width="600">
            <FieldHeaderStyle Width="160" />
            <RowStyle Width="500" />
            <Fields>
                <asp:TemplateField HeaderText="Product">
                    <ItemTemplate>
                        <asp:Label ID="lblProduct" runat="server" Text='<%# getProductName(Eval("product")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Date Added">
                    <ItemTemplate>
                        <asp:Label ID="lblDate" runat="Server" Text='<%# showDate(Eval("dateAdded")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Rating" DataField="score" />
                <asp:BoundField HeaderText="Name" DataField="name" />
                <asp:TemplateField HeaderText="User Comments">
                    <ItemTemplate>
                        <asp:label ID="lblUserComment" runat="Server" Text='<%# eval("review") %>'></asp:label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
    </div>
    
    <asp:SqlDataSource ID="sqlReview" runat="server" SelectCommand="procReviewNewByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="reviewID" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
