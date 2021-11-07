Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Data
Imports System.Data.SqlClient
Imports siteInclude

Partial Class pfa
    Inherits System.Web.UI.Page
    Private dt As DataTable
    Private Const maxImgHeight As Integer = 300
    Private Const maxImgWidth As Integer = 400

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        dt = getData()
        If Not Page.IsPostBack Then
            '1st Run, grab maxRecords and store them in hidden fields on aspx page
            hidMax.Value = dt.Rows.Count.ToString
            hidPos.Value = 0
            addPopupCode(dt.Rows(hidPos.Value)("image"))
            imgPFA.ImageUrl = "~/images/pfaAwards/" & getThumbName(dt.Rows(hidPos.Value)("image"))
        End If
        lblError.Text = ""
    End Sub
    Protected Sub btnBack_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        If hidPos.Value = 0 Then hidPos.Value = hidMax.Value
        hidPos.Value = CInt(hidPos.Value) - 1
        addPopupCode(dt.Rows(hidPos.Value)("image"))
        imgPFA.ImageUrl = "~/images/pfaAwards/" & getThumbName(dt.Rows(hidPos.Value)("image"))
    End Sub
    Protected Sub btnNext_click(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        hidPos.Value = CInt(hidPos.Value) + 1
        If hidPos.Value = hidMax.Value Then hidPos.Value = 0
        addPopupCode(dt.Rows(hidPos.Value)("image"))
        imgPFA.ImageUrl = "~/images/pfaAwards/" & getThumbName(dt.Rows(hidPos.Value)("image"))
    End Sub
    Protected Function getData() As DataTable
        Dim dt As New DataTable
        If Cache("EBPFAImages") Is Nothing Then
            Dim oConn As New SqlConnection(ConfigurationManager.ConnectionStrings("connectionString").ConnectionString)
            Dim oCmd As New SqlCommand("procPFAImagesSelect", oConn)
            Dim da As New SqlDataAdapter
            Dim ds As New DataSet
            oCmd.CommandType = CommandType.StoredProcedure
            Try
                If oCmd.Connection.State = 0 Then oCmd.Connection.Open()
                da = New SqlDataAdapter(oCmd)
                da.Fill(ds)
                If ds.Tables(0).Rows.Count > 0 Then
                    dt = ds.Tables(0)
                    Cache.Add("EBPFAImages", dt, Nothing, Caching.Cache.NoAbsoluteExpiration, New TimeSpan(0, 1, 0), CacheItemPriority.Normal, Nothing)
                End If
            Catch ex As Exception
                lblError.Text = "<font color='red'>An error occured whle retreiving images from database.<br>Sorry for any inconvenience, please try later."
                Dim si As New siteInclude
                si.addError("pfa.aspx.vb", "getData(); " & ex.ToString)
                si = Nothing
            End Try
        Else
            dt = CType(Cache("EBPFAImages"), DataTable)
        End If
        Return dt
    End Function
    Protected Sub setImageSize(ByVal imgURL As String)
        Dim bError As Boolean = False
        Dim bmp As Bitmap
        Try
            bmp = New Bitmap(Page.MapPath(CStr("~/images/pfaAwards/" & imgURL)), False)
        Catch ex As Exception
            bError = True
            lblError.Text = "<font color='red'>Image not Found</font>"
        End Try
        If Not bError Then
            Dim imgHeight As Integer = bmp.Height
            Dim imgWidth As Integer = bmp.Width
            Dim aspect As Decimal = imgWidth / imgHeight
            If aspect < 1.33 Then
                'Image is higher than normal 4:3 image
                imgPFA.Height = maxImgHeight
                imgPFA.Width = maxImgHeight * aspect
            Else
                'Image is wider than normal 4:3 image
                imgPFA.Width = maxImgWidth
                imgPFA.Height = maxImgWidth / aspect
            End If
            bmp.Dispose()
        End If
    End Sub
    Protected Sub addPopupCode(ByVal imgURL As String)
        'Add popup code
        imgPFA.Attributes.Add("onclick", "window.open('images/pfaAwards/" & imgURL & "','imgPop','');")
        imgPFA.Attributes.Add("style", "cursor:pointer;")
    End Sub
    Protected Function getThumbName(ByVal imgName As String)
        Dim result As String = ""
        Dim arr As Array = Split(imgName, ".")
        Try
            result = arr(0) & "_s." & arr(1)
        Catch ex As Exception
            result = "notfound.jpg"
        End Try
        Return result
    End Function
End Class
