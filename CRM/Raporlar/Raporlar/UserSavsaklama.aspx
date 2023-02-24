<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="UserSavsaklama.aspx.cs" Inherits="CRM_Raporlar_UserSavsaklama" %>

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
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


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
                            <dxrp:PanelContent ID="PanelContent2" runat="server">
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
                                            Kullanıcı
                                        </td>
                                        <td align="left" style="width: 250px" valign="top">
                                            <dxe:ASPxComboBox ID="UserId" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                                Width="200px" CssPostfix="Glass" DataSourceID="DSUser" EnableIncrementalFiltering="True"
                                                ImageFolder="~/App_Themes/Glass/{0}/" ValueType="System.Int32" ValueField="UserId"
                                                TextField="UserName" EnableCallbackMode="true" CallbackPageSize="15">
                                                <ButtonStyle Cursor="pointer" Width="13px">
                                                </ButtonStyle>
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td align="left" valign="top">
                                        </td>
                                        <td align="left" style="width: 250px" valign="top">
                                        </td>
                                    </tr>
                                </table>
                            </dxrp:PanelContent>
                        </PanelCollection>
                    </dxrp:ASPxRoundPanel>
                    <hr />
                    <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                        CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="1500px"
                        ClientInstanceName="Grid" OnCustomCallback="grid_CustomCallback">
                        <SettingsText Title="Zamanında Yapılmayan işler Listesi" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
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
                            <dxwgv:GridViewCommandColumn ButtonType="Image" ShowSelectCheckbox="true" VisibleIndex="0"
                                Width="60px">
                                <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                                    <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                                </ClearFilterButton>
                            </dxwgv:GridViewCommandColumn>
                            <dxwgv:GridViewDataColumn FieldName="ID" ShowInCustomizationForm="false" Visible="False">
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="IndexID" Width="50px" Caption="PNR" />
                            <dxwgv:GridViewDataColumn Caption="Tespit Eden" FieldName="CreatedBy" Width="75px">
                                <Settings AutoFilterCondition="Contains" />
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn FieldName="Baslik" Caption="Gündem Tanısı" Width="300px">
                                <DataItemTemplate>
                                    <dxe:ASPxHyperLink ID="IssueLink" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../../Genel/Issue/edit.aspx?id="+Eval("IndexId")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                        Text='<%#Eval("Baslik")%>'>
                                    </dxe:ASPxHyperLink>
                                </DataItemTemplate>
                                <Settings AutoFilterCondition="Contains" />
                            </dxwgv:GridViewDataColumn>
                            <dxwgv:GridViewDataColumn Caption="Birim" FieldName="FirmaName" />
                            <dxwgv:GridViewDataColumn Caption="Departman" FieldName="ProjeName" />
                            <dxwgv:GridViewDataColumn Caption="Gündem Sınıfı" FieldName="VirusSinifName" />
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
