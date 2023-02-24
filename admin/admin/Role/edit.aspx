<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="admin_Role_edit" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CRM</title>
    <link rel="stylesheet" type="text/css" href="../../PreLoad.css" />
    <script type="text/javascript" src="../../PreLoad.js"></script>
    <link rel="stylesheet" type="text/css" href="../../ModelCRM.css" />
    <script src="../../utils.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="HiddenID" Visible="false" />
    <model:DataTable ID="DTRoleUsers" runat="server" />
    <model:DataTable ID="DTRolePermissions" runat="server" />
    <model:DataTable ID="DataTableDurumList" runat="server" />
    <model:DataTable ID="DTDurumlar" runat="server" />
    <asp:SqlDataSource ID="DSUser" runat="server" SelectCommand="SELECT UserID, UserName FROM SecurityUsers Where Active=1 ORDER BY UserName"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DSObject" runat="server" SelectCommand="SELECT ObjectID, ObjectDescription FROM SecurityObjects ORDER BY ObjectDescription"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
    <div>
        <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
            ShowSubMenuShadow="False" AutoPostBack="True" Width="100px">
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
                <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat">
                    <Image Url="~/images/saveclose.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="delete" Text="Sil" Visible="false">
                    <Image Url="~/images/delete.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" TabSpacing="0px" Width="750px">
            <TabPages>
                <dxtc:TabPage Text="Genel" Name="Genel">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel5" HeaderText="" runat="server" Width="100%">
                                <PanelCollection>
                                    <dxrp:PanelContent ID="PanelContent4" runat="server">
                                        <table style="width: 100%" cellspacing="2" cellpadding="0" border="0">
                                            <tr>
                                                <td style="width: 110px" valign="top">
                                                    Gündem Atama Durumu
                                                </td>
                                                <td>
                                                    <dxe:ASPxRadioButtonList Border-BorderWidth="0px" ID="IsBildirimKisiAta" runat="server"
                                                        Font-Names="Tahoma" Font-Size="12px" ItemSpacing="3px" RepeatDirection="vertical">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Merkeze Atayabilir" Value="0" />
                                                            <dxe:ListEditItem Text="Herkese Atayabilir" Value="1" />
                                                        </Items>
                                                        <Paddings Padding="0px" PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px"
                                                            PaddingTop="0px" />
                                                    </dxe:ASPxRadioButtonList>
                                                </td>
                                                <td style="width: 110px" valign="top">
                                                    Kiþiye Özel Uyarý Durumu
                                                </td>
                                                <td>
                                                    <dxe:ASPxRadioButtonList Border-BorderWidth="0px" ID="IsPersonalizedPost" runat="server"
                                                        Font-Names="Tahoma" Font-Size="12px" ItemSpacing="3px" RepeatDirection="vertical">
                                                        <Items>
                                                            <dxe:ListEditItem Text="Kiþiye Özel Uyarý Yapamaz" Value="0" />
                                                            <dxe:ListEditItem Text="Kiþiye Özel Uyarý Yapabilir" Value="1" />
                                                        </Items>
                                                        <Paddings Padding="0px" PaddingBottom="0px" PaddingLeft="0px" PaddingRight="0px"
                                                            PaddingTop="0px" />
                                                    </dxe:ASPxRadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 110px" valign="top">
                                                    Yazýþma Rengi
                                                </td>
                                                <td>
                                                    <%--<dxe:ASPxTextBox ReadOnly="true" ID="YaziRengi" runat="server" />
                                                    <img id="Btn_MalzemeKodu" src="../../../images/search_button.jpg" alt="" width="22px"
                                                        height="19px" onclick="ComboButtonClick('YaziRengi')" style="cursor: pointer" />--%>
                                                    <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                        ID="YaziRenkleriID" EnableIncrementalFiltering="True" EnableCallbackMode="true"
                                                        CallbackPageSize="15">
                                                        <ButtonStyle Width="13px" Cursor="pointer">
                                                        </ButtonStyle>
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="width: 110px" valign="top">
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxrp:PanelContent>
                                </PanelCollection>
                            </dxrp:ASPxRoundPanel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Kullanýcýlar" Name="TabPageKullanici">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" HeaderText="" runat="server" Width="100%">
                                <PanelCollection>
                                    <dxrp:PanelContent ID="PanelContent1" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top">
                                                    <dxwgv:ASPxGridView ID="GridRoleUsers" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DTRoleUsers"
                                                        KeyFieldName="ID" Width="100%" OnRowValidating="GridRoleUsers_RowValidating"
                                                        ClientInstanceName="GridRoleUsers" OnCustomJSProperties="Grid_CustomJSProperties">
                                                        <Columns>
                                                            <dxwgv:GridViewCommandColumn Width="90px" VisibleIndex="0" ButtonType="Image">
                                                                <ClearFilterButton Visible="True" Text="Süzme Ýptal">
                                                                    <Image AlternateText="Süzme Ýptal" Url="~/images/reload2.jpg" />
                                                                </ClearFilterButton>
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
                                                            <dxwgv:GridViewDataColumn FieldName="UserRoleID" Visible="False" />
                                                            <dxwgv:GridViewDataColumn FieldName="RoleID" Visible="False" />
                                                            <dxwgv:GridViewDataComboBoxColumn Caption="Kullanýcý" FieldName="UserID" Width="175px">
                                                                <HeaderStyle ForeColor="#C00000" />
                                                                <PropertiesComboBox TextField="UserName" ValueField="UserID" DataSourceID="DSUser"
                                                                    ValueType="System.Guid" EnableIncrementalFiltering="true">
                                                                </PropertiesComboBox>
                                                            </dxwgv:GridViewDataComboBoxColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Kaydeden" FieldName="CreatedBy">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreatedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataDateColumn Caption="Kayýt Tarihi" FieldName="CreationDate">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreationDate")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataDateColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Deðiþtiren" FieldName="ModifiedBy">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreatedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataDateColumn Caption="Deðiþiklik Tarihi" FieldName="ModificationDate">
                                                                <EditItemTemplate>
                                                                    <%# Eval("ModificationDate")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataDateColumn>
                                                        </Columns>
                                                        <SettingsText Title="Rol Kullanýcý Bilgileri" ConfirmDelete="Kayýt silinsin mi?"
                                                            EmptyDataRow="Yeni satýr ekle" />
                                                        <SettingsPager PageSize="15" ShowSeparators="True">
                                                        </SettingsPager>
                                                        <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                                        </Images>
                                                        <SettingsCustomizationWindow Enabled="True" />
                                                        <Settings ShowPreview="True" ShowTitlePanel="True" />
                                                        <SettingsLoadingPanel Text="Yükleniyor..." />
                                                        <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                                            <AlternatingRow Enabled="True">
                                                            </AlternatingRow>
                                                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                                            </Header>
                                                        </Styles>
                                                        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                                        <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                                                            PopupEditFormModal="true" PopupEditFormWidth="500px" />
                                                    </dxwgv:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxrp:PanelContent>
                                </PanelCollection>
                            </dxrp:ASPxRoundPanel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Ýzinler" Name="TabPageIzin">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel3" HeaderText="" runat="server" Width="100%">
                                <PanelCollection>
                                    <dxrp:PanelContent ID="PanelContent2" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top">
                                                    <dxwgv:ASPxGridView ID="GridRolePermissions" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DTRolePermissions"
                                                        KeyFieldName="ID" Width="1000px" OnRowInserting="GridRolePermissions_RowInserting"
                                                        OnRowUpdating="GridRolePermissions_RowUpdating" OnRowValidating="GridRolePermissions_RowValidating"
                                                        ClientInstanceName="GridRolePermissions" OnCustomJSProperties="Grid_CustomJSProperties">
                                                        <Columns>
                                                            <dxwgv:GridViewCommandColumn Width="90px" VisibleIndex="0" ButtonType="Image">
                                                                <ClearFilterButton Visible="True" Text="Süzme Ýptal">
                                                                    <Image AlternateText="Süzme Ýptal" Url="~/images/reload2.jpg" />
                                                                </ClearFilterButton>
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
                                                            <dxwgv:GridViewDataColumn FieldName="RolePermissionID" Visible="False" />
                                                            <dxwgv:GridViewDataColumn FieldName="RoleID" Visible="False" />
                                                            <dxwgv:GridViewDataComboBoxColumn Caption="Nesne" FieldName="ObjectID" Width="200px">
                                                                <HeaderStyle ForeColor="#C00000" />
                                                                <PropertiesComboBox TextField="ObjectDescription" ValueField="ObjectID" DataSourceID="DSObject"
                                                                    ValueType="System.Guid" EnableIncrementalFiltering="true">
                                                                </PropertiesComboBox>
                                                            </dxwgv:GridViewDataComboBoxColumn>
                                                            <dxwgv:GridViewDataCheckColumn Caption="Görme" FieldName="Select" Width="60px">
                                                                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                                                                    DisplayTextChecked="Evet" DisplayTextUnchecked="Hayýr">
                                                                </PropertiesCheckEdit>
                                                            </dxwgv:GridViewDataCheckColumn>
                                                            <dxwgv:GridViewDataCheckColumn Caption="Kaydetme" FieldName="Insert" Width="60px">
                                                                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                                                                    DisplayTextChecked="Evet" DisplayTextUnchecked="Hayýr">
                                                                </PropertiesCheckEdit>
                                                            </dxwgv:GridViewDataCheckColumn>
                                                            <dxwgv:GridViewDataCheckColumn Caption="Güncelleme" FieldName="Update" Width="60px">
                                                                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                                                                    DisplayTextChecked="Evet" DisplayTextUnchecked="Hayýr">
                                                                </PropertiesCheckEdit>
                                                            </dxwgv:GridViewDataCheckColumn>
                                                            <dxwgv:GridViewDataCheckColumn Caption="Silme" FieldName="Delete" Width="60px">
                                                                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                                                                    DisplayTextChecked="Evet" DisplayTextUnchecked="Hayýr">
                                                                </PropertiesCheckEdit>
                                                            </dxwgv:GridViewDataCheckColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Kaydeden" FieldName="CreatedBy" Width="100px">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreatedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataDateColumn Caption="Kayýt Tarihi" FieldName="CreationDate" Width="100px">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreationDate")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataDateColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Deðiþtiren" FieldName="ModifiedBy" Width="100px">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreatedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataDateColumn Caption="Deðiþiklik Tarihi" FieldName="ModificationDate"
                                                                Width="100px">
                                                                <EditItemTemplate>
                                                                    <%# Eval("ModificationDate")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataDateColumn>
                                                        </Columns>
                                                        <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                                        </Images>
                                                        <Settings ShowFilterRow="false" ShowStatusBar="Hidden" ShowGroupedColumns="False"
                                                            ShowGroupPanel="False" ShowPreview="True" ShowTitlePanel="false" ShowVerticalScrollBar="False" />
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
                                                        <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                                                            PopupEditFormModal="true" PopupEditFormWidth="500px" />
                                                        <SettingsText Title="Rol Ýzin Bilgileri" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya sürükleyiniz."
                                                            ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
                                                    </dxwgv:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxrp:PanelContent>
                                </PanelCollection>
                            </dxrp:ASPxRoundPanel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Gündem Durumu Ýzinleri" Name="TabPageBildirimDurumu">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" HeaderText="" runat="server" Width="100%">
                                <PanelCollection>
                                    <dxrp:PanelContent ID="PanelContent3" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top">
                                                    <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                        CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DataTableDurumList"
                                                        Width="100%" ID="GRD_DurumList" OnRowInserting="GRD_DurumList_RowInserting" OnRowUpdating="GRD_DurumList_RowUpdating"
                                                        OnRowValidating="GRD_DurumList_RowValidating" OnCellEditorInitialize="GRD_DurumList_CellEditorInitialize">
                                                        <SettingsText Title="Kullanýcý Tipleri" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                                                            ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
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
                                                            <dxwgv:GridViewCommandColumn ButtonType="Image" Width="60px" VisibleIndex="0">
                                                                <NewButton Visible="True" Text="Yeni">
                                                                    <Image AlternateText="Yeni" Url="~/images/new.gif"></Image>
                                                                </NewButton>
                                                                <DeleteButton Visible="True" Text="Sil">
                                                                    <Image AlternateText="Sil" Url="~/images/delete.gif"></Image>
                                                                </DeleteButton>
                                                                <CancelButton Visible="True" Text="Ýptal">
                                                                    <Image AlternateText="Ýptal" Url="~/images/delete.gif"></Image>
                                                                </CancelButton>
                                                                <UpdateButton Visible="True" Text="Güncelle">
                                                                    <Image AlternateText="Güncelle" Url="~/images/update.gif"></Image>
                                                                </UpdateButton>
                                                            </dxwgv:GridViewCommandColumn>
                                                            <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataColumn FieldName="UserProfileDurumListID" Visible="False">
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataComboBoxColumn FieldName="DurumID" Caption="Gündem Durumu Adý"
                                                                Width="175px">
                                                                <PropertiesComboBox ValueField="DurumID" TextField="DurumName" DataSourceID="DTDurumlar"
                                                                    ValueType="System.Guid" EnableIncrementalFiltering="true" EnableCallbackMode="true"
                                                                    CallbackPageSize="15">
                                                                </PropertiesComboBox>
                                                                <HeaderStyle ForeColor="#C00000"></HeaderStyle>
                                                            </dxwgv:GridViewDataComboBoxColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreatedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
                                                                <EditItemTemplate>
                                                                    <%# Eval("ModifiedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluþturma Tarihi"
                                                                EditFormSettings-Visible="False" FieldName="CreationDate">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreationDate")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataDateColumn>
                                                            <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Düzenlenme Tarihi"
                                                                EditFormSettings-Visible="False" FieldName="ModificationDate">
                                                                <EditItemTemplate>
                                                                    <%# Eval("ModificationDate")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataDateColumn>
                                                            <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="False">
                                                            </dxwgv:GridViewDataColumn>
                                                        </Columns>
                                                    </dxwgv:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxrp:PanelContent>
                                </PanelCollection>
                            </dxrp:ASPxRoundPanel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </div>
    </form>
</body>
</html>
