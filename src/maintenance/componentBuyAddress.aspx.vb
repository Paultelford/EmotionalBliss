Imports System.Data
Imports System.Data.SqlClient

Partial Class maintenance_componentBuyAddress
    Inherits System.Web.UI.Page
    Private Const _dvAdd_companyPos = 0
    Private Const _dvAdd_Add1Pos = 1
    Private Const _dvAdd_Add2Pos = 2
    Private Const _dvAdd_Add3Pos = 3
    Private Const _dvAdd_Add4Pos = 4
    Private Const _dvAdd_Add5Pos = 5
    Private Const _dvAdd_telPos = 6
    Private Const _dvAdd_emailPos = 7
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub Page_Load(ByVal Sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            If Not Page.IsPostBack Then
                If Request.QueryString("type") = "ship" Then
                    lblTitle.Text = "Add Shipping Address"
                Else
                    lblTitle.Text = "Add Billing Address"
                End If
            End If
        End If
    End Sub


    Protected Sub btnAdd_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As SqlCommand
        Dim txtCompany As TextBox = dvAdd.Rows(_dvAdd_companyPos).FindControl("txtCompany")
        Dim txtAdd1 As TextBox = dvAdd.Rows(_dvAdd_Add1Pos).FindControl("txtAdd1")
        Dim txtAdd2 As TextBox = dvAdd.Rows(_dvAdd_Add2Pos).FindControl("txtAdd2")
        Dim txtAdd3 As TextBox = dvAdd.Rows(_dvAdd_Add3Pos).FindControl("txtAdd3")
        Dim txtAdd4 As TextBox = dvAdd.Rows(_dvAdd_Add4Pos).FindControl("txtAdd4")
        Dim txtAdd5 As TextBox = dvAdd.Rows(_dvAdd_Add5Pos).FindControl("txtAdd5")
        Dim txtTel As TextBox = dvAdd.Rows(_dvAdd_telPos).FindControl("txtTel")
        Dim txtEmail As TextBox = dvAdd.Rows(_dvAdd_emailPos).FindControl("txtEmail")
        If Request.QueryString("type") = "ship" Then
            oCmd = New SqlCommand("procEBShippingInsert", oConn)
        Else
            oCmd = New SqlCommand("procEBBillingInsert", oConn)
        End If
        With oCmd
            .CommandType = CommandType.StoredProcedure
            .Parameters.Add(New SqlParameter("@company", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add1", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add2", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add3", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add4", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@add5", SqlDbType.VarChar, 50))
            .Parameters.Add(New SqlParameter("@tel", SqlDbType.VarChar, 30))
            .Parameters.Add(New SqlParameter("@email", SqlDbType.VarChar, 50))
            .Parameters("@company").Value = txtCompany.Text
            .Parameters("@add1").Value = txtAdd1.Text
            .Parameters("@add2").Value = txtAdd2.Text
            .Parameters("@add3").Value = txtAdd3.Text
            .Parameters("@add4").Value = txtAdd4.Text
            .Parameters("@add5").Value = txtAdd5.Text
            .Parameters("@tel").Value = txtTel.Text
            .Parameters("@email").Value = txtEmail.Text
        End With
        Try
            If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
            oCmd.ExecuteNonQuery()
        Catch ex As Exception
            Response.Write(ex)
            Response.End()
        Finally
            oCmd.Dispose()
            oConn.Dispose()
        End Try
        Server.Transfer("componentBuy.aspx")
    End Sub
End Class
