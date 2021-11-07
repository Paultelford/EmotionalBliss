
Partial Class maintenance_test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim a As New ArrayList
        a.Add("TestVal1")
        a.Add("TestVal2")
        gvTest.DataSource = a
        gvTest.DataBind()
    End Sub

    Protected Sub gvTest_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)
        Response.Write(gv.Rows(0).Cells(0).Text)
    End Sub
End Class
