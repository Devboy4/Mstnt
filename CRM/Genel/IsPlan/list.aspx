<%@ Page Language="C#" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="CRM_Genel_IsPlan_list" %>

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
<head id="Head1" runat="server">
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
            <asp:HiddenField ID="HiddenAfterCallbackUserID" runat="server" />
            <model:DataTable ID="DTList" runat="server" />
            <model:DataTable ID="DTDetail" runat="server" />
            <model:DataTable ID="DTDetail2" runat="server" />
            <asp:SqlDataSource ID="DSUSer" runat="server" SelectCommand="SELECT t1.UserID,(ISNULL(t1.UserName,'')+' ['+ISNULL(t1.FirstName,'')+' '+ISNULL(t1.LastName,'')+']') AS UserName FROM SecurityUsers t1 
            Left Outer Join aspnet_UsersInRoles t2 On t1.UserID=t2.UserID Left Outer Join aspnet_Roles t3 On t2.RoleID=t3.RoleID Where t3.RoleName='Model Standart Kullan�c�' Or t3.RoleName='Administrator' ORDER BY t1.UserName"
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
                    <dxm:MenuItem Name="newPlain">
                        <Template>
                            <table width="150" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                        padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="JavaScript:PopWin = OpenPopupWinBySize('edit.aspx',850,650);PopWin.focus();">
                                        <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/new.gif" />Yeni
                                        �� Plan�
                                    </td>
                                </tr>
                            </table>
                        </Template>
                    </dxm:MenuItem>
                    <dxm:MenuItem Name="List">
                        <Template>
                            <table width="150" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="dxmMenuItemWithImage_Blue" align="left" valign="top" style="cursor: pointer;
                                        padding-right: 4px; border-right-width: 0px; width: 150px;" onclick="Grid.PerformCallback('Tarih');">
                                        <asp:Image ID="Image1" runat="server" Width="16px" Height="16px" ImageUrl="~/images/List.gif" />Tarihe
                                        G�re Listele
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
            <hr />
            <dxrp:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" HeaderText="" Width="600px">
                <PanelCollection>
                    <dxp:panelcontent runat="server">
                        <table border="0" cellpadding="0" cellspacing="1" style="width: 500px">
                            <tr>
                                <td style="width: 100px">
                                    <span style="color: #CC0000">Kullan�c�</span>
                                </td>
                                <td style="width: 175px">
                                    <dxe:ASPxComboBox runat="server" ValueType="System.String" ImageFolder="~/App_Themes/Glass/{0}/"
                                        CssPostfix="Glass" CssFilePath="~/App_Themes/Glass/{0}/styles.css" Width="175px"
                                        ID="UserID" EnableCallbackMode="true" CallbackPageSize="15" ClientInstanceName="cmbUserID"
                                        EnableIncrementalFiltering="True">
                                        <ButtonStyle Width="13px" Cursor="pointer">
                                        </ButtonStyle>
                                        <ClientSideEvents SelectedIndexChanged="function(s,e) {Grid.PerformCallback(s.GetValue());}" />
                                    </dxe:ASPxComboBox>
                                </td>
                                <td style="width: 100px">
                                    Tarih
                                </td>
                                <td style="width: 175px">
                                    <dxe:ASPxDateEdit ID="Tarih1" runat="server" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
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
                Width="1600px" OnCustomCallback="grid_CustomCallback">
                <SettingsText Title="�� Planlar� Listesi" GroupPanel="Gruplamak istedi�iniz kolon ba�l���n� buraya s&#252;r&#252;kleyiniz."
                    ConfirmDelete="Kay�t silinsin mi?" EmptyDataRow="Yeni sat�r ekle" />
                <SettingsPager PageSize="15" ShowSeparators="True">
                </SettingsPager>
                <Images ImageFolder="~/App_Themes/Glass/{0}/">
                </Images>
                <SettingsDetail ShowDetailRow="true" AllowOnlyOneMasterRowExpanded="true" />
                <SettingsCustomizationWindow Enabled="True" />
                <Settings ShowPreview="True" ShowFilterRow="true" />
                <SettingsLoadingPanel Text="Y�kleniyor..." />
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
                    <dxwgv:GridViewCommandColumn ButtonType="Image" VisibleIndex="0" Width="80px">
                        <UpdateButton Text="G&#252;ncelle" Visible="True">
                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                        </UpdateButton>
                        <DeleteButton Text="Sil" Visible="True">
                            <Image AlternateText="Sil" Url="~/images/delete.gif" />
                        </DeleteButton>
                        <CancelButton Text="�ptal" Visible="True">
                            <Image AlternateText="�ptal" Url="~/images/delete.gif" />
                        </CancelButton>
                    </dxwgv:GridViewCommandColumn>
                    <dxwgv:GridViewDataColumn FieldName="ID" Visible="False">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn FieldName="BildirimPlanID" Visible="False">
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Width="100px" Caption="Bildirim Plan Numaras�" FieldName="BildirimPlanID">
                        <DataItemTemplate>
                            <dxe:ASPxHyperLink CssClass="dxeBase" ID="ASPxHyperLink1" Font-Size="8pt" Font-Names="Arial"
                                runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit2.aspx?id="+Eval("BildirimPlanID")+"',850,650);PopWin.focus();"%>
                                Text='<%#Eval("IndexID")%>'>
                            </dxe:ASPxHyperLink>
                        </DataItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataComboBoxColumn FieldName="UserID" Caption="�� Plan� Sahibi">
                        <PropertiesComboBox ValueField="UserID" TextField="UserName" ValueType="System.Guid"
                            DataSourceID="DSUser" EnableIncrementalFiltering="true" EnableCallbackMode="true"
                            CallbackPageSize="15">
                        </PropertiesComboBox>
                    </dxwgv:GridViewDataComboBoxColumn>
                    <dxwgv:GridViewDataMemoColumn FieldName="Description" Caption="A��klama" Width="200px">
                    </dxwgv:GridViewDataMemoColumn>
                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Plan Tarihi"
                        FieldName="Tarih1" EditFormSettings-Visible="False">
                        <EditItemTemplate>
                            <%# Eval("Tarih1")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn Caption="Ekleyen" FieldName="CreatedBy" EditFormSettings-Visible="False">
                        <EditItemTemplate>
                            <%# Eval("CreatedBy")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataColumn Caption="D�zenleyen" FieldName="ModifiedBy" EditFormSettings-Visible="False">
                        <EditItemTemplate>
                            <%# Eval("ModifiedBy")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataColumn>
                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="Olu�turma Tarihi"
                        FieldName="CreationDate" EditFormSettings-Visible="False">
                        <EditItemTemplate>
                            <%# Eval("CreationDate")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataDateColumn PropertiesDateEdit-EditFormatString="dd.MM.yyyy" Caption="D�zenlenme Tarihi"
                        FieldName="ModificationDate" EditFormSettings-Visible="False">
                        <EditItemTemplate>
                            <%# Eval("ModificationDate")%>
                        </EditItemTemplate>
                    </dxwgv:GridViewDataDateColumn>
                    <dxwgv:GridViewDataColumn FieldName="Filter" UnboundType="String" Visible="False">
                    </dxwgv:GridViewDataColumn>
                </Columns>
                <Templates>
                    <DetailRow>
                        <dxwgv:ASPxGridView ID="detailGrid" ClientInstanceName="detailGrid" runat="server"
                            KeyFieldName="ID" DataSourceID="DTDetail" OnBeforePerformDataSelect="detailGrid_DataSelect"
                            Width="100%">
                            <SettingsText EmptyDataRow="#" Title="" />
                            <Settings ShowTitlePanel="true" />
                            <Styles Row-Wrap="True">
                            </Styles>
                            <SettingsDetail IsDetailGrid="true" />
                            <Columns>
                                <dxwgv:GridViewDataColumn FieldName="ID" Visible="false">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="BildirimPlanlariDetayID" Visible="false">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="IssueID" Visible="false">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="IndexNo" Width="50px" Caption="Bildirim No">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn Width="150px" Caption="Bildirim Ba�l�k" FieldName="IssueID">
                                    <DataItemTemplate>
                                        <dxe:ASPxHyperLink CssClass="dxeBase" ID="ASPxHyperLink1" Font-Size="8pt" Width="200px"
                                            Font-Names="Arial" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('../Issue/edit.aspx?id="+Eval("IssueID")+"&NoteOwner=1',850,650);PopWin.focus();"%>
                                            Text='<%#Eval("Baslik")%>'>
                                        </dxe:ASPxHyperLink>
                                    </DataItemTemplate>
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="Sirala" Caption="�� S�ras�">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="KeyWords" Caption="Anahtar Kelime">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="ProjeAdi" Caption="Proje Ad�">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="Durum" Caption="Bildirim Durumu">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataColumn FieldName="FirmaName" Caption="M��teri Ad�">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataDateColumn FieldName="CreationDate" Caption="Kay�t Tarihi">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataColumn FieldName="CreatedBy" Caption="Kay�t Eden">
                                </dxwgv:GridViewDataColumn>
                                <dxwgv:GridViewDataDateColumn FieldName="ModificationDate" Caption="D�zenlenme Tarihi">
                                </dxwgv:GridViewDataDateColumn>
                                <dxwgv:GridViewDataColumn FieldName="ModifiedBy" Caption="D�zenleyen">
                                </dxwgv:GridViewDataColumn>
                            </Columns>
                            <Settings ShowColumnHeaders="true" />
                            <SettingsDetail IsDetailGrid="true" />
                        </dxwgv:ASPxGridView>
                    </DetailRow>
                </Templates>
            </dxwgv:ASPxGridView>
        </div>
    </form>
</body>
</html>
