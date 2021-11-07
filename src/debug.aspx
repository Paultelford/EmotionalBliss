<%@ Page Language="VB" AutoEventWireup="false" CodeFile="debug.aspx.vb" Inherits="debug" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:GridView ID="gvDebugLog" runat="server" DataSourceID="sqlDebug" AutoGenerateColumns="true" EmptyDataText="No debug data found">
            <Columns>
            
            </Columns>
        </asp:GridView>
        <br />
        <asp:Button ID="btnClear" runat="server" Text="Clear Data" OnClick="btnClear_click" />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:HyperLink ID="lnkRefresh" runat="server" Text="Refresh Data" NavigateUrl="~/debug.aspx"></asp:HyperLink>
        <asp:SqlDataSource ID="sqlDebug" runat="server" SelectCommand="procDebugSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
