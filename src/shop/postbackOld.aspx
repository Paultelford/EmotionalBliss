<%@ Page Language="vb" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>ICEPAY Demo: Basic Tutorial</title>
</head>
<body>
<asp:Label ID="lblResult" runat="server"></asp:Label>
<script runat="server">
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ice As New ICEPAY.ICEPAY(10458, "g9P8Qcq7Y5Apa4E6Tsk3XFm7z6RHb4w8DMe5n9G3")
        If ice.OnPostBack Then
            Dim data As ICEPAY.ICEPAY.Postback = ice.GetPostback()
            Select Case data.status.ToUpper()
                Case "OK" ' Successful payment
                    siteInclude.debug("moot")
                Case "OPEN" ' Payment is not yet completed
                Case "ERR" ' Error happened
                Case "REFUND" ' Merchant did a refund
                Case "CBACK" ' Charge back by end-user
            End Select
        End If
    End Sub
</script>    
</body>
</html>