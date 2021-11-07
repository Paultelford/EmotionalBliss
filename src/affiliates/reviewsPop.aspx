<%@ Page Language="VB" AutoEventWireup="false" ValidateRequest="false" CodeFile="reviewsPop.aspx.vb" Inherits="affiliates_reviewsPop" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:FormView ID="fvReview" runat="server" DataSourceID="sqlReviewDetails" DataKeyNames="reviewID" Width="100%" OnDataBound="fvReview_dataBound">
            <ItemTemplate>
                <b>
                Details for <asp:Label id="lblProductName" runat="server" Text='<%# Eval("saleName") %>'></asp:Label> review:
                </b><br /><br />
                <table border="0" width="100%">
                    <tr>
                        <td valign="top" width="200">
                            <table border="0">
                                <tr>
                                    <td>
                                        Name:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Age:
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Sex:
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email:
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        How Often:
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Rating:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRating" runat="server" Text='<%# Eval("rating") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date Added:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDateAdded" runat="Server" Text='<%# showDate(Eval("dateAdded")) %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <br />
                                        
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="60">&nbsp;</td>
                        <td valign="top" height="100%">
                            <center>
                                <asp:Button ID="btnEditReview" runat="server" CommandName="edit" Text="Edit Review" />
                            </center>
                            <table height="100%" border="0" id="tblReviewOld" runat="server">
                                <tr>
                                    <td height="100%">
                                        <asp:Label ID="lblReview" runat="server" Text='<%# formatReview(Eval("review")) %>'></asp:Label><br />        
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="bottom">
                                        
                                    </td>
                                </tr>
                            </table>
                            <table height="100%" border="0" id="tblReview" runat="server">
                                <tr>
                                    <td>
                                        <b>What was your first impression of the item sent to you?</b><br />
                                        <asp:Label ID="lblQ1" runat="server" Text='<%# formatReview(Eval("q1")) %>'></asp:Label><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <b>What was the item like in action?</b><br />
                                        <asp:Label ID="lblQ2" runat="server" Text='<%# formatReview(Eval("q2")) %>'></asp:Label><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <b>Did the item make you feel sexy?</b><br />
                                        <asp:Label ID="lblQ3" runat="server" Text='<%# formatReview(Eval("q3")) %>'></asp:Label><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <b>What reaction did the item acheive - personally to you or with other parties?</b><br />
                                        <asp:Label ID="lblQ4" runat="server" Text='<%# formatReview(Eval("q4")) %>'></asp:Label><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <b>Any other comments?</b><br />
                                        <asp:Label ID="lblQ5" runat="server" Text='<%# formatReview(Eval("q5")) %>'></asp:Label><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <b>How could this item be improved?</b><br />
                                        <asp:Label ID="lblQ6" runat="server" Text='<%# formatReview(Eval("q6")) %>'></asp:Label><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <b>Would you recomend this item to a friend?</b><br />
                                        <asp:Label ID="lblQ7" runat="server" Text='<%# formatReview(Eval("q7")) %>'></asp:Label><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <b>Do you have any hot tips when using this item?</b><br />
                                        <asp:Label ID="lblQ8" runat="server" Text='<%# formatReview(Eval("q8")) %>'></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <center>
                                <asp:Button ID="btnEdit" runat="server" CommandName="edit" Text="Edit Review" />
                            </center>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
            <EditItemTemplate>
                <b>
                Details for <asp:Label id="lblProductName" runat="server" Text='<%# Eval("saleName") %>'></asp:Label> review:
                </b><br /><br />
                <table border="0" width="100%">
                    <tr>
                        <td valign="top" width="200">
                            <table border="0">
                                <tr>
                                    <td>
                                        Name:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Age:
                                    </td>
                                    <td>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Sex:
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Email:
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        How Often:
                                    </td>
                                    <td>
                                    
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Rating:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblRating" runat="server" Text='<%# Eval("rating") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date Added:
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDateAdded" runat="Server" Text='<%# showDate(Eval("dateAdded")) %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="60">&nbsp;</td>
                        <td valign="top">
                            <asp:TextBox ID="txtReview" runat="server" Text='<%# Bind("review") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox>
                            <table height="100%" border="0" >
                                <tr>
                                    <td>
                                        <asp:Label ID="lblQ1Text" runat="server">
                                            <b>What was your first impression of the item sent to you?</b><br />
                                        </asp:Label>                                        
                                        <asp:TextBox ID="lblQ1" runat="server" Text='<%# Bind("q1") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <asp:Label ID="lblQ2Text" runat="server">
                                            <b>What was the item like in action?</b><br />
                                        </asp:Label>
                                        <asp:TextBox ID="lblQ2" runat="server" Text='<%# Bind("q2") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <asp:Label ID="lblQ3Text" runat="server">
                                            <b>Did the item make you feel sexy?</b><br />
                                        </asp:Label>
                                        <asp:TextBox ID="lblQ3" runat="server" Text='<%# Bind("q3") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <asp:Label ID="lblQ4Text" runat="server">
                                            <b>What reaction did the item acheive - personally to you or with other parties?</b><br />
                                        </asp:Label>
                                        <asp:TextBox ID="lblQ4" runat="server" Text='<%# Bind("q4") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <asp:Label ID="lblQ5Text" runat="server">
                                            <b>Any other comments?</b><br />
                                        </asp:Label>
                                        <asp:TextBox ID="lblQ5" runat="server" Text='<%# Bind("q5") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <asp:Label ID="lblQ6Text" runat="server">
                                            <b>How could this item be improved?</b><br />
                                        </asp:Label>
                                        <asp:TextBox ID="lblQ6" runat="server" Text='<%# Bind("q6") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <asp:Label ID="lblQ7Text" runat="server">
                                            <b>Would you recomend this item to a friend?</b><br />
                                        </asp:Label>
                                        <asp:TextBox ID="lblQ7" runat="server" Text='<%# Bind("q7") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox><br /><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td height="100%">
                                        <asp:Label ID="lblQ8Text" runat="server">
                                            <b>Do you have any hot tips when using this item?</b><br />
                                        </asp:Label>
                                        <asp:TextBox ID="lblQ8" runat="server" Text='<%# Bind("q8") %>' TextMode="multiLine" Rows="8" Columns="76"></asp:TextBox><br /><br />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <center>
                    <asp:Button ID="btnUpdate" runat="server" Text="Save Changes" CommandName="update" />
                </center>
            </EditItemTemplate>
        </asp:FormView>
        
        <asp:Label ID="lblComplete" runat="Server"></asp:Label>
        
        <asp:SqlDataSource ID="sqlReviewDetails" runat="server" SelectCommand="procReviewByIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procReviewByIDUpdate" UpdateCommandType="StoredProcedure" OnUpdated="sqlReviewDetails_updated">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="id" Name="reviewID" Type="int32" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="reviewID" Type="int32" />
                <asp:Parameter Name="review" Type="string" Size="4000" />
                <asp:Parameter Name="q1" Type="string" Size="4000" />
                <asp:Parameter Name="q2" Type="string" Size="4000" />
                <asp:Parameter Name="q3" Type="string" Size="4000" />
                <asp:Parameter Name="q4" Type="string" Size="4000" />
                <asp:Parameter Name="q5" Type="string" Size="4000" />
                <asp:Parameter Name="q6" Type="string" Size="4000" />
                <asp:Parameter Name="q7" Type="string" Size="4000" />                
                <asp:Parameter Name="q8" Type="string" Size="4000" />  
            </UpdateParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
