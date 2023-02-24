<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="CRM_Settings_BrDurum_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallbackPanel"
    TagPrefix="dxcp" %>
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
    <script src="edit.js" type="text/javascript"></script>
        <div>
            <model:DataTable ID="DataTableList" runat="server" />
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
                CssPostfix="Glass" DataSourceID="DataTableList" ClientInstanceName="Grid" KeyFieldName="ID" Width="100%"
                OnRowValidating="grid_RowValidating" OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating">
                <SettingsText Title="Br Markalar Listesi" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                    ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                    ShowGroupPanel="True" ShowPreview="True" ShowTitlePanel="True" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <AlternatingRow Enabled="True">
                    </AlternatingRow>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                    </Header>
                </Styles>
                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                <SettingsLoadingPanel Text="Yükleniyor..." />
                <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                    PopupEditFormModal="true" PopupEditFormWidth="500px" />
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
                    <dxwgv:GridViewDataColumn FieldName="BrDurumID" Visible="False">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="Adi" Caption="Adý">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="YaziRengi" Caption="Renk Kodu" Width="50px"
                        Visible="true" ReadOnly="true">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Name="Butonmalzeme_kodu" Caption="" Width="15px" Visible="true">
                        <DataItemTemplate>
                            <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                        </DataItemTemplate>
                        <EditItemTemplate>
                            <img id="Btn_MalzemeKodu" src="../../../images/search_button.jpg" alt="" width="22px"
                                height="19px" onclick="ComboButtonClick('YaziRengi')" style="cursor: pointer" />
                        </EditItemTemplate>
                        <EditFormSettings Visible="True" />
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataMemoColumn Caption="Açýklama" FieldName="Description">
                        <EditFormSettings ColumnSpan="2" />
                    </dxwgv:GridViewDataMemoColumn>
                    <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluþturma Tarihi"
                        FieldName="CreationDate" EditFormSettings-Visible="False">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Düzenlenme Tarihi"
                        FieldName="ModificationDate" EditFormSettings-Visible="False">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="False">
                    </dxwgv:GridViewDataColumn>
                </Columns>
            </dxwgv:ASPxGridView>
            <dxcb:ASPxCallback ID="CallbackGenel" runat="server" ClientInstanceName="CallbackGenel"
                OnCallback="CallbackGenel_Callback">
                <ClientSideEvents CallbackComplete="function(s, e) {CallbackGenelComplete(e.result);}" />
            </dxcb:ASPxCallback>
            <dxcb:ASPxCallback ID="CallbackSearchBrowser" runat="server" ClientInstanceName="CallbackSearchBrowser"
                OnCallback="CallbackSearchBrowser_Callback">
                <ClientSideEvents CallbackComplete="function(s, e) { SearchBrowserCallbackComplete(e); }"
                    EndCallback="function(s, e) { SearchBrowserEndCallback(); }" />
            </dxcb:ASPxCallback>
        </div>
  </asp:Content>
