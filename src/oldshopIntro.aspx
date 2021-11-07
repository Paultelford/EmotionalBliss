<%@ Page Language="VB" MasterPageFile="~/m_shop.master" AutoEventWireup="false" CodeFile="shopIntro.aspx.vb" Inherits="shop_shopIntro" Title="Untitled Page" %>
<%@ Register TagPrefix="menu" TagName="EBMenu" Src="~/EBShopMenu.ascx" %>
<asp:Content ID="ContntMenu" ContentPlaceHolderID="ContentLeftMenu" runat="server">
    <menu:EBMenu ID="ebMenu1" runat="server" menuName="shop" master="m_site">
    </menu:EBMenu>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <img src="/design/shop/images/intimate-massag-ttl.gif" alt="Intimate Massagers" width="600" height="50" /><br />
    <div id="shopcontainer">
        <div id="buttonshop1">
            <a href="shop/product.aspx?id=37" target="_self">
                <img src="/design/shop/images/shop-womolia-btn.jpg" alt="Intimate Massagers - Womolia Heat" width="275" height="180" border="0" /></a>
            </div>
        <div id="buttonshop2">
            <a href="shop/product.aspx?id=34" target="_self">
                <img src="/design/shop/images/shop-femblossom-btn.jpg" alt="Intimate Massagers - Femblossom Heat" width="275" height="180" />
            </a>
        </div>
    </div>
    <div id="shopcontainer">
        <div id="buttonshop1">
            <img src="/design/shop/images/shop-jasmine-btn.jpg" alt="Intimate Massagers - Jasmine Heat" width="275" height="180" />
        </div>
        <div id="buttonshop2">
        </div>
    </div>
    <br class="clear" />
    <img src="/design/shop/images/finger-massag-ttl.gif" alt="'Finger' Massagers" width="600" height="50" />
    <div id="shopcontainer">
        <div id="buttonshop1">
            <img src="/design/shop/images/shop-chandra-btn.jpg" alt="'Finger' Massagers - Chandra" width="275" height="180" />
        </div>
        <div id="buttonshop2">
            <img src="/design/shop/images/shop-isis-btn.jpg" alt="'Finger' Massagers - Isis" width="275" height="180" />
        </div>
    </div>
    <br class="clear" />
    <img src="/design/shop/images/other-products-ttl.gif" alt="Other products" width="600" height="50" />
    <div id="otherproducts">
        <a href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image22','','/design/shop/images/shop-lubricant-btn2.jpg',1)">
            <img src="/design/shop/images/shop-lubricant-btn.jpg" alt="Lubricants" name="Image22" width="120"
                height="45" border="0" id="Image22" /></a><a href="#" onmouseout="MM_swapImgRestore()"
                    onmouseover="MM_swapImage('Image21','','/design/shop/images/shop-accessories-btn2.jpg',1)"><img
                        src="/design/shop/images/shop-accessories-btn.jpg" alt="Accessories" name="Image21" width="120"
                        height="45" border="0" id="Image21" /></a><a href="#" onmouseout="MM_swapImgRestore()"
                            onmouseover="MM_swapImage('Image24','','/design/shop/images/shop-consultancy-btn2.jpg',1)"><img
                                src="/design/shop/images/shop-consultancy-btn.jpg" alt="Online Consultancy" name="Image24"
                                width="120" height="45" border="0" id="Image24" /></a><a href="#" onmouseout="MM_swapImgRestore()"
                                    onmouseover="MM_swapImage('Image23','','/design/shop/images/shop-gift-btn2.jpg',1)"><img src="/design/shop/images/shop-gift-btn.jpg"
                                        alt="Gift Vouchers" name="Image23" width="120" height="45" border="0" id="Image23" /></a><a
                                            href="#" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image20','','/design/shop/images/shop-books-btn2.jpg',1)"><img
                                                src="/design/shop/images/shop-books-btn.jpg" alt="Books" name="Image20" width="120" height="45"
                                                border="0" id="Image20" /></a></div>
<script type="text/JavaScript">
<!--
function MM_swapImgRestore() { //v3.0
  var i,x,a=document.MM_sr; for(i=0;a&&i<a.length&&(x=a[i])&&x.oSrc;i++) x.src=x.oSrc;
}

function MM_preloadImages() { //v3.0
  var d=document; if(d.images){ if(!d.MM_p) d.MM_p=new Array();
    var i,j=d.MM_p.length,a=MM_preloadImages.arguments; for(i=0; i<a.length; i++)
    if (a[i].indexOf("#")!=0){ d.MM_p[j]=new Image; d.MM_p[j++].src=a[i];}}
}

function MM_findObj(n, d) { //v4.01
  var p,i,x;  if(!d) d=document; if((p=n.indexOf("?"))>0&&parent.frames.length) {
    d=parent.frames[n.substring(p+1)].document; n=n.substring(0,p);}
  if(!(x=d[n])&&d.all) x=d.all[n]; for (i=0;!x&&i<d.forms.length;i++) x=d.forms[i][n];
  for(i=0;!x&&d.layers&&i<d.layers.length;i++) x=MM_findObj(n,d.layers[i].document);
  if(!x && d.getElementById) x=d.getElementById(n); return x;
}

function MM_swapImage() { //v3.0
  var i,j=0,x,a=MM_swapImage.arguments; document.MM_sr=new Array; for(i=0;i<(a.length-2);i+=3)
   if ((x=MM_findObj(a[i]))!=null){document.MM_sr[j++]=x; if(!x.oSrc) x.oSrc=x.src; x.src=a[i+2];}
}
//-->
</script>                                                
</asp:Content>
