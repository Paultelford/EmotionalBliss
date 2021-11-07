<%@ Page Language="VB" MasterPageFile="~/MasterPageShop.master" AutoEventWireup="false" CodeFile="reviews.aspx.vb" Inherits="shop_reviews" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Label ID="lblCurrentReviewsText1" runat="server" dbResource="lblCurrentReviewsText1"></asp:Label>&nbsp;
    <asp:Label ID="lblNumberOfReviews" runat="server"></asp:Label>&nbsp;
    <asp:Label ID="lblCurrentReviewstext2" runat="server" dbResource="lblCurrentReviewsText12"></asp:Label>
    <br /><br />
    <asp:GridView ID="gvReviews" runat="server" DataSourceID="sqlReviews" AutoGenerateColumns="true">
        <Columns>
            
        </Columns>
    </asp:GridView>
    
    <asp:SqlDataSource ID="sqlReviews" runat="server" SelectCommand="procReviewsByPosIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:QueryStringParameter QueryStringField="posID" name="countryCode" Type="int32" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

