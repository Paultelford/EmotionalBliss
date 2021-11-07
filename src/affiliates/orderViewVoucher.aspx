<%@ Page Language="VB" AutoEventWireup="false" CodeFile="orderViewVoucher.aspx.vb" Inherits="affiliates_orderViewVoucher" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:DetailsView ID="dvVoucher" runat="server" DataSourceID="sqlVoucher" AutoGenerateRows="false">
            <Fields>
                <asp:BoundField HeaderText="Voucher Number:" DataField="number" />
                <asp:BoundField HeaderText="Created:" DataField="purchaseDate" DataFormatString="{0:dd MMM yyyy}" />
                <asp:TemplateField HeaderText="Type:">
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherType" runat="server" Text='<%# showType(Eval("coupon")) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Fields>   
        </asp:DetailsView>
        
        
        <asp:SqlDataSource ID="sqlVoucher" runat="server" SelectCommand="procVoucherByNumberDetailsSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
            <SelectParameters>
                <asp:QueryStringParameter QueryStringField="v" Name="number" Type="string" Size="8" />
                <asp:SessionParameter SessionField="EBAffEBDistributorCountryCode" Name="countryCode" Type="string" Size="5" />
            </SelectParameters>
        </asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
