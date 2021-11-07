<%@ Page Language="VB" AutoEventWireup="false" CodeFile="iDeal.aspx.vb" Inherits="shop_iDeal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server" method="post">
    <div>
        <INPUT TYPE="text" NAME="orderID" id="orderID" runat="server">
        <INPUT TYPE="text" NAME="pspid" VALUE="EmotionalBliss">
        <INPUT TYPE="text" NAME="RL" VALUE="ncol_2.0">
        <INPUT TYPE="text" NAME="currency" VALUE="EUR">
        <!--<INPUT TYPE="text" NAME="language" VALUE="en_US">-->
        <INPUT TYPE="text" NAME="language" VALUE="nl_NL">
        <INPUT TYPE="text" NAME="amount" id="amount" runat="server">
        <!—optional fields-->
        <!--layout fields-->
        <!--dynamic template page-->
        <!--<INPUT TYPE="text" NAME="TP" VALUE="http://www.jkl.com/yyy/template9694.htm">-->
        <!--or static template page customisation -->
        <!--<INPUT TYPE="text" NAME="LOGO" VALUE="MyLogo.gif">-->
        <!--other optional fields -->
        <!--<INPUT TYPE="text" NAME="accepturl" VALUE="http://81.149.144.46:8060/ebshop/ideal_accepted.asp">
        <INPUT TYPE="text" NAME="declineurl" VALUE="http://81.149.144.46:8060/ebshop/ideal_declined.asp">
        <INPUT TYPE="text" NAME="exceptionurl" VALUE="http://81.149.144.46:8060/ebshop/ideal_exceptional.asp">
        <INPUT TYPE="text" NAME="cancelurl" VALUE="http://81.149.144.46:8060/ebshop/ideal_cancel.asp">-->
        <INPUT TYPE="text" NAME="SHASign" VALUE="The signature of your order">
        <INPUT TYPE="text" NAME="email" id="email" runat="server"> 
        <INPUT TYPE="text" NAME="COM" id="description" runat="server"> 
        <INPUT TYPE="text" NAME="CN" id="customerName" runat="server">
        <INPUT TYPE="text" NAME="catalogurl" VALUE="http://www.emotionalbliss.nl">
        <INPUT TYPE="text" NAME="homeurl" VALUE="http://www.emotionalbliss.nl">
        <!--optional payment method details-->
        <INPUT TYPE="text" NAME="PM" VALUE="iDEAL">
        <INPUT TYPE="text" NAME="BRAND" VALUE="VISA">
        <INPUT TYPE="text" NAME="ownerZIP" id="zip" runat="server">
        <INPUT TYPE="text" NAME="owneraddress" id="address" runat="server">
        <INPUT TYPE="text" NAME="complus" VALUE="123456789">
        <INPUT TYPE="text" NAME="paramplus" VALUE="sessionID=12345&shopperID=45678">
        <INPUT TYPE="text" NAME="USERID" VALUE="ABCDE"><br />
        <input type="submit" value="Submit" />
        
    </div>
    </form>
</body>
<script language="javascript" type="text/javascript">
self.setTimeout("prepareForm()",200);
function prepareForm()
{
    document.getElementById("form1").action="https://i-kassa.rabobank.nl/rik/test/orderstandard.asp";
}
</script>
</html>
