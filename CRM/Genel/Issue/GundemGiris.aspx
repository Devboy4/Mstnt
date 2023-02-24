<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="GundemGiris.aspx.cs"
    Inherits="CRM_Genel_Issue_GundemGiris" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
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
    <div>
        <script type="text/javascript" src="crm_20141215.js"></script>
        <model:DataTable ID="DataTableList" runat="server" />
        <model:DataTable ID="DTDetail" runat="server" />
        <model:DataTable ID="DataTable1" runat="server" />
        <asp:HiddenField ID="UserName" runat="server" />
        <asp:HiddenField ID="HiddenKisiAta" runat="server" />
        <asp:SqlDataSource ID="DSVirusSinif" runat="server" SelectCommand="SELECT IndexId,Adi FROM VirusSinif ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        &nbsp;<br />
        <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" ItemSpacing="0px"
            SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True" ShowSubMenuShadow="False">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem Name="AddExtrachtColumns">
                    <Image Url="~/images/new.gif" />
                    <Template>
                        <table width="150" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="ShowHideCustomizationWindow();">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/undo_16.gif" />Kolon
                                    Ekle / Çýkart
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="NewIssue">
                    <Image Url="~/images/new.gif" />
                    <Template>
                        <table width="150" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="JavaScript:PopWin = OpenPopupWinBySize('./AddIssuePopUp.aspx',800,650);PopWin.focus();">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/Menu_YeniBildirim.gif" />
                                    Yeni Gündem Giriþi
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="SetAntivirus" Text="Seçilenleri Kapat" ToolTip="Seçilenleri Kapat">
                    <Image Url="~/images/saveopen.png" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="meeting" Text="Toplantý Oluþtur" ToolTip="Seçilen Gündemler hakkýnda toplantý oluþtur">
                    <Image Url="~/images/meeting.png" />
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
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" DataSourceID="DataTableList"
            KeyFieldName="ID" ClientInstanceName="Grid" OnCustomCallback="grid_CustomCallback"
            OnHtmlRowPrepared="grid_HtmlRowPrepared" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
            OnAfterPerformCallback="grid_AfterPerformCallback" OnDetailRowGetButtonVisibility="grid_DetailRowGetButtonVisibility">
            <SettingsText Title="Üzerimdeki Tüm Açýk Gündemler" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="#" CustomizationWindowCaption="Kolon Ekle/Çýkart" />
            <SettingsPager PageSize="50" ShowSeparators="True">
            </SettingsPager>
            <SettingsDetail ShowDetailRow="true" />
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsLoadingPanel Text="Yükleniyor..." />
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="true"
                ShowPreview="True" ShowGroupPanel="true" />
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" AllowFocusedRow="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" ShowSelectCheckbox="true" VisibleIndex="0"
                    Width="80px">
                    <HeaderTemplate>
                        <input id="Button1" type="button" onclick="Grid.PerformCallback(true);" value="+"
                            title="Tümünü Seç" />
                        <input id="Button2" type="button" onclick="Grid.PerformCallback(false);" value="-"
                            title="Tümünü Seçme" />
                    </HeaderTemplate>
                    <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                        <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" ShowInCustomizationForm="false" Visible="False"
                    Width="0px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="DurumID" ShowInCustomizationForm="false" Visible="False"
                    Width="0px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="YaziRengi" ShowInCustomizationForm="false" Visible="false"
                    Width="0px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="EventCount" ShowInCustomizationForm="false"
                    Visible="false" Width="0px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="AntSayi" ShowInCustomizationForm="false" Visible="false"
                    Width="0px" SortOrder="Ascending" SortIndex="0" Settings-AllowSort="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Ana" Width="40px" FieldName="IsMain">
                    <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataColumn CellStyle-HorizontalAlign="Right" FieldName="EventCount"
                    Caption=" " Width="30px">
                    <DataItemTemplate>
                        <dxe:ASPxImage runat="server" ID="ImgEventCount" Cursor="pointer" ImageUrl="~/images/uyarivar.gif"
                            Width="20px" Height="20px">
                        </dxe:ASPxImage>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="IndexID" Width="60px" Caption="PNR" Settings-AllowSort="False" />
                <dxwgv:GridViewDataColumn Caption="Tespit Eden" FieldName="CreatedBy" Width="120px">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="Baslik" Caption="Gündem Tanýsý" Width="300px">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="IssueLink" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("ID")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                            Text='<%#Eval("Baslik")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn CellStyle-HorizontalAlign="Right" Caption=" " Width="30px">
                    <DataItemTemplate>
                        <img src="../../../images/details.png" alt="Yazýþma Geçmiþini Aç" style="cursor: pointer"
                            onclick="OpenIssueDetail('<%#Eval("ID") %>')" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn CellStyle-HorizontalAlign="Right" FieldName="AntSay" Caption=" "
                    Width="25px">
                    <DataItemTemplate>
                        <dxe:ASPxImage runat="server" ID="ImgAntSayi" />
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption=" " FieldName="RelatedPop3Id" Width="20px">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="img_Sound" runat="server" NavigateUrl=<%#"JavaScript:OpenMedyaPage('"+Eval("RelatedPop3Id")+"');"%>
                            ImageUrl="~/images/sound.png" ImageHeight="16px" ImageWidth="16px">
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Departman" Settings-AutoFilterCondition="Contains"
                    FieldName="ProjeName" Width="75px" />
                <dxwgv:GridViewDataDateColumn Caption="Tespit Tarihi" SortOrder="Descending" Width="70px"
                    FieldName="BildirimTarihi">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn Caption="Planlanan Op. Tarihi" FieldName="TeslimTarihi"
                    Width="60px">
                    <HeaderCaptionTemplate>
                        Planlanan<br />
                        Op. Tarihi
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataColumn Visible="false" FieldName="MainBaslik" Caption="Ana Gündem"
                    Width="0px">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Sýnýf" FieldName="VirusSinifID" Width="75px">
                    <PropertiesComboBox TextField="Adi" DataSourceID="DSVirusSinif" ValueField="IndexId"
                        EnableIncrementalFiltering="true" ValueType="System.Int32" EnableCallbackMode="true"
                        CallbackPageSize="15">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataTextColumn FieldName="DurumName" Width="75px" SortOrder="Descending"
                    Settings-AllowSort="False" Caption="Durum" GroupIndex="0">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Birim" FieldName="FirmaName" Width="75px" />
                <dxwgv:GridViewDataTextColumn Caption="Op. Süresi" Width="100%" FieldName="HarcananSure"
                    UnboundType="decimal">
                    <PropertiesTextEdit DisplayFormatString="{0:#0}" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataMemoColumn Width="0px" Visible="false" ShowInCustomizationForm="false"
                    Caption="Son Yorum" FieldName="Description">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataMemoColumn Width="0px" Caption="Son Yapýlan Yorum" Visible="false"
                    ShowInCustomizationForm="false" FieldName="SonYorum">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataColumn Caption="Anahtar Kelime" FieldName="KeyWords" Visible="false"
                    Width="0px">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
            </Columns>
            <Templates>
                <DetailRow>
                    <dxwgv:ASPxGridView ID="gvDetail" runat="server" KeyFieldName="ID" OnBeforePerformDataSelect="gvDetail_BeforePerformDataSelect"
                        OnHtmlRowPrepared="gvDetail_HtmlRowPrepared" OnHtmlDataCellPrepared="gvDetail_HtmlDataCellPrepared"
                        OnAfterPerformCallback="gvDetail_AfterPerformCallback" EnableCallBacks="true">
                        <Columns>
                            <dxwgv:GridViewDataColumn FieldName="ID" ShowInCustomizationForm="false" Visible="False"
                                Width="0px">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="DurumID" ShowInCustomizationForm="false" Visible="False"
                                Width="0px">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="YaziRengi" ShowInCustomizationForm="false" Visible="false"
                                Width="0px">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="EventCount" ShowInCustomizationForm="false"
                                Visible="false" Width="0px">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="AntSayi" ShowInCustomizationForm="false" Visible="false"
                                Width="0px" SortOrder="Ascending" SortIndex="0" Settings-AllowSort="False">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn CellStyle-HorizontalAlign="Right" FieldName="ImgNotice"
                                Caption=" " Width="30px">
                                <DataItemTemplate>
                                    <dxe:ASPxImage runat="server" ID="ImgEventCount" Cursor="pointer" ImageUrl="~/images/uyarivar.gif"
                                        Width="20px" Height="20px">
                                    </dxe:ASPxImage>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="IndexID" Width="60px" Caption="PNR" Settings-AllowSort="False" />
                            <dxwgv:GridViewDataColumn Caption="Tespit Eden" FieldName="CreatedBy" Width="120px">
                                <Settings AutoFilterCondition="Contains" />
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="Baslik" Caption="Gündem Tanýsý" Width="300px">
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("ID")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                        Text='<%#Eval("Baslik")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                                <Settings AutoFilterCondition="Contains" />
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption=" " Width="30px">
                                <DataItemTemplate>
                                    <img src="../../../images/details.png" alt="Yazýþma Geçmiþini Aç" style="cursor: pointer"
                                        onclick="OpenIssueDetail('<%#Eval("ID") %>')" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="AntSay" Caption=" " Width="25px">
                                <DataItemTemplate>
                                    <dxe:ASPxImage runat="server" ID="ImgAntSayi" />
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption=" " FieldName="RelatedPop3Id" Width="20px">
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="img_Sound" runat="server" NavigateUrl=<%#"JavaScript:OpenMedyaPage('"+Eval("RelatedPop3Id")+"');"%>
                                        ImageUrl="~/images/sound.png" ImageHeight="16px" ImageWidth="16px">
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption="Departman" Settings-AutoFilterCondition="Contains"
                                FieldName="ProjeName" Width="75px" />
                            <dxwgv:GridViewDataDateColumn Caption="Tespit Tarihi" SortOrder="Descending" Width="60px"
                                FieldName="BildirimTarihi">
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataDateColumn Caption="Planlanan Op. Tarihi" FieldName="TeslimTarihi"
                                Width="60px">
                                <HeaderCaptionTemplate>
                                    Planlanan<br />
                                    Op. Tarihi
                                </HeaderCaptionTemplate>
                            </dxwgv:GridViewDataDateColumn>
                            <dxwgv:GridViewDataTextColumn FieldName="DurumName" Width="75px" SortOrder="Descending"
                                Settings-AllowSort="False" Caption="Durum">
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataColumn Caption="Birim" FieldName="FirmaName" Width="75px" />
                            <dxwgv:GridViewDataTextColumn Caption="Op. Süresi" Width="50px" FieldName="HarcananSure"
                                UnboundType="decimal">
                                <PropertiesTextEdit DisplayFormatString="{0:#0}" />
                            </dxwgv:GridViewDataTextColumn>
                            <dxwgv:GridViewDataColumn Caption="Anahtar Kelime" FieldName="KeyWords" Visible="false"
                                Width="0px">
                                <Settings AutoFilterCondition="Contains" />
                            </dxwgv:GridViewDataColumn>
                        </Columns>
                        <SettingsDetail IsDetailGrid="True" />
                        <Settings ShowColumnHeaders="false" ShowFilterRow="true" />
                        <%--                        <SettingsText EmptyDataRow="#" />
                        <SettingsPager PageSize="50" ShowSeparators="True" />
                        <SettingsLoadingPanel Text="Yükleniyor..." />
                        <Images ImageFolder="~/App_Themes/Glass/{0}/" />
                        <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                            <AlternatingRow Enabled="True">
                            </AlternatingRow>
                            <Header SortingImageSpacing="5px" ImageSpacing="5px">
                            </Header>
                        </Styles>--%>
                    </dxwgv:ASPxGridView>
                </DetailRow>
            </Templates>
        </dxwgv:ASPxGridView>
        <dxwgv:ASPxGridViewExporter ExportedRowType="Selected" ID="gridExport" runat="server">
            <Styles>
                <Cell Font-Names="Verdana" Font-Size="8">
                </Cell>
                <Header Font-Names="Verdana" Font-Size="8">
                </Header>
            </Styles>
        </dxwgv:ASPxGridViewExporter>
        <dxpc:ASPxPopupControl ID="popup" runat="server" AllowDragging="True" AllowResize="True"
            CloseAction="MouseOut" EnableViewState="False" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowFooter="false" ShowHeader="true" Width="600px"
            Height="300px" FooterText="Paneli sað alt köþesinden tutup boyutlandýrabilirsiniz..."
            HeaderText="Gündem Tarihçesi" ClientInstanceName="FeedPopupControl" EnableHierarchyRecreation="True"
            DragElement="Window" Modal="false">
            <Windows>
                <dxpc:PopupWindow HeaderText="Gündem Tarihçesi" Modal="false" Name="Preview">
                </dxpc:PopupWindow>
            </Windows>
        </dxpc:ASPxPopupControl>
        <dxcb:ASPxCallback ID="CallbackPreview" ClientInstanceName="CallbackPreview" OnCallback="CallbackPreview_Callback"
            runat="server">
            <ClientSideEvents CallbackComplete="function(s, e) {   var win = FeedPopupControl.GetWindow(0);
                    FeedPopupControl.SetWindowContentUrl(win,'Preview.aspx?id='+e.result);
                    FeedPopupControl.ShowWindow(win);
                    window.scrollTo(0,0); 
                }" />
        </dxcb:ASPxCallback>
        <dxpc:ASPxPopupControl ID="popup2" runat="server" AllowDragging="True" AllowResize="True"
            CloseAction="MouseOut" EnableViewState="False" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowFooter="false" ShowHeader="true" Width="600px"
            Height="300px" FooterText="Paneli sað alt köþesinden tutup boyutlandýrabilirsiniz..."
            HeaderText="Olay Penceresi" ClientInstanceName="FeedPopupControl2" EnableHierarchyRecreation="True"
            DragElement="Window" Modal="false">
            <Windows>
                <dxpc:PopupWindow HeaderText="Olay Penceresi" Modal="false" Name="Preview">                    
                </dxpc:PopupWindow>
            </Windows>
             <ClientSideEvents Shown="function(s, e) {s.UpdatePosition();}" />
        </dxpc:ASPxPopupControl>
        <dxcb:ASPxCallback ID="CallbackPreview2" ClientInstanceName="CallbackPreview2" runat="server">
            <ClientSideEvents CallbackComplete="function(s, e) {   var win = FeedPopupControl2.GetWindow(0);
                    FeedPopupControl2.SetWindowContentUrl(win,'../../../Frames/Events.aspx?id=0');
                    FeedPopupControl2.ShowWindow(win);}" />
        </dxcb:ASPxCallback>
    </div>
</asp:Content>
