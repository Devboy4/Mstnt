<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="CRM_Genel_WebSiparis_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
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
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="JsBarcode.all.min.js" type="text/javascript"></script>
    <div>
        <script type="text/javascript">
            function LoadPopupExcl() {
                popExcel.Show();
            }
            function PrintBarcode(a, b, c, d, e, f, g, h) {
                if (a != 'Trendyol.com') return false;
                if (b != '') {
                    JsBarcode("#barcode", b);
                    var _adsoyad = document.getElementById("tyadsoyad");
                    var _adres = document.getElementById("tyadres");
                    var _urunad = document.getElementById("tyurunad");
                    var _barkod = document.getElementById("tybarkod");
                    var _beden = document.getElementById("tybeden");
                    var _adet = document.getElementById("tyadet");

                    _adsoyad.innerHTML = c;
                    _adres.innerHTML = d;
                    _urunad.innerHTML = e;
                    _barkod.innerHTML = f;
                    _beden.innerHTML = g;
                    _adet.innerHTML = h + " Adet";

                    var panel = document.getElementById("trendyolpanel");
                    var printWindow = window.open('', '', 'height=700,width=1000');
                    printWindow.document.write('<html><head><title>Trendyol Partner Programý</title>');
                    printWindow.document.write('</head><body >');
                    printWindow.document.write(panel.innerHTML);
                    printWindow.document.write('</body></html>');
                    printWindow.document.close();
                    printWindow.focus();
                    printWindow.print();
                    printWindow.close();

                    //setTimeout(function () {
                    //    printWindow.print();
                    //}, 500);
                    return false;
                }
            }
        </script>
        <model:DataTable ID="DataTableList" runat="server" />
        <asp:HiddenField ID="HiddenUserId" runat="server" />
        <asp:SqlDataSource ID="DSProje" runat="server" SelectCommand="select t1.IndexId, t1.UserName Adi from SecurityUsers t1
                    left outer join Proje  t2 on t1.ProjeID=t2.ProjeID
                     where Active=1 and t2.IsShop=1 Order By t1.UserName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DSDurum" runat="server" SelectCommand="Select t1.IntId IndexID,t1.Adi from WebSiparisDurum t1 Order By t1.Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
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
                <dxm:MenuItem Name="List">
                    <Template>
                        <table width="50" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer; padding-right: 4px; border-right-width: 0px; width: 150px;"
                                    onclick="grid.PerformCallback('x');">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/List.gif" />Listele
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="PopupexcLoad">
                    <Image Url="~/images/new.gif" />
                    <Template>
                        <table width="150" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer; padding-right: 4px; border-right-width: 0px; width: 150px;"
                                    onclick="LoadPopupExcl();">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/xls_ico.gif" />
                                    Excel'den Yükle
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="excel" Text="EXCEL" ToolTip="EXCEL olarak kaydet">
                    <Image Url="~/images/xls_ico.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="pdf" Text="PDF" ToolTip="PDF olarak kaydet">
                    <Image Url="~/images/pdf_icon.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="750px"
            BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
            ShowHeader="False">
            <PanelCollection>
                <dxrp:PanelContent ID="PanelContent1" runat="server">
                    <table border="0" cellpadding="0" cellspacing="3" style="width: 750px">
                        <tr>
                            <td align="left" style="width: 150px" valign="top">Baþlangýç Tarihi
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxDateEdit ID="StartDate" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td align="left" valign="top">Bitiþ Tarihi
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxDateEdit ID="EndDate" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                    </table>
                </dxrp:PanelContent>
            </PanelCollection>
        </dxrp:ASPxRoundPanel>
        <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="100%" OnCustomCallback="grid_CustomCallback"
            OnRowValidating="grid_RowValidating" OnCellEditorInitialize="grid_CellEditorInitialize" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared">
            <SettingsText Title="Web Sipariþleri" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya sürükleyiniz."
                ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
            <SettingsPager PageSize="100" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsLoadingPanel Text="Yükleniyor..." />
            <SettingsEditing Mode="Inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                PopupEditFormModal="false" PopupEditFormWidth="800px" />
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                ShowGroupPanel="True" ShowPreview="True" ShowTitlePanel="True" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <AlternatingRow Enabled="True">
                </AlternatingRow>
                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="80px">
                    <UpdateButton Text="G&#252;ncelle" Visible="True">
                        <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                    </UpdateButton>
                    <DeleteButton Text="Sil" Visible="True">
                        <Image AlternateText="Sil" Url="~/images/delete.gif" />
                    </DeleteButton>
                    <EditButton Text="Deðiþtir" Visible="True">
                        <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                    </EditButton>
                    <CancelButton Text="Ýptal" Visible="True">
                        <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                    </CancelButton>
                    <NewButton Text="Yeni" Visible="True">
                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                    </NewButton>
                    <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                        <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="IndexId" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Atanan Maðaza" FieldName="AtananMagaza" Width="125px">
                    <PropertiesComboBox TextField="Adi" DataSourceID="DSProje" ValueField="IndexId"
                        EnableIncrementalFiltering="true" ValueType="System.Int32" EnableCallbackMode="true"
                        CallbackPageSize="15">
                    </PropertiesComboBox>
                    <HeaderCaptionTemplate>
                        Atanan<br />
                        Maðaza
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataDateColumn Caption="Sipariþ Tarihi" Width="90px"
                    FieldName="SiparisTarihi">
                    <HeaderCaptionTemplate>
                        Sipariþ<br />
                        Tarihi
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataColumn FieldName="WebSiparisNo" Caption="Web Siparis No" Width="70px">
                    <HeaderCaptionTemplate>
                        Web Siparis<br />
                        No
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="UrunCardId" Caption="" Width="70px">
                    <HeaderCaptionTemplate>
                        Ürün<br />
                        Satýr ID
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="FaturaRefNo" Caption="Fatura Ref No" Width="90px">
                    <HeaderCaptionTemplate>
                        Fatura Ref<br />
                        No
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="SiparisKaynak" Caption="Sipariþ Kaynak" Width="110px">
                    <HeaderCaptionTemplate>
                        Sipariþ<br />
                        Kaynak
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="KargoFirma" Caption="" Width="80px">
                    <HeaderCaptionTemplate>
                        Kargo<br />
                        Firma
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="KargoTakipNo" Caption="" Width="90px">
                    <HeaderCaptionTemplate>
                        Kargo Takip<br />
                        No
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption=" " FieldName="" Width="20px">
                    <EditItemTemplate></EditItemTemplate>
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="img_print" runat="server" NavigateUrl=<%#"JavaScript:PrintBarcode('"+Eval("SiparisKaynak")+"','"+Eval("KargoTakipNo")+"','"+Eval("TeslimatAliciAd")+"','"+Eval("TeslimatAdres")+"','"+Eval("UrunAd")+"','"+Eval("Barkod")+"','"+Eval("Beden")+"','"+Eval("Adet")+"');"%>
                            ImageUrl="~/images/printico.gif" ImageHeight="16px" ImageWidth="16px">
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="" FieldName="Durum" Width="125px">
                    <PropertiesComboBox TextField="Adi" DataSourceID="DSDurum" ValueField="IndexId"
                        EnableIncrementalFiltering="true" ValueType="System.Int32" EnableCallbackMode="true"
                        CallbackPageSize="15">
                    </PropertiesComboBox>
                    <HeaderCaptionTemplate>
                        Durum
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataMemoColumn FieldName="MagazaNot" Caption="" Width="120px">
                    <HeaderCaptionTemplate>
                        Maðaza<br />
                        Not
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataMemoColumn FieldName="MusteriNot" Caption="" Width="120px">
                    <HeaderCaptionTemplate>
                        Müþteri<br />
                        Not
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataMemoColumn FieldName="MerkezNot" Caption="" Width="120px">
                    <HeaderCaptionTemplate>
                        Merkez<br />
                        Not
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataColumn FieldName="TeslimatAliciAd" Caption="" Width="100px">
                    <HeaderCaptionTemplate>
                        Teslimat Alýcý<br />
                        Adý
                    </HeaderCaptionTemplate>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="TeslimatTel" Caption="" Width="90px">
                    <HeaderCaptionTemplate>
                        Teslimat<br />
                        Tel
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="UyeMail" Caption="" Width="100px">
                    <HeaderCaptionTemplate>
                        Üye<br />
                        Mail
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataMemoColumn FieldName="TeslimatAdres" Caption="" Width="120px">
                    <HeaderCaptionTemplate>
                        Teslimat<br />
                        Adres
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataColumn FieldName="TeslimatIlce" Caption="" Width="90px">
                    <HeaderCaptionTemplate>
                        Teslimat<br />
                        Ýlçe
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="TeslimatSehir" Caption="" Width="90px">
                    <HeaderCaptionTemplate>
                        Teslimat<br />
                        Þehir
                    </HeaderCaptionTemplate>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="Barkod" Caption="" Width="90px">
                    <HeaderCaptionTemplate>
                        Barkod
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="StokKodu" Caption="" Width="90px">
                    <HeaderCaptionTemplate>
                        Stok<br />
                        Kodu
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="Beden" Caption="" Width="60px">
                    <HeaderCaptionTemplate>
                        Beden
                    </HeaderCaptionTemplate>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="Adet" Caption="" Width="60px">
                    <HeaderCaptionTemplate>
                        Adet
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataMemoColumn FieldName="UrunAd" Caption="" Width="120px">
                    <HeaderCaptionTemplate>
                        Ürün<br />
                        Ad
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataColumn FieldName="Marka" Caption="" Width="100px">
                    <HeaderCaptionTemplate>
                        Marka
                    </HeaderCaptionTemplate>
                    <CellStyle Wrap="False"></CellStyle>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
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
        <dxwgv:ASPxGridViewExporter ID="gridExport" runat="server">
            <Styles>
                <Cell Font-Names="Verdana" Font-Size="8">
                </Cell>
                <Header Font-Names="Verdana" Font-Size="8">
                </Header>
            </Styles>
        </dxwgv:ASPxGridViewExporter>
        <dxpc:ASPxPopupControl ID="popExcel" runat="server" AllowDragging="True" AllowResize="True"
            CloseAction="MouseOut" EnableViewState="False" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowFooter="false" ShowHeader="true" Width="400px"
            Height="200px" FooterText="Paneli sað alt köþesinden tutup boyutlandýrabilirsiniz..."
            HeaderText="Excel Yükleme Alaný" ClientInstanceName="popExcel" EnableHierarchyRecreation="True"
            DragElement="Window" Modal="false">
            <ContentCollection>
                <dxpc:PopupControlContentControl>
                    <table style="border: none; width: 100%">
                        <tr>
                            <td style="text-align: left; width: 20%">Dosya Seç</td>
                            <td style="text-align: left; width: 80%">
                                <asp:FileUpload runat="server" ID="flpExcel" /></td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: left">
                                <dxe:ASPxButton runat="server" Text="Yükle..." ID="btnExcelUpload" OnClick="btnExcelUpload_Click" Image-Url="~/images/add_icon.gif"></dxe:ASPxButton>
                            </td>
                        </tr>
                    </table>

                </dxpc:PopupControlContentControl>
            </ContentCollection>
            <ClientSideEvents Shown="function(s, e) {s.UpdatePosition();}" />
        </dxpc:ASPxPopupControl>
    </div>
    <div style="display: none">
        <div id="trendyolpanel">
            <table style="border-spacing: 5px; border-collapse: separate; width: 1000px; background-color: white; font-family: Verdana; font-size: 14px; table-layout: fixed">
                <tr>
                    <td style="text-align: center; vertical-align: top">
                        <img src="../../../images/trendyollogo.png" alt="" /></td>

                </tr>
                <tr>
                    <td style="text-align: center; vertical-align: top">
                        <svg id="barcode"></svg>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; vertical-align: top">Kargo þirketinin dikkatine, bu bir <b>trendyol.com</b> gönderisidir.
                        
                    </td>
                </tr>
                <tr>

                    <td style="text-align: center; vertical-align: top">Trendyol Öder seçerek iþlem yapabilirsiniz.
                    </td>
                </tr>
                <tr>
                    <td>
                        <b style="font-size: 15px">ALICI BÝLGÝLERÝ</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 10%; text-align: left"><b style="font-size: 14px">Ad Soyad:</b></td>
                                <td style="width: 90%; text-align: left"><span id="tyadsoyad"></span></td>
                            </tr>
                            <tr>
                                <td style="width: 10%; text-align: left"><b style="font-size: 14px">Adres:</b></td>
                                <td style="width: 90%; text-align: left"><span id="tyadres"></span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; vertical-align: top">
                        <img src="../../../images/trendyolkargobilg.png" alt="" /></td>

                </tr>
                <tr>
                    <td>
                        <b style="font-size: 15px">ÜRÜN BÝLGÝLERÝ</b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 65%; text-align: left; vertical-align: top"><span id="tyurunad"></span></td>
                                <td style="width: 10%; text-align: left; vertical-align: top">(No Color)</td>
                                <td style="width: 12%; text-align: left; vertical-align: top"><span id="tybarkod"></span></td>
                                <td style="width: 6%; text-align: left; vertical-align: top"><span id="tybeden"></span></td>
                                <td style="width: 7%; text-align: left; vertical-align: top"><span id="tyadet"></span></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
