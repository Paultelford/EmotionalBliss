<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="deptAdd.aspx.vb" Inherits="maintenance_deptAdd" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="Server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="1000" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src='/images/loading.gif' width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Current Departments:&nbsp;
            <asp:DropDownList ID="drpDepartments" runat="Server" DataSourceID="SqlDepartments" AppendDataBoundItems="true" AutoPostBack="true" DataTextField="name" DataValueField="deptid" OnSelectedIndexChanged="drpDepartment_selectedIndexChanged">
                <asp:ListItem Text="Add New" Value="0"></asp:ListItem>
            </asp:DropDownList>
            <br /><br />
            
            <asp:DetailsView ID="dvDept" runat="server" DefaultMode="insert" DataSourceID="SqlDept" DataKeyNames="deptid" AutoGenerateRows="false" GridLines="none" OnItemInserted="dvDept_inserted" OnDataBound="dvDept_dataBound" OnItemUpdated="dvDept_itemUpdated">
                <HeaderStyle Font-Bold="true" VerticalAlign="top" />
                <Fields>
                    <asp:TemplateField HeaderText="Department:" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="top">
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtDepartment" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDepartment" runat="server" Text='<%# Bind("deptName") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Description:" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="top">
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="3" Columns="40" runat="server" Text='<%# Bind("deptDescription") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDescription" TextMode="multiLine" Rows="3" Columns="40" runat="server" Text='<%# Bind("deptDescription") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Country:" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="top">
                        <InsertItemTemplate>
                            <asp:DropDownList ID="drpCountryCode" runat="server" selectedValue='<%# Bind("deptCountry") %>' EnableViewState="true"></asp:DropDownList>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblCounty" runat="server" Text='<%# Eval("deptCountry") %>'></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Image Filename:" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="top">
                        <InsertItemTemplate>
                            <asp:TextBox ID="txtImage" runat="server" Text='<%# Bind("deptImage") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtImage" runat="server" Text='<%# Bind("deptImage") %>'></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Active:" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="top">
                        <InsertItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("deptActive") %>' />
                        </InsertItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkActive" runat="server" Checked='<%# Bind("deptActive") %>' />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ButtonType="Button" UpdateText="Update" InsertText="Insert" ShowInsertButton="True" ShowEditButton="true" ShowCancelButton="false" />
                </Fields>
            </asp:DetailsView>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    
    <asp:SqlDataSource ID="SqlDepartments" runat="server" SelectCommand="procDeptByCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnSelecting="SqlDepartments_selecting">
        <SelectParameters>
            <asp:Parameter Name="countryCode" Type="string" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDept" runat="server" SelectCommand="procDeptByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" InsertCommand="procDeptInsert" InsertCommandType="StoredProcedure" UpdateCommand="procDeptByIDUpdate" UpdateCommandType="StoredProcedure" DeleteCommand="procDepyByIDDelete" DeleteCommandType="StoredProcedure">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpDepartments" Name="deptID" Type="int32" PropertyName="selectedValue" />
        </SelectParameters>
        <UpdateParameters>
            <asp:Parameter Name="deptName" Type="string" Size="50" />
            <asp:Parameter Name="deptDescription" Type="string" Size="4000" />
            <asp:Parameter Name="deptImage" Type="string" Size="30" />  
            <asp:Parameter Name="deptActive" Type="boolean" />          
        </UpdateParameters>
        <InsertParameters>
            <asp:Parameter Name="deptName" Type="string" Size="50" />
            <asp:Parameter Name="deptDescription" Type="string" Size="4000" />
            <asp:Parameter Name="deptCountry" Type="string" Size="5" />
            <asp:Parameter Name="deptImage" Type="string" Size="30" />  
            <asp:Parameter Name="deptActive" Type="boolean" />          
        </InsertParameters>
        <DeleteParameters>
            <asp:Parameter Name="deptID" Type="int32" />
        </DeleteParameters>
    </asp:SqlDataSource>
</asp:Content>

