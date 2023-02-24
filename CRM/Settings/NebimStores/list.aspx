<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="MarjinalCRM_Settings_NebimStores_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <model:DataTable ID="DataTableList" runat="server" />
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
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="100%"
            OnRowValidating="grid_RowValidating" OnRowUpdating="grid_RowUpdating">
            <Columns>
                <dxwgv:GridViewCommandColumn Width="50px" VisibleIndex="0" ButtonType="Image">
                    <EditButton Visible="True" Text="Değiştir">
                        <Image AlternateText="Değiştir" Url="~/images/edit.gif" />
                    </EditButton>
                    <UpdateButton Visible="True" Text="Güncelle">
                        <Image AlternateText="Güncelle" Url="~/images/update.gif" />
                    </UpdateButton>
                    <CancelButton Visible="True" Text="İptal">
                        <Image AlternateText="İptal" Url="~/images/delete.gif" />
                    </CancelButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                <dxwgv:GridViewDataColumn FieldName="NebimStoreID" Visible="False" />
                <dxwgv:GridViewDataColumn Caption="Adı" FieldName="Adi" Width="100px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Erp Kodu" FieldName="ErpCode" Width="80px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="" FieldName="MinStok" Width="100px" PropertiesSpinEdit-MaxValue="100" PropertiesSpinEdit-MinValue="1">
                    <HeaderCaptionTemplate>
                        Güvenli Min.<br />
                        Stok
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Depo Açık" FieldName="StoreOpen" Width="60px">
                    <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                        DisplayTextChecked="Evet" DisplayTextUnchecked="Hayır">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluşturma Tarihi"
                    EditFormSettings-Visible="False" FieldName="CreationDate">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Düzenlenme Tarihi"
                    EditFormSettings-Visible="False" FieldName="ModificationDate">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="False" />
            </Columns>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                ShowGroupPanel="True" ShowPreview="True" ShowTitlePanel="True" ShowVerticalScrollBar="False" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
                <AlternatingRow Enabled="True" />
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsLoadingPanel Text="Yükleniyor..." />
            <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter"
                PopupEditFormVerticalAlign="WindowCenter" PopupEditFormModal="true" PopupEditFormWidth="500px" />
            <SettingsText Title="E-Ticaret Depolar" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya sürükleyiniz."
                ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="#" />
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
