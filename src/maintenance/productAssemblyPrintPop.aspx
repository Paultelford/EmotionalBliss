<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">

</script>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <input type="button" id="btnPrint" value="Print" onclick="printMe()" />
    <div id="txt">
    
    </div>
    </form>
</body>
</html>
<script language="javascript">
self.setTimeout("parentCall()",200);
function parentCall()
{
    window.opener.remoteCall();
}
function receiveData(html)
{
    document.getElementById("txt").innerHTML=html;
}
function printMe()
{
    document.getElementById("btnPrint").style.display="none";
    window.print();
}
</script>
