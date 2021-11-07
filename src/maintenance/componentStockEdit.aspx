<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="componentStockEdit.aspx.vb" Inherits="maintenance_componentStockEdit" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    
            <asp:DetailsView ID="dvComp" runat="server" DataSourceID="SqlComponent" AutoGenerateRows="false" GridLines="none">
                <FieldHeaderStyle Font-Bold="true" Width="140" />
                <Fields>
                    <asp:BoundField HeaderText="Component:" DataField="componentName" />
                    <asp:BoundField HeaderText="Manufacturer:" DataField="manufacturerName" />
                    <asp:BoundField HeaderText="Quarantine Stock:" DataField="quarantineStock" />
                    <asp:BoundField HeaderText="Component Stock" DataField="componentStock" />
                </Fields>
            </asp:DetailsView>
            <br /><br />
            <table border="0" id="tblDetails" runat="server">
                <tr>
                    <td>
                        <asp:RadioButtonList ID="radAction" runat="server">
                            <asp:ListItem Selected="true" Text="Add" Value="add"></asp:ListItem>
                            <asp:ListItem Text="Remove" Value="rem"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td align="center">
                        To/From
                    </td>
                    <td>
                        <asp:RadioButtonList ID="radDestination" runat="server">
                            <asp:ListItem Selected="True" Text="Stock" Value="stock"></asp:ListItem>
                            <asp:ListItem Text="Quarantine" Value="quarantine"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="3">&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Quantity:</b>
                    </td>
                    <td colspan="2">
                        <asp:TextBox ID="lblQty" runat="server"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="lblQty" ErrorMessage="Required Vield" Display="Dynamic"></asp:RequiredFieldValidator><asp:RangeValidator ID="range1" runat="server" ControlToValidate="lblQty" MinimumValue="1" MaximumValue="999999" ErrorMessage="Invalid quantity" Display="Dynamic"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>Reason:</b>
                    </td>
                    <td colspan="2">
                        <asp:TextBox TextMode="MultiLine" ID="txtReason" runat="server" Columns="50" Rows="5"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td colspan="2">
                        <asp:Button ID="btnSubmit" runat="server" Text="Apply" OnClick="btnSubmit_click" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="panHandle" runat="server" Visible="false">
                Details Added.
                <br />
                Click <asp:LinkButton ID="lbtnBack" runat="server" OnClick="lbtnBack_click" Text="here"></asp:LinkButton> to go back to Stock.
                <br />
                &nbsp;&nbsp;&nbsp;&nbsp;Or
                <br />
                Make another <asp:HyperLink ID="lnkSame" runat="server" Text="Adjustment"></asp:HyperLink>
            </asp:Panel>            
        
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500">
        <ProgressTemplate>
            Updating...<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    
    
    <asp:SqlDataSource ID="SqlComponent" runat="server" SelectCommand="procComponentByIDStockSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="id" Name="compID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

