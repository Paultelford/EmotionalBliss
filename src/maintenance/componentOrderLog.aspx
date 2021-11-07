<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" Debug="true" AutoEventWireup="false" CodeFile="componentOrderLog.aspx.vb" Inherits="maintenance_componentOrderLog" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="../images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <br />
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panCancel" runat="server" BorderWidth="0" Visible="false">
                <table border="0">
                    <tr>
                        <td>
                            Please confirm you want to cancel Order&nbsp;
                            <asp:Label ID="lblOrderIDCancel" runat="server" ForeColor="black"></asp:Label>
                            <br />
                            <asp:Button ID="btnCancel" runat="server" Text="Yes, cancel order" OnClick="btnCancel_click" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btnNo" runat="server" Text="No, dont cancel" OnClick="btnNo_click" />
                        </td>
                    </tr>
                </table>                
                <br />
            </asp:Panel>
            <table border="0">
                <tr>
                    <td>
                        Manufacturer: 
                    </td>
                    <td>
                        <asp:DropDownList ID="drpMan" runat="server" DataSourceID="sqlManufacturers" DataTextField="manufacturerName" DataValueField="manufacturerID" AutoPostBack="true" AppendDataBoundItems="true">
                            <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                
                    <td align="right">
                        Status: 
                    </td>
                    <td align="right">
                        <asp:DropDownList ID="drpStatus" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="All" Value="%"></asp:ListItem>
                            <asp:ListItem Text="Complete" Value="complete"></asp:ListItem>
                            <asp:ListItem Text="Pending" Value="pending"></asp:ListItem>
                            <asp:ListItem Text="Cancelled" Value="cancelled"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <td colspan="4">
                    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="false" DataSourceID="sqlOrders" SkinID="GridView" OnDataBound="gvOrders_dateBound" OnSelectedIndexChanged="gvOrders_selectedIndexChanged" AllowPaging="true" PageSize="25">
                        <HeaderStyle Font-Bold="true" />
                        <Columns>
                            <asp:HyperLinkField HeaderText="Order ID" DataNavigateUrlFields="compOrderID" DataTextField="compOrderID" DataNavigateUrlFormatString="componentOrderView.aspx?id={0}" />
                            <asp:BoundField HeaderText="Date" DataField="orderDate" HtmlEncode="false"  />
                            <asp:BoundField HeaderText="Total" DataField="orderTotal" HtmlEncode="false" DataFormatString="{0:N}" />
                            <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" />
                            <asp:BoundField HeaderText="Status" DataField="status" />
                            <asp:HyperLinkField Text="Download" DataNavigateUrlFields="compOrderID" DataNavigateUrlFormatString="../pdfs/PeartreePurchaseOrder{0}.pdf" />
                            <asp:CommandField SelectText="ReCreate PDF" ShowSelectButton="true" ButtonType="Link" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCancel" runat="server" Text="Cancel" OnClick="lnkCancel_click" CommandArgument='<%# Eval("compOrderID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </table>
            <br />
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
            <asp:Label ID="lblErrorDetail" runat="server" ForeColor="white"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
        
    
    <asp:SqlDataSource ID="sqlOrders" runat="server" SelectCommand="procComponentOrdersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMan" Name="manufacturerID" PropertyName="selectedValue" Type="int32" />
            <asp:ControlParameter ControlID="drpStatus" Name="status" PropertyName="selectedValue" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlManufacturers" runat="server" SelectCommand="procManufacturersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>

