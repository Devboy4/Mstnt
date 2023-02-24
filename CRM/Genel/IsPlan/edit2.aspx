<%@ Page Language="C#" AutoEventWireup="true" CodeFile="edit2.aspx.cs" Inherits="CRM_Genel_IsPlan_edit2" %>



<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxDataView"
    TagPrefix="dxdv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bildirim Takibi - Ýþ Planý</title>
    <link rel="stylesheet" type="text/css" href="../../../ModelCRM.css" />
    <link rel="stylesheet" type="text/css" href="../../../PreLoad.css" />

    <script src="../../../PreLoad.js" type="text/javascript"></script>

    <script src="../../../utils.js" type="text/javascript"></script>

    <script src="../../crm.js" type="text/javascript"></script>

</head>
<body topmargin="0" leftmargin="0">
    <form id="form1" runat="server">
        <div>
            <model:DataTable ID="DataTableList" runat="server" />
            <model:DataTable ID="DTDetail" runat="server" />
            <asp:HiddenField ID="HiddenID" runat="server" />
            <asp:HiddenField ID="HiddenUserID" runat="server" />
            <asp:HiddenField ID="HiddenTarih1" runat="server" />
            <asp:SqlDataSource ID="DSUSer" runat="server" SelectCommand="SELECT t1.UserID,(ISNULL(t1.UserName,'')+' ['+ISNULL(t1.FirstName,'')+' '+ISNULL(t1.LastName,'')+']') AS UserName FROM SecurityUsers t1 
            Left Outer Join aspnet_UsersInRoles t2 On t1.UserID=t2.UserID
Left Outer Join aspnet_Roles t3 On t2.RoleID=t3.RoleID Where t3.RoleName='Model Standart Kullanýcý' Or t3.RoleName='Administrator' ORDER BY t1.UserName"
                ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>"></asp:SqlDataSource>
            <dxm:ASPxMenu ID="ASPxMenu1" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" OnItemClick="ASPxMenu1_ItemClick" SeparatorHeight="100%" SeparatorWidth="2px"
                ShowPopOutImages="True" ShowSubMenuShadow="False" Width="300px">
                <SubMenuStyle GutterWidth="0px" />
                <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
                <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                    X="2" Y="-12" />
                <Items>
                    <dxm:MenuItem Name="save" Text="Ýþ Planýný Kaydet">
                        <Image Url="~/images/add_icon.gif" />
                    </dxm:MenuItem>
                </Items>
                <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
                </SubMenuItemStyle>
            </dxm:ASPxMenu>
            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="100%">
                <PanelCollection>
                    <dxrp:PanelContent ID="PanelContent2" runat="server">
                        <table border="0" cellpadding="0" cellspacing="1">
                            <tr>
                                <td style="width: 100px">
                                    <span style="color: #CC0000;">Kullanýcý</span>
                                </td>
                                <td style="width: 175px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                        ID="UserID" ClientInstanceName="cmbUserID" EnableCallbackMode="true" CallbackPageSize="15"
                                        EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                    </dxe:ASPxComboBox>
                                </td>
                                <td style="width: 100px">
                                    <span style="color: #CC0000;">Tarih</span>
                                </td>
                                <td style="width: 175px">
                                    <dxe:ASPxDateEdit ID="Tarih1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                                        CssPostfix="Glass" EditFormatString="dd.MM.yyyy" ClientInstanceName="Tarih1"
                                        DateOnError="Today" ImageFolder="~/App_Themes/Glass/{0}/">
                                        <ButtonStyle Cursor="pointer" Width="13px">
                                        </ButtonStyle>
                                        <ClientSideEvents DateChanged="function(s, e) { 
                                        var dt=document.getElementById('HiddenTarih1').value
                                        var id=document.getElementById('HiddenID').value;
                                        var usr=document.getElementById('HiddenUserID').value;
                                        parent.location.href='?id='+id+'&Tarih1='+dt+'&UserID='+usr+'';
                                         }" />
                                    </dxe:ASPxDateEdit>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 100px">
                                    Açýklama
                                </td>
                                <td colspan="3">
                                    <dxe:ASPxMemo ID="Description" Width="500px" Height="74px" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </dxrp:PanelContent>
                </PanelCollection>
            </dxrp:ASPxRoundPanel>
            <hr />
            <dxwgv:ASPxGridView runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" KeyFieldName="ID" AutoGenerateColumns="False" DataSourceID="DTDetail"
                Width="100%" ID="grid1">
                <SettingsText Title="Tanýmlanmýþ iþler" ConfirmDelete="Kayýt silinsin mi?" EmptyDataRow="Yeni satýr ekle" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <SettingsCustomizationWindow Enabled="True" />
                <Settings ShowPreview="True" ShowTitlePanel="True" />
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
                    <dxwgv:GridViewCommandColumn ButtonType="Image" Width="60px" VisibleIndex="0">
                        <EditButton Text="Deðiþtir" Visible="True">
                            <Image AlternateText="Deðiþtir" Url="~/images/edit.gif" />
                        </EditButton>
                        <DeleteButton Visible="True" Text="Sil">
                            <Image AlternateText="Sil" Url="~/images/delete.gif"></Image>
                        </DeleteButton>
                        <CancelButton Visible="True" Text="Ýptal">
                            <Image AlternateText="Ýptal" Url="~/images/delete.gif"></Image>
                        </CancelButton>
                        <UpdateButton Visible="True" Text="G&#252;ncelle">
                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif"></Image>
                        </UpdateButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="false">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="BildirimPlanlariDetayID" Visible="false">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="IssueID" Visible="false">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="IndexNo" Width="50px" Caption="Bildirim No">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Width="150px" Caption="Bildirim Baþlýk" FieldName="IssueID">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink CssClass="dxeBase" ID="ASPxHyperLink1" Font-Size="8pt" Font-Names="Arial"
                                runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../Issue/edit.aspx?id="+Eval("IssueID")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                Text='<%#Eval("Baslik")%>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn UnboundType="Integer" Caption="Ýþ Sýrasý" FieldName="Sirala">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="FirmaName" Caption="Müþteri Adý">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="ProjeAdi" Caption="Proje Adý">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="Durum" Caption="Bildirim Durumu">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataDateColumn FieldName="CreationDate" Caption="Kayýt Tarihi">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn FieldName="CreatedBy" Caption="Kayýt Eden">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataDateColumn FieldName="ModificationDate" Caption="Düzenlenme Tarihi">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn FieldName="ModifiedBy" Caption="Düzenleyen">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="KeyWords" Caption="Anahtar Kelime">
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                </Columns>
            </dxwgv:ASPxGridView>
            <hr />
            <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
                CssPostfix="Glass" DataSourceID="DataTableList" KeyFieldName="ID" Width="1600px"
                ClientInstanceName="Grid" OnCustomCallback="Grid_CustomCallback">
                <SettingsText EmptyDataRow="#" Title="Liste" />
                <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                    <AlternatingRow Enabled="True">
                    </AlternatingRow>
                    <Header ImageSpacing="5px" SortingImageSpacing="5px">
                    </Header>
                </Styles>
                <Settings ShowFilterRow="True" ShowPreview="True" ShowStatusBar="Visible" ShowTitlePanel="True" />
                <SettingsBehavior ColumnResizeMode="Control" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <SettingsCustomizationWindow Enabled="True" />
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <Columns>
                    <dxwgv:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="40px"
                        ButtonType="Image">
                        <ClearFilterButton Visible="true" Text="Süzmeyi Temizle">
                            <Image Url="~/images/reload2.jpg" AlternateText="Süzmeyi Temizle" />
                        </ClearFilterButton>
                        <HeaderTemplate>
                            <input id="Button1" type="button" onclick="Grid.PerformCallback(true);" value="+"
                                title="Tümünü Seç" />
                            <input id="Button2" type="button" onclick="Grid.PerformCallback(false);" value="-"
                                title="Tümünü Seçme" />
                        </HeaderTemplate>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn FieldName="ID" ShowInCustomizationForm="false" Visible="False">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="IndexID" Width="30px" Caption="Bildirim No" />
                    <dxwgv:GridViewDataColumn FieldName="Baslik" Caption="Baþlýk" Width="300px">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink ID="lbl_IssueID" CssClass="dxeBase" Font-Size="8pt" Font-Names="Arial"
                                runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../Issue/edit.aspx?id="+Eval("IssueID")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                Text='<%#Eval("Baslik")%>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="Firma" FieldName="FirmaName" />
                    <dxwgv:GridViewDataColumn Caption="Proje" FieldName="ProjeName" />
                    <dxwgv:GridViewDataColumn Caption="Bildirim Durumu" FieldName="DurumName" />
                    <dxwgv:GridViewDataColumn Caption="Sonraki Ýlgili" FieldName="UserName" />
                    <dxwgv:GridViewDataColumn Caption="Bildiren Kiþi" FieldName="CreatedBy" />
                    <dxwgv:GridViewDataDateColumn Caption="Bildirim Tarihi" FieldName="BildirimTarihi">
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn Caption="Anahtar Kelime" FieldName="KeyWords" />
                    <dxwgv:GridViewDataColumn FieldName="Filter" ShowInCustomizationForm="false" UnboundType="String"
                        Visible="False">
                    </dxwgv:GridViewDataColumn>
                </Columns>
            </dxwgv:ASPxGridView>
        </div>
    </form>
</body>
</html>
