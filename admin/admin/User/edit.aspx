<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="admin_User_edit" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CRM</title>
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />
    <script type="text/javascript" src="../../../PreLoad.js"></script>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <script src="../../../utils.js" type="text/javascript"></script>
    <script src="./edit.js?v=1" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:HiddenField runat="server" ID="HiddenID" Visible="false" />
    <asp:HiddenField runat="server" ID="HiddenUserName" Visible="false" />
    <model:DataTable ID="DTUserRoles" runat="server" />
    <model:DataTable ID="DTUserPermissions" runat="server" />
    <model:DataTable ID="DTUserGrup" runat="server" />
    <model:DataTable ID="DTUserGrupList" runat="server" />
    <model:DataTable ID="DTProje" runat="server" />
    <model:DataTable ID="DTAllowedProject" runat="server" />
    <model:DataTable ID="DTSesEmail" runat="server" />
    <asp:SqlDataSource ID="DSRole" runat="server" SelectCommand="SELECT RoleID, Role FROM SecurityRoles ORDER BY Role"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DSFirma" runat="server" SelectCommand="SELECT FirmaID, FirmaName FROM Firma ORDER BY FirmaName"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DSProje" runat="server" SelectCommand="SELECT ProjeID, Adi AS ProjeName FROM Proje ORDER BY Adi"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="DSObject" runat="server" SelectCommand="SELECT ObjectID, ObjectDescription FROM SecurityObjects ORDER BY ObjectDescription"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
    <dxcb:ASPxCallback ID="CallbackSearchBrowser" runat="server" ClientInstanceName="CallbackSearchBrowser"
        OnCallback="CallbackSearchBrowser_Callback">
        <ClientSideEvents CallbackComplete="function(s, e) { SearchBrowserCallbackComplete(e); }"
            EndCallback="function(s, e) { SearchBrowserEndCallback(); }" />
    </dxcb:ASPxCallback>
    <dxcb:ASPxCallback ID="CallbackGenel" runat="server" ClientInstanceName="CallbackGenel"
        OnCallback="CallbackGenel_Callback">
        <ClientSideEvents CallbackComplete="function(s, e) {CallbackGenelComplete(e.result);}" />
    </dxcb:ASPxCallback>
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
                <dxm:MenuItem Name="new" Text="Yeni" Visible="true">
                    <Image Url="~/images/new.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="save" Text="Kaydet" Visible="true">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni" Visible="true">
                    <Image Url="~/images/savenew.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat" Visible="true">
                    <Image Url="~/images/saveclose.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="changeusername" Text="Kullanıcı Adını Değiştir" Visible="true">
                    <Image Url="~/images/Menu_Users.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="delete" Text="Kullanıcıyı Sil" Visible="true">
                    <Image Url="~/images/delete.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="Passive" Text="Aktif / Pasif" Visible="true">
                    <Image Url="~/images/reload2.jpg" />
                </dxm:MenuItem>
            </Items>
            <ClientSideEvents ItemClick="function(s, e) { Menu_ItemClick(s,e); }" />
        </dxm:ASPxMenu>
        <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" TabSpacing="0px" Width="750px">
            <TabPages>
                <dxtc:TabPage Text="Genel" Name="TabPageGenel">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="SingleParagraph"
                                HeaderText="UYARI : " ShowMessageBox="false" ShowSummary="true" Font-Size="10px"
                                Font-Bold="true" />
                            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" HeaderText="" runat="server" Width="100%">
                                <PanelCollection>
                                    <dxrp:PanelContent ID="PanelContent1" runat="server">
                                        <table id="Table2" cellpadding="1" border="0" cellspacing="3" style="width: 100%">
                                            <tr>
                                                <td valign="top">
                                                    <span style="color: #c00000">Kullanıcı Adı</span>
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="Username" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                                <td style="width: 96px" valign="top">
                                                    <span style="color: #c00000">Şifre</span>
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="Password" runat="server" Width="175px" Password="True">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <span style="color: #c00000">Ad</span>
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="FirstName" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                                <td style="width: 96px" valign="top">
                                                    <span style="color: #c00000">Soyad</span>
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="LastName" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    <span style="color: #c00000">E-Posta</span>
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="Email" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                                <td style="width: 96px" valign="top">
                                                    Bölüm
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="Department" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    Öndeğer İlgili Birim
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                        ID="FirmaID" EnableIncrementalFiltering="True" EnableCallbackMode="true" CallbackPageSize="15">
                                                        <ButtonStyle Width="13px" Cursor="pointer">
                                                        </ButtonStyle>
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e){ cmbProjeID.PerformCallback(s.GetValue()); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td style="width: 96px" valign="top">
                                                    Öndeğer Departman
                                                </td>
                                                <td>
                                                    <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                        ID="ProjeID" ClientInstanceName="cmbProjeID" OnCallback="ProjeID_Callback" EnableIncrementalFiltering="True"
                                                        EnableCallbackMode="true" CallbackPageSize="15">
                                                        <ButtonStyle Width="13px" Cursor="pointer">
                                                        </ButtonStyle>
                                                    </dxe:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    Cep
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="CepTel" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                                <td style="width: 96px" valign="top">
                                                    Takip Raporu
                                                </td>
                                                <td>
                                                    <dxe:ASPxCheckBox runat="server" Text="Yapılmadıya düşenleri al" ID="Savsaklamaal"
                                                        Checked="false">
                                                    </dxe:ASPxCheckBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    IP Santral
                                                    <br />
                                                    Kullanıcı Adı
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="IPUserName" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                                <td style="width: 96px" valign="top">
                                                    IP Santral
                                                    <br />
                                                    Şifre
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="IPPassword" Password="true" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top">
                                                    IP Santral
                                                    <br />
                                                    Dahili No
                                                </td>
                                                <td>
                                                    <dxe:ASPxTextBox ID="IPDahili" runat="server" Width="175px">
                                                    </dxe:ASPxTextBox>
                                                </td>
                                                <td style="width: 96px" valign="top">
                                                </td>
                                                <td>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4" valign="top">
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="UserName"
                                                        Font-Size="10px"></asp:RequiredFieldValidator><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                            Display="None" runat="server" ControlToValidate="PassWord" ErrorMessage="Şifre Adı alanı boş ge&#231;ilemez!"
                                                            Font-Bold="True" Font-Size="10px"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                                                ID="RequiredFieldValidator3" Display="None" runat="server" ControlToValidate="FirstName"
                                                                ErrorMessage="Adı alanı boş ge&#231;ilemez!" Font-Bold="True" Font-Size="10px"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                                                    ID="RequiredFieldValidator4" Display="None" runat="server" ControlToValidate="LastName"
                                                                    ErrorMessage="SoyAdı alanı boş ge&#231;ilemez!" Font-Bold="True" Font-Size="10px"></asp:RequiredFieldValidator><asp:RequiredFieldValidator
                                                                        ID="RequiredFieldValidator5" Display="None" runat="server" ControlToValidate="Email"
                                                                        ErrorMessage="E-Posta alanı boş ge&#231;ilemez!" Font-Bold="True" Font-Size="10px"></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                        </table>
                                    </dxrp:PanelContent>
                                </PanelCollection>
                            </dxrp:ASPxRoundPanel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Roller" Name="TabPageRol">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" HeaderText="" runat="server" Width="100%">
                                <PanelCollection>
                                    <dxrp:PanelContent ID="PanelContent2" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top">
                                                    <dxwgv:ASPxGridView ID="GridUserRoles" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DTUserRoles"
                                                        KeyFieldName="ID" Width="100%" OnRowValidating="GridUserRoles_RowValidating"
                                                        ClientInstanceName="GridUserRoles" OnCustomJSProperties="Grid_CustomJSProperties">
                                                        <SettingsText Title="Kullanıcı  Rol Bilgileri" ConfirmDelete="Kayıt silinsin mi?"
                                                            EmptyDataRow="Yeni satır ekle" />
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
                                                            <dxwgv:GridViewCommandColumn Width="90px" VisibleIndex="0" ButtonType="Image">
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
                                                            <dxwgv:GridViewDataColumn FieldName="UserRoleID" Visible="False" />
                                                            <dxwgv:GridViewDataColumn FieldName="UserID" Visible="False" />
                                                            <dxwgv:GridViewDataComboBoxColumn Caption="Rol" FieldName="RoleID" Width="160px">
                                                                <HeaderStyle ForeColor="#C00000" />
                                                                <PropertiesComboBox TextField="Role" ValueField="RoleID" DataSourceID="DSRole" ValueType="System.Guid"
                                                                    EnableIncrementalFiltering="true">
                                                                </PropertiesComboBox>
                                                            </dxwgv:GridViewDataComboBoxColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Kaydeden" FieldName="CreatedBy" Width="100px"
                                                                EditFormSettings-Visible="False">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreatedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataDateColumn Caption="Kayıt Tarihi" FieldName="CreationDate" Width="100px"
                                                                EditFormSettings-Visible="False">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreationDate")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataDateColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Değiştiren" FieldName="ModifiedBy" Width="100px"
                                                                EditFormSettings-Visible="False">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreatedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataDateColumn Caption="Değişiklik Tarihi" FieldName="ModificationDate"
                                                                EditFormSettings-Visible="False" Width="100px">
                                                                <EditFormSettings Visible="False"></EditFormSettings>
                                                                <EditItemTemplate>
                                                                    <%# Eval("ModificationDate")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataDateColumn>
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
                <dxtc:TabPage Text="Kişiye Özel İzinler" Name="TabPageIzin">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel3" HeaderText="" runat="server" Width="100%">
                                <PanelCollection>
                                    <dxrp:PanelContent ID="PanelContent3" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top">
                                                    <dxwgv:ASPxGridView ID="GridUserPermissions" runat="server" AutoGenerateColumns="False"
                                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DTUserPermissions"
                                                        KeyFieldName="ID" Width="1000px" OnRowInserting="GridUserPermissions_RowInserting"
                                                        OnRowUpdating="GridUserPermissions_RowUpdating" OnRowValidating="GridUserPermissions_RowValidating"
                                                        ClientInstanceName="GridUserPermissions" OnCustomJSProperties="Grid_CustomJSProperties">
                                                        <SettingsText Title="Kullanıcı İzin Bilgileri" ConfirmDelete="Kayıt silinsin mi?"
                                                            EmptyDataRow="Yeni satır ekle" />
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
                                                            <dxwgv:GridViewCommandColumn Width="90px" VisibleIndex="0" ButtonType="Image">
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
                                                            <dxwgv:GridViewDataColumn FieldName="UserPermissionID" Visible="False" />
                                                            <dxwgv:GridViewDataColumn FieldName="UserID" Visible="False" />
                                                            <dxwgv:GridViewDataComboBoxColumn Caption="Nesne" FieldName="ObjectID" Width="200px">
                                                                <HeaderStyle ForeColor="#C00000" />
                                                                <PropertiesComboBox TextField="ObjectDescription" ValueField="ObjectID" DataSourceID="DSObject"
                                                                    ValueType="System.Guid" EnableIncrementalFiltering="true">
                                                                </PropertiesComboBox>
                                                            </dxwgv:GridViewDataComboBoxColumn>
                                                            <dxwgv:GridViewDataCheckColumn Caption="Görme" FieldName="Select" Width="60px">
                                                                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                                                                    DisplayTextChecked="Evet" DisplayTextUnchecked="Hayır">
                                                                </PropertiesCheckEdit>
                                                            </dxwgv:GridViewDataCheckColumn>
                                                            <dxwgv:GridViewDataCheckColumn Caption="Kaydetme" FieldName="Insert" Width="60px">
                                                                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                                                                    DisplayTextChecked="Evet" DisplayTextUnchecked="Hayır">
                                                                </PropertiesCheckEdit>
                                                            </dxwgv:GridViewDataCheckColumn>
                                                            <dxwgv:GridViewDataCheckColumn Caption="Güncelleme" FieldName="Update" Width="60px">
                                                                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                                                                    DisplayTextChecked="Evet" DisplayTextUnchecked="Hayır">
                                                                </PropertiesCheckEdit>
                                                            </dxwgv:GridViewDataCheckColumn>
                                                            <dxwgv:GridViewDataCheckColumn Caption="Silme" FieldName="Delete" Width="60px">
                                                                <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32"
                                                                    DisplayTextChecked="Evet" DisplayTextUnchecked="Hayır">
                                                                </PropertiesCheckEdit>
                                                            </dxwgv:GridViewDataCheckColumn>
                                                            <dxwgv:GridViewDataColumn Caption="Kaydeden" FieldName="CreatedBy" Width="100px">
                                                                <EditItemTemplate>
                                                                    <%# Eval("CreatedBy")%>
                                                                </EditItemTemplate>
                                                            </dxwgv:GridViewDataColumn>
                                                            <dxwgv:GridViewDataDateColumn Caption="Kayıt Tarihi" FieldName="CreationDate" Width="100px">
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
                                                </td>
                                            </tr>
                                        </table>
                                    </dxrp:PanelContent>
                                </PanelCollection>
                            </dxrp:ASPxRoundPanel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <%--<dxtc:TabPage Text="Kullanıcı İzinli Grupları" Name="TabPageUserGrup">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxrp:ASPxRoundPanel ID="ASPxRoundPanel4" HeaderText="" runat="server" Width="740px">
                                    <PanelCollection>
                                        <dxrp:PanelContent ID="PanelContent5" runat="server">
                                            <dxwgv:ASPxGridView ID="gridUserGrupList" runat="server" AutoGenerateColumns="False"
                                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DTUserGrupList"
                                                KeyFieldName="ID" Width="700px" OnRowValidating="gridUserGrupList_RowValidating"
                                                ClientInstanceName="gridUserGrupList" OnCellEditorInitialize="gridUserGrupList_CellEditorInitialize">
                                                <SettingsText Title="Kullanıcı / Departman Bağlantıları" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
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
                                                    <dxwgv:GridViewCommandColumn Width="50px" VisibleIndex="0" ButtonType="Image">
                                                        <NewButton Visible="True" Text="Yeni">
                                                            <Image AlternateText="Yeni" Url="~/images/new.gif" />
                                                        </NewButton>
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
                                                    <dxwgv:GridViewDataColumn FieldName="UserGrupID" Visible="False" />
                                                    <dxwgv:GridViewDataComboBoxColumn Caption="Kullanıcı Grubu" FieldName="UserGrupID">
                                                        <HeaderStyle ForeColor="#C00000" />
                                                        <PropertiesComboBox TextField="UserGrupName" ClientInstanceName="cmbUserGrupID" ValueField="UserGrupID"
                                                            DataSourceID="DTUserGrup" ValueType="System.Guid" EnableIncrementalFiltering="true"
                                                            EnableCallbackMode="true" CallbackPageSize="15">
                                                        </PropertiesComboBox>
                                                    </dxwgv:GridViewDataComboBoxColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy">
                                                        <EditFormSettings Visible="False" />
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluşturma Tarihi"
                                                        FieldName="CreationDate">
                                                        <EditFormSettings Visible="False" />
                                                    </dxwgv:GridViewDataDateColumn>
                                                </Columns>
                                            </dxwgv:ASPxGridView>
                                        </dxrp:PanelContent>
                                    </PanelCollection>
                                </dxrp:ASPxRoundPanel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>--%>
                <dxtc:TabPage Text="Kullanıcı İzinli Departmanlar" Name="TabPageAllowedProject">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel5" HeaderText="" runat="server" Width="740px">
                                <PanelCollection>
                                    <dxrp:PanelContent ID="PanelContent4" runat="server">
                                        <dxwgv:ASPxGridView ID="gridAllowedProject" runat="server" AutoGenerateColumns="False"
                                            CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DTAllowedProject"
                                            KeyFieldName="ID" Width="700px" OnRowValidating="gridAllowedProject_RowValidating"
                                            ClientInstanceName="gridAllowedProject" OnCellEditorInitialize="gridAllowedProject_CellEditorInitialize">
                                            <SettingsText Title="Departman Listesi" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
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
                                                <dxwgv:GridViewCommandColumn Width="50px" VisibleIndex="0" ButtonType="Image">
                                                    <NewButton Visible="True" Text="Yeni">
                                                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                                                    </NewButton>
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
                                                <dxwgv:GridViewDataColumn FieldName="FirmaName" Caption="İlgili Birim">
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataComboBoxColumn Caption="Departman" FieldName="ProjeID" Width="175px">
                                                    <HeaderStyle ForeColor="#C00000" />
                                                    <PropertiesComboBox TextField="ProjeName" ClientInstanceName="cmbProjeID" ValueField="ProjeID"
                                                        DataSourceID="DTProje" ValueType="System.Guid" EnableIncrementalFiltering="true"
                                                        EnableCallbackMode="true" CallbackPageSize="15">
                                                    </PropertiesComboBox>
                                                </dxwgv:GridViewDataComboBoxColumn>
                                                <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy">
                                                    <EditFormSettings Visible="False" />
                                                </dxwgv:GridViewDataColumn>
                                                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluşturma Tarihi"
                                                    FieldName="CreationDate">
                                                    <PropertiesDateEdit EditFormatString="dd.MM.yyyy">
                                                    </PropertiesDateEdit>
                                                    <EditFormSettings Visible="False" />
                                                </dxwgv:GridViewDataDateColumn>
                                            </Columns>
                                        </dxwgv:ASPxGridView>
                                    </dxrp:PanelContent>
                                </PanelCollection>
                            </dxrp:ASPxRoundPanel>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
                <dxtc:TabPage Text="Ses Gönderilen E-Postalar" Name="TabSesEmail">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <dxwgv:ASPxGridView ID="GridSesEmail" runat="server" AutoGenerateColumns="False"
                                CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" DataSourceID="DTSesEmail"
                                KeyFieldName="ID" Width="400px" ClientInstanceName="GridSesEmail" OnRowValidating="GridSesEmail_RowValidating">
                                <SettingsText Title="Departman Listesi" EmptyDataRow="Yeni satır ekle" />
                                <SettingsPager PageSize="15" ShowSeparators="True">
                                </SettingsPager>
                                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                </Images>
                                <Settings ShowPreview="True" ShowTitlePanel="false" />
                                <SettingsEditing Mode="inline" />
                                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                    </Header>
                                </Styles>
                                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                <Columns>
                                    <dxwgv:GridViewCommandColumn Width="50px" VisibleIndex="0" ButtonType="Image">
                                        <NewButton Visible="True" Text="Yeni">
                                            <Image AlternateText="Yeni" Url="~/images/new.gif" />
                                        </NewButton>
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
                                    <dxwgv:GridViewDataColumn Caption="" Width="15px" Visible="true">
                                        <DataItemTemplate>
                                            <img src="../../../images/search_button.jpg" alt="" width="22px" height="18px" />
                                        </DataItemTemplate>
                                        <EditItemTemplate>
                                            <img id="email_ara" src="../../../images/search_button.jpg" alt="" width="22px" height="19px"
                                                onclick="ComboButtonClick('GridSesEmail_EmailId')" style="cursor: pointer" />
                                        </EditItemTemplate>
                                    </dxwgv:GridViewDataColumn>
                                    <dxwgv:GridViewDataColumn FieldName="Email" Caption="E-Posta" Width="400px" ReadOnly="true" />
                                    <dxwgv:GridViewDataColumn FieldName="EmailId" Width="0px" ReadOnly="true" />
                                </Columns>
                            </dxwgv:ASPxGridView>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
        </dxtc:ASPxPageControl>
    </div>
    </form>
</body>
</html>
