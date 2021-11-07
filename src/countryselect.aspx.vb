
Partial Class countryselect
    Inherits System.Web.UI.Page

    Private Sub countryselect_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim country As String = Request.QueryString("country")
        Select Case country
            Case "gb"
                Session("EBShopCountry") = "gb"
                Response.Redirect("http://gb.emotionalbliss.com/newHomeIntro.aspx?country=gb")
            Case "EE"
                Session("EBShopCountry") = "EE"
                Response.Redirect("http://ee.emotionalbliss.com/newHomeIntro.aspx?country=EE")
            Case "lt"
                Session("EBShopCountry") = "lt"
                Response.Redirect("http://lt.emotionalbliss.com/newHomeIntro.aspx?country=lt")
            Case "SI"
                Session("EBShopCountry") = "SI"
                Response.Redirect("http://si.emotionalbliss.com/newHomeIntro.aspx?country=SI")
            Case "us"
                Session("EBShopCountry") = "us"
                Response.Redirect("http://us.emotionalbliss.com/newHomeIntro.aspx?country=us")
            Case "fi"
                Session("EBShopCountry") = "fi"
                Response.Redirect("http://fi.emotionalbliss.com/newHomeIntro.aspx?country=fi")
            Case "lu"
                Session("EBShopCountry") = "lu"
                Response.Redirect("http://lu.emotionalbliss.com/newHomeIntro.aspx?country=lu")
            Case "ZA"
                Session("EBShopCountry") = "ZA"
                Response.Redirect("http://za.emotionalbliss.com/newHomeIntro.aspx?country=ZA")
            Case "au"
                Session("EBShopCountry") = "au"
                Response.Redirect("http://au.emotionalbliss.com/newHomeIntro.aspx?country=au")
            Case "fr"
                Session("EBShopCountry") = "fr"
                Response.Redirect("http://fr.emotionalbliss.com/newHomeIntro.aspx?country=fr")
            Case "MT"
                Session("EBShopCountry") = "MT"
                Response.Redirect("http://mt.emotionalbliss.com/newHomeIntro.aspx?country=MT")
            Case "es"
                Session("EBShopCountry") = "es"
                Response.Redirect("http://es.emotionalbliss.com/newHomeIntro.aspx?country=es")
            Case "at"
                Session("EBShopCountry") = "at"
                Response.Redirect("http://at.emotionalbliss.com/newHomeIntro.aspx?country=at")
            Case "de"
                Session("EBShopCountry") = "de"
                Response.Redirect("http://de.emotionalbliss.com/newHomeIntro.aspx?country=de")
            Case "nl"
                Session("EBShopCountry") = "nl"
                Response.Redirect("http://nl.emotionalbliss.com/newHomeIntro.aspx?country=nl")
            Case "se"
                Session("EBShopCountry") = "se"
                Response.Redirect("http://se.emotionalbliss.com/newHomeIntro.aspx?country=se")
            Case "be"
                Session("EBShopCountry") = "be"
                Response.Redirect("http://be.emotionalbliss.com/newHomeIntro.aspx?country=be")
            Case "GR"
                Session("EBShopCountry") = "GR"
                Response.Redirect("http://gr.emotionalbliss.com/newHomeIntro.aspx?country=GR")
            Case "no"
                Session("EBShopCountry") = "no"
                Response.Redirect("http://no.emotionalbliss.com/newHomeIntro.aspx?country=no")
            Case "ch"
                Session("EBShopCountry") = "ch"
                Response.Redirect("http://ch.emotionalbliss.com/newHomeIntro.aspx?country=ch")
            Case "BG"
                Session("EBShopCountry") = "BG"
                Response.Redirect("http://bg.emotionalbliss.com/newHomeIntro.aspx?country=BG")
            Case "HU"
                Session("EBShopCountry") = "HU"
                Response.Redirect("http://hu.emotionalbliss.com/newHomeIntro.aspx?country=HU")
            Case "NZ"
                Session("EBShopCountry") = "NZ"
                Response.Redirect("http://nz.emotionalbliss.com/newHomeIntro.aspx?country=NZ")
            Case "ca"
                Session("EBShopCountry") = "ca"
                Response.Redirect("http://ca.emotionalbliss.com/newHomeIntro.aspx?country=ca")
            Case "is"
                Session("EBShopCountry") = "is"
                Response.Redirect("http://is.emotionalbliss.com/newHomeIntro.aspx?country=is")
            Case "PL"
                Session("EBShopCountry") = "PL"
                Response.Redirect("http://pl.emotionalbliss.com/newHomeIntro.aspx?country=PL")
            Case "CY"
                Session("EBShopCountry") = "CY"
                Response.Redirect("http://cy.emotionalbliss.com/newHomeIntro.aspx?country=CY")
            Case "ie"
                Session("EBShopCountry") = "ie"
                Response.Redirect("http://ie.emotionalbliss.com/newHomeIntro.aspx?country=ie")
            Case "pt"
                Session("EBShopCountry") = "pt"
                Response.Redirect("http://pt.emotionalbliss.com/newHomeIntro.aspx?country=pt")
            Case "cz"
                Session("EBShopCountry") = "cz"
                Response.Redirect("http://cz.emotionalbliss.com/newHomeIntro.aspx?country=cz")
            Case "it"
                Session("EBShopCountry") = "it"
                Response.Redirect("http://it.emotionalbliss.com/newHomeIntro.aspx?country=it")
            Case "RO"
                Session("EBShopCountry") = "RO"
                Response.Redirect("http://ro.emotionalbliss.com/newHomeIntro.aspx?country=RO")
            Case "dk"
                Session("EBShopCountry") = "dk"
                Response.Redirect("http://dk.emotionalbliss.com/newHomeIntro.aspx?country=dk")
            Case "LV"
                Session("EBShopCountry") = "LV"
                Response.Redirect("http://lv.emotionalbliss.com/newHomeIntro.aspx?country=LV")
            Case "SK"
                Session("EBShopCountry") = "SK"
                Response.Redirect("http://sk.emotionalbliss.com/newHomeIntro.aspx?country=SK")
            Case Else
                If Session("EBShopCountry") = "" Then
                    Session("EBShopCountry") = "gb"
                End If
                Exit Select
        End Select

    End Sub
End Class
