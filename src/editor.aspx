<%@ Page Language="VB" MasterPageFile="~/maffs.master" ValidateRequest="false" AutoEventWireup="false" CodeFile="editor.aspx.vb" Inherits="editor" title="Page Editor" %>
<%@ Register TagPrefix="fckeditorv2" Namespace="FredCK.FCKeditorV2" Assembly="FredCK.FCKeditorV2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <center>
    <asp:ScriptManagerProxy ID="smp1" runat="server"></asp:ScriptManagerProxy>
    <asp:UpdateProgress ID="up1" runat="server" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </asp:UpdateProgress>    
    <asp:UpdatePanel id="update1" runat="server">
        <ContentTemplate>
            <asp:HiddenField id="hidUpClick" runat="server"></asp:HiddenField>
            <asp:HiddenField id="hidDownClick" runat="server"></asp:HiddenField>
            <asp:Label id="lblTesting" runat="server"></asp:Label>
            <table border="0" width="930">
                <tr>
                    <td align="left" colspan="2">
                        <a href="http://docs.fckeditor.net/FCKeditor_2.x/Users_Guide" target="_blank"><b>Editor User Guide</b></a>
                    </td>
                </tr>
                <tr>
                    <td align="left" colspan="2">
                        <FCKeditorV2:FCKeditor id="FCKeditor1" runat="server" BasePath="~/EBEditor/" Width="100%" UseBROnCarriageReturn="true" EnableSourceXHTML="true" Height="300" />
                        <table border="0" width="930">
                            <tr>
                                <td align="right">
                                    Type of page to edit:&nbsp;
                                    <asp:DropDownList ID="drpPageType" runat="server" OnSelectedIndexChanged="drpPageType_selectedIndexChanged" AutoPostBack="true">
                                        <asp:ListItem Text="Static Pages" Value="static"></asp:ListItem>
                                        <asp:ListItem Text="Individual Pages" Value="individual"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <asp:Label ID="lblEditing" runat="server"></asp:Label>
                        <br />
                        <asp:Label ID="lblError" runat="server" ForeColor="red"></asp:Label><br />
                        <asp:Label ID="lblComplete" runat="server" Font-Bold="false"></asp:Label>
                        <br />  
                    </td>
                </tr>
                <tr>
                    <td valign="top" width="500" align="left">
                <!-- New controls -->
                        <asp:Panel ID="panNewControls" runat="server" Visible="false">
                            <table width="100%">
                                <tr>
                                    <td width="200">
                                        <b>Page Name:</b>        
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="drpIndividualPage" runat="server" DataTextField="page" DataValueField="page" OnSelectedIndexChanged="drpIndividualPage_selectedIndexChanged" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="drpIndividualPage_dataBinding"> 
                                            <asp:ListItem Text="Please Choose...." Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom">
                                        If the page you want to edit does not exist in the dropdown, then enter its name:
                                    </td>
                                    <td valign="bottom">
                                        <asp:TextBox ID="txtPageSearch" runat="server" MaxLength="200" OnTextChanged="txtPageSearch_textChanged" AutoPostBack="true"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>&nbsp;</td>
                                    <td>
                                        <asp:Label ID="lblPageSearchError" runat="server" ForeColor="red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
               <!-- Std controls -->
                        <asp:Panel ID="panControls" runat="server" Visible="true">
                            <table border="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <table border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblPageNameText" runat="server" Font-Bold="true" Text="Page Name:"></asp:Label>
                                            </td>
                                            <td colspan="2">
                                                <asp:DropDownList ID="drpPage" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpPage_selectedIndexChanged">
                                                </asp:DropDownList><br />
                                                <asp:HiddenField ID="hidStatic" runat="server" />
                                                <asp:HiddenField ID="hidMenuType" runat="server" />
                                                <asp:HiddenField ID="hidName" runat="server" />
                                                <asp:HiddenField ID="hidURL" runat="server" />
                                            </td>
                                        </tr>
                                        <tr id="trEdit1" runat="server" visible="false">
                                            <td>
                                                <asp:Label id="lblCurrentMenuText" Font-Bold="true" runat="server" Text="Current Menu:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="drpCurrentMenu" runat="server" DataSourceID="sqlDepartments" DataTextField="menuName" DataValueField="menuName" OnSelectedIndexChanged="drpCurrentMenu_selectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trEdit2" runat="server" visible="false">
                                            <td>
                                                <asp:Label id="lblShowInMenuText" Font-Bold="true" runat="server" Text="Show on Menu:"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkShowOnMenuEdit" runat="server" OnCheckedChanged="chkShowOnMenuEdit_checkedChanged" AutoPostBack="true" />
                                            </td>
                                        </tr>
                                        <tr id="trEdit3" runat="server" visible="false">
                                            <td colspan="2">
                                                <asp:LinkButton ID="lnkBtnEditPageName" runat="server" Text="Edit Page Name" OnClick="lnkBtnEditPageName_click"></asp:LinkButton>
                                                <br />
                                                <asp:Table ID="tblEditName" runat="server" Visible="false">
                                                <asp:TableRow>
                                                    <asp:TableCell>
                                                        <asp:Label ID="lblNewPageNameEditText" runat="server" Text="Enter new Page Name:"></asp:Label>
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:TextBox ID="txtNewPageNameEdit" runat="server" MaxLength="50" ValidationGroup="editPagename"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtNewPageNameEdit" runat="server" ControlToValidate="txtNewPageNameEdit" ValidationGroup="editPagename" ErrorMessage="* Required" Display="dynamic"></asp:RequiredFieldValidator>
                                                    </asp:TableCell>
                                                    <asp:TableCell>
                                                        <asp:Button ID="btnSubmitNewPageName" runat="server" text="Save" OnClick="btnSubmitNewPageName_click" ValidationGroup="editPagename" />
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                                </asp:Table>
                                            </td>
                                        </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table>
                            <tr>
                                <td colspan="3" id="tdButtons2" runat="server" align="center">
                                    <asp:Button id="btnSubmit" runat="server" Text="Save Changes" onclick="btnSubmit_click" Visible="false" /><br />
                                    <asp:Button ID="btnView" runat="server" Text="Preview Page" OnClick="btnView_click" Visible="false" /><br />
                                    <asp:Button ID="btnViewUser" runat="server" Text="Show Text to Edit" OnClick="btnViewUser_click" Visible="false" /><br />
                                    <asp:Button ID="btnNavigae" runat="server" Text="Navigate To Page" OnClick="btnNavigate_click" Visible="false" />
                                    <asp:Button ID="btnAdd" runat="server" Text="Add New Page" OnClick="btnAdd_click" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" runat="server" visible="false" id="tdNewPage">
                                    New pagename (This will show in the lefthand menu)<br />
                                    <asp:TextBox id="txtNewPageName" runat="server" MaxLength="50" AutoPostBack="false"></asp:TextBox><asp:Button ID="btnNewPageSubmit" runat="server" Text="Create" OnClick="btnNewPageSubmit_click" ValidationGroup="newp" /><br />
                                    <asp:RequiredFieldValidator Display="dynamic" ID="reqTxtNewPageName" runat="server" ErrorMessage="Please enter a new page name.<br>" ControlToValidate="txtNewPageName" ValidationGroup="newp"></asp:RequiredFieldValidator>
                                    Section: <asp:DropDownList ID="drpMenuType" runat="server" DataSourceID="sqlDepartments" DataTextField="menuName" DataValueField="menuName">
                                    </asp:DropDownList><br />
                                    Show on Menu: <asp:CheckBox ID="chkShowOnMenu" runat="server" /><br />
                                    <asp:Label ID="lblNewPageError" runat="server" ForeColor="red"></asp:Label><br />
                                    <asp:Label ID="lblNewPageCriticalError" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="top" align="right">&nbsp;
                        <asp:Panel ID="panEdit" runat="server" Visible="false">
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left" width="100%">    
                                        <b><asp:Label ID="lblPageName" runat="server"></asp:Label></b>
                                        <asp:Panel ID="panHTML" runat="server" BorderWidth="1" Visible="false">
                                            <table border="0" width="100%" cellpadding="6">
                                                <tr>
                                                    <td width="100%">
                                                        <asp:Label ID="lblHTML" runat="server"></asp:Label>        
                                                    </td>
                                                </tr>
                                            </table>			                                      
                                        </asp:Panel>       
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
                <asp:PostBackTrigger ControlID="btnSubmit" /> 
        </Triggers> 
    </asp:UpdatePanel>    
    </center>    
    <asp:SqlDataSource ID="sqlDepartments" runat="server" ConnectionString="<%$ ConnectionStrings:connectionString %>" SelectCommand="procImageMapsByCountryCodeParentSelect" SelectCommandType="StoredProcedure">
        <SelectParameters>
            <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Size="5" Name="countryCode" Type="string" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlIndividualPage" runat="server" SelectCommand="procSiteMenusPageSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

