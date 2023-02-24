<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="CRM_Settings_VirusSinif_list" %>

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
                        <table width="70px" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                    padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id=0',900,600)">
                                    <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/new.gif" /><b>Yeni</b>
                                </td>
                            </tr>
                        </table>
                    </Template>
                </dxm:MenuItem>
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="100%"
            OnRowValidating="grid_RowValidating" OnRowInserting="grid_RowInserting" OnRowUpdating="grid_RowUpdating">
            <SettingsText Title="G�ndem S�n�flar�" GroupPanel="Gruplamak istedi�iniz kolon ba�l���n� buraya s&#252;r&#252;kleyiniz."
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
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="Adi" Caption="Ad�">
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                            runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("ID")+"',900,600);PopWin.focus();"%>
                            Text='<%#Eval("Adi")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataSpinEditColumn Caption="Operasyon S�resi" FieldName="OperasyonDay"
                    Width="80px">
                    <PropertiesSpinEdit AllowMouseWheel="false" DecrementButtonStyle-Width="0px" IncrementButtonStyle-Width="0px"
                        NumberType="Integer" MinValue="0">
                    </PropertiesSpinEdit>
                    <HeaderCaptionTemplate>
                        <center>
                            Operasyon<br />
                            S�resi</center>
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataSpinEditColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Tarih De�i�tirilsin" FieldName="IsDateChange"
                    Width="150px">
                    <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0">
                    </PropertiesCheckEdit>
                    <HeaderCaptionTemplate>
                        <center>
                            Tarih<br />
                            De�i�tirebilsin</center>
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataCheckColumn Caption="Sms G�nderebilsin" FieldName="IsSendSms"
                    Width="150px">
                    <PropertiesCheckEdit ValueType="System.Int32" ValueChecked="1" ValueUnchecked="0">
                    </PropertiesCheckEdit>
                    <HeaderCaptionTemplate>
                        <center>
                            Sms<br />
                            G�nderebilsin</center>
                    </HeaderCaptionTemplate>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataMemoColumn Caption="A��klama" FieldName="Description">
                    <EditFormSettings ColumnSpan="2" />
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="D�zenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Olu�turma Tarihi"
                    EditFormSettings-Visible="False" FieldName="CreationDate">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="D�zenlenme Tarihi"
                    EditFormSettings-Visible="False" FieldName="ModificationDate">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="False">
                </dxwgv:GridViewDataColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
