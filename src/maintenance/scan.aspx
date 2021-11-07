<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="scan.aspx.vb" Inherits="affiliates_scan" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false" AssociatedUpdatePanelID="update1">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>    
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <br />
            <table border="0">
                <tr>
                    <td>
                        Order number to scan:
                    </td>
                    <td>
                        <asp:TextBox ID="txtOrderNo" runat="server" Width="50" MaxLength="6" ValidationGroup="scan" AutoPostBack="false"></asp:TextBox> <asp:Button ID="btnSubmit" ValidationGroup="scan" runat="server" Text="Scan" OnClick="btnSubmit_click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;&nbsp;&nbsp;&nbsp;Or, select...<br />
                    </td>
                    <td>
                    
                    </td>
                </tr>
                <tr>
                    <td>
                        Outstanding orders:
                    </td>
                    <td>
                        <asp:DropDownList ID="drpLatestOrders" runat="server" DataTextField="NID" DataValueField="ID" OnSelectedIndexChanged="drpLatestOrders_selectedIndexChanged" DataSourceID="sqlRecentOrders" AppendDataBoundItems="true" AutoPostBack="true">
                            <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                        </asp:DropDownList>        
                        &nbsp;&nbsp;
                        <asp:DropDownList ID="drpOutstandingQty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpOutstandingQty_selectedIndexChanged">
                            <asp:ListItem Text="Latest 20" Value="20"></asp:ListItem>
                            <asp:ListItem Text="All Orders" Value="10000"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtOrderNo" ErrorMessage="You must enter an order number." Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:RangeValidator ID="ran1" runat="server" ControlToValidate="txtOrderNo" ErrorMessage="Invalid order number." MinimumValue="1" MaximumValue="999999" Display="Dynamic"></asp:RangeValidator>
            <asp:Label ID="lblError" runat="server" ForeColor="blue"></asp:Label><br />
            <asp:Button id="btnScanAnyway" runat="server" Text="Continue With Scan" OnClick="btnScanAnyway_click" Visible="false" />
            <asp:Label ID="lblCriticalError" runat="server" ForeColor="red"></asp:Label>
            <asp:Panel ID="panTracker" runat="Server" Visible="false">
                <br />
                Order ID:<asp:Label ID="lblUserOrderID" runat="server" Font-Bold="true"></asp:Label> has been found, the details are show below. If this is the correct order then please enter the tracker number to complete the scan process.
                <br /><br />
                <table border="0">
                    <tr>
                        <td valign="top">
                            <asp:DetailsView ID="dvBillAdd" runat="server" DataSourceID="sqlBillAdd" AutoGenerateRows="false" GridLines="none" OnDataBound="dvBillAdd_dataBound" Font-Bold="true">
                                <Fields>
                                    <asp:BoundField DataField="shipName" NullDisplayText="*" />
                                    <asp:BoundField DataField="shipAdd1" NullDisplayText="*" />
                                    <asp:BoundField DataField="shipAdd2" NullDisplayText="*" />
                                    <asp:BoundField DataField="shipAdd3" NullDisplayText="*" />
                                    <asp:BoundField DataField="shipAdd4" NullDisplayText="*" />
                                    <asp:BoundField DataField="shipAdd5" NullDisplayText="*" />
                                    <asp:BoundField DataField="shipPostcode" NullDisplayText="*" />
                                    <asp:BoundField DataField="shipCountry" NullDisplayText="*" />
                                    <asp:BoundField DataField="email" NullDisplayText="*" />
                                </Fields>
                            </asp:DetailsView>
                        </td>
                        <td width="40">&nbsp;</td>
                        <td valign="top">
                            <asp:Label ID="lblItemsInOrder" runat="server" Text="<b>Items in Order</b>" Visible="false"></asp:Label>
                            <asp:GridView ID="gvOrderItems" runat="server" DataSourceID="sqlOrderItems" DataKeyNames="itemID" AutoGenerateColumns="false" GridLines="none" ShowHeader="true" OnDataBound="gvOrderItems_dataBound">
                                <Columns>
                                    <asp:BoundField DataField="affProductName" HeaderText="Items In Order" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:TemplateField HeaderText="Qty Oustanding">
                                        <ItemTemplate>  
                                            <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Qty") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Code" DataField="warehouseProductCode" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:TemplateField>
                                        <ItemTemplate>  
                                            <asp:TextBox ID="txtQty" runat="server" Width="40" Text="0" Visible="false" ValidationGroup="tracker"></asp:TextBox>
                                            <asp:RangeValidator ID="ranQty" runat="server" ControlToValidate="txtQty" ErrorMessage="Invalid" ValidationGroup="tracker" MinimumValue="0" MaximumValue="99999"></asp:RangeValidator>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Current Stock" DataField="stock" />
                                </Columns>
                            </asp:GridView>
                            
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkPartOrder" runat="server" OnCheckedChanged="chkPartOrder_changed" Text="Part Order" AutoPostBack="true" /><br />
                            <asp:Label ID="lblPartOrderMsg" runat="server" Visible="false">
                                <b>
                                    Please enter the qty's of each item to be despactched today.
                                </b>
                            </asp:Label>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
          </ContentTemplate>
    </atlas:UpdatePanel>    
    <br /><br />
    <atlas:UpdateProgress ID="up2" runat="server" DynamicLayout="false" AssociatedUpdatePanelID="update2">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update2" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panTrackerEmail" runat="server" Visible="false">
                <table border="0">
                    <tr>
                        <td>
                            Email Type:        
                        </td>
                        <td>
                            <asp:DropDownList ID="drpType" runat="server" OnSelectedIndexChanged="drpType_selectedIndexChanged" AutoPostBack="true">
                                <asp:ListItem Text="Standard" Value="standard"></asp:ListItem>
                                <asp:ListItem Text="Return" Value="return"></asp:ListItem>
                                <asp:ListItem Text="Part Complete" Value="partcomplete"></asp:ListItem>
                                <asp:ListItem Text="Part Complete (Final Part)" Value="partcompletefinal"></asp:ListItem>
                                <asp:ListItem Text="Special (Editable)" Value="special"></asp:ListItem>
                                <asp:ListItem Text="No Email" Value="none"></asp:ListItem>
                            </asp:DropDownList>&nbsp;&nbsp;
                        </td>
                        <td>
                            Please choose email type before scanning the tracker.
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Tracker:
                        </td>
                        <td>
                            <asp:TextBox ID="txtTracker" runat="server" ValidationGroup="tracker2" AutoPostBack="true" OnTextChanged="btnTrackerSubmit_click"></asp:TextBox> 
                        </td>
                        <td>
                            Qty:&nbsp;
                            <asp:DropDownList ID="drpTrackerQty" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpTrackerQty_selectedIndexChanged">
                                <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                <asp:ListItem Text="10" Value="10"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <br />
                <asp:Table ID="tblEmail" runat="server" Visible="false">
                    <asp:TableRow>
                        <asp:TableCell>
                            Enter the email your want to send in the box below, then click on the Tracker text field and scan the parcel.<br />
                            <asp:TextBox ID="txtEmail" runat="server" TextMode="MultiLine" Rows="8" Columns="80"></asp:TextBox>    
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>
                        
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <br /><br />
                <asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtTracker" ErrorMessage="You must enter a tracker code" Display="dynamic" ValidationGroup="tracker"></asp:RequiredFieldValidator>
                <asp:HiddenField ID="hidOrderID" runat="server" />
                <asp:HiddenField ID="hidPrefix" runat="server" /> 
                <asp:HiddenField ID="hidAffClickThroughID" runat="server" />
                <asp:HiddenField ID="hidClickThroughAlreadyDone" runat="server" Value="false" />
                <asp:HiddenField ID="hidGoodsTotal" runat="server" />
                <asp:HiddenField ID="hidOrderVatTotal" runat="server" />
                <asp:HiddenField ID="hidShippingTotal" runat="server" />
                <asp:HiddenField ID="hidShipping" runat="server" />
                <asp:HiddenField ID="hidShippingVatRate" runat="server" />
                <asp:HiddenField ID="hidGoodsTotalInc" runat="server" />
                <asp:HiddenField ID="hidOrderTotal" runat="server" />
                <asp:HiddenField ID="hidFirstScan" runat="server" />
                <asp:HiddenField ID="hidAffID" runat="server" />
            </asp:Panel>
            <asp:Label id="lblComplete" runat="server"></asp:Label>
            <script langauge="javascript" type="text/javascript">
            function focusElement(e){
                //document.getElementById("").focus();
                self.setTimeout("document.getElementById('" + e + "').focus();",600);
            }
            </script>
        </ContentTemplate>
    </atlas:UpdatePanel>       
    
    <asp:SqlDataSource ID="sqlRecentOrders" runat="server" SelectCommand="procShopOrderByCountryCodeSelectDist20" SelectCommandType="StoredProcedure" ConnectionString='<%$ ConnectionStrings:connectionString %>'>
        <SelectParameters>
            <asp:Parameter Name="countryCode" Type="string" Size="5" DefaultValue="zz" />
            <asp:ControlParameter ControlID="drpOutstandingQty" PropertyName="selectedValue" Name="qty" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlBillAdd" runat="server" SelectCommand="procShopCustomerShipAddByOrderIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidOrderID" PropertyName="value" Name="orderID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlOrderItems" runat="server" SelectCommand="procShopOrderItemsByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="hidOrderID" PropertyName="value" Name="orderID" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>


