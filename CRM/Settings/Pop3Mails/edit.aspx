<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="CRM_Settings_Pop3Mails_edit" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxDataView"
    TagPrefix="dxdv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pop3 Mail Listesi</title>
    <script src="../../../utils.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <script src="../../../PreLoad.js" type="text/javascript"></script>
    <script src="../../crm.js" type="text/javascript"></script>
</head>
<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
    <div>
        <model:DataTable ID="DataTableList" runat="server" />
        <model:DataTable ID="DTUsers" runat="server" />
        <asp:HiddenField ID="HiddenID" runat="server" />
        <dxm:ASPxMenu ID="ASPxMenu1" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" OnItemClick="ASPxMenu1_ItemClick" SeparatorHeight="100%" SeparatorWidth="2px"
            ShowPopOutImages="True" ShowSubMenuShadow="False" Width="150px">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="delete" Text="Sil">
                    <Image Url="~/images/delete.gif"></Image>
                </dxm:MenuItem>
            </Items>
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
        </dxm:ASPxMenu>
        <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" ActiveTabIndex="0" TabSpacing="0px"
            Width="750px">
            <ContentStyle>
                <Border BorderColor="#4986A2" />
            </ContentStyle>
            <TabPages>
                <dxtc:TabPage Name="Genel" Text="Genel">
                    <ContentCollection>
                        <dxw:ContentControl ID="ContentControl1" runat="server">
                            <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                CssPostfix="Glass" DataSourceID="DataTableList" OnRowValidating="grid_RowValidating"
                                KeyFieldName="ID" Width="700px" ClientInstanceName="Grid">
                                <SettingsText EmptyDataRow="#" Title="Tanımlı Kişiler" />
                                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                    <AlternatingRow Enabled="True">
                                    </AlternatingRow>
                                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                                    </Header>
                                </Styles>
                                <Settings ShowFilterRow="True" ShowPreview="True" ShowStatusBar="Visible" ShowTitlePanel="True" />
                                <SettingsBehavior ColumnResizeMode="Control" />
                                <SettingsPager PageSize="15" ShowSeparators="True">
                                </SettingsPager>
                                <SettingsCustomizationWindow Enabled="True" />
                                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                </Images>
                                <Columns>
                                    <dxwgv:GridViewCommandColumn VisibleIndex="0" Width="70px" ButtonType="Image">
                                        <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                                            <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
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
                                    <dxwgv:GridViewDataColumn FieldName="ID" ShowInCustomizationForm="false" Visible="False">
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataComboBoxColumn Caption="Kullanıcı Adı" FieldName="UserId">
                                        <PropertiesComboBox DataSourceID="DTUsers" TextField="UserName" ValueField="UserId"
                                            EnableIncrementalFiltering="true" EnableCallbackMode="true" CallbackPageSize="20">
                                        </PropertiesComboBox>
                                    </dxwgv:GridViewDataComboBoxColumn>
                                    <dxwgv:GridViewDataColumn Caption="Adı" FieldName="FirstName">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn Caption="Soyadı" FieldName="LastName">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn Caption="Oluşturan" FieldName="CreatedBy">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataDateColumn Caption="Oluşturma Tarihi" FieldName="CreationDate">
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataDateColumn>
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
            <TabStyle HorizontalAlign="Center">
            </TabStyle>
            <Paddings PaddingLeft="0px" />
        </dxtc:ASPxPageControl>
    </div>
    </form>
</body>
</html>
