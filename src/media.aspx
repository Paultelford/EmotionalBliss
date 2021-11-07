<%@ Page Title="" Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false" Theme="Media" CodeFile="media.aspx.vb" Inherits="media" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="press" master="m_site"></menu:EBMenu>
    <script type="text/JavaScript">
    <!--
            function MM_findObj(n, d) { //v4.01
                var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
                    d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
                }
                if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
                for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
                if (!x && d.getElementById) x = d.getElementById(n); return x;
            }

            function MM_preloadImages() { //v3.0
                var d = document; if (d.images) {
                    if (!d.MM_p) d.MM_p = new Array();
                    var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
                        if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; } 
                }
            }

            function MM_swapImgRestore() { //v3.0
                var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
            }
            
            function MM_swapImage() { //v3.0
                var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
                    if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
        }
    //-->
    </script>
    <script type="text/javascript" src="highslide/highslide.js"></script>
    <script type="text/javascript" src="highslide/highslide-with-html.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="617" border="0" cellspacing="0" cellpadding="0">
        <tr>
            <td align="right">
                <img src="/images/title.gif" width="278" height="81" alt="Media Coverage" />
            </td>
        </tr>
    </table>
    <asp:Table width="617" border="0" cellspacing="0" cellpadding="0" id="tblMedia" runat="server">
    </asp:Table>
    
    <asp:HiddenField ID="hidCell" runat="server" Value="<table width='200' border='0' cellspacing='0' cellpadding='0'>
                    <tr>
                        <td>
                            <img src='/images/media/topNew.gif' width='200' height='8' /></td>
                    </tr>
                    <tr>
                        <td>
                            <table width='100%' cellpadding='0' cellspacing='0'>
                                <tr>
                                    <td width='3' background='/images/media/leftBorder.gif'></td>
                                    <td width='194' align='center'><span class='ArticleTitle'>@title</span></td>
                                    <td width='3' background='/images/media/rightBorder.gif'></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td background='/images/media/topImg.gif' width='200' height='10'>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width='200' border='0' cellspacing='0' cellpadding='0'>
                                <tr>
                                    <td>
                                        <table width='100%' border='0' cellspacing='0' cellpadding='0'>
                                            <tr>
                                                <td width='15'>
                                                    <img src='/images/mid1.gif' width='15' height='122' />
                                                </td>
                                                <td align='center' bgcolor='#5B8FC4'>
                                                    <img src='/images/media/@thumb' width='88' height='122' />
                                                </td>
                                                <td width='12'>
                                                    <img src='/images/mid2.gif' width='12' height='122' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td rowspan='2' valign='top' style='background: #FFFFFF url(/images/tileRight.gif); color: #588EC4;' align='left'>
                                        <!--<span class='ArticleDate'>@date</span>-->
                                        <br />
                                        <br />
                                        <span class='Headline'>@headline<br />
                                        </span>
                                    </td>
                                </tr>
                                <tr>
                                    <td width='115' valign='top' style='background: #FFFFFF url(/images/tileLeft.gif)'>
                                        <img src='/images/mid3.gif' alt='' width='115' height='16' />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height='27'>
                            @button<img src='/images/read.gif' alt='Read Full Article' width='200' height='27' border='0' /></a>
                        </td>
                    </tr>
                </table>" />    
</asp:Content>
