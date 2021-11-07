<%@ Page Language="VB" MasterPageFile="~/m_site.master" AutoEventWireup="false" CodeFile="mediaCoverage.aspx.vb" Inherits="mediaCoverage" title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBMenu.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentLeftMenu" Runat="Server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="press" master="m_site"></menu:EBMenu>
    <script type="text/JavaScript">
<!--
function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}
2
function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}
//-->
</script>
    <style type="text/css">
* {
    font-family: 'Lucida Grande',Helvetica, Arial;
}
.highslide-html {
    background-color: white;
}
.highslide-html-blur {
}
.highslide-html-content {
	position: absolute;
    display: none;
}
.highslide {
	cursor: url(highslide/graphics/zoomin.cur), pointer;
    outline: none;
}

.highslide:link {color: #0099ff; text-decoration:underline;}
.highslide:hover {color: #aaaaaa; text-decoration:none;}
 
.highslide-active-anchor img {
	visibility: hidden;
}

.highslide-wrapper {
	background: white;
}
.highslide-image {
	border: 10px solid white;
}
.highslide-image-blur {
}
.highslide-caption {
    display: none;
    border: 5px solid #ffffff;
    border-top: none;
    padding: 5px;
    background-color: white;
}
.highslide-loading {
    display: block;
	color: black;
	font-size: 8pt;
	font-family: sans-serif;
	font-weight: bold;
    text-decoration: none;
	padding: 2px;
	border: 1px solid black;
    background-color: white;
    padding-left: 22px;
    background-image: url(highslide/graphics/loader.white.gif);
    background-repeat: no-repeat;
    background-position: 3px 1px;
}
a.highslide-credits,
a.highslide-credits i {
    padding: 2px;
    color: silver;
    text-decoration: none;
	font-size: 10px;
}
a.highslide-credits:hover,
a.highslide-credits:hover i {
    color: white;
    background-color: gray;
}

a.highslide-full-expand {
	background: url(highslide/graphics/fullexpand.gif) no-repeat;
	display: block;
	margin: 0 10px 10px 0;
	width: 34px;
	height: 34px;
}
/* Styles for the popup */
.highslide-wrapper {
	background-color: white;
}
.highslide-wrapper .highslide-html-content {
    width: 600px;
    padding: 5px;
}
.highslide-wrapper .highslide-header div {
}
.highslide-wrapper .highslide-header ul {
	margin: 0;
	padding: 0;
	text-align: right;
}
.highslide-wrapper .highslide-header ul li {
	display: inline;
	padding-left: 1em;
}
.highslide-wrapper .highslide-header ul li.highslide-previous, .highslide-wrapper .highslide-header ul li.highslide-next {
	display: none;
}
.highslide-wrapper .highslide-header a {
	font-weight: bold;
	font-size: 18px;
	color: #ffffff;
	text-transform: uppercase;
	text-decoration: none;
	line-height: 30px;
	background: #CC0066;
	margin-right: 30px;
}
.highslide-wrapper .highslide-header a:hover {
	color: #330033;
}
.highslide-wrapper .highslide-header .highslide-move a {
	cursor: move;
}
.highslide-wrapper .highslide-footer {
	height: 11px;
}
.highslide-wrapper .highslide-footer .highslide-resize {
	float: right;
	height: 11px;
	width: 11px;
}
.highslide-wrapper .highslide-body {
}
.highslide-move {
    cursor: move;
}
.highslide-resize {
    cursor: nw-resize;
}

/* These must always be last */
.highslide-display-block {
    display: block;
}
.highslide-display-none {
    display: none;
}

</style>
<script type="text/javascript" src="highslide/highslide.js"></script>

<script type="text/javascript" src="highslide/highslide-with-html.js"></script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div id="welcometext">
        <h2>
            Media Coverage</h2>
        <div id="pressbox">
            <div id="press1">
                <a href="press/daily-mail-press.html" onclick="return hs.htmlExpand(this, { objectType: 'iframe' } )"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image1','','/design/images/press-dailymail-btn-over.jpg',1)">
                    <img src="/design/images/press-dailymail-btn.jpg" alt="Daily Mail" title="Click to enlarge"
                        name="Image1" width="289" height="140" border="0" id="Image1" /></a></div>
            <div id="press2">
                <a href="/design/images/times-oct-08.jpg" onclick="return hs.expand(this)" onmouseout="MM_swapImgRestore()"
                    onmouseover="MM_swapImage('Image2','','/design/images/press-times-btn-over.jpg',1)">
                    <img src="/design/images/press-times-btn.jpg" alt="The Times" title="Click to enlarge"
                        name="Image2" width="289" height="140" border="0" id="Image2" /></a></div>
        </div>
        <div id="pressbox2">
            <div id="press1">
                <a href="press/mother-work-press.html" onclick="return hs.htmlExpand(this, { objectType: 'iframe' } )"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','/design/images/press-mother-btn-over.jpg',1)">
                    <img src="/design/images/press-mother-btn.jpg" alt="Mother at Work" title="Click to enlarge"
                        name="Image9" width="289" height="140" border="0" id="Image9" /></a></div>
            <div id="press2">
                <a href="http://uk.youtube.com/watch?v=lXlx4SLVPIQ" class="highslide" target="new"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image4','','/design/images/press-titchmarsh-btn-over.jpg',1)">
                    <img src="/design/images/press-titchmarsh-btn.jpg" alt="Alan Titchmarsh" title="Click to enlarge"
                        name="Image4" width="289" height="140" border="0" id="Image4" /></a></div>
        </div>
        <div id="pressbox2">
            <div id="press1">
                <a href="press/mens-health-press.html" onclick="return hs.htmlExpand(this, { objectType: 'iframe' } )"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image5','','/design/images/press-menshealth-btn-over.jpg',1)">
                    <img src="/design/images/press-menshealth-btn.jpg" alt="Men's Health" title="Click to enlarge"
                        name="Image5" width="289" height="140" border="0" id="Image5" /></a></div>
            <div id="press2">
                <a href="press/ivillage-press.html" onclick="return hs.htmlExpand(this, { objectType: 'iframe' } )"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image6','','/design/images/press-ivillage-btn-over.jpg',1)">
                    <img src="/design/images/press-ivillage-btn.jpg" alt="iVillage.co.uk" title="Click to enlarge"
                        name="Image6" width="289" height="140" border="0" id="Image6" /></a></div>
        </div>
        <div id="pressbox2">
            <div id="press1">
                <a href="press/discovery-health-press.html" onclick="return hs.htmlExpand(this, { objectType: 'iframe' } )"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image7','','/design/images/press-discovery-btn-over.jpg',1)">
                    <img src="/design/images/press-discovery-btn.jpg" alt="Discovery Channel - Home and Health"
                        title="Click to enlarge" name="Image7" width="289" height="140" border="0" id="Image7" /></a></div>
            <div id="press2">
                <a href="/design/images/whispermag-july08.jpg" class="highslide" onclick="return hs.expand(this)"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image8','','/design/images/press-whispermag-btn-over.jpg',1)">
                    <img src="/design/images/press-whispermag-btn.jpg" alt="WhisperMag" title="Click to enlarge"
                        name="Image8" width="289" height="140" border="0" id="Image8" /></a></div>
        </div>
        <div id="pressbox2">
            <div id="press1">
                <a href="press/victoria-health-press.html" onclick="return hs.htmlExpand(this, { objectType: 'iframe' } )"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image3','','/design/images/press-vic-health-btn-over.jpg',1)">
                    <img src="/design/images/press-victoriahealth-btn.jpg" alt="Victoria Health" title="Click to enlarge"
                        name="Image3" width="289" height="140" border="0" id="Image3" /></a></div>
            <div id="press2">
                <a href="press/the-sun-press.html" class="highslide" onclick="return hs.htmlExpand(this, { objectType: 'iframe' } )"
                    onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image10','','/design/images/press-sun-btn-over.jpg',1)">
                    <img src="/design/images/press-sun-btn.jpg" alt="The Sun" title="Click to enlarge"
                        name="Image10" width="289" height="140" border="0" id="Image10" /></a></div>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        self.setTimeout("preloadImg()",400);
        function preloadImg()
        {
            MM_preloadImages('/design/images/press-dailymail-btn-over.jpg','/design/images/press-discovery-btn-over.jpg','/design/images/press-ivillage-btn-over.jpg','/design/images/press-menshealth-btn-over.jpg','/design/images/press-mother-btn-over.jpg','/design/images/press-sun-btn-over.jpg','/design/images/press-times-btn-over.jpg','/design/images/press-vic-health-btn-over.jpg','/design/images/press-whispermag-btn-over.jpg','/design/images/press-titchmarsh-btn.jpg');
        }
    </script>
</asp:Content>

