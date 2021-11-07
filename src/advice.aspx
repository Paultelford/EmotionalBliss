<%@ Page Title="" Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" CodeFile="advice.aspx.vb" Inherits="advice" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="contentTop" runat="Server">
</asp:Content>
<asp:Content id="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="experts" master="m_site"></menu:EBMenu>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    
<td valign="top" align="left">

                            <h3>
                                Advice</h3>
                            <br>
                            <div id="DashedLineHorizontal">
                            </div>

<table width="500" border="0" cellspacing="20" cellpadding="10">
      
      <tr>
          <td height="38"><a class="bbcaudio" onclick='return clicker2();' href="#" style='font-size: 16px;'>Listen to BBC Audio Clip</a></td>
      </tr>
    </table><br />

                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td>
                                        <br>
                                        <ul id="nav">
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage08.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/haveyou.aspx?m=Experts">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/haveyou.aspx?m=Experts';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            Have you had<br>
                                                                            an Orgasm?
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; cursor: pointer;" onclick='return playaudio("107");'>
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25"
                                                        border="0"></a>
                                                <div id="DashedLineHorizontal">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage03.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=myths&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=myths&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            Myth Busting<br>
                                                                            <br>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; cursor: pointer;" onclick='return playaudio("101all");'>
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25"
                                                        border="0"></a>
                                                <div id="DashedLineHorizontal">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage01.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=sensations&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=sensations&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            Variety of Sensations<br>
                                                                            <br>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; cursor: pointer;" onclick='return playaudio("105all");'>
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25"
                                                        border="0"></a>
                                                <div id="DashedLineHorizontal">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage02.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=Erogenous_Zones&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=Erogenous_Zones&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            Erogenous Zones<br>
                                                                            <br>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; cursor: pointer;" onclick='return playaudio("102all");'>
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25"
                                                        border="0"></a>
                                                <div id="DashedLineHorizontal">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage11.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=masterbation&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=masterbation&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            Why Masturbation<br>
                                                                            is Good
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; "cursor: pointer;" onclick='return playaudio("106");'>
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25"
                                                        border="0"></a>
                                                <div id="Div3">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage04.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=Choosing_your_massager&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=Choosing_your_massager&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            Choosing Your<br>
                                                                            Massager
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; width: 114px; height: 25px;">
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25" style="display: none;"
                                                        border="0"></a>
                                                <div id="DashedLineHorizontal">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage09.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=10&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=10&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            10 Ways...<br>
                                                                            <br>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; cursor: pointer;" onclick='return playaudio("108");'>
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25"
                                                        border="0"></a>
                                                <div id="DashedLineHorizontal">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage06.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=orgasm&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=orgasm&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            How to Achieve<br>
                                                                            an Orgasm
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; width: 114px; height: 25px;" >
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25" style="display: none;"
                                                        border="0"></a>
                                                <div id="DashedLineHorizontal">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage10.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=touching&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=touching&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            Intimate Touching<br>
                                                                            for a Woman
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; "cursor: pointer;" onclick='return playaudio("103");'>
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25"
                                                        border="0"></a>
                                                <div id="DashedLineHorizontal">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage07.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=phases&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=phases&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            4 Phases<br>
                                                                            <br>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; width: 114px; height: 25px;" >
                                                    <img src="images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25" style="display: none;"
                                                        border="0"></a>
                                                <div id="Div2">
                                                </div>
                                            </li>
                                            <li>
                                                <table width="100%" height="150" border="0" cellspacing="0" cellpadding="0">
                                                    <tr>
                                                        <td style="background-image: url(images/advice_cellimage05.jpg);">
                                                            <a style="height: 150px; padding-top: 15px;" href="/static.aspx?p=Sexual_Wellbeing_with_Diabetes&m=Advice">
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0" onclick="document.location='/static.aspx?p=Sexual_Wellbeing_with_Diabetes&m=Advice';">
                                                                    <tr>
                                                                        <td style="padding-left: 10px;">
                                                                            Sexual Wellbeing<br>
                                                                            with Diabetes<br>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="120" valign="bottom">
                                                                            <img src="/images/btn_Read.png" width="99" height="22" border="0">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </a>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <a style="margin-top: 5px; padding-top: 5px; width: 114px; height: 25px;" onclick='return false;'>
                                                    <img src="/images/btn_audio.png" alt="Listen to Audio Clip" width="114" height="25" style="display: none;"
                                                        border="0"></a>
                                                <div id="Div1">
                                                </div>
                                            </li>
                                        </ul>
                                    </td>
                                </tr>
                            </table>
                        
</asp:Content>
