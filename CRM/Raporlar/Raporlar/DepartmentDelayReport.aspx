<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="DepartmentDelayReport.aspx.cs"
    Inherits="CRM_Raporlar_Raporlar_DepartmentDelayReport" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <script type="text/javascript" language="javascript">
            function ShowHideCustomizationWindow() {
                if (Grid.IsCustomizationWindowVisible())
                    Grid.HideCustomizationWindow();
                else Grid.ShowCustomizationWindow();
            }

        </script>
        <model:DataTable ID="DataTableList" runat="server" />
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
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="Grid.PerformCallback('x');">
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
        <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="750px"
            BackColor="#EBF2F4" CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass"
            ShowHeader="False">
            <PanelCollection>
                <dxrp:PanelContent ID="PanelContent1" runat="server">
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
                                İlgili Birim Seç
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxComboBox ID="FirmaID" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" EnableIncrementalFiltering="True" ImageFolder="~/App_Themes/Glass/{0}/"
                                    Width="200px" ValueType="System.String" EnableCallbackMode="true" CallbackPageSize="15" ClientInstanceName="cmbFirmaId">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                    <ClientSideEvents SelectedIndexChanged="function(s, e) {cmbProjeID.PerformCallback(s.GetValue());}" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td align="left" valign="top">
                                Departman Seç
                            </td>
                            <td align="left" style="width: 250px" valign="top">
                                <dxe:ASPxComboBox ID="ProjeID" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                    CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" ValueType="System.String"
                                    EnableIncrementalFiltering="true" CallbackPageSize="15"  EnableCallbackMode="true"
                                    ClientInstanceName="cmbProjeID" OnCallback="ProjeID_Callback">
                                    <ButtonStyle Cursor="pointer" Width="13px">
                                    </ButtonStyle>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>                       
                    </table>
                </dxrp:PanelContent>
            </PanelCollection>
        </dxrp:ASPxRoundPanel>
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" ClientInstanceName="Grid"
            OnCustomCallback="grid_CustomCallback" Width="1200px">
            <SettingsText Title="Departman-Kullanıcı Raporları" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="#" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsLoadingPanel Text="Yükleniyor..." />
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                ShowGroupPanel="True" ShowPreview="True" ShowTitlePanel="True" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <AlternatingRow Enabled="True">
                </AlternatingRow>
                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
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
                <dxwgv:GridViewDataColumn FieldName="Id" Visible="False" ShowInCustomizationForm="false">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="IssueId" Caption="PNR" Width="60px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="UserName" Caption="Kullanıcı">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Gündem Tanımı" FieldName="Baslik">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                            runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../../Genel/Issue/edit.aspx?id="+Eval("IssueId")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                            Text='<%#Eval("Baslik")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Tespit Eden" FieldName="CreatedBy" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataColumn FieldName="FirmaName" Caption="İlgili Birim">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="ProjeName" Caption="Departman">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
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
                <dxwgv:GridViewDataTextColumn FieldName="DurumName" Width="75px" Caption="Durum">
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Anahtar Kelime" Width="100px" FieldName="KeyWords"
                    Visible="false">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <dxwgv:ASPxGridViewExporter ExportedRowType="Selected" ID="gridExport" runat="server">
            <Styles>
                <Cell Font-Names="Verdana" Font-Size="8">
                </Cell>
                <Header Font-Names="Verdana" Font-Size="8">
                </Header>
            </Styles>
        </dxwgv:ASPxGridViewExporter>
    </div>
</asp:Content>
