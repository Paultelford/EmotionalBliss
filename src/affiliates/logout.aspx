<%@ Page Language="VB" AutoEventWireup="false" CodeFile="logout.aspx.vb" Inherits="affiliates_logout" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Logout</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        You are being logged out. Please wait
    </div>
    </form>
    <script language="javascript" type="text/javascript">
    self.setTimeout("document.location='default.aspx';",1000);
    </script>
</body>
</html>
