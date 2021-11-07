<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="productAssembly.aspx.vb" Inherits="maintenance_productAssembly" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='../images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>    
            <asp:MultiView ID="mvBuild" runat="server" ActiveViewIndex="0">
                <asp:View ID="viewSection1" runat="server">
                            Select a product and quantity to build:<br />
                            <asp:Table ID="tblProd" runat="server">
                                <asp:TableRow>
                                    <asp:TableCell>
                                        Master:
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList ID="drpMaster" runat="server" DataSourceID="sqlMaster" DataTextField="name" DataValueField="masterID" AppendDataBoundItems="true" EnableViewState="true" AutoPostBack="true" CausesValidation="false">
                                            <asp:ListItem Text="Select..." Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        Product:
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:DropDownList ID="drpProduct" runat="server" DataSourceID="sqlProduct" DataTextField="productName" DataValueField="productID"></asp:DropDownList>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        Qty:
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtQty" runat="server" MaxLength="4" Width="40" AutoPostBack="false"></asp:TextBox><asp:RequiredFieldValidator ID="req1" runat="server" ControlToValidate="txtQty" ErrorMessage="You must enter a quantity." Display="dynamic"></asp:RequiredFieldValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                                 <asp:TableRow>
                                    <asp:TableCell>
                                        Qty Confirm:
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:TextBox ID="txtQtyConfirm" runat="server" MaxLength="4" Width="40" AutoPostBack="false"></asp:TextBox><asp:RequiredFieldValidator ID="req2" runat="server" ControlToValidate="txtQtyConfirm" ErrorMessage="You must enter a quantity." Display="dynamic"></asp:RequiredFieldValidator><asp:CompareValidator ID="com1" runat="server" ControlToValidate="txtQty" ControlToCompare="txtQtyConfirm" ErrorMessage="* Quantities must match." Display="dynamic" EnableClientScript="false"></asp:CompareValidator>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow>
                                    <asp:TableCell>
                                        &nbsp;
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_click" Text="Check stock" />
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                            <br /><br />
                            <asp:GridView ID="gvComponentList" runat="server" Visible="false" DataSourceID="sqlComponents" EnableViewState="false" EmptyDataText="No data exists." AutoGenerateColumns="false" OnDataBound="gvComponentList_dataBound" Width="80%" GridLines="none">
                                <HeaderStyle Font-Bold="true" />
                                <Columns>
                                    <asp:BoundField HeaderText="Component Type" DataField="name" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:label id="lblStock" runat="server" Text='<%# Eval("currentStock") %>' Visible="false"></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Default Stock">
                                        <ItemTemplate>
                                            <asp:label id="lblDefault" runat="server" Text='<%# Eval("defaultStock") %>'></asp:label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Qty Required" DataField="qty" />    
                                    <asp:BoundField />        
                                </Columns>    
                            </asp:GridView><br /><br />
                                <asp:Table id="tblComments" runat="server" Visible="false">
                                    <asp:TableRow>
                                        <asp:TableCell VerticalAlign="top">
                                            <b>Comments</b>:
                                        </asp:TableCell>
                                        <asp:TableCell>
                                            <asp:TextBox ID="txtComments" MaxLength="500" TextMode="MultiLine" runat="server" Rows="5" Columns="80"></asp:TextBox>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </ContentTemplate>
                        </atlas:UpdatePanel>
                </asp:View>
                <asp:View ID="viewSection2" runat="server">
                    Choose components to use:<br /><br />
                    <asp:GridView ID="gvMasters" runat="server" EnableViewState="true" DataSourceID="sqlMasters" DataKeyNames="masterID" GridLines="none" AutoGenerateColumns="false" OnSelectedIndexChanged="gvMasters_selectedIndexChanged" OnRowDataBound="gvMasters_rowDatabound">
                        <HeaderStyle Font-Bold="true" />
                        <Columns>
                            <asp:BoundField HeaderText="Component Type" DataField="name" />
                            <asp:BoundField HeaderText="Qty Required" DataField="totalQty" />
                            <asp:TemplateField HeaderText="DefaultQty" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDefault" runat="server" Text='<%# replaceNull(Eval("defaultComponentQty")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ShowSelectButton="true" SelectText="Click to set" />
                            <asp:TemplateField HeaderText="DefaultQty" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDefaultID" runat="server" Text='<%# replaceNull(Eval("defaultComponentID")) %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br /><br />
                    <asp:GridView ID="gvComponents" runat="server" DataKeyNames="componentID" ShowFooter="true" DataSourceID="sqlComponents2" AutoGenerateColumns="false" OnDataBound="gvComponents_dataBound" GridLines="none">
                        <HeaderStyle Font-Bold="true" />
                        <FooterStyle Font-Bold="true" />
                        <Columns>
                            <asp:BoundField HeaderText="Component" FooterText="Qty Required" DataField="ComponentName" FooterStyle-VerticalAlign="top" />
                            <asp:BoundField HeaderText="Stock" DataField="CurrentStock" NullDisplayText="0" />
                            <asp:BoundField HeaderText="Manufacturer" DataField="manufacturerName" />
                            <asp:TemplateField HeaderText="Qty to use">
                                <ItemStyle HorizontalAlign="center" />
                                <FooterStyle HorizontalAlign="center" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtQty" runat="server" Width="40" MaxLength="6"></asp:TextBox>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalQty" runat="server"></asp:Label><br />
                                    <asp:Button ID="btnView2Sumbit" runat="server" Text="Submit" OnClick="btnView2Submit_click" />
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:View>
                <asp:View ID="viewSection3" runat="server">
                    Production batch is complete.<br />
                    If the Print Popup didnt open, then click <asp:HyperLink ID="lnkPopup" runat="server" Text="here" Target="_blank"></asp:HyperLink></a>.
                    <span id="prn">
                        <br /><br />
                        <table>
                        <tr>
                            <td>
                                <b>Product:</b>
                            </td>
                            <td>
                                <asp:Label ID="lblProduct" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Ref:</b>
                            </td>
                            <td>
                                <asp:Label ID="lblRef" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Quantity:</b>
                            </td>
                            <td>
                                <asp:Label ID="lblProductQty" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Date:</b>
                            </td>
                            <td>
                                <%=FormatDateTime(Now(), DateFormat.LongDate)%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>User:</b>
                            </td>
                            <td>
                                <%=membership.GetUser.UserName %>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <b>Production Batch ID:</b>
                            </td>
                            <td>
                                <asp:Label ID="lblProductionBatchID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        </table>
                        <br /><br />                
                        <table width="500">
                            <tr>
                                <td>
                                     <asp:Table ID="tblComponentList" runat="server" Width="90%">
                                        <asp:TableRow>
                                            <asp:TableCell>
                                                <b>Component</b>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <b>Manufacturer</b>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <b>Location Bay</b>
                                            </asp:TableCell>
                                            <asp:TableCell>
                                                <b>Qty</b>
                                            </asp:TableCell>
                                        </asp:TableRow>
                                    </asp:Table>
                                </td>
                            </tr>
                        </table>
                        <br />
                        <asp:Table ID="tblCom" runat="server">
                            <asp:TableRow>
                                <asp:TableCell VerticalAlign="top">
                                    <b>Comments</b>:
                                </asp:TableCell>
                                <asp:TableCell>
                                    <asp:Label id="lblComments" runat="server" Width="400"></asp:Label>
                                </asp:TableCell>                    
                            </asp:TableRow>
                        </asp:Table>
                    </span>
                    
                </asp:View>
            </asp:MultiView>
            <br /><br />
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
            <asp:Label ID="lblProdID" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblQty" runat="server" Visible="false"></asp:Label>    
            <asp:Button ID="btnContinue" runat="server" OnClick="btnContinue_click" Text="Continue >" Visible="false"></asp:Button>
        </ContentTemplate>            
    </Atlas:UpdatePanel>
        
    
    
    <asp:SqlDataSource ID="sqlMaster" runat="server" SelectCommand="procProductMastersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlProduct" runat="server" SelectCommand="procProductsByMasterIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpMaster" Name="masterID" Type="int32" PropertyName="selectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlComponents" runat="server" SelectCommand="procComponentsByProductIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpProduct" Name="productID" PropertyName="selectedValue" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlMasters" runat="server" SelectCommand="procComponentMastersByProdIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
           <asp:ControlParameter ControlID="lblProdID" Name="prodID" PropertyName="text" Type="int32" />
           <asp:ControlParameter ControlID="lblQty" Name="buildQty" PropertyName="text" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlComponents2" runat="server" SelectCommand="procComponentsByMasterIDSelect2" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvMasters" Name="masterID" PropertyName="selectedValue" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
    <script language="Javascript" type="text/javascript">
    var win;
    function printPage()
    {
        win=window.open("productAssemblyPrintPop.aspx","assemblyPrintPop");
    }
    function remoteCall()
    {
        alert(document.getElementById("prn").innerHTML);
        win.receiveData(document.getElementById("prn").innerHTML);
    }
    function hideContinueButton()
    {
        //alert("alert!");
        document.getElementById("ctl00_logMaintenance_ContentPlaceHolder1_btnContinue").style.display="none";
    }
    </script>
</asp:Content>

