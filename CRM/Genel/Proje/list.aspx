<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="CRM_Genel_Proje_list" %>

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
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <asp:SqlDataSource ID="DSFirma" runat="server" SelectCommand="SELECT FirmaID,FirmaName FROM Firma ORDER BY FirmaName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSProje" runat="server" SelectCommand="SELECT UserID AS ProjeAmiriID,(ISNULL(UserName,'')+' ['+ISNULL(FirstName,'')+' '+ISNULL(LastName,'')+']') AS ProjeAmir FROM SecurityUsers Where Active=1 ORDER BY UserName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
        <asp:SqlDataSource ID="DSProjeSinif" runat="server" SelectCommand="SELECT ProjeSinifID,Adi As ProjesinifAdi FROM ProjeSiniflari ORDER BY Adi"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
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
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="1600px"
            OnRowValidating="grid_RowValidating" OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating">
            <SettingsText Title="Departmanlar" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <Settings ShowPreview="True" ShowTitlePanel="True" ShowFilterRow="true" />
            <SettingsLoadingPanel Text="Yükleniyor..." />
            <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                PopupEditFormModal="true" PopupEditFormWidth="500px" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <AlternatingRow Enabled="True">
                </AlternatingRow>
                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <Columns>
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="80px">
                    <UpdateButton Text="G&#252;ncelle" Visible="True">
                        <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                    </UpdateButton>
                    <DeleteButton Text="Sil" Visible="True">
                        <Image AlternateText="Sil" Url="~/images/delete.gif" />
                    </DeleteButton>
                    <EditButton Text="Deðiþtir" Visible="True">
                        <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                    </EditButton>
                    <CancelButton Text="Ýptal" Visible="True">
                        <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                    </CancelButton>
                    <NewButton Text="Yeni" Visible="True">
                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                    </NewButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="ProjeID" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataComboBoxColumn FieldName="FirmaID" Caption="Ýlgili Birimler" Width="175px">
                    <PropertiesComboBox ValueField="FirmaID" TextField="FirmaName" ValueType="System.Guid"
                        DataSourceID="DSFirma" EnableIncrementalFiltering="true" EnableCallbackMode="true"
                        CallbackPageSize="15">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn FieldName="ProjeSinifID" Caption="Departman Grubu"
                    Width="175px">
                    <PropertiesComboBox ValueField="ProjeSinifID" TextField="ProjeSinifAdi" ValueType="System.Guid"
                        DataSourceID="DSProjeSinif" EnableIncrementalFiltering="true">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataColumn Width="200px" Caption="Adý" FieldName="Adi">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink CssClass="dxeBase" ID="ASPxHyperLink1" Font-Size="8pt" Font-Names="Arial"
                            runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("ProjeID")+"',850,650);PopWin.focus();"%>
                            Text='<%#Eval("Adi")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                    <EditFormCaptionStyle ForeColor="Red" />
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Maðaza Departmaný" FieldName="IsShop" Width="100px">
                    <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0">
                    </PropertiesCheckEdit>
                    <HeaderCaptionTemplate>
                        <center>
                            Maðaza<br />
                            Departmaný</center>
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Operasyon Süresi" FieldName="OperasyonDay"
                    VisibleIndex="2" Width="80px">
                    <PropertiesSpinEdit AllowMouseWheel="false" DecrementButtonStyle-Width="0px" IncrementButtonStyle-Width="0px"
                        NumberType="Integer" MinValue="0">
                    </PropertiesSpinEdit>
                    <HeaderCaptionTemplate>
                        <center>
                            BR Operasyon<br />
                            Süresi</center>
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataColumn FieldName="ProjeMailAdresi" Caption="E-Mail" Width="150px">
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataComboBoxColumn FieldName="ProjeAmiriID" Caption="Departman Sorumlusu"
                    Width="175px">
                    <PropertiesComboBox ValueField="ProjeAmiriID" TextField="ProjeAmir" ValueType="System.Guid"
                        DataSourceID="DSProje" EnableIncrementalFiltering="true" EnableCallbackMode="true"
                        CallbackPageSize="15">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataMemoColumn FieldName="Description" Caption="Açýklama" Width="150px">
                    <EditFormSettings ColumnSpan="2" />
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataMemoColumn>                
                <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <Settings AutoFilterCondition="Contains" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluþturma Tarihi"
                    FieldName="CreationDate" EditFormSettings-Visible="False">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Düzenlenme Tarihi"
                    FieldName="ModificationDate" EditFormSettings-Visible="False">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
