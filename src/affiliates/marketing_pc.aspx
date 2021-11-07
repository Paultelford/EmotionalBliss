<%@ Page Language="VB" AutoEventWireup="false" CodeFile="marketing_pc.aspx.vb" Inherits="affiliates_marketing_pc" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script language="javascript" type="text/javascript">
        function passPostcode(p) {
            window.opener.receivePostcode(p);
            window.close();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table border="0">
            <tr>
                <td>
                    All complete orders as of <%=Now()%>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="gvPoscode" runat="server" DataSourceID="sqlPostcode" AutoGenerateColumns="false">
                        <Columns>
                            <asp:TemplateField HeaderText="Postcode">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkPostcode" runat="server" Text='<%# Eval("pc") %>' OnClick="lnkPostcode_click" CommandArgument='<%# Eval("pc") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField ItemStyle-Width="20" />
                            <asp:BoundField HeaderText="Number of orders" DataField="items" />
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
        
        
        <asp:SqlDataSource ID="sqlPostcode" runat="server" SelectCommand="procShopCustomerPostcodeByCountryCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="String" Size="5" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
