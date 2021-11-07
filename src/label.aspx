<%@ Page Language="VB" AutoEventWireup="false" CodeFile="label.aspx.vb" Inherits="labels" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style>
        body
        {
            	
        }
    </style>
    <script language="javascript" type="text/javascript">
        function openPopup() {
            window.open("labelPop.aspx?num=" + document.getElementById("num").value);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="panEntry" runat="server" Visible="true">
            Number: <input type="text" name="num" id="num" maxlength="8" />
            <input type="button" value="Submit" onclick="openPopup()" />
            <br />
        </asp:Panel>
    </div>
    </form>
</body>
</html>
