<%@ Page Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="scanExtra.aspx.vb" Inherits="affiliates_scanExtra" title="Untitled Page" %>
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="atlas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <atlas:ScriptManagerProxy ID="smp1" runat="server"></atlas:ScriptManagerProxy>
    <atlas:UpdateProgress ID="up1" runat="server" DynamicLayout="false" AssociatedUpdatePanelID="update1">
        <ProgressTemplate>
            Please Wait....<img src="/images/loading.gif" width="16" height="16" />
        </ProgressTemplate>
    </atlas:UpdateProgress>    
    <atlas:UpdatePanel ID="update1" runat="server">
        <ContentTemplate>
            There are <asp:Label ID="lblQtyLeft" runat="server"></asp:Label> packages left to scan.
            <br /><br />
            <asp:Label ID="lblTrackerText" runat="server" Text="Tracker Number: "></asp:Label>
            <asp:TextBox ID="txtTracker" MaxLength="20" runat="server" OnTextChanged="txtTracker_textChanged" AutoPostBack="true"></asp:TextBox>
            <asp:HyperLink ID="lnkBack" runat="Server" Text="Back to Scan page" NavigateUrl="~/affiliates/scan.aspx" Visible="false"></asp:HyperLink>
            <br /><br /><br />
            <asp:Label ID="lblScanned" runat="server"></asp:Label><br />
            <asp:Label ID="lblError" runat="server"></asp:Label>
            <script langauge="javascript" type="text/javascript">
                function focusElement(e){
                    //document.getElementById("").focus();
                    self.setTimeout("document.getElementById('" + e + "').focus();",600);
                }
            </script>
        </ContentTemplate>
    </atlas:UpdatePanel>
</asp:Content>

