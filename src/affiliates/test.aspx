<%@ Page Language="VB" AutoEventWireup="false" CodeFile="test.aspx.vb" Inherits="affiliates_test" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvTest" runat="Server" DataSourceID="sqlTest"></asp:GridView>
        
        <asp:SqlDataSource ID="sqlTest" runat="server" SelectCommand="procCatalogueRequestByDaySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:Parameter Name="day" Type="datetime" DefaultValue="26 aug 2008" />
                <asp:Parameter Name="countryCode" Type="string" Size="5" DefaultValue="gb" />
                <asp:Parameter Name="type" Type="string" Size="10" DefaultValue="request" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
