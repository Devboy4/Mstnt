<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddIssuePopUp.aspx.cs" Inherits="CRM_Genel_Issue_AddIssuePopUp" %>

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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />
    <script src="../../../PreLoad.js" type="text/javascript"></script>
    <script src="../../../utils.js" type="text/javascript"></script>
    <%--<script src="../../../crm.js" type="text/javascript"></script>--%>
    <script type="text/javascript" src="./crm_20141215.js?v=8"></script>
    <style type="text/css">
        .style1 {
            width: 86px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <model:DataTable ID="DataTableNotDosya" runat="server" />
            <model:DataTable ID="DTRelatedUsers" runat="server" />
            <asp:HiddenField ID="HiddenID" runat="server" />
            <asp:HiddenField ID="HiddenNowDate" runat="server" />
            <asp:HiddenField ID="HiddenNotId" runat="server" />
            <asp:HiddenField ID="HiddenNewIssueId" runat="server" />
            <asp:HiddenField ID="HiddenPId" runat="server" />
            <asp:HiddenField ID="HiddenFId" runat="server" />
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
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <Image Url="~/images/save.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat">
                        <Image Url="~/images/saveclose.gif" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <hr />
            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="" Width="100%"
                Height="100%">
                <PanelCollection>
                    <dxp:PanelContent runat="server">
                        <table cellspacing="0" cellpadding="0" border="0" style="width: 700px">
                            <tr>
                                <td valign="top" style="width: 100px">
                                    <dxe:ASPxCheckBox runat="server" ID="CheckSendSms" ClientInstanceName="CheckSendSms"
                                        Text="SMS" ReadOnly="true" Checked="false">
                                    </dxe:ASPxCheckBox>
                                </td>
                                <td colspan="3" style="width: 600px">
                                    <asp:ValidationSummary ID="VAS1" runat="server" ShowMessageBox="false" ShowSummary="true"
                                        Font-Size="10px" Font-Bold="true" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 100px">
                                    <span style="color: #CC0000">Gündem Tanýsý</span>
                                </td>
                                <td colspan="3" style="width: 600px">
                                    <dxe:ASPxMemo ID="Description" runat="server" Height="71px" Width="100%">
                                    </dxe:ASPxMemo>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Description"
                                        ErrorMessage="Gündem Tanýsý Boþ Ge&#231;ilemez" Display="None" Text="*" Font-Bold="False"
                                        Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 100px">
                                    <span style="color: #CC0000">Ýlgili Birim</span>
                                </td>
                                <td style="width: 250px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="FirmaID"
                                        ClientInstanceName="cmbFirmaID" EnableCallbackMode="true" CallbackPageSize="15" 
                                        EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                        <ClientSideEvents SelectedIndexChanged="function(s,e) {OnFirmaIDChanged(s);}" />
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FirmaID"
                                        ErrorMessage="Ýlgili Birim Boþ Ge&#231;ilemez" Display="None" Text="*" Font-Bold="False"
                                        Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 100px" valign="top">
                                    <span style="color: #CC0000">Departman</span>
                                </td>
                                <td style="width: 250px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="ProjeID"
                                        ClientInstanceName="cmbProjeID" EnableCallbackMode="true" OnCallback="ProjeID_Callback">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                       <%-- <ClientSideEvents EndCallback="function(s, e) {cmbUserID.PerformCallback(s.GetValue());}"
                                            SelectedIndexChanged="function(s, e) {cmbUserID.PerformCallback(s.GetValue());}" />--%>
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ProjeID"
                                        ErrorMessage="Departman Boþ Ge&#231;ilemez" Display="None" Text="*" Font-Bold="False"
                                        Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="style1">
                                    <span style="color: #CC0000">Gündem Durumu</span>
                                </td>
                                <td style="width: 250px;">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="DurumID"
                                        EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DurumID"
                                        ErrorMessage="Gündem Durumu Boþ Ge&#231;ilemez" Display="None" Text="*" Font-Bold="False"
                                        Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 100px;" valign="top">
                                    <span style="color: #CC0000">Önem Derecesi</span>
                                </td>
                                <td style="width: 250px;" valign="top">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="OnemDereceID"
                                        EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="OnemDereceID"
                                        ErrorMessage="Önem Derecesi Boþ Ge&#231;ilemez" Display="None" Text="*" Font-Bold="False"
                                        Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="style1">
                                    <span style="color: #CC0000">Gündem Ýlgilisi</span>
                                </td>
                                <td style="width: 250px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="UserID"
                                        EnableIncrementalFiltering="True" EnableCallbackMode="true" ClientInstanceName="cmbUserID"
                                        OnCallback="UserID_Callback1">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                        <ClientSideEvents EndCallback="function(s, e) {	s.SetSelectedIndex(1);}" />
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="UserID"
                                        ErrorMessage="Gündem Ýlgilisi Boþ Ge&#231;ilemez" Display="None" Text="*" Font-Bold="False"
                                        Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 100px" valign="top">
                                    <span style="color: #CC0000">Gündem Sýnýf</span>
                                </td>
                                <td valign="top" style="width: 250px;">
                                    <dxe:ASPxComboBox ID="VirusSinifID" Visible="true" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" EnableCallbackMode="True" EnableIncrementalFiltering="True"
                                        ImageFolder="~/App_Themes/Glass/{0}/" ValueType="System.String">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){ OnVirusSinifChanged(s); }" />
                                        <ButtonStyle Cursor="pointer" Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="VirusSinifID"
                                        Display="None" ErrorMessage="Gündem Sýnýf Boþ Geçilemez" Font-Bold="False" Font-Size="10px"
                                        Text="*"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="style1">
                                    <span style="color: #CC0000">Operasyon Süresi (Gün)</span>
                                </td>
                                <td style="width: 250px" valign="top">
                                    <dxe:ASPxSpinEdit Width="75px" runat="server" ID="OperasyonSuresi" ClientInstanceName="SpinOp"
                                        MaxLength="4" MaxValue="9999" MinValue="0" Number="0" NumberType="Integer">
                                        <ClientSideEvents ValueChanged="function(s, e){SetSonGecerlilikValue(s.GetNumber());}" />
                                    </dxe:ASPxSpinEdit>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="OperasyonSuresi"
                                        ErrorMessage="Operasyon Süresi Boþ Ge&#231;ilemez" Display="None" Text="*" Font-Bold="False"
                                        Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 100px" valign="top">
                                    <span style="color: #CC0000">Planlanan Operasyon Tarihi</span>
                                </td>
                                <td style="width: 250px" valign="top">
                                    <dxe:ASPxDateEdit ID="TeslimTarihi" ClientInstanceName="d2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                        <ButtonStyle Cursor="pointer" Width="13px">
                                        </ButtonStyle>                                        
                                    </dxe:ASPxDateEdit>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="TeslimTarihi"
                                        ErrorMessage="Planlanan Operasyon Tarihi Boþ Ge&#231;ilemez" Display="None" Text="*"
                                        Font-Bold="False" Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="style1">Not Baþlýk
                                </td>
                                <td colspan="3" style="width: 600px" valign="top">
                                    <dxe:ASPxTextBox ID="Tanim" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="style1">Dosya Ekle
                                </td>
                                <td colspan="2" style="width: 500px" valign="top">
                                    <asp:FileUpload ID="fileUpload" runat="server" Width="500px" BorderWidth="1px" BorderStyle="Solid" />
                                </td>
                                <td style="width: 100px">
                                    <dxe:ASPxButton Image-Url="~/images/add_16.gif" ID="BtnDosyaEkle" runat="server"
                                        Text="Dosya Ekle" OnClick="BtnDosyaEkle_Click" Width="150px">
                                        <Image Url="~/images/add_16.gif"></Image>
                                    </dxe:ASPxButton>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="style1">Dosyalar
                                </td>
                                <td valign="top" style="width: 600px" colspan="3">
                                    <dxe:ASPxButton Image-Url="~/images/delete_16.gif" ID="BtnDeleteNote" runat="server"
                                        Text="Dosya Sil" OnClick="BtnDeleteNote_Click" Width="150px" Height="20px">
                                        <Image Url="~/images/delete_16.gif"></Image>
                                    </dxe:ASPxButton>
                                    <hr />
                                    <dxwgv:ASPxGridView ID="GridNotDosya" runat="server" AutoGenerateColumns="False"
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DataTableNotDosya"
                                        KeyFieldName="ID" Width="500px">
                                        <Columns>
                                            <dxwgv:GridViewCommandColumn Width="30px" VisibleIndex="0" ButtonType="Image">
                                                <DeleteButton Visible="True" Text="Sil">
                                                    <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                                </DeleteButton>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                                            <dxwgv:GridViewDataColumn FieldName="NotDosyaID" Visible="False" />
                                            <dxwgv:GridViewDataColumn FieldName="NotID" Visible="False" />
                                            <dxwgv:GridViewDataTextColumn Caption="Dosya" Name="NotDosya" Width="25%">
                                                <DataItemTemplate>
                                                    <asp:Literal runat="server" ID="LiteralNotDosya" Text='<%# Eval("Link") %>'></asp:Literal>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataTextColumn>
                                            <dxwgv:GridViewDataColumn Caption="Dosya Boyutu" FieldName="DosyaBoyut" Width="70px" />
                                            <dxwgv:GridViewDataColumn Caption="Boyut Türü" FieldName="BoyutTuru" Width="60px" />
                                            <dxwgv:GridViewDataColumn Caption="Dosya Yolu" FieldName="DosyaYolu" Visible="False" />
                                            <dxwgv:GridViewDataColumn Caption="Oluþturma Tarihi" FieldName="CreationDate" Width="90px" />
                                            <dxwgv:GridViewDataColumn FieldName="AllowedRoles" Visible="false" />
                                            <dxwgv:GridViewDataColumn FieldName="DeniedRoles" Visible="false" />
                                            <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="false" />
                                        </Columns>
                                        <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                        </Images>
                                        <Settings ShowFilterRow="False" ShowStatusBar="Hidden" ShowGroupedColumns="False"
                                            ShowGroupPanel="False" ShowPreview="False" ShowTitlePanel="False" ShowVerticalScrollBar="False" />
                                        <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                            </Header>
                                            <AlternatingRow Enabled="True" />
                                        </Styles>
                                        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                        <SettingsPager PageSize="5" ShowSeparators="True">
                                        </SettingsPager>
                                        <SettingsCustomizationWindow Enabled="True" />
                                        <SettingsText Title="Dosyalar" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                                            ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Kayýt yok" />
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="style1">Gündem Ýlgilileri
                                </td>
                                <td colspan="3">
                                    <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DTRelatedUsers"
                                        Width="100%" ID="grid1" OnCustomCallback="grid1_CustomCallback" OnAfterPerformCallback="grid_AfterPerformCallback"
                                        OnHtmlCommandCellPrepared="grid1_HtmlCommandCellPrepared" ClientInstanceName="grid1">
                                        <SettingsText Title="Gündem ilgilileri..." ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="#"
                                            GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz." />
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
                                                <PropertiesComboBox ValueField="Adi" TextField="Adi" DataSourceID="DSProje" EnableIncrementalFiltering="true"
                                                    ValueType="System.String">
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
    </form>
</body>
</html>
