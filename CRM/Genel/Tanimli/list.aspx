<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="list.aspx.cs" Inherits="CRM_Genel_Tanimli_list" %>

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
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
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
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="AddAttachment">
                    <Template>

                        <table width="70px" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer; padding-right: 4px; border-right-width: 0px; width: 150px;"
                                    onclick="JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id=0',900,600)">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/new.gif" /><b>Yeni</b>
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
            Width="1800px">
            <SettingsText Title="Periyodik İşler" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="Kayıt Yok" />
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
                <dxwgv:GridViewCommandColumn Width="80px" VisibleIndex="0" ButtonType="Image">
                    <DeleteButton Visible="True" Text="Sil">
                        <Image AlternateText="Sil" Url="~/images/delete.gif" />
                    </DeleteButton>
                    <CancelButton Visible="True" Text="İptal">
                        <Image AlternateText="İptal" Url="~/images/delete.gif" />
                    </CancelButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="PeriyodikIslerID" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataCheckColumn FieldName="Active" Caption="Aktif">
                    <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataColumn Caption="Gündem Tanımı" FieldName="Baslik">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                            runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("PeriyodikIslerID")+"',900,600);PopWin.focus();"%>
                            Text='<%#Eval("Baslik")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Gündem Sınıf" FieldName="VirusSinif" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataColumn Caption="İlgili Birim" FieldName="Firma" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataColumn Caption="Departman" FieldName="Proje" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataSpinEditColumn Caption="Zaman Aralığı (Gün)" FieldName="Step"
                    UnboundType="Integer">
                    <PropertiesSpinEdit MaxLength="4" MaxValue="1000" NumberType="Integer" MinValue="1">
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Saat" Width="50px" FieldName="Saat" UnboundType="Integer">
                    <PropertiesSpinEdit MaxLength="2" MaxValue="23" NumberType="Integer" MinValue="1">
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Başlangıç Tarihi"
                    FieldName="BaslangicTarihi" Width="175px">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Son İşlem Tarihi"
                    FieldName="SonIslemTarihi">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Sonraki İşlem Tarihi"
                    FieldName="SonrakiIslemTarihi">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataMemoColumn Caption="Açıklama" FieldName="Description">
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluşturma Tarihi"
                    FieldName="CreationDate" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Düzenlenme Tarihi"
                    FieldName="ModificationDate" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
