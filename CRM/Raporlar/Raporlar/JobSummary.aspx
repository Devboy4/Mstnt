<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="JobSummary.aspx.cs" Inherits="CRM_Raporlar_JobSummary" %>

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
                                                <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/List.gif" />Listele
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
                    <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="750px"
                        BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
                        ShowHeader="False">
                        <PanelCollection>
                            <dxrp:PanelContent ID="PanelContent2" runat="server">
                                <table border="0" cellpadding="0" cellspacing="3" style="width: 750px">

                                    <tr>
                                        <td align="left" style="width: 150px" valign="top">Kullanıcı
                                        </td>
                                        <td align="left" style="width: 250px" valign="top">
                                            <dxe:ASPxComboBox ID="UserId" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                Width="200px" CssPostfix="Glass" ClientInstanceName="cmbUserId" DataSourceID="DSUser" EnableIncrementalFiltering="True"
                                                ImageFolder="~/App_Themes/Glass/{0}/" ValueType="System.Int32" ValueField="UserId"
                                                TextField="UserName" EnableCallbackMode="true" CallbackPageSize="15">
                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td align="left" valign="top"></td>
                                        <td align="left" style="width: 250px" valign="top"></td>
                                    </tr>
                                </table>
                            </dxrp:PanelContent>
                        </PanelCollection>
                    </dxrp:ASPxRoundPanel>
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
                            <dxwgv:GridViewDataColumn FieldName="UserId" ShowInCustomizationForm="false" Visible="False">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption="SINIF" FieldName="Sinif" Width="110px">
                                <Settings AutoFilterCondition="Contains" />
                                <CellStyle Font-Bold="true" Font-Size="12px"></CellStyle>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="CvpYok" Caption="" Width="70px">
                                <HeaderCaptionTemplate>
                                    Gündeme Giriş Yapılmış<br />
                                    Cevap Yok
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" Font-Size="12px" Font-Bold="true" runat="server" NavigateUrl=<%#"JavaScript: window.location.href='./JobSummaryDetail.aspx?OpId=1&UserId="+Eval("UserId")+"&Vs="+Eval("Sinif")+"';"%>
                                        Text='<%#Eval("CvpYok")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="BendenBekleniyor" Caption="" Width="50px">
                                <HeaderCaptionTemplate>
                                    Benden Cevap<br />
                                    Bekleniyor
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" Font-Size="12px" Font-Bold="true" runat="server" NavigateUrl=<%#"JavaScript: window.location.href='./JobSummaryDetail.aspx?OpId=2&UserId="+Eval("UserId")+"&Vs="+Eval("Sinif")+"';"%>
                                        Text='<%#Eval("BendenBekleniyor")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="OnlardanBekleniyor" Caption="" Width="50px">
                                <HeaderCaptionTemplate>
                                    Karşıdan Cevap<br />
                                    Bekleniyor
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" Font-Size="12px" Font-Bold="true" runat="server" NavigateUrl=<%#"JavaScript: window.location.href='./JobSummaryDetail.aspx?OpId=3&UserId="+Eval("UserId")+"&Vs="+Eval("Sinif")+"';"%>
                                        Text='<%#Eval("OnlardanBekleniyor")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="SadeceBenKapadim" Caption="" Width="65px">
                                <HeaderCaptionTemplate>
                                    Sadece Ben<br />
                                    Kapattım
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" Font-Size="12px" Font-Bold="true" runat="server" NavigateUrl=<%#"JavaScript: window.location.href='./JobSummaryDetail.aspx?OpId=4&UserId="+Eval("UserId")+"&Vs="+Eval("Sinif")+"';"%>
                                        Text='<%#Eval("SadeceBenKapadim")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="SadeceOnlarKapadi" Caption="" Width="50px">
                                <HeaderCaptionTemplate>
                                    Sadece Diğerleri<br />
                                    Kapattı
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" Font-Size="12px" Font-Bold="true" runat="server" NavigateUrl=<%#"JavaScript: window.location.href='./JobSummaryDetail.aspx?OpId=5&UserId="+Eval("UserId")+"&Vs="+Eval("Sinif")+"';"%>
                                        Text='<%#Eval("SadeceOnlarKapadi")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="Acik" Caption="" Width="40px">
                                <HeaderCaptionTemplate>
                                    Açık
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" Font-Size="12px" Font-Bold="true" runat="server" NavigateUrl=<%#"JavaScript: window.location.href='./JobSummaryDetail.aspx?OpId=6&UserId="+Eval("UserId")+"&Vs="+Eval("Sinif")+"';"%>
                                        Text='<%#Eval("Acik")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="Kapali" Caption="" Width="40px">
                                <HeaderCaptionTemplate>
                                    Kapalı
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" Font-Size="12px" Font-Bold="true" runat="server" NavigateUrl=<%#"JavaScript: window.location.href='./JobSummaryDetail.aspx?OpId=7&UserId="+Eval("UserId")+"&Vs="+Eval("Sinif")+"';"%>
                                        Text='<%#Eval("Kapali")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="Toplam" Caption="" Width="40px">
                                <HeaderCaptionTemplate>
                                    Toplam
                                </HeaderCaptionTemplate>
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" Font-Size="12px" Font-Bold="true" runat="server" NavigateUrl=<%#"JavaScript: window.location.href='./JobSummaryDetail.aspx?OpId=8&UserId="+Eval("UserId")+"&Vs="+Eval("Sinif")+"';"%>
                                        Text='<%#Eval("Toplam")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>

                            </dxwgv:GridViewDataTextColumn>
                        </Columns>
                        <TotalSummary>
                            <dxwgv:ASPxSummaryItem DisplayFormat="Toplam: {0}" SummaryType="Sum" FieldName="CvpYok" ShowInColumn="CvpYok" ShowInGroupFooterColumn="CvpYok" />
                            <dxwgv:ASPxSummaryItem DisplayFormat="Toplam: {0}" SummaryType="Sum" FieldName="BendenBekleniyor" ShowInColumn="BendenBekleniyor" ShowInGroupFooterColumn="BendenBekleniyor" />
                            <dxwgv:ASPxSummaryItem DisplayFormat="Toplam: {0}" SummaryType="Sum" FieldName="OnlardanBekleniyor" ShowInColumn="OnlardanBekleniyor" ShowInGroupFooterColumn="OnlardanBekleniyor" />
                            <dxwgv:ASPxSummaryItem DisplayFormat="Toplam: {0}" SummaryType="Sum" FieldName="SadeceBenKapadim" ShowInColumn="SadeceBenKapadim" ShowInGroupFooterColumn="SadeceBenKapadim" />
                            <dxwgv:ASPxSummaryItem DisplayFormat="Toplam: {0}" SummaryType="Sum" FieldName="SadeceOnlarKapadi" ShowInColumn="SadeceOnlarKapadi" ShowInGroupFooterColumn="SadeceOnlarKapadi" />
                            <dxwgv:ASPxSummaryItem DisplayFormat="Toplam: {0}" SummaryType="Sum" FieldName="Acik" ShowInColumn="Acik" ShowInGroupFooterColumn="Acik" />
                            <dxwgv:ASPxSummaryItem DisplayFormat="Toplam: {0}" SummaryType="Sum" FieldName="Kapali" ShowInColumn="Kapali" ShowInGroupFooterColumn="Kapali" />
                            <dxwgv:ASPxSummaryItem DisplayFormat="Toplam: {0}" SummaryType="Sum" FieldName="Toplam" ShowInColumn="Toplam" ShowInGroupFooterColumn="Toplam" />
                        </TotalSummary>
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
