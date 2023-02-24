<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="CRM_Genel_BR_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxCallback"
    TagPrefix="dxcb" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
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
        <script type="text/javascript">
            function SetDivVisibility(DivId, ImgId) {
                //if((DivId=="div1") || (DivId=="div2"))
                //{
                if (document.getElementById(DivId).style.visibility == "visible") {
                    document.getElementById(DivId).style.visibility = "hidden";
                    document.getElementById(DivId).style.position = "absolute";
                    document.getElementById(ImgId).src = "../../../App_Themes/Glass/PivotGrid/pgCollapsedButton.gif";
                }
                else {
                    document.getElementById(DivId).style.visibility = "visible";
                    document.getElementById(DivId).style.position = "relative";
                    document.getElementById(ImgId).src = "../../../App_Themes/Glass/PivotGrid/pgExpandedButton.gif";
                }
                //}
            }
        </script>
        <model:DataTable ID="DataTableList" runat="server" />
        <asp:SqlDataSource ID="DSBrMarka" runat="server" SelectCommand="Select BrMarkaID,Adi From BrMarka Order By Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DSProje" runat="server" SelectCommand="Select ProjeID,Adi From Proje Order By Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DSBrDurum" runat="server" SelectCommand="Select BrDurumID,Adi From BrDurum Order By Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DSSize" runat="server" SelectCommand="Select Size From BrTablosu Group By Size Order By Size"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
        <asp:SqlDataSource ID="DSStokKodu" runat="server" SelectCommand="Select StokKodu From BrTablosu Group By StokKodu Order By StokKodu"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
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
                <dxm:MenuItem Name="New">
                    <Template>
                        <table width="50" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id=0',850,650);PopWin.focus();">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/new.gif" />Yeni
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="AddAttachment">
                    <Template>
                        <table width="50" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="Grid.PerformCallback('y');">
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
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="ShowHideCustomizationWindow();">
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
        <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" HeaderText="Kriter Ekranı" runat="server"
            Width="750px">
            <PanelCollection>
                <dxp:PanelContent runat="server">
                    <table border="0" cellpadding="0" cellspacing="3" style="width: 750px">
                        <tr>
                            <td align="left" style="width: 150px" valign="top">
                                Başlangıç Tarihi
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxDateEdit ID="StartDate" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                            <td align="left" valign="top">
                                Bitiş Tarihi
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxDateEdit ID="EndDate" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td align="left" style="width: 150px" valign="top">
                                Br No
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxTextBox ID="IndexID" runat="server" Width="170px">
                                </dxe:ASPxTextBox>
                                <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="IndexID"
                                    ErrorMessage="Sadece Sayı Girilmelidir" MaximumValue="100000000" MinimumValue="0"
                                    Type="Integer"></asp:RangeValidator>
                            </td>
                            <td align="left" valign="top">
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                            </td>
                        </tr>
                    </table>
                </dxp:PanelContent>
            </PanelCollection>
        </dxrp:ASPxRoundPanel>
        <table border="0" width="100%" style="background-image: url('../../../App_Themes/Blue (Horizontal orientation)/Web/mItemBack.gif');">
            <tr>
                <td align="center" style="width: 20px;">
                    <img id="img1" src="../../../App_Themes/Glass/PivotGrid/pgCollapsedButton.gif" alt="Daralt/Genişlet"
                        onclick="SetDivVisibility('div1','Img1');" onmouseover="document.getElementById('img1').style.cursor = 'pointer';"
                        onmouseout="document.getElementById('img1').style.cursor = 'default';" />
                </td>
                <td align="left">
                    <dxe:ASPxLabel ID="lbl1" runat="server" Text="Br Kriterleri" />
                </td>
            </tr>
        </table>
        <div id="div1" style="position: absolute; visibility: hidden;">
            <table border="0" cellpadding="0" cellspacing="3" style="width: 750px">
                <tr>
                    <td align="left" style="width: 150px" valign="top">
                        Br Durum Seç
                    </td>
                    <td align="left" valign="top" colspan="3">
                        <asp:CheckBoxList ID="DurumList1" runat="server" CellPadding="0" CellSpacing="2"
                            Font-Bold="False" Font-Names="Arial" Font-Size="11px" RepeatColumns="3" Width="100%">
                        </asp:CheckBoxList>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="1500px"
            ClientInstanceName="Grid" OnCustomCallback="grid_CustomCallback" OnHtmlRowPrepared="grid_HtmlRowPrepared"
            OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared">
            <SettingsText Title="BR Listesi" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
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
                ShowPreview="True" ShowTitlePanel="True" ShowGroupPanel="true" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <AlternatingRow Enabled="True">
                </AlternatingRow>
                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" AllowFocusedRow="true" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="25px">
                    <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                        <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" ShowInCustomizationForm="false" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="YaziRengi" ShowInCustomizationForm="false" Visible="false">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="IndexID" Caption="Br No" Width="30px">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="IssueLink" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("IndexID")+"&NoteOwner=4',850,650);PopWin.focus();"%>
                            Text='<%#Eval("IndexID")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Width="100px"
                    FieldName="Tarih" Caption="Tarih">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <%--<dxwgv:GridViewDataColumn FieldName="RefNo" Caption="Referans No" Width="60px" />--%>
                <dxwgv:GridViewDataComboBoxColumn FieldName="StokKodu" Caption="Stok Kodu" Width="100px">
                    <PropertiesComboBox ValueField="StokKodu" TextField="StokKodu" ValueType="System.String"
                        EnableCallbackMode="true" CallbackPageSize="50" DataSourceID="DsStokKodu" EnableIncrementalFiltering="true">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <%--<dxwgv:GridViewDataColumn FieldName="StokKodu" Caption="Stok Kodu" Width="100px" />--%>
                <dxwgv:GridViewDataColumn FieldName="Renk" Caption="Renk" Width="100px" />
                <dxwgv:GridViewDataComboBoxColumn FieldName="Size" Caption="Size" Width="75px">
                    <PropertiesComboBox ValueField="Size" TextField="Size" ValueType="System.String"
                        EnableCallbackMode="true" CallbackPageSize="50" DataSourceID="DsSize" EnableIncrementalFiltering="true">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Durum" FieldName="BrDurumID" Width="100px">
                    <PropertiesComboBox TextField="Adi" DataSourceID="DSBrDurum" ValueField="BrDurumID"
                        EnableIncrementalFiltering="true" ValueType="System.Guid" EnableCallbackMode="true"
                        CallbackPageSize="15">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="Marka" FieldName="BrMarkaID" Width="100px">
                    <PropertiesComboBox TextField="Adi" DataSourceID="DSBrMarka" ValueField="BrMarkaID"
                        EnableIncrementalFiltering="true" ValueType="System.Guid" EnableCallbackMode="true"
                        CallbackPageSize="15">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="İsteyen" FieldName="isteyenProjeID" Width="100px">
                    <PropertiesComboBox TextField="Adi" DataSourceID="DSProje" ValueField="ProjeID" EnableIncrementalFiltering="true"
                        ValueType="System.Guid" EnableCallbackMode="true" CallbackPageSize="15">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn Caption="İstenen" FieldName="istenilenProjeID"
                    Width="100px">
                    <PropertiesComboBox TextField="Adi" DataSourceID="DSProje" ValueField="ProjeID" EnableIncrementalFiltering="true"
                        ValueType="System.Guid" EnableCallbackMode="true" CallbackPageSize="15">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataColumn FieldName="Adet" Caption="Adet" Width="50px" />
                <dxwgv:GridViewDataColumn FieldName="irsaliyeNo" Caption="İrs. No" Width="50px" />
                <dxwgv:GridViewDataColumn FieldName="MusteriAdi" Caption="Müşteri Adı" Width="150px" />
                <dxwgv:GridViewDataColumn FieldName="MusteriTel" Caption="Müşteri Tel" Width="150px" />
                <dxwgv:GridViewDataColumn FieldName="PersonelAdi" Caption="Personel Adı" Width="150px" />
                <dxwgv:GridViewDataColumn FieldName="CreatedBy" Caption="Ekleyen" Width="50px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="ModifiedBy" Caption="Düzenleyen" Width="50px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Width="100px"
                    FieldName="CreationDate" Caption="Oluşturma Tarihi">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Width="100px"
                    FieldName="ModificationDate" Caption="Düzenlenme Tarihi">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
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
    </div>
</asp:Content>
