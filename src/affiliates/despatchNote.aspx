<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="despatchNote.aspx.vb" Inherits="affiliates_despatchNote" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy id="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress><br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table border="0">
                <tr>
                    <td valign="top">
                         Enter the order no to print
                    </td>
                    <td valign="top">
                        <asp:TextBox id="txtOrderNo" runat="server" Width="80" ValidationGroup="textEntry" MaxLength="11"></asp:TextBox>&nbsp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_click" ValidationGroup="textEntry" /><br />
                        <asp:RequiredFieldValidator ID="reqTxtOrderNo" runat="server" ValidationGroup="textEntry" ControlToValidate="txtOrderNo" ErrorMessage="Please enter an order number." Display="dynamic"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        Or select a recent order from this list
                    </td>
                    <td>
                        <asp:DropDownList ID="drpOrderNo" DataSourceID="sqlRecentOrders" DataTextField="nid" DataValueField="id" runat="server" OnSelectedIndexChanged="drpOrderNo_selcectedIndexChanged" AppendDataBoundItems="true" AutoPostBack="true">
                        </asp:DropDownList>
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="drpOutstandingQty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOutstandingQty_selectedIndexChanged">
                            <asp:ListItem Text="Latest 20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="All Orders" Value="10000"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <br /><br />
                        <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlRecentOrders" runat="server" SelectCommand="procShopOrderByCountryCodeSelect20Paid" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="sqlRecentOrders_selecting">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            <asp:ControlParameter ControlID="drpOutstandingQty" PropertyName="selectedValue" Name="qty" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
   
   <script language="javascript" type="text/javascript">
    function openPopup(id)
    {
        var win=window.open("despatchNotePopup.aspx?id="+id,"despatchPopup","toolbar=yes");
    }
   </script>
</asp:Content>

