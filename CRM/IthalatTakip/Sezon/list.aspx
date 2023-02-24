<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="CRM_IthalatTakip_Tanim_Sezon_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                    <dxm:MenuItem Name="new" Text="Yeni" Visible="false">
                        <TextTemplate>
                            Yeni</TextTemplate>
                        <Image Url="~/images/new.gif" />
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
                </Items>
            </dxm:ASPxMenu>
            <dxwgv:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DTList" KeyFieldName="ID" Width="100%" SettingsEditing-Mode="Inline"
                OnRowValidating="Grid_RowValidating" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating"
                ClientInstanceName="Grid" OnCustomJSProperties="Grid_CustomJSProperties">
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
                    <dxwgv:GridViewDataColumn Caption="Sezon" FieldName="Sezon" Width="30%" />
                    <dxwgv:GridViewDataMemoColumn Caption="Açýklama" FieldName="Aciklama" Width="70%" />
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
                <SettingsText Title="Sezonlar" EmptyDataRow="Yeni satýr ekle" />
            </dxwgv:ASPxGridView>
        </div>
  </asp:Content>
