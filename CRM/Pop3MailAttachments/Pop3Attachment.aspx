<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Pop3Attachment.aspx.cs" Inherits="CRM_Pop3MailAttachments_Pop3Attachment" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxObjectContainer"
    TagPrefix="dxoc" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />

    <script src="../../utils2.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <model:DataTable ID="DTList" runat="server" />
        <div>
            <dxwgv:ASPxGridView ID="Grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DTList" KeyFieldName="ID" SettingsEditing-Mode="Inline"
                ClientInstanceName="Grid" OnHtmlDataCellPrepared="GridNot_HtmlDataCellPrepared">
                <Columns>
                    <dxwgv:GridViewDataColumn FieldName="ID" Caption="ID" Visible="false" />
                    <dxwgv:GridViewDataColumn FieldName="MailId" Caption="MailId" Visible="false" />
                    <dxwgv:GridViewDataColumn FieldName="AttachmentName" Caption="Dosya Adý" Width="200px">
                        <DataItemTemplate>
                            <asp:Label ID="AttachName" Visible="false" runat="server" Text='<%# Eval("AttachmentName") %>'></asp:Label>
                            <asp:Label ID="LabelNotID" Visible="false" runat="server" Text='<%# Eval("MailId") %>'></asp:Label>
                            <asp:Literal runat="server" ID="LiteralNotDosyalar" Text=""></asp:Literal>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                </Columns>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <Settings ShowFilterRow="false" ShowStatusBar="Hidden" ShowGroupedColumns="false"
                    ShowGroupPanel="false" ShowPreview="True" ShowTitlePanel="false" ShowVerticalScrollBar="false" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                </Styles>
                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                <SettingsPager PageSize="3" ShowSeparators="True">
                </SettingsPager>
                <SettingsText Title="Dosyalar" EmptyDataRow="." />
            </dxwgv:ASPxGridView>
        </div>
    </form>
</body>
</html>
