<%@ Page Language="VB" AutoEventWireup="false" CodeFile="duplicateCountryData.aspx.vb" Inherits="affiliates_duplicateCountryData" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table>
            <tr>
                <td>
                    Source country:
                </td>
                <td>
                    <asp:DropDownList ID="drpSource" runat="server" DataTextField="countryName" DataValueField="countryCode" DataSourceID="sqlCountry"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    Destinateion Country:
                </td>
                <td>
                    <asp:DropDownList ID="drpDestination" runat="server" DataTextField="countryName" DataValueField="countryCode" DataSourceID="sqlCountry"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnGo" runat="server" Text="Duplicate" OnClick="btnGo_click" />
                </td>
            </tr>
        </table>
        Note: No data will be deleted from database
        <br /><br /><br />
        <asp:Label ID="lblOutput" runat="server"></asp:Label>
        <br /><br />
        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
        
        <asp:SqlDataSource ID="sqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
