
Partial Class shop_clearSession
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        clear("EBTmpUniqueID")
        clear("EBTmpStatus")
        clear("EBTmpStatusDetail")
        clear("EBTmpError")
    End Sub
    Protected Sub clear(ByVal sess As String)
        Session(sess) = ""
        Response.Write(sess & "<br>")
    End Sub
End Class
