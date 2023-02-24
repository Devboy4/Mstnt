<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SearchBrowser2.aspx.cs" Inherits="CRM_SearchBrowser2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Pragma" content="no-cache">
    <meta http-equiv="Cache-Control" content="no-cache,must-revalidate">
    <title>Search Browser</title>
    <%--    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />

    <script type="text/javascript" src="../../../PreLoad.js"></script>--%>
    <link rel="stylesheet" type="text/css" href="../ModelCRM.css" />
</head>
<body>
    <form id="form1" runat="server">
        <model:DataTable ID="DTGrid" runat="server" />
        <div>
            <dxwgv:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DTGrid" ClientInstanceName="Grid" OnHtmlDataCellPrepared="Grid_HtmlDataCellPrepared" Width="100%">
                <Columns>
                    <dxwgv:GridViewCommandColumn Width="10px" VisibleIndex="0" ButtonType="Image">
                        <ClearFilterButton Visible="True" Text="Süzme İptal">
                            <Image AlternateText="Süzme İptal" Url="~/images/reload2.jpg" />
                        </ClearFilterButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn Caption="" VisibleIndex="1" Width="40px">
                        <DataItemTemplate>
                            <a href="#" onclick="window.returnValue='<%# Container.KeyValue %>';window.close();">
                                <img src="../images/add_16.gif" alt="Seç" style="border: 0;" /></a>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                </Columns>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <Settings ShowFilterRow="true" ShowStatusBar="Hidden" ShowGroupedColumns="false"
                    ShowGroupPanel="false" ShowPreview="false" ShowTitlePanel="false" ShowVerticalScrollBar="false" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                    <TitlePanel HorizontalAlign="Center">
                    </TitlePanel>
                </Styles>
                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" AllowFocusedRow="true" />
                <SettingsPager PageSize="15" ShowSeparators="True" Position="Bottom">
                </SettingsPager>
                <SettingsText Title="Search Browser" EmptyDataRow="Kayıt Yok" />
            </dxwgv:ASPxGridView>
        </div>
    </form>
</body>
</html>
