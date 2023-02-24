<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="CRM_Genel_Tanimli_edit" %>



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
    <title>Bildirim Takibi - İş Planı</title>

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
            <asp:HiddenField ID="HiddenIndexId" runat="server" />
            <dxm:ASPxMenu ID="ASPxMenu1" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" OnItemClick="ASPxMenu1_ItemClick" SeparatorHeight="100%" SeparatorWidth="2px"
                ShowPopOutImages="True" ShowSubMenuShadow="False" Width="50px">
                <SubMenuStyle GutterWidth="0px" />
                <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
                <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                    X="2" Y="-12" />
                <Items>
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <Image Url="~/images/save.gif" />
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
                            <dxw:ContentControl runat="server">
                                <table border="0" cellpadding="0" style="width: 700px" cellspacing="1">
                                    <tr>
                                        <td colspan="6">
                                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="List"
                                                HeaderText="UYARI : " ShowMessageBox="false" ShowSummary="true" Font-Size="10px"
                                                Font-Bold="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <span style="color: #CC0000;">Gündem Tanısı</span>
                                        </td>
                                        <td colspan="5">
                                            <dxe:ASPxMemo ID="Baslik" runat="server" Width="100%" Height="74px" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Baslik"
                                                ErrorMessage="Gündem Tanısı alanı boş geçilemez!" Font-Bold="True" Display="None"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">Açıklama
                                        </td>
                                        <td colspan="6">
                                            <dxe:ASPxMemo ID="Description" Width="100%" Height="74px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <span style="color: #CC0000;">İlgili Birim</span>
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                ID="FirmaId" ClientInstanceName="cmbFirmaId" EnableCallbackMode="true" CallbackPageSize="15"
                                                EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                </ButtonStyle>
                                                <ClientSideEvents SelectedIndexChanged="function(s,e) {cmbProjeId.PerformCallback(s.GetValue());}" />
                                            </dxe:ASPxComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="FirmaId"
                                                ErrorMessage="İlgili birim alanı boş geçilemez!" Font-Bold="True" Display="None"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 100px">
                                            <span style="color: #CC0000;">Departman</span>
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                ID="ProjeId" ClientInstanceName="cmbProjeId" OnCallback="ProjeId_Callback" EnableCallbackMode="true"
                                                CallbackPageSize="15" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                </ButtonStyle>
                                            </dxe:ASPxComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="ProjeId"
                                                ErrorMessage="Departman alanı boş geçilemez!" Font-Bold="True" Display="None"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                        </td>
                                        <td colspan="2">
                                            <dxe:ASPxCheckBox ID="Active" Text="Aktif / Pasif" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <span style="color: #CC0000;">Gündem Sınıf</span>
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxComboBox runat="server" ValueType="System.Int32" ImageFolder="~/App_Themes/Glass/{0}/"
                                                CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                ID="VirusSinifId" ClientInstanceName="cmbVirussinifId" EnableCallbackMode="true"
                                                CallbackPageSize="15" EnableIncrementalFiltering="True">
                                                <ButtonStyle Width="13px" Cursor="pointer">
                                                </ButtonStyle>
                                                <ClientSideEvents SelectedIndexChanged="function(s,e){ grid.PerformCallback(s.GetValue());  }" />
                                            </dxe:ASPxComboBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="VirusSinifId"
                                                ErrorMessage="Gündem Sınıf alanı boş geçilemez!" Font-Bold="True" Display="None"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 100px">
                                            <span style="color: #CC0000;">Zaman Aralığı (Gün)</span>
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxSpinEdit runat="server" ID="Step" MaxLength="4" MaxValue="10000" NumberType="Integer"
                                                MinValue="1">
                                            </dxe:ASPxSpinEdit>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="Step"
                                                ErrorMessage="Zaman Aralığı alanı boş geçilemez!" Font-Bold="True" Display="None"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 100px">
                                            <span style="color: #CC0000;">Saat</span>
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxSpinEdit runat="server" ID="Saat" MaxLength="2" MaxValue="23" NumberType="Integer"
                                                MinValue="0">
                                            </dxe:ASPxSpinEdit>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="Saat"
                                                ErrorMessage="Saat alanı boş geçilemez!" Font-Bold="True" Display="None" Font-Size="10px"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <span style="color: #CC0000;">Başlangıç Tarihi</span>
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxDateEdit ID="BaslangicTarihi" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                                <CalendarProperties TodayButtonText="Bugün" ClearButtonText="Temizle" FastNavProperties-CancelButtonText="İptal"
                                                    FastNavProperties-OkButtonText="Seç">
                                                    <FooterStyle Spacing="4px"></FooterStyle>
                                                    <HeaderStyle Spacing="1px"></HeaderStyle>
                                                </CalendarProperties>
                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="BaslangicTarihi"
                                                ErrorMessage="Başlangıç Tarihi alanı boş geçilemez!" Font-Bold="True" Display="None"
                                                Font-Size="10px"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 100px">Son İşlem Tarihi
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxDateEdit ID="SonIslemTarihi" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                                <CalendarProperties TodayButtonText="Bugün" ClearButtonText="Temizle" FastNavProperties-CancelButtonText="İptal"
                                                    FastNavProperties-OkButtonText="Seç">
                                                    <FooterStyle Spacing="4px"></FooterStyle>
                                                    <HeaderStyle Spacing="1px"></HeaderStyle>
                                                </CalendarProperties>
                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                        <td style="width: 100px">Sonraki İşlem Tarihi
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxDateEdit ID="SonrakiIslemTarihi" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                                <CalendarProperties TodayButtonText="Bugün" ClearButtonText="Temizle" FastNavProperties-CancelButtonText="İptal"
                                                    FastNavProperties-OkButtonText="Seç">
                                                    <FooterStyle Spacing="4px"></FooterStyle>
                                                    <HeaderStyle Spacing="1px"></HeaderStyle>
                                                </CalendarProperties>
                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxDateEdit>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 100px">
                                            <span style="color: #CC0000;">Operasyon Süresi</span>
                                        </td>
                                        <td style="width: 175px">
                                            <dxe:ASPxSpinEdit runat="server" ID="OperationTime" MaxLength="2" MaxValue="99" NumberType="Integer"
                                                MinValue="0">
                                            </dxe:ASPxSpinEdit>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="OperationTime"
                                                ErrorMessage="Operasyon Süresi alanı boş geçilemez!" Font-Bold="True" Display="None" Font-Size="10px"></asp:RequiredFieldValidator>
                                        </td>
                                        <td style="width: 100px"></td>
                                        <td style="width: 175px"></td>
                                        <td style="width: 100px"></td>
                                        <td style="width: 175px"></td>
                                    </tr>
                                </table>
                                <hr />
                                <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" DataSourceID="DataTableList" OnRowValidating="grid_RowValidating"
                                    KeyFieldName="ID" Width="700px" ClientInstanceName="grid" OnCustomCallback="grid_CustomCallback">
                                    <SettingsText EmptyDataRow="#" Title="Liste" />
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
                                        <dxwgv:GridViewCommandColumn VisibleIndex="0" Width="60px"
                                            ButtonType="Image">
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
