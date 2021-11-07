Imports System.Collections

Partial Class maintenance_hash
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim tmp As Hashtable = Session("ebMaintenanceComponentList")
        'gvHash.DataSource = tmp
        'gvHash.DataBind()
        For Each da As DictionaryEntry In tmp
            Response.Write(da.Key & " - " & da.Value & "<br>")
        Next
    End Sub
End Class
