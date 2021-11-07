<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="warranty.aspx.vb" Inherits="affiliates_warranty" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>    
    <atlas:UpdatePanel id="update1" runat="Server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server"></eb:DateControl>
            <br /><br />
            <asp:GridView ID="gvList" runat="server" DataSourceID="sqlList" AutoGenerateColumns="false" SkinID="GridView">
                <Columns>
                    <asp:BoundField HeaderText="Warranty Start Date" DataField="startDate" HtmlEncode="false" DataFormatString="{0:dd MMM yyyy}" />
                    <asp:TemplateField HeaderText="Shopper Name">
                        <ItemTemplate>
                            <asp:Label ID="lblShopper" runat="server" Text='<%# Eval("firstname") & " " & Eval("surname") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Shop" DataField="shopName" />
                    <asp:BoundField HeaderText="Location" DataField="shopLocation" />
                    <asp:BoundField HeaderText="Product" DataField="product" />
                    <asp:BoundField HeaderText="Gender" DataField="gender" />
                </Columns>
            </asp:GridView>
            
            
        
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="sqlList" runat="server" SelectCommand="procWarrantyByDateSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" Name="startDate" PropertyName="getStartDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" Name="endDate" PropertyName="getEndDate" Type="datetime" />
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

