<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="CRM_Settings_Pop3Mails_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <model:DataTable ID="DTList" runat="server" />
    <model:DataTable ID="DTList2" runat="server" />
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
                <dxm:MenuItem Name="new" Text="Yeni" Visible="false">
                    <TextTemplate>
                        Yeni</TextTemplate>
                    <Image Url="~/images/new.gif" />
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
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DTList" KeyFieldName="ID" SettingsEditing-Mode="Inline"
            OnRowValidating="Grid_RowValidating" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating"
            ClientInstanceName="Grid">
            <SettingsText Title="Pop3 Mail Listesi" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya sürükleyiniz."
                ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
            <Columns>
                <dxwgv:GridViewCommandColumn Width="80px" VisibleIndex="0" ButtonType="Image">
                    <NewButton Visible="True" Text="Yeni">
                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                    </NewButton>
                    <EditButton Visible="True" Text="Deðiþtir">
                        <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                    </EditButton>
                    <UpdateButton Visible="True" Text="Güncelle">
                        <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                    </UpdateButton>
                    <DeleteButton Visible="True" Text="Sil">
                        <Image AlternateText="Sil" Url="~/images/delete.gif" />
                    </DeleteButton>
                    <CancelButton Visible="True" Text="Ýptal">
                        <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                    </CancelButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                <dxwgv:GridViewDataColumn FieldName="IndexId" Visible="False" />
                <dxwgv:GridViewDataCheckColumn FieldName="Active" Caption="Aktif" Width="50px">
                    <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataCheckColumn FieldName="Pop3UseSsl" Caption="SSL Kullan" Width="50px">
                    <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32">
                    </PropertiesCheckEdit>
                    <HeaderCaptionTemplate>
                        SSL
                        <br />
                        Kullan
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataTextColumn FieldName="Email" Caption="E-Posta">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                            runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("IndexId")+"',900,600);PopWin.focus();"%>
                            Text='<%#Eval("Email")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataTextColumn FieldName="Pop3Server" Caption="Pop3 Server" />
                <dxwgv:GridViewDataTextColumn FieldName="Pop3Port" Caption="Pop3 Port" />
                <dxwgv:GridViewDataTextColumn FieldName="MailUserName" Caption="Mail UserName" />
                <dxwgv:GridViewDataTextColumn FieldName="MailPassword" Caption="Mail Password" PropertiesTextEdit-Password="true" />
                <dxwgv:GridViewDataColumn Caption="Oluþturan" FieldName="CreatedBy" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluþturma Tarihi"
                    FieldName="CreationDate" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <HeaderCaptionTemplate>
                        Oluþturma
                        <br />
                        Tarihi
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Düzenlenme Tarihi"
                    FieldName="ModificationDate" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <HeaderCaptionTemplate>
                        Düzenlenme
                        <br />
                        Tarihi
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataDateColumn>
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
            <SettingsPager PageSize="10" ShowSeparators="True">
            </SettingsPager>
            <SettingsText Title="Pop E-Posta Adresleri" EmptyDataRow="Yeni satýr ekle" />
            <SettingsEditing Mode="Inline" />
        </dxwgv:ASPxGridView>
        <hr />
        <dxwgv:ASPxGridView ID="Grid2" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DTList2" KeyFieldName="ID" SettingsEditing-Mode="Inline"
            OnRowValidating="Grid2_RowValidating" OnRowInserting="Grid2_RowInserting" OnRowUpdating="Grid2_RowUpdating"
            ClientInstanceName="Grid2">
            <SettingsText Title="Tanýmlý Gönderici Listesi" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya sürükleyiniz."
                ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
            <Columns>
                <dxwgv:GridViewCommandColumn Width="80px" VisibleIndex="0" ButtonType="Image">
                    <NewButton Visible="True" Text="Yeni">
                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                    </NewButton>
                    <EditButton Visible="True" Text="Deðiþtir">
                        <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                    </EditButton>
                    <UpdateButton Visible="True" Text="Güncelle">
                        <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                    </UpdateButton>
                    <DeleteButton Visible="True" Text="Sil">
                        <Image AlternateText="Sil" Url="~/images/delete.gif" />
                    </DeleteButton>
                    <CancelButton Visible="True" Text="Ýptal">
                        <Image AlternateText="Ýptal" Url="~/images/delete.gif" />
                    </CancelButton>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                <dxwgv:GridViewDataColumn FieldName="IndexId" Visible="False" />
                <dxwgv:GridViewDataCheckColumn FieldName="Active" Caption="Aktif" Width="50px">
                    <PropertiesCheckEdit ValueChecked="1" ValueUnchecked="0" ValueType="System.Int32">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataTextColumn FieldName="Email" Caption="Email" />
                <dxwgv:GridViewDataColumn Caption="Oluþturan" FieldName="CreatedBy" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Oluþturma Tarihi"
                    FieldName="CreationDate" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <HeaderCaptionTemplate>
                        Oluþturma
                        <br />
                        Tarihi
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataColumn Caption="Düzenleyen" FieldName="ModifiedBy" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Düzenlenme Tarihi"
                    FieldName="ModificationDate" Width="75px">
                    <EditItemTemplate>
                    </EditItemTemplate>
                    <HeaderCaptionTemplate>
                        Düzenlenme
                        <br />
                        Tarihi
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataDateColumn>
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
            <SettingsPager PageSize="10" ShowSeparators="True">
            </SettingsPager>
            <SettingsEditing Mode="Inline" />
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
