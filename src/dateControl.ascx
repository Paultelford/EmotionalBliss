<%@ Control Language="C#" AutoEventWireup="true" CodeFile="dateControl.ascx.cs" Inherits="dateControl" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:HiddenField ID="hidSelected" runat="server" />
<table id="dateTable">
    <tr id="tblRowMonth" runat="server" visible="false">
        <td valign="top">
            <asp:label ID="lblMonthText" runat="server" Text="Month:"></asp:label>
        </td>
        <td>
            <asp:LinkButton ID="lnkMonth" runat="server" OnClick="lnkMonth_click"></asp:LinkButton>
            <asp:Calendar ID="calMonth" runat="server" Visible="false" OnSelectionChanged="calMonth_selectionChanged"></asp:Calendar>
        </td>        
    </tr>
    <tr id="tblRowStartDate" runat="server">
        <td valign="top">
            <!-- del --><asp:LinkButton ID="lnkStartDateText" runat="server" Text="Start Date:" OnClick="lnkStartDateText_click" Visible="false"></asp:LinkButton>
            <asp:Label ID="lblStartDate" runat="server" Text="Start Date:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtStartDate" runat="server" Text='<%# Bind("date","{0:d MMMM yyyy}") %>' MaxLength="50" OnTextChanged="txtStartDate_textChanged" AutoPostBack="true" /><img alt="Icon" src="/Images/Calendar_scheduleHS.png" id="imgCalStart" />
            <ajaxToolkit:CalendarExtender id="calStart" runat="Server" TargetControlID="txtStartDate" Animated="true" Format="d MMMM yyyy" PopupButtonID="imgCalStart"></ajaxToolkit:CalendarExtender>
            <!-- del --><asp:LinkButton ID="lnkStartDate" runat="server" OnClick="lnkStartDate_click" Visible="false"></asp:LinkButton>
            <!-- del --><asp:Calendar ID="calStart_del" runat="server" Visible="false" FirstDayOfWeek="Sunday"  Font-Size="small"></asp:Calendar>
        </td>
    </tr>
    <tr id="tblRowEndDate" runat="server">
        <td valign="top">
            <!-- del --><asp:LinkButton ID="lnkEndDateText" runat="server" Text="End Date:" OnClick="lnkEndDateText_click" Visible="false"></asp:LinkButton>
            <asp:Label ID="lblEndDate" runat="server" Text="End Date:"></asp:Label>
        </td>
        <td>
            <asp:TextBox ID="txtEndDate" runat="server" Text='<%# Bind("date","{0:d MMMM yyyy}") %>' MaxLength="50" OnTextChanged="txtEndDate_textChanged" AutoPostBack="true" /><img alt="Icon" src="/Images/Calendar_scheduleHS.png" id="imgCalEnd" />
            <ajaxToolkit:CalendarExtender id="calEnd" runat="Server" TargetControlID="txtEndDate" Animated="true" Format="d MMMM yyyy" PopupButtonID="imgCalEnd"></ajaxToolkit:CalendarExtender>
            <!-- del --><asp:LinkButton ID="lnkEndDate" runat="server" OnClick="lnkEndDate_click" Visible="false"></asp:LinkButton>
            <!-- del --><asp:Calendar ID="calEnd_del" runat="server" Visible="false" FirstDayOfWeek="Sunday" ></asp:Calendar>
        </td>
    </tr>
    <tr>
        <td colspan="2" align="center">
            <table id="tblControls" runat="Server" visible="false">
                <tr>
                    <td width="20">
                        <asp:LinkButton ID="lnkYearPrevNav" runat="server" ToolTip="Previous Year" Text="<" OnClick="lnkYearPrevNav_click"></asp:LinkButton>
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkJan" runat="server" Text="J" ToolTip="January" OnClick="lnkMonthNav_click" CommandArgument="1"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkFeb" runat="server" Text="F" ToolTip="February" OnClick="lnkMonthNav_click" CommandArgument="2"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkMar" runat="server" Text="M" ToolTip="March" OnClick="lnkMonthNav_click" CommandArgument="3"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkApr" runat="server" Text="A" ToolTip="April" OnClick="lnkMonthNav_click" CommandArgument="4"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkMay" runat="server" Text="M" ToolTip="May" OnClick="lnkMonthNav_click" CommandArgument="5"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkJun" runat="server" Text="J" ToolTip="June" OnClick="lnkMonthNav_click" CommandArgument="6"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkJul" runat="server" Text="J" ToolTip="July" OnClick="lnkMonthNav_click" CommandArgument="7"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkAug" runat="server" Text="A" ToolTip="August" OnClick="lnkMonthNav_click" CommandArgument="8"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkSep" runat="server" Text="S" ToolTip="September" OnClick="lnkMonthNav_click" CommandArgument="9"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkOct" runat="server" Text="O" ToolTip="October" OnClick="lnkMonthNav_click" CommandArgument="10"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkNov" runat="server" Text="N" ToolTip="November" OnClick="lnkMonthNav_click" CommandArgument="11"></asp:LinkButton>        
                    </td>
                    <td align="center">
                        <asp:LinkButton ID="lnkDec" runat="server" Text="D" ToolTip="December" OnClick="lnkMonthNav_click" CommandArgument="12"></asp:LinkButton>        
                    </td>
                    <td width="20" align="right">
                        <asp:LinkButton ID="lnkYearNextNav" runat="server" Text=">" ToolTip="Next Year" OnClick="lnkYearNextNav_click"></asp:LinkButton>
                    </td>
                </tr>
            </table>&nbsp;
        </td>
    </tr>
</table> 
<asp:Label ID="lblError" runat="server"></asp:Label>