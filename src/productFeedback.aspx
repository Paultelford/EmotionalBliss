<%@ Page Language="VB" MasterPageFile="~/MasterPageSite.master" AutoEventWireup="false" CodeFile="productFeedback.aspx.vb" Inherits="productFeedback" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:MultiView ID="mvFeedback" runat="server" ActiveViewIndex="0">
        <asp:View ID="vPage1" runat="server">
            Firstly I would like to personally thank you for your valued feedback regarding the Emotional Bliss products. You feedback will be remain confidential and will be instrumental in the future development of Emotional Bliss. We have over the past 5 years worked closely with the UK’s leading Psychosexual Therapists to perfect a range of products that were developed to stimulate orgasm by focusing on the shapes, texture and vibrations in conjunction with reducing the noise and introducing heat as an additional sensation in Womolia, Jasmine and Femblossom. 
            <BR />On completing the following questionnaire if you feel there is any additional feedback you would like to provide, you are more than welcome to contact me personally on <a href="mailto:pt@emotionalbliss.co.uk">pt@emotionalbliss.co.uk</a>, the questionnaire will take approximately 10 minutes to complete.
            <br /><br /><br />
            <table border="0">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                               <td>
                                    <b>Please enter your Christain name</b>
                                </td>
                                <td width="40">&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtName" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxtName" runat="server" ControlToValidate="txtName" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3"><br /><br /></td>
                            </tr>
                            <tr>
                                <td>
                                    <b>Please enter your postcode</b>
                                </td>
                                <td width="40">&nbsp;</td>
                                <td>
                                    <asp:TextBox ID="txtPostcode" runat="server" MaxLength="10"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxtPostcode" runat="server" ControlToValidate="txtPostcode" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>                    
                    </td>
                </tr>
                <tr>
                    <td>
                        <br /><br />
                        <b>How would you rate the overall packaging?</b>
                    </td>
                    <td width="60">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rad1_1" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                        </asp:RadioButtonList><asp:RequiredFieldValidator ID="reqRad1_1" runat="server" ControlToValidate="rad1_1" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr><td colspan="2"><br /><br /></td></tr>
                <tr>
                    <td>
                        <b>How do you rate <a href='http://www.emotionalbliss.co.uk' target="_blank">website No1</a>?
                        <br />Please take a few minutes to browse around the site</b>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rad1_2" runat="server" RepeatDirection="horizontal">
                            <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad1_2" runat="server" ControlToValidate="rad1_2" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr><td colspan="2"><br /><br /></td></tr>
                <tr>
                    <td>
                        <b>How do you rate <a href='http://www.emotionalbliss.com/default.asp' target="_blank">website No2</a>?
                        <br />Please take a few minutes to browse around the site</b>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>                
                    <td colspan="2">
                        <asp:RadioButtonList ID="rad1_3" runat="server" RepeatDirection="horizontal">
                            <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad1_3" runat="server" ControlToValidate="rad1_3" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr><td colspan="2"><br /><br /></td></tr>
                <tr>
                    <td>
                        <b>Which website would you feel more confident to purchase from No1 or No2?</b>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rad1_4" runat="server" RepeatDirection="vertical">
                            <asp:ListItem Text="Site 1" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Site 2" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad1_4" runat="server" ControlToValidate="rad1_4" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr><td colspan="2"><br /><br /></td></tr>
                <tr>
                    <td>
                        <b>The following catalogues are the same in content, the difference is the visual images, please rate both catalogues.</b>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table border="0">
                            <tr>
                                <td align="center">
                                    <b><a href='eb1.pdf' target="_blank">Catalogue 1</a></b><br /><br />
                                    <asp:RadioButtonList ID="rad1_5a" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="reqRad1_5a" runat="server" ControlToValidate="rad1_5a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                                <td width="80"></td>
                                <td align="center">
                                    <b><a href='eb2.pdf' target="_blank">Catalogue 2</a></b><br /><br />
                                    <asp:RadioButtonList ID="rad1_5b" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:RequiredFieldValidator ID="reqRad1_5b" runat="server" ControlToValidate="rad1_5b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr><td colspan="2"><br /><br /></td></tr>
                <tr>
                    <td>
                        <b>How do you rate the 30ml Silicon Lubricant?</b>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rad1_6" runat="server" RepeatDirection="horizontal">
                            <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad1_6" runat="server" ControlToValidate="rad1_6" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr><td colspan="2"><br /><br /></td></tr>
                <tr>
                    <td>
                        <b>How do you rate the 30ml Water Based Lubricant?</b>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rad1_7" runat="server" RepeatDirection="horizontal">
                            <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad1_7" runat="server" ControlToValidate="rad1_7" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr><td colspan="2"><br /><br /></td></tr>
                <tr>
                    <td valign="top">
                        <b>Please select the intimate massager you received?</b>
                    </td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:RadioButtonList ID="rad1_8" runat="server" RepeatDirection="vertical">
                            <asp:ListItem Text="Womolia" Value="Womolia"></asp:ListItem>
                            <asp:ListItem Text="Jasmine" Value="Jasmine"></asp:ListItem>
                            <asp:ListItem Text="Femblossom" Value="Femblossom"></asp:ListItem>
                            <asp:ListItem Text="Isis & Chandra" Value="Isis & Chandra"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad1_8" runat="server" ControlToValidate="rad1_8" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
            </table>
            <asp:button ID="btnNext1" runat="server" Text="Continue" OnClick="btnNext1_click" />
        </asp:View>
        <asp:View ID="vPage2" runat="server">
            <table border="0">
                <tr>
                    <td>
                        <b>How would you rate the product?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rad2_1" runat="server" RepeatDirection="horizontal">
                            <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad2_1" runat="server" ControlToValidate="rad2_1" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the vibrations?</b><br />
                        <table border="0">
                            <tr>
                                <td>
                                    Program 1: 80hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2a" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2a" runat="server" ControlToValidate="rad2_2a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 2: 120hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2b" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2b" runat="server" ControlToValidate="rad2_2b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 3: 150hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2c" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2c" runat="server" ControlToValidate="rad2_2c" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 4: 80hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2d" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2d" runat="server" ControlToValidate="rad2_2d" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 5: 120hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2e" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2e" runat="server" ControlToValidate="rad2_2e" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 6: 150hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2f" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2f" runat="server" ControlToValidate="rad2_2f" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 7: The Saw Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2g" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2g" runat="server" ControlToValidate="rad2_2g" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>

                            </tr>
                            <tr>
                                <td>
                                    Program 8: The Triangle Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2h" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2h" runat="server" ControlToValidate="rad2_2h" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 9: The Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_2i" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_2i" runat="server" ControlToValidate="rad2_2i" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br /><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the Heat?</b><br />
                        <table border="0">
                            <tr>
                                <td>
                                    Program 1: 80hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3a" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3a" runat="server" ControlToValidate="rad2_3a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 2: 120hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3b" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3b" runat="server" ControlToValidate="rad2_3b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 3: 150hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3c" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3c" runat="server" ControlToValidate="rad2_3c" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 4: 80hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3d" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3d" runat="server" ControlToValidate="rad2_3d" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 5: 120hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3e" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3e" runat="server" ControlToValidate="rad2_3e" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 6: 150hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3f" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3f" runat="server" ControlToValidate="rad2_3f" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 7: The Saw Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3g" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3g" runat="server" ControlToValidate="rad2_3g" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>

                            </tr>
                            <tr>
                                <td>
                                    Program 8: The Triangle Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3h" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3h" runat="server" ControlToValidate="rad2_3h" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 9: The Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_3i" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_3i" runat="server" ControlToValidate="rad2_3i" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br /><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the Noise for the following functions?</b><br />
                        <table border="0">
                            <tr>
                                <td>
                                    Program 1: 80hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4a" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4a" runat="server" ControlToValidate="rad2_4a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 2: 120hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4b" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4b" runat="server" ControlToValidate="rad2_4b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 3: 150hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4c" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4c" runat="server" ControlToValidate="rad2_4c" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 4: 80hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4d" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4d" runat="server" ControlToValidate="rad2_4d" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 5: 120hz Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4e" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4e" runat="server" ControlToValidate="rad2_4e" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 6: 150hz Constant Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4f" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4f" runat="server" ControlToValidate="rad2_4f" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 7: The Saw Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4g" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4g" runat="server" ControlToValidate="rad2_4g" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>

                            </tr>
                            <tr>
                                <td>
                                    Program 8: The Triangle Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4h" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4h" runat="server" ControlToValidate="rad2_4h" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Program 9: The Pulse Mode
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad2_4i" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad2_4i" runat="server" ControlToValidate="rad2_4i" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>Would you change the colour?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rad2_5" runat="server" RepeatDirection="vertical">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad2_5" runat="server" ControlToValidate="rad2_5" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Recommend a colour</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt2_6" runat="server" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>In your opinion, what would you be prepared to pay for the products?</b><br />
                        <table border="0">
                            <tr>
                                <td>
                                    Isis
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt2_7a" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt2_7a" runat="server" ControlToValidate="txt2_7a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Chandra
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt2_7b" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt2_7b" runat="server" ControlToValidate="txt2_7b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Womolia
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt2_7c" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt2_7c" runat="server" ControlToValidate="txt2_7c" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Jasmine
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt2_7d" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt2_7d" runat="server" ControlToValidate="txt2_7d" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Femblossom
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt2_7e" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt2_7e" runat="server" ControlToValidate="txt2_7e" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>Is free delivery important to your purchase decision?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rad2_8" runat="server" RepeatDirection="vertical">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad2_8" runat="server" ControlToValidate="rad2_8" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>What would be the maximum delivery charge you would be prepared to pay?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        £<asp:TextBox ID="txt2_9" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTxt2_9" runat="server" ControlToValidate="txt2_9" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>If we offered Money Back guarantee on the purchase of any of the Emotional Bliss Massagers for example:  “If you were not happy with your massager you could use the massager and then return it for a credit or exchange it for another within 14 day”. Would this influence your decision to buy?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rad2_10" runat="server" RepeatDirection="horizontal">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad2_10" runat="server" ControlToValidate="rad2_10" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>Please provide any further information you feel will help us to improve our products or service</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt2_11" runat="server" TextMode="multiLine" Rows="10" Columns="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTxt2_11" runat="server" ControlToValidate="txt2_11" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="lblInfo2" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>Please write a review on the Emotional Bliss massager that will be uploaded onto the main website</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt2_12" runat="server" TextMode="multiLine" Rows="10" Columns="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTxt2_12" runat="server" ControlToValidate="txt2_12" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="lblReview2" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnComplete2" runat="server" Text="Finish" OnClick="btnComplete2_click" />
        </asp:View>
        <asp:View ID="vPage3" runat="server">
            <table border="0">
                <tr>
                    <td>
                        <b>How would you rate the product?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    Isis (Small)
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad3_1a" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad3_1a" runat="server" ControlToValidate="rad3_1a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Chandra        
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad3_1b" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad3_1b" runat="server" ControlToValidate="rad3_1b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br /><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the vibrations?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    Isis (Small)
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad3_2a" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad3_2a" runat="server" ControlToValidate="rad3_2a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Chandra
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                     <asp:RadioButtonList ID="rad3_2b" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad3_2b" runat="server" ControlToValidate="rad3_2b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br /><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>How would you rate the Noise for the following functions?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    Isis (Small)
                                </td>
                                <td>&nbsp;</td>
                                <td>
                                    <asp:RadioButtonList ID="rad3_3a" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad3_3a" runat="server" ControlToValidate="rad3_3a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Chandra
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                     <asp:RadioButtonList ID="rad3_3b" runat="server" RepeatDirection="horizontal">
                                        <asp:ListItem Text="Poor" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Average" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Good" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="Very Good" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="Excellent" Value="5"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="reqRad3_3b" runat="server" ControlToValidate="rad3_3b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br /><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>Would you change the colour?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rad3_5" runat="server" RepeatDirection="vertical">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad3_5" runat="server" ControlToValidate="rad3_5" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>Recommend a colour</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt3_6" runat="server" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>In your opinion, what would you be prepared to pay for the products?</b><br />
                        <table border="0">
                            <tr>
                                <td>
                                    Isis
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt3_7a" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt3_7a" runat="server" ControlToValidate="txt3_7a" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Chandra
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt3_7b" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt3_7b" runat="server" ControlToValidate="txt3_7b" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Womolia
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt3_7c" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt3_7c" runat="server" ControlToValidate="txt3_7c" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Jasmine
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt3_7d" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt3_7d" runat="server" ControlToValidate="txt3_7d" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Femblossom
                                </td>
                                <td>
                                    £<asp:TextBox ID="txt3_7e" runat="server" MaxLength="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="reqTxt3_7e" runat="server" ControlToValidate="txt3_7e" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td><br /><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>Is free delivery important to your purchase decision?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rad3_8" runat="server" RepeatDirection="vertical">
                            <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad3_8" runat="server" ControlToValidate="rad3_8" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>What would be the maximum delivery charge you would be prepared to pay</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        £<asp:TextBox ID="txt3_9" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTxt3_9" runat="server" ControlToValidate="txt3_9" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>If we offered Money Back guarantee on the purchase of any of the Emotional Bliss Massagers for example:  “If you were not happy with your massager you could use the massager and then return it for a credit or exchange it for another within 14 day”. Would this influence your decision to buy?</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:RadioButtonList ID="rad3_10" runat="server" RepeatDirection="horizontal">
                           <asp:ListItem Text="Yes" Value="true"></asp:ListItem>
                            <asp:ListItem Text="No" Value="false"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="reqRad3_10" runat="server" ControlToValidate="rad3_10" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>Please provide any further information you feel will help us to improve our products or service</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt3_11" runat="server" TextMode="multiLine" Rows="10" Columns="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTxt3_11" runat="server" ControlToValidate="txt3_11" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="lblInfo3" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td><br /></td>
                </tr>
                <tr>
                    <td>
                        <b>Please write a review on the Emotional Bliss massager that will be uploaded onto the main website</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txt3_12" runat="server" TextMode="multiLine" Rows="10" Columns="100"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqTxt3_12" runat="server" ControlToValidate="txt3_12" ErrorMessage="* Required" display="static"></asp:RequiredFieldValidator>
                        <br />
                        <asp:Label ID="lblReview3" runat="server"></asp:Label>
                    </td>
                </tr>
            </table>    
            <asp:Button ID="btnComplete3" runat="server" Text="Finish" OnClick="btnComplete3_click" />
        </asp:View>
    </asp:MultiView>
    <asp:Label ID="lblError" runat="server"></asp:Label>
</asp:Content>

