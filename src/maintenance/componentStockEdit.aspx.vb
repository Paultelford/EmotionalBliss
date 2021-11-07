Imports siteInclude

Partial Class maintenance_componentStockEdit
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder

    Protected Sub PageLoad(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
        End If
    End Sub

    Protected Sub btnSubmit_click(ByVal sender As Object, ByVal e As EventArgs)
        Dim eb As New siteInclude
        Dim qAdd As Integer = 0
        Dim qRem As Integer = 0
        Dim sAdd As Integer = 0
        Dim sRem As Integer = 0

        Dim bQuarantine As Boolean = False
        Dim sAction As String = lblQty.Text & " items "
        Dim ra As RadioButtonList = _content.FindControl("radAction")
        Dim rd As RadioButtonList = _content.FindControl("radDestination")
        If ra.SelectedValue = "add" Then
            sAction = sAction & " added to "
            If rd.SelectedValue = "stock" Then
                sAction = sAction & " stock."
                sAdd = Convert.ToInt32(lblQty.Text)
            Else
                sAction = sAction & " quarantine."
                bQuarantine = True
                qAdd = Convert.ToInt32(lblQty.Text)
            End If
        Else
            sAction = sAction & " removed from "
            If rd.SelectedValue = "stock" Then
                sAction = sAction & " stock."
                sRem = Convert.ToInt32(lblQty.Text)
            Else
                sAction = sAction & " quarantine."
                bQuarantine = True
                qRem = Convert.ToInt32(lblQty.Text)
            End If
        End If
        sAction = sAction & "<br><b>Reason: </b> " & txtReason.Text
        siteInclude.addToComponentHistory(0, qAdd, qRem, sAdd, sRem, 0, 0, 0, 0, 6, 0, Request.QueryString("id"), sAction, Membership.GetUser.UserName, bQuarantine, 0, True)
        eb = Nothing
        dvComp.DataBind()
        lnkSame.NavigateUrl = "componentStockEdit.aspx?id=" & Request.QueryString("id") & "&masterIndex=" & Request.QueryString("masterIndex") & "&manIndex=" & Request.QueryString("manIndex")
        tblDetails.Visible = False
        panHandle.Visible = True
    End Sub
    Protected Sub lbtnBack_click(ByVal sender As Object, ByVal e As EventArgs)
        Context.Items("masterIndex") = Request.QueryString("masterIndex")
        Context.Items("manIndex") = Request.QueryString("manIndex")
        Server.Transfer("componentStock.aspx")
    End Sub
End Class
