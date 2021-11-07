<%@ Page Language="VB" AutoEventWireup="false" ValidateRequest="false" CodeFile="bulkEmail.aspx.vb" Inherits="maintenance_bulkEmail" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <a href='bulkEmail.aspx'>REFRESH</a>
        <center>
            <table border="1" width="80%" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left" colspan="2">
                        Subject: <asp:TextBox ID="txtSubject" Width="400" runat="server">New 'Heat' Range From Emotional Bliss</asp:TextBox> <asp:RequiredFieldValidator id="reqTxtSubject" runat="Server" ControlToValidate="txtSubject" ErrorMessage="* Required"></asp:RequiredFieldValidator><br />
<FCKeditorV2:FCKeditor id="FCKeditor1" runat="server" BasePath="~/EBEditor/" Width="100%" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="300"
value="Dear Emotional Bliss customer,<br>
<br>
As an existing Emotional Bliss customer you have been given priority to visit the new Emotional Bliss shop so you can view the New “HEAT” range of products. You can purchase the new “HEAT” massager at a reduced price as you may already have an Emotional Bliss adaptor.
<br><br>
Without your support the new “HEAT” range of Intimate massagers would not have been possible.
<br><br>
Please click on the following link http://heat.emotionalbliss.co.uk or on the picture below.<br><br>
Thank you from us all at Emotional Bliss.<br><br>
Yours faithfully<br>
Paul Telford<br>
Director<br>"
>
</FCKeditorV2:FCKeditor><br />
                        <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/400.jpg" />                    
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Button ID="btnSend" runat="server" OnClick="btnSend_click" />
                    </td>
                    <td align="right">
                        Total emails: <asp:Label ID="lblTotalEmails" runat="server"></asp:Label>,&nbsp;
                        Sent: <asp:Label ID="lblSentEmails" runat="server"></asp:Label>,&nbsp;
                        Left: <asp:Label ID="lblLeftEmails" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Table ID="tblResults" runat="server">
                        </asp:Table>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
