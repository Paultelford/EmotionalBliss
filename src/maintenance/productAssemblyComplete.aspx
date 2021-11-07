<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productAssemblyComplete.aspx.vb" Inherits="maintenance_productAssemblyComplete" title="Untitled Page" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
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
            <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true">
                <asp:ListItem Text="Outstanding Batches" Value="outstanding"></asp:ListItem>
                <asp:ListItem Text="Complete Batches" Value="complete"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />    
            <eb:DateControl id="date1" runat="server" onDateChanged="date1_dateChanged"></eb:DateControl>                        
            <br /><br />
            <asp:Panel ID="panOutstanding" runat="server">
                <asp:GridView ID="gvProduction" runat="server" AutoGenerateColumns="false" SkinID="GridView" DataKeyNames="productionID" DataSourceID="SqlProductionBatch" GridLines="none" OnSelectedIndexChanging="gvProduction_selectedIndexChanging" OnDataBound="gvProduction_dataBound">
                    <HeaderStyle Font-Bold="true" />
                    <Columns>
                        <asp:BoundField HeaderText="Product" DataField="productName" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField HeaderText="Start Date">
                            <ItemTemplate>
                                <asp:Label ID="lblStartDate" runat="server" Text='<%# formatStartDate(Eval("productionStartDate")) %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:BoundField HeaderText="Batch Qty" ItemStyle-HorizontalAlign="center" DataField="productionAmount" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:CommandField SelectText="Process" ShowSelectButton="true" ShowCancelButton="false" ShowDeleteButton="false" ShowEditButton="false" />
                        <asp:BoundField ItemStyle-Width="40" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:hyperlink ID="hypLink" runat="server" Text="Reprint"></asp:hyperlink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label id="lblProductID" runat="server" Text='<%# Eval("productID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <br /><br />
            <asp:Panel ID="pan1" runat="server" Visible="false">
                Enter the batches Pass/Fail results:<br />
                <table>
                    <tr>
                        <td>
                            Pass:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPass" runat="server" width="40"></asp:TextBox><asp:RegularExpressionValidator ID="regEx1" runat="server" ControlToValidate="txtPass" ValidationExpression="^(?:(?:[+\-]?\$?)|(?:\$?[+\-]?))?(?:(?:\d{1,3}(?:(?:,\d{3})|(?:\d))*(?:\.(?:\d*|\d+[eE][+\-]\d+))?)|(?:\.\d+(?:[eE][+\-]\d+)?))$" Display="dynamic" ErrorMessage="*Non-numerical"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Fail:
                        </td>
                        <td>
                            <asp:TextBox ID="txtFail" runat="server" width="40"></asp:TextBox><asp:RegularExpressionValidator ID="regEx2" runat="server" ControlToValidate="txtFail" ValidationExpression="^(?:(?:[+\-]?\$?)|(?:\$?[+\-]?))?(?:(?:\d{1,3}(?:(?:,\d{3})|(?:\d))*(?:\.(?:\d*|\d+[eE][+\-]\d+))?)|(?:\.\d+(?:[eE][+\-]\d+)?))$" Display="dynamic" ErrorMessage="*Non-numerical"></asp:RegularExpressionValidator>
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
                <asp:label id="lblError" runat="server" ForeColor="red"></asp:label>
            </asp:Panel>
            <asp:Panel ID="Pan2" runat="server" Visible="false">
                <!--hidden values-->
                <asp:Label ID="lblProductAssemblyID" runat="server" Visible="false"></asp:Label><br />
                <asp:label id="lblFailed" runat="server"></asp:label> Product(s) failed, scrap or recycle the components.<br /><br />
                <asp:GridView ID="gvScrap" runat="server" DataKeyNames="componentID" DataSourceID="SqlScrap" AutoGenerateColumns="false" OnRowDataBound="gvScrap_rowDataBound">
                    <HeaderStyle Font-Bold="true" />
                    <Columns>
                        <asp:BoundField HeaderText="Component" DataField="componentName" />
                        <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" />
                        <asp:BoundField HeaderText="Qty used in production run" DataField="masterQty" />
                        <asp:TemplateField HeaderText="Recycle">
                            <ItemTemplate>
                                <asp:TextBox ID="txtRecycle" runat="server" Width="40"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scrap">
                            <ItemTemplate>
                                <asp:TextBox ID="txtScrap" runat="server" Width="40"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMasterID" runat="server" Text='<%#Eval("masterID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>                
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblStockRemoved" runat="server" Text='<%#Eval("qtyStockRemoved") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMasterQty" runat="server" Text='<%#Eval("masterQty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Label ID="lblAllocationError" runat="server"></asp:Label><br />
                <asp:Button ID="btnScrap" runat="server" Text="Scrap/Recycle" OnClick="btnScrap_click" />
            </asp:Panel>
            
            <asp:Panel ID="panComplete" runat="server" Visible="true">
                <asp:GridView ID="gvComplete" runat="server" SkinID="GridView" DataKeyNames="productionID" DataSourceID="SqlComplete" AutoGenerateColumns="True" OnRowDataBound="gvComplete_rowDataBound" EmptyDataText="No results for selected date range" OnSelectedIndexChanged="gvComplete_selectedIndexChanged" OnDataBound="gvComplete_dataBound">
                    <Columns>
                        <asp:BoundField HeaderText="Batch" DataField="productionID" />
                        <asp:BoundField HeaderText="Product" DataField="productName" />
                        <asp:BoundField HeaderText="Start" DataField="productionStartDate" />
                        <asp:BoundField HeaderText="End" DataField="productionEndDate" />
                        <asp:BoundField HeaderText="Pass" DataField="" />
                        <asp:BoundField HeaderText="Fail" DataField="" />
                        <asp:BoundField HeaderText="Scrap" DataField="" />
                        <asp:HyperLinkField HeaderText="" DataTextFormatString="Print" Target="_blank" DataTextField="productionID" DataNavigateUrlFormatString="productAssemblyCompletePrint.aspx?id={0}" DataNavigateUrlFields="productionID" />
                        <asp:CommandField SelectText="Pending" ShowSelectButton="true" />                        
                        <asp:TemplateField Visible="false"> 
                            <ItemTemplate>
                                <asp:Label ID="lblFaultsProcessed" runat="server" Text='<%# Eval("productionFaultsProcessed") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <br /><br />
                <asp:GridView ID="gvDataEntry" runat="server" DataSourceID="SqlComponents" DataKeyNames="componentID" GridLines="none" AutoGenerateColumns="false" Visible="true" OnRowDataBound="gvDataEntry_databound" Width="600">
                    <Columns>
                        <asp:BoundField HeaderText="Component" DataField="componentName" ReadOnly="true" />
                        <asp:BoundField HeaderText="Location" DataField="locationBay" ReadOnly="true" />
                        <asp:BoundField HeaderText="Qty" ReadOnly="true" />
                        <asp:TemplateField HeaderText="Pass" ItemStyle-Width="50">
                            <ItemTemplate>
                                <asp:TextBox ID="txtPass" runat="Server" Width="40" EnableViewState="true"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Fail" ItemStyle-Width="50">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFail" runat="Server" Width="40"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Scrap" ItemStyle-Width="50">
                            <ItemTemplate>
                                <asp:TextBox ID="txtScrap" runat="Server" Width="40"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# Eval("productionAmount") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblFailed" runat="server" Text='<%# Eval("productionFailed") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalBatchComponents" runat="server" Text='<%# Eval("compQty") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <table border="0" width="600" id="tblInfo" runat="Server" visible="false">
                    <tr>
                        <td valign="top">
                            <b>Info:</b>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInfo" runat="server" TextMode="MultiLine" Rows="5" Columns="50"></asp:TextBox>
                        </td>
                        <td align="right" valign="top">
                            <asp:Button ID="btnDataSubmit" runat="server" Text="Submit" OnClick="btnDataSubmit_click" />
                        </td>
                    </tr>
                </table>
                
                
            </asp:Panel>
            <asp:Label ID="lblError2" runat="server" ForeColor="red"></asp:Label>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="SqlProductionBatch" runat="server" SelectCommand="procProductionBatchOutstandingSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlScrap" runat="server" SelectCommand="procComponentHistoryByProductAssemblyIDFailedSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="lblFailed" PropertyName="text" Name="qtyFailed" />
            <asp:ControlParameter ControlID="lblProductAssemblyID" PropertyName="text" Name="prodAssemblyID" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="SqlComplete" runat="server" SelectCommand="procProductionBatchCompleteSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="date1" PropertyName="getStartDate" Name="startDate" Type="datetime" />
            <asp:ControlParameter ControlID="date1" PropertyName="getEndDate" Name="endDate" Type="datetime" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <asp:SqlDataSource ID="SqlComponents" runat="server" SelectCommand="procProductionBatchByBatchIDComponentsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
               <asp:ControlParameter ControlID="gvComplete" PropertyName="selectedValue" Name="batchID" Type="int32" />
            </SelectParameters>
        </asp:SqlDataSource>
    
    <script language="Javascript">
        function openViewPop(id)
        {
            window.open("productAssemblyView.aspx?id="+id,"ViewPrintPop","toolbars=none");
        }
        function openPrintPop(id)
        {
            window.open("productAssemblyCompletePrint.aspx?id="+id,"FaultPrintPop","toolbars=none");
        }
        function openProdAssemblyPop(id)
        {
            window.open("productAssemblyPDF.aspx?id="+id,"assPop","toolbars=none");            
        }
    </script>
</asp:Content>

