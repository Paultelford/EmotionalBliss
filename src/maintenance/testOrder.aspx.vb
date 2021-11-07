Imports System.Net
Imports System.IO
Imports System.Text
Imports System.Xml
Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class maintenance_testOrder
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            'Clear errors
            lblError.Text = ""
            lblOutput.Text = ""
            lblResult.Text = ""
            'Get last orderid from db and display it (or show db is empty)
            If False Then
                Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                Dim oCmd As New SqlCommand("procOldOrdersLatestSelect", oConn)
                Dim da As New SqlDataAdapter
                Dim ds As New DataSet
                oCmd.CommandType = CommandType.StoredProcedure
                Try
                    If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                    da = New SqlDataAdapter(oCmd)
                    da.Fill(ds)
                    If ds.Tables(0).Rows.Count > 0 Then
                        lblPreamble.Text = "Order: " & ds.Tables(0).Rows(0)("orderID").ToString
                        lblPreambleDate.Text = FormatDateTime(ds.Tables(0).Rows(0)("orderDate"), DateFormat.LongDate) & " " & FormatDateTime(ds.Tables(0).Rows(0)("orderDate"), DateFormat.ShortTime)
                        date1.visible = False
                    Else
                        lblPreamble.Text = "No orders have been transferred from EB Live site yet."
                    End If
                Catch ex As Exception
                    lblError.Text = ex.Message & "<br>Page stopped in Page_Load()"
                    Response.End()
                Finally
                    ds.Dispose()
                    da.Dispose()
                    oCmd.Dispose()
                    oConn.Dispose()
                End Try
            End If
        End If
    End Sub
    Protected Sub date1_dateChanged(ByVal sender As Object, ByVal e As EventArgs)
    End Sub
    Protected Sub BtnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim startDate As String = FormatDateTime(date1.getStartDate, DateFormat.ShortDate)
        Dim endDate As String = FormatDateTime(date1.getEndDate, DateFormat.ShortDate)
        Dim buffer() As Byte = Encoding.ASCII.GetBytes("startdate=" & startDate & "&endDate=" & endDate)
        Dim r As New Random
        Dim myReq As HttpWebRequest = WebRequest.Create("http://www.emotionalbliss.com/a74jkefh64jkhg63f/generateXML.asp?rnd" & r.Next)
        myReq.Method = "POST"
        myReq.ContentType = "application/x-www-form-urlencoded"
        myReq.ContentLength = buffer.Length
        Dim PostData As Stream = myReq.GetRequestStream
        PostData.Write(buffer, 0, buffer.Length)
        PostData.Close()
        Dim WebResp As HttpWebResponse = myReq.GetResponse
        'Response.Write(WebResp.StatusCode)
        Dim answer As Stream = WebResp.GetResponseStream
        Dim _answer As New StreamReader(answer)
        If Trim(_answer.ReadToEnd) = "True" Then
            lblResult.Text = "Results have been recieved..."
            ParseResults()
        Else
            lblResult.Text = "No orders exist for specified date range."
        End If
    End Sub
    Protected Sub ParseResults()
        Dim reader As XmlTextReader = New XmlTextReader("http://www.emotionalbliss.com/xmlorders/ordersNew.xml")
        Dim lbl As Label = _content.FindControl("lblOutput")
        Dim sout As String = ""
        Dim orderID As Integer
        Dim count As Integer = 0
        Dim countNew As Integer = 0
        Dim errors As Integer = 0
        Dim warnings As Integer = 0
        Dim bAddToDB As Boolean
        Dim oc As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Do While reader.Read
            Select Case reader.NodeType
                Case XmlNodeType.Element
                    Select Case CType(reader.Name, String)
                        Case "orders"
                            'Do Nowt really
                        Case "order"
                            bAddToDB = True
                            count = count + 1
                            Try
                                'If order does not already exist on .NET site then add it
                                orderID = CType(reader("id").ToString, Integer)
                                If orderAlreadyExists(oc, orderID, errors) Then bAddToDB = False
                                If bAddToDB Then
                                    addOrder(oc, orderID, reader("country").ToString, CType(reader("prefix").ToString, Integer), reader("date").ToString, errors)
                                    countNew = countNew + 1
                                End If
                            Catch ex As Exception
                                lblError.Text = lblError.Text & "(" & reader("id").ToString & ")-ParseResults-" & ex.Message & "<br>"
                                errors = errors + 1
                            End Try

                        Case "item"
                            Try
                                'If order does not already exist on .NET site then add current items
                                If bAddToDB Then
                                    addItem(oc, orderID, CType(reader("pfid").ToString, Integer), CType(reader("qty").ToString, Integer), errors)
                                End If
                            Catch ex As Exception
                                lblError.Text = lblError.Text & "(" & reader("id").ToString & ",item#" & reader("pfid").ToString & ")-ParseResults-" & ex.Message & "<br>"
                                errors = errors + 1
                            End Try
                            'Do the removing of products/components 
                            '(Will have its own Try/Catch block in Sub)
                            If bAddToDB Then removeWarehouseProducts(oc, errors, orderID, CType(reader("pfid").ToString, Integer), CType(reader("qty").ToString, Integer), warnings)
                    End Select
            End Select
        Loop
        lblResult.Text = lblResult.Text & " (Parsed " & count & " orders, " & countNew & " new)"
        lblOutput.Text = lblOutput.Text & "<br>Finished.... " & errors & " Errors occured, " & warnings & " warnings<br>"
    End Sub
    Protected Sub addOrder(ByRef oConn As SqlConnection, ByVal id As Integer, ByVal country As String, ByVal prefix As Integer, ByVal orderDate As String, ByRef e As Integer)
        Dim oCmd As New SqlCommand("procOldOrdersInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@prefix", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@orderDate", SqlDbType.DateTime))
            .Parameters("@orderID").Value = id
            .Parameters("@country").Value = country
            .Parameters("@prefix").Value = prefix
            .Parameters("@orderDate").Value = orderDate
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = lblError.Text & "(" & id & ")-addOrder-" & ex.Message & "<br>"
            e = e + 1
        Finally
            oCmd.Dispose()
        End Try
    End Sub
    Protected Sub addItem(ByRef oConn As SqlConnection, ByVal id As Integer, ByVal productID As Integer, ByVal qty As Integer, ByRef e As Integer)
        Dim oCmd As New SqlCommand("procOldOrdersItemInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@productID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@qty", SqlDbType.Int))
            .Parameters("@orderID").Value = id
            .Parameters("@productID").Value = productID
            .Parameters("@qty").Value = qty
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblError.Text = lblError.Text & "(" & id & ",item#" & productID & ")-addItem-" & ex.Message & "<br>"
            e = e + 1
        Finally
            oCmd.Dispose()
        End Try
    End Sub
    Protected Function orderAlreadyExists(ByRef oConn As SqlConnection, ByVal id As Integer, ByRef e As Integer) As Boolean
        Dim result As Boolean = False
        Dim oCmd As New SqlCommand("procOldOrdersByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters("@orderID").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                result = True
            End If
        Catch ex As Exception
            e = e + 1
            lblError.Text = lblError.Text & "(" & id & ")-orderAlreadyExists-" & ex.Message & "<br>"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
        End Try
        Return (result)
    End Function
    Protected Sub removeWarehouseProducts(ByRef oConn As SqlConnection, ByRef e As Integer, ByVal orderID As Integer, ByVal pfid As Integer, ByVal qty As Integer, ByRef w As Integer)
        Dim oCmd As New SqlCommand("procWarehouseProductByPFIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim si As siteInclude
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@pfid", SqlDbType.Int))
            .Parameters("@pfid").Value = pfid
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count = 0 Then
                'Error, pfid has not been matched to any product.
                lblOutput.Text = lblOutput.Text & "orderID " & orderID & ", pfid " & pfid & " was not linked to any Warehouse Product. It must be done manually<br>"
                w = w + 1
            Else
                'Link found. Remove items from warehouse stock
                For Each rs As DataRow In ds.Tables(0).Rows
                    si = New siteInclude
                    si.addToWarehouseHistory(CType(rs("newID"), Integer), 0, qty, 4, "Order brought over from EB Live", Membership.GetUser.UserName, orderID, 0, 0, 0)
                Next
                si = Nothing
            End If
        Catch ex As Exception
            lblError.Text = lblError.Text & "(" & orderID & ", item " & pfid & ")-removeWarehouseProducts-" & ex.Message & "<br>"
            e = e + 1
        End Try
    End Sub
End Class
