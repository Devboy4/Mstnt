<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="CRM_Genel_Proje_edit" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="System.Web, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="System.Web.UI" TagPrefix="cc1" %>


<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<%@ Register Src="~/controls/NotGrid.ascx" TagName="NotGrid" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title>CRM</title>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />

    <script src="../../../PreLoad.js" type="text/javascript"></script>

    <script src="../../../utils.js" type="text/javascript"></script>

    <script src="../../crm.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="HiddenID" Visible="false" />
        <model:DataTable ID="DataTableAllowedProjeList" runat="server" />
        <model:DataTable ID="DTProjeUserGrupList" runat="server" />
        <model:DataTable ID="DTUsers" runat="server" />
        <model:DataTable ID="DTUserGrup" runat="server" />
        <asp:SqlDataSource ID="DSUser" runat="server" SelectCommand="SELECT UserID, UserName FROM SecurityUsers Where Active=1 ORDER BY UserName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSUserGrup" runat="server" SelectCommand="SELECT UserGrupID, Adi As UserGrupName FROM UserGrup ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <div>
            <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                ShowSubMenuShadow="False" Width="300px">
                <SubMenuStyle GutterWidth="0px" />
                <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
                <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
                </SubMenuItemStyle>
                <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                    X="2" Y="-12" />
                <Items>
                    <dxm:MenuItem Name="new" Text="Yeni">
                        <Image Url="~/images/new.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <Image Url="~/images/save.gif"></Image>
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat">
                        <Image Url="~/images/saveclose.gif"></Image>
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="delete" Text="Sil">
                        <Image Url="~/images/delete.gif"></Image>
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" TabSpacing="0px" ActiveTabIndex="0"
                EnableCallBacks="True">
                <TabStyle HorizontalAlign="Center">
                </TabStyle>
                <ContentStyle>
                    <Border BorderColor="#4986A2" />
                </ContentStyle>
                <Paddings PaddingLeft="0px" />
                <TabPages>
                    <dxtc:TabPage Name="Genel" Text="Genel">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="800px">
                                    <PanelCollection>
                                        <dxp:PanelContent runat="server">
                                            <table style="width: 100%" cellspacing="2" cellpadding="0" border="0">
                                                <tr>
                                                    <td valign="top" style="width: 110px">
                                                        <span style="color: #C00000">Adý</span></td>
                                                    <td valign="top">
                                                        <dxe:ASPxTextBox runat="server" ID="ProjeName" Width="175px" />
                                                    </td>
                                                    <td valign="top" style="width: 122px">
                                                        <span style="color: #C00000">Ýlgili Birim</span>
                                                    </td>
                                                    <td valign="top">
                                                        <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                            CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                            ID="FirmaID" ClientInstanceName="cmbFirmaID" EnableIncrementalFiltering="True">
                                                            <ButtonStyle Width="13px" Cursor="pointer">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" style="width: 110px">
                                                        <span style="color: #C00000">Departman Sorumlusu</span></td>
                                                    <td valign="top">
                                                        <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                            CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                            ID="ProjeAmiriID" EnableIncrementalFiltering="True">
                                                            <ButtonStyle Width="13px" Cursor="pointer">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td valign="top" style="width: 122px">
                                                        Departman E-Mail Adresi</td>
                                                    <td valign="top">
                                                        <dxe:ASPxTextBox ID="ProjeEmailAdresi" runat="server" Width="175px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" style="width: 110px">
                                                        Departman Grubu</td>
                                                    <td valign="top">
                                                        <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                                            CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                            ID="ProjeSinifID" EnableIncrementalFiltering="True">
                                                            <ButtonStyle Width="13px" Cursor="pointer">
                                                            </ButtonStyle>
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td valign="top" style="width: 122px">
                                                    </td>
                                                    <td valign="top">
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" style="width: 110px">
                                                        Açýklama</td>
                                                    <td colspan="3" valign="top">
                                                        <dxe:ASPxMemo runat="server" ID="Description" Width="569px" Height="74px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </dxp:PanelContent>
                                    </PanelCollection>
                                </dxrp:ASPxRoundPanel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Name="ProjeSettings" Text="Departman Dahilindeki Kiþiler">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxrp:ASPxRoundPanel ID="ASPxRoundPanel3" runat="server" HeaderText="" Width="800px">
                                    <PanelCollection>
                                        <dxp:PanelContent runat="server">
                                            <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DataTableAllowedProjeList"
                                                Width="100%" ID="GRD_AlowedProjeList" OnRowInserting="GRD_AlowedProjeList_RowInserting"
                                                OnRowUpdating="GRD_AlowedProjeList_RowUpdating" OnCellEditorInitialize="GRD_AlowedProjeList_CellEditorInitialize"
                                                OnRowValidating="GRD_AlowedProjeList_RowValidating">
                                                <SettingsText Title="Departman / Kiþi Baðlantýsý" ConfirmDelete="Kayýt silinsin mi?"
                                                    EmptyDataRow="Yeni satýr ekle" />
                                                <SettingsPager PageSize="15" ShowSeparators="True">
                                                </SettingsPager>
                                                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                                </Images>
                                                <SettingsCustomizationWindow Enabled="True" />
                                                <Settings ShowPreview="True" ShowTitlePanel="True" />
                                                <SettingsLoadingPanel Text="Yükleniyor..." />
                                                <SettingsEditing Mode="Inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
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
                                                        <EditButton Visible="True" Text="Deðiþtir">
                                                            <Image AlternateText="Deðiþtir" Url="~/images/edit.gif"></Image>
                                                        </EditButton>
                                                        <NewButton Visible="True" Text="Yeni">
                                                            <Image AlternateText="Yeni" Url="~/images/new.gif"></Image>
                                                        </NewButton>
                                                        <DeleteButton Visible="True" Text="Sil">
                                                            <Image AlternateText="Sil" Url="~/images/delete.gif"></Image>
                                                        </DeleteButton>
                                                        <CancelButton Visible="True" Text="Ýptal">
                                                            <Image AlternateText="Ýptal" Url="~/images/delete.gif"></Image>
                                                        </CancelButton>
                                                        <UpdateButton Visible="True" Text="G&#252;ncelle">
                                                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif"></Image>
                                                        </UpdateButton>
                                                    </dxwgv:GridViewCommandColumn>
                                                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataColumn FieldName="UserAllowedProjectID" Visible="False">
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataComboBoxColumn FieldName="UserID" Caption="Kiþi" Width="175px">
                                                        <PropertiesComboBox ValueField="UserID" TextField="UserName" DataSourceID="DSUser"
                                                            ValueType="System.Guid" EnableIncrementalFiltering="True">
                                                        </PropertiesComboBox>
                                                        <HeaderStyle ForeColor="#C00000"></HeaderStyle>
                                                    </dxwgv:GridViewDataComboBoxColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Adý" FieldName="FirstName">
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                        <EditFormSettings Visible="False" />
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Soyadý" FieldName="LastName">
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                        <EditFormSettings Visible="False" />
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
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
                                        </dxp:PanelContent>
                                    </PanelCollection>
                                </dxrp:ASPxRoundPanel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <%--<dxtc:TabPage Name="ProjeSettings" Text="Kullanýcý Gruplarý Listesi">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="" Width="800px">
                                    <PanelCollection>
                                        <dxp:PanelContent runat="server">
                                            <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DTProjeUserGrupList"
                                                Width="100%" ID="gridProjeUserGrupList" OnRowInserting="gridProjeUserGrupList_RowInserting"
                                                OnCellEditorInitialize="gridProjeUserGrupList_CellEditorInitialize" OnRowValidating="gridProjeUserGrupList_RowValidating">
                                                <SettingsText Title="Kullanýcý Gruplarý" ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
                                                <SettingsPager PageSize="15" ShowSeparators="True">
                                                </SettingsPager>
                                                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                                </Images>
                                                <SettingsCustomizationWindow Enabled="True" />
                                                <Settings ShowPreview="True" ShowTitlePanel="True" />
                                                <SettingsLoadingPanel Text="Yükleniyor..." />
                                                <SettingsEditing Mode="Inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
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
                                                        <EditButton Visible="True" Text="Deðiþtir">
                                                            <Image AlternateText="Deðiþtir" Url="~/images/edit.gif"></Image>
                                                        </EditButton>
                                                        <NewButton Visible="True" Text="Yeni">
                                                            <Image AlternateText="Yeni" Url="~/images/new.gif"></Image>
                                                        </NewButton>
                                                        <DeleteButton Visible="True" Text="Sil">
                                                            <Image AlternateText="Sil" Url="~/images/delete.gif"></Image>
                                                        </DeleteButton>
                                                        <CancelButton Visible="True" Text="Ýptal">
                                                            <Image AlternateText="Ýptal" Url="~/images/delete.gif"></Image>
                                                        </CancelButton>
                                                        <UpdateButton Visible="True" Text="G&#252;ncelle">
                                                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif"></Image>
                                                        </UpdateButton>
                                                    </dxwgv:GridViewCommandColumn>
                                                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataColumn FieldName="ProjeUserGrupListID" Visible="False">
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataComboBoxColumn FieldName="UserGrupID" Caption="Kullanýcý Grubu">
                                                        <PropertiesComboBox ValueField="UserGrupID" TextField="UserGrupName" DataSourceID="DSUserGrup"
                                                            ValueType="System.Guid" EnableIncrementalFiltering="True">
                                                        </PropertiesComboBox>
                                                        <HeaderStyle ForeColor="#C00000"></HeaderStyle>
                                                    </dxwgv:GridViewDataComboBoxColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
                                                    </dxwgv:GridViewDataColumn>
                                                    <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
                                                        <EditItemTemplate>
                                                        </EditItemTemplate>
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
                                        </dxp:PanelContent>
                                    </PanelCollection>
                                </dxrp:ASPxRoundPanel>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>--%>
                </TabPages>
            </dxtc:ASPxPageControl>
        </div>
    </form>
</body>
</html>
