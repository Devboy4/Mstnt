<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="MarjinalCRM_Settings_Sehir_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <model:DataTable ID="DataTableList" runat="server" />
    <asp:SqlDataSource ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" ID="SqlDataSourceUlkeID"
        runat="server" SelectCommand="Select UlkeID,Adi FROM Ulke ORDER BY Adi"></asp:SqlDataSource>
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
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="100%"
            OnRowValidating="grid_RowValidating">
            <SettingsText Title="Þehirler" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsLoadingPanel Text="Yükleniyor..." />
            <SettingsEditing Mode="inline" PopupEditFormModal="true" PopupEditFormVerticalAlign="WindowCenter"
                PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormWidth="500px" />
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                ShowGroupPanel="True" ShowPreview="True" ShowTitlePanel="True" />
            <SettingsEditing Mode="Inline"></SettingsEditing>
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <AlternatingRow Enabled="True">
                </AlternatingRow>
                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <Columns>
                <dxwgv:GridViewCommandColumn Width="80px" VisibleIndex="0" ButtonType="Image">
                    <UpdateButton Visible="True" Text="G&#252;ncelle">
                        <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif"></Image>
                    </UpdateButton>
                    <DeleteButton Visible="True" Text="Sil">
                        <Image AlternateText="Sil" Url="~/images/delete.gif"></Image>
                    </DeleteButton>
                    <EditButton Visible="True" Text="Deðiþtir">
                        <Image AlternateText="Deðiþtir" Url="~/images/edit.gif"></Image>
                    </EditButton>
                    <CancelButton Visible="True" Text="Ýptal">
                        <Image AlternateText="Ýptal" Url="~/images/delete.gif"></Image>
                    </CancelButton>
                    <NewButton Visible="True" Text="Yeni">
                        <Image AlternateText="Yeni" Url="~/images/new.gif"></Image>
                    </NewButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn Visible="False" FieldName="ID">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Visible="False" FieldName="UlkeID">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn VisibleIndex="1" FieldName="Adi" Width="55%" Caption="Adý">
                    <HeaderStyle ForeColor="#C00000"></HeaderStyle>
                </dxwgv:GridViewDataColumn>
               <%-- <dxwgv:GridViewDataSpinEditColumn Caption="Operasyon Süresi" FieldName="OperasyonDay"
                    VisibleIndex="2" Width="80px">
                    <PropertiesSpinEdit AllowMouseWheel="false" DecrementButtonStyle-Width="0px" IncrementButtonStyle-Width="0px"
                        NumberType="Integer" MinValue="0">
                    </PropertiesSpinEdit>
                    <HeaderCaptionTemplate>
                        <center>
                            BR Operasyon<br />
                            Süresi</center>
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataSpinEditColumn>--%>
                <dxwgv:GridViewDataComboBoxColumn FieldName="UlkeID" Caption="Ülke" Width="20%" VisibleIndex="3">
                    <PropertiesComboBox DataSourceID="SqlDataSourceUlkeID" TextField="Adi" ValueField="UlkeID"
                        EnableIncrementalFiltering="true" ValueType="System.Guid">
                    </PropertiesComboBox>
                    <HeaderStyle ForeColor="red" />
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataColumn UnboundType="String" Visible="False" FieldName="Filter">
                </dxwgv:GridViewDataColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
