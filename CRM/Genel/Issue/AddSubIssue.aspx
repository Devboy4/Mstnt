<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/M1.master" CodeFile="AddSubIssue.aspx.cs"
    Inherits="CRM_Genel_Issue_AddSubIssue" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <script type="text/javascript" src="./crm_20141215.js"></script>
        <model:DataTable ID="DataTableNoteDosya" runat="server" />
        <model:DataTable ID="DTRelatedUsers" runat="server" />
        <asp:HiddenField ID="HiddenID" runat="server" />
        <asp:SqlDataSource ID="DSFirma" runat="server" SelectCommand="SELECT IndexId,FirmaName FROM Firma ORDER BY FirmaName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSProje" runat="server" SelectCommand="SELECT IndexId,Adi FROM Proje ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
            ShowSubMenuShadow="False">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem Name="new">
                    <Image Url="~/images/new.gif" />
                    <Template>
                        <table width="50" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 70px;" onclick="JavaScript:location.href='./AddIssue.aspx';">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/new.gif" />Yeni
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni" Visible="false">
                    <Image Url="~/images/savenew.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="saveopen" Text="Kaydet ve Aç">
                    <Image Url="~/images/saveopen.png" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <hr />
        <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="" Width="100%"
            Height="100%">
            <PanelCollection>
                <dxp:PanelContent runat="server">
                    <table cellspacing="1" cellpadding="0" border="0" style="width: 700px">
                        <tr>
                            <td valign="top" style="width: 100px">
                                Ana Gündem
                            </td>
                            <td colspan="3">
                                <dxe:ASPxLabel ID="AnaVirüsBilgisi" runat="server" Font-Bold="true">
                                </dxe:ASPxLabel>
                            </td>
                            <td valign="top" align="left">
                                <dxe:ASPxCheckBox runat="server" ID="CheckSendSms" Text="SMS" Enabled="false" Checked="false">
                                </dxe:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 100px">
                                <span style="color: #CC0000">Gündem Tanýsý</span>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxMemo ID="Description" runat="server" Height="71px" Width="500px">
                                </dxe:ASPxMemo>
                            </td>
                            <td colspan="1" valign="top">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Description"
                                    ErrorMessage="Boþ Ge&#231;ilemez" Font-Bold="False" Font-Size="10px"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 100px">
                                <span style="color: #CC0000">Ýlgili Birim</span>
                            </td>
                            <td style="width: 150px">
                                <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="FirmaID"
                                    ClientInstanceName="cmbFirmaID" EnableCallbackMode="true" CallbackPageSize="15"
                                    EnableIncrementalFiltering="True">
                                    <ButtonStyle Width="13px" Cursor="pointer">
                                    </ButtonStyle>
                                    <ClientSideEvents SelectedIndexChanged="function(s,e) {OnFirmaIDChanged(s);}" />
                                </dxe:ASPxComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FirmaID"
                                    EnableClientScript="False" ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 100px" valign="top">
                                <span style="color: #CC0000">Departman</span>
                            </td>
                            <td style="width: 150px">
                                <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="ProjeID"
                                    ClientInstanceName="cmbProjeID" EnableCallbackMode="true" OnCallback="ProjeID_Callback">
                                    <ButtonStyle Width="13px" Cursor="pointer">
                                    </ButtonStyle>
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                    cmbUserID.PerformCallback(s.GetValue());grid1.PerformCallback('gridbind');}"
                                        EndCallback="function(s, e) {
	                                    cmbUserID.PerformCallback(s.GetValue());grid1.PerformCallback('gridbind');}" />
                                </dxe:ASPxComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ProjeID"
                                    ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 150px">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 100px">
                                <span style="color: #CC0000">Gündem Durumu</span>
                            </td>
                            <td style="width: 150px;">
                                <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="DurumID"
                                    EnableIncrementalFiltering="True">
                                    <ButtonStyle Width="13px" Cursor="pointer">
                                    </ButtonStyle>
                                </dxe:ASPxComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DurumID"
                                    ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 100px;" valign="top">
                                <span style="color: #CC0000">Önem Derecesi</span>
                            </td>
                            <td style="width: 150px;" valign="top">
                                <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="OnemDereceID"
                                    EnableIncrementalFiltering="True">
                                    <ButtonStyle Width="13px" Cursor="pointer">
                                    </ButtonStyle>
                                </dxe:ASPxComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="OnemDereceID"
                                    ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                            </td>
                            <td style="width: 150px; color: #000000;">
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 100px">
                                <span style="color: #CC0000">Gündem Ýlgilisi</span>
                            </td>
                            <td style="width: 150px">
                                <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="UserID"
                                    EnableIncrementalFiltering="True" EnableCallbackMode="true" ClientInstanceName="cmbUserID"
                                    OnCallback="UserID_Callback1">
                                    <ButtonStyle Width="13px" Cursor="pointer">
                                    </ButtonStyle>
                                    <ClientSideEvents EndCallback="function(s, e) {	s.SetSelectedIndex(1);}" />
                                </dxe:ASPxComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="UserID"
                                    ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                            </td>
                            <td valign="top" style="width: 100px">
                                <span style="color: #CC0000">Gündem Sýnýf</span>
                            </td>
                            <td style="width: 150px" valign="top">
                                <dxe:ASPxComboBox runat="server" ValueType="System.String" Visible="true" ImageFolder="~/App_Themes/Glass/{0}/"
                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="VirusSinifID"
                                    EnableIncrementalFiltering="True" EnableCallbackMode="true">
                                    <ButtonStyle Width="13px" Cursor="pointer">
                                    </ButtonStyle>
                                    <ClientSideEvents SelectedIndexChanged="function(s,e){ grid1.PerformCallback('SetUsers|'+s.GetValue()); grid2.PerformCallback('SetUsers|'+s.GetValue()); SetOperationDateByVirussinif(s.GetValue()); }" />
                                </dxe:ASPxComboBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="VirusSinifID"
                                    ErrorMessage="Gündem Sýnýf Boþ Ge&#231;ilemez" Font-Bold="False" Font-Size="10px"></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 100px">
                                <span style="color: #CC0000">Operasyon Süresi (Gün)</span>
                            </td>
                            <td style="width: 150px" valign="top">
                                <dxe:ASPxSpinEdit Width="75px" runat="server" ID="OperasyonSuresi" ClientInstanceName="SpinOp"
                                    MaxLength="4" MaxValue="9999" MinValue="0" Number="0" NumberType="Integer">
                                    <ClientSideEvents ValueChanged="function(s, e){SetSonGecerlilikValue(s.GetNumber());}" />
                                </dxe:ASPxSpinEdit>
                            </td>
                            <td style="width: 100px" valign="top">
                                <span style="color: #CC0000">Planlanan Operasyon Tarihi</span>
                            </td>
                            <td style="width: 150px" colspan="2" valign="top">
                                <dxe:ASPxDateEdit ID="TeslimTarihi" ClientInstanceName="d2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top" style="width: 100px">
                                Alt Gündem Ýlgilileri
                            </td>
                            <td colspan="5">
                                <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DTRelatedUsers"
                                    Width="100%" ID="grid1" OnCustomCallback="grid1_CustomCallback" ClientInstanceName="grid1"
                                    OnHtmlCommandCellPrepared="grid1_HtmlCommandCellPrepared" OnAfterPerformCallback="grid_AfterPerformCallback">
                                    <SettingsText Title="Alt Gündem ilgilileri..." ConfirmDelete="Kayýt silinsin mi?"
                                        EmptyDataRow="#" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz." />
                                    <SettingsPager PageSize="150" ShowSeparators="True">
                                    </SettingsPager>
                                    <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                    </Images>
                                    <SettingsCustomizationWindow Enabled="True" />
                                    <Settings ShowPreview="True" ShowFilterRow="true" ShowGroupedColumns="True" ShowGroupPanel="true"
                                        ShowTitlePanel="True" />
                                    <SettingsLoadingPanel Text="Yükleniyor..." />
                                    <SettingsEditing Mode="Inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                                        PopupEditFormModal="true" PopupEditFormWidth="500px" />
                                    <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                        <AlternatingRow Enabled="True">
                                        </AlternatingRow>
                                        <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                        </Header>
                                    </Styles>
                                    <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="80px"
                                            ButtonType="Image">
                                            <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                                                <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                                            </ClearFilterButton>
                                            <HeaderTemplate>
                                                <input id="Button1" type="button" onclick="grid1.PerformCallback('Select|true');"
                                                    value="+" title="Tümünü Seç" />
                                                <input id="Button2" type="button" onclick="grid1.PerformCallback('Select|false');"
                                                    value="-" title="Tümünü Seçme" />
                                            </HeaderTemplate>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn FieldName="UserID" Visible="False">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="Ýlgili Adý" FieldName="Adi">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataComboBoxColumn GroupIndex="0" Caption="Ýlgili Birim" FieldName="FirmaName">
                                            <PropertiesComboBox ValueField="FirmaName" TextField="FirmaName" DataSourceID="DSFirma"
                                                EnableIncrementalFiltering="true" ValueType="System.String">
                                            </PropertiesComboBox>
                                        </dxwgv:GridViewDataComboBoxColumn>
                                        <dxwgv:GridViewDataComboBoxColumn Caption="Departman" FieldName="ProjeName">
                                            <PropertiesComboBox ValueField="Adi" TextField="Adi" DataSourceID="DSProje" ValueType="System.String"
                                                EnableIncrementalFiltering="true">
                                            </PropertiesComboBox>
                                        </dxwgv:GridViewDataComboBoxColumn>
                                        <dxwgv:GridViewDataColumn FieldName="IsVisible" Visible="false" ShowInCustomizationForm="false"
                                            Width="0">
                                        </dxwgv:GridViewDataColumn>
                                    </Columns>
                                </dxwgv:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </dxp:PanelContent>
            </PanelCollection>
            <HeaderTemplate>
            </HeaderTemplate>
        </dxrp:ASPxRoundPanel>
    </div>
</asp:Content>
