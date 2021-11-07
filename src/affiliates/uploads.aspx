<%@ Page Title="Uploads" Language="VB" MasterPageFile="~/maffs.master" AutoEventWireup="false" CodeFile="uploads.aspx.vb" Inherits="affiliates_uploads" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Click the 'Browse' button and select a document to upload.<br />
    Then choose which group(s) the document is intended for, and click 'Upload'.<br /><br />
    <table border="0" width="100%">
        <tr>
            <td valign="top" width="50%">
                <asp:Panel ID="panUpload" runat="server">
                    <table border="0">
                        <tr>
                            <td colspan="2">
                                <asp:FileUpload ID="fu1" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Affiliates
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkAff" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Distributors
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkDist" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Wholesalers
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkWhole" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Country Reps
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkRep" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Press
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkPress"  runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Press (Public)
                            </td>
                            <td align="right">
                                <asp:CheckBox ID="chkPressPublic"  runat="server" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td valign="top">
                                Document Description:
                            </td>
                            <td>
                                <asp:TextBox ID="txtDecription" runat="server" TextMode="MultiLine" Width="300" Height="130" CssClass="normaltextarea"></asp:TextBox>
                            </td>
                        </tr>            
                    </table>
                    <asp:Button ID="btnUpload" runat="server" Text="Upload" OnClick="btnUpload_click" />
                </asp:Panel>   
            </td>
            <td valign="top">
                <!-- Gridview to make documetns Un/Active -->
                <asp:DropDownList ID="drpAffTypes" runat="server" AutoPostBack="true">
                    <asp:ListItem Text="Affiliates" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Distributors" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Wholesalers" Value="3"></asp:ListItem>
                    <asp:ListItem Text="Country Reps" Value="4"></asp:ListItem>
                    <asp:ListItem Text="Press" Value="6"></asp:ListItem>
                    <asp:ListItem Text="Press (Public)" Value="7"></asp:ListItem>
                </asp:DropDownList>
                <asp:GridView ID="gvDocs" runat="server" AutoGenerateColumns="false" DataSourceID="sqlDocs" DataKeyNames="documentID" Width="100%">
                    <RowStyle Font-Size="X-Small" />
                    <Columns>
                        <asp:BoundField HeaderText="Document" DataField="filename" />
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("description") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Uploaded" DataField="uploaddate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:TemplateField HeaderText="Active" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:CheckBox ID="chkActive" runat="server" AutoPostBack="true" Checked='<%# Eval("active") %>' OnCheckedChanged="chkActive_checkChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>                
            </td>
        </tr>
    </table>
     
    <br />
    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    
    <asp:SqlDataSource ID="sqlDocs" runat="server" SelectCommand="procDocumentByTypeIDSelectAll" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="drpAffTypes" PropertyName="selectedValue" Name="affTypeID" Type="Int16" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCOuntry" runat="server" SelectCommand="procCountrysSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
</asp:SqlDataSource>
</asp:Content>

