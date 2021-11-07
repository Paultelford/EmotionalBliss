<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="shipping.aspx.vb" Inherits="affiliates_shipping" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress id="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel id="update1" runat="server">
        <ContentTemplate>   
    
            <table border="0" width="100%">
                <td valign="top">
                    <asp:Label ID="lblIntro" runat="server" Text="Current Tax & Shipping rates for "></asp:Label>
                    <asp:Label ID="lblCountryName" runat="server" Font-Bold="true"></asp:Label>
                </td>
                <td align="right" valign="top">
                    <asp:Label id="lblShippingVatText" runat="server" text="Shipping Tax Rate(%) "></asp:Label>
                    <asp:TextBox id="txtVatRate" runat="server" width="60" maxlength="4" ValidationGroup="vat"></asp:TextBox>&nbsp;
                    <asp:Button id="btnVatUpdate" runat="server" onClick="btnVatUpdate_Click" ValidationGroup="vat" Text="Set" /><br />
                    <asp:RangeValidator ID="ranTxtVatRange" runat="server" ControlToValidate="txtVatRate" Type="Double" MinimumValue="0" MaximumValue="100" ErrorMessage="Invalid percentage entered." ValidationGroup="vat" Display="dynamic"></asp:RangeValidator>
                    <asp:Label id="lblUpdateComplete" runat="server"></asp:Label>
                 </td>
            </table>
            <br />
            <table border="0" cellspacing="8" width="100%">
                <tr>
                    <td>
                        <asp:GridView ID="gvShipping" runat="server" DataSourceID="sqlShipping" DataKeyNames="id" AutoGenerateColumns="false" SkinID="GridView" EmptyDataText="No shipping weights have been added yet.">
                            <Columns>
                                <asp:BoundField HeaderText="Weight Limit (kg)" DataField="weightLimit" />
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:BoundField HeaderText="Cost (Exc Vat)" DataField="price" />
                                <asp:BoundField ItemStyle-Width="40" ReadOnly="true" />
                                <asp:CommandField ShowDeleteButton="true" ShowEditButton="true" />
                            </Columns>
                        </asp:GridView>
                        <asp:LinkButton ID="lnkAdd" runat="server" Text="Add new weight/price" OnClick="lnkAdd_click"></asp:LinkButton>
                        <asp:Panel ID="panAdd" runat="server" Visible="false">
                            <asp:FormView ID="fvAdd" runat="Server" DataSourceID="SqlShippingAdd" DefaultMode="insert" OnItemInserted="fvAdd_itemInserted">
                                <InsertItemTemplate>
                                    <br /><br />
                                    <table border="0" cellspacing="8">
                                        <tr>
                                            <td>
                                                Weight Limit:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLimit" runat="server" MaxLength="10" Text='<%# Bind("weightLimit") %>' ValidationGroup="add"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqTxtLimit" runat="server" ControlToValidate="txtLimit" ValidationGroup="add" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="ranTxtLimit" runat="server" ControlToValidate="txtLimit" ValidationGroup="add" ErrorMessage="* Invalid" Display="dynamic" Type="Double" MinimumValue="0" MaximumValue="10000"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Price (Exc Vat):
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPrice" runat="server" MaxLength="10" Text='<%# Bind("price") %>' ValidationGroup="add"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqTxtPrice" runat="server" ControlToValidate="txtPrice" ValidationGroup="add" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                                                <asp:RangeValidator ID="ranTxtPrice" runat="server" ControlToValidate="txtPrice" ValidationGroup="add" ErrorMessage="* Invalid" Display="dynamic" Type="currency" MinimumValue="0" MaximumValue="10000"></asp:RangeValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Button ID="btnSubmit" runat="server" ValidationGroup="add" Text="Add" CommandName="insert" />
                                            </td>
                                        </tr>
                                    </table>
                                </InsertItemTemplate>
                            </asp:FormView>
                        </asp:Panel>
                        <asp:Label ID="lblError" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            
            
            
        </ContentTemplate>
    </atlas:UpdatePanel>
        
    <asp:SqlDataSource ID="sqlShipping" runat="server" SelectCommand="procShippingWeightByCountryCodeSelect" SelectCommandType="StoredProcedure" UpdateCommand="procShippingWeightByIDUpdate" UpdateCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" DeleteCommand="procShippingWeightByIDDelete" DeleteCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
        <UpdateParameters>
            <asp:SessionParameter Name="id" Type="int32" />
            <asp:Parameter Name="weightLimit" Type="decimal" />
            <asp:Parameter Name="price" Type="decimal" />
        </UpdateParameters>
        <DeleteParameters>
            <asp:Parameter Name="id" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlShippingAdd" runat="server" InsertCommand="procShippingWeightInsert" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <InsertParameters>
            <asp:Parameter  name="weightLimit" Type="decimal" />
            <asp:Parameter  name="price" Type="decimal" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </InsertParameters>
    </asp:SqlDataSource>
</asp:Content>

