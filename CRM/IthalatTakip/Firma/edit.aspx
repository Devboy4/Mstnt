<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="CRM_IthalatTakip_Tanim_Firma_edit" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />

    <script type="text/javascript" src="../../../PreLoad.js"></script>

    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />

    <script src="../../../utils.js" type="text/javascript"></script>

    <script src="./edit.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField runat="server" ID="HiddenId" Visible="false" />
        <model:DataTable ID="DTMarka" runat="server" />
        <model:DataTable ID="DTUrun" runat="server" />
        <%--        <dxcb:ASPxCallback ID="CallbackGenel" runat="server" ClientInstanceName="CallbackGenel"
            OnCallback="CallbackGenel_Callback">
            <ClientSideEvents CallbackComplete="function(s, e) {CallbackGenelComplete(e.result);}" />
        </dxcb:ASPxCallback>--%>
        <dxcb:ASPxCallback ID="CallbackSearchBrowser" runat="server" ClientInstanceName="CallbackSearchBrowser"
            OnCallback="CallbackSearchBrowser_Callback">
            <ClientSideEvents CallbackComplete="function(s, e) { SearchBrowserCallbackComplete(e); }"
                EndCallback="function(s, e) { SearchBrowserEndCallback(); }" />
        </dxcb:ASPxCallback>
        <div>
            <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                ShowSubMenuShadow="False" AutoPostBack="True" Width="300px">
                
                <SubMenuStyle GutterWidth="0px" />
                <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
                <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
                </SubMenuItemStyle>
                <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                    X="2" Y="-12" />
                <Items>
                    <dxm:MenuItem Name="new" Text="Yeni">
                        <TextTemplate>
                            Yeni</TextTemplate>
                        <Image Url="~/images/new.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <TextTemplate>
                            Kaydet</TextTemplate>
                        <Image Url="~/images/save.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni">
                        <TextTemplate>
                            Kaydet ve Yeni</TextTemplate>
                        <Image Url="~/images/savenew.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat">
                        <TextTemplate>
                            Kaydet ve Kapat</TextTemplate>
                        <Image Url="~/images/saveclose.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="delete" Text="Sil">
                        <TextTemplate>
                            Sil</TextTemplate>
                        <Image Url="~/images/delete.gif" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <hr />
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="SingleParagraph"
                HeaderText="UYARI : " ShowMessageBox="false" ShowSummary="true" Font-Size="10px"
                Font-Bold="true" />
            <table border="0" cellpadding="1" cellspacing="1" width="100%" style="border-style: outset;">
                <tr style="background-color: #ccccff">
                    <td colspan="7">
                        <dxe:ASPxLabel ID="lblFirma" runat="server" Text="FÝRMA" />
                    </td>
                </tr>
                <tr>
                    <td colspan="1">
                        <dxe:ASPxLabel ID="lblAdi" runat="server" Text="Adý" />
                    </td>
                    <td colspan="5">
                        <dxe:ASPxTextBox ID="Adi" runat="server" Width="99%" ClientInstanceName="Adi" />
                    </td>
                    <td colspan="1">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="width: 100px">
                        <dxe:ASPxLabel ID="lblSevkSekliId" runat="server" Text="Sevk Þekli" />
                    </td>
                    <td colspan="1" style="width: 150px">
                        <dxe:ASPxComboBox ID="SevkSekliId" runat="server" Width="99%" ValueType="System.Guid"
                            EnableIncrementalFiltering="false" ClientInstanceName="SevkSekliId" CallbackPageSize="15"
                            EnableCallbackMode="true" OnCallback="SevkSekliId_Callback">
                            <ClientSideEvents SelectedIndexChanged="function(s, e) {SevkSekliIdChanged(s); }"
                                ButtonClick="function(s, e) { ComboButtonClick('SevkSekliId'); }" EndCallback="function(s, e) { ComboEndCallback('SevkSekliId'); }" />
                            <DropDownButton Visible="false">
                            </DropDownButton>
                            <Buttons>
                                <dxe:EditButton Text="...">
                                </dxe:EditButton>
                            </Buttons>
                        </dxe:ASPxComboBox>
                    </td>
                    <td colspan="1" style="width: 100px">
                        <dxe:ASPxLabel ID="lblOdemeSekliId" runat="server" Text="Ödeme Þekli" />
                    </td>
                    <td colspan="1" style="width: 150px">
                        <dxe:ASPxComboBox ID="OdemeSekliId" runat="server" Width="99%" ValueType="System.Guid"
                            EnableIncrementalFiltering="false" ClientInstanceName="OdemeSekliId" CallbackPageSize="15"
                            EnableCallbackMode="true" OnCallback="OdemeSekliId_Callback">
                            <ClientSideEvents SelectedIndexChanged="function(s, e) {OdemeSekliIdChanged(s); }"
                                ButtonClick="function(s, e) { ComboButtonClick('OdemeSekliId'); }" EndCallback="function(s, e) { ComboEndCallback('OdemeSekliId'); }" />
                            <DropDownButton Visible="false">
                            </DropDownButton>
                            <Buttons>
                                <dxe:EditButton Text="...">
                                </dxe:EditButton>
                            </Buttons>
                        </dxe:ASPxComboBox>
                    </td>
                    <td colspan="1" style="width: 100px">
                        <dxe:ASPxLabel ID="lblIskonto" runat="server" Text="Ýskonto" />
                    </td>
                    <td colspan="1" style="width: 150px">
                        <dxe:ASPxSpinEdit runat="server" ID="Iskonto" MaxValue="100" MinValue="0" NumberType="Float"
                            Number="0" DecimalPlaces="2" Increment="10" Width="70px" />
                    </td>
                    <td colspan="1">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="1">
                        <dxe:ASPxLabel ID="lblAciklama" runat="server" Text="Açýklama" />
                    </td>
                    <td colspan="5">
                        <dxe:ASPxMemo ID="Aciklama" runat="server" Width="99%" ClientInstanceName="Aciklama"
                            Rows="2" />
                    </td>
                    <td colspan="1">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <asp:RequiredFieldValidator ID="rfvAdi" runat="server" ControlToValidate="Adi" ErrorMessage="Adý alaný boþ olamaz!"
                            Display="None" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        &nbsp;
                    </td>
                </tr>
            </table>
            <dxtc:ASPxPageControl ID="PageAlt" runat="server" ActiveTabIndex="1" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" TabSpacing="0px"
                TabIndex="0">
                <TabPages>
                    <dxtc:TabPage Text="MARKALAR" Name="TabMarka">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <div style="overflow: scroll; width: 750px; height: 230px">
                                    <dxwgv:ASPxGridView ID="GridMarka" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" DataSourceID="DTMarka" KeyFieldName="ID" SettingsEditing-Mode="Inline"
                                        OnRowInserting="GridMarka_RowInserting" OnRowUpdating="GridMarka_RowUpdating"
                                        OnRowValidating="GridMarka_RowValidating" ClientInstanceName="GridMarka" OnCustomJSProperties="Grid_CustomJSProperties"
                                        OnCellEditorInitialize="GridMarka_CellEditorInitialize">
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
                                                    <Image AlternateText="Güncelle" Url="~/images/update.gif" />
                                                </UpdateButton>
                                                <DeleteButton Visible="True" Text="Sil">
                                                    <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                                </DeleteButton>
                                                <CancelButton Visible="True" Text="Ýptal">
                                                    <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                                                </CancelButton>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                                            <dxwgv:GridViewDataColumn FieldName="FirmaId" Visible="False" />
                                            <dxwgv:GridViewDataColumn FieldName="Marka" Caption="Marka" />
                                        </Columns>
                                        <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                        </Images>
                                        <Settings GridLines="Both" ShowColumnHeaders="true" ShowFilterRow="false" ShowFilterRowMenu="false"
                                            ShowFooter="false" ShowGroupButtons="false" ShowGroupedColumns="false" ShowGroupFooter="Hidden"
                                            ShowGroupPanel="false" ShowHeaderFilterButton="false" ShowPreview="true" ShowStatusBar="Hidden"
                                            ShowTitlePanel="false" UseFixedTableLayout="false" ShowVerticalScrollBar="false"
                                            VerticalScrollableHeight="250" />
                                        <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px" />
                                        </Styles>
                                        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                        <SettingsPager Mode="ShowAllRecords" />
                                        <SettingsText Title="MARKALAR" EmptyDataRow="Yeni Satýr" />
                                    </dxwgv:ASPxGridView>
                                </div>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Text="ÜRÜNLER" Name="TabUrun">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <div style="overflow: scroll; width: 750px; height: 230px">
                                    <dxwgv:ASPxGridView ID="GridUrun" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" DataSourceID="DTUrun" KeyFieldName="ID" SettingsEditing-Mode="Inline"
                                        OnRowInserting="GridUrun_RowInserting" OnRowUpdating="GridUrun_RowUpdating" OnRowValidating="GridUrun_RowValidating"
                                        ClientInstanceName="GridUrun" OnCustomJSProperties="Grid_CustomJSProperties"
                                        OnCellEditorInitialize="GridUrun_CellEditorInitialize">
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
                                                    <Image AlternateText="Güncelle" Url="~/images/update.gif" />
                                                </UpdateButton>
                                                <DeleteButton Visible="True" Text="Sil">
                                                    <Image AlternateText="Sil" Url="~/images/delete.gif" />
                                                </DeleteButton>
                                                <CancelButton Visible="True" Text="Ýptal">
                                                    <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                                                </CancelButton>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                                            <dxwgv:GridViewDataColumn FieldName="FirmaId" Visible="False" />
                                            <dxwgv:GridViewDataComboBoxColumn FieldName="MarkaId" Caption="Marka">
                                                <PropertiesComboBox TextField="Marka" ValueField="Id" DataSourceID="DTMarka" ValueType="System.Guid"
                                                    EnableIncrementalFiltering="true" CallbackPageSize="25" EnableCallbackMode="true">
                                                </PropertiesComboBox>
                                            </dxwgv:GridViewDataComboBoxColumn>
                                            <dxwgv:GridViewDataColumn FieldName="Urun" Caption="Ürün" />
                                        </Columns>
                                        <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                        </Images>
                                        <Settings GridLines="Both" ShowColumnHeaders="true" ShowFilterRow="false" ShowFilterRowMenu="false"
                                            ShowFooter="false" ShowGroupButtons="false" ShowGroupedColumns="false" ShowGroupFooter="Hidden"
                                            ShowGroupPanel="false" ShowHeaderFilterButton="false" ShowPreview="true" ShowStatusBar="Hidden"
                                            ShowTitlePanel="false" UseFixedTableLayout="false" ShowVerticalScrollBar="false"
                                            VerticalScrollableHeight="250" />
                                        <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                            <Header ImageSpacing="5px" SortingImageSpacing="5px" />
                                        </Styles>
                                        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                        <SettingsPager Mode="ShowAllRecords" />
                                        <SettingsText Title="ÜRÜNLER" EmptyDataRow="Yeni Satýr" />
                                    </dxwgv:ASPxGridView>
                                </div>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                </TabPages>
            </dxtc:ASPxPageControl>
        </div>
    </form>
</body>
</html>
