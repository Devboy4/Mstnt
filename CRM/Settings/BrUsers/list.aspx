<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="CRM_Settings_BrUsers_list" %>

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
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div>
            <model:DataTable ID="DataTableList" runat="server" />
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
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <Image Url="~/images/save.gif" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="100%"
                OnRowValidating="grid_RowValidating" OnRowInserting="grid_RowInserting" OnCellEditorInitialize="grid_CellEditorInitialize" OnRowUpdating="grid_RowUpdating">
                <SettingsText Title="Br Yöneticileri" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                    ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <SettingsCustomizationWindow Enabled="True" />
                <SettingsLoadingPanel Text="Yükleniyor..." />
                <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                    PopupEditFormModal="true" PopupEditFormWidth="500px" />
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
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataComboBoxColumn Caption="Yönetici" FieldName="UserID">
                        <HeaderStyle ForeColor="#C00000" />
                        <PropertiesComboBox TextField="UserName" ClientInstanceName="cmbUserID" ValueField="UserID"
                            DataSourceID="DTUsers" ValueType="System.Guid" EnableIncrementalFiltering="true"
                            EnableCallbackMode="true" CallbackPageSize="15">
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataMemoColumn Caption="Açýklama" FieldName="Description">
                        <EditFormSettings ColumnSpan="2" />
                    </dxwgv:GridViewDataMemoColumn>
                    <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
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
                    <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="False">
                    </dxwgv:GridViewDataColumn>
                </Columns>
            </dxwgv:ASPxGridView>
        </div>
  </asp:Content>
