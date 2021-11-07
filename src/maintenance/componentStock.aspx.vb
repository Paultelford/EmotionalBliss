Imports siteInclude

Partial Class maintenance_componentStockView
    Inherits System.Web.UI.Page
    Private _login As LoginView
    Private _content As ContentPlaceHolder
    Private Const _gvComponents_manufacturerPos = 3
    Private Const _gvComponents_quarantinePos = 6
    Private Const _gvComponents_stockPos = 4
    Private Const _gvComponents_totalPos = 7
    Private Const _gvComponents_reorderPos = 9
    Private Const _gvComponents_adjustPos = 10
    Private Const _gvComponents_onOrder = 8
    Private Const _gvComponents_quarantineNewPos = 0
    Private Const _gvComponents_stockNewPos = 1
    Private Const _gvComponents_reorderNewPos = 2
    Private Const _gvOutstanding_OnOrder = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Membership.GetUser Is Nothing Then
            _login = Master.FindControl("logMaintenance")
            _content = _login.FindControl("ContentPlaceHolder1")
            If Page.IsPostBack Then
                gvComponents.Visible = True
                gvOutstanding.Visible = True
            Else
                If Context.Items("masterIndex") <> "" And Context.Items("manIndex") <> "" Then
                    drpMaster.SelectedIndex = Convert.ToInt32(Context.Items("masterIndex"))
                    drpMan.SelectedIndex = Convert.ToInt32(Context.Items("manIndex"))
                    gvComponents.Visible = True
                    gvOutstanding.Visible = True
                    DataBind()
                End If
            End If
        Else
            Response.End()
        End If
    End Sub
    Protected Sub gvOutstanding_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lbl As Label
        For Each row As GridViewRow In gvOutstanding.Rows
            If row.RowType = DataControlRowType.DataRow Then
                lbl = row.FindControl("lblOutstanding")
                gvComponents.Rows(row.RowIndex).Cells(_gvComponents_onOrder).Text = lbl.Text
            End If
        Next
    End Sub
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        Dim body As HtmlControl = Master.FindControl("siteBody")
        body.Attributes.Add("onResize", "for(iLoop=0;iLoop<" & gvComponents.Rows.Count & ";iLoop++) document.getElementById('gvOutRow' + iLoop).style.height=document.getElementById('gvCompRow' + iLoop).offsetHeight-2;")
    End Sub
    Protected Sub gvComponents_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        Dim lnk As HyperLink
        'Create 'Adjust' link querysting
        For Each row As GridViewRow In gvComponents.Rows
            lnk = row.FindControl("lnkAdjust")
            lnk.NavigateUrl = "componentStockEdit.aspx?id=" & gvComponents.DataKeys(row.RowIndex).Value & "&masterIndex=" & drpMaster.SelectedIndex & "&manIndex=" & drpMan.SelectedIndex
        Next
    End Sub
    Protected Sub gvComponents_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        Dim lblQuarantine As Label
        Dim txtQuarantine As TextBox
        Dim lblStock As Label
        Dim txtStock As TextBox
        Dim lblReorder As Label
        Dim txtReorder As TextBox
        'Check stock level compared to reorder levels, and show items that need reorder as red forecolor
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Attributes.Add("id", "gvCompRow" & e.Row.RowIndex)
            If e.Row.RowState = DataControlRowState.Edit Then
                txtQuarantine = e.Row.FindControl("txtQuarantine")
                txtStock = e.Row.FindControl("txtStock")
                lblReorder = e.Row.FindControl("lblReorder")
                'If Convert.ToInt32(e.Row.Cells(_gvComponents_stockPos).Text) < Convert.ToInt32(lblReorder.Text) Then e.Row.ForeColor = Drawing.Color.Red
            Else
                lblQuarantine = e.Row.FindControl("lblQuarantine")
                lblStock = e.Row.FindControl("lblStock")
                lblReorder = e.Row.FindControl("lblReorder")
                Try
                    If Convert.ToInt32(lblStock.Text) < Convert.ToInt32(lblReorder.Text) Then e.Row.ForeColor = Drawing.Color.Red
                Catch ex As Exception
                    '2nd row is not being processed as "state=DataControlRowState.Edit" for some reason
                    lblReorder = e.Row.FindControl("lblReorder")
                    txtStock = e.Row.FindControl("txtStock")
                    If Convert.ToInt32(txtStock.Text) < Convert.ToInt32(lblReorder.Text) Then e.Row.ForeColor = Drawing.Color.Red
                End Try

            End If
        End If
        'add hyperlink to Stock Value Popup
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblStockText As Label = e.Row.FindControl("lblStock")
            e.Row.Cells(_gvComponents_stockPos).Controls.Clear()
            e.Row.Cells(_gvComponents_stockPos).Text = "<a href='componentStockValuePop.aspx?id=" & gvComponents.DataKeys(e.Row.RowIndex).Value & "' target='_blank'><acronym title='View Stock Value'>" & lblStockText.Text & "</acronym></a>"
        End If
    End Sub
    Protected Sub gvOutstanding_rowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs)
        e.Row.Attributes.Add("id", "gvOutRow" & e.Row.RowIndex)
    End Sub
    Protected Sub drpMaster_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If Context.Items("masterIndex") <> "" Then
            'drpMaster.SelectedIndex = Convert.ToInt32(Context.Items("masterIndex"))
            'Context.Items("masterIndex") = ""
            'DataBind()
        End If

        addJS()
    End Sub
    Protected Sub drpMan_dataBound(ByVal sender As Object, ByVal e As EventArgs)
        If Context.Items("manIndex") <> "" Then
            'drpMan.SelectedIndex = Convert.ToInt32(Context.Items("manIndex"))
            'Context.Items("manIndex") = ""
            'DataBind()
        End If
        addJS()
    End Sub
    Protected Function show(ByVal data As Object) As Integer
        Dim result As Integer
        If IsDBNull(Data) Then
            result = 0
        Else
            result = Convert.ToInt32(Data.ToString)
        End If
        Return result
    End Function
    Protected Sub gvComponents_rowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        'Check for invalid input
        Dim bError As Boolean = False
        Dim row As GridViewRow = gvComponents.Rows(e.RowIndex)
        Dim txtQuarantine As TextBox = row.FindControl("txtQuarantine")
        Dim txtStock As TextBox = row.FindControl("txtStock")
        Dim txtReorder As TextBox = row.FindControl("txtReorder")
        If txtQuarantine.Text = "" Or txtStock.Text = "" Then bError = True
        If Not (IsNumeric(txtQuarantine.Text) Or IsNumeric(txtStock.Text)) Then bError = True
        If bError Then
            lblError.Text = "<font color='red'>Invalid input, all values must be numeric.</font>"
        Else
            Dim eb As siteInclude
            Dim add As Integer
            Dim remove As Integer
            'Test for a change in quarantine figures
            If Convert.ToInt32(e.OldValues(_gvComponents_quarantinePos)) <> Convert.ToInt32(e.NewValues(_gvComponents_quarantineNewPos)) Then
                If Convert.ToInt32(e.OldValues(_gvComponents_quarantinePos)) < Convert.ToInt32(e.NewValues(_gvComponents_quarantineNewPos)) Then
                    add = Convert.ToInt32(e.NewValues(_gvComponents_quarantineNewPos)) - Convert.ToInt32(e.OldValues(_gvComponents_quarantinePos))
                    remove = 0
                Else
                    add = 0
                    remove = Convert.ToInt32(e.OldValues(_gvComponents_quarantinePos)) - Convert.ToInt32(e.NewValues(_gvComponents_quarantineNewPos))
                End If
                'Add entry to componentHistory
                eb = New siteInclude
                eb.addToComponentHistory(0, add, remove, 0, 0, 0, 0, 0, 0, 6, 0, gvComponents.DataKeys(e.RowIndex).Value, "Quarantine stock changed from " & e.OldValues(_gvComponents_quarantinePos) & " to " & e.NewValues(_gvComponents_quarantineNewPos), Membership.GetUser.UserName, True)
                eb = Nothing
            End If
            'Test for a change in stock figures
            If Convert.ToInt32(e.OldValues(_gvComponents_stockPos)) <> Convert.ToInt32(e.NewValues(_gvComponents_stockNewPos)) Then
                If Convert.ToInt32(e.OldValues(_gvComponents_stockPos)) < Convert.ToInt32(e.NewValues(_gvComponents_stockNewPos)) Then
                    add = Convert.ToInt32(e.NewValues(_gvComponents_stockNewPos)) - Convert.ToInt32(e.OldValues(_gvComponents_stockPos))
                    remove = 0
                Else
                    add = 0
                    remove = Convert.ToInt32(e.OldValues(_gvComponents_stockPos)) - Convert.ToInt32(e.NewValues(_gvComponents_stockNewPos))
                End If
                eb = New siteInclude
                eb.addToComponentHistory(0, 0, 0, add, remove, 0, 0, 0, 0, 6, 0, gvComponents.DataKeys(e.RowIndex).Value, "Component stock changed from " & e.OldValues(_gvComponents_stockPos) & " to " & e.NewValues(_gvComponents_stockNewPos), Membership.GetUser.UserName, False)
                eb = Nothing
            End If
            'Hide any previous errors
            lblError.Text = ""
        End If
        e.Cancel = True
        gvComponents.EditIndex = -1
        gvComponents.DataBind()
    End Sub
    Protected Sub addJS()
        gvComponents.DataBind()
        'Dim myScript As String = "for(iLoop=0;document.element[iLoop].slice(4)<>'ct10'<iLoop++){"
        Dim myScript As String = ""
        myScript = myScript & "for(iLoop=0;iLoop<" & gvComponents.Rows.Count & ";iLoop++)"
        myScript = myScript & "document.getElementById('gvOutRow' + iLoop).style.height=document.getElementById('gvCompRow' + iLoop).offsetHeight-2;"

        'For iLoop As Integer = 0 To gvComponents.Rows.Count - 1
        'myScript = myScript & "document.getElementById('gvOutRow" & iLoop & "').style.height=document.getElementById('gvCompRow" & iLoop & "').offsetHeight-2;"
        'Next
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myKey", myScript, True)
    End Sub
    Protected Function makeValuePopURL(ByVal id As String) As String
        Dim result As String = "componentStockValuePop.aspx?id=" & id
        Return result
    End Function
    Protected Function showOnOrder(ByVal o As Object) As String
        Dim result As String = "0"
        If Not IsDBNull(o) Then result = o.ToString
        Return result
    End Function

End Class
