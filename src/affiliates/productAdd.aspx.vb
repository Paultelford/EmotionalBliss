Imports siteInclude

Partial Class maintenance_productAdd
    Inherits BasePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Membership.GetUser Is Nothing Then Response.Redirect("default.aspx")
    End Sub
    Protected Sub drpComp_selectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
        If drpComp.SelectedValue = "0" Then
            dvComp.ChangeMode(DetailsViewMode.Insert)
        Else
            dvComp.ChangeMode(DetailsViewMode.Edit)
        End If
    End Sub
    Protected Sub dvComp_itemInserted(ByVal sender As Object, ByVal e As DetailsViewInsertedEventArgs)
        resetControls()
    End Sub
    Protected Sub dvComp_itemUpdated(ByVal sender As Object, ByVal e As DetailsViewUpdatedEventArgs)
        Dim html As String = "<table border='0'><tr><td>&nbsp;</td><td><b>Old Values</b></td><td><b>New Values</b></td></tr>"
        Dim eb As New siteInclude
        For Each row As DetailsViewRow In dvComp.Rows
            If row.RowType = DataControlRowType.DataRow Then
                If Not row.Cells(0).HasControls Then 'Used to ignore the last CommandButton row
                    Try
                        html = html & "<tr><td>" & row.Cells(0).Text & "</td><td>" & e.OldValues(row.RowIndex) & "</td>"
                    Catch ex As Exception
                        eb.addError("componentAdd.aspx.vb", "ERROR 1 on rowIndex " & row.RowIndex & ": row.Cells(0).Text='" & row.Cells(0).Text & "<br>")
                    End Try

                    Try
                        If e.OldValues(row.RowIndex) = e.NewValues(row.RowIndex) Then
                            html = html & "<td>" & e.NewValues(row.RowIndex) & "</td>"
                        Else
                            html = html & "<td><font color='red'>" & e.NewValues(row.RowIndex) & "</td>"
                        End If
                    Catch ex As Exception
                        eb.addError("componentAdd.aspx.vb", "ERROR 2 on rowIndex " & row.RowIndex & "<br>")
                    End Try
                    html = html & "</tr>"
                Else

                End If
            End If
        Next
        html = html & "</table>"
        eb.addToComponentHistory(0, 0, 0, 0, 0, 0, 0, 0, 0, 7, 0, Convert.ToInt32(dvComp.DataKey.Value), html, Membership.GetUser.UserName, False)
        eb = Nothing
        resetControls()
    End Sub
    Protected Sub resetControls()
        drpComp.Items.Clear()
        drpComp.Items.Add(New ListItem("Please Choose....", "0"))
        drpComp.DataBind()
        drpComp.SelectedIndex = 0
        dvComp.DataBind()
    End Sub
End Class
