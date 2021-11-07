<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="consultancyJulia.aspx.vb" Inherits="affiliates_consultancyJulia" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp" runat="server"></atlas:ScriptManagerProxy>    
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <table width="100%">
                <tr>
                    <td width="50%" valign="top">
                        <asp:Label ID="lblUsersText" runat="server">
                            <b>User List</b><hr />
                        </asp:Label>
                        <asp:Panel ID="panUsers" runat="server">
                            <asp:GridView ID="gvUsers" runat="server" DataSourceID="sqlUsers" GridLines="none" DataKeyNames="code" AutoGenerateColumns="false" OnSelectedIndexChanged="gvUsers_selectedIndexChanged">
                                <Columns>
                                    <asp:BoundField HeaderText="Name" DataField="name" NullDisplayText="Anon" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Age" DataField="age" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:BoundField HeaderText="Gender" DataField="sex" />
                                    <asp:BoundField ItemStyle-Width="40" />
                                    <asp:CommandField SelectText="View" ShowSelectButton="true" />                      
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>        
                        Show:&nbsp;
                        <asp:DropDownList ID="drpUserType" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="Users waiting for a reply" Value="1"></asp:ListItem>
                            <asp:ListItem Text="All users" Value="%"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td width="50%" valign="top">
                        <asp:Label ID="lblInfoText" runat="server" Visible="false">
                            <b>User Information</b><hr />
                        </asp:Label>
                        <asp:Panel ID="panInfo" runat="server">
                            <asp:FormView ID="fvInfo" runat="server" OnDataBound="fvInfo_dataBound" Width="100%">
                                <ItemTemplate>
                                    <table border="0" id="tblPersonal" runat="server" width="100%">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblnameText" runat="server" Text="<font color='black'>Name:</font>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblName" runat="server" Text='<%# showName(Eval("name")) %>'></asp:Label>
                                                <asp:HiddenField ID="hidCode" runat="server" Value='<%# Eval("code") %>'></asp:HiddenField>
                                            </td>
                                            <td width="80">&nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblRepliesLeftText" runat="server" Text="<font color='black'>Replies Left:</font>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblRepliesLeft" runat="server" Text='<%# Eval("replysLeft") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblStatusText" runat="server" Text="<font color='black'>Status:</font>"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:Label ID="lblStatus" runat="server" Text='<%# showStatus(Eval("userWaiting")) %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblGenderText" runat="server" Text="<font color='black'>Gender:</font>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblGender" runat="server" Text='<%# Eval("sex") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblPGenderText" runat="server" Text="<font color='black'>Partners Gender:</font>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPGender" runat="server" Text='<%# Eval("psex") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblAgeText" runat="server" Text="<font color='black'>Age:</font>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAge" runat="server" Text='<%# showAge(Eval("age")) %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblPAgeText" runat="server" Text="<font color='black'>Partners Age:</font>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPAge" runat="server" Text='<%# Eval("pAge") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblEthnicText" runat="server" Text="<font color='black'>Ethnic background:</font>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEthnic" runat="server" Text='<%# Eval("ethnic") %>'></asp:Label>
                                            </td>
                                            <td>&nbsp;</td>
                                            <td>
                                                <asp:Label ID="lblPEthnicText" runat="server" Text="<font color='black'>Partners Ethnic background:</font>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblPEthnic" runat="server" Text='<%# Eval("pEthnic") %>'></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblRelationshipText" runat="server" Text="<font color='black'>Length of relationship:</font>"></asp:Label>
                                            </td>
                                            <td colspan="4">
                                                <asp:Label ID="lblRelationship" runat="server">
                                                    <%# Eval("lengthYear") %> Years, <%# Eval("lengthMonth") %> months
                                                </asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" id="tblPersonalExtra" runat="server" visible="false" width="100%">
                                        <tr>
                                            <td valign="top">
                                                <asp:Label ID="lblDisabilityText" runat="server" Text="<font color='black'>Disabilities</font>"></asp:Label><br />
                                                <asp:Label ID="lblDisability" runat="server" Text='<%# Eval("disability") %>'></asp:Label><br />
                                                <asp:Label ID="lblPDisabillityText" runat="server" Text="<font color='black'>Partners Disabilities</font>"></asp:Label><br />
                                                <asp:Label ID="lblPDisability" runat="server" Text='<%# Eval("pDisability") %>'></asp:Label><br />
                                                <asp:Label ID="lblMentalText" runat="server" Text="<font color='black'>Health/Mental Problems</font>"></asp:Label><br />
                                                <asp:Label ID="lblMental" runat="server" Text='<%# Eval("mental") %>'></asp:Label><br />
                                                <asp:Label ID="lblPMentalText" runat="server" Text="<font color='black'>Partners Health/Mental Problems</font>"></asp:Label><br />
                                                <asp:Label ID="lblPMental" runat="server" Text='<%# Eval("pMental") %>'></asp:Label><br />
                                            </td>
                                            <td>&nbsp;</td>
                                            <td valign="top" align="right">
                                                    <table border="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label id="lblMarried" runat="server" Text="Married"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="chkMarried" runat="Server" Checked='<%# Eval("married") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:label ID="lblCohabiting" runat="server" Text="Cohabiting"></asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="CheckBox1" runat="Server" Checked='<%# Eval("cohabiting") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:label ID="lblDivorced" runat="server" Text="Divorced"></asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="CheckBox2" runat="Server" Checked='<%# Eval("divorced") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:label ID="lblWidowed" runat="server" Text="Widowed"></asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="CheckBox3" runat="Server" Checked='<%# Eval("widowed") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:label ID="lblSeperated" runat="server" Text="Seperated"></asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="CheckBox4" runat="Server" Checked='<%# Eval("seperated") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:label ID="lblSingle" runat="server" Text="Single"></asp:label>
                                                            </td>
                                                            <td>
                                                                <asp:CheckBox ID="CheckBox5" runat="Server" Checked='<%# Eval("single") %>' />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblOther" runat="Server" text='<%# Eval("other") %>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:LinkButton ID="lnkShowPersonal" runat="server" Text="Show/Hide extra information" OnClick="lnkShowPersonal_click"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:FormView>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <br /><br />
            <asp:Panel ID="panForm" runat="server" Visible="false">
                <asp:Label ID="lblMessageInstructions" runat="server">
                    Type your message to the cusomter in the box below.<br />
                    When you are finished click the 'Send Message' button below.<br />
                </asp:Label>
                <br />
                <table border="0">
                    <tr>
                        <td colspan="2">
                            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="12" Columns="80"></asp:TextBox>        
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnSendMessage" runat="server" Text="Send Message" OnClick="btnSendMessage_click" />
                            <asp:Label ID="lblConfirmText" runat="server" Text="<font color='red'>Please confirm >></font>" Visible="false"></asp:Label><br />
                            <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
                                <ProgressTemplate>
                                    Please Wait....<img src="/images/loading.gif" width="16" height="16" />
                                </ProgressTemplate>
                            </atlas:UpdateProgress>
                        </td>
                        <td id="tdConfirm" runat="server" visible="false" align="right">
                            <asp:Label ID="lblConfirmInstructions" runat="server">
                                <font color="red">Will this use up 1 Reply </font>
                            </asp:Label>
                            <asp:Button id="btnYes" runat="server" Text="Yes" OnClick="messageSend_click" CommandArgument="true" />
                            &nbsp;Or&nbsp;
                            <asp:Button id="btnNo" runat="server" Text="No" OnClick="messageSend_click" CommandArgument="false" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Label ID="lblMsgSendError" runat="server"></asp:Label>
            <asp:Panel ID="panHistory" runat="server" Visible="true">
                <asp:GridView ID="gvHistory" runat="server" GridLines="none" DataSourceID="sqlHistory" DataKeyNames="logID" ShowHeader="false" AutoGenerateColumns="false" EmptyDataText="" OnDataBound="gvHistory_dataBound">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("date") %>' Visible="false"></asp:Label>
                                <asp:Label ID="lblUserSent" runat="server" Text='<%# Eval("userSent") %>' Visible="false"></asp:Label>
                                <table width="100%" border="0">
                                    <tr id="trHeader" runat="server">
                                        <td colspan="2">
                                            <asp:Label ID="lblMsgHeader" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="6%">
                                            &nbsp;
                                        </td>
                                        <td width="94%">
                                            <asp:Label ID="lblMessage" runat="server" Text='<%# Eval("message") %>'></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </atlas:UpdatePanel>
    
    <asp:SqlDataSource ID="sqlUsers" runat="server" SelectCommand="procConsultancyByWaitingSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpUserType" PropertyName="selectedValue" Name="userWaiting" Type="string" Size="1" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlHistory" runat="server" SelectCommand="procConsultancyMessagesByCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvUsers" PropertyName="selectedValue" Name="code" Type="string" Size="10" DefaultValue="0" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

