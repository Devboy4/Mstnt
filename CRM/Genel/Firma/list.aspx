<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="CRM_Genel_Firma_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    tagprefix="dxw" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <model:DataTable ID="DataTableList" runat="server" />
        <asp:SqlDataSource ID="DSUlke" runat="server" SelectCommand="SELECT UlkeID,Adi AS Ulke FROM Ulke ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSSehir" runat="server" SelectCommand="SELECT SehirID,Adi AS Sehir FROM Sehir ORDER BY Adi"
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
                <dxm:MenuItem Name="excel" Text="EXCEL" ToolTip="EXCEL olarak kaydet">
                    <Image Url="~/images/xls_ico.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="pdf" Text="PDF" ToolTip="PDF olarak kaydet">
                    <Image Url="~/images/pdf_icon.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" AutoGenerateColumns="False"
            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DataTableList"
            KeyFieldName="ID" OnCellEditorInitialize="Grid_CellEditorInitialize" OnCustomCallback="grid_CustomCallback"
            OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating" OnRowValidating="grid_RowValidating"
            Width="1200px">
            <SettingsText Title="İlgili Birimler" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="Yeni satır ekle" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <Settings ShowPreview="True" ShowTitlePanel="True" />
            <SettingsLoadingPanel Text="Yükleniyor..." />
            <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                PopupEditFormModal="true" PopupEditFormWidth="500px" />
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
                    <EditButton Text="Değiştir" Visible="True">
                        <Image AlternateText="Değiştir" Url="~/images/edit.gif" />
                    </EditButton>
                    <CancelButton Text="İptal" Visible="True">
                        <Image AlternateText="İptal" Url="~/images/delete.gif" />
                    </CancelButton>
                    <NewButton Text="Yeni" Visible="True">
                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                    </NewButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn Visible="False" FieldName="ID">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Visible="False" FieldName="FirmaID">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="FirmaName" Caption="Adı" Width="150px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataCheckColumn FieldName="MerkezMagaza" Caption="Merkez İlgili Birim">
                    <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataComboBoxColumn FieldName="UlkeID" Caption="Ülke" Width="175px">
                    <PropertiesComboBox EnableIncrementalFiltering="true" DataSourceID="DSUlke" ValueType="System.Guid"
                        ValueField="UlkeID" TextField="Ulke">
                        <ClientSideEvents SelectedIndexChanged="function(s, e) {OnUlkeIDChanged(s); }" />
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn FieldName="SehirID" Caption="Şehir" Width="175px">
                    <PropertiesComboBox EnableIncrementalFiltering="true" EnableCallbackMode="true" ClientInstanceName="cmbSehirID"
                        DataSourceID="DSSehir" ValueType="System.Guid" ValueField="SehirID" TextField="Sehir">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataCheckColumn UnboundType="Integer" FieldName="Active" Caption="Aktif">
                    <PropertiesCheckEdit DisplayTextChecked="Aktif" DisplayTextUnchecked="Pasif">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataColumn FieldName="CreatedBy" Caption="Ekleyen">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="ModifiedBy" Caption="Düzenleyen">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" FieldName="CreationDate"
                    Caption="Oluşturma Tarihi">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" FieldName="ModificationDate"
                    Caption="Düzenlenme Tarihi">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataColumn UnboundType="String" Visible="False" FieldName="Filter">
                </dxwgv:GridViewDataColumn>
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
    </div>
</asp:Content>
