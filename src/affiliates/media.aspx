<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" ValidateRequest="false" CodeFile="media.aspx.vb" Inherits="affiliates_media" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />

            Select Article:&nbsp;
            <asp:DropDownList id="drpArticle" runat="server" DataSourceID="sqlArticles" DataTextField="title" DataValueField="id" OnDataBinding="drpArticle_dataBinding" OnSelectedIndexChanged="drpArticle_selectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true">
            </asp:DropDownList>
            <br /><br />
            <table width="100%">
                <tr>
                    <td valign="top" width="40%">
                        <asp:DetailsView ID="dvArticle" DataKeyNames="id" runat="server" DataSourceID="sqlArticle" CellPadding="4" CellSpacing="4" DefaultMode="Insert" GridLines="None" HeaderStyle-Width="200" AutoGenerateRows="false" OnDataBound="dvArticle_dataBound">
                            <Fields>
                                <asp:TemplateField HeaderText="Article Title:">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Text='<%# Bind("title") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator id="reqTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtTitle" runat="server" MaxLength="100" Text='<%# Bind("title") %>'></asp:TextBox>
                                        <asp:RequiredFieldValidator id="reqTitle" runat="server" ControlToValidate="txtTitle" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date:">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDate" runat="server" Text='<%# Bind("date","{0:d MMMM yyyy}") %>' MaxLength="50" /><img alt="Icon" src="/Images/Calendar_scheduleHS.png" id="imgCal" />
                                        <ajaxToolkit:CalendarExtender id="cal" runat="Server" TargetControlID="txtDate" Animated="true" Format="d MMMM yyyy" PopupButtonID="imgCal"></ajaxToolkit:CalendarExtender>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtDate" runat="server" Text='<%# Bind("date","{0:d MMMM yyyy}") %>' MaxLength="50" /><img alt="Icon" src="/Images/Calendar_scheduleHS.png" id="imgCal" />
                                        <ajaxToolkit:CalendarExtender id="cal" runat="Server" TargetControlID="txtDate" Animated="true" Format="d MMMM yyyy" PopupButtonID="imgCal"></ajaxToolkit:CalendarExtender>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description:">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="200" Text='<%# Bind("description") %>' Width="200"></asp:TextBox>
                                        <asp:RequiredFieldValidator id="reqDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtDescription" runat="server" MaxLength="200" Text='<%# Bind("description") %>' Width="200"></asp:TextBox>
                                        <asp:RequiredFieldValidator id="reqDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Link Type:">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="drpLinkType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpLinkType_selectedIndexChanged" selectedValue='<%# Bind("type") %>'>
                                            <asp:ListItem text="Select..." Value=""></asp:ListItem>
                                            <asp:ListItem Text="Image" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="URL/Link" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Full HTML" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqLinkType" runat="server" ControlToValidate="drpLinkType" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:DropDownList ID="drpLinkType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpLinkType_selectedIndexChanged" selectedValue='<%# Bind("type") %>'>
                                            <asp:ListItem text="Select..." Value=""></asp:ListItem>
                                            <asp:ListItem Text="Image" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="URL/Link" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="Full HTML" Value="3"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="reqLinkType" runat="server" ControlToValidate="drpLinkType" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="URL:">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtURL" runat="server" MaxLength="255" Text='<%# Bind("url") %>' Width="200"></asp:TextBox>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:TextBox ID="txtURL" runat="server" MaxLength="255" Text='<%# Bind("url") %>' Width="200"></asp:TextBox>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New Image:" HeaderStyle-VerticalAlign="Top">
                                    <EditItemTemplate>
                                        <asp:FileUpload ID="fImage" runat="server" />     
                                        <br/><asp:HyperLink ID="lnkImage" Target="_blank" NavigateUrl='<%# "/images/media/" & Eval("image") %>' runat="server"><asp:Label ID="lblfImage" ForeColor="Blue" Text='<%# Eval("image") & "&nbsp;" %>' runat="server"></asp:Label></asp:HyperLink><font size="-2">(465px × 658px)</font>                                   
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:FileUpload ID="fImage" runat="server" />
                                        <asp:RequiredFieldValidator ID="reqImage" runat="server" ControlToValidate="fImage" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <br/><font size="-2">(465px × 658px)</font>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="New Thumbnail:" HeaderStyle-VerticalAlign="Top">
                                    <EditItemTemplate>
                                        <asp:FileUpload ID="fThumb" runat="server" />
                                        <br/><asp:HyperLink ID="lnkThumb" Target="_blank" NavigateUrl='<%# "/images/media/" & Eval("thumb") %>' runat="server"><asp:Label ID="lblfThumb" ForeColor="blue" Text='<%# Eval("thumb") & "&nbsp;" %>' runat="server"></asp:Label></asp:HyperLink><font size="-2">(88px x 122px)</font>
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:FileUpload ID="fThumb" runat="server" />
                                        <asp:RequiredFieldValidator ID="reqThumb" runat="server" ControlToValidate="fThumb" ErrorMessage="* Required" Display="Dynamic"></asp:RequiredFieldValidator>
                                        <br/><font size="-2">(88px x 122px)</font>
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                                <asp:CheckBoxField HeaderText="Active:" DataField="active" />
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" CommandName="update" Text="Update" />
                                        <asp:HiddenField ID="hidHtml" runat="server" Value='<%# Eval("html") %>' />
                                    </EditItemTemplate>
                                    <InsertItemTemplate>
                                        <asp:Button ID="btnUpdate" runat="server" CommandName="insert" Text="Add" />
                                    </InsertItemTemplate>
                                </asp:TemplateField>
                            </Fields>
                        </asp:DetailsView>
                    </td>
                    <td valign="top" id="tdEditor" runat="server" visible="false" width="60%">
                        Enter the Articles HTML here<br />
                        <fckeditorv2:FCKeditor ID="fck1" runat="server" ToolbarStartExpanded="false" Width="100%" Height="400" BasePath="~/EBEditor/"></fckeditorv2:FCKeditor>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label>
            <br />
        
    
    
    <asp:SqlDataSource ID="sqlArticles" runat="server" SelectCommand="procMediaSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlArticle" runat="server" SelectCommand="procMediaByIDSelect" SelectCommandType="StoredProcedure" UpdateCommand="procMediaByIDUpdate" UpdateCommandType="StoredProcedure" InsertCommand="procMediaInsert" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString%>" OnInserted="sqlArticel_inserted" OnUpdated="sqlArticle_updated">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpArticle" PropertyName="selectedValue" Name="id" Type="Int32" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="title" Type="String" Size="100" />
            <asp:Parameter Name="date" Type="DateTime" />
            <asp:Parameter Name="description" Type="String" Size="100" />
            <asp:Parameter Name="type" Type="Int16" />
            <asp:Parameter Name="URL" Type="String" Size="255" />
            <asp:Parameter Name="active" Type="Boolean" />
            <asp:ControlParameter ControlID="fck1" PropertyName="value" Name="html" Type="String" Size="-1" />
            <asp:Parameter Name="id" Type="Int32" Direction="Output" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="id" Type="Int32" />
            <asp:Parameter Name="title" Type="String" Size="100" />
            <asp:Parameter Name="date" Type="DateTime" />
            <asp:Parameter Name="description" Type="String" Size="100" />
            <asp:Parameter Name="type" Type="Int16" />
            <asp:Parameter Name="URL" Type="String" Size="255" />
            <asp:Parameter Name="active" Type="Boolean" />
            <asp:ControlParameter ControlID="fck1" PropertyName="value" Name="html" Type="String" Size="-1" />
            <asp:Parameter Name="id" Type="Int32" Direction="Output" />
        </UpdateParameters>
    </asp:SqlDataSource>
</asp:Content>

