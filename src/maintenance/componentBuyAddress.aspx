<%@ Page Language="VB" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeFile="componentBuyAddress.aspx.vb" Inherits="maintenance_componentBuyAddress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="lblTitle" runat="server"></asp:Label><br /><br />
    <asp:DetailsView ID="dvAdd" runat="server" AutoGenerateRows="false" DefaultMode="Insert" GridLines="none">
        <Fields>
            <asp:TemplateField HeaderText="Company:">
                <ItemTemplate>
                    <asp:TextBox ID="txtCompany" runat="server" Width="260"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Add1:">
                <ItemTemplate>
                    <asp:TextBox ID="txtAdd1" runat="server" Width="260"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Add2:">
                <ItemTemplate>
                    <asp:TextBox ID="txtAdd2" runat="server" Width="260"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Add3:">
                <ItemTemplate>
                    <asp:TextBox ID="txtAdd3" runat="server" Width="260"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Add4:">
                <ItemTemplate>
                    <asp:TextBox ID="txtAdd4" runat="server" Width="260"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Add5:">
                <ItemTemplate>
                    <asp:TextBox ID="txtAdd5" runat="server" Width="260"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tel:">
                <ItemTemplate>
                    <asp:TextBox ID="txtTel" runat="server" Width="160"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email:">
                <ItemTemplate>
                    <asp:TextBox ID="txtEmail" runat="server" Width="260"></asp:TextBox>
                </ItemTemplate>
            </asp:TemplateField>            
        </Fields>
    </asp:DetailsView>
    <asp:Button ID="btnAdd" runat="server" Text="Add Address" OnClick="btnAdd_click" />

</asp:Content>