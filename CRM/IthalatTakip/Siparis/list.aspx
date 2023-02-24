<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="CRM_IthalatTakip_Siparis_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <model:DataTable ID="DTList" runat="server" />
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
                <dxm:MenuItem Name="new">
                    <Template>
                        <table width="50" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItem_Blue" style="cursor: pointer;" align="center" valign="middle"
                                    onclick="javascript:PopWin = OpenPopupWinBySize('edit.aspx?id=0',800,550);PopWin.focus();">
                                    <img src="../../../images/new.gif" alt="" />&nbsp;<b>Yeni</b>
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <TextTemplate>
                        Kaydet</TextTemplate>
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni" Visible="false">
                    <TextTemplate>
                        Kaydet ve Yeni</TextTemplate>
                    <Image Url="~/images/savenew.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat" Visible="false">
                    <TextTemplate>
                        Kaydet ve Kapat</TextTemplate>
                    <Image Url="~/images/saveclose.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="delete" Text="Sil" Visible="false">
                    <TextTemplate>
                        Sil</TextTemplate>
                    <Image Url="~/images/delete.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Text="Yenile" NavigateUrl="javascript:location.reload(true);">
                    <Image Url="~/images/reload2.jpg" />
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
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DTList" KeyFieldName="ID" SettingsEditing-Mode="Inline"
            Width="100%" ClientInstanceName="Grid" OnCustomJSProperties="Grid_CustomJSProperties"
            OnHtmlDataCellPrepared="Grid_HtmlDataCellPrepared">
            <Columns>
                <dxwgv:GridViewCommandColumn Width="30px" VisibleIndex="0" ButtonType="Image">
                    <ClearFilterButton Visible="True" Text="Süzme Ýptal">
                        <Image AlternateText="Süzme Ýptal" Url="~/images/reload2.jpg" />
                    </ClearFilterButton>
                    <DeleteButton Visible="True" Text="Sil">
                        <Image AlternateText="Sil" Url="~/images/delete.gif" />
                    </DeleteButton>
                    <UpdateButton Visible="True" Text="Güncelle">
                        <Image AlternateText="Güncelle" Url="~/images/update.gif" />
                    </UpdateButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Caption="ID" Visible="false" />
                <dxwgv:GridViewDataColumn FieldName="SiparisNo" Caption="Sipariþ No" Width="60px">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("ID")+"',800,550);PopWin.focus();"%>
                            Text='<%#Eval("SiparisNo")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn Caption="Sipariþ Tarihi" FieldName="SiparisTarihi"
                    Width="80px" />
                <dxwgv:GridViewDataColumn FieldName="NebimNo" Caption="Nebim No" Width="60px" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataColumn FieldName="Asama" Caption="Aþama" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataColumn FieldName="Firma" Caption="Firma" Settings-AutoFilterCondition="Contains">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="ASPxHyperLink2" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../Firma/edit.aspx?id="+Eval("FirmaId")+"',800,500);PopWin.focus();"%>
                            Text='<%#Eval("Firma")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="Sezon" Caption="Sezon" Width="60px" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataColumn FieldName="Marka" Caption="Marka" Width="150px" Settings-AutoFilterCondition="Contains" />
                <%--                    <dxwgv:GridViewDataTextColumn FieldName="Adet" Caption="Adet" PropertiesTextEdit-DisplayFormatString="{0:n0}"
                        Width="60px" />
                    <dxwgv:GridViewDataTextColumn FieldName="Tutar" Caption="Tutar" PropertiesTextEdit-DisplayFormatString="{0:n2}"
                        Width="60px" />
                    <dxwgv:GridViewDataColumn FieldName="ParaBirimi" Caption="Para Birimi" Width="60px"
                        Settings-AutoFilterCondition="Contains" />
                    <dxwgv:GridViewDataTextColumn FieldName="Iskonto" Caption="Ýskonto" PropertiesTextEdit-DisplayFormatString="{0:n2}"
                        Width="60px" />
                    <dxwgv:GridViewDataColumn FieldName="SevkSekli" Caption="Sevk Þekli" Width="60px"
                        Settings-AutoFilterCondition="Contains" />
                    <dxwgv:GridViewDataColumn FieldName="OdemeSekli" Caption="Ödeme Þekli" Width="60px"
                        Settings-AutoFilterCondition="Contains" />--%>
                <dxwgv:GridViewDataColumn FieldName="TasiyiciFirma" Caption="Taþýyýcý Firma" Settings-AutoFilterCondition="Contains" />
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
                <%--<dxwgv:GridViewDataColumn FieldName="Aciklama" Caption="Açýklama" Settings-AutoFilterCondition="Contains" />--%>
                <%--<dxwgv:GridViewDataDateColumn Caption="Tarih" FieldName="Tarih" Width="80px" />
                    <dxwgv:GridViewDataColumn Caption="Kaydeden" FieldName="Kaydeden" Width="100px" />
                    <dxwgv:GridViewDataDateColumn Caption="Kayýt Tarihi" FieldName="CreationDate" Width="80px" />
                    <dxwgv:GridViewDataColumn Caption="Güncelleyen" FieldName="Guncelleyen" Width="100px" />
                    <dxwgv:GridViewDataDateColumn Caption="Güncelleme Tarihi" FieldName="ModificationDate"
                        Width="80px" />--%>
            </Columns>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <Settings ShowFilterRow="True" ShowStatusBar="Hidden" ShowGroupedColumns="True" ShowGroupPanel="True"
                ShowPreview="True" ShowTitlePanel="True" ShowVerticalScrollBar="False" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <SettingsPager PageSize="19" ShowSeparators="True">
            </SettingsPager>
            <SettingsText Title="Sipariþler" EmptyDataRow="." />
        </dxwgv:ASPxGridView>
        <dxwgv:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="Grid" Landscape="false">
        </dxwgv:ASPxGridViewExporter>
    </div>
</asp:Content>
