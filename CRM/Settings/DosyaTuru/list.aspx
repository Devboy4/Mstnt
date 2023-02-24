<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="MarjinalCRM_Settings_DosyaTuru_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                OnRowValidating="grid_RowValidating" OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating">
                <Columns>
                    <dxwgv:GridViewCommandColumn Width="90px" VisibleIndex="0" ButtonType="Image">
                        <NewButton Visible="True" Text="Yeni">
                            <Image AlternateText="Yeni" Url="~/images/new.gif" />
                        </NewButton>
                        <EditButton Visible="True" Text="Deðiþtir">
                            <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                        </EditButton>
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
                    <dxwgv:GridViewDataColumn FieldName="DosyaTuruID" Visible="False" />
                    <dxwgv:GridViewDataColumn Caption="Dosya Uzantýsý" FieldName="DosyaTuru" Width="50%">
                        <HeaderStyle ForeColor="#C00000" />
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Maksimum Boyut" FieldName="MaksimumBoyut" Width="25%" />
                    <dxwgv:GridViewDataComboBoxColumn Caption="Boyut Türü" FieldName="BoyutTuru" Width="25%">
                        <PropertiesComboBox ValueType="System.String">
                            <Items>
                                <dxe:ListEditItem Text="Bayt" Value="Bayt" />
                                <dxe:ListEditItem Text="KB" Value="KB" />
                                <dxe:ListEditItem Text="MB" Value="MB" />
                                <dxe:ListEditItem Text="GB" Value="GB" />
                            </Items>
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluþturma Tarihi"
                        FieldName="CreationDate" Width="20%">
                        <EditItemTemplate>
                            <%# Eval("CreationDate")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn FieldName="AllowedRoles" Visible="False" />
                    <dxwgv:GridViewDataColumn FieldName="DeniedRoles" Visible="False" />
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
                <SettingsText Title="Dosya Türleri" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya sürükleyiniz."
                    ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
            </dxwgv:ASPxGridView>
        </div>
   </asp:Content>
