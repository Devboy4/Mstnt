<%@ Page Language="C#" MasterPageFile="~/M1.master" AutoEventWireup="true" CodeFile="edit.aspx.cs"
    Inherits="CRM_Tanimlar_AppSettings_edit" ValidateRequest="false" %>

<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dxm" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v8.3" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxRoundPanel"
    TagPrefix="dxrp" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxTabControl"
    TagPrefix="dxtc" %>
<%@ Register Assembly="DevExpress.Web.v8.3" Namespace="DevExpress.Web.ASPxClasses"
    TagPrefix="dxw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div>
        <dxm:ASPxMenu ID="menu" runat="server" AutoSeparators="RootOnly" CssFilePath="~/App_Themes/Blue (Horizontal orientation)/{0}/styles.css"
            CssPostfix="Blue" ImageFolder="~/App_Themes/Blue (Horizontal orientation)/{0}/"
            ItemSpacing="0px" SeparatorHeight="100%" SeparatorWidth="2px" ShowPopOutImages="True"
            ShowSubMenuShadow="False" AutoPostBack="True" Width="50px">
            <SubMenuStyle GutterWidth="0px" />
            <RootItemSubMenuOffset FirstItemX="-2" LastItemX="-2" X="-1" />
            <SubMenuItemStyle ImageSpacing="19px" PopOutImageSpacing="30px">
            </SubMenuItemStyle>
            <ItemSubMenuOffset FirstItemX="2" FirstItemY="-12" LastItemX="2" LastItemY="-12"
                X="2" Y="-12" />
            <Items>
                <dxm:MenuItem Name="save" Text="Kaydet">
                    <TextTemplate>
                        Kaydet</TextTemplate>
                    <Image Url="~/images/save.gif" />
                </dxm:MenuItem>
            </Items>
        </dxm:ASPxMenu>
        <dxtc:ASPxPageControl ID="ASPxPageControl1" runat="server" ActiveTabIndex="0" CssFilePath="~/App_Themes/Glass/{0}/styles.css"
            CssPostfix="Glass" ImageFolder="~/App_Themes/Glass/{0}/" TabSpacing="0px" Width="100%">
            <TabPages>
                <dxtc:TabPage Text="MSTNT Programý Genel Ayarlar" Name="TabGeneralSettings">
                    <ContentCollection>
                        <dxw:ContentControl runat="server">
                            <table cellpadding="1" cellspacing="1" border="0" width="100%">
                                <tr>
                                    <td colspan="1" style="width: 100px">
                                        Manþet
                                    </td>
                                    <td colspan="1" style="width: 600px">
                                        <dxe:ASPxMemo Width="600px" Height="100px" ID="txtManset" runat="server" Text="Deriden MSTNT">
                                        </dxe:ASPxMemo>
                                    </td>
                                    <td colspan="1">
                                        Ana ekranda üst bölümde bulunan manþet yazýsýný deðiþtirmenizi saðlar
                                    </td>
                                </tr>
                            </table>
                        </dxw:ContentControl>
                    </ContentCollection>
                </dxtc:TabPage>
            </TabPages>
            <TabStyle HorizontalAlign="Center">
            </TabStyle>
            <ContentStyle>
                <Border BorderColor="#4986A2" />
            </ContentStyle>
            <Paddings PaddingLeft="0px" />
        </dxtc:ASPxPageControl>
    </div>
</asp:Content>
