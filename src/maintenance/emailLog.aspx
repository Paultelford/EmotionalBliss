<%@ Page Title="" Language="VB" MasterPageFile="~/m_affs.master" AutoEventWireup="false" CodeFile="emailLog.aspx.vb" Inherits="maintenance_emailLog" %>
<%@ Register TagPrefix="eb" TagName="DateControl" Src="~/dateControl.ascx" %>
<%@ Register TagPrefix="eb" TagName="Pager" Src="~/Pager.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManagerProxy ID="smp" runat="server"></asp:ScriptManagerProxy>
    
            <table>
                <tr>
                    <td>
                        Date:
                    </td>
                    <td></td>
                    <td>
                        <eb:DateControl id="date1" runat="Server" ></eb:DateControl>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        Profile:
                    </td>
                    <td></td>
                    <td>
                        <asp:DropDownList ID="drpProfile" runat="server" DataSourceID="sqlProfile" DataTextField="name" DataValueField="profile_id" AutoPostBack="true" OnDataBound="drpProfile_dataBound"></asp:DropDownList>
                    </td>
                </tr>
                 <tr>
                    <td>
                        Type:
                    </td>
                    <td></td>
                    <td>
                        <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="All" Value="all"></asp:ListItem>
                            <asp:ListItem Text="Sent" Value="sent"></asp:ListItem>
                            <asp:ListItem Text="Unsent" Value="unsent"></asp:ListItem>
                            <asp:ListItem Text="Failed" Value="failed" Selected="True"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:GridView ID="gvProfile" runat="server" DataSourceID="sqlEmail" AutoGenerateColumns="false" DataKeyNames="mailitem_id" EmptyDataText="No items found." AllowPaging="true" PageSize="20" PagerSettings-Visible="false" Width="99%" HeaderStyle-HorizontalAlign="Left" OnSelectedIndexChanged="gvProfile_selectedIndexChanged">
                <Columns>
                    <asp:BoundField HeaderText="Request Date" DataField="send_request_date" DataFormatString="{0:dd MMM yyyy HH:mm:ss}" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Status" DataField="sent_status" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Sent Date" DataField="sent_date" DataFormatString="{0:dd MMMM yyyy HH:mm:ss}" />
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:TemplateField HeaderText="Recipient">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRecipient" runat="server" Text='<%# Eval("recipients") %>' CommandName="select"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="20" />
                    <asp:BoundField HeaderText="Subject" DataField="subject" />            
                </Columns>
            </asp:GridView>
            <eb:Pager ID="pager1" runat="server" />
            <br /><br />
            <a id="m"></a>
            <asp:DetailsView ID="dvEmail" runat="server" DataSourceID="sqlSingleEmail" AutoGenerateRows="false">
                <Fields>
                    <asp:BoundField HeaderText="Recipients" DataField="recipients" HeaderStyle-Width="100" />
                    <asp:BoundField HeaderText="Subject" DataField="subject" />
                    <asp:TemplateField HeaderText="Body" HeaderStyle-VerticalAlign="Top">
                        <ItemTemplate>
                            <asp:Label ID="lblBody" runat="server" Text='<%# Eval("body") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField HeaderText="Format" DataField="body_format" />
                    <asp:BoundField HeaderText="Date Added" DataField="send_request_date" DataFormatString="{0:dd MMMM yyyy HH:mm:ss}" />
                    <asp:BoundField HeaderText="Send Status" DataField="sent_status" />
                    <asp:BoundField HeaderText="Sent Date" DataField="sent_date" DataFormatString="{0:dd MMMM yyyy HH:mm:ss}" />        
                </Fields>
            </asp:DetailsView>
        
    
    <asp:SqlDataSource ID="sqlProfile" runat="server" SelectCommand="procProfileSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:msdb %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlEmail" runat="server" SelectCommand="procEmailByProfileIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:msdb %>" OnSelecting="sqlEmail_selecting">
        <SelectParameters>
            <asp:Parameter Name="profileID" Type="Int16" />
            <asp:ControlParameter Name="type" Type="String" ControlID="drpType" PropertyName="selectedValue" />
            <asp:ControlParameter Name="startDate" Type="DateTime" ControlID="date1" PropertyName="getStartDate" />
            <asp:ControlParameter Name="endDate" Type="DateTime" ControlID="date1" PropertyName="getEndDate" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlSingleEmail" runat="server" SelectCommand="procEmailByMailItemIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:msdb %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvProfile" PropertyName="selectedValue" Name="mailitem_id" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

