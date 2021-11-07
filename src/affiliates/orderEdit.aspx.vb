Imports System.Data.SqlClient

Partial Class affiliates_orderEdit
    Inherits BasePage
    Protected Const _dvCustomer_cardNoPos As Integer = 18
    Protected Const _dvCustomer_ccEncPos As Integer = 19

    Protected Sub sqlCustomer_updating(ByVal sender As Object, ByVal e As SqlDataSourceCommandEventArgs)
        For Each param As sqlParameter In e.Command.Parameters
            If String.IsNullOrEmpty(param.Value) Then param.Value = ""
        Next
        'Set card parameters
        e.Command.Parameters("@ccEnc").Value = encryptCard(e.Command.Parameters("@cardNo").Value)
        e.Command.Parameters("@cardNo").Value = getCardNo(e.Command.Parameters("@cardNo").Value)
    End Sub
    Protected Sub dvCustomer_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        'Decode the cardnumber
        Dim txtCardNo As TextBox = dvCustomer.Rows(_dvCustomer_cardNoPos).Cells(1).Controls(0)
        Dim lblCCEnc As Label = dvCustomer.FindControl("lblCCEnc")
        txtCardNo.Text = decryptCard(lblCCEnc.Text)
        lnkBack.NavigateUrl = "orderView.aspx?id=" & Request.QueryString("id")
    End Sub
    Protected Function encryptCard(ByVal n As String) As String
        Dim fes As New FE_SymmetricNamespace.FE_Symmetric
        Dim enc As String = fes.EncryptData(ConfigurationManager.AppSettings("aesKey").ToString, n)
        Return enc
    End Function
    Protected Function decryptCard(ByVal n As String) As String
        Dim fes As New FE_SymmetricNamespace.FE_Symmetric
        Dim dec As String = fes.DecryptData(ConfigurationManager.AppSettings("aesKey").ToString, n)
        Return dec
    End Function
    Protected Function getCardNo(ByVal ccNum As String) As String
        Dim result As String = ""
        If Len(ccNum) > 4 Then
            For iLoop As Integer = 1 To Len(ccNum) - 4
                result = result & "*"
            Next
            result = result & Right(ccNum, 4)
        End If
        Return result
    End Function
    Protected Sub sqlCustomer_updated(ByVal sender As Object, ByVal e As SqlDataSourceStatusEventArgs)
        Response.Redirect("orderView.aspx?id=" & Request.QueryString("id"))
    End Sub
End Class
