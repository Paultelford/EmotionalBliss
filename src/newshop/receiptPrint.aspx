<%@ Page Language="VB" AutoEventWireup="false" CodeFile="receiptPrint.aspx.vb" Inherits="shop_receiptPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div id="dataDiv">
    
    </div>
    </form>
    <br /><br /><br />
    <center>
    <input type="button" value="Click To Print" onclick="this.style.visibility='hidden';window.print();" />
    </center>
</body>
</html>
<script language="Javascript" type="text/javascript">
self.setTimeout("getData()",200);
function getData()
{
    document.getElementById("dataDiv").innerHTML=window.opener.startDataTransfer();
}
</script>
