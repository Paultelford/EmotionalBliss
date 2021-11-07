<%@ Page Title="" Language="VB" MasterPageFile="~/msite.master" AutoEventWireup="false"
    CodeFile="productReviews.aspx.vb" Inherits="productReviews" %>

<asp:Content ID="Content1" ContentPlaceHolderID="contentTop" runat="Server">
<style>
    a.ReadReviews
    {
        display: block;
        height: 22px;
        width: 99px;
        text-decoration: none;
        background-image: url(/images2011/btn_ReadReviews.png);
    }
    a.ReadReviews:hover
    {
        background-position: bottom;
    }
    a.WriteReviews {
	display: block;
	height: 22px;
	width:99px;
	text-decoration: none;
	background-image:url(../images/btn_WriteReviews.png);
	}

    a.WriteReviews:hover {
	background-position: bottom;
	}
    h3
    {
        font-size: 20px;
        font-weight: 100;
        background: -webkit-gradient(linear,left top,left bottom,from(#eee),to(#757575));
        -webkit-background-clip: text;
        -webkit-text-fill-color: transparent;
        margin-bottom: -17px;
        letter-spacing: -1px;
    }
    .cellUnderline
    {
        background-image: url(/images2011/line_dashHorizontal.gif);
        background-repeat: repeat-x;
        background-position: bottom;
        padding: 10px;
        line-height: 14px;
    }
    .reviewsTable
    {
        font-size: 13px;
        line-height: 14px;
        font-style: italic;
        margin-top: 10px;
        padding-top: 10px;
        padding-bottom: 50px;
        z-index: 100;
        padding-left: 10px;
        padding-right: 10px;
        text-align: justify;
    }
    .reviewsTable Strong
    {
        font-size: 14px;
        font-weight: 200;
        line-height: 30px;
    }
    .reviewsTable td
    {
        padding-bottom: 30px;
        background-image: url(/images/line_dashHorizontal.gif);
        background-repeat: repeat-x;
        background-position: bottom;
    }
    .reviewsStars
    {
    }
    .reviewsStars td
    {
        background-image: none;
    }
    a.AddReviewRollover, #AddReviewRollover {
	    display: block;
	    background-color:#0FC;
	    height: 28px;
	    width:119px;
	    text-decoration: none;
	    background-image:url('images/btn_addreview.gif');

	}
	
    a.AddReviewRollover:hover, #AddReviewRollover:hover {
	    background-position: bottom;
	}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentLeftMenu" runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <br/>
    <asp:Image ID="imgProductBig" runat="server" />
    <h3>
        <asp:Label runat="server" ID="lblHeader"></asp:Label><span style="color: #8b6ea8;"></span></h3>
    <br>
    <asp:Literal runat="server" ID="litTopDash">
        <div id="DashedLineHorizontal">
    </asp:Literal>
    </div>
    <table width="100%" border="0" cellspacing="0" cellpadding="0" runat="server" id="tblFront">
        <tr>
            <td valign="top" style="padding-left: 20px; padding-top: 0px; line-height: 22px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td align="center">
                                        <table class="reviewsTable" width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td class="cellUnderline" width="50%" align="left">
                                                    <a href="review_womolia.html">
                                                        <img src="images2011/home_womolia_deepLilac.jpg" alt="Womolia" width="230" height="154"
                                                            border="0"></a>
                                                </td>
                                                <td align="center" valign="middle" class="cellUnderline">
                                                    <table width="80%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="padding-left: 5px;">
                                                                <h4>
                                                                    Womolia Heat<br>
                                                                </h4>
                                                            </td>
                                                            <td width="110" valign="bottom">
                                                                <a href="/productReviews.aspx?product=womolia" class="ReadReviews" title="Read Reviews"></a><a href="/productReviews.aspx?product=womolia&write"
                                                                    class="WriteReviews" title="Write Review"></a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cellUnderline" align="left">
                                                    <a href="review_femblossom.html">
                                                        <img src="images2011/home_femblossom_lightJade.jpg" alt="Womolia" width="230" height="154"
                                                            border="0"></a>
                                                </td>
                                                <td class="cellUnderline" align="center">
                                                    <table width="80%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="padding-left: 5px;">
                                                                <h4>
                                                                    Femblossom Heat<br>
                                                                </h4>
                                                            </td>
                                                            <td width="110" valign="bottom">
                                                                <a href="/productReviews.aspx?product=femblossom" class="ReadReviews" title="Read Reviews"></a><a
                                                                    href="/productReviews.aspx?product=femblossom&write" class="WriteReviews" title="Write Review">
                                                                </a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="cellUnderline" align="left">
                                                    <a href="review_chandra.html">
                                                        <img src="images2011/home_chandra_zenGreen.jpg" alt="Womolia" width="230" height="154"
                                                            border="0"></a>
                                                </td>
                                                <td class="cellUnderline" align="center">
                                                    <table width="80%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="padding-left: 5px;">
                                                                <h4>
                                                                    Chandra</h4>
                                                            </td>
                                                            <td width="110" valign="bottom">
                                                                <a href="/productReviews.aspx?product=chandra" class="ReadReviews" title="Read Reviews"></a><a href="/productReviews.aspx?product=chandra&write"
                                                                    class="WriteReviews" title="Write Review"></a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="cellUnderline">
                                                    <a href="review_isis.html">
                                                        <img src="images2011/home_isis_warmOrange.jpg" alt="Womolia" width="230" height="154"
                                                            border="0"></a>
                                                </td>
                                                <td class="cellUnderline" align="center">
                                                    <table width="80%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="padding-left: 5px;">
                                                                <h4>
                                                                    Isis</h4>
                                                            </td>
                                                            <td width="110" valign="bottom">
                                                                <a href="/productReviews.aspx?product=isis" class="ReadReviews" title="Read Reviews"></a><a href="/productReviews.aspx?product=isis&write"
                                                                    class="WriteReviews" title="Write Reviews"></a>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br>
                <br>
            </td>
        </tr>
    </table>
    <asp:Panel id="panReviews" runat="server" Visible="false">
        <asp:HyperLink runat="server" id="lnkAddReview" CssClass="AddReviewRollover"></asp:HyperLink>
        <asp:ListView ID="lvReview" runat="server" OnItemDataBound="lvReview_itemDataBound">
            <LayoutTemplate>
                <table class="reviewsTable" width="100%" border="0" cellspacing="0" cellpadding="0">
                <asp:PlaceHolder ID="itemPlaceHolder" runat="server"></asp:PlaceHolder>
                </table>    
            </LayoutTemplate>
            <ItemTemplate>
                <tr>
                    <td width="550">
                        <b>Reviewed By <asp:Literal id="litReviewedBy" runat="server" Text='<%# Eval("name") %>'></asp:Literal></b>
                        <br />
                        <asp:Literal ID="litReview" runat="server" Text='<%# Eval("review") %>'></asp:Literal>
                        <asp:HiddenField ID="hidScore" runat="server" Value='<%# Eval("score") %>' />
                    </td>
                    <td style="padding-top: 10px;" align="right" valign="top">
                    <table class="reviewsStars" width="75" border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <img src="/images2011/review_star.png" width="15" height="15" id="imgStar1" runat="server" visible="false">
                            </td>
                            <td>
                                <img src="/images2011/review_star.png" width="15" height="15" id="imgStar2" runat="server" visible="false">
                            </td>
                            <td>
                                <img src="/images2011/review_star.png" width="15" height="15" id="imgStar3" runat="server" visible="false">
                            </td>
                            <td>
                                <img src="/images2011/review_star.png" width="15" height="15" id="imgStar4" runat="server" visible="false">
                            </td>
                            <td>
                                <img src="/images2011/review_star.png" width="15" height="15" id="imgStar5" runat="server" visible="false">
                            </td>
                        </tr>
                    </table>
                </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                No reviews found for selected product
            </EmptyDataTemplate>
        </asp:ListView>
        <br>
        <br>
    </asp:Panel>
    <asp:Panel runat="server" ID="panWriteReview" Visible="False">
        <table class="reviewsTable"width="100%" border="0" cellspacing="0" cellpadding="0">
        <tr>
          <td width="550" valign="top">
          
          
          <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
              <td width="150">  <strong>Your Name:</strong><br></td>
              <td>
                  <asp:TextBox ID="txtReviewName" runat="server" MaxLength="20" ValidationGroup="newr" /><asp:RequiredFieldValidator runat="server" ID="reqReviewName" ControlToValidate="txtReviewName" ErrorMessage="* Required" ForeColor="red" Display="Static" ValidationGroup="newr"></asp:RequiredFieldValidator>
              </td>
            </tr>
            <tr>
              <td> <strong>Your Review:</strong><br></td>
              <td>
                <br>
                <asp:TextBox ID="txtReviewText" runat="server" TextMode="MultiLine" Width="370" Height="70" />  
              </td>
            </tr>
          </table>  <br><asp:LinkButton runat="server" ID="lnkReviewInsert" CssClass="AddReviewRollover" OnClick="lnkReviewInsert_click" ValidationGroup="newr"></asp:LinkButton></td>
          <td style="padding-top:10px;" align="right" valign="top"><table class="reviewsStars" style="display: none;" width="75" border="0" cellpadding="0" cellspacing="0">
            <tr>
              <td><img src="/images2011/review_star.png" width="15" height="15"></td>
              <td><img src="/images2011/review_star.png" alt="" width="15" height="15"></td>
              <td><img src="/images2011/review_star.png" alt="" width="15" height="15"></td>
              <td><img src="/images2011/review_star.png" alt="" width="15" height="15"></td>
              <td><img src="/images2011/review_star.png" alt="" width="15" height="15"></td>
            </tr>
          
          </table>
          <asp:DropDownList runat="server" ID="drpScore" AutoPostBack="False">
              <asp:ListItem Text="1 - Poor" Value="1"></asp:ListItem>
              <asp:ListItem Text="2 - Ok" Value="2"></asp:ListItem>
              <asp:ListItem Text="3 - Average" Value="3" Selected="True"></asp:ListItem>
              <asp:ListItem Text="4 - Good" Value="4"></asp:ListItem>
              <asp:ListItem Text="5 - Excellent" Value="5"></asp:ListItem>
          </asp:DropDownList>
          </td>
        </tr>
     
     
      </table>
    </asp:Panel>
    <asp:Label runat="server" ID="lblReviewInsert"></asp:Label>
</asp:Content>
