<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit.aspx.cs" Inherits="CRM_Genel_BR_edit" %>

<%@ Register Src="~/controls/NotGrid.ascx" TagName="NotGrid" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
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

    <script src="../../crm.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%--            <asp:ScriptManager ID="ScriptManager1" runat="server" />--%>
            <asp:HiddenField ID="HiddenID" runat="server" />
            <model:DataTable ID="DTIssueActivity" runat="server" />
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
                    <dxm:MenuItem Name="NewIssue">
                        <Image Url="~/images/new.gif" />
                        <Template>
                            <table width="150" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                        padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="JavaScript:parent.location.href='./edit.aspx?id=0&NoteOwner=4';">
                                        <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/new.gif" />
                                        Yeni
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
                    <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat">
                        <Image Url="~/images/saveclose.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="delete" Text="Sil">
                        <Image Url="~/images/delete.gif" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" ActiveTabIndex="0" TabSpacing="0px"
                Width="800px">
                <ContentStyle>
                    <Border BorderColor="#4986A2" />
                </ContentStyle>
                <TabPages>
                    <dxtc:TabPage Name="Genel" Text="Genel">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <table style="width: 800px" cellspacing="1" cellpadding="0" border="0">
                                    <tbody>
                                        <tr>
                                            <td align="left" colspan="4" style="width: 100%">
                                                <asp:ValidationSummary ID="VAS1" runat="server" ShowMessageBox="false" ShowSummary="true"
                                                    Font-Size="10px" Font-Names="Arial" Font-Bold="true" Width="214px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="6">
                                                <dxe:ASPxLabel ID="BBrNo" Text="BR No :" runat="server" />
                                                <dxe:ASPxLabel ID="IndexID" runat="server" Font-Bold="true" ForeColor="Red">
                                                </dxe:ASPxLabel>
                                                &nbsp; &nbsp;
                                                <dxe:ASPxLabel ID="BCreatedBy" Text="Oluşturan Kişi :" runat="server" />
                                                <dxe:ASPxLabel ID="IssuedBy" runat="server" Font-Bold="true" ForeColor="#000066">
                                                </dxe:ASPxLabel>
                                                &nbsp; &nbsp;
                                                <dxe:ASPxLabel ID="BIssueDate" Text="Oluşturma Tarihi :" runat="server" />
                                                <dxe:ASPxLabel ID="IssuedDate" runat="server" Font-Bold="true" ForeColor="#000066">
                                                </dxe:ASPxLabel>
                                                <br />
                                                <dxe:ASPxLabel ID="BModifiedBy" Text="Düzenleyen Kişi :" runat="server" />
                                                <dxe:ASPxLabel ID="ModifiedBy" runat="server" Font-Bold="true" ForeColor="#000066">
                                                </dxe:ASPxLabel>
                                                &nbsp; &nbsp;
                                                <dxe:ASPxLabel ID="BModificationDate" Text="Düzenlendiği Tarih :" runat="server" />
                                                <dxe:ASPxLabel ID="ModificationDate" runat="server" Font-Bold="true" ForeColor="#000066">
                                                </dxe:ASPxLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px" valign="top">
                                                <span style="color: #CC0000">Adet</span></td>
                                            <td style="width: 175px">
                                                <dxe:ASPxSpinEdit runat="server" Number="1" Width="175px" ReadOnly="true" ID="Adet"
                                                    MaxLength="5" MinValue="0" AllowMouseWheel="true" NumberType="Integer" />
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                <span style="color: #CC0000">Operasyon Süresi</span>
                                            </td>
                                            <td style="width: 175px" valign="top" align="left">
                                                <dxe:ASPxSpinEdit runat="server" Number="6" Width="175px" ID="OperasyonSuresi" MaxLength="3"
                                                    MinValue="0" AllowMouseWheel="true" NumberType="Integer" />
                                            </td>
                                            <td style="width: 100px" valign="top">
                                            </td>
                                            <td style="width: 175px">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px" valign="top">
                                                Stok Kodu</td>
                                            <td style="width: 175px">
                                                <dxe:ASPxTextBox runat="server" ID="StokKodu" Width="175px" />
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                Renk
                                            </td>
                                            <td style="width: 175px" valign="top" align="left">
                                                <dxe:ASPxTextBox runat="server" ID="Renk" Width="175px" />
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                <span style="color: #CC0000">Tarih</span>
                                            </td>
                                            <td style="width: 175px">
                                                <dxe:ASPxDateEdit ID="Tarih" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" Width="175px">
                                                    <ButtonStyle Cursor="pointer" Width="13px">
                                                    </ButtonStyle>
                                                </dxe:ASPxDateEdit>
                                                <asp:RequiredFieldValidator ID="rqrTarih" runat="server" ControlToValidate="Tarih"
                                                    ErrorMessage="Tarih alanı boş geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px" valign="top">
                                                <span style="color: #CC0000">Size</span>
                                            </td>
                                            <td style="width: 175px">
                                                <dxe:ASPxTextBox runat="server" ID="Size" Width="175px" />
                                                <asp:RequiredFieldValidator ID="rqrSize" runat="server" ControlToValidate="Size"
                                                    ErrorMessage="Size alanı boş geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                <span style="color: #CC0000">Marka</span>
                                            </td>
                                            <td style="width: 175px" valign="top" align="left">
                                                <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                    ID="BrMarkaID" EnableIncrementalFiltering="True">
                                                    <ButtonStyle Width="13px" Cursor="pointer">
                                                    </ButtonStyle>
                                                </dxe:ASPxComboBox>
                                                <asp:RequiredFieldValidator ID="rqrBrMarkaID" runat="server" ControlToValidate="BrMarkaID"
                                                    ErrorMessage="Marka alanı boş geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                <span style="color: #CC0000">Durum</span>
                                            </td>
                                            <td style="width: 175px">
                                                <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                    ID="BrDurumID" EnableIncrementalFiltering="True">
                                                    <ButtonStyle Width="13px" Cursor="pointer">
                                                    </ButtonStyle>
                                                </dxe:ASPxComboBox>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="BrMarkaID"
                                                    ErrorMessage="Marka alanı boş geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px" valign="top">
                                                <span style="color: #CC0000">İsteyen</span></td>
                                            <td style="width: 175px">
                                                <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                    ID="isteyenProjeID" EnableIncrementalFiltering="True">
                                                    <ButtonStyle Width="13px" Cursor="pointer">
                                                    </ButtonStyle>
                                                </dxe:ASPxComboBox>
                                                <asp:RequiredFieldValidator ID="rqristeyenProjeID" runat="server" ControlToValidate="isteyenProjeID"
                                                    ErrorMessage="İsteyen alanı boş geçilemez!" Display="None"></asp:RequiredFieldValidator>
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                İstenilen</td>
                                            <td style="width: 175px" valign="top" align="left">
                                                <dxe:ASPxComboBox runat="server" ValueType="System.Guid" ImageFolder="~/App_Themes/Glass/{0}/"
                                                    CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                                    ID="istenilenProjeID" EnableIncrementalFiltering="True">
                                                    <ButtonStyle Width="13px" Cursor="pointer">
                                                    </ButtonStyle>
                                                </dxe:ASPxComboBox>
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                Müşteri Tel
                                            </td>
                                            <td style="width: 175px">
                                                <dxe:ASPxTextBox runat="server" ID="MusteriTel" Width="175px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px" valign="top">
                                                İrsaliye No</td>
                                            <td style="width: 175px">
                                                <dxe:ASPxTextBox ID="irsaliyeNo" runat="server" Width="175px" />
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                Müşteri Adı</td>
                                            <td style="width: 175px" valign="top" align="left">
                                                <dxe:ASPxTextBox runat="server" ID="MusteriAdi" Width="175px" />
                                            </td>
                                            <td style="width: 100px" valign="top">
                                                Personel Adı</td>
                                            <td style="width: 175px">
                                                <dxe:ASPxTextBox ID="PersonelAdi" runat="server" Width="175px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px" valign="top">
                                                Yorum</td>
                                            <td colspan="2">
                                                <dxe:ASPxMemo ID="Description" runat="server" Width="100%" />
                                            </td>
                                            <td style="width: 100px" valign="top">
                                            </td>
                                            <td valign="top" align="left">
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                    <dxtc:TabPage Name="Not" Text="Not">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <uc1:NotGrid ID="NotGrid1" runat="server" />
                            </dxw:ContentControl>
                        </ContentCollection>
                    </dxtc:TabPage>
                </TabPages>
                <TabStyle HorizontalAlign="Center">
                </TabStyle>
                <Paddings PaddingLeft="0px" />
            </dxtc:ASPxPageControl>
            <dxtc:ASPxPageControl ID="ASPxPageControl2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" ActiveTabIndex="0" TabSpacing="0px"
                Width="800px">
                <ContentStyle>
                    <Border BorderColor="#4986A2" />
                </ContentStyle>
                <TabPages>
                    <dxtc:TabPage Text="BR Tarihçesi" Name="TabIsAkisi">
                        <ContentCollection>
                            <dxw:ContentControl runat="server">
                                <div style="overflow: scroll; width: 750px; height: 200px">
                                    <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" DataSourceID="DTIssueActivity" KeyFieldName="ID" Width="750px"
                                        ClientInstanceName="Grid">
                                        <SettingsText Title="Gündem Tarihçesi" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
                                            ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="#" CustomizationWindowCaption="Kolon Ekle/Çıkart" />
                                        <SettingsEditing Mode="Inline" PopupEditFormWidth="750px" PopupEditFormHorizontalOffset="50"
                                            PopupEditFormVerticalOffset="50" />
                                        <SettingsPager PageSize="15" ShowSeparators="True">
                                        </SettingsPager>
                                        <Images ImageFolder="~/App_Themes/Glass/{0}/">
                                        </Images>
                                        <SettingsCustomizationWindow Enabled="True" />
                                        <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                                            ShowPreview="True" />
                                        <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                                            <AlternatingRow Enabled="True">
                                            </AlternatingRow>
                                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                                            </Header>
                                        </Styles>
                                        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                                        <Columns>
                                            <dxwgv:GridViewDataColumn FieldName="ID" Visible="false" />
                                            <dxwgv:GridViewDataColumn FieldName="Comment" Caption="Değişiklik" Width="200px">
                                                <DataItemTemplate>
                                                    <asp:Literal ID="Process" runat="server" Text='<%# Eval("Comment") %>'></asp:Literal>
                                                </DataItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn FieldName="CreatedBy" Width="50px" Caption="Oluşturan"
                                                EditFormSettings-Visible="False">
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataColumn FieldName="DurumName" Width="50px" Caption="BR Durumu"
                                                EditFormSettings-Visible="False">
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                            </dxwgv:GridViewDataColumn>
                                            <dxwgv:GridViewDataDateColumn PropertiesDateEdit-DisplayFormatString="dd.MM.yyyy"
                                                FieldName="CommentDate" Width="50px" Caption="İşlem Tarihi">
                                                <PropertiesDateEdit DisplayFormatString="dd.MM.yyyy">
                                                </PropertiesDateEdit>
                                                <EditFormSettings Visible="False" />
                                                <EditItemTemplate>
                                                </EditItemTemplate>
                                            </dxwgv:GridViewDataDateColumn>
                                        </Columns>
                                    </dxwgv:ASPxGridView>
                                </div>
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
