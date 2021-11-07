<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="feedback.aspx.vb" Inherits="affiliates_feedback" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DisplayAfter="500" DynamicLayout="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            Current Feedback Reports: <asp:Label ID="lblReports" runat="Server" Font-Bold="true"></asp:Label><br /><br />
            <table border="0">
                <tr>
                    <td valign="top">
                        View <asp:DropDownList ID="drpType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpType_selectedIndexChanged">
                            <asp:ListItem Text="All Feedback" Value="all"></asp:ListItem>
                            <asp:ListItem Text="Individual Reports" Value="individual"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                    <td>&nbsp;</td>
                    <td valign="top">
                        <asp:DropDownList Visible="false" id="drpUser" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpUser_selectedIndexChanged" AppendDataBoundItems="true" DataSourceID="sqlUsers" DataTextField="user" DataValueField="id">
                            <asp:ListItem Text="Select user..." Value="0"></asp:ListItem>
                        </asp:DropDownList>                       
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gvLog" runat="server" DataSourceID="sqlLog" Visible="false" AutoGenerateColumns="false" DataKeyNames="id" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvLog_selectedIndexChanged">
                <Columns>
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnName" runat="server" Text='<%# Eval("name") %>' CommandName="select" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Postcode" DataField="postcode" />
                    <asp:BoundField ItemStyle-Width="40" />
                    <asp:BoundField HeaderText="Date" DataField="date" />
                </Columns>
            </asp:GridView>
            
            <table border="0" id="tblFeedback" runat="server">
                <tr>
                    <td>
                        <asp:Label ID="lblName" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;<br /><b><font color='blue'>Page 1 - General Info</font></b></td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the overall packaging?</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_1_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_1_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_1_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_1_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_1_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>How do you rate <a href="http://www.emotionalbliss.co.uk" target='_blank'>website No1?</a></b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_2_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_2_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_2_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_2_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_2_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>How do you rate <a href="http://www.emotionalbliss.com/default.asp" target='_blank'>website No2?</a></b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_3_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_3_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_3_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_3_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_3_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Which website would you feel more confident to purchase from No1 or No2?</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Site1
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_4_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Site2
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_4_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>                    
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>
                        The following catalogues are the same in content, the difference is the visual images, please rate both catalogues.<br />
                        Catalogue 1                
                        </b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5a_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5a_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5a_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5a_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5a_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>
                        Catalogue 2
                        </b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5b_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5b_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5b_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5b_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_5b_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>
                        How do you rate the 30ml Silicon Lubricant?
                        </b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_6_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_6_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_6_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_6_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_6_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>
                        How do you rate the 30ml Water Based Lubricant?
                        </b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_7_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_7_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_7_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_7_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_7_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>
                        Please select the intimate massager you received?
                        </b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Womolia
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_8_W" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Jasmine
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_8_J" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Femblossom
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_8_F" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Isis & Chandra
                                </td>
                                <td>
                                    <asp:Label ID="lbl1_8_I" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the product? </b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_1_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_1_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_1_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_1_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_1_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table id="tbl2" runat="server">
                <tr>
                    <td>&nbsp;<br /><b><font color='blue'>Page 2 - Heat Products</font></b></td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the vibrations?</b><br />
                        <b>Program 1: 80hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2a_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2a_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2a_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2a_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2a_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 2: 120hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2b_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2b_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2b_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2b_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2b_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 3: 150hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2c_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2c_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2c_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2c_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2c_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 4: 80hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2d_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2d_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2d_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2d_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2d_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 5: 120hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2e_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2e_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2e_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2e_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2e_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 6: 150hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2f_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2f_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2f_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2f_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2f_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 7: The Saw Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2g_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2g_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2g_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2g_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2g_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 8: The Triangle Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2h_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2h_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2h_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2h_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2h_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 9: The Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2i_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2i_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2i_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2i_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_2i_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the Heat?</b><br />
                        <b>Program 1: 80hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3a_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3a_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3a_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3a_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3a_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 2: 120hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3b_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3b_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3b_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3b_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3b_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 3: 150hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3c_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3c_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3c_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3c_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3c_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 4: 80hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3d_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3d_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3d_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3d_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3d_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 5: 120hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3e_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3e_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3e_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3e_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3e_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 6: 150hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3f_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3f_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3f_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3f_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3f_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 7: The Saw Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3g_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3g_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3g_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3g_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3g_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 8: The Triangle Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3h_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3h_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3h_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3h_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3h_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 9: The Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3i_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3i_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3i_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3i_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_3i_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the Noise for the following functions?</b><br />
                        <b>Program 1: 80hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4a_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4a_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4a_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4a_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4a_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 2: 120hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4b_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4b_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4b_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4b_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4b_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 3: 150hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4c_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4c_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4c_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4c_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4c_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 4: 80hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4d_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4d_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4d_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4d_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4d_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 5: 120hz Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4e_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4e_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4e_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4e_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4e_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 6: 150hz Constant Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4f_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4f_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4f_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4f_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4f_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 7: The Saw Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4g_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4g_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4g_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4g_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4g_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 8: The Triangle Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4h_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4h_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4h_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4h_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4h_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Program 9: The Pulse Mode</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4i_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4i_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4i_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4i_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_4i_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Would you change the colour?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Yes
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_5_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    No
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_5_0" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Recommend a colour</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl2_6" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>In your opinion, what would you be prepared to pay for the products?</b> <asp:Label ID="lbl2_7Pre" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Isis
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_7a" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Chandra
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_7b" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="140">
                                    Womolia
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_7c" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Jasmine
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_7d" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Femblossom
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_7e" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Is free delivery important to your purchase decision?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Yes
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_8_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    No
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_8_0" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>What would be the maximum delivery charge you would be prepared to pay?</b> <asp:Label ID="lbl2_9Pre" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_9" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>If we offered Money Back guarantee on the purchase of any of the Emotional Bliss Massagers for example:  “If you were not happy with your massager you could use the massager and then return it for a credit or exchange it for another within 14 day”. Would this influence your decision to buy?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Yes
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_10_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="140">
                                    No
                                </td>
                                <td>
                                    <asp:Label ID="lbl2_10_0" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td id="td2TextA" runat="server" visible="false">
                        <b>Please provide any further information you feel will help us to improve our products or service</b><br />
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="140">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="lbl2_11" runat="server" Rows="8" Columns="80" TextMode="MultiLine"></asp:TextBox><br />
                                    <asp:Label ID="lblError2A" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td id="td2TextB" runat="server" visible="false">
                        <b>Please write a review on the Emotional Bliss massager that will be uploaded onto the main website</b><br />
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td width="140">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="lbl2_12" runat="server" Rows="8" Columns="80" TextMode="MultiLine"></asp:TextBox><br />
                                    <asp:Label ID="lblError2B" runat="server"></asp:Label><br />
                                    <asp:Button ID="btnSubmit2" runat="server" Text="Save Changes" OnClick="btnSubmit2_click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
            <table id="tbl3" runat="server">
                <tr>
                    <td>&nbsp;<br /><b><font color='blue'>Page 3 - Finger Products</font></b></td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the product?<br />
                        Isis (Small)</b>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1a_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1a_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1a_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1a_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1a_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Chandra</b>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1b_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1b_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1b_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1b_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_1b_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the vibrations?<br />
                        Isis (Small)</b>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2a_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2a_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2a_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2a_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2a_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Chandra</b>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2b_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2b_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2b_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2b_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_2b_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the Noise for the following products?<br />
                        Isis (Small)</b>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3a_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3a_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3a_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3a_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3a_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Chandra</b>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Poor
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3b_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Average
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3b_2" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3b_3" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Very Good
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3b_4" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Excellent
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_3b_5" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Would you change the colour?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Yes
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_5_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    No
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_5_0" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Recommend a colour</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl3_6" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>In your opinion, what would you be prepared to pay for the products?</b> <asp:Label ID="lbl3_7Pre" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Isis
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_7a" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Chandra
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_7b" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="140">
                                    Womolia
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_7c" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Jasmine
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_7d" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Femblossom
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_7e" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>Is free delivery important to your purchase decision?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Yes
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_8_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    No
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_8_0" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>What would be the maximum delivery charge you would be prepared to pay?</b> <asp:Label ID="lbl3_9Pre" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_9" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>
                        <b>If we offered Money Back guarantee on the purchase of any of the Emotional Bliss Massagers for example:  “If you were not happy with your massager you could use the massager and then return it for a credit or exchange it for another within 14 day”. Would this influence your decision to buy?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0">
                            <tr>
                                <td width="140">
                                    Yes
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_10_1" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="140">
                                    No
                                </td>
                                <td>
                                    <asp:Label ID="lbl3_10_0" runat="server" Text="0"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td id="td3TextA" runat="server" visible="false">
                        <b>Please provide any further information you feel will help us to improve our products or service</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="lbl3_11" runat="server" TextMode="MultiLine" Rows="8" Columns="80"></asp:TextBox><br />
                                    <asp:Label ID="lblError3A" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td id="td3TextB" runat="server" visible="false">
                        <b>Please write a review on the Emotional Bliss massager that will be uploaded onto the main website</b><br />
                        <table border="0">
                            <tr>
                                <td width="140">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="lbl3_12" runat="server" TextMode="MultiLine" Rows="8" Columns="80"></asp:TextBox><br />
                                    <asp:Label ID="lblError3B" runat="server"></asp:Label><br />
                                    <asp:Button ID="btnSubmit3" runat="server" Text="Save Changes" OnClick="btnSubmit3_click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </ContentTemplate>
    </atlas:UpdatePanel>
    <atlas:UpdateProgress ID="up2" runat="server" DisplayAfter="500" DynamicLayout="false" AssociatedUpdatePanelID="update1" Visible="false">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>
    
    
    <asp:SqlDataSource ID="sqlLog" runat="server" SelectCommand="procProductFeedbackSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource id="sqlUsers" runat="server" SelectCommand="procProductFeedbackUsersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
</asp:Content>

