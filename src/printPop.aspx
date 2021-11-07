<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
</head>
<body onLoad="self.setTimeout('callData()',200);">
    <form id="form1" runat="server">
    <center>
        <input type="button" value="Print Page" onClick="printPage()" id="btnPrint">
    </center>
    <table>
        <tr>
            <td width="80">
                &nbsp;
            </td>
            <td>
                <div id="printDiv">
            
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
<script language="javascript">
function callData()
{
    window.opener.remoteCall();
}
function passData(html)
{
    document.getElementById("printDiv").innerHTML=html;
}
function printPage()
{
    document.getElementById("btnPrint").style.display="none";
    window.print();
}
</script>
