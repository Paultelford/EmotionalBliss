<%@ Page Language="VB" AutoEventWireup="false" CodeFile="productxml.aspx.vb" Inherits="productxml" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        
        <table>
            <tr runat="server" visible="false">
                <td>
                    Create Google List for:
                </td>
                <td>
                    <asp:DropDownList ID="drp" runat="server">
                        <asp:ListItem Text="Active Products" Value="1"></asp:ListItem>
                        <asp:ListItem Text="InActive Products" Value="0"></asp:ListItem>
                        <asp:ListItem Text="All Products" Value="%"></asp:ListItem>
                    </asp:DropDownList>
                </td>    
            </tr>
            <tr>
                <td>
                    Filename:
                </td>
                <td>
                    <asp:TextBox ID="txtFilename" runat="server" Text="emotionalbliss_googledata.xml" Width="220"></asp:TextBox><asp:RequiredFieldValidator ID="reqFilename" runat="server" ControlToValidate="txtFilename" ErrorMessage="* Required" Display="Static"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td>
                    <asp:Button ID="btnCreate" runat="server" Text="Generate File" OnClick="btnCreate_click" />
                </td>
            </tr>
        </table><br /><br />
        <asp:HyperLink ID="lnkXML" runat="server"></asp:HyperLink>
    </div>
    </form>
</body>
</html>
