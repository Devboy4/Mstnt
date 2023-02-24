<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="admin_Role_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <model:DataTable ID="DataTableRoles" runat="server" />
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
                        <Image Url="~/images/new.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <Image Url="~/images/save.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni" Visible="false">
                        <Image Url="~/images/savenew.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat" Visible="false">
                        <Image Url="~/images/saveclose.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="delete" Text="Sil" Visible="false">
                        <Image Url="~/images/delete.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Text=" ">
                    </dxm:MenuItem>
                    <dxm:MenuItem Text="Yenile" NavigateUrl="javascript:location.reload(true);">
                        <Image Url="~/images/reload2.jpg" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DataTableRoles" KeyFieldName="ID" Width="100%"
                OnRowValidating="grid_RowValidating" ClientInstanceName="Grid" OnCustomJSProperties="Grid_CustomJSProperties">
                <SettingsText Title="Rol Bilgileri" ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="Yeni satır ekle" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <SettingsCustomizationWindow Enabled="True" />
                <Settings ShowPreview="True" ShowTitlePanel="True" />
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
                        <ClearFilterButton Visible="True" Text="Süzme İptal">
                            <Image AlternateText="Süzme İptal" Url="~/images/reload2.jpg" />
                        </ClearFilterButton>
                        <NewButton Visible="True" Text="Yeni">
                            <Image AlternateText="Yeni" Url="~/images/new.gif" />
                        </NewButton>
                        <EditButton Visible="True" Text="Değiştir">
                            <Image AlternateText="Değiştir" Url="~/images/edit.gif" />
                        </EditButton>
                        <UpdateButton Visible="True" Text="Güncelle">
                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                        </UpdateButton>
                        <DeleteButton Visible="True" Text="Sil">
                            <Image AlternateText="Sil" Url="~/images/delete.gif" />
                        </DeleteButton>
                        <CancelButton Visible="True" Text="İptal">
                            <Image AlternateText="İptal" Url="~/images/delete.gif" />
                        </CancelButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                    <dxwgv:GridViewDataColumn FieldName="RoleID" Visible="False" />
                    <dxwgv:GridViewDataColumn Caption="Rol" FieldName="Role" Width="100px">
                        <HeaderStyle ForeColor="#C00000" />
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink ID="ASPxHyperLink1" Font-Size="8pt" Font-Names="Arial" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("RoleID")+"',800,400);PopWin.focus();"%>
                                Text='<%#Eval("Role")%>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Oluşturan" FieldName="CreatedBy" Width="100px">
                        <EditItemTemplate>
                            <%# Eval("CreatedBy")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataDateColumn Caption="Oluşturulma Tarihi" FieldName="CreationDate"
                        Width="100px">
                        <EditItemTemplate>
                            <%# Eval("CreationDate")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn Caption="Değiştiren" FieldName="ModifiedBy" Width="100px">
                        <EditItemTemplate>
                            <%# Eval("CreatedBy")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataDateColumn Caption="Değişiklik Tarihi" FieldName="ModificationDate"
                        Width="100px">
                        <EditItemTemplate>
                            <%# Eval("ModificationDate")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                </Columns>
            </dxwgv:ASPxGridView>
        </div>
    </asp:Content>
