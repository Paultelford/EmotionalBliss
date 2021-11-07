<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="orderEdit.aspx.vb" Inherits="affiliates_orderEdit" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            <asp:Label ID="lblPleaseWait" runat="server" Text="Please Wait"></asp:Label>
            ....
            <asp:Image ID="imgPleaseWait" runat="server" ImageUrl="~/images/loading.gif" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <table border="0">
                <tr>
                    <td align="right">
                        <asp:HyperLink ID="lnkBack" runat="server" Text="Back"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DetailsView ID="dvCustomer" runat="server" DataKeyNames="customerID" DataSourceID="sqlCustomer" AutoGenerateRows="false" DefaultMode="Edit" GridLines="None" OnDataBound="dvCustomer_dataBound">
                            <Fields>
                                <asp:BoundField HeaderText="Billing Name" DataField="billName" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Billing Add 1" DataField="billAdd1" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Billing Add 2" DataField="billAdd2" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Billing Add 3" DataField="billAdd3" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Billing Add 4" DataField="billAdd4" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Billing Add 5" DataField="billAdd5" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Billing Postcode" DataField="billPostcode" />
                                <asp:BoundField HeaderText="Phone" DataField="phone" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Email" DataField="email" ControlStyle-Width="300" />
                                <asp:BoundField ReadOnly="true" />
                                <asp:BoundField HeaderText="Shipping Name" DataField="shipName" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Shipping Add 1" DataField="shipAdd1" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Shipping Add 2" DataField="shipAdd2" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Shipping Add 3" DataField="shipAdd3" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Shipping Add 4" DataField="shipAdd4" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Shipping Add 5" DataField="shipAdd5" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Shipping Postcode" DataField="shipPostcode" />
                                <asp:BoundField ReadOnly="true" />
                                <asp:BoundField HeaderText="Card Number" DataField="cardNo" ControlStyle-Width="300" />
                                <asp:TemplateField Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="lblCCEnc" runat="server" Text='<%# encryptCard(Eval("cardNo")) %>'></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Start" DataField="cardStart" />
                                <asp:BoundField HeaderText="Exp" DataField="cardExp" />
                                <asp:BoundField HeaderText="Issue" DataField="cardIssue" />
                                <asp:TemplateField HeaderText="Card Type">
                                    <EditItemTemplate> 
                                        <asp:DropDownList ID="drpType" runat="server" SelectedValue='<%# Bind("cardType") %>'>
                                            <asp:ListItem Text="Visa" Value="VISA"></asp:ListItem>
                                            <asp:ListItem Text="Visa Delta/Debit" Value="DELTA"></asp:ListItem>
                                            <asp:ListItem Text="Visa Electron" Value="UKE"></asp:ListItem>
                                            <asp:ListItem Text="Mastercard" Value="MC"></asp:ListItem>
                                            <asp:ListItem Text="UK Maestro" Value="SWITCH"></asp:ListItem>
                                            <asp:ListItem Text="International Maestro" Value="MAESTRO"></asp:ListItem>
                                            <asp:ListItem Text="Solo" Value="SOLO"></asp:ListItem>
                                            <asp:ListItem Text="American Express" Value="AMEX"></asp:ListItem>
                                            <asp:ListItem Text="Diners Club" Value="DINERS"></asp:ListItem>
                                            <asp:ListItem Text="JCB" Value="JCB"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="CV2" DataField="cardCv2" />
                                <asp:BoundField ReadOnly="true" />
                                <asp:BoundField HeaderText="Cheque No" DataField="chequeNo" ControlStyle-Width="300" />
                                <asp:BoundField HeaderText="Account No" DataField="accountNo" ControlStyle-Width="300" />            
                                <asp:CommandField UpdateText="Save Changes" ShowEditButton="true" ShowCancelButton="false" ItemStyle-HorizontalAlign="right" />
                            </Fields>
                        </asp:DetailsView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </atlas:UpdatePanel>
        
    <asp:SqlDataSource ID="sqlCustomer" runat="Server" SelectCommand="procShopCustomerByOrderIDSelect" SelectCommandType="StoredProcedure" UpdateCommand="procShopCustomerByIDUpdate" UpdateCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnUpdating="sqlCustomer_updating" OnUpdated="sqlCustomer_updated">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="orderID" Type="int32" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="customerID" Type="int32" />
            <asp:Parameter Name="billName" Type="string" Size="50" />
            <asp:Parameter Name="billAdd1" Type="string" Size="50" />
            <asp:Parameter Name="billAdd2" Type="string" Size="50" />
            <asp:Parameter Name="billAdd3" Type="string" Size="50" />
            <asp:Parameter Name="billAdd4" Type="string" Size="50" />
            <asp:Parameter Name="billAdd5" Type="string" Size="50" />
            <asp:Parameter Name="billPostcode" Type="string" Size="10" />
            <asp:Parameter Name="phone" Type="string" Size="50" />
            <asp:Parameter Name="email" Type="string" Size="50" />
            <asp:Parameter Name="shipName" Type="string" Size="50" />
            <asp:Parameter Name="shipAdd1" Type="string" Size="50" />
            <asp:Parameter Name="shipAdd2" Type="string" Size="50" />
            <asp:Parameter Name="shipAdd3" Type="string" Size="50" />
            <asp:Parameter Name="shipAdd4" Type="string" Size="50" />
            <asp:Parameter Name="shipAdd5" Type="string" Size="50" />
            <asp:Parameter Name="ShipPostcode" Type="string" Size="10" />
            <asp:Parameter Name="cardNo" Type="string" Size="30" />
            <asp:Parameter Name="ccEnc" Type="string" Size="50" />
            <asp:Parameter Name="cardStart" Type="string" Size="5" />
            <asp:Parameter Name="cardExp" Type="string" Size="5" />
            <asp:Parameter Name="cardIssue" Type="string" Size="3" />
            <asp:Parameter Name="cardType" Type="string" Size="10" />
            <asp:Parameter Name="cardCV2" Type="string" Size="4" />
            <asp:Parameter Name="chequeNo" Type="string" Size="50" />
            <asp:Parameter Name="accountNo" Type="string" Size="50" />            
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

