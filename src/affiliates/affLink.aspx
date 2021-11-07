<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="affLink.aspx.vb" Inherits="affiliates_affLink" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
     <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
            <table id="tblMain" runat="server">
                <tr>
                    <td>
                        Current default page:
                    </td>
                    <td>
                        <asp:Label ID="lblCurrent" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        Set default page:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPage_selectedIndexChanged">
                            <asp:ListItem Text="EB Welcome Page" Value="welcome"></asp:ListItem>
                            <asp:ListItem Text="EB Shop" Value="shop"></asp:ListItem>
                            <asp:ListItem Text="EB Shop Product" Value="product"></asp:ListItem>
                            <asp:ListItem Text="Sexologists" Value="sex"></asp:ListItem>
                            <asp:ListItem Text="Custom Page" Value="custom"></asp:ListItem>
                        </asp:DropDownList>        
                    </td>
                </tr>
                <tr id="trLanguage" runat="server">
                    <td>
                        Language:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpLanguage" runat="server" DataSourceID="sqlLanguage" DataTextField="countryName" DataValueField="countryCode" AutoPostBack="true" OnSelectedIndexChanged="drpLanguage_selectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trParam" runat="server" visible="false">
                    <td>
                        Product:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpParam" runat="server" DataTextField="warehouseProductName" DataValueField="warehouseProductID" AutoPostBack="true" OnSelectedIndexChanged="drpParam_selectedIndexChanged">                
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr id="trCustom" runat="server" visible="false">
                    <td>
                        Custom URL:
                    </td>
                    <td id="tdCustomPage" runat="server" visible="true" colspan="3">
                        <asp:TextBox ID="txtCustom" runat="Server" MaxLength="50" Width="300"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="btnSubmit" runat="server" Text="Save Changes" OnClick="btnSubmit_saveChanges" />
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:Label ID="lblURL" runat="server" Visible="false"></asp:Label><br />
            <asp:Label ID="lblParam" runat="server" Visible="false"></asp:Label>
            <br /><br />
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlLanguage" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionstring %>">
        <SelectParameters>
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

