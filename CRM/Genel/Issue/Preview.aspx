<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Preview.aspx.cs" Inherits="CRM_Genel_Issue_Preview" %>

<%@ Register Src="~/controls/NotGrid.ascx" TagName="NotGrid" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
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
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bildirim Tarih�esi...</title>
</head>
<body>
    <form id="form1" runat="server">
        <model:DataTable ID="DTIssueActivity" runat="server" />
        <model:DataTable ID="DTIssueUsers" runat="server" />
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DTIssueActivity" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
            OnHtmlRowPrepared="grid_HtmlRowPrepared" KeyFieldName="ID" Width="1000px" ClientInstanceName="Grid">
            <SettingsText Title="G�ndem Tarih�esi" GroupPanel="Gruplamak istedi�iniz kolon ba�l���n� buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kay�t silinsin mi?" EmptyDataRow="#" CustomizationWindowCaption="Kolon Ekle/��kart" />
            <SettingsEditing Mode="Inline" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                ShowPreview="True" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <AlternatingRow Enabled="True">
                </AlternatingRow>
                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <Columns>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="false" />
                <dxwgv:GridViewDataColumn FieldName="YaziRengi" Visible="false" />
                <dxwgv:GridViewDataMemoColumn FieldName="Comment" Caption="A��klama" />
                <dxwgv:GridViewDataColumn FieldName="Process" Caption="De�i�iklik" Width="350px">
                    <DataItemTemplate>
                        <asp:Literal ID="Process" runat="server" Text='<%# Eval("Process") %>'></asp:Literal>
                    </DataItemTemplate>
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <%--<dxwgv:GridViewDataMemoColumn FieldName="Process" Caption="De�i�iklik" Width="350px"
                    EditFormSettings-Visible="False">
                </dxwgv:GridViewDataMemoColumn>--%>
                <dxwgv:GridViewDataColumn FieldName="CreatedBy" Caption="Olu�turan">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn FieldName="DurumName" Caption="G�ndem Durumu">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn FieldName="CommentDate" Caption="��lem Tarihi">
                    <PropertiesDateEdit DisplayFormatString="dd.MM.yyyy HH:mm">
                    </PropertiesDateEdit>
                    <EditFormSettings Visible="False" />
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
        </dxwgv:ASPxGridView>
        <table style="width: 320px">
            <tr>
                <td>
                    <dxe:ASPxButton ID="btnDeleteUsers" AutoPostBack="false" Image-Url="~/images/delete_16.gif"
                        runat="server" Visible="false" Text="Se�ilen ilgilileri sil">
                        <ClientSideEvents Click="function(s, e){Grid.PerformCallback('deleteuser|1');}" />
                    </dxe:ASPxButton>
                </td>
                <td>
                    <dxe:ASPxButton ID="btnMakeDontdo" AutoPostBack="false" Image-Url="~/images/arRed.gif"
                        runat="server" Visible="false" Text="Se�ilenleri Yapmad� Yap">
                        <ClientSideEvents Click="function(s, e){Grid.PerformCallback('MakeDontDo|1');}" />
                    </dxe:ASPxButton>
                </td>
            </tr>
        </table>

        <dxwgv:ASPxGridView ID="grid2" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DTIssueUsers" KeyFieldName="ID" Width="100%"
            ClientInstanceName="Grid" OnCustomCallback="grid2_CustomCallback">
            <SettingsText Title="G�ndem �lgilileri" GroupPanel="Gruplamak istedi�iniz kolon ba�l���n� buraya s&#252;r&#252;kleyiniz."
                ConfirmDelete="Kay�t silinsin mi?" EmptyDataRow="#" CustomizationWindowCaption="Kolon Ekle/��kart" />
            <SettingsEditing Mode="Inline" PopupEditFormWidth="750px" PopupEditFormHorizontalOffset="50"
                PopupEditFormVerticalOffset="50" />
            <SettingsPager PageSize="15" ShowSeparators="True">
            </SettingsPager>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <SettingsCustomizationWindow Enabled="True" />
            <Settings ShowFilterRow="True" ShowStatusBar="Visible" ShowGroupedColumns="True"
                ShowPreview="True" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <AlternatingRow Enabled="True">
                </AlternatingRow>
                <Header SortingImageSpacing="5px" ImageSpacing="5px">
                </Header>
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <Columns>
                <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="80px"
                    ButtonType="Image">
                    <ClearFilterButton Visible="true" Text="S�zmeyi Temizle">
                        <Image Url="~/images/reload2.jpg" AlternateText="S�zmeyi Temizle" />
                    </ClearFilterButton>
                    <HeaderTemplate>
                        <input id="Button1" type="button" onclick="Grid.PerformCallback('Select|true');"
                            value="+" title="T�m�n� Se�" />
                        <input id="Button2" type="button" onclick="Grid.PerformCallback('Select|false');"
                            value="-" title="T�m�n� Se�me" />
                    </HeaderTemplate>
                </dxwgv:GridViewCommandColumn>
                <dxwgv:GridViewDataColumn FieldName="UserName" Caption="�lgili">
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataCheckColumn FieldName="IssueAntivirus" Caption="Kapatt�">
                    <PropertiesCheckEdit ValueChecked="1" ValueType="System.Int32" ValueUnchecked="0">
                    </PropertiesCheckEdit>
                </dxwgv:GridViewDataCheckColumn>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="false" />
                <dxwgv:GridViewDataMemoColumn FieldName="ProjeName" Caption="Birim">
                </dxwgv:GridViewDataMemoColumn>
                <dxwgv:GridViewDataColumn FieldName="FirmaName" Caption="�lgili Birim">
                </dxwgv:GridViewDataColumn>
            </Columns>
        </dxwgv:ASPxGridView>
    </form>
</body>
</html>
