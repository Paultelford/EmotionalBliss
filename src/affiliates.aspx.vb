Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class affiliates
    Inherits System.Web.UI.Page
    Dim sProductName As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim useDBTarget As Boolean = True
        Dim target As String = "~\homeIntro.aspx"
        Session("EBAffClickThroughID") = CStr(Request.QueryString("affid"))
        If Request.ServerVariables("QUERY_STRING") <> "" Then
            'Auto add to basket if '&b' is in the querystring
            If len(Request.ServerVariables("QUERY_STRING")) > 2 Then
                If lcase(right(Request.ServerVariables("QUERY_STRING"), 2)) = "&b" Then
                    autoAddToBasket()
                End If
            End If
        End If
        Dim bRedirectToShop As Boolean = False
        Dim sShop As String = "Shop"
        'Get session/target/country details from daatabase
        If useDBTarget Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procAffiliateByIDTargetSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            Dim sTarget As String = ""
            If Request.QueryString("target") <> "" Then sTarget = Request.QueryString("target")
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@affLink", SqlDbType.VarChar, 50))
                .Parameters("@affID").Value = CType(Session("EBAffClickThroughID"), Integer)
                .Parameters("@affLink").Value = sTarget
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    'Set affiliates default target page. (Usually ~/default.aspx?language=xx, unless they request something different)
                    target = ds.Tables(0).Rows(0)("fullURL")
                    'If link contains its own target, then use that instead
                    If Not IsDBNull(ds.Tables(0).Rows(0)("targetURL")) Then target = ds.Tables(0).Rows(0)("targetURL")
                    'Overwrite with passed value if it exists
                    If sTarget <> "" Then target = sTarget
                    'Now set the Country Session variables
                    Dim str As String = Replace(ds.Tables(0).Rows(0)("affLinkParam"), "?", "") ' Remove '?' from string
                    Dim arr As Array = Split(str, "&")
                    Dim tmp As Array
                    'Set shop currency
                    If Not IsDBNull(ds.Tables(0).Rows(0)("currencyCode")) Then Session("EBShopCurrency") = ds.Tables(0).Rows(0)("currencyCode")
                    'Set shopCountry/Langauge
                    For Each s As String In arr
                        If InStr(LCase(s), "language") Then
                            'Set Language Session Variable
                            tmp = Split(s, "=")
                            Session("EBLanguage") = tmp(1)
                            Session("EBShopCountry") = tmp(1)
                        End If
                        If InStr(LCase(s), "country") Then
                            'Set Country Session Variable
                            tmp = Split(s, "=")
                            Session("EBShopCountry") = tmp(1)
                        End If
                    Next
                    Profile.EBCountryName = ds.Tables(0).Rows(0)("countryname")
                    'If redirecting to the shop then check that the product actually exists before redirecting
                    If productExists(Request.QueryString("pid")) Then
                        bRedirectToShop = True
                    End If
                End If
            Catch ex As Exception
                'Log error in Errors DB. 
                Try
                    siteInclude.addError("\affiliates.aspx", "Error parsing Affiliate click through data, affid=" & Request.QueryString("affid") & ":" & ex.ToString)
                Catch c As Exception
                End Try
                target = "~\homeIntro.aspx"
            Finally
                ds.Dispose()
                da.Dispose()
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
        'siteInclude.debug(target)
        'Overwrite these values with ones from querystring (if supplied)
        If Request.QueryString("language") <> "" Then Session("EBLanguage") = Request.QueryString("language")
        logClickThrough()
        If LCase(Session("EBLanguage")) = "be" Or LCase(Session("EBLanguage")) = "nl" Then sShop = "Kopen"
        If bRedirectToShop Then Response.Redirect("~/shop/product.aspx?id=" & Request.QueryString("pid") & "&prod=" & sProductName & "&m=" & sShop)
        Dim r As New Random
        If InStr(target, "?") Then
            Response.Redirect(target & "&" & r.Next)
        Else
            Response.Redirect(target & "?" & r.Next)
        End If
        'Response.Write(target & "<br>EBShopCountry=" & Session("EBShopCountry") & "<br>EBLanguage=" & Session("EBLanguage"))
    End Sub
    Protected Sub logClickThrough()
        If IsNumeric(Request.QueryString("affid")) And CStr(Request.QueryString("affid")) <> "" Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procAffiliateLogInsert", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                .Parameters.Add(New SqlParameter("@ip", SqlDbType.VarChar, 20))
                .Parameters("@affID").Value = CInt(Request.QueryString("affid"))
                .Parameters("@ip").Value = Request.ServerVariables("REMOTE_ADDR")
            End With
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Dim si As New siteInclude
                si.addError("affilaites.aspx", "logClickThrough(affid=" & Request.QueryString("affid") & "); " & ex.ToString)
                si = Nothing
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        End If
    End Sub
    Protected Function productExists(ByVal pid As String) As Boolean
        Dim result As Boolean = False
        If IsNumeric(pid) Then
            Dim dt As New DataTable
            Try
                Dim param() As String = {"@id", "@countryCode"}
                Dim paramValue() As String = {pid, Session("EBShopCountry")}
                Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar}
                Dim paramSize() As Integer = {0, 5}
                dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procProductOnSaleByIDSelect")
                If dt.Rows.Count > 0 Then
                    sProductName = dt.Rows(0)("saleName")
                    result = True
                End If
            Catch ex As Exception
                siteInclude.addError("affiliates.aspx.vb", "productExists(pid=" & pid & ");" & ex.ToString)
            Finally
                dt.Dispose()
            End Try
        End If
        Return result
    End Function
    Protected Sub autoAddToBasket()
        'Customer has come from external site with a product link. Add and redirect to basket.

        Profile.EBCart.emptyBasket()
        Session("EBLanguage") = Request.QueryString("lang")
        Session("EBShopCountry") = Request.QueryString("shop")
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@id", "@countryCode"}
            Dim paramValue() As String = {Request.QueryString("pid"), Request.QueryString("shop")}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar}
            Dim paramSize() As Integer = {0, 5}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procProductOnSaleByIdSelect")
            If dt.Rows.Count > 0 Then
                Dim row As DataRow = dt.Rows(0)
                addDetails_ID.Value = row("id")
                addDetails_Name.Value = row("saleName")
                addDetails_Price.Value = row("unitPriceAfterDiscount")
                addDetails_Discount.Value = "0.00"
                addDetails_PriceIncDiscount.Value = row("unitPriceAfterDiscount")
                addDetails_Vat.Value = row("saleTaxRate")
                addDetails_Currency.Value = row("saleCurrencyCode")
                addDetails_DistBuyingID.Value = row("distBuyingID")
                Profile.EBCart.AddItem(addDetails_ID.Value, addDetails_Name.Value, CType(addDetails_Price.Value, Decimal), CType(addDetails_Discount.Value, Decimal), CType(addDetails_PriceIncDiscount.Value, Decimal), CType(addDetails_Vat.Value, Decimal), CType(addDetails_DistBuyingID.Value, Integer), 0, 0)
                Response.Redirect("\shop\basket.aspx")
            Else

            End If
        Catch ex As Exception
            If Not TypeOf (ex) Is System.Threading.ThreadAbortException Then siteInclude.addError("", "" & ex.ToString())
        Finally
            dt.Dispose()
        End Try

    End Sub
End Class
