<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="ithalat_siparis.aspx.cs"
    Inherits="CRM_IthalatTakip_Rapor_ithalat_siparis" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="./ithalat_siparis.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function ShowHideColonWindow() {
            if (Grid.IsCustomizationWindowVisible())
                Grid.HideCustomizationWindow();
            else
                Grid.ShowCustomizationWindow();
        }
    </script>
    <model:DataTable ID="DTRapor" runat="server" />
    <model:DataTable ID="DTRapor2" runat="server" />
    <dxcb:ASPxCallback ID="CallbackSearchBrowser" runat="server" ClientInstanceName="CallbackSearchBrowser"
        OnCallback="CallbackSearchBrowser_Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { SearchBrowserCallbackComplete(e); }"
            EndCallback="function(s, e) { SearchBrowserEndCallback(); }" />
    </dxcb:ASPxCallback>
    <div>
        <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
            ShowSubMenuShadow="False" AutoPostBack="True">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem>
                    <Template>
                        <table width="50" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItem_Blue" style="cursor: pointer;" onclick="GridPerformCallback(1);">
                                    <b>SORGULA</b>
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <%--                    <dxm:MenuItem>
                        <Template>
                            <table width="50" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="dxmMenuItem_Blue" style="cursor: pointer;" onclick="javascript:ShowHideColonWindow();">
                                        <b>Kolon Penceresini Aç/Kapa</b>
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="excel" Text="EXCEL" ToolTip="EXCEL olarak kaydet">
                        <TextTemplate>
                            EXCEL</TextTemplate>
                        <Image Url="~/images/xls_ico.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="pdf" Text="PDF" ToolTip="PDF olarak kaydet">
                        <TextTemplate>
                            PDF</TextTemplate>
                        <Image Url="~/images/pdf_icon.gif" />
                    </dxm:MenuItem>--%>
            </Items>
        </dxm:ASPxMenu>
        <hr />
        <table border="0" cellpadding="1" cellspacing="1" width="100%" style="border-style: outset;">
            <tr>
                <td colspan="1" style="width: 60px">
                    <dxe:ASPxLabel ID="lblFirmaId" runat="server" Text="Firma" ForeColor="Black" />
                </td>
                <td colspan="1" style="width: 400px">
                    <dxe:ASPxComboBox ID="FirmaId" runat="server" Width="395px" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="FirmaId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="FirmaId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {FirmaIdChanged(s); }" ButtonClick="function(s, e) { ComboButtonClick('FirmaId'); }"
                            EndCallback="function(s, e) { ComboEndCallback('FirmaId'); }" />
                        <DropDownButton Visible="false">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1" style="width: 130px">
                    <dxe:ASPxLabel ID="lblSiparisTarihi1" runat="server" ForeColor="Black" Text="Ýlk Sipariþ Tarihi" />
                </td>
                <td colspan="1" style="width: 130px">
                    <dxe:ASPxDateEdit ID="SiparisTarihi1" runat="server" Width="120px" />
                </td>
                <td colspan="1">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblSezonId" runat="server" Text="Sezon" ForeColor="Black" />
                </td>
                <td colspan="1">
                    <dxe:ASPxComboBox ID="SezonId" runat="server" Width="395px" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="SezonId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="SezonId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {SezonIdChanged(s); }" ButtonClick="function(s, e) { ComboButtonClick('SezonId'); }"
                            EndCallback="function(s, e) { ComboEndCallback('SezonId'); }" />
                        <DropDownButton Visible="false">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblSiparisTarihi2" runat="server" ForeColor="Black" Text="Son Sipariþ Tarihi" />
                </td>
                <td colspan="1">
                    <dxe:ASPxDateEdit ID="SiparisTarihi2" runat="server" Width="120px" />
                </td>
                <td colspan="1">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblMarkaId" runat="server" Text="Marka" ForeColor="Black" />
                </td>
                <td colspan="1">
                    <dxe:ASPxComboBox ID="MarkaId" runat="server" Width="395px" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="MarkaId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="MarkaId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {MarkaIdChanged(s); }" ButtonClick="function(s, e) { ComboButtonClick('MarkaId'); }"
                            EndCallback="function(s, e) { ComboEndCallback('MarkaId'); }" />
                        <DropDownButton Visible="false">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblYuklemeTarihi1" runat="server" ForeColor="Black" Text="Ýlk Yükleme Tarihi" />
                </td>
                <td colspan="1">
                    <dxe:ASPxDateEdit ID="YuklemeTarihi1" runat="server" Width="120px" />
                </td>
                <td colspan="1">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblUrunId" runat="server" Text="Ürün" ForeColor="Black" />
                </td>
                <td colspan="1">
                    <dxe:ASPxComboBox ID="UrunId" runat="server" Width="395px" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="UrunId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="UrunId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {UrunIdChanged(s); }" ButtonClick="function(s, e) { ComboButtonClick('UrunId'); }"
                            EndCallback="function(s, e) { ComboEndCallback('UrunId'); }" />
                        <DropDownButton Visible="false">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblYuklemeTarihi2" runat="server" ForeColor="Black" Text="Son Yükleme Tarihi" />
                </td>
                <td colspan="1">
                    <dxe:ASPxDateEdit ID="YuklemeTarihi2" runat="server" Width="120px" />
                </td>
                <td colspan="1">
                    &nbsp;
                </td>
            </tr>
        </table>
        <hr />
        <dxm:ASPxMenu ID="Menu1" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
            ShowSubMenuShadow="False" AutoPostBack="True">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem Name="excel" Text="EXCEL" ToolTip="EXCEL olarak kaydet">
                    <TextTemplate>
                        EXCEL</TextTemplate>
                    <Image Url="~/images/xls_ico.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="pdf" Text="PDF" ToolTip="PDF olarak kaydet">
                    <TextTemplate>
                        PDF</TextTemplate>
                    <Image Url="~/images/pdf_icon.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DTRapor" ClientInstanceName="Grid" OnCustomCallback="Grid_CustomCallback"
            OnHtmlRowPrepared="Grid_HtmlRowPrepared">
            <ClientSideEvents EndCallback="function(s,e){GridPerformCallback(2);}" />
            <Columns>
                <dxwgv:GridViewCommandColumn Width="10px" VisibleIndex="0" ButtonType="Image">
                    <ClearFilterButton Visible="True" Text="Süzme Ýptal">
                        <Image AlternateText="Süzme Ýptal" Url="~/images/reload2.jpg" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="Tip" Caption="Tip" Visible="false" />
                <dxwgv:GridViewDataColumn FieldName="Firma" Caption="Firma" Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="Marka" Caption="Marka" Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="Urun" Caption="Ürün" Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="Sezon" Caption="Sezon" Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="ProformaTarihi" Caption="P.T." Width="100px" />
                <dxwgv:GridViewDataCheckColumn FieldName="YurtDisi" Caption="Yurt Dýþý" Width="100px" />
                <dxwgv:GridViewDataTextColumn FieldName="Adet" Caption="Adet" Width="100px" PropertiesTextEdit-DisplayFormatString="{0:n0}" />
                <dxwgv:GridViewDataTextColumn FieldName="Tutar" Caption="Sipariþ Tutarý" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                <dxwgv:GridViewDataColumn FieldName="ParaBirimi" Caption="Para Birimi" Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="Asama" Caption="Durum Yeni" Width="100px" />
                <dxwgv:GridViewDataTextColumn FieldName="Iskonto" Caption="Ýskonto" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="%{0:n2}" />
                <dxwgv:GridViewDataColumn FieldName="SevkSekli" Caption="Sevk Þekli" Width="100px" />
                <dxwgv:GridViewDataTextColumn FieldName="GelenTutar" Caption="Gelen Tutar" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                <dxwgv:GridViewDataTextColumn FieldName="OdemeIskonto" Caption="Ödeme Ýskonto" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="%{0:n2}" />
                <dxwgv:GridViewDataTextColumn FieldName="YapilanOdeme" Caption="Yapýlan Ödeme" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                <dxwgv:GridViewDataTextColumn FieldName="KalanOdeme" Caption="Kalan Ödeme" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                <dxwgv:GridViewDataColumn FieldName="OdemeSekli" Caption="Ödeme Þekli" Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="SonYuklemeTarihi" Caption="Son Yükleme Tarihi"
                    Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="LCNo" Caption="LC No" Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="Banka" Caption="Banka" Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="TasiyiciFirma" Caption="Taþýyýcý Firma" Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="OdemeVadesi" Caption="Ödeme Vadesi" Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="TahminiVarisTarihi" Caption="Tah.Varýþ Tar."
                    Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="TahminiDepoGirisTarihi" Caption="Tah.Depo Giriþ Tar."
                    Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="TahminiMagazaDagitimTarihi" Caption="Tah.Mað.Daðýtým Tar."
                    Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="GumrukVarisTarihi" Caption="Gümrük Varýþ Tar."
                    Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="DepoGirisTarihi" Caption="Depo Giriþ Tar."
                    Width="100px" />
                <dxwgv:GridViewDataDateColumn FieldName="MagazaDagitimTarihi" Caption="Mað.Daðýtým Tar."
                    Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="Depo" Caption="Depo" Width="100px" />
                <dxwgv:GridViewDataTextColumn FieldName="Agirlik" Caption="Aðýrlýk" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="{0:n2}" />
                <dxwgv:GridViewDataTextColumn FieldName="PaketAdet" Caption="Paket Adedi" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="{0:n0}" />
                <dxwgv:GridViewDataComboBoxColumn Caption="Barkod" FieldName="BarcodeId" Width="75px">
                    <PropertiesComboBox EnableIncrementalFiltering="true" ValueType="System.Int32">
                        <Items>
                            <dxe:ListEditItem Value="0" Text="" />
                            <dxe:ListEditItem Value="1" Text="ÝSTENDÝ" />
                            <dxe:ListEditItem Value="2" Text="GELDÝ" />
                            <dxe:ListEditItem Value="3" Text="GELMEYECEK" />
                            <dxe:ListEditItem Value="4" Text="BARKODLARI BÝZ ÜRETECEÐÝZ" />
                            <dxe:ListEditItem Value="5" Text="BURADA YAPIÞTIRILACAK" />
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Satýþ Fiyatlarý" FieldName="SalesPriceId"
                    Width="75px">
                    <PropertiesComboBox EnableIncrementalFiltering="true" ValueType="System.Int32">
                        <Items>
                            <dxe:ListEditItem Value="0" Text="" />
                            <dxe:ListEditItem Value="1" Text="BELÝRLENDÝ" />
                            <dxe:ListEditItem Value="2" Text="YOK" />
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Sistem Resim Yüklemesi" FieldName="SystemImageUploadId"
                    Width="75px">
                    <PropertiesComboBox EnableIncrementalFiltering="true" ValueType="System.Int32">
                        <Items>
                            <dxe:ListEditItem Value="0" Text="" />
                            <dxe:ListEditItem Value="1" Text="YAPILDI" />
                            <dxe:ListEditItem Value="2" Text="YAPILMADI" />
                        </Items>
                    </PropertiesComboBox>
                    <HeaderCaptionTemplate>
                        Sistem Resim<br />
                        Yüklemesi
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="V3 Giriþi" FieldName="IsNebimInside" Width="75px">
                    <PropertiesComboBox EnableIncrementalFiltering="true" ValueType="System.Int32">
                        <Items>
                            <dxe:ListEditItem Value="0" Text="" />
                            <dxe:ListEditItem Value="1" Text="GÝRÝLDÝ" />
                            <dxe:ListEditItem Value="2" Text="GÝRÝLMEDÝ" />
                        </Items>
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn FieldName="EksikUrun" Caption="Eksik Ürün" Width="50px"
                    Visible="true">
                    <PropertiesTextEdit DisplayFormatString="{0:n0}">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="FazlaUrun" Caption="Fazla Ürün" Width="50px"
                    Visible="true">
                    <PropertiesTextEdit DisplayFormatString="{0:n0}">
                    </PropertiesTextEdit>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataCheckColumn FieldName="IsEkMaliyet" Caption="Ek Maliyet" Width="50px" />
            </Columns>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <Settings ShowFooter="true" ShowFilterRow="false" ShowStatusBar="Hidden" ShowGroupedColumns="True"
                ShowGroupPanel="True" ShowPreview="True" ShowTitlePanel="True" ShowVerticalScrollBar="False" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <SettingsPager PageSize="19" ShowSeparators="True" />
            <SettingsText Title="Sipariþler" EmptyDataRow="Kayýt Yok" CustomizationWindowCaption="Kolon Ekle/Çýkar" />
        </dxwgv:ASPxGridView>
        <hr />
        <dxm:ASPxMenu ID="Menu2" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
            ShowSubMenuShadow="False" AutoPostBack="True">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem Name="excel" Text="EXCEL" ToolTip="EXCEL olarak kaydet">
                    <TextTemplate>
                        EXCEL</TextTemplate>
                    <Image Url="~/images/xls_ico.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="pdf" Text="PDF" ToolTip="PDF olarak kaydet">
                    <TextTemplate>
                        PDF</TextTemplate>
                    <Image Url="~/images/pdf_icon.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="Grid2" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DTRapor2" ClientInstanceName="Grid2" OnCustomCallback="Grid2_CustomCallback"
            OnHtmlRowPrepared="Grid2_HtmlRowPrepared">
            <Columns>
                <dxwgv:GridViewCommandColumn Width="10px" VisibleIndex="0" ButtonType="Image">
                    <ClearFilterButton Visible="True" Text="Süzme Ýptal">
                        <Image AlternateText="Süzme Ýptal" Url="~/images/reload2.jpg" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="YurtDisiTxt" Caption="Yurtdýþý/Yurtiçi" Width="100px" />
                <dxwgv:GridViewDataColumn FieldName="Urun" Caption="Ürün" Width="200px" />
                <dxwgv:GridViewDataColumn FieldName="ParaBirimi" Caption="Para Birimi" Width="100px" />
                <dxwgv:GridViewDataTextColumn FieldName="Adet" Caption="Adet" Width="100px" PropertiesTextEdit-DisplayFormatString="{0:n0}" />
                <dxwgv:GridViewDataTextColumn FieldName="Tutar" Caption="Sipariþ Tutarý" Width="100px"
                    PropertiesTextEdit-DisplayFormatString="{0:n2}" />
            </Columns>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <Settings ShowFooter="true" ShowFilterRow="false" ShowStatusBar="Hidden" ShowGroupedColumns="false"
                ShowGroupPanel="false" ShowPreview="True" ShowTitlePanel="True" ShowVerticalScrollBar="False" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <SettingsPager PageSize="10" ShowSeparators="True" />
            <SettingsText Title="Toplamlar" EmptyDataRow="Kayýt Yok" CustomizationWindowCaption="Kolon Ekle/Çýkar" />
        </dxwgv:ASPxGridView>
        <dxwgv:ASPxGridViewExporter ID="GridExport1" runat="server" GridViewID="Grid" Landscape="true" />
        <dxwgv:ASPxGridViewExporter ID="GridExport2" runat="server" GridViewID="Grid2" Landscape="true" />
    </div>
</asp:Content>
