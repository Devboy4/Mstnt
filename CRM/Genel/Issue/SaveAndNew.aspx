<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaveAndNew.aspx.cs" Inherits="CRM_Genel_Issue_SaveAndNew" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />

    <script src="../../../PreLoad.js" type="text/javascript"></script>

    <script src="../../../utils.js" type="text/javascript"></script>

    <script src="./crm_20141215.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <model:DataTable ID="DataTableNoteDosya" runat="server" />
            <asp:HiddenField ID="HiddenID" runat="server" />
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
                    <dxm:MenuItem Name="new">
                        <Image Url="~/images/new.gif" />
                        <Template>
                            <table width="50" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                        padding-right: 4px; border-right-width: 0px; width: 70px;" onclick="JavaScript:location.href='./AddIssue.aspx';">
                                        <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/new.gif" />Yeni
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <Image Url="~/images/save.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni">
                        <Image Url="~/images/savenew.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="saveopen" Text="Kaydet ve Aç">
                        <Image Url="~/images/saveopen.png" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <model:DataTable ID="DTRelatedUsers" runat="server" />
            <hr />
            <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                <tr>
                    <td style="width: 50px" valign="top" align="center">
                        <img src="../../../images/info_16.gif" /></td>
                    <td style="width: 450px" valign="top">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 450px">
                            <tr>
                                <td style="width: 100%" align="left" colspan="2">
                                    <span style="font-weight: bold; color: lime">Bildirim girme iþlemi baþarýlý...</span>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px" align="left">
                                    Bildirim Baþlýk</td>
                                <td style="width: 350px" align="left">
                                    <dxe:ASPxLabel ID="SaveText" runat="server" Font-Bold="True" Font-Names="arial" Font-Size="8pt">
                                    </dxe:ASPxLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px" align="left">
                                    Bildirim Link</td>
                                <td style="width: 350px" align="left">
                                    <asp:Literal ID="SonBildirim" runat="server"></asp:Literal></td>
                            </tr>
                            <tr>
                                <td style="width: 100px" align="left">
                                    Dosya Link</td>
                                <td style="width: 350px" align="left" valign="top">
                                    <asp:Literal ID="DosyaLink" runat="server"></asp:Literal></td>
                            </tr>
                        </table>
                </tr>
            </table>
            <hr />
            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="" Width="100%"
                Height="100%">
                <PanelCollection>
                    <dxp:PanelContent runat="server">
                        <table cellspacing="1" cellpadding="0" border="0" style="width: 700px">
                            <tr>
                                <td valign="top" style="width: 100px">
                                    <span style="color: #CC0000">Gündem Tanýsý</span></td>
                                <td colspan="3">
                                    <dxe:ASPxMemo ID="Description" runat="server" Height="71px" Width="500px">
                                    </dxe:ASPxMemo>
                                </td>
                                <td colspan="1" valign="top">
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Description"
                                        ErrorMessage="Boþ Ge&#231;ilemez" Font-Bold="False" Font-Size="10px"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 100px">
                                    <span style="color: #CC0000">Ýlgili Birim</span></td>
                                <td style="width: 150px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" ID="FirmaID"
                                        ClientInstanceName="cmbFirmaID" EnableCallbackMode="true" CallbackPageSize="15"
                                        EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                        <ClientSideEvents SelectedIndexChanged="function(s,e) {OnFirmaIDChanged(s);}" />
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="FirmaID"
                                        EnableClientScript="False" ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 100px" valign="top">
                                    <span style="color: #CC0000">Departman</span></td>
                                <td style="width: 150px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        ID="ProjeID" ClientInstanceName="cmbProjeID" EnableCallbackMode="true" OnCallback="ProjeID_Callback">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                        <ClientSideEvents SelectedIndexChanged="function(s, e) {
	                                    cmbUserID.PerformCallback(s.GetValue());grid1.PerformCallback('gridbind');}" EndCallback="function(s, e) {
	                                    cmbUserID.PerformCallback(s.GetValue());grid1.PerformCallback('gridbind');}" />
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ProjeID"
                                        ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 150px">
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 100px">
                                    <span style="color: #CC0000">Gündem Durumu</span></td>
                                <td style="width: 150px;">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        ID="DurumID" EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="DurumID"
                                        ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 100px;" valign="top">
                                    <span style="color: #CC0000">Önem Derecesi</span></td>
                                <td style="width: 150px;" valign="top">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        ID="OnemDereceID" EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="OnemDereceID"
                                        ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 150px; color: #000000;">
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 100px">
                                    <span style="color: #CC0000">Gündem Ýlgilisi</span></td>
                                <td style="width: 150px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" 
                                        ID="UserID" EnableIncrementalFiltering="True" EnableCallbackMode="true" ClientInstanceName="cmbUserID"
                                        OnCallback="UserID_Callback1">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                        <ClientSideEvents EndCallback="function(s, e) {	s.SetSelectedIndex(1);}" />
                                    </dxe:ASPxComboBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="UserID"
                                        ErrorMessage="Boþ olamaz!"></asp:RequiredFieldValidator>
                                </td>
                                <td style="width: 100px" valign="top">
                                </td>
                                <td colspan="2" valign="top">
                                </td>
                                
                            </tr>
                            <tr>
                                <td valign="top" style="width: 100px">
                                    <span style="color: #CC0000">Operasyon Süresi (Gün)</span></td>
                                <td style="width: 150px" valign="top">
                                    <dxe:ASPxSpinEdit Width="75px" runat="server" ID="OperasyonSuresi" ClientInstanceName="SpinOp"
                                        MaxLength="4" MaxValue="9999" MinValue="0" Number="0" NumberType="Integer">
                                        <ClientSideEvents ButtonClick="function(s, e){SetSonGecerlilikValue(s.GetNumber());}"
                                            LostFocus="function(s, e){SetSonGecerlilikValue(s.GetNumber());}" />
                                    </dxe:ASPxSpinEdit>
                                </td>
                                <td style="width: 100px" valign="top">
                                    <span style="color: #CC0000">Planlanan Operasyon Tarihi</span>
                                </td>
                                <td style="width: 150px" colspan="2" valign="top">
                                    <dxe:ASPxDateEdit ID="TeslimTarihi" ClientInstanceName="d2" runat="server"
                                        CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                        <ButtonStyle Cursor="pointer" Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                           </tr>
                            <tr>
                                <td valign="top" style="width:100px">
                                    Diðer Gündem Ýlgilileri</td>
                                <td colspan="5">
                                    <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DTRelatedUsers"
                                        Width="100%" ID="grid1" OnCustomCallback="grid1_CustomCallback" ClientInstanceName="grid1">
                                        <SettingsText Title="Ayný anda atamak istediðiniz diðer ilgililer..." ConfirmDelete="Kayýt silinsin mi?"
                                            EmptyDataRow="#" />
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
                                            <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="40px"
                                                ButtonType="Image">
                                                <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                                                    <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                                                </ClearFilterButton>
                                                <HeaderTemplate>
                                                    <input id="Button1" type="button" onclick="grid1.PerformCallback(true);" value="+"
                                                        title="Tümünü Seç" />
                                                    <input id="Button2" type="button" onclick="grid1.PerformCallback(false);" value="-"
                                                        title="Tümünü Seçme" />
                                                </HeaderTemplate>
                                            </dxwgv:GridViewCommandColumn>
                                            <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn FieldName="UserID" Visible="False">
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn Caption="Ýlgili Adý" FieldName="Adi">
                                            </dxwgv:GridViewDataColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                </td>
                            </tr>
                        </table>
                    </dxp:PanelContent>
                </PanelCollection>
                <HeaderTemplate>
                </HeaderTemplate>
            </dxrp:ASPxRoundPanel>
        </div>
    </form>
</body>
</html>
