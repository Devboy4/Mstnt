<%@ Page Language="C#"  MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="CRM_FileManager_File_list" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxUploadControl"
    TagPrefix="dxuc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3.Export" Namespace="DevExpress.Web.ASPxGridView.Export"
    TagPrefix="dxwgv" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script src="./list.js" type="text/javascript"></script>


        <asp:HiddenField runat="server" ID="DirectoryId" />
        <model:DataTable ID="DTList" runat="server" />
        <div>
            <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                ShowSubMenuShadow="False" AutoPostBack="True">
                <ClientSideEvents ItemClick="HandleSubmit" />
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
            <hr />
            <table width="100%" cellpadding="0" cellspacing="0">
                <tr>
                    <td style="width: 120px" align="right">
                        <dxe:ASPxLabel runat="server" ID="lblDosya" Text="Yeni Eklenecek Dosya" />
                    </td>
                    <td align="right">
                        <dxuc:ASPxUploadControl runat="server" ID="Uploader" ClientInstanceName="Uploader"
                            OnFileUploadComplete="Uploader_FileUploadComplete" Width="90%">
                            <ClientSideEvents FileUploadComplete="Uploader_Complete" FileUploadStart="Uploader_Start" />
                        </dxuc:ASPxUploadControl>
                    </td>
                    <td style="width: 50px" align="left">
                        <dxe:ASPxButton runat="server" ID="BtnUpload" ClientInstanceName="bUpload" UseSubmitBehavior="false"
                            AutoPostBack="false" Text="Ekle" Width="100%">
                            <ClientSideEvents Click="BtnUpload_Click" />
                        </dxe:ASPxButton>
                    </td>
                </tr>
            </table>
            <dxwgv:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DTList" KeyFieldName="ID" SettingsEditing-Mode="Inline"
                OnRowValidating="Grid_RowValidating" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating"
                ClientInstanceName="Grid" OnCustomJSProperties="Grid_CustomJSProperties" OnCustomCallback="Grid_CustomCallback"
                OnHtmlDataCellPrepared="Grid_HtmlDataCellPrepared" Width="100%">
                <Columns>
                    <dxwgv:GridViewCommandColumn Width="40px" VisibleIndex="0" ButtonType="Image">
                        <%--                        <NewButton Visible="True" Text="Yeni">
                            <Image AlternateText="Yeni" Url="~/images/new.gif" />
                        </NewButton>--%>
                        <EditButton Visible="True" Text="Değiştir">
                            <Image AlternateText="Değiştir" Url="~/images/edit.gif" />
                        </EditButton>
                        <UpdateButton Visible="True" Text="Güncelle">
                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                        </UpdateButton>
                        <DeleteButton Visible="True" Text="Sil">
                            <Image AlternateText="Sil" Url="~/images/delete.gif" />
                        </DeleteButton>
                        <CancelButton Visible="True" Text="İptal">
                            <Image AlternateText="İptal" Url="~/images/delete.gif" />
                        </CancelButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                    <%--                    <dxwgv:GridViewDataColumn FieldName="DirectoryName" Caption="Dizin Adı" ReadOnly="true"
                        Width="30%" />--%>
                    <dxwgv:GridViewDataColumn FieldName="FileName" Caption="Dosya Adı" ReadOnly="true"
                        Width="50%">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl='<%# "../File/preview.aspx?id="+Eval("ID") %>'
                                Text='<%# Eval("FileName") %>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="FileSize" Caption="Boyut" Width="40px" />
                    <dxwgv:GridViewDataColumn FieldName="FileSizeType" Caption="Tür" Width="20px" />
                    <dxwgv:GridViewDataColumn FieldName="Description" Caption="Açıklama" Width="50%" />
                    <dxwgv:GridViewDataColumn FieldName="Kaydeden" Caption="Kaydeden" ReadOnly="true"
                        Width="100px" />
                    <dxwgv:GridViewDataDateColumn FieldName="CreationDate" Caption="Kayıt Tarihi" ReadOnly="true"
                        Width="90px" />
                    <dxwgv:GridViewDataColumn FieldName="Guncelleyen" Caption="Güncelleyen" ReadOnly="true"
                        Width="100px" />
                    <dxwgv:GridViewDataDateColumn FieldName="ModificationDate" Caption="Güncelleme Tarihi"
                        ReadOnly="true" Width="90px" />
                </Columns>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <Settings ShowFilterRow="True" ShowStatusBar="Hidden" ShowGroupedColumns="false"
                    ShowGroupPanel="false" ShowPreview="True" ShowTitlePanel="True" ShowVerticalScrollBar="False" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                </Styles>
                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                <SettingsPager PageSize="17" ShowSeparators="True">
                </SettingsPager>
                <SettingsText Title="Dosyalar" EmptyDataRow="Yeni Kayıt" />
                <SettingsEditing Mode="Inline" />
            </dxwgv:ASPxGridView>
            <dxwgv:ASPxGridViewExporter ID="GridExporter" runat="server" GridViewID="Grid" Landscape="false" />
        </div>
   </asp:Content>
