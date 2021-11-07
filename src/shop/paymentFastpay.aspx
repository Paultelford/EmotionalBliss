<%@ Page Language="VB" AutoEventWireup="false" CodeFile="paymentFastpay.aspx.vb" Inherits="shop_paymentFastpay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <input type="hidden" name="FPIDENTIFY" value="<%=Session("EBTmp_FPIDENTITY") %>"> <br />
        <input type="hidden" name="FPPRICE" value="<%=Session("EBTmp_FPPRICE") %>"> <br />
        <input type="hidden" name="FPNAME" value="<%=Session("EBTmp_FPNAME") %>"> <br />
        <input type="hidden" name="FPREUSE" value="0"> <br />
        <input type="hidden" name="FPMERCHANT" value="55697"> <br />
        <input type="hidden" name="FPCOMPANYNAME" value="Pear Tree UK"> <br />
        <input type="submit" value="If you are not automatically redirected, then click here to continue to FasterPay" />
    </form>
</body>
</html>
