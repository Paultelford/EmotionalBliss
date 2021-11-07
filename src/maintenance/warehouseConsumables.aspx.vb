Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class maintenance_warehouseConsumables
    Inherits System.Web.UI.Page
    Private Const _gvComponents_componentNamePos = 0
    Private Const _gvComponents_manufacturerNamePos = 1
    Private Const _gvComponents_stockPos = 2
    Private Const _gvComponents_qtyUsedNewPos = 3

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub
    Protected Sub gvComponents_rowUpdating(ByVal sender As Object, ByVal e As GridViewUpdateEventArgs)
        'Check that new value entered does not exceed currenct stock
        Dim bError As Boolean = False
        Dim txtQtyUsed As TextBox = gvComponents.Rows(e.RowIndex).FindControl("txtQtyUsed")
        Dim iStock As Integer = Convert.ToInt32(gvComponents.Rows(e.RowIndex).Cells(_gvComponents_stockPos).Text)
        Dim iQtyUsed As Integer = Convert.ToInt32(txtQtyUsed.Text)
        Dim iCompID = Convert.ToInt32(gvComponents.DataKeys(e.RowIndex).Value)
        Dim description As String = "<b>Reason:</b> " & txtReason.Text
        If iStock < iQtyUsed Then bError = True

        If bError Then
            lblError.Text = "<font color='red'>You are trying to take more components than are currently in stock.</font>"
            e.Cancel = True
        Else
            'add to db(componentHistory)
            Dim eb As New siteInclude
            eb.addToComponentHistory(0, 0, 0, 0, iQtyUsed, 0, 0, 0, 0, 9, 0, iCompID, description, Membership.GetUser.UserName, False)
            e.Cancel = True
            gvComponents.Columns(_gvComponents_qtyUsedNewPos).Visible = False
            gvComponents.EditIndex = -1
            pan1.Visible = False
        End If
    End Sub
    Protected Sub gvComponents_rowEditing(ByVal sender As Object, ByVal e As GridViewEditEventArgs)
        gvComponents.Columns(_gvComponents_qtyUsedNewPos).Visible = True
        pan1.Visible = True
        txtReason.Text = ""
        lblError.Text = ""
    End Sub
    Protected Sub gvComponents_rowEditCancel(ByVal sender As Object, ByVal e As GridViewCancelEditEventArgs)
        gvComponents.Columns(_gvComponents_qtyUsedNewPos).Visible = False
        pan1.Visible = False
    End Sub
End Class

