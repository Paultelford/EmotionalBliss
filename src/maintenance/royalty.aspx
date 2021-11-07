<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" Trace="false" AutoEventWireup="false" CodeFile="royalty.aspx.vb" Inherits="maintenance_royalty" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    Current Royalty Earners
    <asp:HiddenField ID="hidAffID" runat="server" Value='<%# Eval("affID") %>' />
    <asp:GridView ID="gvEarners" runat="server" DataKeyNames="affid" DataSourceID="sqlEarners" AutoGenerateColumns="false" SkinID="GridViewRedBG" OnSelectedIndexChanged="gvEarners_selectedIndexChanged">
        <Columns>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("affFirstname") & " " & Eval("affSurname") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="Company" DataField="affCompany" />
            <asp:TemplateField HeaderText="Country">
                <ItemTemplate>
                    <asp:Label ID="lblCountry" runat="server" Text='<%# showCountry(Eval("countryName"),Eval("affSuperRoyalty")) %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:CheckBoxField HeaderText="Active" DataField="affActive" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEdit" runat="server" Text="Edit Amounts" CommandName="Select"></asp:LinkButton>
                    <!-- Hidden values -->
                    <asp:HiddenField ID="hidCountryCode" runat="server" Value='<%# Eval("affCountryCode") %>' />
                    <asp:HiddenField ID="hidSuperRoyalty" runat="server" Value='<%# Eval("affSuperRoyalty") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:LinkButton ID="lnkEditEarner" runat="server" Text="Edit Earner" CommandName="" onClick="lnkEditEarner_click"></asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>            
        </Columns>
    </asp:GridView>
    <asp:LinkButton ID="lnkAdd" runat="server" Text="Add Earner" OnClick="lnkAdd_click"></asp:LinkButton>
    <asp:Label ID="lblAddInstructions" runat="server" Text="<br>This will create a new Royalty Earner. The earner will be able to log into the affiliates section and see their statement with the username and password that you specify.<br><br>" Visible="false"></asp:Label>
    <asp:DetailsView ID="dvAdd" runat="server" DataSourceID="sqlAddEarner" Visible="false" DefaultMode="Insert" GridLines="None" OnItemInserting="dvAdd_itemInserting" OnItemInserted="dvAdd_itemInserted" AutoGenerateRows="false" OnItemUpdating="dvAdd_itemUpdating" OnItemUpdated="dvAdd_itemUpdated">
        <Fields>
            <asp:TemplateField HeaderText="Title:">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="20" text='<%# Bind("affTitle") %>' Width="40"></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtTitle" runat="server" MaxLength="20" text='<%# Bind("affTitle") %>' Width="40"></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Firstname:">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtFirstname" runat="server" MaxLength="50" text='<%# Bind("affFirstname") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtFirstname" runat="server" MaxLength="50" text='<%# Bind("affFirstname") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Surname:">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtSurname" runat="server" MaxLength="50" text='<%# Bind("affSurname") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtSurname" runat="server" MaxLength="50" text='<%# Bind("affSurname") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Company:">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtCompany" runat="server" MaxLength="100" text='<%# Bind("affCompany") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtCompany" runat="server" MaxLength="100" text='<%# Bind("affCompany") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Address:">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtAdd1" runat="server" MaxLength="50" text='<%# Bind("affAdd1") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAdd1" runat="server" MaxLength="50" text='<%# Bind("affAdd1") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtAdd2" runat="server" MaxLength="50" text='<%# Bind("affAdd2") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAdd2" runat="server" MaxLength="50" text='<%# Bind("affAdd2") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtAdd3" runat="server" MaxLength="50" text='<%# Bind("affAdd3") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAdd3" runat="server" MaxLength="50" text='<%# Bind("affAdd3") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtAdd4" runat="server" MaxLength="50" text='<%# Bind("affAdd4") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAdd4" runat="server" MaxLength="50" text='<%# Bind("affAdd4") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <InsertItemTemplate>
                    <asp:TextBox ID="txtAdd5" runat="server" MaxLength="50" text='<%# Bind("affAdd5") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtAdd5" runat="server" MaxLength="50" text='<%# Bind("affAdd5") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Postcode:">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtPostcode" runat="server" MaxLength="10" text='<%# Bind("affPostcode") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPostcode" runat="server" MaxLength="10" text='<%# Bind("affPostcode") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Country:">
                <InsertItemTemplate>
                    <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="sqlCountry" DataTextField="countryName" DataValueField="countryCode" selectedValue='<%# Bind("affCountryCode") %>' OnDataBinding="drpCountry_dataBinding" AppendDataBoundItems="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqDrpCountry" runat="server" ControlToValidate="drpCountry" ErrorMessage="* Required" Display="Static" ValidationGroup="add"></asp:RequiredFieldValidator>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="sqlCountry" DataTextField="countryName" DataValueField="countryCode" selectedValue='<%# Bind("affCountryCode") %>' OnDataBinding="drpCountry_dataBinding" AppendDataBoundItems="true"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="reqDrpCountry" runat="server" ControlToValidate="drpCountry" ErrorMessage="* Required" Display="Static" ValidationGroup="update"></asp:RequiredFieldValidator>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" text='<%# Bind("affEmail") %>'></asp:TextBox>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" text='<%# Bind("affEmail") %>'></asp:TextBox>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Username:">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtUsername" runat="server" MaxLength="20" Text='<%# Bind("affUsername") %>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqTxtUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="* Required" Display="Dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:Label ID="lblUsername" runat="server" Text='<%# Bind("affUsername") %>'></asp:Label>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Password:">
                <InsertItemTemplate>
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="20" Text='<%# Bind("affPassword") %>'></asp:TextBox>
                    <asp:RequiredFieldValidator ID="reqTxtPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="* Required" Display="Dynamic" ValidationGroup="add"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ud="regTxtPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="add" ValidationExpression="[a-zA-Z-0-9]{5,20}" Display="Static" ErrorMessage="Must be 5 characters minimum"></asp:RegularExpressionValidator>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" MaxLength="20" Text='<%# Bind("affPassword") %>'></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ud="regTxtPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="update" ValidationExpression="[a-zA-Z-0-9]{5,20}" Display="Static" ErrorMessage="Must be 5 characters minimum"></asp:RegularExpressionValidator>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Special Earner:<br /><font size='-2'>(More than 1 country)</font>">
                <InsertItemTemplate>
                    <asp:CheckBox ID="chkSuperRoyalty" runat="server" Checked='<%# Bind("affSuperRoyalty") %>' />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox ID="chkSuperRoyalty" runat="server" Checked='<%# Bind("affSuperRoyalty") %>' />
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <InsertItemTemplate>
                    <asp:Button ID="btnAdd" runat="server" CommandName="Insert" Text="Add Earner" ValidationGroup="add" />
                    <asp:Button ID="btnAddCancel" runat="server" onClick="btnAddCancel_click" Text="Cancel" />
                </InsertItemTemplate>
                <EditItemTemplate>
                    <asp:Button ID="btnUpdateTest" runat="server" Text="Save Changes" OnClick="btnUpdate_click" ValidationGroup="update" />
                    <asp:Button ID="btnUpdateCancel" runat="server" Text="Cancel" OnClick="btnUpdateCancel_click" />
                </EditItemTemplate>
            </asp:TemplateField>
        </Fields>        
    </asp:DetailsView>
    
    <asp:Panel ID="panAmounts" runat="server" Visible="false">
        <br /><br />
        <table>
            <tr>
                <td>
                    <asp:DropDownList ID="drpCountry" runat="server" DataSourceID="sqlCountry" DataTextField="countryName" DataValueField="countryCode" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="drpCountry_dataBinding">
                    </asp:DropDownList>    
                </td>
                <td width="50">&nbsp;</td>
                <td>
                    <asp:RadioButton ID="radRetail" runat="server" GroupName="rad" Text="Retail" />
                    <asp:RadioButton ID="radDistributor" runat="server" GroupName="rad" Text="Distributor" />
                </td>
            </tr>
        </table>
        
        <asp:GridView ID="gvAmounts" runat="server" DataSourceID="sqlAmounts" AutoGenerateColumns="false" SkinID="GridView" ShowFooter="true" OnDataBound="gvAmounts_dataBound">
            <Columns>
                <asp:TemplateField HeaderText="Product">
                    <ItemTemplate>
                        <asp:Label ID="lblProduct" runat="server" Text='<%# Eval("saleName") %>'></asp:Label>
                        <asp:Label ID="lblDistBuyingID" runat="server" Text='<%# Eval("distBuyingID") %>' Visible="false"></asp:Label>
                        <asp:Label ID="lblRetail" runat="server" Text='<%# Eval("retail") %>' Visible="false"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="btnSubmit" runat="server" Text="Save Changes" OnClick="btnSubmit_click" ValidationGroup="amount" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:BoundField ItemStyle-Width="40" />
                <asp:TemplateField HeaderText="Earning(Per product)">
                    <ItemTemplate>
                        <asp:TextBox ID="txtEarning" runat="Server" MaxLength="5" Width="40" Text='<%# showAmount(Eval("amount")) %>' ValidationGroup="amount"></asp:TextBox>%
                        <asp:RangeValidator ID="ranTxtEarning" runat="server" ControlToValidate="txtEarning" Type="Double" MinimumValue="0" MaximumValue="99.99" ValidationGroup="amount" ErrorMessage="* Invalid"></asp:RangeValidator>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>
    <br /><br />
    <asp:Label id="lblError" runat="server" ForeColor="Red"></asp:Label>
    <asp:Label id="lblComplete" runat="server" ForeColor="Red"></asp:Label>
    
    
    <asp:SqlDataSource ID="sqlEarners" runat="server" SelectCommand="procAffiliateRoyaltyEarnersSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlAddEarner" runat="server" InsertCommand="procAffiliateEarnerInsert" InsertCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>" UpdateCommand="procAffiliateByIdEarnerUpdate" UpdateCommandType="StoredProcedure">
        <InsertParameters>
            <asp:Parameter Name="affTitle" Type="String" Size="20" />
            <asp:Parameter Name="affFirstname" Type="String" Size="50" />
            <asp:Parameter Name="affSurname" Type="String" Size="50" />
            <asp:Parameter Name="affCompany" Type="String" Size="100" />
            <asp:Parameter Name="affAdd1" Type="String" Size="50" />
            <asp:Parameter Name="affAdd2" Type="String" Size="50" />
            <asp:Parameter Name="affAdd3" Type="String" Size="50" />
            <asp:Parameter Name="affAdd4" Type="String" Size="50" />
            <asp:Parameter Name="affAdd5" Type="String" Size="50" />
            <asp:Parameter Name="affPostCode" Type="String" Size="10" />
            <asp:Parameter Name="affCountryCode" Type="String" Size="5" />
            <asp:Parameter Name="affEmail" Type="String" Size="100" />
            <asp:Parameter Name="affUsername" Type="String" Size="20" />
            <asp:Parameter Name="affPassword" Type="String" Size="20" />
            <asp:Parameter Name="affSuperRoyalty" Type="Boolean" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="affTitle" Type="String" Size="20" />
            <asp:Parameter Name="affFirstname" Type="String" Size="50" />
            <asp:Parameter Name="affSurname" Type="String" Size="50" />
            <asp:Parameter Name="affCompany" Type="String" Size="100" />
            <asp:Parameter Name="affAdd1" Type="String" Size="50" />
            <asp:Parameter Name="affAdd2" Type="String" Size="50" />
            <asp:Parameter Name="affAdd3" Type="String" Size="50" />
            <asp:Parameter Name="affAdd4" Type="String" Size="50" />
            <asp:Parameter Name="affAdd5" Type="String" Size="50" />
            <asp:Parameter Name="affPostCode" Type="String" Size="10" />
            <asp:Parameter Name="affCountryCode" Type="String" Size="5" />
            <asp:Parameter Name="affEmail" Type="String" Size="100" />
            <asp:Parameter Name="affPassword" Type="String" Size="20" />
            <asp:Parameter Name="affSuperRoyalty" Type="Boolean" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlCountry" runat="server" SelectCommand="procCountrySelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="sqlAmounts" runat="server" SelectCommand="procRoyaltyEarningsByAffIDSelect" SelectCommandType="StoredProcedure" ConnectionString="<%$ ConnectionStrings:connectionString %>">
        <SelectParameters>
            <asp:ControlParameter ControlID="gvEarners" PropertyName="selectedValue" Name="affID" Type="int32" />
            <asp:ControlParameter ControlID="drpCountry" PropertyName="selectedValue" Name="countryCode" type="String" Size="5" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>

