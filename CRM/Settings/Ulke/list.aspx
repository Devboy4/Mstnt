<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" 
    Inherits="MarjinalCRM_Settings_Ulke_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
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
                OnRowValidating="grid_RowValidating">
                <SettingsText Title="Ülkeler" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                    ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <SettingsCustomizationWindow Enabled="True" />
                <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                    ShowGroupPanel="True" ShowPreview="True" ShowTitlePanel="True" />
                <SettingsLoadingPanel Text="Yükleniyor..." />
                <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter"
                    PopupEditFormVerticalAlign="WindowCenter" PopupEditFormModal="true" PopupEditFormWidth="500px" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <AlternatingRow Enabled="True">
                    </AlternatingRow>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                    </Header>
                </Styles>
                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                <Columns>
                    <dxwgv:GridViewCommandColumn Width="90px" VisibleIndex="0" ButtonType="Image">
                        <UpdateButton Visible="True" Text="G&#252;ncelle">
                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif"></Image>
                        </UpdateButton>
                        <DeleteButton Visible="True" Text="Sil">
                            <Image AlternateText="Sil" Url="~/images/delete.gif"></Image>
                        </DeleteButton>
                        <EditButton Visible="True" Text="Deðiþtir">
                            <Image AlternateText="Deðiþtir" Url="~/images/edit.gif"></Image>
                        </EditButton>
                        <CancelButton Visible="True" Text="Ýptal">
                            <Image AlternateText="Ýptal" Url="~/images/delete.gif"></Image>
                        </CancelButton>
                        <NewButton Visible="True" Text="Yeni">
                            <Image AlternateText="Yeni" Url="~/images/new.gif"></Image>
                        </NewButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn Visible="False" FieldName="ID">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Visible="False" FieldName="UlkeID">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn VisibleIndex="1" FieldName="Adi" Width="95%" Caption="Adý">
                        <HeaderStyle ForeColor="#C00000"></HeaderStyle>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn UnboundType="String" Visible="False" FieldName="Filter">
                    </dxwgv:GridViewDataColumn>
                </Columns>
            </dxwgv:ASPxGridView>
        </div>
    </asp:Content>