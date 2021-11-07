<%@ Page Language="VB" AutoEventWireup="false" CodeFile="newHomeIntro.aspx.vb" Inherits="newHomeIntro" %>

<!DOCTYPE html>
<html lang="en">

<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
    <meta name="description" content="">
    <meta name="author" content="">

    <title>Emotional Bliss Intimate Massagers and Vibrators</title>

    <!-- Bootstrap core CSS -->
    <style>
        #displaybox {
            z-index: 10000;
            filter: alpha(opacity=90); /*older IE*/
            filter: progid:DXImageTransform.Microsoft.Alpha(opacity=50); /* IE */
            -moz-opacity: .90; /*older Mozilla*/
            -khtml-opacity: 0.9; /*older Safari*/
            opacity: 0.9; /*supported by current Mozilla, Safari, and Opera*/
            background-color: #000000;
            position: fixed;
            top: 0px;
            left: 0px;
            width: 100%;
            height: 100%;
            color: #FFFFFF;
            text-align: center;
            vertical-align: middle;
        }

        .setimgsize {
            width: 30px;
        }

        .countryscroll {
            overflow: auto !important;
            max-height: 250px !important;
        }

        .ReadMoreHover {
            color: blue !important;
        }

            .ReadMoreHover:hover {
                text-decoration: underline !important;
                cursor: pointer !important;
            }


        div#productSlider a span {
            background-color: #8FC2B1 !important;
            color: #fff;
        }
        div#productSlider2 a span {
            background-color: #6B9FCC !important;
            color: #fff;
        }

        @media (max-width: 576px) {
            #carouselExampleControls.d-sm-none.d-md-block {
                display: none
            }
        }

        @media (max-width: 767px) {
            .callinfodata {
                column-count: 1 !important;
            }

                .callinfodata .inrdatacall {
                    display: block;
                    margin-bottom: 20px;
                }
        }
    </style>
    <link href="HomePageJS/css/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" crossorigin="anonymous">
    <link href="HomePageJS/css/style.css" rel="stylesheet" />
    <%--<link href='css/style11.css' rel='stylesheet' type='text/css'>--%>
    <link href="HomePageJS/css/Custome.css" rel="stylesheet" />
    <link rel="stylesheet" href="css/tell_friend.css" type="text/css" media="screen" />
    <script type="text/javascript" src="js/tell_friend.js"></script>

    <script type="text/javascript">
        $(document).ready(function () {
            $(".trigger").click(function () {
                $(".panel").toggle("fast");
                $(this).toggleClass("active");
                return false;
            });
            $("#ReadMoreFH").hide();
            $("#ReadMoreWH").hide();
        });
        function clicker2() {
            var thediv = document.getElementById('displaybox');
            if (thediv.style.display == "none") {
                thediv.style.display = "";
                //thediv.innerHTML = "<table width='100%' height='100%'><tr><td align='center' valign='middle' width='100%' height='100%'><object classid='clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B' codebase='http://www.apple.com/qtactivex/qtplugin.cab' width='640' height='500'><object style='height: 390px; width: 640px'><param name='movie' value='http://www.youtube.com/v/JsuS--yvcCc?version=3&feature=player_detailpage&autoplay=1'><param name='allowFullScreen' value='true'><param name='allowScriptAccess' value='always'><embed src='http://www.youtube.com/v/JsuS--yvcCc?version=3&feature=player_detailpage&autoplay=1' type='application/x-shockwave-flash' allowfullscreen='true' allowScriptAccess='always' width='640' height='500'></object><br><br><a href='#' onclick='return clicker();'>Close Video</a></td></tr></table>";
                thediv.innerHTML = "<table width='100%' height='100%'><tr><td align='center' valign='middle' width='100%' height='100%'><iframe width='320' height='80' src='/audio/EB Front Page Audio.mp3' frameborder='0' allowfullscreen></iframe><br><br><a href='#' onclick='return clicker2();'><img border='0' src='/images/close.png'><br>Close Player</a></td></tr></table>";
            } else {
                thediv.style.display = "none";
                thediv.innerHTML = '';
            }
            return false;
        }
        function playaudio(fn) {
            var thediv = document.getElementById('displaybox');
            if (thediv.style.display == "none") {
                thediv.style.display = "";
                //thediv.innerHTML = "<table width='100%' height='100%'><tr><td align='center' valign='middle' width='100%' height='100%'><object classid='clsid:02BF25D5-8C17-4B23-BC80-D3488ABDDC6B' codebase='http://www.apple.com/qtactivex/qtplugin.cab' width='640' height='500'><object style='height: 390px; width: 640px'><param name='movie' value='http://www.youtube.com/v/JsuS--yvcCc?version=3&feature=player_detailpage&autoplay=1'><param name='allowFullScreen' value='true'><param name='allowScriptAccess' value='always'><embed src='http://www.youtube.com/v/JsuS--yvcCc?version=3&feature=player_detailpage&autoplay=1' type='application/x-shockwave-flash' allowfullscreen='true' allowScriptAccess='always' width='640' height='500'></object><br><br><a href='#' onclick='return clicker();'>Close Video</a></td></tr></table>";
                thediv.innerHTML = "<table width='100%' height='100%'><tr><td align='center' valign='middle' width='100%' height='100%'><iframe width='320' height='80' src='/audio/" + fn + ".mp3' frameborder='0' allowfullscreen></iframe><br><br><a href='#' onclick='return clicker2();'><img border='0' src='/images/close.png'><br>Close Player</a></td></tr></table>";
            } else {
                thediv.style.display = "none";
                thediv.innerHTML = '';
            }
            return false;
        }
        $(function () {
            var CurrentURL = window.location.href;
            const CountryUrl = ["http://gb.emotionalbliss.com/newHomeIntro.aspx", "http://us.emotionalbliss.com/newHomeIntro.aspx", "http://au.emotionalbliss.com/newHomeIntro.aspx", "http://at.emotionalbliss.com/newHomeIntro.aspx", "http://be.emotionalbliss.com/newHomeIntro.aspx", "http://bg.emotionalbliss.com/newHomeIntro.aspx", "http://ca.emotionalbliss.com/newHomeIntro.aspx", "http://cy.emotionalbliss.com/newHomeIntro.aspx", "http://cz.emotionalbliss.com/newHomeIntro.aspx", "http://dk.emotionalbliss.com/newHomeIntro.aspx", "http://ee.emotionalbliss.com/newHomeIntro.aspx", "http://fi.emotionalbliss.com/newHomeIntro.aspx", "http://fr.emotionalbliss.com/newHomeIntro.aspx", "http://de.emotionalbliss.com/newHomeIntro.aspx", "http://gr.emotionalbliss.com/newHomeIntro.aspx", "http://hu.emotionalbliss.com/newHomeIntro.aspx", "http://is.emotionalbliss.com/newHomeIntro.aspx", "http://ie.emotionalbliss.com/newHomeIntro.aspx", "http://it.emotionalbliss.com/newHomeIntro.aspx", "http://lv.emotionalbliss.com/newHomeIntro.aspx", "http://lt.emotionalbliss.com/newHomeIntro.aspx", "http://lu.emotionalbliss.com/newHomeIntro.aspx", "http://mt.emotionalbliss.com/newHomeIntro.aspx", "http://nl.emotionalbliss.com/newHomeIntro.aspx", "http://nz.emotionalbliss.com/newHomeIntro.aspx", "http://no.emotionalbliss.com/newHomeIntro.aspx", "http://pl.emotionalbliss.com/newHomeIntro.aspx", "http://pt.emotionalbliss.com/newHomeIntro.aspx", "http://ro.emotionalbliss.com/newHomeIntro.aspx", "http://sk.emotionalbliss.com/newHomeIntro.aspx", "http://si.emotionalbliss.com/newHomeIntro.aspx", "http://za.emotionalbliss.com/newHomeIntro.aspx", "http://es.emotionalbliss.com/newHomeIntro.aspx", "http://se.emotionalbliss.com/newHomeIntro.aspx", "http://ch.emotionalbliss.com/newHomeIntro.aspx"];
            const Countryimg = ["Media/uk.gif", "Media/us.gif", "Media/au.gif", "Media/at.gif", "Media/be.gif", "Media/bg.gif", "Media/ca.gif", "Media/cy.gif", "Media/cz.gif", "Media/dk.gif", "Media/ee.gif", "Media/fi.gif", "Media/fr.gif", "Media/de.gif", "Media/gr.gif", "Media/hu.gif", "Media/ic.gif", "Media/ie.gif", "Media/it.gif", "Media/lv.gif", "Media/lt.gif", "Media/lu.gif", "Media/mt.gif", "Media/nl.gif", "Media/nz.gif", "Media/no.gif", "Media/pl.gif", "Media/pt.gif", "Media/ro.gif", "Media/sk.gif", "Media/sl.gif", "Media/za.gif", "Media/es.gif", "Media/se.gif", "Media/ch.gif"];
            if (CountryUrl.includes(CurrentURL)) {
                document.getElementById("navbarDropdownMenuLink1").innerHTML = "<img class='setimgsize' src='" + Countryimg[CountryUrl.indexOf(CurrentURL)] + "'>";
            }
            const CountryUrl1 = ["http://gb.emotionalbliss.com/newHomeIntro.aspx?country=gb", "http://us.emotionalbliss.com/newHomeIntro.aspx?country=us", "http://au.emotionalbliss.com/newHomeIntro.aspx?country=au", "http://at.emotionalbliss.com/newHomeIntro.aspx?country=at", "http://be.emotionalbliss.com/newHomeIntro.aspx?country=be", "http://bg.emotionalbliss.com/newHomeIntro.aspx?country=BG", "http://ca.emotionalbliss.com/newHomeIntro.aspx?country=ca", "http://cy.emotionalbliss.com/newHomeIntro.aspx?country=CY", "http://cz.emotionalbliss.com/newHomeIntro.aspx?country=cz", "http://dk.emotionalbliss.com/newHomeIntro.aspx?country=dk", "http://ee.emotionalbliss.com/newHomeIntro.aspx?country=EE", "http://fi.emotionalbliss.com/newHomeIntro.aspx?country=fi", "http://fr.emotionalbliss.com/newHomeIntro.aspx?country=fr", "http://de.emotionalbliss.com/newHomeIntro.aspx?country=de", "http://gr.emotionalbliss.com/newHomeIntro.aspx?country=GR", "http://hu.emotionalbliss.com/newHomeIntro.aspx?country=HU", "http://is.emotionalbliss.com/newHomeIntro.aspx?country=is", "http://ie.emotionalbliss.com/newHomeIntro.aspx?country=ie", "http://it.emotionalbliss.com/newHomeIntro.aspx?country=it", "http://lv.emotionalbliss.com/newHomeIntro.aspx?country=LV", "http://lt.emotionalbliss.com/newHomeIntro.aspx?country=lt", "http://lu.emotionalbliss.com/newHomeIntro.aspx?country=lu", "http://mt.emotionalbliss.com/newHomeIntro.aspx?country=MT", "http://nl.emotionalbliss.com/newHomeIntro.aspx?country=nl", "http://nz.emotionalbliss.com/newHomeIntro.aspx?country=NZ", "http://no.emotionalbliss.com/newHomeIntro.aspx?country=no", "http://pl.emotionalbliss.com/newHomeIntro.aspx?country=PL", "http://pt.emotionalbliss.com/newHomeIntro.aspx?country=pt", "http://ro.emotionalbliss.com/newHomeIntro.aspx?country=RO", "http://sk.emotionalbliss.com/newHomeIntro.aspx?country=SK", "http://si.emotionalbliss.com/newHomeIntro.aspx?country=SI", "http://za.emotionalbliss.com/newHomeIntro.aspx?country=ZA", "http://es.emotionalbliss.com/newHomeIntro.aspx?country=es", "http://se.emotionalbliss.com/newHomeIntro.aspx?country=se", "http://ch.emotionalbliss.com/newHomeIntro.aspx?country=ch"];
            if (CountryUrl1.includes(CurrentURL)) {
                document.getElementById("navbarDropdownMenuLink1").innerHTML = "<img class='setimgsize' src='" + Countryimg[CountryUrl1.indexOf(CurrentURL)] + "'>";
            }
        });
        function ReadMore(option, element) {
            if (option == "more") {
                $("#" + element).show();
                $("#btn" + element).hide();
            }
            if (option == "less") {
                $("#" + element).hide();
                $("#btn" + element).show();
            }
        };
    </script>
    <script src="engine/js/jquery.min.js" type="text/javascript"></script>

</head>

<body>

    <!-- Navigation -->
    <nav class="navbar navbar-expand-lg navbar-dark bg-dark static-top">
        <div class="container">
            <a class="eblogo d-none d-md-block d-lg-none" href="index.html">
                <img src="HomePageJS/images/logo_eb.png" title="Emotional Bliss" />
            </a>
            <!-- <a class="navbar-brand" href="#">Emotional Bliss</a> -->
            <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarResponsive" aria-controls="navbarResponsive" aria-expanded="false" aria-label="Toggle navigation">
                <span class="navbar-toggler-icon"></span>
            </button>
            <div class="collapse navbar-collapse" id="navbarResponsive">
                <ul class="navbar-nav " style="margin: 0 auto">
                    <li class="nav-item active">
                        <a class="nav-link" href="newHomeIntro.aspx">Home
							
                            <span class="sr-only">(current)</span>
                        </a>
                    </li>
                    <li class="nav-item">
                        <%-- <%If Session("EBShopCountry").Equals("") <> True Then
                                %>--%>
                        <a class="nav-link" href="shopIntro.aspx?m=shop">Online Shop</a>
                        <%-- <%
                            End If%>--%>
                    </li>
                    <li class="nav-item">
                        <%--<a class="nav-link" href="advice.aspx?m=Advice">Advice </a>--%>
                        <a class="nav-link" href="advice.aspx?m=Advice">Enlighten </a>
                    </li>
                    <li class="nav-item">
                        <%--<a class="nav-link trigger" href="#">Tell A Friend  </a>--%>
                        <a class="nav-link trigger" href="#">Review   </a>
                    </li>
                    <li class="nav-item pdnzr d-md-none d-lg-block">
                        <a class="nav-link" href="newHomeIntro.aspx">
                            <img src="HomePageJS/images/logo_eb.png" title="Emotional Bliss" />
                        </a>
                    </li>
                    <li class="nav-item">
                        <%--<a class="nav-link" href="productReviews.aspx">Reviews</a>--%>
                        <a class="nav-link" href="/ebcontact.aspx">Contact Us</a>
                    </li>
                    <li class="nav-item">
                        <%--<a class="nav-link" href="static.aspx?p=introduction&m=b2b">B2B </a>--%>
                        <a class="nav-link" href="static.aspx?p=introduction&m=b2b">Track your order </a>
                    </li>
                    <li class="nav-item">
                        <%--<a class="nav-link" href="static.aspx?p=Terms_and_Conditions_of_Site&m=Legal">Legal </a>--%>
                    </li>
                    <li class="nav-item">
                        <%--<a class="nav-link" href="/shop/basket.aspx">Go to Checkout </a>--%>
                        <a class="nav-link" href="/shop/basket.aspx" title="Go to Checkout"><i class="fa fa-shopping-basket" aria-hidden="true"></i></a>
                    </li>
                    <li class="nav-item dropdown" id="myDropdown">
                        <a class="nav-link dropdown-toggle" id="navbarDropdownMenuLink1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Country </a>
                        <div class="dropdown-menu dropdown-primary countryscroll" aria-labelledby="navbarDropdownMenuLink1">
                            <a class="dropdown-item" href="http://gb.emotionalbliss.com/newHomeIntro.aspx?country=gb">
                                <img class="setimgsize" src="Media/uk.gif">
                                &nbsp;United Kingdom</a>
                            <a class="dropdown-item" href="http://us.emotionalbliss.com/newHomeIntro.aspx?country=us">
                                <img class="setimgsize" src="Media/us.gif">
                                &nbsp;United States</a>
                            <a class="dropdown-item" href="http://au.emotionalbliss.com/newHomeIntro.aspx?country=au">
                                <img class="setimgsize" src="Media/au.gif">
                                &nbsp;Australia</a>
                            <a class="dropdown-item" href="http://at.emotionalbliss.com/newHomeIntro.aspx?country=at">
                                <img class="setimgsize" src="Media/at.gif">
                                &nbsp;Austria</a>
                            <a class="dropdown-item" href="http://be.emotionalbliss.com/newHomeIntro.aspx?country=be">
                                <img class="setimgsize" src="Media/be.gif">
                                &nbsp;Belgium</a>
                            <a class="dropdown-item" href="http://bg.emotionalbliss.com/newHomeIntro.aspx?country=BG">
                                <img class="setimgsize" src="Media/bg.gif">
                                &nbsp;Bulgaria</a>
                            <a class="dropdown-item" href="http://ca.emotionalbliss.com/newHomeIntro.aspx?country=ca">
                                <img class="setimgsize" src="Media/ca.gif">
                                &nbsp;Canada</a>
                            <a class="dropdown-item" href="http://cy.emotionalbliss.com/newHomeIntro.aspx?country=CY">
                                <img class="setimgsize" src="Media/cy.gif">
                                &nbsp;Cyprus</a>
                            <a class="dropdown-item" href="http://cz.emotionalbliss.com/newHomeIntro.aspx?country=cz">
                                <img class="setimgsize" src="Media/cz.gif">
                                &nbsp;Czech Republic</a>
                            <a class="dropdown-item" href="http://dk.emotionalbliss.com/newHomeIntro.aspx?country=dk">
                                <img class="setimgsize" src="Media/dk.gif">
                                &nbsp;Denmark</a>
                            <a class="dropdown-item" href="http://ee.emotionalbliss.com/newHomeIntro.aspx?country=EE">
                                <img class="setimgsize" src="Media/ee.gif">
                                &nbsp;Estonia</a>
                            <a class="dropdown-item" href="http://fi.emotionalbliss.com/newHomeIntro.aspx?country=fi">
                                <img class="setimgsize" src="Media/fi.gif">
                                &nbsp;Finland</a>
                            <a class="dropdown-item" href="http://fr.emotionalbliss.com/newHomeIntro.aspx?country=fr">
                                <img class="setimgsize" src="Media/fr.gif">
                                &nbsp;France</a>
                            <a class="dropdown-item" href="http://de.emotionalbliss.com/newHomeIntro.aspx?country=de">
                                <img class="setimgsize" src="Media/de.gif">
                                &nbsp;Germany</a>
                            <a class="dropdown-item" href="http://gr.emotionalbliss.com/newHomeIntro.aspx?country=GR">
                                <img class="setimgsize" src="Media/gr.gif">
                                &nbsp;Greece</a>
                            <a class="dropdown-item" href="http://hu.emotionalbliss.com/newHomeIntro.aspx?country=HU">
                                <img class="setimgsize" src="Media/hu.gif">
                                &nbsp;Hungary</a>
                            <a class="dropdown-item" href="http://is.emotionalbliss.com/newHomeIntro.aspx?country=is">
                                <img class="setimgsize" src="Media/ic.gif">
                                &nbsp;Iceland</a>
                            <a class="dropdown-item" href="http://ie.emotionalbliss.com/newHomeIntro.aspx?country=ie">
                                <img class="setimgsize" src="Media/ie.gif">
                                &nbsp;Ireland</a>
                            <a class="dropdown-item" href="http://it.emotionalbliss.com/newHomeIntro.aspx?country=it">
                                <img class="setimgsize" src="Media/it.gif">
                                &nbsp;Italy</a>
                            <a class="dropdown-item" href="http://lv.emotionalbliss.com/newHomeIntro.aspx?country=LV">
                                <img class="setimgsize" src="Media/lv.gif">
                                &nbsp;Latvia</a>
                            <a class="dropdown-item" href="http://lt.emotionalbliss.com/newHomeIntro.aspx?country=lt">
                                <img class="setimgsize" src="Media/lt.gif">
                                &nbsp;Lithuania</a>
                            <a class="dropdown-item" href="http://lu.emotionalbliss.com/newHomeIntro.aspx?country=lu">
                                <img class="setimgsize" src="Media/lu.gif">
                                &nbsp;Luxembourg</a>
                            <a class="dropdown-item" href="http://mt.emotionalbliss.com/newHomeIntro.aspx?country=MT">
                                <img class="setimgsize" src="Media/mt.gif">
                                &nbsp;Malta</a>
                            <a class="dropdown-item" href="http://nl.emotionalbliss.com/newHomeIntro.aspx?country=nl">
                                <img class="setimgsize" src="Media/nl.gif">
                                &nbsp;Netherlands</a>
                            <a class="dropdown-item" href="http://nz.emotionalbliss.com/newHomeIntro.aspx?country=NZ">
                                <img class="setimgsize" src="Media/nz.gif">
                                &nbsp;New Zealand</a>
                            <a class="dropdown-item" href="http://no.emotionalbliss.com/newHomeIntro.aspx?country=no">
                                <img class="setimgsize" src="Media/no.gif">
                                &nbsp;Norway</a>
                            <a class="dropdown-item" href="http://pl.emotionalbliss.com/newHomeIntro.aspx?country=PL">
                                <img class="setimgsize" src="Media/pl.gif">
                                &nbsp;Poland</a>
                            <a class="dropdown-item" href="http://pt.emotionalbliss.com/newHomeIntro.aspx?country=pt">
                                <img class="setimgsize" src="Media/pt.gif">
                                &nbsp;Portugal</a>
                            <a class="dropdown-item" href="http://ro.emotionalbliss.com/newHomeIntro.aspx?country=RO">
                                <img class="setimgsize" src="Media/ro.gif">
                                &nbsp;Romania</a>
                            <a class="dropdown-item" href="http://sk.emotionalbliss.com/newHomeIntro.aspx?country=SK">
                                <img class="setimgsize" src="Media/sk.gif">
                                &nbsp;Slovakia</a>
                            <a class="dropdown-item" href="http://si.emotionalbliss.com/newHomeIntro.aspx?country=SI">
                                <img class="setimgsize" src="Media/sl.gif">
                                &nbsp;Slovenia</a>
                            <a class="dropdown-item" href="http://za.emotionalbliss.com/newHomeIntro.aspx?country=ZA">
                                <img class="setimgsize" src="Media/za.gif">
                                &nbsp;South Africa</a>
                            <a class="dropdown-item" href="http://es.emotionalbliss.com/newHomeIntro.aspx?country=es">
                                <img class="setimgsize" src="Media/es.gif">
                                &nbsp;Spain</a>
                            <a class="dropdown-item" href="http://se.emotionalbliss.com/newHomeIntro.aspx?country=se">
                                <img class="setimgsize" src="Media/se.gif">
                                &nbsp;Sweden</a>
                            <a class="dropdown-item" href="http://ch.emotionalbliss.com/newHomeIntro.aspx?country=ch">
                                <img class="setimgsize" src="Media/ch.gif">
                                &nbsp;Switzerland</a>
                        </div>
                    </li>
                </ul>
            </div>
        </div>
    </nav>

    <!-- Page Content -->
    <div class="" id="banner">
        <%--<img src="HomePageJS/images/banner1.jpg" class="img-fluid" style="width: 100%;" />--%>
        <div class="container">
            <div class="row">

                <div class="col-lg-12">
                    <h1 class="mt-5 col-lg-6" style="font-size: 35px !important;">Enjoy the profound health benefits of a sensational orgasm.</h1>
                    <p class="mt-5" style="font-size: 22px !important; text-align: right !important; margin-bottom: 7rem !important;"><i>The Female Orgasm is not a luxury it’s a necessity!</i></p>
                    <%--<p class="mt-5 mb-5 ">
                        <a href="/shopIntro.aspx?m=shop">
                            <img src="HomePageJS/images/readmore.png" class="img-fluid"></a>
                    </p>--%>
                </div>

                <%--<div class="col-lg-6">
                    <img src="HomePageJS/images/femblossomDefocus.png" class="img-fluid" style="width: 100%;">
                </div>--%>
            </div>
        </div>

    </div>
    <div class="" id="">
        <div class="container">
            <div class="row">

                <div class="col-lg-12 text-center">
                    <%--<h1 class="mt-5">Emotional Bliss Is In Your Grasp</h1>--%>
                    <h1 class="mt-5" style="font-weight: bold !important;">With stress levels raised like never before, its time to take matters into your own hands and focus on your own self-care.</h1>
                    <%--<p>With stress levels raised like never before, its time to take matters into your own hands and focus on your own self-care.</p>--%>
                    <%--<p style="text-align:justify!important;">After all, when you orgasm your brain releases vital “feel good” chemicals like oxytocin, serotonin, dopamine and endorphins so giving you the natural reset every woman needs. When the brain is re-energised this significantly reduces stress, anxiety and fights against depression whilst stimulating the mind, body and soul with intense pleasure. There’s never been a better time to raise your mood, protect your mental well-being with a sense of calm focused on your own self-care.</p>
                    <p style="text-align:justify!important;">Taking advantage of nature with your own mental pharmacy with the help of emotional bliss. So, whatever your age, our elegant intimate massagers have been designed and developed by Sexologists using sophisticated technology to stimulate the external nerve endings so you can relax and with a click of a button reconnect mind body and soul and experience your very own emotional bliss.</p>--%>
                    <%--<p>This is why it is vital that women reconnect with their body and reclaim their orgasm.</p>--%>
                    <p style="text-align: justify!important">After all, when you orgasm your brain releases vital “feel good” chemicals like oxytocin, serotonin, dopamine and endorphins so giving you the natural reset every woman needs. When the brain is replenished this significantly reduces stress, anxiety and fights against depression whilst stimulating the mind, body and soul with intense pleasure. There’s never been a better time to raise your mood, protect your mental well-being with a sense of calm focused on your own self-care.</p>
                    <p style="text-align: justify!important">Why not take advantage of nature by reconnecting with your own mental pharmacy with the help of emotional bliss, our elegant intimate massagers have been designed by Psychosexual Therapists using sophisticated technology to stimulate the external nerve endings so you can relax and with a click of a button reconnect mind body and soul and experience your very own emotional bliss.</p>
                    <p>
                        <h1 style="font-weight: bold !important; font-style: italic; text-align: center !important;">Self-care is Self-worth</h1>
                    </p>
                    <p style="text-align: justify!important">It is essential for women to orgasm so the brain can replenish and rebalance the natural chemicals within your body, it can take up to 20 minutes of external stimulation but once you achieve orgasm with the first rush of endorphins you can keep going to the next level with multiple orgasms, five orgasms is the average with an emotional bliss intimate massager.</p>
                </div>
            </div>
        </div>

    </div>
    <br />
    <br />
    <img src="HomePageJS/images/topf5.png" class="img-fluid" />
    <div class="" id="aboutus">
        <div class="container">
            <%--<p style="text-align:justify !important;">It is essential for all women to orgasm and reconnect with your body and for your brain to release and absorb the natural chemicals, it can take up to 20 minutes of external stimulation but once you achieve orgasm with emotional bliss you can keep going to the next level with multiple orgasms, five orgasms is the average with an emotional bliss intimate massager.</p>
            <center><h3> Click on the chemicals below to learn more about protecting your mental and emotional well-being.</h3></center>--%>
            <p style="text-align: justify !important;">Why are these chemicals important?</p>
            <p style="text-align: justify !important;">They are so important because chemical imbalances can seriously impact your behaviour and quality of life, men are encouraged to masturbate for health reasons whilst it’s frowned upon for women which is sad, the reality is it is more important for a woman to orgasm which explains why  it is estimated three times as many women on anti-depressants than men, emotional bliss was created to fight this cruel imbalance by encouraging women to reconnect naturally with their mind, body and soul with intense pleasure.</p>
            <p style="text-align: center !important;">Click below to find out more:</p>
        </div>
        <div class="container">

            <div class="" style="min-height: 50px;"></div>

            <div class="row">
                <div class="col-lg-1"></div>
                <div class="col-lg-2" style="padding-right: 0;">
                    <!-- required for floating -->
                    <!-- Nav tabs -->
                    <ul class="nav nav-tabs tabs-left">
                        <li class="active"><a href="#oxytocin" data-toggle="tab">1. Oxytocin</a></li>
                        <li><a href="#serotonin" data-toggle="tab">2. Serotonin</a></li>
                        <li><a href="#dopamine" data-toggle="tab">3. Dopamine</a></li>
                        <li><a href="#endorphins" data-toggle="tab">4. Endorphins</a></li>
                    </ul>
                </div>
                <div class="col-lg-8" style="padding-left: 0;">
                    <!-- Tab panes -->
                    <div class="tab-content">
                        <div class="tab-pane active" id="oxytocin">
                            <%--<p class="text-center">
                                <img src="HomePageJS/images/imgoxytocin1.png" class="img-fluid" alt="Oxytocin" title="Oxytocin" />
                            </p>
                            <p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>--%>
                            <%--<p style="text-align:justify">Reduces cardiovascular stress and improves the immune system.</p>
                            <p style="text-align:justify">Often referred to as “the cuddle hormone,” it has the power to regulate our emotional responses and pro-social behaviours, including trust, empathy, gazing, positive memories, processing of bonding cues, and positive communication.</p>
                            <p style="text-align:justify">Oxytocin released in the brain under stress-free conditions naturally promotes sleep by countering the effects of cortisol, which is known as the stress hormone so having a calming effect.</p>--%>
                            <p style="text-align: justify">Otherwise known as the “love hormone” Oxytocin is made up from nine amino acids and is essential during childbirth, breastfeeding and establishing parental bonding.</p>
                            <p style="text-align: justify">Without adequate oxytocin there’s no desire or interest in intimacy and is considered a significant factor with the loss of libido, other symptoms include vaginal dryness, persistently cold hands and feet, inability to bond with others and anxiety. Increasing the levels of oxytocin improves the desire to be touched and reduces cardiovascular stress, cravings and anxiety whilst improving immune response. Once the brain has been replenished with oxytocin immediately after your first orgasm you are capable of multiple orgasms with continued external stimulation.</p>
                            <p style="text-align: justify">It doesn’t stop there, Oxytocin has the power to regulate your emotional responses and pro-social behaviours including trust, empathy, gazing, positive memories, processing of bonding cues, and positive communication.</p>
                            <p style="text-align: justify">When released in the brain under stress-free conditions naturally promotes sleep by countering the effects of cortisol, which is known as the stress hormone so having a calming effect.</p>
                            <p style="text-align: justify">Research also shows that it may benefit people with an autistic spectrum disorder (ASD), anxiety, and irritable bowel syndrome (IBS).</p>
                        </div>

                        <div class="tab-pane" id="serotonin">
                            <%--<p>Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.</p>--%>
                            <%--<p style="text-align:justify">Is widely used in anti-depressants to prevent loneliness, anxiety and depression by influencing both mood and emotion.</p>
                            <p style="text-align:justify">Known as the “happy chemical” as it greatly influences an overall sense of well-being. It also helps to regulate moods, temper anxiety, and relieve depression. Low levels of serotonin are associated with depression, scientists are not sure if low levels are the cause of depression or if depression causes low levels but what we do now is that low levels of serotonin is a key indicator to depression.</p>
                            <p style="text-align:justify">
                                What is Serotonin?<br />
                                Serotonin is best described as a chemical communicator that carries signals from one part of the brain to another connection 40 million brain cells either directly or indirectly,
                            <ul style="padding-left: 10%;text-align: justify;">
                                <li style="list-style-type: disclosure-closed !important;"><strong>Cognition:</strong> High levels of serotonin have been shown to boost cognitive abilities including memory and learning speed.</li>
                                <li style="list-style-type: disclosure-closed !important;"><strong>Autonomic nervous system function:</strong> Studies have shown that serotonin enhances our autonomic nervous system function or fight-or-flight response.</li>
                                <li style="list-style-type: disclosure-closed !important;"><strong>Mood:</strong>  It’s widely believed that serotonin in the brain helps to reduce anxiety and depression, regulate our emotions, and contribute to and overall sense of well-being. Think of it like a natural mood stabilizer.</li>
                            </ul>
                            </p>

                            <p style="text-align:justify">It has a huge role with consciousness, attention, cognition, and emotion; but it regulates a bunch of other systems throughout your body too.</p>--%>
                            <p style="text-align: justify">Known as the “happy chemical” it greatly influences an overall sense of well-being by regulating mood, sleep, appetite, digestion, learning ability, memory and most importantly reduces anxiety and depression.</p>
                            <p style="text-align: justify">Serotonin is widely used in anti-depressants to prevent loneliness, anxiety and depression by influencing both mood and emotion.</p>
                            <p style="text-align: justify">When serotonin levels are normal you should feel focused, emotionally stable, happier, and calmer. When you reconnect with your body and orgasm your brain administers the correct chemicals and dosages to re-balance the levels of serotonin within your brain naturally.</p>
                        </div>

                        <div class="tab-pane" id="dopamine">
                            <%--<p class="text-center">
                                <img src="HomePageJS/images/imgdopamine1.png" class="img-fluid" alt="Oxytocin" title="Oxytocin" />
                            </p>
                            <p>It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.</p>--%>
                            <%--<p style="text-align:justify">Controls many functions, including behaviour, emotion and cognition.</p>
                            <p style="text-align:justify">Known as the “happy hormone” Dopamine motivates you to take action toward your goals and gives you a surge of reinforcing pleasure when achieving them. Procrastination, self-doubt, and lack of enthusiasm are linked with low levels of dopamine.</p>
                            <p style="text-align:justify">Dopamine protects against depression, anxiety, and sleep trouble.</p>--%>
                            <p style="text-align: justify">Controls many functions, including behaviour, emotion and cognition.</p>
                            <p style="text-align: justify">Known as the “happy hormone” Dopamine motivates you to take action toward your goals and gives you a surge of reinforcing pleasure when achieving them. Dopamine is associated with pleasurable sensations along with learning, memory, motor system function by interacting with the pleasure and reward centre of your brain. Dopamine along with other chemicals like serotonin, oxytocin and endorphins are vital to maintaining your mental and emotional well-being.</p>
                            <p style="text-align: justify">In addition to your mood dopamine also affects movement, memory, and focus. Healthy levels of dopamine drive us to seek and repeat pleasurable activities, while low levels can have an adverse physical and psychological impact.</p>
                            <p style="text-align: justify">Low levels of dopamine are linked with procrastination, self-doubt, lack of enthusiasm, depression and anxiety.</p>
                            <p style="text-align: justify">Chemical imbalances can and will seriously impact your behaviour and quality of life and will have a negative effect on your health which is why reconnecting with your body is not a luxury, it’s a necessity.</p>
                        </div>

                        <div class="tab-pane" id="endorphins">
                            <%--<p class="text-center">
                                <img src="HomePageJS/images/imgendorphins1.png" class="img-fluid" alt="Oxytocin" title="Oxytocin" />
                            </p>
                            <p>It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum. Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book.</p>--%>
                            <%--<p style="text-align:justify">Endorphins respond to pain and stress and helps to alleviate anxiety by acting on the opiate receptors within the brain to reducing pain and boost pleasure, resulting in a feeling of well-being.</p>
                            <p style="text-align:justify">We don’t understand all of the roles endorphins play in the body but we do know endorphins are important to reducing pain and enhance pleasure.</p>
                            <p style="text-align:justify">Other benefits of endorphins include, boosting your self-esteem and weight loss.</p>--%>
                            <p style="text-align: justify">Endorphins are your body’s natural pain reliever which is produced in response to stress, discomfort and anxiety by acting on the opiate receptors within the brain to reduce pain and enhance pleasure.</p>
                            <p style="text-align: justify">We don’t understand all of the roles endorphins play within the body but we do know endorphins are important by minimizing pain and helping us to continue functioning despite injury or working under stressful conditions.</p>
                            <p style="text-align: justify">Other benefits of endorphins include, boosting your self-esteem and weight loss.</p>
                        </div>
                    </div>
                </div>
                <div class="col-lg-1"></div>
            </div>

        </div>
    </div>
    <img src="HomePageJS/images/about-wave-bottom.png" class="img-fluid" />


    <div class="container">
        <div class="row">

            <div class="col-lg-12 text-center">
                <h1 class="mt-5 mb-5">Four Phases Of The Female Orgasm</h1>
                <p style="text-align: justify !important;">
                    An orgasm has been defined as a variable transient peak sensation of intense pleasure, creating an altered state of consciousness, usually accompanied by involuntary, rhythmic contractions of the pelvic striated musculature, often with concomitant anal contractions usually followed by an immense feeling of well-being and contentment.<br />
                    <br />
                    <b>We would prefer to say an orgasm is…</b>emotional bliss.
                                <br /><br />
                </p>
                <p style="text-align: center !important;">Female orgasms can take upwards of 20minutes of external stimulation, these are the four phases.</p>
            </div>
        </div>

        <div class="tabsystm">
            <div id="accordion">
                <div class="card">
                    <div class="card-header" id="headingOne">
                        <h5 class="mb-0" style="text-align: center!important;">
                            <button class="btn btn-link" data-toggle="collapse" data-target="#collapseOne" aria-expanded="true" aria-controls="collapseOne">
                                <span class="putbfr"></span>The anticipation phase 
                            </button>
                        </h5>
                    </div>

                    <div id="collapseOne" class="collapse show" aria-labelledby="headingOne" data-parent="#accordion">
                        <div class="card-body">
                            <%--Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.--%>
                            <%--<p style="text-align:justify;">Make yourself comfortable and relax, apply a little lubricant to the contact area of your massager and your clitoris and vulva then press the On button, you have nine precise levels of vibration so it’s time to explore the new sensations and relax and reconnect with your body and embrace your emotional wellbeing. Your body will respond with increased blood flow creating swelling and increased sensation. Phase 1 normally lasts between one to five minutes.</p>--%>
                            <p style="text-align: justify">Make yourself comfortable and relaxed, apply a little lubricant to the contact area of your massager, your clitoris and vulva then press the On button, you have seven precise levels of vibration with the added dimension of heat so it’s time to explore, relax and reconnect with your body and embrace your emotional wellbeing. Your body will respond with increased blood flow creating swelling and increased sensation. Phase 1 normally lasts between one to five minutes.</p>

                        </div>
                    </div>
                </div>

                <div class="card">
                    <div class="card-header" id="headingTwo">
                        <h5 class="mb-0" style="text-align: center!important;">
                            <button class="btn btn-link collapsed" data-toggle="collapse" data-target="#collapseTwo" aria-expanded="false" aria-controls="collapseTwo">
                                <span class="putbfr"></span>The tingle phase 
                            </button>
                        </h5>
                    </div>
                    <div id="collapseTwo" class="collapse" aria-labelledby="headingTwo" data-parent="#accordion">
                        <div class="card-body">
                            <%--Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.--%>
                            <p style="text-align: justify;">Your clitoris will begin to respond and gradually become erect as the vulva begins to deepen in colour so with continuous external stimulation the pleasure will gradually intensify, keep relaxed and don’t stop believing, enjoy the moment and your orgasm is on its way.</p>
                        </div>
                    </div>
                </div>

                <div class="card">
                    <div class="card-header" id="headingThree">
                        <h5 class="mb-0" style="text-align: center!important;">
                            <button class="btn btn-link collapsed" data-toggle="collapse" data-target="#collapseThree" aria-expanded="false" aria-controls="collapseThree">
                                <span class="putbfr"></span>Its not happening phase 
                            </button>
                        </h5>
                    </div>
                    <div id="collapseThree" class="collapse" aria-labelledby="headingThree" data-parent="#accordion">
                        <div class="card-body">
                            <%--Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.--%>
                            <%--<p style="text-align:justify;">Be patient and keep believing and remain relaxed, this phase is when most women give up thinking “The Tingle Phase” was their orgasm as the level of external stimulation appears to plateau, Not true, your orgasm is on its way but first your cervix and womb begin to lift high within the abdominal cavity so although things are happening on the inside there is no increased sensation on the outside, this can take up to five to ten minutes of continuous stimulation so keep relaxed and your orgasm is only a few minutes away. You will begin to feel the first waves of muscle spasms as your clitoris retracts under its hood in preparation of your orgasm.</p>--%>
                            <p style="text-align: justify">Be patient and keep believing and remain relaxed, this phase is when most women give up thinking “The Tingle Phase” was their orgasm as the level of external stimulation appears to plateau. Not True, your orgasm is on its way so keep believing, your cervix and womb are beginning to lift high within the abdominal cavity so although things are happening internally there is no increased sensations externally unless you have invited emotional bliss to the party, if you have you will now start to feel the heat radiating from the textured natural contours of your intimate massager encapsulating the intense vibration, stay relaxed because this phase can take up to five minutes of continuous stimulation and your orgasm is only a few minutes away. You will begin to feel the first waves of muscle spasms as your clitoris retracts under its hood in preparation of your orgasm, don’t stop and keep going</p>
                        </div>
                    </div>
                </div>

                <div class="card">
                    <div class="card-header" id="headingFour">
                        <h5 class="mb-0" style="text-align: center!important;">
                            <button class="btn btn-link collapsed" data-toggle="collapse" data-target="#collapseFour" aria-expanded="false" aria-controls="collapseFour">
                                <span class="putbfr"></span>Whats just happened phase 
                            </button>
                        </h5>
                    </div>
                    <div id="collapseFour" class="collapse" aria-labelledby="headingFour" data-parent="#accordion">
                        <div class="card-body">
                            <%--Anim pariatur cliche reprehenderit, enim eiusmod high life accusamus terry richardson ad squid. 3 wolf moon officia aute, non cupidatat skateboard dolor brunch. Food truck quinoa nesciunt laborum eiusmod. Brunch 3 wolf moon tempor, sunt aliqua put a bird on it squid single-origin coffee nulla assumenda shoreditch et. Nihil anim keffiyeh helvetica, craft beer labore wes anderson cred nesciunt sapiente ea proident. Ad vegan excepteur butcher vice lomo. Leggings occaecat craft beer farm-to-table, raw denim aesthetic synth nesciunt you probably haven't heard of them accusamus labore sustainable VHS.--%>
                            <p style="text-align: justify;">Boom!…waves of incredible pleasure as the vaginal walls, cervix, uterus and anal and  urethral sphincters contract rhythmically connecting both physical and mental wellbeing by triggering neurotransmitters to actively replenish and rebalance the brain with Oxytocin, Serotonin, Dopamine and Endorphins so empowering your mental and physical wellbeing. The average orgasm will last up to two minutes but it may have taken time and effort to reach the peak but once there you can keep going again and again, each time enriching your brain with natural hormones and chemicals to support you through the day whilst connecting your body with extreme pleasure.</p>
                        </div>
                    </div>
                </div>
            </div>

        </div>


        <div class="row">

            <div class="col-lg-12 text-center">
                <h1 class="mt-5">Self-Care Is Self-Worth</h1>
                <ul class="list-unstyled">
                    <li>Caring for your body is essential for your mental and emotional well-being</li>
                </ul>
            </div>
        </div>

        <div id="products" class="nowitsblue">
            <div class="row productsdata">
                <div class="col-lg-6">
                    <div id="productSlider" class="carousel slide" data-ride="carousel">
                        <div class="carousel-inner">
                            <div class="carousel-item active">
                                <img src="images2011/product_images/Femblossom/Medium/Mood_LightJade.jpg" class="d-block w-100" alt="..." />
                                <%--<img src="images/Mood_lightjade.png" class="d-block w-100" alt="...">--%>
                            </div>
                            <div class="carousel-item">
                                <img src="images2011/product_images/Femblossom/Small/Hand2_lightjade.jpg" class="d-block w-100" alt="..." />
                            </div>
                            <div class="carousel-item">
                                <img src="images2011/product_images/Femblossom/Small/Product_lightjade.jpg" class="d-block w-100" alt="..." />
                            </div>
                            <div class="carousel-item">
                                <img src="images2011/product_images/Femblossom/Small/Hand1_lightjade.jpg" class="d-block w-100" alt="..." />
                            </div>
                        </div>
                        <a class="carousel-control-prev" href="#productSlider" role="button" data-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="sr-only">Previous</span>
                        </a>
                        <a class="carousel-control-next" href="#productSlider" role="button" data-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="sr-only">Next</span>
                        </a>
                    </div>
                    <%--<p>
                        <img src="images2011/product_images/Femblossom/Medium/Mood_LightJade.jpg" class="img-fluid" style="width: 100% !important;" />
                    </p>--%>

                    <div class="callinfodata">
                        <span class="inrdatacall">
                            <p>
                                <img src="Content/Icons/icon1.png" />
                                <%--<img src="HomePageJS/images/icon1.png" />--%>
                            </p>
                            2.5 hrs USB Charging
                        </span>
                        <span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon2.png" />
                            </p>
                            30 Days Standby
                        </span>
                        <span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon3.png" />
                            </p>
                            90 Mins of Runtime
                        </span>
                        <span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon4.png" />
                            </p>
                            7 Simulating Modes
                        </span>
                        <span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon5.png" />
                            </p>
                            Warming Technology
                        </span>
                        <%--<span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon11.png" />
                            </p>
                            USB Charging
                        </span>--%>
                    </div>
                </div>

                <div class="col-lg-6">
                    <div class="headng">
                        <div class="h3" style="font-size: 51px; margin-top: 0; font-weight: 100; margin-bottom: 30px !important; color: #8fc2b1!important;">Femblossom Heat</div>
                    </div>
                    <div class="descshort">
                        <%--<p>This is why it is vital that women reconnect with their body and reclaim their orgasm. This is why it is vital that women reconnect with their body and reclaim their orgasm.</p>--%>
                        <p>Femblossom has a unique curvaceous shape designed to stimulate the thousands of sensitive nerves surrounding the clitoris and the super-sensitive labia simultaneously making for a more intense orgasm. It cleverly works in tune with your body to bring you intense pleasure ensuring that you achieve that perfect orgasm, again and again.<span id="btnReadMoreFH" class="ReadMoreHover" onclick="ReadMore('more','ReadMoreFH')"> read more</span></p>
                        <div id="ReadMoreFH">
                            <p>Pure elegance embracing sophisticated technology with seven different settings incorporating the advanced heat technology the Femblossom offers something for every woman and you will find yourself spoilt for choice.</p>
                            <p>A simple single-click function allows the Femblossom to gradually heat up to intensify the senses stimulating the orgasmic platform with the perfect shape, texture, vibration and heat to delicately increase arousal to heighten your arousal and intensify your pleasure. It’s easy to operate, and extremely quiet when in use.<span id="btnReadLessFH" class="ReadMoreHover" onclick="ReadMore('less','ReadMoreFH')"> read less</span></p>
                        </div>
                    </div>
                    <p>
                        <a href="/shopIntro.aspx?m=shop">
                            <img src="Content/Icons/read-more-green.png" class="img-fluid" />
                            <%--<img src="HomePageJS/images/readmore.png" class="img-fluid">--%></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <%--<a href="/shop/product.aspx?id=618">Buy Now</a>--%>
                        <a href="/shop/product.aspx?id=618">
                            <img src="Content/Icons/buy-now-green.png" /></a>

                    </p>
                </div>
            </div>
            <div class="row productsdata">
                <div class="col-lg-6">
                    <div class="headng">
                        <div class="h3" style="font-size: 51px; margin-top: 0; font-weight: 100; margin-bottom: 30px !important; color: #359cdf!important;">Womolia Heat</div>
                    </div>
                    <div class="descshort">
                        <%--<p>This is why it is vital that women reconnect with their body and reclaim their orgasm. This is why it is vital that women reconnect with their body and reclaim their orgasm.</p>--%>
                        <p>Womolia is a sophisticated design with a gentle curve so the heated angled tip can be used to stimulate the thousands of sensitive nerve endings within the entrance of the vagina, labia and the clitoris by concentrating the sensation on specific areas of your choosing. It cleverly works in tune with your body to bring you intense pleasure ensuring that you achieve that perfect orgasm, again and again.<span id="btnReadMoreWH" class="ReadMoreHover" onclick="ReadMore('more','ReadMoreWH')"> read more</span></p>
                        <div id="ReadMoreWH">
                            <p>Pure elegance embracing sophisticated technology with seven different settings incorporating the advanced heat technology the Womolia offers something for every woman and you will find yourself spoilt for choice.</p>
                            <p>A simple single-click function allows the Womolia to gradually heat up to intensify the senses stimulating the orgasmic platform with the perfect shape, texture, vibration and heat to delicately increase arousal to heighten your arousal and intensify your pleasure. It’s easy to operate, and extremely quiet when in use.<span id="btnReadLessWH" class="ReadMoreHover" onclick="ReadMore('less','ReadMoreWH')"> read less</span></p>
                        </div>
                    </div>
                    <p>
                        <a href="/shopIntro.aspx?m=shop">
                            <img src="Content/Icons/read-more-blue.png" class="img-fluid" />
                            <%--<img src="HomePageJS/images/readmore.png" class="img-fluid">--%></a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <%--<a href="/shop/product.aspx?id=619">Buy Now</a>--%>
                        <a href="/shop/product.aspx?id=619"><img src="Content/Icons/buy-now-blue.png" /></a>
                    </p>
                </div>

                <div class="col-lg-6">
                    <div id="productSlider2" class="carousel slide" data-ride="carousel">
                        <div class="carousel-inner">
                            <div class="carousel-item active">
                                <img src="images2011/product_images/Womolia/Medium/Mood_PowderBlue.jpg" class="d-block w-100" alt="...">
                            </div>
                            <div class="carousel-item">
                                <img src="images2011/product_images/Womolia/Small/Hand2_PowderBlue.jpg" class="d-block w-100" alt="...">
                            </div>
                            <div class="carousel-item">
                                <img src="images2011/product_images/Womolia/Small/Product_PowderBlue.jpg" class="d-block w-100" alt="...">
                            </div>
                            <div class="carousel-item">
                                <img src="images2011/product_images/Womolia/Small/Hand1_PowderBlue.jpg" class="d-block w-100" alt="...">
                            </div>
                        </div>
                        <a class="carousel-control-prev" href="#productSlider2" role="button" data-slide="prev">
                            <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                            <span class="sr-only">Previous</span>
                        </a>
                        <a class="carousel-control-next" href="#productSlider2" role="button" data-slide="next">
                            <span class="carousel-control-next-icon" aria-hidden="true"></span>
                            <span class="sr-only">Next</span>
                        </a>
                    </div>
                    <%--<p>
                        <img src="images2011/product_images/Womolia/Medium/Mood_PowderBlue.jpg" class="img-fluid" style="width: 100% !important;" />
                    </p>--%>
                    <div class="callinfodata">
                        <span class="inrdatacall">
                            <p>
                                <img src="Content/Icons/icon6.png" />
                                <%--<img src="HomePageJS/images/icon6.png" />--%>
                            </p>
                            2.5 hrs USB Charging
                        </span>
                        <span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon7.png" />
                            </p>
                            30 Days Standby
                        </span>
                        <span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon8.png" />
                            </p>
                            90 Mins of Runtime
                        </span>
                        <span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon9.png" />
                            </p>
                            7 Simulating Modes
                        </span>
                        <span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon10.png" />
                            </p>
                            Warming Technology
                        </span>
                        <%--<span class="inrdatacall">
                            <p>
                                <img src="HomePageJS/images/icon11.png" />
                            </p>
                            USB Charging
                        </span>--%>
                    </div>
                </div>
            </div>
        </div>

    </div>


    <img src="HomePageJS/images/bluetop.jpg" width="100%" />
    <!-- Page Content -->
    <div class="" id="testimonials">
        <div class="container">
            <div class="row">
                <div class="col-lg-12 text-center">
                    <h1 class="txtwhite mt-5">Testimonials</h1>
                    <ul class="list-unstyled txtwhite">
                        <li>Words from satisfied customers</li>
                        <li>about emotional bliss</li>
                    </ul>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <%--<div id="carouselExampleControls" class="carousel slide" data-ride="carousel" data-interval="5000">
                        <div class="w-100 carousel-inner" role="listbox">
                            
                            <div class="carousel-item active">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>I can honestly say that the Womolia is my FAVORITE product. Not only does it have a beautiful luxurious design and a feminine feel but it also has an ergonomic shape that stimulates just the right places. Who says beauty and function can't work together?</p>
                                            <p class="smallest mute">Nancy</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>Vibrators have been a part of my wife’s sex life since the 70's. At 61 she was becoming less responsive to her vibrator and figured it was just part of aging. I stumbled on emotional bliss by accident and was curious. An intimate massager designed for a woman by a Psychosexual therapists sounded like a formula for success. So I ordered one. My wife was eager to try it when it arrived. Well let me tell you, it's a winner. It put the OOOOoooooh back in orgasm. She described it as an O starting from inside to out. A slow climb and a long sustained peak before subsiding. Her bodys response was shear joy to watch. Great job! If you could just make it waterproof it would be perfect.</p>
                                            <p class="smallest mute">CS</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>Hello, I just received a Womolia that I purchased for my wife and wanted to commend your company on producing such an outstanding product. My wife is very reserved when it comes to her sexuality and I know that this product will not be intimidating to her as other products might. It's exceptionally well designed, very easy to operate and is surprisingly quiet. I even found the packaging to be extremely well done and the operation booklet to be very informative. Thank you for taking the time design and produce such a well thought out and well-made product. Without question, I will purchase other EB items.</p>
                                            <p class="smallest mute">John</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>My husband, surprised me with the Womolia and boy does it deliver. It’s a great stress reliever and the sensation it provides is unbelievable!!! I’m able to have multiple O’s one after another & what a feeling (can’t describe it in words). Thanks Mr. Lucky for my Gift, actually I’m the lucky one:)</p>
                                            <p class="smallest mute">VEN</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>I have recently purchased the Womolia and found it to be as good as you said. Delivery was extremely good. I had a query on the advertised price and spoke to Customer Services. The query was dealt with immediately and a refund was credited to my account straight away. I was given the impression that they really want the customer to be satisfied and happy. I am certainly that. Mrs S from Merseyside</p>
                                            <p class="smallest mute">Mrs S</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>The product was well made, well presented and felt really good to use. I would not improve anything about the product only a cover to hide the hole where the charger plugs in. the product was well presented and i am glad you thought about including some lubricant. I thought the booklet was really well written.</p>
                                            <p class="smallest mute">Tracey</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>Hi I've been raving about the products for ages based on info only, & having now road tested, today I went into the office with a big smile on my face! The girls thought it was hilarious, but they're all interested & have been on the website. I've always loved the non phallic look & feel & have recommended the products & the excellent website to many clients. I'm impressed with the vibration speed (It's like 0-60 in about 10 seconds) and with the variable speeds!</p>
                                            <p class="smallest mute">Carol</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>If you’ve never had a vibrator before and even if you have, there is nothing like the Femblossom for a mind blowing orgasm! I bought one for my girlfriend a couple of months ago and now we both can’t get enough of it! It always features prominently in our lovemaking and has enhanced the experience greatly. Beautifully designed for a perfect close fit the Femblossom really hits the spot and can be used without ‘getting in the way’. The lube that is supplied with it really does make for a wild orgasm! The Femblossom also looks so good in white and pastel green, you could imagine finding it on a High Street shop shelf next to the hairdryers and electric toothbrushes! Try one of these and you’ll never need another vibrator!</p>
                                            <p class="smallest mute">BR</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>I had the pleasure of trying the Femblossom. I thoroughly enjoyed using this product. Due to the shape, the Femblossom gives you very strong and intense clitoral orgasms and leaves you feeling very satisfied. The item also warms up, giving you a more pleasurable experience. I feel the booklet provided was excellent and very informative. It gave many hints and tips and also involves and encourages you to enjoy with your partner. The Emotional Bliss lubricant is all very good and would recommend you use plenty! The Femblossom will be a regularly used from now on both on my own and with my husband. I look forward to trying other Emotional Bliss products.</p>
                                            <p class="smallest mute">Charlene</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>I thought the Femblossom was great. Packaging was very impressive. On first impression the product looked quality and the catalogue was very in-depth. The massager itself looked a bit clinical but simple too. Loved the different speeds and when the heat kicked in it was lovely. My personal favourite though was the high speed as when I had worked myself up using the other functional speeds giving myself an orgasm was very easy and extremely pleasurable. I would definitely recommend this product and will gladly use again and again.</p>
                                            <p class="smallest mute">Mandi</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>Just wanted to drop a note to say how much your product has helped my sex life. After having my second child I was a little worried about the almost non-event that my orgasms had become. Well all that changed with the introduction of the Femblossom! Intense and completely different. Even my husband is in love with it. We've yet to try the Chandra as the Femblossom is amazing - but it's on our to-do list! Will recommend to every woman I know. Just thank you, thank you, thank you!!</p>
                                            <p class="smallest mute">&nbsp;</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>"Dr. Barb, Thank you for introducing me (and my husband) to the Emotional Bliss vibrator. It has enhanced our pleasure during sexual intercourse, and I give the product an A+. I would recommend it to anyone who is postmenopausal and having difficulty reaching an orgasm. " This quote is from a woman who had not been able to have an orgasm, at least felt like it 'wasn't worth the time and energy'.</p>
                                            <p class="smallest mute">Vicki</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>This is my favourite! I also have a Womolia, which I bought first, but when the Femblossom Heat came out I bought this as an alternative. For me personally this ticks all the boxes, pretty colour, discrete design, acceptably quiet and the extra points of contact achieved by the shape.... well say no more! If you only bought one product from Emotional Bliss, this would be my recommendation.</p>
                                            <p class="smallest mute">Elechim</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=" navi">
                            <a class="" href="#carouselExampleControls" role="button" data-slide="prev">
                                <span class="carousel-control-prev-icon ico" aria-hidden="true"></span>
                                <span class="sr-only">Previous</span>
                            </a>
                            <a class="" href="#carouselExampleControls" role="button" data-slide="next">
                                <span class="carousel-control-next-icon ico" aria-hidden="true"></span>
                                <span class="sr-only">Next</span>
                            </a>
                        </div>


                    </div>--%>
                    <div id="carouselExampleControls" class="carousel slide d-sm-none d-md-block" data-ride="carousel" data-interval="5000">
                        <div class="w-100 carousel-inner" role="listbox">
                            <div class="carousel-item active">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>I can honestly say that the Womolia is my FAVORITE product. Not only does it have a beautiful luxurious design and a feminine feel but it also has an ergonomic shape that stimulates just the right places. Who says beauty and function can't work together?</p>
                                            <p class="smallest mute">Nancy</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>Vibrators have been a part of my wife’s sex life since the 70's. At 61 she was becoming less responsive to her vibrator and figured it was just part of aging. I stumbled on emotional bliss by accident and was curious. An intimate massager designed for a woman by a Psychosexual therapists sounded like a formula for success. So I ordered one. My wife was eager to try it when it arrived. Well let me tell you, it's a winner. It put the OOOOoooooh back in orgasm. She described it as an O starting from inside to out. A slow climb and a long sustained peak before subsiding. Her bodys response was shear joy to watch. Great job! If you could just make it waterproof it would be perfect.</p>
                                            <p class="smallest mute">CS</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>Hello, I just received a Womolia that I purchased for my wife and wanted to commend your company on producing such an outstanding product. My wife is very reserved when it comes to her sexuality and I know that this product will not be intimidating to her as other products might. It's exceptionally well designed, very easy to operate and is surprisingly quiet. I even found the packaging to be extremely well done and the operation booklet to be very informative. Thank you for taking the time design and produce such a well thought out and well-made product. Without question, I will purchase other EB items.</p>
                                            <p class="smallest mute">John</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>My husband, surprised me with the Womolia and boy does it deliver. It’s a great stress reliever and the sensation it provides is unbelievable!!! I’m able to have multiple O’s one after another & what a feeling (can’t describe it in words). Thanks Mr. Lucky for my Gift, actually I’m the lucky one:)</p>
                                            <p class="smallest mute">VEN</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>I have recently purchased the Womolia and found it to be as good as you said. Delivery was extremely good. I had a query on the advertised price and spoke to Customer Services. The query was dealt with immediately and a refund was credited to my account straight away. I was given the impression that they really want the customer to be satisfied and happy. I am certainly that. Mrs S from Merseyside</p>
                                            <p class="smallest mute">Mrs S</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>The product was well made, well presented and felt really good to use. I would not improve anything about the product only a cover to hide the hole where the charger plugs in. the product was well presented and i am glad you thought about including some lubricant. I thought the booklet was really well written.</p>
                                            <p class="smallest mute">Tracey</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>Hi I've been raving about the products for ages based on info only, & having now road tested, today I went into the office with a big smile on my face! The girls thought it was hilarious, but they're all interested & have been on the website. I've always loved the non phallic look & feel & have recommended the products & the excellent website to many clients. I'm impressed with the vibration speed (It's like 0-60 in about 10 seconds) and with the variable speeds!</p>
                                            <p class="smallest mute">Carol</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>If you’ve never had a vibrator before and even if you have, there is nothing like the Femblossom for a mind blowing orgasm! I bought one for my girlfriend a couple of months ago and now we both can’t get enough of it! It always features prominently in our lovemaking and has enhanced the experience greatly. Beautifully designed for a perfect close fit the Femblossom really hits the spot and can be used without ‘getting in the way’. The lube that is supplied with it really does make for a wild orgasm! The Femblossom also looks so good in white and pastel green, you could imagine finding it on a High Street shop shelf next to the hairdryers and electric toothbrushes! Try one of these and you’ll never need another vibrator!</p>
                                            <p class="smallest mute">BR</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>I had the pleasure of trying the Femblossom. I thoroughly enjoyed using this product. Due to the shape, the Femblossom gives you very strong and intense clitoral orgasms and leaves you feeling very satisfied. The item also warms up, giving you a more pleasurable experience. I feel the booklet provided was excellent and very informative. It gave many hints and tips and also involves and encourages you to enjoy with your partner. The Emotional Bliss lubricant is all very good and would recommend you use plenty! The Femblossom will be a regularly used from now on both on my own and with my husband. I look forward to trying other Emotional Bliss products.</p>
                                            <p class="smallest mute">Charlene</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>I thought the Femblossom was great. Packaging was very impressive. On first impression the product looked quality and the catalogue was very in-depth. The massager itself looked a bit clinical but simple too. Loved the different speeds and when the heat kicked in it was lovely. My personal favourite though was the high speed as when I had worked myself up using the other functional speeds giving myself an orgasm was very easy and extremely pleasurable. I would definitely recommend this product and will gladly use again and again.</p>
                                            <p class="smallest mute">Mandi</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>Just wanted to drop a note to say how much your product has helped my sex life. After having my second child I was a little worried about the almost non-event that my orgasms had become. Well all that changed with the introduction of the Femblossom! Intense and completely different. Even my husband is in love with it. We've yet to try the Chandra as the Femblossom is amazing - but it's on our to-do list! Will recommend to every woman I know. Just thank you, thank you, thank you!!</p>
                                            <p class="smallest mute">...</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>"Dr. Barb, Thank you for introducing me (and my husband) to the Emotional Bliss vibrator. It has enhanced our pleasure during sexual intercourse, and I give the product an A+. I would recommend it to anyone who is postmenopausal and having difficulty reaching an orgasm. " This quote is from a woman who had not been able to have an orgasm, at least felt like it 'wasn't worth the time and energy'.</p>
                                            <p class="smallest mute">Vicki</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-4">
                                        <div class="testdesc">
                                            <p>This is my favourite! I also have a Womolia, which I bought first, but when the Femblossom Heat came out I bought this as an alternative. For me personally this ticks all the boxes, pretty colour, discrete design, acceptably quiet and the extra points of contact achieved by the shape.... well say no more! If you only bought one product from Emotional Bliss, this would be my recommendation.</p>
                                            <p class="smallest mute">Elechim</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class=" navi">
                            <a class="" href="#carouselExampleControls" role="button" data-slide="prev">
                                <span class="carousel-control-prev-icon ico" aria-hidden="true"></span>
                                <span class="sr-only">Previous</span>
                            </a>
                            <a class="" href="#carouselExampleControls" role="button" data-slide="next">
                                <span class="carousel-control-next-icon ico" aria-hidden="true"></span>
                                <span class="sr-only">Next</span>
                            </a>
                        </div>
                    </div>

                    <div id="carouselExampleControls1" class="carousel slide d-sm-block d-md-none" data-ride="carousel" data-interval="5000">
                        <div class="w-100 carousel-inner" role="listbox">
                            <div class="carousel-item active">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>I can honestly say that the Womolia is my FAVORITE product. Not only does it have a beautiful luxurious design and a feminine feel but it also has an ergonomic shape that stimulates just the right places. Who says beauty and function can't work together?</p>
                                            <p class="smallest mute">Nancy</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>Vibrators have been a part of my wife’s sex life since the 70's. At 61 she was becoming less responsive to her vibrator and figured it was just part of aging. I stumbled on emotional bliss by accident and was curious. An intimate massager designed for a woman by a Psychosexual therapists sounded like a formula for success. So I ordered one. My wife was eager to try it when it arrived. Well let me tell you, it's a winner. It put the OOOOoooooh back in orgasm. She described it as an O starting from inside to out. A slow climb and a long sustained peak before subsiding. Her bodys response was shear joy to watch. Great job! If you could just make it waterproof it would be perfect.</p>
                                            <p class="smallest mute">CS</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>Hello, I just received a Womolia that I purchased for my wife and wanted to commend your company on producing such an outstanding product. My wife is very reserved when it comes to her sexuality and I know that this product will not be intimidating to her as other products might. It's exceptionally well designed, very easy to operate and is surprisingly quiet. I even found the packaging to be extremely well done and the operation booklet to be very informative. Thank you for taking the time design and produce such a well thought out and well-made product. Without question, I will purchase other EB items.</p>
                                            <p class="smallest mute">John</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>My husband, surprised me with the Womolia and boy does it deliver. It’s a great stress reliever and the sensation it provides is unbelievable!!! I’m able to have multiple O’s one after another & what a feeling (can’t describe it in words). Thanks Mr. Lucky for my Gift, actually I’m the lucky one:)</p>
                                            <p class="smallest mute">VEN</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>I have recently purchased the Womolia and found it to be as good as you said. Delivery was extremely good. I had a query on the advertised price and spoke to Customer Services. The query was dealt with immediately and a refund was credited to my account straight away. I was given the impression that they really want the customer to be satisfied and happy. I am certainly that. Mrs S from Merseyside</p>
                                            <p class="smallest mute">Mrs S</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>The product was well made, well presented and felt really good to use. I would not improve anything about the product only a cover to hide the hole where the charger plugs in. the product was well presented and i am glad you thought about including some lubricant. I thought the booklet was really well written.</p>
                                            <p class="smallest mute">Tracey</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>Hi I've been raving about the products for ages based on info only, & having now road tested, today I went into the office with a big smile on my face! The girls thought it was hilarious, but they're all interested & have been on the website. I've always loved the non phallic look & feel & have recommended the products & the excellent website to many clients. I'm impressed with the vibration speed (It's like 0-60 in about 10 seconds) and with the variable speeds!</p>
                                            <p class="smallest mute">Carol</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>If you’ve never had a vibrator before and even if you have, there is nothing like the Femblossom for a mind blowing orgasm! I bought one for my girlfriend a couple of months ago and now we both can’t get enough of it! It always features prominently in our lovemaking and has enhanced the experience greatly. Beautifully designed for a perfect close fit the Femblossom really hits the spot and can be used without ‘getting in the way’. The lube that is supplied with it really does make for a wild orgasm! The Femblossom also looks so good in white and pastel green, you could imagine finding it on a High Street shop shelf next to the hairdryers and electric toothbrushes! Try one of these and you’ll never need another vibrator!</p>
                                            <p class="smallest mute">BR</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>I had the pleasure of trying the Femblossom. I thoroughly enjoyed using this product. Due to the shape, the Femblossom gives you very strong and intense clitoral orgasms and leaves you feeling very satisfied. The item also warms up, giving you a more pleasurable experience. I feel the booklet provided was excellent and very informative. It gave many hints and tips and also involves and encourages you to enjoy with your partner. The Emotional Bliss lubricant is all very good and would recommend you use plenty! The Femblossom will be a regularly used from now on both on my own and with my husband. I look forward to trying other Emotional Bliss products.</p>
                                            <p class="smallest mute">Charlene</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>I thought the Femblossom was great. Packaging was very impressive. On first impression the product looked quality and the catalogue was very in-depth. The massager itself looked a bit clinical but simple too. Loved the different speeds and when the heat kicked in it was lovely. My personal favourite though was the high speed as when I had worked myself up using the other functional speeds giving myself an orgasm was very easy and extremely pleasurable. I would definitely recommend this product and will gladly use again and again.</p>
                                            <p class="smallest mute">Mandi</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>Just wanted to drop a note to say how much your product has helped my sex life. After having my second child I was a little worried about the almost non-event that my orgasms had become. Well all that changed with the introduction of the Femblossom! Intense and completely different. Even my husband is in love with it. We've yet to try the Chandra as the Femblossom is amazing - but it's on our to-do list! Will recommend to every woman I know. Just thank you, thank you, thank you!!</p>
                                            <p class="smallest mute"></p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>"Dr. Barb, Thank you for introducing me (and my husband) to the Emotional Bliss vibrator. It has enhanced our pleasure during sexual intercourse, and I give the product an A+. I would recommend it to anyone who is postmenopausal and having difficulty reaching an orgasm. " This quote is from a woman who had not been able to have an orgasm, at least felt like it 'wasn't worth the time and energy'.</p>
                                            <p class="smallest mute">Vicki</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="carousel-item ">
                                <div class="row">
                                    <div class="col-sm-12">
                                        <div class="testdesc">
                                            <p>This is my favourite! I also have a Womolia, which I bought first, but when the Femblossom Heat came out I bought this as an alternative. For me personally this ticks all the boxes, pretty colour, discrete design, acceptably quiet and the extra points of contact achieved by the shape.... well say no more! If you only bought one product from Emotional Bliss, this would be my recommendation.</p>
                                            <p class="smallest mute">Elechim</p>
                                            <p class="">
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                                <i class="fa fa-star"></i>
                                            </p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--<div class="carousel-item active">
									<div class="row">
										<div class="col-sm-12">
											<div class="testdesc">
												<p>Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.</p>
												<p class="smallest mute">Joe</p>
												<p class="">
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
												</p>
											</div>
										</div>
									</div>
								</div>
								<div class="carousel-item ">
									<div class="row">
										<div class="col-sm-12">
											<div class="testdesc">
												<p>Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.</p>
												<p class="smallest mute">Joe</p>
												<p class="">
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
												</p>
											</div>
										</div>
										
									</div>
								</div>
								<div class="carousel-item ">
									<div class="row">
										<div class="col-sm-12">
											<div class="testdesc">
												<p>Cras justo odio, dapibus ac facilisis in, egestas eget quam. Donec id elit non mi porta gravida at eget metus. Nullam id dolor id nibh ultricies vehicula ut id elit.</p>
												<p class="smallest mute">Joe</p>
												<p class="">
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
													<i class="fa fa-star"></i>
												</p>
											</div>
										</div>
										
									</div>
								</div>--%>
                        </div>
                        <div class=" navi">
                            <a class="" href="#carouselExampleControls1" role="button" data-slide="prev">
                                <span class="carousel-control-prev-icon ico" aria-hidden="true"></span>
                                <span class="sr-only">Previous</span>
                            </a>
                            <a class="" href="#carouselExampleControls1" role="button" data-slide="next">
                                <span class="carousel-control-next-icon ico" aria-hidden="true"></span>
                                <span class="sr-only">Next</span>
                            </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div id="testibottom">
        <img src="HomePageJS/images/wave-blue-2@2x.png" class="img-fluid" style="width: 100%;" />
    </div>




    <div class="container">
        <div class="row">
            <div class="col-lg-12 text-center mt-5">
                <h1 class="">Advice From Experts</h1>
                <p class="mt-5 mb-5">With stress levels raised like never before, women should take matters into their own hands and focus on their own self-care.</p>
            </div>

            <div class="col-lg-4">
                <div class="thisexprt">
                    <div class="img-gradient">
                        <img class="" src="images/advice_cellimage01.jpg" width="100%" />
                    </div>
                    <h5>Variety of Sensations</h5>
                    <div class="rdmrethng">
                        <img src="HomePageJS/images/play-btn2.png" width="48px" height="48px" onclick='return playaudio("107");' />
                        <a href="/static.aspx?p=sensations&m=Advice">Read More</a>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="thisexprt">
                    <div class="img-gradient">
                        <img class="" src="images/advice_cellimage02.jpg" width="100%" />
                    </div>
                    <h5>Erogenous Zone</h5>
                    <div class="rdmrethng">
                        <img src="HomePageJS/images/play-btn2.png" width="48px" height="48px" onclick='return playaudio("102all");' />
                        <a href="/static.aspx?p=Erogenous_Zones&m=Advice">Read More</a>
                    </div>
                </div>
            </div>
            <div class="col-lg-4">
                <div class="thisexprt">
                    <div class="img-gradient">
                        <img class="" src="images/advice_cellimage11.jpg" width="100%" />
                    </div>
                    <h5>Why Masturbation is good for you</h5>
                    <div class="rdmrethng">
                        <img src="HomePageJS/images/play-btn2.png" width="48px" height="48px" onclick='return playaudio("106");' />
                        <a href="/static.aspx?p=masterbation&m=Advice">Read More</a>
                    </div>
                </div>
            </div>

        </div>

    </div>

    <div class="container">
        <div class="mt-5 mb-5" id="calltoaction">
            <div class="row">
                <div class="col-lg-6">
                    <h1 class="txtwhite ">Subscribe</h1>
                    <ul class="list-unstyled txtwhite">
                        <li>Join our mailing list to get updated</li>
                        <li>about products & advice from experts</li>
                    </ul>
                </div>
                <div class="col-lg-6">
                    <div class="form-group">
                        <label>Please enter your firstname so we can be friends*</label>
                        <input type="text" class="form-control" placeholder="First name">
                    </div>
                    <label>Email address to subscribe*</label>
                    <div class="input-group">
                        <input type="email" class="form-control" placeholder="Email address">
                        <span class="input-group-btn">
                            <button class="btn" type="submit">Subscribe</button>
                        </span>
                    </div>
                    <div class=" lastaccept form-group" style="margin-bottom: 0;">
                        <input type="checkbox" name="acceptterms" class="form-control">
                        <label>I agree to receive your newsletters and accept the data policy*</label>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <%-- Listen Audio Clip --%>
    <div id="displaybox" style="display: none;"></div>
    <%-- Tell a Friend --%>
    <form id="form1" runat="server">
        <div class="panel">
            <h3 class="tellfrndheader">Why not tell a friend and we'll send them a
            <asp:Label ID="lblDiscountAmount" runat="server">£5</asp:Label>
                discount voucher</h3>
            <br />
            <br />
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="100" valign="top">Your Name:
                                </td>
                                <td valign="top">
                                    <label for="YourName">
                                    </label>
                                    <asp:TextBox ID="txtYourName" runat="server" MaxLength="50" ValidationGroup="free" Style="margin-bottom: 5px;"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="reqYourName" runat="server" ControlToValidate="txtYourName" Display="Dynamic" ErrorMessage="* Required field" ForeColor="Red" ValidationGroup="free"></asp:RequiredFieldValidator>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="100" valign="top">Friends Name:
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtFriendsName" runat="server" MaxLength="50" ValidationGroup="free"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="reqFriendsName" runat="server" ControlToValidate="txtFriendsName" Display="Dynamic" ErrorMessage="* Required field" ForeColor="Red" ValidationGroup="free"></asp:RequiredFieldValidator>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="100" valign="top">Your eMail:
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtYourEmail" runat="server" MaxLength="100" ValidationGroup="free"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="reqYourEmail" runat="server" ControlToValidate="txtYourEmail" Display="Dynamic" ErrorMessage="* Required field" ForeColor="Red" ValidationGroup="free"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regYourEmail" runat="server" ControlToValidate="txtYourEmail" Display="Dynamic" ErrorMessage="* Invalid email address" ValidationGroup="free" ForeColor="Red" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$"></asp:RegularExpressionValidator>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td width="100" valign="top">Friends eMail:
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtFriendsEmail" runat="server" MaxLength="100" ValidationGroup="free"></asp:TextBox><br />
                                    <asp:RequiredFieldValidator ID="reqFriendsEmail" runat="server" ControlToValidate="txtFriendsEmail" Display="Dynamic" ErrorMessage="* Required field" ForeColor="Red" ValidationGroup="free"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="regFriendsEmail" runat="server" ControlToValidate="txtFriendsEmail" Display="Dynamic" ErrorMessage="* Invalid email address" ValidationGroup="free" ForeColor="Red" ValidationExpression="^[_a-zA-Z0-9-]+(\.[_a-zA-Z0-9-]+)*@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(([0-9]{1,3})|([a-zA-Z]{2,3})|(aero|coop|info|museum|name))$"></asp:RegularExpressionValidator>
                                    <br />
                                    <br />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td valign="top" width="100" valign="top">Message:
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtMessage" Width="150" Height="100" runat="server" TextMode="MultiLine" ValidationGroup="free"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td valign="bottom" align="right">
                        <table border="0" cellspacing="0" cellpadding="0" class="">
                            <tr>
                                <td>
                                    <asp:LinkButton ID="lnkSend" runat="server" CssClass="SendFriend" OnClick="lnkSend_click" ValidationGroup="free"></asp:LinkButton><br />
                                    <asp:Label ID="lblSentOK" runat="server"></asp:Label><br />
                                </td>
                                <td width="40">&nbsp;</td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="">
                                        <a href="" style="color: #000000; margin-left: 30px;">Close window</a>
                                    </div>
                                </td>
                                <td width="40">&nbsp;</td>
                            </tr>
                        </table>

                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
            </table>

            <div style="clear: both;">
            </div>
        </div>
    </form>
    <div class="" id="footer">
        <div class="container-fluid">
            <div class="row">
                <%--<div class="col-lg-6">
                    Copyright © 2002 - 2021 Emotional Bliss Innovative pioneers in quality and sexual wellbeing.
                </div>--%>
                <div class="col-lg-12 text-right">
                    <ul class="nav">
                        <li><a href="/privacy-statement.html">Privacy Policy</a></li>
                        <li><a href="/emotional-bliss-terms-and-conditions-of-site.html">Terms & Conditions</a></li>
                        <li><a href="/shop/basket.aspx">Shopping Basket</a></li>
                        <li><a href="/ebcontact.aspx">Contact Us</a></li>
                        <li><a href="/static.aspx?p=newsletter&m=home">Join Our Mailing List</a></li>
                        <li><a href="/traceOrder.aspx">Track Your Order</a></li>
                    </ul>
                </div>
            </div>
        </div>
    </div>

    <!-- Bootstrap core JavaScript -->
    <script src="HomePageJS/js/jquery/jquery.slim.min.js"></script>
    <script src="HomePageJS/css/bootstrap/js/bootstrap.bundle.min.js"></script>
    <script>
        jQuery(document).ready(function () {
            jQuery(".tabs-left li").click(function () {
                jQuery(".tabs-left li").removeClass("active");
                jQuery(this).addClass("active");
            });
        });
    </script>


</body>

</html>
