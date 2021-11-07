<%@ Page Language="VB" MasterPageFile="~/MasterPageSite.master" AutoEventWireup="false" CodeFile="warrantyOld.aspx.vb" Inherits="warranty" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="Server">
        <ContentTemplate>
            <asp:HiddenField ID="hidOrderID" runat="Server" />
            <h2>Trace Your Order</h2><br />
            <asp:Panel ID="pan1" runat="Server" BorderWidth="1" BorderColor="#dddddd" Width="500">
                <table border="0">
                    <tr>
                        <td colspan="2">
                            <br />Please enter your Order ID and postcode to start the Warranty Signup process:<br /><br />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" width="40%">
                            Order ID:
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderID" runat="server" MaxLength="12" ValidationGroup="entry"></asp:TextBox><asp:RequiredFieldValidator ID="reqPan1OrderID" runat="server" ControlToValidate="txtOrderID" ErrorMessage="* Required"  ValidationGroup="entry"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Post Code:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPostcode" runat="server" MaxLength="10" ValidationGroup="entry"></asp:TextBox><asp:RequiredFieldValidator ID="reqPan1Postcode" runat="server" ControlToValidate="txtPostcode" ErrorMessage="* Required"  ValidationGroup="entry"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblEntryError" runat="server" ForeColor="red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>&nbsp;</td>
                        <td>
                            <asp:Button ID="btnEntrySubmit" runat="server" Text="Submit" OnClick="btnEntrySubmit_click" ValidationGroup="entry" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblCriticalError" runat="server" ForeColor="red"></asp:Label>
            <br /><br />
            <asp:Panel ID="pan2" runat="server" Visible="false">
                Items in order <asp:Label ID="lblOrderID" runat="server"></asp:Label>
                <asp:GridView ID="gvItems" runat="Server" AutoGenerateColumns="false" DataKeyNames="orderItemID" SkinID="GridView" OnDataBound="gvItems_dataBound">
                    <Columns>
                        <asp:BoundField HeaderText="Product" DataField="affProductname" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Qty Ordered" DataField="qty" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Qty Despatched" DataField="qtyDespatched" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Warrenty Start" DataField="startDate" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Warrenty End" DataField="endDate" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <br /><br />
            <asp:Panel ID="pan3" runat="server" Visible="false">
                Enter your receipt number to start a new warranty with all the above items.<br />
                <asp:TextBox ID="txtReceipt" runat="server" MaxLength="20" ValidationGroup="signup"></asp:TextBox>&nbsp;
                <asp:Button ID="btnSignup" runat="server" ValidationGroup="signup" Text="Submit" OnClick="btnSignup_click" /><br />
                <asp:RequiredFieldValidator ID="reqPan3Signup" runat="server" ControlToValidate="txtReceipt" ErrorMessage="* Please neter a receipt no." Display="static" ValidationGroup="signup"></asp:RequiredFieldValidator>
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>

