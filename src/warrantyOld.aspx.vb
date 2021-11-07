Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class warranty
    Inherits System.Web.UI.Page
    Private Const _orderID_MinChars As Integer = 7
    Private Const _gvItems_endDatePos As Integer = 8
    Private Const warrantyPeriod As Integer = 2

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        lblEntryError.Text = ""
        lblCriticalError.Text = ""
    End Sub
    Protected Sub btnEntrySubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("entry")
        If Page.IsValid() Then
            pan2.Visible = False
            pan3.Visible = False
            'Test orderID for correct format
            Dim Postcode As String = txtPostcode.Text
            Dim userOrderID As String = txtOrderID.Text
            Dim bError As Boolean = False
            Dim ID As Integer
            'Step 1, orderID's length must be a minimum of x characters to be valid (Defined at to pof page)
            If Len(userOrderID) < _orderID_MinChars Then
                'Too short, show error.
                lblEntryError.Text = "Your Order ID is too short. It should look similar to this 20/10000GB"
                bError = True
            End If
            'Setp 2, ignore prefix and '/' if supplied
            If InStr(userOrderID, "/") Then userOrderID = Right(userOrderID, Len(userOrderID) - InStr(userOrderID, "/"))
            If Not bError Then
                'Step 3, make sure last 2 digits are CHARs, and that they are a valid country code
                Try
                    Dim userCountryCode As String = Right(userOrderID, 2)
                    Dim newOrderID As String = Left(userOrderID, Len(userOrderID) - 2)
                    If Not isValidCountry(userCountryCode) Then
                        'Invalid CountryCode
                        lblEntryError.Text = "Your OrderID is invalid. It should look similar to this 20/10000GB"
                        bError = True
                    Else
                        'Make sure the OrderIDNumber is a number
                        If Not IsNumeric(newOrderID) Then
                            'Invalid newOrderID
                            lblEntryError.Text = "Your OrderID is invalid. It should look similar to this 20/10000GB"
                            bError = True
                        End If
                    End If
                Catch ex As Exception
                    lblEntryError.Text = "Your OrderID is invalid. It should look similar to this 20/10000GB"
                    bError = True
                End Try
            End If
            If Not bError Then
                'Step 4, get the orderID
                ID = getOrderID(userOrderID)
                If ID = 0 Then
                    'Details could not be found, orderID must be invalid
                    lblEntryError.Text = "The Order ID you entered could not be found. Please try again."
                    bError = True
                End If
                If ID = 1 Then bError = True 'This suppesses the gvTrace being rendered as surname does not match the order details.
            End If
            If Not bError Then
                'All ok, bind orderlog to gvTrace
                pan2.Visible = True
                showOrderItems(ID)
                hidOrderID.value = CStr(ID)
            End If
        End If
    End Sub
    Protected Sub btnSignup_click(ByVal sender As Object, ByVal e As EventArgs)
        For Each row As GridViewRow In gvItems.Rows
            insertProductWarranty(gvItems.DataKeys(row.RowIndex).Value, hidOrderID.Value, txtReceipt.Text)
        Next
        showOrderItems(hidOrderID.Value)
        pan3.Visible = False
    End Sub
    Protected Sub insertProductWarranty(ByVal orderItemID As Integer, ByVal orderID As Integer, ByVal receipt As String)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procWarrantyInsert", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@orderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@itemID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@receipt", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
            .Parameters("@orderID").Value = orderID
            .Parameters("@itemID").Value = orderItemID
            .Parameters("@receipt").Value = txtReceipt.Text
            .Parameters("@startDate").Value = Now()
            .Parameters("@endDate").Value = Day(Now()) & "/" & Month(Now()) & "/" & Year(Now()) + warrantyPeriod & " " & FormatDateTime(Now(), DateFormat.LongTime)
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            lblCriticalError.Text = "An error occured whilest registering your products. Please try again later."
            Dim si As New siteInclude
            si.addError("warranty.aspx", "btnSignup_click::" & ex.ToString)
            si = Nothing
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub gvItems_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Go through each row, if no end dates appear then show the submit form (pan3)
        Dim bWarrentyExists As Boolean = False
        Dim gv As GridView = CType(sender, GridView)
        For Each row As GridViewRow In gv.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If IsDate(row.Cells(_gvItems_endDatePos).Text) Then bWarrentyExists = True
            End If
        Next
        If Not bWarrentyExists Then pan3.Visible = True
    End Sub
    Protected Function isValidCountry(ByVal cc As String) As Boolean
        Dim result As Boolean = False
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procCountryByCountryCodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@countryCode").Value = cc
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then result = True
        Catch ex As Exception
            lblCriticalError.Text = "We are currently unable to register warrantys, sorry for the inconvenience. <br>Please try later."
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function getOrderID(ByVal uid As String) As Integer
        Dim result As Integer = 0
        Dim newOrderID As Integer = CInt(Left(uid, Len(uid) - 2))
        Dim CountryCode As String = Right(uid, 2)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderByUserOrderIDPostcodeSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@newOrderID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@newOrderID").Value = newOrderID
            .Parameters("@countryCode").Value = CountryCode
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                If postcodeMatch(txtPostcode.text, ds.Tables(0).Rows(0)("billPostcode"), ds.Tables(0).Rows(0)("shipPostcode")) Then
                    result = ds.Tables(0).Rows(0)("id")
                    lblOrderID.Text = ds.Tables(0).Rows(0)("userOrderID")
                Else
                    result = 1
                    lblEntryError.Text = "The Postcode does not match up with the order. Please try again."
                End If
            End If

        Catch ex As Exception
            lblCriticalError.Text = "We are currently unable to register warrantys, sorry for the inconvenience. <br>Please try later."
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Sub showOrderItems(ByVal id As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procWarrantyByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters("@ID").Value = id
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            gvItems.DataSource = ds
            gvItems.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            lblCriticalError.Text = "There was an error returning your order details. Please try again later."
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function postcodeMatch(ByVal target As String, ByVal pc1 As String, ByVal pc2 As String) As Boolean
        Dim result As Boolean = False
        If trimLower(target) = trimLower(pc1) Or trimLower(target) = trimLower(pc2) Then result = True
        Return result
    End Function
    Protected Function trimLower(ByVal pc As String) As String
        'Takes a postcode and removes the spaces in it, and converts to lowercase
        'Used to compare user enterd postcodes
        Dim result As String = pc
        If InStr(pc, " ") Then
            Dim arr = Split(pc, " ")
            result = Trim(arr(0)) & Trim(arr(1))
        End If
        Return LCase(result)
    End Function
End Class
