<%@ Page Language="VB" MasterPageFile="~/m_site.master" Trace="false" Theme="" AutoEventWireup="false" CodeFile="consultancy.aspx.vb" Inherits="consultancy" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="home" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            <h2>Emotional Bliss Consultancy with Julia Cole</h2><br /><br />
            <asp:Panel ID="panLogin" runat="server">
                <asp:Label ID="lblLoginInstructions" runat="server">
                    To start your session please enter your Consultancy ID and surname below:
                </asp:Label>
                <br />
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblCode" runat="server" Text="Consultancy ID:"></asp:Label>    
                        </td>
                        <td>
                            <asp:TextBox ID="txtCode" runat="server" MaxLength="8" ValidationGroup="login"></asp:TextBox><asp:RequiredFieldValidator ID="reqTxtCode" runat="server" ControlToValidate="txtCode" ErrorMessage="* Required" Display="dynamic" ValidationGroup="login"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSurname" runat="server" Text="Surname:"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox id="txtSurname" runat="server" MaxLength="50" ValidationGroup="login" /><asp:RequiredFieldValidator ID="reqTxtSurname" runat="server" ControlToValidate="txtSurname" ErrorMessage="* Required" Display="dynamic" ValidationGroup="login"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnLogin" runat="server" text="Login" ValidationGroup="login" OnClick="btnLogin_click" />
                        </td>
                    </tr>
                </table>
                
            </asp:Panel>
            <asp:Label ID="lblLoginError" runat="server"></asp:Label>
            <asp:Panel ID="panFirstTime" runat="server" Visible="false" width="100%">
                <asp:Label ID="lblFirstTimeOnlyText" runat="server" CssClass="welcometext">
                    Congratulations, you have now activated you Consultancy Voucher.<br />
                    As this is your first visit we would like to ask you a few questions.<br />
                    These questions are completely optional, if you would rather not answer them then just click the <b>'Submit'</b> button below without entering any details, you will not be asked these questions again.<br />
                    But the information you supply could be very helpful to Julia during the Consultation period.<br /><br />
                </asp:Label>
                
                    <asp:DetailsView ID="dvPersonal" HeaderStyle-CssClass="sitetext" runat="server" CellSpacing="6" Width="100%" BorderWidth="0" DefaultMode="Insert" GridLines="None" DataSourceID="sqlPersonal" OnItemInserting="dvPersonal_inserting" AutoGenerateRows="false">
                        <HeaderStyle Width="60" Wrap="true" VerticalAlign="top" Font-Bold="true" />
                        <Fields>
                            <asp:TemplateField HeaderText="Name">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="50" Text='<%# Bind("name") %>'></asp:TextBox>
                                    &nbsp;
                                    <font size="-2">*Does not have to be your real name</font>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Your Gender">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpSex" runat="server" selectedValue='<%# Bind("sex") %>'>
                                        <asp:ListItem Text="Rather not say" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partners Gender">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpPSex" runat="server" selectedValue='<%# Bind("pSex") %>'>
                                        <asp:ListItem Text="Rather not say" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="Male" Value="Male"></asp:ListItem>
                                        <asp:ListItem Text="Female" Value="Female"></asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Your Age">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpAge" runat="server" selectedValue='<%# Bind("age") %>' OnInit="drpAge_init"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partners Age">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpPAge" runat="server" selectedValue='<%# Bind("pAge") %>' OnInit="drpAge_init"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Length of relationship">
                                <ItemTemplate>
                                    Years: <asp:DropDownList ID="drpYears" selectedValue='<%# Bind("lengthYear") %>' runat="server" AppendDataBoundItems="false" OnLoad="drpYears_load"></asp:DropDownList>
                                    &nbsp;&nbsp;
                                    Months <asp:DropDownList ID="drpMonths" selectedValue='<%# Bind("lengthMonth") %>' runat="server" AppendDataBoundItems="false" OnLoad="drpMonths_load"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Children living at home">
                                <ItemTemplate>
                                    <asp:DropDownList ID="drpChildren" runat="server" selectedValue='<%# bind("children") %>' OnLoad="drpChildren_load"></asp:DropDownList>                                
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-VerticalAlign="top" HeaderText="Tick which apply">
                                <ItemTemplate>
                                    <table border="0">
                                        <tr>
                                            <td>
                                                Married
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkMarried" runat="server" Checked='<%# bind("married") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Cohabiting
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkCohabiting" runat="server" Checked='<%# bind("cohabiting") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Divorced
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkDivorced" runat="server" Checked='<%# bind("divorced") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Widowed
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkWidowed" runat="server" Checked='<%# bind("widowed") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Seperated
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkSeperated" runat="server" Checked='<%# bind("seperated") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Single
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkSingle" runat="server" Checked='<%# bind("single") %>' />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Other
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOther" runat="server" Text='<%# Bind("other") %>' MaxLength="2000"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Your ethnic group" DataField="ethnic" />
                            <asp:BoundField HeaderText="Partners ethnic group" DataField="pEthnic" />
                            <asp:TemplateField HeaderText="Your disabilities" HeaderStyle-VerticalAlign="top">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtDisability" text='<%# Bind("disability") %>' CssClass="normaltextarea" runat="server" TextMode="MultiLine" Rows="4" Width="300"></asp:TextBox>
                                    <br />
                                    <font size="-2">Max 4000 characters. <asp:Label ID="lblDisabilityUsed" runat="server"></asp:Label></font>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partners disabilities" HeaderStyle-VerticalAlign="top">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPDisability" text='<%# Bind("pdisability") %>' CssClass="normaltextarea" runat="server" TextMode="MultiLine" Rows="4" Width="300"></asp:TextBox>
                                    <br />
                                    <font size="-2">Max 4000 characters. <asp:Label ID="lblPDisabilityUsed" runat="server"></asp:Label></font>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Your Physical/Mental conditions" HeaderStyle-VerticalAlign="top">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtMental" text='<%# Bind("mental") %>' runat="server" CssClass="normaltextarea" TextMode="MultiLine" Rows="4" Width="300"></asp:TextBox>
                                    <br />
                                    <font size="-2">Max 4000 characters. <asp:Label ID="lblMentalUsed" runat="server"></asp:Label></font>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Partners Physical/Mental conditions" HeaderStyle-VerticalAlign="top">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtPMental" runat="server" text='<%# Bind("pmental") %>' CssClass="normaltextarea" TextMode="MultiLine" Rows="4" width="300"></asp:TextBox>
                                    <br />
                                    <font size="-2">Max 4000 characters. <asp:Label ID="lblPMentalUsed" runat="server"></asp:Label></font>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <InsertItemTemplate>
                                    <asp:Button ID="btnInsert" runat="server" Text="Submit" CommandName="insert" />
                                </InsertItemTemplate>
                                <EditItemTemplate>
                                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CommandName="update" />
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Fields>
                    </asp:DetailsView>
                
                <asp:Label ID="lblTextOverflowError" runat="server"></asp:Label>
            </asp:Panel>
            <asp:Panel ID="panForm" runat="server" Visible="false">
                <asp:FormView ID="fvForm" runat="server" OnDataBound="fvForm_dataBound">
                    <ItemTemplate>
                        <table border="0">
                            <tr>
                                <td>
                                    <asp:label ID="lblConID" runat="server" Font-Bold="true" Text="Consultancy ID:"></asp:label>
                                </td>
                                <td width="40">&nbsp;</td>
                                <td>
                                    <asp:Label ID="lblCode" runat="server" Text='<%# Eval("code") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:label ID="lblRepPurch" runat="server" Font-Bold="true" Text="Replys Purchased:"></asp:label>
                                </td>
                                <td width="40">&nbsp;</td>
                                <td>
                                    <asp:Label ID="lblReplys" runat="server" Text='<%# Eval("replys") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:label ID="lblRepLeft" runat="server" Font-Bold="true" Text="Replys Left:"></asp:label>
                                </td>
                                <td width="40">&nbsp;</td>
                                <td>
                                    <asp:Label ID="lblReplysLeft" runat="server" Text='<%# Eval("replysLeft") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:label ID="lblStatusText" runat="server" Font-Bold="true" Text="Status:"></asp:label>
                                </td>
                                <td width="40">&nbsp;</td>
                                <td>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("status") %>'></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkProfile" runat="server" Text="View/Edit Your Profile" OnClick="lnkProfile_click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:FormView>
                <br /><br />
                <asp:Label ID="lblFormInstructions" runat="server">
                    Enter your message for Julia in the box below.<br />
                    When you are done click the 'Send Message' button.<br />
                    Dont worry if you forgot to tell her something. You can send as many messages as you like.<br /><br />
                    <table border="0" id="tblEntry" runat="server">
                        <tr>
                            <td align="right" colspan="2">
                                <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" CssClass="normaltextarea" Rows="12" Width="600"></asp:TextBox><br />  
                                Maximum 500 words per question
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
                                    <ProgressTemplate>
                                        Please Wait....<img src="/images/loading.gif" width="16" height="16" />
                                    </ProgressTemplate>
                                </atlas:UpdateProgress>
                            </td>
                            <td align="right">
                                <asp:Button ID="btnSendMessage" runat="server" Text="Send Message" OnClick="btnSendMessage_click" />
                            </td>
                        </tr>
                    </table>
                </asp:Label>
                <br />
            </asp:Panel>
            <asp:Panel ID="panMostRecent" runat="server" Visible="false">
                <asp:Label id="lblMostRecentText" runat="server" Text="Most recent message sent/recieved:"></asp:Label><br />
                <table border="0">
                    <tr>
                        <td width="40">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblMostRecent" runat="server"><font color='blue'>No messages have been sent yet</font></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:LinkButton ID="lnkShowHistory" runat="server" Text="<br>Show/Hide message history" OnClick="lnkShowHistory_click"></asp:LinkButton>
            </asp:Panel>
            <asp:Label ID="lblMsgSendError" runat="server"></asp:Label>
            <asp:Panel ID="panHistory" runat="server" Visible="false">
                <asp:LinkButton ID="lnkShowHistory2" runat="server" Text="<br>Show/Hide message history" OnClick="lnkShowHistory_click"></asp:LinkButton>
                <asp:GridView ID="gvHistory" runat="server" GridLines="none" DataSourceID="sqlHistory" DataKeyNames="logID" ShowHeader="false" AutoGenerateColumns="false" EmptyDataText="No messages have been sent yet" OnDataBound="gvHistory_dataBound">
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
        
    <asp:SqlDataSource ID="sqlPersonal" runat="server" SelectCommand="procConsultancyByCodeSelect" SelectCommandType="StoredProcedure" InsertCommand="procConsultancyByCodeUpdate" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" OnInserted="sqlPersonal_inserted" UpdateCommand="procConsultancyByCodeUpdate" UpdateCommandType="StoredProcedure" OnUpdated="sqlPersonal_inserted">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtCode" PropertyName="text" Name="code" Type="string" Size="10" />
        </SelectParameters>
        <InsertParameters>
            <asp:ControlParameter ControlID="txtCode" PropertyName="text" Name="code" Type="string" Size="10" />
            <asp:Parameter Name="name" Type="string" size="50" />
            <asp:Parameter Name="sex" Type="string" size="50" />
            <asp:Parameter Name="age" Type="int32" />
            <asp:Parameter Name="pSex" Type="string" Size="50" />
            <asp:Parameter Name="pAge" Type="int32" />
            <asp:Parameter Name="lengthYear" Type="int16" />
            <asp:Parameter Name="lengthMonth" Type="int16" />
            <asp:Parameter Name="children" Type="int16" />
            <asp:Parameter Name="married" Type="boolean" />
            <asp:Parameter Name="cohabiting" Type="boolean" />
            <asp:Parameter Name="divorced" Type="boolean" />
            <asp:Parameter Name="widowed" Type="boolean" />
            <asp:Parameter Name="seperated" Type="boolean" />
            <asp:Parameter Name="single" Type="boolean" />
            <asp:Parameter Name="other" Type="string" Size="2000" />
            <asp:Parameter Name="ethnic" Type="string" Size="50" />
            <asp:Parameter Name="pEthnic" Type="string" Size="50" />
            <asp:Parameter Name="disability" Type="string" Size="4000" />
            <asp:Parameter Name="pDisability" Type="string" Size="4000" />
            <asp:Parameter Name="mental" Type="string" Size="4000" />
            <asp:Parameter Name="pMental" Type="string" Size="4000" />
        </InsertParameters>
        <UpdateParameters>
            <asp:ControlParameter ControlID="txtCode" PropertyName="text" Name="code" Type="string" Size="10" />
            <asp:Parameter Name="name" Type="string" size="50" />
            <asp:Parameter Name="sex" Type="string" size="50" />
            <asp:Parameter Name="age" Type="int32" />
            <asp:Parameter Name="pSex" Type="string" Size="50" />
            <asp:Parameter Name="pAge" Type="int32" />
            <asp:Parameter Name="lengthYear" Type="int16" />
            <asp:Parameter Name="lengthMonth" Type="int16" />
            <asp:Parameter Name="children" Type="int16" />
            <asp:Parameter Name="married" Type="boolean" />
            <asp:Parameter Name="cohabiting" Type="boolean" />
            <asp:Parameter Name="divorced" Type="boolean" />
            <asp:Parameter Name="widowed" Type="boolean" />
            <asp:Parameter Name="seperated" Type="boolean" />
            <asp:Parameter Name="single" Type="boolean" />
            <asp:Parameter Name="other" Type="string" Size="2000" />
            <asp:Parameter Name="ethnic" Type="string" Size="50" />
            <asp:Parameter Name="pEthnic" Type="string" Size="50" />
            <asp:Parameter Name="disability" Type="string" Size="4000" />
            <asp:Parameter Name="pDisability" Type="string" Size="4000" />
            <asp:Parameter Name="mental" Type="string" Size="4000" />
            <asp:Parameter Name="pMental" Type="string" Size="4000" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlHistory" runat="server" SelectCommand="procConsultancyMessagesByCodeSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="txtCode" PropertyName="text" Name="code" Type="string" Size="10" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <script language="Javascript" type="text/javascript">
        function focusElement(e)
        {
            document.getElementById(e).focus();
        }
    </script>
</asp:Content>

