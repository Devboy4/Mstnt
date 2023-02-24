<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="list.aspx.cs"
    Inherits="admin_User_list" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v8.3" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dxwgv" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Src="~/controls/DataTableControl.ascx" TagName="DataTable" TagPrefix="model" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <model:DataTable ID="DTUsers" runat="server" />
    <asp:SqlDataSource ID="DSUnvan" runat="server" SelectCommand="SELECT UnvanID,Adi FROM Unvan ORDER BY Adi"
        ConnectionString="<%$ ConnectionStrings:LocalSqlServer %>" />
    <div>
        <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
            ShowSubMenuShadow="False" AutoPostBack="True">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem Name="new" Visible="true">
                    <Template>
                        <table width="50" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="dxmMenuItem_Blue" style="cursor: pointer;" align="center" valign="middle"
                                    onclick="javascript:PopWin = OpenPopupWinBySize('edit.aspx?id=0',950,400);PopWin.focus();">
                                    <img src="../../../images/new.gif" alt="" />&nbsp;<b>Yeni</b>
                                </td>
                            </tr>
                        </table>
                    </Template>
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
                <dxm:MenuItem Name="Passive" Text="Sil">
                    <TextTemplate>
                        Pasif Kullanıcılar</TextTemplate>
                    <Image Url="~/images/delete_16.gif" />
                </dxm:MenuItem>
                <dxm:MenuItem Text=" ">
                </dxm:MenuItem>
                <dxm:MenuItem Text="Yenile" NavigateUrl="javascript:location.href='./list.aspx'">
                    <Image Url="~/images/reload2.jpg" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxwgv:ASPxGridView ID="grid" runat="server" AutoGenerateColumns="False" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" DataSourceID="DTUsers" KeyFieldName="ID" Width="100%" SettingsEditing-Mode="Inline"
            OnRowValidating="Grid_RowValidating" OnRowInserting="Grid_RowInserting" OnRowUpdating="Grid_RowUpdating"
            ClientInstanceName="Grid" OnCustomJSProperties="Grid_CustomJSProperties">
            <Columns>
                <%--<dxwgv:GridViewCommandColumn Width="40px" VisibleIndex="0" ButtonType="Image">
                    <NewButton Visible="True" Text="Yeni">
                        <Image AlternateText="Yeni" Url="~/images/new.gif" />
                    </NewButton>
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
                </dxwgv:GridViewCommandColumn>--%>
                <dxwgv:GridViewDataColumn FieldName="ID" Visible="False" />
                <dxwgv:GridViewDataColumn FieldName="UserID" Visible="False" />
                <dxwgv:GridViewDataColumn Caption="Kullanıcı Adı" FieldName="UserName" Width="100px"
                    Settings-AutoFilterCondition="Contains" Visible="true">
                    <HeaderStyle ForeColor="#C00000" />
                    <DataItemTemplate>
                        <dxe:ASPxHyperLink ID="ASPxHyperLink1" runat="server" NavigateUrl=<%#"JavaScript:PopWin = OpenPopupWinBySize('edit.aspx?id="+Eval("UserID")+"',950,400);PopWin.focus();"%>
                            Text='<%#Eval("UserName")%>'>
                        </dxe:ASPxHyperLink>
                    </DataItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataTextColumn Caption="Şifre" FieldName="Password" Width="100px"
                    Visible="false" PropertiesTextEdit-Password="true">
                    <HeaderStyle ForeColor="#C00000" />
                </dxwgv:GridViewDataTextColumn>
                <dxwgv:GridViewDataColumn Caption="Adı" FieldName="FirstName" Width="100px" Visible="true"
                    Settings-AutoFilterCondition="Contains">
                    <HeaderStyle ForeColor="#C00000" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="Soyadı" FieldName="LastName" Width="100px" Visible="true"
                    Settings-AutoFilterCondition="Contains">
                    <HeaderStyle ForeColor="#C00000" />
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataColumn Caption="E-Posta" FieldName="Email" Width="150px" Visible="true"
                    Settings-AutoFilterCondition="Contains">
                    <HeaderStyle ForeColor="#C00000" />
                </dxwgv:GridViewDataColumn>
                <%--                    <dxwgv:GridViewDataColumn Caption="Aktif" FieldName="Aktif" Width="50px" Visible="true">
                        <HeaderStyle ForeColor="#C00000" />
                    </dxwgv:GridViewDataColumn>--%>
                <dxwgv:GridViewDataColumn Caption="Başlık" FieldName="Title" Width="200px" Visible="false" />
                <dxwgv:GridViewDataColumn Caption="Bölüm" FieldName="Department" Width="200px" Visible="false" />
                <dxwgv:GridViewDataColumn Caption="Oluşturan" FieldName="CreatedBy" Width="100px"
                    Visible="false">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn Caption="Oluşturulma Tarihi" FieldName="CreationDate"
                    Width="100px" Visible="false">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
                <dxwgv:GridViewDataColumn Caption="Değiştiren" FieldName="ModifiedBy" Width="100px"
                    Visible="false">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataColumn>
                <dxwgv:GridViewDataDateColumn Caption="Değişiklik Tarihi" FieldName="ModificationDate"
                    Width="100px" Visible="false">
                    <EditItemTemplate>
                    </EditItemTemplate>
                </dxwgv:GridViewDataDateColumn>
            </Columns>
            <Images ImageFolder="~/App_Themes/Glass/{0}/">
            </Images>
            <Settings ShowFilterRow="True" ShowStatusBar="Hidden" ShowGroupedColumns="False"
                ShowGroupPanel="False" ShowPreview="True" ShowTitlePanel="True" ShowVerticalScrollBar="False" />
            <Styles CssFilePath="~/App_Themes/Glass/{0}/styles.css" CssPostfix="Glass">
                <Header ImageSpacing="5px" SortingImageSpacing="5px">
                </Header>
                <AlternatingRow Enabled="True" />
            </Styles>
            <SettingsBehavior ColumnResizeMode="Control" ConfirmDelete="True" />
            <SettingsPager PageSize="19" ShowSeparators="True">
            </SettingsPager>
            <SettingsCustomizationWindow Enabled="True" />
            <SettingsText Title="Kullanıcı Bilgileri" GroupPanel="Gruplamak istediğiniz kolon başlığını buraya sürükleyiniz."
                ConfirmDelete="Kayıt silinsin mi?" EmptyDataRow="Yeni satır ekle" />
        </dxwgv:ASPxGridView>
    </div>
</asp:Content>
