<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DetayAnaliz.aspx.cs" Inherits="CRM_Genel_IsPlan_DetayAnaliz" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxPanel"
    TagPrefix="dxp" %>


<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />

    <script src="../../../PreLoad.js" type="text/javascript"></script>

    <script src="../../../utils.js" type="text/javascript"></script>

    <script src="../../crm.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField ID="HiddenUserID" runat="server" />
            <model:DataTable ID="DTList" runat="server" />
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
                    <dxm:MenuItem Name="List">
                        <Image Url="~/images/List.gif" />
                        <TextTemplate>
                            <table width="50px" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="cursor: pointer; width: 100%" align="center" valign="middle" onclick="Grid.PerformCallback('x');">
                                        Listele
                                    </td>
                                </tr>
                            </table>
                        </TextTemplate>
                    </dxm:MenuItem>
                </Items>
            </dxm:ASPxMenu>
            <hr />
            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="600px">
                <PanelCollection>
                    <dxp:panelcontent runat="server">
                        <table border="0" cellpadding="0" cellspacing="2" style="width: 500px">
                            <tr>
                                <td style="width: 100px">
                                    <span style="color: #CC0000">Kullanýcý</span>
                                </td>
                                <td style="width: 175px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                        ID="UserID" EnableCallbackMode="true" CallbackPageSize="15" ClientInstanceName="cmbUserID"
                                        EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                    </dxe:ASPxComboBox>
                                </td>
                                <td style="width: 100px">
                                </td>
                                <td style="width: 175px">
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    Baþlangýç Tarihi
                                </td>
                                <td style="width: 175px">
                                    <dxe:ASPxDateEdit ID="Tarih1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" EditFormatString="dd.MM.yyyy">
                                        <ButtonStyle Cursor="pointer" Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                                <td style="width: 100px">
                                    Bitiþ Tarihi
                                </td>
                                <td style="width: 175px">
                                    <dxe:ASPxDateEdit ID="Tarih2" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" EditFormatString="dd.MM.yyyy">
                                        <ButtonStyle Cursor="pointer" Width="13px">
                                        </ButtonStyle>
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                        </table>
                    </dxp:panelcontent>
                </PanelCollection>
            </dxrp:ASPxRoundPanel>
            <hr />
            <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DTList" ClientInstanceName="Grid" KeyFieldName="ID"
                Width="100%" OnCustomCallback="grid_CustomCallback">
                <SettingsText Title="Ýþ Planlarý Listesi" GroupPanel="Gruplamak istediðiniz kolon baþlýðýný buraya s&#252;r&#252;kleyiniz."
                    ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <SettingsCustomizationWindow Enabled="True" />
                <Settings ShowPreview="True" ShowFilterRow="true" ShowGroupedColumns="True" ShowGroupPanel="true" />
                <SettingsLoadingPanel Text="Yükleniyor..." />
                <SettingsEditing Mode="inline" PopupEditFormHorizontalAlign="WindowCenter"
                    PopupEditFormVerticalAlign="WindowCenter" PopupEditFormModal="true" PopupEditFormWidth="500px" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <AlternatingRow Enabled="True">
                    </AlternatingRow>
                    <Header SortingImageSpacing="5px" ImageSpacing="5px">
                    </Header>
                </Styles>
                <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
                <Columns>
                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Width="50px" Caption="Bildirim Numarasý" FieldName="IndexID">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink CssClass="dxeBase" ID="ASPxHyperLink1" Font-Size="8pt" Font-Names="Arial"
                                runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../Issue/edit.aspx?id="+Eval("IssueID")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                Text='<%#Eval("IndexID")%>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="Baslik" Caption="Baþlýk" Width="200px">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                                runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../Issue/edit.aspx?id="+Eval("IssueID")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                Text='<%#Eval("Baslik")%>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="Proje" Caption="Proje" Width="100px">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="BildirimDurumu" Caption="Bildirim Durumu" Width="100px">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="Sayi" Width="50px" Caption="Ýþ planý sayýsý">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="Template" Width="175px" Caption="Ýlgili iþ planlarý">
                        <DataItemTemplate>
                            <asp:Literal runat="server" ID="Template" Text='<%# Eval("Template")%>'></asp:Literal>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="False">
                    </dxwgv:GridViewDataColumn>
                </Columns>
            </dxwgv:ASPxGridView>
        </div>
    </form>
</body>
</html>
