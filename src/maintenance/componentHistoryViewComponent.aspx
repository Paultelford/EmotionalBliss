<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" CodeFile="componentHistoryViewComponent.aspx.vb" Inherits="maintenance_componentHistoryViewComponent" Title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="Server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false" DisplayAfter="1000">
        <ProgressTemplate>
            Please Wait...<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl>
            <br />
            <asp:Label ID="lblComponent" runat="server" Font-Bold="true"></asp:Label>
            <br />
            <asp:Label ID="lblDataBoundError" runat="server"></asp:Label>
            <asp:GridView ID="gvHistory" runat="server" AutoGenerateColumns="false" DataKeyNames="componentHistoryID" EmptyDataText="No results found." GridLines="vertical" OnDataBound="gvHistory_dataBoundTotals" ShowFooter="true" CellPadding="2">
                <HeaderStyle Font-Bold="true" HorizontalAlign="left" VerticalAlign="bottom" Font-Size="Small" />
                <FooterStyle Font-Bold="true" HorizontalAlign="left" />
                <Columns>
                    <asp:TemplateField HeaderText="Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDate" runat="server" Text='<%# formatStartDate(Eval("dateChanged")) %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Action" DataField="compAction" ItemStyle-Font-Size="Small" />
                    <asp:HyperLinkField HeaderText="CompBatchID" DataTextField="compBatchID" DataNavigateUrlFields="compBatchID" DataNavigateUrlFormatString="componentHistoryViewBatch.aspx?id={0}" ItemStyle-Font-Size="Small" />
                    <asp:HyperLinkField HeaderText="CompOrderID" DataTextField="compOrderID" DataNavigateUrlFields="compOrderID" DataNavigateUrlFormatString="componentOrderPDF.aspx?id={0}&includeComments=true" Target="_blank" ItemStyle-Font-Size="Small" />
                    <asp:BoundField HeaderText="User" DataField="changedBy" FooterText="<br>Totals" ItemStyle-Font-Size="Small" />
                    <asp:BoundField HeaderText="QuarantineIn" DataField="qtyAdded" ItemStyle-Width="80" ItemStyle-Font-Size="Small" />
                    <asp:BoundField HeaderText="QuarantineOut" DataField="qtyRemoved" ItemStyle-Font-Size="Small" />            
                    <asp:BoundField HeaderText="QtyIn" ItemStyle-Width="60" DataField="qtyStockAdded" ItemStyle-Font-Size="Small" />            
                    <asp:BoundField HeaderText="QtyOut" ItemStyle-Width="60" DataField="qtyStockRemoved" ItemStyle-Font-Size="Small" />        
                    <asp:BoundField HeaderText="ScrappedIn" ItemStyle-Width="70" DataField="qtyScrappedAdded" ItemStyle-Font-Size="Small" />            
                    <asp:BoundField HeaderText="ScrappedOut" DataField="qtyScrappedRemoved" ItemStyle-Font-Size="Small" />        
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:label ID="lblQuarantine" runat="server" Text='<%# Eval("quarantine") %>' Font-Size="Small"></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:label ID="lblActionID" runat="server" Text='<%# Eval("actionID") %>' Font-Size="Small"></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:label ID="lblProdAssemblyID" runat="server" Text='<%# Eval("productAssemblyID") %>' Font-Size="Small"></asp:label>
                        </ItemTemplate>
                    </asp:TemplateField>  
                    <asp:BoundField HeaderText="QtyPassed" ItemStyle-Width="60" DataField="qtyStockAdded" ItemStyle-Font-Size="small" />
                    <asp:BoundField HeaderText="QtyFailed" ItemStyle-Width="60" DataField="qtyFailedAdded" ItemStyle-Font-Size="small" />
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Eval("productionEndDate") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
</asp:Content>

