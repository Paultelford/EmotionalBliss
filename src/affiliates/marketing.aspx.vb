Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_marketing
    Inherits BasePage

    'System
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        lblError.Text = ""
    End Sub

    'User
    Protected Sub btnSearch_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procMarketingSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        'Define couter variables
        Dim iTotalOrders As Integer = 0
        Dim iOrdersInAgeRange As Integer = 0
        Dim iFemalesInAgeRange As Integer = 0
        Dim iMalesInAgeRange As Integer = 0
        Dim iUnknownsInAgeRange As Integer = 0
        Dim iAgeSpecified As Integer = 0
        Dim iAgeUnspecified As Integer = 0
        Dim iLoAge As Integer = 0
        Dim iHiAge As Integer = 100
        Dim d As New siteInclude
        Dim ht As New Hashtable
        Dim otherPC As Integer = 0
        Dim area As String = ""
        Dim bAdd As Boolean = True
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@type", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@postcode", SqlDbType.VarChar, 2))
            .Parameters.Add(New SqlParameter("@ageLo", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@ageHi", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@prodID", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
            .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
            .Parameters("@type").Value = drpType.SelectedValue
            .Parameters("@postcode").Value = "%"
            .Parameters("@ageLo").Value = 0
            .Parameters("@ageHi").Value = 100
            .Parameters("@prodID").Value = drpProduct.SelectedValue
            .Parameters("@startDate").Value = date1.getStartDate
            .Parameters("@endDate").Value = date1.getEndDate
            .Parameters("@countryCode").Value = Session("EBAffEBDistributorCountryCode")
        End With
        Try
            If txtLoAge.Text <> "" Then
                iLoAge = CType(txtLoAge.Text, Integer)
                oCmd.Parameters("@ageLo").Value = iLoAge
            End If
            If txtHiAge.Text <> "" Then
                iHiAge = CType(txtHiAge.Text, Integer)
                oCmd.Parameters("@ageHi").Value = iHiAge
            End If
            If txtPostcode.Text <> "" Then oCmd.Parameters("@postcode").Value = txtPostcode.Text
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                For Each rs As DataRow In ds.Tables(0).Rows
                    bAdd = True
                    If Not (Len(txtPostcode.Text) = 1 And isPostcode2Digit(rs("billPostcode"))) Then
                        iTotalOrders = iTotalOrders + 1
                        If Not IsDBNull(rs("dob")) Then
                            If rs("dob") = "" Then
                                iAgeUnspecified = iAgeUnspecified + 1
                            Else
                                iAgeSpecified = iAgeSpecified + 1
                            End If
                        Else
                            iAgeUnspecified = iAgeUnspecified + 1
                        End If
                        If IsDBNull(rs("postcode_area")) And IsDBNull(rs("postcode_area1")) Then
                            otherPC = otherPC + 1
                        Else
                            If Not IsDBNull(rs("postcode_area1")) Then 'postcode_area1 has text
                                If isPostcode2Digit(rs("billPostcode")) Then 'billPostcode starts with 2 characters
                                    If Not IsDBNull(rs("postcode_area")) Then
                                        area = rs("postcode_area")
                                    Else
                                        otherPC = otherPC + 1
                                        bAdd = False
                                    End If
                                Else
                                    area = rs("postcode_area1")
                                End If
                            Else
                                area = rs("postcode_area")
                            End If
                            If bAdd Then
                                If ht.ContainsKey(area) Then
                                    'Update
                                    ht(area) = CInt(ht(area)) + 1
                                Else
                                    'Create
                                    ht.Add(area, "1")
                                End If
                            End If
                            End If
                            If isAgeRangeMatch(iLoAge, iHiAge, rs("dob"), rs("orderDate")) Then
                                iOrdersInAgeRange = iOrdersInAgeRange + 1
                                If Not IsDBNull(rs("gender")) Then
                                    Select Case LCase(rs("gender"))
                                        Case "male"
                                            iMalesInAgeRange = iMalesInAgeRange + 1
                                        Case "female"
                                            iFemalesInAgeRange = iFemalesInAgeRange + 1
                                        Case Else
                                            iUnknownsInAgeRange = iUnknownsInAgeRange + 1
                                    End Select
                                End If
                            End If
                    End If
                Next
                'gvRegion.DataSource = ht
                'gvRegion.DataBind()
                Dim tCell As TableCell
                Dim tRow As TableRow
                For Each x As String In ht.Keys
                    tCell = New TableCell
                    tCell.Width = 200
                    tRow = New TableRow
                    tCell.Text = x
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    tCell.Text = ht(x)
                    tRow.Cells.Add(tCell)
                    tblRegion.Rows.Add(tRow)
                Next
                'Add unknowns
                If otherPC > 0 Then
                    tCell = New TableCell
                    tRow = New TableRow
                    tCell.Text = "Unknown region"
                    tRow.Cells.Add(tCell)
                    tCell = New TableCell
                    tCell.Text = CStr(otherPC)
                    tRow.Cells.Add(tCell)
                    tblRegion.Rows.Add(tRow)
                End If
                ht.Clear()
                ht = Nothing
            End If
            'Show results onscreen
            lblOrdersDelivered.Text = iTotalOrders
            lblProduct.Text = drpProduct.SelectedItem.Text
            'If drpProduct.SelectedIndex = 0 Then lblProduct.Text = ""
            lblPostcode.Text = UCase(txtPostcode.Text)
            lblAgeSpecified.Text = iAgeSpecified
            lblGaveAge.Text = iAgeSpecified
            lblNoAgeSpecified.Text = iAgeUnspecified
            lblOrderWithinAgeRange.Text = iOrdersInAgeRange
            lblAgePercent.Text = FormatNumber((100 / iAgeSpecified) * iOrdersInAgeRange, 1) & "%"
            lblAgeRequest.Text = "From " & iLoAge & " to " & iHiAge
            lblOrderWithinAgeRange.Text = iOrdersInAgeRange
            lblFemalesInRange.Text = iFemalesInAgeRange
            lblMalesInRange.Text = iMalesInAgeRange
            lblUnknownInRange.Text = iUnknownsInAgeRange
            lblCountryName.Text = getCountryName(Session("EBAFFEBDistributorCountryCode")) & " From " & FormatDateTime(date1.getStartDate, DateFormat.LongDate) & " to " & FormatDateTime(date1.getEndDate, DateFormat.LongDate)
            lblDateCreated.Text = FormatDateTime(Now(), DateFormat.LongDate) & " " & FormatDateTime(Now(), DateFormat.ShortTime)
            pan1.Visible = False
            pan2.Visible = True
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("affiliates/marketing.aspx.vb", "btn_Search_click(); " & ex.ToString)
            si = Nothing
            lblError.Text = "<font color='red'>" & ex.ToString & "</font>"
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnBack_click(ByVal sender As Object, ByVal e As EventArgs)
        pan1.Visible = True
        pan2.Visible = False
    End Sub
    Protected Sub btnPostcode_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim btn As ImageButton = CType(sender, ImageButton)
        Dim js As String = ""
        js = "function receivePostcode(p){" & Chr(10)
        js = js & "getElement('" & txtPostcode.ClientID & "').value=p;" & Chr(10)
        js = js & "}"
        ScriptManager.RegisterStartupScript(btn, Me.GetType, "onloader", "self.setTimeout(""openPopup()"",200);" & Chr(10) & js, True)
    End Sub

    'Functions
    Protected Function isAgeRangeMatch(ByVal lo As Integer, ByVal hi As Integer, ByVal dob As Object, ByVal od As Object) As Boolean
        Dim result As Boolean = False
        Try
            Dim orderDate As Date = CType(od.ToString, Date)
            If Not IsDBNull(dob) Then
                If dob.ToString <> "" Then
                    'Passes all tests, now see if customer was within date range and order was placed
                    Dim dateOfBirth As Date = CType(dob.ToString, Date)
                    Dim customerAgeWhenOrderPlaced As Integer = orderDate.Year - dateOfBirth.Year
                    'Take 1 year off if customer has not had their birthday yet this year
                    If orderDate < CType(Replace(dateOfBirth.Day & "/" & dateOfBirth.Month, "29/2", "28/2") & "/" & orderDate.Year, Date) Then customerAgeWhenOrderPlaced = customerAgeWhenOrderPlaced - 1
                    'If it age is within range, return true
                    If lo <= customerAgeWhenOrderPlaced And hi >= customerAgeWhenOrderPlaced Then result = True
                End If
            End If
        Catch ex As Exception
            lblError.Text = ex.ToString
        End Try
        Return result
    End Function
    Protected Function getCountryName(ByVal cc As String) As String
        Dim result As String = ""
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
            If ds.Tables(0).Rows.Count > 0 Then result = ds.Tables(0).Rows(0)("countryname")
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Return result
    End Function
    Protected Function isPostcode2Digit(ByVal o As Object) As Boolean
        Dim result As Boolean = True
        If Not IsDBNull(o) Then
            Dim s As String = o.ToString
            If Len(s) > 1 Then
                If IsNumeric(Mid(s, 2, 1)) Then result = False
            End If
        End If
        Return result
    End Function
End Class
