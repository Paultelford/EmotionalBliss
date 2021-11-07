Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_royalty
    Inherits System.Web.UI.Page

    'System Events
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Membership.Provider.ApplicationName = "EBProvider"
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
        Dim log As loginview = Master.FindControl("logMaintenance")
        Dim cph As ContentPlaceHolder = log.findcontrol("ContentPlaceHolder1")
        Dim lbl As label = cph.FindControl("lblError")
        lbl.Text = ""
        lblComplete.Text = ""
    End Sub

    'Page Events
    Protected Sub dvAdd_itemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        'Add to membership profile
        Membership.Provider.ApplicationName = "EBAffProvider"
        Dim txtUsername As TextBox = dvAdd.FindControl("txtUsername")
        Dim txtPassword As TextBox = dvAdd.FindControl("txtPassword")
        Membership.CreateUser(txtUsername.Text, txtPassword.Text)
        Membership.Provider.ApplicationName = "EBProvider"
        'New user added to db, show success message and clearup
        lblError.Text = "User sucessfully added."
        dvAdd.Visible = False
        dvAdd.Controls.Clear()
        gvEarners.DataBind()
        lblAddInstructions.Visible = False
        lnkAdd.Visible = True
    End Sub
    Protected Sub dvAdd_itemInserting(ByVal sender As Object, ByVal e As DetailsViewInsertEventArgs)
        'See if Username already exists
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByUsernameSelect", oConn)
        Dim da As New SqlDataAdapter()
        Dim ds As New DataSet
        Dim bError As Boolean = False
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affUsername", SqlDbType.VarChar, 20))
            .Parameters("@affUsername").Value = e.Values("affUsername")
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            If ds.Tables(0).Rows.Count > 0 Then
                'Username already exists
                bError = True
                lblError.Text = "The username '<b>" & e.Values("affUsername") & "'</b> already exists, please choose another and try again"
            End If
        Catch ex As Exception
            bError = True
            lblError.Text = ex.Message
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        e.cancel = bError
    End Sub
    Protected Sub drpCountry_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As dropdownlist = CType(sender, DropDownList)
        drp.items.clear()
        drp.Items.Add(New ListItem("Select country....", ""))
    End Sub
    Protected Sub gvAmounts_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim bRetailFound As Boolean = False
        Dim lblRetail As label
        For Each row As gridviewrow In gvAmounts.Rows
            If row.rowType = DataControlRowType.DataRow Then
                lblRetail = row.FindControl("lblRetail")
                If lCase(lblRetail.Text) = "true" Then bRetailFound = True
            End If
        Next
        radRetail.Checked = bRetailFound
        radDistributor.Checked = Not bRetailFound
    End Sub

    'User Events
    Protected Sub lnkAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        dvAdd.Visible = True
        dvAdd.Controls.Clear()
        dvAdd.DataBind()
        lblAddInstructions.Visible = True
        lnkAdd.Visible = False
        panAmounts.Visible = False
    End Sub
    Protected Sub btnAddCancel_click(ByVal sender As Object, ByVal e As EventArgs)
        dvAdd.Visible = False
        lblAddInstructions.Visible = False
        lnkAdd.Visible = True
        dvAdd.Controls.Clear()
    End Sub
    Protected Sub gvEarners_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        panAmounts.Visible = True
        dvAdd.Controls.Clear()
        dvAdd.Visible = False
        lnkAdd.Visible = True
        lblAddInstructions.Visible = False
        If isSelectedEarnerSuperRoyalty() Then
            'Earner gets royalties from multiple countrys, so show country dropdown
            drpCountry.Visible = True
            radRetail.Visible = True
            radDistributor.Visible = True
            Try
                drpCountry.SelectedIndex = 0
            Catch ex As Exception
                'Error will occur if drpCountry hasnt been databound yet, no need to handle error.
            End Try
        Else
            'Earner only recieves royalties from their own country (Setup by Distributor)
            drpCountry.DataBind()
            drpCountry.SelectedValue = selectedEarnerCountryCode()
            drpCountry.Visible = False
            radRetail.Visible = False
            radDistributor.Visible = False
        End If
    End Sub
    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        'Remove old entries from RoyaltyEarnings table
        removeOldEntries(gvEarners.SelectedValue)
        'Save changes
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As SqlCommand
        Dim bError As Boolean = False
        Dim lbl As Label
        Dim txt As TextBox
        Try
            For Each row As GridViewRow In gvAmounts.Rows
                oCmd = New SqlCommand("procRoyaltyEarningsPeartreeInsert", oConn)
                lbl = row.FindControl("lblDistBuyingID")
                txt = row.FindControl("txtEarning")
                If txt.Text <> "" Then
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@distBuyingID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@amount", SqlDbType.Decimal))
                        .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                        .Parameters.Add(New SqlParameter("@retail", SqlDbType.Bit))
                        .Parameters("@affID").Value = gvEarners.SelectedValue
                        .Parameters("@distBuyingID").Value = lbl.Text
                        .Parameters("@amount").Value = txt.Text
                        .Parameters("@countryCode").Value = selectedEarnerCountryCode()
                        .Parameters("@retail").Value = radRetail.Checked
                    End With
                    If isSelectedEarnerSuperRoyalty() Then
                        'If Earner get royalties from more than 1 country, then use the currenctly selected countryCode from the country dropdown
                        oCmd.Parameters("@countryCode").Value = drpCountry.SelectedValue
                    Else
                        'If earner is only earning from 1 country (standard earner) then the retail value must be set to True, as its the only type of order they can recieve from
                        oCmd.Parameters("@retail").Value = True
                    End If
                    Try
                        If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                        oCmd.ExecuteNonQuery()
                    Catch ex As Exception
                        Throw ex
                    Finally
                        oCmd.Dispose()
                    End Try
                End If
            Next
        Catch ex As Exception
            bError = True
            lblError.Text = ex.ToString
        Finally
            oConn.Dispose()
        End Try
        If Not bError Then
            lblComplete.Text = "<font color='red'>Royalty amounts updated successfully</font>"
            panAmounts.Visible = False
            gvEarners.SelectedIndex = -1
        End If
    End Sub
    Protected Sub lnkEditEarner_click(ByVal sender As Object, ByVal e As eventargs)
        Dim btn As LinkButton = CType(sender, LinkButton)
        Dim dcfc As DataControlFieldCell = (btn.Parent)
        Dim row As GridViewRow = CType(dcfc.Parent, GridViewRow)
        Dim earnerID As Integer = gvEarners.DataKeys(row.rowIndex).Value
        hidAffID.Value = earnerID
        row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffb6c1")
        lnkAdd.Visible = False
        dvAdd.Visible = True
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByIdSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters("@affID").Value = earnerID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            da = New SqlDataAdapter(oCmd)
            da.Fill(ds)
            dvAdd.DataSourceID = ""
            dvAdd.DataSource = ds
            dvAdd.DataBind()
            dvAdd.ChangeMode(DetailsViewMode.Edit)
        Catch ex As Exception
            Response.Write(ex.ToString)
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Sub btnUpdate_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim earnerID As Integer = hidAffID.Value
        Dim txtPassword As textbox = dvAdd.FindControl("txtPassword")
        Dim hidOldPassword As HiddenField = dvAdd.FindControl("hidOldPassword")
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procAffiliateByIdEarnerUpdate", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters.Add(New SqlParameter("@affTitle", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@affFirstname", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@affSurname", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@affCompany", SqlDbType.VarChar, 100))
            .Parameters.Add(New SqlParameter("@affAdd1", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@affAdd2", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@affAdd3", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@affAdd4", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@affAdd5", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@affPostcode", SqlDbType.VarChar, 10))
            .Parameters.Add(New SqlParameter("@affCountryCode", SqlDbType.VarChar, 5))
            .Parameters.Add(New SqlParameter("@affEmail", SqlDbType.VarChar, 100))
            .Parameters.Add(New SqlParameter("@affPassword", SqlDbType.VarChar, 20))
            .Parameters.Add(New SqlParameter("@affSuperRoyalty", SqlDbType.Bit))
            .Parameters("@affID").Value = earnerID
            .Parameters("@affTitle").Value = getField("txtTitle")
            .Parameters("@affFirstname").Value = getField("txtFirstname")
            .Parameters("@affSurname").Value = getField("txtSurname")
            .Parameters("@affCompany").Value = getField("txtCompany")
            .Parameters("@affAdd1").Value = getField("txtAdd1")
            .Parameters("@affAdd2").Value = getField("txtAdd2")
            .Parameters("@affAdd3").Value = getField("txtAdd3")
            .Parameters("@affAdd4").Value = getField("txtAdd4")
            .Parameters("@affAdd5").Value = getField("txtAdd5")
            .Parameters("@affPostcode").Value = getField("txtPostcode")
            .Parameters("@affCountryCode").Value = getField("drpCountry")
            .Parameters("@affEmail").Value = getField("txtEmail")
            .Parameters("@affPassword").Value = getField("txtPassword")
            .Parameters("@affSuperRoyalty").Value = getSuperEarner()
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex.ToString())
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        If txtPassword.Text <> "" Then
            'If password has been changed then update it in the membership profile too
            Membership.Provider.ApplicationName = "EBAffProvider"
            Dim u As MembershipUser = Membership.GetUser(getUsername())
            u.ChangePassword(u.GetPassword(), txtPassword.Text)
            Membership.Provider.ApplicationName = "EBProvider"
        End If
        'Clean up 
        dvAdd.Controls.Clear()
        dvAdd.Visible = False
        lblAddInstructions.Visible = False
        lnkAdd.Visible = True
        gvEarners.DataBind()
        lblError.Text = "Details updated."
    End Sub
    Protected Sub dvAdd_itemUpdating(ByVal sender As Object, ByVal e As DetailsViewUpdateEventArgs)
        
    End Sub
    Protected Sub dvAdd_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        
    End Sub
    Protected Sub btnUpdateCancel_click(ByVal sender As Object, ByVal e As EventArgs)
        dvAdd.Controls.Clear()
        dvAdd.Visible = False
        lblAddInstructions.Visible = False
        lnkAdd.Visible = True
        gvEarners.DataBind()
    End Sub

    'Subs
    Protected Sub removeOldEntries(ByVal affID As Integer)
        Try
            Dim d As New siteInclude
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procRoyaltyEarningsByAffIDDelete", oConn)
            With oCmd
                .CommandType = CommandType.StoredProcedure
                .Parameters.Add(New SqlParameter("@countryCode", SqlDbType.VarChar, 5))
                .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                .Parameters("@countryCode").Value = selectedEarnerCountryCode()
                .Parameters("@affID").Value = affID
            End With
            'If Earner get royalties from more than 1 country, then use the currenctly selected countryCode from the country dropdown
            If isSelectedEarnerSuperRoyalty() Then oCmd.Parameters("@countryCode").Value = drpCountry.SelectedValue
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                oCmd.ExecuteNonQuery()
            Catch ex As Exception
                Throw ex
            Finally
                oCmd.Dispose()
                oConn.Dispose()
            End Try
        Catch ex As Exception
            Dim si As New siteInclude
            si.addError("maintenance/royalty.aspx.vb", "removeOldEntries(affID=" & affID & ", countryCode=" & selectedEarnerCountryCode() & "); " & ex.ToString)
            si = Nothing
        End Try
    End Sub

    'Functions
    Protected Function showCountry(ByVal cc As Object, ByVal super As Boolean) As String
        Dim result As String = "Special"
        If Not super Then
            If Not isdbnull(cc) Then result = cc.ToString()
        End If
        Return result
    End Function
    Protected Function showAmount(ByVal o As Object)
        Dim result As String = ""
        If Not IsDBNull(o) Then result = FormatNumber(o.ToString, 2)
        Return result
    End Function
    Protected Function isSelectedEarnerSuperRoyalty() As Boolean
        Dim hidSuperRoyalty As HiddenField = gvEarners.Rows(gvEarners.SelectedIndex).FindControl("hidSuperRoyalty")
        Return CBool(hidSuperRoyalty.Value)
    End Function
    Protected Function selectedEarnerCountryCode() As String
        Dim hidCountryCode As HiddenField = gvEarners.Rows(gvEarners.SelectedIndex).FindControl("hidCountryCode")
        Return hidCountryCode.Value
    End Function
    Protected Function getField(ByVal s As String) As String
        Dim result As String = "Unknown"
        If lcase(s) = "drpcountry" Then
            Dim drp As DropDownList = dvAdd.FindControl(s)
            Try
                result = drp.SelectedValue
            Catch ex As Exception
            End Try
        Else
            Dim txt As textbox = dvAdd.FindControl(s)
            Try
                result = txt.Text
            Catch ex As Exception
            End Try
        End If
        
        Return result
    End Function
    Protected Function getSuperEarner() As Boolean
        Dim result As Boolean = False
        Dim chk As checkbox
        chk = dvAdd.FindControl("chkSuperRoyalty")
        result = chk.checked
        Return result
    End Function
    Protected Function getUsername() As String
        Dim result As String = ""
        Dim lbl As label = dvAdd.FindControl("lblUsername")
        Try
            result = lbl.Text
        Catch ex As Exception
        End Try
        Return result
    End Function
End Class
