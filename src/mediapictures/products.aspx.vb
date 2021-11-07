Imports System.Drawing
Imports System.Drawing.Imaging

Partial Class mediapictures_products
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Session("ebMediaAccess") <> "true" Then Server.Transfer("~/mediapictures/default.aspx")
        showChoice()
    End Sub
    Protected Sub showChoice()
        Dim aFilename() As String
        Dim aFilenamesmall() As String
        Dim aFilesize() As String
        Dim directory As String = ""
        Select Case LCase(Request.ServerVariables("QUERY_STRING"))
            Case "chi"
                aFilename = New String() {"chandraMood.tif", "chandraBack.tif", "chandraBottom.tif", "chandraDefocus.tif", "chandraFront.tif", "chandraTop.tif", "chandraHand1.tif", "chandraHand2.tif"}
                aFilenamesmall = New String() {"chandraMood", "chandraBack", "chandraBottom", "chandraDefocus", "chandraFront", "chandraTop", "chandraHand1", "chandraHand2"}
                aFilesize = New String() {"2.1 MB", "1.5 MB", "1.9 MB", "4.1 MB", "1.5 MB", "1.9 MB", "1.8 MB", "2.3 MB"}
                directory = "chandra"
            Case "clo"
                aFilename = New String() {"chandra1.gif", "chandra2.gif", "chandra3.gif", "chandra4.gif", "chandra5.gif", "chandra6.gif", "chandra7.gif"}
                aFilenamesmall = New String() {"chandraMood", "chandraBack", "chandraBottom", "chandraDefocus", "chandraFront", "chandraTop", "chandraHand1", "chandraHand2"}
                aFilesize = New String() {"22kb", "18kb", "23kb", "8kb", "24kb", "23kb", "28kb"}
                directory = "webres"
            Case "fhi"
                aFilename = New String() {"femblossomMood.tif", "femblossomBack.tif", "femblossomBottom.tif", "femblossomDefocus.tif", "femblossomDefocus2.tif", "femblossomFront.tif", "femblossomTop.tif", "femblossomHand1.tif", "femblossomHand2.tif"}
                aFilenamesmall = New String() {"femblossomMood", "femblossomBack", "femblossomBottom", "femblossomDefocus", "femblossomDefocus2", "femblossomFront", "femblossomTop", "femblossomHand1", "femblossomHand2"}
                aFilesize = New String() {"1.9 MB", "2.4 MB", "2.9 MB", "4 MB", "6.7 MB", "2.4 MB", "2 MB", "4.5 MB", "3.2 MB"}
                directory = "femblossom"
            Case "flo"
                aFilename = New String() {"femblossom1.gif", "femblossom2.gif", "femblossom3.gif", "femblossom5.gif", "femblossom6.gif", "femblossom7.gif"}
                aFilenamesmall = New String() {"femblossomMood", "femblossomBack", "femblossomBottom", "femblossomDefocus", "femblossomDefocus2", "femblossomFront", "femblossomTop", "femblossomHand1", "femblossomHand2"}
                aFilesize = New String() {"26kb", "22kb", "23kb", "24kb", "22kb", "23kb"}
                directory = "webres"
            Case "ihi"
                aFilename = New String() {"isisMood.tif", "isisBack.tif", "isisBottom.tif", "isisDefocus.tif", "isisFront.tif", "isisTop.tif", "isisHand1.tif", "isisHand2.tif"}
                aFilenamesmall = New String() {"isisMood", "isisBack", "isisBottom", "isisDefocus", "isisFront", "isisTop", "isisHand1", "isisHand2"}
                aFilesize = New String() {"2.4 MB", "1.5 MB", "3.7 MB", "3.7 MB", "1.6 MB", "1.8 MB", "2.5 MB", "2.2 MB"}
                directory = "isis"
            Case "ilo"
                aFilename = New String() {"isis1.gif", "isis2.gif", "isis3.gif", "isis5.gif", "isis6.gif", "isis7.gif"}
                aFilenamesmall = New String() {"isisMood", "isisBack", "isisBottom", "isisDefocus", "isisFront", "isisTop", "isisHand1", "isisHand2"}
                aFilesize = New String() {"24kb", "18kb", "24kb", "24kb", "22kb", "30kb"}
                directory = "webres"
            Case "jhi"
                aFilename = New String() {"jasmineMood.tif", "jasmineBack.tif", "jasmineBottom.tif", "jasmineDefocus.tif", "jasmineFront.tif", "jasmineTop.tif", "jasmineHand1.tif", "jasmineHand2.tif"}
                aFilenamesmall = New String() {"jasmineMood", "jasmineBack", "jasmineBottom", "jasmineDefocus", "jasmineFront", "jasmineTop", "jasmineHand1", "jasmineHand2"}
                aFilesize = New String() {"1.3 MB", "3.7 MB", "2.6 MB", "3.2 MB", "2.7 MB", "2 MB", "3 MB", "3.9 MB"}
                directory = "jasmine"
            Case "jlo"
                aFilename = New String() {"jasmine1.gif", "jasmine3.gif", "jasmine4.gif", "jasmine5.gif", "jasmine6.gif", "jasmine7.gif"}
                aFilenamesmall = New String() {"jasmineMood", "jasmineBack", "jasmineBottom", "jasmineDefocus", "jasmineFront", "jasmineTop", "jasmineHand1", "jasmineHand2"}
                aFilesize = New String() {"27kb", "24kb", "25kb", "23kb", "27kb", "24kb"}
                directory = "webres"
            Case "whi"
                aFilename = New String() {"womoliaMood.tif", "womoliaBack.tif", "womoliaBottom.tif", "womoliaDefocus.tif", "womoliaFront.tif", "womoliaTop.tif", "womoliaHand1.tif", "womoliaHand2.tif"}
                aFilenamesmall = New String() {"womoliaMood", "womoliaBack", "womoliaBottom", "womoliaDefocus", "womoliaFront", "womoliaTop", "womoliaHand1", "womoliaHand2"}
                aFilesize = New String() {"1.7 MB", "1.9 MB", "1 MB", "2.6 MB", "2.6 MB", "1.3 MB", "4.2 MB", "2.8 MB"}
                directory = "womolia"
            Case "wlo"
                aFilename = New String() {"womolia1.gif", "womolia2.gif", "womolia3.gif", "womolia4.gif", "womolia5.gif", "womolia6.gif", "womolia7.gif"}
                aFilenamesmall = New String() {"womoliaMood", "womoliaBack", "womoliaBottom", "womoliaDefocus", "womoliaFront", "womoliaTop", "womoliaHand1", "womoliaHand2"}
                aFilesize = New String() {"27kb", "20kb", "20kb", "21kb", "21kb", "21kb", "21kb"}
                directory = "webres"
            Case "lhi"
                aFilename = New String() {"twobottles.tif"}
                aFilenamesmall = New String() {"twobottles"}
                aFilesize = New String() {"18.8 MB"}
                directory = "lubricants"
            Case "llo"
                aFilename = New String() {"lubricantSilicon.jpg", "lubricantWater.jpg"}
                aFilenamesmall = New String() {"twobottles"}
                aFilesize = New String() {"6kb", "6kb"}
                directory = "webres"
            Case "logos"
                aFilename = New String() {"EBLogo.tif", "EBText.jpg", "EBText_SMALL.jpg", "EBText_EPS.eps"}
                aFilenamesmall = New String() {"EBLogo", "EBText", "EBText_SMALL", "EBText_EPS"}
                aFilesize = New String() {"3.2 MB", "7.7 MB", "290kb", "400kb"}
                directory = "logos"
            Case "julia"
                aFilename = New String() {"juliacole.jpg"}
                aFilenamesmall = New String() {"juliacole"}
                aFilesize = New String() {"2.5 MB"}
                directory = "julia"
        End Select
        'show instructions
        If directory = "webres" Then
            lblInstructions.Text = "Right click on image and select 'Save Picture As...' to download the image"
        Else
            lblInstructions.Text = "Right click on image and select 'Save Target As...' or 'Save Link As...' to download the high res image"
        End If
        'show in table
        Dim iCount As Integer = 0
        Dim tRow As New TableRow
        Dim tCell As TableCell
        For iLoop As Integer = 0 To UBound(aFilename)
            If iCount = 3 Or (iCount = 1 And directory = "webres") Then
                tblImages.Rows.Add(tRow)
                tRow = New TableRow
                iCount = 0
            End If
            tCell = New TableCell
            iCount = iCount + 1
            tCell.HorizontalAlign = HorizontalAlign.Center
            tCell.Font.Name = "arial"
            If directory = "webres" Then
                tCell.Text = "<img border='1' src='images/" & directory & "/" & aFilename(iLoop) & "'></a><br>" & aFilesize(iLoop)
            Else
                tCell.Text = "<a href='images/" & directory & "/" & aFilename(iLoop) & "'><img border='1' src='images/" & directory & "/" & aFilenamesmall(iLoop) & "_s.jpg'></a><br>" & aFilesize(iLoop)
            End If
            tCell.BorderWidth = 2
            tCell.BorderColor = Color.Blue
            tRow.Cells.Add(tCell)
        Next
        tblImages.Rows.Add(tRow)
    End Sub
End Class
