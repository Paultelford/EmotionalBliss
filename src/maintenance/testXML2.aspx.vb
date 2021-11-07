Imports System.Net
Imports System.IO
Imports System.Text

Partial Class maintenance_testXML2
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        Dim buffer() As Byte = Encoding.ASCII.GetBytes("startdate=1%20may%202006&endDate=29%20may%202006")
        Dim myReq As HttpWebRequest = WebRequest.Create("http://81.149.144.46:8060/ebshop/ebmaintenance2/generateXML.asp")
        myReq.Method = "POST"
        myReq.ContentType = "application/x-www-form-urlencoded"
        myReq.ContentLength = buffer.Length
        Dim PostData As Stream = myReq.GetRequestStream
        PostData.Write(buffer, 0, buffer.Length)
        PostData.Close()
        Dim WebResp As HttpWebResponse = myReq.GetResponse
        Response.Write(WebResp.StatusCode)
        Dim answer As Stream = WebResp.GetResponseStream
        Dim _answer As New StreamReader(answer)
        Response.Write(_answer.ReadToEnd)


    End Sub
End Class
