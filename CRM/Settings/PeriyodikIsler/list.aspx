<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="CRM_Settings_PeriyodikIsler_list" %>

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
        <model:DataTable ID="DataTableList" runat="server" />
        <asp:HiddenField ID="UserName" runat="server" />
        <asp:SqlDataSource ID="DSFirma" runat="server" SelectCommand="EXEC FirmaListByUserName @UserName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="UserName" Name="UserName" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DSProje" runat="server" SelectCommand="EXEC AllowedProjeList @UserName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>">
            <SelectParameters>
                <asp:ControlParameter ControlID="UserName" Name="UserName" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
        <asp:SqlDataSource ID="DSUser" runat="server" SelectCommand="SELECT UserID,(ISNULL(UserName,'')+' ['+ISNULL(FirstName,'')+' '+ISNULL(LastName,'')+']') AS UserName, UserName AS UsString FROM SecurityUsers Where Active=1 ORDER BY UserName"
            ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
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
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="1800px"
            OnRowValidating="grid_RowValidating" OnRowInserting="grid_RowInserting" OnCellEditorInitialize="grid_CellEditorInitialize"
            OnRowUpdating="grid_RowUpdating" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared">
            <SettingsText Title="Periyodik ��ler" GroupPanel="Gruplamak istedi�iniz kolon ba�l���n� buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kay�t silinsin mi?" EmptyDataRow="Yeni sat�r ekle" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsLoadingPanel Text="Y�kleniyor..." />
            <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter" PopupEditFormVerticalAlign="WindowCenter"
                PopupEditFormModal="true" PopupEditFormWidth="500px" />
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
                <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="80px">
                    <UpdateButton Text="G&#252;ncelle" Visible="True">
                        <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                    </UpdateButton>
                    <DeleteButton Text="Sil" Visible="True">
                        <Image AlternateText="Sil" Url="~/images/delete.gif" />
                    </DeleteButton>
                    <EditButton Text="De�i�tir" Visible="True">
                        <Image AlternateText="De�i�tir" Url="~/images/edit.gif" />
                    </EditButton>
                    <CancelButton Text="�ptal" Visible="True">
                        <Image AlternateText="�ptal" Url="~/images/delete.gif" />
                    </CancelButton>
                    <NewButton Text="Yeni" Visible="True">
                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                    </NewButton>
                    <ClearFilterButton Visible="true" Text="S�zmeyi Temizle">
                        <Image Url="~/images/reload2.jpg" AlternateText="S�zmeyi Temizle" />
                    </ClearFilterButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="PeriyodikIslerID" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataCheckColumn FieldName="Active" Caption="Aktif" Width="50px">
                    <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Zaman Aral��� (G�n)" FieldName="Step"
                    UnboundType="Integer">
                    <PropertiesSpinEdit MaxLength="4" MaxValue="1000" NumberType="Integer" MinValue="1">
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Saat" Width="50px" FieldName="Saat" UnboundType="Integer">
                    <PropertiesSpinEdit MaxLength="2" MaxValue="23" NumberType="Integer" MinValue="1">
                    </PropertiesSpinEdit>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Ba�lang�� Tarihi"
                    FieldName="BaslangicTarihi" Width="175px">
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Son ��lem Tarihi"
                    FieldName="SonIslemTarihi">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Sonraki ��lem Tarihi"
                    FieldName="SonrakiIslemTarihi">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataComboBoxColumn FieldName="FirmaID" Caption="�lgili Birim" Width="175px">
                    <PropertiesComboBox TextField="FirmaName" EnableCallbackMode="true" CallbackPageSize="15"
                        EnableIncrementalFiltering="true" DataSourceID="DSFirma" ValueField="FirmaID"
                        ValueType="System.Guid">
                        <ClientSideEvents SelectedIndexChanged="function(s, e){ cmbProjeID.PerformCallback(s.GetValue()); }" />
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn FieldName="ProjeID" Caption="Departman" Width="175px">
                    <PropertiesComboBox TextField="Adi" DataSourceID="DSProje" EnableCallbackMode="true"
                        CallbackPageSize="15" EnableIncrementalFiltering="true" ClientInstanceName="cmbProjeID"
                        ValueField="ProjeID" ValueType="System.Guid">
                        <ClientSideEvents EndCallback="function(s, e){ cmbUserID.PerformCallback(s.GetValue()); }"
                            SelectedIndexChanged="function(s, e){ cmbUserID.PerformCallback(s.GetValue()); }" />
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataComboBoxColumn FieldName="UserID" Caption="�lgili Ki�i" Width="175px">
                    <PropertiesComboBox TextField="UserName" DataSourceID="DSUser" EnableCallbackMode="true"
                        CallbackPageSize="15" EnableIncrementalFiltering="true" ClientInstanceName="cmbUserID"
                        ValueField="UserID" ValueType="System.Guid">
                    </PropertiesComboBox>
                </dxwgv:GridViewDataComboBoxColumn>
                <dxwgv:GridViewDataMemoColumn Caption="G�ndem Tan�m�" FieldName="Baslik">
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataMemoColumn Caption="A��klama" FieldName="Description">
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="D�zenleyen" FieldName="ModifiedBy" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Olu�turma Tarihi"
                    FieldName="CreationDate" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="D�zenlenme Tarihi"
                    FieldName="ModificationDate" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
