Imports System.Data
Imports System.Data.SqlClient

Partial Class affiliates_affLinks
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Or Session("EBAffID") = "" Then Response.Redirect("default.aspx")
        If Not Page.IsPostBack Then
            If LCase(Session("EBAffCountryCode")) = "gb" Then
                lblDefault.Text = "http://www.emotionalbliss.co.uk"
            Else
                lblDefault.Text = "http://" & Request.ServerVariables("SERVER_NAME")
            End If
            If Request.ServerVariables("SERVER_PORT") <> 80 Then lblDefault.Text = lblDefault.Text & ":" & Request.ServerVariables("SERVER_PORT")
            lblDefault.Text = lblDefault.Text & "/affiliates.aspx?affid=" & Session("EBAffID")
            'Hide Port number and replace 'Secure' with country code (if viewing via SSL)
            lblDefault.Text = Replace(lblDefault.Text, ":443", "")
            If LCase(Session("EBAffCountryCode")) <> "gb" Then
                lblDefault.Text = Replace(lblDefault.Text, "secure", Session("EBAffCountryCode"))
            End If
            showProductLinks()
        End If
    End Sub
    Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As EventArgs) Handles Me.LoadComplete
        Dim drp As DropDownList = Master.FindControl("drpAffmenu")
        drp.SelectedValue = "links"
    End Sub

    'Page
    Protected Sub drpOtherProducts_dataBinding(ByVal sender As Object, ByVal e As EventArgs)
        Dim drp As DropDownList = CType(sender, DropDownList)
        drp.Items.Clear()
        drp.Items.Add(New ListItem("Choose product to generate link", ""))
    End Sub

    'User
    Protected Sub drpOtherProducts_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        lblOther.Text = "http://" & Request.ServerVariables("SERVER_NAME")
        If Request.ServerVariables("SERVER_PORT") <> 80 Then lblOther.Text = lblOther.Text & ":" & Request.ServerVariables("SERVER_PORT")
        lblOther.Text = lblOther.Text & "/affiliates.aspx?affid=" & Session("EBAffID") & "&amp;pid=" & drpOtherProducts.SelectedValue
    End Sub

    'Subs
    Protected Sub showProductLinks()
        Dim dt As New DataTable
        Try
            Dim param() As String = {"@countryCode", "@allowAffLink"}
            Dim paramValue() As String = {Session("EBAffCountryCode"), "true"}
            Dim paramType() As SqlDbType = {SqlDbType.VarChar, SqlDbType.Bit}
            Dim paramSize() As Integer = {5, 30}
            Dim lnk As String = ""
            dt = siteInclude.getDataTable(param, paramValue, paramType, paramSize, "procProductOnSaleByAllowAffLinkSelect")
            If dt.Rows.Count > 0 Then
                For Each row As DataRow In dt.Rows
                    addLinkText(row("saleName") & ":<br><span style='background-color: #eeeeee;'>")
                    If LCase(Session("EBAffCountryCode")) = "gb" Then
                        lnk = "http://www.emotionalbliss.co.uk"
                    Else
                        lnk = "http://" & Request.ServerVariables("SERVER_NAME")
                    End If
                    If Request.ServerVariables("SERVER_PORT") <> 80 Then lnk = lnk & (":" & Request.ServerVariables("SERVER_PORT"))
                    lnk = lnk & "/affiliates.aspx?affid=" & Session("EBAffID") & "&amp;pid=" & row("id") & "</span><br><br>"
                    lnk = Replace(lnk, ":443", "")
                    If LCase(Session("EBAffCountryCode")) <> "gb" Then
                        lnk = Replace(lnk, "secure", Session("EBAffCountryCode"))
                    End If
                    addLinkText(lnk)
                Next
            End If
        Catch ex As Exception
            Response.Write(ex.ToString)
            siteInclude.addError("affiliates/affLinks.aspx", "showProducts(affid=" & Session("EBAffID") & "); " & ex.ToString())
        Finally
            dt.Dispose()
        End Try
    End Sub
    Protected Sub addLinkText(ByVal txt As String)
        litProductLinks.Text = litProductLinks.Text & txt
    End Sub

End Class
