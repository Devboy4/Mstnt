<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Events.aspx.cs" Inherits="frames_Events" %>

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
    <title>Olay Penceresi...</title>
    <link rel="stylesheet" type="text/css" href="../ModelCRM.css" />
    <link rel="stylesheet" type="text/css" href="../PreLoad.css" />

    <script src="../PreLoad.js" type="text/javascript"></script>

    <script src="../utils.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <model:DataTable ID="DTIssueActivity" runat="server" />
            <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DTIssueActivity" KeyFieldName="ID" Width="1200px"
                ClientInstanceName="Grid" OnHtmlDataCellPrepared="grid_HtmlDataCellPrepared"
                OnHtmlRowPrepared="grid_HtmlRowPrepared">
                <SettingsText Title="G�ndem Tarih�esi" GroupPanel="Gruplamak istedi�iniz kolon ba�l���n� buraya s&#252;r&#252;kleyiniz."
                    ConfirmDelete="Kay�t silinsin mi?" EmptyDataRow="#" CustomizationWindowCaption="Kolon Ekle/��kart" />
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
                    <dxwgv:GridViewDataColumn FieldName="EventDesc" Caption="Olay" Width="175px">
                        <DataItemTemplate>
                            <asp:Literal ID="Process" runat="server" Text='<%# Eval("EventDesc") %>'></asp:Literal>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataMemoColumn FieldName="IssueDesc" Caption="Yap�lan Yorumlar" />
                    <dxwgv:GridViewDataColumn FieldName="PNR" Caption="PNR" Width="80px" />
                    <dxwgv:GridViewDataMemoColumn FieldName="Baslik" Caption="G�ndem Tan�s�">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                                runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../CRM/Genel/Issue/edit.aspx?id="+Eval("IssueID")+"&EventID="+Eval("IndexId")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                Text='<%#Eval("Baslik")%>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataMemoColumn>
                    <dxwgv:GridViewDataColumn FieldName="UserName" Caption="Olu�turan" />
                    <dxwgv:GridViewDataColumn FieldName="DurumName" Caption="G�ndem Durumu" />
                    <dxwgv:GridViewDataColumn FieldName="FirmaName" Caption="�lgili Birim" />
                    <dxwgv:GridViewDataColumn FieldName="ProjeName" Caption="Departman" />
                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-DisplayFormatString="dd.MM.yyyy HH:mm"
                        FieldName="date" Caption="��lem Tarihi">
                        <PropertiesDateEdit DisplayFormatString="dd.MM.yyyy HH:mm">
                        </PropertiesDateEdit>
                    </dxwgv:GridViewDataDateColumn>
                </Columns>
            </dxwgv:ASPxGridView>
        </div>
    </form>
</body>
</html>
