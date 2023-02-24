<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="CRM_Genel_OnayIssues_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    tagprefix="dxw" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                <dxm:MenuItem Name="Ok" Text="Seçilenleri Onayla">
                    <Image Url="~/images/ok-icon.png" />
                </dxm:MenuItem>
                <dxm:MenuItem Name="Cancel" Text="Seçilenleri İptal Et">
                    <Image Url="~/images/delete.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" ClientInstanceName="Grid"
            OnCustomCallback="grid_CustomCallback" Width="1000px">
            <SettingsText Title="Tarih Arttırımı Onay Listesi" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya s&#252;r&#252;kleyiniz."
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
                <dxwgv:GridViewCommandColumn ButtonType="Image" ShowSelectCheckbox="true" VisibleIndex="0"
                    Width="60px">
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
                <dxwgv:GridViewDataColumn FieldName="Id" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="PNR" Caption="PNR" Width="50px">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Gündem Tanımı" FieldName="Baslik">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                            runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../Issue/edit.aspx?id="+Eval("PNR")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                            Text='<%#Eval("Baslik")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="İsteyen" FieldName="CreatedBy" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataMemoColumn Caption="Açıklama" FieldName="Aciklama" Settings-AutoFilterCondition="Contains" />
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Arttıralacak Tarih"
                    FieldName="RequestDate" Width="175px">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="İstek Tarihi"
                    FieldName="CreationDate" Width="175px">
                </dxwgv:GridViewDataDateColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
