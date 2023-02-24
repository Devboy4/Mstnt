<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" ValidateRequest="false"
    Inherits="CRM_Genel_Issue_edit" %>

<%@ Register Src="~/controls/NotGrid.ascx" TagName="NotGrid" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
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
    <script src="../../crm.js" type="text/javascript"></script>
    <script src="./edit1.js" type="text/javascript"></script>
    <style type="text/css">
        .style1
        {
            height: 17px;
        }
        .style2
        {
            height: 17px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:SqlDataSource ID="DSUser" runat="server" SelectCommand="SELECT UserID, UserName FROM SecurityUsers Where Active=1 ORDER BY UserName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSFirma" runat="server" SelectCommand="SELECT IndexId,FirmaName FROM Firma ORDER BY FirmaName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSProje" runat="server" SelectCommand="SELECT IndexId,Adi FROM Proje ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:HiddenField ID="HiddenID" runat="server" />
        <asp:HiddenField ID="HiddenRelatedPop3Id" runat="server" />
        <asp:HiddenField ID="HiddenTitle" runat="server" />
        <asp:HiddenField ID="HiddenUserIds" runat="server" />
        <asp:HiddenField ID="HiddenGundemDosyaYolu" runat="server" />
        <asp:HiddenField ID="AtananKisiID" runat="server" />
        <model:DataTable ID="DTIssueActivity" runat="server" />
        <model:DataTable ID="DTRelatedUsers" runat="server" />
        <model:DataTable ID="DTAltDosyalar" runat="server" />
        <model:DataTable ID="DTAltGundem" runat="server" />
        <model:DataTable ID="DTBrKodlari" runat="server" />
        <model:DataTable ID="DTPhoneBook" runat="server" />
        <model:DataTable ID="DTSelectedSms" runat="server" />
        <model:DataTable ID="DTUsers" runat="server" />
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
                <dxm:MenuItem Name="NewIssue">
                    <Image Url="~/images/new.gif" />
                    <Template>
                        <table width="150" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="JavaScript:parent.opener.location.href='./AddIssue.aspx';parent.close();">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/Menu_YeniBildirim.gif" />
                                    Yeni Gündem Giriþi
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="newalt">
                    <Image Url="~/images/new.gif" />
                    <Template>
                        <table width="150" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="JavaScript:parent.opener.location.href='./AddSubIssue.aspx?SubId='+form1.HiddenID.value;parent.close();">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/Menu_YeniBildirim.gif" />
                                    Alt Gündem Giriþi
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni">
                    <Image Url="~/images/savenew.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat">
                    <Image Url="~/images/saveclose.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="delete" Text="Sil">
                    <Image Url="~/images/delete.gif" />
                </dxm:MenuItem>
            </Items>
            <ClientSideEvents ItemClick="function(s, e) { Menu_ItemClick(s,e); }" />
        </dxm:ASPxMenu>
        <hr />
        <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" ActiveTabIndex="0" TabSpacing="0px"
            Width="100%">
            <ContentStyle>
                <Border BorderColor="#4986A2" />
            </ContentStyle>
            <TabPages>
                <dxtc:TabPage Name="Genel" Text="Genel">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <table style="width: 100%" cellspacing="1" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td align="left" colspan="4" style="width: 100%">
                                            <asp:ValidationSummary ID="VAS1" runat="server" ShowMessageBox="false" ShowSummary="true"
                                                Font-Size="10px" Font-Names="Arial" Font-Bold="true" ForeColor="Red" Width="214px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <table cellspacing="2" border="0" style="width: 100%">
                                                <tr>
                                                    <td>
                                                        PNR No
                                                    </td>
                                                    <td>
                                                        Oluþturan
                                                    </td>
                                                    <td>
                                                        Oluþturma Tarihi
                                                    </td>
                                                    <td>
                                                        Düzenleyen
                                                    </td>
                                                    <td>
                                                        Düzenlendiði Tarih
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxLabel ID="IndexID" runat="server" Font-Bold="true" ForeColor="Red">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxLabel ID="IssuedBy" runat="server" Font-Bold="true" ForeColor="#000066">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxLabel ID="IssuedDate" runat="server" Font-Bold="true" ForeColor="#000066">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxLabel ID="ModifiedBy" runat="server" Font-Bold="true" ForeColor="#000066">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxLabel ID="ModificationDate" runat="server" Font-Bold="true" ForeColor="#000066">
                                                        </dxe:ASPxLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                            <hr />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" valign="top">
                                            PNR'a Git:
                                        </td>
                                        <td colspan="3" valign="top" style="width: 85%">
                                            <table border="0" style="width: 100%" cellpadding="0" cellspacing="1">
                                                <tr>
                                                    <td style="width: 90%">
                                                        <dxe:ASPxTextBox ID="BildirimNo" Width="100%" runat="server" />
                                                    </td>
                                                    <td style="width: 10%">
                                                        <asp:ImageButton runat="server" ID="ImageButton1" ImageUrl="~/images/Button_Go2.gif"
                                                            BorderWidth="0px" OnClick="BtnGo_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" valign="top">
                                            <dxe:ASPxLabel runat="server" ID="lblsetsubIssue" Text="Ana Gündem">
                                            </dxe:ASPxLabel>
                                        </td>
                                        <td colspan="3" valign="top" style="width: 85%">
                                            <asp:Panel runat="server" ID="tblsetmainissue" Width="100%">
                                                <table border="0" cellpadding="0" cellspacing="1" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <dxe:ASPxComboBox ID="MainIssueId" runat="server" Width="100%" ValueType="System.Int32"
                                                                ClientInstanceName="cmbMainIssueId" CallbackPageSize="15" EnableSynchronization="False"
                                                                EnableCallbackMode="true" OnCallback="MainIssueId_Callback">
                                                                <ClientSideEvents SelectedIndexChanged="function(s, e) {MainIssueIdChanged(s); }"
                                                                    ButtonClick="function(s, e) { ComboButtonClick('cmbMainIssueId'); }" EndCallback="function(s, e) { ComboEndCallback('cmbMainIssueId'); }" />
                                                                <DropDownButton Visible="false">
                                                                </DropDownButton>
                                                                <Buttons>
                                                                    <dxe:EditButton Text="...">
                                                                    </dxe:EditButton>
                                                                </Buttons>
                                                            </dxe:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <a runat="server" id="lnkanavirus" href="#" onclick="LoadMainVirusScreen();">Ana gündem
                                                                için týklayýn...</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            Gündem Tanýsý
                                        </td>
                                        <td colspan="3">
                                            <dxe:ASPxMemo ID="Title" ReadOnly="true" runat="server" Height="100px" Width="100%">
                                            </dxe:ASPxMemo>
                                            <asp:Panel runat="server" ID="MailPanel" Width="100%" Visible="false">
                                                <asp:Literal runat="server" ID="ltrMailContent"></asp:Literal>
                                            </asp:Panel>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Title"
                                                ErrorMessage="Baþlýk Boþ Geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            Ýlgili Atama / Mesaj
                                        </td>
                                        <td valign="top" colspan="3">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td style="width: 80%">
                                                        <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                                            CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="100%"
                                                            ID="UserId" EnableIncrementalFiltering="True">
                                                            <ButtonStyle Width="13px" Cursor="pointer">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="width: 24px">
                                                        <img src="../../../images/comment_private.png" alt="Gizli Uyarý" />
                                                    </td>
                                                    <td style="width: 20%">
                                                        <dxe:ASPxCheckBox ID="IsPersonalizedPost" Enabled="false" runat="server" Checked="false"
                                                            Text="Gizli Uyarý">
                                                        </dxe:ASPxCheckBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            Yorum
                                        </td>
                                        <td colspan="3">
                                            <dxe:ASPxMemo ID="Comments" runat="server" Height="100px" Width="100%">
                                            </dxe:ASPxMemo>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 15%" valign="top">
                                            <span style="color: #CC0000">Gündem Durumu</span>
                                        </td>
                                        <td style="width: 35%">
                                            <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="100%"
                                                ID="DurumID" ClientInstanceName="cmbDurumID" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                </ButtonStyle>
                                                <ClientSideEvents ValueChanged="function(s, e){ if(s.GetValue()=='5'){ cmbHataTipID.SetText('Aspirin'); } }" />
                                            </dxe:ASPxComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DurumID"
                                                ErrorMessage="Gündem Durumu Boþ Geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 15%" valign="top">
                                            <span style="color: #CC0000">Önem Derecesi</span>
                                        </td>
                                        <td style="width: 35%" valign="top">
                                            <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="100%"
                                                ID="OnemDereceID" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                </ButtonStyle>
                                            </dxe:ASPxComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="OnemDereceID"
                                                ErrorMessage="Önem Derecesi Boþ Geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            Gündem Tespit Tarihi
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="BildirimTarihi" ReadOnly="true" runat="server" Width="100%"
                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="BildirimTarihi"
                                                ErrorMessage="Gündem Tespit Tarihi Boþ Geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                        <td valign="top">
                                            Planlanan Operasyon Tarihi
                                        </td>
                                        <td valign="top">
                                            <dxe:ASPxDateEdit ID="TeslimTarihi" Width="100%" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="TeslimTarihi"
                                                ErrorMessage="Planlanan Operasyon Tarihi Boþ Geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            Kapatma Tarihi Gir
                                        </td>
                                        <td>
                                            <dxe:ASPxDateEdit ID="ReelOperationDate" Width="100%" ClientInstanceName="AntivirusTarihi"
                                                runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
                                                ImageFolder="~/App_Themes/Glass/{0}/">
                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                </ButtonStyle>
                                                <ClientSideEvents LostFocus="function(s, e){ if(confirm('Kapatmak istermisiniz?')) {  
                                                cmbDurumID.SetText('KAPALI');
                                                 cmbHataTipID.SetText('Aspirin'); 
                                                }
                                                }" />
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td>
                                        </td>
                                        <td valign="top">
                                            <dxe:ASPxCheckBox ID="AsilamaYapildi" Text="Aþýlama Yapýldý" runat="server" ValueType="System.Int32"
                                                ValueChecked="1" ValueUnchecked="0">
                                            </dxe:ASPxCheckBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <%--<span style="color: #CC0000">Virüs Sýnýfý</span>--%>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" Visible="false" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="100%"
                                                ID="VirusSinifID" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                </ButtonStyle>
                                            </dxe:ASPxComboBox>
                                            <%--  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="VirusSinifID"
                                                ErrorMessage="Virüs Sýnýfý Boþ Geçilemez!" Display="None"></asp:RequiredFieldValidator>--%>
                                        </td>
                                        <td valign="top">
                                            Operasyon Yontemi
                                        </td>
                                        <td valign="top">
                                            <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="100%"
                                                ID="HataTipID" ClientInstanceName="cmbHataTipID" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                </ButtonStyle>
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4" valign="top">
                                            <asp:Panel ID="pnlSantralVoice" runat="server" Visible="false">
                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td style="width: 15%" valign="top">
                                                            Santral Görüþmesi Var
                                                        </td>
                                                        <td style="width: 85%; font-weight: bold; font-size: 12px;" valign="top">
                                                            indirmak için <a href="#" id="lnkSantralVoice" onclick="SetSantralLink();">Týklayýnýz...</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="Users" Text="Gündem Ýlgilileri">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentUsers" runat="server">
                            <table border="0" cellspacing="0" cellpadding="0" style="width: 100%">
                                <tr>
                                    <td valign="top" style="width: 100px">
                                        Gündem Ýlgilileri
                                    </td>
                                    <td>
                                        <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DTUsers"
                                            Width="100%" ID="grd_user1" OnCustomCallback="grd_user1_CustomCallback" OnHtmlCommandCellPrepared="grd_user1_HtmlCommandCellPrepared"
                                            ClientInstanceName="grd_user1" OnAfterPerformCallback="grd_user1_AfterPerformCallback">
                                            <SettingsText Title="Gündem Ýlgilileri" ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="#"
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
                                                        <input id="Button1" type="button" onclick="grd_user1.PerformCallback('Select|true');"
                                                            value="+" title="Tümünü Seç" />
                                                        <input id="Button2" type="button" onclick="grd_user1.PerformCallback('Select|false');"
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
                                                         EnableIncrementalFiltering="true"  ValueType="System.String">
                                                    </PropertiesComboBox>
                                                </dxwgv:GridViewDataComboBoxColumn>
                                                <dxwgv:GridViewDataComboBoxColumn Caption="Departman" FieldName="ProjeName">
                                                    <PropertiesComboBox ValueField="Adi" TextField="Adi" DataSourceID="DSProje"   EnableIncrementalFiltering="true" ValueType="System.String">
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
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="AltGundem" Visible="false" Text="Baðlý Dosyalar">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DTAltDosyalar"
                                Width="100%" ID="grid1" ClientInstanceName="grid1" OnCustomCallback="grid1_CustomCallback">
                                <SettingsText Title="Gündem Dosyalarý" ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
                                <SettingsPager PageSize="15" ShowSeparators="True">
                                </SettingsPager>
                                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                </Images>
                                <SettingsCustomizationWindow Enabled="True" />
                                <Settings ShowPreview="True" ShowTitlePanel="True" />
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
                                    <dxwgv:GridViewCommandColumn Width="90px" VisibleIndex="0" ButtonType="Image">
                                        <DeleteButton Text="Sil" Visible="True">
                                            <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                        </DeleteButton>
                                        <HeaderTemplate>
                                            <center>
                                                <dxe:ASPxImage runat="server" ToolTip="Yeni Link" ID="image1" ImageUrl="~/images/new.gif">
                                                    <ClientSideEvents Click="function(s,e){PopWin = OpenPopupWinBySize('Addlink.aspx?id='+form1.HiddenID.value,600,140);PopWin.focus();}" />
                                                </dxe:ASPxImage>
                                            </center>
                                        </HeaderTemplate>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn FieldName="VirusDosyaYoluID" Visible="False">
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn Width="300px" Caption="Dosya" FieldName="DosyaYolu">
                                        <DataItemTemplate>
                                            <asp:Literal runat="server" ID="DosyaLink" Text='<%#Eval("literal") %>'></asp:Literal>
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn Caption="Ekleyen" Width="75px" FieldName="CreatedBy" EditFormSettings-Visible="False">
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataDateColumn Width="75px" PropertiesDateEdit-EditFormatString="dd.MM.yyyy"
                                        Caption="Oluþturma Tarihi" EditFormSettings-Visible="False" FieldName="CreationDate">
                                        <PropertiesDateEdit EditFormatString="dd.MM.yyyy">
                                        </PropertiesDateEdit>
                                        <EditFormSettings Visible="False"></EditFormSettings>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="Not" Text="Not">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <uc1:NotGrid ID="NotGrid1" runat="server" />
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Name="Sms" Text="SMS">
                    <ContentCollection>
                        <dxw:ContentControl ID="cntrlsms" runat="server">
                            <table border="0" style="width: 400px" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td style="width: 100px" valign="top">
                                        Sms Gönder
                                    </td>
                                    <td valign="top">
                                        <dxe:ASPxCheckBox runat="server" ID="chcsms" Checked="false">
                                        </dxe:ASPxCheckBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" valign="top">
                                        <dxwgv:ASPxGridView ID="GridSms" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                            CssPostfix="Glass" DataSourceID="DTPhoneBook" KeyFieldName="ID" Width="400px"
                                            ClientInstanceName="GridSesEmail" OnRowValidating="GridSms_RowValidating">
                                            <SettingsText Title="Sistem Dýþý Sms Gönderilecekler Listesi" EmptyDataRow="Yeni satýr ekle" />
                                            <SettingsPager PageSize="15" ShowSeparators="True">
                                            </SettingsPager>
                                            <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                            </Images>
                                            <Settings ShowPreview="True" ShowTitlePanel="true" />
                                            <SettingsEditing Mode="inline" />
                                            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                </Header>
                                            </Styles>
                                            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn Width="80px" VisibleIndex="0" ButtonType="Image">
                                                    <NewButton Visible="True" Text="Yeni">
                                                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                                                    </NewButton>
                                                    <UpdateButton Visible="True" Text="Güncelle">
                                                        <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                                                    </UpdateButton>
                                                    <DeleteButton Visible="True" Text="Sil">
                                                        <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                                    </DeleteButton>
                                                    <CancelButton Visible="True" Text="Ýptal">
                                                        <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                                                    </CancelButton>
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                                                <dxwgv:GridViewDataColumn Caption="" Width="25px" Visible="true">
                                                    <DataItemTemplate>
                                                        <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                                    </DataItemTemplate>
                                                    <EditItemTemplate>
                                                        <img id="gsm_ara" src="../../../images/search_button.jpg" alt="" width="22px" height="19px"
                                                            onclick="ComboButtonClick('GridSms_SmsId')" style="cursor: pointer" />
                                                    </EditItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataColumn FieldName="PhoneNumber" Caption="Cep Numarasý" Width="400px"
                                                    ReadOnly="true" />
                                                <dxwgv:GridViewDataColumn FieldName="SmsId" Width="0px" ReadOnly="true" />
                                            </Columns>
                                        </dxwgv:ASPxGridView>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" valign="top">
                                        <%--   <dxwgv:ASPxGridView ID="GridSelectedSms" runat="server" AutoGenerateColumns="False"
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DTSelectedSms"
                                            KeyFieldName="ID" Width="400px" OnCustomCallback="GridSelectedSms_CustomCallback"
                                            ClientInstanceName="GridSelectedSms">
                                            <SettingsText Title="Sistemde Sms Gönderilecekler Listesi" EmptyDataRow="#" />
                                            <SettingsPager PageSize="15" ShowSeparators="True">
                                            </SettingsPager>
                                            <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                            </Images>
                                            <Settings ShowPreview="True" ShowTitlePanel="true" ShowFilterRow="true" />
                                            <SettingsEditing Mode="inline" />
                                            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                </Header>
                                            </Styles>
                                            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                            <Columns>
                                                <dxwgv:GridViewCommandColumn ButtonType="Image" ShowSelectCheckbox="true" VisibleIndex="0"
                                                    Width="80px">
                                                    <HeaderTemplate>
                                                        <input id="Button1" type="button" onclick="GridSelectedSms.PerformCallback(true);"
                                                            value="+" title="Tümünü Seç" />
                                                        <input id="Button2" type="button" onclick="GridSelectedSms.PerformCallback(false);"
                                                            value="-" title="Tümünü Seçme" />
                                                    </HeaderTemplate>
                                                    <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                                                        <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                                                    </ClearFilterButton>
                                                </dxwgv:GridViewCommandColumn>
                                                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                                                <dxwgv:GridViewDataCheckColumn FieldName="IssueAntivirus" Width="50px" Caption="Antivirüs">
                                                    <PropertiesCheckEdit ValueChecked="true" ValueType="System.Boolean" ValueUnchecked="false">
                                                    </PropertiesCheckEdit>
                                                </dxwgv:GridViewDataCheckColumn>
                                                <dxwgv:GridViewDataColumn FieldName="UserName" Caption="Gönderilecek Kullanýcýlar"
                                                    Width="375px" ReadOnly="true" />
                                            </Columns>
                                        </dxwgv:ASPxGridView>--%>
                                    </td>
                                </tr>
                            </table>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
            <TabStyle HorizontalAlign="Center">
            </TabStyle>
            <Paddings PaddingLeft="0px" />
        </dxtc:ASPxPageControl>
        <hr />
        <dxtc:ASPxPageControl ID="ASPxPageControl2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" ActiveTabIndex="0" TabSpacing="0px"
            Width="100%">
            <ContentStyle>
                <Border BorderColor="#4986A2" />
            </ContentStyle>
            <TabPages>
                <dxtc:TabPage Text="Gündem Tarihçesi" Name="TabIsAkisi">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxm:ASPxMenu ID="Menu2" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
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
                                    <dxm:MenuItem Name="AddAttachment">
                                        <Template>
                                            <table width="50" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                                        padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="Grid.PerformCallback('x');">
                                                        <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/save.gif" />Kaydet
                                                    </td>
                                                </tr>
                                            </table>
                                        </Template>
                                    </dxm:MenuItem>
                                </Items>
                            </dxm:ASPxMenu>
                            <%-- <div style="overflow: scroll; width: 750px; height: 200px">--%>
                            <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" DataSourceID="DTIssueActivity" KeyFieldName="ID" Width="100%"
                                ClientInstanceName="Grid" OnCustomCallback="grid_CustomCallback">
                                <SettingsText Title="Gündem Tarihçesi" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                                    ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="#" CustomizationWindowCaption="Kolon Ekle/Çýkart" />
                                <SettingsEditing Mode="Inline" PopupEditFormWidth="750px" PopupEditFormHorizontalOffset="50"
                                    PopupEditFormVerticalOffset="50" />
                                <SettingsPager PageSize="15" ShowSeparators="True">
                                </SettingsPager>
                                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                </Images>
                                <SettingsCustomizationWindow Enabled="True" />
                                <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                                    ShowPreview="True" />
                                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                    <AlternatingRow Enabled="True">
                                    </AlternatingRow>
                                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                    </Header>
                                </Styles>
                                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="40px">
                                        <UpdateButton Text="G&#252;ncelle" Visible="True">
                                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                                        </UpdateButton>
                                        <DeleteButton Text="Sil" Visible="false">
                                            <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                        </DeleteButton>
                                        <EditButton Text="Deðiþtir" Visible="True">
                                            <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                                        </EditButton>
                                        <CancelButton Text="Ýptal" Visible="True">
                                            <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                                        </CancelButton>
                                    </dxwgv:GridViewCommandColumn>
                                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="false" />
                                    <dxwgv:GridViewDataColumn FieldName="Process" Caption="Deðiþiklik">
                                        <DataItemTemplate>
                                            <asp:Literal ID="Process" runat="server" Text='<%# Eval("Process") %>'></asp:Literal>
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataMemoColumn FieldName="Comment" Caption="Açýklama" />
                                    <dxwgv:GridViewDataColumn FieldName="CreatedBy" Caption="Oluþturan" EditFormSettings-Visible="False">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn FieldName="DurumName" Caption="Gündem Durumu" EditFormSettings-Visible="False">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-DisplayFormatString="dd.MM.yyyy"
                                        FieldName="CommentDate" Caption="Ýþlem Tarihi">
                                        <PropertiesDateEdit DisplayFormatString="dd.MM.yyyy HH:mm">
                                        </PropertiesDateEdit>
                                        <EditFormSettings Visible="False" />
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                            <%-- </div>--%>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Alt Gündemler" Name="TabAltvirus">
                    <ContentCollection>
                        <dxw:ContentControl ID="ccAltvirus" runat="server">
                            <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DTAltGundem"
                                Width="100%" ID="grid2" ClientInstanceName="grid2">
                                <SettingsText Title="Alt Gündemler" ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="#" />
                                <SettingsPager PageSize="15" ShowSeparators="True">
                                </SettingsPager>
                                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                </Images>
                                <SettingsCustomizationWindow Enabled="True" />
                                <Settings ShowPreview="True" ShowTitlePanel="True" />
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
                                    <dxwgv:GridViewDataColumn FieldName="ID" ShowInCustomizationForm="false" Visible="False">
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn FieldName="IndexID" Width="60px" Caption="PNR" />
                                    <dxwgv:GridViewDataColumn FieldName="Baslik" Caption="Gündem Tanýsý" Width="200px">
                                        <DataItemTemplate>
                                            <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                                                runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("ID")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                                Text='<%#Eval("Baslik")%>'>
                                            </dxe:ASPxHyperLink>
                                        </DataItemTemplate>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn Caption=" " Width="20px">
                                        <DataItemTemplate>
                                            <img src="../../../images/details.png" alt="Yazýþma Geçmiþini Aç" style="cursor: pointer"
                                                onclick="CallbackPreview.PerformCallback('<%#Eval("IndexId") %>');" />
                                        </DataItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn FieldName="Durum" Caption="Durum">
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Gündem Tespit Tarihi" FieldName="BildirimTarihi">
                                        <HeaderCaptionTemplate>
                                            Tespit
                                            <br />
                                            Tarihi
                                        </HeaderCaptionTemplate>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Planlanan Operasyon Tarihi" FieldName="TeslimTarihi">
                                        <HeaderCaptionTemplate>
                                            Planlanan Opr.<br />
                                            Tarihi
                                        </HeaderCaptionTemplate>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluþturma Tarihi"
                                        EditFormSettings-Visible="False" FieldName="CreationDate">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataDateColumn>
                                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Düzenlenme Tarihi"
                                        EditFormSettings-Visible="False" FieldName="ModificationDate">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
            <TabStyle HorizontalAlign="Center">
            </TabStyle>
            <Paddings PaddingLeft="0px" />
        </dxtc:ASPxPageControl>
        <dxpc:ASPxPopupControl ID="popup" runat="server" AllowDragging="True" AllowResize="True"
            CloseAction="OuterMouseClick" EnableViewState="False" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowFooter="false" ShowHeader="false" Width="600px"
            Height="300px" FooterText="Paneli sað alt köþesinden tutup boyutlandýrabilirsiniz..."
            HeaderText="Gündem Tarihçesi" ClientInstanceName="FeedPopupControl" EnableHierarchyRecreation="True"
            DragElement="Window" Modal="false">
            <Windows>
                <dxpc:PopupWindow HeaderText="Gündem Tarihçesi" Modal="false" Name="Preview">
                </dxpc:PopupWindow>
            </Windows>
        </dxpc:ASPxPopupControl>
        <dxcb:ASPxCallback ID="CallbackPreview" ClientInstanceName="CallbackPreview" OnCallback="CallbackPreview_Callback"
            runat="server">
            <ClientSideEvents CallbackComplete="function(s, e) {   var win = FeedPopupControl.GetWindow(0);
                    FeedPopupControl.SetWindowContentUrl(win,'Preview.aspx?id='+e.result);
                    FeedPopupControl.ShowWindow(win);}" />
        </dxcb:ASPxCallback>
        <dxcb:ASPxCallback ID="CallbackSearchBrowser" runat="server" ClientInstanceName="CallbackSearchBrowser"
            OnCallback="CallbackSearchBrowser_Callback">
            <ClientSideEvents CallbackComplete="function(s, e) { SearchBrowserCallbackComplete(e); }"
                EndCallback="function(s, e) { SearchBrowserEndCallback(); }" />
        </dxcb:ASPxCallback>
        <dxcb:ASPxCallback ID="CallbackGenel" runat="server" ClientInstanceName="CallbackGenel"
            OnCallback="CallbackGenel_Callback">
            <ClientSideEvents CallbackComplete="function(s, e) {CallbackGenelComplete(e.result);}" />
        </dxcb:ASPxCallback>
    </div>
    </form>
</body>
</html>
