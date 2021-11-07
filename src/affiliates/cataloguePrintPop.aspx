<%@ Page Language="VB" AutoEventWireup="false" CodeFile="cataloguePrintPop.aspx.vb" Inherits="affiliates_cataloguePrintPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Catalogue Requests</title>
    <style>
    td{font-size:13pt;}
    </style>
</head>
<body leftmargin="100">
<center>
    <form id="form1" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0" width="800">
            <tr>
                <td>
                    <br /> 
                    <asp:Table ID="tbl" runat="server" CellPadding="15" BorderWidth="0" GridLines="none" CellSpacing="6" Width="100%">
                    </asp:Table>   
                    <br /><br /><br /><br /><br />     
                </td>
            </tr>
        </table>
        <br /><br /><br /><br />
        <asp:Button ID="btnSend" runat="server" Text="Set all requests on screen as 'Sent'" OnClick="btnSend_click" />
        <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
        <asp:HiddenField ID="hidIDs" runat="server" />
    </div>
    </form>
</center>
<script language="javascript" type="text/javascript">
function editAddress(id)
{
    document.location="catalogueEdit.aspx?id=" + id;
}
</script>
</body>
</html>
