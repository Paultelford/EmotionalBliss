Imports System.IO
Imports System.Data

Partial Class productxml
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Response.Write(Server.MapPath("uploads"))
    End Sub
    Protected Sub btnCreate_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim xml As New StringBuilder
        xml.AppendLine("<?xml version=""1.0"" encoding=""UTF-8"" ?>")
        xml.AppendLine("<feed xmlns=""http://www.w3.org/2005/Atom"" xmlns:g=""http://base.google.com/ns/1.0"">")
        xml.AppendLine("<title>emotionalbliss_googledata.xml</title>")
        xml.AppendLine("<link rel=""self"" href=""http://www.emotionalbliss.co.uk"" />")
        xml.AppendLine("<updated>" & Year(Now()) & "-" & Month(Now()) & "-" & Day(Now()) & "T" & FormatDateTime(Now(), DateFormat.LongTime) & "</updated>")
        xml.AppendLine("<author>")
        xml.AppendLine("<name>Emotional Bliss</name>")
        xml.AppendLine("</author>")
        xml.AppendLine("<id>tag:emotionalbliss.co.uk,2010-18-02</id>")
        'Do products
        Dim dt As New DataTable
        Try
            Dim param() As String = {}
            Dim paramValue() As String = {}
            Dim paramType() As SqlDbType = {}
            Dim paramSize() As Integer = {}
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procXmlProductsByActiveSelect")
            appendData(xml, dt)
        Catch ex As Exception
            Response.Write("Products::" & ex.ToString)
            siteInclude.addError("", "" & ex.ToString())
        Finally
            dt.Dispose()
        End Try
        'Finish up
        'End xml
        xml.AppendLine("</feed>")
        'Create file
        Dim fs As StreamWriter = New StreamWriter(Server.MapPath("uploads") & "\" & txtFilename.Text)
        fs.Write(xml)
        fs.Close()
        'Show link to file
        lnkXML.NavigateUrl = "http://" & Request.ServerVariables("HTTP_HOST") & "/uploads/" & txtFilename.Text
        lnkXML.Text = txtFilename.Text
    End Sub

    Protected Sub appendData(ByRef xml As StringBuilder, ByRef dt As DataTable)
        For Each row As DataRow In dt.Rows
            xml.AppendLine("<entry>")
            'Title (Product Name)
            xml.AppendLine("<title>" & htmlEncode(row("saleName")) & "</title>")
            'Brand (Category/Master Product)
            xml.AppendLine("<g:brand>EB</g:brand>")
            'Condition
            xml.AppendLine("<g:condition>new</g:condition>")
            'Description
            'text = Replace(row("description"), "<br>", Chr(10))
            'text = Replace(text, "<br/>", Chr(10))
            'text = Replace(text, "&nbsp;", " ")
            'text = Regex.Replace(text, "<(.|\n)*?>", String.Empty)
            'text = row("description")
            Select Case LCase(row("saleName"))
                Case "chandra"
                    xml.AppendLine("<summary>" & HttpUtility.HtmlEncode(htmlEncode("The Chandra is a cute and discreet personal massager which can be placed on the finger. With one speed and frequency, the Chandra can be used to directly stimulate the clitoris or the labia (vaginal lips).  The Chandra is slightly larger than the Isis, however with a powerful speed of 110 Hz it provides a more intense vibration whilst still only creating a quiet and slight buzzing sound.  The Chandra is included with three different sized finger bands so you do not worry about finding the right size to fit you  Please note your intimate massager must be charged for 12 hours before the first use.")) & "</summary>")
                Case "isis"
                    xml.AppendLine("<summary>" & HttpUtility.HtmlEncode(htmlEncode("The Isis is a delicate, smooth and discreet personal massager that looks and works like a finger, except the vibrations it creates are a big improvement on nature. The Isis is ideal to tease the clitoris or labia (vaginal lips) directly.  With one speed and frequency, the vibration setting is 80Hz. Despite the intense speed, the Isis creates no more than a faint buzzing sound.  The Isis includes three different sized finger bands so you do not worry about finding the right size to fit you. Please note your intimate massager must be charged for 12 hours before the first use.")) & "</summary>")
                Case "femblossom ""heat"""
                    xml.AppendLine("<summary>" & HttpUtility.HtmlEncode(htmlEncode("The Femblossom has a unique and curvaceous shape that is designed to simultaneously caress the clitoris and the labia (the ultra sensitive lips of the vagina) to achieve stronger and more intense orgasms.  The variation of speeds and frequencies available leaves you spoilt for choice. The new Femblossom Heat is the Femblossom with a surprise, it heats up! The single click function enables the Femblossom to gradually heat up to coincide with the speed and frequency of the programme selected creating an additional pleasurable (and warm) experience. Not only does it heat up but it also has a unique antibacterial agent which means a few minutes after use and after wiping with water, it becomes sterile and ready to use again.  Emotional Bliss Intimate Massagers are designed to stimulate orgasms  • Naturally - Our designs caress the natural contours of the body  • With more speeds and frequencies– There are nine different vibrating settings ranging from Tantalizing to Intense.  • Quietly – Our products are some of the quietest massagers on the market.  • With heat  - Emotional Bliss massagers are the ONLY products on the market that have heat technology that enables our product to heat up during usage helping create even  hotter orgasms. Please note your intimate massager must be charged for 12 hours before the first use.")) & "</summary>")
                Case "womolia ""heat"""
                    xml.AppendLine("<summary>" & HttpUtility.HtmlEncode(htmlEncode("The Womolia is a beautiful gentle curved shape with an angled tip, which is designed to directly stimulate the clitoris. Unobtrusive, it is the perfect product for new and inexperienced users as it allows stimulation on the outside rather than the inside. However for experienced users, the tip of the Womolia can also be inserted into the vagina for a more intense orgasm.  With a variety of speed and frequency settings available, there is something to suit you whatever your mood. The new Womolia Heat is the Womolia with a difference, it heats up! The single click function enables the Womolia to gradually heat up to coincide with the speed and frequency of the program selected creating an additional pleasurable (and warm) experience.  Not only does it heat up but it also has a unique antibacterial agent which means a few minutes after use and after wiping with water, it becomes sterile and ready to use again.  Emotional Bliss Intimate Massagers are designed to stimulate orgasms…  • Naturally - Our products are designed to caress the natural contours of the body.  • With more speeds and frequencies - There are nine different vibrating settings ranging from Tantalizing to Intense.  • Quietly - Our products are some of the quietest massagers on the market.  • With heat - Emotional Bliss massagers are the ONLY products on the market that have heat technology that enables our products to heat up during usage helping create even hotter orgasms. Please note your intimate massager must be charged for 12 hours before the first use.")) & "</summary>")
            End Select
            'Image Link
            If IsDBNull("saleImageName") Then
                'xml.AppendLine("<g:image_link></g:image_link>")
            Else
                xml.AppendLine("<g:image_link>" & "http://" & Request.ServerVariables("HTTP_HOST") & "/images/products/" & row("saleImageName") & "</g:image_link>")
            End If
            'Product Link
            xml.AppendLine("<link href=""" & "http://" & row("saleCountryCode") & ".emotionalbliss.com/shop/product.aspx?id=" & row("id") & "&amp;prod=" & Replace(row("saleName"), " ""Heat""", "") & "&amp;m=shop" & """ />")
            'Price
            xml.AppendLine("<g:price>" & row("saleUnitPrice") & "</g:price>")
            'Product Type
            xml.AppendLine("<g:product_type>Health &amp; Beauty &gt; Personal Care &gt; Massagers</g:product_type>")
            'ID
            If IsDBNull(row("saleProdCode")) Then
                xml.AppendLine("<id></id>")
            Else
                xml.AppendLine("<id>" & row("saleName") & "</id>")
            End If
            xml.AppendLine("</entry>")
        Next
    End Sub

    Protected Function htmlEncode(ByVal x As Object) As String
        Dim result As String = ""
        If Not IsDBNull(x) Then result = HttpUtility.HtmlEncode(x.ToString)
        Return result
    End Function

    Protected Function removeBatchID(ByVal s As String) As String
        Dim result As String = s
        If InStr(s, "_") Then
            Dim arr As Array = Split(s, "_")
            result = arr(1)
        End If
        Return result
    End Function
    Protected Function getCondition(ByVal c As Integer) As String
        Dim result As String = "new"
        If c = 1 Then result = "used"
        Return result
    End Function
    Protected Function calcNewPrice(ByVal price As Object, ByVal discount As Object) As String
        Dim iPrice As Decimal = price.ToString
        Dim iDiscount As Decimal = discount.ToString
        Dim iDiscountPrice As Decimal = iPrice * iDiscount
        Dim iResult As Decimal = iPrice - iDiscountPrice
        Return FormatNumber(iResult, 2)
    End Function
End Class
