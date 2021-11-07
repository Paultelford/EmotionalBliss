<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/masterPageShop.master" Theme="WinXP_Blue" CodeFile="countryChange.aspx.vb" Inherits="shop_countryChange"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <asp:Label ID="lblHeader" runat="server" CssClass="lightbluehead" Text="<font color='red'>Warning</font>"></asp:Label>
    <br /><br />
    You have items in your basket, if you wish to continue and change country, then the basket will be emptied,
    <br /><br />
    <table border="0" width="500">
        <tr>
            <td>
                <asp:Button ID="btnChange" runat="server" Text="Empty basket and change to " OnClick="btnChange_click" />
            </td>
            <td align="right">
                <input type="button" onclick="document.location='basket.aspx'" value="Dont change, take me to My Basket" />
            </td>
        </tr>
    </table>   
    <asp:HiddenField ID="newCountryCode" runat="server" EnableViewState="true" />
</asp:Content>