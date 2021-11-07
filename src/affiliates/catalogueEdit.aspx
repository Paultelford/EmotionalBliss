<%@ Page Language="VB" AutoEventWireup="false" CodeFile="catalogueEdit.aspx.vb" Inherits="affiliates_catalogueEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Catalogue Edit Address</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DetailsView ID="dvAddress" runat="server" DataSourceID="sqlAddress" DataKeyNames="id" DefaultMode="Edit" AutoGenerateRows="false" OnItemCommand="dvAddress_itemCommand" OnItemUpdated="dvAddress_itemUpdated">
            <Fields>
                <asp:BoundField HeaderText="Name" DataField="name" />
                <asp:BoundField HeaderText="Add1" DataField="add1" />
                <asp:BoundField HeaderText="Add2" DataField="add2" />
                <asp:BoundField HeaderText="Add3" DataField="add3" />
                <asp:BoundField HeaderText="Add4" DataField="add4" />
                <asp:BoundField HeaderText="Add5" DataField="add5" />
                <asp:BoundField HeaderText="Postcode" DataField="postcode" />
                <asp:CommandField UpdateText="Save Changes" ShowEditButton="true" ButtonType="Button" ShowCancelButton="true" />
            </Fields>
        </asp:DetailsView>
    </div>
    </form>
    
    
    <asp:SqlDataSource ID="sqlAddress" runat="server" SelectCommand="procCatalogueRequestByIDSelect" SelectCommandType="StoredProcedure" UpdateCommand="procCatalogueRequestByIDAddUpdate" UpdateCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="id" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="id" Type="int32" />
            <asp:Parameter Name="name" Type="string" Size="50" />
            <asp:Parameter Name="add1" Type="string" Size="50" />
            <asp:Parameter Name="add2" Type="string" Size="50" />
            <asp:Parameter Name="add3" Type="string" Size="50" />
            <asp:Parameter Name="add4" Type="string" Size="50" />
            <asp:Parameter Name="add5" Type="string" Size="50" />
            <asp:Parameter Name="postcode" Type="string" Size="10" />
        </UpdateParameters>
    </asp:SqlDataSource>
</body>
</html>
