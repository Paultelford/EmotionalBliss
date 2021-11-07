Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class warranty
    Inherits BasePage
    Protected monthNames() As String = {"Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
    Dim regID As Integer = 0

    'System
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Set dates on 1st run
            For iLoop As Integer = 1 To 31
                drpDay.Items.Add(New ListItem(iLoop, iLoop))
                drpDOBDay.Items.Add(New ListItem(iLoop, iLoop))
            Next
            drpDay.SelectedValue = Day(Now())
            drpDOBDay.SelectedValue = 1
            For iLoop As Integer = 1 To 12
                drpMonth.Items.Add(New ListItem(monthNames(iLoop - 1), iLoop))
                drpDOBMonth.Items.Add(New ListItem(monthNames(iLoop - 1), iLoop))
            Next
            drpMonth.SelectedValue = Month(Now())
            drpDOBMonth.SelectedValue = 0
            For iLoop As Integer = 2004 To Year(Now())
                drpYear.Items.Add(New ListItem(iLoop, iLoop))
            Next
            drpYear.SelectedValue = Year(Now())
            For iLoop As Integer = 1920 To Year(Now()) - 10
                drpDOBYear.Items.Add(New ListItem(iLoop, iLoop))
            Next
            drpDOBYear.SelectedValue = 1970
            loadDBResources()
        End If
        lblAgreeError.Text = ""
    End Sub

    'Page
    Protected Sub drpShopCountry_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        drpShopCountry.SelectedValue = "gb"
    End Sub
    Protected Sub drpCountry_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        drpCountry.SelectedValue = "gb"
    End Sub

    'User
    Protected Sub btnRegister_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Dim bError As Boolean = False
        Page.Validate()
        If chkAgree.Checked Then
            If Page.IsValid Then
                'Test dates
                If Not IsDate(drpDay.SelectedValue & "/" & drpMonth.SelectedValue & "/" & drpYear.SelectedValue) Then
                    lblPurchaseDateError.Text = "<br>" & getDBResourceString("errInvalid", "global")
                    bError = True
                Else
                    lblPurchaseDateError.Text = String.Empty
                End If

                If Not IsDate(drpDOBDay.SelectedValue & "/" & drpDOBMonth.SelectedValue & "/" & drpDOBYear.SelectedValue) Then
                    lblDOBDateError.Text = "<br>" & getDBResourceString("errInvalid", "global")
                    bError = True
                Else
                    lblDOBDateError.Text = String.Empty
                End If
                If Not bError Then
                    'Commit details to database
                    Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
                    Dim oCmd As New SqlCommand("procWarrantyInsert", oConn)
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@shopName", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@shopLocation", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@shopCountry", SqlDbType.VarChar, 5))
                        .Parameters.Add(New SqlParameter("@product", SqlDbType.VarChar, 20))
                        .Parameters.Add(New SqlParameter("@purchaseDate", SqlDbType.DateTime))
                        .Parameters.Add(New SqlParameter("@purchasePrice", SqlDbType.VarChar, 20))
                        .Parameters.Add(New SqlParameter("@firstname", SqlDbType.VarChar, 30))
                        .Parameters.Add(New SqlParameter("@surname", SqlDbType.VarChar, 30))
                        .Parameters.Add(New SqlParameter("@gender", SqlDbType.VarChar, 10))
                        .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@birthDate", SqlDbType.DateTime))
                        .Parameters.Add(New SqlParameter("@address", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@town", SqlDbType.VarChar, 50))
                        .Parameters.Add(New SqlParameter("@postcode", SqlDbType.VarChar, 10))
                        .Parameters.Add(New SqlParameter("@country", SqlDbType.VarChar, 5))
                        .Parameters.Add(New SqlParameter("@phone", SqlDbType.VarChar, 30))
                        .Parameters.Add(New SqlParameter("@recievePromo", SqlDbType.Bit))
                        .Parameters.Add(New SqlParameter("@startDate", SqlDbType.DateTime))
                        .Parameters.Add(New SqlParameter("@endDate", SqlDbType.DateTime))
                        .Parameters.Add(New SqlParameter("@pk", SqlDbType.Int))
                        .Parameters("@shopName").Value = txtShopName.Text
                        .Parameters("@shopLocation").Value = txtShopLoc.Text
                        .Parameters("@shopCountry").Value = drpShopCountry.SelectedValue
                        .Parameters("@product").Value = drpProduct.SelectedValue
                        .Parameters("@purchaseDate").Value = FormatDateTime(drpDay.SelectedValue & "/" & drpMonth.SelectedValue & "/" & drpYear.SelectedValue, DateFormat.LongDate)
                        .Parameters("@purchasePrice").Value = txtPrice.Text
                        .Parameters("@firstname").Value = txtFirstname.Text
                        .Parameters("@surname").Value = txtSurname.Text
                        .Parameters("@gender").Value = drpGender.SelectedValue
                        .Parameters("@email").Value = txtEmail.Text
                        .Parameters("@birthDate").Value = FormatDateTime(drpDOBDay.SelectedValue & "/" & drpDOBMonth.SelectedValue & "/" & drpDOBYear.SelectedValue, DateFormat.LongDate)
                        .Parameters("@address").Value = txtAddress.Text
                        .Parameters("@town").Value = txtTown.Text
                        .Parameters("@postcode").Value = txtPostcode.Text
                        .Parameters("@country").Value = drpCountry.SelectedValue
                        .Parameters("@phone").Value = txtPhone.Text
                        .Parameters("@recievePromo").Value = Not chkPromo.Checked
                        .Parameters("@startDate").Value = Now()
                        .Parameters("@endDate").Value = FormatDateTime(Day(Now()) & "/" & Month(Now()) & "/" & Year(Now()) + 2, DateFormat.LongDate)
                        .Parameters("@pk").Direction = ParameterDirection.Output
                    End With
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                        regID = oCmd.Parameters("@pk").Value
                    Catch ex As Exception
                        lblCriticalError.Text = "We are unable to accept Warranty Registrans at the moment. Sorry for any inconveinience.<br>Please try again later"
                        lblCriticalError.Text = getDBResourceString("CriticalError")
                        bError = True
                        siteInclude.addError("warranty.aspx", "btnRegister_click::" & ex.ToString)
                    Finally
                        oCmd.Dispose()
                        oConn.Dispose()
                    End Try
                    If Not bError Then
                        pan1.Visible = False
                        lblComplete.Text = getDBResourceString("CompleteMessage")
                        'send email
                        Try
                            siteInclude.sendEmail(txtEmail.Text, "", "", "EB Warranty", "noreply@emotionalbliss.com", "Emotional Bliss", getEmailBody())
                        Catch ex As Exception
                            siteInclude.debug(ex.ToString())
                        End Try

                    End If
                End If
            End If
        Else
            lblAgreeError.Text = "* " & getDBResourceString("errRequired", "global")
        End If
    End Sub

    'Subs
    Protected Sub loadDBResources()
        lblIntroduction.Text = getDBResourceString("Introduction")
        lblShopName.Text = getDBResourceString("ShopName")
        lblShopLocation.Text = getDBResourceString("ShopLocation")
        lblShopCountry.Text = getDBResourceString("ShopCountry")
        lblProduct.Text = getDBResourceString("Product")
        lblPurchaseDate.Text = getDBResourceString("PurchaseDate")
        lblPurchasePrice.Text = getDBResourceString("PurchasePrice")
        lblFirstname.Text = getDBResourceString("Firstname")
        lblSurname.Text = getDBResourceString("Surname")
        lblGender.Text = getDBResourceString("Gender")
        lblEmail.Text = getDBResourceString("EmailAddress")
        lblDOB.Text = getDBResourceString("DateOfBirth")
        lblAddress.Text = getDBResourceString("Address")
        lblCity.Text = getDBResourceString("City")
        lblPostcode.Text = getDBResourceString("Postcode")
        lblCountry.Text = getDBResourceString("Country")
        lblPhone.Text = getDBResourceString("Telephone")
        lblPromo.Text = getDBResourceString("ReceivePromotions")
        btnRegister.ImageUrl = siteInclude.trimCrap(getDBResourceString("RegisterProduct"))
        lblCurrencySign.Text = getCurrencyCodeByCountryCode(Session("EBLanguage"))
        lblYesIAgree.Text = getDBResourceString("YesIAgree")
        lblAgreementNote.Text = getDBResourceString("AgreementNote")
        'Validators
        reqPan1ShopName.ErrorMessage = getDBResourceString("errRequired", "global")
        reqPan1ShopLoc.ErrorMessage = getDBResourceString("errRequired", "global")
        reqTxtPrice.ErrorMessage = getDBResourceString("errRequired", "global")
        pan1ReqFirstname.ErrorMessage = getDBResourceString("errRequired", "global")
        pan1ReqSurname.ErrorMessage = getDBResourceString("errRequired", "global")
        pan1Req1Email.ErrorMessage = getDBResourceString("errRequired", "global")
        regEmail.ErrorMessage = getDBResourceString("errInvalid", "global")
        pan1ReqAddress.ErrorMessage = getDBResourceString("errRequired", "global")
        pan1ReqTown.ErrorMessage = getDBResourceString("errRequired", "global")
        panReqPostcode.ErrorMessage = getDBResourceString("errRequired", "global")
        pan1Req1Email.ErrorMessage = getDBResourceString("errRequired", "global")
        pan1Req1Email.ErrorMessage = getDBResourceString("errRequired", "global")
        pan1Req1Email.ErrorMessage = getDBResourceString("errRequired", "global")
        pan1Req1Email.ErrorMessage = getDBResourceString("errRequired", "global")
    End Sub

    'Funtions
    Protected Function getEmailBody() As String
        Dim body As String = getDBResourceString("EmailReply") & "<br><br>"
        body = body & trimCrap(getDBResourceString("ShopNameConfirmationEmail")) & ": " & txtShopName.Text & "<br>"
        body = body & trimCrap(getDBResourceString("ShopLocationConfirmationEmail")) & ": " & txtShopLoc.Text & "<br>"
        body = body & trimCrap(getDBResourceString("ShopCountryConfirmationEmail")) & ": " & drpShopCountry.SelectedItem.Text & "<br>"
        body = body & trimCrap(getDBResourceString("ProductConfirmationEmail")) & ": " & drpProduct.SelectedItem.Text & "<br>"
        body = body & trimCrap(getDBResourceString("PurchaseDateConfirmationEmail")) & ": " & FormatDateTime(drpDay.SelectedValue & "/" & drpMonth.SelectedValue & "/" & drpYear.SelectedValue, DateFormat.LongDate) & "<br>"
        body = body & trimCrap(getDBResourceString("PurchasePriceConfirmationEmail")) & ": " & txtPrice.Text & "<br>"
        body = body & trimCrap(getDBResourceString("RegistrationDate")) & ": " & FormatDateTime(Now(), DateFormat.LongDate) & "<br>"
        body = body & trimCrap(getDBResourceString("RegistrationID")) & ": " & regID & "<br>"
        body = body & trimCrap(getDBResourceString("CustomerInfo")) & ": " & "<br>"
        'body = body & trimCrap(getDBResourceString("Firstname")) & ": " & txtFirstname.Text & "<br>"
        'body = body & trimCrap(getDBResourceString("Surname")) & ": " & txtSurname.Text & "<br>"
        'body = body & trimCrap(getDBResourceString("Gender")) & ": " & drpGender.SelectedItem.Text & "<br>"
        'body = body & trimCrap(getDBResourceString("EmailAddress")) & ": " & txtEmail.Text & "<br>"
        'body = body & trimCrap(getDBResourceString("DateOfBirth")) & ": " & FormatDateTime(drpDOBDay.SelectedValue & "/" & drpDOBMonth.SelectedValue & "/" & drpDOBYear.SelectedValue, DateFormat.LongDate) & "<br>"
        'body = body & trimCrap(getDBResourceString("Address")) & ": " & txtAddress.Text & "<br>"
        'body = body & trimCrap(getDBResourceString("City")) & ": " & txtTown.Text & "<br>"
        'body = body & trimCrap(getDBResourceString("Postcode")) & ": " & txtPostcode.Text & "<br>"
        'body = body & trimCrap(getDBResourceString("Country")) & ": " & drpCountry.SelectedItem.Text & "<br>"
        'body = body & trimCrap(getDBResourceString("Telephone")) & ": " & txtPhone.Text & "<br>"
        If txtFirstname.Text <> "" Then body = body & txtFirstname.Text & "<br>"
        If txtSurname.Text <> "" Then body = body & txtSurname.Text & "<br>"
        body = body & siteInclude.trimCrap(getDBResourceString("Gender")) & ": " & drpGender.SelectedItem.Text & "<br>"
        If txtEmail.Text <> "" Then body = body & siteInclude.trimCrap(getDBResourceString("EmailAddress")) & ": " & txtEmail.Text & "<br>"
        body = body & siteInclude.trimCrap(getDBResourceString("DateOfBirth")) & ": " & FormatDateTime(drpDOBDay.SelectedValue & "/" & drpDOBMonth.SelectedValue & "/" & drpDOBYear.SelectedValue, DateFormat.LongDate) & "<br>"
        If txtAddress.Text <> "" Then body = body & siteInclude.trimCrap(getDBResourceString("Address")) & ": " & txtAddress.Text & "<br>"
        If txtTown.Text <> "" Then body = body & siteInclude.trimCrap(getDBResourceString("City")) & ": " & txtTown.Text & "<br>"
        If txtPostcode.Text <> "" Then body = body & siteInclude.trimCrap(getDBResourceString("Postcode")) & ": " & txtPostcode.Text & "<br>"
        body = body & siteInclude.trimCrap(getDBResourceString("Country")) & ": " & drpCountry.SelectedItem.Text & "<br>"
        If txtPhone.Text <> "" Then body = body & siteInclude.trimCrap(getDBResourceString("Telephone")) & ": " & txtPhone.Text & "<br>"


        Return body
    End Function
End Class
