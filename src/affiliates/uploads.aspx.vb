Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_uploads
    Inherits BasePage
    Private Const _affType_affiliate As Integer = 1
    Private Const _affType_distributor As Integer = 2
    Private Const _affType_wholesaler As Integer = 3
    Private Const _affType_countryRep As Integer = 4
    Private Const _affType_press As Integer = 6
    Private Const _affType_pressPublic As Integer = 7

    'System
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If (Membership.GetUser Is Nothing) Or (Session("EBAffID") = "") Then Response.Redirect("default.aspx")
        lblError.Text = ""
        If Not Page.IsPostBack Then
            'Bind country dropdown
            'drpCountry.List.Items.Clear()
            Dim dt As New DataTable
            Try
                Dim param() As String = {}
                Dim paramValue() As String = {}
                Dim paramType() As SqlDbType = {}
                Dim paramSize() As Integer = {0}
                dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procCountrySelect")
                For Each row As DataRow In dt.Rows
                    'drpCountry.List.Items.Add(New ListItem(row("countryName"), row("countryCode")))
                Next
            Catch ex As Exception
                siteInclude.addError("affiliates/uploads.aspx.vb", "Page_Load(); " & ex.ToString())
            Finally
                dt.Dispose()
            End Try
        End If
    End Sub

    'Page

    'User
    Protected Sub btnUpload_click(ByVal sender As Object, ByVal e As EventArgs)
        Page.Validate()
        Dim bSuccess As Boolean = False
        Dim bNoItemsChecked As Boolean = True
        Dim bError As Boolean = False
        If Page.IsValid Then
            'Make sure at least 1 checkbox is checked

            If fu1.HasFile Then
                fu1.SaveAs(Request.ServerVariables("APPL_PHYSICAL_PATH") & "uploads\documents\" & fu1.FileName)
                If chkAff.Checked Then
                    bNoItemsChecked = False
                    bSuccess = addToDataBase(_affType_affiliate, fu1.FileName)
                    If Not bSuccess Then bError = True
                End If
                If chkDist.Checked Then
                    bNoItemsChecked = False
                    bSuccess = addToDataBase(_affType_distributor, fu1.FileName)
                    If Not bSuccess Then bError = True
                End If
                If chkWhole.Checked Then
                    bNoItemsChecked = False
                    bSuccess = addToDataBase(_affType_wholesaler, fu1.FileName)
                    If Not bSuccess Then bError = True
                End If
                If chkRep.Checked Then
                    bNoItemsChecked = False
                    bSuccess = addToDataBase(_affType_countryRep, fu1.FileName)
                    If Not bSuccess Then bError = True
                End If
                If chkPress.Checked Then
                    bNoItemsChecked = False
                    bSuccess = addToDataBase(_affType_press, fu1.FileName)
                    If Not bSuccess Then bError = True
                End If
                If chkPressPublic.Checked Then
                    bNoItemsChecked = False
                    bSuccess = addToDataBase(_affType_pressPublic, fu1.FileName)
                    If Not bSuccess Then bError = True
                End If
            Else
                lblError.Text = "Please choose a file to upload"
            End If
        End If
        If bError Then
            lblError.Text = "File successfully uploaded."
        End If
    End Sub
    Protected Sub chkActive_checkChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chk As CheckBox = CType(sender, CheckBox)
        Dim docID As Integer = 0
        Dim dcfc As DataControlFieldCell = (chk.Parent)
        Dim row As GridViewRow = CType(dcfc.Parent, GridViewRow)
        docID = gvDocs.DataKeys(row.RowIndex).Value
        Try
            Dim param() As String = {"@documentID", "@active"}
            Dim paramValue() As String = {docID.ToString, chk.Checked.ToString}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.Bit}
            Dim paramSize() As Integer = {0, 0}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procDocumentByIDActiveUpdate")
        Catch ex As Exception
            lblError.Text = ex.Message
            siteInclude.addError("affilites/uploads.aspx.vb", "chkActive_checkChanged(docID=" & docID & "); " & ex.ToString)
        Finally
        End Try
        gvDocs.DataBind()
    End Sub

    'Functions
    Protected Function addToDataBase(ByVal affType As Integer, ByVal file As String) As Boolean
        Dim bError As Boolean = False
        Try
            Dim param() As String = {"affiliateType", "filename", "description", "countryCode"}
            Dim paramValue() As String = {affType, file, txtDecription.Text, Session("EBAffEBDistributorCountryCode")}
            Dim paramType() As SqlDbType = {SqlDbType.Int, SqlDbType.VarChar, SqlDbType.VarChar, SqlDbType.VarChar}
            Dim paramSize() As Integer = {0, 100, -1, 5}
            siteInclude.executeNonQuery(param, paramValue, paramType, paramSize, "procDocumentInsert")
        Catch ex As Exception
            bError = True
            lblError.Text = "An error occured while updatig the database."
            siteInclude.addError("affiliates.aspx", "addToDataBase(affType=" & affType & ", file=" & file & "); " & ex.ToString)
        End Try
        Return Not bError
    End Function
End Class
