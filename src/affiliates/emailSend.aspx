<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="emailSend.aspx.vb" Inherits="maintenance_emailSend" %>
<%@ Register TagPrefix="FCKeditorV2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>Bulk Email</h2>
<br /><br />
            <asp:Panel ID="panBody" runat="server" Width="100%">
                <center>
                    <asp:Image ID="imgLogo" runat="server" ImageUrl="~/images/logo.gif" Visible="false" /><br />
                </center>
                <table border="0" width="100%" class="text">
                    <tr>
                        <td width="60">
                            Subject:
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubject" runat="server" Width="260" AutoPostBack="false" CausesValidation="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <FCKeditorV2:FCKeditor id="FCKeditor1" BasePath="~/EBEditor/" runat="server" ToolbarSet="Default" Width="700" Height="360" /><br />
                        </td>
                    </tr>
                </table>
                <br />
                <center class="text">
                    
                    <font color='#777777' size='-2'>Copyright ©<% =Now.Year%> <asp:Label ID="lblCompany" runat="server"></asp:Label>. All rights reserved.</font><br />
                    <font color='#777777'>If you like our newsletter and feel it is of value to you, perhaps it is of value to a colleague, - please pass it on.<br />However if you no longer wish to receive this newsletter please "<asp:HyperLink ID="lnkUnsubscribe" runat="server">unsubscribe</asp:HyperLink>" and you will be removed from our mailing list.</font>
                </center>
                <br />
            </asp:Panel><br />
            <table border="0" width="100%">
                <tr>
                    <td>&nbsp;</td>
                    <td valign="top">
                        <asp:DropDownList ID="drpGroups" runat="server" AppendDataBoundItems="true" DataSourceID="sqlGroups" DataTextField="groupName" DataValueField="groupID" AutoPostBack="false" CssClass="text" OnDataBinding="drpGroups_dataBinding">
                        </asp:DropDownList>  
                        <table>
                             <tr>
                                <td colspan="1">
                                    <asp:label ID="lbl1" runat="server" CssClass="text">Or enter an email address:</asp:label><br />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:label ID="lbl2" runat="server" CssClass="text">(Seperate multiple addresses with a comma)</asp:label>
                                </td>
                            </tr>
                        </table>      
                    </td>
                    <td valgin="top" align="right">
                        <asp:Button ID="btnSend" runat="server" Text="Send Email" CausesValidation="true" OnClick="btnSend_click" />        <br />
                        
                        <asp:Label ID="lblSent" runat="server" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
               
            </table>
            <br />
            
            <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtSubject" ErrorMessage="Please enter an email Subject" Display="dynamic"></asp:RequiredFieldValidator>
            <asp:Label ID="lblError" runat="server" ForeColor="red" Font-Bold="true"></asp:Label>
            <asp:Label ID="lblAddresses" runat="server"></asp:Label>
    
    <asp:SqlDataSource ID="SqlGroups" runat="server" SelectCommand="procEmailGroupsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>