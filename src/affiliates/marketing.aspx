<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="marketing.aspx.vb" Inherits="affiliates_marketing" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>   
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="pan1" runat="server">
                <eb:DateControl id="date1" runat="server"></eb:DateControl>    
                <br />
                <table border="0">
                    <tr>
                        <td>
                            Order Type:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpType" runat="server" AutoPostBack="false">
                                <asp:ListItem Text="All" Value="%"></asp:ListItem>
                                <asp:ListItem Text="Phone" Value="25"></asp:ListItem>
                                <asp:ListItem Text="Post" Value="30"></asp:ListItem>
                                <asp:ListItem Text="Web" Value="20"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Postcode:<br />
                            <font size"-2">(First 2 digits only)</font>
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtPostcode" runat="server" MaxLength="2" Width="107"></asp:TextBox>&nbsp;
                            <asp:ImageButton ID="btnPostcode" runat="server" ImageUrl="~/images/mag.gif" OnClick="btnPostcode_click" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Age Range:<br />
                            <font size="-2">(When ordered)</font>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLoAge" runat="server" Width="40" MaxLength="3"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:TextBox ID="txtHiAge" runat="server" Width="40" MaxLength="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Product:
                        </td>
                        <td>
                            <asp:DropDownList ID="drpProduct" runat="server" DataSourceID="sqlProducts" DataTextField="salename" DataValueField="distBuyingID" AppendDataBoundItems="true">
                                <asp:ListItem Text="All" Value="%"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Show Vouchers:
                        </td>
                        <td>
                            <asp:CheckBox ID="chkVoucher" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2"></td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSubmit" runat="Server" Text="Submit" OnClick="btnSearch_click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="pan2" runat="server" Visible="false">
            <fieldset>
                <b>Emotionalbliss Marketing report <asp:Label ID="lblDates" runat="server"></asp:Label> for <asp:Label ID="lblCountryName" runat="server"></asp:Label></b><br />
                <table border="0">
                    <tr>
                        <td align="right"> 
                            Orders Delivered:
                        </td>
                        <td>
                            <asp:Label ID="lblOrdersDelivered" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Product:
                        </td>
                        <td>
                            <asp:Label ID="lblProduct" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Postcode:
                        </td>
                        <td>
                            <asp:Label ID="lblPostcode" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            No age specified:
                        </td>
                        <td>
                            <asp:Label ID="lblNoAgeSpecified" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            Age specified:
                        </td>
                        <td>
                            <asp:Label ID="lblAgeSpecified" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <b>Region of Purchaser</b>
                <asp:Table ID="tblRegion" runat="server">
                </asp:Table>
                <br />
                <b>Specified Age Range request <asp:Label ID="lblAgeRequest" runat="server"></asp:Label> </b><br />
                From <asp:Label ID="lblGaveAge" runat="server" Font-Bold="true"></asp:Label> orders who gave an age, the number between the age range specified: <asp:Label ID="lblOrderWithinAgeRange" runat="server" Font-Bold="true"></asp:Label><br />
                % from the above: <asp:Label ID="lblAgePercent" runat="server" Font-Bold="true"></asp:Label><br />
                Breakdown of genders from the <asp:Label ID="lblOrderTotal" runat="server" Font-Bold="true"></asp:Label> orders within the specified age range<br />
                Females in age range: <asp:Label id="lblFemalesInRange" runat="server" Font-Bold="true"></asp:Label><br />
                Males in age range: <asp:Label id="lblMalesInRange" runat="server" Font-Bold="true"></asp:Label><br />
                Unknown gender in age range: <asp:Label ID="lblUnknownInRange" runat="Server" Font-Bold="true"></asp:Label><br /><br />
                
                
                <b>End of Marketing report created <asp:Label ID="lblDateCreated" runat="server"></asp:Label></b>
                </fieldset>
                <br /><br />
                <asp:Button ID="btnBack" runat="server" Text="Back to Query" OnClick="btnBack_click" />
            </asp:Panel>
            <asp:Label id="lblError" runat="Server"></asp:Label>
            <asp:Label ID="lblError2" runat="server"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    <script language="javascript" type="text/javascript">
        function getElement(e) {
            if (__nonMSDOMBrowser)
                return document.getElementById(e);
            else
                return document.all[e];
        }
        var win;
        function openPopup() {
            win = window.open("marketing_pc.aspx", "postcodes", "");
        }
    </script>
    
    <asp:SqlDataSource ID="sqlproducts" runat="server" SelectCommand="procProductOnSaleByCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="String" Size="5" />
            <asp:Parameter Name="deptID" Type="String" Size="5" DefaultValue="%" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

