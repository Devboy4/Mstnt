<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="CRM_IthalatTakip_Tanim_Guvenlik_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <model:DataTable ID="DTList" runat="server" />
        <div>
            <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                ShowSubMenuShadow="False" AutoPostBack="True">
                
                <SubMenuStyle GutterWidth="0px" />
                <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
                <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
                </SubMenuItemStyle>
                <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                    X="2" Y="-12" />
                <Items>
                    <dxm:MenuItem Name="new">
                        <Template>
                            <table width="50" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="dxmMenuItem_Blue" style="cursor: pointer;" align="center" valign="middle"
                                        onclick="javascript:PopWin = OpenPopupWinBySize('edit.aspx?id=0',800,500);PopWin.focus();">
                                        <img src="../../../images/new.gif" alt="" />&nbsp;<b>Yeni</b>
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="save" Text="Kaydet">
                        <TextTemplate>
                            Kaydet</TextTemplate>
                        <Image Url="~/images/save.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="savenew" Text="Kaydet ve Yeni" Visible="false">
                        <TextTemplate>
                            Kaydet ve Yeni</TextTemplate>
                        <Image Url="~/images/savenew.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="saveclose" Text="Kaydet ve Kapat" Visible="false">
                        <TextTemplate>
                            Kaydet ve Kapat</TextTemplate>
                        <Image Url="~/images/saveclose.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="delete" Text="Sil" Visible="false">
                        <TextTemplate>
                            Sil</TextTemplate>
                        <Image Url="~/images/delete.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Text="Yenile" NavigateUrl="javascript:location.reload(true);">
                        <Image Url="~/images/reload2.jpg" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="excel" Text="EXCEL" ToolTip="EXCEL olarak kaydet">
                        <TextTemplate>
                            EXCEL</TextTemplate>
                        <Image Url="~/images/xls_ico.gif" />
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="pdf" Text="PDF" ToolTip="PDF olarak kaydet">
                        <TextTemplate>
                            PDF</TextTemplate>
                        <Image Url="~/images/pdf_icon.gif" />
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <dxwgv:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DTList" KeyFieldName="ID" Width="1200px" SettingsEditing-Mode="Inline"
                ClientInstanceName="Grid" OnCustomJSProperties="Grid_CustomJSProperties" OnHtmlDataCellPrepared="Grid_HtmlDataCellPrepared">
                <Columns>
                    <dxwgv:GridViewCommandColumn Width="30px" VisibleIndex="0" ButtonType="Image">
                        <ClearFilterButton Visible="True" Text="Süzme Ýptal">
                            <Image AlternateText="Süzme Ýptal" Url="~/images/reload2.jpg" />
                        </ClearFilterButton>
                        <DeleteButton Visible="True" Text="Sil">
                            <Image AlternateText="Sil" Url="~/images/delete.gif" />
                        </DeleteButton>
                        <UpdateButton Visible="True" Text="Güncelle">
                            <Image AlternateText="Güncelle" Url="~/images/update.gif" />
                        </UpdateButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                    <dxwgv:GridViewDataColumn Caption="Açýklama" FieldName="Aciklama" Settings-AutoFilterCondition="Contains">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("ID")+"',800,500);PopWin.focus();"%>
                                Text='<%#Eval("Aciklama")%>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                </Columns>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <Settings ShowFilterRow="True" ShowStatusBar="Hidden" ShowGroupedColumns="True" ShowGroupPanel="True"
                    ShowPreview="True" ShowTitlePanel="True" ShowVerticalScrollBar="False" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                </Styles>
                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                <SettingsPager PageSize="19" ShowSeparators="True">
                </SettingsPager>
                <SettingsText Title="Kayýt Giriþi Güvenlik" EmptyDataRow="." />
            </dxwgv:ASPxGridView>
            <dxwgv:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="Grid" Landscape="false">
            </dxwgv:ASPxGridViewExporter>
        </div>
  </asp:Content>

