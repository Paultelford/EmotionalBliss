<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="testXML.aspx.vb" Inherits="maintenance_testXML" title="Untitled Page" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:DataList ID="dl1" runat="server" DataSourceID="XmlDS">
        <HeaderTemplate>
            <table border="0">
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td>
                    Order No:
                </td>
                <td>
                    <%#XPath("orderid")%>
                </td>
            </tr>
            <tr>
                <td>
                    
                </td>
                <td>
                    <asp:DataList ID="dl1" runat="server" DataSource='<%# XPathSelect("item") %>'>
                        <ItemTemplate>
                    
                            ID <%#XPath("@pfid")%><br />
                            Qty <%#XPath("@qty")%><br />
                            
                            </tr>
                        </ItemTemplate>
                    </asp:DataList>
                </td>
            </tr>
            <tr>
                <td colspan="2">&nbsp;</td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table>
        </FooterTemplate>
    </asp:DataList>
    
    
    <asp:XmlDataSource ID="XmlDS" runat="server" DataFile="http://81.149.144.46:8060/upload/ordersNew.xml" XPath="orders/order">
    </asp:XmlDataSource>
</asp:Content>

