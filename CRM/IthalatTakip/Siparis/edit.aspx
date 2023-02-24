<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="CRM_IthalatTakip_Siparis_edit" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />
    <script type="text/javascript" src="../../../PreLoad.js"></script>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <script src="../../../utils.js" type="text/javascript"></script>
    <script src="./edit.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="HiddenId" Visible="false" />
    <model:DataTable ID="DTUrun" runat="server" />
    <model:DataTable ID="DTYukleme" runat="server" />
    <dxcb:ASPxCallback ID="CallbackGenel" runat="server" ClientInstanceName="CallbackGenel"
        OnCallback="CallbackGenel_Callback">
        <ClientSideEvents CallbackComplete="function(s, e) {CallbackGenelComplete(e.result);}" />
    </dxcb:ASPxCallback>
    <dxcb:ASPxCallback ID="CallbackSearchBrowser" runat="server" ClientInstanceName="CallbackSearchBrowser"
        OnCallback="CallbackSearchBrowser_Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { SearchBrowserCallbackComplete(e); }"
            EndCallback="function(s, e) { SearchBrowserEndCallback(); }" />
    </dxcb:ASPxCallback>
    <div>
        <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
            ShowSubMenuShadow="False" AutoPostBack="True" Width="300px">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem Name="new" Text="Yeni">
                    <TextTemplate>
                        Yeni</TextTemplate>
                    <Image Url="~/images/new.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <TextTemplate>
                        Kaydet</TextTemplate>
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni">
                    <TextTemplate>
                        Kaydet ve Yeni</TextTemplate>
                    <Image Url="~/images/savenew.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat">
                    <TextTemplate>
                        Kaydet ve Kapat</TextTemplate>
                    <Image Url="~/images/saveclose.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="delete" Text="Sil">
                    <TextTemplate>
                        Sil</TextTemplate>
                    <Image Url="~/images/delete.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <hr />
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="SingleParagraph"
            HeaderText="UYARI : " ShowMessageBox="false" ShowSummary="true" Font-Size="10px"
            Font-Bold="true" />
        <table border="0" cellpadding="1" cellspacing="1" style="border-style: outset;">
            <tr>
                <td colspan="1" style="width: 100px">
                    <dxe:ASPxLabel ID="lblSiparisNo" runat="server" Text="Sipariþ No" />
                </td>
                <td colspan="1" style="width: 150px">
                    <dxe:ASPxTextBox ID="SiparisNo" runat="server" Width="50px" ClientInstanceName="SiparisNo"
                        ReadOnly="true" />
                </td>
                <td colspan="1" style="width: 100px">
                    <dxe:ASPxLabel ID="lblNebimNo" runat="server" Text="Nebim No" />
                </td>
                <td colspan="1" style="width: 150px">
                    <dxe:ASPxTextBox ID="NebimNo" runat="server" Width="145px" ClientInstanceName="NebimNo" />
                </td>
                <td colspan="1" style="width: 100px">
                    <dxe:ASPxLabel ID="lblSiparisTarihi" runat="server" Text="Sipariþ Tarihi" />
                </td>
                <td colspan="1" style="width: 150px">
                    <dxe:ASPxDateEdit ID="SiparisTarihi" runat="server" Width="100px" />
                </td>
            </tr>
            <tr>
                <td colspan="1" style="width: 100px">
                    <dxe:ASPxLabel ID="lblBarcode" runat="server" Text="Barkod" ForeColor="#C00000" />
                </td>
                <td colspan="1" style="width: 150px">
                    <dxe:ASPxComboBox runat="server" ID="BarcodeId" SelectedIndex="-1" EnableIncrementalFiltering="true"
                        ValueType="System.String" Width="150px">
                        <Items>
                            <dxe:ListEditItem Value="1" Text="ÝSTENDÝ" />
                            <dxe:ListEditItem Value="2" Text="GELDÝ" />
                            <dxe:ListEditItem Value="3" Text="GELMEYECEK" />
                            <dxe:ListEditItem Value="4" Text="BARKODLARI BÝZ ÜRETECEÐÝZ" />
                            <dxe:ListEditItem Value="5" Text="BURADA YAPIÞTIRILACAK" />
                        </Items>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1" style="width: 100px">
                    Satýþ Fiyatlarý
                </td>
                <td colspan="1" style="width: 150px">
                    <dxe:ASPxComboBox runat="server" ID="SalesPriceId" SelectedIndex="-1" EnableIncrementalFiltering="true"
                        ValueType="System.String" Width="150px">
                        <Items>
                            <dxe:ListEditItem Value="0" Text="" />
                            <dxe:ListEditItem Value="1" Text="BELÝRLENDÝ" />
                            <dxe:ListEditItem Value="2" Text="YOK" />
                        </Items>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1" style="width: 100px">
                    Sistem Resim Yüklemesi
                </td>
                <td colspan="1" style="width: 150px">
                    <dxe:ASPxComboBox runat="server" ID="SystemImageUploadId" SelectedIndex="-1" EnableIncrementalFiltering="true"
                        ValueType="System.String" Width="150px">
                        <Items>
                            <dxe:ListEditItem Value="0" Text="" />
                            <dxe:ListEditItem Value="1" Text="YAPILDI" />
                            <dxe:ListEditItem Value="2" Text="YAPILMADI" />
                        </Items>
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblAsamaId" runat="server" Text="Aþama" ForeColor="#C00000" />
                </td>
                <td colspan="3">
                    <dxe:ASPxComboBox ID="AsamaId" runat="server" Width="100%" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" EnableCallbackMode="true" ClientInstanceName="AsamaId"
                        OnCallback="AsamaId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {AsamaIdChanged(s); }" ButtonClick="function(s, e) { ComboButtonClick('AsamaId'); }"
                            EndCallback="function(s, e) { ComboEndCallback('AsamaId'); }" />
                        <DropDownButton Visible="False">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblSezonId" runat="server" Text="Sezon" ForeColor="#C00000" />
                </td>
                <td colspan="1">
                    <dxe:ASPxComboBox ID="SezonId" runat="server" Width="145px" ValueType="System.Guid"
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
            </tr>
            <tr>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblFirmaId" runat="server" Text="Firma" ForeColor="#C00000" />
                </td>
                <td colspan="3">
                    <dxe:ASPxComboBox ID="FirmaId" runat="server" Width="100%" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="FirmaId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="FirmaId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {FirmaIdChanged(s); }" ButtonClick="function(s, e) { ComboButtonClick('FirmaId'); }"
                            EndCallback="function(s, e) { ComboEndCallback('FirmaId'); }" />
                        <DropDownButton Visible="False">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblMarkaId" runat="server" Text="Marka" ForeColor="#C00000" />
                </td>
                <td colspan="1">
                    <dxe:ASPxComboBox ID="MarkaId" runat="server" Width="145px" ValueType="System.Guid"
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
            </tr>
            <tr>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblSevkSekliId" runat="server" Text="Sevk Þekli" />
                </td>
                <td colspan="1">
                    <dxe:ASPxComboBox ID="SevkSekliId" runat="server" Width="145px" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="SevkSekliId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="SevkSekliId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {SevkSekliIdChanged(s); }"
                            ButtonClick="function(s, e) { ComboButtonClick('SevkSekliId'); }" EndCallback="function(s, e) { ComboEndCallback('SevkSekliId'); }" />
                        <DropDownButton Visible="false">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblOdemeSekliId" runat="server" Text="Ödeme Þekli" />
                </td>
                <td colspan="1">
                    <dxe:ASPxComboBox ID="OdemeSekliId" runat="server" Width="100%" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="OdemeSekliId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="OdemeSekliId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {OdemeSekliIdChanged(s); }"
                            ButtonClick="function(s, e) { ComboButtonClick('OdemeSekliId'); }" EndCallback="function(s, e) { ComboEndCallback('OdemeSekliId'); }" />
                        <DropDownButton Visible="False">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblAdet" runat="server" Text="Adet" />
                </td>
                <td colspan="1">
                    <dxe:ASPxSpinEdit runat="server" ID="Adet" MinValue="0" NumberType="Integer" DecimalPlaces="0"
                        Increment="1" Width="100px" />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblTasiyiciFirmaId" runat="server" Text="Taþýyýcý Firma" />
                </td>
                <td colspan="3">
                    <dxe:ASPxComboBox ID="TasiyiciFirmaId" runat="server" Width="100%" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="TasiyiciFirmaId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="TasiyiciFirmaId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {TasiyiciFirmaIdChanged(s); }"
                            ButtonClick="function(s, e) { ComboButtonClick('TasiyiciFirmaId'); }" EndCallback="function(s, e) { ComboEndCallback('TasiyiciFirmaId'); }" />
                        <DropDownButton Visible="False">
                        </DropDownButton>
                        <Buttons>
                            <dxe:EditButton Text="...">
                            </dxe:EditButton>
                        </Buttons>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblTutar" runat="server" Text="Tutar" />
                </td>
                <td colspan="1">
                    <dxe:ASPxSpinEdit runat="server" ID="Tutar" MinValue="0" NumberType="Float" DecimalPlaces="2"
                        Increment="10" Width="100px" />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblAciklama" runat="server" Text="Açýklama" />
                </td>
                <td colspan="3">
                    <dxe:ASPxMemo ID="Aciklama" runat="server" Width="400px" ClientInstanceName="Aciklama"
                        Rows="2" />
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblParaBirimiId" runat="server" Text="Para Birimi" />
                </td>
                <td colspan="1">
                    <dxe:ASPxComboBox ID="ParaBirimiId" runat="server" Width="100px" ValueType="System.Guid"
                        EnableIncrementalFiltering="false" ClientInstanceName="ParaBirimiId" CallbackPageSize="15"
                        EnableCallbackMode="true" OnCallback="ParaBirimiId_Callback">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {ParaBirimiIdChanged(s); }"
                            ButtonClick="function(s, e) { ComboButtonClick('ParaBirimiId'); }" EndCallback="function(s, e) { ComboEndCallback('ParaBirimiId'); }" />
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
                <td colspan="1" style="width: 100px">
                    <dxe:ASPxLabel ID="lblSonYuklemeTarihi" runat="server" Text="Son Yük.Tarihi" />
                </td>
                <td colspan="1" style="width: 150px">
                    <dxe:ASPxDateEdit ID="SonYuklemeTarihi" runat="server" Width="100px" />
                </td>
                <td colspan="2">
                    <dxe:ASPxCheckBox runat="server" ID="YurtDisi" Text="Yurt Dýþý" Checked="false" />
                </td>
                <td colspan="1">
                    <dxe:ASPxLabel ID="lblIskonto" runat="server" Text="Ýskonto(%)" />
                </td>
                <td colspan="1">
                    <dxe:ASPxSpinEdit runat="server" ID="Iskonto" MaxValue="100" MinValue="0" NumberType="Float"
                        DecimalPlaces="2" Increment="10" Width="100px" />
                </td>
            </tr>
            <tr>
                <td colspan="1" style="width: 100px">
                    V3 Giriþi
                </td>
                <td colspan="1" style="width: 150px">
                    <dxe:ASPxComboBox runat="server" ID="IsNebimInside" SelectedIndex="-1" EnableIncrementalFiltering="true"
                        ValueType="System.String" Width="150px">
                        <Items>
                            <dxe:ListEditItem Value="0" Text="" />
                            <dxe:ListEditItem Value="1" Text="YAPILDI" />
                            <dxe:ListEditItem Value="2" Text="YAPILMADI" />
                        </Items>
                    </dxe:ASPxComboBox>
                </td>
                <td colspan="1">
                    Ýlgili PNR/LAR
                </td>
                <td colspan="3">
                    <asp:Literal ID="ltrPnrNumbers" runat="server" Text="" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <asp:RequiredFieldValidator ID="rfvSezonId" runat="server" ControlToValidate="SezonId"
                        ErrorMessage="Sezon alaný boþ olamaz!" Display="None" />
                    <asp:RequiredFieldValidator ID="rfvAsamaId" runat="server" ControlToValidate="AsamaId"
                        ErrorMessage="Aþama alaný boþ olamaz!" Display="None" />
                    <asp:RequiredFieldValidator ID="rfvFirmaId" runat="server" ControlToValidate="FirmaId"
                        ErrorMessage="Firma alaný boþ olamaz!" Display="None" />
                    <asp:RequiredFieldValidator ID="rfvMarkaId" runat="server" ControlToValidate="MarkaId"
                        ErrorMessage="Marka alaný boþ olamaz!" Display="None" />
                    <asp:RequiredFieldValidator ID="rfvBarcodeId" runat="server" ControlToValidate="BarcodeId"
                        ErrorMessage="Barkod alaný boþ olamaz!" Display="None" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    &nbsp;
                </td>
            </tr>
        </table>
        <dxtc:ASPxPageControl ID="PageAlt" runat="server" ActiveTabIndex="1" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" TabSpacing="0px" TabIndex="0">
            <TabPages>
                <dxtc:TabPage Text="ÜRÜNLER" Name="TabUrun">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <div style="overflow: scroll; width: 750px; height: 200px">
                                <dxwgv:ASPxGridView ID="GridUrun" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" DataSourceID="DTUrun" KeyFieldName="ID" ClientInstanceName="GridUrun"
                                    OnCellEditorInitialize="GridUrun_CellEditorInitialize" OnRowInserting="GridUrun_RowInserting"
                                    OnRowUpdating="GridUrun_RowUpdating" OnRowValidating="GridUrun_RowValidating"
                                    OnCustomJSProperties="Grid_CustomJSProperties" OnInitNewRow="GridUrun_InitNewRow"
                                    OnStartRowEditing="GridUrun_StartRowEditing" Width="450px">
                                    <%--<ClientSideEvents EndCallback="function(s,e){ GridEndCallback(s); }" />--%>
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn Width="40px" VisibleIndex="0" ButtonType="Image">
                                            <HeaderTemplate>
                                                <img src="../../../images/new.gif" alt="Yeni Satir" style="cursor: pointer" onclick="GridUrun.AddNewRow();" />
                                            </HeaderTemplate>
                                            <ClearFilterButton Visible="true" Text="S&#252;zme Ýptal">
                                                <Image AlternateText="S&#252;zme Ýptal" Url="~/images/reload2.jpg" />
                                            </ClearFilterButton>
                                            <EditButton Visible="true" Text="Deðiþtir">
                                                <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                                            </EditButton>
                                            <UpdateButton Visible="true" Text="G&#252;ncelle">
                                                <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                                            </UpdateButton>
                                            <DeleteButton Visible="true" Text="Sil">
                                                <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                            </DeleteButton>
                                            <CancelButton Visible="true" Text="Ýptal">
                                                <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                                            </CancelButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataColumn FieldName="ID" Caption="ID" Visible="false" Width="100px" />
                                        <dxwgv:GridViewDataColumn FieldName="Urun" Caption="Ürün" Visible="true" ReadOnly="true"
                                            Width="230px">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridUrunId')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="Adet" Caption="Adet" Width="50px" Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n0}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="Tutar" Caption="Tutar" Width="50px" Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataColumn FieldName="Aciklama" Caption="Açýklama" Visible="true"
                                            Width="230px" />
                                        <dxwgv:GridViewDataColumn FieldName="UrunId" Caption="UrunId" Width="0px" Visible="true"
                                            ReadOnly="true" />
                                    </Columns>
                                    <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                    <SettingsPager Mode="ShowAllRecords" />
                                    <SettingsEditing Mode="Inline"></SettingsEditing>
                                    <Settings GridLines="Both" ShowColumnHeaders="true" ShowFilterRow="false" ShowFilterRowMenu="false"
                                        ShowFooter="false" ShowGroupButtons="false" ShowGroupedColumns="false" ShowGroupFooter="Hidden"
                                        ShowGroupPanel="false" ShowHeaderFilterButton="false" ShowPreview="true" ShowStatusBar="Hidden"
                                        ShowTitlePanel="false" UseFixedTableLayout="false" ShowVerticalScrollBar="false"
                                        VerticalScrollableHeight="250" />
                                    <SettingsText Title="Ürünler" EmptyDataRow="." />
                                    <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                    </Images>
                                    <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px" />
                                        <AlternatingRow Enabled="True" />
                                    </Styles>
                                </dxwgv:ASPxGridView>
                            </div>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="YÜKLEMELER" Name="TabYukleme">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <div style="overflow: scroll; width: 750px; height: 200px">
                                <dxwgv:ASPxGridView ID="GridYukleme" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" DataSourceID="DTYukleme" KeyFieldName="ID" ClientInstanceName="GridYukleme"
                                    OnCellEditorInitialize="GridYukleme_CellEditorInitialize" OnRowInserting="GridYukleme_RowInserting"
                                    OnRowUpdating="GridYukleme_RowUpdating" OnRowValidating="GridYukleme_RowValidating"
                                    OnCustomJSProperties="Grid_CustomJSProperties" OnInitNewRow="GridYukleme_InitNewRow"
                                    OnStartRowEditing="GridYukleme_StartRowEditing">
                                    <%--<ClientSideEvents EndCallback="function(s,e){ GridEndCallback(s); }" />--%>
                                    <Columns>
                                        <dxwgv:GridViewCommandColumn Width="40px" VisibleIndex="0" ButtonType="Image">
                                            <HeaderTemplate>
                                                <img src="../../../images/new.gif" alt="Yeni Satir" style="cursor: pointer" onclick="GridYukleme.AddNewRow();" />
                                            </HeaderTemplate>
                                            <ClearFilterButton Visible="true" Text="S&#252;zme Ýptal">
                                                <Image AlternateText="S&#252;zme Ýptal" Url="~/images/reload2.jpg" />
                                            </ClearFilterButton>
                                            <EditButton Visible="true" Text="Deðiþtir">
                                                <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                                            </EditButton>
                                            <UpdateButton Visible="true" Text="G&#252;ncelle">
                                                <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                                            </UpdateButton>
                                            <DeleteButton Visible="true" Text="Sil">
                                                <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                            </DeleteButton>
                                            <CancelButton Visible="true" Text="Ýptal">
                                                <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                                            </CancelButton>
                                        </dxwgv:GridViewCommandColumn>
                                        <dxwgv:GridViewDataColumn FieldName="ID" Caption="ID" Visible="false" Width="100px" />
                                        <dxwgv:GridViewDataTextColumn FieldName="YuklemeNo" Caption="Yük.No" Width="40px"
                                            Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n0}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataColumn FieldName="Asama" Caption="Aþama" Visible="true" ReadOnly="true"
                                            Width="150px">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true" Name="AsamaAra">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridAsamaId')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn FieldName="Urun" Caption="Ürün" Visible="true" ReadOnly="true"
                                            Width="200px">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true" Name="UrunAra">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridUrunId2')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn FieldName="Sezon" Caption="Sezon" Visible="true" ReadOnly="true"
                                            Width="60px">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true" Name="SezonAra">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridSezonId')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="Adet" Caption="Adet" Width="60px" Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n0}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="Tutar" Caption="Tutar" Width="100px" Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="Iskonto" Caption="Ýskonto" Width="60px"
                                            Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataDateColumn FieldName="ProformaTarihi" Caption="Proforma Tarihi"
                                            Width="165px" />
                                        <dxwgv:GridViewDataColumn FieldName="SevkSekli" Caption="Sevk Þekli" Visible="true"
                                            ReadOnly="true" Width="60px">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true" Name="SevkSekliAra">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridSevkSekliId')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="GelenTutar" Caption="Gelen Tutar" Width="100px"
                                            Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="OdemeIskonto" Caption="Öd.Ýskonto" Width="60px"
                                            Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="YapilanOdeme" Caption="Yapýlan Ödeme" Width="100px"
                                            Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="KalanOdeme" Caption="Kalan Ödeme" Width="100px"
                                            Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataColumn FieldName="OdemeSekli" Caption="Ödeme Þekli" Visible="true"
                                            ReadOnly="true" Width="60px">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true" Name="OdemeSekliAra">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridOdemeSekliId')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataDateColumn FieldName="SonYuklemeTarihi" Caption="Son.Yük.Tarihi"
                                            Width="165px" Visible="true" />
                                        <dxwgv:GridViewDataColumn FieldName="LCNo" Caption="LC No" Visible="true" Width="100px" />
                                        <dxwgv:GridViewDataColumn FieldName="Banka" Caption="Banka" Visible="true" ReadOnly="true"
                                            Width="100px" />
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true" Name="BankaAra">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridBankaId')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn FieldName="TasiyiciFirma" Caption="Taþýyýcý Firma" Visible="true"
                                            ReadOnly="true" Width="100px">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true" Name="TasiyiciFirmaAra">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridTasiyiciFirmaId')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataDateColumn FieldName="OdemeVadesi" Caption="Ödeme Vadesi" Width="165px"
                                            Visible="true" ReadOnly="true" />
                                        <dxwgv:GridViewDataDateColumn FieldName="TahminiVarisTarihi" Caption="Tah.Varýþ Tarihi"
                                            Width="165px" Visible="true" />
                                        <dxwgv:GridViewDataDateColumn FieldName="GumrukVarisTarihi" Caption="Gümrük Varýþ Tarihi"
                                            Width="165px" Visible="true" />
                                        <dxwgv:GridViewDataDateColumn FieldName="TahminiDepoGirisTarihi" Caption="Tah.Depo Giriþ Tarihi"
                                            Width="165px" Visible="true" />
                                        <dxwgv:GridViewDataDateColumn FieldName="DepoGirisTarihi" Caption="Depo Giriþ Tarihi"
                                            Width="165px" Visible="true" />
                                        <dxwgv:GridViewDataColumn FieldName="Depo" Caption="Depo" Visible="true" ReadOnly="true"
                                            Width="100px">
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true" Name="DepoAra">
                                            <DataItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                            </DataItemTemplate>
                                            <EditItemTemplate>
                                                <img src="../../../images/search_button.jpg" alt="" width="22px" height="19px" onclick="ComboButtonClick('GridDepoId')"
                                                    style="cursor: pointer" />
                                            </EditItemTemplate>
                                            <EditFormSettings Visible="True" />
                                        </dxwgv:GridViewDataColumn>
                                        <dxwgv:GridViewDataDateColumn FieldName="TahminiMagazaDagitimTarihi" Caption="Tah.Maðaza Dað.Tarihi"
                                            Width="165px" Visible="true" />
                                        <dxwgv:GridViewDataDateColumn FieldName="MagazaDagitimTarihi" Caption="Maðaza Daðýtým Tarihi"
                                            Width="165px" Visible="true" />
                                        <dxwgv:GridViewDataTextColumn FieldName="Agirlik" Caption="Aðýrlýk" Width="60px"
                                            Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n2}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataTextColumn FieldName="PaketAdet" Caption="Paket Adedi" Width="60px"
                                            Visible="true">
                                            <PropertiesTextEdit DisplayFormatString="{0:n0}">
                                            </PropertiesTextEdit>
                                        </dxwgv:GridViewDataTextColumn>
                                        <dxwgv:GridViewDataColumn FieldName="Aciklama" Caption="Açýklama" Visible="true"
                                            Width="150px" />
                                        <dxwgv:GridViewDataColumn FieldName="UrunId" Caption="UrunId" Width="0px" Visible="true"
                                            ReadOnly="true" />
                                        <dxwgv:GridViewDataColumn FieldName="SezonId" Caption="SezonId" Width="0px" Visible="true"
                                            ReadOnly="true" />
                                        <dxwgv:GridViewDataColumn FieldName="AsamaId" Caption="AsamaId" Width="0px" Visible="true"
                                            ReadOnly="true" />
                                        <dxwgv:GridViewDataColumn FieldName="SevkSekliId" Caption="SevkSekliId" Width="0px"
                                            Visible="true" ReadOnly="true" />
                                        <dxwgv:GridViewDataColumn FieldName="OdemeSekliId" Caption="OdemeSekliId" Width="0px"
                                            Visible="true" ReadOnly="true" />
                                        <dxwgv:GridViewDataColumn FieldName="BankaId" Caption="BankaId" Width="0px" Visible="true"
                                            ReadOnly="true" />
                                        <dxwgv:GridViewDataColumn FieldName="TasiyiciFirmaId" Caption="TasiyiciFirmaId" Width="0px"
                                            Visible="true" ReadOnly="true" />
                                        <dxwgv:GridViewDataColumn FieldName="DepoId" Caption="DepoId" Width="0px" Visible="true"
                                            ReadOnly="true" />
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
                                    <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                    <SettingsPager Mode="ShowAllRecords" />
                                    <SettingsEditing Mode="Inline"></SettingsEditing>
                                    <Settings GridLines="Both" ShowColumnHeaders="true" ShowFilterRow="false" ShowFilterRowMenu="false"
                                        ShowFooter="false" ShowGroupButtons="false" ShowGroupedColumns="false" ShowGroupFooter="Hidden"
                                        ShowGroupPanel="false" ShowHeaderFilterButton="false" ShowPreview="true" ShowStatusBar="Hidden"
                                        ShowTitlePanel="false" UseFixedTableLayout="false" ShowVerticalScrollBar="false"
                                        VerticalScrollableHeight="250" />
                                    <SettingsText Title="Yüklemeler" EmptyDataRow="." />
                                    <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                    </Images>
                                    <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                        <Header ImageSpacing="5px" SortingImageSpacing="5px" />
                                        <AlternatingRow Enabled="True" />
                                    </Styles>
                                </dxwgv:ASPxGridView>
                            </div>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </div>
    </form>
</body>
</html>
