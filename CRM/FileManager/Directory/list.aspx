<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs" Inherits="CRM_FileManager_Directory_list" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v8.3" Namespace="DevExpress.Web.ASPxTreeList"
    TagPrefix="dxwtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v8.3.Export" Namespace="DevExpress.Web.ASPxTreeList.Export"
    TagPrefix="dxwtl" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <model:DataTable ID="DTList" runat="server" />
        <div>
            <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
                ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
                ShowSubMenuShadow="False" AutoPostBack="True">
                <%--<ClientSideEvents ItemClick="HandleSubmit" />--%>
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
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr>
                    <td align="center">
                        <dxe:ASPxLabel ID="lbl" runat="server" Text="DİZİNLER" Font-Bold="true" Font-Size="Medium" />
                    </td>
                </tr>
            </table>
            <dxwtl:ASPxTreeList ID="TreeList" runat="server" AutoGenerateColumns="False" DataSourceID="DTList"
                Width="100%" KeyFieldName="ID" ParentFieldName="ParentId" ClientInstanceName="TreeList"
                OnInitNewNode="TreeList_InitNewNode" OnNodeInserting="TreeList_NodeInserting"
                OnNodeUpdating="TreeList_NodeUpdating" OnNodeValidating="TreeList_NodeValidating">
                <Columns>
                    <dxwtl:TreeListDataColumn FieldName="DirectoryName" Caption="Dizin Adı" Width="50%">
                        <DataCellTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="width: 20px">
                                        <img src="../../../images/closed.png" alt="" />
                                    </td>
                                    <td>
                                        <%--<%# Container.Text %>--%>
                                        <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl=<%# "../File/list.aspx?id="+Eval("ID") %>
                                            Text='<%# Container.Text %>'>
                                        </dxe:ASPxHyperLink>
                                    </td>
                                </tr>
                            </table>
                        </DataCellTemplate>
                    </dxwtl:TreeListDataColumn>
                    <dxwtl:TreeListDataColumn FieldName="Description" Caption="Açıklama" Width="50%" />
                    <dxwtl:TreeListDataColumn FieldName="ID" ReadOnly="true" Visible="false" />
                    <dxwtl:TreeListDataColumn FieldName="ParentId" ReadOnly="true" Visible="false" />
                    <dxwtl:TreeListCommandColumn ShowNewButtonInHeader="true" Width="40px" ButtonType="Image">
                        <NewButton Visible="true" Text="Yeni">
                            <Image AlternateText="Yeni" Url="~/images/new.gif" />
                        </NewButton>
                        <EditButton Visible="true" Text="Değiştir">
                            <Image AlternateText="Değiştir" Url="~/images/edit.gif" />
                        </EditButton>
                        <DeleteButton Visible="true" Text="Sil">
                            <Image AlternateText="Sil" Url="~/images/delete.gif" />
                        </DeleteButton>
                        <UpdateButton Visible="true" Text="Güncelle">
                            <Image AlternateText="G&#252;ncelle" Url="~/images/update.gif" />
                        </UpdateButton>
                        <CancelButton Visible="true" Text="İptal">
                            <Image AlternateText="İptal" Url="~/images/delete.gif" />
                        </CancelButton>
                    </dxwtl:TreeListCommandColumn>
                </Columns>
                <SettingsBehavior ExpandCollapseAction="NodeDblClick" AllowDragDrop="false" />
                <Settings GridLines="Both" ShowColumnHeaders="true" ShowFooter="false" ShowRoot="true"
                    ShowTreeLines="true" SuppressOuterGridLines="true" />
                <%--<Images ImageFolder="~/App_Themes/Glass/{0}/" />--%>
                <SettingsText LoadingPanelText="Yükleniyor..." ConfirmDelete="Silinsin mi?" RecursiveDeleteError="Bu dizin alt dizinlere sahip!" />
            </dxwtl:ASPxTreeList>
            <dxwtl:ASPxTreeListExporter ID="TreeListExporter" runat="server" TreeListID="TreeList" />
        </div>
   </asp:Content>
