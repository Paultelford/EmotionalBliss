<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="warehouseAssemblyComplete.aspx.vb" Inherits="maintenance_warehouseAssemblyComplete" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="../images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    Outstanding warehouse assembly batches:
    <br /><br />
    <asp:GridView ID="gvBatches" runat="server" SkinID="GridView" DataSourceID="SqlBatches" DataKeyNames="warehouseID" AutoGenerateColumns="false" GridLines="none" OnSelectedIndexChanging="gvBatches_selectedInxedChanging">
        <HeaderStyle Font-Bold="true" />
        <Columns>
            <asp:BoundField HeaderText="Boxed Product" DataField="warehouseProductName" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:TemplateField HeaderText="Start Date">
                <ItemTemplate>
                    <asp:Label ID="lblStartDate" runat="server" Text='<%# formatStartDate(Eval("warehouseStartDate")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField ItemStyle-Width="40" />
            <asp:BoundField HeaderText="Batch Qty" DataField="warehouseAmount" />
            <asp:BoundField ItemStyle-Width="40" />
            <asp:CommandField SelectText="Process" ShowCancelButton="false" ShowSelectButton="true" />
            <asp:TemplateField Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblProductID" runat="server" Text='<%# Eval("warehouseProductID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <br /><br />
    <asp:Panel ID="pan1" runat="server" Visible="false">
        Enter the batches Pass/Fail results:<br />
        <table border="0">
            <tr>
                <td>
                    Pass:
                </td>
                <td>
                    <asp:TextBox ID="txtPass" runat="server" Width="40"></asp:TextBox>
                </td>
            </tr>
            <tr id="fail" runat="server" visible="false">
                <td>
                    Fail:
                </td>
                <td>
                    <asp:TextBox ID="txtFail" runat="server" Width="40"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_click" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
    </asp:Panel>
    
    <asp:SqlDataSource ID="SqlBatches" runat="server" SelectCommand="procWarehouseBatchOutstandingSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
</asp:Content>

