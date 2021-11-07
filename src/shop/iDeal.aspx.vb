Imports System.Data
Imports System.Data.SqlClient

Partial Class shop_iDeal
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim iOrderID As Integer
        Try
            iOrderID = CInt(Session("EBTmpUniqueID"))
        Catch ex As Exception
            'If error is found, session must have been lost
            Response.End()
        End Try
        'Response.Write("order=" & iOrderID)
        'Response.End()
        populateForm(iOrderID)
    End Sub
    Protected Sub populateForm(ByVal id As Integer)
        Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
        Dim oCmd As New SqlCommand("procShopOrderCustomerIdealDetailsByIDSelect", oConn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim inputOrderID As HtmlInputText = FindControl("orderID")
        Dim inputAmount As HtmlInputText = FindControl("amount")
        Dim inputEmail As HtmlInputText = FindControl("email")
        Dim inputDescription As HtmlInputText = FindControl("description")
        Dim inputCustomerName As HtmlInputText = FindControl("customerName")
        Dim inputZip As HtmlInputText = FindControl("zip")
        Dim inputAddress As HtmlInputText = FindControl("address")

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
                Dim rs As DataRow = ds.Tables(0).Rows(0)
                'populate controls
                inputOrderID.Value = CStr(id)
                inputAmount.Value = CStr(CLng(rs("orderTotal") * 100))
                inputEmail.Value = rs("email")
                inputDescription.Value = "EB Order"
                inputCustomerName.Value = rs("customerName")
                inputZip.Value = rs("postcode")
                inputAddress.Value = rs("address")
            Else
                'Order not found!!
                Response.Write("Order not found")
                Response.End()
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
            Response.End()
        Finally
            ds.Dispose()
            da.Dispose()
            oCmd.Dispose()
            oConn.Dispose()
        End Try

        'inputOrderID.Value = Session("EBiDealTmp_orderID")
        
    End Sub
End Class
