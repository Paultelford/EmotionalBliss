<%@ Page Language="VB" AutoEventWireup="false" CodeFile="returnsOutstandingPop.aspx.vb" Inherits="maintenance_returnsOutstandingPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <center>
                <asp:Button ID="btnPrint" runat="server" Text="Print" OnClick="btnPrint_click" OnClientClick="printscr()" />
            </center>
            <br /><br />
            <table border="0">
                <tr>
                    <td>
                        <b>Returns ID:</b>
                    </td>
                    <td width="80">&nbsp;</td>
                    <td>
                        <asp:Label ID="lblReturnsID" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Date:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Label ID="lblDate" runat="server" Text="8 November 2007"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Items Returned:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:GridView ID="gvList" runat="server" GridLines="none" DataSourceID="sqlMasters" RowStyle-Height="22" AutoGenerateColumns="false" AlternatingRowStyle-BackColor="#eeeeee">
                            <Columns>
                                <asp:BoundField DataField="name" ItemStyle-Font-Bold="false" />
                                <asp:BoundField ItemStyle-Width="140" />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        __________
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><br /><br /><br /></td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Comments / Report:</b>
                    </td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:TextBox ID="txtReport" runat="server" TextMode="multiLine" Rows="20" Columns="70"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="3"><br /><br /><br /><br /></td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        <b>Signed by</b>  ___________________________ &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>Date</b>  ___________________________
                    </td>
                </tr>
            </table>
            
        </div>
        
        <asp:SqlDataSource ID="SqlMasters" runat="server" SelectCommand="procProductMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        </asp:SqlDataSource>
    </form>
</body>
<script language="javascript" type="text/javascript">
    function printscr(){
        document.getElementById("btnPrint").style.display="none";
        window.print();   
    }
</script>
</html>
