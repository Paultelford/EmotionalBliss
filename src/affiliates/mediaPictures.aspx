<%@ Page Title="" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="mediaPictures.aspx.vb" Inherits="affiliates_mediaPictures" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <br />
    <asp:label ID="lblInstructions" runat="server" Text="" Visible="true"></asp:label>
    <br /><br />
    <table width="100%">
        <tr>
            <td valign="top">
                <!-- Menu -->
                <a href="mediaPictures.aspx?clo">Chandra LoRes</a><br /><br />
                <a href="mediaPictures.aspx?chi">Chandra HiRes</a><br /><br />
                <a href="mediaPictures.aspx?flo">Femblossom LoRes</a><br /><br />
                <a href="mediaPictures.aspx?fhi">Femblossom HiRes</a><br /><br />
                <a href="mediaPictures.aspx?ilo">Isis LoRes</a><br /><br />
                <a href="mediaPictures.aspx?ihi">Isis HiRes</a><br /><br />
                <a href="mediaPictures.aspx?wlo">Womolia LoRes</a><br /><br />
                <a href="mediaPictures.aspx?whi">Womolia HiRes</a><br /><br />
                <a href="mediaPictures.aspx?llo">Lubricants LoRes</a><br /><br />
                <a href="mediaPictures.aspx?lhi">Lubricants LoRes</a><br /><br />
                <a href="mediaPictures.aspx?logos">Logos</a><br /><br />
                <a href="mediaPictures.aspx?julia">Julia Cole</a><br /><br />
            </td>
            <td width="30">&nbsp;</td>
            <td valign="top">
                <!-- Images rendered here -->
                <asp:Table BorderWidth="0" id="tblImages" runat="server" CellSpacing="8">
                </asp:Table>
            </td>                       
        </tr>
    </table>
    
</asp:Content>

