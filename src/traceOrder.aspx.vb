Imports System.Data
Imports System.Data.SqlClient
Imports System.Threading
Imports System.Globalization

Partial Class traceOrder
    Inherits BasePage
    Private Const _orderID_MinChars As Integer = 7
    'Private bLanguageChanged As Boolean = False

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Request.QueryString("surname") <> "" And Request.QueryString("orderid") <> "" And Not Page.IsPostBack Then
            txtSurname.Text = Request.QueryString("surname")
            txtOrderID.Text = Request.QueryString("orderid")
        End If
        lblEntryError.Text = ""
        lblCriticalError.Text = ""
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate("entry")
        If Page.IsValid() Then
            'Test orderID for correct format
            Dim Surname As String = txtSurname.Text
            Dim userOrderID As String = txtOrderID.Text
            Dim bError As Boolean = False
            Dim ID As Integer
            'Step 1, orderID's length must be a minimum of x characters to be valid (Defined at to pof page)
            If Len(userOrderID) < _orderID_MinChars Then
                'Too short, show error.
                lblEntryError.Text = CType(GetLocalResourceObject("EntryError1"), String)
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
                        lblEntryError.Text = CType(GetLocalResourceObject("EntryError1"), String)
                        bError = True
                    Else
                        'Make sure the OrderIDNumber is a number
                        If Not IsNumeric(newOrderID) Then
                            'Invalid newOrderID
                            lblEntryError.Text = CType(GetLocalResourceObject("EntryError1"), String)
                            bError = True
                        End If
                    End If
                Catch ex As Exception
                    lblEntryError.Text = CType(GetLocalResourceObject("EntryError1"), String)
                    bError = True
                End Try
            End If
            If Not bError Then
                'Step 4, get the orderID
                ID = getOrderID(userOrderID)
                If ID = 0 Then
                    'Details could not be found, orderID must be invalid
                    lblEntryError.Text = CType(GetLocalResourceObject("EntryError2"), String)
                    bError = True
                End If
                If ID = 1 Then bError = True 'This suppesses the gvTrace being rendered as surname does not match the order details.
            End If
            If Not bError Then
                'All ok, bind orderlog to gvTrace
                panTrace.Visible = True
                showOrder(ID)
            End If
        End If
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
            lblCriticalError.Text = CType(GetLocalResourceObject("CriticalError1"), String)
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
        Dim oCmd As New SqlCommand("procShopOrderByUserOrderIDSelect", oConn)
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
                If InStr(LCase(ds.Tables(0).Rows(0)("billName")), LCase(txtSurname.Text)) <> 0 Then
                    result = ds.Tables(0).Rows(0)("id")
                Else
                    result = 1
                    lblEntryError.Text = CType(GetLocalResourceObject("EntryError3"), String)
                End If
            End If

        Catch ex As Exception
            lblCriticalError.Text = CType(GetLocalResourceObject("CriticalError1"), String)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Sub showOrder(ByVal id As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procOrderLogByOrderIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@ID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@customerVisible", SqlDbType.Bit))
            .Parameters("@ID").Value = id
            .Parameters("@customerVisible").Value = True
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            gvTrace.DataSource = ds
            gvTrace.DataBind()
        Catch ex As Exception
            Response.Write(ex)
            lblCriticalError.Text = CType(GetLocalResourceObject("CriticalError2"), String)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
End Class
