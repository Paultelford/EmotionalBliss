<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productMaster.aspx.vb" Inherits="maintenance_productMaster" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table border="0">
                <tr>
                    <td>
                        Select a Master Product to Edit:&nbsp;
                    </td>
                    <td valign="top">
                        <asp:DropDownList ID="drpMaster" runat="server" AppendDataBoundItems="true" DataTextField="name" DataValueField="masterID" DataSourceID="SqlMasters" OnSelectedIndexChanged="drpMaster_selectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>    
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        Or enter a new one below and click 'Add'
                    </td>
                </tr>
            </table>
            <br />
            <table border="0">
                <tr>
                    <td>
                        <asp:TextBox ID="txtMaster" runat="server" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtMaster" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                    </td>
                    <td>
                        <asp:Button ID="btnAddEdit" runat="server" Text="Add" OnClick="btnAddEdit_click" />            
                    </td>
                </tr>            
            </table>
        </ContentTemplate>
        <Triggers>
            <atlas:AsyncPostBackTrigger ControlID="drpMaster" EventName="selectedIndexChanged" />
        </Triggers>
    </atlas:UpdatePanel>  
    <br />
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000">
        <ProgressTemplate>
            Please Wait...<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>  
    
    <asp:SqlDataSource ID="SqlMasters" runat="server" SelectCommand="procProductMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

