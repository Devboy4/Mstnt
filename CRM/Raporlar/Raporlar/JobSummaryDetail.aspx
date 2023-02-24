<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="JobSummaryDetail.aspx.cs" Inherits="CRM_Raporlar_JobSummaryDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dxpc" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="HiddenID" runat="server" />
    <asp:SqlDataSource ID="DSUser" runat="server" SelectCommand="SELECT IndexId UserId,(ISNULL(UserName,'')+' ['+ISNULL(FirstName,'')+' '+ISNULL(LastName,'')+']') AS UserName, UserName AS UsString FROM SecurityUsers Where Active=1 ORDER BY UserName"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
    <div>
        <model:DataTable ID="DataTableList" runat="server" />
        <dxrp:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" HeaderText="" Width="100%"
            Height="100%">
            <PanelCollection>
                <dxp:PanelContent ID="PanelContent1" runat="server">
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
                            <dxm:MenuItem Name="AddAttachment">
                                <Template>
                                    <table width="50" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer; padding-right: 4px; border-right-width: 0px; width: 150px;"
                                                onclick="Grid.PerformCallback('x');">
                                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/List.gif" />Yenile
                                            </td>
                                        </tr>
                                    </table>
                                </Template>
                            </dxm:MenuItem>
                            <dxm:MenuItem Name="AddExtrachtColumns">
                                <Image Url="~/images/new.gif" />
                                <Template>
                                    <table width="150" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer; padding-right: 4px; border-right-width: 0px; width: 150px;"
                                                onclick="ShowHideCustomizationWindow();">
                                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/undo_16.gif" />Kolon
                                                Ekle / Çıkart
                                            </td>
                                        </tr>
                                    </table>
                                </Template>
                            </dxm:MenuItem>
                            <dxm:MenuItem Name="excel" Text="EXCEL" ToolTip="EXCEL olarak kaydet">
                                <Image Url="~/images/xls_ico.gif" />
                            </dxm:MenuItem>
                            <dxm:MenuItem Name="pdf" Text="PDF" ToolTip="PDF olarak kaydet">
                                <Image Url="~/images/pdf_icon.gif" />
                            </dxm:MenuItem>
                        </Items>
                    </dxm:ASPxMenu>
                    <hr />
                    <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                        CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID"
                        ClientInstanceName="Grid" OnCustomCallback="grid_CustomCallback">
                        <SettingsText Title="İŞ ÖZETİ RAPORU" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
                            ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="#" CustomizationWindowCaption="Kolon Ekle/Çıkart" />
                        <SettingsEditing Mode="inline" PopupEditFormWidth="750px" PopupEditFormHorizontalOffset="50"
                            PopupEditFormVerticalOffset="50" />
                        <SettingsPager PageSize="50" ShowSeparators="True">
                        </SettingsPager>
                        <Images ImageFolder="~/App_Themes/Glass/{0}/">
                        </Images>
                        <SettingsCustomizationWindow Enabled="True" />
                        <SettingsLoadingPanel Text="Yükleniyor..." />
                        <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                            ShowPreview="True" ShowTitlePanel="True" ShowGroupPanel="true" ShowFooter="true" />
                        <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                            <AlternatingRow Enabled="True">
                            </AlternatingRow>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                        </Styles>
                        <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" AllowFocusedRow="true" />
                        <Columns>
                            <dxwgv:GridViewDataColumn FieldName="ID" ShowInCustomizationForm="false" Visible="False">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption="PNR" FieldName="Pnr" Width="60px">
                                <Settings AutoFilterCondition="Contains" />
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                                        runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../../Genel/Issue/edit.aspx?id="+Eval("Pnr")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                        Text='<%#Eval("Pnr")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataDateColumn Caption="" FieldName="GorevTarihi"
                                Width="60px">
                                <HeaderCaptionTemplate>
                                    Görev<br />
                                    Tarihi
                                </HeaderCaptionTemplate>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataDateColumn Caption="" FieldName="SonIslemTarihi"
                                Width="60px">
                                <HeaderCaptionTemplate>
                                    Son İşlem<br />
                                    Tarihi
                                </HeaderCaptionTemplate>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="SonIslemBugunFark" Caption="" Width="50px">
                                <HeaderCaptionTemplate>
                                    Son İşem<br />
                                    Bugün Farkı                                    
                                </HeaderCaptionTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="SonIslemFark" Caption="" Width="50px">
                                <HeaderCaptionTemplate>
                                    Son İşem<br />
                                    ilk işlem Farkı                                    
                                </HeaderCaptionTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataColumn Caption="Sınıf" FieldName="Sinif" Width="90px">
                                <Settings AutoFilterCondition="Contains" />
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption="Tespit Eden" FieldName="CreatedBy" Width="120px">
                                <Settings AutoFilterCondition="Contains" />
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption="Konu Başlığı" FieldName="Baslik" Width="250px">
                                <Settings AutoFilterCondition="Contains" />
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                                        runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../../Genel/Issue/edit.aspx?id="+Eval("Pnr")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                        Text='<%#Eval("Baslik")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataCheckColumn FieldName="CvpYok" Width="70px">
                                <HeaderCaptionTemplate>
                                    Gündeme Giriş Yapılmış<br />
                                    Cevap Yok
                                </HeaderCaptionTemplate>
                                <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0"></PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn FieldName="BendenBekleniyor" Caption="" Width="55px">
                                <HeaderCaptionTemplate>
                                    Benden Cevap<br />
                                    Bekleniyor
                                </HeaderCaptionTemplate>
                                <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0"></PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn FieldName="OnlardanBekleniyor" Caption="" Width="55px">
                                <HeaderCaptionTemplate>
                                    Karşıdan Cevap<br />
                                    Bekleniyor
                                </HeaderCaptionTemplate>
                                <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0"></PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn FieldName="SadeceBenKapadim" Caption="" Width="55px">
                                <HeaderCaptionTemplate>
                                    Sadece Ben<br />
                                    Kapattım
                                </HeaderCaptionTemplate>
                                <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0"></PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn FieldName="SadeceOnlarKapadi" Caption="" Width="55px">
                                <HeaderCaptionTemplate>
                                    Sadece Diğerleri<br />
                                    Kapattı
                                </HeaderCaptionTemplate>
                                <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0"></PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn FieldName="Acik" Caption="" Width="50px">
                                <HeaderCaptionTemplate>
                                    Açık
                                </HeaderCaptionTemplate>
                                <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0"></PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                            <dxwgv:GridViewDataCheckColumn FieldName="Kapali" Caption="" Width="50px">
                                <HeaderCaptionTemplate>
                                    Kapalı
                                </HeaderCaptionTemplate>
                                <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0"></PropertiesCheckEdit>
                            </dxwgv:GridViewDataCheckColumn>
                        </Columns>
                    </dxwgv:ASPxGridView>
                    <dxwgv:ASPxGridViewExporter ID="gridExport" runat="server">
                        <Styles>
                            <Cell Font-Names="Verdana" Font-Size="8">
                            </Cell>
                            <Header Font-Names="Verdana" Font-Size="8">
                            </Header>
                        </Styles>
                    </dxwgv:ASPxGridViewExporter>
                </dxp:PanelContent>
            </PanelCollection>
        </dxrp:ASPxRoundPanel>
    </div>
</asp:Content>
