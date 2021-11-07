<%@ Page Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="traceOrder.aspx.vb" Inherits="traceOrder" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="home" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
                <asp:Label ID="lblHeader" runat="server">
                <h2><asp:Literal ID="litTraceHeader" runat="server" Text='<%$ Resources:litTraceHeader %>'></asp:Literal></h2>
                </asp:Label>
                <br />
                <asp:Panel BorderColor="#dddddd" ID="pan1" runat="server" Width="560" BorderWidth="0">
                    <br />
                    <table border="0">
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblInstructions" runat="server" Text="<%$ Resources:lblInstructions %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">&nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right" width="40%">
                                <asp:Label id="lblYourSurname" runat="server" Text="<%$ Resources:lblYourSurname %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSurname" runat="Server" ValidationGroup="entry"></asp:TextBox><asp:RequiredFieldValidator ID="reqSurname" runat="server" ErrorMessage="<%$ Resources:Invalid %>" ValidationGroup="entry" ControlToValidate="txtSurname" Display="dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td align="right">
                                <asp:Label ID="lblOrderID" runat="server" Text="<%$ Resources:lblOrderID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderID" runat="Server" ValidationGroup="entry"></asp:TextBox><asp:RequiredFieldValidator ID="reqOrderID" runat="server" ErrorMessage="<%$ Resources:Required %>" ValidationGroup="entry" ControlToValidate="txtOrderID" Display="dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblEntryError" runat="server" ForeColor="red"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkSubmit" runat="server" OnClick="btnSubmit_click" CssClass="SubmitRollover" ToolTip="Submit" dbResource="cssSubmit"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <br /><br />
                <asp:Panel ID="panTrace" runat="server" Visible="false">
                    <asp:GridView ID="gvTrace" runat="Server" AutoGenerateColumns="false" SkinID="GridView" EmptyDataText="<%$ Resources:gvTrace_empty %>" Width="100%">
                        <Columns>
                            <asp:BoundField HeaderText="<%$ Resources:gvTrace_headerDate %>" DataField="date" ItemStyle-VerticalAlign="top" ItemStyle-Width="20%" />
                            <asp:BoundField ItemStyle-Width="20" />
                            <asp:TemplateField HeaderText="Action" ItemStyle-VerticalAlign="top">
                                <ItemTemplate>
                                    <asp:Label ID="lblAction" runat="server" Text='<%# Replace(Eval("message"),chr(10),"<br>") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                <asp:Label ID="lblCriticalError" runat="server" ForeColor="red"></asp:Label>
</asp:Content>

