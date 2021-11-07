Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_royalty
    Inherits System.Web.UI.Page

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Init
        
    End Sub
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        lblComplete.Text = ""
        lblError.Text = ""
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
                oCmd = New SqlCommand("procRoyaltyEarningsInsert", oConn)
                lbl = row.FindControl("lblDistBuyingID")
                txt = row.FindControl("txtEarning")
                If txt.Text <> "" Then
                    With oCmd
                        .CommandType = CommandType.StoredProcedure
                        .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@distBuyingID", SqlDbType.Int))
                        .Parameters.Add(New SqlParameter("@amount", SqlDbType.Decimal))
                        .Parameters("@affID").Value = gvEarners.SelectedValue
                        .Parameters("@distBuyingID").Value = lbl.Text
                        .Parameters("@amount").Value = txt.Text
                    End With
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
            gvAmounts.Visible = False
        End If
    End Sub
    Protected Sub removeOldEntries(ByVal affID As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procRoyaltyEarningsByAffIDDelete", oConn)
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@affID", SqlDbType.Int))
            .Parameters("@affID").Value = affID
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
    End Sub
    Protected Function showAmount(ByVal o As Object)
        Dim result As String = ""
        If Not IsDBNull(o) Then result = FormatNumber(o.ToString, 2)
        Return result
    End Function
    Protected Sub gvEarners_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        gvAmounts.Visible = True
    End Sub
End Class
